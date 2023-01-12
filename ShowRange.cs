using ColossalFramework.UI;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MoreCityStatistics
{
    /// <summary>
    /// UI to allow the user to select which years or dates to show
    /// </summary>
    public class ShowRange
    {
        // use singleton pattern:  there can be only one Show Range interface in the game
        private static readonly ShowRange _instance = new ShowRange();
        public static ShowRange instance { get { return _instance; } }
        private ShowRange() { }

        // the ranges that can be shown
        public enum ShowRangeOption
        {
            All,
            Since,
            Range
        }
        private ShowRangeOption _showRangeOption;

        // initial values obtained from game save file
        private ShowRangeOption _initialValueShowRangeOption;
        private float _initialValueFromSlider;
        private float _initialValueToSlider;

        // UI components that are referenced after they are created
        private UILabel _rangeToShowLabel;

        private UIPanel  _showAllPanel;
        private UISprite _showAllRadio;
        private UILabel  _showAllLabel;

        private UIPanel  _showSincePanel;
        private UISprite _showSinceRadio;
        private UILabel  _showSinceLabel;

        private UIPanel  _showRangePanel;
        private UISprite _showRangeRadio;
        private UILabel  _showRangeLabel;

        private UISlider _fromSlider;
        private UILabel  _fromLabel;

        private UISlider _toSlider;
        private UILabel  _toLabel;

        // values for handling Real Time mod
        private bool _realTimeModEnabled;
        private DateTime _sliderBaseDate;

        /// <summary>
        /// initialize show range
        /// </summary>
        public void Initialize()
        {
            // with singleton pattern, all fields must be initialized or they will contain data from the previous game

            // initialize show range option
            _showRangeOption = ShowRangeOption.All;

            // initialize initial values
            _initialValueShowRangeOption = ShowRangeOption.All;
            _initialValueFromSlider = 0f;
            _initialValueToSlider = 0f;

            // clear all the UI components
            ClearUIComponents();

            // initialize for Real Time mod
            _realTimeModEnabled = ModUtil.IsWorkshopModEnabled(ModUtil.ModIDRealTime);
            _sliderBaseDate = DateTime.MinValue.Date;
        }

        /// <summary>
        /// deinitialize show range
        /// </summary>
        public void Deinitialize()
        {
            // clear all the UI components
            ClearUIComponents();
        }

        /// <summary>
        /// clear UI components
        /// </summary>
        private void ClearUIComponents()
        {
            // clear all UI references
            _rangeToShowLabel = null;

            _showAllPanel = null;
            _showAllRadio = null;
            _showAllLabel = null;

            _showSincePanel = null;
            _showSinceRadio = null;
            _showSinceLabel = null;

            _showRangePanel = null;
            _showRangeRadio = null;
            _showRangeLabel = null;

            _fromSlider = null;
            _fromLabel  = null;

            _toSlider = null;
            _toLabel  = null;
        }

        /// <summary>
        /// create the Show Range UI on the options panel
        /// </summary>
        public bool CreateUI(UIPanel optionsPanel, Vector2 size, Vector3 relativePosition, Color32 textColor)
        {
            // create Show Range panel
            UIPanel showRangePanel = optionsPanel.AddUIComponent<UIPanel>();
            if (showRangePanel == null)
            {
                LogUtil.LogError("Unable to create Show Range panel.");
                return false;
            }
            showRangePanel.name = "ShowRangePanel";
            showRangePanel.autoSize = false;
            showRangePanel.size = size;
            showRangePanel.relativePosition = relativePosition;

            // create range to show label
            _rangeToShowLabel = showRangePanel.AddUIComponent<UILabel>();
            if (_rangeToShowLabel == null)
            {
                LogUtil.LogError("Unable to create Range To Show label.");
                return false;
            }
            _rangeToShowLabel.name = "RangeToShowLabel";
            _rangeToShowLabel.autoSize = false;
            _rangeToShowLabel.size = new Vector2(showRangePanel.size.x, 15f);
            _rangeToShowLabel.relativePosition = new Vector3(5f, 7f);
            _rangeToShowLabel.textScale = 0.75f;
            _rangeToShowLabel.textColor = textColor;

            // create panel to hold show range options
            UIPanel showRangeOptionPanel = showRangePanel.AddUIComponent<UIPanel>();
            if (showRangeOptionPanel == null)
            {
                LogUtil.LogError("Unable to create Show Range option panel.");
                return false;
            }
            showRangeOptionPanel.name = "ShowRangeOptionPanel";
            showRangeOptionPanel.autoSize = false;
            showRangeOptionPanel.size = new Vector2(size.x - 10f, 15f);
            showRangeOptionPanel.relativePosition = new Vector3(5f, _rangeToShowLabel.relativePosition.y + _rangeToShowLabel.size.y);

            // create the show range option for All
            if (!CreateShowRangeOption(showRangeOptionPanel,
                                       ShowRangeOption.All,
                                       0f,
                                       textColor,
                                       out _showAllPanel,
                                       out _showAllRadio,
                                       out _showAllLabel))
            {
                return false;
            }

            // create the show range option for From Only
            if (!CreateShowRangeOption(showRangeOptionPanel,
                                       ShowRangeOption.Since,
                                       _showAllPanel.relativePosition.x + _showAllPanel.size.x,
                                       textColor,
                                       out _showSincePanel,
                                       out _showSinceRadio,
                                       out _showSinceLabel))
            {
                return false;
            }

            // create the show range option for Range
            if (!CreateShowRangeOption(showRangeOptionPanel,
                                       ShowRangeOption.Range,
                                       _showSincePanel.relativePosition.x + _showSincePanel.size.x,
                                       textColor,
                                       out _showRangePanel,
                                       out _showRangeRadio,
                                       out _showRangeLabel))
            {
                return false;
            }

            // create the range slider for From
            if (!CreateRangeSlider(showRangePanel,
                                   "From",
                                   showRangeOptionPanel.relativePosition.y + showRangeOptionPanel.size.y + 4f,
                                   textColor,
                                   out UIPanel fromSliderPanel,
                                   out _fromSlider,
                                   out _fromLabel))
            {
                return false;
            }

            // create the range slider for To
            if (!CreateRangeSlider(showRangePanel,
                                   "To",
                                   fromSliderPanel.relativePosition.y + fromSliderPanel.size.y + 4f,
                                   textColor,
                                   out UIPanel _,
                                   out _toSlider,
                                   out _toLabel))
            {
                return false;
            }

            // initialize slider min and max values
            // the slider clamps the value to be in the min/max range, so min/max must be initalized before the slider values
            UpdatePanel();

            // set initial Show Range option values to the values previously read from the game save file
            SelectedOption = _initialValueShowRangeOption;
            _fromSlider.value = _initialValueFromSlider;
            _toSlider.value = _initialValueToSlider;

            // initialize slider labels
            UpdateSliderLabels();

            // now set slider event handlers
            _fromSlider.eventValueChanged += FromSlider_eventValueChanged;
            _toSlider.eventValueChanged += ToSlider_eventValueChanged;

            // initialize UI text
            UpdateUIText();

            // success
            return true;
        }

        /// <summary>
        /// create the panel, radio button, and label for a show range option
        /// </summary>
        public bool CreateShowRangeOption(
            UIPanel showRangeOptionPanel,
            ShowRangeOption showRangeOption,
            float left,
            Color32 textColor,
            out UIPanel panel,
            out UISprite radio,
            out UILabel label)
        {
            // construct prefix for UI component names
            string namePrefix = "Show" + showRangeOption.ToString();

            // width for one option is panel width divided by number of options
            float optionWidth = showRangeOptionPanel.size.x / Enum.GetNames(typeof(ShowRangeOption)).Length;

            // create panel
            panel = showRangeOptionPanel.AddUIComponent<UIPanel>();
            if (panel == null)
            {
                LogUtil.LogError($"Unable to create {namePrefix} panel.");
                radio = null;
                label = null;
                return false;
            }
            panel.name = namePrefix + "Panel";
            panel.autoSize = false;
            panel.size = new Vector2(optionWidth, 15f);
            panel.relativePosition = new Vector3(left, 0f);
            panel.eventClicked += RadioButton_eventClicked;

            // create radio button
            radio = panel.AddUIComponent<UISprite>();
            if (radio == null)
            {
                Debug.LogError($"Unable to create {namePrefix} radio button.");
                label = null;
                return false;
            }
            radio.name = namePrefix + "Radio";
            radio.autoSize = false;
            radio.size = new Vector2(15f, 15f);
            radio.relativePosition = new Vector3(0f, 0f);

            // create label
            label = panel.AddUIComponent<UILabel>();
            if (label == null)
            {
                LogUtil.LogError($"Unable to create {namePrefix} label.");
                return false;
            }
            label.name = namePrefix + "Label";
            label.autoSize = false;
            label.size = new Vector2(optionWidth - 17f, 15f);
            label.relativePosition = new Vector3(17f, 2.5f);
            label.textScale = 0.75f;
            label.textColor = textColor;

            // success
            return true;
        }

        /// <summary>
        /// create a date or year slider
        /// </summary>
        public bool CreateRangeSlider(
            UIPanel showRangePanel,
            string namePrefix,
            float top,
            Color32 textColor,
            out UIPanel sliderPanel,
            out UISlider slider,
            out UILabel label)
        {
            // create slider from template
            sliderPanel = showRangePanel.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsSliderTemplate")) as UIPanel;
            if (sliderPanel == null)
            {
                LogUtil.LogError($"Unable to attach {namePrefix} slider panel.");
                slider = null;
                label = null;
                return false;
            }
            sliderPanel.autoSize = false;
            float labelWidth = (_realTimeModEnabled ? 70f : 40f);
            sliderPanel.size = new Vector2(showRangePanel.size.x - 10f - labelWidth, 15f);
            sliderPanel.relativePosition = new Vector3(10f, top);
            sliderPanel.autoLayout = false;

            // get the From slider
            slider = sliderPanel.Find<UISlider>("Slider");
            if (slider == null)
            {
                LogUtil.LogError($"Unable to find {namePrefix} slider.");
                label = null;
                return false;
            }
            slider.autoSize = false;
            slider.size = new Vector2(sliderPanel.size.x, sliderPanel.size.y);
            slider.relativePosition = new Vector3(0f, 0f);
            slider.orientation = UIOrientation.Horizontal;
            slider.stepSize = 1f;      // 1 year or 1 day
            slider.scrollWheelAmount = (_realTimeModEnabled ? 30f : 5f);    // 30 days or 5 years
            slider.minValue = -3f;
            slider.maxValue = -1f;
            slider.value = -2f;

            // hide label from template
            UILabel sliderLabel = sliderPanel.Find<UILabel>("Label");
            if (sliderLabel == null)
            {
                LogUtil.LogError($"Unable to find {namePrefix} label.");
                label = null;
                return false;
            }
            sliderLabel.isVisible = false;

            // create label
            label = showRangePanel.AddUIComponent<UILabel>();
            if (label == null)
            {
                LogUtil.LogError($"Unable to create {namePrefix} label.");
                return false;
            }
            label.name = namePrefix + "Label";
            label.autoSize = false;
            label.size = new Vector2(labelWidth, sliderPanel.size.y);
            label.relativePosition = new Vector3(sliderPanel.relativePosition.x + sliderPanel.size.x + 5f, sliderPanel.relativePosition.y + 4.5f);
            label.text = (_realTimeModEnabled ? "00/00/0000" : "0000");
            label.textScale = 0.625f;
            label.textColor = textColor;
            label.textAlignment = UIHorizontalAlignment.Left;

            // success
            return true;
        }

        /// <summary>
        /// update UI text based on current language
        /// </summary>
        public void UpdateUIText()
        {
            // update labels
            Translation translation = Translation.instance;
            _rangeToShowLabel.text = translation.Get(_realTimeModEnabled ? Translation.Key.DatesToShow : Translation.Key.YearsToShow);
            _showAllLabel.text     = translation.Get(Translation.Key.All  );
            _showSinceLabel.text   = translation.Get(Translation.Key.Since);
            _showRangeLabel.text   = translation.Get(Translation.Key.Range);
        }

        /// <summary>
        /// handle click on one of the radio buttons
        /// </summary>
        private void RadioButton_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // set option based on which panel was clicked
            if      (component == _showAllPanel  ) { SelectedOption = ShowRangeOption.All;   }
            else if (component == _showSincePanel) { SelectedOption = ShowRangeOption.Since; }
            else if (component == _showRangePanel) { SelectedOption = ShowRangeOption.Range; }

            // update main panel
            UserInterface.instance.UpdateMainPanel();

            // event is used
            eventParam.Use();
        }

        /// <summary>
        /// handle change in From slider
        /// </summary>
        private void FromSlider_eventValueChanged(UIComponent component, float value)
        {
            // update slider label
            UpdateSliderLabel(_fromLabel, _fromSlider);

            // update main panel
            UserInterface.instance.UpdateMainPanel();
        }

        /// <summary>
        /// handle change in To slider
        /// </summary>
        private void ToSlider_eventValueChanged(UIComponent component, float value)
        {
            // update slider label
            UpdateSliderLabel(_toLabel, _toSlider);

            // update main panel
            UserInterface.instance.UpdateMainPanel();
        }

        /// <summary>
        /// update the label associated with a slider
        /// </summary>
        private void UpdateSliderLabel(UILabel label, UISlider slider)
        {
            // make sure label and slider were created
            if (label != null && slider != null)
            {
                if (_realTimeModEnabled)
                {
                    // compute and display date
                    label.text = _sliderBaseDate.AddDays((int)slider.value).Date.ToString("dd/MM/yyyy");
                }
                else
                {
                    // display year
                    label.text = ((int)slider.value).ToString();
                }
                label.Invalidate();
            }
        }

        /// <summary>
        /// update labels on both sliders with the current value
        /// </summary>
        private void UpdateSliderLabels()
        {
            UpdateSliderLabel(_fromLabel, _fromSlider);
            UpdateSliderLabel(_toLabel,   _toSlider);
        }

        /// <summary>
        /// update the Show Range panel
        /// </summary>
        public void UpdatePanel()
        {
            // no need to lock the thread here for working with snapshots because this routine is called from only 2 places:
            //      ShowRange.CreateUI in which case threading is not yet a concern
            //      MainPanel.UpdatePanel which has already locked the thread

            // get first and last snapshot dates
            DateTime firstDate;
            DateTime lastDate;
            Snapshots snapshotsInstance = Snapshots.instance;
            int snapshotsCount = snapshotsInstance.Count;
            if (snapshotsCount == 0)
            {
                // use current game date
                firstDate = lastDate = SimulationManager.instance.m_currentGameTime.Date;
            }
            else
            {
                // use first and last snapshot dates
                firstDate = snapshotsInstance[0].SnapshotDateTime.Date;
                lastDate = snapshotsInstance[snapshotsCount - 1].SnapshotDateTime;

                // if last date/time has a time component, use the next day
                if (lastDate != lastDate.Date)
                {
                    if (lastDate.Date == DateTime.MaxValue.Date)
                    {
                        lastDate = DateTime.MaxValue.Date;
                    }
                    else
                    {
                        lastDate = lastDate.Date.AddDays(1);
                    }
                }
            }

            // copmpute new min and max values for the sliders
            float newMinValue;
            float newMaxValue;
            bool sliderBaseDateChanged;
            if (_realTimeModEnabled)
            {
                // the slider value represents the number of days from the base date
                sliderBaseDateChanged = (_sliderBaseDate != firstDate);
                _sliderBaseDate = firstDate;

                // sliders go from first date to last date, but make sure there is at least 1 day
                newMinValue = 0;
                newMaxValue = (lastDate == firstDate ? 1f : (float)((lastDate - firstDate).TotalDays));
            }
            else
            {
                // slider base date is not used, so not changed
                sliderBaseDateChanged = false;

                // compute first year and last year
                // if last date is January 1, then last year is that year
                // if last date is not Jan 1, then last year is the year after the last date
                int firstYear = firstDate.Year;
                int lastYear = lastDate.Year + (lastDate.Month == 1 && lastDate.Day == 1 ? 0 : 1);

                // sliders go from first year to last year, but make sure there is at least 1 year
                newMinValue = firstYear;
                newMaxValue = (lastYear == firstYear ? firstYear + 1 : lastYear);
            }

            // check if need to change slider min or max values
            // only need to check one slider because both sliders use the same min and max
            if (_fromSlider.minValue != newMinValue || _fromSlider.maxValue != newMaxValue || sliderBaseDateChanged)
            {
                // set min and max values on both sliders
                // setting the min and max values forces the slider value to be in the range
                _fromSlider.minValue = _toSlider.minValue = newMinValue;
                _fromSlider.maxValue = _toSlider.maxValue = newMaxValue;

                // the UISlider does not automatically update the thumb when the min and max values are changed, so update the thumb explicitly
                MethodInfo updateValueIndicators = typeof(UISlider).GetMethod("UpdateValueIndicators", BindingFlags.NonPublic | BindingFlags.Instance);
                updateValueIndicators.Invoke(_fromSlider, new object[] { _fromSlider.value });
                updateValueIndicators.Invoke(_toSlider,   new object[] { _toSlider.value   });

                // update slider labels
                UpdateSliderLabels();
            }
        }

        /// <summary>
        /// the currently selected show range option
        /// </summary>
        public ShowRangeOption SelectedOption
        {
            get
            {
                return _showRangeOption;
            }
            private set
            {
                // save value
                _showRangeOption = value;

                // set all radio buttons
                SetRadioButton(_showAllRadio,   _showRangeOption == ShowRangeOption.All  );
                SetRadioButton(_showSinceRadio, _showRangeOption == ShowRangeOption.Since);
                SetRadioButton(_showRangeRadio, _showRangeOption == ShowRangeOption.Range);

                // show or hide sliders and slider labels
                _fromSlider.isVisible = _fromLabel.isVisible = (_showRangeOption == ShowRangeOption.Since || _showRangeOption == ShowRangeOption.Range);
                _toSlider.isVisible   = _toLabel.isVisible   = (                                             _showRangeOption == ShowRangeOption.Range);
            }
        }

        /// <summary>
        /// get the from/to year/date from the slider
        /// </summary>
        public int FromYear { get { return (_fromSlider != null ? (int)_fromSlider.value : 0); } }
        public int ToYear   { get { return (_toSlider   != null ? (int)_toSlider.value   : 0); } }
        public DateTime FromDate { get { return (_fromSlider != null ? _sliderBaseDate.AddDays((int)_fromSlider.value) : DateTime.MinValue.Date); } }
        public DateTime ToDate   { get { return (_toSlider   != null ? _sliderBaseDate.AddDays((int)_toSlider.value  ) : DateTime.MaxValue.Date); } }

        /// <summary>
        /// set the radio button (i.e. sprite) status
        /// </summary>
        private void SetRadioButton(UISprite radioButton, bool value)
        {
            if (radioButton != null)
            {
                radioButton.spriteName = (value ? "check-checked" : "check-unchecked");
            }
        }

        /// <summary>
        /// write the show range user selections to the game save file
        /// </summary>
        public void Serialize(BinaryWriter writer)
        {
            // write current values; these will be used as initial values when the game is loaded
            writer.Write((int)SelectedOption);      // convert enum to int
            writer.Write(_fromSlider != null ? _fromSlider.value : 0f);
            writer.Write(_toSlider   != null ? _toSlider.value   : 0f);
        }

        /// <summary>
        /// read the show range user selections from the game save file
        /// </summary>
        public void Deserialize(BinaryReader reader, int version)
        {
            // read initial values
            _initialValueShowRangeOption = (version < 8 ? (reader.ReadBoolean() ? ShowRangeOption.All : ShowRangeOption.Range ) : (ShowRangeOption)reader.ReadInt32());
            _initialValueFromSlider = reader.ReadSingle();
            _initialValueToSlider   = reader.ReadSingle();
        }
    }
}
