using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuizServer
{
    public partial class GetParticipants : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(sg.partisipants);            
        }
    }
}