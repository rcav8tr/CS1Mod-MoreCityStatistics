using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        // flags
        private bool _initialized;
        private bool _realTimeModEnabled;
        private bool _snapshotTaken;

        // for locking the thread
        private static readonly object _lockObject = new object();

        /// <summary>
        /// initialize snapshots
        /// </summary>
        public void Initialize()
        {
            // with singleton pattern, all fields must be initialized or they will contain data from the previous game
            // except _snapshotTaken gets initialized in SimulationTick when the game date is known
            _instance.Clear();
            Loaded = false;
            _initialized = false;

            // determine if RealTime mod is enabled
            _realTimeModEnabled = ModUtil.IsRealTimeModEnabled();
        }

        /// <summary>
        /// deinitialize snapshots
        /// </summary>
        public void Deinitialize()
        {
            // clear the snapshots to (hopefully) reclaim memory
            _instance.Clear();
            Loaded = false;
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
            if (!Loaded)
            {
                return;
            }

            // lock thread while working with snapshots
            LockThread();

            try
            {
                // get game date and time
                DateTime gameDateTime = SimulationManager.instance.m_currentGameTime;
                DateTime gameDate = gameDateTime.Date;
                TimeSpan gameTime = gameDateTime - gameDate;

                // complete initialization, now that current game date is known
                if (!_initialized)
                {
                    // if a snapshot exists for current game date, then prevent overwriting it
                    Snapshot snapshot = new Snapshot(gameDate);
                    int index = _instance.BinarySearch(snapshot);
                    _snapshotTaken = (index >= 0);

                    // initialization is complete
                    _initialized = true;
                }

                // when Real Time mod is disabled (normal case), take a snapshot on first day of month
                // when Real Time mod is enabled, take a snapshot at noon every day
                if ((!_realTimeModEnabled && gameDate.Day == 1) || (_realTimeModEnabled && gameTime.Hours >= 12))
                {
                    // check if a snapshot exists for the current game date
                    Snapshot snapshot = new Snapshot(gameDate);
                    int index = _instance.BinarySearch(snapshot);
                    if (index < 0)
                    {
                        // snapshot does not exist for current game date
                        // take a snapshot
                        snapshot = Snapshot.TakeSnapshot();

                        // insert the snapshot into the list in the correct place to keep the list sorted
                        // this will usually be at the end of the list
                        index = ~index;
                        _instance.Insert(index, snapshot);
                        _snapshotTaken = true;

                        // update main panel
                        UserInterface.instance.UpdateMainPanel();
                    }
                    else
                    {
                        // snapshot exists for the current game date
                        // if a snapshot is not yet taken, then overwrite existing snapshot
                        if (!_snapshotTaken)
                        {
                            // take a snapshot
                            snapshot = Snapshot.TakeSnapshot();

                            // replace existing snapshot with the snapshot just taken
                            _instance[index] = snapshot;
                            _snapshotTaken = true;

                            // update main panel
                            UserInterface.instance.UpdateMainPanel();
                        }
                    }
                }
                else
                {
                    // it is not time to take a snapshot
                    // clear flag so next snapshot can be taken when it is time
                    _snapshotTaken = false;
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
