// Decompiled with JetBrains decompiler
// Type: Manager.GameSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UploaderSystem;

namespace Manager
{
  public sealed class GameSystem : Singleton<GameSystem>
  {
    public readonly Version UserInfoFileVersion = new Version(0, 0, 0);
    public readonly Version NetSaveFileVersion = new Version(0, 0, 0);
    public readonly string[] cultureNames = new string[1]
    {
      "ja-JP"
    };
    public string networkSceneName = string.Empty;
    public GameSystem.DownloadInfo downloadInfo = new GameSystem.DownloadInfo();
    public GameSystem.ApplauseInfo applauseInfo = new GameSystem.ApplauseInfo();
    public const string BGDir = "bg/";
    public const string CardFDir = "cardframe/Front/";
    public const string CardBDir = "cardframe/Back/";
    public const string SystemDir = "system/";
    public const string VersionFile = "version.dat";
    public const string SetupFile = "setup.xml";
    public const string HNFile = "hn.dat";
    public const string UserInfoFile = "userinfo.dat";
    public const string NetSaveFile = "netsave.dat";
    public const string DownloadInfoFile = "dli.sav";
    public const string ApplauseInfoFile = "ali.sav";
    public const int ProductNo = 100;
    public const string CharaFileMark = "【AIS_Chara】";
    public const string ClothesFileMark = "【AIS_Clothes】";
    public const string HousingFileMark = "【AIS_Housing】";
    public const string StudioFileMark = "【AIS_Studio】";
    public const string GameSystemVersion = "1.0.6";
    public bool agreePolicy;

    public string EncryptedMacAddress { get; private set; }

    public string UserUUID { get; private set; }

    public void SetUserUUID(string uuid)
    {
      this.UserUUID = uuid;
    }

    public string UserPasswd { get; private set; }

    public string HandleName { get; private set; }

    public void SaveHandleName(string hn)
    {
      this.HandleName = hn;
      File.WriteAllBytes(UserData.Create("system/") + "hn.dat", YS_Assist.EncryptAES(Encoding.UTF8.GetBytes(this.HandleName), "illusion", "ai-syoujyo"));
    }

    public void LoadHandleName()
    {
      string path = UserData.Create("system/") + "hn.dat";
      if (File.Exists(path))
      {
        try
        {
          this.HandleName = Encoding.UTF8.GetString(YS_Assist.DecryptAES(File.ReadAllBytes(path), "illusion", "ai-syoujyo"));
        }
        catch (Exception ex)
        {
          this.HandleName = string.Empty;
        }
      }
      else
        this.HandleName = string.Empty;
    }

    public void GenerateUserInfo(bool forceGenerate = false)
    {
      if (!forceGenerate && this.LoadIdAndPass())
        return;
      this.UserUUID = YS_Assist.CreateUUID();
      this.UserPasswd = YS_Assist.GeneratePassword62(16);
      this.SaveIdAndPass();
    }

    public void SaveIdAndPass()
    {
      File.WriteAllLines(UserData.Create("system/") + "userinfo.dat", new string[3]
      {
        this.UserInfoFileVersion.ToString(),
        this.UserUUID,
        this.UserPasswd
      });
    }

    public bool LoadIdAndPass()
    {
      try
      {
        string path = UserData.Create("system/") + "userinfo.dat";
        if (!File.Exists(path))
          return false;
        string[] strArray = File.ReadAllLines(path);
        if (((IList<string>) strArray).IsNullOrEmpty<string>())
          return false;
        Version version = new Version(strArray[0]);
        if (strArray.Length != 3 || (strArray[1].IsNullOrEmpty() || strArray[2].IsNullOrEmpty()))
          return false;
        this.UserUUID = strArray[1];
        this.UserPasswd = strArray[2];
        return true;
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    public Version GameVersion { get; private set; } = new Version("1.0.0");

    public void SaveVersion()
    {
      File.WriteAllText(DefaultData.Create("system/") + "version.dat", "1.0.6");
      Debug.LogFormat("SystemVersion : {0}", new object[1]
      {
        (object) "1.0.6"
      });
    }

    public void LoadVersion()
    {
      string path = DefaultData.Path + "system//version.dat";
      if (!File.Exists(path))
        return;
      string str = File.ReadAllText(path);
      if (str.IsNullOrEmpty())
        return;
      this.GameVersion = new Version(str);
    }

    public GameSystem.Language language { get; private set; }

    public int languageInt
    {
      get
      {
        return (int) this.language;
      }
    }

    public string cultrureName
    {
      get
      {
        return this.cultureNames[this.languageInt];
      }
    }

    private void LoadLanguage()
    {
      string str = UserData.Path + "setup.xml";
      if (!File.Exists(str))
      {
        this.language = GameSystem.Language.Japanese;
      }
      else
      {
        try
        {
          XElement xelement = XElement.Load(str);
          if (xelement == null)
            return;
          foreach (XElement element in xelement.Elements())
          {
            if (element.Name.ToString() == "Language")
            {
              this.language = (GameSystem.Language) int.Parse(element.Value);
              break;
            }
          }
        }
        catch (XmlException ex)
        {
        }
      }
    }

    public void SaveNetworkSetting()
    {
      using (FileStream fileStream = new FileStream(UserData.Create("system/") + "netsave.dat", FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          binaryWriter.Write(this.NetSaveFileVersion.ToString());
          binaryWriter.Write(this.agreePolicy);
        }
      }
    }

    public void LoadNetworkSetting()
    {
      string path = UserData.Path + "system//netsave.dat";
      if (!File.Exists(path))
        return;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          Version version = new Version(binaryReader.ReadString());
          this.agreePolicy = binaryReader.ReadBoolean();
        }
      }
    }

    public bool SaveDownloadInfo()
    {
      using (FileStream fileStream = new FileStream(UserData.Create("system/") + "dli.sav", FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          HashSet<string> hsChara = this.downloadInfo.hsChara;
          int count1 = hsChara.Count;
          binaryWriter.Write(count1);
          foreach (string str in hsChara)
            binaryWriter.Write(str);
          HashSet<string> hsHousing = this.downloadInfo.hsHousing;
          int count2 = hsHousing.Count;
          binaryWriter.Write(count2);
          foreach (string str in hsHousing)
            binaryWriter.Write(str);
        }
      }
      return true;
    }

    public bool LoadDownloadInfo()
    {
      string path = UserData.Path + "system/dli.sav";
      if (!File.Exists(path))
        return false;
      this.downloadInfo.hsChara.Clear();
      this.downloadInfo.hsHousing.Clear();
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          int num1 = binaryReader.ReadInt32();
          for (int index = 0; index < num1; ++index)
            this.downloadInfo.hsChara.Add(binaryReader.ReadString());
          int num2 = binaryReader.ReadInt32();
          for (int index = 0; index < num2; ++index)
            this.downloadInfo.hsHousing.Add(binaryReader.ReadString());
        }
      }
      return true;
    }

    public void AddDownload(DataType type, string uuid)
    {
      switch (type)
      {
        case DataType.Chara:
          this.downloadInfo.hsChara.Add(uuid);
          break;
        case DataType.Housing:
          this.downloadInfo.hsHousing.Add(uuid);
          break;
        default:
          return;
      }
      this.SaveDownloadInfo();
    }

    public bool IsDownload(DataType type, string uuid)
    {
      if (type == DataType.Chara)
        return this.downloadInfo.hsChara.Contains(uuid);
      return type == DataType.Housing && this.downloadInfo.hsHousing.Contains(uuid);
    }

    public bool SaveApplauseInfo()
    {
      using (FileStream fileStream = new FileStream(UserData.Create("system/") + "ali.sav", FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          HashSet<string> hsChara = this.applauseInfo.hsChara;
          int count1 = hsChara.Count;
          binaryWriter.Write(count1);
          foreach (string str in hsChara)
            binaryWriter.Write(str);
          HashSet<string> hsHousing = this.applauseInfo.hsHousing;
          int count2 = hsHousing.Count;
          binaryWriter.Write(count2);
          foreach (string str in hsHousing)
            binaryWriter.Write(str);
        }
      }
      return true;
    }

    public bool LoadApplauseInfo()
    {
      string path = UserData.Path + "system/ali.sav";
      if (!File.Exists(path))
        return false;
      this.applauseInfo.hsChara.Clear();
      this.applauseInfo.hsHousing.Clear();
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          int num1 = binaryReader.ReadInt32();
          for (int index = 0; index < num1; ++index)
            this.applauseInfo.hsChara.Add(binaryReader.ReadString());
          int num2 = binaryReader.ReadInt32();
          for (int index = 0; index < num2; ++index)
            this.applauseInfo.hsHousing.Add(binaryReader.ReadString());
        }
      }
      return true;
    }

    public void AddApplause(DataType type, string uuid)
    {
      switch (type)
      {
        case DataType.Chara:
          this.applauseInfo.hsChara.Add(uuid);
          break;
        case DataType.Housing:
          this.applauseInfo.hsHousing.Add(uuid);
          break;
        default:
          return;
      }
      this.SaveApplauseInfo();
    }

    public bool IsApplause(DataType type, string uuid)
    {
      if (type == DataType.Chara)
        return this.applauseInfo.hsChara.Contains(uuid);
      return type == DataType.Housing && this.applauseInfo.hsHousing.Contains(uuid);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      string path1 = UserData.Path + "bg/";
      if (!System.IO.Directory.Exists(path1))
        System.IO.Directory.CreateDirectory(path1);
      string path2 = UserData.Path + "cardframe/Back/";
      if (!System.IO.Directory.Exists(path2))
        System.IO.Directory.CreateDirectory(path2);
      string path3 = UserData.Path + "cardframe/Front/";
      if (!System.IO.Directory.Exists(path3))
        System.IO.Directory.CreateDirectory(path3);
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.EncryptedMacAddress = YS_Assist.CreateIrregularStringFromMacAddress();
      this.GenerateUserInfo(false);
      this.LoadHandleName();
      this.GameVersion = new Version(0, 0, 0);
      this.LoadVersion();
      this.LoadLanguage();
      this.LoadNetworkSetting();
      this.LoadDownloadInfo();
      this.LoadApplauseInfo();
    }

    private void OnApplicationQuit()
    {
    }

    public enum Language
    {
      Japanese,
    }

    public class CultureScope : IDisposable
    {
      private CultureInfo culture;

      public CultureScope()
      {
        string cultrureName = Singleton<GameSystem>.Instance.cultrureName;
        this.culture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultrureName);
      }

      void IDisposable.Dispose()
      {
        Thread.CurrentThread.CurrentCulture = this.culture;
      }
    }

    public class DownloadInfo
    {
      public HashSet<string> hsChara = new HashSet<string>();
      public HashSet<string> hsHousing = new HashSet<string>();
    }

    public class ApplauseInfo
    {
      public HashSet<string> hsChara = new HashSet<string>();
      public HashSet<string> hsHousing = new HashSet<string>();
    }
  }
}
