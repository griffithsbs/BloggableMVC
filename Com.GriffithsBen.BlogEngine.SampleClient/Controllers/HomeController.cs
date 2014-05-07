using Com.GriffithsBen.BlogEngine.Concrete;
using Com.GriffithsBen.BlogEngine.SampleClient.Models;
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

        /// <summary>
        /// Return a view of all blog entries
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            List<SmartBloggable> model = new List<SmartBloggable>();

            foreach (BlogPost post in this.Repository.GetBlogEntries()) {
                SmartBloggable smartBlogPost = new SmartBloggable(post);
                smartBlogPost.ModelData.Add("Id", post.Id.ToString());
                model.Add(smartBlogPost);
            }

            return View(model);
        }

        /// <summary>
        /// Return a view of a single blog entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlogEntry(int id) {

            // retrieve a blog post entity from the mock repository
            BlogPost post = this.Repository.GetBlogEntryById(id);
            if (post == null) {
                return new HttpStatusCodeResult(404);
            }
            // wrap the entity in a SmartBlogPost to decorate it with SmartBlogPost behaviour
            SmartBloggable model = new SmartBloggable(post);
            model.ModelData.Add("Id", post.Id.ToString());

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            BlogPost model = this.Repository.GetBlogEntryById(id);
            if (model == null) {
                return new HttpStatusCodeResult(404);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BlogPost model) {
            if (ModelState.IsValid) {
                this.Repository.UpdateBlogEntry(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}