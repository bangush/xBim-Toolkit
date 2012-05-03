#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcEvaporatorTypes.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.HVACDomain;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcEvaporatorTypes
    {
        private readonly IModel _model;

        public IfcEvaporatorTypes(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcEvaporatorType> Items
        {
            get { return this._model.InstancesOfType<IfcEvaporatorType>(); }
        }
    }
}