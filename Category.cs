using ColossalFramework.UI;
using System;
using System.IO;
using UnityEngine;

namespace MoreCityStatistics
{
    /// <summary>
    /// properties and UI elements for one category
    /// </summary>
    public class Category
    {
        // constants
        private const float UIHeight = 17f;

        // category type
        public enum CategoryType
        {
            Electricity,
            Water,
            WaterTank,
            Sewage,
            Landfill,
            Garbage,
            Education,
            EducationLevel,
            Happiness,
            Healthcare,
            Deathcare,
            Childcare,
            Eldercare,
            Zoning,
            ZoneLevel,
            ZoneBuildings,
            ZoneDemand,
            Traffic,
            Pollution,
            FireSafety,
            Crime,
            PublicTransportation,
            Population,
            Households,
            Employment,
            OutsideConnections,
            LandValue,
            NaturalResources,
            Heating,
            Tourism,
            Tours,
            TaxRate,
            CityEconomy,
            ResidentialIncome,
            CommercialIncome,
            IndustrialIncome,
            OfficeIncome,
            TourismIncome,
            ServiceExpenses,
            ParkAreas,
            IndustryAreas,
            CampusAreas,
            TransportEconomy,
            GameLimits
        }

        // description translation keys
        // enum member names must exactly match the translation keys in the CategoryDescription translation file
        // most description keys are the same as the CategoryType
        public enum DescriptionKey
        {
            Electricity,
            Water,
            WaterTank,
            Sewage,
            Landfill,
            Garbage,
            Education,
            EducationLevel,
            Happiness,
            Healthcare,
            Deathcare,
            Childcare,
            Eldercare,
            Zoning,
            ZoneLevel,
            ZoneBuildings,
            ZoneDemand,
            Traffic,
            Pollution,
            FireSafety,
            Crime,
            PublicTransportation,
            Population,
            Households,
            Employment,
            OutsideConnections,
            LandValue,
            NaturalResources,
            Heating,
            Tourism,
            Tours,
            TaxRate,
            CityEconomy,
            ResidentialIncome,
            CommercialIncome,
            IndustrialIncome,
            OfficeIncome,
            TourismIncome,
            ServiceExpenses,
            ParkAreas,
            IndustryAreas,
            Fishing,
            CampusAreas,
            TransportEconomy,
            GameLimits
        }


        // main properties set by the constructor
        private readonly CategoryType _type;
        private readonly DescriptionKey _descriptionKey;
        private readonly Statistics _statistics;

        // properties needed by the UI
        private string _description;
        private bool _expanded;

        // UI components that are referenced after they are created
        private UIPanel _panel;
        private UISprite _expansionIcon;
        private UILabel _label;

        /// <summary>
        /// constructor to set main properties
        /// </summary>
        public Category(CategoryType type, DescriptionKey descriptionKey)
        {
            // initialize
            _type = type;
            _descriptionKey = descriptionKey;
            _statistics = new Statistics();

            // description key depends on DLC
            if (_descriptionKey == DescriptionKey.IndustryAreas && SteamHelper.IsDLCOwned(SteamHelper.DLC.UrbanDLC) && !SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
            {
                _descriptionKey = DescriptionKey.Fishing;
            }

            // initialize UI text
            UpdateUIText();

            // logging to get list of categories
            // LogUtil.LogInfo(_description);
        }

        // read-only accessors
        public CategoryType Type { get { return _type; } }
        public Statistics Statistics {  get { return _statistics; } }
        public string Description { get { return _description; } }

        /// <summary>
        /// whether or not the category is expanded
        /// </summary>
        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;

                // check if panel and expansion icon are set
                if (_panel != null && _expansionIcon != null)
                {
                    // set panel height and expansion icon
                    _panel.height = UIHeight + (value ? Statistic.UIHeight * _statistics.CountEnabled : 0);
                    _expansionIcon.spriteName = (value ? "IconDownArrow2Focused" : "ArrowRightFocused");
                }
            }
        }

        /// <summary>
        /// create category UI on the categories scrollable panel
        /// </summary>
        public bool CreateUI(UIScrollablePanel categoriesScrollablePanel)
        {
            // build name prefix
            string namePrefix = Type.ToString();

            // create panel to hold the UI components
            _panel = categoriesScrollablePanel.AddUIComponent<UIPanel>();
            if (_panel == null)
            {
                LogUtil.LogError($"Unable to create category panel for [{Type}].");
                return false;
            }
            _panel.name = namePrefix + "Panel";
            _panel.autoSize = false;
            _panel.size = new Vector2(categoriesScrollablePanel.size.x - 4f, UIHeight);
            _panel.relativePosition = new Vector3(0f, 0f);  // scrollable panel uses auto layout
            _panel.clipChildren = true;      // prevents contained statistics from being displayed when category is collapsed
            _panel.autoLayoutStart = LayoutStart.TopLeft;
            _panel.autoLayoutDirection = LayoutDirection.Vertical;
            _panel.autoLayout = true;

            // create the expansion panel
            UIPanel expansionPanel = _panel.AddUIComponent<UIPanel>();
            if (expansionPanel == null)
            {
                LogUtil.LogError($"Unable to create category expansion panel for [{Type}].");
                return false;
            }
            expansionPanel.name = namePrefix + "ExpansionPanel";
            expansionPanel.autoSize = false;
            expansionPanel.size = new Vector2(_panel.size.x, UIHeight);
            expansionPanel.relativePosition = new Vector3(0f, 0f);
            expansionPanel.eventClicked += Category_eventClicked;

            // create the expansion icon
            const float componentHeight = UIHeight - 2f;
            _expansionIcon = expansionPanel.AddUIComponent<UISprite>();
            if (_expansionIcon == null)
            {
                LogUtil.LogError($"Unable to create category expansion icon for [{Type}].");
                return false;
            }
            _expansionIcon.name = namePrefix + "ExpansionIcon";
            _expansionIcon.autoSize = false;
            _expansionIcon.size = new Vector2(componentHeight, componentHeight);
            _expansionIcon.relativePosition = new Vector3(0f, 0f);

            // create the label
            _label = expansionPanel.AddUIComponent<UILabel>();
            if (_label == null)
            {
                LogUtil.LogError($"Unable to create category label for [{Type}].");
                return false;
            }
            _label.name = namePrefix + "Label";
            _label.relativePosition = new Vector3(_expansionIcon.relativePosition.x + _expansionIcon.size.x + 3f, 3.5f);
            _label.autoSize = false;
            _label.size = new Vector2(_panel.size.x - _label.relativePosition.x, componentHeight);
            _label.textScale = 0.75f;
            _label.textColor = new Color32(185, 221, 254, 255);;

            // create statistics UI
            if (!_statistics.CreateUI())
            {
                return false;
            }

            // set initial expansion status to the status previously read from the game save file
            Expanded = Expanded;

            // if there are no enabled statistics, then hide and collapse the category
            if (_statistics.CountEnabled == 0)
            {
                _panel.isVisible = false;
                Expanded = false;
            }

            // success
            return true;
        }

        /// <summary>
        /// handle click on category panel
        /// </summary>
        private void Category_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // ignore if event was already used by statistic
            if (eventParam.used)
            {
                return;
            }

            // toggle expansion status
            Expanded = !Expanded;
            eventParam.Use();
        }

        /// <summary>
        /// add a statistic panel to the category panel
        /// </summary>
        public UIPanel AddStatisticPanel()
        {
            return _panel.AddUIComponent<UIPanel>();
        }

        /// <summary>
        /// update UI text
        /// </summary>
        public void UpdateUIText()
        {
            // obtain the translated description
            _description = Translations.instance.CategoryDescription.Get(_descriptionKey.ToString());

            // update the label
            if (_label != null)
            {
                _label.text = _description;
            }

            // update the statistics
            _statistics.UpdateUIText();
        }

        /// <summary>
        /// verify category
        /// </summary>
        public static void Verify()
        {
            // verify category types
            foreach (CategoryType categoryType in Enum.GetValues(typeof(CategoryType)))
            {
                // verify category type is created exactly once
                int found = 0;
                foreach (Category category in Categories.instance)
                {
                    if (categoryType == category.Type)
                    {
                        found++;
                    }
                }
                if (found != 1)
                {
                    LogUtil.LogError($"Category type [{categoryType}] is created {found} times, but should be created exactly once.");
                }
            }

            // verify every description key enum value has a description key in the translation file
            Translation translationCategoryDescription = Translations.instance.CategoryDescription;
            DescriptionKey[] descriptionKeyValues = (DescriptionKey[])Enum.GetValues(typeof(DescriptionKey));
            foreach (DescriptionKey descriptionKeyValue in descriptionKeyValues)
            {
                _ = translationCategoryDescription.Get(descriptionKeyValue.ToString());
            }

            // verify every description key in the translation file has a description key enum value
            foreach (string descriptionKey in translationCategoryDescription.GetKeys())
            {
                bool found = false;
                foreach (DescriptionKey descriptionKeyValue in descriptionKeyValues)
                {
                    if (descriptionKey == descriptionKeyValue.ToString())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    LogUtil.LogError($"Category description translation key [{descriptionKey}] in the translation file does not have a description key enum value.");
                }
            }
        }

        /// <summary>
        /// write the category to the game save file
        /// </summary>
        public void Serialize(BinaryWriter writer)
        {
            // write category expansion status
            writer.Write(Expanded);

            // write statistics
            _statistics.Serialize(writer);
        }

        /// <summary>
        /// read the category from the game save file
        /// </summary>
        public void Deserialize(BinaryReader reader, int version)
        {
            // read category expansion status
            Expanded = reader.ReadBoolean();

            // read statistics
            _statistics.Deserialize(reader, version);
        }
    }
}
