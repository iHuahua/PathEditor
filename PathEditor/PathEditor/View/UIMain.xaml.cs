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
        private EnvVar env = EnvControl.GetSystemEnv();

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
            Env = EnvControl.GetSystemEnv();
        }

        public UIMain(string xmlPath)
            : this()
        {
            if (!File.Exists(xmlPath))
                return;
        }

        
    }
}
