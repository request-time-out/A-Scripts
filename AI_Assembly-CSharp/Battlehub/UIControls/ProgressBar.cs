// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class ProgressBar : MonoBehaviour
  {
    [SerializeField]
    private Image[] Images;
    [SerializeField]
    private Outline[] Outlines;
    private float[] m_alpha;
    [SerializeField]
    private float Speed;
    public bool IsInProgress;

    public ProgressBar()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.m_alpha = new float[this.Images.Length];
      for (int index = 0; index < this.Images.Length; ++index)
      {
        this.m_alpha[index] = (float) index / (float) this.Images.Length;
        Color color = ((Graphic) this.Images[index]).get_color();
        color.a = (__Null) (double) this.m_alpha[index];
        ((Graphic) this.Images[index]).set_color(color);
        Color effectColor = ((Shadow) this.Outlines[index]).get_effectColor();
        effectColor.a = (__Null) (double) this.m_alpha[index];
        ((Shadow) this.Outlines[index]).set_effectColor(effectColor);
      }
    }

    private void FixedUpdate()
    {
      if (!this.IsInProgress)
        return;
      for (int index = 0; index < this.Images.Length; ++index)
      {
        ((Graphic) this.Images[index]).set_color(this.UpdateAlpha(((Graphic) this.Images[index]).get_color(), index));
        ((Shadow) this.Outlines[index]).set_effectColor(this.UpdateAlpha(((Shadow) this.Outlines[index]).get_effectColor(), index));
      }
    }

    private Color UpdateAlpha(Color color, int index)
    {
      this.m_alpha[index] -= Time.get_deltaTime() * this.Speed;
      if ((double) this.m_alpha[index] < 0.0)
        this.m_alpha[index] = 1f;
      color.a = (__Null) (double) Mathf.Clamp01(this.m_alpha[index]);
      return color;
    }
  }
}
