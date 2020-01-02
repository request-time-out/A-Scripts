// Decompiled with JetBrains decompiler
// Type: SceneAssist.PointerAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SceneAssist
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
      for (int index = 0; index < this.listClickAction.Count; ++index)
        this.listClickAction[index].Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      for (int index = 0; index < this.listDownAction.Count; ++index)
        this.listDownAction[index].Invoke();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      for (int index = 0; index < this.listEnterAction.Count; ++index)
        this.listEnterAction[index].Invoke();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      for (int index = 0; index < this.listExitAction.Count; ++index)
        this.listExitAction[index].Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      for (int index = 0; index < this.listUpAction.Count; ++index)
        this.listUpAction[index].Invoke();
    }
  }
}
