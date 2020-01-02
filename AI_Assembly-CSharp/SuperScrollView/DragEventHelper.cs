// Decompiled with JetBrains decompiler
// Type: SuperScrollView.DragEventHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperScrollView
{
  public class DragEventHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
  {
    public DragEventHelper.OnDragEventHandler mOnBeginDragHandler;
    public DragEventHelper.OnDragEventHandler mOnDragHandler;
    public DragEventHelper.OnDragEventHandler mOnEndDragHandler;

    public DragEventHelper()
    {
      base.\u002Ector();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (this.mOnBeginDragHandler == null)
        return;
      this.mOnBeginDragHandler(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (this.mOnDragHandler == null)
        return;
      this.mOnDragHandler(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      if (this.mOnEndDragHandler == null)
        return;
      this.mOnEndDragHandler(eventData);
    }

    public delegate void OnDragEventHandler(PointerEventData eventData);
  }
}
