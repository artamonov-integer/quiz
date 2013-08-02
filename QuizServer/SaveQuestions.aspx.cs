using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace QuizServer
{
    public partial class SaveQuestions : System.Web.UI.Page
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
                str = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>" + str;
                XmlDocument newXmlQuestions = XmlStringConverter.convertStringToXml(str);
                if (newXmlQuestions != null)
                {
                    sg.xmlQuestions = newXmlQuestions;
                    sg.questions = XmlStringConverter.convertXmlToString(newXmlQuestions);
                }
            }
        }
    }
}