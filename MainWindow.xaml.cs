using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SokudaKun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArtKeyHook keyhook = ArtKeyHook.GetInstance();
        private Utils utils = new Utils();
        private List<keycode> latest_keys;

        public MainWindow()
        {
            InitializeComponent();
            this.keyhook.StartKeyEvent(this.KeyEvent);
        }
        private void KeyEvent(uint uiKey)
        {
            this.latest_keys = utils.GetKeysWithBuffer(uiKey);
            string keys_str = utils.KeysToStr(this.latest_keys);
            Debug.WriteLine(keys_str);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var config = new Config();
            config.Show();
        }
    }
}
