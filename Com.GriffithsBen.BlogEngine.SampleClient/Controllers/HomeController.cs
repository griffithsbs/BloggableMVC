using Com.GriffithsBen.BlogEngine.Models;
using Com.GriffithsBen.BlogEngine.SampleClient.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.SampleClient.Controllers {
    public class HomeController : Controller {

        private BlogRepository Repository { get; set; }

        public HomeController() {
            this.Repository = new BlogRepository();
        }

        // TODO
        /// <summary>
        /// Return a view of all blog entries
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// Return a view of a single blog entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlogEntry(int id) {

            /* more realistically, we would construct a BlogEntry based on an instance of
             * a blog entity retrievd from the repository
             */
            BlogEntry model = this.Repository.GetBlogEntryById(id);

            if (model == null) {
                return new HttpStatusCodeResult(404);
            }

            return View(model);
        }

    }
}