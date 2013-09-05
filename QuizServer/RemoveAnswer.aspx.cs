using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace QuizServer
{
    public partial class RemoveAnswer : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id)) 
            {
                //XDocument x = new XDocument();
                //x.Elements().Where(xx => xx.Attribute("id").Value == id).Remove();
                if (sg.path == null)
                    sg.updateSingleton(Request.PhysicalApplicationPath);
                XmlDocument xmlAnswers = sg.xmlAnswers;
                foreach (XmlElement answer in xmlAnswers.DocumentElement.ChildNodes)
                {
                    if (answer.GetAttribute("id") != null) 
                    {
                        if (answer.GetAttribute("id").Equals(id)) 
                        {
                            xmlAnswers.DocumentElement.RemoveChild(answer);
                            sg.answers = XmlStringConverter.convertXmlToString(xmlAnswers);
                            return;
                        }
                    }
                }
            }            
        }
    }
}