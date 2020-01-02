// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.EventTriggerNoScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.EventSystems
{
  [AddComponentMenu("Event/Event Trigger NoScroll")]
  public class EventTriggerNoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
  {
    [SerializeField]
    private List<EventTriggerNoScroll.Entry> m_Delegates;

    protected EventTriggerNoScroll()
    {
      base.\u002Ector();
    }

    public List<EventTriggerNoScroll.Entry> triggers
    {
      get
      {
        if (this.m_Delegates == null)
          this.m_Delegates = new List<EventTriggerNoScroll.Entry>();
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
        EventTriggerNoScroll.Entry trigger = this.triggers[index];
        if (trigger.eventID == id)
          trigger.callback?.Invoke(eventData);
      }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 0, (BaseEventData) eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 1, (BaseEventData) eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 5, (BaseEventData) eventData);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 6, (BaseEventData) eventData);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 2, (BaseEventData) eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 3, (BaseEventData) eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 4, (BaseEventData) eventData);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
      this.Execute((EventTriggerType) 9, eventData);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
      this.Execute((EventTriggerType) 10, eventData);
    }

    public virtual void OnMove(AxisEventData eventData)
    {
      this.Execute((EventTriggerType) 11, (BaseEventData) eventData);
    }

    public virtual void OnUpdateSelected(BaseEventData eventData)
    {
      this.Execute((EventTriggerType) 8, eventData);
    }

    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 12, (BaseEventData) eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 13, (BaseEventData) eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      this.Execute((EventTriggerType) 14, (BaseEventData) eventData);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Execute((EventTriggerType) 15, eventData);
    }

    public virtual void OnCancel(BaseEventData eventData)
    {
      this.Execute((EventTriggerType) 16, eventData);
    }

    [Serializable]
    public class TriggerEvent : UnityEvent<BaseEventData>
    {
      public TriggerEvent()
      {
        base.\u002Ector();
      }
    }

    [Serializable]
    public class Entry
    {
      public EventTriggerType eventID = (EventTriggerType) 4;
      public EventTriggerNoScroll.TriggerEvent callback = new EventTriggerNoScroll.TriggerEvent();
    }
  }
}
