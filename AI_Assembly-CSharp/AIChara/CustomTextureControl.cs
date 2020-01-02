// Decompiled with JetBrains decompiler
// Type: AIChara.CustomTextureControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIChara
{
  public class CustomTextureControl
  {
    public CustomTextureCreate[] createCustomTex;

    public CustomTextureControl(
      int num,
      string drawMatManifest,
      string drawMatABName,
      string drawMatName,
      Transform trfParent = null)
    {
      this.createCustomTex = new CustomTextureCreate[num];
      for (int index = 0; index < num; ++index)
        this.createCustomTex[index] = new CustomTextureCreate(trfParent);
      this.matDraw = CommonLib.LoadAsset<Material>(drawMatABName, drawMatName, true, drawMatManifest);
      Singleton<Character>.Instance.AddLoadAssetBundle(drawMatABName, drawMatManifest);
    }

    public Material matDraw { get; private set; }

    public bool Initialize(
      int index,
      string createMatManifest,
      string createMatABName,
      string createMatName,
      int width,
      int height,
      RenderTextureFormat format = 0)
    {
      return this.createCustomTex != null && index < this.createCustomTex.Length && (this.createCustomTex[index] != null && this.createCustomTex[index].Initialize(createMatManifest, createMatABName, createMatName, width, height, format));
    }

    public void Release()
    {
      foreach (CustomTextureCreate customTextureCreate in this.createCustomTex)
        customTextureCreate.Release();
      Object.Destroy((Object) this.matDraw);
      this.matDraw = (Material) null;
    }

    public bool SetNewCreateTexture(int index, int propertyId)
    {
      if (this.createCustomTex == null || index >= this.createCustomTex.Length || this.createCustomTex[index] == null)
        return false;
      this.createCustomTex[index].RebuildTextureAndSetMaterial();
      Texture createTexture = this.createCustomTex[index].GetCreateTexture();
      if (Object.op_Inequality((Object) null, (Object) this.matDraw))
        this.matDraw.SetTexture(propertyId, createTexture);
      return true;
    }
  }
}
