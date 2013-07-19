using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace QuizServer
{
    public partial class AddParticipants : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Request.QueryString["n"];
            string mail = Request.QueryString["m"];
            string login = Request.QueryString["l"];
            string points = Request.QueryString["p"];
            string id = System.Guid.NewGuid().ToString();

            int newPoint = -1;
            try
            {
                newPoint = Convert.ToInt32(points); 
            }catch(FormatException ex)
            {
                newPoint = -1;
            }
            if(!string.IsNullOrEmpty(points)){
                XmlDocument doc = sg.xmlParticipants;
                XmlElement param = doc.CreateElement("participant");
                param.SetAttribute("id", id);
                param.SetAttribute("name", name);
                param.SetAttribute("mail", mail);
                param.SetAttribute("login", login);
                param.SetAttribute("points", points);

                bool isAdded = false;
                 foreach (XmlElement node in doc.DocumentElement.ChildNodes)
            {
                int point = -1;
                foreach (XmlAttribute attr in node.Attributes)
                {
                    try{
                    if (attr.Name.Equals("points"))
                        point = Convert.ToInt32(attr.Value);
                    }
                    catch(FormatException ex)
                    {
                        point = -1;
                    }
                }
                if (!isAdded && point > -1 && point<=newPoint) 
                {
                    if (!string.IsNullOrEmpty(login))
                    {   
                        doc.DocumentElement.InsertBefore(param,node);
                        sg.participants = XmlStringConverter.convertXmlToString(doc);
                        isAdded = true;                                              
                        return;
                    }          
                }                
            }
            if (!isAdded) 
            {
                doc.DocumentElement.AppendChild(param);
                sg.participants = XmlStringConverter.convertXmlToString(doc);
            }
            }
        }
    }
}