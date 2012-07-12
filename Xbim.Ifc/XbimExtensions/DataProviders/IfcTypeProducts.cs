#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcTypeProducts.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc.Kernel;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcTypeProducts
    {
        private readonly IModel _model;

        public IfcTypeProducts(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcTypeProduct> Items
        {
            get { return this._model.InstancesOfType<IfcTypeProduct>(); }
        }

        public IfcElementTypes IfcElementTypes
        {
            get { return new IfcElementTypes(_model); }
        }

        public IfcWindowStyles IfcWindowStyles
        {
            get { return new IfcWindowStyles(_model); }
        }

        public IfcDoorStyles IfcDoorStyles
        {
            get { return new IfcDoorStyles(_model); }
        }
    }
}