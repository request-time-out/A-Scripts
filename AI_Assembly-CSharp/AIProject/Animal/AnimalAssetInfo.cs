// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalAssetInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Animal
{
  public struct AnimalAssetInfo
  {
    public AnimalAssetInfo(
      int _TableID,
      string _BundleName,
      string _AssetName,
      string _ManifestName)
    {
      this.TableID = _TableID;
      this.BundleName = _BundleName;
      this.AssetName = _AssetName;
      this.ManifestName = _ManifestName;
    }

    public int TableID { get; private set; }

    public string BundleName { get; private set; }

    public string AssetName { get; private set; }

    public string ManifestName { get; private set; }
  }
}
