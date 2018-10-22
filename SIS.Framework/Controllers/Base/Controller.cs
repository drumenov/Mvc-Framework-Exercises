using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Security.Contracts;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests;
using System.IO;
using System.Runtime.CompilerServices;

namespace SIS.Framework.Controllers.Base
{
    public abstract class Controller
    {
        private ViewEngine viewEngine { get; } = new ViewEngine();

        protected Controller() {
            this.ViewModel = new ViewModel();
        }

        public IIdentity Identity => this.Request.Session.GetParameter("auth") as IIdentity;

        public IHttpRequest Request { get; set; }

        protected ViewModel ViewModel { get; }

        public Model ModelState { get; } = new Model();

        protected IViewable View([CallerMemberName] string caller = "") {
            string controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try {
                viewContent = this.viewEngine.GetViewContent(controllerName, caller);
            }
            catch (FileNotFoundException e) {
                this.ViewModel.Data["Error"] = e.Message;
                viewContent = this.viewEngine.GetErrorContent();
            }
            string renderedContent = this.viewEngine.RenderHtml(viewContent, this.ViewModel.Data);

            return new ViewResult(new View(renderedContent));
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);

        protected void SignIn(IIdentity auth) {
            this.Request.Session.AddParameter("auth", auth);
        }

        protected void SignOut() {
            this.Request.Session.ClearParameters();
        }

    }
}
