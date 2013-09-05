using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace QuizServer
{
    public partial class AddAnswer : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] bytes = new byte[Request.InputStream.Length];
            int i = Request.InputStream.Read(bytes, 0, bytes.Length);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string str = enc.GetString(bytes);
            if (!string.IsNullOrEmpty(str))
            {
                if (sg.path == null)
                    sg.updateSingleton(Request.PhysicalApplicationPath);
                XmlDocument newXmlAnswers = XmlStringConverter.convertStringToXml(str);
                XmlDocument xmlAnswers = sg.xmlAnswers;

                foreach (XmlElement newAnswer in newXmlAnswers.DocumentElement.ChildNodes)
                {
                    bool isExist = false;
                    string content = newAnswer.GetAttribute("content");
                    if (!string.IsNullOrEmpty(content))
                    {
                        XmlElement lastElement=null;
                        bool isLast = false;
                        foreach (XmlElement answer in xmlAnswers.DocumentElement.ChildNodes)
                        {
                            if (!isExist && answer.GetAttribute("content") != null)
                            {
                                string oldContent = answer.GetAttribute("content");
                                if (!isLast && String.Compare(oldContent, content) > 0) 
                                {
                                    lastElement = answer;
                                    isLast = true;
                                }
                                if(answer.GetAttribute("content").Equals(content))
                                {
                                    isExist = true;
                                    //return;
                                }
                            }
                        }
                        if (!isExist)
                        {
                            XmlElement param = xmlAnswers.CreateElement("answer");
                            param.SetAttribute("id", newAnswer.GetAttribute("id"));
                            param.SetAttribute("content", newAnswer.GetAttribute("content"));
                            if (lastElement != null)
                            {
                                xmlAnswers.DocumentElement.InsertBefore(param, lastElement);
                            }
                            else 
                            {
                                xmlAnswers.DocumentElement.AppendChild(param);
                            }
                        }
                    }
                }
                sg.answers = XmlStringConverter.convertXmlToString(xmlAnswers);
            }
            Response.ContentType = "text/xml";
            Response.Write("1");
        }
    }
}