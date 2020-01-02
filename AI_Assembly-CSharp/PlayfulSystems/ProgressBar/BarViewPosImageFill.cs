// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewPosImageFill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (RectTransform))]
  public class BarViewPosImageFill : ProgressBarProView
  {
    [SerializeField]
    private RectTransform rectTrans;
    [SerializeField]
    private Image referenceImage;
    [Range(-1f, 1f)]
    [SerializeField]
    private float offset;

    public override void UpdateView(float currentValue, float targetValue)
    {
      this.rectTrans.set_anchorMin(this.GetAnchor(currentValue));
      this.rectTrans.set_anchorMax(this.GetAnchor(currentValue));
    }

    private Vector2 GetAnchor(float currentDisplay)
    {
      switch ((int) this.referenceImage.get_fillMethod())
      {
        case 0:
          return this.GetAnchorHorizontal(currentDisplay, this.referenceImage.get_fillOrigin());
        case 1:
          return this.GetAnchorVertical(currentDisplay, this.referenceImage.get_fillOrigin());
        case 4:
          return this.GetAnchorRadial360(currentDisplay, this.referenceImage.get_fillOrigin(), this.referenceImage.get_fillClockwise());
        default:
          return Vector2.op_Multiply(Vector2.get_one(), 0.5f);
      }
    }

    private Vector2 GetAnchorHorizontal(float fillAmount, int fillOrigin)
    {
      return new Vector2(fillOrigin != 1 ? fillAmount : 1f - fillAmount, (float) (0.5 + 0.5 * (double) this.offset));
    }

    private Vector2 GetAnchorVertical(float fillAmount, int fillOrigin)
    {
      return new Vector2((float) (0.5 + 0.5 * (double) this.offset), fillOrigin != 1 ? fillAmount : 1f - fillAmount);
    }

    private Vector2 GetAnchorRadial360(float fillAmount, int fillOrigin, bool fillClockwise)
    {
      float degrees = (float) (360.0 * (!fillClockwise ? (double) fillAmount : -(double) fillAmount));
      switch (fillOrigin)
      {
        case 0:
          degrees += !fillClockwise ? 90f : -90f;
          break;
        case 2:
          degrees += !fillClockwise ? 270f : -270f;
          break;
        case 3:
          degrees += !fillClockwise ? 180f : -180f;
          break;
      }
      return this.GetPointOnCircle(degrees);
    }

    private Vector2 GetPointOnCircle(float degrees)
    {
      float num = (float) (0.25 + 0.25 * (double) this.offset);
      return new Vector2((float) (0.5 + (double) num * (double) Mathf.Cos((float) Math.PI / 180f * degrees)), (float) (0.5 + (double) num * (double) Mathf.Sin((float) Math.PI / 180f * degrees)));
    }
  }
}
