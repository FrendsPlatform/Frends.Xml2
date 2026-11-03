# Changelog

## [2.0.0] - 2026-11-03
### Changed
- **[Breaking Change]** Task now targets .NET 8.
- **[Breaking Change]** The `Error` result property is now an object with `Message` and `AdditionalInfo` instead of a string.
- **[Breaking Change]** Unexpected failures (e.g. invalid XML) now throw by default; set `ThrowErrorOnFailure` to false to get them in the result instead.
- Added `Success` property to the result.
- Added `ThrowErrorOnFailure` and `ErrorMessageOnFailure` options.
- Task now supports cancellation.

## [1.2.0] - 2026-07-14
### Changed
- Updated outdated repository URLs

## [1.1.0] - 2025-03-25
### Changed
- Update packages:
  System.Text.Encoding.CodePages  6.0.0   -> 9.0.3
  coverlet.collector              3.1.0   -> 6.0.4
  Microsoft.NET.Test.Sdk          16.11.0 -> 17.13.0
  NUnit                           3.13.2  -> 4.3.2
  NUnit3TestAdapter               4.0.0   -> 5.0.0

## [1.0.0] - 2023-10-11
### Added
- Initial implementation