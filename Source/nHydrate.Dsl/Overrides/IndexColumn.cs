using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using System.ComponentModel;

namespace nHydrate.Dsl
{
    partial class IndexColumn
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public IndexColumn(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public IndexColumn(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }
        #endregion

        public virtual bool IsInternal { get; set; }

        public override string Definition
        {
            get
            {
                if (this.Index == null)
                {
                    return "(Not Defined)";
                }
                else
                {
                    var field = this.Index.Entity.Fields.FirstOrDefault(x => x.Id == this.FieldID);
                    if (field != null)
                        return field.Name;
                    else
                        return "(Not Defined)";
                }
            }
        }

        [Browsable(false)]
        public Field Field
        {
            get
            {
                if (this.Index == null) return null;
                if (this.Index.Entity == null) return null;
                if (this.Index.Entity.Fields == null) return null;
                return this.Index.Entity.Fields.FirstOrDefault(x => x.Id == this.FieldID);
            }
        }

        protected override void OnDeleting()
        {
            if (this.Index != null)
            {
                if (!this.Index.Entity.nHydrateModel.IsLoading && !this.Index.Entity.IsDeleting)
                {
                    //If this is the primary key then CANCEL
                    if (this.Index.IndexType == IndexTypeConstants.PrimaryKey)
                        throw new Exception("This is a managed index for the primary key and cannot be removed.");

                    //If this is the last column then remove index
                    if (this.Index.IndexColumns.Count == 1)
                    {
                        using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                        {
                            this.Index.Delete();
                            transaction.Commit();
                        }
                    }
                }
            }

            base.OnDeleting();
        }

        public override string ToString()
        {
            var f = this.GetField();
            if (f == null)
                return "(Undefined)";
            else
                return f.Name;
        }

    }

    partial class IndexColumnBase
    {
        partial class FieldIDPropertyHandler
        {
            protected override void OnValueChanged(IndexColumnBase element, Guid oldValue, Guid newValue)
            {
                if (element.Index != null)
                {
                    if (!element.Index.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
                    {
                        if (element.Index.IndexType != IndexTypeConstants.User)
                            throw new Exception("This is a managed index and cannot be modified.");
                    }
                }
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

        partial class AscendingPropertyHandler
        {
            protected override void OnValueChanged(IndexColumnBase element, bool oldValue, bool newValue)
            {
                if (element.Index != null)
                {
                    if (!element.Index.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
                    {
                        if (element.Index.IndexType != IndexTypeConstants.User)
                            throw new Exception("This is a managed index and cannot be modified.");
                    }
                }
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

    }

}
