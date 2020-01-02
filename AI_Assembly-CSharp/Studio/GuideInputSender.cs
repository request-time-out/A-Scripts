// Decompiled with JetBrains decompiler
// Type: Studio.GuideInputSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideInputSender : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
  {
    [SerializeField]
    private GuideInput guideInput;
    [SerializeField]
    private int index;

    public GuideInputSender()
    {
      base.\u002Ector();
    }

    public void OnDeselect(BaseEventData eventData)
    {
      if (!Object.op_Implicit((Object) this.guideInput))
        return;
      this.guideInput.selectIndex = -1;
    }

    public void OnSelect(BaseEventData eventData)
    {
      if (!Object.op_Implicit((Object) this.guideInput))
        return;
      this.guideInput.selectIndex = this.index;
    }
  }
}
