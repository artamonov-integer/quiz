using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuizServer
{
    public partial class RemoveAllParticipants : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            sg.xmlParticipants.DocumentElement.RemoveAll();
            sg.participants = XmlStringConverter.convertXmlToString(sg.xmlParticipants);
        }
    }
}