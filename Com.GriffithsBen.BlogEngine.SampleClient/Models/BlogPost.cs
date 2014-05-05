﻿using Com.GriffithsBen.BlogEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.GriffithsBen.BlogEngine.SampleClient.Models {

    /// <summary>
    /// This is a mock data access object, which for the purposes of this example is also being used as
    /// the view model.
    /// 
    /// We implement the IBloggable interface so that we can decorate the model with SmartBlogPost behaviour
    /// </summary>
    public class BlogPost : IBloggable {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string DisplayName { get; set; }

        public IEnumerable<IBloggable> Comments { get; set; }
    }
}