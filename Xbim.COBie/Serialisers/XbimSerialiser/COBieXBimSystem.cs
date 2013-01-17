﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc.Kernel;
using Xbim.Ifc.ProductExtension;
using Xbim.Ifc.UtilityResource;
using Xbim.Ifc.Extensions;
using Xbim.XbimExtensions;
using System.Reflection;
using Xbim.Ifc.PropertyResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimSystem : COBieXBim
    {
        #region Properties
        public IfcSystem IfcSystemObj { get; set; }
        int SystemProdutIndex { get; set; }
        #endregion
        

        public COBieXBimSystem(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            IfcSystemObj = null;
            
        }

        #region Methods
        // <summary>
        /// Create and setup objects held in the System COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieSystemRow to read data from</param>
        public void SerialiseSystem(COBieSheet<COBieSystemRow> cOBieSheet)
        {
            using (Transaction trans = Model.BeginTransaction("Add System"))
            {

                try
                {

                    ProgressIndicator.ReportMessage("Starting Systems...");
                    ProgressIndicator.Initialise("Creating Systems", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        ProgressIndicator.IncrementAndUpdate();
                        COBieSystemRow row = cOBieSheet[i];
                        if (ValidateString(row.Name))
                        {
                            if ((IfcSystemObj == null) ||
                                (row.Name.ToLower() != IfcSystemObj.Name.ToString().ToLower())
                                )
                            {
                                AddSystem(row);
                                AddProducts(row);
                                SystemProdutIndex = 1;
                            }
                            else
                            {
                                AddProducts(row);
                            }
                        }
                    }

                    ProgressIndicator.Finalise();                    
                    trans.Commit();

                }
                catch (Exception)
                {
                    trans.Rollback();
                    //TODO: Catch with logger?
                    throw;
                }
            }
        }

        /// <summary>
        /// Add system group and fill with data from COBieSystemRow
        /// </summary>
        /// <param name="row">COBieSystemRow holding the data</param>
        private void AddSystem(COBieSystemRow row)
        {
            //we are merging so check for an existing item name, assume the same item as should be the same building
            if (CheckIfExistOnMerge<IfcSystem>(row.Name))
            {
                string testName = row.Name.ToLower().Trim();
                IfcSystemObj = Model.InstancesWhere<IfcSystem>(bs => bs.Name.ToString().ToLower().Trim() == testName).FirstOrDefault();
                return;//we have it so no need to create
            }

            IfcSystemObj = GetGroupInstance(row.ExtObject);//Model.New<IfcSystem>();
            IfcSystemObj.Name = row.Name;
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(IfcSystemObj, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to IfcSystemObj.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, IfcSystemObj.OwnerHistory))
            {
                //Add Category
                AddCategory(row.Category, IfcSystemObj);
                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, IfcSystemObj);
                //Add Description
                if (ValidateString(row.Description)) IfcSystemObj.Description = row.Description;
            }
            
        }

        /// <summary>
        /// Create an instance of an group object via a string name
        /// </summary>
        /// <param name="groupTypeName">String holding object type name we want to create</param>
        /// <param name="model">Model object</param>
        /// <returns></returns>
        public IfcSystem GetGroupInstance(string groupTypeName)
        {
            groupTypeName = groupTypeName.Trim().ToUpper();
            IfcType ifcType;
            IfcSystem ifcSystem = null;
            if ((IfcInstances.IfcTypeLookup.TryGetValue(groupTypeName, out ifcType)) &&
                (typeof(IfcSystem).IsAssignableFrom(ifcType.Type)) //check it is a system class name
                )
            {
                MethodInfo method = typeof(IModel).GetMethod("New", Type.EmptyTypes);
                MethodInfo generic = method.MakeGenericMethod(ifcType.Type);
                var eleObj = generic.Invoke(Model, null);
                if (eleObj is IfcSystem)
                    ifcSystem = (IfcSystem)eleObj;
            }


            if (ifcSystem == null)
                ifcSystem = Model.New<IfcSystem>();
            return ifcSystem;
        }

        /// <summary>
        /// Add products to system group and fill with data from COBieSystemRow
        /// </summary>
        /// <param name="componentName">COBieSystemRow holding the data</param>
        private void AddProducts(COBieSystemRow row)
        {
            string componentNames = row.ComponentNames;
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, IfcSystemObj.OwnerHistory))
            {
                
                List<IfcProduct> ifcProductList = new List<IfcProduct>();

                foreach (string componentName in SplitTheString(componentNames))
                {
                    IfcProduct ifcProduct = null;
                    if (ValidateString(componentName))
                    {
                        string compName = componentName.ToLower().Trim();
                        ifcProduct = Model.InstancesOfType<IfcProduct>().Where(p => p.Name.ToString().ToLower().Trim() == compName).FirstOrDefault();
                        if (ifcProduct != null)
                            ifcProductList.Add(ifcProduct);
                    }
                    if (ifcProduct == null)
                    {
                        string elementTypeName = GetPrefixType(componentName);
                        if (string.IsNullOrEmpty(elementTypeName))
                        {
                            elementTypeName = "IfcDistributionElement";
                        }
                        ifcProduct = COBieXBimComponent.GetElementInstance(elementTypeName, Model);
                        if (ifcProduct != null)
                        {
                            if (string.IsNullOrEmpty(componentName) || (componentName == Constants.DEFAULT_STRING))
                            {
                                ifcProduct.Name = ""; //row.Name + " " + SystemProdutIndex.ToString();
                                SystemProdutIndex++;
                            }
                            else
                                ifcProduct.Name = componentName;
                            ifcProduct.Description = "Created to maintain relationship with System object from COBie information";
                            ifcProductList.Add(ifcProduct); 
                        }

                    }
                }
                if (ifcProductList.Count == 0) //no products created so create an IfcDistributionElement as place holder
                {
                    IfcProduct ifcProduct = COBieXBimComponent.GetElementInstance("IfcDistributionElement", Model);
                    if (ifcProduct != null)
                    {
                        ifcProduct.Name = ""; // row.Name + " " + SystemProdutIndex.ToString(); ;
                        ifcProduct.Description = "Created to maintain relationship with System object from COBie information";
                        ifcProductList.Add(ifcProduct);
                    }
                }
               
                //if we have found product then add to the IfcSystem group
                foreach (IfcProduct ifcProd in ifcProductList)
                {
                    if (IfcSystemObj.IsGroupedBy != null) //if we already have a IfcRelAssignsToGroup assigned to IsGroupedBy
                    {
                        if (!IfcSystemObj.IsGroupedBy.RelatedObjects.Contains(ifcProd)) //check to see if product already exists in group
                            IfcSystemObj.AddObjectToGroup(ifcProd);//if not add
                    }
                    else
                        IfcSystemObj.AddObjectToGroup(ifcProd);
                }
            }
        }

        public string GetPrefixType(string value)
        {
            value = value.ToUpper();
            if (value.Contains("IFC"))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == ' ')
                    {
                        return value.Substring(0, i);
                    }
                }
            }
            return null; //default type
            
        }

        #endregion


        
    }
}
