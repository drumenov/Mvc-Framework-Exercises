using SIS.Framework.ActionResults.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly string fullyQualifiedLayoutName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData) {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
            this.fullyQualifiedLayoutName = MvcContext.Get.LayoutPath;
        }

        private string ReadFile(string html = "") {
            if (!File.Exists(html)) {
                throw new FileNotFoundException($"View does not exist at {html}");

            }
            return File.ReadAllText(html);
        }

        public string Render() {
            string contentHtml = this.ReadFile(this.fullyQualifiedTemplateName);
            string layoutHtml = this.ReadFile(this.fullyQualifiedLayoutName);
            string fullHtml = this.GenerateFullHtml(contentHtml, layoutHtml, MvcContext.Get.PatternToReplace);
            string rederedHtml = this.RenderHtml(fullHtml);
            return rederedHtml;
        }

        private string RenderHtml(string fullHtml) {
            string renderedHtml = fullHtml;

            if (this.viewData.Any()) {
                foreach (KeyValuePair<string, object> parameter in this.viewData) {
                    renderedHtml = renderedHtml.Replace($"{{{{{parameter.Key}}}}}", parameter.Value.ToString());
                }
            }
            return renderedHtml;
        }

        private string GenerateFullHtml(string content, string layout, string pattern) {
            return layout.Replace(MvcContext.Get.PatternToReplace, content);
        }
    }
}
