using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace SokudaKun
{
    delegate void KeysSignal(List<keycode> latest_keys);
    public enum State
    {
        Waiting = 0,
        Running,
        Setting,
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private State state = State.Waiting;
        private ArtKeyHook keyhook = ArtKeyHook.GetInstance();
        private Utils utils = new Utils();
        private List<keycode> latest_keys;
        private List<KeysSignal> funcs = new List<KeysSignal>();
        private Config win_config;
        private Settings settings = new Settings();
        private string xml_file = "./settings.xml";

        public MainWindow()
        {
            InitializeComponent();
            this.keyhook.StartKeyEvent(this.KeyEvent);
            this.funcs.Add(this.RunStop);
            this.LoadSettings();
            this.ShowSettings();
        }
        protected virtual void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.settings.Cycle = int.Parse(this.CycleBox.Text);
            this.SaveSettings();
        }

        private void SaveSettings()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(this.xml_file, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, this.settings);
            sw.Close();
        }

        private void LoadSettings()
        {
            if (File.Exists(this.xml_file))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
                System.IO.StreamReader sr = new System.IO.StreamReader(this.xml_file, new System.Text.UTF8Encoding(false));
                this.settings = null;
                this.settings = (Settings)serializer.Deserialize(sr);
                sr.Close();
            }
        }


        private void ShowSettings()
        {
            if (this.settings.StartKeys == null)
            {
                this.settings.StartKeys = new List<keycode>() { keycode.F10 };
                this.settings.StopKeys = new List<keycode>() { keycode.LAlt };
                this.settings.Cycle = 1000;
            }
            this.MainMessage.Text = "クリックの間隔を入力後 " + utils.KeysToStr(this.settings.StartKeys) + " で実行してください。" +
                                    "クリック停止は " + utils.KeysToStr(this.settings.StopKeys) + " です。";
            this.CycleBox.Text = this.settings.Cycle.ToString();
            this.SaveSettings();
        }

        private bool CheckEqual(List<keycode> a, List<keycode> b)
        {
            return a.SequenceEqual(b);
        }

        //関数定義
        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;

        private void DoClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void RunStop(List<keycode> latest_keys)
        {
            Debug.WriteLine("[Main] " + utils.KeysToStr(latest_keys));
            if ((latest_keys.Count == this.settings.StopKeys.Count) && this.state == State.Running)
            {
                if (this.CheckEqual(latest_keys, this.settings.StopKeys))
                {
                    Debug.WriteLine("[Main] Stop click");
                    this.state = State.Waiting;
                    this.BtnConfig.IsEnabled = true;
                    this.CycleBox.IsEnabled = true;
                }
            }
            else if ((latest_keys.Count == this.settings.StartKeys.Count) && this.state == State.Waiting)
            {
                if (this.CheckEqual(latest_keys, this.settings.StartKeys))
                {
                    Debug.WriteLine("[Main] Start click");
                    this.state = State.Running;
                    this.BtnConfig.IsEnabled = false;
                    this.CycleBox.IsEnabled = false;
                    this.settings.Cycle = int.Parse(this.CycleBox.Text);
                    Task.Run(() => {
                        while (true)
                        {
                            if (this.state == State.Running)
                            {
                                this.DoClick();
                                Thread.Sleep(this.settings.Cycle);
                            }
                            else break;
                        }
                    });
                }

            }

        }
        private void KeyEvent(uint uiKey)
        {
            this.latest_keys = utils.GetKeysWithBuffer(uiKey);
            this.funcs.ForEach(item =>
            {
                item(this.latest_keys);
            });
        }

        private void Set_Config(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[Main] Config / OK");
            this.settings = this.win_config.settings;
            this.win_config.Close();
            this.ShowSettings();
        }
        private void Close_Config(object sender, EventArgs e)
        {
            Debug.WriteLine("[Main] Config / Closed");
            this.funcs.Remove(this.win_config.KeyEvent);
            this.state = State.Waiting;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.state = State.Setting;
            this.win_config = new Config(this.settings);
            this.funcs.Add(this.win_config.KeyEvent);
            this.win_config.OK.Click += this.Set_Config;
            this.win_config.Closed += this.Close_Config;
            this.win_config.Show();
        }

        private void CycleBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
        private void CycleBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
