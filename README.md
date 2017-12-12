# Giraffe.DotLiquid

![Giraffe](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe/master/giraffe.png)

DotLiquid templating engine http handlers for the Giraffe web framework.

[![NuGet Info](https://buildstats.info/nuget/Giraffe.DotLiquid?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.DotLiquid/)

| Windows | Linux |
| :------ | :---- |
| [![Windows Build status](https://ci.appveyor.com/api/projects/status/914030ec0lrc0vti/branch/develop?svg=true)](https://ci.appveyor.com/project/dustinmoris/giraffe-dotliquid/branch/develop) | [![Linux Build status](https://travis-ci.org/giraffe-fsharp/Giraffe.DotLiquid.svg?branch=develop)](https://travis-ci.org/giraffe-fsharp/Giraffe.DotLiquid/builds?branch=develop) |
| [![Windows Build history](https://buildstats.info/appveyor/chart/dustinmoris/giraffe-dotliquid?branch=develop&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/dustinmoris/giraffe-dotliquid/history?branch=develop) | [![Linux Build history](https://buildstats.info/travisci/chart/giraffe-fsharp/Giraffe.DotLiquid?branch=develop&includeBuildsFromPullRequest=false)](https://travis-ci.org/giraffe-fsharp/Giraffe.DotLiquid/builds?branch=develop) |

## Table of contents

- [Documentation](#documentation)
    - [dotLiquid](#dotliquid)
    - [dotLiquidTemplate](#dotliquidtemplate)
    - [dotLiquidHtmlTemplate](#dotliquidhtmltemplate)
- [Samples](#samples)
- [More information](#more-information)
- [License](#license)

## Documentation

The `Giraffe.DotLiquid` NuGet package adds additional `HttpHandler` functions to render DotLiquid templates in Giraffe.

### dotLiquid

`dotLiquid` uses the [DotLiquid](http://dotliquidmarkup.org/) template engine to set or modify the body of the `HttpResponse`. This http handler triggers a response to the client and other http handlers will not be able to modify the HTTP headers afterwards any more.

The `dotLiquid` handler requires the content type and the actual template to be passed in as two string values together with an object model. This handler is supposed to be used as the base handler for other http handlers which want to utilize the DotLiquid template engine (e.g. you could create an SVG handler on top of it).

#### Example:

```fsharp
open Giraffe
open Giraffe.DotLiquid

type Person =
    {
        FirstName : string
        LastName  : string
    }

let fooBar = { FirstName = "Foo"; LastName = "Bar" }

let template = "<html><head><title>DotLiquid</title></head><body><p>First name: {{ firstName }}<br />Last name: {{ lastName }}</p></body></html>"

let app =
    choose [
        route  "/foo" >=> dotLiquid "text/html" template fooBar
    ]
```

### dotLiquidTemplate

`dotLiquidTemplate` uses the [DotLiquid](http://dotliquidmarkup.org/) template engine to set or modify the body of the `HttpResponse`. This http handler triggers a response to the client and other http handlers will not be able to modify the HTTP headers afterwards any more.

This http handler takes a relative path of a template file, an associated model and the contentType of the response as parameters.

#### Example:

```fsharp
open Giraffe
open Giraffe.DotLiquid

type Person =
    {
        FirstName : string
        LastName  : string
    }

let fooBar = { FirstName = "Foo"; LastName = "Bar" }

let app =
    choose [
        route  "/foo" >=> dotLiquidTemplate "text/html" "templates/person.html" fooBar
    ]
```

### dotLiquidHtmlTemplate

`dotLiquidHtmlTemplate` is the same as `dotLiquidTemplate` except that it automatically sets the response as `text/html`.

#### Example:

```fsharp
open Giraffe
open Giraffe.DotLiquid

type Person =
    {
        FirstName : string
        LastName  : string
    }

let fooBar = { FirstName = "Foo"; LastName = "Bar" }

let app =
    choose [
        route  "/foo" >=> dotLiquidHtmlTemplate "templates/person.html" fooBar
    ]
```

## Samples

Please find a fully functioning sample application under [./samples/GiraffeDotLiquidSample/](https://github.com/giraffe-fsharp/Giraffe.DotLiquid/tree/master/samples/GiraffeDotLiquidSample).

## More information

For more information about Giraffe, how to set up a development environment, contribution guidelines and more please visit the [main documentation](https://github.com/giraffe-fsharp/Giraffe#table-of-contents) page.

## License

[Apache 2.0](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe.DotLiquid/master/LICENSE)