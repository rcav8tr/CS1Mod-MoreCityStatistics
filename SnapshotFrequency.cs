using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MoreCityStatistics
{
    /// <summary>
    /// a snapshot frequency dropdown selector
    /// </summary>
    public class SnapshotFrequency
    {
        // use singleton pattern:  there can be only one Snapshot Frequency in the game
        private static readonly SnapshotFrequency _instance = new SnapshotFrequency();
        public static SnapshotFrequency instance { get { return _instance; } }
        private SnapshotFrequency() { }

        // custom event triggered when snapshot frequency changes
        public event EventHandler<EventArgs> eventSnapshotFrequencyChanged;

        // UI elements
        private UIPanel _panel;
        private UILabel _label;
        private UIDropDown _dropdown;

        // miscellaneous private
        private int _initialValueSnapshotsPerPeriod;
        private bool _realTimeModEnabled;
        private List<int> _snapshotsPerPeriod = null;
        private List<int> _triggerIntervals = null;

        /// <summary>
        /// initialize snapshot frequency
        /// </summary>
        public void Initialize()
        {
            // with singleton pattern, all fields must be initialized or they will contain data from the previous game

            // initialize snapshots per period, 1 is valid for both cases
            _initialValueSnapshotsPerPeriod = 1;

            // get status of Real Time mod
            _realTimeModEnabled = ModUtil.IsWorkshopModEnabled(ModUtil.ModIDRealTime) || ModUtil.IsWorkshopModEnabled(ModUtil.ModIDRealTime2);

            // initialize lists based on Real Time mod
            _snapshotsPerPeriod = new List<int>();
            _triggerIntervals   = new List<int>();
            if (_realTimeModEnabled)
            {
                // set snapshots per day and intervals in minutes
                const int MinutesPerDay = 24 * 60;
                _snapshotsPerPeriod.Add(   1); _triggerIntervals.Add(0                                                                 );   //   1 snapshot  per day, taken at noon (special case)
                _snapshotsPerPeriod.Add(   2); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //   2 snapshots per day, every 12 hours
                _snapshotsPerPeriod.Add(   3); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //   3 snapshots per day, every  8 hours
                _snapshotsPerPeriod.Add(   4); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //   4 snapshots per day, every  6 hours
                _snapshotsPerPeriod.Add(   6); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //   6 snapshots per day, every  4 hours
                _snapshotsPerPeriod.Add(   8); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //   8 snapshots per day, every  3 hours
                _snapshotsPerPeriod.Add(  12); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //  12 snapshots per day, every  2 hours
                _snapshotsPerPeriod.Add(  24); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //  24 snapshots per day, every  1 hour
                _snapshotsPerPeriod.Add(2*24); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //  48 snapshots per day, every 30 minutes
                _snapshotsPerPeriod.Add(3*24); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //  72 snapshots per day, every 20 minutes
                _snapshotsPerPeriod.Add(4*24); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   //  96 snapshots per day, every 15 minutes
                _snapshotsPerPeriod.Add(6*24); _triggerIntervals.Add(MinutesPerDay / _snapshotsPerPeriod[_snapshotsPerPeriod.Count - 1]);   // 144 snapshots per day, every 10 minutes
            }
            else
            {
                // set snapshots per month and intervals in days
                _snapshotsPerPeriod.Add( 1); _triggerIntervals.Add(31);     //  1 snapshot  per month, only on day 1
                _snapshotsPerPeriod.Add( 2); _triggerIntervals.Add(15);     //  2 snapshots per month, every 15 days: on days 1 16
                _snapshotsPerPeriod.Add( 3); _triggerIntervals.Add(10);     //  3 snapshots per month, every 10 days: on days 1 11 21
                _snapshotsPerPeriod.Add( 4); _triggerIntervals.Add( 8);     //  4 snapshots per month, every  8 days: on days 1  9 17 25
                _snapshotsPerPeriod.Add( 5); _triggerIntervals.Add( 6);     //  5 snapshots per month, every  6 days: on days 1  7 13 19 25
                _snapshotsPerPeriod.Add( 6); _triggerIntervals.Add( 5);     //  6 snapshots per month, every  5 days: on days 1  6 11 16 21 26
                _snapshotsPerPeriod.Add( 8); _triggerIntervals.Add( 4);     //  8 snapshots per month, every  4 days: on days 1  5  9 13 17 21 25 29
                _snapshotsPerPeriod.Add(10); _triggerIntervals.Add( 3);     // 10 snapshots per month, every  3 days: on days 1  4  7 10 13 16 19 22 25 28
                _snapshotsPerPeriod.Add(15); _triggerIntervals.Add( 2);     // 15 snapshots per month, every  2 days: on days 1  3  5  7  9 11 13 15 17 19 21 23 25 27 29
                _snapshotsPerPeriod.Add(31); _triggerIntervals.Add( 1);     // 31 snapshots per month, every  1 day
            }

            // clear all the UI components to prepare for creating UI
            ClearUIComponents();
        }

        /// <summary>
        /// deinitialize show Range
        /// </summary>
        public void Deinitialize()
        {
            // clear snapshot frequency data
            _snapshotsPerPeriod = null;
            _triggerIntervals = null;

            // clear all the UI components
            ClearUIComponents();
        }

        /// <summary>
        /// clear UI components
        /// </summary>
        private void ClearUIComponents()
        {
            // clear all UI references
            _panel = null;
            _label = null;
            _dropdown = null;
        }

        /// <summary>
        /// create the Snapshot Frequency UI on the options panel
        /// </summary>
        public bool CreateUI(UIPanel optionsPanel)
        {
            // create a new dropdown from the template
            // the template is a UIPanel that contains a UILabel and a UIDropdown
            _panel = optionsPanel.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsDropdownTemplate")) as UIPanel;
            if (_panel == null)
            {
                LogUtil.LogError("Unable to attach component from template [OptionsDropdownTemplate].");
                return false;
            }
            _panel.name = "SnapshotFrequency";
            _panel.autoLayout = false;

            // get label on the panel
            _label = _panel.Find<UILabel>("Label");
            if (_label == null)
            {
                LogUtil.LogError($"Unable to find component [Label] on panel [{_panel.name}].");
                return false;
            }
            _label.autoSize = false;
            _label.textAlignment = UIHorizontalAlignment.Left;
            _label.verticalAlignment = UIVerticalAlignment.Bottom;
            _label.anchor = UIAnchorStyle.Left | UIAnchorStyle.Top;
            _label.relativePosition = new Vector3(5f, 0f);

            // get dropdown on the panel
            _dropdown = _panel.Find<UIDropDown>("Dropdown");
            if (_dropdown == null)
            {
                LogUtil.LogError($"Unable to find component [Dropdown] on panel [{_panel.name}].");
                return false;
            }
            _dropdown.autoSize = false;
            _dropdown.relativePosition = new Vector3(5f, _label.size.y);
            _dropdown.textFieldPadding.left = 5;
            _dropdown.textFieldPadding.right = 5;
            _dropdown.itemPadding.left = 2;
            _dropdown.itemPadding.right = 2;
            _dropdown.canFocus = false;

            // initialize the dropdown
            UpdateDropdownList();

            // find dropdown item that matches the initial value that was read from the game save file
            // if not found, default to first item
            _dropdown.selectedIndex = 0;
            for (int i = 0; i < _snapshotsPerPeriod.Count; i++)
            {
                if (_initialValueSnapshotsPerPeriod == _snapshotsPerPeriod[i])
                {
                    _dropdown.selectedIndex = i;
                    break;
                }
            }

            // now add event handler
            _dropdown.eventSelectedIndexChanged += dropdown_eventSelectedIndexChanged;

            // success
            return true;
        }

        /// <summary>
        /// update UI text based on current language
        /// </summary>
        public void UpdateUIText()
        {
            // update dropdown list
            UpdateDropdownList();
        }

        // expose some properties that access the underlying components
        public float dropdownHeight
        {
            get { return _dropdown.height; }
            set { _dropdown.height = value; }
        }

        public int itemHeight
        {
            get { return _dropdown.itemHeight; }
            set { _dropdown.itemHeight = value; }
        }

        public int listHeight
        {
            get { return _dropdown.listHeight; }
            set { _dropdown.listHeight = value; }
        }

        public Vector3 relativePosition
        {
            get { return _panel.relativePosition; }
            set { _panel.relativePosition = value; }
        }

        public int selectedSnapshotsPerPeriod
        {
            get { return _snapshotsPerPeriod[_dropdown.selectedIndex]; }
        }

        public int selectedTriggerInterval
        {
            get { return _triggerIntervals[_dropdown.selectedIndex]; }
        }

        public Vector2 size
        {
            get { return _panel.size; }
            set
            {
                // resize child components
                _panel.size = value;
                _label.width = value.x - 10f;
                _dropdown.width = value.x - 10f;
            }
        }

        public Color32 textColor
        {
            get { return _label.textColor; }
            set { _label.textColor = value; _dropdown.textColor = value; }
        }

        public float textScale
        {
            get { return _label.textScale; }
            set { _label.textScale = value; _dropdown.textScale = value; }
        }

        /// <summary>
        /// convert dropdown index changed event to eventSnapshotFrequencyChanged
        /// </summary>
        private void dropdown_eventSelectedIndexChanged(UIComponent component, int value)
        {
            // raise eventSnapshotFrequencyChanged
            eventSnapshotFrequencyChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// update the items in the dropdown list
        /// </summary>
        private void UpdateDropdownList()
        {
            // get label text
            Translation translationInstance = Translation.instance;
            _label.text = translationInstance.Get(_realTimeModEnabled ? Translation.Key.SnapshotsPerDay : Translation.Key.SnapshotsPerMonth);

            // populate the items
            int numItems = _snapshotsPerPeriod.Count;
            string[] newItems = new string[numItems];
            for (int i = 0; i < numItems; i++)
            {
                // get snapshots per period
                int snapshotsPerPeriod = _snapshotsPerPeriod[i];
                newItems[i] = snapshotsPerPeriod.ToString() + " - ";

                if (_realTimeModEnabled)
                {
                    // check for special case
                    if (snapshotsPerPeriod == 1)
                    {
                        // 1 snapshot per day is taken at noon
                        newItems[i] += translationInstance.Get(Translation.Key.AtNoon);
                    }
                    else
                    {
                        // trigger interval is in minutes, display as hours:minutes:sseconds
                        newItems[i] += translationInstance.Get(Translation.Key.Every) + " " + (_triggerIntervals[i] / 60).ToString("00") + ":" + (_triggerIntervals[i] % 60).ToString("00") + ":00";
                    }
                }
                else
                {
                    // check for special cases
                    if (snapshotsPerPeriod == 1)
                    {
                        // 1 snapshot per month is taken on day 1
                        newItems[i] += translationInstance.Get(Translation.Key.OnDay1);
                    }
                    else if (snapshotsPerPeriod == 31)
                    {
                        // 31 snapshots per month are taken every day
                        newItems[i] += translationInstance.Get(Translation.Key.EveryDay);
                    }
                    else
                    {
                        // trigger interval is in days
                        newItems[i] += string.Format(translationInstance.Get(Translation.Key.EveryXDays), _triggerIntervals[i].ToString());
                    }
                }
            }

            // use the new array, this does NOT change the selected index
            _dropdown.items = newItems;
        }

        /// <summary>
        /// write the snapshot frequency user selection to the game save file
        /// </summary>
        public void Serialize(BinaryWriter writer)
        {
            // write current value; this will be used as initial value when the game is loaded
            writer.Write(selectedSnapshotsPerPeriod);
        }

        /// <summary>
        /// read the snapshot frequency user selection from the game save file
        /// </summary>
        public void Deserialize(BinaryReader reader, int version)
        {
            // check version
            if (version < 6)
            {
                // for both with and without Real Time mod, default to 1
                _initialValueSnapshotsPerPeriod = 1;
            }
            else
            {
                // read initial value
                _initialValueSnapshotsPerPeriod = reader.ReadInt32();
            }
        }
    }
}
