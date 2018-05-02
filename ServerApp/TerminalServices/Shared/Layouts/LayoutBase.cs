using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace ServerApp.TerminalServices.Shared.Layouts
{
	// definicia zakladnych prvkov rozlozenia obrazovky
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

		// ziskanie layoutu zo suboru
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

		// unikatny opis layoutu
		protected virtual string GetLayoutName(string namespacePrefix, string fileName)
		{
			return $"{namespacePrefix}.{fileName}.ui";
		}

		public string Name { get; private set; }
	}
}
