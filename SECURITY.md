# Security Policy

## Supported Versions

We actively support the following versions with security updates:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability, please **do not** open a public issue. Instead, please report it via one of the following methods:

1. **Email**: Send details to [security@yourdomain.com] (if you have a security email)
2. **GitHub Security Advisory**: Use GitHub's private vulnerability reporting feature (if enabled)

### What to Include

When reporting a vulnerability, please include:

- A description of the vulnerability
- Steps to reproduce the issue
- Potential impact
- Suggested fix (if you have one)

### Response Time

We will acknowledge your report within 48 hours and provide a more detailed response within 7 days, indicating the next steps in handling your report.

### Disclosure Policy

- We will notify you when we receive your report
- We will keep you informed of our progress
- We will notify you when the vulnerability is fixed
- We will credit you in the security advisory (unless you prefer to remain anonymous)

## Security Best Practices

When using AppSettingsConverter:

- **Never commit sensitive data** in JSON configuration files
- **Validate input files** before processing
- **Review output** before using in production environments
- **Keep the tool updated** to the latest version
- **Use secure file permissions** for configuration files containing sensitive information

## Known Security Considerations

- The tool processes JSON files as-is without validation of content
- Output files may contain sensitive information - ensure proper file permissions
- The tool does not encrypt or protect sensitive data in any way

Thank you for helping keep AppSettingsConverter and its users safe!

