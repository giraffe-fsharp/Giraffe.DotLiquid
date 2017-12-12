module GiraffeDotLiquidSample

open System
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Giraffe
open DotLiquid

// ---------------------------------
// Web app
// ---------------------------------

type Person =
    {
        FirstName : string
        LastName  : string
    }

let personTemplate = "<html><head><title>DotLiquid</title></head><body><p>First name: {{ firstName }}<br />Last name: {{ lastName }}</p></body></html>"

let fooBar = { FirstName = "Foo"; LastName = "Bar" }

let webApp =
    choose [
        GET >=>
            choose [
                route "/"        >=> text "index"
                route "/person"  >=> dotLiquid "text/html" personTemplate fooBar
                route "/person2" >=> dotLiquidTemplate "text/html" "Templates/Person.liquid" fooBar
                route "/person3" >=> dotLiquidHtmlTemplate "Templates/Person.liquid" fooBar
            ]
        text "Not Found" |> RequestErrors.notFound ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> ServerErrors.INTERNAL_ERROR (text ex.Message)

// ---------------------------------
// Main
// ---------------------------------

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffeErrorHandler(errorHandler)
       .UseStaticFiles()
       .UseGiraffe webApp

let configureLogging (loggerBuilder : ILoggingBuilder) =
    loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
                 .AddConsole()
                 .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHost.CreateDefaultBuilder()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0