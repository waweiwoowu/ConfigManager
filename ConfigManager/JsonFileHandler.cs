using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ConfigManager
{
    /// <summary>
    /// The JsonFileHandler class provides methods to interact with a JSON file, 
    /// including reading, writing, and managing sections and key-value pairs.
    /// </summary>
    public class JsonFileHandler
    {
        private readonly string _filePath;
        private Dictionary<string, Dictionary<string, string>> _sections;

        /// <summary>
        /// Initializes a new instance of the JsonFileHandler class with the specified file path.
        /// Loads the data from the JSON file into memory.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        public JsonFileHandler(string filePath)
        {
            _filePath = filePath;
            LoadFile();
        }

        #region Loading Data

        /// <summary>
        /// Loads the JSON file into memory as a dictionary of sections and key-value pairs.
        /// If the file does not exist, creates an empty JSON file.
        /// </summary>
        private void LoadFile()
        {
            if (!File.Exists(_filePath))
            {
                // Create an empty JSON file
                _sections = new Dictionary<string, Dictionary<string, string>>();
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_sections, Newtonsoft.Json.Formatting.Indented));
            }
            else
            {
                string jsonContent = File.ReadAllText(_filePath);
                _sections = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonContent)
                            ?? new Dictionary<string, Dictionary<string, string>>();
            }
        }

        #endregion

        #region Reading Data

        /// <summary>
        /// Retrieves the value for a specified key in a given section as a specific type.
        /// If the section or key does not exist, the default value is returned.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to retrieve the value from.</param>
        /// <param name="key">The key to retrieve the value for.</param>
        /// <param name="defaultValue">The default value to return if the key or section does not exist.</param>
        /// <returns>The value associated with the specified key, cast to the specified type, or the default value.</returns>
        public T Get<T>(string section, string key, T defaultValue)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var value))
                {
                    try
                    {
                        // Attempt to convert the string value to the desired type
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        // Handle cases where the conversion is not possible
                        return defaultValue;
                    }
                    catch (FormatException)
                    {
                        // Handle format errors (e.g., parsing "abc" as an int)
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves the array for a specified key in a given section.
        /// If the section or key does not exist, the default array is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to retrieve the array from.</param>
        /// <param name="key">The key to retrieve the array for.</param>
        /// <param name="defaultValue">The default array to return if the key or section does not exist.</param>
        /// <returns>The array associated with the specified key, or the default array.</returns>
        public T[] GetArray<T>(string section, string key, T[] defaultValue)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var jsonArray))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T[]>(jsonArray) ?? defaultValue;
                    }
                    catch (JsonException)
                    {
                        // Handle deserialization error
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves a two-dimensional array for a specified key in a given section.
        /// If the section or key does not exist, the default array is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to retrieve the array from.</param>
        /// <param name="key">The key to retrieve the array for.</param>
        /// <param name="defaultValue">The default array to return if the key or section does not exist.</param>
        /// <returns>The two-dimensional array associated with the specified key, or the default array.</returns>
        public T[,] GetArray<T>(string section, string key, T[,] defaultValue)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var jsonArray))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T[,]>(jsonArray) ?? defaultValue;
                    }
                    catch (JsonException)
                    {
                        // Handle deserialization error
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves a jagged array for a specified key in a given section.
        /// If the section or key does not exist, the default array is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to retrieve the array from.</param>
        /// <param name="key">The key to retrieve the array for.</param>
        /// <param name="defaultValue">The default jagged array to return if the key or section does not exist.</param>
        /// <returns>The jagged array associated with the specified key, or the default array.</returns>
        public T[][] GetArray<T>(string section, string key, T[][] defaultValue)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var jsonArray))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T[][]>(jsonArray) ?? defaultValue;
                    }
                    catch (JsonException)
                    {
                        // Handle deserialization error
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves all section names from the JSON file.
        /// </summary>
        /// <returns>A list of section names.</returns>
        public List<string> GetSections()
        {
            return new List<string>(_sections.Keys);
        }

        /// <summary>
        /// Retrieves all keys in a specified section.
        /// </summary>
        /// <param name="section">The section to retrieve keys from.</param>
        /// <returns>A list of keys in the specified section.</returns>
        /// <exception cref="ArgumentException">Thrown when the section does not exist.</exception>
        public List<string> GetKeys(string section)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                return new List<string>(sectionData.Keys);
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }

        /// <summary>
        /// Checks if a specified section exists in the JSON file.
        /// </summary>
        /// <param name="section">The section to check.</param>
        /// <returns>True if the section exists; otherwise, false.</returns>
        public bool SectionExists(string section)
        {
            return _sections.ContainsKey(section);
        }

        /// <summary>
        /// Checks if a specified key exists in a given section of the JSON file.
        /// </summary>
        /// <param name="section">The section to check.</param>
        /// <param name="key">The key to check for existence.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public bool KeyExists(string section, string key)
        {
            return _sections.TryGetValue(section, out var sectionData) && sectionData.ContainsKey(key);
        }
        #endregion

        #region Writing Data

        /// <summary>
        /// Creates a new section in the JSON file.
        /// </summary>
        /// <param name="section">The name of the section to create.</param>
        /// <exception cref="ArgumentException">Thrown when the section already exists.</exception>
        public void CreateSection(string section)
        {
            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }
            else
            {
                throw new ArgumentException($"Section '{section}' already exists.");
            }
        }

        /// <summary>
        /// Sets or updates the value for a specified key in a given section.
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <typeparam name="T">The type of the value to assign (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to modify or create.</param>
        /// <param name="key">The key to modify or create.</param>
        /// <param name="value">The value to assign to the key, converted to a string.</param>
        public void Set<T>(string section, string key, T value)
        {
            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }

            // Convert the value to a string representation
            string stringValue = value.ToString();
            _sections[section][key] = stringValue;
        }

        /// <summary>
        /// Sets or updates the value for a specified key in a given section, handling array data.
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <param name="section">The section to modify or create.</param>
        /// <param name="key">The key to modify or create.</param>
        /// <param name="arrayValue">The array value to assign to the key.</param>
        public void SetArray<T>(string section, string key, T[] arrayValue)
        {
            string jsonArray = JsonConvert.SerializeObject(arrayValue);

            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }

            _sections[section][key] = jsonArray;
        }

        /// <summary>
        /// Sets or updates the value for a specified key in a given section, handling two-dimensional array data.
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <param name="section">The section to modify or create.</param>
        /// <param name="key">The key to modify or create.</param>
        /// <param name="arrayValue">The two-dimensional array value to assign to the key.</param>
        public void SetArray<T>(string section, string key, T[,] arrayValue)
        {
            string jsonArray = JsonConvert.SerializeObject(arrayValue);

            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }

            _sections[section][key] = jsonArray;
        }

        /// <summary>
        /// Sets or updates the value for a specified key in a given section, handling jagged array data.
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array (e.g., int, float, double, string).</typeparam>
        /// <param name="section">The section to modify or create.</param>
        /// <param name="key">The key to modify or create.</param>
        /// <param name="arrayValue">The jagged array value to assign to the key.</param>
        public void SetArray<T>(string section, string key, T[][] arrayValue)
        {
            string jsonArray = JsonConvert.SerializeObject(arrayValue);

            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }

            _sections[section][key] = jsonArray;
        }
        #endregion

        #region Deleting Data

        /// <summary>
        /// Deletes a specified section from the JSON file.
        /// </summary>
        /// <param name="section">The section to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the section does not exist.</exception>
        public void DeleteSection(string section)
        {
            if (_sections.ContainsKey(section))
            {
                _sections.Remove(section);
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }

        /// <summary>
        /// Deletes a specified key from a given section in the JSON file.
        /// </summary>
        /// <param name="section">The section containing the key to delete.</param>
        /// <param name="key">The key to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the section or key does not exist.</exception>
        public void DeleteKey(string section, string key)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.ContainsKey(key))
                {
                    sectionData.Remove(key);
                }
                else
                {
                    throw new ArgumentException($"Key '{key}' does not exist in section '{section}'.");
                }
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }
        #endregion

        #region Saving Data

        /// <summary>
        /// Saves the current state of the JSON file to disk.
        /// Overwrites the existing file with the current data in memory.
        /// </summary>
        public void SaveFile()
        {
            string jsonContent = JsonConvert.SerializeObject(_sections, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, jsonContent);
        }
        #endregion
    }
}
