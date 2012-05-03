#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcStructuralLoadSingleDisplacements.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.StructuralLoadResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcStructuralLoadSingleDisplacements
    {
        private readonly IModel _model;

        public IfcStructuralLoadSingleDisplacements(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcStructuralLoadSingleDisplacement> Items
        {
            get { return this._model.InstancesOfType<IfcStructuralLoadSingleDisplacement>(); }
        }

        public IfcStructuralLoadSingleDisplacementDistortions IfcStructuralLoadSingleDisplacementDistortions
        {
            get { return new IfcStructuralLoadSingleDisplacementDistortions(_model); }
        }
    }
}