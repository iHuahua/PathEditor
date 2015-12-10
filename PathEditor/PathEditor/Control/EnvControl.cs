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
            catch
            {
                env = null;
            }

            return env;
        }

        public static void SetSystemEnv(EnvVar env)
        {
            if (env == null)
                return;

            foreach (var modifyGroup in env.EnvGroups)
            {
                if (modifyGroup.EnvUnits.Count <= 0)
                    continue;

                EnvironmentVariableTarget target;
                if (!Enum.TryParse(modifyGroup.Target, out target))
                    continue;

                EnvGroup sysGroup = new EnvGroup(target.ToString(), Environment.GetEnvironmentVariables(target));

                // 删除系统中不包含在Group里的变量
                for (int uid = 0; uid < sysGroup.EnvUnits.Count; uid++)
                {
                    EnvUnit unit = sysGroup.EnvUnits[uid];
                    if (!modifyGroup.EnvUnits.Contains(unit))
                    {
                        Environment.SetEnvironmentVariable(unit.Name, string.Empty, target);
                        sysGroup.EnvUnits.Remove(unit);
                        --uid;
                    }
                }

                // 添加Group里的新变量
                for (int uid = 0; uid < modifyGroup.EnvUnits.Count; uid++)
                {
                    EnvUnit unit = modifyGroup.EnvUnits[uid];
                    if (!sysGroup.EnvUnits.Contains(unit))
                    {
                        Environment.SetEnvironmentVariable(unit.Name, unit.ToPathString(), target);
                    }
                }
            }

        }

        public static EnvVar LoadEnvXml(string fn)
        {
            if (!File.Exists(fn))
                return null;

            EnvVar env = null;

            try
            {
                using (StreamReader reader = new StreamReader(fn))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(EnvVar));
                    env = (EnvVar)xmlSerializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch
            {
                env = null;
            }

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
