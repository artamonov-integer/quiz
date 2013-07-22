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
using System.Windows.Shapes;

namespace QuizApp
{
    /// <summary>
    /// Логика взаимодействия для PersonalDataWindow.xaml
    /// </summary>
    public partial class PersonalDataWindow : Window
    {
        public PersonalDataWindow(int points)
        {
            InitializeComponent();
            this.PointsLabel.Content = points.ToString();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.NameTextBox.Text) &&
                !string.IsNullOrEmpty(this.LoginTextBox.Text) &&
                !string.IsNullOrEmpty(this.MailTextBox.Text))
            {
                this.DialogResult = true;
                Close();
            }
        }
    }
}
