# Frends.XML.XPathQuery

License: CHECK [LICENSE](LICENSE.md) FILE
[![Build](https://github.com/FrendsPlatform/Frends.CrossplatformXML/actions/workflows/XPathQuery_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.CrossplatformXML/actions)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.XML.XPathQuery)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.CrossplatformXML/Frends.XML.XPathQuery|main)

Task for querying XML using XPath.

## Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-tasks/api/v2.

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
