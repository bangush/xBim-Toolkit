﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc.Extensions
// Filename:    TypeObjectExtensions.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


using Xbim.Ifc.Kernel;
using Xbim.Ifc.MeasureResource;
using Xbim.Ifc.ProductExtension;
using Xbim.Ifc.PropertyResource;
using Xbim.Ifc.QuantityResource;
using Xbim.Ifc.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;




#endregion

namespace Xbim.Ifc.Extensions
{
    public static class TypeObjectExtensions
    {
        /// <summary>
        /// Returns the propertyset of the specified name, null if it does not exist
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pSetName"></param>
        /// <returns></returns>
        public static IfcPropertySet GetPropertySet(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName)
        {
            if (obj.HasPropertySets == null) return null;
            else return obj.HasPropertySets.Where<IfcPropertySet>(r => r.Name == pSetName).FirstOrDefault();
        }

        public static IfcPropertySingleValue GetPropertySingleValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyName)
        {
            IfcPropertySet pset = GetPropertySet(obj, pSetName);
            if (pset != null)
                return pset.HasProperties.Where<IfcPropertySingleValue>(p => p.Name == propertyName).FirstOrDefault();
            return null;
        }

        public static IfcValue GetPropertySingleValueValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyName)
        {
            IfcPropertySingleValue psv = GetPropertySingleValue(obj, pSetName, propertyName);
            return psv.NominalValue;
        }

        public static List<IfcPropertySet> GetAllPropertySets(this Xbim.Ifc.Kernel.IfcTypeObject obj)
        {
            List<IfcPropertySet> result = new List<IfcPropertySet>();
            if (obj.HasPropertySets != null)
            {
                foreach (IfcPropertySetDefinition def in obj.HasPropertySets)
                {
                    if (def is IfcPropertySet) result.Add(def as IfcPropertySet);
                }
            }
            return result;
        }

        public static Dictionary<IfcLabel, Dictionary<IfcIdentifier, IfcValue>> GetAllPropertySingleValues(this Xbim.Ifc.Kernel.IfcTypeObject obj)
        {
            Dictionary<IfcLabel, Dictionary<IfcIdentifier, IfcValue>> result = new Dictionary<IfcLabel, Dictionary<IfcIdentifier, IfcValue>>();
            PropertySetDefinitionSet pSets = obj.HasPropertySets;
            if (pSets == null) return result;
            IEnumerable<IfcPropertySet> pSetsPure = pSets.OfType<IfcPropertySet>();
            foreach (IfcPropertySet pSet in pSetsPure)
            {
                Dictionary<IfcIdentifier, IfcValue> value = new Dictionary<IfcIdentifier, IfcValue>();
                IfcLabel psetName = pSet.Name ?? null;
                foreach (IfcProperty prop in pSet.HasProperties)
                {
                    IfcPropertySingleValue singleVal = prop as IfcPropertySingleValue;
                    if (singleVal == null) continue;
                    value.Add(prop.Name, singleVal.NominalValue);
                }
                result.Add(psetName, value);
            }
            return result;
        }

        public static void DeletePropertySingleValueValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyName)
        {
            IfcPropertySingleValue psv = GetPropertySingleValue(obj, pSetName, propertyName);
            if (psv != null) psv.NominalValue = null;
        }

        public static IfcPropertyTableValue GetPropertyTableValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyTableName)
        {
            IfcPropertySet pset = GetPropertySet(obj, pSetName);
            if (pset != null)
                return pset.HasProperties.Where<IfcPropertyTableValue>(p => p.Name == propertyTableName).FirstOrDefault();
            return null;
        }

        public static IfcValue GetPropertyTableItemValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyTableName, IfcValue definingValue)
        {
            IfcPropertyTableValue table = GetPropertyTableValue(obj, pSetName, propertyTableName);
            if (table == null) return null;
            IList<IfcValue> definingValues = table.DefiningValues;
            if (definingValues == null) return null;
            if (!definingValues.Contains(definingValue)) return null;
            int index = definingValues.IndexOf(definingValue);

            if (table.DefinedValues.Count < index + 1) return null;
            return table.DefinedValues[index];
        }

        public static void SetPropertyTableItemValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyTableName, IfcValue definingValue, IfcValue definedValue)
        {
            SetPropertyTableItemValue(obj, pSetName, propertyTableName, definingValue, definedValue, null, null);
        }

        public static void SetPropertyTableItemValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyTableName, IfcValue definingValue, IfcValue definedValue, IfcUnit definingUnit, IfcUnit definedUnit)
        {
            IfcPropertySet pset = GetPropertySet(obj, pSetName);
            IModel model = null;
            if (pset == null)
            {
                IPersistIfcEntity ent = obj as IPersistIfcEntity;
                model = ent != null ? ent.ModelOf : obj.ModelOf;
                pset = model.New<IfcPropertySet>();
                pset.Name = pSetName;
                obj.AddPropertySet(pset);
            }
            IfcPropertyTableValue table = GetPropertyTableValue(obj, pSetName, propertyTableName);
            if (table == null)
            {
                IPersistIfcEntity ent = obj as IPersistIfcEntity;
                model = ent != null ? ent.ModelOf : obj.ModelOf;
                table = model.New<IfcPropertyTableValue>(tb => { tb.Name = propertyTableName; });
                pset.HasProperties.Add_Reversible(table);
                table.DefinedUnit = definedUnit;
                table.DefiningUnit = definingUnit;
            }
            if (table.DefiningUnit != definingUnit || table.DefinedUnit != definedUnit)
                throw new Exception("Inconsistent definition of the units in the property table.");

            IfcValue itemValue = GetPropertyTableItemValue(obj, pSetName, propertyTableName, definingValue);
            if (itemValue != null)
            {
                itemValue = definedValue;
            }
            else
            {
                table.DefiningValues.Add_Reversible(definingValue);
                table.DefinedValues.Add_Reversible(definedValue);

                //check of integrity
                if (table.DefinedValues.Count != table.DefiningValues.Count)
                    throw new Exception("Inconsistent state of the property table. Number of defined and defining values are not the same.");
            }
        }

        public static IfcPropertySingleValue SetPropertySingleValue(this Xbim.Ifc.Kernel.IfcTypeObject obj, string pSetName, string propertyName, IfcValue value)
        {
            IfcPropertySet pset = GetPropertySet(obj, pSetName);
            IfcPropertySingleValue property = null;
            IModel model = null;
            if (pset == null)
            {
                //if (value == null) return;
                IPersistIfcEntity ent = obj as IPersistIfcEntity;
                model = ent!= null? ent.ModelOf : obj.ModelOf;
                pset = model.New<IfcPropertySet>();
                pset.Name = pSetName;
                obj.AddPropertySet(pset);
            }

            //change existing property of the same name from the property set
            IfcPropertySingleValue singleVal = GetPropertySingleValue(obj, pSetName, propertyName);
            if (singleVal != null)
            {
                property = singleVal;
                singleVal.NominalValue = value;
            }
            else
            {
                //if (value == null) return;
                IPersistIfcEntity ent = obj as IPersistIfcEntity;
                model = ent != null ? ent.ModelOf : obj.ModelOf;
                property = model.New<IfcPropertySingleValue>(psv => { psv.Name = propertyName; psv.NominalValue = value; });
                pset.HasProperties.Add_Reversible(property);
            }
            return property;
        }

        public static IfcPhysicalSimpleQuantity GetElementPhysicalSimpleQuantity(this IfcTypeObject elem, string pSetName, string qualityName)
        {
            IfcElementQuantity elementQuality = GetElementQuantity(elem, pSetName);
            if (elementQuality != null)
            {
                return elementQuality.Quantities.Where<IfcPhysicalSimpleQuantity>(sq => sq.Name == qualityName).FirstOrDefault();
            }
            else
                return null;
        }

        public static IfcElementQuantity GetElementQuantity(this IfcTypeObject elem, string pSetName)
        {
            if (elem.HasPropertySets == null) return null;
            IfcElementQuantity elemQuant = elem.HasPropertySets.Where<IfcElementQuantity>(r => r.Name == pSetName).FirstOrDefault();
            return elemQuant;
        }

        public static void RemoveElementPhysicalSimpleQuantity(this IfcTypeObject elem, string pSetName, string qualityName)
        {
            IfcElementQuantity elementQuantity = GetElementQuantity(elem, pSetName);
            if (elementQuantity != null)
            {
                IfcPhysicalSimpleQuantity simpleQuantity = elementQuantity.Quantities.Where<IfcPhysicalSimpleQuantity>(sq => sq.Name == qualityName).FirstOrDefault();
                if (simpleQuantity != null)
                {
                    elementQuantity.Quantities.Remove_Reversible(simpleQuantity);
                }
            }
        }

        public static void SetElementPhysicalSimpleQuantity(this IfcTypeObject elem, string qSetName, string qualityName, double value, XbimQuantityTypeEnum quantityType, IfcNamedUnit unit)
        {
            IModel model = null;

            if (elem is IPersistIfcEntity) 
                model = (elem as IPersistIfcEntity).ModelOf;
            else  
                model = elem.ModelOf;

            IfcElementQuantity qset = GetElementQuantity(elem, qSetName);
            if (qset == null)
            {
                qset = model.New<IfcElementQuantity>();
                qset.Name = qSetName;
                if (elem.HasPropertySets == null) elem.CreateHasPropertySets();
                elem.HasPropertySets.Add_Reversible(qset);
            }

            //remove existing simple quality
            IfcPhysicalSimpleQuantity simpleQuality = GetElementPhysicalSimpleQuantity(elem, qSetName, qualityName);
            if (simpleQuality != null)
            {
                IfcElementQuantity elementQuality = GetElementQuantity(elem, qSetName);
                elementQuality.Quantities.Remove_Reversible(simpleQuality);
                model.Delete(simpleQuality);
            }

            switch (quantityType)
            {
                case XbimQuantityTypeEnum.AREA:
                    simpleQuality = model.New<IfcQuantityArea>(sq => sq.AreaValue = value);
                    break;
                case XbimQuantityTypeEnum.COUNT:
                    simpleQuality = model.New<IfcQuantityCount>(sq => sq.CountValue = value);
                    break;
                case XbimQuantityTypeEnum.LENGTH:
                    simpleQuality = model.New<IfcQuantityLength>(sq => sq.LengthValue = value);
                    break;
                case XbimQuantityTypeEnum.TIME:
                    simpleQuality = model.New<IfcQuantityTime>(sq => sq.TimeValue = value);
                    break;
                case XbimQuantityTypeEnum.VOLUME:
                    simpleQuality = model.New<IfcQuantityVolume>(sq => sq.VolumeValue = value);
                    break;
                case XbimQuantityTypeEnum.WEIGHT:
                    simpleQuality = model.New<IfcQuantityWeight>(sq => sq.WeightValue = value);
                    break;
                default:
                    return;
            }

            simpleQuality.Unit = unit;
            simpleQuality.Name = qualityName;

            qset.Quantities.Add_Reversible(simpleQuality);
        }
    }
}
