﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions;

namespace Xbim.IO
{
    public struct XbimGeometryHandle
    {
        /// <summary>
        /// The unique ID of the geometry
        /// </summary>
        public int GeometryLabel;
        /// <summary>
        /// The label of the Ifc Entity that holds the surface style render
        /// </summary>
        public int SurfaceStyleLabel;
        /// <summary>
        /// The label of the Ifc Entity that the geomtry represents
        /// </summary>
        public int ProductLabel;
        /// <summary>
        /// The id of the Ifc Type of the Product represented
        /// </summary>
        public short IfcTypeId;
        /// <summary>
        /// The type of geometric representation
        /// </summary>
        public XbimGeometryType GeometryType;
        /// <summary>
        /// A handle to a geometry object
        /// </summary>
        /// <param name="geometryLabel">The unique ID of the geometry</param>
        /// <param name="geometryType">The type of geometric representation</param>
        /// <param name="productLabel">The label of the Ifc Entity that the geomtry represents</param>
        /// <param name="ifcTypeId">The id of the Ifc Type of the Product represented</param>
        /// <param name="surfaceStyleLabel">The label of the Ifc Entity that holds the surface style render</param>
        public XbimGeometryHandle(int geometryLabel, XbimGeometryType geometryType, int productLabel, short ifcTypeId, int surfaceStyleLabel)
        {
            GeometryLabel = geometryLabel;
            SurfaceStyleLabel = surfaceStyleLabel;
            ProductLabel = productLabel;
            IfcTypeId = ifcTypeId;
            GeometryType = geometryType;
        }

        /// <summary>
        /// Returns the surface style for rendering this object
        /// </summary>
        public XbimSurfaceStyle SurfaceStyle
        {
            get
            {
                return new XbimSurfaceStyle(this.IfcTypeId, this.SurfaceStyleLabel);
            }
        }
    }
}
