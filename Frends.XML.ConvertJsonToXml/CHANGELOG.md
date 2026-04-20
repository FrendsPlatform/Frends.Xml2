# Changelog

## [1.1.0] - 2025-03-25
### Changed
- Update packages:
  System.ComponentModel.Annotations  4.7.0  -> 5.0.0
  coverlet.collector                 3.1.2  -> 6.0.4
  Microsoft.NET.Test.Sdk             16.6.1 -> 17.13.0
  nunit                              3.12.0 -> 4.3.2
  NUnit3TestAdapter                  3.17.0 -> 5.0.0

## [1.0.1] - 2024-02-20
### Changed
- Refactored code to enable simple json transformation.
- Added feature to add default xml declaration if it's not provided in the json input.
- Updated documentation.
- Added [DisplayFormat(DataFormatString = "Json")] annotation to the input json field.
- Added more tests for different situations.

### Updated
- Newtonsoft.Json to version 13.0.3.

## [1.0.0] - 2023-11-28
### Changed
- Initial implementation
