// Decompiled with JetBrains decompiler
// Type: SliderValueText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
  [SerializeField]
  private Slider slider;
  [SerializeField]
  private Text label;
  private bool _isParcent;

  public SliderValueText()
  {
    base.\u002Ector();
  }

  public bool isParcent
  {
    get
    {
      return this._isParcent;
    }
    set
    {
      this._isParcent = value;
      this.UpdateText(this.value);
    }
  }

  public string text
  {
    get
    {
      return Object.op_Equality((Object) this.label, (Object) null) ? string.Empty : this.label.get_text();
    }
    set
    {
      if (!Object.op_Implicit((Object) this.label))
        return;
      this.label.set_text(value);
    }
  }

  public float value
  {
    get
    {
      return Object.op_Equality((Object) this.slider, (Object) null) ? 0.0f : this.slider.get_value();
    }
    set
    {
      if (!Object.op_Implicit((Object) this.slider))
        return;
      this.slider.set_value(value);
    }
  }

  private void UpdateText(float f)
  {
    this.text = this.isParcent ? f.ToString("P0") : (f * 100f).ToString("0");
  }

  private void Awake()
  {
    if (Object.op_Equality((Object) this.slider, (Object) null))
      this.slider = (Slider) ((Component) this).get_gameObject().GetComponent<Slider>();
    if (Object.op_Inequality((Object) this.slider, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__0)));
      this.UpdateText(this.slider.get_value());
    }
    if (!Object.op_Equality((Object) this.label, (Object) null))
      return;
    this.label = (Text) ((Component) ((Component) this).get_gameObject().get_transform()).GetComponentInChildren<Text>();
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.slider, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent<float>) this.slider.get_onValueChanged()).RemoveListener(new UnityAction<float>((object) this, __methodptr(\u003COnDestroy\u003Em__1)));
  }
}
