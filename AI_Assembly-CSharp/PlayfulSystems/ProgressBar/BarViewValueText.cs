// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewValueText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (Text))]
  public class BarViewValueText : ProgressBarProView
  {
    [SerializeField]
    private string prefix = string.Empty;
    [SerializeField]
    private float maxValue = 100f;
    [SerializeField]
    private string numberUnit = "%";
    [SerializeField]
    private string suffix = string.Empty;
    [SerializeField]
    private Text text;
    [SerializeField]
    private float minValue;
    [SerializeField]
    private int numDecimals;
    [SerializeField]
    private bool showMaxValue;
    private float lastDisplayValue;

    public override bool CanUpdateView(float currentValue, float targetValue)
    {
      float roundedDisplayValue = this.GetRoundedDisplayValue(currentValue);
      if ((double) currentValue >= 0.0 && Mathf.Approximately(this.lastDisplayValue, roundedDisplayValue))
        return false;
      this.lastDisplayValue = this.GetRoundedDisplayValue(currentValue);
      return true;
    }

    public override void UpdateView(float currentValue, float targetValue)
    {
      this.text.set_text(this.prefix + this.FormatNumber(this.GetRoundedDisplayValue(currentValue)) + this.numberUnit + (!this.showMaxValue ? string.Empty : " / " + this.FormatNumber(this.maxValue) + this.numberUnit) + this.suffix);
    }

    private float GetDisplayValue(float num)
    {
      return Mathf.Lerp(this.minValue, this.maxValue, num);
    }

    private float GetRoundedDisplayValue(float num)
    {
      float displayValue = this.GetDisplayValue(num);
      if (this.numDecimals == 0)
        return Mathf.Round(displayValue);
      float num1 = Mathf.Pow(10f, (float) this.numDecimals);
      return Mathf.Round(displayValue * num1) / num1;
    }

    private string FormatNumber(float num)
    {
      return num.ToString("N" + (object) this.numDecimals);
    }
  }
}
