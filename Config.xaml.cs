using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SokudaKun
{
    /// <summary>
    /// Config.xaml の相互作用ロジック
    /// </summary>
    public partial class Config : Window
    {
        private Utils utils = new Utils();
        public Settings settings;

        #region "最大化・最小化・閉じるボタンの非表示設定"

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_STYLE = -16;
        const int WS_SYSMENU = 0x80000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            int style = GetWindowLong(handle, GWL_STYLE);
            style = style & (~WS_SYSMENU);
            SetWindowLong(handle, GWL_STYLE, style);
        }

        #endregion
        public Config(Settings _settings)
        {
            this.settings = Settings.DeepCopy(_settings);
            InitializeComponent();
            this.StartTBox.Text = this.utils.KeysToStr(this.settings.StartKeys);
            this.StopTBox.Text = this.utils.KeysToStr(this.settings.StopKeys);
        }

        public void KeyEvent(List<keycode> _latest_keys)
        {
            List<keycode> latest_keys = this.utils.DeepCopyKeys(_latest_keys);
            string keys_str = this.utils.KeysToStr(latest_keys);
            Debug.WriteLine("[Conf] " + keys_str);
            if ((bool)this.StartCBox.IsChecked)
            {
                this.StartTBox.Text = keys_str;
                this.settings.StartKeys = latest_keys;
            }
            if ((bool)this.StopCBox.IsChecked)
            {
                this.StopTBox.Text = keys_str;
                this.settings.StopKeys = latest_keys;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StartCBox_Checked(object sender, RoutedEventArgs e)
        {
            if (StartCBox.IsChecked == null)
                Debug.WriteLine("Start Null"); //Do not reach here
            else if ((bool)StartCBox.IsChecked)
                Debug.WriteLine("Start ON");
            else
                Debug.WriteLine("Start OFF"); //Do not reach here
        }

        private void StopCBox_Checked(object sender, RoutedEventArgs e)
        {
            if (StopCBox.IsChecked == null)
                Debug.WriteLine("Stop Null"); //Do not reach here
            else if ((bool)StopCBox.IsChecked)
                Debug.WriteLine("Stop ON");
            else
                Debug.WriteLine("Stop OFF"); //Do not reach here

        }
    }
}
