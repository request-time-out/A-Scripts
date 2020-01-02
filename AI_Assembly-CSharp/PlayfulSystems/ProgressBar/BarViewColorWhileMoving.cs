// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewColorWhileMoving
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (Graphic))]
  public class BarViewColorWhileMoving : ProgressBarProView
  {
    [SerializeField]
    private Color colorStatic = Color.get_white();
    [SerializeField]
    private Color colorMoving = Color.get_blue();
    [SerializeField]
    private float blendTimeOnMove = 0.2f;
    [SerializeField]
    private float blendTimeOnStop = 0.05f;
    [SerializeField]
    protected Graphic graphic;
    private bool isMoving;

    private void OnEnable()
    {
      this.SetDefaultColor();
    }

    public override void UpdateView(float currentValue, float targetValue)
    {
      bool flag = (double) currentValue != (double) targetValue;
      if (this.isMoving == flag)
        return;
      this.isMoving = flag;
      this.graphic.CrossFadeColor(this.GetCurrentColor(), !this.isMoving ? this.blendTimeOnStop : this.blendTimeOnMove, false, true);
    }

    private Color GetCurrentColor()
    {
      return this.isMoving ? this.colorMoving : this.colorStatic;
    }

    private void SetDefaultColor()
    {
      this.graphic.get_canvasRenderer().SetColor(this.GetCurrentColor());
    }
  }
}
