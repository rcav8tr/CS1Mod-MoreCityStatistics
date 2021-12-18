using ColossalFramework.Plugins;

namespace MoreCityStatistics
{
    /// <summary>
    /// utility routines for workshop mods
    /// </summary>
    public static class ModUtil
    {
        /// <summary>
        /// return whether or not the Real Time mod is enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsRealTimeModEnabled()
        {
            return IsWorkshopModEnabled(1420955187L);
        }

        /// <summary>
        /// return whether or not the specified workshop mod ID is enabled
        /// </summary>
        public static bool IsWorkshopModEnabled(ulong workshopID)
        {
            return IsWorkshopModEnabled(FindWorkshopMod(workshopID));
        }

        /// <summary>
        /// return whether or not the specified workshop mod is enabled
        /// </summary>
        public static bool IsWorkshopModEnabled(PluginManager.PluginInfo mod)
        {
            // if mod is specified, return its enabled status
            if (mod != null)
            {
                return mod.isEnabled;
            }

            // mod not specified, so it is not enabled
            return false;
        }

        /// <summary>
        /// return the specified workshop mod
        /// </summary>
        public static PluginManager.PluginInfo FindWorkshopMod(ulong workshopID)
        {
            // do each plug in
            foreach (PluginManager.PluginInfo mod in PluginManager.instance.GetPluginsInfo())
            {
                // ignore builtin mods and camera script
                if (!mod.isBuiltin && !mod.isCameraScript)
                {
                    // check against the workshop ID
                    if (mod.publishedFileID.AsUInt64 == workshopID)
                    {
                        // found the mod, return it
                        return mod;
                    }
                }
            }

            // mod not found
            return null;
        }
    }
}
