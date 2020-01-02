// Decompiled with JetBrains decompiler
// Type: ME_EffectSettingColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ME_EffectSettingColor : MonoBehaviour
{
  public Color Color;
  private Color previousColor;

  public ME_EffectSettingColor()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    this.Update();
  }

  private void Update()
  {
    if (!Color.op_Inequality(this.previousColor, this.Color))
      return;
    this.UpdateColor();
  }

  private void UpdateColor()
  {
    ME_ColorHelper.ChangeObjectColorByHUE(((Component) this).get_gameObject(), ME_ColorHelper.ColorToHSV(this.Color).H);
    this.previousColor = this.Color;
  }
}
