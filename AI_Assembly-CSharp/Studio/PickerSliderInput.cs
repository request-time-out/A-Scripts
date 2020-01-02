// Decompiled with JetBrains decompiler
// Type: Studio.PickerSliderInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Component.UI.ColorPicker;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class PickerSliderInput : PickerSlider
  {
    [Tooltip("RedInputField")]
    [SerializeField]
    private TMP_InputField inputR;
    [Tooltip("GreenInputField")]
    [SerializeField]
    private TMP_InputField inputG;
    [Tooltip("BlueInputField")]
    [SerializeField]
    private TMP_InputField inputB;
    [Tooltip("AlphaInputField")]
    [SerializeField]
    private TMP_InputField inputA;
    [Tooltip("R or H")]
    [SerializeField]
    private TextMeshProUGUI textR;
    [Tooltip("G or S")]
    [SerializeField]
    private TextMeshProUGUI textG;
    [Tooltip("B or V")]
    [SerializeField]
    private TextMeshProUGUI textB;

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
      float[] numArray1 = new float[4]
      {
        this.sliderR.get_value(),
        this.sliderG.get_value(),
        this.sliderB.get_value(),
        this.sliderA.get_value()
      };
      int num1 = 0;
      int num2;
      if (this.isHSV)
      {
        TMP_InputField inputR = this.inputR;
        float[] numArray2 = numArray1;
        int index1 = num1;
        int num3 = index1 + 1;
        string str1 = this.ConvertTextFromValue(0, 360, numArray2[index1]);
        inputR.set_text(str1);
        TMP_InputField inputG = this.inputG;
        float[] numArray3 = numArray1;
        int index2 = num3;
        int num4 = index2 + 1;
        string str2 = this.ConvertTextFromValue(0, 100, numArray3[index2]);
        inputG.set_text(str2);
        TMP_InputField inputB = this.inputB;
        float[] numArray4 = numArray1;
        int index3 = num4;
        int num5 = index3 + 1;
        string str3 = this.ConvertTextFromValue(0, 100, numArray4[index3]);
        inputB.set_text(str3);
        TMP_InputField inputA = this.inputA;
        float[] numArray5 = numArray1;
        int index4 = num5;
        num2 = index4 + 1;
        string str4 = this.ConvertTextFromValue(0, 100, numArray5[index4]);
        inputA.set_text(str4);
      }
      else
      {
        TMP_InputField inputR = this.inputR;
        float[] numArray2 = numArray1;
        int index1 = num1;
        int num3 = index1 + 1;
        string str1 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray2[index1]);
        inputR.set_text(str1);
        TMP_InputField inputG = this.inputG;
        float[] numArray3 = numArray1;
        int index2 = num3;
        int num4 = index2 + 1;
        string str2 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray3[index2]);
        inputG.set_text(str2);
        TMP_InputField inputB = this.inputB;
        float[] numArray4 = numArray1;
        int index3 = num4;
        int num5 = index3 + 1;
        string str3 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray4[index3]);
        inputB.set_text(str3);
        TMP_InputField inputA = this.inputA;
        float[] numArray5 = numArray1;
        int index4 = num5;
        num2 = index4 + 1;
        string str4 = this.ConvertTextFromValue(0, 100, numArray5[index4]);
        inputA.set_text(str4);
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
        float[] numArray1 = new float[3]
        {
          this.sliderR.get_value(),
          this.sliderG.get_value(),
          this.sliderB.get_value()
        };
        int num1 = 0;
        int num2;
        if (isOn)
        {
          ((TMP_Text) this.textR).set_text("色合い");
          ((TMP_Text) this.textG).set_text("鮮やかさ");
          ((TMP_Text) this.textB).set_text("明るさ");
          TMP_InputField inputR = this.inputR;
          float[] numArray2 = numArray1;
          int index1 = num1;
          int num3 = index1 + 1;
          string str1 = this.ConvertTextFromValue(0, 360, numArray2[index1]);
          inputR.set_text(str1);
          TMP_InputField inputG = this.inputG;
          float[] numArray3 = numArray1;
          int index2 = num3;
          int num4 = index2 + 1;
          string str2 = this.ConvertTextFromValue(0, 100, numArray3[index2]);
          inputG.set_text(str2);
          TMP_InputField inputB = this.inputB;
          float[] numArray4 = numArray1;
          int index3 = num4;
          num2 = index3 + 1;
          string str3 = this.ConvertTextFromValue(0, 100, numArray4[index3]);
          inputB.set_text(str3);
        }
        else
        {
          ((TMP_Text) this.textR).set_text("赤");
          ((TMP_Text) this.textG).set_text("緑");
          ((TMP_Text) this.textB).set_text("青");
          TMP_InputField inputR = this.inputR;
          float[] numArray2 = numArray1;
          int index1 = num1;
          int num3 = index1 + 1;
          string str1 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray2[index1]);
          inputR.set_text(str1);
          TMP_InputField inputG = this.inputG;
          float[] numArray3 = numArray1;
          int index2 = num3;
          int num4 = index2 + 1;
          string str2 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray3[index2]);
          inputG.set_text(str2);
          TMP_InputField inputB = this.inputB;
          float[] numArray4 = numArray1;
          int index3 = num4;
          num2 = index3 + 1;
          string str3 = this.ConvertTextFromValue(0, (int) byte.MaxValue, numArray4[index3]);
          inputB.set_text(str3);
        }
      }));
    }
  }
}
