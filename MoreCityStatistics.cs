using ICities;

namespace MoreCityStatistics
{
    public class MoreCityStatistics : IUserMod
    {
        // required name and description of this mod
        public string Name => "More City Statistics";
        public string Description => "Record and graph more city statistics";

        /// <summary>
        /// mod is enabled
        /// </summary>
        public void OnEnabled()
        {
            // initialize translations
            Translations.instance.Initialize();
        }

        /// <summary>
        /// mod is disabled
        /// </summary>
        public void OnDisabled()
        {
            // deinitialize translations
            Translations.instance.Deinitialize();
        }

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