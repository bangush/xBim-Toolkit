﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcLinearMomentMeasure.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.Ifc.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.MeasureResource
{
    [Serializable]
    public struct IfcLinearMomentMeasure : IPersistIfc, IfcDerivedMeasureValue
    {
        #region ISupportIfcParser Members

        public void IfcParse(int propIndex, IPropertyValue value)
        {
            if (propIndex == 0)
                _theValue = value.RealVal;
            else
                throw new Exception(string.Format("Attribute index {0} is out of range for {1}", propIndex + 1,
                                                  this.GetType().Name.ToUpper()));
        }

        #endregion

        #region ExpressType Members

        public string ToPart21
        {
            get { return IfcReal.AsPart21(_theValue); }
        }

        #endregion

        private double _theValue;

        Type ExpressType.UnderlyingSystemType
        {
            get { return _theValue.GetType(); }
        }

        public object Value
        {
            get { return _theValue; }
        }

        public override string ToString()
        {
            string str = _theValue.ToString();
            if (str.IndexOfAny(new[] {'.', 'E', 'e'}) == -1) str += ".";
            return str;
        }

        public IfcLinearMomentMeasure(double val)
        {
            _theValue = val;
        }


        public IfcLinearMomentMeasure(string val)
        {
            _theValue = Convert.ToDouble(val);
        }

        public static implicit operator IfcLinearMomentMeasure(double? value)
        {
            if (value.HasValue)
                return new IfcLinearMomentMeasure((double) value);
            else
                return new IfcLinearMomentMeasure();
        }

        public static implicit operator IfcLinearMomentMeasure(double value)
        {
            return new IfcLinearMomentMeasure(value);
        }

        public static implicit operator double(IfcLinearMomentMeasure obj)
        {
            return (obj._theValue);
        }

        public static explicit operator double(IfcLinearMomentMeasure? obj)
        {
            if (obj.HasValue)
                return ((IfcLinearMomentMeasure) obj)._theValue;
            else
                return 0.0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;
            return ((IfcLinearMomentMeasure) obj)._theValue == _theValue;
        }

        public static bool operator ==(IfcLinearMomentMeasure obj1, IfcLinearMomentMeasure obj2)
        {
            return Equals(obj1, obj2);
        }

        public static bool operator !=(IfcLinearMomentMeasure obj1, IfcLinearMomentMeasure obj2)
        {
            return !Equals(obj1, obj2);
        }

        public override int GetHashCode()
        {
            return _theValue.GetHashCode();
        }

        public static explicit operator StepP21Token(IfcLinearMomentMeasure? value)
        {
            if (value.HasValue)
                return new StepP21Token(((IfcLinearMomentMeasure) value).ToString());
            else
                return new StepP21Token("$");
        }

        #region ISupportIfcParser Members

        public string WhereRule()
        {
            return "";
        }

        #endregion
    }
}