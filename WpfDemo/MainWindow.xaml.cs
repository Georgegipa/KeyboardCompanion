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
using System.Runtime.InteropServices;
namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        int i = 0;
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            Console.WriteLine("hello world");
        }
        
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxes();

        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void TextBoxes()
        {
            string text = "";
            if (i == 0)
            {
                Console1TextBox.Clear();
                Console2TextBox.Clear();
                Console3TextBox.Clear();
            }
            i++;

            if (i != 0)
                text = "\n";
            text += i + ".";
            text += " Test";
            Console1TextBox.AppendText(text);
            Console1TextBox.ScrollToEnd();
            Console2TextBox.AppendText(text);
            Console2TextBox.ScrollToEnd();
            Console3TextBox.AppendText(text);
            Console3TextBox.ScrollToEnd();
        }

        private void DisconnectConnectionSubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Console2TextBox.AppendText("\nDisconnect");
        }

        private void RefreshConnectionSubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Console2TextBox.AppendText("\nRefresh");
        }

        private void ExitConnectionSubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();  
        }

        private void Console1AutocScrollChecked(object sender, RoutedEventArgs e)
        {

        }
        private void Console2AutocScrollChecked(object sender, RoutedEventArgs e)
        {

        }
        private void Console3AutocScrollChecked(object sender, RoutedEventArgs e)
        {

        }
        private void Console1CopyClick(object sender, RoutedEventArgs e)
        {

        }
        private void Console2CopyClick(object sender, RoutedEventArgs e)
        {

        }
        private void Console3CopyClick(object sender, RoutedEventArgs e)
        {

        }
        private void Console1ClearClick(object sender, RoutedEventArgs e)
        {

        }
        private void Console2ClearClick(object sender, RoutedEventArgs e)
        {

        }
        private void Console3ClearClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
