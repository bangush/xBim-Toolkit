#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcRelDecomposess.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.Kernel;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcRelDecomposess
    {
        private readonly IModel _model;

        public IfcRelDecomposess(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcRelDecomposes> Items
        {
            get { return this._model.InstancesOfType<IfcRelDecomposes>(); }
        }

        public IfcRelNestss IfcRelNestss
        {
            get { return new IfcRelNestss(_model); }
        }

        public IfcRelAggregatess IfcRelAggregatess
        {
            get { return new IfcRelAggregatess(_model); }
        }
    }
}