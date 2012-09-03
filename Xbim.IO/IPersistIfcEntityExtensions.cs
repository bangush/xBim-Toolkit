﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.XbimExtensions.Interfaces;
using System.IO;
using Xbim.IO.Parser;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.XbimExtensions;
using Xbim.Common.Exceptions;
using System.Reflection;

namespace Xbim.IO
{
    public static class IPersistIfcEntityExtensions
    {
       

        #region Write the properties of an IPersistIfcEntity to a stream

        /// <summary>
        /// Returns the index value of this type for use in Xbim datanase storage
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ushort TypeId(this  IPersistIfcEntity entity)
        {
            return IfcInstances.IfcEntities[entity.GetType()].TypeId;
        }

        /// <summary>
        /// Returns the Xbim meta data about the Ifc Properties of the Type
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static IfcType IfcType(this  IPersistIfcEntity entity)
        {
            return IfcInstances.IfcEntities[entity.GetType()];
        }

        public static object SecondaryKeyValue(this  IPersistIfcEntity entity)
        {

            PropertyInfo pInfo = entity.IfcType().PrimaryIndex;
            if (pInfo != null)
                return pInfo.GetValue(entity, null);
            else
                return null;
        }

        internal static void WriteEntity(this IPersistIfcEntity entity, TextWriter tw, byte[] propertyData)
        {
            tw.Write(string.Format("#{0}={1}", Math.Abs(entity.EntityLabel), entity.GetType().Name.ToUpper()));
            BinaryReader br = new BinaryReader(new MemoryStream(propertyData));
            P21ParseAction action = (P21ParseAction)br.ReadByte();
            bool comma = false; //the first property
            XbimParserState parserState = new XbimParserState(entity);
            while (action != P21ParseAction.EndEntity)
            {
                switch (action)
                {
                    case P21ParseAction.BeginList:
                        tw.Write("(");
                        break;
                    case P21ParseAction.EndList:
                        tw.Write(")");
                        break;
                    case P21ParseAction.BeginComplex:
                        tw.Write("&SCOPE");
                        break;
                    case P21ParseAction.EndComplex:
                        tw.Write("ENDSCOPE");
                        break;
                    case P21ParseAction.SetIntegerValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write(br.ReadInt64().ToString());
                        break;
                    case P21ParseAction.SetHexValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write(Convert.ToString(br.ReadInt64(),16));
                        break;
                    case P21ParseAction.SetFloatValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write(IfcReal.AsPart21(br.ReadDouble()));
                        break;
                    case P21ParseAction.SetStringValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write(br.ReadString());
                        break;
                    case P21ParseAction.SetEnumValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("." + br.ReadString() + ".");
                        break;
                    case P21ParseAction.SetBooleanValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write(br.ReadBoolean() ? ".T." : ".F.");
                        break;
                    case P21ParseAction.SetNonDefinedValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("$");
                        break;
                    case P21ParseAction.SetOverrideValue:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("*");
                        break;
                    case P21ParseAction.SetObjectValueUInt16:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("#"+ br.ReadUInt16().ToString());
                        break;
                    case P21ParseAction.SetObjectValueUInt32:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("#" + br.ReadUInt32().ToString());
                        break;
                    case P21ParseAction.SetObjectValueInt64:
                        if (comma) tw.Write(",");
                        comma = true;
                        tw.Write("#" + br.ReadUInt64().ToString());
                        break;
                    case P21ParseAction.BeginNestedType:
                        if (comma) tw.Write(",");
                        comma = false;
                        tw.Write(br.ReadString()+"(");
                        break;
                    case P21ParseAction.EndNestedType:
                        comma = true;
                        tw.Write(")");
                        break;
                    case P21ParseAction.EndEntity:
                        tw.Write(");");
                        break;
                    case P21ParseAction.NewEntity:
                        comma = false;
                        tw.Write("(");
                        break;
                    default:
                        throw new Exception("Invalid Property Record #" + entity.EntityLabel + " EntityType: " + entity.GetType().Name);
                }
                action = (P21ParseAction)br.ReadByte();
            }
            tw.WriteLine();
        }
        /// <summary>
        /// Writes the entity to a TextWriter in the Part21 format
        /// </summary>
        /// <param name="entityWriter">The TextWriter</param>
        /// <param name="entity">The entity to write</param>
        internal static void WriteEntity( this IPersistIfcEntity entity, TextWriter entityWriter)
        {

            entityWriter.Write(string.Format("#{0}={1}(", Math.Abs(entity.EntityLabel), entity.GetType().Name.ToUpper()));
            IfcType ifcType = IfcInstances.IfcEntities[entity.GetType()];
            bool first = true;
            
            foreach (IfcMetaProperty ifcProperty in ifcType.IfcProperties.Values)
            //only write out persistent attributes, ignore inverses
            {
                if (ifcProperty.IfcAttribute.State == IfcAttributeState.DerivedOverride)
                {
                    if (!first)
                        entityWriter.Write(',');
                    entityWriter.Write('*');
                    first = false;
                }
                else
                {
                    Type propType = ifcProperty.PropertyInfo.PropertyType;
                    object propVal = ifcProperty.PropertyInfo.GetValue(entity, null);
                    if (!first)
                        entityWriter.Write(',');
                    WriteProperty(propType, propVal, entityWriter);
                    first = false;
                }
            }
            entityWriter.WriteLine(");");

        }

        /// <summary>
        /// Writes a property of an entity to the TextWriter in the Part21 format
        /// </summary>
        /// <param name="propType"></param>
        /// <param name="propVal"></param>
        /// <param name="entityWriter"></param>
        private static void WriteProperty(Type propType, object propVal, TextWriter entityWriter)
        {
            Type itemType;
            if (propVal == null) //null or a value type that maybe null
                entityWriter.Write('$');

            else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //deal with undefined types (nullables)
            {
                if (typeof(ExpressComplexType).IsAssignableFrom(propVal.GetType()))
                {
                    entityWriter.Write('(');
                    bool first = true;
                    foreach (var compVal in ((ExpressComplexType)propVal).Properties)
                    {
                        if (!first)
                            entityWriter.Write(',');
                        WriteProperty(compVal.GetType(), compVal, entityWriter);
                        first = false;
                    }
                    entityWriter.Write(')');
                }
                else if ((typeof(ExpressType).IsAssignableFrom(propVal.GetType())))
                {
                    ExpressType expressVal = (ExpressType)propVal;
                    WriteValueType(expressVal.UnderlyingSystemType, expressVal.Value, entityWriter);
                }
                else // if (propVal.GetType().IsEnum)
                {
                    WriteValueType(propVal.GetType(), propVal, entityWriter);
                }

            }
            else if (typeof(ExpressComplexType).IsAssignableFrom(propType))
            {
                entityWriter.Write('(');
                bool first = true;
                foreach (var compVal in ((ExpressComplexType)propVal).Properties)
                {
                    if (!first)
                        entityWriter.Write(',');
                    WriteProperty(compVal.GetType(), compVal, entityWriter);
                    first = false;
                }
                entityWriter.Write(')');
            }
            else if (typeof(ExpressType).IsAssignableFrom(propType))
            //value types with a single property (IfcLabel, IfcInteger)
            {
                Type realType = propVal.GetType();
                if (realType != propType)
                //we have a type but it is a select type use the actual value but write out explricitly
                {
                    entityWriter.Write(realType.Name.ToUpper());
                    entityWriter.Write('(');
                    WriteProperty(realType, propVal, entityWriter);
                    entityWriter.Write(')');
                }
                else //need to write out underlying property value
                {
                    ExpressType expressVal = (ExpressType)propVal;
                    WriteValueType(expressVal.UnderlyingSystemType, expressVal.Value, entityWriter);
                }
            }
            else if (typeof(ExpressEnumerable).IsAssignableFrom(propType) &&
                     (itemType = GetItemTypeFromGenericType(propType)) != null)
            //only process lists that are real lists, see cartesianpoint
            {
                entityWriter.Write('(');
                bool first = true;
                foreach (var item in ((ExpressEnumerable)propVal))
                {
                    if (!first)
                        entityWriter.Write(',');
                    WriteProperty(itemType, item, entityWriter);
                    first = false;
                }
                entityWriter.Write(')');
            }
            else if (typeof(IPersistIfcEntity).IsAssignableFrom(propType))
            //all writable entities must support this interface and ExpressType have been handled so only entities left
            {
                entityWriter.Write('#');
                entityWriter.Write(Math.Abs(((IPersistIfcEntity)propVal).EntityLabel));
            }
            else if (propType.IsValueType) //it might be an in-built value type double, string etc
            {
                WriteValueType(propVal.GetType(), propVal, entityWriter);
            }
            else if (typeof(ExpressSelectType).IsAssignableFrom(propType))
            // a select type get the type of the actual value
            {
                if (propVal.GetType().IsValueType) //we have a value type, so write out explicitly
                {
                    entityWriter.Write(propVal.GetType().Name.ToUpper());
                    entityWriter.Write('(');
                    WriteProperty(propVal.GetType(), propVal, entityWriter);
                    entityWriter.Write(')');
                }
                else //could be anything so re-evaluate actual type
                {
                    WriteProperty(propVal.GetType(), propVal, entityWriter);
                }
            }
            else
                throw new Exception(string.Format("Entity  has illegal property {0} of type {1}",
                                                  propType.Name, propType.Name));
        }

        /// <summary>
        /// Writes the value of a property to the TextWriter in the Part 21 format
        /// </summary>
        /// <param name="pInfoType"></param>
        /// <param name="pVal"></param>
        /// <param name="entityWriter"></param>
        private static void WriteValueType(Type pInfoType, object pVal, TextWriter entityWriter)
        {
            if (pInfoType == typeof(Double))
                entityWriter.Write(string.Format(new Part21Formatter(), "{0:R}", pVal));
            else if (pInfoType == typeof(String)) //convert  string
            {
                if (pVal == null)
                    entityWriter.Write('$');
                else
                {
                    entityWriter.Write('\'');
                    entityWriter.Write(IfcText.Escape((string)pVal));
                    entityWriter.Write('\'');
                }
            }
            else if (pInfoType == typeof(Int16) || pInfoType == typeof(Int32) || pInfoType == typeof(Int64))
                entityWriter.Write(pVal.ToString());
            else if (pInfoType.IsEnum) //convert enum
                entityWriter.Write(string.Format(".{0}.", pVal.ToString().ToUpper()));
            else if (pInfoType == typeof(Boolean))
            {
                bool b = (bool)pVal;
                entityWriter.Write(string.Format(".{0}.", b ? "T" : "F"));
            }
            else if (pInfoType == typeof(DateTime)) //convert  TimeStamp
                entityWriter.Write(string.Format(new Part21Formatter(), "{0:T}", pVal));
            else if (pInfoType == typeof(Guid)) //convert  Guid string
            {
                if (pVal == null)
                    entityWriter.Write('$');
                else
                    entityWriter.Write(string.Format(new Part21Formatter(), "{0:G}", pVal));
            }
            else if (pInfoType == typeof(bool?)) //convert  logical
            {
                bool? b = (bool?)pVal;
                entityWriter.Write(!b.HasValue ? "$" : string.Format(".{0}.", b.Value ? "T" : "F"));
            }
            else
                throw new ArgumentException(string.Format("Invalid Value Type {0}", pInfoType.Name), "pInfoType");
        }

        internal static void WriteEntity(this IPersistIfcEntity entity, BinaryWriter entityWriter)
        {
           
            IfcType ifcType = IfcInstances.IfcEntities[entity.GetType()];
            entityWriter.Write(Convert.ToByte(P21ParseAction.NewEntity));
            entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
            foreach (IfcMetaProperty ifcProperty in ifcType.IfcProperties.Values)
            //only write out persistent attributes, ignore inverses
            {
                if (ifcProperty.IfcAttribute.State == IfcAttributeState.DerivedOverride)
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetOverrideValue));
                else
                {
                    Type propType = ifcProperty.PropertyInfo.PropertyType;
                    object propVal = ifcProperty.PropertyInfo.GetValue(entity, null);
                    WriteProperty(propType, propVal, entityWriter);
                }
            }
            entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
            entityWriter.Write(Convert.ToByte(P21ParseAction.EndEntity));
        }

        private static  void WriteProperty(Type propType, object propVal, BinaryWriter entityWriter)
        {
            Type itemType;
            if (propVal == null) //null or a value type that maybe null
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetNonDefinedValue));
            else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //deal with undefined types (nullables)
            {
                if (typeof(ExpressComplexType).IsAssignableFrom(propVal.GetType()))
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
                    foreach (var compVal in ((ExpressComplexType)propVal).Properties)
                        WriteProperty(compVal.GetType(), compVal, entityWriter);
                    entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
                }
                else if ((typeof(ExpressType).IsAssignableFrom(propVal.GetType())))
                {
                    ExpressType expressVal = (ExpressType)propVal;
                    WriteValueType(expressVal.UnderlyingSystemType, expressVal.Value, entityWriter);
                }
                else
                {
                    WriteValueType(propVal.GetType(), propVal, entityWriter);
                }
            }
            else if (typeof(ExpressComplexType).IsAssignableFrom(propType))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
                foreach (var compVal in ((ExpressComplexType)propVal).Properties)
                    WriteProperty(compVal.GetType(), compVal, entityWriter);
                entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
            }
            else if (typeof(ExpressType).IsAssignableFrom(propType))
            //value types with a single property (IfcLabel, IfcInteger)
            {
                Type realType = propVal.GetType();
                if (realType != propType)
                //we have a type but it is a select type use the actual value but write out explicitly
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.BeginNestedType));
                    entityWriter.Write(realType.Name.ToUpper());
                    entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
                    WriteProperty(realType, propVal, entityWriter);
                    entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
                    entityWriter.Write(Convert.ToByte(P21ParseAction.EndNestedType));
                }
                else //need to write out underlying property value
                {
                    ExpressType expressVal = (ExpressType)propVal;
                    WriteValueType(expressVal.UnderlyingSystemType, expressVal.Value, entityWriter);
                }
            }
            else if (typeof(ExpressEnumerable).IsAssignableFrom(propType) &&
                     (itemType = GetItemTypeFromGenericType(propType)) != null)
            //only process lists that are real lists, see cartesianpoint
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
                foreach (var item in ((ExpressEnumerable)propVal))
                    WriteProperty(itemType, item, entityWriter);
                entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
            }
            else if (typeof(IPersistIfcEntity).IsAssignableFrom(propType))
            //all writable entities must support this interface and ExpressType have been handled so only entities left
            {
                long val = Math.Abs(((IPersistIfcEntity)propVal).EntityLabel);
                if (val <= UInt16.MaxValue)
                {
                    entityWriter.Write((byte)P21ParseAction.SetObjectValueUInt16);
                    entityWriter.Write(Convert.ToUInt16(val));
                }
                else if (val <= UInt32.MaxValue)
                {
                    entityWriter.Write((byte)P21ParseAction.SetObjectValueUInt32);
                    entityWriter.Write(Convert.ToUInt32(val));
                }
                else if (val <= Int64.MaxValue)
                {
                    entityWriter.Write((byte)P21ParseAction.SetObjectValueInt64);
                    entityWriter.Write(val);
                }
                else
                    throw new Exception("Entity Label exceeds maximim value for a long number");
            }
            else if (propType.IsValueType) //it might be an in-built value type double, string etc
            {
                WriteValueType(propVal.GetType(), propVal, entityWriter);
            }
            else if (typeof(ExpressSelectType).IsAssignableFrom(propType))
            // a select type get the type of the actual value
            {
                if (propVal.GetType().IsValueType) //we have a value type, so write out explicitly
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.BeginNestedType));
                    entityWriter.Write(propVal.GetType().Name.ToUpper());
                    entityWriter.Write(Convert.ToByte(P21ParseAction.BeginList));
                    WriteProperty(propVal.GetType(), propVal, entityWriter);
                    entityWriter.Write(Convert.ToByte(P21ParseAction.EndList));
                    entityWriter.Write(Convert.ToByte(P21ParseAction.EndNestedType));
                }
                else //could be anything so re-evaluate actual type
                {
                    WriteProperty(propVal.GetType(), propVal, entityWriter);
                }
            }
            else
                throw new Exception(string.Format("Entity  has illegal property {0} of type {1}",
                                                  propType.Name, propType.Name));
        }

        private static  void WriteValueType(Type pInfoType, object pVal, BinaryWriter entityWriter)
        {
            if (pInfoType == typeof(Double))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetFloatValue));
                entityWriter.Write((double)pVal);
            }
            else if (pInfoType == typeof(String)) //convert  string
            {
                if (pVal == null)
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetNonDefinedValue));
                else
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetStringValue));
                    entityWriter.Write((string)pVal);
                }
            }
            else if (pInfoType == typeof(Int16))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetIntegerValue));
                entityWriter.Write((long)(Int16)pVal);
            }
            else if (pInfoType == typeof(Int32))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetIntegerValue));
                entityWriter.Write((long)(Int32)pVal);
            }
            else if (pInfoType == typeof(Int64))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetIntegerValue));
                entityWriter.Write((long)pVal);
            }
            else if (pInfoType.IsEnum) //convert enum
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetEnumValue));
                entityWriter.Write(pVal.ToString().ToUpper());
            }
            else if (pInfoType == typeof(Boolean))
            {
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetBooleanValue));
                entityWriter.Write((bool)pVal);
            }

            else if (pInfoType == typeof(DateTime)) //convert  TimeStamp
            {
                IfcTimeStamp ts = IfcTimeStamp.ToTimeStamp((DateTime)pVal);
                entityWriter.Write(Convert.ToByte(P21ParseAction.SetIntegerValue));
                entityWriter.Write((long)ts);
            }
            else if (pInfoType == typeof(Guid)) //convert  Guid string
            {
                if (pVal == null)
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetNonDefinedValue));
                else
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetStringValue));
                    entityWriter.Write((string)pVal);
                }
            }
            else if (pInfoType == typeof(bool?)) //convert  logical
            {
                bool? b = (bool?)pVal;
                if (!b.HasValue)
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetNonDefinedValue));
                else
                {
                    entityWriter.Write(Convert.ToByte(P21ParseAction.SetBooleanValue));
                    entityWriter.Write(b.Value);
                }
            }
            else
                throw new ArgumentException(string.Format("Invalid Value Type {0}", pInfoType.Name), "pInfoType");
        }

        /// <summary>
        ///   Writes the in memory data of the entity to a stream
        /// </summary>
        /// <param name = "entityStream"></param>
        /// <param name = "entityWriter"></param>
        /// <param name = "item"></param>
        private static int WriteEntityToSteam(MemoryStream entityStream, BinaryWriter entityWriter, IPersistIfcEntity item)
        {
            entityWriter.Seek(0, SeekOrigin.Begin);
            entityWriter.Write((int)0);
            item.WriteEntity(entityWriter);
            int len = Convert.ToInt32(entityStream.Position);
            entityWriter.Seek(0, SeekOrigin.Begin);
            entityWriter.Write(len);
            entityWriter.Seek(0, SeekOrigin.Begin);
            return len;
        }
        #endregion

        #region Functions to read property data
        /// <summary>
        /// Populates an entites properties from the binary stream
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cache"></param>
        /// <param name="br"></param>
        /// <param name="unCached">If true instances inside the propert are not added to the cache</param>
        public static void ReadEntityProperties(this IPersistIfcEntity entity, IfcPersistedInstanceCache cache, BinaryReader br, bool unCached = false)
        {
            P21ParseAction action = (P21ParseAction)br.ReadByte();

            XbimParserState parserState = new XbimParserState(entity);
            while (action != P21ParseAction.EndEntity)
            {
                switch (action)
                {
                    case P21ParseAction.BeginList:
                        parserState.BeginList();
                        break;
                    case P21ParseAction.EndList:
                        parserState.EndList();
                        break;
                    case P21ParseAction.BeginComplex:
                        break;
                    case P21ParseAction.EndComplex:
                        break;
                    case P21ParseAction.SetIntegerValue:
                        parserState.SetIntegerValue(br.ReadInt64());
                        break;
                    case P21ParseAction.SetHexValue:
                        parserState.SetHexValue(br.ReadInt64());
                        break;
                    case P21ParseAction.SetFloatValue:
                        parserState.SetFloatValue(br.ReadDouble());
                        break;
                    case P21ParseAction.SetStringValue:
                        parserState.SetStringValue(br.ReadString());
                        break;
                    case P21ParseAction.SetEnumValue:
                        parserState.SetEnumValue(br.ReadString());
                        break;
                    case P21ParseAction.SetBooleanValue:
                        parserState.SetBooleanValue(br.ReadBoolean());
                        break;
                    case P21ParseAction.SetNonDefinedValue:
                        parserState.SetNonDefinedValue();
                        break;
                    case P21ParseAction.SetOverrideValue:
                        parserState.SetOverrideValue();
                        break;
                    case P21ParseAction.SetObjectValueUInt16:
                        parserState.SetObjectValue(cache.GetInstance(br.ReadUInt16(), false, unCached));
                        break;
                    case P21ParseAction.SetObjectValueUInt32:
                        parserState.SetObjectValue(cache.GetInstance(br.ReadUInt32(), false, unCached));
                        break;
                    case P21ParseAction.SetObjectValueInt64:
                        parserState.SetObjectValue(cache.GetInstance(br.ReadInt64(), false, unCached));
                        break;
                    case P21ParseAction.BeginNestedType:
                        parserState.BeginNestedType(br.ReadString());
                        break;
                    case P21ParseAction.EndNestedType:
                        parserState.EndNestedType();
                        break;
                    case P21ParseAction.EndEntity:
                        parserState.EndEntity();
                        break;
                    case P21ParseAction.NewEntity:
                        parserState = new XbimParserState(entity);
                        break;
                    default:
                        throw new XbimException("Invalid Property Record #" + entity.EntityLabel + " EntityType: " + entity.GetType().Name);
                }
                action = (P21ParseAction)br.ReadByte();
            }
        }

        #endregion

        #region Helper Functions

        internal static Type GetItemTypeFromGenericType(Type genericType)
        {
            if (genericType == typeof(ICoordinateList))
                return typeof(IfcLengthMeasure); //special case for coordinates
            if (genericType.IsGenericType || genericType.IsInterface)
            {
                Type[] genericTypes = genericType.GetGenericArguments();
                if (genericTypes.GetUpperBound(0) >= 0)
                    return genericTypes[genericTypes.GetUpperBound(0)];
                return null;
            }
            if (genericType.BaseType != null)
                return GetItemTypeFromGenericType(genericType.BaseType);
            return null;
        }

        #endregion
    }
}
