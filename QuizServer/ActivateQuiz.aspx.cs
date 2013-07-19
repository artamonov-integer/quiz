using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuizServer
{
    public partial class ActivateQuiz : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            string isActive = Request.QueryString["p"];
            Response.ContentType = "text/xml";
            if (!string.IsNullOrEmpty(isActive)) 
            {
                if (isActive.Equals("1"))
                {
                    sg.isActive = true;
                    Response.Write("1");
                }
                else if (isActive.Equals("0"))
                {
                    sg.isActive = false;
                    Response.Write("0");
                }
            }
            else
                Response.Write("0");
        }
    }
}