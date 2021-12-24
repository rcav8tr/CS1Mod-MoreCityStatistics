using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using System.Reflection;

namespace MoreCityStatistics
{
    /// <summary>
    /// handle Options
    /// </summary>
    public class Options
    {
        // use singleton pattern:  there can be only one Options in the game
        private static readonly Options _instance = new Options();
        public static Options instance { get { return _instance; } }
        private Options() { }

        // translation keys
        // enum member names must exactly match the translation keys in the Miscellaneous translation file
        private enum TranslationKey
        {
            Title,
            General,
            ChooseYourLanguage,
            GameLanguage,
            LanguageName,
            InGame,
            DeleteSnapshots,
            SaveSettingsAndSnapshots,
            OptionValidOnlyInGame
        }

        // special language code for game language
        public const string GameLanguageCode = "00";

        // status of settings check box
        public bool SaveSettingsAndSnapshots { get; private set; }

        /// <summary>
        /// create options UI
        /// </summary>
        public void CreateUI(UIHelperBase helper)
        {
            // create general options group
            Translation miscellaneous = Translations.instance.Miscellaneous;
            UIHelperBase groupGeneral = helper.AddGroup(miscellaneous.Get(TranslationKey.General.ToString()));

            // construct list of supported language names, first entry is game language
            string[] languageNames = new string[Translation.SupportedLanguageCodes.Length + 1];
            languageNames[0] = miscellaneous.Get(TranslationKey.GameLanguage.ToString());
            for (int i = 0; i < Translation.SupportedLanguageCodes.Length; i++)
            {
                // get each language name in its own language (i.e. ignore configured language)
                languageNames[i + 1] = miscellaneous.Get(TranslationKey.LanguageName.ToString(), Translation.SupportedLanguageCodes[i]);
            }

            // compute index of configured language
            int defaultIndex = 0;
            Configuration config = ConfigurationUtil<Configuration>.Load();
            string configuredLanguageCode = config.LanguageCode;
            if (configuredLanguageCode != GameLanguageCode)
            {
                for (int i = 0; i < Translation.SupportedLanguageCodes.Length; i++)
                {
                    if (configuredLanguageCode == Translation.SupportedLanguageCodes[i])
                    {
                        defaultIndex = i + 1;
                        break;
                    }
                }
            }

            // allow user to change language
            groupGeneral.AddDropdown(miscellaneous.Get(TranslationKey.ChooseYourLanguage.ToString()), languageNames, defaultIndex, OnLanguageChanged);


            // no method was found by which the UI for in-game options can be created only when in a game, but not when in editors or the main menu
            // so the in-game options are always created and each option handler checks if it is in a game and displays an error message if not in a game

            // in-game options group
            UIHelperBase groupInGame = helper.AddGroup(miscellaneous.Get(TranslationKey.InGame.ToString()));

            // allow user to delete snapshots
            groupInGame.AddButton(miscellaneous.Get(TranslationKey.DeleteSnapshots.ToString()), OnDeleteSnapshotsClicked);

            // allow user to NOT save settings and snapshots, default is to save
            SaveSettingsAndSnapshots = true;
            groupInGame.AddCheckbox(miscellaneous.Get(TranslationKey.SaveSettingsAndSnapshots.ToString()), SaveSettingsAndSnapshots, OnSaveCheckChanged);
        }

        /// <summary>
        /// handle user change in language
        /// </summary>
        private void OnLanguageChanged(int index)
        {
            // get the selected language code
            string languageCode;
            if (index == 0)
            {
                languageCode = GameLanguageCode;
            }
            else
            {
                languageCode = Translation.SupportedLanguageCodes[index - 1];
            }

            // save the selected language code so it is available next time it is needed
            Configuration.SaveLanguageCode(languageCode);
            LogUtil.LogInfo($"Languaged changed to [{languageCode}]");

            // inform the Main Options Panel about locale change
            // this will trigger MoreCityStatistics.OnSettingsUI which calls Options.CreateUI to recreate this mod's Options UI
            MethodInfo onLocaleChanged = typeof(OptionsMainPanel).GetMethod("OnLocaleChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            if (onLocaleChanged != null)
            {
                OptionsMainPanel optionsMainPanel = UIView.library.Get<OptionsMainPanel>("OptionsPanel");
                if (optionsMainPanel != null)
                {
                    onLocaleChanged.Invoke(optionsMainPanel, new object[] { });
                }
            }

            // inform the Content Manager Panel about locale change
            onLocaleChanged = typeof(ContentManagerPanel).GetMethod("OnLocaleChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            if (onLocaleChanged != null)
            {
                ContentManagerPanel contentManagerPanel = UIView.library.Get<ContentManagerPanel>("ContentManagerPanel");
                if (contentManagerPanel != null)
                {
                    onLocaleChanged.Invoke(contentManagerPanel, new object[] { });
                }
            }

            // update UI for the rest of this mod's UI
            UserInterface.instance.UpdateUI();
        }

        /// <summary>
        /// return the selected language code or the game's language code if game language is selected
        /// </summary>
        public string GetLanguageCode()
        {
            // check if should use game language
            Configuration config = ConfigurationUtil<Configuration>.Load();
            string configuredLanguageCode = config.LanguageCode;
            if (configuredLanguageCode == GameLanguageCode)
            {
                // use game language code
                return LocaleManager.instance.language;
            }
            else
            {
                // use configured language code
                return configuredLanguageCode;
            }
        }

        /// <summary>
        /// handle click on Delete Snapshots
        /// </summary>
        private void OnDeleteSnapshotsClicked()
        {
            // available only while in a game
            if (MCSLoading.IsGameLoaded)
            {
                // delete snapshots and update main panel
                Snapshots.instance.Clear();
                UserInterface.instance.UpdateMainPanel();
            }
            else
            {
                DisplayErrorMessage(TranslationKey.OptionValidOnlyInGame);
            }
        }

        /// <summary>
        /// handle check change for Save
        /// </summary>
        private void OnSaveCheckChanged(bool isChecked)
        {
            // available only while in a game
            if (MCSLoading.IsGameLoaded)
            {
                // save check box status that will be looked at when saving the game
                SaveSettingsAndSnapshots = isChecked;
            }
            else
            {
                DisplayErrorMessage(TranslationKey.OptionValidOnlyInGame);
            }
        }

        /// <summary>
        /// display an error message
        /// </summary>
        private void DisplayErrorMessage(TranslationKey translationKey)
        {
            Translation miscellaneous = Translations.instance.Miscellaneous;
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                miscellaneous.Get(TranslationKey.Title.ToString()),
                miscellaneous.Get(translationKey.ToString()),
                true);
        }
    }
}
