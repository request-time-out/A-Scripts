// Decompiled with JetBrains decompiler
// Type: LoadedAssetBundle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LoadedAssetBundle
{
  public AssetBundle m_AssetBundle;
  public uint m_ReferencedCount;

  public LoadedAssetBundle(AssetBundle assetBundle)
  {
    this.m_AssetBundle = assetBundle;
    this.m_ReferencedCount = 1U;
  }
}
