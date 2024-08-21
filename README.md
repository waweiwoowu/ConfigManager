# ConfigManager Documentation

## Overview

The `ConfigManager` package provides utilities for managing configuration files in different formats, including INI, JSON, and XML. This documentation covers the methods available in each class within the package.

## Namespace
- **`ConfigManager`**

## Classes

- **`IniFileHandler`**: Handles operations related to INI files.
- **`JsonFileHandler`**: Handles operations related to JSON files.
- **`XmlFileHandler`**: Handles operations related to XML files.

#### `IniFileHandler(string filePath)`
  - **Description**: Initializes a new instance of the `IniFileHandler` class. Loads the INI file and parses its content into memory.
  - **Parameters**:
    - `filePath` (`string`): The path to the INI file.
  - **Exceptions**: 
    - `FileNotFoundException`: Thrown when the specified INI file is not found.

#### `JsonFileHandler(string filePath)`
  - **Description**: Initializes a new instance of the `JsonFileHandler` class. Loads the JSON file and parses its content into memory.
  - **Parameters**:
    - `filePath` (`string`): The path to the JSON file.
  - **Exceptions**: 
    - `FileNotFoundException`: Thrown when the specified JSON file is not found.

#### `XmlFileHandler(string filePath)`
  - **Description**: Initializes a new instance of the `XmlFileHandler` class. Loads the XML file and parses its content into memory.
  - **Parameters**:
    - `filePath` (`string`): The path to the XML file.
  - **Exceptions**: 
    - `FileNotFoundException`: Thrown when the specified XML file is not found.

## Methods

### Loading Data

#### `void LoadFile()`
- **Description**: Loads and parses the file into a dictionary structure. Each section is stored as a key, with another dictionary of key-value pairs as the value.
- **Exceptions**: 
  - `FileNotFoundException`: Thrown when the specified file is not found.

### Reading Data

#### `string GetString(string section, string key, string defaultValue)`
- **Description**: Retrieves the value associated with a given key in a specified section as a string.
- **Parameters**:
  - `section` (`string`): The section containing the key.
  - `key` (`string`): The key whose value is to be retrieved.
  - `defaultValue` (`string`): The default value to return if the key is not found.
- **Returns**: `string` - The value associated with the specified key, or the default value if the key is not found.

#### `bool GetBool(string section, string key, bool defaultValue)`
- **Description**: Retrieves the value associated with a given key in a specified section as a boolean.
- **Parameters**:
  - `section` (`string`): The section containing the key.
  - `key` (`string`): The key whose value is to be retrieved.
  - `defaultValue` (`bool`): The default value to return if the key is not found or cannot be parsed.
- **Returns**: `bool` - The boolean value associated with the specified key, or the default value if the key is not found or cannot be parsed.
- **Exceptions**: 
  - `FormatException`: Thrown when the value cannot be parsed as a boolean.

#### `int GetInt(string section, string key, int defaultValue)`
- **Description**: Retrieves the value associated with a given key in a specified section as an integer.
- **Parameters**:
  - `section` (`string`): The section containing the key.
  - `key` (`string`): The key whose value is to be retrieved.
  - `defaultValue` (`int`): The default value to return if the key is not found or cannot be parsed.
- **Returns**: `int` - The integer value associated with the specified key, or the default value if the key is not found or cannot be parsed.
- **Exceptions**: 
  - `FormatException`: Thrown when the value cannot be parsed as an integer.

#### `float GetFloat(string section, string key, float defaultValue)`
- **Description**: Retrieves the value associated with a given key in a specified section as a float.
- **Parameters**:
  - `section` (`string`): The section containing the key.
  - `key` (`string`): The key whose value is to be retrieved.
  - `defaultValue` (`float`): The default value to return if the key is not found or cannot be parsed.
- **Returns**: `float` - The float value associated with the specified key, or the default value if the key is not found or cannot be parsed.
- **Exceptions**: 
  - `FormatException`: Thrown when the value cannot be parsed as a float.

#### `double GetDouble(string section, string key, double defaultValue)`
- **Description**: Retrieves the value associated with a given key in a specified section as a double.
- **Parameters**:
  - `section` (`string`): The section containing the key.
  - `key` (`string`): The key whose value is to be retrieved.
  - `defaultValue` (`double`): The default value to return if the key is not found or cannot be parsed.
- **Returns**: `double` - The double value associated with the specified key, or the default value if the key is not found or cannot be parsed.
- **Exceptions**: 
  - `FormatException`: Thrown when the value cannot be parsed as a double.

#### `List<string> GetSections()`
- **Description**: Retrieves a list of all sections in the file.
- **Returns**: `List<string>` - A list of section names.

#### `List<string> GetKeys(string section)`
- **Description**: Retrieves a list of all keys in a specified section.
- **Parameters**:
  - `section` (`string`): The section whose keys are to be retrieved.
- **Returns**: `List<string>` - A list of keys in the specified section.
- **Exceptions**: 
  - `ArgumentException`: Thrown when the specified section does not exist.

#### `Dictionary<string, string> GetAllKeyValues(string section)`
- **Description**: Retrieves all key-value pairs in a specified section.
- **Parameters**:
  - `section` (`string`): The section whose key-value pairs are to be retrieved.
- **Returns**: `Dictionary<string, string>` - A dictionary of key-value pairs in the specified section.
- **Exceptions**: 
  - `ArgumentException`: Thrown when the specified section does not exist.

#### `bool SectionExists(string section)`
- **Description**: Checks if a specified section exists in the file.
- **Parameters**:
  - `section` (`string`): The section to check for existence.
- **Returns**: `bool` - True if the section exists; otherwise, false.

#### `bool KeyExists(string section, string key)`
- **Description**: Checks if a specified key exists in a given section of the file.
- **Parameters**:
  - `section` (`string`): The section to check.
  - `key` (`string`): The key to check for existence.
- **Returns**: `bool` - True if the key exists; otherwise, false.

### Writing Data

#### `void SetString(string section, string key, string value)`
- **Description**: Sets or updates the value for a specified key in a given section. If the section or key does not exist, it is created.
- **Parameters**:
  - `section` (`string`): The section to modify or create.
  - `key` (`string`): The key to modify or create.
  - `value` (`string`): The value to assign to the key.

#### `void CreateSection(string section)`
- **Description**: Creates a new section in the file.
- **Parameters**:
  - `section` (`string`): The name of the section to create.
- **Exceptions**: 
  - `ArgumentException`: Thrown when the section already exists.

### Deleting Data

#### `void DeleteSection(string section)`
- **Description**: Deletes a specified section from the file.
- **Parameters**:
  - `section` (`string`): The section to delete.
- **Exceptions**: 
  - `ArgumentException`: Thrown when the section does not exist.

#### `void DeleteKey(string section, string key)`
- **Description**: Deletes a specified key from a given section in the file.
- **Parameters**:
  - `section` (`string`): The section containing the key to delete.
  - `key` (`string`): The key to delete.
- **Exceptions**: 
  - `ArgumentException`: Thrown when the section or key does not exist.

### Saving Data

#### `void SaveFile()`
- **Description**: Saves the current state of the file to disk. Overwrites the existing file with the current data in memory.
