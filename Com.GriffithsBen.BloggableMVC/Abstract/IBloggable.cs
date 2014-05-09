﻿using Com.GriffithsBen.BloggableMVC.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GriffithsBen.BloggableMVC.Abstract {
    
    public interface IBloggable {

        string Title { get; set; }

        string Content { get; set; }

        string Author { get; set; }

        DateTime Date { get; set; }

        string DisplayName { get; set; }

        IEnumerable<IBloggable> Comments { get; set; }

    }
}