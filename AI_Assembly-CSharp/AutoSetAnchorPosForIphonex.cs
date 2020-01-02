// Decompiled with JetBrains decompiler
// Type: AutoSetAnchorPosForIphonex
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class AutoSetAnchorPosForIphonex : MonoBehaviour
{
  public Canvas mCanvas;

  public AutoSetAnchorPosForIphonex()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (Screen.get_width() == 1125 && Screen.get_height() == 2436)
    {
      float num = 0.05418719f * (float) ((CanvasScaler) ((Component) this.mCanvas).GetComponent<CanvasScaler>()).get_referenceResolution().y;
      RectTransform component = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      component.set_offsetMin(new Vector2(0.0f, num));
      component.set_offsetMax(new Vector2(0.0f, -num));
    }
    else
    {
      if (Screen.get_height() != 1125 || Screen.get_width() != 2436)
        return;
      float y = (float) ((CanvasScaler) ((Component) this.mCanvas).GetComponent<CanvasScaler>()).get_referenceResolution().y;
      float num1 = 0.056f * y;
      float num2 = (float) ((double) y / 1125.0 * 132.0);
      RectTransform component = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      component.set_offsetMin(new Vector2(num2, num1));
      component.set_offsetMax(new Vector2(-num2, 0.0f));
    }
  }
}
