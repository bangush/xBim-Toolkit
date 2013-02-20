﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Presentation
// Filename:    DrawingControl3D.xaml.cs
// Published:   01, 2012
// Last Edited: 9:05 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.ModelGeometry;
using Xbim.ModelGeometry.Scene;
using Xbim.XbimExtensions;
using Xbim.Ifc2x3.SharedComponentElements;
using Xbim.XbimExtensions.Interfaces;
using Xbim.IO;
using System.Diagnostics;
using System.Windows.Markup;
using Xbim.Common.Exceptions;
using System.Threading;
using Xbim.Ifc2x3;
using HelixToolkit.Wpf;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xbim.Common.Geometry;

#endregion

namespace Xbim.Presentation
{


    /// <summary>
    ///   Interaction logic for DrawingControl3D.xaml
    /// </summary>
    public partial class DrawingControl3D : UserControl
    {
        public DrawingControl3D()
        {
            InitializeComponent();
            Viewport = Canvas;
            Canvas.MouseDown += Canvas_MouseDown;
            this.Loaded += DrawingControl3D_Loaded;
           
        }

        void DrawingControl3D_Loaded(object sender, RoutedEventArgs e)
        {
            ShowSpaces = false;
        }


        #region Fields
        private List<XbimScene<WpfMeshGeometry3D, WpfMaterial>> scenes = new List<XbimScene<WpfMeshGeometry3D, WpfMaterial>>();


      

       
        protected RayMeshGeometry3DHitTestResult _hitResult;
       
        private XbimRect3D modelBounds;
        private XbimRect3D viewBounds;
        private event ProgressChangedEventHandler _progressChanged;
        private int? _currentProduct; 
        public event ProgressChangedEventHandler ProgressChanged
        {
            add { _progressChanged += value; }
            remove { _progressChanged -= value; }
        }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public Model3D Model3d { get; set; }



        #endregion

        #region Events

        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent("SelectionChangedEvent", RoutingStrategy.Bubble,
                                             typeof(SelectionChangedEventHandler), typeof(DrawingControl3D));

        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        public new static readonly RoutedEvent LoadedEvent =
            EventManager.RegisterRoutedEvent("LoadedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                                             typeof(DrawingControl3D));

        public new event RoutedEventHandler Loaded
        {
            add { AddHandler(LoadedEvent, value); }
            remove { RemoveHandler(LoadedEvent, value); }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            var pos = e.GetPosition(Canvas);
            var hit = FindHit(pos);
        
            if (hit != null)
            {
                XbimMeshLayer<WpfMeshGeometry3D, WpfMaterial> layer = hit.ModelHit.GetValue(TagProperty) as XbimMeshLayer<WpfMeshGeometry3D, WpfMaterial>; //get the fragments
                if (layer!=null)
                {

                    var frag = layer.Visible.Meshes.Find(hit.VertexIndex1);
                    if (!frag.IsEmpty)
                    {

                        MeshGeometry3D m = ((WpfMeshGeometry3D)(layer.Visible)).GetWpfMeshGeometry3D(frag);
                        Highlighted.Mesh = new Mesh3D(m.Positions, m.TriangleIndices);
                       
                        int id = frag.EntityLabel;
                        IList remove;
                        if (_currentProduct.HasValue)
                        {
                            remove = new int[] { _currentProduct.Value };
                        }
                        else
                            remove = new int[] { };
                        _hitResult = hit;
                        _currentProduct = (int)id;
              
                        SelectedItem = _currentProduct.Value;
                        //if (!PropertiesBillBoard.IsRendering)
                        //{
                        //    this.Viewport.Children.Add(PropertiesBillBoard);
                        //    PropertiesBillBoard.IsRendering = true;
                        //}
                        //PropertiesBillBoard.Text = Model.Instances[_currentProduct.Value].SummaryString().EnumerateToString(null, "\n");
                        //PropertiesBillBoard.Position = hit.PointHit;
             
                
                        return;
                    }
                }
            }

            //PropertiesBillBoard.IsRendering = false;          
            //this.Viewport.Children.Remove(PropertiesBillBoard);
            
            Highlighted.Mesh = null;
            _currentProduct = null;
            SelectedItem = -1;


        }

        #endregion

        #region Dependency Properties



        public XbimModel Model
        {
            get { return (XbimModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(XbimModel), typeof(DrawingControl3D), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                                                                      new PropertyChangedCallback(OnModelChanged)));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                XbimModel model = e.NewValue as XbimModel;
                d3d.LoadGeometry(model);
            }

        }


        public bool ShowWalls
        {
            get { return (bool)GetValue(ShowWallsProperty); }
            set { SetValue(ShowWallsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowWallsProperty =
            DependencyProperty.Register("ShowWalls", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowWallsChanged));

        private static void OnShowWallsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    bool on = (bool)e.NewValue;
                    if (on)
                        d3d.Show<IfcWall>();
                    else
                        d3d.Hide<IfcWall>();
                }
            }
        }

        public bool ShowDoors
        {
            get { return (bool)GetValue(ShowDoorsProperty); }
            set { SetValue(ShowDoorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDoorsProperty =
            DependencyProperty.Register("ShowDoors", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowDoorsChanged));

        private static void OnShowDoorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    bool on = (bool)e.NewValue;
                    if (on)
                        d3d.Show<IfcDoor>();
                    else
                        d3d.Hide<IfcDoor>();
                }
            }
        }

        public bool ShowWindows
        {
            get { return (bool)GetValue(ShowWindowsProperty); }
            set { SetValue(ShowWindowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowWindowsProperty =
            DependencyProperty.Register("ShowWindows", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowWindowsChanged));

        private static void OnShowWindowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                        d3d.Show<IfcWindow>();
                    else
                        d3d.Hide<IfcWindow>();
                }
            }
        }

        public bool ShowSlabs
        {
            get { return (bool)GetValue(ShowSlabsProperty); }
            set { SetValue(ShowSlabsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSlabsProperty =
            DependencyProperty.Register("ShowSlabs", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowSlabsChanged));

        private static void OnShowSlabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                        d3d.Show<IfcSlab>();
                    else
                        d3d.Hide<IfcSlab>();
                }
            }
        }
        public bool ShowFurniture
        {
            get { return (bool)GetValue(ShowFurnitureProperty); }
            set { SetValue(ShowFurnitureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowFurnitureProperty =
            DependencyProperty.Register("ShowFurniture", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowFurnitureChanged));

        private static void OnShowFurnitureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                        d3d.Show<IfcFurnishingElement>();
                    else
                        d3d.Hide<IfcFurnishingElement>();
                }
            }
        }

        public bool ShowGridLines
        {
            get { return (bool)GetValue(ShowGridLinesProperty); }
            set { SetValue(ShowGridLinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowGridLinesChanged));

        private static void OnShowGridLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                        d3d.Viewport.Children.Insert(0, d3d.GridLines);
                    else
                        d3d.Viewport.Children.Remove( d3d.GridLines);
                }
            }
        }
        public bool ShowSpaces
        {
            get { return (bool)GetValue(ShowSpacesProperty); }
            set { SetValue(ShowSpacesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWalls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSpacesProperty =
            DependencyProperty.Register("ShowSpaces", typeof(bool), typeof(DrawingControl3D), new UIPropertyMetadata(true, OnShowSpacesChanged));

        private static void OnShowSpacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingControl3D d3d = d as DrawingControl3D;
            if (d3d != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                        d3d.Show<IfcSpace>();
                    else
                        d3d.Hide<IfcSpace>();
                }
            }
        }

        public HelixToolkit.Wpf.HelixViewport3D Viewport
        {
            get { return (HelixToolkit.Wpf.HelixViewport3D)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Viewport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register("Viewport", typeof(HelixToolkit.Wpf.HelixViewport3D), typeof(DrawingControl3D), new PropertyMetadata(null));



        public int SelectedItem
        {
            get { return (int)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(int?), typeof(DrawingControl3D),
                                        new UIPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedItemChanged)));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DrawingControl3D)
            {
                DrawingControl3D d3d = d as DrawingControl3D;
                if (e.OldValue is int) //there is an old value to deselect
                {
                    int oldVal = (int)e.OldValue;
                    d3d.Deselect(oldVal);

                }
                if (e.NewValue is int)
                {
                    int newVal = (int)e.NewValue;
                    d3d.Select(newVal);
                }


            }

        }

        private void Deselect(int oldVal)
        {
           
            Highlighted.Mesh = null;
        }

        private void Select(int newVal)
        {
            foreach (var scene in scenes)
            {
                IXbimMeshGeometry3D mesh = scene.GetMeshGeometry3D(newVal);
                WpfMeshGeometry3D wpfGeom = new WpfMeshGeometry3D(mesh);
                Highlighted.Mesh = new Mesh3D(wpfGeom.Mesh.Positions, wpfGeom.Mesh.TriangleIndices);
               
            }
        }


        private RayMeshGeometry3DHitTestResult FindHit(Point position)
        {
            RayMeshGeometry3DHitTestResult result = null;
            HitTestResultCallback callback = hit =>
            {
                var rayHit = hit as RayMeshGeometry3DHitTestResult;
                if (rayHit != null)
                {
                    if (rayHit.MeshHit != null)
                    {
                        result = rayHit;
                        return HitTestResultBehavior.Stop;
                    }
                }

                return HitTestResultBehavior.Continue;
            };
            var hitParams = new PointHitTestParameters(position);
            VisualTreeHelper.HitTest(Viewport.Viewport, null, callback, hitParams);
            return result;
        }



        #endregion

      


        public double PercentageLoaded
        {
            get { return (double)GetValue(PercentageLoadedProperty); }
            set { SetValue(PercentageLoadedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PercentageLoaded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageLoadedProperty =
            DependencyProperty.Register("PercentageLoaded", typeof(double), typeof(DrawingControl3D),
                                        new UIPropertyMetadata(0.0));
      

        private void ClearGraphics()
        {
            PercentageLoaded = 0;
            _hitResult = null;
            _currentProduct = null;
            Opaques.Children.Clear();
            Transparents.Children.Clear();
            modelBounds = XbimRect3D.Empty;
            viewBounds = new XbimRect3D(0, 0, 0, 10000, 10000, 5000);    
            scenes = new List<XbimScene<WpfMeshGeometry3D, WpfMaterial>>();
            Viewport.ResetCamera();
           // PropertiesBillBoard.IsRendering = false;
            Highlighted.Mesh = null;
            

        }



        private XbimRect3D GetModelBounds(XbimModel model)
        {
            XbimRect3D box = new XbimRect3D();
            if (model == null) return box;
            bool first = true;
            foreach (XbimGeometryData shape in model.GetGeometryData(XbimGeometryType.BoundingBox))
            {
                XbimMatrix3D matrix3d = XbimMatrix3D.FromArray(shape.TransformData);
                XbimRect3D bb = XbimRect3D.FromArray(shape.ShapeData);
                bb.TransformBy(matrix3d);
                if (first) { box = bb; first = false; }
                else box.Union(bb);
            }
            return box;
        }

        private void LoadGeometry(XbimModel model)
        {
            
            //reset all the visuals
            ClearGraphics();
            
            if (model == null) return; //nothing to do
            model.RefencedModels.CollectionChanged += RefencedModels_CollectionChanged;
            //build the geometric scene and render as we go
            XbimScene<WpfMeshGeometry3D, WpfMaterial> scene = BuildScene(model);
            
            scenes.Add(scene);
            ShowSpaces = false;
            RecalculateView(model);
           
        }

        private void RecalculateView(XbimModel model)
        {
            if (!modelBounds.IsEmpty) //we have  geometry so create view box
                viewBounds = modelBounds;

            //adjust for the units of the model
            double metre = model.GetModelFactors.OneMetre;
            Viewport.DefaultCamera.NearPlaneDistance = 0.125 * metre;
            Viewport.Camera.NearPlaneDistance = 0.125 * metre;
            Viewport.DefaultCamera.FarPlaneDistance = Math.Max(Math.Max(
                                                                viewBounds.SizeX,
                                                                viewBounds.SizeY),
                                                                viewBounds.SizeY) * 3;
            Viewport.Camera.FarPlaneDistance = Viewport.DefaultCamera.FarPlaneDistance;

            //get bounding box for the whole scene and adapt gridlines to the model units

            double metresWide = viewBounds.SizeY;
            double metresLong = viewBounds.SizeX;
            long gridWidth = Convert.ToInt64(metresWide / (metre * 10));
            long gridLen = Convert.ToInt64(metresLong / (metre * 10));
            if (gridWidth > 10 || gridLen > 10)
                this.GridLines.MinorDistance = metre * 10;
            else
                this.GridLines.MinorDistance = metre;
            this.GridLines.Width = (gridWidth + 1) * 10 * metre;
            this.GridLines.Length = (gridLen + 1) * 10 * metre;

            this.GridLines.MajorDistance = metre * 10;
            this.GridLines.Thickness = 0.01 * metre;
            XbimPoint3D p3d = viewBounds.Centroid();
            TranslateTransform3D t3d = new TranslateTransform3D(p3d.X, p3d.Y, viewBounds.Z);
            this.GridLines.Transform = t3d;
           
            //make sure whole scene is visible
            ViewHome();
        }

       
        private void DrawLayer(XbimMeshLayer<WpfMeshGeometry3D, WpfMaterial> layer)
        {
            
            //move it to the visual element
           
            layer.Show();
            
            GeometryModel3D m3d = (WpfMeshGeometry3D)layer.Visible;
            m3d.SetValue(TagProperty, layer);
            //sort out materials and bind
            if (layer.Style.RenderBothFaces)
                m3d.BackMaterial = m3d.Material = (WpfMaterial)layer.Material;
            else if (layer.Style.SwitchFrontAndRearFaces)
                m3d.BackMaterial = (WpfMaterial)layer.Material;
            else
                m3d.Material = (WpfMaterial)layer.Material;
            ModelVisual3D mv = new ModelVisual3D();
            mv.Content = m3d;
            if (layer.Style.IsTransparent)
                Transparents.Children.Add(mv);
            else
                Opaques.Children.Add(mv);
            foreach (var subLayer in layer.SubLayers)
                DrawLayer(subLayer);
        }

        void RefencedModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
            {
                XbimReferencedModel refModel = e.NewItems[0] as XbimReferencedModel;
                XbimScene<WpfMeshGeometry3D, WpfMaterial> scene = BuildScene(refModel.Model);
                scenes.Add(scene);
                RecalculateView(Model);
            }
        }

        private XbimScene<WpfMeshGeometry3D, WpfMaterial> BuildScene(XbimModel model)
        {
            XbimScene<WpfMeshGeometry3D, WpfMaterial> scene = new XbimScene<WpfMeshGeometry3D, WpfMaterial>(model);
            XbimGeometryHandleCollection handles = new XbimGeometryHandleCollection(model.GetGeometryHandles()
                                                       .Exclude(IfcEntityNameEnum.IFCFEATUREELEMENT));
            double total = handles.Count;
            double processed = 0;
          
            Parallel.ForEach<KeyValuePair<string,XbimGeometryHandleCollection>>(handles.FilterByBuildingElementTypes(), layerContent =>
       //  foreach (var layerContent in handles.FilterByBuildingElementTypes())
	
            {
                string elementTypeName = layerContent.Key;
                XbimGeometryHandleCollection layerHandles = layerContent.Value;
                IEnumerable<XbimGeometryData> geomColl = model.GetGeometryData(layerHandles);
                XbimColour colour = scene.LayerColourMap[elementTypeName];
                XbimMeshLayer<WpfMeshGeometry3D, WpfMaterial> layer = new XbimMeshLayer<WpfMeshGeometry3D, WpfMaterial>(colour) { Name = elementTypeName };              
                //add all content initially into the hidden field
                foreach (var geomData in geomColl)
                {
                    layer.AddToHidden(geomData, model);
                    processed++;
                    int progress = Convert.ToInt32(100.0 * processed / total);
                }
                this.Dispatcher.BeginInvoke(new Action(() => { DrawLayer(layer); }), System.Windows.Threading.DispatcherPriority.Background);
                lock (scene)
                {
                    scene.Add(layer);
                    if (modelBounds.IsEmpty) modelBounds = layer.BoundingBoxHidden();
                    else modelBounds.Union(layer.BoundingBoxHidden());
                }
            }
            );
            this.Dispatcher.BeginInvoke(new Action(() => { Hide<IfcSpace>(); }), System.Windows.Threading.DispatcherPriority.Background);
            //scene.Balance();   
            return scene;
        }


        private void DrawScene(XbimScene<WpfMeshGeometry3D, WpfMaterial> scene)
        {

            foreach (var layer in scene.Layers.Where(l => l.HasContent))
            {
                //move it to the visual element
                layer.ShowAll();
                GeometryModel3D m3d = (WpfMeshGeometry3D)layer.Visible;
                m3d.SetValue(TagProperty, layer.Name);         
                //sort out materials and bind
                if (layer.Style.RenderBothFaces)
                    m3d.BackMaterial = m3d.Material = (WpfMaterial)layer.Material;
                else if (layer.Style.SwitchFrontAndRearFaces)
                    m3d.BackMaterial = (WpfMaterial)layer.Material;
                else
                    m3d.Material = (WpfMaterial)layer.Material;
                ModelVisual3D mv = new ModelVisual3D();
                mv.Content = m3d;
                if (layer.Style.IsTransparent)
                    Transparents.Children.Add(mv);
                else
                    Opaques.Children.Add(mv);
            }
        }

        private void GenerateGeometry(object s, DoWorkEventArgs args)
        {
            //BackgroundWorker worker = s as BackgroundWorker;
            //XbimModel model = args.Argument as XbimModel;

            //if (worker != null && model != null)
            //{
            //    worker.ReportProgress(0, "Reading Geometry");

            //    XbimGeometryHandleCollection handles = new XbimGeometryHandleCollection(model.GetGeometryHandles()
            //                                            .Exclude(IfcEntityNameEnum.IFCFEATUREELEMENT));
            //    double total = handles.Count;
            //    double processed = 0;
            //    foreach (var ss in handles.GetSurfaceStyles())
            //    {
            //        ss.GeometryData = model.GetGeometryData(handles.GetGeometryHandles(ss)).ToList();
            //        processed += ss.GeometryData.Count;
            //        int progress = Convert.ToInt32(100.0 * processed / total);
            //        worker.ReportProgress(progress, ss);
            //        Thread.Sleep(100);
            //    }
            //}
            //worker.ReportProgress(-1, "Complete");
            //args.Result = model;
        }




       

        /// <summary>
        ///   Hides all instances of the specified type
        /// </summary>
        public void Hide<T>()
        {
            IfcType ifcType = IfcMetaData.IfcType(typeof(T));
            string toHide = ifcType.Name + ";";
            foreach (var subType in ifcType.NonAbstractSubTypes)
                toHide += subType.Name + ";";
            foreach (var scene in scenes)
                foreach (var layer in scene.SubLayers) //go over top level layers only
                    if (toHide.Contains(layer.Name + ";"))
                        layer.HideAll();
        }

        public void Hide(int hideProduct)
        {
            //ModelVisual3D item;
            //if (_items.TryGetValue(hideProduct, out item))
            //{
            //    ModelVisual3D parent = VisualTreeHelper.GetParent(item) as ModelVisual3D;
            //    if (parent != null)
            //    {
            //        _hidden.Add(item, parent);
            //        parent.Children.Remove(item);
            //    }
            //    return;
            //}
        }

        private void Show<T>()
        {
            IfcType ifcType = IfcMetaData.IfcType(typeof(T));
            string toShow = ifcType.Name + ";";
            foreach (var subType in ifcType.NonAbstractSubTypes)
                toShow += subType.Name + ";";
            foreach (var scene in scenes)
                foreach (var layer in scene.SubLayers) //go over top level layers only
                    if (toShow.Contains(layer.Name + ";"))
                        layer.ShowAll();
        }

        public void ShowAll()
        {
            //scene.ShowAll();
        }

        public void HideAll()
        {
            //scene.HideAll();
        }


        public void ViewHome()
        {
            XbimPoint3D c = viewBounds.Centroid();
            Point3D p = new Point3D(c.X, c.Y, c.Z);
            Viewport.Camera = Viewport.DefaultCamera;
            CameraHelper.LookAt(Viewport.Camera, p, new Vector3D(-1, 1, -0.5), new Vector3D(0, 0, 1), 0);
            Rect3D r3d = new Rect3D(viewBounds.X, viewBounds.Y, viewBounds.Z, viewBounds.SizeX, viewBounds.SizeY, viewBounds.SizeZ);
            Viewport.ZoomExtents(r3d);
            
        }


        public void ZoomSelected()
        {
            //ModelVisual3D selVis;
            //if (SelectedItem.HasValue && _items.TryGetValue(SelectedItem.Value, out selVis))
            //{
            //    Rect3D bounds = VisualTreeHelper.GetDescendantBounds(selVis);
            //    if (!bounds.IsEmpty)
            //    {
            //        bounds = bounds.Inflate(bounds.SizeX / 2, bounds.SizeY / 2, bounds.SizeZ / 2);
            //        Viewport.ZoomExtents(bounds);
            //    }
            //}
        }


    }
}