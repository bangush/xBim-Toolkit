﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Transactions;
using Xbim.Ifc.Kernel;
using Xbim.Ifc.ApprovalResource;
using Xbim.Ifc.ControlExtension;
using Xbim.Ifc.SelectTypes;
using Xbim.Ifc.MeasureResource;
using Xbim.Ifc.ProcessExtensions;
using Xbim.Ifc.ActorResource;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimIssue : COBieXBim
    {
        #region Fields
        IfcValue[] _riskTypeEnum;
        IfcValue[] _assessmentOfRiskEnum;
        IfcValue[] _riskConsequenceEnum;
        IfcValue[] _riskRatingEnum;
        IfcValue[] _riskOwnerEnum;
        #endregion

        #region Properties
        public IEnumerable<IfcTask> IfcTasks { get; private set; }
        public IEnumerable<IfcPersonAndOrganization> IfcPersonAndOrganizations { get; private set; }
        #endregion
       

        public COBieXBimIssue(COBieXBimContext xBimContext)
            : base(xBimContext)
        {
            //see http://www.buildingsmart-tech.org/ifc/ifc2x3/tc1/html/psd/IfcSharedFacilitiesElements/Pset_Risk.xml
            _riskTypeEnum = GetValueArray("Business : Hazard : HealthAndSafety : Insurance : Other : NotKnown : Unset : Change : Claim : Coordination : Environmental : Function : IndoorAirQuality : Installation : RFI : Safety : Specification");
            _assessmentOfRiskEnum = GetValueArray("AlmostCertain : VeryLikely : Likely : VeryPossible : Possible : SomewhatPossible : Unlikely :VeryUnlikely : Rare : Other : UnKnown : Unset : HasOccurred : High : Moderate : Low");
            _riskConsequenceEnum = GetValueArray("Catastrophic : Severe : Major : Considerable : Moderate : Some : Minor : VeryLow : Insignificant : Other : UnKnown : Unset : VaryHigh : High : Low");
            _riskRatingEnum = GetValueArray("Critical : VeryHigh : High : Considerable : Moderate : Some : Low : VeryLow : Insignificant : Other : UnKnown : Unset ");
            _riskOwnerEnum = GetValueArray("Designer : Specifier : Constructor : Installer : Maintainer : Other : UnKnown : Unset ");
        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Issue COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieIssueRow to read data from</param>
        public void SerialiseIssue(COBieSheet<COBieIssueRow> cOBieSheet)
        {
            using (Transaction trans = Model.BeginTransaction("Add Issue"))
            {
                try
                {
                    ProgressIndicator.ReportMessage("Starting Issues...");
                    ProgressIndicator.Initialise("Creating Issues", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        ProgressIndicator.IncrementAndUpdate();
                        COBieIssueRow row = cOBieSheet[i];
                        AddIssue(row);
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
        /// Add an IfcApproval to the model based on COBieIssueRow data
        /// </summary>
        /// <param name="row">COBieIssueRow data</param>
        private void AddIssue(COBieIssueRow row)
        {

            if (CheckIfExistOnMerge(row)) //check on merge to see if IfcApproval exists
                return; //already exists

            //create the property set to attach to the approval
            IfcPropertySet ifcPropertySet = Model.New<IfcPropertySet>();
            ifcPropertySet.Name = "Pset_Risk";
            ifcPropertySet.Description = "An indication of exposure to mischance, peril, menace, hazard or loss";
           

            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcPropertySet, row.ExtSystem, row.CreatedBy, row.CreatedOn);
            
            //using statement will set the Model.OwnerHistoryAddObject to ifcPropertySet.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcPropertySet.OwnerHistory))
            {
                //create the approval object
                IfcApproval ifcApproval = Model.New<IfcApproval>();
                //set relationship
                IfcRelAssociatesApproval ifcRelAssociatesApproval = Model.New<IfcRelAssociatesApproval>();
                ifcRelAssociatesApproval.RelatingApproval = ifcApproval;
                ifcRelAssociatesApproval.RelatedObjects.Add_Reversible(ifcPropertySet);

                if (ValidateString(row.Name))
                    ifcApproval.Name = row.Name;

                if (ValidateString(row.Type))
                {
                    IfcValue[] ifcValues = GetValueArray(row.Type);
                    AddPropertyEnumeratedValue(ifcPropertySet, "RiskType", "Identifies the predefined types of risk from which the type required may be set.", ifcValues, _riskTypeEnum, null);
                }

                if (ValidateString(row.Risk))
                {
                    IfcValue[] ifcValues = GetValueArray(row.Risk);
                    AddPropertyEnumeratedValue(ifcPropertySet, "RiskRating", "Rating of the risk that may be determined from a combination of the risk assessment and risk consequence.", ifcValues, _riskRatingEnum, null);
                }

                if (ValidateString(row.Chance))
                {
                    IfcValue[] ifcValues = GetValueArray(row.Chance);
                    AddPropertyEnumeratedValue(ifcPropertySet, "AssessmentOfRisk", "Likelihood of risk event occurring.", ifcValues, _assessmentOfRiskEnum, null);
                }

                if (ValidateString(row.Impact))
                {
                    IfcValue[] ifcValues = GetValueArray(row.Impact);
                    AddPropertyEnumeratedValue(ifcPropertySet, "RiskConsequence", "Indicates the level of severity of the consequences that the risk would have in case it happens.", ifcValues, _riskConsequenceEnum, null);
                }

                if (ValidateString(row.SheetName1) && ValidateString(row.RowName1))
                {
                    SetRelObjectToApproval(row.SheetName1, row.RowName1,  ifcApproval,  ifcRelAssociatesApproval);
                }

                if (ValidateString(row.SheetName2) && ValidateString(row.RowName2))
                {
                    SetRelObjectToApproval(row.SheetName2, row.RowName2, ifcApproval, ifcRelAssociatesApproval);
                }

                if (ValidateString(row.Description))
                    ifcApproval.Description = row.Description;

                if (ValidateString(row.Owner))
                {
                    IfcValue[] ifcValues = GetValueArray(row.Owner);
                    AddPropertyEnumeratedValue(ifcPropertySet, "RiskOwner", "A determination of who is the owner of the risk by reference to principal roles of organizations within a project.", ifcValues, _riskOwnerEnum, null);
                }

                if (ValidateString(row.Mitigation))
                    AddPropertySingleValue(ifcPropertySet, "PreventiveMeassures", "Identifies preventive measures to be taken to mitigate risk.", new IfcText(row.Mitigation), null);

                //Add Identifier
                if (ValidateString(row.ExtIdentifier))
                    ifcApproval.Identifier = row.ExtIdentifier; // AddGlobalId(row.ExtIdentifier, ifcPropertySet); //IfcApproval gas no GlobalId
            }
        }

        /// <summary>
        /// Check to see if IfcApproval exists in model
        /// </summary>
        /// <param name="row">COBieIssueRow data</param>
        /// <returns>bool</returns>
        private bool CheckIfExistOnMerge(COBieIssueRow row)
        {
            if (XBimContext.IsMerge)
            {
                if (ValidateString(row.Name)) //we have a primary key to check
                {
                    string testName = row.Name.ToLower().Trim();
                    IfcApproval testObj = Model.InstancesWhere<IfcApproval>(bs => bs.Name.ToString().ToLower().Trim() == testName).FirstOrDefault();
                    if (testObj != null)
                    {
#if DEBUG
                        Console.WriteLine("{0} : {1} exists so skip on merge", testObj.GetType().Name, row.Name);
#endif
                        return true; //we have it so no need to create
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Set the IfcRelAssociatesApproval object
        /// </summary>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="rowName">Row name</param>
        /// <param name="ifcApproval">IfcApproval object</param>
        /// <param name="ifcRelAssociatesApproval">IfcRelAssociatesApproval object</param>
        private void SetRelObjectToApproval(string sheetName, string rowName, IfcApproval ifcApproval, IfcRelAssociatesApproval ifcRelAssociatesApproval)
        {
            IfcRoot ifcRoot = GetRootObject(sheetName, rowName);
            //sheetName = sheetName.ToLower().Trim();
            if (ifcRoot != null) //we have a object
            {
                ifcRelAssociatesApproval.RelatedObjects.Add_Reversible(ifcRoot);
                return;
            }

            if (sheetName == Constants.WORKSHEET_CONTACT)
            {
                IfcActorSelect ifcActorSelect = GetActorSelect(rowName);
                if (ifcActorSelect != null)
                {
                    //see if the relation ship exists, if so no need to create
                    IfcActorSelect IfcActorSelectTest = Model.InstancesOfType<IfcApprovalActorRelationship>()
                                                        .Where(aar => aar.Approval == ifcApproval)
                                                        .Select(aar => aar.Actor).OfType<IfcActorSelect>()
                                                        .Where(po => po == ifcActorSelect)
                                                        .FirstOrDefault();
                    if (IfcActorSelectTest == null)
                    {
                        IfcApprovalActorRelationship ifcApprovalActorRelationship = Model.New<IfcApprovalActorRelationship>();
                        ifcApprovalActorRelationship.Actor = ifcActorSelect;
                        ifcApprovalActorRelationship.Approval = ifcApproval;
                    }
                }
            }
        }

        /// <summary>
        /// Get the IfcTask object for the passed name
        /// </summary>
        /// <param name="name">IfcTask name</param>
        /// <returns>IfcTask Object</returns>
        private IfcTask GetTask(string name)
        {
            IfcTask ifcTask = null;

            if (IfcTasks == null)
                IfcTasks = Model.InstancesOfType<IfcTask>();

            name = name.ToLower().Trim();
            ifcTask = IfcTasks.Where(t => t.Name.ToString().ToLower().Trim() == name).FirstOrDefault();

            
            return ifcTask;
        }

        #endregion


        
    }
}
