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
using System.Drawing;
using System.Collections.Specialized;
using System.Drawing.Imaging;

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
        }

        private void LoadAnswersList_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                List<Answer> answerList = new List<Answer>();
                string filename = dlg.FileName;
                StreamReader sr = new StreamReader(filename);
                string answerStr = "";
                while (!sr.EndOfStream)
                {
                    answerStr = sr.ReadLine();
                    Answer answer = new Answer(System.Guid.NewGuid().ToString(), answerStr);
                    answerList.Add(answer);
                }
                sr.Close();
                saveAnswers(answerList);
                loadAnswers(this.answers);
                refreshAnswersListBox(this.answers);
            }
        }

        private bool isAlreadyExist(string content)
        {
            bool isExist = false;
            foreach (Answer answer in answers)
            {
                if (content.Equals(answer.content))
                    isExist = true;
            }
            return isExist;
        }

        private List<Answer> filterList(string answerPath, List<Answer> answersList)
        {
            List<Answer> filterList = new List<Answer>();
            foreach (Answer answer in answersList)
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
        }

        private List<Participant> getParticipantList()
        {
            List<Participant> participantList = new List<Participant>();
            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/GetParticipants.aspx");
            WebResponse response = request.GetResponse();
            xmlDoc.Load(response.GetResponseStream());
            response.Close();

            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                Participant participant = new Participant();
                foreach (XmlAttribute attr in table.Attributes)
                {
                    if (attr.Name.Equals("id"))
                        participant.Id = attr.Value;
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

        private List<Answer> loadAnswers(List<Answer> answerList)
        {
            answerList.Clear();
            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/GetAnswers.aspx");
            WebResponse response = request.GetResponse();
            xmlDoc.Load(response.GetResponseStream());
            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                Answer answer = new Answer();
                foreach (XmlAttribute attr in table.Attributes)
                {
                    if (attr.Name.Equals("id"))
                        answer.id = attr.Value;
                    else if (attr.Name.Equals("content"))
                        answer.content = attr.Value;
                }
                answerList.Add(answer);
            }
            return answerList;
        }

        private List<Question> loadQuestions()
        {
            List<Question> loadedQuestions = new List<Question>();
            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/getQuestionsForAdmin.aspx");
            WebResponse response = request.GetResponse();
            xmlDoc.Load(response.GetResponseStream());
            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
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

        private void refreshQuestionsListBox()
        {
            foreach (Question question in this.questions)
            {
                QuestionControl qc = new QuestionControl(this.answers, this.host, this.port);
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
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/SaveQuestions.aspx");
            request.ContentType = "text/xml";
            request.Method = "POST";
            XmlDocument data = questionsToXml();
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
                newStream.Close();
            }
            request.GetResponse();
        }

        private void AnswerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            refreshAnswersListBox(filterList(this.AnswerTextBox.Text, this.answers));
        }

        private void RefreshParticipantsButton_Click(object sender, RoutedEventArgs e)
        {
            this.participants = getParticipantList();
            refreshParticipantsListBox(participants);
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionControl qc = new QuestionControl(this.answers, this.host, this.port);
            qc.NumTextBox.Text = (this.QuestionsList.Items.Count+1).ToString();
            this.QuestionsList.Items.Add(qc);
        }

        private void QuestionsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            saveQuestions();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadConfig();
            try
            {
                loadAnswers(this.answers);
                refreshAnswersListBox(this.answers);
                this.questions = loadQuestions();
                refreshQuestionsListBox();
            }
            catch (WebException ex) 
            {
                MessageBox.Show("No connection!");
            }
        }

        private void AddAnswerButtom_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.AnswerTextBox.Text))
            {
                List<Answer> answerList = new List<Answer>();
                string answerStr = this.AnswerTextBox.Text;
                Answer answer = new Answer(System.Guid.NewGuid().ToString(), answerStr);
                answerList.Add(answer);
                saveAnswers(answerList);
                loadAnswers(this.answers);
                refreshAnswersListBox(this.answers);
            }
        }

        private void RemoveQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.QuestionsList.SelectedItem != null)
            {
                this.QuestionsList.Items.Remove(this.QuestionsList.SelectedItem);
            }
        }

        private void loadConfig()
        {
            StreamReader sr = null;
            try
            {
                if (File.Exists("config.txt"))
                {
                    sr = new StreamReader("config.txt");
                    while (!sr.EndOfStream)
                    {
                        string conf = sr.ReadLine();
                        int index = conf.IndexOf('=');
                        if (index > 0)
                        {
                            string confName = conf.Substring(0, index).Trim();
                            string value = conf.Substring(index + 1).Trim();
                            if (confName.Equals("port"))
                                this.port = value;
                            else if (confName.Equals("host"))
                                this.host = value;
                        }
                    }
                }
                else 
                {
                    this.port = "8080";
                    this.host = "localhost";
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

        private XmlDocument questionsToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("body");
            foreach (QuestionControl qc in QuestionsList.Items)
            {
                qc.saveQuestion();
                Question question = qc.question;
                if (question != null)
                {
                    XmlElement questionElement = doc.CreateElement("question");
                    questionElement.SetAttribute("id", question.id);
                    questionElement.SetAttribute("number", question.number);
                    questionElement.SetAttribute("content", question.content);
                    questionElement.SetAttribute("image", question.image);
                    questionElement.SetAttribute("answerId", question.answerId);
                    questionElement.SetAttribute("answerContent", question.answerContent);
                    root.AppendChild(questionElement);
                }
            }
            doc.AppendChild(root);
            return doc;
        }

        private void saveAnswers(List<Answer> answerList)
        {
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/AddAnswer.aspx");
            request.ContentType = "text/xml";
            request.Method = "POST";
            XmlDocument data = answerListToXml(answerList);
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
                newStream.Close();
            }

            WebResponse response = request.GetResponse();
            response.Close();
        }

        

        private void RemoveAnswerButtom_Click(object sender, RoutedEventArgs e)
        {
            if (this.AnswersList.SelectedItem != null)
            {
                string id = ((Answer)this.AnswersList.SelectedItem).id;
                removeAnswer(id);
                loadAnswers(this.answers);
                refreshAnswersListBox(this.answers);
            }
        }
        private void removeAnswer(string id)
        {
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/RemoveAnswer.aspx?id=" + id);
            request.GetResponse();
        }

        private void MenuItem_Click_Stop(object sender, RoutedEventArgs e)
        {
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/ActivateQuiz.aspx?p=0");
            request.GetResponse();
        }

        private void MenuItem_Click_Start(object sender, RoutedEventArgs e)
        {
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/ActivateQuiz.aspx?p=1");
            request.GetResponse();            
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            saveQuestions();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/SaveQuiz.aspx");
            request.GetResponse();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow(this.host, this.port);
            sw.Owner = this;
            Nullable<bool> result = sw.ShowDialog();
            if (result==true) 
            {
                string h = sw.HostTextBox.Text;
                string p = sw.PortTextBox.Text;
                try
                {
                    WebRequest request = WebRequest.Create("http://" + h + ":" + p + "/Default.aspx");
                    request.GetResponse();
                }
                catch (WebException ex) 
                {
                    MessageBox.Show("No connection!");
                    return;
                }
                this.port = p;
                this.host = h;

                loadAnswers(this.answers);
                refreshAnswersListBox(this.answers);
                this.questions = loadQuestions();
                refreshQuestionsListBox();
            }
        }

        private void SaveParticipantsButton_Click(object sender, RoutedEventArgs e)
        {

            List<Question> loadedQuestions = new List<Question>();
            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/getParticipants.aspx");
            WebResponse response = request.GetResponse();
            xmlDoc.Load(response.GetResponseStream());
            if (!File.Exists("loaded.xml"))
            {
                StreamWriter sw = File.CreateText("loaded.xml");
                sw.Close();
                XmlTextWriter textWritter = new XmlTextWriter("loaded.xml", System.Text.Encoding.UTF8);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("body");
                textWritter.WriteEndElement();
                textWritter.Close();
            }
            xmlDoc.Save("loaded.xml");
                       
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveQuestions();
            WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/SaveQuiz.aspx");
            request.GetResponse();
        }
    }
    class Participant
    {
        public string Login { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Id { get; set; }

        public Participant(){}

        public Participant(string Login, string Name, string Mail, int Points, string Id)
        {
            this.Login = Login;
            this.Name = Name;
            this.Mail = Mail;
            this.Points = Points;
            this.Id = Id;
        }
    }
}
