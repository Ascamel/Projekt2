using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

namespace Projekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string values;

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "PPM File | *.ppm"
            };
            
            bool? result = dialog.ShowDialog();

            if (result is not true) return;

            string fileName = dialog.FileName;

            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);




            using (StreamReader reader = new StreamReader(fs, Encoding.ASCII))
            {
                values = reader.ReadToEnd();
            }
            Trace.WriteLine(values);

        }
    }
}
