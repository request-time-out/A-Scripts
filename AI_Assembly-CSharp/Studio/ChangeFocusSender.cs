// Decompiled with JetBrains decompiler
// Type: Studio.ChangeFocusSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class ChangeFocusSender : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
  {
    [SerializeField]
    private ChangeFocus changeFocus;
    [SerializeField]
    private int index;

    public ChangeFocusSender()
    {
      base.\u002Ector();
    }

    public void OnDeselect(BaseEventData eventData)
    {
      if (!Object.op_Implicit((Object) this.changeFocus))
        return;
      this.changeFocus.select = -1;
    }

    public void OnSelect(BaseEventData eventData)
    {
      if (!Object.op_Implicit((Object) this.changeFocus))
        return;
      this.changeFocus.select = this.index;
    }
  }
}
