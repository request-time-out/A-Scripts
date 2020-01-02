// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.PointerEnterExitListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class PointerEnterExitListener : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IEventSystemHandler
  {
    public PointerEnterExitListener()
    {
      base.\u002Ector();
    }

    public event EventHandler<PointerEventArgs> PointerEnter;

    public event EventHandler<PointerEventArgs> PointerExit;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      if (this.PointerEnter == null)
        return;
      this.PointerEnter((object) this, new PointerEventArgs(eventData));
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      if (this.PointerExit == null)
        return;
      this.PointerExit((object) this, new PointerEventArgs(eventData));
    }
  }
}
