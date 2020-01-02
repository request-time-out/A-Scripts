// Decompiled with JetBrains decompiler
// Type: ConfigScene.ToggleUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public class ToggleUI : Toggle
  {
    public ToggleUI()
    {
      base.\u002Ector();
    }

    public event ToggleUI.OnClickDelegate onPointerClick;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);
      if (this.onPointerClick == null)
        return;
      this.onPointerClick(this.get_isOn());
    }

    public delegate void OnClickDelegate(bool _value);
  }
}
