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

        private string getAnswers() 
        {
            string answer = "";
            try{
                StreamReader sr = new StreamReader(path + "answers.xml");
                answer = sr.ReadToEnd();
                sr.Close();
            }catch(FileNotFoundException ex)
            {
                answer="file not found";
            }
            return answer;
        }
    }
    class Singleton
    {
        //private static string answers = "";
        //private static string questions = "";
        //private static string partisipants = "";
        static Singleton()
        {
            Instance = new Singleton();
            
        }
        private Singleton() { }
        public static Singleton Instance { get; private set; }
    }
}