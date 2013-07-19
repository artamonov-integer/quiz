using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuizServer
{
    public partial class GetQuestions : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.isActive)
            {
                Response.ContentType = "text/xml";
                Response.Write(sg.questions);
            }
            else
            {
                Response.Write("0");
            }
        }
    }
}