﻿using Com.GriffithsBen.BlogEngine.Concrete;
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

            // retrieve a blog post entity from the mock repository
            BlogPost post = this.Repository.GetBlogEntryById(id);
            if (post == null) {
                return new HttpStatusCodeResult(404);
            }
            // wrap the entity in a SmartBlogPost to decorate it with SmartBlogPost behaviour
            SmartBlogPost model = new SmartBlogPost(post);

            return View(model);
        }

    }
}