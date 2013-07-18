using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;

namespace QuizServer
{
    public class Singleton
    {
        private string path = @"C:\projects\quiz\trunk\trunk\QuizServer\bin\";
        public string answers = "";
        public string questions = "";
        public string partisipants = "";
        public int participantId;
        public XmlDocument xmlPartisipants;
        public XmlDocument xmlAnswers;

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
                xmlPartisipants = XmlStringConverter.convertStringToXml(partisipants);
                xmlAnswers = XmlStringConverter.convertStringToXml(answers);

            }
            catch (FileNotFoundException e) { }
            finally 
            {
                sr.Close();
            }
        }
        public static Singleton Instance { get; private set; }
        public string getPath() {
            return this.path;
        }
    }
}