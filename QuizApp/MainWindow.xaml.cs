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
//using System.Drawing.Imaging;

namespace QuizApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Answer> answers;
        public List<Question> questions;
        public Question curQuestion;
        public int curNumQuestion;
        public string port;
        public string host;
        public MainWindow()
        {
            InitializeComponent();
            this.answers = new List<Answer>();
            this.questions = new List<Question>();
            this.curNumQuestion = 0;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {   this.answers = new List<Answer>();
            this.questions = new List<Question>();
            bool isActiveQuize = loadQuestions(this.questions);
            if (isActiveQuize)
            {
                this.StartGrid.Visibility = Visibility.Hidden;
                this.QuestionGrid.Visibility = Visibility.Visible;
                this.curNumQuestion = 0;
                loadAnswers(this.answers);
                if (this.questions != null && this.questions.Count > 0)
                    showQuestion(this.questions[0]);
            }
            else 
            {
                MessageBox.Show("Quiz is ended!");
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            restartQuiz();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (curNumQuestion + 1 < this.questions.Count)
            {
                this.curNumQuestion++;
                showQuestion(this.questions[this.curNumQuestion]);
            }
            else 
            {
                int points = calculatePoints(this.questions);
                PersonalDataWindow pdw = new PersonalDataWindow(points);
                pdw.Owner = this;
                Nullable<bool> result = pdw.ShowDialog();
                if (result == true)
                {
                    string name = pdw.NameTextBox.Text;
                    string login = pdw.LoginTextBox.Text;
                    string mail = pdw.MailTextBox.Text;
                    try
                    {
                        WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/AddParticipants.aspx?n=" + name +
                            "&l=" + login + "&m=" + mail + "&p=" + points.ToString());
                        request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        System.Windows.MessageBox.Show("No connection!");
                        return;
                    }
                }
                restartQuiz();
            }
        }

        private void restartQuiz() 
        {
            this.StartGrid.Visibility = Visibility.Visible;
            this.QuestionGrid.Visibility = Visibility.Hidden;            
        }

        private void FilterTextAnswer_KeyUp(object sender, KeyEventArgs e)
        {
            refreshAnswerComboBox(filterList(this.FilterTextAnswer.Text, this.answers));
        }

        private void refreshAnswerComboBox(List<Answer> answerList)
        {
            this.AnswerComboBox.Items.Clear();
            foreach (Answer answer in answerList)
            {
                this.AnswerComboBox.Items.Add(answer);
            }
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

        private bool loadQuestions(List<Question> loadedQuestions)
        {
            bool isLoaded = true;
            loadedQuestions.Clear();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                WebRequest request = WebRequest.Create("http://" + host + ":" + port + "/getQuestions.aspx");
                WebResponse response = request.GetResponse();

                xmlDoc.Load(response.GetResponseStream());
                if (!xmlDoc.DocumentElement.InnerText.Equals("0"))
                {
                    foreach (XmlElement table in xmlDoc.DocumentElement.ChildNodes)
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
                        }
                        loadedQuestions.Add(question);
                    }
                }
                else
                    isLoaded = false;
            }
            catch (WebException wex) 
            {
                isLoaded = false;
            }
            return isLoaded;
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

        private void AnswerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AnswerComboBox.SelectedItem != null)
            {
                Answer answer = ((Answer)this.AnswerComboBox.SelectedItem);
                this.FilterTextAnswer.Text = answer.content;
                if (this.curQuestion!=null)
                    this.curQuestion.answer = answer.id;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadConfig();
            try
            {
                WebRequest request = WebRequest.Create("http://" + this.host + ":" + this.port + "/Default.aspx");
                request.GetResponse();
            }
            catch (WebException ex)
            {
                MessageBox.Show("No connection!");
            }
        }

        private void showQuestion(Question question) 
        {
            this.curQuestion = question;
            this.QuestionTextBox.Text = this.curQuestion.content;
            this.AnswerComboBox.Items.Clear();
            this.FilterTextAnswer.Text = "";
            if (!string.IsNullOrEmpty(this.curQuestion.image))
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                this.QuestionImage.Source = (ImageSource)converter.ConvertFromString("http://" + host + ":" + port + "/GetImage.aspx?n=" + this.curQuestion.image);
            }
        }

        private int calculatePoints(List<Question> questionList) 
        {
            int points = 0;
            foreach (Question question in questionList) 
            {
                if (question.isWrightAnswer())
                    points++;
            }
            return points;
        }

        public class Question
        {
            public string id { get; set; }
            public string number { get; set; }
            public string content { get; set; }
            public string answerId { get; set; }
            public string answer { get; set; }
            public string image { get; set; }

            public Question()
            {
                this.id = System.Guid.NewGuid().ToString();
            }

            public Question(string id, string number, string content, string answerId, string answer, string image)
            {
                this.id = id;
                this.number = number;
                this.content = content;
                this.answerId = answerId;
                this.answer = answer;
                this.image = image;
            }
            public bool isWrightAnswer() 
            {
                bool isWright = false;
                if (!string.IsNullOrEmpty(this.answer) && !string.IsNullOrEmpty(this.answerId) && this.answer.Equals(this.answerId)) 
                {
                    isWright = true;
                }
                return isWright;
            }
        }
        public class Answer
        {
            public string id { get; set; }
            public string content { get; set; }
            public Answer() { }
            public Answer(string id, string content)
            {
                this.id = id;
                this.content = content;
            }
            public override string ToString()
            {
                if (!string.IsNullOrEmpty(content))
                    return this.content;
                else
                    return base.ToString();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow(this.host, this.port);
            sw.Owner = this;
            Nullable<bool> result = sw.ShowDialog();
            if (result == true)
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
                    System.Windows.MessageBox.Show("No connection!");
                    return;
                }
                this.port = p;
                this.host = h;

                loadAnswers(this.answers);
                loadQuestions(this.questions);
            }
        }
    }
}
