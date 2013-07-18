using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace QuizServer
{
    public partial class GetImage : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {            
            string fileName = Request.QueryString["n"];
            string path = sg.getPath() + @"\images\";
            string fullName = path + fileName;
            DirectoryInfo uploads = new DirectoryInfo(path);
            if (!uploads.Exists)
                uploads.Create();
            if (File.Exists(fullName))
            {
                Response.ContentType = "image/jpg";
                Response.WriteFile(fullName);
            }
            else
                Response.Write("File not found");
        }
    }
}