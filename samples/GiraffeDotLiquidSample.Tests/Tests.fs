module GiraffeDotLiquidSample.Tests

open System
open System.Net.Http
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Xunit

// ---------------------------------
// Test server/client setup
// ---------------------------------

let createHost() =
    WebHostBuilder()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Configure(Action<IApplicationBuilder> GiraffeDotLiquidSample.configureApp)

// ---------------------------------
// Helper functions
// ---------------------------------

let runTask task =
    task
    |> Async.AwaitTask
    |> Async.RunSynchronously

let get (client : HttpClient) (path : string) =
    path
    |> client.GetAsync
    |> runTask

let ensureSuccess (response : HttpResponseMessage) =
    if not response.IsSuccessStatusCode
    then response.Content.ReadAsStringAsync() |> runTask |> failwithf "%A"
    else response

let isOfType (contentType : string) (response : HttpResponseMessage) =
    Assert.Equal(contentType, response.Content.Headers.ContentType.MediaType)
    response

let readText (response : HttpResponseMessage) =
    response.Content.ReadAsStringAsync()
    |> runTask

let shouldEqual (expected : string) (actual : string) =
    Assert.Equal(expected, actual)

// ---------------------------------
// Tests
// ---------------------------------

[<Fact>]
let ``Test /person returns html content`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()
    let expected = @"<html><head><title>DotLiquid</title></head><body><p>First name: Foo<br />Last name: Bar</p></body></html>"

    get client "/person"
    |> ensureSuccess
    |> isOfType "text/html"
    |> readText
    |> shouldEqual expected

[<Fact>]
let ``Test /person2 returns html content`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()

    let expected = @"<html>
<head>
    <title>DotLiquid</title>
</head>
<body>
    <p><strong>First name:</strong> Foo<br /><strong>Last name:</strong> Bar</p>
</body>
</html>"

    get client "/person2"
    |> ensureSuccess
    |> isOfType "text/html"
    |> readText
    |> shouldEqual expected

[<Fact>]
let ``Test /person3 returns html content`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()

    let expected = @"<html>
<head>
    <title>DotLiquid</title>
</head>
<body>
    <p><strong>First name:</strong> Foo<br /><strong>Last name:</strong> Bar</p>
</body>
</html>"

    get client "/person3"
    |> ensureSuccess
    |> isOfType "text/html"
    |> readText
    |> shouldEqual expected