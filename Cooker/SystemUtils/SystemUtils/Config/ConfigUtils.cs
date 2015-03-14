using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cooker
{
    namespace SystemUtils
    {
        internal class ConfigUtils
        {
            // 将str转换为KConfigRootType类型
            public static ConfigRootType ConvertStringToRootType(string str)
            {
                str = str.ToUpper();
                if (str == ConfigRootString.SoftwareRoot)
                    return ConfigRootType.crtSystem;
                else if (str == ConfigRootString.DepartmentRoot)
                    return ConfigRootType.crtDepartment;
                else if (str == ConfigRootString.OperationRoot)
                    return ConfigRootType.crtOperation;
                else if (str == ConfigRootString.MachineRoot)
                    return ConfigRootType.crtMachine;
                else if (str == ConfigRootString.SoftwareRoot)
                    return ConfigRootType.crtSoftware;
                else if (str == ConfigRootString.UserRoot)
                    return ConfigRootType.crtUser;
                else if (str == ConfigRootString.GroupRoot)
                    return ConfigRootType.crtGroup;
                else return ConfigRootType.crtUnknown;
            }

            // 将KConfigRootType类型转换为string
            public static string ConvertRootTypeToString(ConfigRootType type)
            {
                switch (type)
                {
                    case ConfigRootType.crtSystem:
                        return ConfigRootString.SystemRoot;
                    case ConfigRootType.crtDepartment:
                        return ConfigRootString.DepartmentRoot;
                    case ConfigRootType.crtOperation:
                        return ConfigRootString.OperationRoot;
                    case ConfigRootType.crtMachine:
                        return ConfigRootString.MachineRoot;
                    case ConfigRootType.crtSoftware:
                        return ConfigRootString.SoftwareRoot;
                    case ConfigRootType.crtUser:
                        return ConfigRootString.UserRoot;
                    case ConfigRootType.crtGroup:
                        return ConfigRootString.GroupRoot;
                    default:
                        return null;
                }
            }
        }
    }
}
