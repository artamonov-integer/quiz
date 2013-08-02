﻿using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Security.AccessControl;

namespace QuizServer
{
    public class Singleton
    {
        private string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\data\";
        //private string path = Environment.CurrentDirectory + @"\data\";
        //private bool b =  RecurceAccess(Environment.CurrentDirectory);
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
            RecurceAccess(path);
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

        private void RecurceAccess(string folder)
        {
            try
            {
                System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
                string user = wi.Name;
                AddDirectorySecurity(folder, @user, FileSystemRights.FullControl, AccessControlType.Allow);
                string[] folders = Directory.GetDirectories(folder);
                for (int i = 0; i < folders.Length; i++)
                {
                    RecurceAccess(folders[i]);
                }
            }
            catch (Exception e) { 
                //MessageBox.Show(e.ToString()); 
            }
        }
        //объявляет метод
        public static void AddFileSecurity(string fileName, string account,
          FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }
        //объявляет метод
        public static void AddDirectorySecurity(string fileName, string account,
          FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            DirectorySecurity fSecurity = Directory.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            Directory.SetAccessControl(fileName, fSecurity);
        }
    }
}