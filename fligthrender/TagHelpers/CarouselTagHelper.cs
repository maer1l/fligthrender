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
            output.Attributes.SetAttribute("style", $"position: relative; width: 100%; height: 500px; display: flex; flex-direction: row;");
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.AppendHtml(@"
            <style>
                .active{
                display: inline;
                }

                .hidden{
                    display: none;
                }

                .carousel-btn{
                    border: none;
                    border-radius: 50%;
                    width: 35px;
                    height: 35px;
                    text-align: center;
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
            output.Content.AppendHtml(@"
                <div style='flex: 1;'>
                    <button class='carousel-btn' onclick='nextImg()' style='position: absolute; top: 50%; right: 3%;'>▶</button>
                </div>
            ");
            output.Content.AppendHtml("<div style='display: flex; flex-direction: column; flex: 2; justify-content: center; align-items: center;'>");
            for (int i = 0; i < paths.Count(); i++)
            {
                string className = i == 0 ? "img active" : "img hidden";
                output.Content.AppendHtml($"<img src='{paths[i]}' name='{Path.GetFileNameWithoutExtension(paths[i])}' class='{className}' style=\"height: 100%;\"/>");
            }
            output.Content.AppendHtml("</div>");

            output.Content.AppendHtml(@"
                <div style='flex: 1;'>
                    <button class='carousel-btn' onclick='prevImg()' style='position: absolute; top: 50%; left: 3%;'>◀</button>
                </div>
            ");
            //output.Content.AppendHtml(@"
            //    <button class='carousel-btn' onclick='nextImg()' style='position: absolute; top: 50%; right: 3%;'>▶</button>
            //    <button class='carousel-btn' onclick='prevImg()' style='position: absolute; top: 50%; left: 3%;'>◀</button>
            //");
        }
    }
}
