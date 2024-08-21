using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ConfigManager
{
    /// <summary>
    /// The XmlFileHandler class provides methods to interact with an XML file, 
    /// including reading, writing, and managing sections and key-value pairs.
    /// </summary>
    public class XmlFileHandler
    {
        private readonly string _filePath;
        private XElement _rootElement;

        /// <summary>
        /// Initializes a new instance of the XmlFileHandler class with the specified file path.
        /// Loads the data from the XML file into memory.
        /// </summary>
        /// <param name="filePath">The path to the XML file.</param>
        public XmlFileHandler(string filePath)
        {
            _filePath = filePath;
            LoadFile();
        }

        #region Loading Data

        /// <summary>
        /// Loads the XML file into memory as an XElement.
        /// If the file does not exist, creates an empty XML file.
        /// </summary>
        private void LoadFile()
        {
            if (!File.Exists(_filePath))
            {
                // Create an empty XML file with a root element
                _rootElement = new XElement("Root");
                _rootElement.Save(_filePath);
            }
            else
            {
                _rootElement = XElement.Load(_filePath);
            }
        }

        #endregion

        #region Reading Data

        /// <summary>
        /// Retrieves the value for a specified key in a given section.
        /// If the section or key does not exist, the default value is returned.
        /// </summary>
        /// <param name="section">The section (element) to retrieve the value from.</param>
        /// <param name="key">The key (element) to retrieve the value for.</param>
        /// <param name="defaultValue">The default value to return if the key or section does not exist.</param>
        /// <returns>The value associated with the specified key, or the default value.</returns>
        public string GetString(string section, string key, string defaultValue)
        {
            var sectionElement = _rootElement.Element(section);
            if (sectionElement != null)
            {
                var keyElement = sectionElement.Element(key);
                if (keyElement != null)
                {
                    return keyElement.Value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Retrieves all section names (element names) from the XML file.
        /// </summary>
        /// <returns>A list of section names.</returns>
        public List<string> GetSections()
        {
            var sections = new List<string>();
            foreach (var element in _rootElement.Elements())
            {
                sections.Add(element.Name.LocalName);
            }
            return sections;
        }

        /// <summary>
        /// Retrieves all keys (element names) in a specified section (element).
        /// </summary>
        /// <param name="section">The section (element) to retrieve keys from.</param>
        /// <returns>A list of keys in the specified section.</returns>
        /// <exception cref="ArgumentException">Thrown when the section does not exist.</exception>
        public List<string> GetKeys(string section)
        {
            var sectionElement = _rootElement.Element(section);
            if (sectionElement != null)
            {
                var keys = new List<string>();
                foreach (var element in sectionElement.Elements())
                {
                    keys.Add(element.Name.LocalName);
                }
                return keys;
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }

        /// <summary>
        /// Checks if a specified section (element) exists in the XML file.
        /// </summary>
        /// <param name="section">The section (element) to check.</param>
        /// <returns>True if the section exists; otherwise, false.</returns>
        public bool SectionExists(string section)
        {
            return _rootElement.Element(section) != null;
        }

        /// <summary>
        /// Checks if a specified key (element) exists in a given section (element) of the XML file.
        /// </summary>
        /// <param name="section">The section (element) to check.</param>
        /// <param name="key">The key (element) to check for existence.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public bool KeyExists(string section, string key)
        {
            var sectionElement = _rootElement.Element(section);
            return sectionElement?.Element(key) != null;
        }

        /// <summary>
        /// Reads all key-value pairs in a specified section (element).
        /// </summary>
        /// <param name="section">The section (element) to read key-value pairs from.</param>
        /// <returns>A dictionary containing all key-value pairs in the specified section.</returns>
        /// <exception cref="ArgumentException">Thrown when the section does not exist.</exception>
        public Dictionary<string, string> GetAllKeyValuePairs(string section)
        {
            var sectionElement = _rootElement.Element(section);
            if (sectionElement != null)
            {
                var keyValuePairs = new Dictionary<string, string>();
                foreach (var element in sectionElement.Elements())
                {
                    keyValuePairs[element.Name.LocalName] = element.Value;
                }
                return keyValuePairs;
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }
        #endregion

        #region Writing Data

        /// <summary>
        /// Sets or updates the value for a specified key (element) in a given section (element).
        /// If the section or key does not exist, it is created.
        /// </summary>
        /// <param name="section">The section (element) to modify or create.</param>
        /// <param name="key">The key (element) to modify or create.</param>
        /// <param name="value">The value to assign to the key.</param>
        public void SetString(string section, string key, string value)
        {
            var sectionElement = _rootElement.Element(section) ?? new XElement(section);
            var keyElement = sectionElement.Element(key) ?? new XElement(key);

            keyElement.Value = value;

            if (sectionElement.Element(key) == null)
            {
                sectionElement.Add(keyElement);
            }

            if (_rootElement.Element(section) == null)
            {
                _rootElement.Add(sectionElement);
            }
        }

        /// <summary>
        /// Creates a new section (element) in the XML file.
        /// </summary>
        /// <param name="section">The name of the section (element) to create.</param>
        /// <exception cref="ArgumentException">Thrown when the section already exists.</exception>
        public void CreateSection(string section)
        {
            if (!SectionExists(section))
            {
                _rootElement.Add(new XElement(section));
            }
            else
            {
                throw new ArgumentException($"Section '{section}' already exists.");
            }
        }
        #endregion

        #region Deleting Data

        /// <summary>
        /// Deletes a specified section (element) from the XML file.
        /// </summary>
        /// <param name="section">The section (element) to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the section does not exist.</exception>
        public void DeleteSection(string section)
        {
            var sectionElement = _rootElement.Element(section);
            if (sectionElement != null)
            {
                sectionElement.Remove();
            }
            else
            {
                throw new ArgumentException($"Section '{section}' does not exist.");
            }
        }

        /// <summary>
        /// Deletes a specified key (element) from a given section (element) in the XML file.
        /// </summary>
        /// <param name="section">The section (element) containing the key (element) to delete.</param>
        /// <param name="key">The key (element) to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the section or key does not exist.</exception>
        public void DeleteKey(string section, string key)
        {
            var sectionElement = _rootElement.Element(section);
            if (sectionElement != null)
            {
                var keyElement = sectionElement.Element(key);
                if (keyElement != null)
                {
                    keyElement.Remove();
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
        /// Saves the current state of the XML file to disk.
        /// Overwrites the existing file with the current data in memory.
        /// </summary>
        public void SaveFile()
        {
            _rootElement.Save(_filePath);
        }
        #endregion
    }
}
