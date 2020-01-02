// Decompiled with JetBrains decompiler
// Type: HItemNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class HItemNode : ScrollCylinderNode
{
  [SerializeField]
  private Image IconImage;
  [SerializeField]
  private Toggle toggle;
  [SerializeField]
  private Text NumMark;
  public Text NumTxt;
  [SerializeField]
  private RectTransform scaleSet;

  public Toggle Toggle
  {
    get
    {
      return this.toggle;
    }
  }

  public GameObject ScaleSet
  {
    get
    {
      return ((Component) this.scaleSet).get_gameObject();
    }
  }

  private void Start()
  {
    this.ToggleCheckMark = (Image) ((Component) this.toggle.graphic).GetComponent<Image>();
  }

  protected override void Update()
  {
    this.tmpColor = ((Graphic) this.BG).get_color();
    float num = 0.0f;
    float deltaTime = Time.get_deltaTime();
    this.tmpColor.a = (__Null) (double) Mathf.SmoothDamp((float) this.tmpColor.a, this.endA, ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
    ((Graphic) this.BG).set_color(this.tmpColor);
    if (Object.op_Inequality((Object) this.ToggleCheckMark, (Object) null))
    {
      this.tmpColor.r = ((Graphic) this.ToggleCheckMark).get_color().r;
      this.tmpColor.g = ((Graphic) this.ToggleCheckMark).get_color().g;
      this.tmpColor.b = ((Graphic) this.ToggleCheckMark).get_color().b;
      ((Graphic) this.ToggleCheckMark).set_color(this.tmpColor);
    }
    this.tmpColor.r = ((Graphic) this.IconImage).get_color().r;
    this.tmpColor.g = ((Graphic) this.IconImage).get_color().g;
    this.tmpColor.b = ((Graphic) this.IconImage).get_color().b;
    ((Graphic) this.IconImage).set_color(this.tmpColor);
    if (Object.op_Inequality((Object) this.text, (Object) null))
    {
      this.tmpColor.r = ((Graphic) this.text).get_color().r;
      this.tmpColor.g = ((Graphic) this.text).get_color().g;
      this.tmpColor.b = ((Graphic) this.text).get_color().b;
      ((Graphic) this.text).set_color(this.tmpColor);
    }
    if (Object.op_Inequality((Object) this.NumMark, (Object) null))
    {
      this.tmpColor.r = ((Graphic) this.text).get_color().r;
      this.tmpColor.g = ((Graphic) this.text).get_color().g;
      this.tmpColor.b = ((Graphic) this.text).get_color().b;
      ((Graphic) this.NumMark).set_color(this.tmpColor);
    }
    if (Object.op_Inequality((Object) this.NumTxt, (Object) null))
    {
      this.tmpColor.r = ((Graphic) this.text).get_color().r;
      this.tmpColor.g = ((Graphic) this.text).get_color().g;
      this.tmpColor.b = ((Graphic) this.text).get_color().b;
      ((Graphic) this.NumTxt).set_color(this.tmpColor);
    }
    this.tmpScl = ((Component) this.BG).get_transform().get_localScale();
    Vector3 zero = Vector3.get_zero();
    if (this.prephaseScale == 0 && this.phaseScale == 1 || this.prephaseScale == 1 && this.phaseScale == 0)
      this.tmpScl = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTime, float.PositiveInfinity, deltaTime);
    else
      this.tmpScl = Vector3.SmoothDamp(this.tmpScl, this.endScl, ref zero, this.smoothTimeV2, float.PositiveInfinity, deltaTime);
    ((Component) this.BG).get_transform().set_localScale(this.tmpScl);
    ((Transform) this.scaleSet).set_localScale(this.tmpScl);
  }
}
