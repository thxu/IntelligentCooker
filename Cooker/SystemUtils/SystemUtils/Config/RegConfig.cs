using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;

namespace Cooker
{
    namespace SystemUtils
    {
        internal class RegistryConfig : Config
        {
            public RegistryConfig(RegistryKey rootKey, string rootSection)
            {
                FReg = rootKey;
                FRootSection = rootSection;
                FRootType = ConfigRootType.crtSoftware;
                FRootId = 0;
            }
            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root="SYSTEM"或"SOFTWARE"时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public override void SetRoot(string root, int id)
            {
                FRootType = ConfigUtils.ConvertStringToRootType(root);
                FRootId = id;
            }
            /*-------------------------------------------------------------------------
                设定当前配置信息的根部分为"root\id"，当root=crtSystem或crtSoftware时，id没有
                意义。之后的读写操作都在此根下。
            --------------------------------------------------------------------------*/
            public override void SetRoot(ConfigRootType root, int id)
            {
                FRootType = root;
                FRootId = id;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，包括sub-section和自身
            --------------------------------------------------------------------------*/
            public override void EraseSection(string section)
            {
                if (FRootType != ConfigRootType.crtSoftware) return;
                try
                {
                    FReg.DeleteSubKeyTree(FRootSection + "\\" + section);
                }
                catch(Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下所有配置值，不包括sub-section和自身
            --------------------------------------------------------------------------*/
            public override void ClearSectionValues(string section)
            {
                if (FRootType != ConfigRootType.crtSoftware) return;
                StringCollection Idents = new StringCollection();
                try
                {
                    ReadSection(section, ref Idents);
                    for(int i=0; i< Idents.Count; i++)
                    {
                        DeleteValue(section, Idents[i]);
                    }
                }
               catch(Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有配置名称，不包括sub-section，放在strings中
            --------------------------------------------------------------------------*/
            public override void ReadSection(string section, ref StringCollection strings)
            {
                strings.Clear();
                if (FRootType != ConfigRootType.crtSoftware) return;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    foreach (string valueName in key.GetValueNames())
                    {
                        strings.Add(valueName);
                    }
                    key.Close();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读出section下所有直接的sub-section名称，放在strings中。
                sub-section名称中不包含'\'。
            --------------------------------------------------------------------------*/
            public override void ReadSections(string section, ref StringCollection strings)
            {
                strings.Clear();
                if (FRootType != ConfigRootType.crtSoftware) return;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        strings.Add(subKeyName);
                    }
                    key.Close();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，删除section下名为ident的配置信息
            --------------------------------------------------------------------------*/
            public override void DeleteValue(string section, string ident)
            {
                if (FRootType != ConfigRootType.crtSoftware) return;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    key.DeleteValue(ident);
                    key.Close();
                }
                catch(Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的字符串型值，
                如果配置信息不可访问或者为空或者不是字符串类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override string ReadString(string section, string ident, string defVal)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return defVal;
                string Val = "";
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    Val = key.GetValue(ident, defVal).ToString();
                    key.Close();
                    if (Val == "") 
                        Val = defVal;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
                return Val;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的整型值，
                如果配置信息不可访问或者不是整数类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override int ReadInteger(string section, string ident, int defVal)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return defVal;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    int Val = Convert.ToInt32(key.GetValue(ident, defVal)) ;
                    key.Close();
                    return Val;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
                return defVal;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，读取section下名为ident的配置信息的bool型值，
                如果配置信息不可访问或者不是bool类型，则返回缺省值defVal
            --------------------------------------------------------------------------*/
            public override bool ReadBool(string section, string ident, bool defVal)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return defVal;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    int Val = (int)key.GetValue(ident,Convert.ToInt32(defVal));
                    key.Close();
                    if (Val == 0)
                        return false;
                    else
                        return true;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
                return defVal;
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的字符串型值为value
            --------------------------------------------------------------------------*/
            public override void WriteString(string section, string ident, string value)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return ;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    key.SetValue(ident, value, RegistryValueKind.String);
                    key.Close();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的整型值为value
            --------------------------------------------------------------------------*/
            public override void WriteInteger(string section, string ident, int value)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return ;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    key.SetValue(ident, value, RegistryValueKind.DWord);
                    key.Close();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            /*-------------------------------------------------------------------------
                在当前的Root下，设置section下名为ident的配置信息的bool型值为value
            --------------------------------------------------------------------------*/
            public override void WriteBool(string section, string ident, bool value)
            {
                if (FRootType != ConfigRootType.crtSoftware) 
                    return;
                try
                {
                    RegistryKey key = OpenSubKey(FReg,FRootSection + "\\" + section, true);
                    if(value)
                        key.SetValue(ident, 1, RegistryValueKind.DWord);
                    else
                        key.SetValue(ident, 0, RegistryValueKind.DWord);
                    key.Close();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            private RegistryKey OpenSubKey(RegistryKey rootKey,string section, bool writable)
            {
                RegistryKey key = null;
                try
                {
                    key = rootKey.OpenSubKey(section, writable);
                    if (key == null)
                        key = rootKey.CreateSubKey(section);                    
                }
                catch(Exception e)
                {
                    Logger.LogException(e);
                }
                return key;
            }
            private RegistryKey FReg;
            private string FRootSection;
            private ConfigRootType FRootType;
            private int FRootId; 
        }
    }
}
