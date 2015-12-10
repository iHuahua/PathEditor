using Huahua.Control;
using Huahua.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        }

        public UIMain(string xmlPath)
            : this()
        {
            if (!File.Exists(xmlPath))
                return;
            Env = EnvControl.GetSystemEnv();
        }

        private void OnRestore(object sender, RoutedEventArgs e)
        {
            EnvControl.SetSystemEnv(Env);
        }

        private void OnExport(object sender, RoutedEventArgs e)
        {
            EnvControl.SaveEnvXml("path.xml", Env);
        }

        private void OnExportSystem(object sender, RoutedEventArgs e)
        {
            EnvControl.SaveEnvXml("path.xml", EnvControl.GetSystemEnv());
        }
    }
}
