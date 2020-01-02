// Decompiled with JetBrains decompiler
// Type: UGUI_AssistLibrary.UIAL_EventTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UGUI_AssistLibrary
{
  public class UIAL_EventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
  {
    private List<UIAL_EventTrigger.Entry> m_Delegates;

    protected UIAL_EventTrigger()
    {
      base.\u002Ector();
    }

    public List<UIAL_EventTrigger.Entry> triggers
    {
      get
      {
        if (this.m_Delegates == null)
          this.m_Delegates = new List<UIAL_EventTrigger.Entry>();
        return this.m_Delegates;
      }
      set
      {
        this.m_Delegates = value;
      }
    }

    private void Execute(EventTriggerType id, BaseEventData eventData)
    {
      int index = 0;
      for (int count = this.triggers.Count; index < count; ++index)
      {
        UIAL_EventTrigger.Entry trigger = this.triggers[index];
        if (trigger.eventID == id && trigger.callback != null && (id != 4 || this.isClick(trigger.buttonType)))
          trigger.callback.Invoke(eventData);
      }
    }

    private bool isClick(UIAL_EventTrigger.ButtonType type)
    {
      return (type & UIAL_EventTrigger.ButtonType.Left) != (UIAL_EventTrigger.ButtonType) 0 && Input.GetMouseButtonUp(0) || (type & UIAL_EventTrigger.ButtonType.Right) != (UIAL_EventTrigger.ButtonType) 0 && Input.GetMouseButtonUp(1) || (type & UIAL_EventTrigger.ButtonType.Center) != (UIAL_EventTrigger.ButtonType) 0 && Input.GetMouseButtonUp(2);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 0, (BaseEventData) eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 1, (BaseEventData) eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 4, (BaseEventData) eventData);
    }

    public class TriggerEvent : UnityEvent<BaseEventData>
    {
      public TriggerEvent()
      {
        base.\u002Ector();
      }
    }

    [Flags]
    public enum ButtonType
    {
      Left = 1,
      Right = 2,
      Center = 4,
    }

    public class Entry
    {
      public UIAL_EventTrigger.ButtonType buttonType = UIAL_EventTrigger.ButtonType.Left | UIAL_EventTrigger.ButtonType.Right | UIAL_EventTrigger.ButtonType.Center;
      public EventTriggerType eventID = (EventTriggerType) 4;
      public UIAL_EventTrigger.TriggerEvent callback = new UIAL_EventTrigger.TriggerEvent();
    }
  }
}
