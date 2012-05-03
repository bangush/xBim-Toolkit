﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcRelAssignsToGroup.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.Kernel
{
    /// <summary>
    ///   This objectified relationship (IfcRelAssignsToGroup) handles the assignment of objects (subtypes of IfcObject) to a group (subtypes of IfcGroup).
    ///   The relationship handles the assignment of group members to the group object. It allows for grouping arbitrary objects within a group, including other groups. 
    ///   The grouping relationship can be applied in a recursive manner. The resulting group is of type IfcGroup. 
    ///   The Purpose attribute defined at the supertype IfcReleationship, may assign a descriptor, that defines the purpose of the group.
    ///   The inherited attribute RelatedObjects gives the references to the objects, which are the elements within the group. The RelatingGroup is the group, that comprises all elements.
    /// </summary>
    [IfcPersistedEntity, Serializable]
    public class IfcRelAssignsToGroup : IfcRelAssigns
    {
        #region Fields

        private IfcGroup _relatingGroup;

        #endregion

        /// <summary>
        ///   Reference to group that finally contains all assigned group members.
        /// </summary>
        /// <remarks>
        ///   WR1   :   The instance to with the relation points shall not be contained in the List of RelatedObjects.
        /// </remarks>
        [IfcAttribute(7, IfcAttributeState.Mandatory)]
        public IfcGroup RelatingGroup
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _relatingGroup;
            }
            set { ModelManager.SetModelValue(this, ref _relatingGroup, value, v => RelatingGroup = v, "RelatingGroup"); }
        }

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
                    base.IfcParse(propIndex, value);
                    break;
                case 6:
                    _relatingGroup = (IfcGroup) value.EntityVal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("P21 index value out of range in {0}",
                                                                        this.GetType().Name));
            }
        }
    }
}