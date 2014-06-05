#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.Generator.EFCodeFirst.Generators.Entity
{
    public class EntityGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _currentTable;

        public EntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _currentTable = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}.Generated.cs", _currentTable.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}.cs", _currentTable.PascalName); }
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
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                this.AppendEntityClass();
                sb.AppendLine("}");
                sb.AppendLine();
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
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine("using System.Text.RegularExpressions;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Data.Entity;");
            sb.AppendLine("using System.Data.Linq;");
            sb.AppendLine("using System.Data.Entity.ModelConfiguration;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.Data.Entity.Core.Objects.DataClasses;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            var doubleDerivedClassName = _currentTable.PascalName;
            if (_currentTable.GeneratesDoubleDerived)
            {
                doubleDerivedClassName = _currentTable.PascalName + "Base";

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The collection to hold '" + _currentTable.PascalName + "' entities");
                if (!string.IsNullOrEmpty(_currentTable.Description))
                    StringHelper.LineBreakCode(sb, _currentTable.Description, "	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
                if (_currentTable.IsAbstract)
                    sb.AppendLine("	public abstract partial class " + _currentTable.PascalName + " : " + doubleDerivedClassName);
                else
                    sb.AppendLine("	public partial class " + _currentTable.PascalName + " : " + doubleDerivedClassName);
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The collection to hold '" + _currentTable.PascalName + "' entities");
            if (!string.IsNullOrEmpty(_currentTable.Description))
                sb.AppendLine("	/// " + _currentTable.Description);
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[DataContract]");
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	[System.Data.Linq.Mapping.Table(Name = \"" + _currentTable.PascalName + "\")]");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            
            if (_currentTable.IsTenant)
                sb.AppendLine("	[EdmEntityTypeAttribute(NamespaceName = \"" + this.GetLocalNamespace() + ".Entity" + "\", Name = \"" + _model.TenantPrefix + "_" + _currentTable.PascalName + "\")]");
            else
                sb.AppendLine("	[EdmEntityTypeAttribute(NamespaceName = \"" + this.GetLocalNamespace() + ".Entity" + "\", Name = \"" + _currentTable.PascalName + "\")]");

            sb.AppendLine("	[nHydrate.EFCore.Attributes.FieldNameConstantsAttribute(typeof(" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants))]");
            sb.AppendLine("	[System.ComponentModel.DataAnnotations.MetadataType(typeof(" + this.InterfaceAssemblyNamespace + ".Entity.Metadata." + _currentTable.PascalName + "Metadata))]");

            //Add known types for all descendants
            foreach (var table in _currentTable.GetTablesInheritedFromHierarchy().Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("	[KnownType(typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "))]");
            }

            if (!string.IsNullOrEmpty(_currentTable.Description))
            {
                sb.AppendLine("	[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(_currentTable.Description) + "\")]");
            }

            if (_currentTable.Immutable)
                sb.AppendLine("	[System.ComponentModel.ImmutableObject(true)]");

            sb.AppendLine("	[nHydrate.EFCore.Attributes.EntityMetadata(\"" + _currentTable.PascalName + "\", " + _currentTable.AllowAuditTracking.ToString().ToLower() + ", " + _currentTable.AllowCreateAudit.ToString().ToLower() + ", " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", " + _currentTable.AllowTimestamp.ToString().ToLower() + ", \"" + StringHelper.ConvertTextToSingleLineCodeString(_currentTable.Description) + "\", " + _currentTable.EnforcePrimaryKey.ToString().ToLower() + ", " + _currentTable.Immutable.ToString().ToLower() + ", " + (_currentTable.TypedTable != TypedTableConstants.None).ToString().ToLower() + ", \"" + _currentTable.GetSQLSchema() + "\")]");

            //Auditing
            if (_currentTable.AllowAuditTracking)
                sb.AppendLine("	[nHydrate.EFCore.Attributes.EntityHistory(typeof(" + this.GetLocalNamespace() + ".Audit." + _currentTable.PascalName + "Audit))]");

            foreach (var meta in _currentTable.MetaData)
            {
                sb.AppendLine("	[nHydrate.EFCore.Attributes.CustomMetadata(Key = \"" + StringHelper.ConvertTextToSingleLineCodeString(meta.Key) + "\", Value = \"" + meta.Value.Replace("\"", "\\\"") + "\")]");
            }

            var boInterface = "nHydrate.EFCore.DataAccess.IBusinessObject";
            if (_currentTable.Immutable) boInterface = "nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject";
            boInterface += ", " + this.GetLocalNamespace() + ".IEntityWithContext";
            boInterface += ", " + "System.ComponentModel.IDataErrorInfo";

            if (_currentTable.IsAbstract)
            {
                if (_currentTable.ParentTable == null)
                    sb.Append("	public abstract partial class " + doubleDerivedClassName + " : nHydrate.EFCore.DataAccess.BaseEntity, " + boInterface + ", " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ", System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">");
                else
                    sb.Append("	public abstract partial class " + doubleDerivedClassName + " : " + this.GetLocalNamespace() + ".Entity." + _currentTable.ParentTable.PascalName + ", " + boInterface + ", " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ", System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">");
            }
            else //NON-Abstract
            {
                if (_currentTable.ParentTable == null)
                    sb.Append("	public " + (_currentTable.GeneratesDoubleDerived ? "abstract " : "") + "partial class " + doubleDerivedClassName + " : nHydrate.EFCore.DataAccess.BaseEntity, " + boInterface + ", " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ", System.ICloneable, System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">");
                else
                    sb.Append("	public " + (_currentTable.GeneratesDoubleDerived ? "abstract " : "") + "partial class " + doubleDerivedClassName + " : " + this.GetLocalNamespace() + ".Entity." + _currentTable.ParentTable.PascalName + ", " + boInterface + ", " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ", System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">");
            }

            //if (_currentTable.ParentTable == null)
            //    sb.Append("	public partial class " + _currentTable.PascalName);
            //else
            //    sb.Append("	public partial class " + _currentTable.PascalName + " : " + this.GetLocalNamespace() + ".Entity." + _currentTable.ParentTable.PascalName);

            if (_currentTable.AllowCreateAudit || _currentTable.AllowModifiedAudit || _currentTable.AllowTimestamp)
            {
                sb.Append(", nHydrate.EFCore.DataAccess.IAuditable");
            }
            sb.AppendLine();

            sb.AppendLine("	{");
            this.AppendedFieldEnum();
            this.AppendConstructors();
            this.AppendProperties();
            this.AppendGenerateEvents();
            this.AppendRegionGetMaxLength();
            //this.AppendParented();
            this.AppendClone();
            this.AppendIsEquivalent();
            this.AppendRegionGetValue();
            this.AppendRegionSetValue();
            this.AppendNavigationProperties();
            this.AppendAuditQuery();
            this.AppendStaticSQLHelpers();
            this.AppendDeleteDataScaler();
            this.AppendUpdateDataScaler();
            this.AppendRegionGetDatabaseFieldName();
            this.AppendContextReference();
            this.AppendIAuditable();
            //this.AppendStaticMethods();
            this.AppendIEquatable();
            this.AppendIDataErrorInfo();
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        private void AppendedFieldEnum()
        {
            var imageColumnList = _currentTable.GetColumnsByType(System.Data.SqlDbType.Image).OrderBy(x => x.Name).ToList();
            if (imageColumnList.Count != 0)
            {
                sb.AppendLine("		#region FieldImageConstants Enumeration");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// An enumeration of this object's image type fields");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public enum FieldImageConstants");
                sb.AppendLine("		{");
                foreach (var column in imageColumnList)
                {
                    sb.AppendLine("			/// <summary>");
                    sb.AppendLine("			/// Field mapping for the image parameter '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                    sb.AppendLine("			/// </summary>");
                    sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image parameter '" + column.PascalName + "' property\")]");
                    sb.AppendLine("			" + column.PascalName + ",");
                }
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
            }

            sb.AppendLine("		#region FieldNameConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _currentTable.PascalName + "' table.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + (_currentTable.ParentTable == null ? "" : "new ") + "enum FieldNameConstants");
            sb.AppendLine("		{");
            foreach (var column in _currentTable.GeneratedColumnsFullHierarchy)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                sb.AppendLine("			/// </summary>");

                if (column.PrimaryKey)
                {
                    sb.AppendLine("			[nHydrate.EFCore.Attributes.PrimaryKeyAttribute()]");
                }

                if (column.PrimaryKey || _currentTable.Immutable)
                {
                    sb.AppendLine("			[System.ComponentModel.ReadOnlyAttribute(true)]");
                }

                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
                sb.AppendLine("			" + column.PascalName + ",");
            }

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedByPascalName + "' property");
                sb.AppendLine("			/// </summary>");
                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedByPascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.CreatedByPascalName + ",");
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property");
                sb.AppendLine("			/// </summary>");
                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.CreatedDatePascalName + ",");
            }

            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property");
                sb.AppendLine("			/// </summary>");
                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.ModifiedByPascalName + ",");
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property");
                sb.AppendLine("			/// </summary>");
                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.ModifiedDatePascalName + ",");
            }

            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendConstructors()
        {
            string scope = "public";
            if (_currentTable.Immutable)
                scope = "protected internal";

            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new instance of the " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + " class");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		" + scope + " " + _currentTable.PascalName + "()");
            sb.AppendLine("		{");
            if (_currentTable.PrimaryKeyColumns.Count == 1 && _currentTable.PrimaryKeyColumns[0].DataType == System.Data.SqlDbType.UniqueIdentifier)
                sb.AppendLine("			this." + _currentTable.PrimaryKeyColumns[0].PascalName + " = Guid.NewGuid();");
            sb.Append(this.SetInitialValues("this"));
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Overload with key
            if (_currentTable.PrimaryKeyColumns.Count == _currentTable.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.None))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new instance of the " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + " class with a defined primary key");
                sb.AppendLine("		/// </summary>");
                sb.Append("		" + scope + " " + _currentTable.PascalName + "(");
                int index = 0;
                foreach (Column pkColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.PascalName))
                {
                    sb.Append(pkColumn.GetCodeType() + " " + pkColumn.CamelName);
                    if (index < _currentTable.PrimaryKeyColumns.Count - 1)
                        sb.Append(", ");
                    index++;
                }
                sb.AppendLine(")");

                sb.AppendLine("			: this()");
                sb.AppendLine("		{");
                foreach (Column pkColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.PascalName))
                {
                    sb.AppendLine("			this." + pkColumn.PascalName + " = " + pkColumn.CamelName + ";");
                }
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendClone()
        {
            if (_currentTable.IsAbstract)
                return;

            string modifieraux;
            if (_currentTable.ParentTable == null)
            {
                modifieraux = "virtual";
            }
            else
            {
                if (_currentTable.ParentTable.IsAbstract)
                    modifieraux = "virtual";
                else
                    modifieraux = "new";
            }

            sb.AppendLine("		#region Clone");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + modifieraux + " object Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			return " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".Clone(this);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object with defined, default values and new PK");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + modifieraux + " object CloneAsNew()");
            sb.AppendLine("		{");
            sb.AppendLine("			var item = " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".Clone(this);");
            foreach (var pk in _currentTable.GeneratedColumns.Where(x => x.Identity == IdentityTypeConstants.Database && x.IsNumericType))
            {
                sb.AppendLine("			item._" + pk.CamelName + " = 0;");
            }
            sb.Append(this.SetInitialValues("item"));
            sb.AppendLine("			return item;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + _currentTable.PascalName + " Clone(" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + " item)");
            sb.AppendLine("		{");
            sb.AppendLine("			var newItem = new " + _currentTable.PascalName + "();");
            foreach (var column in _currentTable.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("			newItem." + column.PascalName + " = item." + column.PascalName + ";");
            }
            sb.AppendLine("			return newItem;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIsEquivalent()
        {
            sb.AppendLine("		#region IsEquivalent");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if all of the fields for the specified object exactly matches the current object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item\">The object to compare</param>");
            sb.AppendLine("		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)");
            sb.AppendLine("		{");
            sb.AppendLine("			return ((System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">)this).Equals(item as " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (Column column in _currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                string roleName;
                string pascalRoleName;
                Table typeTable = null;
                if (_currentTable.IsColumnRelatedToTypeTable(column, out pascalRoleName) || (column.PrimaryKey && _currentTable.TypedTable != TypedTableConstants.None))
                {
                    typeTable = _currentTable.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                    if (typeTable == null) typeTable = _currentTable;
                    if (typeTable != null)
                    {
                        var nullSuffix = string.Empty;
                        if (column.AllowNull)
                            nullSuffix = "?";

                        sb.AppendLine("		/// <summary>");

                        sb.AppendLine("		/// This property is a wrapper for the typed enumeration for the '" + column.PascalName + "' field.");
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");
                        sb.AppendLine("		public virtual " + this.InterfaceAssemblyNamespace + "." + typeTable.PascalName + "Constants" + nullSuffix + " " + pascalRoleName + typeTable.PascalName + "Value");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return (" + this.InterfaceAssemblyNamespace + "." + typeTable.PascalName + "Constants" + nullSuffix + ")this." + column.PascalName + "; }");
                        sb.AppendLine("			set { this." + column.PascalName + " = (" + column.GetCodeType(true) + ")value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }

                if (column.PrimaryKey && _currentTable.ParentTable != null)
                {
                    //PK in descendant, do not process
                }
                //else if (_currentTable.IsColumnRelatedToTypeTable(column, out roleName))
                //{
                //    var typeTable = _currentTable.GetRelatedTypeTableByColumn(column, out roleName);

                //    //If this column is a type table column then generate a special property
                //    sb.AppendLine("		/// <summary>");
                //    if (!string.IsNullOrEmpty(column.Description))
                //        sb.AppendLine("		/// " + column.Description + "");
                //    else
                //        sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                //    sb.AppendLine("		/// </summary>");
                //    sb.AppendLine("		/// <remarks>" + column.GetIntellisenseRemarks() + "</remarks>");
                //    sb.AppendLine("		[DataMember]");

                //    if (column.IsTextType && column.Length > 0)
                //    {
                //        sb.AppendLine("		[StringLength(" + column.Length + ")]");
                //    }

                //    //TODO: NEXT VERSION: CODE FIRST CTP5 DOES NOT SUPPORT ENUMS SO ADD INTEGERS
                //    //sb.AppendLine("		public virtual " + typeTable.PascalName + "Constants " + typeTable.PascalName + " { get; set; }");
                //    sb.AppendLine("		public virtual " + column.GetCodeType() + " " + column.PascalName + " { get; set; }");

                //    sb.AppendLine();
                //}
                else
                {
                    sb.AppendLine("		/// <summary>");
                    if (!string.IsNullOrEmpty(column.Description))
                    {
                        StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                    }
                    else
                        sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field.");
                }
                //If this field has a related convenience property then explain it
                if (typeTable != null)
                {
                    sb.AppendLine("		/// This property has an additional enumeration wrapper property " + pascalRoleName + typeTable.PascalName + "Value. Use it as a strongly-typed property.");
                }
                else if (column.PrimaryKey && _currentTable.TypedTable != TypedTableConstants.None)
                {
                    sb.AppendLine("		/// This property has an additional enumeration wrapper property " + pascalRoleName + typeTable.PascalName + "Value. Use it as a strongly-typed property.");
                }

                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <remarks>" + column.GetIntellisenseRemarks() + "</remarks>");
                sb.AppendLine("		[DataMember]");

                sb.AppendLine("		[System.ComponentModel.Browsable(" + column.IsBrowsable.ToString().ToLower() + ")]");
                if (!string.IsNullOrEmpty(column.Category))
                    sb.AppendLine("		[System.ComponentModel.Category(\"" + column.Category + "\")]");
                sb.AppendLine("		[EdmScalarPropertyAttribute(EntityKeyProperty = " + (column.PrimaryKey ? "true" : "false") + ", IsNullable = " + (column.AllowNull ? "true" : "false") + ")]");
                sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.GetFriendlyName() + "\")]");

                if (column.UIDataType != System.ComponentModel.DataAnnotations.DataType.Custom)
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType." + column.UIDataType.ToString() + ")]");
                }

                if (!string.IsNullOrEmpty(column.Mask))
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
                }

                if (column.PrimaryKey)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");

                if (column.PrimaryKey || _currentTable.Immutable || column.ComputedColumn || column.IsReadOnly)
                    sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

                if (!string.IsNullOrEmpty(column.Description))
                    sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                foreach (var meta in column.MetaData)
                {
                    sb.AppendLine("	[nHydrate.EFCore.Attributes.CustomMetadata(Key = \"" + StringHelper.ConvertTextToSingleLineCodeString(meta.Key) + "\", Value = \"" + meta.Value.Replace("\"", "\\\"") + "\")]");
                }

                sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");

                if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml && column.Length > 0)
                {
                    sb.AppendLine("		[StringLength(" + column.Length + ")]");
                }

                if (column.ComputedColumn)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]");

                sb.AppendLine("		public virtual " + column.GetCodeType() + " " + column.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + column.CamelName + "; }");
                sb.AppendLine("			set { _" + column.CamelName + " = value; }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            //Audit Fields
            if (_currentTable.ParentTable == null)
            {
                if (_currentTable.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedByPascalName, "string", "The audit field for the 'Created By' parameter.", "public");
                if (_currentTable.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedDatePascalName, "DateTime?", "The audit field for the 'Created Date' parameter.", "public");
                if (_currentTable.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' parameter.", "public");
                if (_currentTable.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedDatePascalName, "DateTime?", "The audit field for the 'Modified Date' parameter.", "public");
                if (_currentTable.AllowTimestamp) GenerateAuditField(_model.Database.TimestampPascalName, "byte[]", "The audit field for the 'Timestamp' parameter.", "public");
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendNavigationProperties()
        {
            sb.AppendLine("		#region Navigation Properties");
            sb.AppendLine();

            #region Parent Relations
            {
                var relationList = _currentTable.GetRelations().Where(x => x.IsValidEFRelation);
                foreach (Relation relation in relationList)
                {
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    var childTable = (Table)relation.ChildTableRef.Object;

                    //If not both generated then do not process this code block
                    if (!parentTable.Generated || !childTable.Generated)
                    {
                        //Do Nothing
                    }

                    //inheritance relationship
                    else if (parentTable == childTable.ParentTable && relation.IsOneToOne)
                    {
                        //Do Nothing
                    }

                    //Do not walk to type tables
                    //else if ((parentTable.TypedTable != TypedTableConstants.None) || (childTable.TypedTable != TypedTableConstants.None))
                    //{
                    //    //Do Nothing
                    //}

                    //1-1 relations
                    else if (relation.IsOneToOne)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + _currentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[DataMember]");
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped]");
                        sb.AppendLine("		public virtual " + childTable.PascalName + " " + relation.PascalRoleName + childTable.PascalName + " { get; set; }");
                        sb.AppendLine();

                        //Add interface map
                        sb.AppendLine("		" + this.InterfaceAssemblyNamespace + ".Entity.I" + childTable.PascalName + " " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + "." + relation.PascalRoleName + childTable.PascalName + "");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return this." + relation.PascalRoleName + childTable.PascalName + "; }");
                        sb.AppendLine("			set { this." + relation.PascalRoleName + childTable.PascalName + " = (" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ")value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }

                    //Process the associative tables
                    else if (childTable.AssociativeTable)
                    {
                        var associativeRelations = childTable.GetRelationsWhereChild();
                        Relation targetRelation = null;
                        Relation otherRelation = null;
                        var relation1 = associativeRelations.First();
                        var relation2 = associativeRelations.Last();
                        if (_currentTable == ((Table)relation1.ParentTableRef.Object)) targetRelation = relation2;
                        else targetRelation = relation1;
                        if (targetRelation == relation2) otherRelation = relation1;
                        else otherRelation = relation2;
                        var targetTable = targetRelation.ParentTableRef.Object as Table;

                        if (targetTable.Generated && (targetTable.TypedTable != TypedTableConstants.EnumOnly))
                        {
                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine("		/// The navigation definition for walking " + _currentTable.PascalName + "->" + targetTable.PascalName + (string.IsNullOrEmpty(otherRelation.PascalRoleName) ? "" : " (role: '" + otherRelation.PascalRoleName + "')"));
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		[DataMember]");
                            //sb.AppendLine("		public virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + targetTable.PascalName + "> " + targetTable.PascalName + "List { get; set; }");
                            sb.AppendLine("		public virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + targetTable.PascalName + "> " + targetTable.PascalName + "List");
                            sb.AppendLine("		{");
                            sb.AppendLine("			get");
                            sb.AppendLine("			{");
                            sb.AppendLine("				if (_" + targetTable.PascalName + "List == null) _" + targetTable.PascalName + "List = new List<" + this.GetLocalNamespace() + ".Entity." + targetTable.PascalName + ">();");
                            sb.AppendLine("				return _" + targetTable.PascalName + "List;");
                            sb.AppendLine("			}");
                            sb.AppendLine("			set { _" + targetTable.PascalName + "List = value; }");
                            sb.AppendLine("		}");
                            sb.AppendLine("		protected virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + targetTable.PascalName + "> _" + targetTable.PascalName + "List { get; set; }");
                            sb.AppendLine();

                            //Interface implementation
                            sb.AppendLine("		ICollection<" + this.InterfaceAssemblyNamespace + ".Entity.I" + targetTable.PascalName + "> " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + "." + relation.PascalRoleName + targetTable.PascalName + "List");
                            sb.AppendLine("		{");
                            sb.AppendLine("			get { return this." + relation.PascalRoleName + targetTable.PascalName + "List.Cast<" + this.InterfaceAssemblyNamespace + ".Entity.I" + targetTable.PascalName + ">().ToList(); }");
                            sb.AppendLine("		}");
                            sb.AppendLine();
                        }
                    }

                    //Process relations where Current Table is the parent
                    else if (parentTable == _currentTable && parentTable.Generated && childTable.Generated && (childTable.TypedTable != TypedTableConstants.EnumOnly) && !childTable.AssociativeTable)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[DataMember]");
                        //sb.AppendLine("		public virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List { get; set; }");
                        sb.AppendLine("		public virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get");
                        sb.AppendLine("			{");
                        sb.AppendLine("				if (_" + relation.PascalRoleName + childTable.PascalName + "List == null) _" + relation.PascalRoleName + childTable.PascalName + "List = new List<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ">();");
                        sb.AppendLine("				return _" + relation.PascalRoleName + childTable.PascalName + "List;");
                        sb.AppendLine("			}");
                        sb.AppendLine("			set { _" + relation.PascalRoleName + childTable.PascalName + "List = value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine("		protected virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "> _" + relation.PascalRoleName + childTable.PascalName + "List { get; set; }");
                        sb.AppendLine();

                        //Interface implementation
                        sb.AppendLine("		ICollection<" + this.InterfaceAssemblyNamespace + ".Entity.I" + childTable.PascalName + "> " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + "." + relation.PascalRoleName + childTable.PascalName + "List");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return this." + relation.PascalRoleName + childTable.PascalName + "List.Cast<" + this.InterfaceAssemblyNamespace + ".Entity.I" + childTable.PascalName + ">().ToList(); }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }
            }
            #endregion

            #region Child Relations
            {
                var relationList = _currentTable.GetRelationsWhereChild().Where(x => x.IsValidEFRelation).AsEnumerable();
                foreach (Relation relation in relationList)
                {
                    Table parentTable = (Table)relation.ParentTableRef.Object;
                    Table childTable = (Table)relation.ChildTableRef.Object;

                    //Do not walk to associative
                    if ((parentTable.TypedTable == TypedTableConstants.EnumOnly) || (childTable.TypedTable == TypedTableConstants.EnumOnly))
                    {
                        //Do Nothing
                    }

                    //inheritance relationship
                    else if (parentTable == childTable.ParentTable && relation.IsOneToOne)
                    {
                        //Do Nothing
                    }

                    //Process relations where Current Table is the child
                    else if (childTable == _currentTable && parentTable.Generated && childTable.Generated && !parentTable.IsInheritedFrom(_currentTable))
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[DataMember]");
                        //sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped]");
                        sb.AppendLine("		public virtual " + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + " { get; set; }");
                        sb.AppendLine();

                        //Add interface map
                        sb.AppendLine("		" + this.InterfaceAssemblyNamespace + ".Entity.I" + parentTable.PascalName + " " + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + "." + relation.PascalRoleName + parentTable.PascalName + "");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return this." + relation.PascalRoleName + parentTable.PascalName + "; }");
                        sb.AppendLine("			set { this." + relation.PascalRoleName + parentTable.PascalName + " = (" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + ")value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }
            }
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendGenerateEvents()
        {
            sb.AppendLine("		#region Property Holders");
            sb.AppendLine();

            foreach (var column in _currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                if (column.PrimaryKey && _currentTable.ParentTable != null)
                {
                    //PK in descendant, do not process
                }
                else
                {
                    sb.AppendLine("		/// <summary />");
                    sb.AppendLine("		protected " + column.GetCodeType() + " _" + column.CamelName + ";");
                    this.AppendPropertyEventDeclarations(column, column.GetCodeType());
                }
            }

            //Audit Fields
            if (_currentTable.ParentTable == null)
            {
                if (_currentTable.AllowCreateAudit) this.AppendPropertyEventDeclarations(_model.Database.CreatedByPascalName, "string");
                if (_currentTable.AllowCreateAudit) this.AppendPropertyEventDeclarations(_model.Database.CreatedDatePascalName, "DateTime?");
                if (_currentTable.AllowModifiedAudit) this.AppendPropertyEventDeclarations(_model.Database.ModifiedByPascalName, "string");
                if (_currentTable.AllowModifiedAudit) this.AppendPropertyEventDeclarations(_model.Database.ModifiedDatePascalName, "DateTime?");
                if (_currentTable.AllowTimestamp) this.AppendPropertyEventDeclarations(_model.Database.TimestampPascalName, "byte[]");
            }

            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //TODO: Implement this!!
            sb.AppendLine("		public event PropertyChangedEventHandler PropertyChanged;");
            sb.AppendLine("		public event PropertyChangingEventHandler PropertyChanging;");
            sb.AppendLine();
        }

        private void AppendPropertyEventDeclarations(Column column, string codeType)
        {
            //Table typetable = _currentTable.GetRelatedTypeTableByColumn(column);
            //if (typetable != null)
            //{
            //  this.AppendPropertyEventDeclarations(typetable.PascalName, codeType);
            //}
            //else
            {
                this.AppendPropertyEventDeclarations(column.PascalName, codeType);
            }
        }

        private void AppendPropertyEventDeclarations(string columnName, string codeType)
        {
            //Do not support custom events
            if (!_model.EnableCustomChangeEvents) return;

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value change is a pending.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public event EventHandler<nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + ">> " + columnName + "Changing;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changing event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changing(nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changing != null)");
            sb.AppendLine("				this." + columnName + "Changing(this, e);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value has changed.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public event EventHandler<ChangedEventArgs<" + codeType + ">> " + columnName + "Changed;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changed event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changed(ChangedEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changed != null)");
            sb.AppendLine("				this." + columnName + "Changed(this, e);");
            sb.AppendLine("		}");
            sb.AppendLine();
        }

        private void AppendRegionGetMaxLength()
        {
            sb.AppendLine("		#region GetMaxLength");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the maximum size of the field value.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static int GetMaxLength(FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _currentTable.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
                if (_currentTable.GeneratedColumns.Contains(column))
                {
                    //This is an actual column in this table
                    switch (column.DataType)
                    {
                        case System.Data.SqlDbType.Text:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.NText:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Image:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Xml:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Char:
                        case System.Data.SqlDbType.NChar:
                        case System.Data.SqlDbType.NVarChar:
                        case System.Data.SqlDbType.VarChar:
                            if ((column.Length == 0) && (ModelHelper.SupportsMax(column.DataType)))
                                sb.AppendLine("					return int.MaxValue;");
                            else
                                sb.AppendLine("					return " + column.Length + ";");
                            break;
                        default:
                            sb.AppendLine("					return 0;");
                            break;
                    }
                }
                else
                {
                    //This is an inherited column so check the base
                    sb.AppendLine("					return " + _currentTable.ParentTable.PascalName + ".GetMaxLength(" + _currentTable.ParentTable.PascalName + ".FieldNameConstants." + column.PascalName + ");");
                }
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMaxLength((FieldNameConstants)field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("		#region GetFieldNameConstants");
            sb.AppendLine();
            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()");
            sb.AppendLine("		{");
            sb.AppendLine("			return typeof(FieldNameConstants);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetFieldType
            sb.AppendLine("		#region GetFieldType");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the system type of a field on this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static System.Type GetFieldType(FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants'.\");");
            sb.AppendLine();
            sb.AppendLine("			switch ((FieldNameConstants)field)");
            sb.AppendLine("			{");
            foreach (var column in _currentTable.GeneratedColumnsFullHierarchy)
            {
                sb.AppendLine("				case FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants'.\");");
            sb.AppendLine();
            sb.AppendLine("			return GetFieldType((" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants)field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetValue
            sb.AppendLine("		#region Get/Set Value");
            sb.AppendLine();
            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)this).GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
            sb.AppendLine("			return this.GetValue((FieldNameConstants)field, defaultValue);");
            sb.AppendLine("		}");
            sb.AppendLine();

            if (!_currentTable.Immutable)
            {
                sb.AppendLine("		void nHydrate.EFCore.DataAccess.IBusinessObject.SetValue(System.Enum field, object newValue)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
                sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
                sb.AppendLine("			this.SetValue((FieldNameConstants)field, newValue);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //If this is not derived then add the Primary key stuff
            if (_currentTable.ParentTable == null)
            {
                sb.AppendLine("		#region PrimaryKey");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Hold the primary key for this object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;");
                sb.AppendLine("		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return null; }");
                #region OLD CODE
                //sb.AppendLine("			get");
                //sb.AppendLine("			{");
                //sb.Append("				if (_primaryKey == null)");
                //sb.Append(" _primaryKey = new " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + "PrimaryKey(");

                //var ii = 0;
                //foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
                //{
                //  sb.Append("this." + column.PascalName);
                //  if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
                //    sb.Append(", ");
                //  ii++;
                //}
                //sb.AppendLine(");");
                //sb.AppendLine("				return _primaryKey;");
                //sb.AppendLine("			}");
                #endregion
                sb.AppendLine("		}");

                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
            }

        }

        private string SetInitialValues(string propertyObjectPrefix)
        {
            //TODO - Audit Trail not implemented
            var setOriginalGuid = String.Format("\t\t\t" + propertyObjectPrefix + "._{0} = System.Guid.NewGuid();", _currentTable.PrimaryKeyColumns.First().CamelName);

            var returnVal = new StringBuilder();
            if (_currentTable.PrimaryKeyColumns.Count == 1 && ((Column)_currentTable.PrimaryKeyColumns.First()).DataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                if (_currentTable.PrimaryKeyColumns.First().DataType == System.Data.SqlDbType.UniqueIdentifier)
                    returnVal.AppendLine(setOriginalGuid);
            }

            //DEFAULT PROPERTIES START
            foreach (var column in _currentTable.GeneratedColumns.Where(x => x.DataType != System.Data.SqlDbType.Timestamp).ToList())
            {
                if (!string.IsNullOrEmpty(column.Default))
                {
                    var defaultValue = column.GetCodeDefault();

                    //Write the actual code
                    if (!string.IsNullOrEmpty(defaultValue))
                        returnVal.AppendLine("			" + propertyObjectPrefix + "._" + column.CamelName + " = " + defaultValue + ";");
                }
            }
            //DEFAULT PROPERTIES END

            return returnVal.ToString();
        }

        private void AppendParented()
        {
            sb.AppendLine("		#region IsParented");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if this object is part of a collection or is detached");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
            sb.AppendLine(_currentTable.ParentTable == null ? "		public virtual bool IsParented" : "		public override bool IsParented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return (this.EntityState != System.Data.EntityState.Detached); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionGetValue()
        {
            sb.AppendLine("		#region GetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(FieldNameConstants field, object defaultValue)");
            sb.AppendLine("		{");
            var allColumns = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated).ToList();
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var relationParentTable = (Table)column.ParentTableRef.Object;
                var childColumnList = relationParentTable.AllRelationships.FindByChildColumn(column);
                sb.AppendLine("			if (field == FieldNameConstants." + column.PascalName + ")");
                if (column.AllowNull)
                    sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
                else
                    sb.AppendLine("				return this." + column.PascalName + ";");
            }

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.CreatedByPascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.CreatedByPascalName + " == null) ? defaultValue : this." + _model.Database.CreatedByPascalName + ");");
                sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.CreatedDatePascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.CreatedDatePascalName + " == null) ? defaultValue : this." + _model.Database.CreatedDatePascalName + ");");
            }

            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.ModifiedByPascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.ModifiedByPascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedByPascalName + ");");
                sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.ModifiedDatePascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.ModifiedDatePascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedDatePascalName + ");");
            }

            //Now do the primary keys
            foreach (var dbColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                //TODO
            }

            sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionSetValue()
        {
            if (_currentTable.Immutable)
                return;

            sb.AppendLine("		#region SetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine("		public virtual void SetValue(FieldNameConstants field, object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			SetValue(field, newValue, false);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
            sb.AppendLine("		public virtual void SetValue(FieldNameConstants field, object newValue, bool fixLength)");
            sb.AppendLine("		{");

            var allColumns = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated).ToList();
            var count = 0;
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                if (column.Generated)
                {
                    //If not the first one, add an 'ELSE' for speed
                    if (count == 0) sb.Append("			");
                    else sb.Append("			else ");

                    sb.AppendLine("if (field == FieldNameConstants." + column.PascalName + ")");
                    sb.AppendLine("			{");
                    if (column.PrimaryKey)
                    {
                        sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a primary key and cannot be set!\");");
                    }
                    else if (column.ComputedColumn)
                    {
                        sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a computed parameter and cannot be set!\");");
                    }
                    else if (column.IsReadOnly)
                    {
                        sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a read-only parameter and cannot be set!\");");
                    }
                    else
                    {
                        if (ModelHelper.IsTextType(column.DataType))
                        {
                            sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperInternal((string)newValue, fixLength, GetMaxLength(field));");
                            #region old code
                            //sb.AppendLine("				if (newValue == null)");
                            //sb.AppendLine("				{");
                            //sb.AppendLine("					this." + column.PascalName + " = null;");
                            //sb.AppendLine("				}");
                            //sb.AppendLine("				else");
                            //sb.AppendLine("				{");
                            //sb.AppendLine("					string v = newValue.ToString();");
                            //sb.AppendLine("					if (fixLength)");
                            //sb.AppendLine("					{");
                            //sb.AppendLine("						int fieldLength = GetMaxLength(FieldNameConstants." + column.PascalName + ");");
                            //sb.AppendLine("						if ((fieldLength > 0) && (v.Length > fieldLength)) v = v.Substring(0, fieldLength);");
                            //sb.AppendLine("					}");
                            //sb.AppendLine("					this." + column.PascalName + " = v;");
                            //sb.AppendLine("				}");
                            //sb.AppendLine("				return;");
                            #endregion
                        }
                        else if (column.DataType == System.Data.SqlDbType.Float)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (ModelHelper.IsDateType(column.DataType))
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.Bit)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.Int)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.BigInt)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else
                        {
                            //All other types
                            sb.AppendLine("				if (newValue == null)");
                            sb.AppendLine("				{");
                            if (column.AllowNull)
                                sb.AppendLine("					this." + column.PascalName + " = null;");
                            else
                                sb.AppendLine("					throw new Exception(\"Field '" + column.PascalName + "' does not allow null values!\");");
                            sb.AppendLine("				}");
                            sb.AppendLine("				else");
                            sb.AppendLine("				{");
                            var relationParentTable = (Table)column.ParentTableRef.Object;
                            var list = relationParentTable.AllRelationships.FindByChildColumn(column).ToList();
                            if (list.Count > 0)
                            {
                                var relation = list.First() as Relation;
                                var pTable = relation.ParentTableRef.Object as Table;
                                if (pTable.Generated && pTable.TypedTable != TypedTableConstants.EnumOnly)
                                {
                                    var cTable = relation.ChildTableRef.Object as Table;
                                    var s = pTable.PascalName;
                                    sb.AppendLine("					if (newValue is " + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + ")");
                                    sb.AppendLine("					{");
                                    if (column.EnumType == string.Empty)
                                    {
                                        var columnRelationship = relation.ColumnRelationships.GetByParentField(column);
                                        var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
                                        sb.AppendLine("						this." + column.PascalName + " = ((" + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + ")newValue)." + parentColumn.PascalName + ";");

                                        //REMOVE PK FOR NOW
                                        //sb.AppendLine("					}");
                                        //sb.AppendLine("					else if (newValue is nHydrate.EFCore.DataAccess.IPrimaryKey)");
                                        //sb.AppendLine("					{");
                                        //sb.AppendLine("						this." + column.PascalName + " = ((" + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + "PrimaryKey)newValue)." + parentColumn.PascalName + ";");
                                    }
                                    else //This is an Enumeration type
                                        sb.AppendLine("						throw new Exception(\"Field '" + column.PascalName + "' does not allow values of this type!\");");

                                    sb.AppendLine("					} else");
                                }

                            }
                            if (column.AllowStringParse)
                            {
                                sb.AppendLine("					if (newValue is string)");
                                sb.AppendLine("					{");
                                if (column.EnumType == "")
                                {
                                    sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse((string)newValue);");
                                    sb.AppendLine("					} else if (!(newValue is " + column.GetCodeType() + ")) {");
                                    if (column.GetCodeType() == "int")
                                        sb.AppendLine("						this." + column.PascalName + " = Convert.ToInt32(newValue);");
                                    else
                                        sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse(newValue.ToString());");
                                }
                                else //This is an Enumeration type
                                {
                                    sb.AppendLine("						this." + column.PascalName + " = (" + column.EnumType + ")Enum.Parse(typeof(" + column.EnumType + "), newValue.ToString());");
                                }
                                sb.AppendLine("					}");
                                sb.AppendLine("					else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
                                sb.AppendLine("					{");
                                sb.AppendLine("						throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
                                sb.AppendLine("					}");
                                sb.AppendLine("					else");
                            }

                            if (column.GetCodeType() == "string")
                                sb.AppendLine("					this." + column.PascalName + " = newValue.ToString();");
                            else
                                sb.AppendLine("					this." + column.PascalName + " = (" + column.GetCodeType() + ")newValue;");

                            sb.AppendLine("				}");
                            //sb.AppendLine("				return;");
                        } //All non-string types

                    }
                    sb.AppendLine("			}");
                    count++;
                }

            }

            sb.AppendLine("			else");
            sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIEquatable()
        {
            sb.AppendLine("		#region Equals");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares two objects of '" + _currentTable.PascalName + "' type and determines if all properties match");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>True if all properties match, false otherwise</returns>");
            sb.AppendLine("		bool System.IEquatable<" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + ">.Equals(" + this.InterfaceAssemblyNamespace + ".Entity.I" + _currentTable.PascalName + " other)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.Equals(other);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares two objects of '" + _currentTable.PascalName + "' type and determines if all properties match");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>True if all properties match, false otherwise</returns>");
            sb.AppendLine("		public override bool Equals(object obj)");
            sb.AppendLine("		{");
            sb.AppendLine("			var other = obj as " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ";");
            sb.AppendLine("			if (other == null) return false;");
            sb.AppendLine("			return (");

            var allColumns = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated).OrderBy(x => x.Name).ToList();
            var index = 0;
            foreach (var column in allColumns)
            {
                sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
                if (index < allColumns.Count - 1) sb.Append(" &&");
                sb.AppendLine();
                index++;
            }

            sb.AppendLine("				);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Serves as a hash function for this type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override int GetHashCode()");
            sb.AppendLine("		{");
            sb.AppendLine("			return base.GetHashCode();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIDataErrorInfo()
        {
            sb.AppendLine("		#region IDataErrorInfo");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		string System.ComponentModel.IDataErrorInfo.Error");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.GetObjectDataErrorInfo(); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		/// <param name=\"columnName\"></param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		string System.ComponentModel.IDataErrorInfo.this[string columnName]");
            sb.AppendLine("		{");
            sb.AppendLine("			get");
            sb.AppendLine("			{");
            sb.AppendLine("				if (string.IsNullOrEmpty(columnName))");
            sb.AppendLine("					return string.Empty;");
            sb.AppendLine();
            sb.AppendLine("				var retval = GetObjectPropertyDataErrorInfo(columnName);");
            sb.AppendLine("				if (string.IsNullOrEmpty(retval))");
            sb.AppendLine("				{");

            //Validate all required fields
            {
                var cList = _currentTable.GeneratedColumns.Where(x => !x.AllowNull && x.IsTextType).ToList();
                if (cList.Count > 0)
                {
                    sb.AppendLine("					switch (columnName.ToLower())");
                    sb.AppendLine("					{");
                    foreach (var column in cList)
                    {
                        sb.AppendLine("						case \"" + column.PascalName.ToLower() + "\":");
                        sb.AppendLine("							if (string.IsNullOrEmpty(this." + column.PascalName + ") || string.IsNullOrEmpty(this." + column.PascalName + ".Trim()))");
                        sb.AppendLine("								retval = \"" + column.GetFriendlyName() + " is required!\";");
                        sb.AppendLine("							break;");
                    }
                    sb.AppendLine("						default:");
                    sb.AppendLine("							break;");
                    sb.AppendLine("					}");
                    sb.AppendLine();
                }
            }

            //Validate all regular expression fields
            {
                var cList = _currentTable.GeneratedColumns.Where(x => !string.IsNullOrEmpty(x.ValidationExpression) && x.IsTextType).ToList();
                if (cList.Count > 0)
                {
                    sb.AppendLine("					switch (columnName.ToLower())");
                    sb.AppendLine("					{");
                    foreach (var column in cList)
                    {
                        sb.AppendLine("						case \"" + column.PascalName.ToLower() + "\":");
                        sb.AppendLine("							if (!string.IsNullOrEmpty(this." + column.PascalName + ") && !System.Text.RegularExpressions.Regex.IsMatch(" + column.PascalName + ", @\"" + column.ValidationExpression.Replace("\"", @"""""") + "\", System.Text.RegularExpressions.RegexOptions.None))");
                        sb.AppendLine("								retval = \"" + column.GetFriendlyName() + " is not the correct format!\";");
                        sb.AppendLine("							break;");
                    }
                    sb.AppendLine("						default:");
                    sb.AppendLine("							break;");
                    sb.AppendLine("					}");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("				}");
            sb.AppendLine("				return retval;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendContextReference()
        {
            sb.AppendLine("		#region Context");
            sb.AppendLine();
            sb.AppendLine("		" + _model.ProjectName + "Entities IEntityWithContext.Context");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _internalContext; }");
            sb.AppendLine("			set { _internalContext = value; }");
            sb.AppendLine("		}");
            sb.AppendLine("		private " + _model.ProjectName + "Entities _internalContext = null;");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIAuditable()
        {
            if (!_currentTable.AllowCreateAudit && !_currentTable.AllowModifiedAudit && !_currentTable.AllowTimestamp)
                return;

            sb.AppendLine("		#region Auditing");
            sb.AppendLine("		string nHydrate.EFCore.DataAccess.IAuditable.CreatedBy");
            sb.AppendLine("		{");
            if (_currentTable.AllowCreateAudit)
                sb.AppendLine("			get { return this." + _model.Database.CreatedByPascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.DateTime? nHydrate.EFCore.DataAccess.IAuditable.CreatedDate");
            sb.AppendLine("		{");
            if (_currentTable.AllowCreateAudit)
                sb.AppendLine("			get { return this." + _model.Database.CreatedDatePascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsCreateAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_currentTable.AllowCreateAudit ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsModifyAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_currentTable.AllowModifiedAudit ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsTimestampAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_currentTable.AllowTimestamp ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		string nHydrate.EFCore.DataAccess.IAuditable.ModifiedBy");
            sb.AppendLine("		{");
            if (_currentTable.AllowModifiedAudit)
                sb.AppendLine("			get { return this." + _model.Database.ModifiedByPascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.DateTime? nHydrate.EFCore.DataAccess.IAuditable.ModifiedDate");
            sb.AppendLine("		{");
            if (_currentTable.AllowModifiedAudit)
                sb.AppendLine("			get { return this." + _model.Database.ModifiedDatePascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		byte[] nHydrate.EFCore.DataAccess.IAuditable.TimeStamp");
            sb.AppendLine("		{");
            if (_currentTable.AllowTimestamp)
                sb.AppendLine("			get { return this." + _model.Database.TimestampPascalName + "; }");
            else
                sb.AppendLine("			get { return new byte[0]; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            var auditMethodModifier = "virtual";
            if (_currentTable.ParentTable != null)
                auditMethodModifier = "override";

            sb.AppendLine("		internal " + auditMethodModifier + " void ResetModifiedBy(string modifier)");
            sb.AppendLine("		{");
            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("			if (this." + _model.Database.ModifiedByPascalName + " != modifier)");
                sb.AppendLine("				this." + _model.Database.ModifiedByPascalName + " = modifier;");
            }
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal " + auditMethodModifier + " void ResetCreatedBy(string modifier)");
            sb.AppendLine("		{");
            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("			if (this." + _model.Database.CreatedByPascalName + " != modifier)");
                sb.AppendLine("				this." + _model.Database.CreatedByPascalName + " = modifier;");
            }
            sb.AppendLine("			this.ResetModifiedBy(modifier);");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendAuditQuery()
        {
            if (!_currentTable.AllowAuditTracking) return;

            sb.AppendLine("		#region GetAuditRecords");
            sb.AppendLine();

            var modifier = "virtual";
            if (_currentTable.GetTableHierarchy().Count(x => x != _currentTable && x.AllowAuditTracking) != 0)
                modifier = "new";

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " IEnumerable<" + this.InterfaceAssemblyNamespace + ".Audit.I" + _currentTable.PascalName + "Audit> GetAuditRecords()");
            sb.AppendLine("		{");

            sb.AppendLine("			if (this._internalContext == null)");
            sb.AppendLine("				throw new Exception(\"The entity is not attached to a valid context. Audit records cannot be loaded.\");");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _currentTable.PascalName + "Audit.GetAuditRecords(");

            foreach (var column in _currentTable.PrimaryKeyColumns)
            {
                sb.Append("this." + column.PascalName + ", ");
            }
            sb.AppendLine("this._internalContext.ConnectionString);");

            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"pageOffset\">The page offset needed for pagination starting from page 1</param>");
            sb.AppendLine("		/// <param name=\"recordsPerPage\">The number of records to be returned on a page.</param>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " nHydrate.EFCore.DataAccess.AuditPaging<" + this.InterfaceAssemblyNamespace + ".Audit.I" + _currentTable.PascalName + "Audit> GetAuditRecords(int pageOffset, int recordsPerPage)");
            sb.AppendLine("		{");

            sb.AppendLine("			if (this._internalContext == null)");
            sb.AppendLine("				throw new Exception(\"The entity is not attached to a valid context. Audit records cannot be loaded.\");");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _currentTable.PascalName + "Audit.GetAuditRecords(pageOffset, recordsPerPage, null, null, ");

            foreach (var column in _currentTable.PrimaryKeyColumns)
            {
                sb.Append("this." + column.PascalName + ", ");
            }
            sb.AppendLine("this._internalContext.ConnectionString);");

            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"pageOffset\">The page offset needed for pagination starting from page 1</param>");
            sb.AppendLine("		/// <param name=\"recordsPerPage\">The number of records to be returned on a page.</param>");
            sb.AppendLine("		/// <param name=\"startDate\">The starting date used when searching for records.</param>");
            sb.AppendLine("		/// <param name=\"endDate\">The ending date used when searching for records.</param>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " nHydrate.EFCore.DataAccess.AuditPaging<" + this.GetLocalNamespace() + ".Interfaces.Audit.I" + _currentTable.PascalName + "Audit> GetAuditRecords(int pageOffset, int recordsPerPage, DateTime? startDate, DateTime? endDate)");
            sb.AppendLine("		{");

            sb.AppendLine("			if (this._internalContext == null)");
            sb.AppendLine("				throw new Exception(\"The entity is not attached to a valid context. Audit records cannot be loaded.\");");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _currentTable.PascalName + "Audit.GetAuditRecords(pageOffset, recordsPerPage, startDate, endDate, ");

            foreach (var column in _currentTable.PrimaryKeyColumns)
            {
                sb.Append("this." + column.PascalName + ", ");
            }
            sb.AppendLine("this._internalContext.ConnectionString);");

            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void GenerateAuditField(string columnName, string codeType, string description, string propertyScope, bool isConcurrency = false)
        {
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }
            sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
            sb.AppendLine("		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]");
            sb.AppendLine("		[DataMemberAttribute()]");
            sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");

            if (isConcurrency)
                sb.AppendLine("		[System.ComponentModel.DataAnnotations.ConcurrencyCheck()]");

            sb.AppendLine("		" + propertyScope + " virtual " + codeType + " " + columnName);
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _" + StringHelper.DatabaseNameToCamelCase(columnName) + "; }");
            if (propertyScope == "public")
            {
                //OLD CODE - we tried to hide the setter but serialization hates this
                sb.AppendLine("			protected internal set");
                //sb.AppendLine("			set");
            }
            else
            {
                sb.AppendLine("			set");
            }
            sb.AppendLine("			{");

            //Cannot hide setter but gut the thing so cannot make changes
            if (_currentTable.Immutable)
            {
                sb.AppendLine("				//Setter is left for deserialization but should never be used");
            }
            else
            {
                //sb.AppendLine("				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + ">(value, \"" + columnName + "\");");
                //if (_model.EnableCustomChangeEvents)
                //    sb.AppendLine("				On" + columnName + "Changing(eventArg);");
                //else
                //    sb.AppendLine("				//On" + columnName + "Changing(eventArg);");
                //sb.AppendLine("				if (eventArg.Cancel) return;");
                //sb.AppendLine("				ReportPropertyChanging(\"" + columnName + "\");");
                //sb.AppendLine("				this.OnPropertyChanging(\"" + columnName + "\");");
                //sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = eventArg.Value;");
                //sb.AppendLine("				ReportPropertyChanged(\"" + columnName + "\");");
                //sb.AppendLine("				this.OnPropertyChanged(\"" + columnName + "\");");
                //if (_model.EnableCustomChangeEvents)
                //    sb.AppendLine("				On" + columnName + "Changed(eventArg);");
                //else
                //    sb.AppendLine("				//On" + columnName + "Changed(eventArg);");
                
                sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = value;");
            }

            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected " + codeType + " _" + StringHelper.DatabaseNameToCamelCase(columnName) + ";");
            sb.AppendLine();

        }

        private void AppendDeleteDataScaler()
        {
            if (_currentTable.Immutable) return;

            sb.AppendLine("		#region DeleteData");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where, new nHydrate.EFCore.DataAccess.QueryOptimizer(), new ContextStartup(null), " + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities.GetConnectionString());");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"optimizer\">The optimization object to use for running queries</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, nHydrate.EFCore.DataAccess.QueryOptimizer optimizer)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where, optimizer, new ContextStartup(null), " + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities.GetConnectionString());");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where, new nHydrate.EFCore.DataAccess.QueryOptimizer(), new ContextStartup(null), connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"optimizer\">The optimization object to use for running queries</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, nHydrate.EFCore.DataAccess.QueryOptimizer optimizer, ContextStartup startup, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (optimizer == null)");
            sb.AppendLine("				optimizer = new nHydrate.EFCore.DataAccess.QueryOptimizer();");
            sb.AppendLine("				if (startup == null) startup = new ContextStartup(null);");
            sb.AppendLine();
            sb.AppendLine("			using (var connection = " + this.GetLocalNamespace() + ".DBHelper.GetConnection(" + this.GetLocalNamespace() + ".Util.StripEFCS2Normal(connectionString)))");
            sb.AppendLine("			{");
            sb.AppendLine("				using (var dc = new DataContext(connection))");
            sb.AppendLine("				{");
            sb.AppendLine("					var template = dc.GetTable<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query>();");
            sb.AppendLine("					using (var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query>(dc, template, where))");
            sb.AppendLine("					{");
            sb.AppendLine("						if (startup.CommandTimeout != null && startup.CommandTimeout > 0) cmd.CommandTimeout = startup.CommandTimeout.Value;");
            sb.AppendLine("						else cmd.CommandTimeout = 30;");
            sb.AppendLine();
            sb.AppendLine("						var parser = LinqSQLParser.Create(cmd.CommandText, LinqSQLParser.ObjectTypeConstants.Table);");
            sb.Append("						var sql = \"CREATE TABLE #t (");

            var ii = 0;
            foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                sb.Append("[" + column.DatabaseName + "] " + column.DatabaseType);
                if (ii < _currentTable.PrimaryKeyColumns.Count - 1) sb.Append(", ");
                ii++;
            }

            sb.AppendLine(")\";");
            sb.Append("						sql += \"INSERT INTO #t (");

            ii = 0;
            foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                sb.Append("[" + column.DatabaseName + "]");
                if (ii < _currentTable.PrimaryKeyColumns.Count - 1) sb.Append(", ");
                ii++;
            }

            sb.AppendLine(")\";");

            sb.Append("						sql += \"SELECT ");

            ii = 0;
            foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                sb.Append("[t0].[" + column.DatabaseName + "]");
                if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
                    sb.Append(", ");
                ii++;
            }
            sb.AppendLine(" #t\\r\\n\";");
            sb.AppendLine("						sql += parser.GetFromClause(optimizer) + \"\\r\\n\";");
            sb.AppendLine("						sql += parser.GetWhereClause();");
            sb.AppendLine("						sql += \"\\r\\n\";");
            sb.AppendLine();

            var tableList = new List<Table>(_currentTable.GetTableHierarchy());
            tableList.Reverse();
            sb.AppendLine("						var noLock = string.Empty;");
            foreach (var table in tableList)
            {
                sb.AppendLine("						noLock = (optimizer.NoLocking ? \"WITH (NOLOCK) \" : string.Empty);");
                sb.Append("						sql += \"DELETE [" + table.DatabaseName + "] FROM [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] \" + noLock + \"INNER JOIN #t ON ");

                ii = 0;
                foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    sb.Append("[" + table.GetSQLSchema() + "].[" + table.DatabaseName + "].[" + column.DatabaseName + "] = #t.[" + column.DatabaseName + "]");
                    if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
                        sb.Append(" AND ");
                    ii++;
                }
                sb.AppendLine("\\r\\n\";");
            }

            sb.AppendLine("						sql += \";select @@rowcount\";");
            sb.AppendLine("						if (startup.IsAdmin) sql = LinqSQLParser.RemapTenantToAdminSql(sql);");
            sb.AppendLine("						sql = \"set ansi_nulls off;\" + sql;");
            sb.AppendLine("						cmd.CommandText = sql;");
            sb.AppendLine("						dc.Connection.Open();");
            sb.AppendLine("						var startTime = DateTime.Now;");
            sb.AppendLine("						object p = cmd.ExecuteScalar();");
            sb.AppendLine("						var endTime = DateTime.Now;");
            sb.AppendLine("						optimizer.TotalMilliseconds = (long)endTime.Subtract(startTime).TotalMilliseconds;");
            sb.AppendLine("						dc.Connection.Close();");
            sb.AppendLine("						return (int)p;");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendUpdateDataScaler()
        {
            if (_currentTable.Immutable) return;

            sb.AppendLine("		#region UpdateData");
            sb.AppendLine();

            //All types that are valid for these operations
            var typeList = new List<string>();
            typeList.Add("int");
            typeList.Add("Single");
            typeList.Add("decimal");
            typeList.Add("double");
            typeList.Add("short");
            typeList.Add("string");
            typeList.Add("DateTime");
            typeList.Add("bool");
            typeList.Add("Guid");
            //typeList.Add("byte[]");

            var tableList = new List<Table>(_currentTable.GetTableHierarchy());
            tableList.Remove(_currentTable);
            var columnList = new Dictionary<string, Column>();

            //Need the columns in order from base UP
            foreach (var t in tableList.OrderBy(x => x.Name))
            {
                foreach (var c in t.GetColumns().Where(x => x.Generated))
                {
                    if (!columnList.ContainsKey(c.DatabaseName.ToLower()))
                        columnList.Add(c.DatabaseName.ToLower(), c);
                }
            }

            //Add primary Keys
            foreach (var c in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                if (columnList.ContainsKey(c.DatabaseName.ToLower()))
                    columnList.Remove(c.DatabaseName.ToLower());
            }

            foreach (var typeName in typeList)
            {
                //var nullTypeName = typeName + (typeName == "string" || typeName == "byte[]" ? string.Empty : "?");
                var nullTypeName = typeName + (typeName == "string" ? string.Empty : "?");

                #region Normal Call

                //The NON-nullable call
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                sb.AppendLine("		/// <returns>The number of records affected</returns>");
                sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + typeName + " newValue)");
                sb.AppendLine("		{");
                sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ");");
                sb.AppendLine("		}");
                sb.AppendLine();

                //The Nullable call
                if (nullTypeName != typeName)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                    sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                    sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                    sb.AppendLine("		/// <returns>The number of records affected</returns>");
                    sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + nullTypeName + " newValue)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ");");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                }

                #endregion

                #region Overload for connection string

                //The NON-nullable call
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
                sb.AppendLine("		/// <returns>The number of records affected</returns>");
                sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + typeName + " newValue, string connectionString)");
                sb.AppendLine("		{");
                sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", connectionString);");
                sb.AppendLine("		}");
                sb.AppendLine();

                //Add an overload for startup object if there are tenant tables
                if (_model.Database.Tables.Any(x => x.Generated && x.IsTenant))
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                    sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                    sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                    sb.AppendLine("		/// <param name=\"startup\">A configuration object</param>");
                    sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
                    sb.AppendLine("		/// <returns>The number of records affected</returns>");
                    sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + typeName + " newValue, ContextStartup startup, string connectionString)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", startup, connectionString);");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                }

                //The Nullable call
                if (nullTypeName != typeName)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                    sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                    sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                    sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
                    sb.AppendLine("		/// <returns>The number of records affected</returns>");
                    sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + nullTypeName + " newValue, string connectionString)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", connectionString);");
                    sb.AppendLine("		}");
                    sb.AppendLine();

                    //Add an overload for startup object if there are tenant tables
                    if (_model.Database.Tables.Any(x => x.Generated && x.IsTenant))
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                        sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                        sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                        sb.AppendLine("		/// <param name=\"startup\">A configuration object</param>");
                        sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
                        sb.AppendLine("		/// <returns>The number of records affected</returns>");
                        sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + nullTypeName + " newValue, ContextStartup startup, string connectionString)");
                        sb.AppendLine("		{");
                        sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", startup, connectionString);");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }

                #endregion

                #region Overload for connection object

                //The NON-nullable call
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                sb.AppendLine("		/// <param name=\"connection\">An open database connection</param>");
                sb.AppendLine("		/// <param name=\"transaction\">The database connection transaction</param>");
                sb.AppendLine("		/// <returns>The number of records affected</returns>");
                sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + typeName + " newValue, System.Data.IDbConnection connection, System.Data.Common.DbTransaction transaction)");
                sb.AppendLine("		{");
                sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + typeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", null, connection, transaction);");
                sb.AppendLine("		}");
                sb.AppendLine();

                //The Nullable call
                if (nullTypeName != typeName)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
                    sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
                    sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
                    sb.AppendLine("		/// <param name=\"connection\">An open database connection</param>");
                    sb.AppendLine("		/// <param name=\"transaction\">The database connection transaction</param>");
                    sb.AppendLine("		/// <returns>The number of records affected</returns>");
                    sb.AppendLine("		public static int UpdateData(Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">> select, Expression<Func<" + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, bool>> where, " + nullTypeName + " newValue, System.Data.IDbConnection connection, System.Data.Common.DbTransaction transaction)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ", " + this.GetLocalNamespace() + "." + _currentTable.PascalName + "Query, " + nullTypeName + ">.UpdateData(select, where, newValue, \"" + _currentTable.DatabaseName + "\", GetDatabaseFieldName, " + _currentTable.AllowModifiedAudit.ToString().ToLower() + ", null, connection, transaction);");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                }

                #endregion

            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendRegionGetDatabaseFieldName()
        {
            sb.AppendLine("		#region GetDatabaseFieldName");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetDatabaseFieldName(" + _currentTable.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetDatabaseFieldName(field.ToString());");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetDatabaseFieldName(string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _currentTable.GeneratedColumns)
            {
                if (column.Generated)
                    sb.AppendLine("				case \"" + column.PascalName + "\": return \"" + column.Name + "\";");
            }

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedByPascalName + "\": return \"" + _model.Database.CreatedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedDatePascalName + "\": return \"" + _model.Database.CreatedDateColumnName + "\";");
            }

            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedByPascalName + "\": return \"" + _model.Database.ModifiedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedDatePascalName + "\": return \"" + _model.Database.ModifiedDateColumnName + "\";");
            }

            if (_currentTable.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampPascalName + "\": return \"" + _model.Database.TimestampColumnName + "\";");
            }

            sb.AppendLine("			}");
            sb.AppendLine("			return string.Empty;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendStaticSQLHelpers()
        {
            sb.AppendLine("		#region Static SQL Methods");
            sb.AppendLine();

            var allColumns = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated).ToList();

            #region GetFieldAliasFromFieldNameSqlMapping
            sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetFieldAliasFromFieldNameSqlMapping(string alias)");
            sb.AppendLine("		{");
            sb.AppendLine("			alias = alias.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			switch (alias.ToLower())");
            sb.AppendLine("			{");
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                sb.AppendLine("				case \"" + column.DatabaseName.ToLower() + "\": return \"" + column.PascalName.ToLower() + "\";");
            }
            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedDateColumnName.ToLower() + "\": return \"" + _model.Database.CreatedDatePascalName.ToLower() + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedByColumnName.ToLower() + "\": return \"" + _model.Database.CreatedByPascalName.ToLower() + "\";");
            }
            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedDateColumnName.ToLower() + "\": return \"" + _model.Database.ModifiedDatePascalName.ToLower() + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedByColumnName.ToLower() + "\": return \"" + _model.Database.ModifiedByPascalName.ToLower() + "\";");
            }
            if (_currentTable.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampColumnName.ToLower() + "\": return \"" + _model.Database.TimestampPascalName.ToLower() + "\";");
            }
            sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region GetTableFromFieldAliasSqlMapping
            sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetTableFromFieldAliasSqlMapping(string alias)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (alias.ToLower())");
            sb.AppendLine("			{");

            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var table = column.ParentTableRef.Object as Table;
                if (table.IsTenant)
                    sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": return \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\";");
                else
                    sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": return \"" + table.DatabaseName + "\";");
            }

            var tableName = _currentTable.DatabaseName;
            if (_currentTable.IsTenant)
                tableName = _model.TenantPrefix + "_" + _currentTable.DatabaseName;

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedByPascalName.ToLower() + "\": return \"" + tableName + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedDatePascalName.ToLower() + "\": return \"" + tableName + "\";");
            }
            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedByPascalName.ToLower() + "\": return \"" + tableName + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedDatePascalName.ToLower() + "\": return \"" + tableName + "\";");
            }
            if (_currentTable.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampPascalName.ToLower() + "\": return \"" + tableName + "\";");
            }

            sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region GetTableFromFieldNameSqlMapping
            sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetTableFromFieldNameSqlMapping(string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field.ToLower())");
            sb.AppendLine("			{");

            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var table = column.ParentTableRef.Object as Table;
                if (table.IsTenant)
                    sb.AppendLine("				case \"" + column.DatabaseName.ToLower() + "\": return \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\";");
                else
                    sb.AppendLine("				case \"" + column.DatabaseName.ToLower() + "\": return \"" + table.DatabaseName + "\";");
            }

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedByColumnName.ToLower() + "\": return \"" + tableName + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedDateColumnName.ToLower() + "\": return \"" + tableName + "\";");
            }
            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedByColumnName.ToLower() + "\": return \"" + tableName + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedDateColumnName.ToLower() + "\": return \"" + tableName + "\";");
            }
            if (_currentTable.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampColumnName.ToLower() + "\": return \"" + tableName + "\";");
            }

            sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region GetRemappedLinqSql
            sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetRemappedLinqSql(string sql, string parentAlias, LinqSQLFromClauseCollection childTables)");
            sb.AppendLine("		{");
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + column.DatabaseName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\") + \"].[" + column.DatabaseName.ToLower() + "]\");");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + column.DatabaseName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\") + \"].[" + column.DatabaseName.ToLower() + "]\", RegexOptions.IgnoreCase);");
            }
            if (_currentTable.AllowCreateAudit)
            {
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + _model.Database.CreatedByPascalName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.CreatedByPascalName.ToLower() + "]\");");
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + _model.Database.CreatedDatePascalName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.CreatedDatePascalName.ToLower() + "]\");");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + _model.Database.CreatedByPascalName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.CreatedByPascalName.ToLower() + "]\", RegexOptions.IgnoreCase);");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + _model.Database.CreatedDatePascalName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.CreatedDatePascalName.ToLower() + "]\", RegexOptions.IgnoreCase);");
            }
            if (_currentTable.AllowModifiedAudit)
            {
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + _model.Database.ModifiedByPascalName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.ModifiedByPascalName.ToLower() + "]\");");
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + _model.Database.ModifiedDatePascalName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.ModifiedDatePascalName.ToLower() + "]\");");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + _model.Database.ModifiedByPascalName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.ModifiedByPascalName.ToLower() + "]\", RegexOptions.IgnoreCase);");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + _model.Database.ModifiedDatePascalName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.ModifiedDatePascalName.ToLower() + "]\", RegexOptions.IgnoreCase);");
            }
            if (_currentTable.AllowTimestamp)
            {
                //sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + _model.Database.TimestampPascalName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.TimestampPascalName.ToLower() + "]\");");
                sb.AppendLine("			sql = System.Text.RegularExpressions.Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + _model.Database.TimestampPascalName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + _model.Database.TimestampPascalName.ToLower() + "]\", RegexOptions.IgnoreCase);");
            }

            sb.AppendLine("			return sql;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        #endregion

    }
}