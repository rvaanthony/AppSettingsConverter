# Examples

This directory contains example files to help you understand how AppSettingsConverter works.

## Files

- **appsettings.json** - A sample input JSON configuration file with various nested structures
- **appsettings.expected.json** - The expected output when flattening `appsettings.json` with `__` delimiter

## Usage

To test with the example file:

```bash
# From the project root
AppSettingsConverter examples/appsettings.json __ --format json --output examples/output.json

# Compare with expected output
diff examples/output.json examples/appsettings.expected.json
```

## What's Included

The example `appsettings.json` demonstrates:

- Simple key-value pairs
- Nested objects
- Deeply nested structures
- Arrays
- Different data types (strings, numbers, booleans)
- Connection strings
- Logging configuration
- Feature flags

This gives you a comprehensive example of how the converter handles various JSON structures.

