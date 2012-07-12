#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcRectangleHollowProfileDefs.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc.ProfileResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcRectangleHollowProfileDefs
    {
        private readonly IModel _model;

        public IfcRectangleHollowProfileDefs(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcRectangleHollowProfileDef> Items
        {
            get { return this._model.InstancesOfType<IfcRectangleHollowProfileDef>(); }
        }
    }
}