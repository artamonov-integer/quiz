using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace QuizServer
{
    public static class XmlStringConverter
    {
        public static string convertXmlToString(XmlDocument xml)
        {
            string str = "";
            try
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter tx = new XmlTextWriter(sw);
                xml.WriteTo(tx);
                str = sw.ToString();
                sw.Close();
                tx.Close();
            }
            catch (IOException e) { }  
            return str;
            
        }

        public static XmlDocument convertStringToXml(string str)
        {
            XmlDocument xml = new XmlDocument();
            StringReader sr = new StringReader(str);
            xml.Load(sr);
            return xml;
        }
    }
}