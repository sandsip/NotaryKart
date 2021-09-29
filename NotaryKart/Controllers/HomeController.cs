using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotaryKart.Models;

namespace NotaryKart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUS()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet("{MainMenuname}/{SubMenuName}", Name = "WebPage")]
        public IActionResult WebPage(string MainMenuname, string SubMenuName)
        {
            // Get the product as indicated by the ID from a database or some repository.
            //var product = this.productRepository.Find(id);
            var metaInfo = SEOPageMetaHelper.Collections.Where(s => s.Item1.Equals(SubMenuName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            // If a product with the specified ID was not found, return a 404 Not Found response.
            if (metaInfo == null)
            {
                return this.NotFound();
            }

            // Get the actual friendly version of the title.
            //string friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Title);

            // Compare the title with the friendly title.
            if (!string.Equals(metaInfo.Item2, SubMenuName, StringComparison.Ordinal))
            {
                // If the title is null, empty or does not match the friendly title, return a 301 Permanent
                // Redirect to the correct friendly URL.
                //return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
            }
            var reqUrl = Request.HttpContext.Request;
            var urlHost = reqUrl.Host;
            var urlPath = reqUrl.Path;
            var urlQueryString = reqUrl.QueryString;
            if (urlQueryString == null)
            {
                ViewBag.URL = urlHost + urlPath;
            }
            else
            {
                ViewBag.URL = urlHost + urlPath + urlQueryString;
            }
            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create(ViewBag.URL);
            HttpWReq.Method = "GET";
            HttpWReq.Headers.Add("accept-encoding", "gzip, deflate, br");
            HttpWReq.Headers.Add("cache-control", "max-age=0");
            HttpWReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
            HttpWReq.Headers.Add("accept-encoding", "gzip, deflate, br");
            HttpWReq.Headers.Add("accept-language", "en-US,en;q=0.9");
            HttpWReq.Headers.Add("cache-control", "max-age=0");
            HttpWReq.Headers.Add("upgrade-insecure-requests", "1");
            //request.Headers["X-My-Custom-Header"] = "the-value";
            // The URL the client has browsed to is correct, show them the view containing the product.
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
