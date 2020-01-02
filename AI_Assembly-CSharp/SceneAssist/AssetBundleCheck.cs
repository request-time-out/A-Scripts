// Decompiled with JetBrains decompiler
// Type: SceneAssist.AssetBundleCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SceneAssist
{
  public static class AssetBundleCheck
  {
    public static bool IsSimulation
    {
      get
      {
        return false;
      }
    }

    public static string[] FindAllAssetName(
      string assetBundleName,
      string _regex,
      bool _WithExtension = true,
      RegexOptions _options = RegexOptions.None)
    {
      _regex = _regex.ToLower();
      AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetBundleManager.BaseDownloadingURL + assetBundleName);
      string[] array;
      if (_WithExtension)
      {
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        // ISSUE: reference to a compiler-generated field
        if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache0 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0;
        array = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache0).Where<string>((Func<string, bool>) (v => AssetBundleCheck.CheckRegex(v, _regex, _options))).ToArray<string>();
      }
      else
      {
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        // ISSUE: reference to a compiler-generated field
        if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1 = new Func<string, string>(Path.GetFileNameWithoutExtension);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache1 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1;
        array = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache1).Where<string>((Func<string, bool>) (v => AssetBundleCheck.CheckRegex(v, _regex, _options))).ToArray<string>();
      }
      assetBundle.Unload(true);
      return array;
    }

    private static bool CheckRegex(string _value, string _regex, RegexOptions _options)
    {
      return Regex.Match(_value, _regex, _options).Success;
    }

    public static bool FindFile(string _assetBundleName, string _fineName, bool _WithExtension = false)
    {
      _fineName = _fineName.ToLower();
      AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetBundleManager.BaseDownloadingURL + _assetBundleName);
      if (Object.op_Equality((Object) assetBundle, (Object) null))
        return false;
      bool flag;
      if (_WithExtension)
      {
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        // ISSUE: reference to a compiler-generated field
        if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetBundleCheck.\u003C\u003Ef__mg\u0024cache2 = new Func<string, string>(Path.GetFileName);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache2 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache2;
        flag = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache2).ToList<string>().FindIndex((Predicate<string>) (s => s == _fineName)) != -1;
      }
      else
      {
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        // ISSUE: reference to a compiler-generated field
        if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetBundleCheck.\u003C\u003Ef__mg\u0024cache3 = new Func<string, string>(Path.GetFileNameWithoutExtension);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache3 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache3;
        flag = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache3).ToList<string>().FindIndex((Predicate<string>) (s => s == _fineName)) != -1;
      }
      assetBundle.Unload(true);
      return flag;
    }
  }
}
