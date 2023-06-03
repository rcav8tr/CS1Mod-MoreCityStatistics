using ColossalFramework.Globalization;
using ColossalFramework.UI;
using System;
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
        // the enums are in the same order that the categories and statistics are shown on the UI,
        // but the order here is not important because the value of the enum is not saved
        public enum StatisticType
        {
            ElectricityConsumptionPercent,
            ElectricityConsumption,
            ElectricityProduction,

            WaterConsumptionPercent,
            WaterConsumption,
            WaterPumpingCapacity,

            WaterTankReservedPercent,
            WaterTankReserved,
            WaterTankStorageCapacity,

            SewageProductionPercent,
            SewageProduction,
            SewageDrainingCapacity,

            LandfillStoragePercent,
            LandfillStorage,
            LandfillCapacity,

            GarbageProductionPercent,
            GarbageProduction,
            GarbageProcessingCapacity,

            EducationElementaryEligiblePercent,
            EducationElementaryEligible,
            EducationElementaryCapacity,
            EducationHighSchoolEligiblePercent,
            EducationHighSchoolEligible,
            EducationHighSchoolCapacity,
            EducationUniversityEligiblePercent,
            EducationUniversityEligible,
            EducationUniversityCapacity,
            EducationLibraryUsersPercent,
            EducationLibraryUsers,
            EducationLibraryCapacity,
            EducationLevelUneducatedPercent,
            EducationLevelEducatedPercent,
            EducationLevelWellEducatedPercent,
            EducationLevelHighlyEducatedPercent,
            EducationLevelUneducated,
            EducationLevelEducated,
            EducationLevelWellEducated,
            EducationLevelHighlyEducated,

            HappinessGlobal,
            HappinessResidential,
            HappinessCommercial,
            HappinessIndustrial,
            HappinessOffice,

            HealthcareAverageHealth,
            HealthcareSickPercent,
            HealthcareSick,
            HealthcareHealCapacity,

            DeathcareCemeteryBuriedPercent,
            DeathcareCemeteryBuried,
            DeathcareCemeteryCapacity,
            DeathcareCrematoriumDeceasedPercent,
            DeathcareCrematoriumDeceased,
            DeathcareCrematoriumCapacity,
            DeathcareDeathRate,

            ChildcareAverageHealth,
            ChildcareSickPercent,
            ChildcareSick,
            ChildcarePopulation,
            ChildcareBirthRate,

            EldercareAverageHealth,
            EldercareSickPercent,
            EldercareSick,
            EldercarePopulation,
            EldercareAverageLifeSpan,

            ZoningResidentialPercent,
            ZoningCommercialPercent,
            ZoningIndustrialPercent,
            ZoningOfficePercent,
            ZoningUnzonedPercent,
            ZoningTotal,
            ZoningResidential,
            ZoningCommercial,
            ZoningIndustrial,
            ZoningOffice,
            ZoningUnzoned,

            ZoneLevelResidentialAverage,
            ZoneLevelResidential1,
            ZoneLevelResidential2,
            ZoneLevelResidential3,
            ZoneLevelResidential4,
            ZoneLevelResidential5,
            ZoneLevelCommercialAverage,
            ZoneLevelCommercial1,
            ZoneLevelCommercial2,
            ZoneLevelCommercial3,
            ZoneLevelIndustrialAverage,
            ZoneLevelIndustrial1,
            ZoneLevelIndustrial2,
            ZoneLevelIndustrial3,
            ZoneLevelOfficeAverage,
            ZoneLevelOffice1,
            ZoneLevelOffice2,
            ZoneLevelOffice3,

            ZoneBuildingsResidentialPercent,
            ZoneBuildingsCommercialPercent,
            ZoneBuildingsIndustrialPercent,
            ZoneBuildingsOfficePercent,
            ZoneBuildingsTotal,
            ZoneBuildingsResidential,
            ZoneBuildingsCommercial,
            ZoneBuildingsIndustrial,
            ZoneBuildingsOffice,

            ZoneDemandResidential,
            ZoneDemandCommercial,
            ZoneDemandIndustrialOffice,

            TrafficAverageFlow,
            TrafficPedestriansPercent,
            TrafficCyclistsPercent,
            TrafficPrivateVehiclesPercent,
            TrafficPublicTransportCargoPercent,
            TrafficTrucksPercent,
            TrafficCityServiceVehiclesPercent,
            TrafficDummyTrafficPercent,
            TrafficTotalCount,
            TrafficPedestriansCount,
            TrafficCyclistsCount,
            TrafficPrivateVehiclesCount,
            TrafficPublicTransportCargoCount,
            TrafficTrucksCount,
            TrafficCityServiceVehiclesCount,
            TrafficDummyTrafficCount,

            PollutionAverageGround,
            PollutionAverageDrinkingWater,
            PollutionAverageNoise,

            FireSafetyHazard,

            CrimeRate,
            CrimeDetainedCriminalsPercent,
            CrimeDetainedCriminals,
            CrimeJailsCapacity,

            CommercialCashAccumulatedPercent,
            CommercialCashAccumulated,
            CommercialCashCapacity,
            CommercialCashCollected,

            PublicTransportationTotalTotal,
            PublicTransportationTotalResidents,
            PublicTransportationTotalTourists,
            PublicTransportationBusTotal,
            PublicTransportationBusResidents,
            PublicTransportationBusTourists,
            PublicTransportationTrolleybusTotal,
            PublicTransportationTrolleybusResidents,
            PublicTransportationTrolleybusTourists,
            PublicTransportationTramTotal,
            PublicTransportationTramResidents,
            PublicTransportationTramTourists,
            PublicTransportationMetroTotal,
            PublicTransportationMetroResidents,
            PublicTransportationMetroTourists,
            PublicTransportationTrainTotal,
            PublicTransportationTrainResidents,
            PublicTransportationTrainTourists,
            PublicTransportationShipTotal,
            PublicTransportationShipResidents,
            PublicTransportationShipTourists,
            PublicTransportationAirTotal,
            PublicTransportationAirResidents,
            PublicTransportationAirTourists,
            PublicTransportationMonorailTotal,
            PublicTransportationMonorailResidents,
            PublicTransportationMonorailTourists,
            PublicTransportationCableCarTotal,
            PublicTransportationCableCarResidents,
            PublicTransportationCableCarTourists,
            PublicTransportationTaxiTotal,
            PublicTransportationTaxiResidents,
            PublicTransportationTaxiTourists,

            IntercityTravelArrivingTotal,
            IntercityTravelArrivingResidents,
            IntercityTravelArrivingTourists,
            IntercityTravelDepartingTotal,
            IntercityTravelDepartingResidents,
            IntercityTravelDepartingTourists,
            IntercityTravelDummyTrafficTotal,
            IntercityTravelDummyTrafficResidents,
            IntercityTravelDummyTrafficTourists,

            PopulationTotal,
            PopulationChildrenPercent,
            PopulationTeensPercent,
            PopulationYoungAdultsPercent,
            PopulationAdultsPercent,
            PopulationSeniorsPercent,
            PopulationChildren,
            PopulationTeens,
            PopulationYoungAdults,
            PopulationAdults,
            PopulationSeniors,

            HouseholdsOccupiedPercent,
            HouseholdsOccupied,
            HouseholdsAvailable,

            EmploymentPeopleEmployed,
            EmploymentJobsAvailable,
            EmploymentUnfilledJobs,
            EmploymentUnemploymentPercent,
            EmploymentUnemployed,
            EmploymentEligibleWorkers,

            OutsideConnectionsImportTotal,
            OutsideConnectionsImportGoods,
            OutsideConnectionsImportForestry,
            OutsideConnectionsImportFarming,
            OutsideConnectionsImportOre,
            OutsideConnectionsImportOil,
            OutsideConnectionsImportMail,
            OutsideConnectionsExportTotal,
            OutsideConnectionsExportGoods,
            OutsideConnectionsExportForestry,
            OutsideConnectionsExportFarming,
            OutsideConnectionsExportOre,
            OutsideConnectionsExportOil,
            OutsideConnectionsExportMail,
            OutsideConnectionsExportFish,

            LandValueAverage,

            NaturalResourcesForestUsedPercent,
            NaturalResourcesForestUsed,
            NaturalResourcesForestAvailable,
            NaturalResourcesFertileLandUsedPercent,
            NaturalResourcesFertileLandUsed,
            NaturalResourcesFertileLandAvailable,
            NaturalResourcesOreUsedPercent,
            NaturalResourcesOreUsed,
            NaturalResourcesOreAvailable,
            NaturalResourcesOilUsedPercent,
            NaturalResourcesOilUsed,
            NaturalResourcesOilAvailable,

            HeatingConsumptionPercent,
            HeatingConsumption,
            HeatingProduction,

            TourismCityAttractiveness,
            TourismLowWealthPercent,
            TourismMediumWealthPercent,
            TourismHighWealthPercent,
            TourismTotal,
            TourismLowWealth,
            TourismMediumWealth,
            TourismHighWealth,
            TourismExchangeStudentBonus,

            ToursTotalTotal,
            ToursTotalResidents,
            ToursTotalTourists,
            ToursWalkingTourTotal,
            ToursWalkingTourResidents,
            ToursWalkingTourTourists,
            ToursSightseeingTotal,
            ToursSightseeingResidents,
            ToursSightseeingTourists,
            ToursBalloonTotal,
            ToursBalloonResidents,
            ToursBalloonToursits,

            TaxRateResidentialLow,
            TaxRateResidentialHigh,
            TaxRateCommercialLow,
            TaxRateCommercialHigh,
            TaxRateIndustrial,
            TaxRateOffice,

            InvestmentsChirpAir,
            InvestmentsChirperCrypto,
            InvestmentsDeathcareServiceFund,
            InvestmentsFarmingIndustry,
            InvestmentsForestryIndustry,
            InvestmentsGreasyGasoline,
            InvestmentsGenericIndustry,
            InvestmentsHealthcareServiceFund,
            InvestmentsOilIndustry,
            InvestmentsOreIndustry,
            InvestmentsChirpyCruises,
            InvestmentsTrafficJellyLogistics,
            InvestmentsVeryLegitCompany,
            InvestmentsGainsLastMonth,
            InvestmentsTotalGains,

            CityEconomyTotalIncome,
            CityEconomyTotalExpenses,
            CityEconomyTotalProfit,
            CityEconomyBankBalance,
            CityEconomyLoanBalance,
            CityEconomyCityValue,
            CityEconomyCityValuePerCapita,
            CityEconomyGrossDomesticProduct,
            CityEconomyGrossDomesticProductPerCapita,
            CityEconomyConsumption,
            CityEconomyConsumptionPercent,
            CityEconomyGovernmentSpending,
            CityEconomyGovernmentSpendingPercent,
            CityEconomyExports,
            CityEconomyImports,
            CityEconomyNetExports,
            CityEconomyNetExportsPercent,

            ResidentialIncomeTotalPercent,
            ResidentialIncomeTotal,
            ResidentialIncomeLowDensityTotal,
            ResidentialIncomeLowDensity1,
            ResidentialIncomeLowDensity2,
            ResidentialIncomeLowDensity3,
            ResidentialIncomeLowDensity4,
            ResidentialIncomeLowDensity5,
            ResidentialIncomeLowDensitySelfSufficient,
            ResidentialIncomeHighDensityTotal,
            ResidentialIncomeHighDensity1,
            ResidentialIncomeHighDensity2,
            ResidentialIncomeHighDensity3,
            ResidentialIncomeHighDensity4,
            ResidentialIncomeHighDensity5,
            ResidentialIncomeHighDensitySelfSufficient,
            ResidentialIncomeWallToWall,

            CommercialIncomeTotalPercent,
            CommercialIncomeTotal,
            CommercialIncomeLowDensityTotal,
            CommercialIncomeLowDensity1,
            CommercialIncomeLowDensity2,
            CommercialIncomeLowDensity3,
            CommercialIncomeHighDensityTotal,
            CommercialIncomeHighDensity1,
            CommercialIncomeHighDensity2,
            CommercialIncomeHighDensity3,
            CommercialIncomeSpecializedTotal,
            CommercialIncomeLeisure,
            CommercialIncomeTourism,
            CommercialIncomeOrganic,
            CommercialIncomeWallToWall,

            IndustrialIncomeTotalPercent,
            IndustrialIncomeTotal,
            IndustrialIncomeGenericTotal,
            IndustrialIncomeGeneric1,
            IndustrialIncomeGeneric2,
            IndustrialIncomeGeneric3,
            IndustrialIncomeSpecializedTotal,
            IndustrialIncomeForestry,
            IndustrialIncomeFarming,
            IndustrialIncomeOre,
            IndustrialIncomeOil,

            OfficeIncomeTotalPercent,
            OfficeIncomeTotal,
            OfficeIncomeGenericTotal,
            OfficeIncomeGeneric1,
            OfficeIncomeGeneric2,
            OfficeIncomeGeneric3,
            OfficeIncomeSpecializedTotal,
            OfficeIncomeITCluster,
            OfficeIncomeWallToWall,
            OfficeIncomeFinancial,

            TourismIncomeTotalPercent,
            TourismIncomeTotal,
            TourismIncomeCommercialZones,
            TourismIncomeTransportation,
            TourismIncomeParkAreas,
            TourismIncomeHotels,

            ServiceExpensesTotalPercent,
            ServiceExpensesTotal,
            ServiceExpensesRoads,
            ServiceExpensesElectricity,
            ServiceExpensesWaterSewageHeating,
            ServiceExpensesGarbage,
            ServiceExpensesHealthcare,
            ServiceExpensesFire,
            ServiceExpensesEmergency,
            ServiceExpensesPolice,
            ServiceExpensesBanks,
            ServiceExpensesEducation,
            ServiceExpensesParksPlazas,
            ServiceExpensesServicePoints,
            ServiceExpensesUniqueBuildings,
            ServiceExpensesGenericSportsArenas,
            ServiceExpensesLoans,
            ServiceExpensesPolicies,

            ParkAreasTotalIncomePercent,
            ParkAreasTotalIncome,
            ParkAreasTotalExpensesPercent,
            ParkAreasTotalExpenses,
            ParkAreasTotalProfit,
            ParkAreasCityParkIncome,
            ParkAreasCityParkExpenses,
            ParkAreasCityParkProfit,
            ParkAreasAmusementParkIncome,
            ParkAreasAmusementParkExpenses,
            ParkAreasAmusementParkProfit,
            ParkAreasZooIncome,
            ParkAreasZooExpenses,
            ParkAreasZooProfit,
            ParkAreasNatureReserveIncome,
            ParkAreasNatureReserveExpenses,
            ParkAreasNatureReserveProfit,

            IndustryAreasTotalIncomePercent,
            IndustryAreasTotalIncome,
            IndustryAreasTotalExpensesPercent,
            IndustryAreasTotalExpenses,
            IndustryAreasTotalProfit,
            IndustryAreasForestryIncome,
            IndustryAreasForestryExpenses,
            IndustryAreasForestryProfit,
            IndustryAreasFarmingIncome,
            IndustryAreasFarmingExpenses,
            IndustryAreasFarmingProfit,
            IndustryAreasOreIncome,
            IndustryAreasOreExpenses,
            IndustryAreasOreProfit,
            IndustryAreasOilIncome,
            IndustryAreasOilExpenses,
            IndustryAreasOilProfit,
            IndustryAreasWarehousesFactoriesIncome,
            IndustryAreasWarehousesFactoriesExpenses,
            IndustryAreasWarehousesFactoriesProfit,

            FishingIndustryFishingIncome,
            FishingIndustryFishingExpenses,
            FishingIndustryFishingProfit,

            CampusAreasTotalIncomePercent,
            CampusAreasTotalIncome,
            CampusAreasTotalExpensesPercent,
            CampusAreasTotalExpenses,
            CampusAreasTotalProfit,
            CampusAreasTradeSchoolIncome,
            CampusAreasTradeSchoolExpenses,
            CampusAreasTradeSchoolProfit,
            CampusAreasLiberalArtsCollegeIncome,
            CampusAreasLiberalArtsCollegeExpenses,
            CampusAreasLiberalArtsCollegeProfit,
            CampusAreasUniversityIncome,
            CampusAreasUniversityExpenses,
            CampusAreasUniversityProfit,

            HotelsTotalIncomePercent,
            HotelsTotalIncome,
            HotelsTotalExpensesPercent,
            HotelsTotalExpenses,
            HotelsTotalProfit,
            HotelsTotalPopularity,
            HotelsSightseeingPopularity,
            HotelsShoppingPopularity,
            HotelsBusinessPopularity,
            HotelsNaturePopularity,
            HotelsGuestsVisitingPercent,
            HotelsGuestsVisiting,
            HotelsGuestsCapacity,

            TransportEconomyTotalIncomePercent,
            TransportEconomyTotalIncome,
            TransportEconomyTotalExpensesPercent,
            TransportEconomyTotalExpenses,
            TransportEconomyTotalProfit,
            TransportEconomyBusIncome,
            TransportEconomyBusExpenses,
            TransportEconomyBusProfit,
            TransportEconomyTrolleybusIncome,
            TransportEconomyTrolleybusExpenses,
            TransportEconomyTrolleybusProfit,
            TransportEconomyTramIncome,
            TransportEconomyTramExpenses,
            TransportEconomyTramProfit,
            TransportEconomyMetroIncome,
            TransportEconomyMetroExpenses,
            TransportEconomyMetroProfit,
            TransportEconomyTrainIncome,
            TransportEconomyTrainExpenses,
            TransportEconomyTrainProfit,
            TransportEconomyShipIncome,
            TransportEconomyShipExpenses,
            TransportEconomyShipProfit,
            TransportEconomyAirIncome,
            TransportEconomyAirExpenses,
            TransportEconomyAirProfit,
            TransportEconomyMonorailIncome,
            TransportEconomyMonorailExpenses,
            TransportEconomyMonorailProfit,
            TransportEconomyCableCarIncome,
            TransportEconomyCableCarExpenses,
            TransportEconomyCableCarProfit,
            TransportEconomyTaxiIncome,
            TransportEconomyTaxiExpenses,
            TransportEconomyTaxiProfit,
            TransportEconomyToursIncome,
            TransportEconomyToursExpenses,
            TransportEconomyToursProfit,
            TransportEconomyTollBoothIncome,
            TransportEconomyTollBoothExpenses,
            TransportEconomyTollBoothProfit,
            TransportEconomyMailExpenses,
            TransportEconomyMailProfit,
            TransportEconomySpaceElevatorExpenses,
            TransportEconomySpaceElevatorProfit,

            GameLimitsBuildingsUsedPercent,
            GameLimitsBuildingsUsed,
            GameLimitsBuildingsCapacity,
            GameLimitsCitizensUsedPercent,
            GameLimitsCitizensUsed,
            GameLimitsCitizensCapacity,
            GameLimitsCitizenUnitsUsedPercent,
            GameLimitsCitizenUnitsUsed,
            GameLimitsCitizenUnitsCapacity,
            GameLimitsCitizenInstancesUsedPercent,
            GameLimitsCitizenInstancesUsed,
            GameLimitsCitizenInstancesCapacity,
            GameLimitsDisastersUsedPercent,
            GameLimitsDisastersUsed,
            GameLimitsDisastersCapacity,
            GameLimitsDistrictsUsedPercent,
            GameLimitsDistrictsUsed,
            GameLimitsDistrictsCapacity,
            GameLimitsEventsUsedPercent,
            GameLimitsEventsUsed,
            GameLimitsEventsCapacity,
            GameLimitsGameAreasUsedPercent,
            GameLimitsGameAreasUsed,
            GameLimitsGameAreasCapacity,
            GameLimitsNetworkLanesUsedPercent,
            GameLimitsNetworkLanesUsed,
            GameLimitsNetworkLanesCapacity,
            GameLimitsNetworkNodesUsedPercent,
            GameLimitsNetworkNodesUsed,
            GameLimitsNetworkNodesCapacity,
            GameLimitsNetworkSegmentsUsedPercent,
            GameLimitsNetworkSegmentsUsed,
            GameLimitsNetworkSegmentsCapacity,
            GameLimitsPaintedAreasUsedPercent,
            GameLimitsPaintedAreasUsed,
            GameLimitsPaintedAreasCapacity,
            GameLimitsPathUnitsUsedPercent,
            GameLimitsPathUnitsUsed,
            GameLimitsPathUnitsCapacity,
            GameLimitsPropsUsedPercent,
            GameLimitsPropsUsed,
            GameLimitsPropsCapacity,
            GameLimitsRadioChannelsUsedPercent,
            GameLimitsRadioChannelsUsed,
            GameLimitsRadioChannelsCapacity,
            GameLimitsRadioContentsUsedPercent,
            GameLimitsRadioContentsUsed,
            GameLimitsRadioContentsCapacity,
            GameLimitsTransportLinesUsedPercent,
            GameLimitsTransportLinesUsed,
            GameLimitsTransportLinesCapacity,
            GameLimitsTreesUsedPercent,
            GameLimitsTreesUsed,
            GameLimitsTreesCapacity,
            GameLimitsVehiclesUsedPercent,
            GameLimitsVehiclesUsed,
            GameLimitsVehiclesCapacity,
            GameLimitsVehiclesParkedUsedPercent,
            GameLimitsVehiclesParkedUsed,
            GameLimitsVehiclesParkedCapacity,
            GameLimitsZoneBlocksUsedPercent,
            GameLimitsZoneBlocksUsed,
            GameLimitsZoneBlocksCapacity
        }

        // main properties set by the constructor
        private readonly Category _category;
        private readonly StatisticType _type;
        private readonly Translation.Key _descriptionKey1;
        private readonly Translation.Key _descriptionKey2;
        private readonly Translation.Key _unitsKey;
        private readonly FieldInfo _snapshotField;
        private readonly PropertyInfo _snapshotProperty;

        // properties needed for UI operations
        private bool _selected;
        private bool _enabled;
        private string _descriptionUnits;

        // UI elements that are referenced after they are created
        private UIPanel _panel;
        private UISprite _checkbox;
        private UILabel _description;
        private UILabel _amount;

        // text and line colors for this statistic
        private Color32 _textColor;
        private Color32 _lineColor;

        // needed by the graph
        private string _categoryDescription;
        private string _categoryDescriptionUnits;
        private string _units;
        private string _numberFormat;


        // statistic colors, a color can be used by more than one statistic
        #region statistic colors

        // color conversions
        private const float DarkerMultiplier = 0.7f;        // make a color darker
        private const float LineColorMultiplier = 0.75f;    // convert text color to line color

        // neutral
        private static Color    _colorNeutral1,                 _colorNeutral2;

        // info view mode
        private static Color32  _colorInfoTrafficTarget;
        private static Color32  _colorInfoPollution;
        private static Color32  _colorInfoNoisePollution;
        private static Color32  _colorInfoLandValue;
        private static Color32  _colorInfoHeating1,             _colorInfoHeating2;
        private static Color32  _colorInfoTourism;

        // routes
        private static Color32  _colorRoutePedestrian1,         _colorRoutePedestrian2;
        private static Color32  _colorRouteCyclist1,            _colorRouteCyclist2;
        private static Color32  _colorRoutePrivateCar1,         _colorRoutePrivateCar2;
        private static Color32  _colorRoutePublicTransport1,    _colorRoutePublicTransport2;
        private static Color32  _colorRouteCargoTruck1,         _colorRouteCargoTruck2;
        private static Color32  _colorRouteServiceVehicle1,     _colorRouteServiceVehicle2;
        private static Color32  _colorRouteDummy1,              _colorRouteDummy2,              _colorRouteDummy3;
        private static Color32  _colorRouteTotal;

        // zones
        private static Color32  _colorZoneResidentialLow;
        private static Color32  _colorZoneResidentialHigh;
        private static Color32  _colorZoneCommercialLow;
        private static Color32  _colorZoneCommercialHigh;
        private static Color32  _colorZoneIndustrial;
        private static Color32  _colorZoneOffice;
        private static Color32  _colorZoneUnzoned;

        // zone levels
        private static Color32  _colorLevelResidential,         _colorResidentialLevel1,        _colorResidentialLevel2,        _colorResidentialLevel3,    _colorResidentialLevel4,    _colorResidentialLevel5;
        private static Color32  _colorLevelCommercial,          _colorCommercialLevel1,         _colorCommercialLevel2,         _colorCommercialLevel3;
        private static Color32  _colorLevelIndustrial,          _colorIndustrialLevel1,         _colorIndustrialLevel2,         _colorIndustrialLevel3;
        private static Color32  _colorLevelOffice,              _colorOfficeLevel1,             _colorOfficeLevel2,             _colorOfficeLevel3;

        // mid zones
        private static Color32  _colorZoneResidentialMid;
        private static Color32  _colorZoneCommercialMid;

        // transport
        private static Color32  _colorTransportBus1,            _colorTransportBus2,            _colorTransportBus3;
        private static Color32  _colorTransportTrolleybus1,     _colorTransportTrolleybus2,     _colorTransportTrolleybus3;
        private static Color32  _colorTransportTram1,           _colorTransportTram2,           _colorTransportTram3;
        private static Color32  _colorTransportMetro1,          _colorTransportMetro2,          _colorTransportMetro3;
        private static Color32  _colorTransportTrain1,          _colorTransportTrain2,          _colorTransportTrain3;
        private static Color32  _colorTransportShip1,           _colorTransportShip2,           _colorTransportShip3;
        private static Color32  _colorTransportAir1,            _colorTransportAir2,            _colorTransportAir3;
        private static Color32  _colorTransportMonorail1,       _colorTransportMonorail2,       _colorTransportMonorail3;
        private static Color32  _colorTransportCableCar1,       _colorTransportCableCar2,       _colorTransportCableCar3;
        private static Color32  _colorTransportTaxi1,           _colorTransportTaxi2,           _colorTransportTaxi3;
        private static Color32  _colorTransportPedestrian1,     _colorTransportPedestrian2,     _colorTransportPedestrian3;
        private static Color32  _colorTransportTouristBus1,     _colorTransportTouristBus2;
        private static Color32  _colorTransportHotAirBalloon1,  _colorTransportHotAirBalloon2;
        private static Color32  _colorTransportTotal1,          _colorTransportTotal2;

        // transfer reason
        private static Color32  _colorTransferGoods1,           _colorTransferGoods2;
        private static Color32  _colorTransferForestry1,        _colorTransferForestry2,        _colorTransferForestry3;
        private static Color32  _colorTransferFarming1,         _colorTransferFarming2,         _colorTransferFarming3;
        private static Color32  _colorTransferOre1,             _colorTransferOre2,             _colorTransferOre3;
        private static Color32  _colorTransferOil1,             _colorTransferOil2,             _colorTransferOil3;
        private static Color32  _colorTransferMail1,            _colorTransferMail2,            _colorTransferMail3;
        private static Color32  _colorTransferFish1,            _colorTransferFish2,            _colorTransferFish3;
        private static Color32  _colorTransferTotal1,           _colorTransferTotal2,           _colorTransferTotal3;

        // natural resources
        private static Color32  _colorResourceForestry1,        _colorResourceForestry2;
        private static Color32  _colorResourceFertility1,       _colorResourceFertility2;
        private static Color32  _colorResourceOre1,             _colorResourceOre2;
        private static Color32  _colorResourceOil1,             _colorResourceOil2;

        // education
        private static Color32  _colorUneducated1,              _colorUneducated2;
        private static Color32  _colorEducated1,                _colorEducated2;
        private static Color32  _colorWellEducated1,            _colorWellEducated2;
        private static Color32  _colorHighlyEducated1,          _colorHighlyEducated2;

        // age group
        private static Color32  _colorChild;
        private static Color32  _colorTeen;
        private static Color32  _colorYoung;
        private static Color32  _colorAdult;
        private static Color32  _colorSenior;

        // tourism
        private static Color32  _colorTouristsLowWealth1,       _colorTouristsLowWealth2;
        private static Color32  _colorTouristsMediumWealth1,    _colorTouristsMediumWealth2;
        private static Color32  _colorTouristsHighWealth1,      _colorTouristsHighWealth2;
        private static Color32  _colorExchangeStudent;
        private static Color32  _colorTouristsTotal;

        // statistics from StatisticsPanel
        private static Color32  _colorStatisticsHappiness;
        private static Color32  _colorStatisticsBirthRate;
        private static Color32  _colorStatisticsDeathRate;
        private static Color32  _colorStatisticsPopulation;
        private static Color32  _colorStatisticsEmployment;
        private static Color32  _colorStatisticsJobs;
        private static Color32  _colorStatisticsCityValue;
        private static Color32  _colorStatisticsCityBudget;

        // miscellaneous that get specific colors
        private static Color32  _colorElectricity1,             _colorElectricity2;
        private static Color32  _colorWater1,                   _colorWater2;
        private static Color32  _colorWaterTank1,               _colorWaterTank2;
        private static Color32  _colorSewage1,                  _colorSewage2;
        private static Color32  _colorGarbage1,                 _colorGarbage2;
        private static Color32  _colorLandfill1,                _colorLandfill2;
        private static Color32  _colorHealthcare1,              _colorHealthcare2,              _colorHealthcareAverage;
        private static Color32  _colorDeathcare1,               _colorDeathcare2,               _colorDeathcare3,           _colorDeathcare4;
        private static Color32  _colorChildcare1,               _colorChildcare2;
        private static Color32  _colorEldercare1,               _colorEldercare2;
        private static Color32  _colorFireSafety;
        private static Color32  _colorCrime1,                   _colorCrime2,                   _colorCrimeRate;
        private static Color32  _colorBanks;
        private static Color32  _colorCash1,                    _colorCash2,                    _colorCash3;
        private static Color32  _colorArriving1,                _colorArriving2,                _colorArriving3;
        private static Color32  _colorDeparting1,               _colorDeparting2,               _colorDeparting3;
        private static Color32  _colorHouseholds1,              _colorHouseholds2;
        private static Color32  _colorEmployment1,              _colorEmployment2,              _colorEmployment3;
        private static Color32  _colorUnemployment1,            _colorUnemployment2;
        private static Color32  _colorChirper;
        private static Color32  _colorInvestmentGain1,          _colorInvestmentGain2;
        private static Color32  _colorCityTotalIncome,          _colorCityTotalExpenses,        _colorCityTotalProfit;
        private static Color32  _colorBankBalance;
        private static Color32  _colorIncomeSelfSufficient;
        private static Color32  _colorIncomeLeisure,            _colorTourismIncome,            _colorIncomeOrganic;
        private static Color32  _colorIncomeITCluster,          _colorIncomeFinancial,          _colorIncomeOfficeSpecialized;
        private static Color32  _colorIncomeWallToWall;
        private static Color32  _colorParks;
        private static Color32  _colorEmergency;
        private static Color32  _colorServicePoints;
        private static Color32  _colorUniqueBuildings;
        private static Color32  _colorGenericSportsArenas;
        private static Color32  _colorEconomy;
        private static Color32  _colorPolicies;
        private static Color32  _colorHotels;
        private static Color32  _colorHotelSightseeing;
        private static Color32  _colorHotelShopping;
        private static Color32  _colorHotelBusiness;
        private static Color32  _colorHotelNature;
        private static Color32  _colorHotelGuests1,             _colorHotelGuests2;
        private static Color32  _colorCityPark1,                _colorCityPark2,                _colorCityPark3;
        private static Color32  _colorAmusementPark1,           _colorAmusementPark2,           _colorAmusementPark3;
        private static Color32  _colorZoo1,                     _colorZoo2,                     _colorZoo3;
        private static Color32  _colorNatureReserve1,           _colorNatureReserve2,           _colorNatureReserve3;
        private static Color32  _colorTradeSchool1,             _colorTradeSchool2,             _colorTradeSchool3;
        private static Color32  _colorLiberalArts1,             _colorLiberalArts2,             _colorLiberalArts3;
        private static Color32  _colorUniversity1,              _colorUniversity2,              _colorUniversity3;
        private static Color32  _colorTollBooth1,               _colorTollBooth2,               _colorTollBooth3;
        private static Color32                                  _colorSpaceElevator2,           _colorSpaceElevator3;

        #endregion


        /// <summary>
        /// constructor to set main and other properties
        /// </summary>
        public Statistic(Category category, StatisticType type, Translation.Key descriptionKey1, Translation.Key descriptionKey2, Translation.Key unitsKey)
        {
            // save params
            _category = category;
            _type = type;
            _descriptionKey1 = descriptionKey1;
            _descriptionKey2 = descriptionKey2;
            _unitsKey = unitsKey;

            // description key depends on DLC
            if (_descriptionKey1 == Translation.Key.WaterSewage && SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC))
            {
                _descriptionKey1 = Translation.Key.WaterSewageHeating;
            }

            // set number format
            if (_type.ToString().EndsWith("Percent")               ||
                _type.ToString().EndsWith("PerCapita")             ||
                _type == StatisticType.TourismExchangeStudentBonus ||
                _type == StatisticType.HotelsTotalPopularity       ||
                _type == StatisticType.HotelsSightseeingPopularity ||
                _type == StatisticType.HotelsShoppingPopularity    ||
                _type == StatisticType.HotelsBusinessPopularity    ||
                _type == StatisticType.HotelsNaturePopularity
                )
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

            // get field or property info
            Snapshot.GetFieldProperty(_type, out _snapshotField, out _snapshotProperty);
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
            const float checkboxSize = UIHeight - 2f;
            _checkbox = _panel.AddUIComponent<UISprite>();
            if (_checkbox == null)
            {
                LogUtil.LogError($"Unable to create statistic checkbox for [{_type}].");
                return false;
            }
            _checkbox.name = namePrefix + "Checkbox";
            _checkbox.autoSize = false;
            _checkbox.size = new Vector2(checkboxSize, checkboxSize);
            _checkbox.relativePosition = new Vector3(20f, 0f);

            // create the description label
            _description = _panel.AddUIComponent<UILabel>();
            if (_description == null)
            {
                LogUtil.LogError($"Unable to create statistic description label for [{_type}].");
                return false;
            }
            _description.name = namePrefix + "Description";
            _description.textScale = 0.625f;
            _description.relativePosition = new Vector3(_checkbox.relativePosition.x + _checkbox.size.x + 3f, 3f);
            _description.autoSize = false;
            _description.size = new Vector2(_panel.size.x - _description.relativePosition.x, UIHeight);

            // create the amount label on top of the description
            _amount = _panel.AddUIComponent<UILabel>();
            if (_amount == null)
            {
                LogUtil.LogError($"Unable to create statistic amount label for [{_type}].");
                return false;
            }
            _amount.name = namePrefix + "Amount";
            _amount.textScale = _description.textScale;
            _amount.autoSize = false;
            _amount.size = new Vector2(_description.size.x - 100f, UIHeight);
            _amount.relativePosition = new Vector3(_panel.size.x - _amount.size.x - 2f, _description.relativePosition.y);
            _amount.textAlignment = UIHorizontalAlignment.Right;
            _amount.BringToFront();

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
            bool dlcAirports            = SteamHelper.IsDLCOwned(SteamHelper.DLC.AirportDLC);             // 01/25/22
            bool dlcPlazasPromenades    = SteamHelper.IsDLCOwned(SteamHelper.DLC.PlazasAndPromenadesDLC); // 09/14/22
            bool dlcFinancialDistricts  = SteamHelper.IsDLCOwned(SteamHelper.DLC.FinancialDistrictsDLC);  // 12/13/22
            bool dlcHotelsRetreats      = SteamHelper.IsDLCOwned(SteamHelper.DLC.HotelDLC);               // 05/23/23

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

                case StatisticType.TrafficCyclistsPercent:
                case StatisticType.TrafficCyclistsCount:
                    DisableForInactiveDLC(dlcAfterDark);
                    break;

                case StatisticType.CommercialCashAccumulatedPercent:
                case StatisticType.CommercialCashAccumulated:
                case StatisticType.CommercialCashCapacity:
                case StatisticType.CommercialCashCollected:
                    DisableForInactiveDLC(dlcFinancialDistricts);
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

                case StatisticType.InvestmentsChirpAir:
                case StatisticType.InvestmentsChirperCrypto:
                case StatisticType.InvestmentsDeathcareServiceFund:
                case StatisticType.InvestmentsFarmingIndustry:
                case StatisticType.InvestmentsForestryIndustry:
                case StatisticType.InvestmentsGreasyGasoline:
                case StatisticType.InvestmentsGenericIndustry:
                case StatisticType.InvestmentsHealthcareServiceFund:
                case StatisticType.InvestmentsOilIndustry:
                case StatisticType.InvestmentsOreIndustry:
                case StatisticType.InvestmentsChirpyCruises:
                case StatisticType.InvestmentsTrafficJellyLogistics:
                case StatisticType.InvestmentsVeryLegitCompany:
                case StatisticType.InvestmentsGainsLastMonth:
                case StatisticType.InvestmentsTotalGains:
                    DisableForInactiveDLC(dlcFinancialDistricts);
                    break;

                case StatisticType.ResidentialIncomeLowDensitySelfSufficient:
                case StatisticType.ResidentialIncomeHighDensitySelfSufficient:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.ResidentialIncomeWallToWall:
                    DisableForInactiveDLC(dlcPlazasPromenades);
                    break;

                case StatisticType.CommercialIncomeSpecializedTotal:
                    // display specialized total only if there are at least 2 specialized incomes
                    DisableForInactiveDLC((dlcAfterDark ? 2 : 0) + (dlcGreenCities ? 1 : 0) + (dlcPlazasPromenades ? 1 : 0) >= 2);
                    break;

                case StatisticType.CommercialIncomeLeisure:
                case StatisticType.CommercialIncomeTourism:
                    DisableForInactiveDLC(dlcAfterDark);
                    break;

                case StatisticType.CommercialIncomeOrganic:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.CommercialIncomeWallToWall:
                    DisableForInactiveDLC(dlcPlazasPromenades);
                    break;

                case StatisticType.OfficeIncomeGenericTotal:
                    // display generic total only if there is at least 1 specialized income
                    DisableForInactiveDLC((dlcGreenCities ? 1 : 0) + (dlcPlazasPromenades ? 1 : 0) + (dlcFinancialDistricts ? 1 : 0) >= 1);
                    break;

                case StatisticType.OfficeIncomeSpecializedTotal:
                    // display specialized total only if there are at least 2 specialized incomes
                    DisableForInactiveDLC((dlcGreenCities ? 1 : 0) + (dlcPlazasPromenades ? 1 : 0) + (dlcFinancialDistricts ? 1 : 0) >= 2);
                    break;

                case StatisticType.OfficeIncomeITCluster:
                    DisableForInactiveDLC(dlcGreenCities);
                    break;

                case StatisticType.OfficeIncomeWallToWall:
                    DisableForInactiveDLC(dlcPlazasPromenades);
                    break;

                case StatisticType.OfficeIncomeFinancial:
                    DisableForInactiveDLC(dlcFinancialDistricts);
                    break;

                case StatisticType.TourismIncomeParkAreas:
                    DisableForInactiveDLC(dlcParkLife);
                    break;

                case StatisticType.TourismIncomeHotels:
                    DisableForInactiveDLC(dlcHotelsRetreats);
                    break;

                case StatisticType.ServiceExpensesEmergency:
                    DisableForInactiveDLC(dlcNaturalDisasters);
                    break;

                case StatisticType.ServiceExpensesBanks:
                    DisableForInactiveDLC(dlcFinancialDistricts);
                    break;

                case StatisticType.ServiceExpensesServicePoints:
                    DisableForInactiveDLC(dlcPlazasPromenades);
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

                case StatisticType.FishingIndustryFishingIncome:
                case StatisticType.FishingIndustryFishingExpenses:
                case StatisticType.FishingIndustryFishingProfit:
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

                case StatisticType.HotelsTotalIncomePercent:
                case StatisticType.HotelsTotalIncome:
                case StatisticType.HotelsTotalExpensesPercent:
                case StatisticType.HotelsTotalExpenses:
                case StatisticType.HotelsTotalProfit:
                case StatisticType.HotelsTotalPopularity:
                case StatisticType.HotelsSightseeingPopularity:
                case StatisticType.HotelsShoppingPopularity:
                case StatisticType.HotelsBusinessPopularity:
                case StatisticType.HotelsNaturePopularity:
                case StatisticType.HotelsGuestsVisitingPercent:
                case StatisticType.HotelsGuestsVisiting:
                case StatisticType.HotelsGuestsCapacity:
                    DisableForInactiveDLC(dlcHotelsRetreats);
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

                case StatisticType.GameLimitsPaintedAreasUsedPercent:
                case StatisticType.GameLimitsPaintedAreasUsed:
                case StatisticType.GameLimitsPaintedAreasCapacity:
                    DisableForInactiveDLC(dlcPlazasPromenades || dlcParkLife || dlcIndustries || dlcCampus || dlcAirports);
                    break;
            }

            // set text and line color
            SetTextLineColor();

            // logging to get list of statistics
            //UpdateUIText();
            //LogUtil.LogInfo("• " + _descriptionUnits);

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
        /// define all colors used by statistics
        /// </summary>
        public static void DefineColors()
        {
            try
            {
                // generally for colors:
                //      colors are lighter for usage (variable named with 1) and darker for capacity (variable named with 2)
                //      usage percent color is same as usage amount color because it is expected that percent and amount will not be shown at the same time and using the same color reduces the number of unique colors to be defined
                //      colors are lighter for income (variable named with 1), middle for expense (variable named with 2), and darker for profit (variable named with 3)

                // get colors from InfoManager
                if (InfoManager.exists)
                {
                    // get neutral color
                    _colorNeutral1 = InfoManager.instance.m_properties.m_neutralColor; _colorNeutral2 = _colorNeutral1 * DarkerMultiplier;

                    // get info mode colors
                    InfoProperties.ModeProperties[] modeProperties = InfoManager.instance.m_properties.m_modeProperties;
                    _colorInfoTrafficTarget  = modeProperties[(int)InfoManager.InfoMode.Traffic       ].m_targetColor;
                    _colorInfoPollution      = modeProperties[(int)InfoManager.InfoMode.Pollution     ].m_activeColor;
                    _colorInfoNoisePollution = modeProperties[(int)InfoManager.InfoMode.NoisePollution].m_activeColor;
                    _colorInfoLandValue      = modeProperties[(int)InfoManager.InfoMode.LandValue     ].m_activeColor;
                    _colorInfoHeating1       = modeProperties[(int)InfoManager.InfoMode.Heating       ].m_targetColor; _colorInfoHeating2 = _colorInfoHeating1.Multiply(DarkerMultiplier);
                    _colorInfoTourism        = modeProperties[(int)InfoManager.InfoMode.Tourism       ].m_activeColor;

                    // get traffic route colors
                    Color[] routeColors = InfoManager.instance.m_properties.m_routeColors;
                    _colorRoutePedestrian1      = routeColors[(int)InfoProperties.RouteType.Pedestrian];         _colorRoutePedestrian2      = _colorRoutePedestrian1     .Multiply(DarkerMultiplier);
                    _colorRouteCyclist1         = routeColors[(int)InfoProperties.RouteType.Cyclist];            _colorRouteCyclist2         = _colorRouteCyclist1        .Multiply(DarkerMultiplier);
                    _colorRoutePrivateCar1      = routeColors[(int)InfoProperties.RouteType.PrivateCar];         _colorRoutePrivateCar2      = _colorRoutePrivateCar1     .Multiply(DarkerMultiplier);
                    _colorRoutePublicTransport1 = routeColors[(int)InfoProperties.RouteType.PublicTransport];    _colorRoutePublicTransport2 = _colorRoutePublicTransport1.Multiply(DarkerMultiplier);
                    _colorRouteCargoTruck1      = routeColors[(int)InfoProperties.RouteType.CargoTruck];         _colorRouteCargoTruck2      = _colorRouteCargoTruck1     .Multiply(DarkerMultiplier);
                    _colorRouteServiceVehicle1  = routeColors[(int)InfoProperties.RouteType.ServiceVehicle];     _colorRouteServiceVehicle2  = _colorRouteServiceVehicle1 .Multiply(DarkerMultiplier);
                    _colorRouteDummy1           = _colorNeutral2;                                                _colorRouteDummy2           = _colorRouteDummy1          .Multiply(DarkerMultiplier); _colorRouteDummy3 = _colorRouteDummy2.Multiply(DarkerMultiplier);
                    _colorRouteTotal            = new Color32(6, 92, 177, 255);    // color taken manually from Traffic Routes info view icon
                }
                else
                {
                    LogUtil.LogError("InfoManager not ready.");
                }

                // get colors from ZoneManager
                if (ZoneManager.exists)
                {
                    // get zone colors
                    Color[] zoneColors = ZoneManager.instance.m_properties.m_zoneColors;
                    _colorZoneResidentialLow  = zoneColors[(int)ItemClass.Zone.ResidentialLow ];
                    _colorZoneResidentialHigh = zoneColors[(int)ItemClass.Zone.ResidentialHigh];
                    _colorZoneCommercialLow   = zoneColors[(int)ItemClass.Zone.CommercialLow  ];
                    _colorZoneCommercialHigh  = zoneColors[(int)ItemClass.Zone.CommercialHigh ];
                    _colorZoneIndustrial      = zoneColors[(int)ItemClass.Zone.Industrial     ];
                    _colorZoneOffice          = zoneColors[(int)ItemClass.Zone.Office         ];
                    _colorZoneUnzoned         = new Color32(152, 106, 65, 255);  // color taken manually from De-Zone tool icon

                    // make mid colors halfway between low and high colors
                    _colorZoneResidentialMid = Color32.Lerp(_colorZoneResidentialLow, _colorZoneResidentialHigh, 0.5f);
                    _colorZoneCommercialMid  = Color32.Lerp(_colorZoneCommercialLow,  _colorZoneCommercialHigh,  0.5f);

                    // logic for levels colors is copied from LevelsInfoViewPanel.UpdatePanel for initializing the radial chart colors
                    // start with a darker (i.e. 70%) version of each zone color and then interpolate between neutral color and that color
                    _colorLevelResidential = _colorZoneResidentialMid.Multiply(0.7f);
                    _colorLevelCommercial  = _colorZoneCommercialMid .Multiply(0.7f);
                    _colorLevelIndustrial  = _colorZoneIndustrial    .Multiply(0.7f);
                    _colorLevelOffice      = _colorZoneOffice        .Multiply(0.7f);
                    _colorResidentialLevel1 = Color32.Lerp(_colorNeutral1, _colorLevelResidential, 0.200f);
                    _colorResidentialLevel2 = Color32.Lerp(_colorNeutral1, _colorLevelResidential, 0.400f);
                    _colorResidentialLevel3 = Color32.Lerp(_colorNeutral1, _colorLevelResidential, 0.600f);
                    _colorResidentialLevel4 = Color32.Lerp(_colorNeutral1, _colorLevelResidential, 0.800f);
                    _colorResidentialLevel5 = Color32.Lerp(_colorNeutral1, _colorLevelResidential, 1.000f);
                    _colorCommercialLevel1  = Color32.Lerp(_colorNeutral1, _colorLevelCommercial,  0.333f);
                    _colorCommercialLevel2  = Color32.Lerp(_colorNeutral1, _colorLevelCommercial,  0.667f);
                    _colorCommercialLevel3  = Color32.Lerp(_colorNeutral1, _colorLevelCommercial,  1.000f);
                    _colorIndustrialLevel1  = Color32.Lerp(_colorNeutral1, _colorLevelIndustrial,  0.333f);
                    _colorIndustrialLevel2  = Color32.Lerp(_colorNeutral1, _colorLevelIndustrial,  0.667f);
                    _colorIndustrialLevel3  = Color32.Lerp(_colorNeutral1, _colorLevelIndustrial,  1.000f);
                    _colorOfficeLevel1      = Color32.Lerp(_colorNeutral1, _colorLevelOffice,      0.333f);
                    _colorOfficeLevel2      = Color32.Lerp(_colorNeutral1, _colorLevelOffice,      0.667f);
                    _colorOfficeLevel3      = Color32.Lerp(_colorNeutral1, _colorLevelOffice,      1.000f);
                }
                else
                {
                    LogUtil.LogError("ZoneManager not ready.");
                }

                // get colors from TransportManager
                if (TransportManager.exists)
                {
                    // get transport colors
                    Color[] transportColors = TransportManager.instance.m_properties.m_transportColors;
                    _colorTransportBus1           = transportColors[(int)TransportInfo.TransportType.Bus          ]; _colorTransportBus2           = _colorTransportBus1          .Multiply(DarkerMultiplier); _colorTransportBus3        = _colorTransportBus2       .Multiply(DarkerMultiplier);
                    _colorTransportTrolleybus1    = transportColors[(int)TransportInfo.TransportType.Trolleybus   ]; _colorTransportTrolleybus2    = _colorTransportTrolleybus1   .Multiply(DarkerMultiplier); _colorTransportTrolleybus3 = _colorTransportTrolleybus2.Multiply(DarkerMultiplier);
                    _colorTransportTram1          = transportColors[(int)TransportInfo.TransportType.Tram         ]; _colorTransportTram2          = _colorTransportTram1         .Multiply(DarkerMultiplier); _colorTransportTram3       = _colorTransportTram2      .Multiply(DarkerMultiplier);
                    _colorTransportMetro1         = transportColors[(int)TransportInfo.TransportType.Metro        ]; _colorTransportMetro2         = _colorTransportMetro1        .Multiply(DarkerMultiplier); _colorTransportMetro3      = _colorTransportMetro2     .Multiply(DarkerMultiplier);
                    _colorTransportTrain1         = transportColors[(int)TransportInfo.TransportType.Train        ]; _colorTransportTrain2         = _colorTransportTrain1        .Multiply(DarkerMultiplier); _colorTransportTrain3      = _colorTransportTrain2     .Multiply(DarkerMultiplier);
                    _colorTransportShip1          = transportColors[(int)TransportInfo.TransportType.Ship         ]; _colorTransportShip2          = _colorTransportShip1         .Multiply(DarkerMultiplier); _colorTransportShip3       = _colorTransportShip2      .Multiply(DarkerMultiplier);
                    _colorTransportAir1           = transportColors[(int)TransportInfo.TransportType.Airplane     ]; _colorTransportAir2           = _colorTransportAir1          .Multiply(DarkerMultiplier); _colorTransportAir3        = _colorTransportAir2       .Multiply(DarkerMultiplier);
                    _colorTransportMonorail1      = transportColors[(int)TransportInfo.TransportType.Monorail     ]; _colorTransportMonorail2      = _colorTransportMonorail1     .Multiply(DarkerMultiplier); _colorTransportMonorail3   = _colorTransportMonorail2  .Multiply(DarkerMultiplier);
                    _colorTransportCableCar1      = transportColors[(int)TransportInfo.TransportType.CableCar     ]; _colorTransportCableCar2      = _colorTransportCableCar1     .Multiply(DarkerMultiplier); _colorTransportCableCar3   = _colorTransportCableCar2  .Multiply(DarkerMultiplier);
                    _colorTransportTaxi1          = transportColors[(int)TransportInfo.TransportType.Taxi         ]; _colorTransportTaxi2          = _colorTransportTaxi1         .Multiply(DarkerMultiplier); _colorTransportTaxi3       = _colorTransportTaxi2      .Multiply(DarkerMultiplier);
                    _colorTransportPedestrian1    = transportColors[(int)TransportInfo.TransportType.Pedestrian   ]; _colorTransportPedestrian2    = _colorTransportPedestrian1   .Multiply(DarkerMultiplier); _colorTransportPedestrian3 = _colorTransportPedestrian2.Multiply(DarkerMultiplier);
                    _colorTransportTouristBus1    = transportColors[(int)TransportInfo.TransportType.TouristBus   ]; _colorTransportTouristBus2    = _colorTransportTouristBus1   .Multiply(DarkerMultiplier);
                    _colorTransportHotAirBalloon1 = transportColors[(int)TransportInfo.TransportType.HotAirBalloon]; _colorTransportHotAirBalloon2 = _colorTransportHotAirBalloon1.Multiply(DarkerMultiplier);

                    // get transport total colors
                    _colorTransportTotal1 = new Color32(206, 248, 000, 255);            // color taken manually from TransportInfoViewPanel for Total text
                    _colorTransportTotal2 = _colorTransportTotal1.Multiply(DarkerMultiplier);
                }
                else
                {
                    LogUtil.LogError("TransportManager not ready.");
                }

                // get colors from TransferManager
                if (TransferManager.exists)
                {
                    // get transfer colors
                    Color[] transferColors = TransferManager.instance.m_properties.m_resourceColors;
                    _colorTransferGoods1    = transferColors[(int)TransferManager.TransferReason.Goods]; _colorTransferGoods2    = _colorTransferGoods1   .Multiply(DarkerMultiplier);
                    _colorTransferForestry1 = transferColors[(int)TransferManager.TransferReason.Logs ]; _colorTransferForestry2 = _colorTransferForestry1.Multiply(DarkerMultiplier); _colorTransferForestry3 = _colorTransferForestry2.Multiply(DarkerMultiplier);
                    _colorTransferFarming1  = transferColors[(int)TransferManager.TransferReason.Grain]; _colorTransferFarming2  = _colorTransferFarming1 .Multiply(DarkerMultiplier); _colorTransferFarming3  = _colorTransferFarming2 .Multiply(DarkerMultiplier);
                    _colorTransferOre1      = transferColors[(int)TransferManager.TransferReason.Ore  ]; _colorTransferOre2      = _colorTransferOre1     .Multiply(DarkerMultiplier); _colorTransferOre3      = _colorTransferOre2     .Multiply(DarkerMultiplier);
                    _colorTransferOil1      = transferColors[(int)TransferManager.TransferReason.Oil  ];
                    _colorTransferMail1     = transferColors[(int)TransferManager.TransferReason.Mail ]; _colorTransferMail2     = _colorTransferMail1    .Multiply(DarkerMultiplier); _colorTransferMail3     = _colorTransferMail2    .Multiply(DarkerMultiplier);
                    _colorTransferFish1     = transferColors[(int)TransferManager.TransferReason.Fish ]; _colorTransferFish2     = _colorTransferFish1    .Multiply(DarkerMultiplier); _colorTransferFish3     = _colorTransferFish2    .Multiply(DarkerMultiplier);

                    // make oil lighter because the original color is very dark
                    _colorTransferOil1 = _colorTransferOil1.Multiply(1.4f);
                    _colorTransferOil2 = _colorTransferOil1.Multiply(DarkerMultiplier);
                    _colorTransferOil3 = _colorTransferOil2.Multiply(DarkerMultiplier);

                    // get transfer total colors
                    _colorTransferTotal1 = new Color32(128, 128, 128, 255);          // a shade of gray
                    _colorTransferTotal2 = _colorTransferTotal1.Multiply(DarkerMultiplier);
                    _colorTransferTotal3 = _colorTransferTotal2.Multiply(DarkerMultiplier);
                }
                else
                {
                    LogUtil.LogError("TransferManager not ready.");
                }

                // get colors from NaturalResourceManager
                if (NaturalResourceManager.exists)
                {
                    // get resource colors
                    Color[] resourceColors = NaturalResourceManager.instance.m_properties.m_resourceColors;
                    _colorResourceForestry1  = resourceColors[(int)NaturalResourceManager.Resource.Forest   ]; _colorResourceForestry2  = _colorResourceForestry1 .Multiply(DarkerMultiplier);
                    _colorResourceFertility1 = resourceColors[(int)NaturalResourceManager.Resource.Fertility]; _colorResourceFertility2 = _colorResourceFertility1.Multiply(DarkerMultiplier);
                    _colorResourceOre1       = resourceColors[(int)NaturalResourceManager.Resource.Ore      ]; _colorResourceOre2       = _colorResourceOre1      .Multiply(DarkerMultiplier);
                    _colorResourceOil1       = resourceColors[(int)NaturalResourceManager.Resource.Oil      ];

                    // make oil lighter because the original color is very dark
                    _colorResourceOil1 = _colorResourceOil1.Multiply(1.5f);
                    _colorResourceOil2 = _colorResourceOil1.Multiply(DarkerMultiplier);
                }
                else
                {
                    LogUtil.LogError("NaturalResourceManager not ready.");
                }

                // get colors from EducationInfoViewPanel
                EducationInfoViewPanel educationInfoViewPanel = UIView.library.Get<EducationInfoViewPanel>(typeof(EducationInfoViewPanel).Name);
                if (educationInfoViewPanel != null)
                {
                    // get education level colors
                    _colorUneducated1     = educationInfoViewPanel.m_UneducatedColor;     _colorUneducated2     = _colorUneducated1    .Multiply(DarkerMultiplier);
                    _colorEducated1       = educationInfoViewPanel.m_EducatedColor;       _colorEducated2       = _colorEducated1      .Multiply(DarkerMultiplier);
                    _colorWellEducated1   = educationInfoViewPanel.m_WellEducatedColor;   _colorWellEducated2   = _colorWellEducated1  .Multiply(DarkerMultiplier);
                    _colorHighlyEducated1 = educationInfoViewPanel.m_HighlyEducatedColor; _colorHighlyEducated2 = _colorHighlyEducated1.Multiply(DarkerMultiplier);
                }
                else
                {
                    LogUtil.LogError("Unable to find EducationInfoViewPanel.");
                }

                // get colors from PopulationInfoViewPanel
                PopulationInfoViewPanel populationInfoViewPanel = UIView.library.Get<PopulationInfoViewPanel>(typeof(PopulationInfoViewPanel).Name);
                if (populationInfoViewPanel != null)
                {
                    // get age group colors
                    _colorChild  = populationInfoViewPanel.m_ChildColor;
                    _colorTeen   = populationInfoViewPanel.m_TeenColor;
                    _colorYoung  = populationInfoViewPanel.m_YoungColor;
                    _colorAdult  = populationInfoViewPanel.m_AdultColor;
                    _colorSenior = populationInfoViewPanel.m_SeniorColor;
                }
                else
                {
                    LogUtil.LogError("Unable to find PopulationInfoViewPanel.");
                }

                // get colors from TourismInfoViewPanel
                TourismInfoViewPanel tourismInfoViewPanel = UIView.library.Get<TourismInfoViewPanel>(typeof(TourismInfoViewPanel).Name);
                if (tourismInfoViewPanel != null)
                {
                    // get tourism colors
                    UIRadialChart touristWealthChart   = tourismInfoViewPanel.Find<UIRadialChart>("TouristWealthChart");
                    UIRadialChart exchangeStudentChart = tourismInfoViewPanel.Find<UIRadialChart>("ExchangeStudentChart");
                    _colorTouristsLowWealth1    = touristWealthChart.GetSlice(0).innerColor; _colorTouristsLowWealth2    = _colorTouristsLowWealth1   .Multiply(DarkerMultiplier);
                    _colorTouristsMediumWealth1 = touristWealthChart.GetSlice(1).innerColor; _colorTouristsMediumWealth2 = _colorTouristsMediumWealth1.Multiply(DarkerMultiplier);
                    _colorTouristsHighWealth1   = touristWealthChart.GetSlice(2).innerColor; _colorTouristsHighWealth2   = _colorTouristsHighWealth1  .Multiply(DarkerMultiplier);
                    _colorExchangeStudent       = exchangeStudentChart.GetSlice(0).innerColor;
                    _colorTouristsTotal         = exchangeStudentChart.GetSlice(1).innerColor;
                }
                else
                {
                    LogUtil.LogError("Unable to find TourismInfoViewPanel.");
                }

                // get colors from StatisticsPanel
                StatisticsPanel statisticsPanel = UIView.library.Get<StatisticsPanel>(typeof(StatisticsPanel).Name);
                if (statisticsPanel != null)
                {
                    // get statistics names field info
                    FieldInfo fiStatisticsNames = typeof(StatisticsPanel).GetField("StatisticsNames", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fiStatisticsNames != null)
                    {
                        // get statistics names
                        string[] statisticsNames = (string[])fiStatisticsNames.GetValue(statisticsPanel);
                        if (statisticsNames != null)
                        {
                            // get statistics colors field info
                            FieldInfo fiStatisticsColors = typeof(StatisticsPanel).GetField("m_statisticsColors", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (fiStatisticsColors != null)
                            {
                                // get statistics colors
                                Color32[] statisticsColors = (Color32[])fiStatisticsColors.GetValue(statisticsPanel);
                                if (statisticsColors != null)
                                {
                                    // loop over statistic names searching for the ones we care about
                                    for (int i = 0; i < statisticsNames.Length; i++)
                                    {
                                        // statistic color index is same as statistic name index
                                        switch (statisticsNames[i])
                                        {
                                            case "Happiness":  _colorStatisticsHappiness  = statisticsColors[i]; break;
                                            case "Birth":      _colorStatisticsBirthRate  = statisticsColors[i]; break;
                                            case "Death":      _colorStatisticsDeathRate  = statisticsColors[i]; break;
                                            case "Population": _colorStatisticsPopulation = statisticsColors[i]; break;
                                            case "Employment": _colorStatisticsEmployment = statisticsColors[i]; break;
                                            case "Jobs":       _colorStatisticsJobs       = statisticsColors[i]; break;
                                            case "Value":      _colorStatisticsCityValue  = statisticsColors[i]; break;
                                            case "Budget":     _colorStatisticsCityBudget = statisticsColors[i]; break;
                                        }
                                    }

                                }
                                else
                                {
                                    LogUtil.LogError("Unable to get StatisticsPanel.m_statisticsColors.");
                                }
                            }
                            else
                            {
                                LogUtil.LogError("Unable to find StatisticsPanel.m_statisticsColors.");
                            }
                        }
                        else
                        {
                            LogUtil.LogError("Unable to get StatisticsPanel.StatisticsNames.");
                        }
                    }
                    else
                    {
                        LogUtil.LogError("Unable to find StatisticsPanel.StatisticsNames.");
                    }
                }
                else
                {
                    LogUtil.LogError("Unable to find StatisticsPanel.");
                }

                // miscellaneous specific colors
                _colorElectricity1 = new Color32(002, 168, 254, 255);               // color taken manually from Electricity info view icon
                _colorElectricity2 = new Color32(045, 098, 143, 255);               // color taken manually from Electricity info view icon

                _colorWater1 = new Color32(134, 187, 241, 255);                     // color taken manually from Water info view icon
                _colorWater2 = new Color32(023, 113, 206, 255);                     // color taken manually from Water info view icon

                _colorWaterTank1 = _colorWater1.Multiply(DarkerMultiplier);         // darker version of Water
                _colorWaterTank2 = _colorWater2.Multiply(DarkerMultiplier);         // darker version of Water

                _colorSewage1 = new Color32(038, 153, 134, 255);                    // color taken manually from WaterInfoViewPanel sewage section
                _colorSewage2 = _colorSewage1.Multiply(DarkerMultiplier);

                _colorGarbage1 = new Color32(163, 152, 000, 255);                   // color taken manually from Garbage info view icon
                _colorGarbage2 = new Color32(091, 076, 069, 255);                   // color taken manually from Garbage info view icon

                _colorLandfill1 = _colorGarbage1.Multiply(1.4f);                    // lighter version of Garbage
                _colorLandfill2 = _colorGarbage2.Multiply(1.4f);                    // lighter version of Garbage

                _colorHealthcare1 = new Color32(248, 021, 013, 255);                // color taken manually from Healthcare toolbar icon
                _colorHealthcare2 = _colorHealthcare1.Multiply(DarkerMultiplier);
                _colorHealthcareAverage = new Color32(013, 166, 066, 255);          // color taken manually from HealthInfoViewPanel Efficiency legend

                _colorDeathcare1 = new Color32(255, 000, 128, 255);                 // a shade of red
                _colorDeathcare2 = _colorDeathcare1.Multiply(DarkerMultiplier);
                _colorDeathcare3 = new Color32(255, 128, 128, 255);                 // another shade of red
                _colorDeathcare4 = _colorDeathcare3.Multiply(DarkerMultiplier);

                _colorChildcare1 = new Color32(                                     // color is half way between Child and Teen
                    (byte)(_colorChild.r / 2 + _colorTeen.r / 2),
                    (byte)(_colorChild.g / 2 + _colorTeen.g / 2),
                    (byte)(_colorChild.b / 2 + _colorTeen.b / 2), 255);
                _colorChildcare2 = _colorChildcare1.Multiply(DarkerMultiplier);

                _colorEldercare1 = _colorSenior.Multiply(1.25f);                    // lighter version of Senior
                _colorEldercare2 = _colorEldercare1.Multiply(DarkerMultiplier);

                _colorFireSafety = new Color32(232, 159, 056, 255);                 // color taken manually from Fire Department toolbar icon

                _colorCrime1    = new Color32(169, 095, 002, 255);                  // color taken manually from Police Department toolbar icon
                _colorCrime2    = _colorCrime1.Multiply(DarkerMultiplier);
                _colorCrimeRate = new Color32(128, 128, 128, 255);                  // a shade of gray

                _colorBanks = new Color32(253, 186, 156, 255);                      // color taken manually from Banks (piggy bank) tab icon

                _colorCash1 = new Color32(254, 214, 57, 255);                       // color taken manually from specialized district icon coin
                _colorCash2 = _colorCash1.Multiply(DarkerMultiplier);
                _colorCash3 = _colorCash2.Multiply(DarkerMultiplier);

                _colorArriving1 = Color.green;                                      // just green
                _colorArriving2 = _colorArriving1.Multiply(DarkerMultiplier);
                _colorArriving3 = _colorArriving2.Multiply(DarkerMultiplier);

                _colorDeparting1 = Color.red;                                       // just red
                _colorDeparting2 = _colorDeparting1.Multiply(DarkerMultiplier);
                _colorDeparting3 = _colorDeparting2.Multiply(DarkerMultiplier);

                _colorHouseholds1 = new Color32(206, 248, 000, 255);                // color taken manually from CityInfoPanel for households text
                _colorHouseholds2 = _colorHouseholds1.Multiply(DarkerMultiplier);

                _colorEmployment1 = _colorStatisticsJobs;
                _colorEmployment2 = _colorEmployment1.Multiply(DarkerMultiplier);
                _colorEmployment3 = _colorEmployment2.Multiply(DarkerMultiplier);
                _colorUnemployment1 = _colorStatisticsEmployment;
                _colorUnemployment2 = _colorUnemployment1.Multiply(DarkerMultiplier);

                _colorChirper = new Color32(34, 207, 247, 255);                     // color taken manually from InvestmentIconChirpCoin

                _colorInvestmentGain1 = new Color32(255, 236, 48, 255);             // color taken from StockExchangeWorldInfoPanel total investment gains
                _colorInvestmentGain2 = _colorInvestmentGain1.Multiply(DarkerMultiplier);

                _colorCityTotalIncome   = new Color32(090, 225, 020, 255);          // color taken manually from EconomyPanel
                _colorCityTotalExpenses = new Color32(254, 150, 089, 255);          // color taken manually from EconomyPanel
                _colorCityTotalProfit   = new Color32(                              // color is halfway between income and expenses
                    (byte)(_colorCityTotalIncome.r / 2 + _colorCityTotalExpenses.r / 2),
                    (byte)(_colorCityTotalIncome.g / 2 + _colorCityTotalExpenses.g / 2),
                    (byte)(_colorCityTotalIncome.b / 2 + _colorCityTotalExpenses.b / 2), 255);

                _colorBankBalance = new Color32(185, 221, 254, 255);                // color taken manually from InfoPanel.IncomePanel

                _colorIncomeSelfSufficient = new Color32(118, 234, 122, 255);       // color taken manually from EconomyPanel

                _colorIncomeLeisure = new Color32(135, 209, 218, 255);              // color taken manually from specialized district icon
                _colorTourismIncome = new Color32(242, 219, 057, 255);              // color taken manually from specialized district icon
                _colorIncomeOrganic = new Color32(132, 159, 000, 255);              // color taken manually from specialized district icon

                _colorIncomeITCluster = new Color32(039, 192, 231, 255);            // color taken manually from specialized district icon
                _colorIncomeOfficeSpecialized = new Color32(                        // color is halfway between IT Cluster and Office Zone
                    (byte)(_colorIncomeITCluster.r / 2 + _colorZoneOffice.r / 2),
                    (byte)(_colorIncomeITCluster.g / 2 + _colorZoneOffice.g / 2),
                    (byte)(_colorIncomeITCluster.b / 2 + _colorZoneOffice.b / 2), 255);
                _colorIncomeFinancial = _colorCash1;

                _colorIncomeWallToWall = new Color32(217, 142, 69, 255);            // color taken manually from specialized district icon, same color used for all 3 Wall-To-Wall

                _colorParks               = new Color32(073, 115, 122, 255);        // color taken manually from the horse of the Parks & Plazas toolbar icon
                _colorEmergency           = new Color32(254, 131, 000, 255);        // color taken manually from Landscape toolbar Disaster tab icon
                _colorServicePoints       = new Color32(211, 212, 212, 255);        // color taken manually from Pedestrian Areas tab icon
                _colorUniqueBuildings     = new Color32(082, 108, 113, 255);        // color taken manually from Unique Buildings toolbar icon
                _colorGenericSportsArenas = new Color32(076, 108, 173, 255);        // color taken manually from Education toolbar Varsity Sports tab icon
                _colorEconomy             = new Color32(061, 159, 010, 255);        // color taken manually from Economy toolbar icon
                _colorPolicies            = new Color32(208, 210, 211, 255);        // color taken manually from Policies toolbar icon

                _colorHotels           = new Color32(125, 189, 200, 255);           // color taken manually from Hotel info icon
                _colorHotelSightseeing = new Color32(194, 143,  31, 255);           // color taken manually from HotelWorldInfoPanel popularity icon and progress bar
                _colorHotelShopping    = new Color32(183,  87,  34, 255);           // color taken manually from HotelWorldInfoPanel popularity icon and progress bar
                _colorHotelBusiness    = new Color32( 28, 133, 190, 255);           // color taken manually from HotelWorldInfoPanel popularity icon and progress bar
                _colorHotelNature      = new Color32( 58, 165,  58, 255);           // color taken manually from HotelWorldInfoPanel popularity icon and progress bar
                _colorHotelGuests1     = new Color32(235, 201,  96, 255);           // color taken manually from HotelWorldInfoPanel room rate icon
                _colorHotelGuests2     = _colorHotelGuests1.Multiply(DarkerMultiplier);

                _colorCityPark1      = new Color32(244, 223, 168, 255);             // color taken manually from City Park main gate arch
                _colorCityPark2      = _colorCityPark1.Multiply(DarkerMultiplier);
                _colorCityPark3      = _colorCityPark2.Multiply(DarkerMultiplier);
                _colorAmusementPark1 = new Color32(204, 136, 083, 255);             // color taken manually from Amusement Park main gate path
                _colorAmusementPark2 = _colorAmusementPark1.Multiply(DarkerMultiplier);
                _colorAmusementPark3 = _colorAmusementPark2.Multiply(DarkerMultiplier);
                _colorZoo1           = new Color32(221, 185, 110, 255);             // color taken manually from Zoo main gate path
                _colorZoo2           = _colorZoo1.Multiply(DarkerMultiplier);
                _colorZoo3           = _colorZoo2.Multiply(DarkerMultiplier);
                _colorNatureReserve1 = new Color32(098, 145, 078, 255);             // color taken manually from Nature Reserve main gate building roof
                _colorNatureReserve2 = _colorNatureReserve1.Multiply(DarkerMultiplier);
                _colorNatureReserve3 = _colorNatureReserve2.Multiply(DarkerMultiplier);

                _colorTradeSchool1 = new Color32(232, 216, 172, 255);               // color taken manually from Trade School administration building roof
                _colorTradeSchool2 = _colorTradeSchool1.Multiply(DarkerMultiplier);
                _colorTradeSchool3 = _colorTradeSchool2.Multiply(DarkerMultiplier);
                _colorLiberalArts1 = new Color32(241, 181, 113, 255);               // color taken manually from Liberal Arts College administration building roof
                _colorLiberalArts2 = _colorLiberalArts1.Multiply(DarkerMultiplier);
                _colorLiberalArts3 = _colorLiberalArts2.Multiply(DarkerMultiplier);
                _colorUniversity1  = new Color32(172, 208, 203, 255);               // color taken manually from University administration building roof
                _colorUniversity2  = _colorUniversity1.Multiply(DarkerMultiplier);
                _colorUniversity3  = _colorUniversity2.Multiply(DarkerMultiplier);

                _colorTollBooth1 = new Color32(183, 148, 205, 255);                 // color taken manually from Road toolbar Toll Booth icon car
                _colorTollBooth2 = _colorTollBooth1.Multiply(DarkerMultiplier);
                _colorTollBooth3 = _colorTollBooth2.Multiply(DarkerMultiplier);

                _colorSpaceElevator2 = new Color32(087, 254, 255, 255);             // color taken manually from Space Elevator rings; Space Elevator has no income, so no color1
                _colorSpaceElevator3 = _colorSpaceElevator2.Multiply(DarkerMultiplier);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }

        /// <summary>
        /// set the text color and line color for this statistic
        /// </summary>
        private void SetTextLineColor()
        {
            // colors cannot be set in the constructor because many of the colors are obtained from the game's UI elements and
            // at the time the statistic constructor is called some of the game's UI elements may not yet be initialized

            // huge switch statement, one case for each statistic type to set the text color
            switch (_type)
            {
                case StatisticType.ElectricityConsumptionPercent:                   _textColor = _colorElectricity1;                break;
                case StatisticType.ElectricityConsumption:                          _textColor = _colorElectricity1;                break;
                case StatisticType.ElectricityProduction:                           _textColor = _colorElectricity2;                break;

                case StatisticType.WaterConsumptionPercent:                         _textColor = _colorWater1;                      break;
                case StatisticType.WaterConsumption:                                _textColor = _colorWater1;                      break;
                case StatisticType.WaterPumpingCapacity:                            _textColor = _colorWater2;                      break;

                case StatisticType.WaterTankReservedPercent:                        _textColor = _colorWaterTank1;                  break;
                case StatisticType.WaterTankReserved:                               _textColor = _colorWaterTank1;                  break;
                case StatisticType.WaterTankStorageCapacity:                        _textColor = _colorWaterTank2;                  break;

                case StatisticType.SewageProductionPercent:                         _textColor = _colorSewage1;                     break;
                case StatisticType.SewageProduction:                                _textColor = _colorSewage1;                     break;
                case StatisticType.SewageDrainingCapacity:                          _textColor = _colorSewage2;                     break;

                case StatisticType.LandfillStoragePercent:                          _textColor = _colorLandfill1;                   break;
                case StatisticType.LandfillStorage:                                 _textColor = _colorLandfill1;                   break;
                case StatisticType.LandfillCapacity:                                _textColor = _colorLandfill2;                   break;

                case StatisticType.GarbageProductionPercent:                        _textColor = _colorGarbage1;                    break;
                case StatisticType.GarbageProduction:                               _textColor = _colorGarbage1;                    break;
                case StatisticType.GarbageProcessingCapacity:                       _textColor = _colorGarbage2;                    break;

                case StatisticType.EducationElementaryEligiblePercent:              _textColor = _colorUneducated1;                 break;
                case StatisticType.EducationElementaryEligible:                     _textColor = _colorUneducated1;                 break;
                case StatisticType.EducationElementaryCapacity:                     _textColor = _colorUneducated2;                 break;
                case StatisticType.EducationHighSchoolEligiblePercent:              _textColor = _colorEducated1;                   break;
                case StatisticType.EducationHighSchoolEligible:                     _textColor = _colorEducated1;                   break;
                case StatisticType.EducationHighSchoolCapacity:                     _textColor = _colorEducated2;                   break;
                case StatisticType.EducationUniversityEligiblePercent:              _textColor = _colorWellEducated1;               break;
                case StatisticType.EducationUniversityEligible:                     _textColor = _colorWellEducated1;               break;
                case StatisticType.EducationUniversityCapacity:                     _textColor = _colorWellEducated2;               break;
                case StatisticType.EducationLibraryUsersPercent:                    _textColor = _colorHighlyEducated1;             break;
                case StatisticType.EducationLibraryUsers:                           _textColor = _colorHighlyEducated1;             break;
                case StatisticType.EducationLibraryCapacity:                        _textColor = _colorHighlyEducated2;             break;
                case StatisticType.EducationLevelUneducatedPercent:                 _textColor = _colorUneducated1;                 break;
                case StatisticType.EducationLevelEducatedPercent:                   _textColor = _colorEducated1;                   break;
                case StatisticType.EducationLevelWellEducatedPercent:               _textColor = _colorWellEducated1;               break;
                case StatisticType.EducationLevelHighlyEducatedPercent:             _textColor = _colorHighlyEducated1;             break;
                case StatisticType.EducationLevelUneducated:                        _textColor = _colorUneducated1;                 break;
                case StatisticType.EducationLevelEducated:                          _textColor = _colorEducated1;                   break;
                case StatisticType.EducationLevelWellEducated:                      _textColor = _colorWellEducated1;               break;
                case StatisticType.EducationLevelHighlyEducated:                    _textColor = _colorHighlyEducated1;             break;

                case StatisticType.HappinessGlobal:                                 _textColor = _colorStatisticsHappiness;         break;
                case StatisticType.HappinessResidential:                            _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.HappinessCommercial:                             _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.HappinessIndustrial:                             _textColor = _colorZoneIndustrial;              break;
                case StatisticType.HappinessOffice:                                 _textColor = _colorZoneOffice;                  break;

                case StatisticType.HealthcareAverageHealth:                         _textColor = _colorHealthcareAverage;           break;
                case StatisticType.HealthcareSickPercent:                           _textColor = _colorHealthcare1;                 break;
                case StatisticType.HealthcareSick:                                  _textColor = _colorHealthcare1;                 break;
                case StatisticType.HealthcareHealCapacity:                          _textColor = _colorHealthcare2;                 break;

                case StatisticType.DeathcareCemeteryBuriedPercent:                  _textColor = _colorDeathcare1;                  break;
                case StatisticType.DeathcareCemeteryBuried:                         _textColor = _colorDeathcare1;                  break;
                case StatisticType.DeathcareCemeteryCapacity:                       _textColor = _colorDeathcare2;                  break;
                case StatisticType.DeathcareCrematoriumDeceasedPercent:             _textColor = _colorDeathcare3;                  break;
                case StatisticType.DeathcareCrematoriumDeceased:                    _textColor = _colorDeathcare3;                  break;
                case StatisticType.DeathcareCrematoriumCapacity:                    _textColor = _colorDeathcare4;                  break;
                case StatisticType.DeathcareDeathRate:                              _textColor = _colorStatisticsDeathRate;         break;

                case StatisticType.ChildcareAverageHealth:                          _textColor = _colorHealthcareAverage;           break;
                case StatisticType.ChildcareSickPercent:                            _textColor = _colorChildcare1;                  break;
                case StatisticType.ChildcareSick:                                   _textColor = _colorChildcare1;                  break;
                case StatisticType.ChildcarePopulation:                             _textColor = _colorChildcare2;                  break;
                case StatisticType.ChildcareBirthRate:                              _textColor = _colorStatisticsBirthRate;         break;

                case StatisticType.EldercareAverageHealth:                          _textColor = _colorHealthcareAverage;           break;
                case StatisticType.EldercareSickPercent:                            _textColor = _colorEldercare1;                  break;
                case StatisticType.EldercareSick:                                   _textColor = _colorEldercare1;                  break;
                case StatisticType.EldercarePopulation:                             _textColor = _colorEldercare2;                  break;
                case StatisticType.EldercareAverageLifeSpan:                        _textColor = _colorStatisticsDeathRate;         break;

                case StatisticType.ZoningResidentialPercent:                        _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoningCommercialPercent:                         _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoningIndustrialPercent:                         _textColor = _colorZoneIndustrial;              break;
                case StatisticType.ZoningOfficePercent:                             _textColor = _colorZoneOffice;                  break;
                case StatisticType.ZoningUnzonedPercent:                            _textColor = _colorZoneUnzoned;                 break;
                case StatisticType.ZoningTotal:                                     _textColor = _colorNeutral1;                    break;
                case StatisticType.ZoningResidential:                               _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoningCommercial:                                _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoningIndustrial:                                _textColor = _colorZoneIndustrial;              break;
                case StatisticType.ZoningOffice:                                    _textColor = _colorZoneOffice;                  break;
                case StatisticType.ZoningUnzoned:                                   _textColor = _colorZoneUnzoned;                 break;

                case StatisticType.ZoneLevelResidentialAverage:                     _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoneLevelResidential1:                           _textColor = _colorResidentialLevel1;           break;
                case StatisticType.ZoneLevelResidential2:                           _textColor = _colorResidentialLevel2;           break;
                case StatisticType.ZoneLevelResidential3:                           _textColor = _colorResidentialLevel3;           break;
                case StatisticType.ZoneLevelResidential4:                           _textColor = _colorResidentialLevel4;           break;
                case StatisticType.ZoneLevelResidential5:                           _textColor = _colorResidentialLevel5;           break;
                case StatisticType.ZoneLevelCommercialAverage:                      _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoneLevelCommercial1:                            _textColor = _colorCommercialLevel1;            break;
                case StatisticType.ZoneLevelCommercial2:                            _textColor = _colorCommercialLevel2;            break;
                case StatisticType.ZoneLevelCommercial3:                            _textColor = _colorCommercialLevel3;            break;
                case StatisticType.ZoneLevelIndustrialAverage:                      _textColor = _colorZoneIndustrial;              break;
                case StatisticType.ZoneLevelIndustrial1:                            _textColor = _colorIndustrialLevel1;            break;
                case StatisticType.ZoneLevelIndustrial2:                            _textColor = _colorIndustrialLevel2;            break;
                case StatisticType.ZoneLevelIndustrial3:                            _textColor = _colorIndustrialLevel3;            break;
                case StatisticType.ZoneLevelOfficeAverage:                          _textColor = _colorZoneOffice;                  break;
                case StatisticType.ZoneLevelOffice1:                                _textColor = _colorOfficeLevel1;                break;
                case StatisticType.ZoneLevelOffice2:                                _textColor = _colorOfficeLevel2;                break;
                case StatisticType.ZoneLevelOffice3:                                _textColor = _colorOfficeLevel3;                break;

                case StatisticType.ZoneBuildingsResidentialPercent:                 _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoneBuildingsCommercialPercent:                  _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoneBuildingsIndustrialPercent:                  _textColor = _colorZoneIndustrial;              break;
                case StatisticType.ZoneBuildingsOfficePercent:                      _textColor = _colorZoneOffice;                  break;
                case StatisticType.ZoneBuildingsTotal:                              _textColor = _colorNeutral1;                    break;
                case StatisticType.ZoneBuildingsResidential:                        _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoneBuildingsCommercial:                         _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoneBuildingsIndustrial:                         _textColor = _colorZoneIndustrial;              break;
                case StatisticType.ZoneBuildingsOffice:                             _textColor = _colorZoneOffice;                  break;

                case StatisticType.ZoneDemandResidential:                           _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ZoneDemandCommercial:                            _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.ZoneDemandIndustrialOffice:                      _textColor = _colorZoneIndustrial;              break;

                case StatisticType.TrafficAverageFlow:                              _textColor = _colorInfoTrafficTarget;           break;
                case StatisticType.TrafficPedestriansPercent:                       _textColor = _colorRoutePedestrian1;            break;
                case StatisticType.TrafficCyclistsPercent:                          _textColor = _colorRouteCyclist1;               break;
                case StatisticType.TrafficPrivateVehiclesPercent:                   _textColor = _colorRoutePrivateCar1;            break;
                case StatisticType.TrafficPublicTransportCargoPercent:              _textColor = _colorRoutePublicTransport1;       break;
                case StatisticType.TrafficTrucksPercent:                            _textColor = _colorRouteCargoTruck1;            break;
                case StatisticType.TrafficCityServiceVehiclesPercent:               _textColor = _colorRouteServiceVehicle1;        break;
                case StatisticType.TrafficDummyTrafficPercent:                      _textColor = _colorRouteDummy1;                 break;
                case StatisticType.TrafficTotalCount:                               _textColor = _colorRouteTotal;                  break;
                case StatisticType.TrafficPedestriansCount:                         _textColor = _colorRoutePedestrian2;            break;
                case StatisticType.TrafficCyclistsCount:                            _textColor = _colorRouteCyclist2;               break;
                case StatisticType.TrafficPrivateVehiclesCount:                     _textColor = _colorRoutePrivateCar2;            break;
                case StatisticType.TrafficPublicTransportCargoCount:                _textColor = _colorRoutePublicTransport2;       break;
                case StatisticType.TrafficTrucksCount:                              _textColor = _colorRouteCargoTruck2;            break;
                case StatisticType.TrafficCityServiceVehiclesCount:                 _textColor = _colorRouteServiceVehicle2;        break;
                case StatisticType.TrafficDummyTrafficCount:                        _textColor = _colorRouteDummy2;                 break;

                case StatisticType.PollutionAverageGround:                          _textColor = _colorInfoPollution;               break;
                case StatisticType.PollutionAverageDrinkingWater:                   _textColor = _colorInfoPollution;               break;
                case StatisticType.PollutionAverageNoise:                           _textColor = _colorInfoNoisePollution;          break;

                case StatisticType.FireSafetyHazard:                                _textColor = _colorFireSafety;                  break;

                case StatisticType.CrimeRate:                                       _textColor = _colorCrimeRate;                   break;
                case StatisticType.CrimeDetainedCriminalsPercent:                   _textColor = _colorCrime1;                      break;
                case StatisticType.CrimeDetainedCriminals:                          _textColor = _colorCrime1;                      break;
                case StatisticType.CrimeJailsCapacity:                              _textColor = _colorCrime2;                      break;

                case StatisticType.CommercialCashAccumulatedPercent:                _textColor = _colorCash1;                       break;
                case StatisticType.CommercialCashAccumulated:                       _textColor = _colorCash1;                       break;
                case StatisticType.CommercialCashCapacity:                          _textColor = _colorCash2;                       break;
                case StatisticType.CommercialCashCollected:                         _textColor = _colorCash3;                       break;

                case StatisticType.PublicTransportationTotalTotal:                  _textColor = _colorTransportTotal1;             break;
                case StatisticType.PublicTransportationTotalResidents:              _textColor = _colorTransportTotal1;             break;
                case StatisticType.PublicTransportationTotalTourists:               _textColor = _colorTransportTotal2;             break;
                case StatisticType.PublicTransportationBusTotal:                    _textColor = _colorTransportBus1;               break;
                case StatisticType.PublicTransportationBusResidents:                _textColor = _colorTransportBus1;               break;
                case StatisticType.PublicTransportationBusTourists:                 _textColor = _colorTransportBus2;               break;
                case StatisticType.PublicTransportationTrolleybusTotal:             _textColor = _colorTransportTrolleybus1;        break;
                case StatisticType.PublicTransportationTrolleybusResidents:         _textColor = _colorTransportTrolleybus1;        break;
                case StatisticType.PublicTransportationTrolleybusTourists:          _textColor = _colorTransportTrolleybus2;        break;
                case StatisticType.PublicTransportationTramTotal:                   _textColor = _colorTransportTram1;              break;
                case StatisticType.PublicTransportationTramResidents:               _textColor = _colorTransportTram1;              break;
                case StatisticType.PublicTransportationTramTourists:                _textColor = _colorTransportTram2;              break;
                case StatisticType.PublicTransportationMetroTotal:                  _textColor = _colorTransportMetro1;             break;
                case StatisticType.PublicTransportationMetroResidents:              _textColor = _colorTransportMetro1;             break;
                case StatisticType.PublicTransportationMetroTourists:               _textColor = _colorTransportMetro2;             break;
                case StatisticType.PublicTransportationTrainTotal:                  _textColor = _colorTransportTrain1;             break;
                case StatisticType.PublicTransportationTrainResidents:              _textColor = _colorTransportTrain1;             break;
                case StatisticType.PublicTransportationTrainTourists:               _textColor = _colorTransportTrain2;             break;
                case StatisticType.PublicTransportationShipTotal:                   _textColor = _colorTransportShip1;              break;
                case StatisticType.PublicTransportationShipResidents:               _textColor = _colorTransportShip1;              break;
                case StatisticType.PublicTransportationShipTourists:                _textColor = _colorTransportShip2;              break;
                case StatisticType.PublicTransportationAirTotal:                    _textColor = _colorTransportAir1;               break;
                case StatisticType.PublicTransportationAirResidents:                _textColor = _colorTransportAir1;               break;
                case StatisticType.PublicTransportationAirTourists:                 _textColor = _colorTransportAir2;               break;
                case StatisticType.PublicTransportationMonorailTotal:               _textColor = _colorTransportMonorail1;          break;
                case StatisticType.PublicTransportationMonorailResidents:           _textColor = _colorTransportMonorail1;          break;
                case StatisticType.PublicTransportationMonorailTourists:            _textColor = _colorTransportMonorail2;          break;
                case StatisticType.PublicTransportationCableCarTotal:               _textColor = _colorTransportCableCar1;          break;
                case StatisticType.PublicTransportationCableCarResidents:           _textColor = _colorTransportCableCar1;          break;
                case StatisticType.PublicTransportationCableCarTourists:            _textColor = _colorTransportCableCar2;          break;
                case StatisticType.PublicTransportationTaxiTotal:                   _textColor = _colorTransportTaxi1;              break;
                case StatisticType.PublicTransportationTaxiResidents:               _textColor = _colorTransportTaxi1;              break;
                case StatisticType.PublicTransportationTaxiTourists:                _textColor = _colorTransportTaxi2;              break;

                case StatisticType.IntercityTravelArrivingTotal:                    _textColor = _colorArriving1;                   break;
                case StatisticType.IntercityTravelArrivingResidents:                _textColor = _colorArriving2;                   break;
                case StatisticType.IntercityTravelArrivingTourists:                 _textColor = _colorArriving3;                   break;
                case StatisticType.IntercityTravelDepartingTotal:                   _textColor = _colorDeparting1;                  break;
                case StatisticType.IntercityTravelDepartingResidents:               _textColor = _colorDeparting2;                  break;
                case StatisticType.IntercityTravelDepartingTourists:                _textColor = _colorDeparting3;                  break;
                case StatisticType.IntercityTravelDummyTrafficTotal:                _textColor = _colorRouteDummy1;                 break;
                case StatisticType.IntercityTravelDummyTrafficResidents:            _textColor = _colorRouteDummy2;                 break;
                case StatisticType.IntercityTravelDummyTrafficTourists:             _textColor = _colorRouteDummy3;                 break;

                case StatisticType.PopulationTotal:                                 _textColor = _colorStatisticsPopulation;        break;
                case StatisticType.PopulationChildrenPercent:                       _textColor = _colorChild;                       break;
                case StatisticType.PopulationTeensPercent:                          _textColor = _colorTeen;                        break;
                case StatisticType.PopulationYoungAdultsPercent:                    _textColor = _colorYoung;                       break;
                case StatisticType.PopulationAdultsPercent:                         _textColor = _colorAdult;                       break;
                case StatisticType.PopulationSeniorsPercent:                        _textColor = _colorSenior;                      break;
                case StatisticType.PopulationChildren:                              _textColor = _colorChild;                       break;
                case StatisticType.PopulationTeens:                                 _textColor = _colorTeen;                        break;
                case StatisticType.PopulationYoungAdults:                           _textColor = _colorYoung;                       break;
                case StatisticType.PopulationAdults:                                _textColor = _colorAdult;                       break;
                case StatisticType.PopulationSeniors:                               _textColor = _colorSenior;                      break;

                case StatisticType.HouseholdsOccupiedPercent:                       _textColor = _colorHouseholds1;                 break;
                case StatisticType.HouseholdsOccupied:                              _textColor = _colorHouseholds1;                 break;
                case StatisticType.HouseholdsAvailable:                             _textColor = _colorHouseholds2;                 break;

                case StatisticType.EmploymentPeopleEmployed:                        _textColor = _colorEmployment1;                 break;
                case StatisticType.EmploymentJobsAvailable:                         _textColor = _colorEmployment2;                 break;
                case StatisticType.EmploymentUnfilledJobs:                          _textColor = _colorEmployment3;                 break;
                case StatisticType.EmploymentUnemploymentPercent:                   _textColor = _colorUnemployment1;               break;
                case StatisticType.EmploymentUnemployed:                            _textColor = _colorUnemployment1;               break;
                case StatisticType.EmploymentEligibleWorkers:                       _textColor = _colorUnemployment2;               break;

                case StatisticType.OutsideConnectionsImportTotal:                   _textColor = _colorTransferTotal1;              break;
                case StatisticType.OutsideConnectionsImportGoods:                   _textColor = _colorTransferGoods1;              break;
                case StatisticType.OutsideConnectionsImportForestry:                _textColor = _colorTransferForestry1;           break;
                case StatisticType.OutsideConnectionsImportFarming:                 _textColor = _colorTransferFarming1;            break;
                case StatisticType.OutsideConnectionsImportOre:                     _textColor = _colorTransferOre1;                break;
                case StatisticType.OutsideConnectionsImportOil:                     _textColor = _colorTransferOil1;                break;
                case StatisticType.OutsideConnectionsImportMail:                    _textColor = _colorTransferMail1;               break;
                case StatisticType.OutsideConnectionsExportTotal:                   _textColor = _colorTransferTotal2;              break;
                case StatisticType.OutsideConnectionsExportGoods:                   _textColor = _colorTransferGoods2;              break;
                case StatisticType.OutsideConnectionsExportForestry:                _textColor = _colorTransferForestry2;           break;
                case StatisticType.OutsideConnectionsExportFarming:                 _textColor = _colorTransferFarming2;            break;
                case StatisticType.OutsideConnectionsExportOre:                     _textColor = _colorTransferOre2;                break;
                case StatisticType.OutsideConnectionsExportOil:                     _textColor = _colorTransferOil2;                break;
                case StatisticType.OutsideConnectionsExportMail:                    _textColor = _colorTransferMail2;               break;
                case StatisticType.OutsideConnectionsExportFish:                    _textColor = _colorTransferFish2;               break;

                case StatisticType.LandValueAverage:                                _textColor = _colorInfoLandValue;               break;

                case StatisticType.NaturalResourcesForestUsedPercent:               _textColor = _colorResourceForestry1;           break;
                case StatisticType.NaturalResourcesForestUsed:                      _textColor = _colorResourceForestry1;           break;
                case StatisticType.NaturalResourcesForestAvailable:                 _textColor = _colorResourceForestry2;           break;
                case StatisticType.NaturalResourcesFertileLandUsedPercent:          _textColor = _colorResourceFertility1;          break;
                case StatisticType.NaturalResourcesFertileLandUsed:                 _textColor = _colorResourceFertility1;          break;
                case StatisticType.NaturalResourcesFertileLandAvailable:            _textColor = _colorResourceFertility2;          break;
                case StatisticType.NaturalResourcesOreUsedPercent:                  _textColor = _colorResourceOre1;                break;
                case StatisticType.NaturalResourcesOreUsed:                         _textColor = _colorResourceOre1;                break;
                case StatisticType.NaturalResourcesOreAvailable:                    _textColor = _colorResourceOre2;                break;
                case StatisticType.NaturalResourcesOilUsedPercent:                  _textColor = _colorResourceOil1;                break;
                case StatisticType.NaturalResourcesOilUsed:                         _textColor = _colorResourceOil1;                break;
                case StatisticType.NaturalResourcesOilAvailable:                    _textColor = _colorResourceOil2;                break;

                case StatisticType.HeatingConsumptionPercent:                       _textColor = _colorInfoHeating1;                break;
                case StatisticType.HeatingConsumption:                              _textColor = _colorInfoHeating1;                break;
                case StatisticType.HeatingProduction:                               _textColor = _colorInfoHeating2;                break;

                case StatisticType.TourismCityAttractiveness:                       _textColor = _colorInfoTourism;                 break;
                case StatisticType.TourismLowWealthPercent:                         _textColor = _colorTouristsLowWealth1;          break;
                case StatisticType.TourismMediumWealthPercent:                      _textColor = _colorTouristsMediumWealth1;       break;
                case StatisticType.TourismHighWealthPercent:                        _textColor = _colorTouristsHighWealth1;         break;
                case StatisticType.TourismTotal:                                    _textColor = _colorTouristsTotal;               break;
                case StatisticType.TourismLowWealth:                                _textColor = _colorTouristsLowWealth2;          break;
                case StatisticType.TourismMediumWealth:                             _textColor = _colorTouristsMediumWealth2;       break;
                case StatisticType.TourismHighWealth:                               _textColor = _colorTouristsHighWealth2;         break;
                case StatisticType.TourismExchangeStudentBonus:                     _textColor = _colorExchangeStudent;             break;

                case StatisticType.ToursTotalTotal:                                 _textColor = _colorTransportTotal1;             break;
                case StatisticType.ToursTotalResidents:                             _textColor = _colorTransportTotal1;             break;
                case StatisticType.ToursTotalTourists:                              _textColor = _colorTransportTotal2;             break;
                case StatisticType.ToursWalkingTourTotal:                           _textColor = _colorTransportPedestrian1;        break;
                case StatisticType.ToursWalkingTourResidents:                       _textColor = _colorTransportPedestrian1;        break;
                case StatisticType.ToursWalkingTourTourists:                        _textColor = _colorTransportPedestrian2;        break;
                case StatisticType.ToursSightseeingTotal:                           _textColor = _colorTransportTouristBus1;        break;
                case StatisticType.ToursSightseeingResidents:                       _textColor = _colorTransportTouristBus1;        break;
                case StatisticType.ToursSightseeingTourists:                        _textColor = _colorTransportTouristBus2;        break;
                case StatisticType.ToursBalloonTotal:                               _textColor = _colorTransportHotAirBalloon1;     break;
                case StatisticType.ToursBalloonResidents:                           _textColor = _colorTransportHotAirBalloon1;     break;
                case StatisticType.ToursBalloonToursits:                            _textColor = _colorTransportHotAirBalloon2;     break;

                case StatisticType.TaxRateResidentialLow:                           _textColor = _colorZoneResidentialLow;          break;
                case StatisticType.TaxRateResidentialHigh:                          _textColor = _colorZoneResidentialHigh;         break;
                case StatisticType.TaxRateCommercialLow:                            _textColor = _colorZoneCommercialLow;           break;
                case StatisticType.TaxRateCommercialHigh:                           _textColor = _colorZoneCommercialHigh;          break;
                case StatisticType.TaxRateIndustrial:                               _textColor = _colorZoneIndustrial;              break;
                case StatisticType.TaxRateOffice:                                   _textColor = _colorZoneOffice;                  break;

                case StatisticType.InvestmentsChirpAir:                             _textColor = _colorTransportAir1;               break;
                case StatisticType.InvestmentsChirperCrypto:                        _textColor = _colorChirper;                     break;
                case StatisticType.InvestmentsDeathcareServiceFund:                 _textColor = _colorDeathcare1;                  break;
                case StatisticType.InvestmentsFarmingIndustry:                      _textColor = _colorTransferFarming1;            break;
                case StatisticType.InvestmentsForestryIndustry:                     _textColor = _colorTransferForestry1;           break;
                case StatisticType.InvestmentsGreasyGasoline:                       _textColor = _colorRouteServiceVehicle1;        break;
                case StatisticType.InvestmentsGenericIndustry:                      _textColor = _colorZoneIndustrial;              break;
                case StatisticType.InvestmentsHealthcareServiceFund:                _textColor = _colorHealthcare1;                 break;
                case StatisticType.InvestmentsOilIndustry:                          _textColor = _colorTransferOil1;                break;
                case StatisticType.InvestmentsOreIndustry:                          _textColor = _colorTransferOre1;                break;
                case StatisticType.InvestmentsChirpyCruises:                        _textColor = _colorTransportShip1;              break;
                case StatisticType.InvestmentsTrafficJellyLogistics:                _textColor = _colorInfoTrafficTarget;           break;
                case StatisticType.InvestmentsVeryLegitCompany:                     _textColor = _colorCrime1;                      break;
                case StatisticType.InvestmentsGainsLastMonth:                       _textColor = _colorInvestmentGain1;             break;
                case StatisticType.InvestmentsTotalGains:                           _textColor = _colorInvestmentGain2;             break;

                case StatisticType.CityEconomyTotalIncome:                          _textColor = _colorCityTotalIncome;             break;
                case StatisticType.CityEconomyTotalExpenses:                        _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.CityEconomyTotalProfit:                          _textColor = _colorCityTotalProfit;             break;
                case StatisticType.CityEconomyBankBalance:                          _textColor = _colorBankBalance;                 break;
                case StatisticType.CityEconomyLoanBalance:                          _textColor = _colorEconomy;                     break;
                case StatisticType.CityEconomyCityValue:                            _textColor = _colorStatisticsCityValue;         break;
                case StatisticType.CityEconomyCityValuePerCapita:                   _textColor = _colorStatisticsCityValue;         break;
                case StatisticType.CityEconomyGrossDomesticProduct:                 _textColor = _colorStatisticsCityBudget;        break;
                case StatisticType.CityEconomyGrossDomesticProductPerCapita:        _textColor = _colorStatisticsCityBudget;        break;
                case StatisticType.CityEconomyConsumption:                          _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.CityEconomyConsumptionPercent:                   _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.CityEconomyGovernmentSpending:                   _textColor = _colorStatisticsDeathRate;         break;
                case StatisticType.CityEconomyGovernmentSpendingPercent:            _textColor = _colorStatisticsDeathRate;         break;
                case StatisticType.CityEconomyExports:                              _textColor = _colorTransferTotal2;              break;
                case StatisticType.CityEconomyImports:                              _textColor = _colorTransferTotal1;              break;
                case StatisticType.CityEconomyNetExports:                           _textColor = _colorTransferTotal3;              break;
                case StatisticType.CityEconomyNetExportsPercent:                    _textColor = _colorTransferTotal3;              break;

                case StatisticType.ResidentialIncomeTotalPercent:                   _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ResidentialIncomeTotal:                          _textColor = _colorZoneResidentialMid;          break;
                case StatisticType.ResidentialIncomeLowDensityTotal:                _textColor = _colorZoneResidentialLow;          break;
                case StatisticType.ResidentialIncomeLowDensity1:                    _textColor = _colorResidentialLevel1;           break;
                case StatisticType.ResidentialIncomeLowDensity2:                    _textColor = _colorResidentialLevel2;           break;
                case StatisticType.ResidentialIncomeLowDensity3:                    _textColor = _colorResidentialLevel3;           break;
                case StatisticType.ResidentialIncomeLowDensity4:                    _textColor = _colorResidentialLevel4;           break;
                case StatisticType.ResidentialIncomeLowDensity5:                    _textColor = _colorResidentialLevel5;           break;
                case StatisticType.ResidentialIncomeLowDensitySelfSufficient:       _textColor = _colorIncomeSelfSufficient;        break;
                case StatisticType.ResidentialIncomeHighDensityTotal:               _textColor = _colorZoneResidentialHigh;         break;
                case StatisticType.ResidentialIncomeHighDensity1:                   _textColor = _colorResidentialLevel1;           break;
                case StatisticType.ResidentialIncomeHighDensity2:                   _textColor = _colorResidentialLevel2;           break;
                case StatisticType.ResidentialIncomeHighDensity3:                   _textColor = _colorResidentialLevel3;           break;
                case StatisticType.ResidentialIncomeHighDensity4:                   _textColor = _colorResidentialLevel4;           break;
                case StatisticType.ResidentialIncomeHighDensity5:                   _textColor = _colorResidentialLevel5;           break;
                case StatisticType.ResidentialIncomeHighDensitySelfSufficient:      _textColor = _colorIncomeSelfSufficient;        break;
                case StatisticType.ResidentialIncomeWallToWall:                     _textColor = _colorIncomeWallToWall;            break;

                case StatisticType.CommercialIncomeTotalPercent:                    _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.CommercialIncomeTotal:                           _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.CommercialIncomeLowDensityTotal:                 _textColor = _colorZoneCommercialLow;           break;
                case StatisticType.CommercialIncomeLowDensity1:                     _textColor = _colorCommercialLevel1;            break;
                case StatisticType.CommercialIncomeLowDensity2:                     _textColor = _colorCommercialLevel2;            break;
                case StatisticType.CommercialIncomeLowDensity3:                     _textColor = _colorCommercialLevel3;            break;
                case StatisticType.CommercialIncomeHighDensityTotal:                _textColor = _colorZoneCommercialHigh;          break;
                case StatisticType.CommercialIncomeHighDensity1:                    _textColor = _colorCommercialLevel1;            break;
                case StatisticType.CommercialIncomeHighDensity2:                    _textColor = _colorCommercialLevel2;            break;
                case StatisticType.CommercialIncomeHighDensity3:                    _textColor = _colorCommercialLevel3;            break;
                case StatisticType.CommercialIncomeSpecializedTotal:                _textColor = _colorLevelCommercial;             break;
                case StatisticType.CommercialIncomeLeisure:                         _textColor = _colorIncomeLeisure;               break;
                case StatisticType.CommercialIncomeTourism:                         _textColor = _colorTourismIncome;               break;
                case StatisticType.CommercialIncomeOrganic:                         _textColor = _colorIncomeOrganic;               break;
                case StatisticType.CommercialIncomeWallToWall:                      _textColor = _colorIncomeWallToWall;            break;

                case StatisticType.IndustrialIncomeTotalPercent:                    _textColor = _colorZoneIndustrial;              break;
                case StatisticType.IndustrialIncomeTotal:                           _textColor = _colorZoneIndustrial;              break;
                case StatisticType.IndustrialIncomeGenericTotal:                    _textColor = _colorLevelIndustrial;             break;
                case StatisticType.IndustrialIncomeGeneric1:                        _textColor = _colorIndustrialLevel1;            break;
                case StatisticType.IndustrialIncomeGeneric2:                        _textColor = _colorIndustrialLevel2;            break;
                case StatisticType.IndustrialIncomeGeneric3:                        _textColor = _colorIndustrialLevel3;            break;
                case StatisticType.IndustrialIncomeSpecializedTotal:                _textColor = _colorTransferTotal1;              break;
                case StatisticType.IndustrialIncomeForestry:                        _textColor = _colorTransferForestry1;           break;
                case StatisticType.IndustrialIncomeFarming:                         _textColor = _colorTransferFarming1;            break;
                case StatisticType.IndustrialIncomeOre:                             _textColor = _colorTransferOre1;                break;
                case StatisticType.IndustrialIncomeOil:                             _textColor = _colorTransferOil1;                break;

                case StatisticType.OfficeIncomeTotalPercent:                        _textColor = _colorZoneOffice;                  break;
                case StatisticType.OfficeIncomeTotal:                               _textColor = _colorZoneOffice;                  break;
                case StatisticType.OfficeIncomeGenericTotal:                        _textColor = _colorLevelOffice;                 break;
                case StatisticType.OfficeIncomeGeneric1:                            _textColor = _colorOfficeLevel1;                break;
                case StatisticType.OfficeIncomeGeneric2:                            _textColor = _colorOfficeLevel2;                break;
                case StatisticType.OfficeIncomeGeneric3:                            _textColor = _colorOfficeLevel3;                break;
                case StatisticType.OfficeIncomeSpecializedTotal:                    _textColor = _colorIncomeOfficeSpecialized;     break;
                case StatisticType.OfficeIncomeITCluster:                           _textColor = _colorIncomeITCluster;             break;
                case StatisticType.OfficeIncomeWallToWall:                          _textColor = _colorIncomeWallToWall;            break;
                case StatisticType.OfficeIncomeFinancial:                           _textColor = _colorIncomeFinancial;             break;

                case StatisticType.TourismIncomeTotalPercent:                       _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.TourismIncomeTotal:                              _textColor = _colorZoneCommercialMid;           break;
                case StatisticType.TourismIncomeCommercialZones:                    _textColor = _colorTourismIncome;               break;
                case StatisticType.TourismIncomeTransportation:                     _textColor = _colorTransportTotal1;             break;
                case StatisticType.TourismIncomeParkAreas:                          _textColor = _colorParks;                       break;
                case StatisticType.TourismIncomeHotels:                             _textColor = _colorHotels;                      break;

                case StatisticType.ServiceExpensesTotalPercent:                     _textColor = _colorZoneOffice;                  break;
                case StatisticType.ServiceExpensesTotal:                            _textColor = _colorZoneOffice;                  break;
                case StatisticType.ServiceExpensesRoads:                            _textColor = _colorTransportTotal1;             break;
                case StatisticType.ServiceExpensesElectricity:                      _textColor = _colorElectricity1;                break;
                case StatisticType.ServiceExpensesWaterSewageHeating:               _textColor = _colorWater1;                      break;
                case StatisticType.ServiceExpensesGarbage:                          _textColor = _colorGarbage1;                    break;
                case StatisticType.ServiceExpensesHealthcare:                       _textColor = _colorHealthcare1;                 break;
                case StatisticType.ServiceExpensesFire:                             _textColor = _colorFireSafety;                  break;
                case StatisticType.ServiceExpensesEmergency:                        _textColor = _colorEmergency;                   break;
                case StatisticType.ServiceExpensesPolice:                           _textColor = _colorCrimeRate;                   break;
                case StatisticType.ServiceExpensesBanks:                            _textColor = _colorBanks;                       break;
                case StatisticType.ServiceExpensesEducation:                        _textColor = _colorEducated1;                   break;
                case StatisticType.ServiceExpensesParksPlazas:                      _textColor = _colorParks;                       break;
                case StatisticType.ServiceExpensesServicePoints:                    _textColor = _colorServicePoints;               break;
                case StatisticType.ServiceExpensesUniqueBuildings:                  _textColor = _colorUniqueBuildings;             break;
                case StatisticType.ServiceExpensesGenericSportsArenas:              _textColor = _colorGenericSportsArenas;         break;
                case StatisticType.ServiceExpensesLoans:                            _textColor = _colorEconomy;                     break;
                case StatisticType.ServiceExpensesPolicies:                         _textColor = _colorPolicies;                    break;

                case StatisticType.ParkAreasTotalIncomePercent:                     _textColor = _colorCityTotalIncome;             break;
                case StatisticType.ParkAreasTotalIncome:                            _textColor = _colorCityTotalIncome;             break;
                case StatisticType.ParkAreasTotalExpensesPercent:                   _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.ParkAreasTotalExpenses:                          _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.ParkAreasTotalProfit:                            _textColor = _colorCityTotalProfit;             break;
                case StatisticType.ParkAreasCityParkIncome:                         _textColor = _colorCityPark1;                   break;
                case StatisticType.ParkAreasCityParkExpenses:                       _textColor = _colorCityPark2;                   break;
                case StatisticType.ParkAreasCityParkProfit:                         _textColor = _colorCityPark3;                   break;
                case StatisticType.ParkAreasAmusementParkIncome:                    _textColor = _colorAmusementPark1;              break;
                case StatisticType.ParkAreasAmusementParkExpenses:                  _textColor = _colorAmusementPark2;              break;
                case StatisticType.ParkAreasAmusementParkProfit:                    _textColor = _colorAmusementPark3;              break;
                case StatisticType.ParkAreasZooIncome:                              _textColor = _colorZoo1;                        break;
                case StatisticType.ParkAreasZooExpenses:                            _textColor = _colorZoo2;                        break;
                case StatisticType.ParkAreasZooProfit:                              _textColor = _colorZoo3;                        break;
                case StatisticType.ParkAreasNatureReserveIncome:                    _textColor = _colorNatureReserve1;              break;
                case StatisticType.ParkAreasNatureReserveExpenses:                  _textColor = _colorNatureReserve2;              break;
                case StatisticType.ParkAreasNatureReserveProfit:                    _textColor = _colorNatureReserve3;              break;

                case StatisticType.IndustryAreasTotalIncomePercent:                 _textColor = _colorCityTotalIncome;             break;
                case StatisticType.IndustryAreasTotalIncome:                        _textColor = _colorCityTotalIncome;             break;
                case StatisticType.IndustryAreasTotalExpensesPercent:               _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.IndustryAreasTotalExpenses:                      _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.IndustryAreasTotalProfit:                        _textColor = _colorCityTotalProfit;             break;
                case StatisticType.IndustryAreasForestryIncome:                     _textColor = _colorTransferForestry1;           break;
                case StatisticType.IndustryAreasForestryExpenses:                   _textColor = _colorTransferForestry2;           break;
                case StatisticType.IndustryAreasForestryProfit:                     _textColor = _colorTransferForestry3;           break;
                case StatisticType.IndustryAreasFarmingIncome:                      _textColor = _colorTransferFarming1;            break;
                case StatisticType.IndustryAreasFarmingExpenses:                    _textColor = _colorTransferFarming2;            break;
                case StatisticType.IndustryAreasFarmingProfit:                      _textColor = _colorTransferFarming3;            break;
                case StatisticType.IndustryAreasOreIncome:                          _textColor = _colorTransferOre1;                break;
                case StatisticType.IndustryAreasOreExpenses:                        _textColor = _colorTransferOre2;                break;
                case StatisticType.IndustryAreasOreProfit:                          _textColor = _colorTransferOre3;                break;
                case StatisticType.IndustryAreasOilIncome:                          _textColor = _colorTransferOil1;                break;
                case StatisticType.IndustryAreasOilExpenses:                        _textColor = _colorTransferOil2;                break;
                case StatisticType.IndustryAreasOilProfit:                          _textColor = _colorTransferOil3;                break;
                case StatisticType.IndustryAreasWarehousesFactoriesIncome:          _textColor = _colorTransferMail1;               break;
                case StatisticType.IndustryAreasWarehousesFactoriesExpenses:        _textColor = _colorTransferMail2;               break;
                case StatisticType.IndustryAreasWarehousesFactoriesProfit:          _textColor = _colorTransferMail3;               break;

                case StatisticType.FishingIndustryFishingIncome:                    _textColor = _colorTransferFish1;               break;
                case StatisticType.FishingIndustryFishingExpenses:                  _textColor = _colorTransferFish2;               break;
                case StatisticType.FishingIndustryFishingProfit:                    _textColor = _colorTransferFish3;               break;

                case StatisticType.CampusAreasTotalIncomePercent:                   _textColor = _colorCityTotalIncome;             break;
                case StatisticType.CampusAreasTotalIncome:                          _textColor = _colorCityTotalIncome;             break;
                case StatisticType.CampusAreasTotalExpensesPercent:                 _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.CampusAreasTotalExpenses:                        _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.CampusAreasTotalProfit:                          _textColor = _colorCityTotalProfit;             break;
                case StatisticType.CampusAreasTradeSchoolIncome:                    _textColor = _colorTradeSchool1;                break;
                case StatisticType.CampusAreasTradeSchoolExpenses:                  _textColor = _colorTradeSchool2;                break;
                case StatisticType.CampusAreasTradeSchoolProfit:                    _textColor = _colorTradeSchool3;                break;
                case StatisticType.CampusAreasLiberalArtsCollegeIncome:             _textColor = _colorLiberalArts1;                break;
                case StatisticType.CampusAreasLiberalArtsCollegeExpenses:           _textColor = _colorLiberalArts2;                break;
                case StatisticType.CampusAreasLiberalArtsCollegeProfit:             _textColor = _colorLiberalArts3;                break;
                case StatisticType.CampusAreasUniversityIncome:                     _textColor = _colorUniversity1;                 break;
                case StatisticType.CampusAreasUniversityExpenses:                   _textColor = _colorUniversity2;                 break;
                case StatisticType.CampusAreasUniversityProfit:                     _textColor = _colorUniversity3;                 break;

                case StatisticType.HotelsTotalIncomePercent:                        _textColor = _colorCityTotalIncome;             break;
                case StatisticType.HotelsTotalIncome:                               _textColor = _colorCityTotalIncome;             break;
                case StatisticType.HotelsTotalExpensesPercent:                      _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.HotelsTotalExpenses:                             _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.HotelsTotalProfit:                               _textColor = _colorCityTotalProfit;             break;
                case StatisticType.HotelsTotalPopularity:                           _textColor = _colorNeutral1;                    break;
                case StatisticType.HotelsSightseeingPopularity:                     _textColor = _colorHotelSightseeing;            break;
                case StatisticType.HotelsShoppingPopularity:                        _textColor = _colorHotelShopping;               break;
                case StatisticType.HotelsBusinessPopularity:                        _textColor = _colorHotelBusiness;               break;
                case StatisticType.HotelsNaturePopularity:                          _textColor = _colorHotelNature;                 break;
                case StatisticType.HotelsGuestsVisitingPercent:                     _textColor = _colorHotelGuests1;                break;
                case StatisticType.HotelsGuestsVisiting:                            _textColor = _colorHotelGuests1;                break;
                case StatisticType.HotelsGuestsCapacity:                            _textColor = _colorHotelGuests2;                break;

                case StatisticType.TransportEconomyTotalIncomePercent:              _textColor = _colorCityTotalIncome;             break;
                case StatisticType.TransportEconomyTotalIncome:                     _textColor = _colorCityTotalIncome;             break;
                case StatisticType.TransportEconomyTotalExpensesPercent:            _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.TransportEconomyTotalExpenses:                   _textColor = _colorCityTotalExpenses;           break;
                case StatisticType.TransportEconomyTotalProfit:                     _textColor = _colorCityTotalProfit;             break;
                case StatisticType.TransportEconomyBusIncome:                       _textColor = _colorTransportBus1;               break;
                case StatisticType.TransportEconomyBusExpenses:                     _textColor = _colorTransportBus2;               break;
                case StatisticType.TransportEconomyBusProfit:                       _textColor = _colorTransportBus3;               break;
                case StatisticType.TransportEconomyTrolleybusIncome:                _textColor = _colorTransportTrolleybus1;        break;
                case StatisticType.TransportEconomyTrolleybusExpenses:              _textColor = _colorTransportTrolleybus2;        break;
                case StatisticType.TransportEconomyTrolleybusProfit:                _textColor = _colorTransportTrolleybus3;        break;
                case StatisticType.TransportEconomyTramIncome:                      _textColor = _colorTransportTram1;              break;
                case StatisticType.TransportEconomyTramExpenses:                    _textColor = _colorTransportTram2;              break;
                case StatisticType.TransportEconomyTramProfit:                      _textColor = _colorTransportTram3;              break;
                case StatisticType.TransportEconomyMetroIncome:                     _textColor = _colorTransportMetro1;             break;
                case StatisticType.TransportEconomyMetroExpenses:                   _textColor = _colorTransportMetro2;             break;
                case StatisticType.TransportEconomyMetroProfit:                     _textColor = _colorTransportMetro3;             break;
                case StatisticType.TransportEconomyTrainIncome:                     _textColor = _colorTransportTrain1;             break;
                case StatisticType.TransportEconomyTrainExpenses:                   _textColor = _colorTransportTrain2;             break;
                case StatisticType.TransportEconomyTrainProfit:                     _textColor = _colorTransportTrain3;             break;
                case StatisticType.TransportEconomyShipIncome:                      _textColor = _colorTransportShip1;              break;
                case StatisticType.TransportEconomyShipExpenses:                    _textColor = _colorTransportShip2;              break;
                case StatisticType.TransportEconomyShipProfit:                      _textColor = _colorTransportShip3;              break;
                case StatisticType.TransportEconomyAirIncome:                       _textColor = _colorTransportAir1;               break;
                case StatisticType.TransportEconomyAirExpenses:                     _textColor = _colorTransportAir2;               break;
                case StatisticType.TransportEconomyAirProfit:                       _textColor = _colorTransportAir3;               break;
                case StatisticType.TransportEconomyMonorailIncome:                  _textColor = _colorTransportMonorail1;          break;
                case StatisticType.TransportEconomyMonorailExpenses:                _textColor = _colorTransportMonorail2;          break;
                case StatisticType.TransportEconomyMonorailProfit:                  _textColor = _colorTransportMonorail3;          break;
                case StatisticType.TransportEconomyCableCarIncome:                  _textColor = _colorTransportCableCar1;          break;
                case StatisticType.TransportEconomyCableCarExpenses:                _textColor = _colorTransportCableCar2;          break;
                case StatisticType.TransportEconomyCableCarProfit:                  _textColor = _colorTransportCableCar3;          break;
                case StatisticType.TransportEconomyTaxiIncome:                      _textColor = _colorTransportTaxi1;              break;
                case StatisticType.TransportEconomyTaxiExpenses:                    _textColor = _colorTransportTaxi2;              break;
                case StatisticType.TransportEconomyTaxiProfit:                      _textColor = _colorTransportTaxi3;              break;
                case StatisticType.TransportEconomyToursIncome:                     _textColor = _colorTransportPedestrian1;        break;
                case StatisticType.TransportEconomyToursExpenses:                   _textColor = _colorTransportPedestrian2;        break;
                case StatisticType.TransportEconomyToursProfit:                     _textColor = _colorTransportPedestrian3;        break;
                case StatisticType.TransportEconomyTollBoothIncome:                 _textColor = _colorTollBooth1;                  break;
                case StatisticType.TransportEconomyTollBoothExpenses:               _textColor = _colorTollBooth2;                  break;
                case StatisticType.TransportEconomyTollBoothProfit:                 _textColor = _colorTollBooth3;                  break;
                case StatisticType.TransportEconomyMailExpenses:                    _textColor = _colorTransferMail2;               break;
                case StatisticType.TransportEconomyMailProfit:                      _textColor = _colorTransferMail3;               break;
                case StatisticType.TransportEconomySpaceElevatorExpenses:           _textColor = _colorSpaceElevator2;              break;
                case StatisticType.TransportEconomySpaceElevatorProfit:             _textColor = _colorSpaceElevator3;              break;

                case StatisticType.GameLimitsBuildingsUsedPercent:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsBuildingsUsed:                         _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsBuildingsCapacity:                     _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsCitizensUsedPercent:                   _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizensUsed:                          _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizensCapacity:                      _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsCitizenUnitsUsedPercent:               _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizenUnitsUsed:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizenUnitsCapacity:                  _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsCitizenInstancesUsedPercent:           _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizenInstancesUsed:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsCitizenInstancesCapacity:              _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsDisastersUsedPercent:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsDisastersUsed:                         _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsDisastersCapacity:                     _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsDistrictsUsedPercent:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsDistrictsUsed:                         _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsDistrictsCapacity:                     _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsEventsUsedPercent:                     _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsEventsUsed:                            _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsEventsCapacity:                        _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsGameAreasUsedPercent:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsGameAreasUsed:                         _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsGameAreasCapacity:                     _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsNetworkLanesUsedPercent:               _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkLanesUsed:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkLanesCapacity:                  _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsNetworkNodesUsedPercent:               _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkNodesUsed:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkNodesCapacity:                  _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsNetworkSegmentsUsedPercent:            _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkSegmentsUsed:                   _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsNetworkSegmentsCapacity:               _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsPaintedAreasUsedPercent:               _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPaintedAreasUsed:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPaintedAreasCapacity:                  _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsPathUnitsUsedPercent:                  _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPathUnitsUsed:                         _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPathUnitsCapacity:                     _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsPropsUsedPercent:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPropsUsed:                             _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsPropsCapacity:                         _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsRadioChannelsUsedPercent:              _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsRadioChannelsUsed:                     _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsRadioChannelsCapacity:                 _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsRadioContentsUsedPercent:              _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsRadioContentsUsed:                     _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsRadioContentsCapacity:                 _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsTransportLinesUsedPercent:             _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsTransportLinesUsed:                    _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsTransportLinesCapacity:                _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsTreesUsedPercent:                      _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsTreesUsed:                             _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsTreesCapacity:                         _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsVehiclesUsedPercent:                   _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsVehiclesUsed:                          _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsVehiclesCapacity:                      _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsVehiclesParkedUsedPercent:             _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsVehiclesParkedUsed:                    _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsVehiclesParkedCapacity:                _textColor = _colorNeutral2;                    break;
                case StatisticType.GameLimitsZoneBlocksUsedPercent:                 _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsZoneBlocksUsed:                        _textColor = _colorNeutral1;                    break;
                case StatisticType.GameLimitsZoneBlocksCapacity:                    _textColor = _colorNeutral2;                    break;

                default:
                    LogUtil.LogError($"Unhandled statistic type [{_type}] when setting colors.");
                    _textColor = _colorNeutral1;
                    break;
            }

            // compute line color from text color
            // for unknown reasons, graph lines appear brighter than the text, so make the line color a bit darker than the text so they match more closely
            _lineColor = new Color32(
                (byte)(_textColor.r * LineColorMultiplier),
                (byte)(_textColor.g * LineColorMultiplier),
                (byte)(_textColor.b * LineColorMultiplier), 255);
        }

        /// <summary>
        /// update UI text
        /// </summary>
        public void UpdateUIText()
        {
            // obtain the translated description, which may or may not have a second part
            string description = Translation.instance.Get(_descriptionKey1);
            if (_descriptionKey2 != Translation.Key.None)
            {
                description += "-" + Translation.instance.Get(_descriptionKey2);
            }

            // obtain the translated units
            _units = Translation.instance.Get(_unitsKey);

            // combine description and units
            _descriptionUnits = description + " (" + _units + ")";

            // combine categtory and description
            _categoryDescription = _category.Description + "-" + description;

            // combine categtory, description, and units
            _categoryDescriptionUnits = _category.Description + "-" + _descriptionUnits;

            // update description
            if (_description != null)
            {
                _description.text = _descriptionUnits;
                _description.textColor = _textColor;
                Configuration config = ConfigurationUtil<Configuration>.Load();
                switch ((Options.CategoryStatisticTextSize)config.CategoryStatisticTextSize)
                {
                    case Options.CategoryStatisticTextSize.Normal:      _description.textScale = 0.625f; _description.relativePosition = new Vector3(_description.relativePosition.x, 3.0f); break;
                    case Options.CategoryStatisticTextSize.Large:       _description.textScale = 0.750f; _description.relativePosition = new Vector3(_description.relativePosition.x, 2.0f); break;
                    case Options.CategoryStatisticTextSize.ExtraLarge:  _description.textScale = 0.875f; _description.relativePosition = new Vector3(_description.relativePosition.x, 0.5f); break;
                }
            }

            // update amount
            if (_amount != null)
            {
                _amount.textColor = _textColor;
                if (_description != null)
                {
                    _amount.textScale = _description.textScale;
                    _amount.relativePosition = new Vector3(_amount.relativePosition.x, _description.relativePosition.y);
                }
            }
        }

        /// <summary>
        /// update the amount from the snapshot
        /// </summary>
        public void UpdateAmount(Snapshot snapshot)
        {
            // make sure label is valid
            if (_amount != null)
            {
                // get the value from the field or property
                object snapshotValue = null;
                if (_snapshotField != null)
                {
                    snapshotValue = _snapshotField.GetValue(snapshot);
                }
                else if (_snapshotProperty != null)
                {
                    snapshotValue = _snapshotProperty.GetValue(snapshot, null);
                }

                // display the value
                if (snapshotValue == null)
                {
                    _amount.text = "";
                }
                else
                {
                    _amount.text = Convert.ToDouble(snapshotValue).ToString(_numberFormat, LocaleManager.cultureInfo);
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
            // check version
            if (
                (version < 3 &&
                    (
                    _type == StatisticType.CityEconomyLoanBalance                       ||
                    _type == StatisticType.CityEconomyCityValue                         ||
                    _type == StatisticType.CityEconomyCityValuePerCapita                ||
                    _type == StatisticType.CityEconomyGrossDomesticProduct              ||
                    _type == StatisticType.CityEconomyGrossDomesticProductPerCapita     ||
                    _type == StatisticType.CityEconomyConsumption                       ||
                    _type == StatisticType.CityEconomyConsumptionPercent                ||
                    _type == StatisticType.CityEconomyGovernmentSpending                ||
                    _type == StatisticType.CityEconomyGovernmentSpendingPercent         ||
                    _type == StatisticType.CityEconomyExports                           ||
                    _type == StatisticType.CityEconomyImports                           ||
                    _type == StatisticType.CityEconomyNetExports                        ||
                    _type == StatisticType.CityEconomyNetExportsPercent
                    )
                )
                ||
                (version < 4 &&
                    (
                    _type == StatisticType.ResidentialIncomeWallToWall                  ||
                    _type == StatisticType.CommercialIncomeWallToWall                   ||
                    _type == StatisticType.OfficeIncomeSpecializedTotal                 ||
                    _type == StatisticType.OfficeIncomeWallToWall                       ||
                    _type == StatisticType.ServiceExpensesServicePoints
                    )
                )
                ||
                (version < 5 &&
                    (
                    _type == StatisticType.TrafficPedestriansPercent                    ||
                    _type == StatisticType.TrafficCyclistsPercent                       ||
                    _type == StatisticType.TrafficPrivateVehiclesPercent                ||
                    _type == StatisticType.TrafficPublicTransportCargoPercent           ||
                    _type == StatisticType.TrafficTrucksPercent                         ||
                    _type == StatisticType.TrafficCityServiceVehiclesPercent            ||
                    _type == StatisticType.TrafficDummyTrafficPercent                   ||
                    _type == StatisticType.TrafficTotalCount                            ||
                    _type == StatisticType.TrafficPedestriansCount                      ||
                    _type == StatisticType.TrafficCyclistsCount                         ||
                    _type == StatisticType.TrafficPrivateVehiclesCount                  ||
                    _type == StatisticType.TrafficPublicTransportCargoCount             ||
                    _type == StatisticType.TrafficTrucksCount                           ||
                    _type == StatisticType.TrafficCityServiceVehiclesCount              ||
                    _type == StatisticType.TrafficDummyTrafficCount
                    )
                )
                ||
                (version < 7 &&
                    (
                    _type == StatisticType.CommercialCashAccumulatedPercent             ||
                    _type == StatisticType.CommercialCashAccumulated                    ||
                    _type == StatisticType.CommercialCashCapacity                       ||
                    _type == StatisticType.CommercialCashCollected                      ||
                    _type == StatisticType.InvestmentsChirpAir                          ||
                    _type == StatisticType.InvestmentsChirperCrypto                     ||
                    _type == StatisticType.InvestmentsDeathcareServiceFund              ||
                    _type == StatisticType.InvestmentsFarmingIndustry                   ||
                    _type == StatisticType.InvestmentsForestryIndustry                  ||
                    _type == StatisticType.InvestmentsGreasyGasoline                    ||
                    _type == StatisticType.InvestmentsGenericIndustry                   ||
                    _type == StatisticType.InvestmentsHealthcareServiceFund             ||
                    _type == StatisticType.InvestmentsOilIndustry                       ||
                    _type == StatisticType.InvestmentsOreIndustry                       ||
                    _type == StatisticType.InvestmentsChirpyCruises                     ||
                    _type == StatisticType.InvestmentsTrafficJellyLogistics             ||
                    _type == StatisticType.InvestmentsVeryLegitCompany                  ||
                    _type == StatisticType.InvestmentsGainsLastMonth                    ||
                    _type == StatisticType.InvestmentsTotalGains                        ||
                    _type == StatisticType.OfficeIncomeFinancial                        ||
                    _type == StatisticType.ServiceExpensesBanks
                    )
                )
                ||
                (version < 8 &&
                    (
                    _type == StatisticType.IntercityTravelArrivingTotal                 ||
                    _type == StatisticType.IntercityTravelArrivingResidents             ||
                    _type == StatisticType.IntercityTravelArrivingTourists              ||
                    _type == StatisticType.IntercityTravelDepartingTotal                ||
                    _type == StatisticType.IntercityTravelDepartingResidents            ||
                    _type == StatisticType.IntercityTravelDepartingTourists             ||
                    _type == StatisticType.IntercityTravelDummyTrafficTotal             ||
                    _type == StatisticType.IntercityTravelDummyTrafficResidents         ||
                    _type == StatisticType.IntercityTravelDummyTrafficTourists
                    )
                )
                ||
                (version < 9 &&
                    (
                    _type == StatisticType.TourismIncomeHotels                          ||
                    _type == StatisticType.HotelsTotalIncomePercent                     ||
                    _type == StatisticType.HotelsTotalIncome                            ||
                    _type == StatisticType.HotelsTotalExpensesPercent                   ||
                    _type == StatisticType.HotelsTotalExpenses                          ||
                    _type == StatisticType.HotelsTotalProfit                            ||
                    _type == StatisticType.HotelsTotalPopularity                        ||
                    _type == StatisticType.HotelsSightseeingPopularity                  ||
                    _type == StatisticType.HotelsShoppingPopularity                     ||
                    _type == StatisticType.HotelsBusinessPopularity                     ||
                    _type == StatisticType.HotelsNaturePopularity                       ||
                    _type == StatisticType.HotelsGuestsVisitingPercent                  ||
                    _type == StatisticType.HotelsGuestsVisiting                         ||
                    _type == StatisticType.HotelsGuestsCapacity
                    )
                )
               )
            {
                // not selected by default
                Selected = false;
            }
            else
            {
                // read selection status
                Selected = reader.ReadBoolean();
            }
        }
    }
}
