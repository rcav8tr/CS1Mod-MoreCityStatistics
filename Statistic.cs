using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MoreCityStatistics
{

    /// <summary>
    /// properties and UI elements for one statistic
    /// </summary>
    public class Statistic
    {
        // constants
        public const float UIHeight = 15f;

        // statistic type
        // all the enums are prefixed with the category to ensure they are differentiated (e.g. Electricity Consumption vs Water Consumption)
        // the enum member names must exactly match the Snapshot field/property names
        // the enums are in the same order that the categories and statistics are shown on the UI
        public enum StatisticType
        {
            ElectricityConsumptionPercent, ElectricityConsumption, ElectricityProduction,
            WaterConsumptionPercent, WaterConsumption, WaterPumpingCapacity,
            WaterTankReservedPercent, WaterTankReserved, WaterTankStorageCapacity,
            SewageProductionPercent, SewageProduction, SewageDrainingCapacity,
            LandfillStoragePercent, LandfillStorage, LandfillCapacity,
            GarbageProductionPercent, GarbageProduction, GarbageProcessingCapacity,
            EducationElementaryEligiblePercent, EducationElementaryEligible, EducationElementaryCapacity,
            EducationHighSchoolEligiblePercent, EducationHighSchoolEligible, EducationHighSchoolCapacity,
            EducationUniversityEligiblePercent, EducationUniversityEligible, EducationUniversityCapacity,
            EducationLibraryUsersPercent, EducationLibraryUsers, EducationLibraryCapacity,
            EducationLevelUneducatedPercent, EducationLevelEducatedPercent, EducationLevelWellEducatedPercent, EducationLevelHighlyEducatedPercent,
            EducationLevelUneducated, EducationLevelEducated, EducationLevelWellEducated, EducationLevelHighlyEducated,
            HappinessGlobal, HappinessResidential, HappinessCommercial, HappinessIndustrial, HappinessOffice,
            HealthcareAverageHealth, HealthcareSickPercent, HealthcareSick, HealthcareHealCapacity,
            DeathcareCemeteryBuriedPercent, DeathcareCemeteryBuried, DeathcareCemeteryCapacity,
            DeathcareCrematoriumDeceasedPercent, DeathcareCrematoriumDeceased, DeathcareCrematoriumCapacity,
            DeathcareDeathRate,
            ChildcareAverageHealth, ChildcareSickPercent, ChildcareSick, ChildcarePopulation, ChildcareBirthRate,
            EldercareAverageHealth, EldercareSickPercent, EldercareSick, EldercarePopulation, EldercareAverageLifeSpan,
            ZoningResidentialPercent, ZoningCommercialPercent, ZoningIndustrialPercent, ZoningOfficePercent, ZoningUnzonedPercent,
            ZoningTotal, ZoningResidential, ZoningCommercial, ZoningIndustrial, ZoningOffice, ZoningUnzoned,
            ZoneLevelResidentialAverage, ZoneLevelResidential1, ZoneLevelResidential2, ZoneLevelResidential3, ZoneLevelResidential4, ZoneLevelResidential5,
            ZoneLevelCommercialAverage, ZoneLevelCommercial1, ZoneLevelCommercial2, ZoneLevelCommercial3,
            ZoneLevelIndustrialAverage, ZoneLevelIndustrial1, ZoneLevelIndustrial2, ZoneLevelIndustrial3,
            ZoneLevelOfficeAverage, ZoneLevelOffice1, ZoneLevelOffice2, ZoneLevelOffice3,
            ZoneBuildingsResidentialPercent, ZoneBuildingsCommercialPercent, ZoneBuildingsIndustrialPercent, ZoneBuildingsOfficePercent,
            ZoneBuildingsTotal, ZoneBuildingsResidential, ZoneBuildingsCommercial, ZoneBuildingsIndustrial, ZoneBuildingsOffice,
            ZoneDemandResidential, ZoneDemandCommercial, ZoneDemandIndustrialOffice,
            TrafficAverageFlow,
            PollutionAverageGround, PollutionAverageDrinkingWater, PollutionAverageNoise,
            FireSafetyHazard,
            CrimeRate, CrimeDetainedCriminalsPercent, CrimeDetainedCriminals, CrimeJailsCapacity,
            PublicTransportationTotalTotal, PublicTransportationTotalResidents, PublicTransportationTotalTourists,
            PublicTransportationBusTotal, PublicTransportationBusResidents, PublicTransportationBusTourists,
            PublicTransportationTrolleybusTotal, PublicTransportationTrolleybusResidents, PublicTransportationTrolleybusTourists,
            PublicTransportationTramTotal, PublicTransportationTramResidents, PublicTransportationTramTourists,
            PublicTransportationMetroTotal, PublicTransportationMetroResidents, PublicTransportationMetroTourists,
            PublicTransportationTrainTotal, PublicTransportationTrainResidents, PublicTransportationTrainTourists,
            PublicTransportationShipTotal, PublicTransportationShipResidents, PublicTransportationShipTourists,
            PublicTransportationAirTotal, PublicTransportationAirResidents, PublicTransportationAirTourists,
            PublicTransportationMonorailTotal, PublicTransportationMonorailResidents, PublicTransportationMonorailTourists,
            PublicTransportationCableCarTotal, PublicTransportationCableCarResidents, PublicTransportationCableCarTourists,
            PublicTransportationTaxiTotal, PublicTransportationTaxiResidents, PublicTransportationTaxiTourists,
            PopulationTotal,
            PopulationChildrenPercent, PopulationTeensPercent, PopulationYoungAdultsPercent, PopulationAdultsPercent, PopulationSeniorsPercent,
            PopulationChildren, PopulationTeens, PopulationYoungAdults, PopulationAdults, PopulationSeniors,
            HouseholdsOccupiedPercent, HouseholdsOccupied, HouseholdsAvailable,
            EmploymentPeopleEmployed, EmploymentJobsAvailable, EmploymentUnfilledJobs,
            EmploymentUnemploymentPercent, EmploymentUnemployed, EmploymentEligibleWorkers,
            OutsideConnectionsImportTotal, OutsideConnectionsImportGoods, OutsideConnectionsImportForestry, OutsideConnectionsImportFarming, OutsideConnectionsImportOre, OutsideConnectionsImportOil, OutsideConnectionsImportMail,
            OutsideConnectionsExportTotal, OutsideConnectionsExportGoods, OutsideConnectionsExportForestry, OutsideConnectionsExportFarming, OutsideConnectionsExportOre, OutsideConnectionsExportOil, OutsideConnectionsExportMail, OutsideConnectionsExportFish,
            LandValueAverage,
            NaturalResourcesForestUsedPercent, NaturalResourcesForestUsed, NaturalResourcesForestAvailable,
            NaturalResourcesFertileLandUsedPercent, NaturalResourcesFertileLandUsed, NaturalResourcesFertileLandAvailable,
            NaturalResourcesOreUsedPercent, NaturalResourcesOreUsed, NaturalResourcesOreAvailable,
            NaturalResourcesOilUsedPercent, NaturalResourcesOilUsed, NaturalResourcesOilAvailable,
            HeatingConsumptionPercent, HeatingConsumption, HeatingProduction,
            TourismCityAttractiveness,
            TourismLowWealthPercent, TourismMediumWealthPercent, TourismHighWealthPercent,
            TourismTotal, TourismLowWealth, TourismMediumWealth, TourismHighWealth,
            TourismExchangeStudentBonus,
            ToursTotalTotal, ToursTotalResidents, ToursTotalTourists,
            ToursWalkingTourTotal, ToursWalkingTourResidents, ToursWalkingTourTourists,
            ToursSightseeingTotal, ToursSightseeingResidents, ToursSightseeingTourists,
            ToursBalloonTotal, ToursBalloonResidents, ToursBalloonToursits,
            TaxRateResidentialLow, TaxRateResidentialHigh, TaxRateCommercialLow, TaxRateCommercialHigh, TaxRateIndustrial, TaxRateOffice,
            CityEconomyTotalIncome, CityEconomyTotalExpenses, CityEconomyTotalProfit, CityEconomyBankBalance,
            ResidentialIncomeTotalPercent, ResidentialIncomeTotal,
            ResidentialIncomeLowDensityTotal, ResidentialIncomeLowDensity1, ResidentialIncomeLowDensity2, ResidentialIncomeLowDensity3, ResidentialIncomeLowDensity4, ResidentialIncomeLowDensity5, ResidentialIncomeLowDensitySelfSufficient,
            ResidentialIncomeHighDensityTotal, ResidentialIncomeHighDensity1, ResidentialIncomeHighDensity2, ResidentialIncomeHighDensity3, ResidentialIncomeHighDensity4, ResidentialIncomeHighDensity5, ResidentialIncomeHighDensitySelfSufficient,
            CommercialIncomeTotalPercent, CommercialIncomeTotal,
            CommercialIncomeLowDensityTotal, CommercialIncomeLowDensity1, CommercialIncomeLowDensity2, CommercialIncomeLowDensity3,
            CommercialIncomeHighDensityTotal, CommercialIncomeHighDensity1, CommercialIncomeHighDensity2, CommercialIncomeHighDensity3,
            CommercialIncomeSpecializedTotal, CommercialIncomeLeisure, CommercialIncomeTourism, CommercialIncomeOrganic,
            IndustrialIncomeTotalPercent, IndustrialIncomeTotal,
            IndustrialIncomeGenericTotal, IndustrialIncomeGeneric1, IndustrialIncomeGeneric2, IndustrialIncomeGeneric3,
            IndustrialIncomeSpecializedTotal, IndustrialIncomeForestry, IndustrialIncomeFarming, IndustrialIncomeOre, IndustrialIncomeOil,
            OfficeIncomeTotalPercent, OfficeIncomeTotal,
            OfficeIncomeGenericTotal, OfficeIncomeGeneric1, OfficeIncomeGeneric2, OfficeIncomeGeneric3,
            OfficeIncomeITCluster,
            TourismIncomeTotalPercent, TourismIncomeTotal,
            TourismIncomeCommercialZones, TourismIncomeTransportation, TourismIncomeParkAreas,
            ServiceExpensesTotalPercent, ServiceExpensesTotal,
            ServiceExpensesRoads, ServiceExpensesElectricity, ServiceExpensesWaterSewageHeating, ServiceExpensesGarbage,
            ServiceExpensesHealthcare, ServiceExpensesFire, ServiceExpensesEmergency, ServiceExpensesPolice, ServiceExpensesEducation,
            ServiceExpensesParksPlazas, ServiceExpensesUniqueBuildings, ServiceExpensesGenericSportsArenas, ServiceExpensesLoans, ServiceExpensesPolicies,
            ParkAreasTotalIncomePercent, ParkAreasTotalIncome, ParkAreasTotalExpensesPercent, ParkAreasTotalExpenses, ParkAreasTotalProfit,
            ParkAreasCityParkIncome, ParkAreasCityParkExpenses, ParkAreasCityParkProfit,
            ParkAreasAmusementParkIncome, ParkAreasAmusementParkExpenses, ParkAreasAmusementParkProfit,
            ParkAreasZooIncome, ParkAreasZooExpenses, ParkAreasZooProfit,
            ParkAreasNatureReserveIncome, ParkAreasNatureReserveExpenses, ParkAreasNatureReserveProfit,
            IndustryAreasTotalIncomePercent, IndustryAreasTotalIncome, IndustryAreasTotalExpensesPercent, IndustryAreasTotalExpenses, IndustryAreasTotalProfit,
            IndustryAreasForestryIncome, IndustryAreasForestryExpenses, IndustryAreasForestryProfit,
            IndustryAreasFarmingIncome, IndustryAreasFarmingExpenses, IndustryAreasFarmingProfit,
            IndustryAreasOreIncome, IndustryAreasOreExpenses, IndustryAreasOreProfit,
            IndustryAreasOilIncome, IndustryAreasOilExpenses, IndustryAreasOilProfit,
            IndustryAreasWarehousesFactoriesIncome, IndustryAreasWarehousesFactoriesExpenses, IndustryAreasWarehousesFactoriesProfit,
            IndustryAreasFishingIndustryIncome, IndustryAreasFishingIndustryExpenses, IndustryAreasFishingIndustryProfit,
            CampusAreasTotalIncomePercent, CampusAreasTotalIncome, CampusAreasTotalExpensesPercent, CampusAreasTotalExpenses, CampusAreasTotalProfit,
            CampusAreasTradeSchoolIncome, CampusAreasTradeSchoolExpenses, CampusAreasTradeSchoolProfit,
            CampusAreasLiberalArtsCollegeIncome, CampusAreasLiberalArtsCollegeExpenses, CampusAreasLiberalArtsCollegeProfit,
            CampusAreasUniversityIncome, CampusAreasUniversityExpenses, CampusAreasUniversityProfit,
            TransportEconomyTotalIncomePercent, TransportEconomyTotalIncome, TransportEconomyTotalExpensesPercent, TransportEconomyTotalExpenses, TransportEconomyTotalProfit,
            TransportEconomyBusIncome, TransportEconomyBusExpenses, TransportEconomyBusProfit,
            TransportEconomyTrolleybusIncome, TransportEconomyTrolleybusExpenses, TransportEconomyTrolleybusProfit,
            TransportEconomyTramIncome, TransportEconomyTramExpenses, TransportEconomyTramProfit,
            TransportEconomyMetroIncome, TransportEconomyMetroExpenses, TransportEconomyMetroProfit,
            TransportEconomyTrainIncome, TransportEconomyTrainExpenses, TransportEconomyTrainProfit,
            TransportEconomyShipIncome, TransportEconomyShipExpenses, TransportEconomyShipProfit,
            TransportEconomyAirIncome, TransportEconomyAirExpenses, TransportEconomyAirProfit,
            TransportEconomyMonorailIncome, TransportEconomyMonorailExpenses, TransportEconomyMonorailProfit,
            TransportEconomyCableCarIncome, TransportEconomyCableCarExpenses, TransportEconomyCableCarProfit,
            TransportEconomyTaxiIncome, TransportEconomyTaxiExpenses, TransportEconomyTaxiProfit,
            TransportEconomyToursIncome, TransportEconomyToursExpenses, TransportEconomyToursProfit,
            TransportEconomyTollBoothIncome, TransportEconomyTollBoothExpenses, TransportEconomyTollBoothProfit,
            TransportEconomyMailExpenses, TransportEconomyMailProfit,
            TransportEconomySpaceElevatorExpenses, TransportEconomySpaceElevatorProfit,
            GameLimitsBuildingsUsedPercent, GameLimitsBuildingsUsed, GameLimitsBuildingsCapacity,
            GameLimitsCitizensUsedPercent, GameLimitsCitizensUsed, GameLimitsCitizensCapacity,
            GameLimitsCitizenUnitsUsedPercent, GameLimitsCitizenUnitsUsed, GameLimitsCitizenUnitsCapacity,
            GameLimitsCitizenInstancesUsedPercent, GameLimitsCitizenInstancesUsed, GameLimitsCitizenInstancesCapacity,
            GameLimitsDisastersUsedPercent, GameLimitsDisastersUsed, GameLimitsDisastersCapacity,
            GameLimitsDistrictsUsedPercent, GameLimitsDistrictsUsed, GameLimitsDistrictsCapacity,
            GameLimitsEventsUsedPercent, GameLimitsEventsUsed, GameLimitsEventsCapacity,
            GameLimitsGameAreasUsedPercent, GameLimitsGameAreasUsed, GameLimitsGameAreasCapacity,
            GameLimitsNetworkLanesUsedPercent, GameLimitsNetworkLanesUsed, GameLimitsNetworkLanesCapacity,
            GameLimitsNetworkNodesUsedPercent, GameLimitsNetworkNodesUsed, GameLimitsNetworkNodesCapacity,
            GameLimitsNetworkSegmentsUsedPercent, GameLimitsNetworkSegmentsUsed, GameLimitsNetworkSegmentsCapacity,
            GameLimitsParkAreasUsedPercent, GameLimitsParkAreasUsed, GameLimitsParkAreasCapacity,
            GameLimitsPathUnitsUsedPercent, GameLimitsPathUnitsUsed, GameLimitsPathUnitsCapacity,
            GameLimitsPropsUsedPercent, GameLimitsPropsUsed, GameLimitsPropsCapacity,
            GameLimitsRadioChannelsUsedPercent, GameLimitsRadioChannelsUsed, GameLimitsRadioChannelsCapacity,
            GameLimitsRadioContentsUsedPercent, GameLimitsRadioContentsUsed, GameLimitsRadioContentsCapacity,
            GameLimitsTransportLinesUsedPercent, GameLimitsTransportLinesUsed, GameLimitsTransportLinesCapacity,
            GameLimitsTreesUsedPercent, GameLimitsTreesUsed, GameLimitsTreesCapacity,
            GameLimitsVehiclesUsedPercent, GameLimitsVehiclesUsed, GameLimitsVehiclesCapacity,
            GameLimitsVehiclesParkedUsedPercent, GameLimitsVehiclesParkedUsed, GameLimitsVehiclesParkedCapacity,
            GameLimitsZoneBlocksUsedPercent, GameLimitsZoneBlocksUsed, GameLimitsZoneBlocksCapacity
        }

        // description translation keys
        // enum member names must exactly match the translation keys in the StatisticDescription translation file
        public enum DescriptionKey
        {
            None,
            Consumption,
            Production,
            PumpingCapacity,
            Reserved,
            StorageCapacity,
            DrainingCapacity,
            Storage,
            Capacity,
            ProcessingCapacity,
            Elementary,
            HighSchool,
            University,
            PublicLibrary,
            Eligible,
            Users,
            Uneducated,
            Educated,
            WellEducated,
            HighlyEducated,
            Global,
            Residential,
            Commercial,
            Industrial,
            Office,
            Unzoned,
            AverageHealth,
            Sick,
            HealCapacity,
            Cemetery,
            Buried,
            Crematorium,
            Deceased,
            DeathRate,
            SickChildrenTeens,
            ChildrenTeens,
            BirthRate,
            SickSeniors,
            AverageLifeSpan,
            Average,
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            IndustrialOffice,
            Flow,
            Ground,
            DrinkingWater,
            Noise,
            Hazard,
            Rate,
            DetainedCriminals,
            JailsCapacity,
            Total,
            Bus,
            Trolleybus,
            Tram,
            Metro,
            Train,
            Ship,
            Air,
            Monorail,
            CableCar,
            Taxi,
            Children,
            Teens,
            YoungAdults,
            Adults,
            Seniors,
            Occupied,
            Available,
            PeopleEmployed,
            JobsAvailable,
            UnfilledJobs,
            Unemployment,
            Unemployed,
            EligibleWorkers,
            Import,
            Export,
            Goods,
            Forestry,
            Farming,
            Ore,
            Oil,
            Mail,
            Fish,
            Forest,
            FertileLand,
            Used,
            CityAttractiveness,
            LowWealth,
            MediumWealth,
            HighWealth,
            ExchangeStudentBonus,
            WalkingTour,
            SightseeingBus,
            Balloon,
            Income,
            Expenses,
            Profit,
            BankBalance,
            LowDensity,
            HighDensity,
            SelfSufficient,
            SpecializedTotal,
            Leisure,
            Tourism,
            OrganicAndLocalProduce,
            Generic,
            ITCluster,
            CommercialZones,
            PublicTransportation,
            Roads,
            Electricity,
            WaterSewage,
            WaterSewageHeating,
            Garbage,
            Healthcare,
            Fire,
            Emergency,
            Police,
            Education,
            ParksPlazasLandscaping,
            UniqueBuildings,
            GenericSportsArenas,
            Loans,
            Policies,
            CityPark,
            AmusementPark,
            Zoo,
            NatureReserve,
            WarehousesAndFactories,
            FishingIndustry,
            TradeSchool,
            LiberalArtsCollege,
            Tours,
            TollBooth,
            SpaceElevator,
            Buildings,
            Citizens,
            CitizenUnits,
            CitizenInstances,
            Disasters,
            Districts,
            Events,
            GameAreas,
            NetworkSegments,
            NetworkNodes,
            NetworkLanes,
            ParkAreas,
            IndustryAreas,
            CampusAreas,
            ParkIndustryAreas,
            ParkCampusAreas,
            IndustryCampusAreas,
            ParkIndustryCampusAreas,
            PathUnits,
            Props,
            RadioChannels,
            RadioContents,
            TransportLines,
            Trees,
            Vehicles,
            VehiclesParked,
            ZoneBlocks
        }

        // units translation keys
        // enum member names must exactly match the translation keys in the StatisticUnits translation file
        public enum UnitsKey
        {
            PctOfProduction,
            MegaWatts,
            PctOfPumpingCapacity,
            CubicMetersPerWeek,
            PctOfStorageCapacity,
            CubicMeters,
            PctOfDrainingCapacity,
            PctOfCapacity,
            Units,
            PctOfProcessingCapacity,
            UnitsPerWeek,
            Students,
            Visitors,
            PctOfPopulation,
            Citizens,
            Percent,
            PctOfHealCapacity,
            CitizensPerWeek,
            PctOfChildrenTeens,
            PctOfSeniors,
            Years,
            Level1To5,
            Level1To3,
            PctOfResidential,
            PctOfCommercial,
            PctOfIndustrial,
            PctOfOffice,
            PctOfJailsCapacity,
            TotalPerWeek,
            ResidentsPerWeek,
            TouristsPerWeek,
            PctOfAvailable,
            Squares,
            Households,
            Jobs,
            HouseholdsPlusJobs,
            PctOfEligible,
            PctOfForestAvailable,
            PctOfFertileLandAvailable,
            PctPerWeekOfOreAvailable,
            PctPerWeekOfOilAvailable,
            PctOfTotal,
            Hectare,
            PctOfCityIncome,
            PctOfCityExpenses,
            MoneyPerSquareMeter,
            MoneyPerWeek,
            Money,
            Amount
        }

        // main properties set by the constructor
        private readonly Category _category;
        private readonly StatisticType _type;
        private readonly DescriptionKey _descriptionKey1;
        private readonly DescriptionKey _descriptionKey2;
        private readonly UnitsKey _unitsKey;
        private readonly Color32 _textColor;

        // properties needed for UI operations
        private bool _selected;
        private bool _enabled;
        private string _descriptionUnits;

        // UI elements that are referenced after they are created
        private UIPanel _panel;
        private UISprite _checkbox;
        private UILabel _label;

        // needed by the graph
        private string _categoryDescription;
        private string _categoryDescriptionUnits;
        private string _units;
        private readonly Color32 _lineColor;
        private readonly string _numberFormat;

        /// <summary>
        /// constructor to set main and other properties
        /// </summary>
        public Statistic(Category category, StatisticType type, DescriptionKey descriptionKey1, DescriptionKey descriptionKey2, UnitsKey unitsKey, Color32 textColor)
        {
            // save params
            _category = category;
            _type = type;
            _descriptionKey1 = descriptionKey1;
            _descriptionKey2 = descriptionKey2;
            _unitsKey = unitsKey;
            _textColor = textColor;

            // description key depends on DLC
            if (_descriptionKey1 == DescriptionKey.WaterSewage && SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC))
            {
                _descriptionKey1 = DescriptionKey.WaterSewageHeating;
            }
            if (_descriptionKey1 == DescriptionKey.ParkIndustryCampusAreas)
            {
                bool dlcParkLife   = SteamHelper.IsDLCOwned(SteamHelper.DLC.ParksDLC);               // 05/24/18
                bool dlcIndustries = SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC);            // 10/23/18
                bool dlcCampus     = SteamHelper.IsDLCOwned(SteamHelper.DLC.CampusDLC);              // 05/21/19
                if      ( dlcParkLife && !dlcIndustries && !dlcCampus) { _descriptionKey1 = DescriptionKey.ParkAreas;           }
                else if (!dlcParkLife &&  dlcIndustries && !dlcCampus) { _descriptionKey1 = DescriptionKey.IndustryAreas;       }
                else if (!dlcParkLife && !dlcIndustries &&  dlcCampus) { _descriptionKey1 = DescriptionKey.CampusAreas;         }
                else if ( dlcParkLife &&  dlcIndustries && !dlcCampus) { _descriptionKey1 = DescriptionKey.ParkIndustryAreas;   }
                else if ( dlcParkLife && !dlcIndustries &&  dlcCampus) { _descriptionKey1 = DescriptionKey.ParkCampusAreas;     }
                else if (!dlcParkLife &&  dlcIndustries &&  dlcCampus) { _descriptionKey1 = DescriptionKey.IndustryCampusAreas; }
            }

            // compute line color from text color
            // for unknown reasons, graph lines appear brighter than the text, so make the line color a bit darker than the text so they match more closely
            const float LineColorMultiplier = 0.75f;
            _lineColor = new Color32((byte)(_textColor.r * LineColorMultiplier), (byte)(_textColor.g * LineColorMultiplier), (byte)(_textColor.b * LineColorMultiplier), 255);

            // set number format
            if (_type.ToString().EndsWith("Percent") || _type == StatisticType.TourismExchangeStudentBonus)
            {
                _numberFormat = "N1";
            }
            else if (_type == StatisticType.ZoneLevelResidentialAverage ||
                     _type == StatisticType.ZoneLevelCommercialAverage  ||
                     _type == StatisticType.ZoneLevelIndustrialAverage  ||
                     _type == StatisticType.ZoneLevelOfficeAverage)
            {
                _numberFormat = "N2";
            }
            else
            {
                _numberFormat = "N0";
            }

            // initialize UI text
            UpdateUIText();

            // logging to get list of statistics
            // LogUtil.LogInfo("• " + _descriptionUnits);
        }

        // read-only accessors
        public StatisticType Type { get { return _type; } }
        public bool Enabled { get { return _enabled; } }
        public string CategoryDescription { get { return _categoryDescription; } }
        public string CategoryDescriptionUnits { get { return _categoryDescriptionUnits; } }
        public string Units { get { return _units; } }
        public Color32 LineColor { get { return _lineColor; } }
        public string NumberFormat { get { return _numberFormat; } }

        /// <summary>
        /// whether or not the statistic is selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;

                // set the checkbox sprite
                if (_checkbox != null)
                {
                    _checkbox.spriteName = (value ? "AchievementCheckedTrue" : "AchievementCheckedFalse");
                }
            }
        }

        /// <summary>
        /// create statistic UI in the category
        /// </summary>
        public bool CreateUI()
        {
            // build name prefix
            string namePrefix = _type.ToString();

            // create a panel to hold the UI components
            _panel = _category.AddStatisticPanel();
            if (_panel == null)
            {
                LogUtil.LogError($"Unable to create statistic panel for [{_type}].");
                return false;
            }
            _panel.name = namePrefix + "Panel";
            _panel.autoSize = false;
            _panel.size = new Vector2(_panel.parent.size.x, UIHeight);
            _panel.relativePosition = new Vector3(0f, 0f);    // category panel uses auto layout
            _panel.eventClicked += Statistic_eventClicked;

            // create the checkbox
            const float componentHeight = UIHeight - 2f;
            _checkbox = _panel.AddUIComponent<UISprite>();
            if (_checkbox == null)
            {
                LogUtil.LogError($"Unable to create statistic checkbox for [{_type}].");
                return false;
            }
            _checkbox.name = namePrefix + "Checkbox";
            _checkbox.autoSize = false;
            _checkbox.size = new Vector2(componentHeight, componentHeight);
            _checkbox.relativePosition = new Vector3(20f, 0f);

            // create the label
            _label = _panel.AddUIComponent<UILabel>();
            if (_label == null)
            {
                LogUtil.LogError($"Unable to create statistic label for [{_type}].");
                return false;
            }
            _label.name = namePrefix + "Label";
            _label.textScale = 0.625f;
            _label.relativePosition = new Vector3(_checkbox.relativePosition.x + _checkbox.size.x + 3f, 3f);
            _label.autoSize = false;
            _label.size = new Vector2(_panel.size.x - _label.relativePosition.x, componentHeight);

            // set initial selection status to the status previously read from the game save file
            Selected = Selected;

            // get DLC flags (used often)
            // there are no dependencies on Match Day (aka Football) and Concerts (aka MusicFestival) DLCs
            bool dlcAfterDark           = SteamHelper.IsDLCOwned(SteamHelper.DLC.AfterDarkDLC);           // 09/24/15
            bool dlcSnowfall            = SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC);            // 02/18/16
            bool dlcNaturalDisasters    = SteamHelper.IsDLCOwned(SteamHelper.DLC.NaturalDisastersDLC);    // 11/29/16
            bool dlcMassTransit         = SteamHelper.IsDLCOwned(SteamHelper.DLC.InMotionDLC);            // 05/18/17
            bool dlcGreenCities         = SteamHelper.IsDLCOwned(SteamHelper.DLC.GreenCitiesDLC);         // 10/19/17
            bool dlcParkLife            = SteamHelper.IsDLCOwned(SteamHelper.DLC.ParksDLC);               // 05/24/18
            bool dlcIndustries          = SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC);            // 10/23/18
            bool dlcCampus              = SteamHelper.IsDLCOwned(SteamHelper.DLC.CampusDLC);              // 05/21/19
            bool dlcSunsetHarbor        = SteamHelper.IsDLCOwned(SteamHelper.DLC.UrbanDLC);               // 03/26/20

            // disable statistic for inactive DLC
            // statistic is still present, just hidden so it cannot be selected
            _enabled = true;
            switch (_type)
            {
                case StatisticType.WaterTankReservedPercent:
                case StatisticType.WaterTankReserved:
                case StatisticType.WaterTankStorageCapacity:
                    DisableForInactiveDLC(dlcNaturalDisasters);
                    break;

                case StatisticType.PublicTransportationTrolleybusTotal:
                case StatisticType.PublicTransportationTrolleybusResidents:
                case StatisticType.PublicTransportationTrolleybusTourists:
                    DisableForInactiveDLC(dlcSunsetHarbor);
                    break;

                case StatisticType.PublicTransportationTramTotal:
                case StatisticType.PublicTransportationTramResidents:
                case StatisticType.PublicTransportationTramTourists:
                    DisableForInactiveDLC(dlcSnowfall);
                    break;

                case StatisticType.PublicTransportationMonorailTotal:
                case StatisticType.PublicTransportationMonorailResidents:
                case StatisticType.PublicTransportationMonorailTourists:
                    DisableForInactiveDLC(dlcMassTransit);
                    break;

                case StatisticType.PublicTransportationCableCarTotal:
                case StatisticType.PublicTransportationCableCarResidents:
                case StatisticType.PublicTransportationCableCarTourists:
                    DisableForInactiveDLC(dlcMassTransit);
                    break;

                case StatisticType.PublicTransportationTaxiTotal:
                case StatisticType.PublicTransportationTaxiResidents:
                case StatisticType.PublicTransportationTaxiTourists:
                    DisableForInactiveDLC(dlcAfterDark);
                    break;

                case StatisticType.OutsideConnectionsImportMail:
                case StatisticType.OutsideConnectionsExportMail:
                    DisableForInactiveDLC(dlcIndustries);
                    break;

                case StatisticType.OutsideConnectionsExportFish:
                    DisableForInactiveDLC(dlcSunsetHarbor);
                    break;

                case StatisticType.HeatingConsumptionPercent:
                case StatisticType.HeatingConsumption:
                case StatisticType.HeatingProduction:
                    DisableForInactiveDLC(dlcSnowfall);
                    break;

                case StatisticType.TourismExchangeStudentBonus:
                    DisableForInactiveDLC(dlcCampus);
                    break;

                case StatisticType.ToursTotalTotal:
                case StatisticType.ToursTotalResidents:
                case StatisticType.ToursTotalTourists:
                case StatisticType.ToursWalkingTourTotal:
                case StatisticType.ToursWalkingTourResidents:
                case StatisticType.ToursWalkingTourTourists:
                case StatisticType.ToursSightseeingTotal:
                case StatisticType.ToursSightseeingResidents:
                case StatisticType.ToursSightseeingTourists:
                case StatisticType.ToursBalloonTotal:
                case StatisticType.ToursBalloonResidents:
                case StatisticType.ToursBalloonToursits:
                    DisableForInactiveDLC(dlcParkLife);
                    break;

                case StatisticType.ResidentialIncomeLowDensitySelfSufficient:
                case StatisticType.ResidentialIncomeHighDensitySelfSufficient:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.CommercialIncomeSpecializedTotal:
                case StatisticType.CommercialIncomeLeisure:
                case StatisticType.CommercialIncomeTourism:
                    DisableForInactiveDLC(dlcAfterDark);
                    break;

                case StatisticType.CommercialIncomeOrganic:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.OfficeIncomeGenericTotal:
                case StatisticType.OfficeIncomeITCluster:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.TourismIncomeParkAreas:
                    DisableForInactiveDLC(dlcParkLife);
                    break;

                case StatisticType.ServiceExpensesEmergency:
                    DisableForInactiveDLC(dlcNaturalDisasters);
                    break;

                case StatisticType.ServiceExpensesGenericSportsArenas:
                    DisableForInactiveDLC(dlcCampus);
                    break;

                case StatisticType.ParkAreasTotalIncomePercent:
                case StatisticType.ParkAreasTotalIncome:
                case StatisticType.ParkAreasTotalExpensesPercent:
                case StatisticType.ParkAreasTotalExpenses:
                case StatisticType.ParkAreasTotalProfit:
                case StatisticType.ParkAreasCityParkIncome:
                case StatisticType.ParkAreasCityParkExpenses:
                case StatisticType.ParkAreasCityParkProfit:
                case StatisticType.ParkAreasAmusementParkIncome:
                case StatisticType.ParkAreasAmusementParkExpenses:
                case StatisticType.ParkAreasAmusementParkProfit:
                case StatisticType.ParkAreasZooIncome:
                case StatisticType.ParkAreasZooExpenses:
                case StatisticType.ParkAreasZooProfit:
                case StatisticType.ParkAreasNatureReserveIncome:
                case StatisticType.ParkAreasNatureReserveExpenses:
                case StatisticType.ParkAreasNatureReserveProfit:
                    DisableForInactiveDLC(dlcParkLife);
                    break;

                case StatisticType.IndustryAreasTotalIncomePercent:
                case StatisticType.IndustryAreasTotalIncome:
                case StatisticType.IndustryAreasTotalExpensesPercent:
                case StatisticType.IndustryAreasTotalExpenses:
                case StatisticType.IndustryAreasTotalProfit:
                case StatisticType.IndustryAreasForestryIncome:
                case StatisticType.IndustryAreasForestryExpenses:
                case StatisticType.IndustryAreasForestryProfit:
                case StatisticType.IndustryAreasFarmingIncome:
                case StatisticType.IndustryAreasFarmingExpenses:
                case StatisticType.IndustryAreasFarmingProfit:
                case StatisticType.IndustryAreasOreIncome:
                case StatisticType.IndustryAreasOreExpenses:
                case StatisticType.IndustryAreasOreProfit:
                case StatisticType.IndustryAreasOilIncome:
                case StatisticType.IndustryAreasOilExpenses:
                case StatisticType.IndustryAreasOilProfit:
                case StatisticType.IndustryAreasWarehousesFactoriesIncome:
                case StatisticType.IndustryAreasWarehousesFactoriesExpenses:
                case StatisticType.IndustryAreasWarehousesFactoriesProfit:
                    DisableForInactiveDLC(dlcIndustries);
                    break;

                case StatisticType.IndustryAreasFishingIndustryIncome:
                case StatisticType.IndustryAreasFishingIndustryExpenses:
                case StatisticType.IndustryAreasFishingIndustryProfit:
                    DisableForInactiveDLC(dlcSunsetHarbor);
                    break;

                case StatisticType.CampusAreasTotalIncomePercent:
                case StatisticType.CampusAreasTotalIncome:
                case StatisticType.CampusAreasTotalExpensesPercent:
                case StatisticType.CampusAreasTotalExpenses:
                case StatisticType.CampusAreasTotalProfit:
                case StatisticType.CampusAreasTradeSchoolIncome:
                case StatisticType.CampusAreasTradeSchoolExpenses:
                case StatisticType.CampusAreasTradeSchoolProfit:
                case StatisticType.CampusAreasLiberalArtsCollegeIncome:
                case StatisticType.CampusAreasLiberalArtsCollegeExpenses:
                case StatisticType.CampusAreasLiberalArtsCollegeProfit:
                case StatisticType.CampusAreasUniversityIncome:
                case StatisticType.CampusAreasUniversityExpenses:
                case StatisticType.CampusAreasUniversityProfit:
                    DisableForInactiveDLC(dlcCampus);
                    break;

                case StatisticType.TransportEconomyTrolleybusIncome:
                case StatisticType.TransportEconomyTrolleybusExpenses:
                case StatisticType.TransportEconomyTrolleybusProfit:
                    DisableForInactiveDLC(dlcSunsetHarbor);
                    break;

                case StatisticType.TransportEconomyTramIncome:
                case StatisticType.TransportEconomyTramExpenses:
                case StatisticType.TransportEconomyTramProfit:
                    DisableForInactiveDLC(dlcSnowfall);
                    break;

                case StatisticType.TransportEconomyMonorailIncome:
                case StatisticType.TransportEconomyMonorailExpenses:
                case StatisticType.TransportEconomyMonorailProfit:
                    DisableForInactiveDLC(dlcMassTransit);
                    break;

                case StatisticType.TransportEconomyCableCarIncome:
                case StatisticType.TransportEconomyCableCarExpenses:
                case StatisticType.TransportEconomyCableCarProfit:
                    DisableForInactiveDLC(dlcMassTransit);
                    break;

                case StatisticType.TransportEconomyTaxiIncome:
                case StatisticType.TransportEconomyTaxiExpenses:
                case StatisticType.TransportEconomyTaxiProfit:
                    DisableForInactiveDLC(dlcAfterDark);
                    break;

                case StatisticType.TransportEconomyToursIncome:
                case StatisticType.TransportEconomyToursExpenses:
                case StatisticType.TransportEconomyToursProfit:
                    DisableForInactiveDLC(dlcParkLife);
                    break;

                case StatisticType.TransportEconomyMailExpenses:
                case StatisticType.TransportEconomyMailProfit:
                    DisableForInactiveDLC(dlcIndustries);
                    break;

                case StatisticType.GameLimitsDisastersUsedPercent:
                case StatisticType.GameLimitsDisastersUsed:
                case StatisticType.GameLimitsDisastersCapacity:
                    DisableForInactiveDLC(dlcNaturalDisasters);
                    break;

                case StatisticType.GameLimitsParkAreasUsedPercent:
                case StatisticType.GameLimitsParkAreasUsed:
                case StatisticType.GameLimitsParkAreasCapacity:
                    DisableForInactiveDLC(dlcParkLife || dlcIndustries || dlcCampus);
                    break;
            }

            // success
            return true;
        }

        /// <summary>
        /// handle click on statistic panel
        /// </summary>
        private void Statistic_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // toggle only if currently selected (i.e. will be unselected) OR not currently selected and selecting it would not exceed max selected
            if (Selected || (!Selected && Categories.instance.CountSelected < MainPanel.MaximumSelectedStatistics))
            {
                // toggle selected status and update main panel
                Selected = !Selected;
                UserInterface.instance.UpdateMainPanel();
                eventParam.Use();
            }
        }

        /// <summary>
        /// disable, deselect, and hide the statistic if DLC is inactive
        /// </summary>
        private void DisableForInactiveDLC(bool dlc)
        {
            if (!dlc)
            {
                _enabled = false;
                Selected = false;
                _panel.isVisible = false;
            }
        }

        /// <summary>
        /// update UI text
        /// </summary>
        public void UpdateUIText()
        {
            // obtain the translated description, which may or may not have a second part
            string description = Translations.instance.StatisticDescription.Get(_descriptionKey1.ToString());
            if (_descriptionKey2 != DescriptionKey.None)
            {
                description += "-" + Translations.instance.StatisticDescription.Get(_descriptionKey2.ToString());
            }

            // obtain the translated units
            _units = Translations.instance.StatisticUnits.Get(_unitsKey.ToString());

            // combine description and units
            _descriptionUnits = description + " (" + _units + ")";

            // combine categtory and description
            _categoryDescription = _category.Description + "-" + description;

            // combine categtory, description, and units
            _categoryDescriptionUnits = _category.Description + "-" + _descriptionUnits;

            // update label
            if (_label != null)
            {
                _label.text = _descriptionUnits;
                _label.textColor = _textColor;
            }
        }

        /// <summary>
        /// verify statistic
        /// </summary>
        public static void Verify()
        {
            // verify statistic types
            StatisticType[] statisticTypes = (StatisticType[])Enum.GetValues(typeof(StatisticType));
            foreach (StatisticType statisticType in statisticTypes)
            {
                // verify statistic type is created exactly once in all categories
                int found = 0;
                foreach (Category category in Categories.instance)
                {
                    foreach (Statistic statistic in category.Statistics)
                    {
                        if (statisticType == statistic.Type)
                        {
                            found++;
                        }
                    }
                }
                if (found != 1)
                {
                    LogUtil.LogError($"Statistic type [{statisticType}] is created {found} times, but should be created exactly once.");
                }

                // verify statistic type has a field or property in the snapshot
                Snapshot.GetFieldProperty(statisticType, out FieldInfo field, out PropertyInfo property);
                if (field == null && property == null)
                {
                    LogUtil.LogError($"Statistic type [{statisticType}] is not defined as a field or property in the snapshot.");
                }
            }

            // verify every field and property in the snapshot (except SnapshotDate) has a statistic type
            List<string> fieldPropertyNames = new List<string>();
            foreach (FieldInfo field in typeof(Snapshot).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                fieldPropertyNames.Add(field.Name);
            }
            foreach (PropertyInfo property in typeof(Snapshot).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                fieldPropertyNames.Add(property.Name);
            }
            foreach (string fieldPropertyName in fieldPropertyNames)
            {
                if (fieldPropertyName != "SnapshotDate")
                {
                    bool found = false;
                    foreach (StatisticType statisticType in statisticTypes)
                    {
                        if (fieldPropertyName == statisticType.ToString())
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        LogUtil.LogError($"Snapshot field/property [{fieldPropertyName}] is not defined as a statistic type.");
                    }
                }
            }

            // verify every description key enum value (except None) has a description key in the translation file
            Translation translationStatisticDescription = Translations.instance.StatisticDescription;
            DescriptionKey[] descriptionKeyEnumValues = (DescriptionKey[])Enum.GetValues(typeof(DescriptionKey));
            foreach (DescriptionKey descriptionKeyEnumValue in descriptionKeyEnumValues)
            {
                if (descriptionKeyEnumValue != DescriptionKey.None)
                {
                    _ = translationStatisticDescription.Get(descriptionKeyEnumValue.ToString());
                }
            }

            // verify every description key in the translation file has a description key enum value
            foreach (string translationDescriptionKey in translationStatisticDescription.GetKeys())
            {
                bool found = false;
                foreach (DescriptionKey descriptionKeyEnumValue in descriptionKeyEnumValues)
                {
                    if (translationDescriptionKey == descriptionKeyEnumValue.ToString())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    LogUtil.LogError($"Statistic description translation key [{translationDescriptionKey}] in the translation file does not have a description key enum value.");
                }
            }

            // verify every units key enum value has a units key in the translation file
            Translation translationStatisticUnits = Translations.instance.StatisticUnits;
            UnitsKey[] unitsKeyEnumValues = (UnitsKey[])Enum.GetValues(typeof(UnitsKey));
            foreach (UnitsKey unitsKeyEnumValue in unitsKeyEnumValues)
            {
                _ = translationStatisticUnits.Get(unitsKeyEnumValue.ToString());
            }

            // verify every units key in the translation file has a units key enum value
            foreach (string translationUnitsKey in translationStatisticUnits.GetKeys())
            {
                bool found = false;
                foreach (UnitsKey unitsKeyEnumValue in unitsKeyEnumValues)
                {
                    if (translationUnitsKey == unitsKeyEnumValue.ToString())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    LogUtil.LogError($"Statistic units translation key [{translationUnitsKey}] in the translation file does not have a units key enum value.");
                }
            }
        }

        /// <summary>
        /// write the statistic to the game save file
        /// </summary>
        public void Serialize(BinaryWriter writer)
        {
            // write selection status
            writer.Write(Selected);
        }

        /// <summary>
        /// read the statistic from the game save file
        /// </summary>
        public void Deserialize(BinaryReader reader, int version)
        {
            // read selection status
            Selected = reader.ReadBoolean();
        }
    }
}
