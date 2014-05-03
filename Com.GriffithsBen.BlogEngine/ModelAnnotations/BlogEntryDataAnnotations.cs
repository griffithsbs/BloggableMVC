using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Com.GriffithsBen.BlogEngine.Models {
    
    public partial class BlogEntryDataAnnotations {

        public class BlogEntry {

            [ScaffoldColumn(false)]
            public int Id;
            
            [ScaffoldColumn(false)]
            public int? SynopsisLength;

            [Required]
            [Display(Name = "Title of blog post")]
            public string Title;

            [Required]
            [Display(Name = "Contents of blog post")]
            [DataType(DataType.MultilineText)]
            public string Content;

            [Required]
            [Display(Name = "Date of blog post")]
            public DateTime Date;

            [Display(Name = "Display name for url")]
            [StringLength(100)]
            [Required]
            public string DisplayName;

            public MvcHtmlString ContentHtml;
            
            [ScaffoldColumn(false)]
            public string Synopsis;

            [ScaffoldColumn(false)]
            public MvcHtmlString SynopsisHtml;

            [Display(Name = "Comments")]
            public IEnumerable<Comment> Comments;
        }
    }

}
