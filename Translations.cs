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
        private Translations()
        {
            // initialize the translations once when first referenced and then never again
            // the constructor string parameter for each translation must match the translation file name in the Translations folder
            CategoryDescription  = new Translation("CategoryDescription");
            Miscellaneous        = new Translation("Miscellaneous");
            StatisticDescription = new Translation("StatisticDescription");
            StatisticUnits       = new Translation("StatisticUnits");
        }

        // the translations
        public readonly Translation CategoryDescription;
        public readonly Translation Miscellaneous;
        public readonly Translation StatisticDescription;
        public readonly Translation StatisticUnits;
    }
}
