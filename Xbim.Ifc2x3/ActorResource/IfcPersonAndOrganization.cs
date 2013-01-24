﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcPersonAndOrganization.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xbim.XbimExtensions.SelectTypes;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;

#endregion

namespace Xbim.Ifc2x3.ActorResource
{
    [IfcPersistedEntityAttribute, Serializable]
    public class PersonAndOrganizationCollection : XbimList<IfcPersonAndOrganization>
    {
        internal PersonAndOrganizationCollection(IPersistIfcEntity owner)
            : base(owner)
        {
        }
    }


    [IfcPersistedEntityAttribute, Serializable, IndexedClass]
    public class IfcPersonAndOrganization : IfcActorSelect, ISupportChangeNotification, INotifyPropertyChanged,
                                            IPersistIfcEntity, IfcObjectReferenceSelect, INotifyPropertyChanging
    {

        public override bool Equals(object obj)
        {
            // Check for null
            if (obj == null) return false;

            // Check for type
            if (this.GetType() != obj.GetType()) return false;

            // Cast as IfcRoot
            IfcPersonAndOrganization root = (IfcPersonAndOrganization)obj;
            return this == root;
        }
        public override int GetHashCode()
        {
            return Math.Abs(_entityLabel); //good enough as most entities will be in collections of  only one model, equals distinguishes for model
        }

        public static bool operator ==(IfcPersonAndOrganization left, IfcPersonAndOrganization right)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(left, right))
                return true;

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
                return false;

            return (Math.Abs(left.EntityLabel) == Math.Abs(right.EntityLabel)) && (left.ModelOf == right.ModelOf);

        }

        public static bool operator !=(IfcPersonAndOrganization left, IfcPersonAndOrganization right)
        {
            return !(left == right);
        }
        #region IPersistIfcEntity Members

        private int _entityLabel;
        private IModel _model;

        public IModel ModelOf
        {
            get { return _model; }
        }

        void IPersistIfcEntity.Bind(IModel model, int entityLabel)
        {
            _model = model;
            _entityLabel = entityLabel;
        }

        bool IPersistIfcEntity.Activated
        {
            get { return _entityLabel > 0; }
        }

        public int EntityLabel
        {
            get { return _entityLabel; }
        }

        void IPersistIfcEntity.Activate(bool write)
        {
            if (_model != null && _entityLabel <= 0) _entityLabel = _model.Activate(this, false);
            if (write) _model.Activate(this, write);
        }

        #endregion


        #region Fields and Events

        private IfcPerson _thePerson;
        private IfcOrganization _theOrganization;
        private ActorRoleCollection _roles;

        #endregion

        #region Constructors & Initialisers

        #endregion

        #region Ifc Properties

        /// <summary>
        ///   The person who is related to the organization.
        /// </summary>

        [IfcAttribute(1, IfcAttributeState.Mandatory)]
        public IfcPerson ThePerson
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _thePerson;
            }
            set { this.SetModelValue(this, ref _thePerson, value, v => ThePerson = v, "ThePerson"); }
        }

        /// <summary>
        ///   The organization to which the person is related.
        /// </summary>

        [IfcAttribute(2, IfcAttributeState.Mandatory)]
        public IfcOrganization TheOrganization
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _theOrganization;
            }
            set
            {
                this.SetModelValue(this, ref _theOrganization, value, v => TheOrganization = v,
                                           "TheOrganization");
            }
        }

        /// <summary>
        ///   Roles played by the person within the context of an organization. Use RoleString to set value
        /// </summary>

        [IfcAttribute(3, IfcAttributeState.Optional)]
        public ActorRoleCollection Roles
        {
            get
            {
                ((IPersistIfcEntity) this).Activate(false);
                return _roles;
            }
            private set { this.SetModelValue(this, ref _roles, value, v => Roles = v, "Roles"); }
        }

        #endregion

        #region Properties

        [Browsable(true)]
        public string RolesString
        {
            get { return _roles == null ? null : _roles.ToString("D; ", null); }
            set
            {
                throw new NotImplementedException();
                //using (Transaction txn = BeginTransaction(string.Format("Roles = {0}", value), true))
                //{
                //    if (string.IsNullOrEmpty(value))
                //    {
                //        Transaction.AddPropertyChange(v => _roles = v, _roles, null);
                //        _roles = null;
                //    }
                //    else
                //    {
                //        string[] roles = value.Split(new string[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                //        if (string.Join("; ", roles) == RolesString) //no real change so exit transaction
                //            return;
                //        if (_roles == null)
                //        {
                //            ActorRoleCollection c = new ActorRoleCollection();
                //            Transaction.AddPropertyChange(v => _roles = v, null, c);
                //            _roles = c;
                //        }
                //        else
                //            _roles.Clear_Reversible();

                //        if (roles != null)
                //        {
                //            foreach (string item in roles)
                //            {
                //                ActorRole aRole = new ActorRole(item);
                //                aRole.OnModelAdd(Model);
                //                _roles.Add_Reversible(aRole);
                //            }
                //        }
                //    }
                //    Transaction.AddTransactionReversedHandler(() => { NotifyPropertyChanged("Roles"); NotifyPropertyChanged("RolesString"); });
                //    if (txn != null) txn.Commit();
                //    NotifyPropertyChanged("Roles");
                //    NotifyPropertyChanged("RolesString");
                //}
            }
        }

        #endregion

        #region Ifc Inverse Relationships

        #endregion

        #region Part 21 Step file Parse routines

        public virtual void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                    _thePerson = value.EntityVal as IfcPerson;
                    break;
                case 1:
                    _theOrganization = value.EntityVal as IfcOrganization;
                    break;
                case 2:
                    if (_roles == null) _roles = new ActorRoleCollection(this);
                    _roles.Add(value.EntityVal as IfcActorRole);
                    break;
                default:
                    this.HandleUnexpectedAttribute(propIndex, value); break;
            }
        }

        #endregion

        #region Collection initialisation routines

      

        #endregion

        //TODO: Resolve

        ////public bool IsEquivalent(PersonAndOrganization po)
        ////{
        ////    if (po == null) return false;
        ////    return (po._theOrganization.IsEquivalent(_theOrganization)
        ////                && po._thePerson.IsEquivalent(_thePerson)
        ////                && po._roles.IsEquivalent(_roles));
        ////}

        #region INotifyPropertyChanged Members

        [field: NonSerialized] //don't serialize events
            private event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        void ISupportChangeNotification.NotifyPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [field: NonSerialized] //don't serialize events
            private event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { PropertyChanging += value; }
            remove { PropertyChanging -= value; }
        }

        #endregion

        #region ISupportChangeNotification Members

        void ISupportChangeNotification.NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region ISupportIfcParser Members

        public string WhereRule()
        {
            return "";
        }

        #endregion
    }
}