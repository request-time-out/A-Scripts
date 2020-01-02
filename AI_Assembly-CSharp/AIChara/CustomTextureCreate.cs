// Decompiled with JetBrains decompiler
// Type: AIChara.CustomTextureCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public class CustomTextureCreate
  {
    private RenderTextureFormat rtFormat;
    private RenderTexture createTex;
    private Material matCreate;
    private Texture texMain;
    public Transform trfParent;

    public CustomTextureCreate(Transform _trfParent = null)
    {
      this.trfParent = _trfParent;
    }

    public int baseW { get; private set; }

    public int baseH { get; private set; }

    public bool Initialize(
      string createMatManifest,
      string createMatABName,
      string createMatName,
      int width,
      int height,
      RenderTextureFormat format = 0)
    {
      this.baseW = width;
      this.baseH = height;
      this.rtFormat = format;
      this.matCreate = CommonLib.LoadAsset<Material>(createMatABName, createMatName, true, string.Empty);
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return false;
      this.texMain = this.matCreate.GetTexture(ChaShader.MainTex);
      this.createTex = new RenderTexture(this.baseW, this.baseH, 0, this.rtFormat);
      this.createTex.set_useMipMap(true);
      return true;
    }

    public void Release()
    {
      Object.Destroy((Object) this.createTex);
      this.createTex = (RenderTexture) null;
      Object.Destroy((Object) this.matCreate);
      this.matCreate = (Material) null;
    }

    public void ReleaseCreateMaterial()
    {
      Object.Destroy((Object) this.matCreate);
      this.matCreate = (Material) null;
    }

    public void SetMainTexture(Texture tex)
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return;
      this.texMain = tex;
    }

    public void SetTexture(string propertyName, Texture tex)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetTexture(propertyName, tex);
    }

    public void SetTexture(int propertyID, Texture tex)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetTexture(propertyID, tex);
    }

    public void SetColor(string propertyName, Color color)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetColor(propertyName, color);
    }

    public Color GetColor(string propertyName)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetColor(propertyName) : Color.get_white();
    }

    public void SetColor(int propertyID, Color color)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetColor(propertyID, color);
    }

    public Color GetColor(int propertyID)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetColor(propertyID) : Color.get_white();
    }

    public void SetOffsetAndTilingDirect(
      string propertyName,
      float tx,
      float ty,
      float ox,
      float oy)
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetTextureOffset(propertyName, new Vector2(ox, oy));
      this.matCreate.SetTextureScale(propertyName, new Vector2(tx, ty));
    }

    public void SetOffsetAndTilingDirect(int propertyID, float tx, float ty, float ox, float oy)
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetTextureOffset(propertyID, new Vector2(ox, oy));
      this.matCreate.SetTextureScale(propertyID, new Vector2(tx, ty));
    }

    public void SetOffsetAndTiling(
      string propertyName,
      int addW,
      int addH,
      float addPx,
      float addPy)
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return;
      float tx = (float) this.baseW / (float) addW;
      float ty = (float) this.baseH / (float) addH;
      float ox = (float) -((double) addPx / (double) this.baseW) * tx;
      float oy = (float) -(((double) this.baseH - (double) addPy - (double) addH) / (double) this.baseH) * ty;
      this.SetOffsetAndTilingDirect(propertyName, tx, ty, ox, oy);
    }

    public void SetOffsetAndTiling(int propertyID, int addW, int addH, float addPx, float addPy)
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return;
      float tx = (float) this.baseW / (float) addW;
      float ty = (float) this.baseH / (float) addH;
      float ox = (float) -((double) addPx / (double) this.baseW) * tx;
      float oy = (float) -(((double) this.baseH - (double) addPy - (double) addH) / (double) this.baseH) * ty;
      this.SetOffsetAndTilingDirect(propertyID, tx, ty, ox, oy);
    }

    public void SetFloat(string propertyName, float value)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetFloat(propertyName, value);
    }

    public float GetFloat(string propertyName)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetFloat(propertyName) : 0.0f;
    }

    public void SetFloat(int propertyID, float value)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetFloat(propertyID, value);
    }

    public float GetFloat(int propertyID)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetFloat(propertyID) : 0.0f;
    }

    public void SetVector4(string propertyName, Vector4 value)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetVector(propertyName, value);
    }

    public Vector4 GetVector4(string propertyName)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetVector(propertyName) : Vector4.get_zero();
    }

    public void SetVector4(int propertyID, Vector4 value)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.matCreate))
        return;
      this.matCreate.SetVector(propertyID, value);
    }

    public Vector4 GetVector4(int propertyID)
    {
      return Object.op_Inequality((Object) null, (Object) this.matCreate) ? this.matCreate.GetVector(propertyID) : Vector4.get_zero();
    }

    public Texture RebuildTextureAndSetMaterial()
    {
      if (Object.op_Equality((Object) null, (Object) this.matCreate))
        return (Texture) null;
      bool sRgbWrite = GL.get_sRGBWrite();
      GL.set_sRGBWrite(true);
      Graphics.SetRenderTarget(this.createTex);
      GL.Clear(false, true, Color.get_clear());
      Graphics.SetRenderTarget((RenderTexture) null);
      Graphics.Blit(this.texMain, this.createTex, this.matCreate, 0);
      GL.set_sRGBWrite(sRgbWrite);
      return (Texture) this.createTex;
    }

    public Texture GetCreateTexture()
    {
      return (Texture) this.createTex;
    }
  }
}
