# WkHtmlToWrapper

Yes! This is **ANOTHER** wrapper for the incredible and archived [wkhtmltopdf](https://github.com/wkhtmltopdf/wkhtmltopdf).

## Why have you done this?

> Well, because it was fun to.

I've used [Rotativa.AspNetCore](https://github.com/webgio/Rotativa.AspNetCore) library many times since started with AspNet and
I found it useful and works fine most of the time, and under the roof it uses wkhtmltopdf.

Although I like Rotativa I feel that it misses many wkhtmltopdf configurations when using it with the default Rotativa's classes, 
and it was also coupled directly to MVC, so I've decided to create a brand new wrapper that enables you to easily 
use `wkhtmltopdf` inside and outside of an AspNet project.

Additionaly, it was added many other informations and parameters/arguments that are nice but hidden at Rotativa's API.

## Will keep maintaining this repo?

Partially. I have no intention of maintaining with all my will something that is [clearly deprecated](https://wkhtmltopdf.org/status.html). 
But until it **dies** completely I'll try to keep up with the repo.

There are many others awesome PDF generators that you might consider. 
Such as [WeasyPrint](https://github.com/Kozea/WeasyPrint/) that is a more modern and maintainable library.

> But if you still stick with this, feel free to use.

## NuGet 

```bash
# Wrapper library
dotnet add package WkHtmlTo.Wrapper

# AspNetCore MVC
dotnet add package WkHtmlTo.Wrapper.AspNetCore

# Blazor server
dotnet add package WkHtmlTo.Wrapper.BlazorServer
```

## How to use?

The heart of the library is the **Wrapper**, so everything ends up on him and you can use it anywhere you want. 
However there are some abstractions created for Razor and Blazor server components .

### AspNetCore abstraction

