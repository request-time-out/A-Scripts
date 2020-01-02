// Decompiled with JetBrains decompiler
// Type: Studio.PanelComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class PanelComponent : MonoBehaviour
  {
    public Renderer[] renderer;
    public Color defColor;
    public Vector4 defUV;
    public float defRot;
    public bool defClamp;

    public PanelComponent()
    {
      base.\u002Ector();
    }

    public void SetMainTex(Texture2D _texture)
    {
      foreach (Renderer renderer in this.renderer)
        renderer.get_material().SetTexture(ItemShader._MainTex, (Texture) _texture);
    }

    public void UpdateColor(OIItemInfo _info)
    {
      foreach (Renderer renderer in this.renderer)
      {
        renderer.get_material().SetColor(ItemShader._Color, _info.colors[0].mainColor);
        renderer.get_material().SetVector(ItemShader._patternuv1, _info.colors[0].pattern.uv);
        renderer.get_material().SetFloat(ItemShader._patternuv1Rotator, _info.colors[0].pattern.rot);
        renderer.get_material().SetFloat(ItemShader._patternclamp1, !_info.colors[0].pattern.clamp ? 0.0f : 1f);
      }
    }

    public void Setup()
    {
      Renderer renderer = this.renderer.SafeGet<Renderer>(0);
      if (Object.op_Equality((Object) renderer, (Object) null))
        return;
      Material sharedMaterial = renderer.get_sharedMaterial();
      this.defColor = sharedMaterial.GetColor("_Color");
      this.defUV = sharedMaterial.GetVector("_patternuv1");
      this.defRot = sharedMaterial.GetFloat("_patternuv1Rotator");
      this.defClamp = !Mathf.Approximately(sharedMaterial.GetFloat("_patternclamp1"), 0.0f);
    }
  }
}
