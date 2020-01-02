// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_UnderwaterRenderingSlave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater
{
  [RequireComponent(typeof (Camera))]
  public class LuxWater_UnderwaterRenderingSlave : MonoBehaviour
  {
    private LuxWater_UnderWaterRendering waterrendermanager;
    private bool readyToGo;
    public Camera cam;

    public LuxWater_UnderwaterRenderingSlave()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      this.Invoke("GetWaterrendermanager", 0.0f);
    }

    private void GetWaterrendermanager()
    {
      LuxWater_UnderWaterRendering instance = LuxWater_UnderWaterRendering.instance;
      if (!Object.op_Inequality((Object) instance, (Object) null))
        return;
      this.waterrendermanager = instance;
      this.readyToGo = true;
    }

    private void OnPreCull()
    {
      if (!this.readyToGo)
        return;
      this.waterrendermanager.RenderWaterMask(this.cam, true);
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
      if (this.readyToGo)
        this.waterrendermanager.RenderUnderWater(src, dest, this.cam, true);
      else
        Graphics.Blit((Texture) src, dest);
    }
  }
}
