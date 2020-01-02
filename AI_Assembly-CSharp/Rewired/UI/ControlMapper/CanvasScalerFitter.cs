// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CanvasScalerFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired.Utils;
using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
  [RequireComponent(typeof (CanvasScalerExt))]
  public class CanvasScalerFitter : MonoBehaviour
  {
    [SerializeField]
    private CanvasScalerFitter.BreakPoint[] breakPoints;
    private CanvasScalerExt canvasScaler;
    private int screenWidth;
    private int screenHeight;
    private Action ScreenSizeChanged;

    public CanvasScalerFitter()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this.canvasScaler = (CanvasScalerExt) ((Component) this).GetComponent<CanvasScalerExt>();
      this.Update();
      this.canvasScaler.ForceRefresh();
    }

    private void Update()
    {
      if (Screen.get_width() == this.screenWidth && Screen.get_height() == this.screenHeight)
        return;
      this.screenWidth = Screen.get_width();
      this.screenHeight = Screen.get_height();
      this.UpdateSize();
    }

    private void UpdateSize()
    {
      if (this.canvasScaler.get_uiScaleMode() != 1 || this.breakPoints == null)
        return;
      float num1 = (float) Screen.get_width() / (float) Screen.get_height();
      float num2 = float.PositiveInfinity;
      int index1 = 0;
      for (int index2 = 0; index2 < this.breakPoints.Length; ++index2)
      {
        float num3 = Mathf.Abs(num1 - this.breakPoints[index2].screenAspectRatio);
        if (((double) num3 <= (double) this.breakPoints[index2].screenAspectRatio || MathTools.IsNear(this.breakPoints[index2].screenAspectRatio, 0.01f)) && (double) num3 < (double) num2)
        {
          num2 = num3;
          index1 = index2;
        }
      }
      this.canvasScaler.set_referenceResolution(this.breakPoints[index1].referenceResolution);
    }

    [Serializable]
    private class BreakPoint
    {
      [SerializeField]
      public string name;
      [SerializeField]
      public float screenAspectRatio;
      [SerializeField]
      public Vector2 referenceResolution;
    }
  }
}
