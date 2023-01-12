using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MoreCityStatistics
{
    /// <summary>
    /// the list of categories
    /// </summary>
    public class Categories : List<Category>
    {
        // use singleton pattern:  there can be only one list of categories in the game
        private static readonly Categories _instance = new Categories();
        public static Categories instance { get { return _instance; } }
        private Categories() { }

        /// <summary>
        /// initialize categories
        /// </summary>
        public void Initialize()
        {
            // initialize categories and statistics
            #region Categories and Statistics
            _instance.Clear();
            Category category;

            _instance.Add(category = new Category(Category.CategoryType.Electricity, Translation.Key.Electricity));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityConsumptionPercent,              Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.PctOfProduction             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityConsumption,                     Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.MegaWatts                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityProduction,                      Translation.Key.Production,             Translation.Key.None,           Translation.Key.MegaWatts                   ));

            _instance.Add(category = new Category(Category.CategoryType.Water, Translation.Key.Water));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterConsumptionPercent,                    Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.PctOfPumpingCapacity        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterConsumption,                           Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.CubicMetersPerWeek          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterPumpingCapacity,                       Translation.Key.PumpingCapacity,        Translation.Key.None,           Translation.Key.CubicMetersPerWeek          ));

            _instance.Add(category = new Category(Category.CategoryType.WaterTank, Translation.Key.WaterTank));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankReservedPercent,                   Translation.Key.Reserved,               Translation.Key.None,           Translation.Key.PctOfStorageCapacity        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankReserved,                          Translation.Key.Reserved,               Translation.Key.None,           Translation.Key.CubicMeters                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankStorageCapacity,                   Translation.Key.StorageCapacity,        Translation.Key.None,           Translation.Key.CubicMeters                 ));

            _instance.Add(category = new Category(Category.CategoryType.Sewage, Translation.Key.Sewage));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageProductionPercent,                    Translation.Key.Production,             Translation.Key.None,           Translation.Key.PctOfDrainingCapacity       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageProduction,                           Translation.Key.Production,             Translation.Key.None,           Translation.Key.CubicMetersPerWeek          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageDrainingCapacity,                     Translation.Key.DrainingCapacity,       Translation.Key.None,           Translation.Key.CubicMetersPerWeek          ));

            _instance.Add(category = new Category(Category.CategoryType.Landfill, Translation.Key.Landfill));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillStoragePercent,                     Translation.Key.Storage,                Translation.Key.None,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillStorage,                            Translation.Key.Storage,                Translation.Key.None,           Translation.Key.Units                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillCapacity,                           Translation.Key.Capacity,               Translation.Key.None,           Translation.Key.Units                       ));

            _instance.Add(category = new Category(Category.CategoryType.Garbage, Translation.Key.Garbage));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProductionPercent,                   Translation.Key.Production,             Translation.Key.None,           Translation.Key.PctOfProcessingCapacity     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProduction,                          Translation.Key.Production,             Translation.Key.None,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProcessingCapacity,                  Translation.Key.ProcessingCapacity,     Translation.Key.None,           Translation.Key.UnitsPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.Education, Translation.Key.Education));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryEligiblePercent,         Translation.Key.Elementary,             Translation.Key.Eligible,       Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryEligible,                Translation.Key.Elementary,             Translation.Key.Eligible,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryCapacity,                Translation.Key.Elementary,             Translation.Key.Capacity,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolEligiblePercent,         Translation.Key.HighSchool,             Translation.Key.Eligible,       Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolEligible,                Translation.Key.HighSchool,             Translation.Key.Eligible,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolCapacity,                Translation.Key.HighSchool,             Translation.Key.Capacity,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityEligiblePercent,         Translation.Key.University,             Translation.Key.Eligible,       Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityEligible,                Translation.Key.University,             Translation.Key.Eligible,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityCapacity,                Translation.Key.University,             Translation.Key.Capacity,       Translation.Key.Students                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryUsersPercent,               Translation.Key.PublicLibrary,          Translation.Key.Users,          Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryUsers,                      Translation.Key.PublicLibrary,          Translation.Key.Users,          Translation.Key.Visitors                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryCapacity,                   Translation.Key.PublicLibrary,          Translation.Key.Capacity,       Translation.Key.Visitors                    ));

            _instance.Add(category = new Category(Category.CategoryType.EducationLevel, Translation.Key.EducationLevel));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelUneducatedPercent,            Translation.Key.Uneducated,             Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelEducatedPercent,              Translation.Key.Educated,               Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelWellEducatedPercent,          Translation.Key.WellEducated,           Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelHighlyEducatedPercent,        Translation.Key.HighlyEducated,         Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelUneducated,                   Translation.Key.Uneducated,             Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelEducated,                     Translation.Key.Educated,               Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelWellEducated,                 Translation.Key.WellEducated,           Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelHighlyEducated,               Translation.Key.HighlyEducated,         Translation.Key.None,           Translation.Key.Citizens                    ));

            _instance.Add(category = new Category(Category.CategoryType.Happiness, Translation.Key.Happiness));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessGlobal,                            Translation.Key.Global,                 Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessResidential,                       Translation.Key.Residential,            Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessCommercial,                        Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessIndustrial,                        Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessOffice,                            Translation.Key.Office,                 Translation.Key.None,           Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.Healthcare, Translation.Key.Healthcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareAverageHealth,                    Translation.Key.AverageHealth,          Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareSickPercent,                      Translation.Key.Sick,                   Translation.Key.None,           Translation.Key.PctOfHealCapacity           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareSick,                             Translation.Key.Sick,                   Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareHealCapacity,                     Translation.Key.HealCapacity,           Translation.Key.None,           Translation.Key.Citizens                    ));

            _instance.Add(category = new Category(Category.CategoryType.Deathcare, Translation.Key.Deathcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryBuriedPercent,             Translation.Key.Cemetery,               Translation.Key.Buried,         Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryBuried,                    Translation.Key.Cemetery,               Translation.Key.Buried,         Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryCapacity,                  Translation.Key.Cemetery,               Translation.Key.Capacity,       Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumDeceasedPercent,        Translation.Key.Crematorium,            Translation.Key.Deceased,       Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumDeceased,               Translation.Key.Crematorium,            Translation.Key.Deceased,       Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumCapacity,               Translation.Key.Crematorium,            Translation.Key.Capacity,       Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareDeathRate,                         Translation.Key.DeathRate,              Translation.Key.None,           Translation.Key.CitizensPerWeek             ));

            _instance.Add(category = new Category(Category.CategoryType.Childcare, Translation.Key.Childcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareAverageHealth,                     Translation.Key.AverageHealth,          Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareSickPercent,                       Translation.Key.SickChildrenTeens,      Translation.Key.None,           Translation.Key.PctOfChildrenTeens          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareSick,                              Translation.Key.SickChildrenTeens,      Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcarePopulation,                        Translation.Key.ChildrenTeens,          Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareBirthRate,                         Translation.Key.BirthRate,              Translation.Key.None,           Translation.Key.CitizensPerWeek             ));

            _instance.Add(category = new Category(Category.CategoryType.Eldercare, Translation.Key.Eldercare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareAverageHealth,                     Translation.Key.AverageHealth,          Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareSickPercent,                       Translation.Key.SickSeniors,            Translation.Key.None,           Translation.Key.PctOfSeniors                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareSick,                              Translation.Key.SickSeniors,            Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercarePopulation,                        Translation.Key.Seniors,                Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareAverageLifeSpan,                   Translation.Key.AverageLifeSpan,        Translation.Key.None,           Translation.Key.Years                       ));

            _instance.Add(category = new Category(Category.CategoryType.Zoning, Translation.Key.Zoning));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningResidentialPercent,                   Translation.Key.Residential,            Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningCommercialPercent,                    Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningIndustrialPercent,                    Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningOfficePercent,                        Translation.Key.Office,                 Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningUnzonedPercent,                       Translation.Key.Unzoned,                Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningTotal,                                Translation.Key.Total,                  Translation.Key.None,           Translation.Key.Squares                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningResidential,                          Translation.Key.Residential,            Translation.Key.None,           Translation.Key.Squares                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningCommercial,                           Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.Squares                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningIndustrial,                           Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.Squares                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningOffice,                               Translation.Key.Office,                 Translation.Key.None,           Translation.Key.Squares                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningUnzoned,                              Translation.Key.Unzoned,                Translation.Key.None,           Translation.Key.Squares                     ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneLevel, Translation.Key.ZoneLevel));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidentialAverage,                Translation.Key.Residential,            Translation.Key.Average,        Translation.Key.Level1To5                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential1,                      Translation.Key.Residential,            Translation.Key.Level1,         Translation.Key.PctOfResidential            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential2,                      Translation.Key.Residential,            Translation.Key.Level2,         Translation.Key.PctOfResidential            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential3,                      Translation.Key.Residential,            Translation.Key.Level3,         Translation.Key.PctOfResidential            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential4,                      Translation.Key.Residential,            Translation.Key.Level4,         Translation.Key.PctOfResidential            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential5,                      Translation.Key.Residential,            Translation.Key.Level5,         Translation.Key.PctOfResidential            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercialAverage,                 Translation.Key.Commercial,             Translation.Key.Average,        Translation.Key.Level1To3                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial1,                       Translation.Key.Commercial,             Translation.Key.Level1,         Translation.Key.PctOfCommercial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial2,                       Translation.Key.Commercial,             Translation.Key.Level2,         Translation.Key.PctOfCommercial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial3,                       Translation.Key.Commercial,             Translation.Key.Level3,         Translation.Key.PctOfCommercial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrialAverage,                 Translation.Key.Industrial,             Translation.Key.Average,        Translation.Key.Level1To3                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial1,                       Translation.Key.Industrial,             Translation.Key.Level1,         Translation.Key.PctOfIndustrial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial2,                       Translation.Key.Industrial,             Translation.Key.Level2,         Translation.Key.PctOfIndustrial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial3,                       Translation.Key.Industrial,             Translation.Key.Level3,         Translation.Key.PctOfIndustrial             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOfficeAverage,                     Translation.Key.Office,                 Translation.Key.Average,        Translation.Key.Level1To3                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice1,                           Translation.Key.Office,                 Translation.Key.Level1,         Translation.Key.PctOfOffice                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice2,                           Translation.Key.Office,                 Translation.Key.Level2,         Translation.Key.PctOfOffice                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice3,                           Translation.Key.Office,                 Translation.Key.Level3,         Translation.Key.PctOfOffice                 ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneBuildings, Translation.Key.ZoneBuildings));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsResidentialPercent,            Translation.Key.Residential,            Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsCommercialPercent,             Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsIndustrialPercent,             Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsOfficePercent,                 Translation.Key.Office,                 Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsTotal,                         Translation.Key.Total,                  Translation.Key.None,           Translation.Key.HouseholdsPlusJobs          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsResidential,                   Translation.Key.Residential,            Translation.Key.None,           Translation.Key.Households                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsCommercial,                    Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.Jobs                        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsIndustrial,                    Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.Jobs                        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsOffice,                        Translation.Key.Office,                 Translation.Key.None,           Translation.Key.Jobs                        ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneDemand, Translation.Key.ZoneDemand));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandResidential,                      Translation.Key.Residential,            Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandCommercial,                       Translation.Key.Commercial,             Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandIndustrialOffice,                 Translation.Key.IndustrialOffice,       Translation.Key.None,           Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.Traffic, Translation.Key.Traffic));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficAverageFlow,                         Translation.Key.Average,                Translation.Key.Flow,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPedestriansPercent,                  Translation.Key.Pedestrians,            Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficCyclistsPercent,                     Translation.Key.Cyclists,               Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPrivateVehiclesPercent,              Translation.Key.PrivateVehicles,        Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPublicTransportCargoPercent,         Translation.Key.PublicTransportAndCargo,Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficTrucksPercent,                       Translation.Key.Trucks,                 Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficCityServiceVehiclesPercent,          Translation.Key.CityServiceVehicles,    Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficDummyTrafficPercent,                 Translation.Key.DummyTraffic,           Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficTotalCount,                          Translation.Key.Total,                  Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPedestriansCount,                    Translation.Key.Pedestrians,            Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficCyclistsCount,                       Translation.Key.Cyclists,               Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPrivateVehiclesCount,                Translation.Key.PrivateVehicles,        Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficPublicTransportCargoCount,           Translation.Key.PublicTransportAndCargo,Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficTrucksCount,                         Translation.Key.Trucks,                 Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficCityServiceVehiclesCount,            Translation.Key.CityServiceVehicles,    Translation.Key.None,           Translation.Key.Count                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficDummyTrafficCount,                   Translation.Key.DummyTraffic,           Translation.Key.None,           Translation.Key.Count                       ));

            _instance.Add(category = new Category(Category.CategoryType.Pollution, Translation.Key.Pollution));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageGround,                     Translation.Key.Average,                Translation.Key.Ground,         Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageDrinkingWater,              Translation.Key.Average,                Translation.Key.DrinkingWater,  Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageNoise,                      Translation.Key.Average,                Translation.Key.Noise,          Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.FireSafety, Translation.Key.FireSafety));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.FireSafetyHazard,                           Translation.Key.Hazard,                 Translation.Key.None,           Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.Crime, Translation.Key.Crime));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeRate,                                  Translation.Key.Rate,                   Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeDetainedCriminalsPercent,              Translation.Key.DetainedCriminals,      Translation.Key.None,           Translation.Key.PctOfJailsCapacity          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeDetainedCriminals,                     Translation.Key.DetainedCriminals,      Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeJailsCapacity,                         Translation.Key.JailsCapacity,          Translation.Key.None,           Translation.Key.Citizens                    ));

            _instance.Add(category = new Category(Category.CategoryType.CommercialCash, Translation.Key.CommercialCash));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialCashAccumulatedPercent,           Translation.Key.Accumulated,            Translation.Key.None,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialCashAccumulated,                  Translation.Key.Accumulated,            Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialCashCapacity,                     Translation.Key.Capacity,               Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialCashCollected,                    Translation.Key.Collected,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.PublicTransportation, Translation.Key.PublicTransportation));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalTotal,             Translation.Key.Total,                  Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalResidents,         Translation.Key.Total,                  Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalTourists,          Translation.Key.Total,                  Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusTotal,               Translation.Key.Bus,                    Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusResidents,           Translation.Key.Bus,                    Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusTourists,            Translation.Key.Bus,                    Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusTotal,        Translation.Key.Trolleybus,             Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusResidents,    Translation.Key.Trolleybus,             Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusTourists,     Translation.Key.Trolleybus,             Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramTotal,              Translation.Key.Tram,                   Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramResidents,          Translation.Key.Tram,                   Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramTourists,           Translation.Key.Tram,                   Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroTotal,             Translation.Key.Metro,                  Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroResidents,         Translation.Key.Metro,                  Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroTourists,          Translation.Key.Metro,                  Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainTotal,             Translation.Key.Train,                  Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainResidents,         Translation.Key.Train,                  Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainTourists,          Translation.Key.Train,                  Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipTotal,              Translation.Key.Ship,                   Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipResidents,          Translation.Key.Ship,                   Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipTourists,           Translation.Key.Ship,                   Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirTotal,               Translation.Key.Air,                    Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirResidents,           Translation.Key.Air,                    Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirTourists,            Translation.Key.Air,                    Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailTotal,          Translation.Key.Monorail,               Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailResidents,      Translation.Key.Monorail,               Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailTourists,       Translation.Key.Monorail,               Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarTotal,          Translation.Key.CableCar,               Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarResidents,      Translation.Key.CableCar,               Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarTourists,       Translation.Key.CableCar,               Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiTotal,              Translation.Key.Taxi,                   Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiResidents,          Translation.Key.Taxi,                   Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiTourists,           Translation.Key.Taxi,                   Translation.Key.None,           Translation.Key.TouristsPerWeek             ));

            _instance.Add(category = new Category(Category.CategoryType.IntercityTravel, Translation.Key.IntercityTravel));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelArrivingTotal,               Translation.Key.Arriving,               Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelArrivingResidents,           Translation.Key.Arriving,               Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelArrivingTourists,            Translation.Key.Arriving,               Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDepartingTotal,              Translation.Key.Departing,              Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDepartingResidents,          Translation.Key.Departing,              Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDepartingTourists,           Translation.Key.Departing,              Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDummyTrafficTotal,           Translation.Key.DummyTraffic,           Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDummyTrafficResidents,       Translation.Key.DummyTraffic,           Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IntercityTravelDummyTrafficTourists,        Translation.Key.DummyTraffic,           Translation.Key.None,           Translation.Key.TouristsPerWeek             ));

            _instance.Add(category = new Category(Category.CategoryType.Population, Translation.Key.Population));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTotal,                            Translation.Key.Total,                  Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationChildrenPercent,                  Translation.Key.Children,               Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTeensPercent,                     Translation.Key.Teens,                  Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationYoungAdultsPercent,               Translation.Key.YoungAdults,            Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationAdultsPercent,                    Translation.Key.Adults,                 Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationSeniorsPercent,                   Translation.Key.Seniors,                Translation.Key.None,           Translation.Key.PctOfPopulation             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationChildren,                         Translation.Key.Children,               Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTeens,                            Translation.Key.Teens,                  Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationYoungAdults,                      Translation.Key.YoungAdults,            Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationAdults,                           Translation.Key.Adults,                 Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationSeniors,                          Translation.Key.Seniors,                Translation.Key.None,           Translation.Key.Citizens                    ));

            _instance.Add(category = new Category(Category.CategoryType.Households, Translation.Key.Households));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsOccupiedPercent,                  Translation.Key.Occupied,               Translation.Key.None,           Translation.Key.PctOfAvailable              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsOccupied,                         Translation.Key.Occupied,               Translation.Key.None,           Translation.Key.Households                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsAvailable,                        Translation.Key.Available,              Translation.Key.None,           Translation.Key.Households                  ));

            _instance.Add(category = new Category(Category.CategoryType.Employment, Translation.Key.Employment));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentPeopleEmployed,                   Translation.Key.PeopleEmployed,         Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentJobsAvailable,                    Translation.Key.JobsAvailable,          Translation.Key.None,           Translation.Key.Jobs                        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnfilledJobs,                     Translation.Key.UnfilledJobs,           Translation.Key.None,           Translation.Key.Jobs                        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnemploymentPercent,              Translation.Key.Unemployment,           Translation.Key.None,           Translation.Key.PctOfEligible               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnemployed,                       Translation.Key.Unemployed,             Translation.Key.None,           Translation.Key.Citizens                    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentEligibleWorkers,                  Translation.Key.EligibleWorkers,        Translation.Key.None,           Translation.Key.Citizens                    ));

            _instance.Add(category = new Category(Category.CategoryType.OutsideConnections, Translation.Key.OutsideConnections));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportTotal,              Translation.Key.Import,                 Translation.Key.Total,          Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportGoods,              Translation.Key.Import,                 Translation.Key.Goods,          Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportForestry,           Translation.Key.Import,                 Translation.Key.Forestry,       Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportFarming,            Translation.Key.Import,                 Translation.Key.Farming,        Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportOre,                Translation.Key.Import,                 Translation.Key.Ore,            Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportOil,                Translation.Key.Import,                 Translation.Key.Oil,            Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportMail,               Translation.Key.Import,                 Translation.Key.Mail,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportTotal,              Translation.Key.Export,                 Translation.Key.Total,          Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportGoods,              Translation.Key.Export,                 Translation.Key.Goods,          Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportForestry,           Translation.Key.Export,                 Translation.Key.Forestry,       Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportFarming,            Translation.Key.Export,                 Translation.Key.Farming,        Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportOre,                Translation.Key.Export,                 Translation.Key.Ore,            Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportOil,                Translation.Key.Export,                 Translation.Key.Oil,            Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportMail,               Translation.Key.Export,                 Translation.Key.Mail,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportFish,               Translation.Key.Export,                 Translation.Key.Fish,           Translation.Key.UnitsPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.LandValue, Translation.Key.LandValue));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandValueAverage,                           Translation.Key.Average,                Translation.Key.None,           Translation.Key.MoneyPerSquareMeter         ));

            _instance.Add(category = new Category(Category.CategoryType.NaturalResources, Translation.Key.NaturalResources));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestUsedPercent,          Translation.Key.Forest,                 Translation.Key.Used,           Translation.Key.PctOfForestAvailable        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestUsed,                 Translation.Key.Forest,                 Translation.Key.Used,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestAvailable,            Translation.Key.Forest,                 Translation.Key.Available,      Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandUsedPercent,     Translation.Key.FertileLand,            Translation.Key.Used,           Translation.Key.PctOfFertileLandAvailable   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandUsed,            Translation.Key.FertileLand,            Translation.Key.Used,           Translation.Key.Hectare                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandAvailable,       Translation.Key.FertileLand,            Translation.Key.Available,      Translation.Key.Hectare                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreUsedPercent,             Translation.Key.Ore,                    Translation.Key.Used,           Translation.Key.PctPerWeekOfOreAvailable    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreUsed,                    Translation.Key.Ore,                    Translation.Key.Used,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreAvailable,               Translation.Key.Ore,                    Translation.Key.Available,      Translation.Key.Units                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilUsedPercent,             Translation.Key.Oil,                    Translation.Key.Used,           Translation.Key.PctPerWeekOfOilAvailable    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilUsed,                    Translation.Key.Oil,                    Translation.Key.Used,           Translation.Key.UnitsPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilAvailable,               Translation.Key.Oil,                    Translation.Key.Available,      Translation.Key.Units                       ));

            _instance.Add(category = new Category(Category.CategoryType.Heating, Translation.Key.Heating));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingConsumptionPercent,                  Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.PctOfProduction             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingConsumption,                         Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.MegaWatts                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingProduction,                          Translation.Key.Production,             Translation.Key.None,           Translation.Key.MegaWatts                   ));

            _instance.Add(category = new Category(Category.CategoryType.Tourism, Translation.Key.Tourism));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismCityAttractiveness,                  Translation.Key.CityAttractiveness,     Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismLowWealthPercent,                    Translation.Key.LowWealth,              Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismMediumWealthPercent,                 Translation.Key.MediumWealth,           Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismHighWealthPercent,                   Translation.Key.HighWealth,             Translation.Key.None,           Translation.Key.PctOfTotal                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismTotal,                               Translation.Key.Total,                  Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismLowWealth,                           Translation.Key.LowWealth,              Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismMediumWealth,                        Translation.Key.MediumWealth,           Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismHighWealth,                          Translation.Key.HighWealth,             Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismExchangeStudentBonus,                Translation.Key.ExchangeStudentBonus,   Translation.Key.None,           Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.Tours, Translation.Key.Tours));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalTotal,                            Translation.Key.Total,                  Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalResidents,                        Translation.Key.Total,                  Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalTourists,                         Translation.Key.Total,                  Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourTotal,                      Translation.Key.WalkingTour,            Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourResidents,                  Translation.Key.WalkingTour,            Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourTourists,                   Translation.Key.WalkingTour,            Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingTotal,                      Translation.Key.SightseeingBus,         Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingResidents,                  Translation.Key.SightseeingBus,         Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingTourists,                   Translation.Key.SightseeingBus,         Translation.Key.None,           Translation.Key.TouristsPerWeek             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonTotal,                          Translation.Key.Balloon,                Translation.Key.None,           Translation.Key.TotalPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonResidents,                      Translation.Key.Balloon,                Translation.Key.None,           Translation.Key.ResidentsPerWeek            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonToursits,                       Translation.Key.Balloon,                Translation.Key.None,           Translation.Key.TouristsPerWeek             ));

            _instance.Add(category = new Category(Category.CategoryType.TaxRate, Translation.Key.TaxRate));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateResidentialLow,                      Translation.Key.Residential,            Translation.Key.LowDensity,     Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateResidentialHigh,                     Translation.Key.Residential,            Translation.Key.HighDensity,    Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateCommercialLow,                       Translation.Key.Commercial,             Translation.Key.LowDensity,     Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateCommercialHigh,                      Translation.Key.Commercial,             Translation.Key.HighDensity,    Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateIndustrial,                          Translation.Key.Industrial,             Translation.Key.None,           Translation.Key.Percent                     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateOffice,                              Translation.Key.Office,                 Translation.Key.None,           Translation.Key.Percent                     ));

            _instance.Add(category = new Category(Category.CategoryType.Investments, Translation.Key.Investments));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsChirpAir,                        Translation.Key.ChirpAir,               Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsChirperCrypto,                   Translation.Key.ChirperCrypto,          Translation.Key.None,           Translation.Key.MoneyPerCoin                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsDeathcareServiceFund,            Translation.Key.DeathcareServiceFund,   Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsFarmingIndustry,                 Translation.Key.FarmingIndustry,        Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsForestryIndustry,                Translation.Key.ForestryIndustry,       Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsGreasyGasoline,                  Translation.Key.GreasyGasoline,         Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsGenericIndustry,                 Translation.Key.GenericIndustry,        Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsHealthcareServiceFund,           Translation.Key.HealthcareServiceFund,  Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsOilIndustry,                     Translation.Key.OilIndustry,            Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsOreIndustry,                     Translation.Key.OreIndustry,            Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsChirpyCruises,                   Translation.Key.ChirpyCruises,          Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsTrafficJellyLogistics,           Translation.Key.TrafficJellyLogistics,  Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsVeryLegitCompany,                Translation.Key.VeryLegitCompany,       Translation.Key.None,           Translation.Key.MoneyPerShare               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsGainsLastMonth,                  Translation.Key.GainsLastMonth,         Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.InvestmentsTotalGains,                      Translation.Key.TotalGains,             Translation.Key.None,           Translation.Key.Money                       ));

            _instance.Add(category = new Category(Category.CategoryType.CityEconomy, Translation.Key.CityEconomy));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalIncome,                     Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalExpenses,                   Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalProfit,                     Translation.Key.Total,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyBankBalance,                     Translation.Key.BankBalance,            Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyLoanBalance,                     Translation.Key.LoanBalance,            Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyCityValue,                       Translation.Key.CityValue,              Translation.Key.None,           Translation.Key.Money                       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyCityValuePerCapita,              Translation.Key.CityValue,              Translation.Key.None,           Translation.Key.MoneyPerCapita              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyGrossDomesticProduct,            Translation.Key.GrossDomesticProduct,   Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyGrossDomesticProductPerCapita,   Translation.Key.GrossDomesticProduct,   Translation.Key.None,           Translation.Key.MoneyPerWeekPerCapita       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyConsumption,                     Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyConsumptionPercent,              Translation.Key.Consumption,            Translation.Key.None,           Translation.Key.PctOfGrossDomesticProduct   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyGovernmentSpending,              Translation.Key.GovernmentSpending,     Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyGovernmentSpendingPercent,       Translation.Key.GovernmentSpending,     Translation.Key.None,           Translation.Key.PctOfGrossDomesticProduct   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyExports,                         Translation.Key.Exports,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyImports,                         Translation.Key.Imports,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyNetExports,                      Translation.Key.NetExports,             Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyNetExportsPercent,               Translation.Key.NetExports,             Translation.Key.None,           Translation.Key.PctOfGrossDomesticProduct   ));

            _instance.Add(category = new Category(Category.CategoryType.ResidentialIncome, Translation.Key.ResidentialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeTotalPercent,              Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeTotal,                     Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensityTotal,           Translation.Key.LowDensity,             Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity1,               Translation.Key.LowDensity,             Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity2,               Translation.Key.LowDensity,             Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity3,               Translation.Key.LowDensity,             Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity4,               Translation.Key.LowDensity,             Translation.Key.Level4,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity5,               Translation.Key.LowDensity,             Translation.Key.Level5,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensitySelfSufficient,  Translation.Key.LowDensity,             Translation.Key.SelfSufficient, Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensityTotal,          Translation.Key.HighDensity,            Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity1,              Translation.Key.HighDensity,            Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity2,              Translation.Key.HighDensity,            Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity3,              Translation.Key.HighDensity,            Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity4,              Translation.Key.HighDensity,            Translation.Key.Level4,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity5,              Translation.Key.HighDensity,            Translation.Key.Level5,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensitySelfSufficient, Translation.Key.HighDensity,            Translation.Key.SelfSufficient, Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeWallToWall,                Translation.Key.WallToWall,             Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.CommercialIncome, Translation.Key.CommercialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTotalPercent,               Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTotal,                      Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensityTotal,            Translation.Key.LowDensity,             Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity1,                Translation.Key.LowDensity,             Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity2,                Translation.Key.LowDensity,             Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity3,                Translation.Key.LowDensity,             Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensityTotal,           Translation.Key.HighDensity,            Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity1,               Translation.Key.HighDensity,            Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity2,               Translation.Key.HighDensity,            Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity3,               Translation.Key.HighDensity,            Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeSpecializedTotal,           Translation.Key.SpecializedTotal,       Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLeisure,                    Translation.Key.Leisure,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTourism,                    Translation.Key.Tourism,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeOrganic,                    Translation.Key.OrganicAndLocalProduce, Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeWallToWall,                 Translation.Key.WallToWall,             Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.IndustrialIncome, Translation.Key.IndustrialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeTotalPercent,               Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeTotal,                      Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGenericTotal,               Translation.Key.Generic,                Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric1,                   Translation.Key.Generic,                Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric2,                   Translation.Key.Generic,                Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric3,                   Translation.Key.Generic,                Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeSpecializedTotal,           Translation.Key.SpecializedTotal,       Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeForestry,                   Translation.Key.Forestry,               Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeFarming,                    Translation.Key.Farming,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeOre,                        Translation.Key.Ore,                    Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeOil,                        Translation.Key.Oil,                    Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.OfficeIncome, Translation.Key.OfficeIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeTotalPercent,                   Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeTotal,                          Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGenericTotal,                   Translation.Key.Generic,                Translation.Key.Total,          Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric1,                       Translation.Key.Generic,                Translation.Key.Level1,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric2,                       Translation.Key.Generic,                Translation.Key.Level2,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric3,                       Translation.Key.Generic,                Translation.Key.Level3,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeSpecializedTotal,               Translation.Key.SpecializedTotal,       Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeITCluster,                      Translation.Key.ITCluster,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeWallToWall,                     Translation.Key.WallToWall,             Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeFinancial,                      Translation.Key.Financial,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.TourismIncome, Translation.Key.TourismIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTotalPercent,                  Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTotal,                         Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeCommercialZones,               Translation.Key.CommercialZones,        Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTransportation,                Translation.Key.PublicTransportation,   Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeParkAreas,                     Translation.Key.ParkAreas,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.ServiceExpenses, Translation.Key.ServiceExpenses));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesTotalPercent,                Translation.Key.Total,                  Translation.Key.None,           Translation.Key.PctOfCityExpenses           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesTotal,                       Translation.Key.Total,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesRoads,                       Translation.Key.Roads,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesElectricity,                 Translation.Key.Electricity,            Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesWaterSewageHeating,          Translation.Key.WaterSewage,            Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesGarbage,                     Translation.Key.Garbage,                Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesHealthcare,                  Translation.Key.Healthcare,             Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesFire,                        Translation.Key.Fire,                   Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesEmergency,                   Translation.Key.Emergency,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesPolice,                      Translation.Key.Police,                 Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesBanks,                       Translation.Key.Banks,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesEducation,                   Translation.Key.Education,              Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesParksPlazas,                 Translation.Key.ParksPlazasLandscaping, Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesServicePoints,               Translation.Key.ServicePoints,          Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesUniqueBuildings,             Translation.Key.UniqueBuildings,        Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesGenericSportsArenas,         Translation.Key.GenericSportsArenas,    Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesLoans,                       Translation.Key.Loans,                  Translation.Key.None,           Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesPolicies,                    Translation.Key.Policies,               Translation.Key.None,           Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.ParkAreas, Translation.Key.ParkAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalIncomePercent,                Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalIncome,                       Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalExpensesPercent,              Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.PctOfCityExpenses           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalExpenses,                     Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalProfit,                       Translation.Key.Total,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkIncome,                    Translation.Key.CityPark,               Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkExpenses,                  Translation.Key.CityPark,               Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkProfit,                    Translation.Key.CityPark,               Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkIncome,               Translation.Key.AmusementPark,          Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkExpenses,             Translation.Key.AmusementPark,          Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkProfit,               Translation.Key.AmusementPark,          Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooIncome,                         Translation.Key.Zoo,                    Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooExpenses,                       Translation.Key.Zoo,                    Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooProfit,                         Translation.Key.Zoo,                    Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveIncome,               Translation.Key.NatureReserve,          Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveExpenses,             Translation.Key.NatureReserve,          Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveProfit,               Translation.Key.NatureReserve,          Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.IndustryAreas, Translation.Key.IndustryAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalIncomePercent,            Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalIncome,                   Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalExpensesPercent,          Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.PctOfCityExpenses           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalExpenses,                 Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalProfit,                   Translation.Key.Total,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryIncome,                Translation.Key.Forestry,               Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryExpenses,              Translation.Key.Forestry,               Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryProfit,                Translation.Key.Forestry,               Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingIncome,                 Translation.Key.Farming,                Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingExpenses,               Translation.Key.Farming,                Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingProfit,                 Translation.Key.Farming,                Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreIncome,                     Translation.Key.Ore,                    Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreExpenses,                   Translation.Key.Ore,                    Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreProfit,                     Translation.Key.Ore,                    Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilIncome,                     Translation.Key.Oil,                    Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilExpenses,                   Translation.Key.Oil,                    Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilProfit,                     Translation.Key.Oil,                    Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesIncome,     Translation.Key.WarehousesAndFactories, Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesExpenses,   Translation.Key.WarehousesAndFactories, Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesProfit,     Translation.Key.WarehousesAndFactories, Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.FishingIndustry, Translation.Key.FishingIndustry));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.FishingIndustryFishingIncome,               Translation.Key.Fishing,                Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.FishingIndustryFishingExpenses,             Translation.Key.Fishing,                Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.FishingIndustryFishingProfit,               Translation.Key.Fishing,                Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.CampusAreas, Translation.Key.CampusAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalIncomePercent,              Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalIncome,                     Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalExpensesPercent,            Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.PctOfCityExpenses           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalExpenses,                   Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalProfit,                     Translation.Key.Total,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolIncome,               Translation.Key.TradeSchool,            Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolExpenses,             Translation.Key.TradeSchool,            Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolProfit,               Translation.Key.TradeSchool,            Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeIncome,        Translation.Key.LiberalArtsCollege,     Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeExpenses,      Translation.Key.LiberalArtsCollege,     Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeProfit,        Translation.Key.LiberalArtsCollege,     Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityIncome,                Translation.Key.University,             Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityExpenses,              Translation.Key.University,             Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityProfit,                Translation.Key.University,             Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.TransportEconomy, Translation.Key.TransportEconomy));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalIncomePercent,         Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.PctOfCityIncome             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalIncome,                Translation.Key.Total,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalExpensesPercent,       Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.PctOfCityExpenses           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalExpenses,              Translation.Key.Total,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalProfit,                Translation.Key.Total,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusIncome,                  Translation.Key.Bus,                    Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusExpenses,                Translation.Key.Bus,                    Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusProfit,                  Translation.Key.Bus,                    Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusIncome,           Translation.Key.Trolleybus,             Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusExpenses,         Translation.Key.Trolleybus,             Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusProfit,           Translation.Key.Trolleybus,             Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramIncome,                 Translation.Key.Tram,                   Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramExpenses,               Translation.Key.Tram,                   Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramProfit,                 Translation.Key.Tram,                   Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroIncome,                Translation.Key.Metro,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroExpenses,              Translation.Key.Metro,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroProfit,                Translation.Key.Metro,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainIncome,                Translation.Key.Train,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainExpenses,              Translation.Key.Train,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainProfit,                Translation.Key.Train,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipIncome,                 Translation.Key.Ship,                   Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipExpenses,               Translation.Key.Ship,                   Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipProfit,                 Translation.Key.Ship,                   Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirIncome,                  Translation.Key.Air,                    Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirExpenses,                Translation.Key.Air,                    Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirProfit,                  Translation.Key.Air,                    Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailIncome,             Translation.Key.Monorail,               Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailExpenses,           Translation.Key.Monorail,               Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailProfit,             Translation.Key.Monorail,               Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarIncome,             Translation.Key.CableCar,               Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarExpenses,           Translation.Key.CableCar,               Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarProfit,             Translation.Key.CableCar,               Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiIncome,                 Translation.Key.Taxi,                   Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiExpenses,               Translation.Key.Taxi,                   Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiProfit,                 Translation.Key.Taxi,                   Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursIncome,                Translation.Key.Tours,                  Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursExpenses,              Translation.Key.Tours,                  Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursProfit,                Translation.Key.Tours,                  Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothIncome,            Translation.Key.TollBooth,              Translation.Key.Income,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothExpenses,          Translation.Key.TollBooth,              Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothProfit,            Translation.Key.TollBooth,              Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMailExpenses,               Translation.Key.Mail,                   Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMailProfit,                 Translation.Key.Mail,                   Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomySpaceElevatorExpenses,      Translation.Key.SpaceElevator,          Translation.Key.Expenses,       Translation.Key.MoneyPerWeek                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomySpaceElevatorProfit,        Translation.Key.SpaceElevator,          Translation.Key.Profit,         Translation.Key.MoneyPerWeek                ));

            _instance.Add(category = new Category(Category.CategoryType.GameLimits, Translation.Key.GameLimits));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsUsedPercent,             Translation.Key.Buildings,              Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsUsed,                    Translation.Key.Buildings,              Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsCapacity,                Translation.Key.Buildings,              Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensUsedPercent,              Translation.Key.Citizens,               Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensUsed,                     Translation.Key.Citizens,               Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensCapacity,                 Translation.Key.Citizens,               Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsUsedPercent,          Translation.Key.CitizenUnits,           Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsUsed,                 Translation.Key.CitizenUnits,           Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsCapacity,             Translation.Key.CitizenUnits,           Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesUsedPercent,      Translation.Key.CitizenInstances,       Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesUsed,             Translation.Key.CitizenInstances,       Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesCapacity,         Translation.Key.CitizenInstances,       Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersUsedPercent,             Translation.Key.Disasters,              Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersUsed,                    Translation.Key.Disasters,              Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersCapacity,                Translation.Key.Disasters,              Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsUsedPercent,             Translation.Key.Districts,              Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsUsed,                    Translation.Key.Districts,              Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsCapacity,                Translation.Key.Districts,              Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsUsedPercent,                Translation.Key.Events,                 Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsUsed,                       Translation.Key.Events,                 Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsCapacity,                   Translation.Key.Events,                 Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasUsedPercent,             Translation.Key.GameAreas,              Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasUsed,                    Translation.Key.GameAreas,              Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasCapacity,                Translation.Key.GameAreas,              Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesUsedPercent,          Translation.Key.NetworkLanes,           Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesUsed,                 Translation.Key.NetworkLanes,           Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesCapacity,             Translation.Key.NetworkLanes,           Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesUsedPercent,          Translation.Key.NetworkNodes,           Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesUsed,                 Translation.Key.NetworkNodes,           Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesCapacity,             Translation.Key.NetworkNodes,           Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsUsedPercent,       Translation.Key.NetworkSegments,        Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsUsed,              Translation.Key.NetworkSegments,        Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsCapacity,          Translation.Key.NetworkSegments,        Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPaintedAreasUsedPercent,          Translation.Key.PaintedAreas,           Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPaintedAreasUsed,                 Translation.Key.PaintedAreas,           Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPaintedAreasCapacity,             Translation.Key.PaintedAreas,           Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsUsedPercent,             Translation.Key.PathUnits,              Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsUsed,                    Translation.Key.PathUnits,              Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsCapacity,                Translation.Key.PathUnits,              Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsUsedPercent,                 Translation.Key.Props,                  Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsUsed,                        Translation.Key.Props,                  Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsCapacity,                    Translation.Key.Props,                  Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsUsedPercent,         Translation.Key.RadioChannels,          Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsUsed,                Translation.Key.RadioChannels,          Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsCapacity,            Translation.Key.RadioChannels,          Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsUsedPercent,         Translation.Key.RadioContents,          Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsUsed,                Translation.Key.RadioContents,          Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsCapacity,            Translation.Key.RadioContents,          Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesUsedPercent,        Translation.Key.TransportLines,         Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesUsed,               Translation.Key.TransportLines,         Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesCapacity,           Translation.Key.TransportLines,         Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesUsedPercent,                 Translation.Key.Trees,                  Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesUsed,                        Translation.Key.Trees,                  Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesCapacity,                    Translation.Key.Trees,                  Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesUsedPercent,              Translation.Key.Vehicles,               Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesUsed,                     Translation.Key.Vehicles,               Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesCapacity,                 Translation.Key.Vehicles,               Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedUsedPercent,        Translation.Key.VehiclesParked,         Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedUsed,               Translation.Key.VehiclesParked,         Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedCapacity,           Translation.Key.VehiclesParked,         Translation.Key.Capacity,       Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksUsedPercent,            Translation.Key.ZoneBlocks,             Translation.Key.Used,           Translation.Key.PctOfCapacity               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksUsed,                   Translation.Key.ZoneBlocks,             Translation.Key.Used,           Translation.Key.Amount                      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksCapacity,               Translation.Key.ZoneBlocks,             Translation.Key.Capacity,       Translation.Key.Amount                      ));

            #endregion


            // verify each category type is created exactly once
            foreach (Category.CategoryType categoryType in Enum.GetValues(typeof(Category.CategoryType)))
            {
                int found = 0;
                foreach (Category cat in _instance)
                {
                    if (categoryType == cat.Type)
                    {
                        found++;
                    }
                }
                if (found != 1)
                {
                    LogUtil.LogError($"Category type [{categoryType}] is created {found} times, but should be created exactly once.");
                }
            }

            // verify statistic types
            Statistic.StatisticType[] statisticTypes = (Statistic.StatisticType[])Enum.GetValues(typeof(Statistic.StatisticType));
            foreach (Statistic.StatisticType statisticType in statisticTypes)
            {
                // verify each statistic type is created exactly once in all categories
                int found = 0;
                foreach (Category cat in _instance)
                {
                    foreach (Statistic statistic in cat.Statistics)
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

            // verify every field and property in the snapshot (except SnapshotDateTime) has a statistic type
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
                if (fieldPropertyName != "SnapshotDateTime")
                {
                    bool found = false;
                    foreach (Statistic.StatisticType statisticType in statisticTypes)
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
        }

        /// <summary>
        /// deinitialize categories
        /// </summary>
        public void Deinitialize()
        {
            _instance.Clear();
        }

        /// <summary>
        /// return the count of selected statistics
        /// </summary>
        public int CountSelected
        {
            get
            {
                int count = 0;
                foreach (Category category in _instance)
                {
                    count += category.Statistics.CountSelected;
                }
                return count;
            }
        }

        /// <summary>
        /// return selected statistics
        /// </summary>
        public Statistics SelectedStatistics
        {
            get
            {
                Statistics selectedStatistics = new Statistics();
                foreach (Category category in _instance)
                {
                    foreach (Statistic statistic in category.Statistics)
                    {
                        if (statistic.Selected)
                        {
                            selectedStatistics.Add(statistic);
                        }
                    }
                }
                return selectedStatistics;
            }
        }

        /// <summary>
        /// return all statistics
        /// </summary>
        public Statistics AllStatistics
        {
            get
            {
                Statistics allStatistics = new Statistics();
                foreach (Category category in _instance)
                {
                    foreach (Statistic statistic in category.Statistics)
                    {
                        allStatistics.Add(statistic);
                    }
                }
                return allStatistics;
            }
        }

        /// <summary>
        /// create UI
        /// </summary>
        public bool CreateUI(UIScrollablePanel categoriesScrollablePanel)
        {
            // define statistic colors once
            Statistic.DefineColors();

            // create UI for each category
            foreach (Category category in _instance)
            {
                if (!category.CreateUI(categoriesScrollablePanel))
                {
                    return false;
                }
            }

            // success
            return true;
        }

        /// <summary>
        /// update UI text
        /// </summary>
        public void UpdateUIText()
        {
            foreach (Category category in _instance)
            {
                category.UpdateUIText();
            }
        }

        /// <summary>
        /// update all statistic amounts
        /// </summary>
        public void UpdateStatisticAmounts(Snapshot snapshot)
        {
            foreach (Category category in _instance)
            {
                category.UpdateStatisticAmounts(snapshot);
            }
        }

        /// <summary>
        /// expand all categories
        /// </summary>
        public void ExpandAll()
        {
            foreach (Category category in _instance)
            {
                category.Expanded = true;
            }
        }

        /// <summary>
        /// collapse all categories
        /// </summary>
        public void CollapseAll()
        {
            foreach (Category category in _instance)
            {
                category.Expanded = false;
            }
        }

        /// <summary>
        /// deselect all statistics
        /// </summary>
        public void DeselectAllStatistics()
        {
            foreach (Category category in _instance)
            {
                foreach (Statistic statistic in category.Statistics)
                {
                    statistic.Selected = false;
                }
            }
        }

        /// <summary>
        /// write the categories to the game save file
        /// </summary>
        public void Serialize(BinaryWriter writer)
        {
            // write each category
            foreach (Category category in _instance)
            {
                category.Serialize(writer);
            }
        }

        /// <summary>
        /// read the categories from the game save file
        /// </summary>
        public void Deserialize(BinaryReader reader, int version)
        {
            // read each category
            foreach (Category category in _instance)
            {
                category.Deserialize(reader, version);
            }
        }
    }
}
