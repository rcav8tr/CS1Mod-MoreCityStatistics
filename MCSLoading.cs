using ColossalFramework.UI;
using ICities;
using System;
using System.Reflection;

namespace MoreCityStatistics
{
    /// <summary>
    /// handle game loading and unloading
    /// </summary>
    /// <remarks>A new instance of Loading is NOT created when loading a game from the Pause Menu.</remarks>
    public class MCSLoading : LoadingExtensionBase
    {
        // whether or not the game is loaded
        public static bool IsGameLoaded { get; private set; }

        public override void OnLevelLoaded(LoadMode mode)
        {
            // do base processing
            base.OnLevelLoaded(mode);

            try
            {
                // game is not loaded
                IsGameLoaded = false;

                // check for new or loaded game
                if (mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario || mode == LoadMode.LoadGame)
                {
                    // initialize user interface singleton
                    // other singletons get initialized in MCSSerializableData, which happens before here
                    if (!UserInterface.instance.Initialize()) return;

                    // game is loaded
                    IsGameLoaded = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
            finally
            {
                // refresh options
                RefreshOptions();
            }
        }

        public override void OnLevelUnloading()
        {
            // do base processing
            base.OnLevelUnloading();

            try
            {
                // deinitialize
                ShowRange.instance.Deinitialize();
                Categories.instance.Deinitialize();
                Snapshots.instance.Deinitialize();
                UserInterface.instance.Deinitialize();
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
            finally
            {
                // game is not loaded
                IsGameLoaded = false;

                // refresh options
                RefreshOptions();
            }
        }

        /// <summary>
        /// refresh plugins on the the main options panel
        /// </summary>
        private void RefreshOptions()
        {
            // this will trigger MoreCityStatistics.OnSettingsUI which calls Options.CreateUI to recreate this mod's Options UI with or without the in-game options
            MethodInfo refreshPlugins = typeof(OptionsMainPanel).GetMethod("RefreshPlugins", BindingFlags.Instance | BindingFlags.NonPublic);
            if (refreshPlugins != null)
            {
                OptionsMainPanel optionsMainPanel = UIView.library.Get<OptionsMainPanel>("OptionsPanel");
                if (optionsMainPanel != null)
                {
                    refreshPlugins.Invoke(optionsMainPanel, new object[] { });
                }
            }
        }
    }
}