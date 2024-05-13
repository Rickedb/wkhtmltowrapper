# WkHtmlToWrapper

Yes! This is **ANOTHER** wrapper for the incredible and archived [wkhtmltopdf](https://github.com/wkhtmltopdf/wkhtmltopdf).

## Why have you done this?

> _Mainly because it was fun to.__

I've used [Rotativa.AspNetCore](https://github.com/webgio/Rotativa.AspNetCore) library many times since started with AspNet which
I've found it very useful, works fine most of the time and uses wkhtmltopdf under the roof.

Although I like Rotativa I feel that it misses many wkhtmltopdf configurations when using it with the default Rotativa's classes, 
most of the time if you need to use any other command option you would need to append it with a custom string containing all
the desired options. 

It was also directly coupled to MVC/AspNetCore, which obligates you to use the entire AspNet engine to render html, so in cases you have
the html already done or other engines to generate you could not convert to PDF without going through the AspNet engine.

That's why I've decided to create a brand new wrapper that enables you to easily use `wkhtmltopdf` inside and outside of an AspNet project.

## Will keep maintaining this repo?

Partially. I have no intention of maintaining something that the main thing is [already deprecated](https://wkhtmltopdf.org/status.html). 
But until it **dies** completely I'll try to keep up with the repo by adding some new .NET features and upgrading .NET versions.

You might consider many others awesome PDF generators that are still maintained, such as [WeasyPrint](https://github.com/Kozea/WeasyPrint/).

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
However there are some abstractions created for Razor and Blazor server components. 

### Setting up environment

[Download the binary](https://wkhtmltopdf.org/downloads.html) of the operational system where your code will be running.

> If you run in both OS like Windows for development and publish on Linux, you should consider downloading both.


### Usage examples

#### Basic usage
```csharp
var wrapper = new WkHtmlToPdfWrapper(); //When not providing the root path of the executable, it considers the same path of your assembly
var html = "<html><body><h1>Hello World!</h1></body></html>";
var options = new HtmlOptions(html);
var result = await wrapper.ConvertAsync(options);
if(result.Success)
{
	var bytes = result.GetBytes();
	//Do your thing...
}
```

### AspNetCore usage

When using .net MVC, it's possible to return 3 types of results: `ViewResult`, `FileStreamResult` and `FileContentResult`.

All of them might lead to the same purpose that is to download the pdf, so please always consider using `FileStreamResult` which will stream the download gradually to the client
instead the others that will output the file full size in one shot.

#### Dependency injection

```csharp
//Program.cs
builder.Services.AddWkHtmlToWrapper();
```

```csharp
//Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddWkHtmlToWrapper();
    //...
}
```

#### ViewResult
```csharp
public IActionResult MyView()
{
    //It will search for MyView.cshtml
    var result = new PdfViewResult(new MyViewModel());
    return result;
}
```

#### FileStreamResult

```csharp
public IActionResult StreamDownload()
{
    //It will search for "AnotherView.cshtml"
    var result = new PdfFileStreamResult("AnotherView");
    return result;
}
```

#### FileContentResult

```csharp
public IActionResult Download()
{
    //It will search for "AnotherView.cshtml" using MyViewModel
    var result = new PdfFileContentResult("AnotherView", new MyViewModel());
    return result;
}
```

### Blazor server usage

At Blazor server it relies on the new `HtmlRenderer` class to render the component string. 

> At the point we are now with Blazor Server, unlike the **AspNetCore MVC**, it only renders the given component without considering the root component _(e.g.: `MainLayout.razor`)_, 
so you might need to include your entire HTML at the rendered component.

#### Dependency injection

```csharp
//Program.cs
builder.Services.AddWkHtmlToPdfWrapper();
```

#### Include the JS

Open `Components\App.razor` and include:

```html
<script src="_content/WkHtmlTo.Wrapper.BlazorServer/WkHtmlTo.Wrapper.js"></script>
```

> This will enable to trigger download at the client's browser.

#### Exporting component

When exporting, it will runs the complete initialization lifecycle until it renders to Html and the your pdf

##### FetchData.razor (Page to be exported)

```
@page "/fetchdata"

<PageTitle>Weather forecast</PageTitle>

@using WkHtmlToPdf.Wrapper.BlazorServer.Tests.Data
@inject WeatherForecastService ForecastService

<h1>Weather forecast</h1>

<p>This component demonstrates fetching data from a service.</p>

@if (forecasts == null)
{
    <p><em>Loading...</em></p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var forecast in forecasts)
            {
                <tr>
                    <td>@forecast.Date.ToShortDateString()</td>
                    <td>@forecast.TemperatureC</td>
                    <td>@forecast.TemperatureF</td>
                    <td>@forecast.Summary</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
    }
}
```

##### Index.razor (Page where we want to run export)
```
@page "/"

<PageTitle>Index</PageTitle>

<h1>Hello, world!</h1>

Welcome to your new app.
<SurveyPrompt Title="How is Blazor working for you?" />
<button class="btn btn-primary" @onclick="ExportDataAsync">Click me to export data</button>

@code {
    public async Task RunAsync()
    {
        await Renderer.GenerateAndDownloadAsync<FetchData>();
    }
}
```