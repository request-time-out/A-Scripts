// Decompiled with JetBrains decompiler
// Type: ConfigScene.SliderUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public class SliderUI : Slider
  {
    public SliderUI()
    {
      base.\u002Ector();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      if (this.get_onValueChanged() != null)
        ((UnityEvent<float>) this.get_onValueChanged()).Invoke(this.get_value());
      base.OnPointerDown(eventData);
    }
  }
}
