using fligthrender.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace fligthrender.TagHelpers
{
    public class CarouselTagHelper : TagHelper
    {
        public PlaneWithPhoto plane { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("style", $"position: relative; width: 50%; height: 500px;");
            output.TagMode = TagMode.StartTagAndEndTag;
            for (int i = 0; i < plane.paths.Count; i++)
            {
                string className = i == 0 ? "img active" : "img hidden";
                output.Content.AppendHtml($"<img src='{plane.paths[i]}' name='{Path.GetFileNameWithoutExtension(plane.paths[i])}' class='{className}' style=\"width: 100%; height: 500px; object-fit: cover;\"/>");
            }
            output.Content.AppendHtml(@"
                <button onclick='nextImg()' style='position: absolute; top: 50%; right: 0%;'>▶</button>
                <button onclick='prevImg()' style='position: absolute; top: 50%; left: 0%;'>◀</button>
            ");

        }
    }
}
