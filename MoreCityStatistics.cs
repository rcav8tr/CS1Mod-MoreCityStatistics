using ColossalFramework.Globalization;
using ICities;

namespace MoreCityStatistics
{
    public class MoreCityStatistics : IUserMod
    {
        // translation keys
        // enum member names must exactly match the translation keys in the Miscellaneous translation file
        private enum TranslationKey
        {
            Title,
            Description
        }

        // required name and description of this mod, always in the game language (not the language selected in Options)
        public string Name { get { return Translations.instance.Miscellaneous.Get(TranslationKey.Title.ToString(), LocaleManager.instance.language); } }
        public string Description { get { return Translations.instance.Miscellaneous.Get(TranslationKey.Description.ToString(), LocaleManager.instance.language); } }

        /// <summary>
        /// user settings
        /// </summary>
        public void OnSettingsUI(UIHelperBase helper)
        {
            // create options UI
            Options.instance.CreateUI(helper);
        }
    }
}