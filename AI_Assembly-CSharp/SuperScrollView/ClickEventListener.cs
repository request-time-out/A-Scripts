// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ClickEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperScrollView
{
  public class ClickEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Action<GameObject> mClickedHandler;
    private Action<GameObject> mDoubleClickedHandler;
    private Action<GameObject> mOnPointerDownHandler;
    private Action<GameObject> mOnPointerUpHandler;
    private bool mIsPressed;

    public ClickEventListener()
    {
      base.\u002Ector();
    }

    public static ClickEventListener Get(GameObject obj)
    {
      ClickEventListener clickEventListener = (ClickEventListener) obj.GetComponent<ClickEventListener>();
      if (Object.op_Equality((Object) clickEventListener, (Object) null))
        clickEventListener = (ClickEventListener) obj.AddComponent<ClickEventListener>();
      return clickEventListener;
    }

    public bool IsPressd
    {
      get
      {
        return this.mIsPressed;
      }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.get_clickCount() == 2)
      {
        if (this.mDoubleClickedHandler == null)
          return;
        this.mDoubleClickedHandler(((Component) this).get_gameObject());
      }
      else
      {
        if (this.mClickedHandler == null)
          return;
        this.mClickedHandler(((Component) this).get_gameObject());
      }
    }

    public void SetClickEventHandler(Action<GameObject> handler)
    {
      this.mClickedHandler = handler;
    }

    public void SetDoubleClickEventHandler(Action<GameObject> handler)
    {
      this.mDoubleClickedHandler = handler;
    }

    public void SetPointerDownHandler(Action<GameObject> handler)
    {
      this.mOnPointerDownHandler = handler;
    }

    public void SetPointerUpHandler(Action<GameObject> handler)
    {
      this.mOnPointerUpHandler = handler;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      this.mIsPressed = true;
      if (this.mOnPointerDownHandler == null)
        return;
      this.mOnPointerDownHandler(((Component) this).get_gameObject());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      this.mIsPressed = false;
      if (this.mOnPointerUpHandler == null)
        return;
      this.mOnPointerUpHandler(((Component) this).get_gameObject());
    }
  }
}
