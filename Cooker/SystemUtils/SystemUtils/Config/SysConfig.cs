using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Win32;
namespace Cooker
{
    namespace SystemUtils
    {
        internal class SystemConfig : Config
        {
            public SystemConfig(string connectString, string company, string software)
            {
                FRegConfig = null;
                FRootType = ConfigRootType.crtUnknown;
                FRootId = 0;
                FCompany = company;
                FSoftware = software;
                FConnectString = connectString;
            }
            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root="SYSTEM"或"SOFTWARE"时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public override void SetRoot(string root, int id)
            {
                FMutex.WaitOne();
                FRootType = ConfigUtils.ConvertStringToRootType(root);
                FRootId = id;
                FMutex.ReleaseMutex();
            }
            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root=crtSystem或crtSoftware时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public override void SetRoot(ConfigRootType root, int id)
            {
                FMutex.WaitOne();
                FRootType = root;
                FRootId = id;
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，包括sub-section和自身
            --------------------------------------------------------------------------*/
            public override void EraseSection(string section)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.EraseSection(section);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，不包括sub-section和自身
            --------------------------------------------------------------------------*/
            public override void ClearSectionValues(string section)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.ClearSectionValues(section);
                FMutex.ReleaseMutex();        
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有配置名称，不包括sub-section，放在strings中
            --------------------------------------------------------------------------*/
            public override void ReadSection(string section, ref StringCollection strings)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.ReadSection(section, ref strings);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有直接的sub-section名称，放在strings中。
                sub-section名称中不包含'\'。
            --------------------------------------------------------------------------*/
            public override void ReadSections(string section, ref StringCollection strings)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.ReadSections(section,ref strings);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下名为ident的配置信息
            --------------------------------------------------------------------------*/
            public override void DeleteValue(string section, string ident)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.DeleteValue(section, ident);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的字符串型值，
                如果配置信息不可访问或者为空或者不是字符串类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override string ReadString(string section, string ident, string defVal)
            {
                string Value;
                Config config = null;
                FMutex.WaitOne();
                if (!DemandConfig(ref config)) 
                    Value = defVal;
                else
                    Value = config.ReadString(section, ident, defVal);
                FMutex.ReleaseMutex();
                return Value;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的整型值，
                如果配置信息不可访问或者不是整数类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override int ReadInteger(string section, string ident, int defVal)
            {
                int Value;
                Config config = null;
                FMutex.WaitOne();
                if (!DemandConfig(ref config))
                    Value = defVal;
                else
                    Value = config.ReadInteger(section, ident, defVal);
                FMutex.ReleaseMutex();
                return Value;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的bool型值，
                如果配置信息不可访问或者不是bool类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override bool ReadBool(string section, string ident, bool defVal)
            {
                bool Value;
                Config config = null;
                FMutex.WaitOne();
                if (!DemandConfig(ref config))
                    Value = defVal;
                else
                    Value = config.ReadBool(section, ident, defVal);
                FMutex.ReleaseMutex();
                return Value;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的字符串型值为value
            --------------------------------------------------------------------------*/
            public override void WriteString(string section, string ident, string value)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.WriteString(section, ident, value);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的整型值为value
            --------------------------------------------------------------------------*/
            public override void WriteInteger(string section, string ident, int value)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.WriteInteger(section, ident, value);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的bool型值为value
            --------------------------------------------------------------------------*/
            public override void WriteBool(string section, string ident, bool value)
            {
                Config config = null;
                FMutex.WaitOne();
                if (DemandConfig(ref config))
                    config.WriteBool(section, ident, value);
                FMutex.ReleaseMutex();
            }

            /*-------------------------------------------------------------------------
                按照当前的root类型，选择合适的config对象，如果对象不存在则创建
            --------------------------------------------------------------------------*/
            private bool DemandConfig(ref Config config)
            {
                if( FRootType == ConfigRootType.crtUnknown) 
                    return false;
                if( FRootType == ConfigRootType.crtSoftware )
                {
                    if( FRegConfig == null )
                    {
                        try
                        {
                            FRegConfig = new RegistryConfig(Registry.LocalMachine, "Software\\"+FCompany+"\\"+FSoftware);
                        }
                        catch(Exception e )
                        {
                            Logger.LogException(e);
                            return false;
                        }
                    }
                    config = FRegConfig;
                }
                config.SetRoot(FRootType, FRootId);
                return true;
            }

            private ConfigRootType FRootType;
            private int FRootId;
            private string FConnectString;
            private string FCompany;
            private string FSoftware;
            private RegistryConfig FRegConfig;
            private static Mutex FMutex = new Mutex();

        }
    }
}
