using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigManager
{
    /// <summary>
    /// The IniFileHandler class provides methods to read, write, delete, and manage 
    /// sections and key-value pairs in an INI file.
    /// </summary>
    public class IniFileHandler
    {
        private readonly string _filePath;
        private Dictionary<string, Dictionary<string, string>> _sections;

        /// <summary>
        /// Initializes a new instance of the IniFileHandler class.
        /// Loads the INI file and parses its content into memory.
        /// </summary>
        /// <param name="filePath">The path to the INI file.</param>
        /// <exception cref="FileNotFoundException">Thrown when the specified INI file is not found.</exception>
        public IniFileHandler(string filePath)
        {
            _filePath = filePath;
            _sections = new Dictionary<string, Dictionary<string, string>>();
            LoadFile();
        }

        #region Loading Data
        /// <summary>
        /// Loads and parses the INI file into a dictionary structure.
        /// Each section is stored as a key, with another dictionary of key-value pairs as the value.
        /// If the file does not exist, creates an empty INI file.
        /// </summary>
        private void LoadFile()
        {
            if (!File.Exists(_filePath))
            {
                // Create an empty INI file
                File.WriteAllText(_filePath, string.Empty);
                _sections = new Dictionary<string, Dictionary<string, string>>();
                return;
            }

            string currentSection = null;
            foreach (var line in File.ReadAllLines(_filePath))
            {
                var trimmedLine = line.Trim();

                // Skip empty lines and comments
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";"))
                {
                    continue;
                }

                // Identify section headers
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Trim('[', ']');
                    if (!_sections.ContainsKey(currentSection))
                    {
                        _sections[currentSection] = new Dictionary<string, string>();
                    }
                }
                else if (currentSection != null)
                {
                    // Parse key-value pairs
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Split(new[] { ';' }, 2)[0].Trim(); // Ignore comments
                        _sections[currentSection][key] = value;
                    }
                }
            }
        }

        #endregion

        #region Reading Data

        /// <summary>
        /// Retrieves the value associated with a given key in a specified section as a string.
        /// </summary>
        /// <param name="section">The section containing the key.</param>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>
        public string GetString(string section, string key, string defaultValue)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves the value associated with a given key in a specified section as a boolean.
        /// </summary>
        /// <param name="section">The section containing the key.</param>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <param name="defaultValue">The default value to return if the key is not found or cannot be parsed.</param>
        /// <returns>The boolean value associated with the specified key, or the default value if the key is not found or cannot be parsed.</returns>
        /// <exception cref="FormatException">Thrown when the value cannot be parsed as a boolean.</exception>
        public bool GetBool(string section, string key, bool defaultValue)
        {
            string value = GetString(section, key, defaultValue.ToString());
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            throw new FormatException($"The value '{value}' in section '{section}' for key '{key}' is not a valid boolean.");
        }

        /// <summary>
        /// Retrieves the value associated with a given key in a specified section as an integer.
        /// </summary>
        /// <param name="section">The section containing the key.</param>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <param name="defaultValue">The default value to return if the key is not found or cannot be parsed.</param>
        /// <returns>The integer value associated with the specified key, or the default value if the key is not found or cannot be parsed.</returns>
        /// <exception cref="FormatException">Thrown when the value cannot be parsed as an integer.</exception>
        public int GetInt(string section, string key, int defaultValue)
        {
            string value = GetString(section, key, defaultValue.ToString());
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            throw new FormatException($"The value '{value}' in section '{section}' for key '{key}' is not a valid integer.");
        }

        /// <summary>
        /// Retrieves the value associated with a given key in a specified section as a float.
        /// </summary>
        /// <param name="section">The section containing the key.</param>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <param name="defaultValue">The default value to return if the key is not found or cannot be parsed.</param>
        /// <returns>The float value associated with the specified key, or the default value if the key is not found or cannot be parsed.</returns>
        /// <exception cref="FormatException">Thrown when the value cannot be parsed as a float.</exception>
        public float GetFloat(string section, string key, float defaultValue)
        {
            string value = GetString(section, key, defaultValue.ToString());
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            throw new FormatException($"The value '{value}' in section '{section}' for key '{key}' is not a valid float.");
        }

        /// <summary>
        /// Retrieves the value associated with a given key in a specified section as a double.
        /// </summary>
        /// <param name="section">The section containing the key.</param>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <param name="defaultValue">The default value to return if the key is not found or cannot be parsed.</param>
        /// <returns>The double value associated with the specified key, or the default value if the key is not found or cannot be parsed.</returns>
        /// <exception cref="FormatException">Thrown when the value cannot be parsed as a double.</exception>
        public double GetDouble(string section, string key, double defaultValue)
        {
            string value = GetString(section, key, defaultValue.ToString());
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            throw new FormatException($"The value '{value}' in section '{section}' for key '{key}' is not a valid double.");
        }

        /// <summary>
        /// Retrieves a list of all sections in the INI file.
        /// </summary>
        /// <returns>A list of section names.</returns>
        public List<string> GetSections()
        {
            return _sections.Keys.ToList();
        }

        /// <summary>
        /// Retrieves a list of all keys in a specified section.
        /// </summary>
        /// <param name="section">The section whose keys are to be retrieved.</param>
        /// <returns>A list of keys in the specified section.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified section does not exist.</exception>
        public List<string> GetKeys(string section)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                return sectionData.Keys.ToList();
            }
            throw new ArgumentException($"Section '{section}' does not exist.");
        }

        /// <summary>
        /// Retrieves all key-value pairs in a specified section.
        /// </summary>
        /// <param name="section">The section whose key-value pairs are to be retrieved.</param>
        /// <returns>A dictionary of key-value pairs in the specified section.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified section does not exist.</exception>
        public Dictionary<string, string> GetAllKeyValues(string section)
        {
            if (_sections.TryGetValue(section, out var sectionData))
            {
                return new Dictionary<string, string>(sectionData);
            }
            throw new ArgumentException($"Section '{section}' does not exist.");
        }

        /// <summary>
        /// Checks if a specified section exists in the INI file.
        /// </summary>
        /// <param name="section">The section to check for existence.</param>
        /// <returns>True if the section exists; otherwise, false.</returns>
        public bool SectionExists(string section)
        {
            return _sections.ContainsKey(section);
        }

        /// <summary>
        /// Checks if a specified key exists in a given section of the INI file.
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
        /// Sets or updates the value for a specified key in a given section.
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <param name="section">The section to modify or create.</param>
        /// <param name="key">The key to modify or create.</param>
        /// <param name="value">The value to assign to the key.</param>
        public void SetString(string section, string key, string value)
        {
            if (!_sections.ContainsKey(section))
            {
                _sections[section] = new Dictionary<string, string>();
            }

            _sections[section][key] = value;
        }

        /// <summary>
        /// Creates a new section in the INI file.
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
        #endregion

        #region Deleting Data

        /// <summary>
        /// Deletes a specified section from the INI file.
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
        /// Deletes a specified key from a given section in the INI file.
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
        /// Saves the current state of the INI file to disk.
        /// Overwrites the existing file with the current data in memory.
        /// </summary>
        public void SaveFile()
        {
            var lines = new List<string>();

            foreach (var section in _sections)
            {
                lines.Add($"[{section.Key}]");
                foreach (var kvp in section.Value)
                {
                    lines.Add($"{kvp.Key} = {kvp.Value}");
                }
                lines.Add(""); // Add a blank line after each section
            }

            File.WriteAllLines(_filePath, lines);
        }
        #endregion
    }
}
