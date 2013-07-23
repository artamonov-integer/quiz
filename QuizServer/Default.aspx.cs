using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;

namespace QuizServer
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/xml";
            Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>" +
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "</content>");
        }
    }   
}