// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCanvasSort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace CharaCustom
{
  public class CustomCanvasSort : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
  {
    [SerializeField]
    private CustomCanvasSortControl ccsCtrl;
    [SerializeField]
    private Canvas canvas;

    public CustomCanvasSort()
    {
      base.\u002Ector();
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
      if (!Input.GetMouseButton(0) || !Object.op_Inequality((Object) null, (Object) this.ccsCtrl))
        return;
      this.ccsCtrl.SortCanvas(this.canvas);
    }

    public virtual void OnBeginDrag(PointerEventData ped)
    {
    }

    public virtual void OnDrag(PointerEventData ped)
    {
    }

    public virtual void OnEndDrag(PointerEventData ped)
    {
    }
  }
}
