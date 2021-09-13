using System;
using System.Diagnostics;
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
using System.Windows.Forms;
using System.IO;
using Clipboard = System.Windows.Clipboard;
using System.Drawing;
using Point = System.Windows.Point;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Threading;
using Microsoft.Toolkit.Mvvm;
using WpfApp1.Model;
using Cursor = System.Windows.Input.Cursor;
//using Control = System.Windows.Controls.Control;
using Control = System.Windows.Forms.Control;
using MessageBox = System.Windows.MessageBox;
using TextDataFormat = System.Windows.Forms.TextDataFormat;
using System.Media;
using System.Data;

namespace WpfApp1
{


    public partial class MainWindow : Window
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);


        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEX(IntPtr hWndParent, IntPtr hWndChildAfter, IntPtr lpszClass, IntPtr lpszWindow);


        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            Thread.Sleep(1000);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }



        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);



        // Constants
        private const string OUT_PUT_PATH = @"C:\Users\rober\OptionsTimeAndSales\";
        private const string FILE_NAME = "_Options.txt";
        private const string STOCKSLIST = @"C:\Users\rober\OptionsTimeAndSales\nasdaq100.txt";


        public MainWindow()
        {
            InitializeComponent();
            // Set some default values to popluate listbox
            SetUpApplication();

        }
        public void PlaySimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();
        }
        private  void ExecuteScript(List<string> Symbols)
        {

            // Column Headers for Datatable
            List<string> columnheaders = new List<string>
            {
                "Time",
                "Contract",
                "QTY",
                "Price",
                "Exchange",
                "Market",
                "Delta",
                "Imp Vol",
                "Underlying",
                "Condition",
                "Not Used"
            };


            foreach (var symbol in Symbols)
            {
                // Script to scrape ToS Options Table into clipboard
                GetOptionsTimeAndSales(symbol);
                
                // Write Contents of clipboard into text file
                WriteText(OUT_PUT_PATH, FILE_NAME, symbol, false);


                // Generate a Datatable from Text file 
                DataTableGenerator dataTableGenerator = new DataTableGenerator(OUT_PUT_PATH + symbol + FILE_NAME,columnheaders);
                DataTable dataTable = dataTableGenerator.GenerateDataTable();

                // Convert Datatable into JSON format and save file
                DataTableToJSONwithStringBuilder jsonBuilder = new DataTableToJSONwithStringBuilder(dataTable, OUT_PUT_PATH + symbol, true);

                // Populate ListBox with completed tasks and execute method
                _ = OptionsList.Items.Add(jsonBuilder.BuildJSON());
                OptionsList.ScrollIntoView(jsonBuilder);


                // ---- UNUSED METHODS
                // JSONFileParser jSONFileParser = new JSONFileParser();
                //jSONFileParser.OutPutDelimited(OUT_PUT_PATH + symbol + FILE_NAME);
                //jSONFileParser.OutPutDelimited("C:\\Users\\rober\\OptionsTimeAndSales\\" + symbol + "Options.txt");
                //Start Json implemention below
                //OptionsList.Items.Add(option);
            }

            //End Process
            PlaySimpleSound();
            MessageBox.Show("Process Complete");

        }

        private bool GetRunningProcesses(string ProcessName)
        {
            int _count = 0;
            bool _processExists = false;
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    if (p.MainWindowTitle.Contains(ProcessName))
                    {
                        _processExists = true;
                        _count++;
                    }
                }
            }
            if (!_processExists)
            {
                MessageBox.Show(ProcessName + "...Not Running or Open", "Process Not Found", MessageBoxButton.OK, MessageBoxImage.Information);

                return false;
            }
            if (_count > 1)
            {
                MessageBox.Show(ProcessName + "...Too Many Windows with same name", "Close Redunant or Unnecessary Windows", MessageBoxButton.OK, MessageBoxImage.Information);

                return false;
            }
            return true;
        }    
        public void SetUpApplication()
        {
            List<string> stocks = new List<string>();

            var lines = System.IO.File.ReadAllLines(STOCKSLIST);
            foreach (var line in lines)
            {
                SymbolsList.Items.Add(line);
            }



            SymbolsList.Items.Add("AAPL");
            SymbolsList.Items.Add("GME");
            SymbolsList.Items.Add("BB");
            SymbolsList.Items.Add("CUK");
            SymbolsList.Items.Add("CNK");
            SymbolsList.Items.Add("FLO");
            SymbolsList.Items.Add("MSFT");
            SymbolsList.Items.Add("SKLZ");
            SymbolsList.Items.Add("K");

        }

        public void GetOptionsTimeAndSales(string Symbol)
        {
            Process[] processes = Process.GetProcesses();
                
            foreach (Process p in processes)
            {
                //Console.WriteLine(p + "\n");
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    if (p.MainWindowTitle.Contains("Options Time & Sales"))
                    {
                        Debug.WriteLine("Process HWND" + p.Handle);

                        // Clear Clipboard For Validation of Copy
                        Clipboard.Clear();

                        // SET ThinkOrSwim TO FOREGROUND 

                        SetForegroundWindow(p.MainWindowHandle);
                        //SetCursorPos(200, 200);   
                        MoveWindow(p.MainWindowHandle, 0, 0, 1280, 720, true);

                        //  Select Options Search Feild
                        Thread.Sleep(1000);
                        LeftMouseClick(35, 65);
                        Thread.Sleep(200);

                        // Clear Options Seach Field
                        SendKeys.SendWait("{DELETE}");
                        Thread.Sleep(300);
                       

                        // Enter Symbol     
                        SendKeys.SendWait(Symbol);
                        Thread.Sleep(1000);


                        //Execute Serach
                        SendKeys.SendWait("{TAB}");
                        Thread.Sleep(1000);

                        while (!Clipboard.ContainsText(System.Windows.TextDataFormat.Text))
                        {
                            // Set Cursor to Expand Options T&S Table
                            LeftMouseClick(90, 90);
                            Thread.Sleep(1000);


                            // Set Focus to Options T&S Table
                            // allow 3 seconds to load all options data prior to Copy 
                            LeftMouseClick(205, 205);
                            Thread.Sleep(3000);

                            // Select all Data and Copy to Clipboard
                            SendKeys.SendWait("^a");
                            Thread.Sleep(500);
                            SendKeys.SendWait("^c");
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            
        }

        public static void WriteText (string OutPutPath, string FileName, string Symbol, bool Timestamp)
        {
            string _timestamp = DateTime.Now.ToString("yyyyMMddHH");
            

            if (Timestamp)
            {
                File.WriteAllText(OutPutPath + Symbol + _timestamp + FileName, Clipboard.GetText());
            }else
                File.WriteAllText(OutPutPath + Symbol + FileName, Clipboard.GetText());

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (GetRunningProcesses("Options Time & Sales"))
            {


                List<string> symbolList = new List<string>();
                foreach (var item in SymbolsList.Items)
                {
                    symbolList.Add(item.ToString());

                }
                foreach (var symbol in symbolList)
                {
                    Console.WriteLine(symbol.ToString());
                }
                ExecuteScript(symbolList);
            }
            else
            {
                return;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SymbolTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Symbols_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Symbols(object sender, RoutedEventArgs e)
        {
            SymbolsList.Items.Add(SymbolTextBox.Text);
            SymbolTextBox.Clear();
            SymbolTextBox.Focus();
        }

        private void Remove_Symbol(object sender, RoutedEventArgs e)
        {
            SymbolsList.Items.RemoveAt(SymbolsList.SelectedIndex);
        }
    }

}
