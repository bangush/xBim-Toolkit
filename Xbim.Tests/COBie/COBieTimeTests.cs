﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBie;
using Xbim.XbimExtensions;
using Xbim.IO;
using System.Diagnostics;

namespace Xbim.Tests.COBie
{
    [DeploymentItem(SourceFile, Root)]
    [TestClass]
    public class COBieTimeTests
    {
        private const string Root = "TestSourceFiles";
        private const string SourceModelLeaf = "Clinic-Handover.xbim";
        private const string SourceFile = Root + @"\" + SourceModelLeaf;
        
        static COBieContext _cobieContext = new COBieContext();
        COBieQueries cobieEngine = new COBieQueries(_cobieContext);
        static IModel _model;

        [ClassInitialize]
        public static void LoadModel(TestContext context)
        {

            _model = new XbimFileModelServer();
            _model.Open(SourceFile);
            _cobieContext = new COBieContext();
            _cobieContext.COBieGlobalValues.Add("FILENAME", SourceFile);
            _cobieContext.COBieGlobalValues.Add("DEFAULTDATE", DateTime.Now.ToString(Constants.DATE_FORMAT));
            _cobieContext.Model = _model;
            COBieQueries cobieEngine = new COBieQueries(_cobieContext);
        }

        [ClassCleanup]
        public static void CloseModel()
        {
            if (_model != null)
                _model.Dispose();
            _model = null;
            _cobieContext = null;
        }
        
        [TestMethod]
        [Ignore]
        public void Time_On_All()
        {
            ContactTime();
            FacilityTime();
            FloorTime();
            SpaceTime();
            ZoneTime();
            TypeTime();
            ComponentTime();
            SystemTime();
            AssemblyTime();
            ConnectionTime();
            SpareTime();
            ResourceTime();
            JobTime();
            ImpactTime();
            DocumentTime();
            AttributeTime();
            //CoordinateTime();
            IssueTime();
        }

        [TestMethod]
        public void Time_On_Contacts()
        {
            Assert.IsTrue(ContactTime() < new TimeSpan(0,0,2));
        }

        [TestMethod]
        public void Time_On_Facility()
        {
            Assert.IsTrue(FacilityTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Floor()
        {
            Assert.IsTrue(FloorTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Space()
        {
            Assert.IsTrue(SpaceTime()  < new TimeSpan(0, 0, 4));
        }

        [TestMethod]
        public void Time_On_Zone()
        {
            Assert.IsTrue(ZoneTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Type()
        {
            Assert.IsTrue(TypeTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Component()
        {
            Assert.IsTrue(ComponentTime() < new TimeSpan(0, 0, 30));
        }

        [TestMethod]
        public void Time_On_System()
        {
            Assert.IsTrue(SystemTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Assembly()
        {
            Assert.IsTrue(AssemblyTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Connection()
        {
            Assert.IsTrue(ConnectionTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Spare()
        {
            Assert.IsTrue(SpareTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Resource()
        {
            Assert.IsTrue(ResourceTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Job()
        {
            Assert.IsTrue(JobTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Impact()
        {
            Assert.IsTrue(ImpactTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Document()
        {
            Assert.IsTrue(DocumentTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Attribute()
        {
            Assert.IsTrue(AttributeTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        [Ignore]
        public void Time_On_Coordinate()
        {
            Assert.IsTrue(CoordinateTime() < new TimeSpan(0, 0, 2));
        }

        [TestMethod]
        public void Time_On_Issue()
        {
            Assert.IsTrue(IssueTime() < new TimeSpan(0, 0, 2));
        }

        //---------------------------------------------------------------------------
        private TimeSpan ContactTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieContactSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Contact Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan FacilityTime()
        {
            //COBieQueries cobieEngine = new COBieQueries(_cobieContext);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieFacilitySheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Facility Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan FloorTime()
        {
            //COBieQueries cobieEngine = new COBieQueries(_cobieContext);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieFloorSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Floor Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan SpaceTime()
        {
            //COBieQueries cobieEngine = new COBieQueries(_cobieContext);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieSpaceSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Space Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan ZoneTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieZoneSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Zone Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan TypeTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieTypeSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Type Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan ComponentTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieComponentSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Component Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan SystemTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieSystemSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("System Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan AssemblyTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieAssemblySheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Assembly Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan ConnectionTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieConnectionSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Connection Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan SpareTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieSpareSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Spare Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan ResourceTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieResourceSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Resource Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan JobTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieJobSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Job Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan ImpactTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieImpactSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Impact Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan DocumentTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieDocumentSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Document Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan AttributeTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieAttributeSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Attribute Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan CoordinateTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieCoordinateSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Coordinate Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }

        private TimeSpan IssueTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            cobieEngine.GetCOBieIssueSheet();
            timer.Stop();
            Debug.WriteLine(string.Format("Issue Sheet Time = {0}", timer.Elapsed.TotalSeconds.ToString()));
            return timer.Elapsed;
        }


        
    }
}
