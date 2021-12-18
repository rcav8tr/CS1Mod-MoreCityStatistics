using ColossalFramework.UI;
using ICities;

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
            OptionsWorkOnlyInGame,
            DeleteSnapshots,
            SaveSettingsAndSnapshots
        }

        // status of settings check box
        public bool SaveSettingsAndSnapshots { get; private set; }

        /// <summary>
        /// create options UI
        /// gets called when a game or editor is loaded
        /// </summary>
        public void CreateUI(UIHelperBase helper)
        {
            // no method was found by which the options UI can be created only for a game, but not for the main menu options and editors
            // so options are always created and each option handler checks if it is in a game and displays an error message if not in a game

            // display a message at the top
            Translation miscellaneous = Translations.instance.Miscellaneous;
            UIHelperBase group = helper.AddGroup(miscellaneous.Get(TranslationKey.OptionsWorkOnlyInGame.ToString()));

            // allow user to delete snapshots
            group.AddButton(miscellaneous.Get(TranslationKey.DeleteSnapshots.ToString()), OnDeleteSnapshotsClicked);

            // allow user to NOT save settings and snapshots, default is to save
            SaveSettingsAndSnapshots = true;
            group.AddCheckbox(miscellaneous.Get(TranslationKey.SaveSettingsAndSnapshots.ToString()), SaveSettingsAndSnapshots, OnSaveCheckChanged);
        }

        /// <summary>
        /// handle click on Delete Snapshots
        /// </summary>
        public void OnDeleteSnapshotsClicked()
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
                OptionsNotAvailable();
            }
        }

        /// <summary>
        /// handle check change for Save
        /// </summary>
        public void OnSaveCheckChanged(bool isChecked)
        {
            // available only while in a game
            if (MCSLoading.IsGameLoaded)
            {
                // save check box status that will be looked at when saving the game
                SaveSettingsAndSnapshots = isChecked;
            }
            else
            {
                OptionsNotAvailable();
            }
        }

        /// <summary>
        /// display a message for options not available
        /// </summary>
        public void OptionsNotAvailable()
        {
            Translation miscellaneous = Translations.instance.Miscellaneous;
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                miscellaneous.Get(TranslationKey.Title.ToString()),
                miscellaneous.Get(TranslationKey.OptionsWorkOnlyInGame.ToString()),
                true);
        }
    }
}
