using ColossalFramework.UI;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

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
            // define statistic colors
            // generally for colors:
            //      colors are lighter for usage and darker for capacity
            //      usage percent color is same as usage amount color because it is expected that percent and amount will not be shown at the same time and using the same color reduces the number of unique colors to be defined
            #region Colors

            // define multiplier to make a color darker
            const float DarkerMultipler = 0.7f;

            // get colors from InfoManager
            if (!InfoManager.exists)
            {
                LogUtil.LogError("InfoManager not ready.");
                return;
            }
            Color colorNeutral1 = InfoManager.instance.m_properties.m_neutralColor; Color colorNeutral2 = colorNeutral1 * DarkerMultipler;
            InfoProperties.ModeProperties[] modeProperties = InfoManager.instance.m_properties.m_modeProperties;
            Color colorInfoTrafficTarget  = modeProperties[(int)InfoManager.InfoMode.Traffic       ].m_targetColor;
            Color colorInfoPollution      = modeProperties[(int)InfoManager.InfoMode.Pollution     ].m_activeColor;
            Color colorInfoNoisePollution = modeProperties[(int)InfoManager.InfoMode.NoisePollution].m_activeColor;
            Color colorInfoLandValue      = modeProperties[(int)InfoManager.InfoMode.LandValue     ].m_activeColor;
            Color colorInfoHeating1       = modeProperties[(int)InfoManager.InfoMode.Heating       ].m_targetColor; Color32 colorInfoHeating2 = colorInfoHeating1 * DarkerMultipler;
            Color colorInfoTourism        = modeProperties[(int)InfoManager.InfoMode.Tourism       ].m_activeColor;

            // get colors from ZoneManager
            if (!ZoneManager.exists)
            {
                LogUtil.LogError("ZoneManager not ready.");
                return;
            }
            Color[] zoneColors = ZoneManager.instance.m_properties.m_zoneColors;
            Color colorZoneResidentialLow  = zoneColors[(int)ItemClass.Zone.ResidentialLow ];
            Color colorZoneResidentialHigh = zoneColors[(int)ItemClass.Zone.ResidentialHigh];
            Color colorZoneCommercialLow   = zoneColors[(int)ItemClass.Zone.CommercialLow  ];
            Color colorZoneCommercialHigh  = zoneColors[(int)ItemClass.Zone.CommercialHigh ];
            Color colorZoneIndustrial      = zoneColors[(int)ItemClass.Zone.Industrial     ];
            Color colorZoneOffice          = zoneColors[(int)ItemClass.Zone.Office         ];
            Color32 colorZoneUnzoned       = new Color32(152, 106, 65, 255);  // color taken manually from De-Zone tool icon

            // make mid colors halfway between low and high colors
            Color colorZoneResidentialMid = Color.Lerp(colorZoneResidentialLow, colorZoneResidentialHigh, 0.5f);
            Color colorZoneCommercialMid  = Color.Lerp(colorZoneCommercialLow,  colorZoneCommercialHigh,  0.5f);


            // get colors from TransportManager
            if (!TransportManager.exists)
            {
                LogUtil.LogError("TransportManager not ready.");
                return;
            }
            Color[] transportColors = TransportManager.instance.m_properties.m_transportColors;
            Color32 colorTransportBus1           = transportColors[(int)TransportInfo.TransportType.Bus          ]; Color32 colorTransportBus2           = colorTransportBus1          .Multiply(DarkerMultipler); Color32 colorTransportBus3        = colorTransportBus2       .Multiply(DarkerMultipler);
            Color32 colorTransportTrolleybus1    = transportColors[(int)TransportInfo.TransportType.Trolleybus   ]; Color32 colorTransportTrolleybus2    = colorTransportTrolleybus1   .Multiply(DarkerMultipler); Color32 colorTransportTrolleybus3 = colorTransportTrolleybus2.Multiply(DarkerMultipler);
            Color32 colorTransportTram1          = transportColors[(int)TransportInfo.TransportType.Tram         ]; Color32 colorTransportTram2          = colorTransportTram1         .Multiply(DarkerMultipler); Color32 colorTransportTram3       = colorTransportTram2      .Multiply(DarkerMultipler);
            Color32 colorTransportMetro1         = transportColors[(int)TransportInfo.TransportType.Metro        ]; Color32 colorTransportMetro2         = colorTransportMetro1        .Multiply(DarkerMultipler); Color32 colorTransportMetro3      = colorTransportMetro2     .Multiply(DarkerMultipler);
            Color32 colorTransportTrain1         = transportColors[(int)TransportInfo.TransportType.Train        ]; Color32 colorTransportTrain2         = colorTransportTrain1        .Multiply(DarkerMultipler); Color32 colorTransportTrain3      = colorTransportTrain2     .Multiply(DarkerMultipler);
            Color32 colorTransportShip1          = transportColors[(int)TransportInfo.TransportType.Ship         ]; Color32 colorTransportShip2          = colorTransportShip1         .Multiply(DarkerMultipler); Color32 colorTransportShip3       = colorTransportShip2      .Multiply(DarkerMultipler);
            Color32 colorTransportAir1           = transportColors[(int)TransportInfo.TransportType.Airplane     ]; Color32 colorTransportAir2           = colorTransportAir1          .Multiply(DarkerMultipler); Color32 colorTransportAir3        = colorTransportAir2       .Multiply(DarkerMultipler);
            Color32 colorTransportMonorail1      = transportColors[(int)TransportInfo.TransportType.Monorail     ]; Color32 colorTransportMonorail2      = colorTransportMonorail1     .Multiply(DarkerMultipler); Color32 colorTransportMonorail3   = colorTransportMonorail2  .Multiply(DarkerMultipler);
            Color32 colorTransportCableCar1      = transportColors[(int)TransportInfo.TransportType.CableCar     ]; Color32 colorTransportCableCar2      = colorTransportCableCar1     .Multiply(DarkerMultipler); Color32 colorTransportCableCar3   = colorTransportCableCar2  .Multiply(DarkerMultipler);
            Color32 colorTransportTaxi1          = transportColors[(int)TransportInfo.TransportType.Taxi         ]; Color32 colorTransportTaxi2          = colorTransportTaxi1         .Multiply(DarkerMultipler); Color32 colorTransportTaxi3       = colorTransportTaxi2      .Multiply(DarkerMultipler);
            Color32 colorTransportPedestrian1    = transportColors[(int)TransportInfo.TransportType.Pedestrian   ]; Color32 colorTransportPedestrian2    = colorTransportPedestrian1   .Multiply(DarkerMultipler); Color32 colorTransportPedestrian3 = colorTransportPedestrian2.Multiply(DarkerMultipler);
            Color32 colorTransportTouristBus1    = transportColors[(int)TransportInfo.TransportType.TouristBus   ]; Color32 colorTransportTouristBus2    = colorTransportTouristBus1   .Multiply(DarkerMultipler);
            Color32 colorTransportHotAirBalloon1 = transportColors[(int)TransportInfo.TransportType.HotAirBalloon]; Color32 colorTransportHotAirBalloon2 = colorTransportHotAirBalloon1.Multiply(DarkerMultipler);

            // get colors from TransferManager
            if (!TransferManager.exists)
            {
                LogUtil.LogError("TransferManager not ready.");
                return;
            }
            Color[] transferColors = TransferManager.instance.m_properties.m_resourceColors;
            Color32 colorTransferGoods1    = transferColors[(int)TransferManager.TransferReason.Goods]; Color32 colorTransferGoods2    = colorTransferGoods1   .Multiply(DarkerMultipler);
            Color32 colorTransferForestry1 = transferColors[(int)TransferManager.TransferReason.Logs ]; Color32 colorTransferForestry2 = colorTransferForestry1.Multiply(DarkerMultipler); Color32 colorTransferForestry3 = colorTransferForestry2.Multiply(DarkerMultipler);
            Color32 colorTransferFarming1  = transferColors[(int)TransferManager.TransferReason.Grain]; Color32 colorTransferFarming2  = colorTransferFarming1 .Multiply(DarkerMultipler); Color32 colorTransferFarming3  = colorTransferFarming2 .Multiply(DarkerMultipler);
            Color32 colorTransferOre1      = transferColors[(int)TransferManager.TransferReason.Ore  ]; Color32 colorTransferOre2      = colorTransferOre1     .Multiply(DarkerMultipler); Color32 colorTransferOre3      = colorTransferOre2     .Multiply(DarkerMultipler);
            Color32 colorTransferOil1      = transferColors[(int)TransferManager.TransferReason.Oil  ];
            Color32 colorTransferMail1     = transferColors[(int)TransferManager.TransferReason.Mail ]; Color32 colorTransferMail2     = colorTransferMail1    .Multiply(DarkerMultipler); Color32 colorTransferMail3     = colorTransferMail2    .Multiply(DarkerMultipler);
            Color32 colorTransferFish1     = transferColors[(int)TransferManager.TransferReason.Fish ]; Color32 colorTransferFish2     = colorTransferFish1    .Multiply(DarkerMultipler); Color32 colorTransferFish3     = colorTransferFish2    .Multiply(DarkerMultipler);

            // make oil lighter because the original color is very dark
            colorTransferOil1 = colorTransferOil1.Multiply(1.4f);
            Color32 colorTransferOil2 = colorTransferOil1.Multiply(DarkerMultipler);
            Color32 colorTransferOil3 = colorTransferOil2.Multiply(DarkerMultipler);

            // get colors from NaturalResourceManager
            if (!NaturalResourceManager.exists)
            {
                LogUtil.LogError("NaturalResourceManager not ready.");
                return;
            }
            Color[] resourceColors = NaturalResourceManager.instance.m_properties.m_resourceColors;
            Color32 colorResourceForestry1  = resourceColors[(int)NaturalResourceManager.Resource.Forest   ]; Color32 colorResourceForestry2  = colorResourceForestry1 .Multiply(DarkerMultipler);
            Color32 colorResourceFertility1 = resourceColors[(int)NaturalResourceManager.Resource.Fertility]; Color32 colorResourceFertility2 = colorResourceFertility1.Multiply(DarkerMultipler);
            Color32 colorResourceOre1       = resourceColors[(int)NaturalResourceManager.Resource.Ore      ]; Color32 colorResourceOre2       = colorResourceOre1      .Multiply(DarkerMultipler);
            Color32 colorResourceOil1       = resourceColors[(int)NaturalResourceManager.Resource.Oil      ];

            // make oil lighter because the original color is very dark
            colorResourceOil1 = colorResourceOil1.Multiply(1.5f);
            Color32 colorResourceOil2 = colorResourceOil1.Multiply(DarkerMultipler);

            // get colors from EducationInfoViewPanel
            EducationInfoViewPanel educationInfoViewPanel = UIView.library.Get<EducationInfoViewPanel>(typeof(EducationInfoViewPanel).Name);
            if (educationInfoViewPanel == null)
            {
                LogUtil.LogError("Unable to find EducationInfoViewPanel.");
                return;
            }
            Color32 colorUneducated1     = educationInfoViewPanel.m_UneducatedColor;     Color32 colorUneducated2     = colorUneducated1    .Multiply(DarkerMultipler);
            Color32 colorEducated1       = educationInfoViewPanel.m_EducatedColor;       Color32 colorEducated2       = colorEducated1      .Multiply(DarkerMultipler);
            Color32 colorWellEducated1   = educationInfoViewPanel.m_WellEducatedColor;   Color32 colorWellEducated2   = colorWellEducated1  .Multiply(DarkerMultipler);
            Color32 colorHighlyEducated1 = educationInfoViewPanel.m_HighlyEducatedColor; Color32 colorHighlyEducated2 = colorHighlyEducated1.Multiply(DarkerMultipler);

            // get colors from PopulationInfoViewPanel
            PopulationInfoViewPanel populationInfoViewPanel = UIView.library.Get<PopulationInfoViewPanel>(typeof(PopulationInfoViewPanel).Name);
            if (populationInfoViewPanel == null)
            {
                LogUtil.LogError("Unable to find PopulationInfoViewPanel.");
                return;
            }
            Color32 colorChild  = populationInfoViewPanel.m_ChildColor;
            Color32 colorTeen   = populationInfoViewPanel.m_TeenColor;
            Color32 colorYoung  = populationInfoViewPanel.m_YoungColor;
            Color32 colorAdult  = populationInfoViewPanel.m_AdultColor;
            Color32 colorSenior = populationInfoViewPanel.m_SeniorColor;

            // get colors from TourismInfoViewPanel
            TourismInfoViewPanel tourismInfoViewPanel = UIView.library.Get<TourismInfoViewPanel>(typeof(TourismInfoViewPanel).Name);
            if (tourismInfoViewPanel == null)
            {
                LogUtil.LogError("Unable to find TourismInfoViewPanel.");
                return;
            }
            UIRadialChart touristWealthChart   = tourismInfoViewPanel.Find<UIRadialChart>("TouristWealthChart");
            UIRadialChart exchangeStudentChart = tourismInfoViewPanel.Find<UIRadialChart>("ExchangeStudentChart");
            Color32 colorTouristsLowWealth1    = touristWealthChart.GetSlice(0).innerColor; Color32 colorTouristsLowWealth2    = colorTouristsLowWealth1   .Multiply(DarkerMultipler);
            Color32 colorTouristsMediumWealth1 = touristWealthChart.GetSlice(1).innerColor; Color32 colorTouristsMediumWealth2 = colorTouristsMediumWealth1.Multiply(DarkerMultipler);
            Color32 colorTouristsHighWealth1   = touristWealthChart.GetSlice(2).innerColor; Color32 colorTouristsHighWealth2   = colorTouristsHighWealth1  .Multiply(DarkerMultipler);
            Color32 colorExchangeStudent       = exchangeStudentChart.GetSlice(0).innerColor;
            Color32 colorTouristsTotal         = exchangeStudentChart.GetSlice(1).innerColor;

            // get colors from StatisticsPanel
            // statistic color index is same as statistic name index
            StatisticsPanel statisticsPanel = UIView.library.Get<StatisticsPanel>(typeof(StatisticsPanel).Name);
            if (statisticsPanel == null)
            {
                LogUtil.LogError("Unable to find StatisticsPanel.");
                return;
            }
            FieldInfo fiStatisticsNames = typeof(StatisticsPanel).GetField("StatisticsNames", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fiStatisticsNames == null)
            {
                LogUtil.LogError("Unable to find StatisticsPanel.StatisticsNames.");
                return;
            }
            string[] statisticsNames = (string[])fiStatisticsNames.GetValue(statisticsPanel);
            if (statisticsNames == null)
            {
                LogUtil.LogError("Unable to get StatisticsPanel.StatisticsNames.");
                return;
            }
            Color32 colorStatisticsHappiness  = new Color32(0, 0, 0, 0);
            Color32 colorStatisticsBirthRate  = new Color32(0, 0, 0, 0);
            Color32 colorStatisticsDeathRate  = new Color32(0, 0, 0, 0);
            Color32 colorStatisticsPopulation = new Color32(0, 0, 0, 0);
            Color32 colorstatisticsEmployment = new Color32(0, 0, 0, 0);
            Color32 colorstatisticsJobs       = new Color32(0, 0, 0, 0);
            for (int i = 0; i < statisticsNames.Length; i++)
            {
                switch (statisticsNames[i])
                {
                    case "Happiness":  colorStatisticsHappiness  = statisticsPanel.StatisticsColors[i]; break;
                    case "Birth":      colorStatisticsBirthRate  = statisticsPanel.StatisticsColors[i]; break;
                    case "Death":      colorStatisticsDeathRate  = statisticsPanel.StatisticsColors[i]; break;
                    case "Population": colorStatisticsPopulation = statisticsPanel.StatisticsColors[i]; break;
                    case "Employment": colorstatisticsEmployment = statisticsPanel.StatisticsColors[i]; break;
                    case "Jobs":       colorstatisticsJobs       = statisticsPanel.StatisticsColors[i]; break;
                }
            }

            // define colors for specific statistics
            Color32 colorElectricity1 = new Color32(002, 168, 254, 255);           // color taken manually from Electricity info view icon
            Color32 colorElectricity2 = new Color32(045, 098, 143, 255);           // color taken manually from Electricity info view icon

            Color32 colorWater1 = new Color32(134, 187, 241, 255);                 // color taken manually from Water info view icon
            Color32 colorWater2 = new Color32(023, 113, 206, 255);                 // color taken manually from Water info view icon

            Color32 colorWaterTank1 = colorWater1.Multiply(DarkerMultipler);      // darker version of Water
            Color32 colorWaterTank2 = colorWater2.Multiply(DarkerMultipler);      // darker version of Water

            Color32 colorSewage1 = new Color32(038, 153, 134, 255);                // color taken manually from WaterInfoViewPanel sewage section
            Color32 colorSewage2 = colorSewage1.Multiply(DarkerMultipler);

            Color32 colorGarbage1 = new Color32(163, 152, 000, 255);               // color taken manually from Garbage info view icon
            Color32 colorGarbage2 = new Color32(091, 076, 069, 255);               // color taken manually from Garbage info view icon

            Color32 colorLandfill1 = colorGarbage1.Multiply(1.4f);                // lighter version of Garbage
            Color32 colorLandfill2 = colorGarbage2.Multiply(1.4f);                // lighter version of Garbage

            Color32 colorHealthcareAverage = new Color32(013, 166, 066, 255);      // color taken manually from HealthInfoViewPanel Efficiency legend
            Color32 colorHealthcare1 = new Color32(248, 021, 013, 255);            // color taken manually from Healthcare toolbar icon
            Color32 colorHealthcare2 = colorHealthcare1.Multiply(DarkerMultipler);

            Color32 colorDeathcare1 = new Color32(255, 000, 128, 255);             // a shade of red
            Color32 colorDeathcare2 = colorDeathcare1.Multiply(DarkerMultipler);
            Color32 colorDeathcare3 = new Color32(255, 128, 128, 255);             // another shade of red
            Color32 colorDeathcare4 = colorDeathcare3.Multiply(DarkerMultipler);

            // color is half way between Child and Teen
            Color32 colorChildcare1 = new Color32((byte)(colorChild.r / 2 + colorTeen.r / 2), (byte)(colorChild.g / 2 + colorTeen.g / 2), (byte)(colorChild.b / 2 + colorTeen.b / 2), 255);
            Color32 colorChildcare2 = colorChildcare1.Multiply(DarkerMultipler);

            Color32 colorEldercare1 = colorSenior.Multiply(1.25f);                // lighter version of Senior
            Color32 colorEldercare2 = colorEldercare1.Multiply(DarkerMultipler);

            // logic for levels colors is copied from LevelsInfoViewPanel.UpdatePanel for initializing the radial chart colors
            // basically start with a darker (i.e. 70%) version of each zone color and then interpolate between neutral color and that color
            Color colorLevelResidential = colorZoneResidentialMid * 0.7f;
            Color colorLevelCommercial  = colorZoneCommercialMid * 0.7f;
            Color colorLevelIndustrial  = colorZoneIndustrial * 0.7f;
            Color colorLevelOffice      = colorZoneOffice * 0.7f;
            Color colorResidentialLevel1 = Color.Lerp(colorNeutral1, colorLevelResidential, 0.200f);
            Color colorResidentialLevel2 = Color.Lerp(colorNeutral1, colorLevelResidential, 0.400f);
            Color colorResidentialLevel3 = Color.Lerp(colorNeutral1, colorLevelResidential, 0.600f);
            Color colorResidentialLevel4 = Color.Lerp(colorNeutral1, colorLevelResidential, 0.800f);
            Color colorResidentialLevel5 = Color.Lerp(colorNeutral1, colorLevelResidential, 1.000f);
            Color colorCommercialLevel1  = Color.Lerp(colorNeutral1, colorLevelCommercial,  0.333f);
            Color colorCommercialLevel2  = Color.Lerp(colorNeutral1, colorLevelCommercial,  0.667f);
            Color colorCommercialLevel3  = Color.Lerp(colorNeutral1, colorLevelCommercial,  1.000f);
            Color colorIndustrialLevel1  = Color.Lerp(colorNeutral1, colorLevelIndustrial,  0.333f);
            Color colorIndustrialLevel2  = Color.Lerp(colorNeutral1, colorLevelIndustrial,  0.667f);
            Color colorIndustrialLevel3  = Color.Lerp(colorNeutral1, colorLevelIndustrial,  1.000f);
            Color colorOfficeLevel1      = Color.Lerp(colorNeutral1, colorLevelOffice,      0.333f);
            Color colorOfficeLevel2      = Color.Lerp(colorNeutral1, colorLevelOffice,      0.667f);
            Color colorOfficeLevel3      = Color.Lerp(colorNeutral1, colorLevelOffice,      1.000f);

            Color32 colorFireSafety = new Color32(232, 159, 056, 255);             // color taken manually from Fire Department toolbar icon

            Color32 colorCrimeRate = new Color32(128, 128, 128, 255);              // a shade of gray
            Color32 colorCrime1    = new Color32(169, 095, 002, 255);              // color taken manually from Police Department toolbar icon
            Color32 colorCrime2    = colorCrime1.Multiply(DarkerMultipler);

            Color32 colorTransportTotal1 = new Color32(206, 248, 000, 255);        // color taken manually from TransportInfoViewPanel for Total text
            Color32 colorTransportTotal2 = colorTransportTotal1.Multiply(DarkerMultipler);

            Color32 colorHouseholds1 = new Color32(206, 248, 000, 255);            // color taken manually from CityInfoPanel for households text
            Color32 colorHouseholds2 = colorHouseholds1.Multiply(DarkerMultipler);

            Color32 colorEmployment1 = colorstatisticsJobs;
            Color32 colorEmployment2 = colorEmployment1.Multiply(DarkerMultipler);
            Color32 colorEmployment3 = colorEmployment2.Multiply(DarkerMultipler);
            Color32 colorUnemployment1 = colorstatisticsEmployment;
            Color32 colorUnemployment2 = colorUnemployment1.Multiply(DarkerMultipler);

            Color32 colorTransferTotal1 = new Color32(128, 128, 128, 255);         // a shade of gray
            Color32 colorTransferTotal2 = colorTransferTotal1.Multiply(DarkerMultipler);

            Color32 colorCityTotalIncome   = new Color32(090, 225, 020, 255);      // color taken manually from EconomyPanel
            Color32 colorCityTotalExpenses = new Color32(254, 150, 089, 255);      // color taken manually from EconomyPanel
            Color32 colorCityTotalProfit   = new Color32((byte)(colorCityTotalIncome.r / 2 + colorCityTotalExpenses.r / 2),     // color is halfway between income and expenses
                                                         (byte)(colorCityTotalIncome.g / 2 + colorCityTotalExpenses.g / 2),
                                                         (byte)(colorCityTotalIncome.b / 2 + colorCityTotalExpenses.b / 2), 255);
            Color32 colorBankBalance       = new Color32(185, 221, 254, 255);      // color taken manually from InfoPanel.IncomePanel

            Color32 colorIncomeSelfSufficient = new Color32(118, 234, 122, 255);   // color taken manually from EconomyPanel

            Color32 colorIncomeLeisure = new Color32(135, 209, 218, 255);          // color taken manually from specialized district icon
            Color32 colorTourismIncome = new Color32(242, 219, 057, 255);          // color taken manually from specialized district icon
            Color32 colorIncomeOrganic = new Color32(132, 159, 000, 255);          // color taken manually from specialized district icon

            Color32 colorIncomeITCluster = new Color32(039, 192, 231, 255);        // color taken manually from specialized district icon

            Color32 colorParks = new Color32(073, 115, 122, 255);                  // color taken manually from the horse of the Parks & Plazas toolbar icon

            Color32 colorEmergency           = new Color32(254, 131, 000, 255);    // color taken manually from Landscape toolbar Disaster tab icon
            Color32 colorUniqueBuildings     = new Color32(082, 108, 113, 255);    // color taken manually from Unique Buildings toolbar icon
            Color32 colorGenericSportsArenas = new Color32(076, 108, 173, 255);    // color taken manually from Education toolbar Varsity Sports tab icon
            Color32 colorEconomy             = new Color32(061, 159, 010, 255);    // color taken manually from Economy toolbar icon
            Color32 colorPolicies            = new Color32(208, 210, 211, 255);    // color taken manually from Policies toolbar icon

            Color32 colorCityPark1      = new Color32(244, 223, 168, 255);         // color taken manually from City Park main gate arch
            Color32 colorCityPark2      = colorCityPark1.Multiply(DarkerMultipler);
            Color32 colorCityPark3      = colorCityPark2.Multiply(DarkerMultipler);
            Color32 colorAmusementPark1 = new Color32(204, 136, 083, 255);         // color taken manually from Amusement Park main gate path
            Color32 colorAmusementPark2 = colorAmusementPark1.Multiply(DarkerMultipler);
            Color32 colorAmusementPark3 = colorAmusementPark2.Multiply(DarkerMultipler);
            Color32 colorZoo1           = new Color32(221, 185, 110, 255);         // color taken manually from Zoo main gate path
            Color32 colorZoo2           = colorZoo1.Multiply(DarkerMultipler);
            Color32 colorZoo3           = colorZoo2.Multiply(DarkerMultipler);
            Color32 colorNatureReserve1 = new Color32(098, 145, 078, 255);         // color taken manually from Nature Reserve main gate building roof
            Color32 colorNatureReserve2 = colorNatureReserve1.Multiply(DarkerMultipler);
            Color32 colorNatureReserve3 = colorNatureReserve2.Multiply(DarkerMultipler);

            Color32 colorTradeSchool1 = new Color32(232, 216, 172, 255);           // color taken manually from Trade School administration building roof
            Color32 colorTradeSchool2 = colorTradeSchool1.Multiply(DarkerMultipler);
            Color32 colorTradeSchool3 = colorTradeSchool2.Multiply(DarkerMultipler);
            Color32 colorLiberalArts1 = new Color32(241, 181, 113, 255);           // color taken manually from Liberal Arts College administration building roof
            Color32 colorLiberalArts2 = colorLiberalArts1.Multiply(DarkerMultipler);
            Color32 colorLiberalArts3 = colorLiberalArts2.Multiply(DarkerMultipler);
            Color32 colorUniversity1  = new Color32(172, 208, 203, 255);           // color taken manually from University administration building roof
            Color32 colorUniversity2  = colorUniversity1.Multiply(DarkerMultipler);
            Color32 colorUniversity3  = colorUniversity2.Multiply(DarkerMultipler);

            Color32 colorTollBooth1 = new Color32(183, 148, 205, 255);             // color taken manually from Road toolbar Toll Booth icon car
            Color32 colorTollBooth2 = colorTollBooth1.Multiply(DarkerMultipler);
            Color32 colorTollBooth3 = colorTollBooth2.Multiply(DarkerMultipler);

            Color32 colorSpaceElevator2 = new Color32(087, 254, 255, 255);         // color taken manually from Space Elevator rings
            Color32 colorSpaceElevator3 = colorSpaceElevator2.Multiply(DarkerMultipler);

            #endregion


            // initialize categories and statistics
            #region Categories and Statistics
            _instance.Clear();
            Category category;

            _instance.Add(category = new Category(Category.CategoryType.Electricity, Category.DescriptionKey.Electricity));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityConsumptionPercent,                  Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfProduction,             colorElectricity1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityConsumption,                         Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.MegaWatts,                   colorElectricity1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ElectricityProduction,                          Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.MegaWatts,                   colorElectricity2           ));

            _instance.Add(category = new Category(Category.CategoryType.Water, Category.DescriptionKey.Water));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterConsumptionPercent,                        Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPumpingCapacity,        colorWater1                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterConsumption,                               Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMetersPerWeek,          colorWater1                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterPumpingCapacity,                           Statistic.DescriptionKey.PumpingCapacity,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMetersPerWeek,          colorWater2                 ));

            _instance.Add(category = new Category(Category.CategoryType.WaterTank, Category.DescriptionKey.WaterTank));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankReservedPercent,                       Statistic.DescriptionKey.Reserved,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfStorageCapacity,        colorWaterTank1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankReserved,                              Statistic.DescriptionKey.Reserved,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMeters,                 colorWaterTank1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.WaterTankStorageCapacity,                       Statistic.DescriptionKey.StorageCapacity,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMeters,                 colorWaterTank2             ));

            _instance.Add(category = new Category(Category.CategoryType.Sewage, Category.DescriptionKey.Sewage));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageProductionPercent,                        Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfDrainingCapacity,       colorSewage1                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageProduction,                               Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMetersPerWeek,          colorSewage1                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.SewageDrainingCapacity,                         Statistic.DescriptionKey.DrainingCapacity,          Statistic.DescriptionKey.None,              Statistic.UnitsKey.CubicMetersPerWeek,          colorSewage2                ));

            _instance.Add(category = new Category(Category.CategoryType.Landfill, Category.DescriptionKey.Landfill));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillStoragePercent,                         Statistic.DescriptionKey.Storage,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCapacity,               colorLandfill1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillStorage,                                Statistic.DescriptionKey.Storage,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.Units,                       colorLandfill1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandfillCapacity,                               Statistic.DescriptionKey.Capacity,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.Units,                       colorLandfill2              ));

            _instance.Add(category = new Category(Category.CategoryType.Garbage, Category.DescriptionKey.Garbage));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProductionPercent,                       Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfProcessingCapacity,     colorGarbage1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProduction,                              Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.UnitsPerWeek,                colorGarbage1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GarbageProcessingCapacity,                      Statistic.DescriptionKey.ProcessingCapacity,        Statistic.DescriptionKey.None,              Statistic.UnitsKey.UnitsPerWeek,                colorGarbage2               ));

            _instance.Add(category = new Category(Category.CategoryType.Education, Category.DescriptionKey.Education));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryEligiblePercent,             Statistic.DescriptionKey.Elementary,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.PctOfCapacity,               colorUneducated1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryEligible,                    Statistic.DescriptionKey.Elementary,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.Students,                    colorUneducated1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationElementaryCapacity,                    Statistic.DescriptionKey.Elementary,                Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Students,                    colorUneducated2            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolEligiblePercent,             Statistic.DescriptionKey.HighSchool,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.PctOfCapacity,               colorEducated1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolEligible,                    Statistic.DescriptionKey.HighSchool,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.Students,                    colorEducated1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationHighSchoolCapacity,                    Statistic.DescriptionKey.HighSchool,                Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Students,                    colorEducated2              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityEligiblePercent,             Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.PctOfCapacity,               colorWellEducated1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityEligible,                    Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Eligible,          Statistic.UnitsKey.Students,                    colorWellEducated1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationUniversityCapacity,                    Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Students,                    colorWellEducated2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryUsersPercent,                   Statistic.DescriptionKey.PublicLibrary,             Statistic.DescriptionKey.Users,             Statistic.UnitsKey.PctOfCapacity,               colorHighlyEducated1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryUsers,                          Statistic.DescriptionKey.PublicLibrary,             Statistic.DescriptionKey.Users,             Statistic.UnitsKey.Visitors,                    colorHighlyEducated1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLibraryCapacity,                       Statistic.DescriptionKey.PublicLibrary,             Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Visitors,                    colorHighlyEducated2        ));

            _instance.Add(category = new Category(Category.CategoryType.EducationLevel, Category.DescriptionKey.EducationLevel));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelUneducatedPercent,                Statistic.DescriptionKey.Uneducated,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorUneducated1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelEducatedPercent,                  Statistic.DescriptionKey.Educated,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorEducated1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelWellEducatedPercent,              Statistic.DescriptionKey.WellEducated,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorWellEducated1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelHighlyEducatedPercent,            Statistic.DescriptionKey.HighlyEducated,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorHighlyEducated1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelUneducated,                       Statistic.DescriptionKey.Uneducated,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorUneducated1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelEducated,                         Statistic.DescriptionKey.Educated,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorEducated1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelWellEducated,                     Statistic.DescriptionKey.WellEducated,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorWellEducated1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EducationLevelHighlyEducated,                   Statistic.DescriptionKey.HighlyEducated,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorHighlyEducated1        ));

            _instance.Add(category = new Category(Category.CategoryType.Happiness, Category.DescriptionKey.Happiness));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessGlobal,                                Statistic.DescriptionKey.Global,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorStatisticsHappiness    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessResidential,                           Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessCommercial,                            Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessIndustrial,                            Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HappinessOffice,                                Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneOffice             ));

            _instance.Add(category = new Category(Category.CategoryType.Healthcare, Category.DescriptionKey.Healthcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareAverageHealth,                        Statistic.DescriptionKey.AverageHealth,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorHealthcareAverage      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareSickPercent,                          Statistic.DescriptionKey.Sick,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfHealCapacity,           colorHealthcare1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareSick,                                 Statistic.DescriptionKey.Sick,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorHealthcare1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HealthcareHealCapacity,                         Statistic.DescriptionKey.HealCapacity,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorHealthcare2            ));

            _instance.Add(category = new Category(Category.CategoryType.Deathcare, Category.DescriptionKey.Deathcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryBuriedPercent,                 Statistic.DescriptionKey.Cemetery,                  Statistic.DescriptionKey.Buried,            Statistic.UnitsKey.PctOfCapacity,               colorDeathcare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryBuried,                        Statistic.DescriptionKey.Cemetery,                  Statistic.DescriptionKey.Buried,            Statistic.UnitsKey.Citizens,                    colorDeathcare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCemeteryCapacity,                      Statistic.DescriptionKey.Cemetery,                  Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Citizens,                    colorDeathcare2             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumDeceasedPercent,            Statistic.DescriptionKey.Crematorium,               Statistic.DescriptionKey.Deceased,          Statistic.UnitsKey.PctOfCapacity,               colorDeathcare3             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumDeceased,                   Statistic.DescriptionKey.Crematorium,               Statistic.DescriptionKey.Deceased,          Statistic.UnitsKey.Citizens,                    colorDeathcare3             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareCrematoriumCapacity,                   Statistic.DescriptionKey.Crematorium,               Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Citizens,                    colorDeathcare4             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.DeathcareDeathRate,                             Statistic.DescriptionKey.DeathRate,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.CitizensPerWeek,             colorStatisticsDeathRate    ));

            _instance.Add(category = new Category(Category.CategoryType.Childcare, Category.DescriptionKey.Childcare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareAverageHealth,                         Statistic.DescriptionKey.AverageHealth,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorHealthcareAverage      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareSickPercent,                           Statistic.DescriptionKey.SickChildrenTeens,         Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfChildrenTeens,          colorChildcare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareSick,                                  Statistic.DescriptionKey.SickChildrenTeens,         Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorChildcare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcarePopulation,                            Statistic.DescriptionKey.ChildrenTeens,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorChildcare2             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ChildcareBirthRate,                             Statistic.DescriptionKey.BirthRate,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.CitizensPerWeek,             colorStatisticsBirthRate    ));

            _instance.Add(category = new Category(Category.CategoryType.Eldercare, Category.DescriptionKey.Eldercare));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareAverageHealth,                         Statistic.DescriptionKey.AverageHealth,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorHealthcareAverage      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareSickPercent,                           Statistic.DescriptionKey.SickSeniors,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfSeniors,                colorEldercare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareSick,                                  Statistic.DescriptionKey.SickSeniors,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorEldercare1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercarePopulation,                            Statistic.DescriptionKey.Seniors,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorEldercare2             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EldercareAverageLifeSpan,                       Statistic.DescriptionKey.AverageLifeSpan,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.Years,                       colorStatisticsDeathRate    ));

            _instance.Add(category = new Category(Category.CategoryType.Zoning, Category.DescriptionKey.Zoning));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningResidentialPercent,                       Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningCommercialPercent,                        Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningIndustrialPercent,                        Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningOfficePercent,                            Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningUnzonedPercent,                           Statistic.DescriptionKey.Unzoned,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneUnzoned            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningTotal,                                    Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningResidential,                              Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningCommercial,                               Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningIndustrial,                               Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningOffice,                                   Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoningUnzoned,                                  Statistic.DescriptionKey.Unzoned,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.Squares,                     colorZoneUnzoned            ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneLevel, Category.DescriptionKey.ZoneLevel));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidentialAverage,                    Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Average,           Statistic.UnitsKey.Level1To5,                   colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential1,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.PctOfResidential,            colorResidentialLevel1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential2,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.PctOfResidential,            colorResidentialLevel2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential3,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.PctOfResidential,            colorResidentialLevel3      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential4,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Level4,            Statistic.UnitsKey.PctOfResidential,            colorResidentialLevel4      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelResidential5,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.Level5,            Statistic.UnitsKey.PctOfResidential,            colorResidentialLevel5      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercialAverage,                     Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.Average,           Statistic.UnitsKey.Level1To3,                   colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial1,                           Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.PctOfCommercial,             colorCommercialLevel1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial2,                           Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.PctOfCommercial,             colorCommercialLevel2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelCommercial3,                           Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.PctOfCommercial,             colorCommercialLevel3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrialAverage,                     Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.Average,           Statistic.UnitsKey.Level1To3,                   colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial1,                           Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.PctOfIndustrial,             colorIndustrialLevel1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial2,                           Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.PctOfIndustrial,             colorIndustrialLevel2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelIndustrial3,                           Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.PctOfIndustrial,             colorIndustrialLevel3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOfficeAverage,                         Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.Average,           Statistic.UnitsKey.Level1To3,                   colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice1,                               Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.PctOfOffice,                 colorOfficeLevel1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice2,                               Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.PctOfOffice,                 colorOfficeLevel2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneLevelOffice3,                               Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.PctOfOffice,                 colorOfficeLevel3           ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneBuildings, Category.DescriptionKey.ZoneBuildings));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsResidentialPercent,                Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsCommercialPercent,                 Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsIndustrialPercent,                 Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsOfficePercent,                     Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsTotal,                             Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.HouseholdsPlusJobs,          colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsResidential,                       Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Households,                  colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsCommercial,                        Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Jobs,                        colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsIndustrial,                        Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Jobs,                        colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneBuildingsOffice,                            Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Jobs,                        colorZoneOffice             ));

            _instance.Add(category = new Category(Category.CategoryType.ZoneDemand, Category.DescriptionKey.ZoneDemand));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandResidential,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandCommercial,                           Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ZoneDemandIndustrialOffice,                     Statistic.DescriptionKey.IndustrialOffice,          Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneIndustrial         ));

            _instance.Add(category = new Category(Category.CategoryType.Traffic, Category.DescriptionKey.Traffic));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TrafficAverageFlow,                             Statistic.DescriptionKey.Average,                   Statistic.DescriptionKey.Flow,              Statistic.UnitsKey.Percent,                     colorInfoTrafficTarget      ));

            _instance.Add(category = new Category(Category.CategoryType.Pollution, Category.DescriptionKey.Pollution));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageGround,                         Statistic.DescriptionKey.Average,                   Statistic.DescriptionKey.Ground,            Statistic.UnitsKey.Percent,                     colorInfoPollution          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageDrinkingWater,                  Statistic.DescriptionKey.Average,                   Statistic.DescriptionKey.DrinkingWater,     Statistic.UnitsKey.Percent,                     colorInfoPollution          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PollutionAverageNoise,                          Statistic.DescriptionKey.Average,                   Statistic.DescriptionKey.Noise,             Statistic.UnitsKey.Percent,                     colorInfoNoisePollution     ));

            _instance.Add(category = new Category(Category.CategoryType.FireSafety, Category.DescriptionKey.FireSafety));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.FireSafetyHazard,                               Statistic.DescriptionKey.Hazard,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorFireSafety             ));

            _instance.Add(category = new Category(Category.CategoryType.Crime, Category.DescriptionKey.Crime));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeRate,                                      Statistic.DescriptionKey.Rate,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorCrimeRate              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeDetainedCriminalsPercent,                  Statistic.DescriptionKey.DetainedCriminals,         Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfJailsCapacity,          colorCrime1                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeDetainedCriminals,                         Statistic.DescriptionKey.DetainedCriminals,         Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorCrime1                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CrimeJailsCapacity,                             Statistic.DescriptionKey.JailsCapacity,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorCrime2                 ));

            _instance.Add(category = new Category(Category.CategoryType.PublicTransportation, Category.DescriptionKey.PublicTransportation));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalTotal,                 Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalResidents,             Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTotalTourists,              Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTotal2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusTotal,                   Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportBus1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusResidents,               Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportBus1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationBusTourists,                Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportBus2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusTotal,            Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTrolleybus1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusResidents,        Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTrolleybus1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrolleybusTourists,         Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTrolleybus2   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramTotal,                  Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTram1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramResidents,              Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTram1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTramTourists,               Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTram2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroTotal,                 Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportMetro1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroResidents,             Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportMetro1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMetroTourists,              Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportMetro2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainTotal,                 Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTrain1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainResidents,             Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTrain1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTrainTourists,              Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTrain2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipTotal,                  Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportShip1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipResidents,              Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportShip1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationShipTourists,               Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportShip2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirTotal,                   Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportAir1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirResidents,               Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportAir1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationAirTourists,                Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportAir2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailTotal,              Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportMonorail1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailResidents,          Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportMonorail1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationMonorailTourists,           Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportMonorail2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarTotal,              Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportCableCar1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarResidents,          Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportCableCar1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationCableCarTourists,           Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportCableCar2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiTotal,                  Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTaxi1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiResidents,              Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTaxi1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PublicTransportationTaxiTourists,               Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTaxi2         ));

            _instance.Add(category = new Category(Category.CategoryType.Population, Category.DescriptionKey.Population));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTotal,                                Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorStatisticsPopulation   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationChildrenPercent,                      Statistic.DescriptionKey.Children,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorChild                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTeensPercent,                         Statistic.DescriptionKey.Teens,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorTeen                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationYoungAdultsPercent,                   Statistic.DescriptionKey.YoungAdults,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorYoung                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationAdultsPercent,                        Statistic.DescriptionKey.Adults,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorAdult                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationSeniorsPercent,                       Statistic.DescriptionKey.Seniors,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfPopulation,             colorSenior                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationChildren,                             Statistic.DescriptionKey.Children,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorChild                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationTeens,                                Statistic.DescriptionKey.Teens,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorTeen                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationYoungAdults,                          Statistic.DescriptionKey.YoungAdults,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorYoung                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationAdults,                               Statistic.DescriptionKey.Adults,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorAdult                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.PopulationSeniors,                              Statistic.DescriptionKey.Seniors,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorSenior                 ));

            _instance.Add(category = new Category(Category.CategoryType.Households, Category.DescriptionKey.Households));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsOccupiedPercent,                      Statistic.DescriptionKey.Occupied,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfAvailable,              colorHouseholds1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsOccupied,                             Statistic.DescriptionKey.Occupied,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.Households,                  colorHouseholds1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HouseholdsAvailable,                            Statistic.DescriptionKey.Available,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.Households,                  colorHouseholds2            ));

            _instance.Add(category = new Category(Category.CategoryType.Employment, Category.DescriptionKey.Employment));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentPeopleEmployed,                       Statistic.DescriptionKey.PeopleEmployed,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorEmployment1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentJobsAvailable,                        Statistic.DescriptionKey.JobsAvailable,             Statistic.DescriptionKey.None,              Statistic.UnitsKey.Jobs,                        colorEmployment2            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnfilledJobs,                         Statistic.DescriptionKey.UnfilledJobs,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.Jobs,                        colorEmployment3            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnemploymentPercent,                  Statistic.DescriptionKey.Unemployment,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfEligible,               colorUnemployment1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentUnemployed,                           Statistic.DescriptionKey.Unemployed,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorUnemployment1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.EmploymentEligibleWorkers,                      Statistic.DescriptionKey.EligibleWorkers,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.Citizens,                    colorUnemployment2          ));

            _instance.Add(category = new Category(Category.CategoryType.OutsideConnections, Category.DescriptionKey.OutsideConnections));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportTotal,                  Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Total,             Statistic.UnitsKey.Units,                       colorTransferTotal1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportGoods,                  Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Goods,             Statistic.UnitsKey.Units,                       colorTransferGoods1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportForestry,               Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Forestry,          Statistic.UnitsKey.Units,                       colorTransferForestry1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportFarming,                Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Farming,           Statistic.UnitsKey.Units,                       colorTransferFarming1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportOre,                    Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Ore,               Statistic.UnitsKey.Units,                       colorTransferOre1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportOil,                    Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Oil,               Statistic.UnitsKey.Units,                       colorTransferOil1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsImportMail,                   Statistic.DescriptionKey.Import,                    Statistic.DescriptionKey.Mail,              Statistic.UnitsKey.Units,                       colorTransferMail1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportTotal,                  Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Total,             Statistic.UnitsKey.Units,                       colorTransferTotal2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportGoods,                  Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Goods,             Statistic.UnitsKey.Units,                       colorTransferGoods2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportForestry,               Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Forestry,          Statistic.UnitsKey.Units,                       colorTransferForestry2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportFarming,                Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Farming,           Statistic.UnitsKey.Units,                       colorTransferFarming2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportOre,                    Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Ore,               Statistic.UnitsKey.Units,                       colorTransferOre2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportOil,                    Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Oil,               Statistic.UnitsKey.Units,                       colorTransferOil2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportMail,                   Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Mail,              Statistic.UnitsKey.Units,                       colorTransferMail2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OutsideConnectionsExportFish,                   Statistic.DescriptionKey.Export,                    Statistic.DescriptionKey.Fish,              Statistic.UnitsKey.Units,                       colorTransferFish2          ));

            _instance.Add(category = new Category(Category.CategoryType.LandValue, Category.DescriptionKey.LandValue));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.LandValueAverage,                               Statistic.DescriptionKey.Average,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerSquareMeter,         colorInfoLandValue          ));

            _instance.Add(category = new Category(Category.CategoryType.NaturalResources, Category.DescriptionKey.NaturalResources));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestUsedPercent,              Statistic.DescriptionKey.Forest,                    Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfForestAvailable,        colorResourceForestry1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestUsed,                     Statistic.DescriptionKey.Forest,                    Statistic.DescriptionKey.Used,              Statistic.UnitsKey.UnitsPerWeek,                colorResourceForestry1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesForestAvailable,                Statistic.DescriptionKey.Forest,                    Statistic.DescriptionKey.Available,         Statistic.UnitsKey.UnitsPerWeek,                colorResourceForestry2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandUsedPercent,         Statistic.DescriptionKey.FertileLand,               Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfFertileLandAvailable,   colorResourceFertility1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandUsed,                Statistic.DescriptionKey.FertileLand,               Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Hectare,                     colorResourceFertility1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesFertileLandAvailable,           Statistic.DescriptionKey.FertileLand,               Statistic.DescriptionKey.Available,         Statistic.UnitsKey.Hectare,                     colorResourceFertility2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreUsedPercent,                 Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctPerWeekOfOreAvailable,    colorResourceOre1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreUsed,                        Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Used,              Statistic.UnitsKey.UnitsPerWeek,                colorResourceOre1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOreAvailable,                   Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Available,         Statistic.UnitsKey.Units,                       colorResourceOre2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilUsedPercent,                 Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctPerWeekOfOilAvailable,    colorResourceOil1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilUsed,                        Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Used,              Statistic.UnitsKey.UnitsPerWeek,                colorResourceOil1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.NaturalResourcesOilAvailable,                   Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Available,         Statistic.UnitsKey.Units,                       colorResourceOil2           ));

            _instance.Add(category = new Category(Category.CategoryType.Heating, Category.DescriptionKey.Heating));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingConsumptionPercent,                      Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfProduction,             colorInfoHeating1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingConsumption,                             Statistic.DescriptionKey.Consumption,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.MegaWatts,                   colorInfoHeating1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.HeatingProduction,                              Statistic.DescriptionKey.Production,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.MegaWatts,                   colorInfoHeating2           ));

            _instance.Add(category = new Category(Category.CategoryType.Tourism, Category.DescriptionKey.Tourism));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismCityAttractiveness,                      Statistic.DescriptionKey.CityAttractiveness,        Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorInfoTourism            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismLowWealthPercent,                        Statistic.DescriptionKey.LowWealth,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorTouristsLowWealth1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismMediumWealthPercent,                     Statistic.DescriptionKey.MediumWealth,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorTouristsMediumWealth1  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismHighWealthPercent,                       Statistic.DescriptionKey.HighWealth,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfTotal,                  colorTouristsHighWealth1    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismTotal,                                   Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTouristsTotal          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismLowWealth,                               Statistic.DescriptionKey.LowWealth,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTouristsLowWealth2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismMediumWealth,                            Statistic.DescriptionKey.MediumWealth,              Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTouristsMediumWealth2  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismHighWealth,                              Statistic.DescriptionKey.HighWealth,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTouristsHighWealth2    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismExchangeStudentBonus,                    Statistic.DescriptionKey.ExchangeStudentBonus,      Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorExchangeStudent        ));

            _instance.Add(category = new Category(Category.CategoryType.Tours, Category.DescriptionKey.Tours));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalTotal,                                Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalResidents,                            Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursTotalTourists,                             Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTotal2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourTotal,                          Statistic.DescriptionKey.WalkingTour,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportPedestrian1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourResidents,                      Statistic.DescriptionKey.WalkingTour,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportPedestrian1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursWalkingTourTourists,                       Statistic.DescriptionKey.WalkingTour,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportPedestrian2   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingTotal,                          Statistic.DescriptionKey.SightseeingBus,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportTouristBus1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingResidents,                      Statistic.DescriptionKey.SightseeingBus,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportTouristBus1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursSightseeingTourists,                       Statistic.DescriptionKey.SightseeingBus,            Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportTouristBus2   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonTotal,                              Statistic.DescriptionKey.Balloon,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.TotalPerWeek,                colorTransportHotAirBalloon1));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonResidents,                          Statistic.DescriptionKey.Balloon,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.ResidentsPerWeek,            colorTransportHotAirBalloon1));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ToursBalloonToursits,                           Statistic.DescriptionKey.Balloon,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.TouristsPerWeek,             colorTransportHotAirBalloon2));

            _instance.Add(category = new Category(Category.CategoryType.TaxRate, Category.DescriptionKey.TaxRate));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateResidentialLow,                          Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.LowDensity,        Statistic.UnitsKey.Percent,                     colorZoneResidentialLow     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateResidentialHigh,                         Statistic.DescriptionKey.Residential,               Statistic.DescriptionKey.HighDensity,       Statistic.UnitsKey.Percent,                     colorZoneResidentialHigh    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateCommercialLow,                           Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.LowDensity,        Statistic.UnitsKey.Percent,                     colorZoneCommercialLow      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateCommercialHigh,                          Statistic.DescriptionKey.Commercial,                Statistic.DescriptionKey.HighDensity,       Statistic.UnitsKey.Percent,                     colorZoneCommercialHigh     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateIndustrial,                              Statistic.DescriptionKey.Industrial,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TaxRateOffice,                                  Statistic.DescriptionKey.Office,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.Percent,                     colorZoneOffice             ));

            _instance.Add(category = new Category(Category.CategoryType.CityEconomy, Category.DescriptionKey.CityEconomy));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalIncome,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalExpenses,                       Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyTotalProfit,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalProfit        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CityEconomyBankBalance,                         Statistic.DescriptionKey.BankBalance,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.Money,                       colorBankBalance            ));

            _instance.Add(category = new Category(Category.CategoryType.ResidentialIncome, Category.DescriptionKey.ResidentialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeTotalPercent,                  Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityIncome,             colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeTotal,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneResidentialMid     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensityTotal,               Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorZoneResidentialLow     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity1,                   Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity2,                   Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity3,                   Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel3      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity4,                   Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level4,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel4      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensity5,                   Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level5,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel5      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeLowDensitySelfSufficient,      Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.SelfSufficient,    Statistic.UnitsKey.MoneyPerWeek,                colorIncomeSelfSufficient   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensityTotal,              Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorZoneResidentialHigh    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity1,                  Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity2,                  Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity3,                  Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel3      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity4,                  Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level4,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel4      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensity5,                  Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level5,            Statistic.UnitsKey.MoneyPerWeek,                colorResidentialLevel5      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ResidentialIncomeHighDensitySelfSufficient,     Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.SelfSufficient,    Statistic.UnitsKey.MoneyPerWeek,                colorIncomeSelfSufficient   ));

            _instance.Add(category = new Category(Category.CategoryType.CommercialIncome, Category.DescriptionKey.CommercialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTotalPercent,                   Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityIncome,             colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTotal,                          Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensityTotal,                Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorZoneCommercialLow      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity1,                    Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity2,                    Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLowDensity3,                    Statistic.DescriptionKey.LowDensity,                Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensityTotal,               Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorZoneCommercialHigh     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity1,                   Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity2,                   Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeHighDensity3,                   Statistic.DescriptionKey.HighDensity,               Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorCommercialLevel3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeSpecializedTotal,               Statistic.DescriptionKey.SpecializedTotal,          Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorLevelCommercial        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeLeisure,                        Statistic.DescriptionKey.Leisure,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorIncomeLeisure          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeTourism,                        Statistic.DescriptionKey.Tourism,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTourismIncome          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CommercialIncomeOrganic,                        Statistic.DescriptionKey.OrganicAndLocalProduce,    Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorIncomeOrganic          ));

            _instance.Add(category = new Category(Category.CategoryType.IndustrialIncome, Category.DescriptionKey.IndustrialIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeTotalPercent,                   Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityIncome,             colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeTotal,                          Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneIndustrial         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGenericTotal,                   Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorLevelIndustrial        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric1,                       Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorIndustrialLevel1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric2,                       Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorIndustrialLevel2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeGeneric3,                       Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorIndustrialLevel3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeSpecializedTotal,               Statistic.DescriptionKey.SpecializedTotal,          Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransferTotal1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeForestry,                       Statistic.DescriptionKey.Forestry,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransferForestry1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeFarming,                        Statistic.DescriptionKey.Farming,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransferFarming1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeOre,                            Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransferOre1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustrialIncomeOil,                            Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransferOil1           ));

            _instance.Add(category = new Category(Category.CategoryType.OfficeIncome, Category.DescriptionKey.OfficeIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeTotalPercent,                       Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityIncome,             colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeTotal,                              Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGenericTotal,                       Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Total,             Statistic.UnitsKey.MoneyPerWeek,                colorLevelOffice            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric1,                           Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level1,            Statistic.UnitsKey.MoneyPerWeek,                colorOfficeLevel1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric2,                           Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level2,            Statistic.UnitsKey.MoneyPerWeek,                colorOfficeLevel2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeGeneric3,                           Statistic.DescriptionKey.Generic,                   Statistic.DescriptionKey.Level3,            Statistic.UnitsKey.MoneyPerWeek,                colorOfficeLevel3           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.OfficeIncomeITCluster,                          Statistic.DescriptionKey.ITCluster,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorIncomeITCluster        ));

            _instance.Add(category = new Category(Category.CategoryType.TourismIncome, Category.DescriptionKey.TourismIncome));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTotalPercent,                      Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityIncome,             colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTotal,                             Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneCommercialMid      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeCommercialZones,                   Statistic.DescriptionKey.CommercialZones,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTourismIncome          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeTransportation,                    Statistic.DescriptionKey.PublicTransportation,      Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TourismIncomeParkAreas,                         Statistic.DescriptionKey.ParkAreas,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorParks                  ));

            _instance.Add(category = new Category(Category.CategoryType.ServiceExpenses, Category.DescriptionKey.ServiceExpenses));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesTotalPercent,                    Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.PctOfCityExpenses,           colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesTotal,                           Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorZoneOffice             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesRoads,                           Statistic.DescriptionKey.Roads,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorTransportTotal1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesElectricity,                     Statistic.DescriptionKey.Electricity,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorElectricity1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesWaterSewageHeating,              Statistic.DescriptionKey.WaterSewage,               Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorWater1                 ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesGarbage,                         Statistic.DescriptionKey.Garbage,                   Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorGarbage1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesHealthcare,                      Statistic.DescriptionKey.Healthcare,                Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorHealthcare1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesFire,                            Statistic.DescriptionKey.Fire,                      Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorFireSafety             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesEmergency,                       Statistic.DescriptionKey.Emergency,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorEmergency              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesPolice,                          Statistic.DescriptionKey.Police,                    Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorCrimeRate              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesEducation,                       Statistic.DescriptionKey.Education,                 Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorEducated1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesParksPlazas,                     Statistic.DescriptionKey.ParksPlazasLandscaping,    Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorParks                  ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesUniqueBuildings,                 Statistic.DescriptionKey.UniqueBuildings,           Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorUniqueBuildings        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesGenericSportsArenas,             Statistic.DescriptionKey.GenericSportsArenas,       Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorGenericSportsArenas    ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesLoans,                           Statistic.DescriptionKey.Loans,                     Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorEconomy                ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ServiceExpensesPolicies,                        Statistic.DescriptionKey.Policies,                  Statistic.DescriptionKey.None,              Statistic.UnitsKey.MoneyPerWeek,                colorPolicies               ));

            _instance.Add(category = new Category(Category.CategoryType.ParkAreas, Category.DescriptionKey.ParkAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalIncomePercent,                    Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.PctOfCityIncome,             colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalIncome,                           Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalExpensesPercent,                  Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.PctOfCityExpenses,           colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalExpenses,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasTotalProfit,                           Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalProfit        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkIncome,                        Statistic.DescriptionKey.CityPark,                  Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityPark1              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkExpenses,                      Statistic.DescriptionKey.CityPark,                  Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityPark2              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasCityParkProfit,                        Statistic.DescriptionKey.CityPark,                  Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityPark3              ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkIncome,                   Statistic.DescriptionKey.AmusementPark,             Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorAmusementPark1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkExpenses,                 Statistic.DescriptionKey.AmusementPark,             Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorAmusementPark2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasAmusementParkProfit,                   Statistic.DescriptionKey.AmusementPark,             Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorAmusementPark3         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooIncome,                             Statistic.DescriptionKey.Zoo,                       Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorZoo1                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooExpenses,                           Statistic.DescriptionKey.Zoo,                       Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorZoo2                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasZooProfit,                             Statistic.DescriptionKey.Zoo,                       Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorZoo3                   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveIncome,                   Statistic.DescriptionKey.NatureReserve,             Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorNatureReserve1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveExpenses,                 Statistic.DescriptionKey.NatureReserve,             Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorNatureReserve2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.ParkAreasNatureReserveProfit,                   Statistic.DescriptionKey.NatureReserve,             Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorNatureReserve3         ));

            _instance.Add(category = new Category(Category.CategoryType.IndustryAreas, Category.DescriptionKey.IndustryAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalIncomePercent,                Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.PctOfCityIncome,             colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalIncome,                       Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalExpensesPercent,              Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.PctOfCityExpenses,           colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalExpenses,                     Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasTotalProfit,                       Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalProfit        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryIncome,                    Statistic.DescriptionKey.Forestry,                  Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferForestry1      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryExpenses,                  Statistic.DescriptionKey.Forestry,                  Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferForestry2      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasForestryProfit,                    Statistic.DescriptionKey.Forestry,                  Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferForestry3      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingIncome,                     Statistic.DescriptionKey.Farming,                   Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferFarming1       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingExpenses,                   Statistic.DescriptionKey.Farming,                   Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferFarming2       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFarmingProfit,                     Statistic.DescriptionKey.Farming,                   Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferFarming3       ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreIncome,                         Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferOre1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreExpenses,                       Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferOre2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOreProfit,                         Statistic.DescriptionKey.Ore,                       Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferOre3           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilIncome,                         Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferOil1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilExpenses,                       Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferOil2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasOilProfit,                         Statistic.DescriptionKey.Oil,                       Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferOil3           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesIncome,         Statistic.DescriptionKey.WarehousesAndFactories,    Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferMail1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesExpenses,       Statistic.DescriptionKey.WarehousesAndFactories,    Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferMail2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasWarehousesFactoriesProfit,         Statistic.DescriptionKey.WarehousesAndFactories,    Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferMail3          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFishingIndustryIncome,             Statistic.DescriptionKey.FishingIndustry,           Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferFish1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFishingIndustryExpenses,           Statistic.DescriptionKey.FishingIndustry,           Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferFish2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.IndustryAreasFishingIndustryProfit,             Statistic.DescriptionKey.FishingIndustry,           Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferFish3          ));

            _instance.Add(category = new Category(Category.CategoryType.CampusAreas, Category.DescriptionKey.CampusAreas));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalIncomePercent,                  Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.PctOfCityIncome,             colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalIncome,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalExpensesPercent,                Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.PctOfCityExpenses,           colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalExpenses,                       Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTotalProfit,                         Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalProfit        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolIncome,                   Statistic.DescriptionKey.TradeSchool,               Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTradeSchool1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolExpenses,                 Statistic.DescriptionKey.TradeSchool,               Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTradeSchool2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasTradeSchoolProfit,                   Statistic.DescriptionKey.TradeSchool,               Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTradeSchool3           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeIncome,            Statistic.DescriptionKey.LiberalArtsCollege,        Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorLiberalArts1           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeExpenses,          Statistic.DescriptionKey.LiberalArtsCollege,        Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorLiberalArts2           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasLiberalArtsCollegeProfit,            Statistic.DescriptionKey.LiberalArtsCollege,        Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorLiberalArts3           ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityIncome,                    Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorUniversity1            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityExpenses,                  Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorUniversity2            ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.CampusAreasUniversityProfit,                    Statistic.DescriptionKey.University,                Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorUniversity3            ));

            _instance.Add(category = new Category(Category.CategoryType.TransportEconomy, Category.DescriptionKey.TransportEconomy));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalIncomePercent,             Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.PctOfCityIncome,             colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalIncome,                    Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalIncome        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalExpensesPercent,           Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.PctOfCityExpenses,           colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalExpenses,                  Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalExpenses      ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTotalProfit,                    Statistic.DescriptionKey.Total,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorCityTotalProfit        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusIncome,                      Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportBus1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusExpenses,                    Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportBus2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyBusProfit,                      Statistic.DescriptionKey.Bus,                       Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportBus3          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusIncome,               Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrolleybus1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusExpenses,             Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrolleybus2   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrolleybusProfit,               Statistic.DescriptionKey.Trolleybus,                Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrolleybus3   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramIncome,                     Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTram1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramExpenses,                   Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportTram2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTramProfit,                     Statistic.DescriptionKey.Tram,                      Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTram3         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroIncome,                    Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportMetro1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroExpenses,                  Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportMetro2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMetroProfit,                    Statistic.DescriptionKey.Metro,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportMetro3        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainIncome,                    Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrain1        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainExpenses,                  Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrain2        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTrainProfit,                    Statistic.DescriptionKey.Train,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTrain3        ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipIncome,                     Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportShip1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipExpenses,                   Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportShip2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyShipProfit,                     Statistic.DescriptionKey.Ship,                      Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportShip3         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirIncome,                      Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportAir1          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirExpenses,                    Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportAir2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyAirProfit,                      Statistic.DescriptionKey.Air,                       Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportAir3          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailIncome,                 Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportMonorail1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailExpenses,               Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportMonorail2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMonorailProfit,                 Statistic.DescriptionKey.Monorail,                  Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportMonorail3     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarIncome,                 Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportCableCar1     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarExpenses,               Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportCableCar2     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyCableCarProfit,                 Statistic.DescriptionKey.CableCar,                  Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportCableCar3     ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiIncome,                     Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTaxi1         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiExpenses,                   Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportTaxi2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTaxiProfit,                     Statistic.DescriptionKey.Taxi,                      Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportTaxi3         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursIncome,                    Statistic.DescriptionKey.Tours,                     Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportPedestrian1   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursExpenses,                  Statistic.DescriptionKey.Tours,                     Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransportPedestrian2   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyToursProfit,                    Statistic.DescriptionKey.Tours,                     Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransportPedestrian3   ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothIncome,                Statistic.DescriptionKey.TollBooth,                 Statistic.DescriptionKey.Income,            Statistic.UnitsKey.MoneyPerWeek,                colorTollBooth1             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothExpenses,              Statistic.DescriptionKey.TollBooth,                 Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTollBooth2             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyTollBoothProfit,                Statistic.DescriptionKey.TollBooth,                 Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTollBooth3             ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMailExpenses,                   Statistic.DescriptionKey.Mail,                      Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorTransferMail2          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomyMailProfit,                     Statistic.DescriptionKey.Mail,                      Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorTransferMail3          ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomySpaceElevatorExpenses,          Statistic.DescriptionKey.SpaceElevator,             Statistic.DescriptionKey.Expenses,          Statistic.UnitsKey.MoneyPerWeek,                colorSpaceElevator2         ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.TransportEconomySpaceElevatorProfit,            Statistic.DescriptionKey.SpaceElevator,             Statistic.DescriptionKey.Profit,            Statistic.UnitsKey.MoneyPerWeek,                colorSpaceElevator3         ));

            _instance.Add(category = new Category(Category.CategoryType.GameLimits, Category.DescriptionKey.GameLimits));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsUsedPercent,                 Statistic.DescriptionKey.Buildings,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsUsed,                        Statistic.DescriptionKey.Buildings,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsBuildingsCapacity,                    Statistic.DescriptionKey.Buildings,                 Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensUsedPercent,                  Statistic.DescriptionKey.Citizens,                  Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensUsed,                         Statistic.DescriptionKey.Citizens,                  Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizensCapacity,                     Statistic.DescriptionKey.Citizens,                  Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsUsedPercent,              Statistic.DescriptionKey.CitizenUnits,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsUsed,                     Statistic.DescriptionKey.CitizenUnits,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenUnitsCapacity,                 Statistic.DescriptionKey.CitizenUnits,              Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesUsedPercent,          Statistic.DescriptionKey.CitizenInstances,          Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesUsed,                 Statistic.DescriptionKey.CitizenInstances,          Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsCitizenInstancesCapacity,             Statistic.DescriptionKey.CitizenInstances,          Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersUsedPercent,                 Statistic.DescriptionKey.Disasters,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersUsed,                        Statistic.DescriptionKey.Disasters,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDisastersCapacity,                    Statistic.DescriptionKey.Disasters,                 Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsUsedPercent,                 Statistic.DescriptionKey.Districts,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsUsed,                        Statistic.DescriptionKey.Districts,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsDistrictsCapacity,                    Statistic.DescriptionKey.Districts,                 Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsUsedPercent,                    Statistic.DescriptionKey.Events,                    Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsUsed,                           Statistic.DescriptionKey.Events,                    Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsEventsCapacity,                       Statistic.DescriptionKey.Events,                    Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasUsedPercent,                 Statistic.DescriptionKey.GameAreas,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasUsed,                        Statistic.DescriptionKey.GameAreas,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsGameAreasCapacity,                    Statistic.DescriptionKey.GameAreas,                 Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesUsedPercent,              Statistic.DescriptionKey.NetworkLanes,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesUsed,                     Statistic.DescriptionKey.NetworkLanes,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkLanesCapacity,                 Statistic.DescriptionKey.NetworkLanes,              Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesUsedPercent,              Statistic.DescriptionKey.NetworkNodes,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesUsed,                     Statistic.DescriptionKey.NetworkNodes,              Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkNodesCapacity,                 Statistic.DescriptionKey.NetworkNodes,              Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsUsedPercent,           Statistic.DescriptionKey.NetworkSegments,           Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsUsed,                  Statistic.DescriptionKey.NetworkSegments,           Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsNetworkSegmentsCapacity,              Statistic.DescriptionKey.NetworkSegments,           Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsParkAreasUsedPercent,                 Statistic.DescriptionKey.ParkIndustryCampusAreas,   Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsParkAreasUsed,                        Statistic.DescriptionKey.ParkIndustryCampusAreas,   Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsParkAreasCapacity,                    Statistic.DescriptionKey.ParkIndustryCampusAreas,   Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsUsedPercent,                 Statistic.DescriptionKey.PathUnits,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsUsed,                        Statistic.DescriptionKey.PathUnits,                 Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPathUnitsCapacity,                    Statistic.DescriptionKey.PathUnits,                 Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsUsedPercent,                     Statistic.DescriptionKey.Props,                     Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsUsed,                            Statistic.DescriptionKey.Props,                     Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsPropsCapacity,                        Statistic.DescriptionKey.Props,                     Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsUsedPercent,             Statistic.DescriptionKey.RadioChannels,             Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsUsed,                    Statistic.DescriptionKey.RadioChannels,             Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioChannelsCapacity,                Statistic.DescriptionKey.RadioChannels,             Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsUsedPercent,             Statistic.DescriptionKey.RadioContents,             Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsUsed,                    Statistic.DescriptionKey.RadioContents,             Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsRadioContentsCapacity,                Statistic.DescriptionKey.RadioContents,             Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesUsedPercent,            Statistic.DescriptionKey.TransportLines,            Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesUsed,                   Statistic.DescriptionKey.TransportLines,            Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTransportLinesCapacity,               Statistic.DescriptionKey.TransportLines,            Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesUsedPercent,                     Statistic.DescriptionKey.Trees,                     Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesUsed,                            Statistic.DescriptionKey.Trees,                     Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsTreesCapacity,                        Statistic.DescriptionKey.Trees,                     Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesUsedPercent,                  Statistic.DescriptionKey.Vehicles,                  Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesUsed,                         Statistic.DescriptionKey.Vehicles,                  Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesCapacity,                     Statistic.DescriptionKey.Vehicles,                  Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedUsedPercent,            Statistic.DescriptionKey.VehiclesParked,            Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedUsed,                   Statistic.DescriptionKey.VehiclesParked,            Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsVehiclesParkedCapacity,               Statistic.DescriptionKey.VehiclesParked,            Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksUsedPercent,                Statistic.DescriptionKey.ZoneBlocks,                Statistic.DescriptionKey.Used,              Statistic.UnitsKey.PctOfCapacity,               colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksUsed,                       Statistic.DescriptionKey.ZoneBlocks,                Statistic.DescriptionKey.Used,              Statistic.UnitsKey.Amount,                      colorNeutral1               ));
            category.Statistics.Add(new Statistic(category, Statistic.StatisticType.GameLimitsZoneBlocksCapacity,                   Statistic.DescriptionKey.ZoneBlocks,                Statistic.DescriptionKey.Capacity,          Statistic.UnitsKey.Amount,                      colorNeutral2               ));

            #endregion


            // verify category and statistic
            Category.Verify();
            Statistic.Verify();
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
        /// create UI
        /// </summary>
        public bool CreateUI(UIScrollablePanel categoriesScrollablePanel)
        {
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
