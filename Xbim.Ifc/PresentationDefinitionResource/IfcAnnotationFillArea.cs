﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcAnnotationFillArea.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.Ifc.GeometryResource;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.PresentationDefinitionResource
{
    [IfcPersistedEntity, Serializable]
    public class IfcAnnotationFillArea : IfcRepresentationItem
    {
        #region Fields

        private IfcCurve _outerBoundary;
        private CurveSet _innerBoundaries;

        #endregion

        #region Part 21 Step file Parse routines

        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcCurve OuterBoundary
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _outerBoundary;
            }
            set { ModelManager.SetModelValue(this, ref _outerBoundary, value, v => OuterBoundary = v, "OuterBoundary"); }
        }

        [IfcAttribute(2, IfcAttributeState.Optional, IfcAttributeType.Set, IfcAttributeType.Class, 1)]
        public CurveSet InnerBoundaries
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _innerBoundaries;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _innerBoundaries, value, v => InnerBoundaries = v,
                                           "InnerBoundaries");
            }
        }


        public override void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _outerBoundary = (IfcCurve) value.EntityVal;
                    break;
                case 1:
                    if (_innerBoundaries == null) _innerBoundaries = new CurveSet(this);
                    _innerBoundaries.Add((IfcCurve)value.EntityVal);
                    break;
                default:
                    throw new Exception(string.Format("Attribute index {0} is out of range for {1}", propIndex + 1,
                                                      this.GetType().Name.ToUpper()));
            }
        }

        #endregion

        #region Methods

        public void AddInnerBoundary(IfcCurve inner)
        {
            if (_innerBoundaries == null)
                ModelManager.SetModelValue(this, ref _innerBoundaries, new CurveSet(this), v => _innerBoundaries = v,
                                           "InnerBoundaries");
            _innerBoundaries.Add_Reversible(inner);
        }

        #endregion

        public override string WhereRule()
        {
            return "";
        }
    }
}