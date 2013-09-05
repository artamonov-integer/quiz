using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace QuizServer
{
    public partial class SaveQuiz : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.path == null)
                sg.updateSingleton(Request.PhysicalApplicationPath);
            sg.saveXml();
        }
    }
}