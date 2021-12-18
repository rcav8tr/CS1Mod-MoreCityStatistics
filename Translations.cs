namespace MoreCityStatistics
{
    /// <summary>
    /// translations used by this mod
    /// </summary>
    public class Translations
    {
        // use singleton pattern:  there can be only one set of translations in the game
        private static readonly Translations _instance = new Translations();
        public static Translations instance { get { return _instance; } }
        private Translations() { }


        // the translations
        public Translation CategoryDescription;
        public Translation Miscellaneous;
        public Translation StatisticDescription;
        public Translation StatisticUnits;


        /// <summary>
        /// initialize translations
        /// </summary>
        public void Initialize()
        {
            // with singleton pattern, all fields must be initialized or they will contain data from the previous game
            // the constructor string parameter must match the translation file name in the Translations folder
            CategoryDescription  = new Translation("CategoryDescription");
            Miscellaneous        = new Translation("Miscellaneous");
            StatisticDescription = new Translation("StatisticDescription");
            StatisticUnits       = new Translation("StatisticUnits");
        }

        /// <summary>
        /// deinitialize translations
        /// </summary>
        public void Deinitialize()
        {
            // clear the translations to (hopefully) reclaim memory
            if (CategoryDescription  != null) { CategoryDescription .Clear(); }
            if (Miscellaneous        != null) { Miscellaneous       .Clear(); }
            if (StatisticDescription != null) { StatisticDescription.Clear(); }
            if (StatisticUnits       != null) { StatisticUnits      .Clear(); }
        }
    }
}
