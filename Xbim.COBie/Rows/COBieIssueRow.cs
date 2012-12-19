﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieIssueRow : COBieRow
    {
        public COBieIssueRow(ICOBieSheet<COBieIssueRow> parentSheet)
            : base(parentSheet) { }

        [COBieAttributes(0, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_PrimaryKey, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_ForeignKey, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required_Information, "CreatedOn", 19, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.IssueCategory", COBieAttributeState.Required_Reference_PickList, "Type", 255, COBieAllowedType.Text)]
        public string Type { get; set; }

        [COBieAttributes(4, COBieKeyType.ForeignKey, "PickLists.IssueRisk", COBieAttributeState.Required_Reference_PickList, "Risk", 255, COBieAllowedType.Text)]
        public string Risk { get; set; }

        [COBieAttributes(5, COBieKeyType.ForeignKey, "PickLists.IssueChance", COBieAttributeState.Required_Reference_PickList, "Chance", 255, COBieAllowedType.Text)]
        public string Chance { get; set; }

        [COBieAttributes(6, COBieKeyType.ForeignKey, "PickLists.IssueImpact", COBieAttributeState.Required_Reference_PickList, "Impact", 255, COBieAllowedType.Text)]
        public string Impact { get; set; }

        [COBieAttributes(7, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName1", 255, COBieAllowedType.Text)]
        public string SheetName1 { get; set; }

        [COBieAttributes(8, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_Reference_PrimaryKey, "RowName1", 255, COBieAllowedType.AlphaNumeric)]
        public string RowName1 { get; set; }

        [COBieAttributes(9, COBieKeyType.CompoundKey_ForeignKey, "PickLists.SheetType", COBieAttributeState.Required_Reference_PickList, "SheetName2", 255, COBieAllowedType.Text)]
        public string SheetName2 { get; set; }

        [COBieAttributes(10, COBieKeyType.CompoundKey, "", COBieAttributeState.Required_Reference_PrimaryKey, "RowName2", 255, COBieAllowedType.AlphaNumeric)]
        public string RowName2 { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(12, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required_Reference_PrimaryKey, "Owner", 255, COBieAllowedType.Email)]
        public string Owner { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.Required_Information, "Mitigation", 255, COBieAllowedType.AlphaNumeric)]
        public string Mitigation { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtSystem", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtSystem { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtObject", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtObject { get; set; }

        [COBieAttributes(16, COBieKeyType.None, "", COBieAttributeState.Required_System, "ExtIdentifier", 255, COBieAllowedType.AlphaNumeric)]
        public string ExtIdentifier { get; set; }
    }
}
