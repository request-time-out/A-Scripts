// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewSizeAnchorsShadow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (Image))]
  public class BarViewSizeAnchorsShadow : BarViewSizeAnchors
  {
    [SerializeField]
    private BarViewSizeAnchorsShadow.ShadowType shadowType;

    public override void UpdateView(float currentValue, float targetValue)
    {
      if (Mathf.Approximately(currentValue, targetValue) || this.shadowType == BarViewSizeAnchorsShadow.ShadowType.Gaining && (double) targetValue < (double) currentValue || this.shadowType == BarViewSizeAnchorsShadow.ShadowType.Losing && (double) targetValue > (double) currentValue)
      {
        ((Component) this.rectTrans).get_gameObject().SetActive(false);
        this.isDisplaySizeZero = true;
      }
      else
      {
        this.isDisplaySizeZero = false;
        ((Component) this.rectTrans).get_gameObject().SetActive(true);
        if (this.shadowType == BarViewSizeAnchorsShadow.ShadowType.Gaining)
          this.SetPivot(0.0f, targetValue);
        else
          this.SetPivot(targetValue, currentValue);
      }
    }

    public enum ShadowType
    {
      Gaining,
      Losing,
    }
  }
}
