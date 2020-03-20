using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelseId.Core.BFF.Sample.Client.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class StatusCodeModel : PageModel
    {
        public int ErrorStatusCode { get; set; }

        public void OnGet(string code)
        {
            ErrorStatusCode = int.Parse(code);
        }
    }
}