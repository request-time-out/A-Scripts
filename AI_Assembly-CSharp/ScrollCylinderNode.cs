// Decompiled with JetBrains decompiler
// Type: ScrollCylinderNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ScrollCylinderNode : MonoBehaviour
{
  public Image BG;
  public Text text;
  public float smoothTime;
  public float smoothTimeV2;
  public float[] alpha;
  public float[] scale;
  protected int phaseScale;
  protected int prephaseScale;
  protected Image ToggleCheckMark;
  protected float endA;
  protected Vector3 endScl;
  protected Color tmpColor;
  protected Vector3 tmpScl;

  public ScrollCylinderNode()
  {
    base.\u002Ector();
  }

  protected virtual void Update()
  {
    this.tmpColor = ((Graphic) this.BG).get_color();
    float num = 0.0f;
    float deltaTime = Time.get_deltaTime();
    this.tmpColor.a = (__Null) (double) Mathf.SmoothDamp((float) this.tmpColor.a, this.endA, ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
    ((Graphic) this.BG).set_color(this.tmpColor);
    Toggle component = (Toggle) ((Component) this).GetComponent<Toggle>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      this.ToggleCheckMark = (Image) ((Component) component.graphic).GetComponent<Image>();
      if (Object.op_Inequality((Object) this.ToggleCheckMark, (Object) null))
      {
        this.tmpColor.r = ((Graphic) this.ToggleCheckMark).get_color().r;
        this.tmpColor.g = ((Graphic) this.ToggleCheckMark).get_color().g;
        this.tmpColor.b = ((Graphic) this.ToggleCheckMark).get_color().b;
        ((Graphic) this.ToggleCheckMark).set_color(this.tmpColor);
      }
    }
    if (Object.op_Inequality((Object) this.text, (Object) null))
    {
      this.tmpColor.r = ((Graphic) this.text).get_color().r;
      this.tmpColor.g = ((Graphic) this.text).get_color().g;
      this.tmpColor.b = ((Graphic) this.text).get_color().b;
      ((Graphic) this.text).set_color(this.tmpColor);
    }
    this.tmpScl = ((Component) this.BG).get_transform().get_localScale();
    Vector3 zero = Vector3.get_zero();
    this.tmpScl = this.prephaseScale == 0 && this.phaseScale == 1 || this.prephaseScale == 1 && this.phaseScale == 0 ? Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTime, float.PositiveInfinity, deltaTime) : Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTimeV2, float.PositiveInfinity, deltaTime);
    ((Component) this.BG).get_transform().set_localScale(this.tmpScl);
    if (!Object.op_Inequality((Object) this.text, (Object) null))
      return;
    ((Component) this.text).get_transform().set_localScale(this.tmpScl);
  }

  public void ChangeBGAlpha(int id)
  {
    this.endA = this.alpha[id];
  }

  public void ChangeScale(int id)
  {
    this.prephaseScale = this.phaseScale;
    this.endScl = new Vector3(this.scale[id], this.scale[id], this.scale[id]);
    this.phaseScale = id;
  }
}
