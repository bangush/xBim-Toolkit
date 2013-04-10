﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcSurfaceCurveSweptAreaSolid.cs
// Published:   01, 2012
// Last Edited: 18:12 PM on 28 06 2012
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.MeasureResource;
using System.Runtime.Serialization;

using Xbim.XbimExtensions.Interfaces;

#endregion

namespace Xbim.Ifc2x3.GeometricModelResource
{
    [IfcPersistedEntityAttribute]
    public class IfcSurfaceCurveSweptAreaSolid : IfcSweptAreaSolid
    {

        #region Fields

        private IfcCurve _directrix;
        private IfcParameterValue _startParam;
        private IfcParameterValue _endParam;
        private IfcSurface _referenceSurface;

        #endregion

        #region Constructors

        #endregion

        #region Part 21 Step file Parse routines

        /// <summary>
        /// The curve used to define the sweeping operation. The solid is generated by sweeping the SELF\IfcSweptAreaSolid.SweptArea along the Directrix.
        /// </summary>

        [IfcAttribute(3, IfcAttributeState.Mandatory)]
        public IfcCurve Directrix
        {
            get
            {
                ((IPersistIfcEntity)this).Activate(false);
                return _directrix;
            }
            set { this.SetModelValue(this, ref _directrix, value, v => Directrix = v, "Directrix"); }
        }

        /// <summary>
        /// The parameter value on the Directrix at which the sweeping operation commences.
        /// </summary>

        [IfcAttribute(4, IfcAttributeState.Mandatory)]
        public IfcParameterValue StartParam
        {
            get
            {
                ((IPersistIfcEntity)this).Activate(false);
                return _startParam;
            }
            set { this.SetModelValue(this, ref _startParam, value, v => StartParam = v, "StartParam"); }
        }

        /// <summary>
        /// The parameter value on the Directrix at which the sweeping operation ends.
        /// </summary>

        [IfcAttribute(5, IfcAttributeState.Mandatory)]
        public IfcParameterValue EndParam
        {
            get
            {
                ((IPersistIfcEntity)this).Activate(false);
                return _endParam;
            }
            set { this.SetModelValue(this, ref _endParam, value, v => EndParam = v, "EndParam"); }
        }

        /// <summary>
        /// 	 The parameter value on the Directrix at which the sweeping operation ends.
        /// </summary>

        [IfcAttribute(6, IfcAttributeState.Mandatory)]
        public IfcSurface ReferenceSurface
        {
            get
            {
                ((IPersistIfcEntity)this).Activate(false);
                return _referenceSurface;
            }
            set { this.SetModelValue(this, ref _referenceSurface, value, v => ReferenceSurface = v, "ReferenceSurface"); }
        }



        public override void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                case 1:
                    base.IfcParse(propIndex, value);
                    break;
                case 2:
                    _directrix = (IfcCurve)value.EntityVal;
                    break;
                case 3:
                    _startParam = new IfcParameterValue(value.RealVal);
                    break;
                case 4:
                    _endParam = new IfcParameterValue(value.RealVal);
                    break;
                case 5:
                    _referenceSurface = (IfcSurface)value.EntityVal;
                    break;
                default:
                    this.HandleUnexpectedAttribute(propIndex, value);
                    break;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Ifc Schema Validation Methods

        #endregion

    }
}
