#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcClassificationReferences.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.ExternalReferenceResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcClassificationReferences
    {
        private readonly IModel _model;

        public IfcClassificationReferences(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcClassificationReference> Items
        {
            get { return this._model.InstancesOfType<IfcClassificationReference>(); }
        }
    }
}