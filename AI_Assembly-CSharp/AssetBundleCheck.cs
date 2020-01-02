// Decompiled with JetBrains decompiler
// Type: AssetBundleCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class AssetBundleCheck
{
  public static bool IsSimulation
  {
    get
    {
      return false;
    }
  }

  public static bool IsFile(string assetBundleName, string fileName = "")
  {
    return File.Exists(AssetBundleManager.BaseDownloadingURL + assetBundleName);
  }

  public static bool IsManifest(string manifest)
  {
    return AssetBundleManager.ManifestBundlePack.ContainsKey(manifest);
  }

  public static bool IsManifestOrBundle(string bundle)
  {
    return AssetBundleManager.ManifestBundlePack.ContainsKey(bundle) || AssetBundleCheck.IsFile(bundle, string.Empty);
  }

  public static string[] GetAllAssetName(
    string assetBundleName,
    bool _WithExtension = true,
    string manifestAssetBundleName = null,
    bool isAllCheck = false)
  {
    if (manifestAssetBundleName == null && isAllCheck && AssetBundleManager.AllLoadedAssetBundleNames.Contains(assetBundleName))
    {
      foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.ManifestBundlePack)
      {
        LoadedAssetBundle loadedAssetBundle;
        if (keyValuePair.Value.LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle))
        {
          if (_WithExtension)
          {
            string[] allAssetNames = loadedAssetBundle.m_AssetBundle.GetAllAssetNames();
            // ISSUE: reference to a compiler-generated field
            if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
            }
            // ISSUE: reference to a compiler-generated field
            Func<string, string> fMgCache0 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache0;
            return ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache0).ToArray<string>();
          }
          string[] allAssetNames1 = loadedAssetBundle.m_AssetBundle.GetAllAssetNames();
          // ISSUE: reference to a compiler-generated field
          if (AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1 = new Func<string, string>(Path.GetFileNameWithoutExtension);
          }
          // ISSUE: reference to a compiler-generated field
          Func<string, string> fMgCache1 = AssetBundleCheck.\u003C\u003Ef__mg\u0024cache1;
          return ((IEnumerable<string>) allAssetNames1).Select<string, string>(fMgCache1).ToArray<string>();
        }
      }
    }
    LoadedAssetBundle loadedAssetBundle1 = AssetBundleManager.GetLoadedAssetBundle(assetBundleName, out string _, manifestAssetBundleName);
    AssetBundle assetBundle = loadedAssetBundle1 == null ? AssetBundle.LoadFromFile(AssetBundleManager.BaseDownloadingURL + assetBundleName) : loadedAssetBundle1.m_AssetBundle;
    string[] array;
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
      array = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache2).ToArray<string>();
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
      array = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache3).ToArray<string>();
    }
    if (loadedAssetBundle1 == null)
      assetBundle.Unload(true);
    return array;
  }
}
