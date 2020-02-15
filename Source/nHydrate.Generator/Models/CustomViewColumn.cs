#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomViewColumn : ColumnBase, ICodeFacadeObject
    {
        #region Member Variables

        protected const int _def_sortOrder = 0;
        protected const bool _def_UIVisible = false;
        protected const string _def_codefacade = "";
        protected const string _def_default = "";
        protected const string _def_friendlyName = "";
        protected const bool _def_isPrimaryKey = false;

        #endregion

        #region Constructor

        public CustomViewColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public CustomViewColumn()
            : base(null)
        {

        }

        #endregion

        #region Property Implementations

        public bool IsPrimaryKey { get; set; } = _def_isPrimaryKey;

        public Reference RelationshipRef { get; set; } = null;

        public string Default { get; set; } = _def_default;

        public Reference ParentViewRef { get; set; } = null;

        public string FriendlyName { get; set; } = _def_friendlyName;

        public int SortOrder { get; set; } = _def_sortOrder;

        public bool UiVisible { get; set; } = _def_UIVisible;

        internal string EnumType { get; set; } = string.Empty;

        public override string DatabaseType
        {
            get
            {
                var retval = this.DataType.ToString();
                switch (this.DataType)
                {
                    case System.Data.SqlDbType.Char:
                    case System.Data.SqlDbType.NChar:
                    case System.Data.SqlDbType.NVarChar:
                    case System.Data.SqlDbType.VarChar:
                        retval += "(" + this.Length + ")"; ;
                        break;
                }
                return retval;
            }
        }

        #endregion

        #region Methods

        public virtual string GetFriendlyName()
        {
            if (string.IsNullOrEmpty(this.FriendlyName))
                return this.PascalName;
            else
                return this.FriendlyName;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute(node, "generated", this.Generated);

                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.FriendlyName != _def_friendlyName)
                    XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

                if (this.UiVisible != _def_UIVisible)
                    XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UiVisible);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

                if (this.IsPrimaryKey != _def_isPrimaryKey)
                    XmlHelper.AddAttribute(node, "isPrimaryKey", this.IsPrimaryKey);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute(node, "default", this.Default);

                if (this.Length != _def_length)
                    XmlHelper.AddAttribute(node, "length", this.Length);

                if (this.Scale != _def_scale)
                    XmlHelper.AddAttribute(node, "scale", this.Scale);

                XmlHelper.AddAttribute(node, "id", this.Id);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "sortOrder", this.SortOrder);

                var parentViewRefNode = oDoc.CreateElement("parentTableRef");
                ParentViewRef.XmlAppend(parentViewRefNode);
                node.AppendChild(parentViewRefNode);

                XmlHelper.AddAttribute(node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

                if (this.IsBrowsable != _def_isBrowsable)
                    XmlHelper.AddAttribute(node, "isBrowsable", this.IsBrowsable);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
                this.UiVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                this.IsPrimaryKey = XmlHelper.GetAttributeValue(node, "isPrimaryKey", _def_isPrimaryKey);

                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
                this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
                this.Scale = XmlHelper.GetAttributeValue(node, "scale", _scale);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                var parentViewRefNode = node.SelectSingleNode("parentTableRef");
                ParentViewRef = new Reference(this.Root);
                if (parentViewRefNode != null)
                    ParentViewRef.XmlLoad(parentViewRefNode);

                var typeString = node.Attributes["type"].Value;
                if (!string.IsNullOrEmpty(typeString))
                    _dataType = (System.Data.SqlDbType)int.Parse(typeString);

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
                this.IsBrowsable = XmlHelper.GetAttributeValue(node, "isBrowsable", _def_isBrowsable);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Helpers

        public override Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public override Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.CustomViewColumn;
            return returnVal;
        }

        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
            }
        }

        public override string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
                if (allowNullable && (this.AllowNull || forceNull))
                    retval += "?";

                return retval;
            }
            else
            {
                return base.GetCodeType(allowNullable, forceNull);
            }
        }

        #endregion

        public override string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #region ICodeFacadeObject Members

        public string CodeFacade { get; set; } = _def_codefacade;

        public string GetCodeFacade()
        {
            if (this.CodeFacade == "")
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}