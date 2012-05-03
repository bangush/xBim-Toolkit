﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcPumpTypeEnum.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

namespace Xbim.Ifc.HVACDomain
{
    /// <summary>
    ///   Defines general types of pumps
    /// </summary>
    public enum IfcPumpTypeEnum
    {
        CIRCULATOR,
        ENDSUCTION,
        SPLITCASE,
        VERTICALINLINE,
        VERTICALTURBINE,
        USERDEFINED,
        NOTDEFINED
    }
}