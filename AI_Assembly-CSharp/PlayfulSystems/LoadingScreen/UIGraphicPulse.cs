// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.UIGraphicPulse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.LoadingScreen
{
  [RequireComponent(typeof (Graphic))]
  public class UIGraphicPulse : MonoBehaviour
  {
    public Graphic gfx;
    public bool doPulse;
    public Color defaultColor;
    public Color pulseColor;
    public float pulseDuration;
    private bool isPulsing;
    private float pulseTime;

    public UIGraphicPulse()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      if (this.isPulsing != this.doPulse)
        this.SetPulsing(this.doPulse);
      if (!this.isPulsing)
        return;
      this.pulseTime += Time.get_deltaTime();
      this.gfx.set_color(Color.Lerp(this.defaultColor, this.pulseColor, this.GetAlpha()));
    }

    private void SetPulsing(bool state)
    {
      this.isPulsing = state;
      if (!this.isPulsing)
        this.gfx.set_color(this.defaultColor);
      else
        this.pulseTime = 0.0f;
    }

    private float GetAlpha()
    {
      return (float) (0.5 + 0.5 * (double) Mathf.Sin((float) (((double) this.pulseTime + (double) this.pulseDuration / 4.0) / 3.14159274101257 * 20.0) / this.pulseDuration));
    }
  }
}
