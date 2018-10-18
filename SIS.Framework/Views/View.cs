using SIS.Framework.ActionResults.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData) {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
        }

        private string ReadFile() {
            if (!File.Exists(this.fullyQualifiedTemplateName)) {
                throw new FileNotFoundException($"View does not exist at {this.fullyQualifiedTemplateName}");
    
        }
            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }

        public string Render() {
            string fullHtml = this.ReadFile();
            string rederedHtml = this.RenderHtml(fullHtml);
            return rederedHtml;
        }

        private string RenderHtml(string fullHtml) {
            string renderedHtml = fullHtml;

            if (this.viewData.Any()) {
                foreach (KeyValuePair<string, object> parameter in this.viewData) {
                    renderedHtml = renderedHtml.Replace($"{{{{{{{parameter.Key}}}}}}}", parameter.Value.ToString());
                }
            }
            return renderedHtml;
        }
    }
}
