using Com.GriffithsBen.BlogEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.SampleClient.Controllers {
    public class HomeController : Controller {

        // TODO
        private BlogEntry GetBlogEntry(int id) {
            return new BlogEntry();
        }

        // TODO
        /// <summary>
        /// Return a view of all blog entries
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View();
        }

        public ActionResult BlogEntry(int id) {

            BlogEntry model = this.GetBlogEntry(id);

            if (model == null) {
                return new HttpStatusCodeResult(404);
            }

            return View(model);
        }

    }
}