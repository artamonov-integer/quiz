using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;

namespace QuizServer
{
    public partial class SaveImage : System.Web.UI.Page
    {
        //Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileName = Request.QueryString["n"];
            savePicture(fileName);
            Response.Write("1");
        }
        public void savePicture(string fileName)
        {
            byte[] buffer = new byte[Request.InputStream.Length];
            int bytesRead = Request.InputStream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string path = Request.PhysicalApplicationPath + @"\images\";
                string fullName = path + fileName;
                DirectoryInfo uploads = new DirectoryInfo(path);
                if (!uploads.Exists)
                    uploads.Create();
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);                    
                    img.Save(path + fileName, ImageFormat.Jpeg);
                }
            }
        }
    }
}