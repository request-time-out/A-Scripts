// Decompiled with JetBrains decompiler
// Type: AIProject.AssetUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEx;

namespace AIProject
{
  public static class AssetUtility
  {
    public static T LoadAsset<T>(AssetBundleInfo info) where T : Object
    {
      return AssetUtility.LoadAsset<T>((string) info.assetbundle, (string) info.asset, (string) info.manifest);
    }

    public static T LoadAsset<T>(string assetbundleName, string assetName, string manifestName = "") where T : Object
    {
      manifestName = !manifestName.IsNullOrEmpty() ? manifestName : (string) null;
      T obj = CommonLib.LoadAsset<T>(assetbundleName, assetName, false, manifestName);
      AssetBundleManager.UnloadAssetBundle(assetbundleName, true, manifestName, false);
      return obj;
    }
  }
}
