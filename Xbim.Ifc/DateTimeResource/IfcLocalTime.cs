﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcLocalTime.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using Xbim.Ifc.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.DateTimeResource
{
    [IfcPersistedEntity, Serializable]
    public class IfcLocalTime : IfcDateTimeSelect, IPersistIfcEntity, INotifyPropertyChanged, ISupportChangeNotification,
                                IfcObjectReferenceSelect, INotifyPropertyChanging
    {
#if SupportActivation

        #region IPersistIfcEntity Members

        private long _entityLabel;
        private IModel _model;

        IModel IPersistIfcEntity.ModelOf
        {
            get { return _model; }
        }

        void IPersistIfcEntity.Bind(IModel model, long entityLabel)
        {
            _model = model;
            _entityLabel = entityLabel;
        }

        bool IPersistIfcEntity.Activated
        {
            get { return _entityLabel > 0; }
        }

        public long EntityLabel
        {
            get { return _entityLabel; }
        }

        void IPersistIfcEntity.Activate(bool write)
        {
            if (_model != null && _entityLabel <= 0) _entityLabel = _model.Activate(this, false);
            if (write) _model.Activate(this, write);
        }

        #endregion

#endif

        #region Fields

        private IfcHourInDay _hourComponent;
        private IfcMinuteInHour? _minuteComponent;
        private IfcSecondInMinute? _secondComponent;
        private IfcCoordinatedUniversalTimeOffset _zone;
        private IfcDaylightSavingHour? _daylightSavingOffset;

        #endregion

        /// <summary>
        ///   The number of hours of the local time.
        /// </summary>
        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcHourInDay HourComponent
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _hourComponent;
            }
            set { ModelManager.SetModelValue(this, ref _hourComponent, value, v => HourComponent = v, "HourComponent"); }
        }

        /// <summary>
        ///   The number of minutes of the local time.
        /// </summary>
        [IfcAttribute(2, IfcAttributeState.Optional)]
        public IfcMinuteInHour? MinuteComponent
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _minuteComponent;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _minuteComponent, value, v => MinuteComponent = v,
                                           "MinuteComponent");
            }
        }

        /// <summary>
        ///   The number of seconds of the local time.
        /// </summary>
        [IfcAttribute(3, IfcAttributeState.Optional)]
        public IfcSecondInMinute? SecondComponent
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _secondComponent;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _secondComponent, value, v => SecondComponent = v,
                                           "SecondComponent");
            }
        }

        /// <summary>
        ///   The relationship of the local time to coordinated universal time.
        /// </summary>
        [IfcAttribute(4, IfcAttributeState.Optional)]
        public IfcCoordinatedUniversalTimeOffset Zone
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _zone;
            }
            set { ModelManager.SetModelValue(this, ref _zone, value, v => Zone = v, "Zone"); }
        }

        /// <summary>
        ///   The offset of daylight saving time from basis time.
        /// </summary>
        [IfcAttribute(5, IfcAttributeState.Optional)]
        public IfcDaylightSavingHour? DaylightSavingOffset
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _daylightSavingOffset;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _daylightSavingOffset, value, v => DaylightSavingOffset = v,
                                           "DaylightSavingOffset");
            }
        }

        #region ISupportIfcParser Members

        public void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _hourComponent = (int) value.IntegerVal;
                    break;
                case 1:
                    _minuteComponent = (int) value.IntegerVal;
                    break;
                case 2:
                    _secondComponent = value.RealVal;
                    break;
                case 3:
                    _zone = (IfcCoordinatedUniversalTimeOffset) value.EntityVal;
                    break;
                case 4:
                    _daylightSavingOffset = (int) value.IntegerVal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("P21 index value out of range in {0}",
                                                                        this.GetType().Name));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        [field: NonSerialized] //don't serialize events
            private event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        void ISupportChangeNotification.NotifyPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [field: NonSerialized] //don't serialize events
            private event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { PropertyChanging += value; }
            remove { PropertyChanging -= value; }
        }

        #endregion

        #region ISupportChangeNotification Members

        void ISupportChangeNotification.NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region ISupportIfcParser Members

        public string WhereRule()
        {
            if (_secondComponent.HasValue && !_minuteComponent.HasValue)
                return "WR21 LocalTime : The seconds shall only exist if the minutes exists.\n";
            else
                return "";
        }

        #endregion
    }
}