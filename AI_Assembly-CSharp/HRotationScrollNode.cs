// Decompiled with JetBrains decompiler
// Type: HRotationScrollNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class HRotationScrollNode : ScrollCylinderNode
{
  public int id = -1;

  protected override void Update()
  {
    this.tmpColor = ((Graphic) this.BG).get_color();
    float num = 0.0f;
    float deltaTime = Time.get_deltaTime();
    this.tmpColor.a = (__Null) (double) Mathf.SmoothDamp((float) this.tmpColor.a, this.endA, ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
    ((Graphic) this.BG).set_color(this.tmpColor);
    if (Object.op_Inequality((Object) this.text, (Object) null))
      ((Graphic) this.text).set_color(this.tmpColor);
    this.tmpScl = ((Component) this.BG).get_transform().get_localScale();
    Vector3 zero = Vector3.get_zero();
    if (this.prephaseScale == 0 && this.phaseScale == 1 || this.prephaseScale == 1 && this.phaseScale == 0)
      this.tmpScl = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTime, float.PositiveInfinity, deltaTime);
    else
      this.tmpScl = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTimeV2, float.PositiveInfinity, deltaTime);
    ((Component) this.BG).get_transform().set_localScale(this.tmpScl);
    if (!Object.op_Inequality((Object) this.text, (Object) null))
      return;
    ((Component) this.text).get_transform().set_localScale(this.tmpScl);
  }
}
