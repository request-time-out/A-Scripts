// Decompiled with JetBrains decompiler
// Type: SaveAssist.AssetBundleAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

namespace SaveAssist
{
  public abstract class AssetBundleAssist
  {
    protected string savePath = string.Empty;
    protected string assetBundleName = string.Empty;
    protected string assetName = string.Empty;

    public AssetBundleAssist(string _savePath, string _assetBundleName, string _assetName)
    {
      this.savePath = _savePath;
      this.assetBundleName = _assetBundleName;
      this.assetName = _assetName;
    }

    public void Save()
    {
      using (FileStream fileStream = new FileStream(this.savePath, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter bw = new BinaryWriter((Stream) fileStream))
          this.SaveFunc(bw);
      }
    }

    public abstract void SaveFunc(BinaryWriter bw);

    public void Load()
    {
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(this.assetBundleName, this.assetName, false, string.Empty);
      if (Object.op_Inequality((Object) null, (Object) textAsset))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          memoryStream.Write(textAsset.get_bytes(), 0, textAsset.get_bytes().Length);
          memoryStream.Seek(0L, SeekOrigin.Begin);
          using (BinaryReader br = new BinaryReader((Stream) memoryStream))
            this.LoadFunc(br);
        }
      }
      AssetBundleManager.UnloadAssetBundle(this.assetBundleName, true, (string) null, true);
    }

    public abstract void LoadFunc(BinaryReader br);
  }
}
