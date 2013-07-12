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

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl : UserControl
    {
        public List<string> answers = null;
        public string path = "images/";
        public string id;
        public QuestionControl(List<string> answers)
        {
            InitializeComponent();
            this.answers = answers;
            this.id = System.Guid.NewGuid().ToString();
        }

        private void FilterTextAnswer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                refreshAnswerComboBox(this.answers);                                
            }
        }

        private void refreshAnswerComboBox(List<string> answerList) 
        {            
            this.AnswerComboBox.Items.Clear();
            foreach (string answer in answerList) 
            {
                this.AnswerComboBox.Items.Add(answer);
            }
        }

        private void AnswerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AnswerComboBox.SelectedItem != null) 
            {
                this.FilterTextAnswer.Text = (string) this.AnswerComboBox.SelectedItem;
            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image files|*.png";

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
                                System.IO.FileStream fs = new System.IO.FileStream(path + this.id + extention, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                                fs.Seek(0, System.IO.SeekOrigin.Begin);
                                fs.Write(bytes, 0, bytes.Length); // запись массива байт
                                fs.Flush();
                                fs.Dispose(); // освобождаем ресурсы                                
                                fs.Close();
                                ImageSourceConverter converter = new ImageSourceConverter();
                                this.QuestionImage.Source = (ImageSource)converter.ConvertFromString(path + this.id + extention);                                
                                //this.QuestionImage.
                            }
                            catch (Exception exc) {
                                this.QuestionTextBox.Text = exc.ToString();
                            }
                        }
                    }                    
                }                                
            }  
        }
    }
}
