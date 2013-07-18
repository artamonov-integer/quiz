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
        //string path = @"C:\projects\quiz\trunk\trunk\QuizServer\bin\";
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            //var par1 = Request.QueryString["p1"];
            //var par2 = Request.QueryString["p2"];
            
            /*byte[] bytes = new byte[Request.InputStream.Length];
            int i = Request.InputStream.Read(bytes, 0, bytes.Length);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string str = enc.GetString(bytes);*/
            //savePicture();
            //Response.Write(getAnswers());
            //Request.InputStream
            //var par2 = Request.QueryString["id"];
            //Response.ContentType = "image/jpg";
            //Response.WriteFile(@"C:\projects\quiz\foo.jpg");

        }

        public string getAnswers() 
        {            
            string answers = sg.answers;
            return answers;
        }

        public string getParticipants()
        {
            string partisipants = sg.partisipants;
            return partisipants;
        }

        public string getQuestions()
        {
            string questions = sg.questions;
            return questions;
        }
        public void savePicture() 
        {
            byte[] buffer = new byte[Request.InputStream.Length];
            int bytesRead = Request.InputStream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);
                    img.Save(@"C:\projects\quiz\foo.jpg", ImageFormat.Jpeg);
                }
            }
        }
    }   
}