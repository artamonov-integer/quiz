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

            if (!string.IsNullOrEmpty(points) && !string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(name))
            {
                int newPoint;
                try
                {
                    newPoint = Convert.ToInt32(points);
                }
                catch (FormatException ex)
                {
                    return;
                }
                if (sg.path == null)
                    sg.updateSingleton(Request.PhysicalApplicationPath);
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
                    string pointStr = node.GetAttribute("points");
                        try
                        {
                            point = Convert.ToInt32(pointStr);
                        }
                        catch (FormatException ex)
                        {
                            doc.DocumentElement.RemoveChild(node);
                        }
                    if (!isAdded && point > -1 && point <= newPoint)
                    {

                        doc.DocumentElement.InsertBefore(param, node);
                        sg.participants = XmlStringConverter.convertXmlToString(doc);
                        isAdded = true;
                        sg.saveParticipants();
                        return;

                    }
                }
                if (!isAdded)
                {
                    doc.DocumentElement.AppendChild(param);
                    sg.participants = XmlStringConverter.convertXmlToString(doc);
                    sg.saveParticipants();
                }                
            }
        }
    }
}