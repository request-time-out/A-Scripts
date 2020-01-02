// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.PickerSliderInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  public class PickerSliderInput : PickerSlider
  {
    [Tooltip("RedInputField")]
    [SerializeField]
    private InputField inputR;
    [Tooltip("GreenInputField")]
    [SerializeField]
    private InputField inputG;
    [Tooltip("BlueInputField")]
    [SerializeField]
    private InputField inputB;
    [Tooltip("AlphaInputField")]
    [SerializeField]
    private InputField inputA;
    [Tooltip("R or H")]
    [SerializeField]
    private Text textR;
    [Tooltip("G or S")]
    [SerializeField]
    private Text textG;
    [Tooltip("B or V")]
    [SerializeField]
    private Text textB;

    public string ConvertTextFromValue(int min, int max, float value)
    {
      return ((int) Mathf.Lerp((float) min, (float) max, value)).ToString();
    }

    public float ConvertValueFromText(int min, int max, string buf)
    {
      int result;
      return buf.IsNullOrEmpty() || !int.TryParse(buf, out result) ? 0.0f : Mathf.InverseLerp((float) min, (float) max, (float) result);
    }

    public void SetInputText()
    {
      if (this.isHSV)
      {
        this.inputR.set_text(this.ConvertTextFromValue(0, 360, (float) this.color.r));
        this.inputG.set_text(this.ConvertTextFromValue(0, 100, (float) this.color.g));
        this.inputB.set_text(this.ConvertTextFromValue(0, 100, (float) this.color.b));
        this.inputA.set_text(this.ConvertTextFromValue(0, 100, (float) this.color.a));
      }
      else
      {
        this.inputR.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, (float) this.color.r));
        this.inputG.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, (float) this.color.g));
        this.inputB.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, (float) this.color.b));
        this.inputA.set_text(this.ConvertTextFromValue(0, 100, (float) this.color.a));
      }
    }

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderR.get_onValueChanged()), (Action<M0>) (value =>
      {
        if (this.isHSV)
          this.inputR.set_text(this.ConvertTextFromValue(0, 360, value));
        else
          this.inputR.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, value));
      }));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputR.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Em__1)));
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderG.get_onValueChanged()), (Action<M0>) (value =>
      {
        if (this.isHSV)
          this.inputG.set_text(this.ConvertTextFromValue(0, 100, value));
        else
          this.inputG.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, value));
      }));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputG.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Em__3)));
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderB.get_onValueChanged()), (Action<M0>) (value =>
      {
        if (this.isHSV)
          this.inputB.set_text(this.ConvertTextFromValue(0, 100, value));
        else
          this.inputB.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, value));
      }));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputB.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Em__5)));
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderA.get_onValueChanged()), (Action<M0>) (value =>
      {
        if (this.isHSV)
          this.inputA.set_text(this.ConvertTextFromValue(0, 100, value));
        else
          this.inputA.set_text(this.ConvertTextFromValue(0, 100, value));
      }));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputA.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Em__7)));
      ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isHSV, (UnityEngine.Component) this), (Action<M0>) (isOn =>
      {
        if (isOn)
        {
          this.textR.set_text("色合い");
          this.textG.set_text("鮮やかさ");
          this.textB.set_text("明るさ");
        }
        else
        {
          this.textR.set_text("赤");
          this.textG.set_text("緑");
          this.textB.set_text("青");
          this.inputR.set_text(this.ConvertTextFromValue(0, (int) byte.MaxValue, (float) this.color.r));
        }
      }));
    }
  }
}
