// Decompiled with JetBrains decompiler
// Type: Studio.PointerAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Studio
{
  public class PointerAction : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IEventSystemHandler
  {
    public List<UnityAction> listClickAction;
    public List<UnityAction> listDownAction;
    public List<UnityAction> listEnterAction;
    public List<UnityAction> listExitAction;
    public List<UnityAction> listUpAction;

    public PointerAction()
    {
      base.\u002Ector();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      using (List<UnityAction>.Enumerator enumerator = this.listClickAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      using (List<UnityAction>.Enumerator enumerator = this.listDownAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      using (List<UnityAction>.Enumerator enumerator = this.listEnterAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      using (List<UnityAction>.Enumerator enumerator = this.listExitAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      using (List<UnityAction>.Enumerator enumerator = this.listUpAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }
  }
}
