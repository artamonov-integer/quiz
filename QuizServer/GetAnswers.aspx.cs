﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuizServer
{
    public partial class GetAnswers : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.path == null)
                sg.updateSingleton(Request.PhysicalApplicationPath);
            Response.ContentType = "text/xml";
            Response.Write(sg.answers);   
        }
    }
}