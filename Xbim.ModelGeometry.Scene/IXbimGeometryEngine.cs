﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Common.Geometry;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.RepresentationResource;
using Xbim.IO;
using Xbim.XbimExtensions;

namespace Xbim.ModelGeometry.Scene
{
    public interface IXbimGeometryEngine : IGeometryManager
    {
        /// <summary>
        /// Returns the geometry in mixed mode, this is the faster way of henerating the geometry
        /// </summary>
        /// <param name="product"></param>
        /// <param name="xbimGeometryType"></param>
        /// <returns></returns>
        IXbimGeometryModelGroup GetGeometry3D(IfcProduct product);

        /// <summary>
        /// Returns the geometry formatted to a specific type
        /// </summary>
        /// <param name="product"></param>
        /// <param name="xbimGeometryType"></param>
        /// <returns></returns>
        IXbimGeometryModelGroup GetGeometry3D(IfcProduct product, XbimGeometryType xbimGeometryType);
       
        /// <summary>
        /// Initialises the geometry engine and resets any cached data
        /// </summary>
        /// <param name="model"></param>
        void Init(XbimModel model);

        /// <summary>
        /// Returns the 3D geometry for the representation item
        /// </summary>
        /// <param name="repItem"></param>
        /// <returns></returns>
        IXbimGeometryModelGroup GetGeometry3D(IfcRepresentationItem repItem);


        IXbimGeometryModelGroup GetGeometry3D(IfcRepresentation representation);
        /// <summary>
        /// Returns the geometry in 3D of a given type 
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="xbimGeometryType"></param>
        /// <returns></returns>
        IXbimGeometryModelGroup GetGeometry3D(IfcSolidModel solid, XbimGeometryType xbimGeometryType);
        /// <summary>
        /// Returns a geometry object represented by the string data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xbimGeometryType"></param>
        /// <returns></returns>
        IXbimGeometryModel GetGeometry3D(String data, XbimGeometryType xbimGeometryType);
        /// <summary>
        /// Deflection used to calculate tangental angle when converting curves to linear segments during triangulation
        /// </summary>
        double Deflection { get; set; }
        /// <summary>
        /// The normal distance between two points at which they are determined to be the same
        /// </summary>
        double Precision { get; set; }
        /// <summary>
        /// The maximum distance between two points at which they are determined to be the same
        /// </summary>
        double PrecisionMax { get; set; }

        /// <summary>
        /// Starts or resumes the caching of  geometries
        /// </summary>
        /// <param name="clear">If true any existing cached data is cleared</param>
        void CacheStart(bool clear);

        /// <summary>
        /// Stops or pauses the caching of geometries
        /// </summary>
        /// <param name="p">If true any existing cached data is cleared</param>
        void CacheStop(bool clear);

        /// <summary>
        /// True if geometry caching is currently on
        /// </summary>
        /// <returns></returns>
        bool Caching { get; }
    }
}
