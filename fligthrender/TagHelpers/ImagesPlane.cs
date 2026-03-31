using fligthrender.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.Text;

namespace fligthrender.TagHelpers
{
    public class ImagesPlane : TagHelper
    {
        public List<string> Path { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Path == null || Path.Count == 0)
            {
                output.SuppressOutput();
                return;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<style>");
            sb.AppendLine(".gallery-container { text-align:center; }");
            sb.AppendLine(".main-image { width:650px; height:400px; margin-bottom:20px; border:2px solid #333; }");
            sb.AppendLine(".thumbs { display:flex; flex-wrap:wrap; justify-content:start; gap:10px; max-width:650px; margin:0 auto; }");
            sb.AppendLine(".thumbs img { width:100px; height:100px; cursor:pointer; border:2px solid transparent; }");
            sb.AppendLine(".thumbs img.active { border-color:white; }");
            sb.AppendLine("</style>");

            sb.AppendLine("<div class='gallery-container'>");
            sb.AppendLine($"<img id='mainImage' src='{Path[0]}' class='main-image' />");

            sb.AppendLine("<div class='thumbs'>");
            for (int i = 0; i < Path.Count; i++)
            {
                var activeClass = i == 0 ? "active" : "";
                sb.AppendLine($"<img src='{Path[i]}' class='{activeClass}' onclick='changeImage(this)' />");
            }
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("<script>");
            sb.AppendLine("function changeImage(el) {");
            sb.AppendLine("  document.getElementById('mainImage').src = el.src;");
            sb.AppendLine("  var thumbs = document.querySelectorAll('.thumbs img');");
            sb.AppendLine("  thumbs.forEach(t => t.classList.remove('active'));");
            sb.AppendLine("  el.classList.add('active');");
            sb.AppendLine("}");
            sb.AppendLine("</script>");

            output.TagName = ""; 
            output.Content.SetHtmlContent(sb.ToString());

        }
    }
}