using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchEngine.Pages
{
    public class SecureLoadModel : PageModel
    {
        public void OnPost(string hash, string url)
        {
            ViewData["hash"] = hash;
            ViewData["targetUrl"] = url;
        }
    }
}
