﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcAnnotationCurveOccurrence.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using Xbim.Ifc.GeometryResource;
using Xbim.Ifc.SelectTypes;

#endregion

namespace Xbim.Ifc.PresentationDefinitionResource
{
    public class IfcAnnotationCurveOccurrence : IfcAnnotationOccurrence, IfcDraughtingCalloutElement
    {
        public override string WhereRule()
        {
            string baseErr = base.WhereRule();
            if (Item != null && !(Item is IfcCurve))
                baseErr +=
                    "WR31 AnnotationCurveOccurrence : The Item that is styled by an IfcAnnotationCurveOccurrence relation shall be (if provided) a subtype of IfcCurve. ";
            return baseErr;
        }
    }
}