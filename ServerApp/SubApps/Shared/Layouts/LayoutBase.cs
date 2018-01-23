using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace ServerApp.SubApps.Shared.Layouts
{
    public class LayoutBase : ILayout
    {
        public LayoutBase()
		{
			Name = "ly" + GetType().Name;
		}

        public string GetLayout()
        {
            return GetLayoutInt();
        }

        public static string WordWrap(string text, int MaxLengthOfWord)
        {
            // rozdelenie na viac ako dva riadky
            if (text.Length > MaxLengthOfWord)
            {
                int ind = text.IndexOf(" ");
                if (ind == -1)
                    return text;
                int ind2 = text.IndexOf(" ", ind);
                while (ind2 < MaxLengthOfWord & ind2 != -1)
                {
                    ind = ind2;
                    ind2 = text.IndexOf(" ", ind + 1);
                }
                text = text.Remove(ind, 1).Insert(ind, "\n");
            }
            return text;
        }
        public static string WordCrop(string text, int maxLengthOfText)
        {
            return text.Length <= maxLengthOfText ? text : text.Substring(0, maxLengthOfText - 3).Trim() + "...";
        }

        protected virtual string GetLayoutInt()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            XmlDocument doc = new XmlDocument();
            Type thisType = this.GetType();
            string namespacePrefix = thisType.Namespace;
            string fileName = thisType.Name;
            Stream layoutStream = a.GetManifestResourceStream(GetLayoutName(namespacePrefix, fileName));
            doc.Load(layoutStream);

            return doc.OuterXml;
        }

        protected virtual string GetLayoutName(string namespacePrefix, string fileName)
        {
            return $"{namespacePrefix}.{fileName}.ui";
        }

        public string Name { get; private set; }
    }
}
