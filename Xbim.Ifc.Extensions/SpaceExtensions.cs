﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc.Extensions
// Filename:    SpaceExtensions.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc.GeometricModelResource;
using Xbim.Ifc.GeometryResource;
using Xbim.Ifc.ProductExtension;
using Xbim.Ifc.RepresentationResource;
using Xbim.Ifc.SharedBldgElements;
using Xbim.XbimExtensions;

#endregion

namespace Xbim.Ifc.Extensions
{
    public static class SpaceExtensions
    {
        #region Representation methods

        public static IfcShapeRepresentation GetFootPrintRepresentation(this IfcSpace space)
        {
            if (space.Representation != null)
                return
                    space.Representation.Representations.OfType<IfcShapeRepresentation>().FirstOrDefault(
                        r => string.Compare(r.RepresentationIdentifier.GetValueOrDefault(), "FootPrint", true) == 0);
            return null;
        }

        #endregion

        #region Generator methods

        /// <summary>
        ///   If the space has a footprint represenation this will generate a set of walls conforming to that footprint, otherwise returns null
        /// </summary>
        /// <param name = "space"></param>
        /// <param name = "model"></param>
        /// <returns></returns>
        public static List<IfcWall> GenerateWalls(this IfcSpace space, IModel model)
        {
            IfcShapeRepresentation fp = GetFootPrintRepresentation(space);
            if (fp != null)
            {
                IfcRepresentationItem rep = fp.Items.FirstOrDefault();
                if (rep != null && rep is IfcGeometricCurveSet) //we have a set of curves and inner boundaries
                {
                }
                else if (rep != null)
                {
                }
            }
            return null;
        }

        #endregion

        public static void AddBoundingElement(this IfcSpace space, IModel model, IfcElement element,
                                              IfcPhysicalOrVirtualEnum physicalOrVirtualBoundary,
                                              IfcInternalOrExternalEnum internalOrExternalBoundary)
        {
            //avoid adding element which is already defined as bounding element
            if (space.HasBoundingElement(model, element)) return;

            IfcRelSpaceBoundary relation = model.New<IfcRelSpaceBoundary>(rel =>
                                                                              {
                                                                                  rel.RelatingSpace = space;
                                                                                  rel.InternalOrExternalBoundary =
                                                                                      internalOrExternalBoundary;
                                                                                  rel.RelatedBuildingElement = element;
                                                                                  rel.PhysicalOrVirtualBoundary =
                                                                                      physicalOrVirtualBoundary;
                                                                              });
        }

        public static bool HasBoundingElement(this IfcSpace space, IModel model, IfcElement element)
        {
            IfcRelSpaceBoundary relation =
                model.InstancesWhere<IfcRelSpaceBoundary>(
                    rel => rel.RelatingSpace == space && rel.RelatedBuildingElement == element).FirstOrDefault();
            return relation != null;
        }
    }
}