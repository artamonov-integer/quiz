using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string host = null;
        string port = null;
        List<Answer> answers = null;
        List<Participant> participants = null;
        List<Question> questions = null;
        public MainWindow()
        {
            InitializeComponent();
            this.answers = new List<Answer>();
            this.participants = new List<Participant>();
            this.questions = new List<Question>();
            this.ParticipantsList.ItemsSource = this.participants;
            //this.QuestionsList.Items.Add(new QuestionControl(this.answers));
        }        

        private void LoadAnswersList_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(System.IO.File.ReadAllText("answers.xml"));

                XmlElement root = doc.DocumentElement;
                
                //this.AnswersList.Items.Clear();
                string filename = dlg.FileName;
                StreamReader sr = new StreamReader(filename);
                string answerStr = "";
                while(!sr.EndOfStream)
                {
                    answerStr = sr.ReadLine();
                    if (!isAlreadyExist(answerStr))
                    {
                        Answer answer = new Answer(System.Guid.NewGuid().ToString(), answerStr);
                        this.AnswersList.Items.Add(answerStr);
                        this.answers.Add(answer);
                        XmlElement param = doc.CreateElement("answer");
                        param.SetAttribute("id", answer.id);
                        param.SetAttribute("content", answer.content);
                        root.AppendChild(param);
                    }
                }
                sr.Close();
                doc.Save("answers.xml");
            }  
        }

        private bool isAlreadyExist(string content) 
        {
            bool isExist = false;
            foreach(Answer answer in answers)
            {
                if (content.Equals(answer.content))
                    isExist = true;
            }
            return isExist;
        }

        private List<Answer> filterList(string answerPath, List<Answer> answersList) 
        {
            List<Answer> filterList = new List<Answer>();
            foreach(Answer answer in answersList)
            {
                if (answer.content.IndexOf(answerPath) >= 0) 
                {
                    filterList.Add(answer);
                }
            }
            return filterList;
        }

        private void refreshAnswersListBox(List<Answer> answersList) 
        {
            this.AnswersList.Items.Clear();
            foreach (Answer answer in answersList)
            {
                this.AnswersList.Items.Add(answer);
            }
        }

        private void refreshParticipantsListBox(List<Participant> participiantList) 
        {
            this.participants = getParticipantList();
            this.ParticipantsList.ItemsSource = this.participants;
            this.ParticipantsList.Items.Refresh();
            //this.ParticipantsList.Items.Clear();
            //foreach (Participant participant in participiantList) 
            //{
            //    this.ParticipantsList.Items.Add(participant.Login);
            //}
        }

        private List<Participant> getParticipantList() 
        {
            List<Participant> participantList = new List<Participant>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("participants.xml");
            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                // перебираем все атрибуты элемента
                Participant participant = new Participant();
                foreach (XmlAttribute attr in table.Attributes)
                {                    
                    if (attr.Name.Equals("id"))
                        participant.Id = Convert.ToInt32(attr.Value);
                    else if (attr.Name.Equals("name"))
                        participant.Name = attr.Value;
                    else if (attr.Name.Equals("login"))
                        participant.Login = attr.Value;
                    else if (attr.Name.Equals("mail"))
                        participant.Mail = attr.Value;
                    else if (attr.Name.Equals("points"))
                        participant.Points = Convert.ToInt32(attr.Value);
                }
                participantList.Add(participant);
            }
            return participantList;
        }

        private List<Answer> loadAnswers() 
        {
            List<Answer> loadedAnswers = new List<Answer>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("answers.xml");
            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                // перебираем все атрибуты элемента
                Answer answer = new Answer();
                foreach (XmlAttribute attr in table.Attributes)
                {
                    if (attr.Name.Equals("id"))
                        answer.id = attr.Value;
                    else if (attr.Name.Equals("content"))
                        answer.content = attr.Value;                    
                }                
                loadedAnswers.Add(answer);
            }
            return loadedAnswers;
        }

        private List<Question> loadQuestions()
        {
            List<Question> loadedQuestions = new List<Question>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("questions.xml");
            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                // перебираем все атрибуты элемента
                Question question = new Question();
                foreach (XmlAttribute attr in table.Attributes)
                {
                    if (attr.Name.Equals("number"))
                        question.number = attr.Value;
                    else if (attr.Name.Equals("content"))
                        question.content = attr.Value;
                    else if (attr.Name.Equals("image"))
                        question.image = attr.Value;
                    else if (attr.Name.Equals("answerId"))
                        question.answerId = attr.Value;
                    else if (attr.Name.Equals("answerContent"))
                        question.answerContent = attr.Value;
                }
                loadedQuestions.Add(question);
            }
            return loadedQuestions;
        }

        /*private void saveAnswers() 
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(System.IO.File.ReadAllText("answers.xml"));                        
            XmlElement root = doc.DocumentElement;            

            foreach(Answer answer in this.answers)
            {
                XmlElement param = doc.CreateElement("answer");
                param.SetAttribute("id", answer.id);
                param.SetAttribute("content", answer.content);
                root.AppendChild(param);                
            }
            doc.Save("answers.xml");
        }*/

        private void refreshQuestionsListBox() 
        {
            foreach(Question question in questions)
            {
                QuestionControl qc = new QuestionControl(this.answers);
                qc.setQuestion(question);
                this.QuestionsList.Items.Add(qc);
            }
            foreach (QuestionControl qc in QuestionsList.Items) 
            {
                qc.answers = this.answers;
            }
        }

        private void saveQuestions() 
        {
            XmlTextWriter textWritter = new XmlTextWriter("questions.xml", Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("body");
            textWritter.WriteEndElement();
            textWritter.Close();

            XmlDocument doc = new XmlDocument();
            doc.Load("questions.xml");
            XmlElement root = doc.DocumentElement;
            foreach (QuestionControl qc in QuestionsList.Items)
            {
                qc.saveQuestion();
                Question question = qc.question;
                if (question != null) 
                {
                    XmlElement questionElement = doc.CreateElement("question");
                    questionElement.SetAttribute("number", question.number);
                    questionElement.SetAttribute("content", question.content);
                    questionElement.SetAttribute("image", question.image);
                    questionElement.SetAttribute("answerId", question.answerId);
                    questionElement.SetAttribute("answerContent", question.answerContent);
                    root.AppendChild(questionElement);                    
                }
            }
            doc.Save("questions.xml");           
        }

        private void AnswerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter) 
            {
                refreshAnswersListBox(filterList(this.AnswerTextBox.Text, this.answers));
            }
        }

        private void RefreshParticipantsButton_Click(object sender, RoutedEventArgs e)
        {
            this.participants = getParticipantList();
            refreshParticipantsListBox(participants);
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionControl qc = new QuestionControl(this.answers);            
            this.QuestionsList.Items.Add(qc);            
        }

        private void QuestionsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            saveQuestions();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.answers = loadAnswers();
            refreshAnswersListBox(this.answers);
            this.questions = loadQuestions();
            refreshQuestionsListBox();
            loadConfig();
        }

        private void AddAnswerButtom_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(this.AnswerTextBox.Text))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(System.IO.File.ReadAllText("answers.xml"));
                XmlElement root = doc.DocumentElement;
                string answerStr = this.AnswerTextBox.Text;
                if (!isAlreadyExist(answerStr))
                {
                    Answer answer = new Answer(System.Guid.NewGuid().ToString(), answerStr);
                    this.AnswersList.Items.Add(answerStr);
                    this.answers.Add(answer);
                    XmlElement param = doc.CreateElement("answer");
                    param.SetAttribute("id", answer.id);
                    param.SetAttribute("content", answer.content);
                    root.AppendChild(param);
                }                   
                doc.Save("answers.xml");            
            }
        }

        private void RemoveQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.QuestionsList.SelectedItem != null) 
            {
                this.QuestionsList.Items.Remove(this.QuestionsList.SelectedItem);
            }
        }

        private void sortAnswerList() 
        {

        }

        private void loadConfig() 
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("config.txt");
                while (!sr.EndOfStream)
                {
                    string conf = sr.ReadLine();
                    int index = conf.IndexOf('=');
                    if(index > 0){
                        string confName = conf.Substring(0,index).Trim();
                        string value = conf.Substring(index+1).Trim();
                        if (confName.Equals("port"))
                            this.port = value;
                        else if (confName.Equals("host"))
                            this.host = value;
                    }
                }                
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Config file not found!");
            }
            finally 
            {
                sr.Close();
            }
        }

        public XmlDocument answerListToXml(List<Answer> answerList) 
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("body");            
            foreach (Answer answer in answerList)
            {
                XmlElement param = doc.CreateElement("answer");
                param.SetAttribute("id", answer.id);
                param.SetAttribute("content", answer.content);
                root.AppendChild(param);
            }
            doc.AppendChild(root);
            return doc;
        }

        private void TestButtom_Click(object sender, RoutedEventArgs e)
        {
            WebRequest request = WebRequest.Create("http://"+host+":"+port+"/Default.aspx");
            request.ContentType = "text/xml";
            request.Method = "POST";
            XmlDocument data = answerListToXml(this.answers);
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            data.WriteTo(tx);
            string str = sw.ToString();
            sw.Close();
            tx.Close();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                //data.Save(newStream);
                newStream.Close();
            }
            
            WebResponse response = request.GetResponse();
            response.Close(); 
        }
    }
    class Participant
    {
        public string Login { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int Id { get; set; }

        public Participant() 
        {

        }

        public Participant(string Login, string Name, string Mail, int Points, int Id)
        {
            this.Login = Login;
            this.Name = Name;
            this.Mail = Mail;
            this.Points = Points;
            this.Id = Id;
        }
    }
}
