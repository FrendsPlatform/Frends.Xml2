# Changelog

## [2.0.0] - 2026-11-03
### Changed
- **[Breaking Change]** Merged the `Output` parameter group into `Input`; `OutputType`, `OutputFilePath`, `OutputEncoding` and `AddSignatureToSourceFile` are now found in the `Input` tab.
- **[Breaking Change]** The task now returns a result object containing `Success`, `Output` and `Error`. The signed XML or the output file path, previously returned in `Result`, is now in `Output`.
- Added `ThrowErrorOnFailure` and `ErrorMessageOnFailure` options. When `ThrowErrorOnFailure` is disabled, failures are returned in the `Error` property of the result instead of being thrown.
- Updated the task to target .NET 8.

## [1.3.0] - 2026-07-14
### Changed
- Updated outdated repository URLs

## [1.2.0] - 2025-07-10
### Changed
- Update documentation link

## [1.1.0] - 2025-03-25
### Changed
- Update packages:
  System.ComponentModel.Annotations  4.7.0  -> 5.0.0
  System.Security.Cryptography.Xml   8.0.1  -> 9.0.3
  coverlet.collector                 3.1.2  -> 6.0.4
  Microsoft.NET.Test.Sdk             16.6.1 -> 17.13.0
  nunit                              3.12.0 -> 4.3.2
  NUnit3TestAdapter                  3.17.0 -> 5.0.0

## [1.0.0] - 2024-06-13
### Changed
- Initial implementation
