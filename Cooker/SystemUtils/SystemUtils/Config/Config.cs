using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Cooker
{
    namespace SystemUtils
    {
        // 枚举所有可能的配置信息名称的root部分
        public enum ConfigRootType
        {
            crtUnknown = -1,    // 未知的root
            crtSystem = 0,      // SYSTEM
            crtDepartment = 1,  // DEPARTMENT
            crtOperation = 2,   // OPERATION
            crtMachine = 3,     // MACHINE
            crtSoftware = 4,    // SOFTWARE
            crtUser = 5,        // USER
            crtGroup = 6,       // GROUP
            crtRole = 7         // Role
        };
        public class ConfigRootString
        {
            // 定义合法配置信息名称的root串值的别名
            public static readonly string  SystemRoot = "SYSTEM";
            public static readonly string  DepartmentRoot = "DEPARTMENT";
            public static readonly string  OperationRoot = "OPERATION";
            public static readonly string  MachineRoot = "MACHINE";
            public static readonly string  SoftwareRoot = "SOFTWARE";
            public static readonly string  UserRoot = "USER";
            public static readonly string  GroupRoot = "GROUP";

            public static readonly string  StringType = "STRING";
            public static readonly string  IntegerType = "INTEGER";
            public static readonly string  BoolType = "BOOL";

            public static readonly string  TrueValue = "TRUE";
            public static readonly string  FalseValue = "FALSE";
        }
        public abstract class Config
        {

            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root="SYSTEM"或"SOFTWARE"时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public abstract void SetRoot(string root, int id);
            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root=crtSystem或crtSoftware时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public abstract void SetRoot(ConfigRootType root, int id);

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，包括sub-section和自身
            --------------------------------------------------------------------------*/
            public abstract void EraseSection(string section);

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，不包括sub-section和自身
            --------------------------------------------------------------------------*/
             public abstract void ClearSectionValues(string section);

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有配置名称，不包括sub-section，放在strings中
            --------------------------------------------------------------------------*/
            public abstract void ReadSection(string section, ref StringCollection  strings);

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有直接的sub-section名称，放在strings中。
                sub-section名称中不包含'\'。
            --------------------------------------------------------------------------*/
            public abstract void ReadSections(string section, ref StringCollection strings);

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下名为ident的配置信息
            --------------------------------------------------------------------------*/
            public abstract void DeleteValue(string section, string ident);

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的字符串型值，
                如果配置信息不可访问或者为空或者不是字符串类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public abstract string ReadString(string section, string ident, string defVal);

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的整型值，
                如果配置信息不可访问或者不是整数类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public abstract int ReadInteger(string section, string ident, int defVal);

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的bool型值，
                如果配置信息不可访问或者不是bool类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public abstract  bool ReadBool(string section, string ident, bool defVal);

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的字符串型值为value
            --------------------------------------------------------------------------*/
            public abstract void WriteString(string section, string ident, string value);

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的整型值为value
            --------------------------------------------------------------------------*/
            public abstract void WriteInteger(string section, string ident, int value);

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的bool型值为value
            --------------------------------------------------------------------------*/
            public abstract void WriteBool(string section, string ident, bool value);

        }
    }
}
