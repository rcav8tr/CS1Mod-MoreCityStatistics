using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MoreCityStatistics
{
    /// <summary>
    /// the snapshots
    /// </summary>
    public class Snapshots : List<Snapshot>
    {
        // use singleton pattern:  there can be only one list of snapshots in the game
        private static readonly Snapshots _instance = new Snapshots();
        public static Snapshots instance { get { return _instance; } }
        private Snapshots() { }

        // miscellaneous public
        public bool Loaded;

        // exporting
        private string ExportPath { get { return ColossalFramework.IO.DataLocation.localApplicationData; } }
        public string ExportPathFile { get { return Path.Combine(ExportPath, "MoreCityStatistics.csv"); } }
        public enum StatisticsToExport
        {
            All,
            Selected
        }

        // miscellaneous private
        private bool _realTimeModEnabled;
        private DateTime _nextSnapshotDateTime;
        private DateTime _previousGameDateTime;
        private static readonly object _lockObject = new object();
        private static readonly DateTime MaxDate = DateTime.MaxValue.Date;

        /// <summary>
        /// initialize snapshots
        /// </summary>
        public void Initialize()
        {
            // with singleton pattern, all fields must be initialized or they will contain data from the previous game
            // except _snapshotTaken gets initialized in SimulationTick when the game date is known
            _instance.Clear();
            Loaded = false;
            _realTimeModEnabled = ModUtil.IsWorkshopModEnabled(ModUtil.ModIDRealTime) || ModUtil.IsWorkshopModEnabled(ModUtil.ModIDRealTime2);
            _nextSnapshotDateTime = DateTime.MinValue;
            _previousGameDateTime = DateTime.MaxValue;

            // debug logging
            if (ConfigurationUtil<Configuration>.Load().DebugLogging)
            {
                LogUtil.LogInfo($"Snapshots.Initialize _realTimeModEnabled={_realTimeModEnabled}");
            }
        }

        /// <summary>
        /// deinitialize snapshots
        /// </summary>
        public void Deinitialize()
        {
            // clear the snapshots to (hopefully) reclaim memory
            _instance.Clear();
            Loaded = false;

            // debug logging
            if (ConfigurationUtil<Configuration>.Load().DebugLogging)
            {
                LogUtil.LogInfo($"Snapshots.Deinitialize _realTimeModEnabled={_realTimeModEnabled}");
            }
        }

        /// <summary>
        /// lock thread while working with snapshots
        /// because the simulation thread writes snapshots and the UI thread reads snapshots
        /// </summary>
        public void LockThread()
        {
            Monitor.Enter(_lockObject);
        }

        /// <summary>
        /// unlock thread when done working with snapshots
        /// </summary>
        public void UnlockThread()
        {
            Monitor.Exit(_lockObject);
        }

        /// <summary>
        /// every simulation tick, check if should take a snapshot
        /// </summary>
        public void SimulationTick()
        {
            // if snapshots were not successfully loaded, then don't take any new snapshots
            // this also prevents taking a snapshot after OnLevelUnloaded (ie. user ended game) but OnAfterSimulationTick still gets triggered several more times
            if (!Loaded)
            {
                // debug logging
                if (ConfigurationUtil<Configuration>.Load().DebugLogging)
                {
                    LogUtil.LogInfo($"Snapshots.SimulationTick Loaded={Loaded}");
                }

                return;
            }

            // lock thread while working with snapshots
            LockThread();

            try
            {
                // get current game date/time
                DateTime currentGameDateTime = SimulationManager.instance.m_currentGameTime;

                // when current game date/time is less than previous game/time,
                // it means either this is the initial call to this routine or user set the game time back using a mod
                // in any case, set a new next snapshot date/time
                // note that with the Real Time mod enabled, continually pausing and running the game (by holding down the space key)
                // can cause the current game time to go back by up to a few seconds, but worst case is that the time goes back
                // across the trigger interval which causes another snapshot to immediately be taken which replaces the snapshot just taken
                if (currentGameDateTime < _previousGameDateTime)
                {
                    SetNextSnapshotDateTime(currentGameDateTime);

                    // debug logging
                    if (ConfigurationUtil<Configuration>.Load().DebugLogging)
                    {
                        LogUtil.LogInfo($"Snapshots.SimulationTick curr<prev currentGameDateTime=[{currentGameDateTime:yyyy/MM/dd HH:mm:ss.ffffff}] _previousGameDateTime=[{_previousGameDateTime:yyyy/MM/dd HH:mm:ss.ffffff}]");
                    }
                }

                // when current game date/time is more than 3 days past previous game/time,
                // it means a mod was probably used to set the game date into the future
                // set a new next snapshot date/time
                if ((currentGameDateTime - _previousGameDateTime).Days > 3)
                {
                    SetNextSnapshotDateTime(currentGameDateTime);

                    // debug logging
                    if (ConfigurationUtil<Configuration>.Load().DebugLogging)
                    {
                        LogUtil.LogInfo($"Snapshots.SimulationTick curr-prev>3 currentGameDateTime=[{currentGameDateTime:yyyy/MM/dd HH:mm:ss.ffffff}] _previousGameDateTime=[{_previousGameDateTime:yyyy/MM/dd HH:mm:ss.ffffff}]");
                    }
                }

                // set previous game/time
                _previousGameDateTime = currentGameDateTime;

                // take a snapshot when game date/time is after next snapshot date/time
                // a snapshot is NOT taken exactly at next snapshot date/time to avoid continuous snapshots if game is paused
                // only take a snapshot before December 31, 9999 00:00:00
                if (currentGameDateTime > _nextSnapshotDateTime && currentGameDateTime < MaxDate)
                {
                    // take a snapshot using next snapshot date/time, not current game date/time
                    // this way snapshot date/times are on nice date/time boundaries which makes it possible to find an exact match below
                    // with Real Time mod enabled  and top sim speed, current game date/time will usually be several seconds after the next snapshot date/time
                    // with Real Time mod disabled and top sim speed, current game date/time will usually be several minutes after the next snapshot date/time
                    Snapshot snapshot = Snapshot.TakeSnapshot(_nextSnapshotDateTime, true);

                    // check if snapshot date/time is after last snapshot date/time
                    if (_instance.Count == 0 || _nextSnapshotDateTime > _instance[_instance.Count - 1].SnapshotDateTime)
                    {
                        // add this snapshot to the end
                        // this will be the usual case, so this performance enhancement avoids the BinarySearch below
                        _instance.Add(snapshot);
                    }
                    else
                    {
                        // check if a snapshot exists for the next snapshot date/time
                        int index = _instance.BinarySearch(snapshot);
                        if (index < 0)
                        {
                            // snapshot does not exist for next snapshot date/time
                            // insert the snapshot into the list in the correct place to keep the list sorted
                            _instance.Insert(~index, snapshot);
                        }
                        else
                        {
                            // snapshot exists for the next snapshot date/time
                            // replace existing snapshot with the snapshot just taken
                            _instance[index] = snapshot;
                        }
                    }

                    // set next snapshot date/time
                    SetNextSnapshotDateTime(currentGameDateTime);

                    // update main panel
                    UserInterface.instance.UpdateMainPanel();
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
            finally
            {
                // make sure thread is unlocked
                UnlockThread();
            }
        }

        /// <summary>
        /// set the date/time next snapshot should be taken
        /// next snapshot date/time is the next scheduled date/time after the current game date/time
        /// </summary>
        public void SetNextSnapshotDateTime(DateTime gameDateTime)
        {
            // get game date without time
            DateTime gameDate = gameDateTime.Date;

            // get selected snapshots per period and trigger interval
            int snapshotsPerPeriod = SnapshotFrequency.instance.selectedSnapshotsPerPeriod;
            int triggerInterval    = SnapshotFrequency.instance.selectedTriggerInterval;

            // check for Real Time mod
            if (_realTimeModEnabled)
            {
                // snapshots are per day and trigger interval is in minutes

                // special case for 1 snapshot per day taken at noon
                if (snapshotsPerPeriod == 1)
                {
                    // next snapshot date/time is either game date at noon or game date + 1 at noon
                    // check if game date/time is past game date at noon
                    DateTime gameDateNoon = gameDate + new TimeSpan(12, 0, 0);
                    if (gameDateTime > gameDateNoon)
                    {
                        // use noon of the next day, but avoid invalid date
                        if (gameDate == MaxDate)
                        {
                            _nextSnapshotDateTime = DateTime.MaxValue;
                        }
                        else
                        {
                            _nextSnapshotDateTime = gameDateNoon.AddDays(1);
                        }
                    }
                    else
                    {
                        // use noon of game date
                        _nextSnapshotDateTime = gameDateNoon;
                    }
                }
                else
                {
                    // check if game date/time is after last snapshot time of the game date
                    int lastSnapshotTimeOfDayInMinutes = (snapshotsPerPeriod - 1) * triggerInterval;
                    if (gameDateTime > gameDate.AddMinutes(lastSnapshotTimeOfDayInMinutes))
                    {
                        // use 00:00 of next day, but avoid invalid date
                        if (gameDate == MaxDate)
                        {
                            _nextSnapshotDateTime = DateTime.MaxValue;
                        }
                        else
                        {
                            _nextSnapshotDateTime = gameDate.AddDays(1);
                        }
                    }
                    else
                    {
                        // game date/time is on or before last snapshot time of the game date
                        // next snapshot date/time is the first trigger interval time of the game date that is on or after the game date/time
                        double gameTimeOfDayInMinutes = (double)(gameDateTime.Ticks - gameDate.Ticks) / TimeSpan.TicksPerMinute;    // game time of day, in minutes, including any fractional minutes
                        int triggerIntervalCount = (int)Math.Ceiling(gameTimeOfDayInMinutes / triggerInterval);                     // number of trigger intervals needed to equal or exceed game time of day, rounding up any minute fractions
                        int nextSnapshotMinutes = triggerInterval * triggerIntervalCount;                                           // integral number of minutes to take the next snapshot
                        _nextSnapshotDateTime = gameDate.AddMinutes(nextSnapshotMinutes);                                           // add the number of minutes to the game date
                    }
                }
            }
            else
            {
                // snapshots are per month and trigger interval is in days

                // get game month and days in game month
                DateTime gameMonth = new DateTime(gameDate.Year, gameDate.Month, 1);
                int daysInGameMonth = DateTime.DaysInMonth(gameMonth.Year, gameMonth.Month);

                // check if game date/time is after last snapshot day of the game month
                int lastSnapshotDayOfMonth = (snapshotsPerPeriod - 1) * triggerInterval + 1;                        // 1-based last snapshot day of game month
                while (lastSnapshotDayOfMonth > daysInGameMonth) { lastSnapshotDayOfMonth -= triggerInterval; }     // make sure last snapshot day of game month is a day in the month
                DateTime lastSnapshotDateOfMonth = gameMonth.AddDays(lastSnapshotDayOfMonth - 1);                   // compute last snapshot date of game month
                if (gameDateTime > lastSnapshotDateOfMonth)
                {
                    // use day 1 of the next month, but avoid invalid date
                    if (gameMonth.Year == 9999 && gameMonth.Month == 12)
                    {
                        _nextSnapshotDateTime = DateTime.MaxValue;
                    }
                    else
                    {
                        _nextSnapshotDateTime = gameMonth.AddMonths(1);
                    }
                }
                else
                {
                    // game date/time is on or before last snapshot day of the game month
                    // next snapshot date is the first trigger interval day of the game month that is on or after the game date/time
                    double gameDayOfMonth = (double)(gameDateTime.Ticks - gameMonth.Ticks) / TimeSpan.TicksPerDay;  // game 0-based day of month, in days, including any fractional days
                    int triggerIntervalCount = (int)Math.Ceiling(gameDayOfMonth / triggerInterval);                 // number of trigger intervals needed to equal or exceed game day of month, rounding up any day fractions
                    int nextSnapshotDayOfMonth = triggerIntervalCount * triggerInterval + 1;                        // 1-based day of the month to take the next snapshot
                    while (nextSnapshotDayOfMonth > daysInGameMonth) { nextSnapshotDayOfMonth -= triggerInterval; } // make sure snapshot day of month is a day in the month
                    _nextSnapshotDateTime = gameMonth.AddDays(nextSnapshotDayOfMonth - 1);                          // add 0-based day of month to game month
                }
            }

            // debug logging
            if (ConfigurationUtil<Configuration>.Load().DebugLogging)
            {
                LogUtil.LogInfo($"Snapshots.SetNextSnapshotDateTime _nextSnapshotDateTime=[{_nextSnapshotDateTime:yyyy/MM/dd HH:mm:ss}] gameDateTime=[{gameDateTime:yyyy/MM/dd HH:mm:ss}].");
            }
        }

        /// <summary>
        /// export selected or all statistics to a file
        /// </summary>
        public void Export(StatisticsToExport statisticsToExport)
        {
            // make sure export path exists
            if (!Directory.Exists(ExportPath))
            {
                LogUtil.LogError("Export directory not found:" + Environment.NewLine + ExportPath);
                return;
            }

            // no need to lock the thread here because this routine is called only from the Pause menu options,
            // which pauses the simulation, which prevents any new snapshots from being taken

            try
            {
                // get statistics to export
                Statistics statistics = (statisticsToExport == StatisticsToExport.All ? Categories.instance.AllStatistics : Categories.instance.SelectedStatistics);

                // get the snapshot field/property for each statistic
                // every statistic has either a field or a property in a snapshot
                FieldInfo[] snapshotFields = new FieldInfo[statistics.Count];
                PropertyInfo[] snapshotProperties = new PropertyInfo[statistics.Count];
                for (int i = 0; i < statistics.Count; i++)
                {
                    Snapshot.GetFieldProperty(statistics[i].Type, out snapshotFields[i], out snapshotProperties[i]);
                }

                // create the file, overwriting existing file
                using (StreamWriter writer = new StreamWriter(ExportPathFile, false))
                {
                    // write heading row
                    const string Separator = "\t";
                    StringBuilder heading = new StringBuilder("\"" + Translation.instance.Get(Translation.Key.SnapshotDateTime) + "\"");
                    foreach (Statistic statistic in statistics)
                    {
                        heading.Append(Separator + "\"" + statistic.CategoryDescriptionUnits.Replace("\"", "\"\"") + "\"");
                    }
                    writer.WriteLine(heading);

                    // do each snapshot
                    foreach (Snapshot snapshot in _instance)
                    {
                        // construct snapshot row starting with snapshot date
                        StringBuilder row = new StringBuilder(snapshot.SnapshotDateTime.ToShortDateString() + " " + snapshot.SnapshotDateTime.ToShortTimeString(), statistics.Count * 4);

                        // append each statistic value to the row
                        for (int i = 0; i < statistics.Count; i++)
                        {
                            // get the snapshot value from either the field or the property
                            object snapshotValue = null;
                            if (snapshotFields[i] != null)
                            {
                                snapshotValue = snapshotFields[i].GetValue(snapshot);
                            }
                            else if (snapshotProperties[i] != null)
                            {
                                snapshotValue = snapshotProperties[i].GetValue(snapshot, null);
                            }

                            // add the snapshot value to the total
                            if (snapshotValue == null)
                            {
                                // append only the separator
                                row.Append(Separator);
                            }
                            else
                            {
                                // append the separator and the value
                                row.Append(Separator + Convert.ToDouble(snapshotValue).ToString("F2", LocaleManager.cultureInfo));
                            }
                        }

                        // write the row
                        writer.WriteLine(row);
                    }
                }

                LogUtil.LogInfo($"Exported {_instance.Count} snapshots for {statistics.Count} statistics to file:" + Environment.NewLine + ExportPathFile);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }

        /// <summary>
        /// write the snapshots to the game save file
        /// </summary>
        public void Serialize(ISerializableData serializableDataManager)
        {
            // erase any existing snapshot data
            EraseData(serializableDataManager);

            // maximum number of bytes that can be serialized at once
            // from ColossalFramework.IO.DataSerializer.WriteByteArray
            const long MaxSerializationLength = 16711680L;

            // do each snapshot
            int snapshotBlock = 0;
            long snapshotSize = 0;
            int snapshotCounter = 0;
            while (snapshotCounter < _instance.Count)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        do
                        {
                            // serialize the snapshot and advance the snapshot counter
                            _instance[snapshotCounter].Serialize(writer);
                            snapshotCounter++;

                            // get snapshot size from first snapshot
                            if (snapshotCounter == 1)
                            {
                                snapshotSize = ms.Position;

                                // logging to get snapshot size
                                // LogUtil.LogInfo($"Snapshot size = [{snapshotSize}] bytes.");
                            }

                        // loop until next snapshot would exceed max size or this snapshot is the last one
                        } while (!(ms.Position + snapshotSize >= MaxSerializationLength || snapshotCounter == _instance.Count));
                    }

                    // save accumulated snapshots and advance the block counter
                    serializableDataManager.SaveData(MCSSerializableData.SnapshotSerializationID(snapshotBlock), ms.ToArray());
                    snapshotBlock++;
                }
            }
        }

        /// <summary>
        /// read the snapshots from the game save file
        /// </summary>
        public void Deserialize(ISerializableData serializableDataManager, int version)
        {
            // load first snapshot block from the game file
            int snapshotBlock = 0;
            byte[] data = serializableDataManager.LoadData(MCSSerializableData.SnapshotSerializationID(snapshotBlock));

            // keep processing while there are snapshot blocks
            while (data != null)
            {
                // read the snapshots from the block
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        // keep reading snapshots while there is data
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            _instance.Add(Snapshot.Deserialize(reader, version));
                        }
                    }
                }

                // get next snapshot block from the game file
                snapshotBlock++;
                data = serializableDataManager.LoadData(MCSSerializableData.SnapshotSerializationID(snapshotBlock));
            }

            // special snapshot date/time processing for snapshots before version 6
            if (version < 6)
            {
                // check if other than day 1 is found on any snapshot
                // other than day 1 indicates that the Real Time mod was enabled when the snapshots where taken
                // this logic will not detect the case where Real Time was enabled and there is only 1 snapshot and that snapshot was on day 1,
                // but this case should be extremely rare and in this case it is okay to leave the snapshot time at 00:00
                bool realTimeWasEnabled = false;
                foreach (Snapshot snapshot in _instance)
                {
                    if (snapshot.SnapshotDateTime.Day != 1)
                    {
                        realTimeWasEnabled = true;
                        break;
                    }
                }

                // if Real Time mod was enabled, then add 12 hours to each snapshot to make it noon
                if (realTimeWasEnabled)
                {
                    foreach (Snapshot snapshot in _instance)
                    {
                        snapshot.SnapshotDateTime = snapshot.SnapshotDateTime.AddHours(12);
                    }
                }
            }
        }

        /// <summary>
        /// erase snapshots from the game file
        /// </summary>
        public void EraseData(ISerializableData serializableDataManager)
        {
            // get first snapshot block ID
            int snapshotBlock = 0;
            string snapshotBlockID = MCSSerializableData.SnapshotSerializationID(snapshotBlock);

            // keep processing while the data contains the snapshot block ID
            while (serializableDataManager.EnumerateData().Contains(snapshotBlockID))
            {
                // erase the snapshot block
                serializableDataManager.EraseData(snapshotBlockID);

                // get next snapshot block ID
                snapshotBlock++;
                snapshotBlockID = MCSSerializableData.SnapshotSerializationID(snapshotBlock);
            }
        }
    }
}
