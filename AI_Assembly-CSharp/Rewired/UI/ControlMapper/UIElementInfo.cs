// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIElementInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
  {
    public string identifier;
    public int intData;
    public Text text;

    protected UIElementInfo()
    {
      base.\u002Ector();
    }

    public event Action<GameObject> OnSelectedEvent;

    public void OnSelect(BaseEventData eventData)
    {
      if (this.OnSelectedEvent == null)
        return;
      this.OnSelectedEvent(((Component) this).get_gameObject());
    }
  }
}
