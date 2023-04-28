using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost(string _searchQuery, bool _scramble)
        {
            ViewData["debugOutput"] = $"{_searchQuery} || Scramble = {_scramble}";

            /*if (ModelState.IsValid == false)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Query", new { searchQuery = searchQuery});
            }*/
        }


    }
}
