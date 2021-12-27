using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using System.Reflection;
using UnityEngine;

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
            Translation translation = Translation.instance;
            UIHelperBase groupGeneral = helper.AddGroup(translation.Get(Translation.Key.General));

            // construct list of supported language names, first entry is game language
            string[] supportedLanguageCodes = translation.SupportedLanguageCodes;
            string[] languageNames = new string[supportedLanguageCodes.Length + 1];
            languageNames[0] = translation.Get(Translation.Key.GameLanguage);
            for (int i = 0; i < supportedLanguageCodes.Length; i++)
            {
                // get each language name in its own language (i.e. ignore configured language)
                languageNames[i + 1] = translation.Get(Translation.Key.LanguageName, supportedLanguageCodes[i]);
            }

            // compute index of configured language
            int defaultIndex = 0;
            Configuration config = ConfigurationUtil<Configuration>.Load();
            string configuredLanguageCode = config.LanguageCode;
            if (configuredLanguageCode != GameLanguageCode)
            {
                for (int i = 0; i < supportedLanguageCodes.Length; i++)
                {
                    if (configuredLanguageCode == supportedLanguageCodes[i])
                    {
                        defaultIndex = i + 1;
                        break;
                    }
                }
            }

            // allow user to change language
            groupGeneral.AddDropdown(translation.Get(Translation.Key.ChooseYourLanguage), languageNames, defaultIndex, OnLanguageChanged);

            // create in-game options only when a game is loaded
            if (MCSLoading.IsGameLoaded)
            {
                // in-game options group
                UIHelperBase groupInGame = helper.AddGroup(translation.Get(Translation.Key.InGame));

                // allow user to export all or selected statistics
                groupInGame.AddButton(translation.Get(Translation.Key.ExportAllStatistics), OnExportAllStatisticsClicked);
                groupInGame.AddButton(translation.Get(Translation.Key.ExportSelectedStatistics), OnExportSelectedStatisticsClicked);

                // show export location to user
                UITextField exportLocation = groupInGame.AddTextfield(translation.Get(Translation.Key.ExportFile), Snapshots.instance.ExportPathFile, (string text) => { } ) as UITextField;
                exportLocation.readOnly = true;
                exportLocation.autoSize = false;
                exportLocation.size = new Vector2(exportLocation.parent.parent.size.x - 30f, 2f * exportLocation.size.y);
                exportLocation.multiline = true;

                // allow user to delete snapshots
                groupInGame.AddSpace(50);
                groupInGame.AddButton(translation.Get(Translation.Key.DeleteSnapshots), OnDeleteSnapshotsClicked);

                // allow user to NOT save settings and snapshots, default is to save
                SaveSettingsAndSnapshots = true;
                groupInGame.AddSpace(30);
                groupInGame.AddCheckbox(translation.Get(Translation.Key.SaveSettingsAndSnapshots), SaveSettingsAndSnapshots, OnSaveCheckChanged);
            }
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
                languageCode = Translation.instance.SupportedLanguageCodes[index - 1];
            }

            // save the selected language code so it is available next time it is needed
            Configuration.SaveLanguageCode(languageCode);
            LogUtil.LogInfo($"Languaged changed to [{languageCode}]");

            // inform the Main Options Panel about locale change
            // this will trigger MoreCityStatistics.OnSettingsUI which calls Options.CreateUI to recreate this mod's Options UI with the newly selected language
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
        /// handle click on export all statistics
        /// </summary>
        private void OnExportAllStatisticsClicked()
        {
            // export all statistics
            Snapshots.instance.Export(Snapshots.StatisticsToExport.All);
        }

        /// <summary>
        /// handle click on export selected statistics
        /// </summary>
        private void OnExportSelectedStatisticsClicked()
        {
            // export selected statistics
            Snapshots.instance.Export(Snapshots.StatisticsToExport.Selected);
        }

        /// <summary>
        /// handle click on Delete Snapshots
        /// </summary>
        private void OnDeleteSnapshotsClicked()
        {
            // delete snapshots and update main panel
            Snapshots.instance.Clear();
            UserInterface.instance.UpdateMainPanel();
        }

        /// <summary>
        /// handle check change for Save
        /// </summary>
        private void OnSaveCheckChanged(bool isChecked)
        {
            // save check box status that will be looked at when saving the game
            SaveSettingsAndSnapshots = isChecked;
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
    }
}
