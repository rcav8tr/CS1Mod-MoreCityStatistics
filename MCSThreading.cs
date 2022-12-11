using ICities;
using System;

namespace MoreCityStatistics
{
    /// <summary>
    /// handle threading
    /// </summary>
    public class MCSThreading : ThreadingExtensionBase
    {
        // initialization
        private bool _gameDateInitialized;

        // simulation tick counting for testing
        //private DateTime _previousGameDate;
        //private int _tickCounter;

        /// <summary>
        /// called once when a game is loaded
        /// </summary>
        public override void OnCreated(IThreading threading)
        {
            // do base processing
            base.OnCreated(threading);

            // not initialized
            _gameDateInitialized = false;
        }

        /// <summary>
        /// called after every simulation tick
        /// simulation ticks occur even when the game is paused
        /// </summary>
        public override void OnAfterSimulationTick()
        {
            // do base processing
            base.OnAfterSimulationTick();

            // The following analysis was performed to determine the likelihood that a snapshot could be missed.
            // The way a snapshot could be missed if is there is no simulation tick in the snapshot interval.
            // This means the game date/time progressed over a whole snapshot interval without a simulation tick.
            // This is most likely to occur on the shortest possible snapshot interval.
            // With the Real Time mod disabled, the shortest snapshot interval is 1 day.
            // With the Real Time mod enabled, the shortest snapshot interval is 10 minutes.

            // The table below shows approximate ticks per game day on a minimal city:  new game, no roads, no buildings, no population, no DLC, no Real Time mod.
            // Ticks per game day do not change for cities with more roads, buildings, population, etc.

            // sim speed x1   = 585 ticks/day:    base game, More Simulation Speed Options, V10Speed
            // sim speed x2   = 293 ticks/day:    base game, More Simulation Speed Options, V10Speed
            // sim speed x4   = 146 ticks/day:    base game, More Simulation Speed Options, V10Speed
            // sim speed x6   =  98 ticks/day:               More Simulation Speed Options
            // sim speed x8   =  73 ticks/day:                                              V10Speed
            // sim speed x9   =  65 ticks/day:               More Simulation Speed Options
            // sim speed x16  =  37 ticks/day:                                              V10Speed
            // sim speed x32  =  18 ticks/day:                                              V10Speed
            // sim speed x64  =   9 ticks/day:                                              V10Speed
            // sim speed x128 = 4-5 ticks/day:                                              V10Speed
            // sim speed x256 = 2-3 ticks/day:                                              V10Speed
            // sim speed x512 = 1-2 ticks/day:                                              V10Speed

            // On my PC, going past x16 on V10Speed did not make the minimal city run any faster.  Perhaps a faster PC could make use of the higher speeds on V10Speed.
            // In the worst case (V10speed at x512), there is still at least one tick per game day.
            // Speed Slider V2 mod does not cause ticks per game day to change even when Speed Slider V2 is used with the other speed mods.
            // Game Speed mod can only slow down the game and causes more ticks per game day, so there is no concern with that mod on missing a snapshot.
            // Real Time mod running at its fastest speed has about 228 ticks per 10 minutes, which is plenty of ticks to avoid missing a snapshot in a 10 minute interval.

            // when game date is initialized, process snapshots
            if (_gameDateInitialized)
            {
                Snapshots.instance.SimulationTick();

                // simulation tick counting, note that pausing the game will adversely affect tick counting
                //_tickCounter++;
                //DateTime gameDate = SimulationManager.instance.m_currentGameTime.Date;
                //if (gameDate > _previousGameDate)
                //{
                //    LogUtil.LogInfo($"[{gameDate:yyyy/MM/dd)}] [{_tickCounter}] ticks/day");
                //    _previousGameDate = gameDate;
                //    _tickCounter = 0;
                //}
            }
        }

        /// <summary>
        /// the game date is not initialized until the first call to OnUpdate
        /// </summary>
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // do base processing
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            // initialize simulation tick counting
            //if (!_gameDateInitialized)
            //{
            //    _previousGameDate = SimulationManager.instance.m_currentGameTime.Date;
            //    _tickCounter = 0;
            //}

            // game date is initialized
            _gameDateInitialized = true;
        }
    }
}
