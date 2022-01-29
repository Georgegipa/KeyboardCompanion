using System;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataFormats = System.Windows.Forms.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using MacrosEngine.Verifier;
using Path = System.IO.Path;

namespace KeyboardCompanionWpf
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            //this.WindowState = WindowState.Minimized;
            //TrayIcon();
        }

        private ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            //https://www.codeproject.com/Articles/290013/Formless-System-Tray-Application
            item = new ToolStripMenuItem();
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
            item.Click += exitAction;
            menu.Items.Add(item);
            return menu;
        }

        private void TrayIcon()
        { 
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon("assets/keypad.ico");
            //new System.Drawing.Icon("assets/keypad2.ico");
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = Create();
        }

        private void CloseAction()
        {
            //notifyIcon.Dispose();
            Environment.Exit(1);
        }

        private void exitAction(object? sender, EventArgs e)
        {
            CloseAction();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new SerialPortWindow();     
            w.Show();   
        }

        private void MAinWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
              CloseAction();
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //open only 1 file at a time
                if (files.Length != 1)
                {
                    Console.WriteLine("Can't read more than 1 files at the same time!");
                }
                else
                {
                    if (files[0].EndsWith(".txt"))//open only txt files
                    {
                        Console.WriteLine("--Checking File:"+Path.GetFileName(files[0]));
                        Verifier.VerifyFile(files[0],true);
                        Console.WriteLine("--Check Completed!");
                    }
                    else 
                        Console.WriteLine("File not Supported");
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                Console.WriteLine("Text detected");
                string[] content = (string[])e.Data.GetData(DataFormats.Text);
                Console.WriteLine(content);
                foreach (var line in content)
                {
                    Console.WriteLine(line);
                }
            }

            throw new NotImplementedException();
        }
    }
}
