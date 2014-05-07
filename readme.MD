A simple little blogging engine for ASP.NET MVC, consisting of a small number of models and helper methods.

The blog engine classes can be used in two ways:

1. An existing domain model or view model can be wrapped inside a SmartBloggable object in order to provide some 
basic blog content markup behaviour. The wrapped model must implement the Bloggable interface.

2. Alternatively, the SmartBloggable methods can be used indirectly via the HtmlHelper extension methods within the namespace 
Com.GriffithsBen.BlogEngine.Extensions.