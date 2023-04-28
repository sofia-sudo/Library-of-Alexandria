using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SearchEngine.Pages
{
    public class QueryModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private string query;

        public QueryModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost(string searchQuery)
        {
            ViewData["searchQuery"] = $"{searchQuery}";
            ViewData["Results"] = DMS.Query(searchQuery);
            query = searchQuery;
        }

        private void GetResultsFromDb()
        {
            /* query DB
             * retrieve info into array of type Result
             * check count
             * 
             * if count is low
             * query again term by term
             * retrieve info into array of type Result
             * 
             * save array to viewdata
             * 
             */
        }
    }
}
