BloggableMVC
============

A library of utilities for developing a blogging site as part of an ASP.NET MVC application.

The bloggable utilities can be used in two ways:

1. An existing domain model or view model can be wrapped inside a SmartBloggable object in order to provide blog content markup behaviour. The wrapped model must implement the IBloggable interface.

2. Alternatively, the SmartBloggable behaviour can be used indirectly via the HtmlHelper extension methods within the namespace Com.GriffithsBen.BloggableMVC.Extensions.

Blog content can be marked up with simple, safe tags which are then converted into equivalent HTML tags by the SmartBloggable wrapper. The list of tags to be used is entirely up to the developer. The default tags (so far) are as follows:

tag | html equivalent |
--- | ---
`[b]` | `<span class="bold">`
`[i]` | `<i>`
`[p]` | `<p>`
`[quote]` | `<blockquote>`
`[link url="x" title="y"]` | `<a href="x" title="y">`
`[code]` | `<pre>`

The list of tags can be modified, extended, or replaced entirely at an Application or instance level.

The html used to replace a tag may have mandatory attributes set on it (for example, by default, a [b] tag is replaced with an
HTML span element with a class attribute on it with value "bold" )

Tags may also be configured to accept attributes which are then converted into equivalent HTML attributes. For example,
by default the [link] tag can accept url and title attributes which are converted into href and title attributes respectively.

BloggableMVC requires MVC4 or higher.

The sample client project has a dependency on MVC5 and requires Twitter Bootstrap and jQuery (tested with 1.10.2).