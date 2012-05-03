#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcEnergyPropertiess.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.SharedBldgServiceElements;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcEnergyPropertiess
    {
        private readonly IModel _model;

        public IfcEnergyPropertiess(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcEnergyProperties> Items
        {
            get { return this._model.InstancesOfType<IfcEnergyProperties>(); }
        }

        public IfcElectricalBasePropertiess IfcElectricalBasePropertiess
        {
            get { return new IfcElectricalBasePropertiess(_model); }
        }
    }
}