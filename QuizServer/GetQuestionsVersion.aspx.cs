using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace QuizServer
{
    
    public partial class GetQuestionVersion : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.path == null)
                sg.updateSingleton(Request.PhysicalApplicationPath);
            string version = sg.questionsVersion.ToString();
            Response.ContentEncoding = new UTF8Encoding();
            Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>"+version+"</content>");
        }
    }
}