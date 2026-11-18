![Giraffe](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe/master/giraffe.png)

# Giraffe.DotLiquid

DotLiquid templating engine http handlers for the Giraffe web framework.

[![NuGet Info](https://buildstats.info/nuget/Giraffe.DotLiquid?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.DotLiquid/)

### Linux, macOS and Windows Build Status

![.NET Core](https://github.com/giraffe-fsharp/Giraffe.DotLiquid/workflows/.NET%20Core/badge.svg?branch=master)

[![Build history](https://buildstats.info/github/chart/giraffe-fsharp/Giraffe.DotLiquid?branch=master&includeBuildsFromPullRequest=false)](https://github.com/giraffe-fsharp/Giraffe.DotLiquid/actions?query=branch%3Amaster++)

## Table of contents

- [Documentation](#documentation)
    - [dotLiquid](#dotliquid)
    - [dotLiquidTemplate](#dotliquidtemplate)
    - [dotLiquidHtmlTemplate](#dotliquidhtmltemplate)
- [Samples](#samples)
- [Nightly builds and NuGet feed](#nightly-builds-and-nuget-feed)
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

## Nightly builds and NuGet feed

### NuGet trusted publishing

GitHub releases publish through [build.yml](.github/workflows/build.yml) using
GitHub OIDC and `NuGet/login` as the personal NuGet account `dsyme`, with package
permissions provided by the `fsprojects` NuGet organization.

Before publishing, configure a [NuGet trusted publishing policy](https://www.nuget.org/account/trustedpublishing)
with package owner `fsprojects`, GitHub repository owner `giraffe-fsharp`,
repository `Giraffe.DotLiquid`, workflow filename `build.yml`, no environment,
and scope to push new versions of the existing package `Giraffe.DotLiquid`.
Keep `dsyme` an active member of the NuGet organization.

After a successful trusted-publishing release, remove the obsolete GitHub
`NUGET_KEY` secret and revoke its NuGet API key if it is not used elsewhere.

### Package feeds

The test projects run on .NET 8 so they do not depend on the .NET 5 runtime's
obsolete OpenSSL libraries on current CI runners. The published library and
sample application's target frameworks are unchanged.

All official release packages are published to the official and public NuGet feed.

`master` is the integration branch; open pull requests against `master`.
GitHub Actions builds and tests pull requests and pushes to `master` on Linux,
Windows and macOS. Successful pushes to `master` also publish unofficial
pre-release packages to the [project's NuGet feed on GitHub](https://github.com/orgs/giraffe-fsharp/packages).
Pull requests never publish packages. Official nuget.org publication remains
triggered only by a published GitHub release.

Unofficial packages use the version `0.0.0-ci.<run-id>`.

All other builds, such as builds triggered by pull requests produce a NuGet package which can be downloaded as an artifact from the individual GitHub action.

After merging the master-only migration, maintainers can remove the obsolete
`develop` branch and its branch protection rule. Check that it still has no
commits missing from `master` before deleting it.

## More information

For more information about Giraffe, how to set up a development environment, contribution guidelines and more please visit the [main documentation](https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md) page.

## License

[Apache 2.0](https://raw.githubusercontent.com/giraffe-fsharp/Giraffe.DotLiquid/master/LICENSE)