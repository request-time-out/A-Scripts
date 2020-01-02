// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (Graphic))]
  public class BarViewColor : ProgressBarProView
  {
    [SerializeField]
    private Color defaultColor = Color.get_white();
    [SerializeField]
    private Color gainColor = Color.get_white();
    [SerializeField]
    private Color lossColor = Color.get_white();
    [SerializeField]
    private float flashTime = 0.2f;
    [SerializeField]
    protected Graphic graphic;
    [Header("Color Options")]
    [Tooltip("The default color of the bar can be set by the ProgressBar.SetbarColor()")]
    [SerializeField]
    private bool canOverrideColor;
    [Tooltip("Change color of the bar automatically based on it's value.")]
    [SerializeField]
    private bool useGradient;
    [SerializeField]
    private Gradient barGradient;
    private Color flashColor;
    private float flashcolorAlpha;
    private float currentValue;
    [Header("Color Animation")]
    [SerializeField]
    private bool flashOnGain;
    [SerializeField]
    private bool flashOnLoss;
    private Coroutine colorAnim;

    private void OnEnable()
    {
      this.flashcolorAlpha = 0.0f;
      this.UpdateColor();
    }

    public override void NewChangeStarted(float currentValue, float targetValue)
    {
      if (!this.flashOnGain && !this.flashOnLoss || (double) targetValue > (double) currentValue && !this.flashOnGain || ((double) targetValue < (double) currentValue && !this.flashOnLoss || !((Component) this).get_gameObject().get_activeInHierarchy()))
        return;
      if (this.colorAnim != null)
        this.StopCoroutine(this.colorAnim);
      this.colorAnim = this.StartCoroutine(this.DoBarColorAnim((double) targetValue >= (double) currentValue ? this.gainColor : this.lossColor, this.flashTime));
    }

    [DebuggerHidden]
    private IEnumerator DoBarColorAnim(Color targetColor, float duration)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BarViewColor.\u003CDoBarColorAnim\u003Ec__Iterator0()
      {
        duration = duration,
        targetColor = targetColor,
        \u0024this = this
      };
    }

    public override void SetBarColor(Color color)
    {
      if (!this.canOverrideColor)
        return;
      this.defaultColor = color;
      this.useGradient = false;
      if (this.colorAnim != null)
        return;
      this.UpdateColor();
    }

    private void SetOverrideColor(Color color, float alpha)
    {
      this.flashColor = color;
      this.flashcolorAlpha = alpha;
    }

    public override void UpdateView(float currentValue, float targetValue)
    {
      this.currentValue = currentValue;
      if (this.colorAnim != null)
        return;
      this.UpdateColor();
    }

    private void UpdateColor()
    {
      this.graphic.get_canvasRenderer().SetColor(this.GetCurrentColor(this.currentValue));
    }

    private Color GetCurrentColor(float percentage)
    {
      if ((double) this.flashcolorAlpha >= 1.0)
        return this.flashColor;
      if ((double) this.flashcolorAlpha > 0.0)
        return Color.Lerp(!this.useGradient ? this.defaultColor : this.barGradient.Evaluate(percentage), this.flashColor, this.flashcolorAlpha);
      return this.useGradient ? this.barGradient.Evaluate(percentage) : this.defaultColor;
    }
  }
}
