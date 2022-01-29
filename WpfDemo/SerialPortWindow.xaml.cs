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

namespace KeyboardCompanionWpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SerialPortWindow : Window
    {
        int i = 0;
        public SerialPortWindow()
        {
            InitializeComponent();
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
