using System;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataFormats = System.Windows.Forms.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using MacrosEngine.Verifier;
using Path = System.IO.Path;
using MacrosEngine.UKP;
namespace KeyboardCompanionWpf
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            //this.WindowState = WindowState.Minimized;
            TrayIcon();
        }

        private ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            //https://www.codeproject.com/Articles/290013/Formless-System-Tray-Application
            var item = new ToolStripMenuItem();
            item.Text = "About";
            menu.Items.Add(item);
            
            menu.Items.Add(new ToolStripSeparator());
            var submenu = new ToolStripMenuItem();
            submenu.Text = "Available ports";
            item = new ToolStripMenuItem();
            item.Text = "Demo Port";
            submenu.DropDownItems.Add(item);
            menu.Items.Add(submenu);
            menu.Items.Add(new ToolStripSeparator());

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += ExitAction;
            menu.Items.Add(item);
            return menu;
        }

        private void TrayIcon()
        { 
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon("assets/keypad_16.ico");//icon must have always copy and build action set to none
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenuStrip = Create();
        }

        private void CloseAction()
        {
            _notifyIcon.Dispose();
            Environment.Exit(1);
        }

        private void ExitAction(object? sender, EventArgs e)
        {
            CloseAction();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new SerialPortWindow();     
            w.Show();   
        }

        private void TestPopupClick(object sender, RoutedEventArgs e)
        {
            var win = new PopUp(); 
            win.Show();   
        }
        
        private void TestFileClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void MAinWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
              CloseAction();
        }

        private void FileOnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //check all dropped files
                foreach (var f in ((string[])e.Data.GetData(DataFormats.FileDrop))!)
                {
                    if (f.EndsWith(".txt"))//open only txt files
                    {
                        Console.WriteLine("--Checking File:"+Path.GetFileName(f));
                        var er= Verifier.VerifyFile(f,true);
                        Console.WriteLine("--Check Completed!\nErrors:" + er.ErrorNum + "\nWarnings:" + er.WarningNum);
                        // if (er.ErrorNum == 0 && er.WarningNum == 0)
                        // {
                        //     UKP.GenerateDefaultMacros(ReadAll);
                        // }
                    }
                    else
                    {
                         Console.WriteLine(Path.GetFileName(f)+" filetype not Supported");
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                Console.WriteLine("Text detected");
                string content = (string)e.Data.GetData(DataFormats.Text);
                string[] lines = content!.Split(Environment.NewLine);
                Console.WriteLine("--Checking Dropped Text");
                var er= Verifier.VerifyText(lines,true);
                Console.WriteLine("--Check Completed!\nErrors:"+er.ErrorNum+"\nWarnings:"+er.WarningNum);
                if (er.ErrorNum == 0 && er.WarningNum == 0)
                {
                    UKP.GenerateDefaultMacros(lines);
                }
            }

            throw new NotImplementedException();
        }
    }
}
