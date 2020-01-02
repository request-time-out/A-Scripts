// Decompiled with JetBrains decompiler
// Type: Studio.MapDragButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class MapDragButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IInitializePotentialDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    [SerializeField]
    private Button button;
    public Action onBeginDragFunc;
    public Action onDragFunc;
    public Action onEndDragFunc;

    public MapDragButton()
    {
      base.\u002Ector();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.cameraCtrl.isCursorLock = false;
      if (Singleton<GameCursor>.IsInstance())
        Singleton<GameCursor>.Instance.SetCursorLock(true);
      if (!Object.op_Implicit((Object) this.button))
        return;
      ((Selectable) this.button).set_transition((Selectable.Transition) 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (this.onBeginDragFunc == null)
        return;
      this.onBeginDragFunc();
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (this.onDragFunc == null)
        return;
      this.onDragFunc();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.cameraCtrl.isCursorLock = true;
      if (Singleton<GameCursor>.IsInstance())
        Singleton<GameCursor>.Instance.SetCursorLock(false);
      if (Object.op_Implicit((Object) this.button))
        ((Selectable) this.button).set_transition((Selectable.Transition) 1);
      if (this.onEndDragFunc == null)
        return;
      this.onEndDragFunc();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.cameraCtrl.isCursorLock = true;
      if (Singleton<GameCursor>.IsInstance())
        Singleton<GameCursor>.Instance.SetCursorLock(false);
      if (!Object.op_Implicit((Object) this.button))
        return;
      ((Selectable) this.button).set_transition((Selectable.Transition) 1);
    }
  }
}
