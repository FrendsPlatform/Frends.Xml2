(Get-Content 'Frends.XSLT.Transform\\Definitions\\LicenseKey.cs') `
-replace '__LICENSE_KEY__', "$env:LICENSE_KEY" | `
Out-File 'Frends.XSLT.Transform\\Definitions\\LicenseKey.cs'; `
gc 'Frends.XSLT.Transform\\Definitions\\LicenseKey.cs'
