//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PROJECTNAMESPACE
{
    /// <summary>
    /// A helper class to manage XML documents
    /// </summary>
    internal static class XmlHelper
    {

        #region GetNode

        /// <summary>
        /// Gets a node from an XPath query
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static System.Xml.XmlNode GetNode(this System.Xml.XmlNode xmlNode, string xpath)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = xmlNode.SelectSingleNode(xpath);
                return node;
            }
            catch { throw; }
        }

        /// <summary>
        /// Gets a node from an XPath query
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xpath"></param>
        /// <param name="nsManager"></param>
        /// <returns></returns>
        public static System.Xml.XmlNode GetNode(this System.Xml.XmlNode xmlNode, string xpath, XmlNamespaceManager nsManager)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = xmlNode.SelectSingleNode(xpath, nsManager);
                return node;
            }
            catch { throw; }
        }

        #endregion

        #region GetNodeValue

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeValue(this System.Xml.XmlDocument document, string xpath, string defaultValue)
        {
            try
            {
                return GetNodeValue(document.DocumentElement, xpath, defaultValue);
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeValue(this System.Xml.XmlNode element, string xpath, string defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else return node.InnerText;
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetNodeValue(this System.Xml.XmlNode element, string xpath, int defaultValue)
        {
            var b = element.GetNodeValue(xpath, (int?)defaultValue);
            if (b == null) return defaultValue;
            else return b.Value;
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetNodeValue(this System.Xml.XmlNode element, string xpath, int? defaultValue)
        {
            try
            {
                var node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else
                {
                    var v = node.InnerText.ToLower();
                    int newv;
                    if (int.TryParse(v, out newv))
                        return newv;
                    return defaultValue;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single GetNodeValue(this System.Xml.XmlNode element, string xpath, Single defaultValue)
        {
            var b = element.GetNodeValue(xpath, (Single?)defaultValue);
            if (b == null) return defaultValue;
            else return b.Value;
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single? GetNodeValue(this System.Xml.XmlNode element, string xpath, Single? defaultValue)
        {
            try
            {
                var node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else
                {
                    var v = node.InnerText.ToLower();
                    Single newv;
                    if (Single.TryParse(v, out newv))
                        return newv;
                    return defaultValue;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetNodeValue(this System.Xml.XmlNode element, string xpath, double defaultValue)
        {
            var b = element.GetNodeValue(xpath, (double?)defaultValue);
            if (b == null) return defaultValue;
            else return b.Value;
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double? GetNodeValue(this System.Xml.XmlNode element, string xpath, double? defaultValue)
        {
            try
            {
                var node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else
                {
                    var v = node.InnerText.ToLower();
                    double newv;
                    if (double.TryParse(v, out newv))
                        return newv;
                    return defaultValue;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetNodeValue(this System.Xml.XmlNode element, string xpath, bool defaultValue)
        {
            var b = element.GetNodeValue(xpath, (bool?)defaultValue);
            if (b == null) return defaultValue;
            else return b.Value;
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool? GetNodeValue(this System.Xml.XmlNode element, string xpath, bool? defaultValue)
        {
            try
            {
                var node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else
                {
                    var v = node.InnerText.ToLower();
                    bool newv;
                    if (bool.TryParse(v, out newv))
                        return newv;
                    return defaultValue;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetNodeValue(this System.Xml.XmlNode element, string xpath, DateTime defaultValue)
        {
            var b = element.GetNodeValue(xpath, (DateTime?)defaultValue);
            if (b == null) return defaultValue;
            else return b.Value;
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetNodeValue(this System.Xml.XmlNode element, string xpath, DateTime? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(xpath);
                if (node == null) return defaultValue;
                else
                {
                    var v = node.InnerText.ToLower();
                    DateTime newv;
                    if (DateTime.TryParse(v, out newv))
                        return newv;
                    return defaultValue;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xpath"></param>
        /// <param name="nsManager"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeValue(this System.Xml.XmlDocument document, string xpath, XmlNamespaceManager nsManager, string defaultValue)
        {
            return GetNodeValue(document.DocumentElement, xpath, nsManager, defaultValue);
        }

        /// <summary>
        /// Get a node's value based on an XPath query
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xpath"></param>
        /// <param name="nsManager"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeValue(this System.Xml.XmlNode element, string xpath, XmlNamespaceManager nsManager, string defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(xpath, nsManager);
                if (node == null) return defaultValue;
                else return node.InnerText;
            }
            catch { throw; }
        }

        #endregion

        #region GetNodeXML

        /// <summary>
        /// Gets the actual XML of a node
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <param name="useOuter"></param>
        /// <returns></returns>
        public static string GetNodeXML(this XmlDocument document, string xpath, string defaultValue, bool useOuter)
        {
            try
            {
                XmlNode node = null;
                node = document.SelectSingleNode(xpath);
                if (node == null)
                    return defaultValue;
                else if (useOuter)
                    return node.OuterXml;
                else
                    return node.InnerXml;
            }
            catch { throw; }
        }

        /// <summary>
        /// Gets the actual XML of a node
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeXML(this XmlDocument document, string xpath, string defaultValue)
        {
            return GetNodeXML(document, xpath, defaultValue, false);
        }

        #endregion

        #region GetAttributeValue

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode node, string attributeName)
        {
            return GetAttribute(node, attributeName, string.Empty);
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode node, string attributeName, string defaultValue)
        {
            XmlAttribute attr = node.Attributes[attributeName];

            //If we cannot find it by name then look and look case insensitive
            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name.ToLower() == attributeName.ToLower())
                    attr = attribute;
            }

            if (attr == null) return defaultValue;
            else return attr.Value;
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid GetAttribute(this XmlNode node, string attributeName, Guid defaultValue)
        {
            return new Guid(node.GetAttribute(attributeName, defaultValue.ToString()));
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetAttribute(this XmlNode node, string attributeName, double defaultValue)
        {
            return double.Parse(node.GetAttribute(attributeName, defaultValue.ToString()));
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetAttribute(this XmlNode node, string attributeName, int defaultValue)
        {
            return int.Parse(node.GetAttribute(attributeName, defaultValue.ToString()));
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetAttribute(this XmlNode node, string attributeName, long defaultValue)
        {
            return long.Parse(node.GetAttribute(attributeName, defaultValue.ToString()));
        }

        /// <summary>
        /// Get a node attribute value for the specified attribute name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetAttribute(this XmlNode node, string attributeName, bool defaultValue)
        {
            return bool.Parse(node.GetAttribute(attributeName, defaultValue.ToString()));
        }

        #endregion

        #region AddElement

        /// <summary>
        /// Adds a node to the XML tree
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlNode AddElement(this XmlElement element, string name, string value)
        {
            XmlDocument document = null;
            XmlElement newItem = null;

            document = element.OwnerDocument;
            newItem = document.CreateElement(name);
            if (value != null)
            {
                if (value.GetType() == typeof(string))
                {
                    if (!string.IsNullOrEmpty(value))
                        newItem.InnerText = value;
                }
                else
                    newItem.InnerText = value;
            }

            return element.AppendChild(newItem);
        }

        /// <summary>
        /// Adds a node to the XML tree
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlNode AddElement(this XmlDocument document, string name, string value)
        {
            XmlElement newItem = document.CreateElement(name);
            document.AppendChild(newItem);
            newItem.AddElement(name, value);
            return newItem;
        }

        /// <summary>
        /// Adds a node to the XML tree
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlNode AddElement(this XmlElement element, string name)
        {
            return element.AddElement(name, null);
        }

        /// <summary>
        /// Adds a node to the XML tree
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlNode AddElement(this XmlDocument xmlDocument, string name)
        {
            return xmlDocument.AddElement(name, null);
        }

        /// <summary>
        /// Adds an attribute to an XML node
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlAttribute AddAttribute(XmlElement element, string name, string value)
        {
            XmlDocument docOwner = null;
            XmlAttribute newAttribute = null;

            docOwner = element.OwnerDocument;
            newAttribute = docOwner.CreateAttribute(name);
            newAttribute.InnerText = value;
            element.Attributes.Append(newAttribute);
            return newAttribute;
        }

        #endregion

        #region RemoveElement

        /// <summary>
        /// Removes a set of nodes based on an XPath query
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xpath"></param>
        public static void RemoveElement(this XmlDocument document, string xpath)
        {
            XmlNode parentNode = null;
            XmlNodeList nodes = null;

            nodes = document.SelectNodes(xpath);
            foreach (XmlElement node in nodes)
            {
                parentNode = node.ParentNode;
                node.RemoveAll();
                parentNode.RemoveChild(node);
            }
        }

        /// <summary>
        /// Removes an XML node
        /// </summary>
        /// <param name="element"></param>
        public static void RemoveElement(this XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            parentNode.RemoveChild(element);
        }

        /// <summary>
        /// Removes an attribute from a node
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        public static void RemoveAttribute(XmlElement element, string attributeName)
        {
            XmlAttribute attribute = null;
            attribute = (XmlAttribute)element.Attributes.GetNamedItem(attributeName);
            element.Attributes.Remove(attribute);
        }

        #endregion

        #region UpdateElement

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="xpath"></param>
        /// <param name="newValue"></param>
        public static void UpdateElement(this XmlDocument xmlDocument, string xpath, string newValue)
        {
            xmlDocument.SelectSingleNode(xpath).InnerText = newValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="attributeName"></param>
        /// <param name="newValue"></param>
        public static void UpdateAttribute(this XmlElement xmlElement, string attributeName, string newValue)
        {
            XmlAttribute attrTemp = null;
            attrTemp = (XmlAttribute)xmlElement.Attributes.GetNamedItem(attributeName);
            attrTemp.InnerText = newValue;
        }

        #endregion

        #region GetElement

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static XmlElement GetElement(this XmlElement parentElement, string tagName)
        {
            XmlNodeList list = parentElement.GetElementsByTagName(tagName);
            if (list.Count > 0)
                return (XmlElement)list[0];
            else
                return null;
        }

        #endregion

        #region StripToUTF8

        /// <summary>
        /// Strip off all invalid characters for an XML file node or attribute value
        /// </summary>
        /// <param name="text">The text to clean</param>
        public static string EnsureValidXMLValue(string text)
        {
            byte[] validsmallchars = new byte[] { 10, 13 };
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                //Only CR,LF are valid for chars less than space (32)
                if (c < ' ')
                {
                    if (validsmallchars.Contains((byte)c))
                        sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}