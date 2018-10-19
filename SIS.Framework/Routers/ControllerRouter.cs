using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Base;
using SIS.Framework.Controllers.Base;
using SIS.Framework.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IControllerRouter
    {
        private IDependencyContainer dependencyContainer;

        public ControllerRouter(IDependencyContainer dependencyContainer) {
            this.dependencyContainer = dependencyContainer;
        }

        public IHttpResponse Handle(IHttpRequest request) {
            string controllerName = String.Empty;
            string actionName = String.Empty;
            string requestMethod = request.RequestMethod.ToString();
            if (request.Path == "/") {
                controllerName = "Home";
                actionName = "Index";
            }
            else {
                string[] splittedRequestPath = request.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                if (splittedRequestPath.Length == 2) {
                    controllerName = splittedRequestPath[0].Capitalize();
                    actionName = splittedRequestPath[1].Capitalize();
                }
            }
            Controller controller = this.GetController(controllerName, request);

            if (controller == null) {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            MethodInfo action = this.GetAction(requestMethod, controller, actionName);

            if (controller == null || action == null) {
                throw new NullReferenceException();
            }

            object[] actionParameters = this.MapActionParameters(action, request, controller);
            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            return this.PrepareResponse(actionResult);
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller) {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];
            for (int i = 0; i < mappedActionParameters.Length; i++) {
                ParameterInfo currentParameter = actionParametersInfo[i];
                if (currentParameter.ParameterType.IsPrimitive || currentParameter.ParameterType == typeof(String)) {
                    mappedActionParameters[i] = this.ProcessPrimitiveParameter(currentParameter, request);
                }
                else {
                    object bindingModel = this.ProcessBindingModelParameters(currentParameter, request);
                    controller.ModelState.IsValid = this.IsValidModel(bindingModel, bindingModel.GetType());
                    mappedActionParameters[i] = bindingModel;
                }
            }
            return mappedActionParameters;
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters) {
            return action.Invoke(controller, actionParameters) as IActionResult;
        }


        private bool? IsValidModel(object bindingModel, Type bindingModelType) {
            PropertyInfo[] properties = bindingModelType.GetProperties();
            foreach (PropertyInfo property in properties) {
                ValidationAttribute[] propertyValidationAttributes = property.GetCustomAttributes().Where(ca => ca is ValidationAttribute).Cast<ValidationAttribute>().ToArray();

                foreach (ValidationAttribute validationAttribute in propertyValidationAttributes) {
                    var propertyValue = property.GetValue(bindingModel);
                    if (!validationAttribute.IsValid(propertyValue)) {
                        return false;
                    }
                }
            }
            return true;
        }

        private object ProcessPrimitiveParameter(ParameterInfo currentParameter, IHttpRequest request) {
            object parameterValue = this.GetParameterFromRequestData(currentParameter.Name, request);
            return parameterValue;
        }

        private object GetParameterFromRequestData(string parameterName, IHttpRequest request) {
            object parameterValue = null;
            if (request.QueryData.ContainsKey(parameterName))
                parameterValue = request.QueryData[parameterName];
            if (request.FormData.ContainsKey(parameterName))
                parameterValue = request.FormData[parameterName];
            return parameterValue;
        }

        private object ProcessBindingModelParameters(ParameterInfo currentParameter, IHttpRequest request) {
            Type bindingModelType = currentParameter.ParameterType;

            object bindingModelInstance = Activator.CreateInstance(bindingModelType);
            PropertyInfo[] bindingModelProperties = bindingModelType.GetProperties();

            foreach (PropertyInfo property in bindingModelProperties) {
                try {
                    object value = this.GetParameterFromRequestData(property.Name, request);
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }
            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }


        private IHttpResponse PrepareResponse(IActionResult actionResult) {
            string invocationResult = actionResult.Invoke();
            if (actionResult is IViewable) {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }

            if (actionResult is IRedirectable) {
                return new RedirectResult(invocationResult);
            }

            throw new InvalidOperationException("UnSopported Action");
        }



        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName) {
            MethodInfo action = null;
            IEnumerable<MethodInfo> actions = this.GetSuitableMethods(controller, actionName);
            foreach (MethodInfo methodInfo in actions) {
                IEnumerable<HttpMethodAttribute> attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(attr => attr is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET") {
                    return methodInfo;
                }

                foreach (HttpMethodAttribute attr in attributes) {
                    if (attr.IsValid(requestMethod)) {
                        return methodInfo;
                    }
                }
            }
            return action;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName) {
            if (controller == null) {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetController(string controllerName, IHttpRequest request) {
            if (String.IsNullOrWhiteSpace(controllerName)) {
                return null;
            }

            string fullyQualifiedControllerName = $"{MvcContext.Get.AssemblyName}.{MvcContext.Get.ControllersFolder}.{controllerName}{MvcContext.Get.ControllersSuffix}, {MvcContext.Get.AssemblyName}";
            Type controllerType = Type.GetType(fullyQualifiedControllerName);
            Controller controller = this.dependencyContainer.CreateInstance(controllerType) as Controller;
            return controller;
        }
    }
}
