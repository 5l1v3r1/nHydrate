#region Copyright (c) 2006-2020 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2020 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.EFCSDL
{
    public class AuditEntityGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _item;

        public AuditEntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _model = model;
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}Audit.Generated.cs", _item.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}Audit.cs", _item.PascalName); }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    sb = new StringBuilder();
                    this.GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Audit");
                sb.AppendLine("{");
                this.AppendEntityClass();
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Xml.Serialization;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The object to hold the '" + _item.PascalName + "Audit' entity");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");

            sb.AppendLine("	public partial class " + _item.PascalName + "Audit : " + this.GetLocalNamespace() + ".IAudit, System.IComparable, System.IComparable<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit>");

            sb.AppendLine("	{");
            this.AppendConstructors();
            this.AppendProperties();
            this.AppendCompare();
            sb.AppendLine("	}");
            sb.AppendLine();

            #region Add the AuditResultFieldCompare class
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// ");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public interface I" + _item.PascalName + "AuditResultFieldCompare : " + this.GetLocalNamespace() + ".IAuditResultFieldCompare");
            sb.AppendLine("	{");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// ");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		new " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants Field { get; }");
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// A comparison class for audit comparison results");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	/// <typeparam name=\"T\"></typeparam>");
            sb.AppendLine("	public class " + _item.PascalName + "AuditResultFieldCompare<T> : " + this.GetLocalNamespace() + ".AuditResultFieldCompare<T, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants>, I" + _item.PascalName + "AuditResultFieldCompare");
            sb.AppendLine("	{");
            sb.AppendLine("		internal " + _item.PascalName + "AuditResultFieldCompare(T value1, T value2, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, System.Type dataType)");
            sb.AppendLine("			: base(value1, value2, field, dataType)");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
            #endregion

        }

        private void AppendConstructors()
        {
            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine($"		internal {_item.PascalName}Audit()");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            var columnList = new Dictionary<string, Column>();
            var tableList = new List<Table>(new Table[] { _item });

            //This is for inheritance which is NOT supported right now
            //List<Table> tableList = new List<Table>(_currentTable.GetTableHierarchy().Where(x => x.AllowAuditTracking).Reverse());
            foreach (Table table in tableList)
            {
                foreach (Column column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                    {
                        if (!columnList.ContainsKey(column.Name))
                            columnList.Add(column.Name, column);
                    }
                }
            }

            foreach (Column column in columnList.Values)
            {
                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(column.Description))
                    sb.AppendLine("		/// " + column.Description + "");
                else
                    sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + column.GetCodeType() + " " + column.PascalName + " { get; protected internal set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The primary key");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int __RowId { get; protected set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The type of audit");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + this.GetLocalNamespace() + ".AuditTypeConstants AuditType { get; protected internal set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The date of the audit");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public DateTime AuditDate { get; protected internal set; }");
            sb.AppendLine();

            //if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The modifier value of the audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public string ModifiedBy { get; protected internal set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendCompare()
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Given two audit items this method returns a set of differences");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item1\"></param>");
            sb.AppendLine("		/// <param name=\"item2\"></param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public static " + this.GetLocalNamespace() + ".AuditResult<" + _item.PascalName + "Audit> CompareAudits(" + _item.PascalName + "Audit item1, " + _item.PascalName + "Audit item2)");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new " + this.GetLocalNamespace() + ".AuditResult<" + _item.PascalName + "Audit>(item1, item2);");
            sb.AppendLine("			var differences = new List<I" + _item.PascalName + "AuditResultFieldCompare>();");
            sb.AppendLine();

            foreach (Column column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                {
                    sb.AppendLine("			if (item1." + column.PascalName + " != item2." + column.PascalName + ")");
                    sb.AppendLine("				differences.Add(new " + _item.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ", item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
                }
            }

            sb.AppendLine();
            sb.AppendLine("			retval.Differences = (IEnumerable<" + this.GetLocalNamespace() + ".IAuditResultFieldCompare>)differences;");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            //CompareTo
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares the current object with another object of the same type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"other\">An object to compare with this object.</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public int CompareTo(" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit other)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (other.AuditDate < this.AuditDate) return -1;");
            sb.AppendLine("			else if (this.AuditDate < other.AuditDate) return 1;");
            sb.AppendLine("			else return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares the current object with another object of the same type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"other\">An object to compare with this object.</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		int IComparable.CompareTo(object other)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (other is " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit) return this.CompareTo((" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit)other);");
            sb.AppendLine("			else return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
        }

        #endregion

    }
}