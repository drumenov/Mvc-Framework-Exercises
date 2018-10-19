using SIS.Framework.ActionResults.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

	private readonly string layoutFullyQualifiedName;

	private readonly string patternToReplaceInLayout;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData, string layoutHtmlPath = ..., string patternToReplace = MvcContext.Get.PatternToReplace) {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
		this.layoutFullyQualifiedName = layoutHtmlPath;
		this.patternToReplaceInLayout = patternToReplace;
        }

        private string ReadFile(string html = this.fullyQualifiedTemplateName) {
            if (!File.Exists(html)) {
                throw new FileNotFoundException($"View does not exist at {html}");
    
        }
            return File.ReadAllText(html);
        }

        public string Render() {
            string contentHtml = this.ReadFile();
		string layoutHtml = this.ReadFile(this.layoutFullyQualifiedName);
		string fullHtml = this.GenerateFullHtml(contentHtml, layoutHtml);
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
	
	private string GenerateFullHtml(string content, string layout) {
		return layout.Remplace(this.patternToReplaceInLayout, content);
	}
    }
}
