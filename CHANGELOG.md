# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Support for outputting to file with `--output` option
- Support for JSON array output format with `--format json` option
- Proper JSON escaping for values containing special characters
- Comprehensive error handling with exit codes
- Help message with `--help` option
- Support for null JSON values
- Improved command-line argument parsing

### Changed
- Fixed trailing comma issue in output
- Improved error messages
- Better handling of edge cases
- Enhanced project metadata and properties
- Enhanced code structure and organization

### Fixed
- JSON value escaping (quotes, newlines, etc.)
- Null value handling
- Delimiter validation
- File read/write error handling

## [1.0.0] - 2024-01-XX

### Added
- Initial release
- Basic JSON flattening functionality
- Support for custom delimiters
- Console output format

