// Decompiled with JetBrains decompiler
// Type: LoadedAssetBundleDependencies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public class LoadedAssetBundleDependencies
{
  public string m_Key;
  public int m_ReferencedCount;
  public string[] m_BundleNames;

  public LoadedAssetBundleDependencies(string key, string[] bundleNames)
  {
    this.m_Key = key;
    this.m_BundleNames = bundleNames;
    this.m_ReferencedCount = 1;
  }
}
