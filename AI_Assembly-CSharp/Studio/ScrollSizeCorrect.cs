// Decompiled with JetBrains decompiler
// Type: Studio.ScrollSizeCorrect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class ScrollSizeCorrect : MonoBehaviour
  {
    [SerializeField]
    private RectTransform transBase;
    [SerializeField]
    private RectTransform transTarget;
    [SerializeField]
    private Scrollbar scrollbar;

    public ScrollSizeCorrect()
    {
      base.\u002Ector();
    }

    private void OnRectTransformDimensionsChange()
    {
      Rect rect1 = this.transBase.get_rect();
      double height1 = (double) ((Rect) ref rect1).get_height();
      Rect rect2 = this.transTarget.get_rect();
      double height2 = (double) ((Rect) ref rect2).get_height();
      this.scrollbar.set_size(Mathf.Min(1f, (float) (height1 / height2)));
      if ((double) this.scrollbar.get_value() != 0.0)
        return;
      this.scrollbar.set_value(0.1f);
      this.scrollbar.set_value(0.0f);
    }
  }
}
