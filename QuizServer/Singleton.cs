using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;

namespace QuizServer
{
    public class Singleton
    {
        private string path = Environment.CurrentDirectory + @"\data\";
        public string answers = "";
        public string questions = "";
        public string participants = "";
        public bool isActive;
        public XmlDocument xmlParticipants;
        public XmlDocument xmlAnswers;
        public XmlDocument xmlQuestions;

        static Singleton()
        {
            Instance = new Singleton();

        }
        private Singleton()
        {
            isActive = false;
            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {                
                DirectoryInfo uploads = new DirectoryInfo(path);
                if (!uploads.Exists)
                {
                    try
                    {
                        uploads.Create();
                    }
                    catch (Exception e) 
                    {
                        this.path = @"C:\Temp\";
                    }
                }
                if (!File.Exists(path + "answers.xml"))
                {
                    sw = File.CreateText(path + "answers.xml");
                    sw.Close();
                    XmlTextWriter textWritter = new XmlTextWriter(path + "answers.xml", System.Text.Encoding.UTF8);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("body");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                }
                sr = new StreamReader(path + "answers.xml");
                answers = sr.ReadToEnd();
                sr.Close();

                if (!File.Exists(path + "questions.xml"))
                {
                    sw = File.CreateText(path + "questions.xml");
                    sw.Close();
                    XmlTextWriter textWritter = new XmlTextWriter(path + "questions.xml", System.Text.Encoding.UTF8);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("body");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                }
                sr = new StreamReader(path + "questions.xml");
                questions = sr.ReadToEnd();
                sr.Close();

                if (!File.Exists(path + "participants.xml"))
                {
                    sw = File.CreateText(path + "participants.xml");
                    sw.Close();
                    XmlTextWriter textWritter = new XmlTextWriter(path + "participants.xml", System.Text.Encoding.UTF8);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("body");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                }
                sr = new StreamReader(path + "participants.xml");
                participants = sr.ReadToEnd();
                sr.Close();

                xmlParticipants = XmlStringConverter.convertStringToXml(participants);
                xmlAnswers = XmlStringConverter.convertStringToXml(answers);
                xmlQuestions = XmlStringConverter.convertStringToXml(questions);

            }
            catch (FileNotFoundException e) { }
            finally
            {
                sr.Close();
            }
        }
        public static Singleton Instance { get; private set; }
        public string getPath()
        {
            return this.path;
        }
        public void saveXml()
        {
            xmlAnswers.Save(path + "answers.xml");
            xmlParticipants.Save(path + "participants.xml");
            xmlQuestions.Save(path + "questions.xml");
        }
        public void saveParticipants() 
        {
            xmlParticipants.Save(path + "participants.xml");
        }
    }
}