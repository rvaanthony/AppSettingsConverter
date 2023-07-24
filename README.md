# AppSettingsConverter

`AppSettingsConverter` is a .NET console application that reads a JSON configuration file, flattens its contents, and outputs the flattened configuration in a specific format.

## Usage

To use the `AppSettingsConverter` application, follow these steps:

1. Open a command prompt or terminal window.

2. Navigate to the directory where the `AppSettingsConverter.exe` is located.

3. Run the application with the following command-line arguments:

   ```
   AppSettingsConverter.exe <path_to_json_file> <delimiter>
   ```

   Replace `<path_to_json_file>` with the file path to the JSON configuration file that you want to flatten.

   Replace `<delimiter>` with the delimiter you want to use for flattening. The delimiter can be either `__` or `:`.

4. The application will process the JSON configuration file, flatten its contents, and output the flattened configuration in the specified format.

## Example

Suppose we have a JSON configuration file named `appsettings.json` with the following content:

```json
{
  "ApiUrl": "www.api.com",
  "Emails": {
    "Admin": "admin@admin.com"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

To flatten the configuration using `__` as the delimiter, we can run the following command:

```
AppSettingsConverter.exe appsettings.json __
```

The output will be:

```json
{
   "name": "ApiUrl",
   "value": "www.api.com",
   "slotSetting": false
},
{
   "name": "Emails__Admin",
   "value": "admin@admin.com",
   "slotSetting": false
},
{
   "name": "Logging__LogLevel__Default",
   "value": "Information",
   "slotSetting": false
},
{
   "name": "Logging__LogLevel__Microsoft.AspNetCore",
   "value": "Warning",
   "slotSetting": false
}
```

## Note

- Ensure that the JSON configuration file is accessible and in the correct format.

- The application will display an error message if the file is not found or if there are issues with the JSON format.

- The `slotSetting` property in the output will always be `false` in this version of the application. Modify the code to set it based on your requirements.

- The application uses `System.Text.Json` for parsing the JSON. Ensure that you have the appropriate .NET runtime installed on your system.

- Feel free to modify the output format or customize the code to suit your specific needs.

## License

This project is licensed under the [MIT License](LICENSE).

---

Feel free to customize the README with any additional information or instructions as per your needs.
