using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace KeyboardTrainer
{
    /// <summary>
    /// Логика взаимодействия для EditTrain.xaml
    /// </summary>
    public partial class EditTrain : Window
    {
        BinaryFormatter formatter;

        public EditTrain()
        {
            InitializeComponent();

            formatter = new BinaryFormatter();

            InitializeSettings();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("training.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, new TextRange(trainText.Document.ContentStart, trainText.Document.ContentEnd).Text);
            }

            SaveTestcases();

            MessageBox.Show("Текст успешно сохранен");

            this.DialogResult = true;
        }

        private void InitializeSettings()
        {
            Paragraph p = trainText.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 1;

            using (FileStream fs = new FileStream("training.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    string text = (string)formatter.Deserialize(fs);
                    trainText.AppendText(text);
                }
            }
        }

        private void SaveTestcases()
        {
            string text = new TextRange(trainText.Document.ContentStart, trainText.Document.ContentEnd).Text;
            text = text.Replace("\r", " ");
            MainWindow.settings.testcases.AddRange(text.Split('\n'));
            MainWindow.settings.testcases.RemoveAll(s => s == "" || s == " ");
        }
    }
}
