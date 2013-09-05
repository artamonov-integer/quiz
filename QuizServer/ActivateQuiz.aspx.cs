using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace QuizServer
{
    public partial class ActivateQuiz : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.path == null)
                sg.updateSingleton(Request.PhysicalApplicationPath);
            string isActive = Request.QueryString["p"];
            Response.ContentType = "text/xml";
            Response.ContentEncoding = new UTF8Encoding();
            if (!string.IsNullOrEmpty(isActive)) 
            {
                if (isActive.Equals("1"))
                {
                    sg.isActive = true;
                    Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>1</content>");
                }
                else if (isActive.Equals("0"))
                {
                    sg.isActive = false;
                    Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>0</content>");
                }
            }
            else
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>0</content>");
            sg.saveXml();
        }
    }
}