// Decompiled with JetBrains decompiler
// Type: Studio.Utility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Studio
{
  internal static class Utility
  {
    public static bool SetColor(ref Color currentValue, Color newValue)
    {
      if (currentValue.r == newValue.r && currentValue.g == newValue.g && (currentValue.b == newValue.b && currentValue.a == newValue.a))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    {
      if (currentValue.Equals((object) newValue))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
    {
      if ((object) currentValue == null && (object) newValue == null || (object) currentValue != null && currentValue.Equals((object) newValue))
        return false;
      currentValue = newValue;
      return true;
    }

    public static void SaveColor(BinaryWriter _writer, Color _color)
    {
      _writer.Write((float) _color.r);
      _writer.Write((float) _color.g);
      _writer.Write((float) _color.b);
      _writer.Write((float) _color.a);
    }

    public static Color LoadColor(BinaryReader _reader)
    {
      Color color;
      color.r = (__Null) (double) _reader.ReadSingle();
      color.g = (__Null) (double) _reader.ReadSingle();
      color.b = (__Null) (double) _reader.ReadSingle();
      color.a = (__Null) (double) _reader.ReadSingle();
      return color;
    }

    public static T LoadAsset<T>(string _bundle, string _file, string _manifest) where T : Object
    {
      return CommonLib.LoadAsset<T>(_bundle, _file, true, _manifest);
    }

    public static float StringToFloat(string _text)
    {
      float result = 0.0f;
      return float.TryParse(_text, out result) ? result : 0.0f;
    }

    public static string GetCurrentTime()
    {
      DateTime now = DateTime.Now;
      return string.Format("{0}_{1:00}{2:00}_{3:00}{4:00}_{5:00}_{6:000}", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute, (object) now.Second, (object) now.Millisecond);
    }

    public static Color ConvertColor(int _r, int _g, int _b)
    {
      Color color;
      return ColorUtility.TryParseHtmlString(string.Format("#{0:X2}{1:X2}{2:X2}", (object) _r, (object) _g, (object) _b), ref color) ? color : Color.get_clear();
    }

    public static void PlaySE(SoundPack.SystemSE _systemSE)
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      Singleton<Resources>.Instance.SoundPack.Play(_systemSE);
    }

    public static string GetManifest(string _bundlePath, string _file)
    {
      if (AssetBundleCheck.IsSimulation)
        return string.Empty;
      foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.ManifestBundlePack.Where<KeyValuePair<string, AssetBundleManager.BundlePack>>((Func<KeyValuePair<string, AssetBundleManager.BundlePack>, bool>) (v => Regex.Match(v.Key, "studio(\\d*)").Success)))
      {
        if (((IEnumerable<string>) keyValuePair.Value.AssetBundleManifest.GetAllAssetBundles()).Any<string>((Func<string, bool>) (_s => _s == _bundlePath)))
          return keyValuePair.Key;
      }
      return string.Empty;
    }
  }
}
