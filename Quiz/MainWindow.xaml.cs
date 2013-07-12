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

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Answer> answers = null;
        List<Participant> participants = null;
        public MainWindow()
        {
            InitializeComponent();
            this.answers = new List<Answer>();
            this.participants = new List<Participant>();
            this.ParticipantsList.ItemsSource = this.participants;
            this.QuestionsList.Items.Add(new QuestionControl(this.answers));
        }        

        private void LoadAnswersList_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //this.AnswersList.Items.Clear();
                string filename = dlg.FileName;
                StreamReader sr = new StreamReader(filename);
                string answer = "";
                while(!sr.EndOfStream)
                {
                    answer = sr.ReadLine();
                    if (!isAlreadyExist(answer))
                    {
                        this.AnswersList.Items.Add(answer);
                        this.answers.Add(new Answer(System.Guid.NewGuid().ToString(), answer));
                    }
                }
                sr.Close();       
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

        private void saveAnswers() 
        {

        }

        private void refreshQuestionsListBox() 
        {
            foreach (QuestionControl qc in QuestionsList.Items) 
            {
                qc.answers = this.answers;
            }
        }

        private void saveQuestions() {
            
        }

        private void AnswerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.answers = loadAnswers();
            refreshAnswersListBox(this.answers);
            refreshQuestionsListBox();
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
