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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl : UserControl
    {
        public List<Answer> answers;
        public List<Answer> filterAnswers = null;
        public string path = "images/";
        public Question question;
        public string id;
        public QuestionControl(List<Answer> answers)
        {
            InitializeComponent();
            this.answers = answers;
            this.filterAnswers = new List<Answer>(); 
            this.id = System.Guid.NewGuid().ToString();
            this.question = new Question();
        }

        private void FilterTextAnswer_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            {
                refreshAnswerComboBox(filterList(this.FilterTextAnswer.Text, this.answers));                                
            }
        }

        private void refreshAnswerComboBox(List<Answer> answerList) 
        {            
            this.AnswerComboBox.Items.Clear();
            foreach (Answer answer in answerList) 
            {
                this.AnswerComboBox.Items.Add(answer);
            }
        }

        private void AnswerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AnswerComboBox.SelectedItem != null) 
            {
                Answer answer = ((Answer)this.AnswerComboBox.SelectedItem);
                this.FilterTextAnswer.Text = answer.content;
                this.question.answerId = answer.id;
                this.question.answerContent = answer.content;
            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image files|*.png;*.jpg";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //откроем поток для записи в файл
                    string extention = null;
                    int index = fileName.IndexOf('.');
                    if (index > 0)
                    {
                        extention = fileName.Substring(index);
                        if (!string.IsNullOrEmpty(extention))
                        {
                            //read
                            Byte[] bytes = new byte[stream.Length];
                            int i;
                            //for (i = 0; i < stream.Length - 1; i++)
                            //{
                                stream.Read((byte[])bytes, 0, (int)stream.Length);
                            //}
                            stream.Close();

                            //write
                            try
                            {
                                this.id = System.Guid.NewGuid().ToString();
                                System.IO.FileStream fs = new System.IO.FileStream(path + this.id + extention, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                                fs.Seek(0, System.IO.SeekOrigin.Begin);
                                fs.Write(bytes, 0, bytes.Length); // запись массива байт
                                fs.Flush();
                                fs.Dispose(); // освобождаем ресурсы                                
                                fs.Close();

                                /*BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(fileName);
                                bitmap.EndInit();
                                QuestionImage.Source = bitmap;*/

                                ImageSourceConverter converter = new ImageSourceConverter();
                                this.QuestionImage.Source = (ImageSource)converter.ConvertFromString(path + this.id + extention);
                                this.question.image = path + this.id + extention;
                            }
                            catch (Exception exc) {
                                this.QuestionTextBox.Text = exc.ToString();
                            }
                        }
                    }                    
                }                                
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

        public void setQuestion(Question question)
        {
            this.question = question;
            this.QuestionTextBox.Text = question.content;
            this.FilterTextAnswer.Text = question.answerContent;
            this.NumTextBox.Text = question.number;
            ImageSourceConverter converter = new ImageSourceConverter();
            this.QuestionImage.Source = (ImageSource)converter.ConvertFromString(question.image);
        }

        private void addImage(string fileName) 
        {

        }

        public void saveQuestion() 
        {
            this.question.content = this.QuestionTextBox.Text;
            this.question.number = this.NumTextBox.Text;
        }

        private void QuestionImageClick(object sender, MouseButtonEventArgs e)
        {
            var image = new Image { Source = ((Image)sender).Source, Width = 400, Stretch = Stretch.Uniform };
            var stack = new StackPanel {HorizontalAlignment = System.Windows.HorizontalAlignment.Center};
            var closeButton = new Button() { Content = "Закрыть", Width = 120, Height = 30, Margin = new Thickness(10) };
            var win = new Window { Content = stack, Width = 400, Height = 400, WindowStartupLocation = WindowStartupLocation.CenterOwner};
            stack.Children.Add(image);
            stack.Children.Add(closeButton);
            closeButton.Click += (s, ee) => win.Close();
            win.ShowDialog();
            win.Focus();
        }
    }
    public class Question 
    {
        public string number { get; set; }
        public string content { get; set; }
        public string answerId { get; set; }
        public string answerContent { get; set; }
        public string image { get; set; }

        public Question() 
        {

        }

        public Question(string number, string content, string answerId, string answerContent, string image)
        {
            this.number = number;
            this.content = content;
            this.answerId = answerId;
            this.answerContent = answerContent;
            this.image = image;
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
}
