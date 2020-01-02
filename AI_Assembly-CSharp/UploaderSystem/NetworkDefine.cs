// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetworkDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UploaderSystem
{
  public static class NetworkDefine
  {
    public static readonly Version NetInfoVersion = new Version("1.0.0");
    public static readonly Color colorWhite = new Color(0.9215686f, 0.8862745f, 0.8431373f);
    public const string CacheFileMark = "【CacheFile】";
    public const int CacheFileVersion = 100;
    public const string CacheSettingPath = "cache/cachesetting.dat";
    public const string CacheCharaDir = "cache/chara/";
    public const string CacheHousingDir = "cache/housing/";
    public const string cryptPW = "aisyoujyo";
    public const string cryptSALT = "phpaddress";
    public const string AIS_Check_URLFile = "ais_check_url.dat";
    public const string AIS_Version_URLFile = "ais_version_url.dat";
    public const string AIS_System_URLFile = "ais_system_url.dat";
    public const string AIS_UploadChara_URLFile = "ais_uploadChara_url.dat";
    public const string AIS_UploadHousing_URLFile = "ais_uploadHousing_url.dat";
  }
}
