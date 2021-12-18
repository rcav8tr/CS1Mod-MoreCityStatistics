using ColossalFramework.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MoreCityStatistics
{
    /// <summary>
    /// get translated text
    /// logic loosely adapted from Traffic Manager: President Edition (TM:PE) by Krzychu1245
    /// </summary>
    public class Translation
    {
        // translations are in CSV files in the Translations folder of the project
        // translation files are embedded resources in the mod DLL
        // translation files were created using LibreOffice Calc

        // translation file format:
        //      line 1: blank,language code 1,language code 2,...,language code n
        //      line 2: translation key 1,translated text 1,translated text 2,...,translated text n
        //      line 3: translation key 2,translated text 1,translated text 2,...,translated text n
        //      ...
        //      line m: translation key m-1,translated text 1,translated text 2,...,translated text n

        // translation file rules:
        //      the file should contain a translation for every language code supported by the game
        //      the file must contain a translation for the default language code
        //      a blank translation will use the translation for the default language
        //      each language code, translation key, and translated text may or may not be enclosed in double quotes
        //      a blank line is skipped
        //      a line without a translation key is skipped
        //      a translation key cannot be duplicated
        //      spaces around the comma separators will be included in the value
        //      to include a comma in the value, the value must be enclosed in double quotes
        //      to include a double quote in the value, use two double quotes inside the double quoted value


        // default language code
        private const string DefaultLanguageCode = "en";    // English

        // file from which translations were read
        private readonly string _fileName;

        // translations for a single language
        // the dictionary key is the translation key
        // the dictionary value is the translated text for the key
        private class TranslationLaguage : Dictionary<string, string> { }

        // translations for all languages in the file
        // the dictionary key is the language code
        // the dictionary value contains the translations for the language
        private Dictionary<string, TranslationLaguage> _languages = new Dictionary<string, TranslationLaguage>();

        /// <summary>
        /// construct a translation from the specified filename
        /// </summary>
        public Translation(string filename)
        {
            // save the filename
            _fileName = filename;

            // make sure the translation CSV file exists
            // assumes the namespace of this class is the same as the name space of the project
            string translationFile = $"{typeof(Translation).Namespace}.Translations.{filename}.csv";
            if (!Assembly.GetExecutingAssembly().GetManifestResourceNames().Contains(translationFile))
            {
                LogUtil.LogError($"Translation file [{translationFile}] does not exist.");
                return;
            }

            // read the lines from the translations CSV file
            string[] lines;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(translationFile))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    lines = reader.ReadToEnd().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                }
            }

            // translation file must contain at least one line for the language codes
            if (lines.Length < 1)
            {
                LogUtil.LogError($"Translation file [{translationFile}] must contain at least one line.");
                return;
            }

            // read the language codes from the first line
            List<string> languageCodes = new List<string>();
            using (StringReader reader = new StringReader(lines[0]))
            {
                // read and ignore the first value, which should be blank
                ReadCSVValue(reader);

                // read language codes
                string languageCode = ReadCSVValue(reader);
                while (languageCode.Length != 0)
                {
                    // add the language code to the list
                    languageCodes.Add(languageCode);

                    // initialize empty language
                    _languages[languageCode] = new TranslationLaguage();

                    // get next language code
                    languageCode = ReadCSVValue(reader);
                }
            }

            // translations must contain default language code
            if (!_languages.ContainsKey(DefaultLanguageCode))
            {
                LogUtil.LogError($"Translation file [{translationFile}] must contain translations for default language code [{DefaultLanguageCode}].");
                return;
            }

            // read each subsequent line
            for (int i = 1; i < lines.Length; i++)
            {
                // do only non-blank lines
                string line = lines[i];
                if (line.Length > 0)
                {
                    // create a string reader on the line
                    using (StringReader reader = new StringReader(line))
                    {
                        // the first value in the line is the translation key
                        // if translation key is blank, skip the line
                        string translationKey = ReadCSVValue(reader);
                        if (translationKey.Length != 0)
                        {
                            // check for duplicates
                            if (_languages[DefaultLanguageCode].ContainsKey(translationKey))
                            {
                                LogUtil.LogError($"Translation key [{translationKey}] is duplicated in translation file [{translationFile}].");
                                return;
                            }
                            else
                            {
                                // read the translated text for each language code
                                foreach (string languageCode in languageCodes)
                                {
                                    _languages[languageCode][translationKey] = ReadCSVValue(reader);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// read a CSV value
        /// </summary>
        private string ReadCSVValue(StringReader reader)
        {
            // the value to return
            StringBuilder value = new StringBuilder();

            // read until non-quoted comma or end-of-string is reached
            bool inQuotes = false;
            int currentChar = reader.Read();
            while (currentChar != -1)
            {
                // check for double quote char
                if (currentChar == '\"')
                {
                    // check whether or not already in double quotes
                    if (!inQuotes)
                    {
                        // not already in double quotes
                        // this double quote is the start of a quoted string, don't append the double quote
                        inQuotes = true;
                    }
                    else
                    {
                        // already in double quotes, check next char
                        if (reader.Peek() == '\"')
                        {
                            // next char is double quote
                            // consume the second double quote and replace the two consecutive double quotes with one double qoute
                            reader.Read();
                            value.Append((char)currentChar);
                        }
                        else
                        {
                            // next char is not double quote
                            // this double quote is the end of a quoted string, don't append the double quote
                            inQuotes = false;
                        }
                    }
                }
                else
                {
                    // a comma not in double quotes ends the value, don't append the comma
                    if (currentChar == ',' && !inQuotes)
                    {
                        break;
                    }

                    // all other cases, append the char
                    value.Append((char)currentChar);
                }

                // get next char
                currentChar = reader.Read();
            }

            // return the value
            return value.ToString();
        }

        /// <summary>
        /// get the translation of the key using the current language
        /// </summary>
        public string Get(string translationKey)
        {
            // get translations for the current language
            string languageCode = LocaleManager.instance.language;
            if (!_languages.TryGetValue(languageCode, out TranslationLaguage translationLanguage))
            {
                LogUtil.LogError($"Unknown language code [{languageCode}] when getting translation for key [{translationKey}] in file [{_fileName}].");
                return translationKey;
            }

            // get translated text for the translation key
            if (!translationLanguage.TryGetValue(translationKey, out string translatedText))
            {
                LogUtil.LogError($"Translation key [{translationKey}] not found for language [{languageCode}] in file [{_fileName}].");
                return translationKey;
            }

            // check for blank translation
            if (string.IsNullOrEmpty(translatedText))
            {
                // get translation from default language
                translatedText = _languages[DefaultLanguageCode][translationKey];

                // if still blank, then use key
                if (string.IsNullOrEmpty(translatedText))
                {
                    LogUtil.LogError($"Translation is blank for default language [{DefaultLanguageCode}] and key [{translationKey}] in file [{_fileName}].");
                    return translationKey;
                }
            }

            // return the translated text
            return translatedText;
        }

        /// <summary>
        ///  get the translation keys
        /// </summary>
        public string[] GetKeys()
        {
            return _languages[DefaultLanguageCode].Keys.ToArray();
        }

        /// <summary>
        /// clear the translations
        /// </summary>
        public void Clear()
        {
            foreach (string languageCode in _languages.Keys)
            {
                _languages[languageCode].Clear();
            }
            _languages.Clear();
        }
    }
}
