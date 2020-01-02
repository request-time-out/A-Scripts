// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_UnderWaterBlur
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater
{
  [RequireComponent(typeof (Camera))]
  public class LuxWater_UnderWaterBlur : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.3a2840a53u5j")]
    public float blurSpread;
    public int blurDownSample;
    public int blurIterations;
    private Vector2[] m_offsets;
    private Material blurMaterial;
    private Material blitMaterial;
    private LuxWater_UnderWaterRendering waterrendermanager;
    private bool doBlur;
    private bool initBlur;

    public LuxWater_UnderWaterBlur()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this.blurMaterial = new Material(Shader.Find("Hidden/Lux Water/BlurEffectConeTap"));
      this.blitMaterial = new Material(Shader.Find("Hidden/Lux Water/UnderWaterPost"));
      this.Invoke("GetWaterrendermanagerInstance", 0.0f);
    }

    private void OnDisable()
    {
      if (Object.op_Implicit((Object) this.blurMaterial))
        Object.DestroyImmediate((Object) this.blurMaterial);
      if (!Object.op_Implicit((Object) this.blitMaterial))
        return;
      Object.DestroyImmediate((Object) this.blitMaterial);
    }

    private void GetWaterrendermanagerInstance()
    {
      this.waterrendermanager = LuxWater_UnderWaterRendering.instance;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
      if (Object.op_Equality((Object) this.waterrendermanager, (Object) null))
      {
        Graphics.Blit((Texture) src, dest);
      }
      else
      {
        this.doBlur = this.waterrendermanager.activeWaterVolume > -1;
        if (this.doBlur || this.initBlur)
        {
          this.initBlur = false;
          int num1 = ((Texture) src).get_width() / this.blurDownSample;
          int num2 = ((Texture) src).get_height() / this.blurDownSample;
          RenderTexture renderTexture = RenderTexture.GetTemporary(num1, num2, 0, (RenderTextureFormat) 9);
          this.DownSample(src, renderTexture);
          for (int iteration = 0; iteration < this.blurIterations; ++iteration)
          {
            RenderTexture temporary = RenderTexture.GetTemporary(num1, num2, 0, (RenderTextureFormat) 9);
            this.FourTapCone(renderTexture, temporary, iteration);
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = temporary;
          }
          Shader.SetGlobalTexture("_UnderWaterTex", (Texture) renderTexture);
          Graphics.Blit((Texture) src, dest, this.blitMaterial, 1);
          RenderTexture.ReleaseTemporary(renderTexture);
        }
        else
          Graphics.Blit((Texture) src, dest);
      }
    }

    private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
      float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
      this.m_offsets[0].x = (__Null) -(double) num;
      this.m_offsets[0].y = (__Null) -(double) num;
      this.m_offsets[1].x = (__Null) -(double) num;
      this.m_offsets[1].y = (__Null) (double) num;
      this.m_offsets[2].x = (__Null) (double) num;
      this.m_offsets[2].y = (__Null) (double) num;
      this.m_offsets[3].x = (__Null) (double) num;
      this.m_offsets[3].y = (__Null) -(double) num;
      if (iteration == 0)
        Graphics.BlitMultiTap((Texture) source, dest, this.blurMaterial, this.m_offsets);
      else
        Graphics.BlitMultiTap((Texture) source, dest, this.blurMaterial, this.m_offsets);
    }

    private void DownSample(RenderTexture source, RenderTexture dest)
    {
      float num = 1f;
      this.m_offsets[0].x = (__Null) -(double) num;
      this.m_offsets[0].y = (__Null) -(double) num;
      this.m_offsets[1].x = (__Null) -(double) num;
      this.m_offsets[1].y = (__Null) (double) num;
      this.m_offsets[2].x = (__Null) (double) num;
      this.m_offsets[2].y = (__Null) (double) num;
      this.m_offsets[3].x = (__Null) (double) num;
      this.m_offsets[3].y = (__Null) -(double) num;
      Graphics.BlitMultiTap((Texture) source, dest, this.blurMaterial, this.m_offsets);
    }
  }
}
