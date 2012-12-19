﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xbim.SceneJSWebViewer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Media.Media3D;
    using Xbim.Ifc2x3.PresentationAppearanceResource;
    using Xbim.Ifc2x3.ProductExtension;
    using Xbim.Ifc2x3.SharedBldgElements;
    using Xbim.IO;
    using Xbim.ModelGeometry.Scene;
    using Xbim.Common.Logging;
    using Xbim.XbimExtensions;
    using Xbim.XbimExtensions.Interfaces;
    using Xbim.Ifc2x3.Kernel;

    /// <summary>
    /// An XBim implementation of an <see cref="IModelStream"/>. 
    /// </summary>
    /// <remarks>Provides access to semantic and geometric data within an IFC model
    /// using the XBIM.IFC library</remarks>
    public class XBimModelStream : IModelStream
    {
        #region private members
        private List<XbimSurfaceStyle> MaterialList;
        private List<String> TypeList = new List<string>();
        private List<GeometryHeader> ProductsList = new List<GeometryHeader>();
        private Camera DefaultCamera = new Camera();
        private XbimModel _model;

        private string _modelId;
        #endregion


        #region Static / Factory Members
        /// <summary>
        /// A Factory method to acquire a <see cref="IModelStream"/> for the requested model file.
        /// </summary>
        /// <param name="model">A path to the required XBIM file</param>
        /// <returns></returns>
        public static IModelStream GetModelStream(String model)
        {
            IModelStream stream;
            bool success = models.TryGetValue(model, out stream);
            if (success && stream != null)
            {
                return stream;
            }
            else
            {
                stream = new XBimModelStream(model);
                models.AddOrUpdate(model, (k) => stream, (k, v) => stream);
                return stream;
            }
        }

        // TODO: should this be a Dispose pattern, rather than static?
        /// <summary>
        /// Explicitly close the model, and release any resources
        /// </summary>
        /// <param name="model"></param>
        public static void CloseModel(String model)
        {
            IModelStream stream;
            bool success = models.TryRemove(model, out stream);
            if (success)
            {
                Logger.DebugFormat("Closing Model {0}", model);
                stream.Close();
                // TODO: Implement Displose pattern
            }
            stream = null;
        }

        private static readonly ILogger Logger = LoggerFactory.GetLogger();
        private static ConcurrentDictionary<String, IModelStream> models = new ConcurrentDictionary<String, IModelStream>();

        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="XBimModelStream"/> class from being created.
        /// </summary>
        /// <param name="model">The model.</param>
        private XBimModelStream(String model)
        {
            string xbimFile = model + ".xbim";
           

            if (!File.Exists(xbimFile))
            {
                Logger.WarnFormat("Timed out waiting for XBIM files to become available for model : {0}", model);
                throw new System.TimeoutException("Model Stream Timed Out Waiting for Model Caching");
            }

            _modelId = model;
            _model = new XbimModel();
            _model.Open(xbimFile);
             Init(model);
        }

        #endregion

       


        #region SceneJSTest.IModelStream
        public void Close()
        {
            
            _model.Dispose();
            
            _model = null;
        }

        private BoundingBox GetModelBounds()
        {
            BoundingBox box = new BoundingBox();

            foreach (XbimGeometryData shape in _model.GetGeometryData(XbimGeometryType.BoundingBox))
            {
                
                Matrix3D matrix3d = new Matrix3D().FromArray(shape.TransformData);
                BoundingBox bb = BoundingBox.FromArray(shape.ShapeData);
                bb.TransformBy(matrix3d);
                box.IncludeBoundingBox(bb);
            }
            return box;
        }
        public Camera GetCamera()
        {
            //get the model boundaries
            BoundingBox box = GetModelBounds();
            return new Camera(box.PointMin.X, box.PointMin.Y, box.PointMin.Z, box.PointMax.X, box.PointMax.Y, box.PointMax.Z);
        }

        public GeometryData GetGeometryData(String entityId)
        {
            Int32 id = Convert.ToInt32(entityId);
            MemoryStream ms = new MemoryStream();
            ushort tally = 0;
            byte[] wm = null;
            foreach (XbimGeometryData geom in _model.GetGeometryData(id, XbimGeometryType.TriangulatedMesh))
            {
                ms.Write(geom.ShapeData, 0, geom.ShapeData.Length);
                tally++;
                if (wm == null) wm = geom.TransformData;
            }
            return new GeometryData(id, ms.ToArray(), 0, tally, wm);

        }

        public List<GeometryHeader> GetGeometryHeaders()
        {
            return ProductsList;
        }

        public List<XbimSurfaceStyle> GetMaterials()
        {
            return MaterialList;
        }

        public List<String> GetTypes()
        {
            return TypeList;
        }

  
        private static Material GetDefinedMaterial(IfcSurfaceStyle surfaceStyle)
        {
            
            if (surfaceStyle == null || surfaceStyle.Styles.Count == 0) return null;
            IfcSurfaceStyleRendering rgb = surfaceStyle.Styles.First as IfcSurfaceStyleRendering;
            if (rgb == null) return null;
            string materialName = surfaceStyle.Name.HasValue ? surfaceStyle.Name.Value.ToString() : surfaceStyle.EntityLabel.ToString();
            return new Material(materialName + "Material",
                     rgb.SurfaceColour.Red,
                     rgb.SurfaceColour.Green,
                     rgb.SurfaceColour.Blue,
                     (1.0 - (double)rgb.Transparency.Value.Value),
                     0.0);
        }

        private Func<TransformNode, bool> FilterByType(Type t)
        {
            return p => (p.Product.GetType() == t || p.Product.GetType().IsSubclassOf(t));
        }

        public void Init(string model)
        {

            var handles = _model.GetGeometryHandles();
            var surfaceStyles = handles.GetSurfaceStyles();
            MaterialList = new List<XbimSurfaceStyle>(surfaceStyles);
            foreach (XbimSurfaceStyle surfaceStyle in MaterialList)
            {
                
                 //try to get any material defined in the model
                Material definedMaterial = GetDefinedMaterial(surfaceStyle.IfcSurfaceStyle(_model));
                surfaceStyle.TagRenderMaterial = definedMaterial; //store the material
                String materialName = String.Empty;
                Material material = null;
                if (definedMaterial != null)
                {
                    materialName = definedMaterial.Name;
                    material = definedMaterial;
                }
                else
                {
                    material = DefaultMaterials.LookupMaterial(surfaceStyle.IfcTypeId);
                    if (material == null)
                    {
                        Logger.WarnFormat("Could not locate default material for entity type #{0} in model {1}", surfaceStyle.IfcType.Name, _modelId);
                        // set null material as SHOCKING PINK
                        material = new Material(surfaceStyle.IfcType.Name, 0.98823529411764705882352941176471d, 0.05882352941176470588235294117647d, 0.75294117647058823529411764705882d, 1.0d, 0.0d);
                    }
                    materialName = material.Name;
                }


                //add all the products with per surface style
                GeometryHeader geomHeader = new GeometryHeader();
                geomHeader.Type = materialName;
                geomHeader.Material = materialName;
                if (material.Alpha < 1)
                {
                    geomHeader.LayerPriority = 1;
                }
                MaterialList.Add(surfaceStyle);
                ProductsList.Add(geomHeader);
                foreach (var geomHandle in handles.GetGeometryHandles(surfaceStyle))
                {
                    geomHeader.Geometries.Add(geomHandle.GeometryLabel.ToString());
                }
            }

        }

        private void DumpProducts()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("-- Types: {0}", TypeList.Count);
            sb.AppendLine();
            foreach (string type in TypeList)
            {
                sb.Append("\t");
                sb.AppendLine(type);
            }

            sb.AppendLine();
            sb.AppendFormat("-- Materials: {0}", MaterialList.Count);
            sb.AppendLine();
            foreach (var material in MaterialList)
            {
                sb.Append("\t");
                sb.AppendLine(material.TagRenderMaterial.ToString());
            }

            sb.AppendLine();
            sb.AppendFormat("-- Products: {0}", ProductsList.Count);
            sb.AppendLine("\t [Type] - [Material]");
            sb.AppendLine();
            long totalEntities = 0;
            foreach (var product in ProductsList)
            {
                sb.Append("\t");
                sb.Append(product.Type);
                sb.Append(" - ");
                sb.Append(product.Material);
                sb.AppendFormat(": {0} entities", product.Geometries.Count);
                sb.AppendLine();
                totalEntities += product.Geometries.Count;
            }
            sb.AppendFormat("-- Total Entities: {0}", totalEntities);
            Logger.DebugFormat("- Model Manifest for: {0}\n\n{1}", _modelId, sb);
        }

        public string QueryData(string id, string query)
        {
            IfcProduct product = _model.Instances[Convert.ToInt32(id)] as IfcProduct;
            if (product != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ID " + id);
                sb.Append(" Name: ");
                sb.Append(product.ToString());
                sb.Append(", IFC Type: " + product.GetType().Name);
                Logger.DebugFormat("Query result: ", sb);
                return sb.ToString();
            }
            else
                return "You sent a query of: '" + query + "' for id: '" + id + "'";
        }

        #endregion SceneJSTest.IModelStream

    }
}