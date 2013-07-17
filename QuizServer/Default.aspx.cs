using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace QuizServer
{
    public partial class _Default : System.Web.UI.Page
    {
        string path = @"C:\projects\quiz\trunk\trunk\QuizServer\bin\";
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            //var par1 = Request.QueryString["p1"];
            //var par2 = Request.QueryString["p2"];
            
            byte[] bytes = new byte[Request.InputStream.Length];
            int i = Request.InputStream.Read(bytes, 0, bytes.Length);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string str = enc.GetString(bytes);
            Response.Write(getAnswers());
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
    }   
}