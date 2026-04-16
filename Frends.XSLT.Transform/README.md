# Frends.XSLT.Transform

License: CHECK [LICENSE](LICENSE.md) FILE
[![Build](https://github.com/FrendsPlatform/Frends.CrossplatformXML/actions/workflows/Transform_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.CrossplatformXML/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.CrossplatformXML/Frends.XSLT.Transform|main)

Task for transforming XML using XSLT.

## Installing

You can install the Task via frends UI Task View.

## Licensing

License key for Saxon is only inserted during the build process and you can ask Jefim for details if you need to build this on your own machine.

### License key

If you are updating the license key to repository secrets, then make sure that you escape the dollar signs in it (so that any '$' should become '`$' for PowerShell to be able to process it properly).

## Building

Rebuild the project

`dotnet build`

Run tests

`dotnet test`

Create a NuGet package

`dotnet pack --configuration Release`
