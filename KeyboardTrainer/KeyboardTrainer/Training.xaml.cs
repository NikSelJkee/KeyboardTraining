using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace KeyboardTrainer
{
    /// <summary>
    /// Логика взаимодействия для Training.xaml
    /// </summary>
    public partial class Training : Window
    {
        public static DispatcherTimer dt;

        int charset = 0, counter = 0;
        float charsetPerSecond = 0;
        public Training()
        {
            InitializeComponent();

            InitializeTraining();

            TimerStart();
        }

        private void InitializeTraining()
        {
            FocusManager.SetFocusedElement(this, userText);

            trainTimer.Content += MainWindow.settings.Time.ToString();
            countMistakes.Content += MainWindow.settings.Mistakes.ToString();
            keyboardSpeed.Content += MainWindow.settings.KeyboardSpeed.ToString() + " сим/мин";
            currentDifficult.Content += MainWindow.settings.Difficult;
            currentText.Content = MainWindow.settings.testcases[counter];
            countText.Content += $"{counter + 1}/" + MainWindow.settings.testcases.Count.ToString();
            userText.Text = "";
        }

        private void TimerStart()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            MainWindow.settings.Time++;

            MainWindow.settings.KeyboardSpeed = (int)charsetPerSecond * 60;
            keyboardSpeed.Content = "Скорость набора: " + MainWindow.settings.KeyboardSpeed + " сим/мин";

            charsetPerSecond = 0;

            trainTimer.Content = "Время: " + MainWindow.settings.Time.ToString();
        }

        private void SaveResult()
        {
            dt.Stop();

            if (MessageBox.Show("Программа обучения выполнена. Сохранить результаты?",
                        "Программа обучения выполнена", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = $"{MainWindow.settings.UserName}_result";
                dialog.DefaultExt = ".txt";
                dialog.Filter = "Text documents (.txt)|*.txt";

                if (dialog.ShowDialog() == true)
                {
                    string file = $"Имя: {MainWindow.settings.UserName}\n";
                    file += $"Уровень: {MainWindow.settings.Difficult}\n";
                    file += $"Время: {MainWindow.settings.Time}\n";
                    file += $"Ошибки: {MainWindow.settings.Mistakes}\n";
                    file += $"Скорость набора: {MainWindow.settings.KeyboardSpeed} сим/мин";

                    File.WriteAllText(dialog.FileName, file);

                    this.DialogResult = true;
                }
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void userText_TextChanged(object sender, TextChangedEventArgs e)
        {
            charsetPerSecond++;

            if (userText.Text != "" && userText.Text[charset] != MainWindow.settings.testcases[counter][charset])
            {
                MainWindow.settings.Mistakes++;
                countMistakes.Content = $"Ошибки: {MainWindow.settings.Mistakes}";
            }

            charset++;

            if (userText.Text.Length >= MainWindow.settings.testcases[counter].Length)
            {
                charset = -1;
                counter++;

                if (counter >= MainWindow.settings.testcases.Count)
                {
                    SaveResult();
                }
                else
                {
                    countText.Content = "Строка: " + $"{counter + 1}/" + MainWindow.settings.testcases.Count.ToString();
                    currentText.Content = MainWindow.settings.testcases[counter];
                    userText.Text = "";
                }
            }
        }

        private void userText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                e.Handled = true;
        }
    }
}
