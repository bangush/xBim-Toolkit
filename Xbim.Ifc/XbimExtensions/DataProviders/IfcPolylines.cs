#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcPolylines.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc.GeometryResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcPolylines
    {
        private readonly IModel _model;

        public IfcPolylines(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcPolyline> Items
        {
            get { return this._model.InstancesOfType<IfcPolyline>(); }
        }
    }
}