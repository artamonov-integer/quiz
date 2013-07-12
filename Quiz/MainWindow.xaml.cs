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
        List<string> answers = null;
        List<Participant> participants = null;
        public MainWindow()
        {
            InitializeComponent();
            this.answers = new List<string>();
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
                this.AnswersList.Items.Clear();
                string filename = dlg.FileName;
                StreamReader sr = new StreamReader(filename);
                string answer = "";
                while(!sr.EndOfStream)
                {
                    answer = sr.ReadLine();
                    this.AnswersList.Items.Add(answer);
                    this.answers.Add(answer);
                }
                sr.Close();       
            }  
        }

        private List<string> filterList(string answerPath, List<string> answersList) 
        {
            List<string> filterList = new List<string>();
            foreach(string answer in answersList)
            {
                if (answer.IndexOf(answerPath) >= 0) 
                {
                    filterList.Add(answer);
                }
            }
            return filterList;
        }

        private void refreshAnswersListBox(List<string> answersList) 
        {
            this.AnswersList.Items.Clear();
            foreach (string answer in answersList)
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
