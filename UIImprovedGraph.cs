using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace MoreCityStatistics
{
    /// <summary>
    /// improved UIGraph
    /// </summary>
    public class UIImprovedGraph : UISprite
    {
        /// <summary>
        /// settings for a curve on the graph
        /// </summary>
        private class CurveSettings
        {
            // the settings for one curve
            private string _description;
            private string _units;
            private string _numberFormat;
            private double?[] _data;
            private float _width;
            private Color32 _color;

            // values computed from the data
            private double _minValue;
            private double _maxValue;

            /// <summary>
            /// construct a curve setting
            /// </summary>
            public CurveSettings(string description, string units, string numberFormat, double?[] data, float width, Color32 color)
            {
                // check parameters
                if (string.IsNullOrEmpty(description))  { throw new ArgumentNullException("description");   }
                if (string.IsNullOrEmpty(units))        { throw new ArgumentNullException("units");         }
                if (string.IsNullOrEmpty(numberFormat)) { throw new ArgumentNullException("numberFormat");  }
                if (data == null)                       { throw new ArgumentNullException("data");          }
                if (data.Length == 0)                   { throw new ArgumentOutOfRangeException("data");    }
                if (width <= float.Epsilon)             { throw new ArgumentOutOfRangeException("width");   }

                // save parameters
                _description = description;
                _units = units;
                _numberFormat = numberFormat;
                _data = data;
                _width = width;
                _color = color;

                // compute min and max of data points that have a value
                bool hasValue = false;
                _minValue = double.MaxValue;
                _maxValue = double.MinValue;
                foreach (double? dataPoint in _data)
                {
                    if (dataPoint.HasValue)
                    {
                        double dataValue = dataPoint.Value;
                        _minValue = Math.Min(_minValue, dataValue);
                        _maxValue = Math.Max(_maxValue, dataValue);
                        hasValue = true;
                    }
                }

                // if curve has no value, then use 0 and 1
                if (!hasValue)
                {
                    _minValue = 0f;
                    _maxValue = 1f;
                }
            }

            // readonly accessors for the settings
            public string Description   { get { return _description;    } }
            public string Units         { get { return _units;          } }
            public string NumberFormat  { get { return _numberFormat;   } }
            public double?[] Data       { get { return _data;           } }
            public float Width          { get { return _width;          } }
            public Color32 Color        { get { return _color;          } }

            // readonly accessors for the computed values
            public double MinValue      { get { return _minValue;       } }
            public double MaxValue      { get { return _maxValue;       } }
        }

        // values that control the look of the graph
        private Color32 _textColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        private Color32 _axesColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        private Color32 _helpAxesColor = new Color32(128, 128, 128, byte.MaxValue);
        private float _axesWidth = 1f;
        private float _helpAxesWidth = 0.6f;    // thick enough so lines are still drawn at low resolutions
        private Rect _graphRect = new Rect(0.1f, 0.1f, 0.8f, 0.8f);
        private UIFont _font;

        // accessors that invalidate the graph whenever changed
        public Color32 TextColor     { get { return _textColor;     } set { _textColor     = value; Invalidate(); } }
        public Color32 AxesColor     { get { return _axesColor;     } set { _axesColor     = value; Invalidate(); } }
        public Color32 HelpAxesColor { get { return _helpAxesColor; } set { _helpAxesColor = value; Invalidate(); } }
        public float AxesWidth       { get { return _axesWidth;     } set { _axesWidth     = value; Invalidate(); } }
        public float HelpAxesWidth   { get { return _helpAxesWidth; } set { _helpAxesWidth = value; Invalidate(); } }
        public Rect GraphRect        { get { return _graphRect;     } set { _graphRect     = value; Invalidate(); } }
        public UIFont Font           { get { return _font;          } set { _font          = value; Invalidate(); } }

        // the date/times for the horizontal axis
        private DateTime[] _dateTimes;

        // start, end, and increment years when displaying the horizontal axis in years
        private int _startYear;
        private int _endYear;
        private int _incrementYear;

        // start and end dates when displaying the horizontal axis in months, days, or hours
        private DateTime _startDate;
        private DateTime _endDate;

        // ticks for the start, end, and range of date/times on the horizontal axis
        private long _startTicks;
        private long _endTicks;
        private long _graphTickRange;

        // how to increment the date/time on the horizontal axis
        private enum DateTimeIncrement
        {
            Years,
            Months6,
            Months3,
            Months2,
            Months1,
            Days10,
            Days5,
            Days2,
            Days1,
            Hours12,
            Hours6,
            Hours2
        }
        DateTimeIncrement _dateTimeIncrement;

        // date constants (without time component)
        private static readonly DateTime MaxDate = DateTime.MaxValue.Date;
        private static readonly DateTime MinDate = DateTime.MinValue.Date;

        // the curves to be shown on the graph
        private List<CurveSettings> _curves = new List<CurveSettings>();

        // values computed from the curves that define how to display the vertical axis
        private double _minCurveValue;
        private double _maxCurveValue;
        private double _startValue;
        private double _endValue;
        private double _incrementValue;
        private double _graphValueRange;

        /// <summary>
        /// make sure font is initialized
        /// </summary>
        public override void Start()
        {
            // do base processing
            base.Start();

            // initialize font
            if (Application.isPlaying && (_font == null || !_font.isValid))
            {
                _font = GetUIView().defaultFont;
            }
        }

        /// <summary>
        /// get the text render data
        /// </summary>
        private UIRenderData textRenderData
        {
            get
            {
                // if there are not 2, then add one
                while (m_RenderData.Count <= 1)
                {
                    UIRenderData item = UIRenderData.Obtain();
                    m_RenderData.Add(item);
                }

                // return the one that was added previously
                return m_RenderData[1];
            }
        }

        /// <summary>
        /// set the date/times for the horizontal axis
        /// </summary>
        public void SetDateTimes(DateTime[] dateTimes)
        {
            // validate parameter
            if (dateTimes == null || dateTimes.Length == 0)
            {
                throw new ArgumentNullException("At least one date/time must be specified.");
            }

            // verify date/times are in ascending order with no duplicates
            DateTime dateTime = dateTimes[0];
            for (int i = 1; i < dateTimes.Length; i++)
            {
                if (dateTimes[i] <= dateTime)
                {
                    throw new InvalidOperationException("Date/times must be in ascending order with no duplicates.");
                }
                dateTime = dateTimes[i];
            }

            // save the date/times
            _dateTimes = dateTimes;

            // get first and last data date/times, will be the same when there is only 1 entry
            DateTime firstDataDateTime = _dateTimes[0];
            DateTime lastDataDateTime = _dateTimes[_dateTimes.Length - 1];

            // compute first and last data dates with no time component
            // if the last data date/time has a time component, use the next day, but don't exceed max date
            DateTime firstDataDate = firstDataDateTime.Date;
            DateTime lastDataDate = lastDataDateTime.Date.AddDays(lastDataDateTime != lastDataDateTime.Date && lastDataDateTime.Date != MaxDate ? 1 : 0);
            int dataDays = (lastDataDate - firstDataDate).Days;

            // compute first and last graph dates with no time component
            DateTime firstGraphDate;
            DateTime lastGraphDate;
            const int ThresholdDays = 16;
            if (dataDays >= ThresholdDays)
            {
                // for ThresholdDays or more, use the first of the month for both first and last graph dates

                // first graph date is first day of the first data month
                firstGraphDate = new DateTime(firstDataDate.Year, firstDataDate.Month, 1);

                // check last data day
                if (lastDataDate.Day == 1)
                {
                    // last graph date is last data date
                    lastGraphDate = lastDataDate;
                }
                else
                {
                    // last graph date is the first day of the month following the last data month, but don't exceed max date
                    // this ensures there will be at least one month between first graph date and last graph date
                    lastGraphDate = ((lastDataDate.Year == 9999 && lastDataDate.Month == 12) ? MaxDate : new DateTime(lastDataDate.Year, lastDataDate.Month, 1).AddMonths(1));
                }
            }
            else
            {
                // for less than ThresholdDays, use the first and last data dates directly
                firstGraphDate = firstDataDate;
                lastGraphDate = lastDataDate;

                // if last graph date is same as first graph date, then set last graph date to the next day
                // this ensures there will be at least one day between first graph date and last graph date
                if (lastGraphDate == firstGraphDate)
                {
                    if (lastGraphDate == MaxDate)
                    {
                        // last graph date is already at max date, cannot add one day
                        // instead, set first graph date to 1 day before max date
                        firstGraphDate = MaxDate.AddDays(-1);
                    }
                    else
                    {
                        lastGraphDate = lastGraphDate.AddDays(1);
                    }
                }
            }

            // compute graph years
            int firstGraphYear = firstGraphDate.Year;
            int lastGraphYear = lastGraphDate.Year + (lastGraphDate.Month == 1 && lastGraphDate.Day == 1 ? 0 : 1);  // for Jan 1, use the year; for other, use next year
            int graphYears = lastGraphYear - firstGraphYear;

            // check graph years
            if (graphYears >= 5)
            {
                // 5 or more graph years

                // increment by years and compute year increment amount
                _dateTimeIncrement = DateTimeIncrement.Years;
                _incrementYear = Mathf.CeilToInt(Mathf.Pow(10f, Mathf.FloorToInt(Mathf.Log10(0.5f * graphYears))));

                // compute start/end year
                _startYear = _incrementYear * Mathf.FloorToInt((float)firstGraphYear / _incrementYear);
                _endYear   = _incrementYear * Mathf.CeilToInt ((float)lastGraphYear  / _incrementYear);

                // if more than 15 divisions, double the increment and recompute
                if ((float)(_endYear - _startYear) / _incrementYear > 15f)
                {
                    _incrementYear *= 2;
                    _startYear = _incrementYear * Mathf.FloorToInt((float)firstGraphYear / _incrementYear);
                    _endYear   = _incrementYear * Mathf.CeilToInt ((float)lastGraphYear  / _incrementYear);
                }

                // if less than 5 divisions and increment divides evenly by 2, halve the increment and recompute
                if ((float)(_endYear - _startYear) / _incrementYear < 5f && _incrementYear % 2 == 0)
                {
                    _incrementYear /= 2;
                    _startYear = _incrementYear * Mathf.FloorToInt((float)firstGraphYear / _incrementYear);
                    _endYear   = _incrementYear * Mathf.CeilToInt ((float)lastGraphYear  / _incrementYear);
                }

                // set start/end date to January 1 of the start/end year just computed
                _startDate = (_startYear == 0   ? MinDate : new DateTime(_startYear, 1, 1));
                _endDate   = (_endYear >= 10000 ? MaxDate : new DateTime(_endYear,   1, 1));
            }
            else
            {
                // 4 or less graph years

                // compute graph months
                int firstGraphMonth = 12 * firstGraphDate.Year + firstGraphDate.Month;
                int lastGraphMonth  = 12 * lastGraphDate.Year  + lastGraphDate.Month;
                int graphMonths = lastGraphMonth - firstGraphMonth;

                // check for 13 graph months thru 4 year graph years
                if (graphMonths >= 13)
                {
                    if (graphYears == 4)
                    {
                        // for 4 years, increment by 6 months:  year, Jul, year+1, Jul, year+2, Jul, year+3, Jul, year+4
                        _dateTimeIncrement = DateTimeIncrement.Months6;
                    }
                    else if (graphYears == 3)
                    {
                        // for 3 years, increment by 3 months:  year, Apr, Jul, Oct, year+1, Apr, Jul, Oct, year+2, Apr, Jul, Oct, year+3
                        _dateTimeIncrement = DateTimeIncrement.Months3;
                    }
                    else
                    {
                        // for 2 years, increment by 2 months:  year, Mar, May, Jul, Sep, Nov, year+1, Mar, May, Jul, Sep, Nov, year+2
                        _dateTimeIncrement = DateTimeIncrement.Months2;
                    }

                    // set start/end date to January 1 of the first/last graph year
                    _startDate = (firstGraphYear == 0    ? MinDate : new DateTime(firstGraphYear, 1, 1));
                    _endDate   = (lastGraphYear >= 10000 ? MaxDate : new DateTime(lastGraphYear,  1, 1));
                }
                else
                {
                    // 12 or less graph months

                    // compute graph days
                    int graphDays = (lastGraphDate - firstGraphDate).Days;

                    // check for ThresholdDays thru 12 graph months
                    if (graphDays >= ThresholdDays)
                    {
                        if (graphMonths >= 6)
                        {
                            // for 6 to 12 months, increment by 1 month:  Mon, Mon+1, Mon+2, Mon+3, Mon+4, Mon+5, Mon+6, Mon+7, Mon+8, Mon+9, Mon+10, Mon+11, Mon+12
                            _dateTimeIncrement = DateTimeIncrement.Months1;
                        }
                        else if (graphMonths >= 3)
                        {
                            // for 3 to 5 months, increment by 10 days:  Mon 1, 11, 21, Mon+1 1, 11, 21, Mon+2 1, 11, 21, Mon+3 1, 11, 21, Mon+4 1, 1, 21, Mon+5 1
                            _dateTimeIncrement = DateTimeIncrement.Days10;
                        }
                        else if (graphMonths == 2)
                        {
                            // for 2 months, increment by 5 days:  Mon 1, 6, 11, 16, 21, 26, Mon+1 1, 6, 11, 16, 21, 26, Mon+2 1
                            // this also includes ThresholdDays to 1 month where first and last graph dates are in different months
                            _dateTimeIncrement = DateTimeIncrement.Days5;
                        }
                        else
                        {
                            // for ThresholdDays days to 1 month where first and last graph dates are in the same month, increment by 2 days
                            // for a month with 31 or 30 days:  Mon 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, Mon+1 1
                            // for a month with 29 or 28 days:  Mon 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, Mon+1 1
                            _dateTimeIncrement = DateTimeIncrement.Days2;
                        }
                    }
                    else
                    {
                        // less than ThresholdDays

                        // check graph days
                        if (graphDays >= 8)
                        {
                            // for 8 to ThresholdDays-1 days, increment by 1 day:  Mon day, day+1, day+2, day+3, day+4, day+5, day+6, day+7, day+8, day+9, day+10, day+11, day+12, day+13, day+14, day+15
                            _dateTimeIncrement = DateTimeIncrement.Days1;
                        }
                        else if (graphDays >= 4)
                        {
                            // for 4 to 7 days, increment by 12 hours:  Mon day, 12:00, Mon day+1, 12:00, Mon day+2, 12:00, Mon day+3, 12:00, Mon day+4, 12:00, Mon day+5, 12:00, Mon day+6, 12:00, Mon day+7
                            _dateTimeIncrement = DateTimeIncrement.Hours12;
                        }
                        else if (graphDays >= 2)
                        {
                            // for 2 to 3 days, increment by 6 hours:  Mon day, 06:00, 12:00, 18:00, Mon day+1, 06:00, 12:00, 18:00, Mon day+2, 06:00, 12:00, 18:00, Mon day+3
                            _dateTimeIncrement = DateTimeIncrement.Hours6;
                        }
                        else
                        {
                            // for 1 day, increment by 2 hours:  Mon day, 02:00, 04:00, 06:00, 08:00, 10:00, 12:00, 14:00, 16:00, 18:00, 20:00, 22:00, Mon day+1
                            _dateTimeIncrement = DateTimeIncrement.Hours2;
                        }
                    }

                    // set start/end date to first/last graph date
                    _startDate = firstGraphDate;
                    _endDate = lastGraphDate;
                }
            }

            // compute start, end, and graph range in ticks
            _startTicks = _startDate.Ticks;
            _endTicks = _endDate.Ticks;
            _graphTickRange = _endTicks - _startTicks;

            // reset curve min and max values before curves are added
            _minCurveValue = double.MaxValue;
            _maxCurveValue = double.MinValue;
        }

        /// <summary>
        /// add a curve to the graph
        /// </summary>
        public void AddCurve(string description, string units, string numberFormat, double?[] data, float width, Color32 color)
        {
            // date/times must be set first
            if (_dateTimes == null)
            {
                throw new InvalidOperationException("Date/times must be set before adding a curve.");
            }
            if (data.Length != _dateTimes.Length)
            {
                throw new InvalidOperationException("Curve data must have the same number of entries as the date/times.");
            }

            // create a new curve
            CurveSettings curve = new CurveSettings(description, units, numberFormat, data, width, color);
            _curves.Add(curve);

            // compute new min and max values
            _minCurveValue = Math.Min(_minCurveValue, curve.MinValue);
            _maxCurveValue = Math.Max(_maxCurveValue, curve.MaxValue);

            // get min and max values
            double minValue = _minCurveValue;
            double maxValue = _maxCurveValue;

            // if min value is positive and less than 30% of max curve value, then use zero for min value
            if (minValue > 0d && minValue < 0.3d * _maxCurveValue)
            {
                minValue = 0d;
            }

            // if max value is negative and more than 30% of min curve value, then use zero for max value
            if (maxValue < 0d && maxValue > 0.3d * _minCurveValue)
            {
                maxValue = 0d;
            }

            // if min and max values are same, set max value to min value + 1
            if (maxValue == minValue)
            {
                maxValue = Math.Floor(minValue) + 1d;
            }

            // compute increment, start, and end values
            ComputeIncrementStartEnd(minValue, maxValue, out _incrementValue, out _startValue, out _endValue);

            // When the curve has very large values (like Bank Balance when the Unlimited Money built-in mod is enabled) and only that one curve is included,
            // the min and max values are the same because the 1 added above to the max value is insignificant compared to the magnitude of the values.
            // Furthermore, the increment is insignificant compared to the magnitude of the values.
            // The insignificant increment value would result in an infinite loop when the horizontal lines and labels are drawn on the graph.
            // When the increment is insignificant, increase the max value by 10, 100, 1000 etc. over the min value
            // until the increment is significant compared to the magnitude of the start and end values.
            double adder = 10d;
            while (_incrementValue <= Math.Max(Math.Abs(_endValue), Math.Abs(_startValue)) * 1E-14)
            {
                // compute new max value
                maxValue = Math.Floor(minValue) + adder;
                adder *= 10d;

                // compute new increment, start, and end values
                ComputeIncrementStartEnd(minValue, maxValue, out _incrementValue, out _startValue, out _endValue);
            }

            // if more than 15 divisions, double the increment and recompute start and end
            if ((_endValue - _startValue) / _incrementValue > 15d)
            {
                _incrementValue *= 2d;
                ComputeStartEnd(minValue, maxValue, _incrementValue, out _startValue, out _endValue);
            }

            // if less than 5 divisions and increment divides evenly by 2, halve the increment and recompute start and end
            if ((_endValue - _startValue) / _incrementValue < 5d && _incrementValue % 2d == 0)
            {
                _incrementValue /= 2d;
                ComputeStartEnd(minValue, maxValue, _incrementValue, out _startValue, out _endValue);
            }

            // compute graph value range, if range is 1,2,3, then use an increment smaller than 1, but keep the same start and end values
            _graphValueRange = _endValue - _startValue;
            if (_graphValueRange == 1d)
            {
                _incrementValue = 0.1d;
            }
            else if (_graphValueRange == 2d)
            {
                _incrementValue = 0.2d;
            }
            else if (_graphValueRange == 3d)
            {
                _incrementValue = 0.5d;
            }
        }

        /// <summary>
        /// compute increment, start, and end values from the min and max values
        /// </summary>
        private void ComputeIncrementStartEnd(double minValue, double maxValue, out double incrementValue, out double startValue, out double endValue)
        {
            // compute whole number increment
            incrementValue = (long)Math.Ceiling(Math.Pow(10d, (long)Math.Floor(Math.Log10(0.5d * (maxValue - minValue)))));

            // increment cannot be zero
            if (incrementValue == 0d)
            {
                incrementValue = 1d;
            }

            // compute start and end values
            ComputeStartEnd(minValue, maxValue, incrementValue, out startValue, out endValue);
        }

        /// <summary>
        /// compute start and end values from the min, max, and increment values
        /// </summary>
        private void ComputeStartEnd(double minValue, double maxValue, double incrementValue, out double startValue, out double endValue)
        {
            // compute whole number start and end values
            startValue = incrementValue * Math.Floor  (minValue / incrementValue);
            endValue   = incrementValue * Math.Ceiling(maxValue / incrementValue);
        }

        /// <summary>
        /// clear the graph
        /// </summary>
        public void Clear()
        {
            _dateTimes = null;
            _curves.Clear();
            Invalidate();
        }

        /// <summary>
        /// when the user hovers the cursor near a data point, show data for the point
        /// </summary>
        protected override void OnTooltipHover(UIMouseEventParameter p)
        {
            // assume no data point found
            bool foundDataPoint = false;

            // there must be at least one curve
            if (_curves.Count >= 1)
            {
                PixelsToUnits();

                // compute the cursor position relative to the graph rect
                Vector2 hitPosition = GetHitPosition(p);
                hitPosition.x /= size.x;
                hitPosition.y /= size.y;
                hitPosition.x = (     hitPosition.x - _graphRect.xMin) / _graphRect.width;
                hitPosition.y = (1f - hitPosition.y - _graphRect.yMin) / _graphRect.height;

                // cursor must be in the graph rect
                if (hitPosition.x >= 0f && hitPosition.x <= 1f && hitPosition.y >= 0f && hitPosition.y <= 1f)
                {
                    // compute date/time index according to X hit position
                    const double MinTooltipDistance = 0.01d;
                    int dateTimeIndex = -1;
                    double minDistanceX = MinTooltipDistance;
                    for (int i = 0; i < _dateTimes.Length; i++)
                    {
                        double posX = (double)(_dateTimes[i].Ticks - _startTicks) / _graphTickRange;
                        double distanceX = Math.Abs(posX - hitPosition.x);
                        if (distanceX < minDistanceX)
                        {
                            dateTimeIndex = i;
                            minDistanceX = distanceX;
                        }
                    }

                    // date/time must be found
                    if (dateTimeIndex >= 0)
                    {
                        // find the curve with a data point closest to the Y hit position
                        CurveSettings curve = null;
                        double minDistanceY = MinTooltipDistance;
                        for (int i = 0; i < _curves.Count; i++)
                        {
                            double? dataValue = _curves[i].Data[dateTimeIndex];
                            if (dataValue.HasValue)
                            {
                                double posY = (dataValue.Value - _startValue) / _graphValueRange;
                                double distanceY = Math.Abs(posY - hitPosition.y);
                                if (distanceY < minDistanceY)
                                {
                                    curve = _curves[i];
                                    minDistanceY = distanceY;
                                }
                            }
                        }

                        // curve must be found
                        if (curve != null)
                        {
                            // found a data point
                            foundDataPoint = true;

                            // set the tool tip text, format date separately from time
                            // so that the Date Format mod can find and replace the date formatting string without affecting the time formatting
                            DateTime pointDateTime = _dateTimes[dateTimeIndex];
                            m_Tooltip = curve.Description + Environment.NewLine +
                                        pointDateTime.ToString("dd/MM/yyyy") + " " + pointDateTime.ToString("HH:mm") + Environment.NewLine +
                                        curve.Data[dateTimeIndex].Value.ToString(curve.NumberFormat, LocaleManager.cultureInfo) + " " + curve.Units;

                            // compute the tool tip box position to follow the cursor
                            UIView uIView = GetUIView();
                            Vector2 cursorPositionOnScreen = uIView.ScreenPointToGUI(p.position / uIView.inputScale);
                            Vector3 vector3 = tooltipBox.pivot.UpperLeftToTransform(tooltipBox.size, tooltipBox.arbitraryPivotOffset);
                            Vector2 tooltipPosition = cursorPositionOnScreen + new Vector2(vector3.x, vector3.y);

                            // make sure tooltip box is entirely on the screen
                            Vector2 screenResolution = uIView.GetScreenResolution();
                            if (tooltipPosition.x < 0f)
                            {
                                tooltipPosition.x = 0f;
                            }
                            if (tooltipPosition.y < 0f)
                            {
                                tooltipPosition.y = 0f;
                            }
                            if (tooltipPosition.x + tooltipBox.width > screenResolution.x)
                            {
                                tooltipPosition.x = screenResolution.x - tooltipBox.width;
                            }
                            if (tooltipPosition.y + tooltipBox.height > screenResolution.y)
                            {
                                tooltipPosition.y = screenResolution.y - tooltipBox.height;
                            }
                            tooltipBox.relativePosition = tooltipPosition;
                        }
                    }
                }
            }

            // check if data point was found
            if (foundDataPoint)
            {
                base.OnTooltipHover(p);
            }
            else
            {
                base.OnTooltipLeave(p);
            }
            RefreshTooltip();
        }

        /// <summary>
        /// called when graph needs to be rendered
        /// </summary>
        protected override void OnRebuildRenderData()
        {
            try
            {
                // make sure font is defined and valid
                if (_font == null || !_font.isValid)
                {
                    _font = GetUIView().defaultFont;
                }

                // proceed only if things needed to render are valid
                if (atlas != null && atlas.material != null && _font != null && _font.isValid && isVisible && spriteInfo != null)
                {
                    // clear the text render
                    textRenderData.Clear();

                    // copy material from base atlas
                    renderData.material = atlas.material;
                    textRenderData.material = atlas.material;

                    // get items from base render data
                    PoolList<Vector3> vertices = renderData.vertices;
                    PoolList<int> triangles = renderData.triangles;
                    PoolList<Vector2> uvs = renderData.uvs;
                    PoolList<Color32> colors = renderData.colors;

                    // draw axes and labels
                    DrawAxesAndLabels(vertices, triangles, uvs, colors);

                    // draw each curve
                    foreach (CurveSettings curve in _curves)
                    {
                        DrawCurve(vertices, triangles, uvs, colors, curve);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }

        /// <summary>
        /// compute the X position of the date/time on the graph
        /// </summary>
        private float NormalizeDateTime(DateTime dateTime)
        {
            return -0.5f + _graphRect.xMin + _graphRect.width * (dateTime.Ticks - _startTicks) / _graphTickRange;
        }

        /// <summary>
        /// compute the Y position of the value on the graph
        /// </summary>
        private float NormalizeValue(double value)
        {
            return (float)(-0.5f + _graphRect.yMin + _graphRect.height * (value - _startValue) / _graphValueRange);
        }

        /// <summary>
        /// draw an axis line
        /// logic adapted from UIGraph.AddSolidQuad
        /// </summary>
        private void DrawAxisLine(Vector2 corner1, Vector2 corner2, Color32 col, PoolList<Vector3> vertices, PoolList<int> triangles, PoolList<Vector2> uvs, PoolList<Color32> colors)
        {
            // ignore if no sprite info
            if (spriteInfo == null)
            {
                return;
            }

            // draw a solid line
            Rect region = spriteInfo.region;
            uvs.Add(new Vector2(0.75f * region.xMin + 0.25f * region.xMax, 0.75f * region.yMin + 0.25f * region.yMax));
            uvs.Add(new Vector2(0.25f * region.xMin + 0.75f * region.xMax, 0.75f * region.yMin + 0.25f * region.yMax));
            uvs.Add(new Vector2(0.25f * region.xMin + 0.75f * region.xMax, 0.25f * region.yMin + 0.75f * region.yMax));
            uvs.Add(new Vector2(0.75f * region.xMin + 0.25f * region.xMax, 0.25f * region.yMin + 0.75f * region.yMax));
            vertices.Add(new Vector3(corner1.x, corner1.y));
            vertices.Add(new Vector3(corner2.x, corner1.y));
            vertices.Add(new Vector3(corner2.x, corner2.y));
            vertices.Add(new Vector3(corner1.x, corner2.y));
            AddTriangles(triangles, vertices.Count);
            colors.Add(col);
            colors.Add(col);
            colors.Add(col);
            colors.Add(col);
        }

        /// <summary>
        /// draw the axes and labels on the graph
        /// logic is adapted from UIGraph.BuildLabels
        /// </summary>
        private void DrawAxesAndLabels(PoolList<Vector3> vertices, PoolList<int> triangles, PoolList<Vector2> uvs, PoolList<Color32> colors)
        {
            // ignore if no curves
            if (_curves.Count == 0)
            {
                return;
            }

            // compute some values used often
            float pixelRatio = PixelsToUnits();
            float ratioXY = size.x / size.y;
            Vector3 baseSize = pixelRatio * size;
            Vector2 maxTextSize = new Vector2(size.x, size.y);
            Vector3 center = pivot.TransformToCenter(size, arbitraryPivotOffset) * pixelRatio;

            // some variables that get re-used
            float lineWidth;
            Color32 lineColor;
            float normalizedX;
            float normalizedY;
            Vector2 corner1;
            Vector2 corner2;

            // draw each horizontal line and the value labels to the left
            lineWidth = AxesWidth;
            lineColor = AxesColor;
            string numberformat = _incrementValue < 1d ? "N1" : "N0";
            for (double value = _startValue; value <= _endValue + _incrementValue / 2d; value += _incrementValue)
            {
                // compute normalized Y value
                normalizedY = NormalizeValue(value);

                // for unknown reasons, must obtain the renderer again for each label to ensure a value with a space separator (e.g. French "1 234") is rendered correctly
                using (UIFontRenderer uIFontRenderer = _font.ObtainRenderer())
                {
                    // draw the value label
                    uIFontRenderer.textScale = 1f;
                    uIFontRenderer.vectorOffset = new Vector3(0f, (0f - height) * pixelRatio * (0.5f - normalizedY) + pixelRatio * 8f, 0f);
                    uIFontRenderer.pixelRatio = pixelRatio;
                    uIFontRenderer.maxSize = maxTextSize;
                    uIFontRenderer.defaultColor = TextColor;
                    uIFontRenderer.Render(value.ToString(numberformat, LocaleManager.cultureInfo), textRenderData);
                }

                // draw axis line
                corner1 = new Vector2(-0.5f + _graphRect.xMin, normalizedY - pixelRatio * lineWidth);
                corner2 = new Vector2(corner1.x + _graphRect.width, normalizedY + pixelRatio * lineWidth);
                DrawAxisLine(Vector3.Scale(corner1, baseSize) + center, Vector3.Scale(corner2, baseSize) + center, lineColor, vertices, triangles, uvs, colors);

                // first line is main axis line; subsequent lines are helper lines
                lineWidth = HelpAxesWidth;
                lineColor = HelpAxesColor;
            }

            // draw each vertical line and the labels below
            lineWidth = AxesWidth;
            lineColor = AxesColor;
            float yPos = height * pixelRatio * (-1f + _graphRect.yMin / 2f) + pixelRatio * 4f;
            if (_dateTimeIncrement == DateTimeIncrement.Years)
            {
                // do each year
                for (int year = _startYear; year <= _endYear; year += _incrementYear)
                {
                    // compute date for the year, normally Jan 1 of the year
                    DateTime date;
                    if (year < 1)
                    {
                        date = new DateTime(1, 1, 1);
                    }
                    else if (year > 9999)
                    {
                        date = new DateTime(9999, 12, 31);
                    }
                    else
                    {
                        date = new DateTime(year, 1, 1);
                    }

                    // compute normalized X value
                    normalizedX = NormalizeDateTime(date);

                    // render date label
                    RenderDateLabel(normalizedX, yPos, maxTextSize, year.ToString());

                    // draw axis line
                    corner1 = new Vector2(normalizedX - pixelRatio * lineWidth / ratioXY, -0.5f + _graphRect.yMin);
                    corner2 = new Vector2(normalizedX + pixelRatio * lineWidth / ratioXY, corner1.y + _graphRect.height);
                    DrawAxisLine(Vector3.Scale(corner1, baseSize) + center, Vector3.Scale(corner2, baseSize) + center, lineColor, vertices, triangles, uvs, colors);

                    // first line is main axis line; subsequent lines are helper lines
                    lineWidth = HelpAxesWidth;
                    lineColor = HelpAxesColor;
                }
            }
            else
            {
                // do each month or day
                DateTime date = _startDate;
                while (date <= _endDate)
                {
                    // compute normalized X value
                    normalizedX = NormalizeDateTime(date);

                    // default to NOT adjust label higher
                    bool adjustLabelHigher = false;

                    // compute date text
                    string dateLabel;
                    if (_dateTimeIncrement == DateTimeIncrement.Months6 || _dateTimeIncrement == DateTimeIncrement.Months3 || _dateTimeIncrement == DateTimeIncrement.Months2)
                    {
                        // for Dec 31, 9999, show year 10000
                        if (date == MaxDate)
                        {
                            dateLabel = "10000";
                        }
                        // for January 1, include year
                        else if (date.Month == 1 && date.Day == 1)
                        {
                            dateLabel = GetMonthLabel(date) + Environment.NewLine + date.Year.ToString();
                            adjustLabelHigher = true;
                        }
                        // for other dates, show month name
                        else
                        {
                            dateLabel = GetMonthLabel(date);
                        }
                    }
                    else if (_dateTimeIncrement == DateTimeIncrement.Months1)
                    {
                        // for first date or January, show month name and year
                        if (date == _startDate || date.Month == 1)
                        {
                            dateLabel = GetMonthLabel(date) + Environment.NewLine + date.Year.ToString();
                            adjustLabelHigher = true;
                        }
                        // for other months, show month name
                        else
                        {
                            dateLabel = GetMonthLabel(date);
                        }
                    }
                    else if (_dateTimeIncrement == DateTimeIncrement.Days10 || _dateTimeIncrement == DateTimeIncrement.Days5 || _dateTimeIncrement == DateTimeIncrement.Days2)
                    {
                        // for day 1, show month name and day number
                        if (date.Day == 1)
                        {
                            dateLabel = GetMonthLabel(date) + " 1";
                        }
                        // for other days, show day number
                        else
                        {
                            dateLabel = date.Day.ToString();
                        }
                    }
                    else if (_dateTimeIncrement == DateTimeIncrement.Days1)
                    {
                        // for first date or day 1, show month name and day number
                        if (date == _startDate || date.Day == 1)
                        {
                            dateLabel = GetMonthLabel(date) + " " + date.Day.ToString();
                        }
                        // for other days, show day number
                        else
                        {
                            dateLabel = date.Day.ToString();
                        }
                    }
                    else
                    {
                        // for midnight, show month name and day number
                        if (date.Hour == 0 && date.Minute == 0)
                        {
                            dateLabel = GetMonthLabel(date) + " " + date.Day.ToString();
                        }
                        // for other times, show hours and minutes
                        else
                        {
                            dateLabel = date.ToString("HH:mm");
                        }
                    }

                    // render date label, adjusting higher if needed
                    RenderDateLabel(normalizedX, yPos + (adjustLabelHigher ? 0.018f : 0f), maxTextSize, dateLabel);

                    // draw axis line
                    corner1 = new Vector2(normalizedX - pixelRatio * lineWidth / ratioXY, -0.5f + _graphRect.yMin);
                    corner2 = new Vector2(normalizedX + pixelRatio * lineWidth / ratioXY, corner1.y + _graphRect.height);
                    DrawAxisLine(Vector3.Scale(corner1, baseSize) + center, Vector3.Scale(corner2, baseSize) + center, lineColor, vertices, triangles, uvs, colors);

                    // first line is main axis line; subsequent lines are helper lines
                    lineWidth = HelpAxesWidth;
                    lineColor = HelpAxesColor;

                    // compute next date according to date increment
                    try
                    {
                        switch (_dateTimeIncrement)
                        {
                            case DateTimeIncrement.Months6: date = date.AddMonths(6); break;
                            case DateTimeIncrement.Months3: date = date.AddMonths(3); break;
                            case DateTimeIncrement.Months2: date = date.AddMonths(2); break;
                            case DateTimeIncrement.Months1: date = date.AddMonths(1); break;
                            case DateTimeIncrement.Days10:  date = (date.Day == 21 ? new DateTime(date.Year, date.Month, 1).AddMonths(1) : date.AddDays(10)); break;
                            case DateTimeIncrement.Days5:   date = (date.Day == 26 ? new DateTime(date.Year, date.Month, 1).AddMonths(1) : date.AddDays( 5)); break;
                            case DateTimeIncrement.Days2:
                                // if at last day of month, use first day of next month, otherwise increment by 2 days
                                int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                                int dateDay = date.Day;
                                if ((daysInMonth == 31 && dateDay == 29) ||
                                    (daysInMonth == 30 && dateDay == 29) ||
                                    (daysInMonth == 29 && dateDay == 27) ||
                                    (daysInMonth == 28 && dateDay == 27))
                                {
                                    date = new DateTime(date.Year, date.Month, 1).AddMonths(1);
                                }
                                else
                                {
                                    date = date.AddDays(2);
                                }
                                break;
                            case DateTimeIncrement.Days1:   date = date.AddDays(1); break;
                            case DateTimeIncrement.Hours12: date = date.AddHours(12); break;
                            case DateTimeIncrement.Hours6:  date = date.AddHours( 6); break;
                            case DateTimeIncrement.Hours2:  date = date.AddHours( 2); break;
                            default:
                                LogUtil.LogError($"Unhandled date time increment [{_dateTimeIncrement}].");
                                date = MaxDate;
                                break;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // computing next date exceeded max value
                        // if already at max date, then break out of loop, otherwise use max date
                        if (date == MaxDate)
                        {
                            break;
                        }
                        date = MaxDate;
                    }
                }
            }

            // draw the horizontal axis line again so it covers the bottom ends of the vertical lines
            lineWidth = AxesWidth;
            lineColor = AxesColor;
            normalizedY = NormalizeValue(_startValue);
            corner1 = new Vector2(-0.5f + _graphRect.xMin, normalizedY - pixelRatio * lineWidth);
            corner2 = new Vector2(corner1.x + _graphRect.width, normalizedY + pixelRatio * lineWidth);
            DrawAxisLine(Vector3.Scale(corner1, baseSize) + center, Vector3.Scale(corner2, baseSize) + center, lineColor, vertices, triangles, uvs, colors);
        }

        /// <summary>
        /// get translated month label
        /// </summary>
        private string GetMonthLabel(DateTime date)
        {
            Translation.Key key = (Translation.Key)Enum.Parse(typeof(Translation.Key), "Month" + date.Month);
            return Translation.instance.Get(key);
        }

        /// <summary>
        /// render a date label below the graph
        /// </summary>
        private void RenderDateLabel(float xPos, float yPos, Vector2 maxTextSize, string dateText)
        {
            float pixelRatio = PixelsToUnits();
            using (UIFontRenderer uIFontRenderer = _font.ObtainRenderer())
            {
                // draw the date label
                uIFontRenderer.textScale = 1f;
                uIFontRenderer.vectorOffset = new Vector3(width * pixelRatio * xPos, yPos, 0f);
                uIFontRenderer.pixelRatio = pixelRatio;
                uIFontRenderer.maxSize = maxTextSize;
                uIFontRenderer.textAlign = UIHorizontalAlignment.Center;
                uIFontRenderer.defaultColor = TextColor;
                uIFontRenderer.Render(dateText, textRenderData);
            }
        }

        /// <summary>
        /// draw a curve on the graph
        /// logic adapted from UIGraph.BuildMeshData
        /// </summary>
        private void DrawCurve(PoolList<Vector3> vertices, PoolList<int> triangles, PoolList<Vector2> uvs, PoolList<Color32> colors, CurveSettings curve)
        {
            // ignore if no sprite info
            if (spriteInfo == null)
            {
                return;
            }

            // ignore if no data
            if (_dateTimes.Length == 0)
            {
                return;
            }

            using (PoolList<Vector2> uvsLine = PoolList<Vector2>.Obtain())
            {
                // compute uvs for a line
                Rect region = spriteInfo.region;
                uvsLine.Add(new Vector2(0.75f * region.xMin + 0.25f * region.xMax, 0.75f * region.yMin + 0.25f * region.yMax));
                uvsLine.Add(new Vector2(0.25f * region.xMin + 0.75f * region.xMax, 0.75f * region.yMin + 0.25f * region.yMax));
                uvsLine.Add(new Vector2(0.25f * region.xMin + 0.75f * region.xMax, 0.25f * region.yMin + 0.75f * region.yMax));
                uvsLine.Add(new Vector2(0.75f * region.xMin + 0.25f * region.xMax, 0.25f * region.yMin + 0.75f * region.yMax));

                using (PoolList<Vector2> uvsDot = PoolList<Vector2>.Obtain())
                {
                    // compute uvs for a dot
                    uvsDot.Add(new Vector2(region.xMin, region.yMin));
                    uvsDot.Add(new Vector2(region.xMax, region.yMin));
                    uvsDot.Add(new Vector2(region.xMax, region.yMax));
                    uvsDot.Add(new Vector2(region.xMin, region.yMax));

                    using (PoolList<Color32> colorsLine = PoolList<Color32>.Obtain())
                    {
                        // compute colors for a line
                        colorsLine.Add(curve.Color);
                        colorsLine.Add(curve.Color);
                        colorsLine.Add(curve.Color);
                        colorsLine.Add(curve.Color);

                        using (PoolList<Color32> colorsDot = PoolList<Color32>.Obtain())
                        {
                            // compute colors for a dot
                            // dot color is a darker version of the curve color
                            const float DotColorMultiplier = 0.6f;
                            Color32 dotColor = new Color32((byte)(curve.Color.r * DotColorMultiplier), (byte)(curve.Color.g * DotColorMultiplier), (byte)(curve.Color.b * DotColorMultiplier), 255);
                            colorsDot.Add(dotColor);
                            colorsDot.Add(dotColor);
                            colorsDot.Add(dotColor);
                            colorsDot.Add(dotColor);

                            // compute some values used often
                            float pixelRatio = PixelsToUnits();
                            float ratioXY = size.x / size.y;
                            Vector3 baseSize = pixelRatio * size;
                            Vector3 center = pivot.TransformToCenter(size, arbitraryPivotOffset) * pixelRatio;

                            // compute the X and Y locations of the first data point
                            double? previousData = curve.Data[0];
                            Vector3 previousPoint = default;
                            previousPoint.x = NormalizeDateTime(_dateTimes[0]);
                            previousPoint.y = NormalizeValue(previousData ?? 0f);

                            // do each data point starting with 1
                            for (int i = 1; i < curve.Data.Length; i++)
                            {
                                // compute the X and Y locations of the current point
                                double? currentData = curve.Data[i];
                                Vector3 currentPoint = default;
                                currentPoint.x = NormalizeDateTime(_dateTimes[i]);
                                currentPoint.y = NormalizeValue(currentData ?? 0f);

                                // if previous and current data points have values, draw a line between the points
                                if (previousData.HasValue && currentData.HasValue)
                                {
                                    // compute distances between current and previous points
                                    float distanceX = currentPoint.x - previousPoint.x;
                                    float distanceY = currentPoint.y - previousPoint.y;
                                    float distanceXY = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

                                    // draw a line from previous point to current point
                                    uvs.AddRange(uvsLine);
                                    Vector3 vectorLine = default;
                                    vectorLine.x = pixelRatio * curve.Width * distanceY / (distanceXY * ratioXY);
                                    vectorLine.y = (0f - pixelRatio) * curve.Width * distanceX / distanceXY;
                                    vertices.Add(Vector3.Scale(previousPoint + vectorLine, baseSize) + center);
                                    vertices.Add(Vector3.Scale(currentPoint  + vectorLine, baseSize) + center);
                                    vertices.Add(Vector3.Scale(currentPoint  - vectorLine, baseSize) + center);
                                    vertices.Add(Vector3.Scale(previousPoint - vectorLine, baseSize) + center);
                                    AddTriangles(triangles, vertices.Count);
                                    colors.AddRange(colorsLine);
                                }

                                // if previous point has a value, draw a dot on it
                                if (previousData.HasValue)
                                {
                                    uvs.AddRange(uvsDot);
                                    Vector3 vectorDot1 = new Vector3(pixelRatio * curve.Width / ratioXY, 0f, 0f);
                                    Vector3 vectorDot2 = new Vector3(0f, pixelRatio * curve.Width, 0f);
                                    vertices.Add(Vector3.Scale(previousPoint - vectorDot1 - vectorDot2, baseSize) + center);
                                    vertices.Add(Vector3.Scale(previousPoint + vectorDot1 - vectorDot2, baseSize) + center);
                                    vertices.Add(Vector3.Scale(previousPoint + vectorDot1 + vectorDot2, baseSize) + center);
                                    vertices.Add(Vector3.Scale(previousPoint - vectorDot1 + vectorDot2, baseSize) + center);
                                    AddTriangles(triangles, vertices.Count);
                                    colors.AddRange(colorsDot);
                                }

                                // copy current to previous
                                previousPoint = currentPoint;
                                previousData = curve.Data[i];
                            }

                            // if last point has a value, draw a dot on it
                            if (previousData.HasValue)
                            {
                                // draw the dot
                                uvs.AddRange(uvsDot);
                                Vector3 vectorDot1 = new Vector3(pixelRatio * curve.Width / ratioXY, 0f, 0f);
                                Vector3 vectorDot2 = new Vector3(0f, pixelRatio * curve.Width, 0f);
                                vertices.Add(Vector3.Scale(previousPoint - vectorDot1 - vectorDot2, baseSize) + center);
                                vertices.Add(Vector3.Scale(previousPoint + vectorDot1 - vectorDot2, baseSize) + center);
                                vertices.Add(Vector3.Scale(previousPoint + vectorDot1 + vectorDot2, baseSize) + center);
                                vertices.Add(Vector3.Scale(previousPoint - vectorDot1 + vectorDot2, baseSize) + center);
                                AddTriangles(triangles, vertices.Count);
                                colors.AddRange(colorsDot);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// add triangles based on the vertices count
        /// </summary>
        private void AddTriangles(PoolList<int> triangles, int verticesCount)
        {
            triangles.Add(verticesCount - 4);
            triangles.Add(verticesCount - 3);
            triangles.Add(verticesCount - 2);
            triangles.Add(verticesCount - 4);
            triangles.Add(verticesCount - 2);
            triangles.Add(verticesCount - 1);
        }
    }
}
