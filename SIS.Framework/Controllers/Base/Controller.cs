using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests;
using System.Runtime.CompilerServices;

namespace SIS.Framework.Controllers.Base
{
    public abstract class Controller
    {
        protected Controller() {
            this.ViewModel = new ViewModel();
        }	

        public IHttpRequest Request { get; set; }

        protected ViewModel ViewModel { get; }

        public Model ModelState { get; } = new Model();

        protected IViewable View([CallerMemberName] string caller = "") {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

            IRenderable view = new View(fullyQualifiedName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
