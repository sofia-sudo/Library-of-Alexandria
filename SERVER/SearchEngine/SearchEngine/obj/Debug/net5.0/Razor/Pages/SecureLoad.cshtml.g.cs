#pragma checksum "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\SecureLoad.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4cfed3b42fa7ffd4effbb7fe62f93eea67ef03e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(SearchEngine.Pages.Pages_SecureLoad), @"mvc.1.0.razor-page", @"/Pages/SecureLoad.cshtml")]
namespace SearchEngine.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\_ViewImports.cshtml"
using SearchEngine;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\SecureLoad.cshtml"
using HtmlAgilityPack;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e4cfed3b42fa7ffd4effbb7fe62f93eea67ef03e", @"/Pages/SecureLoad.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af26fa6bfe7ecb384bfed20ecd84e9709178582a", @"/Pages/_ViewImports.cshtml")]
    public class Pages_SecureLoad : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\SecureLoad.cshtml"
  
    ViewData["Title"] = ""; // Do not add name of site to the title as this may compromise user's safety
                            // if accessing censored information in their country.

    HtmlWeb web = new HtmlWeb(); // Create a new HtmlWeb instance to load the page
    HtmlDocument pageContent = web.Load((string)ViewData["targetUrl"]); // Save page to pageContent

    string temp = pageContent.DocumentNode.OuterHtml;

    temp = temp.Replace("/pf/dist/components/combinations/default.css?d=137", "https://www.reuters.com/pf/dist/components/combinations/default.css?d=137");

    string targetTitle = pageContent.DocumentNode.SelectSingleNode("//title").InnerText;

    HtmlDocument formattedContent = new HtmlDocument();
    formattedContent.LoadHtml(temp);

#line default
#line hidden
#nullable disable
            WriteLiteral("    <hr />\r\n    <h4>");
#nullable restore
#line 21 "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\SecureLoad.cshtml"
   Write(targetTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
    <p>Not all features will work in SecureLoad as external scripts are disabled for security purposes.</p>
    <p>You will not be able to follow any links on this site as it is loaded in SecureLoad.</p>
    <p>The Library of Alexandria assumes no responsibility for the content of this site.</p>
    <hr />


    ");
#nullable restore
#line 28 "C:\Users\sofia\source\repos\SearchEngine\SERVER\SearchEngine\SearchEngine\Pages\SecureLoad.cshtml"
Write(Html.Raw(formattedContent.DocumentNode.OuterHtml));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SearchEngine.Pages.SecureLoadModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<SearchEngine.Pages.SecureLoadModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<SearchEngine.Pages.SecureLoadModel>)PageContext?.ViewData;
        public SearchEngine.Pages.SecureLoadModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591