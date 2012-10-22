﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xbim.COBie.Rows
{
    [Serializable()]
    public class COBieTypeRow : COBieRow
    {
        public COBieTypeRow(ICOBieSheet<COBieTypeRow> parentSheet)
            : base(parentSheet) { }

        [COBieAttributes(0, COBieKeyType.PrimaryKey, "", COBieAttributeState.Required, "Name", 255, COBieAllowedType.AlphaNumeric)]
        public string Name { get; set; }

        [COBieAttributes(1, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required, "CreatedBy", 255, COBieAllowedType.Email)]
        public string CreatedBy { get; set; }

        [COBieAttributes(2, COBieKeyType.None, "", COBieAttributeState.Required, "CreatedOn", 19, COBieAllowedType.ISODateTime)]
        public string CreatedOn { get; set; }

        [COBieAttributes(3, COBieKeyType.ForeignKey, "PickLists.Category-Product", COBieAttributeState.Required, "Category", 255, COBieAllowedType.Text)]
        public string Category { get; set; }

        [COBieAttributes(4, COBieKeyType.None, "", COBieAttributeState.Required, "Description", 255, COBieAllowedType.AlphaNumeric)]
        public string Description { get; set; }

        [COBieAttributes(5, COBieKeyType.ForeignKey, "PickLists.AssetType", COBieAttributeState.Required, "AssetType", 255, COBieAllowedType.Text)]
        public string AssetType { get; set; }

        [COBieAttributes(6, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required, "Manufacturer", 255, COBieAllowedType.Email)]
        public string Manufacturer { get; set; }

        [COBieAttributes(7, COBieKeyType.None, "", COBieAttributeState.Required, "ModelNumber", 255, COBieAllowedType.AlphaNumeric)]
        public string ModelNumber { get; set; }

        [COBieAttributes(8, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required, "WarrantyGuarantorParts", 255, COBieAllowedType.Email)]
        public string WarrantyGuarantorParts { get; set; }

        [COBieAttributes(9, COBieKeyType.None, "", COBieAttributeState.Required, "WarrantyDurationParts", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string WarrantyDurationParts { get; set; }

        [COBieAttributes(10, COBieKeyType.ForeignKey, "Contact.Email", COBieAttributeState.Required, "WarrantyGuarantorLabor", 255, COBieAllowedType.Email)]
        [COBieAlias("WarrantyGuarantorLabour")]
        public string WarrantyGuarantorLabor { get; set; }

        [COBieAttributes(11, COBieKeyType.None, "", COBieAttributeState.Required, "WarrantyDurationLabor", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        [COBieAlias("WarrantyDurationLabour")]
        public string WarrantyDurationLabor { get; set; }

        [COBieAttributes(12, COBieKeyType.ForeignKey, "PickLists.DurationUnit", COBieAttributeState.Required, "WarrantyDurationUnit", 255, COBieAllowedType.Text)]
        public string WarrantyDurationUnit { get; set; }

        [COBieAttributes(13, COBieKeyType.None, "", COBieAttributeState.System, "ExtSystem", 255, COBieAllowedType.Text)]
        public string ExtSystem { get; set; }

        [COBieAttributes(14, COBieKeyType.None, "", COBieAttributeState.System, "ExtObject", 255, COBieAllowedType.Text)]
        public string ExtObject { get; set; }

        [COBieAttributes(15, COBieKeyType.None, "", COBieAttributeState.System, "ExtIdentifier", 255, COBieAllowedType.Text)]
        public string ExtIdentifier { get; set; }

        [COBieAttributes(16, COBieKeyType.None, "", COBieAttributeState.As_Specified, "ReplacementCost", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string ReplacementCost { get; set; }

        [COBieAttributes(17, COBieKeyType.None, "", COBieAttributeState.As_Specified, "ExpectedLife", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string ExpectedLife { get; set; }

        [COBieAttributes(18, COBieKeyType.None, "", COBieAttributeState.As_Specified, "DurationUnit", 255, COBieAllowedType.Text)]
        public string DurationUnit { get; set; }

        [COBieAttributes(19, COBieKeyType.None, "", COBieAttributeState.As_Specified, "WarrantyDescription", 255, COBieAllowedType.AlphaNumeric)]
        public string WarrantyDescription { get; set; }

        [COBieAttributes(20, COBieKeyType.None, "", COBieAttributeState.Required, "NominalLength", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string NominalLength { get; set; }

        [COBieAttributes(21, COBieKeyType.None, "", COBieAttributeState.Required, "NominalWidth", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string NominalWidth { get; set; }

        [COBieAttributes(22, COBieKeyType.None, "", COBieAttributeState.Required, "NominalHeight", Constants.DOUBLE_MAXSIZE, COBieAllowedType.Numeric)]
        public string NominalHeight { get; set; }

        [COBieAttributes(23, COBieKeyType.None, "", COBieAttributeState.As_Specified, "ModelReference", 255, COBieAllowedType.AlphaNumeric)]
        public string ModelReference { get; set; }

        [COBieAttributes(24, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Shape", 255, COBieAllowedType.AlphaNumeric)]
        public string Shape { get; set; }

        [COBieAttributes(25, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Size", 255, COBieAllowedType.AlphaNumeric)]
        public string Size { get; set; }

        [COBieAttributes(26, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Color", 255, COBieAllowedType.AlphaNumeric)]
        [COBieAlias("Colour")]
        public string Color { get; set; }

        [COBieAttributes(27, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Finish", 255, COBieAllowedType.AlphaNumeric)]
        public string Finish { get; set; }

        [COBieAttributes(28, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Grade", 255, COBieAllowedType.AlphaNumeric)]
        public string Grade { get; set; }

        [COBieAttributes(29, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Material", 255, COBieAllowedType.AlphaNumeric)]
        public string Material { get; set; }

        [COBieAttributes(30, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Constituents", 255, COBieAllowedType.AlphaNumeric)]
        public string Constituents { get; set; }

        [COBieAttributes(31, COBieKeyType.None, "", COBieAttributeState.As_Specified, "Features", 255, COBieAllowedType.AlphaNumeric)]
        public string Features { get; set; }

        [COBieAttributes(32, COBieKeyType.None, "", COBieAttributeState.As_Specified, "AccessibilityPerformance", 255, COBieAllowedType.AlphaNumeric)]
        public string AccessibilityPerformance { get; set; }

        [COBieAttributes(33, COBieKeyType.None, "", COBieAttributeState.As_Specified, "CodePerformance", 255, COBieAllowedType.AlphaNumeric)]
        public string CodePerformance { get; set; }

        [COBieAttributes(34, COBieKeyType.None, "", COBieAttributeState.As_Specified, "SustainabilityPerformance", 255, COBieAllowedType.AlphaNumeric)]
        public string SustainabilityPerformance { get; set; }
    }
}
