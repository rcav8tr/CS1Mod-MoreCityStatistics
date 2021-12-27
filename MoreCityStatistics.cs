using ColossalFramework.Globalization;
using ICities;

namespace MoreCityStatistics
{
    public class MoreCityStatistics : IUserMod
    {
        // required name and description of this mod, always in the game language (not the language selected in Options)
        public string Name { get { return Translation.instance.Get(Translation.Key.Title, LocaleManager.instance.language); } }
        public string Description { get { return Translation.instance.Get(Translation.Key.Description, LocaleManager.instance.language); } }

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