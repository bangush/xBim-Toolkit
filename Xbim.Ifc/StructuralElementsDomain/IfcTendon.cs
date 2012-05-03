﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcTendon.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.Ifc.MeasureResource;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.StructuralElementsDomain
{
    [IfcPersistedEntity, Serializable]
    public class IfcTendon : IfcReinforcingElement
    {
        #region Fields

        private IfcTendonTypeEnum _predefinedType;
        private IfcPositiveLengthMeasure _nominalDiameter;
        private IfcAreaMeasure _crossSectionArea;
        private IfcForceMeasure? _tensionForce;
        private IfcPressureMeasure? _preStress;
        private IfcNormalisedRatioMeasure? _frictionCoefficient;
        private IfcPositiveLengthMeasure? _anchorageSlip;
        private IfcPositiveLengthMeasure? _minCurvatureRadius;

        #endregion

        #region Properties

        [IfcAttribute(10, IfcAttributeState.Mandatory)]
        public IfcTendonTypeEnum PredefinedType
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _predefinedType;
            }
            set { ModelManager.SetModelValue(this, ref _predefinedType, value, v => PredefinedType = v, "PredefinedType"); }
        }

        [IfcAttribute(11, IfcAttributeState.Mandatory)]
        public IfcPositiveLengthMeasure NominalDiameter
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _nominalDiameter;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _nominalDiameter, value, v => NominalDiameter = v,
                                           "NominalDiameter");
            }
        }

        [IfcAttribute(12, IfcAttributeState.Mandatory)]
        public IfcAreaMeasure CrossSectionArea
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _crossSectionArea;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _crossSectionArea, value, v => CrossSectionArea = v,
                                           "CrossSectionArea");
            }
        }

        [IfcAttribute(13, IfcAttributeState.Optional)]
        public IfcForceMeasure? TensionForce
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _tensionForce;
            }
            set { ModelManager.SetModelValue(this, ref _tensionForce, value, v => TensionForce = v, "TensionForce"); }
        }

        [IfcAttribute(14, IfcAttributeState.Optional)]
        public IfcPressureMeasure? PreStress
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _preStress;
            }
            set { ModelManager.SetModelValue(this, ref _preStress, value, v => PreStress = v, "PreStress"); }
        }

        [IfcAttribute(15, IfcAttributeState.Optional)]
        public IfcNormalisedRatioMeasure? FrictionCoefficient
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _frictionCoefficient;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _frictionCoefficient, value, v => FrictionCoefficient = v,
                                           "FrictionCoefficient");
            }
        }

        [IfcAttribute(16, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? AnchorageSlip
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _anchorageSlip;
            }
            set { ModelManager.SetModelValue(this, ref _anchorageSlip, value, v => AnchorageSlip = v, "AnchorageSlip"); }
        }

        [IfcAttribute(17, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? MinCurvatureRadius
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _minCurvatureRadius;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _minCurvatureRadius, value, v => MinCurvatureRadius = v,
                                           "MinCurvatureRadius");
            }
        }

        #endregion

        public override void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    base.IfcParse(propIndex, value);
                    break;
                case 9:
                    _predefinedType = (IfcTendonTypeEnum) Enum.Parse(typeof (IfcTendonTypeEnum), value.StringVal, true);
                    break;
                case 10:
                    _nominalDiameter = value.RealVal;
                    break;
                case 11:
                    _crossSectionArea = value.RealVal;
                    break;
                case 12:
                    _tensionForce = value.RealVal;
                    break;
                case 13:
                    _preStress = value.RealVal;
                    break;
                case 14:
                    _frictionCoefficient = value.RealVal;
                    break;
                case 15:
                    _anchorageSlip = value.RealVal;
                    break;
                case 16:
                    _minCurvatureRadius = value.RealVal;
                    break;
                default:
                    throw new Exception(string.Format("Attribute index {0} is out of range for {1}", propIndex + 1,
                                                      this.GetType().Name.ToUpper()));
            }
        }

        public override string WhereRule()
        {
            string baseErr = base.WhereRule();
            if (_predefinedType == IfcTendonTypeEnum.USERDEFINED && !ObjectType.HasValue)
                baseErr +=
                    "WR1 Tendon : The attribute ObjectType shall be given, if the bar predefined type is set to USERDEFINED.\n";
            return baseErr;
        }
    }
}