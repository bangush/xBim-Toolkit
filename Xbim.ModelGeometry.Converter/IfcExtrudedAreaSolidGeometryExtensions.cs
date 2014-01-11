﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;

namespace Xbim.ModelGeometry.Converter
{
    public static class IfcExtrudedAreaSolidGeometryExtensions
    {
        /// <summary>
        /// returns a Hash for the geometric behaviour of this object
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public static int GetGeometryHashCode(this IfcExtrudedAreaSolid solid)
        {
            int round = solid.ModelOf.ModelFactors.Rounding;
            return Math.Round(solid.Depth, round).GetHashCode() ^ 
                   solid.ExtrudedDirection.GetGeometryHashCode() ^
                   solid.Position.GetGeometryHashCode() ^ 
                   solid.SweptArea.GetGeometryHashCode();
        }

        /// <summary>
        /// Compares two objects for geomtric equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">object to compare with</param>
        /// <returns></returns>
        public static bool GeometricEquals(this IfcExtrudedAreaSolid a, IfcRepresentationItem b)
        {
            IfcExtrudedAreaSolid eas = b as IfcExtrudedAreaSolid;
            if(eas == null) return false; //different types are not the same
            double precision = a.ModelOf.ModelFactors.Precision;
            return Math.Abs(a.Depth - eas.Depth) <= precision &&
                   a.ExtrudedDirection.GeometricEquals(eas.ExtrudedDirection) &&
                   a.Position.GeometricEquals(eas.Position) &&
                   a.SweptArea.GeometricEquals(eas.SweptArea);
        }
    }
}
