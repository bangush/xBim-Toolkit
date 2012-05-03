﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcAxis2Placement2D.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xbim.Ifc.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.GeometryResource
{
    /// <summary>
    ///   The location and orientation in two dimensional space of two mutually perpendicular axes.
    /// </summary>
    /// <remarks>
    ///   Definition from ISO/CD 10303-42:1992: The location and orientation in two dimensional space of two mutually perpendicular axes. An axis2_placement_2d is defined in terms of a point, (inherited from the placement supertype), and an axis. It can be used to locate and originate an object in two dimensional space and to define a placement coordinate system. The class includes a point which forms the origin of the placement coordinate system. A direction vector is required to complete the definition of the placement coordinate system. The reference direction defines the placement X axis direction, the placement Y axis is derived from this. 
    ///   Definition from IAI: If the RefDirection attribute is not given, the placement defaults to P[1] (x-axis) as [1.,0.] and P[2] (y-axis) as [0.,1.]. 
    ///   NOTE: Corresponding STEP name: axis2_placement_2d, please refer to ISO/IS 10303-42:1994, p. 28 for the final definition of the formal standard. 
    ///   HISTORY: New entity in IFC Release 1.5.
    ///   Illustration
    ///   Definition of the IfcAxis2Placement2D within the two-dimensional coordinate system. 
    ///   EXPRESS specification:
    /// </remarks>
    [IfcPersistedEntity, Serializable]
    public class IfcAxis2Placement2D : IfcPlacement, IfcAxis2Placement
    {
        #region Fields

        private IfcDirection _refDirection;

        #endregion

        #region Part 21 Step file Parse routines

        /// <summary>
        ///   Optional. The direction used to determine the direction of the local X Axis.
        /// </summary>
        [DataMember(Order = 1, IsRequired = false, EmitDefaultValue = false)]
        [IfcAttribute(2, IfcAttributeState.Optional)]
        public IfcDirection RefDirection
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _refDirection;
            }
            set { ModelManager.SetModelValue(this, ref _refDirection, value, v => RefDirection = v, "RefDirection"); }
        }


        public override void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    base.IfcParse(propIndex, value);
                    break;
                case 1:
                    _refDirection = (IfcDirection) value.EntityVal;
                    break;
                default:
                    throw new Exception(string.Format("Attribute index {0} is out of range for {1}", propIndex + 1,
                                                      this.GetType().Name.ToUpper()));
            }
        }

        #endregion

        /// <summary>
        ///   Optional.   P[1]: The normalized direction of the placement X Axis. This is (1.0,0.0,0.0) if RefDirection is omitted. P[2]: The normalized direction of the placement Y Axis. This is a derived attribute and is orthogonal to P[1].
        /// </summary>
        public List<IfcDirection> P
        {
            get
            {
                List<IfcDirection> p = new List<IfcDirection>(2);
                if (RefDirection == null)
                {
                    p.Add(new IfcDirection(1, 0));
                    p.Add(new IfcDirection(0, 1));
                }
                else
                {
                    p.Add(RefDirection);
                    p.Add(new IfcDirection(-RefDirection[1], RefDirection[0]));
                }
                return p;
            }
        }

        public override string ToString()
        {
            if (_refDirection != null)
                return string.Format("L={0}, D={1}", Location, _refDirection);
            else
                return base.ToString();
        }

        #region Ifc Schema Validation Methods

        public override string WhereRule()
        {
            string err = "";
            if (Location.Dim != 2)
                err += "WR2 Axis2Placement2D: The dimensionality of the placement location shall be 2.\n";
            if (RefDirection != null && RefDirection.Dim != 2)
                err +=
                    "WR1 Axis2Placement2D: The RefDirection when given should only reference a two-dimensional IfcDirection.\n";

            return err;
        }

        #endregion
    }
}