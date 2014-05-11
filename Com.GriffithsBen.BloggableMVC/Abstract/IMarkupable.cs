using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Abstract {

    public interface IMarkupable {
        /// <summary>
        /// The content which can be marked up
        /// </summary>
        string Content { get; set; }

    }
}
