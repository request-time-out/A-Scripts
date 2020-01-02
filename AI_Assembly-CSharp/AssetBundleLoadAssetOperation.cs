// Decompiled with JetBrains decompiler
// Type: AssetBundleLoadAssetOperation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
{
  public abstract bool IsEmpty();

  public abstract T GetAsset<T>() where T : Object;

  public abstract T[] GetAllAssets<T>() where T : Object;
}
