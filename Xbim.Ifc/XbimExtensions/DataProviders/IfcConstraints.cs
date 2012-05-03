#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcConstraints.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.ConstraintResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcConstraints
    {
        private readonly IModel _model;

        public IfcConstraints(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcConstraint> Items
        {
            get { return this._model.InstancesOfType<IfcConstraint>(); }
        }

        public IfcObjectives IfcObjectives
        {
            get { return new IfcObjectives(_model); }
        }

        public IfcMetrics IfcMetrics
        {
            get { return new IfcMetrics(_model); }
        }
    }
}