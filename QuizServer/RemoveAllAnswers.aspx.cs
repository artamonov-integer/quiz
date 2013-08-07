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
    public partial class RemoveAllAnswers : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            sg.xmlAnswers.DocumentElement.RemoveAll();
            sg.answers = XmlStringConverter.convertXmlToString(sg.xmlAnswers);
        }
    }
}