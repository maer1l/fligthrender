using fligthrender.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace fligthrender.TagHelpers
{
    public class CarouselTagHelper : TagHelper
    {
        public List<string> paths { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("style", $"position: relative; width: 50%; height: 500px;");
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.AppendHtml(@"
            <style>
                .active{
                display: inline;
                }

                .hidden{
                    display: none;
                }
            </style>

            <script>
                currentIndex = 0
                function nextImg() {
                    let elems = [...document.getElementsByClassName(""img"")];
                    let curElem = elems[currentIndex];
                    curElem.classList.remove('active');
                    curElem.classList.add('hidden');
                    currentIndex == elems.length - 1 ? currentIndex = 0 : currentIndex += 1;
                    let nElem = elems[currentIndex]
                    nElem.classList.remove('hidden');
                    nElem.classList.add('active');
                }

                function prevImg() {
                    let elems = [...document.getElementsByClassName(""img"")];
                    let curElem = elems[currentIndex];
                    curElem.classList.remove('active');
                    curElem.classList.add('hidden');
                    currentIndex == 0 ? currentIndex = elems.length - 1 : currentIndex -= 1;
                    let nElem = elems[currentIndex]
                    nElem.classList.remove('hidden');
                    nElem.classList.add('active');
                }
            </script>
            ");

            for (int i = 0; i < paths.Count(); i++)
            {
                string className = i == 0 ? "img active" : "img hidden";
                output.Content.AppendHtml($"<img src='{paths[i]}' name='{Path.GetFileNameWithoutExtension(paths[i])}' class='{className}' style=\"width: 100%; height: 500px; object-fit: cover;\"/>");
            }
            output.Content.AppendHtml(@"
                <button onclick='nextImg()' style='position: absolute; top: 50%; right: 0%;'>▶</button>
                <button onclick='prevImg()' style='position: absolute; top: 50%; left: 0%;'>◀</button>
            ");
        }
    }
}
