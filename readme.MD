A simple little library of utilities to facilitate developing a bespoke blog as part of an ASP.NET MVC application.

The bloggable utilities can be used in two ways:

1. An existing domain model or view model can be wrapped inside a SmartBloggable object in order to provide some 
basic blog content markup behaviour. The wrapped model must implement the IBloggable interface.

2. Alternatively, the SmartBloggable methods can be used indirectly via the HtmlHelper extension methods within the namespace 
Com.GriffithsBen.BloggableMVC.Extensions.

Blog content can be marked up with simple, safe tags which are then converted into equivalent HTML tags by the 
SmartBloggable wrapper. The list of tags to be used is entirely up to the developer. The default tags are as follows:

tag		 html equivalent
[b]	  => em
i	  => i
p	  => p
quote => blockquote

The list of tags can be modified, extended, or replaced entirely at an Application or instance level.

