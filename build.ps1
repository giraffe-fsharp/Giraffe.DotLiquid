# ----------------------------------------------
# Build script
# ----------------------------------------------

param
(
    [switch] $Release,
    [switch] $ExcludeTests,
    [switch] $ExcludeSamples,
    [switch] $Pack,
    [switch] $Run,
    [switch] $ClearOnly
)

# ----------------------------------------------
# Main
# ----------------------------------------------

$ErrorActionPreference = "Stop"

Import-module "$PSScriptRoot/.psscripts/build-functions.ps1" -Force

Write-BuildHeader "Starting Giraffe.DotLiquid build script"

if ($ClearOnly.IsPresent)
{
    Remove-OldBuildArtifacts
    return
}

$giraffeDotLiquid      = ".\src\Giraffe.DotLiquid\Giraffe.DotLiquid.fsproj"
$giraffeDotLiquidTests = ".\tests\Giraffe.DotLiquid.Tests\Giraffe.DotLiquid.Tests.fsproj"
$sampleApp             = ".\samples\GiraffeDotLiquidSample\GiraffeDotLiquidSample.fsproj"
$sampleAppTests        = ".\samples\GiraffeDotLiquidSample.Tests\GiraffeDotLiquidSample.Tests.fsproj"

$version = Get-ProjectVersion $giraffeDotLiquid
Update-AppVeyorBuildVersion $version

if (Test-IsAppVeyorBuildTriggeredByGitTag)
{
    $gitTag = Get-AppVeyorGitTag
    Test-CompareVersions $version $gitTag
}

Write-DotnetCoreVersions
Remove-OldBuildArtifacts

$configuration = if ($Release.IsPresent) { "Release" } else { "Debug" }

Write-Host "Building Giraffe.DotLiquid..." -ForegroundColor Magenta
dotnet-build   $giraffeDotLiquid "-c $configuration"

if (!$ExcludeTests.IsPresent -and !$Run.IsPresent)
{
    Write-Host "Building and running tests..." -ForegroundColor Magenta

    dotnet-build   $giraffeDotLiquidTests
    dotnet-test    $giraffeDotLiquidTests
}

if (!$ExcludeSamples.IsPresent -and !$Run.IsPresent)
{
    Write-Host "Building and testing samples..." -ForegroundColor Magenta
    dotnet-build   $sampleApp
    dotnet-build   $sampleAppTests
    dotnet-test    $sampleAppTests
}

if ($Run.IsPresent)
{
    Write-Host "Launching sample application..." -ForegroundColor Magenta
    dotnet-build   $sampleApp
    dotnet-run     $sampleApp
}

if ($Pack.IsPresent)
{
    Write-Host "Packaging Giraffe.DotLiquid NuGet package..." -ForegroundColor Magenta

    dotnet-pack $giraffeDotLiquid "-c $configuration"
}

Write-SuccessFooter "Giraffe.DotLiquid build completed successfully!"