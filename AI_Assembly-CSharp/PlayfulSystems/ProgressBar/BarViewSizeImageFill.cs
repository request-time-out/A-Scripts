// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewSizeImageFill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (Image))]
  public class BarViewSizeImageFill : ProgressBarProView
  {
    [SerializeField]
    private bool hideOnEmpty = true;
    [SerializeField]
    private int numSteps = 10;
    [SerializeField]
    protected Image image;
    [SerializeField]
    private bool useDiscreteSteps;
    private bool isDisplaySizeZero;

    public override bool CanUpdateView(float currentValue, float targetValue)
    {
      return ((Behaviour) this).get_isActiveAndEnabled() || this.isDisplaySizeZero;
    }

    public override void UpdateView(float currentValue, float targetValue)
    {
      if (this.hideOnEmpty && (double) currentValue <= 0.0)
      {
        ((Component) this.image).get_gameObject().SetActive(false);
        this.isDisplaySizeZero = true;
      }
      else
      {
        this.isDisplaySizeZero = false;
        ((Component) this.image).get_gameObject().SetActive(true);
        this.image.set_fillAmount(this.GetDisplayValue(currentValue));
      }
    }

    private float GetDisplayValue(float display)
    {
      return !this.useDiscreteSteps ? display : Mathf.Round(display * (float) this.numSteps) / (float) this.numSteps;
    }
  }
}
