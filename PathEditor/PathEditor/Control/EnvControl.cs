using Huahua.Model;
using System;
using System.IO;

namespace Huahua.Control
{
    public static class EnvControl
    {
        public static EnvVar GetSystemEnv()
        {
            EnvVar env = new EnvVar();
            try
            {
                foreach (EnvironmentVariableTarget tag in Enum.GetValues(typeof(EnvironmentVariableTarget)))
                {
                    if (tag == EnvironmentVariableTarget.Process)
                        continue;
                    env.EnvGroups.Add(new EnvGroup(tag.ToString(), Environment.GetEnvironmentVariables(tag)));
                }
            }
            catch { }

            return env;
        }

        public static void SetSystemEnv(EnvVar env)
        {
            throw new NotImplementedException("暂未实现");
        }

        public static EnvVar LoadEnvXml(string fn)
        {
            if (!File.Exists(fn))
                return null;

            EnvVar env = new EnvVar();

            try
            {
                using (StreamReader reader = new StreamReader(fn))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(env.GetType());
                    env = (EnvVar)xmlSerializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch { }

            return env;
        }

        public static bool SaveEnvXml(string fn, EnvVar env)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fn))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(env.GetType());
                    xmlSerializer.Serialize(writer, env);
                    writer.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
