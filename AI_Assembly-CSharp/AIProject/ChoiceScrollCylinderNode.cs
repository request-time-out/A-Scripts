// Decompiled with JetBrains decompiler
// Type: AIProject.ChoiceScrollCylinderNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class ChoiceScrollCylinderNode : ScrollCylinderNode
  {
    private Vector3 _localScale = Vector3.get_zero();
    [SerializeField]
    private Image _alphaBG;
    [SerializeField]
    private Image _scaleBG;
    private float _alpha;

    protected override void Update()
    {
      this.tmpColor = new Color(0.0f, 0.0f, 0.0f, this._alpha);
      float num1 = 0.0f;
      float deltaTime = Time.get_deltaTime();
      this.tmpColor.a = (__Null) (double) (this._alpha = Mathf.SmoothDamp((float) this.tmpColor.a, this.endA, ref num1, this.smoothTime, float.PositiveInfinity, deltaTime));
      for (int index = 0; index < 3; ++index)
      {
        ref Color local = ref this.tmpColor;
        int num2 = index;
        Color color = ((Graphic) this._alphaBG).get_color();
        double num3 = (double) ((Color) ref color).get_Item(index);
        ((Color) ref local).set_Item(num2, (float) num3);
      }
      ((Graphic) this._alphaBG).set_color(this.tmpColor);
      if (Object.op_Inequality((Object) this.text, (Object) null))
      {
        for (int index = 0; index < 3; ++index)
        {
          ref Color local = ref this.tmpColor;
          int num2 = index;
          Color color = ((Graphic) this.text).get_color();
          double num3 = (double) ((Color) ref color).get_Item(index);
          ((Color) ref local).set_Item(num2, (float) num3);
        }
        ((Graphic) this.text).set_color(this.tmpColor);
      }
      this.tmpScl = this._localScale;
      Vector3 zero = Vector3.get_zero();
      if (this.prephaseScale == 0 && this.phaseScale == 1 || this.prephaseScale == 1 && this.phaseScale == 0)
        this.tmpScl = this._localScale = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTime, float.PositiveInfinity, deltaTime);
      else
        this.tmpScl = this._localScale = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTimeV2, float.PositiveInfinity, deltaTime);
      ((Component) this._scaleBG).get_transform().set_localScale(this.tmpScl);
      if (!Object.op_Inequality((Object) this.text, (Object) null))
        return;
      ((Component) this.text).get_transform().set_localScale(this.tmpScl);
    }
  }
}
