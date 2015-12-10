using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Huahua.Model
{
    /// <summary>
    /// 环境变量单元
    /// </summary>
    [XmlRoot]
    public class EnvUnit : INotifyPropertyChanged
    {
        private string name;
        private string name_;
        private List<string> paths;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 环境变量名称
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [XmlIgnore]
        public string Name_
        {
            get { return name_; }
            private set { name_ = value; }
        }

        /// <summary>
        /// 环境变量值
        /// </summary>
        [XmlArray]
        public List<string> Paths
        {
            get { return paths; }
            set
            {
                paths = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Paths"));
            }
        }

        public EnvUnit()
        {
            Name = Name_ = string.Empty;
            Paths = new List<string>();
        }

        public EnvUnit(DictionaryEntry kv)
            : this()
        {
            string k = (string)kv.Key;
            string v = (string)kv.Value;
            Name = Name_ = k;
            Paths.AddRange(v.Split(';'));
        }

        public string ToPathString()
        {
            return string.Join(";", paths.ToArray());
        }

        public override string ToString()
        {
            return string.Format("[{0}]:[{1}]", name, ToPathString());
        }

        public override bool Equals(object obj)
        {
            EnvUnit rh = obj as EnvUnit;
            if (rh == null)
                return false;

            if ((Name == rh.Name) && (ToPathString() == rh.ToPathString()))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 环境变量组
    /// 通常一个Application有三个组：Process/User/Machine
    /// 分别对应 当前程序/当前用户/系统
    /// 由于需要修改系统环境变量，所以需要请求管理员权限
    /// </summary>
    [XmlRoot]
    public class EnvGroup : INotifyPropertyChanged
    {
        private string target;
        private ObservableCollection<EnvUnit> envUnits;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 环境变量组名
        /// </summary>
        [XmlAttribute]
        public string Target
        {
            get { return target; }
            set
            {
                target = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Target"));
            }
        }

        [XmlArray]
        public ObservableCollection<EnvUnit> EnvUnits
        {
            get { return envUnits; }
            set
            {
                envUnits = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("EnvUnits"));
            }
        }

        public EnvGroup()
        {
            EnvUnits = new ObservableCollection<EnvUnit>();
        }

        public EnvGroup(string tag, IDictionary col)
            : this()
        {
            Target = tag;
            foreach (DictionaryEntry kv in col)
            {
                EnvUnits.Add(new EnvUnit(kv));
            }
        }
    }

    /// <summary>
    /// 用于序列化XML
    /// </summary>
    [XmlRoot("EnvRoot")]
    public class EnvVar : INotifyPropertyChanged
    {
        private ObservableCollection<EnvGroup> envGroups;

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlArray("EnvGroups")]
        public ObservableCollection<EnvGroup> EnvGroups
        {
            get { return envGroups; }
            set
            {
                envGroups = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("EnvGroups"));
            }
        }

        public EnvVar()
        {
            EnvGroups = new ObservableCollection<EnvGroup>();
        }
    }
}
