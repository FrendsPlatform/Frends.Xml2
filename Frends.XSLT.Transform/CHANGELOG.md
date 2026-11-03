# Changelog

## [3.0.0] - 2026-11-03
### Changed
- The Task now targets .NET 8.
- **[Breaking Change]** Failures during XSLT compilation are now handled the same way as transformation failures, and error messages are no longer wrapped in "Error running XSLT transformation".

### Added
- Added `Success` and `Error` properties to the Task result.
- Added `ThrowErrorOnFailure` and `ErrorMessageOnFailure` options. By default the Task still throws on failure.

## [2.4.0] - 2026-07-14
### Changed
- Updated outdated repository URLs

## [2.3.0] - 2026-04-15
### Fixed
- Replace null in the XsltParameters list with an empty string to prevent null reference exceptions.

## [2.2.0] - 2026-04-09
### Changed
- Fixed persistent file locking issues by loading external documents into memory instead of accessing files directly during transformation.

## [2.1.0] - 2025-10-07
### Changed
- Updated documentation to clarify how to specify XSLT versions (1.0, 2.0, 3.0) using the stylesheet version attribute.

## [2.0.0] - 2025-01-15
### Changed
- Added an option to enable or disable external entity processing. This option is disabled by default starting from version 2.0.0, which may break XSLTs that rely on external entities. Set this option to true to enable support for external entities if required.

## [1.1.4] - 2024-10-10
### Changed
- Updated SaxonCS to 12.5.0

## [1.0.3] - 2023-08-03
## Changed
- Closing streams and clearing assembly load context to fix memory leak. 

## [1.0.2] - 2023-01-17
## Changed
- Upgrade Saxon version to 12.0

## [1.0.1] - 2023-01-16
### Changed
- Changed internal activation mechanism

## [1.0.0] - 2022-05-04
### Added
- Initial implementation
