// Decompiled with JetBrains decompiler
// Type: AssetBundleCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AssetBundleCreate
{
  public AssetBundleCreateRequest m_CreateRequest;
  public uint m_ReferencedCount;

  public AssetBundleCreate(AssetBundleCreateRequest request)
  {
    this.m_CreateRequest = request;
    this.m_ReferencedCount = 1U;
  }
}
