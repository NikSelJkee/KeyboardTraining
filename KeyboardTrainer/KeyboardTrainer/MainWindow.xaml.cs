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

namespace KeyboardTrainer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Settings settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = new Settings();

            if (!File.Exists("training.dat"))
                File.Create("training.dat");

            LoadTestcases();
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void editTrain_Click(object sender, RoutedEventArgs e)
        {
            EditTrain editTrain = new EditTrain();
            editTrain.ShowDialog();
        }

        private void startTrain_Click(object sender, RoutedEventArgs e)
        {
            ResetSettings();

            Training training = new Training();
            if (training.ShowDialog() == false)
                Training.dt.Stop();
        }

        private void ResetSettings()
        {
            settings.UserName = userName.Text;
            settings.Difficult = difficult.Text;
            settings.Mistakes = 0;
            settings.Time = 0;
        }

        private void LoadTestcases()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("training.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    string text = (string)formatter.Deserialize(fs);
                    text = text.Replace("\r", " ");
                    text = text.Replace(" \n", "\n");
                    settings.testcases.AddRange(text.Split('\n'));
                    settings.testcases.RemoveAll(s => s == "" || s == " ");                  
                }
            }
        }
    }
}
