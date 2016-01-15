using Huahua.Control;
using Huahua.Model;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Huahua.View
{
    /// <summary>
    /// UIMain.xaml 的交互逻辑
    /// </summary>
    public partial class UIMain : MetroWindow
    {
        private EnvVar env;

        public event PropertyChangedEventHandler PropertyChanged;

        public EnvVar Env
        {
            get { return env; }
            set
            {
                env = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Env"));
            }
        }

        public UIMain()
        {
            InitializeComponent();
            DataContext = this;
            LoadFromSystem();
        }

        public UIMain(string xmlPath)
            : this()
        {
            if (!File.Exists(xmlPath))
                return;
            else
                LoadFromFile(xmlPath);
        }

        private void OnLoadFromFile(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML File|*.xml|All File|*.*";
            dialog.DefaultExt = ".xml";
            if (dialog.ShowDialog() == true)
                LoadFromFile(dialog.FileName);
        }

        private void OnLoadFromSystem(object sender, RoutedEventArgs e)
        {
            LoadFromSystem();
        }

        private void OnRestore(object sender, RoutedEventArgs e)
        {
            RestoreToSystem();
        }

        private void OnExportCurrent(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML File|*.xml|All File|*.*";
            dialog.DefaultExt = ".xml";
            if (dialog.ShowDialog() == true)
                EnvControl.SaveEnvXml(dialog.FileName, Env);
        }

        private void OnExportSystem(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML File|*.xml|All File|*.*";
            dialog.DefaultExt = ".xml";
            if (dialog.ShowDialog() == true)
                EnvControl.SaveEnvXml(dialog.FileName, EnvControl.GetSystemEnv());
        }

        private void LoadFromFile(string filePath)
        {
            Env = EnvControl.LoadEnvXml(filePath);
        }

        private void LoadFromSystem()
        {
            Env = EnvControl.GetSystemEnv();
        }

        private void RestoreToSystem()
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Save current Environment Variables to system ?\r\nIt could not be undo.\r\nPress 'Yes' to continue", "", MessageBoxButton.YesNo, MessageBoxImage.Question))
                EnvControl.SetSystemEnv(Env);
        }
    }
}
