# Changelog

## [2.1.0] - 2026-02-12
### Fixed
- Prevent XML External Entity (XXE) attack, by adding option to disable External Entities

## [2.0.0] - 2025-04-08
### Added
- Upgrade dependencies
  - Saxon 12.0.0 -> 12.5.0
  - Newtonsoft.Json 12.0.3 -> 13.0.3
- Adding option `SchemaAware`, with default value `false`
  - The default value the same as was before, so this will not break existing task usage. 
- [Breaking] Adding option `SchemaValidationMode`, with default value `None`
  - The default value `None` is different from the previously used `Lax` mode.
  - To maintain the same behavior as before, set `SchemaValidationMode` to `Lax`.

## [1.1.0] - 2023-08-14
### Added
- Added option to return raw xml data instead of JToken.

## [1.0.1] - 2023-01-20
### Fixed
- Fixed `text()` query handling, so task no longer crashes (issue #13) 

## [1.0.0] - 2023-01-19
### Added
- Initial implementation