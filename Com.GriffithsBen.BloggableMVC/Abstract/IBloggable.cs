using System;
using System.Collections.Generic;

namespace Com.GriffithsBen.BloggableMVC.Abstract {
    
    public interface IBloggable : IMarkupable {

        string Title { get; set; }

        string Author { get; set; }

        DateTime Date { get; set; }

        string DisplayName { get; set; }

        IEnumerable<IMarkupable> Comments { get; set; }

    }
}
