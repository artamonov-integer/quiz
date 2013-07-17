using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace QuizServer
{
    public class Singleton
    {
        private string path = @"C:\projects\quiz\trunk\trunk\QuizServer\bin\";
        public string answers = "";
        public string questions = "";
        public string partisipants = "";
        static Singleton()
        {
            Instance = new Singleton();

        }
        private Singleton() 
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path + "answers.xml");
                answers = sr.ReadToEnd();
                sr.Close();
                sr = new StreamReader(path + "questions.xml");
                questions = sr.ReadToEnd();
                sr.Close();
                sr = new StreamReader(path + "participants.xml");
                partisipants = sr.ReadToEnd();                
            }
            catch (FileNotFoundException e) { }
            finally 
            {
                sr.Close();
            }
        }
        public static Singleton Instance { get; private set; }
    }
}