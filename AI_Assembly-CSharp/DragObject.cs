// Decompiled with JetBrains decompiler
// Type: DragObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Studio;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IEventSystemHandler
{
  [SerializeField]
  protected Canvas m_Canvas;
  protected RectTransform m_RectCanvas;
  protected RectTransform m_RectTransform;
  protected Rect rectArea;
  protected Vector2 vecRate;

  public DragObject()
  {
    base.\u002Ector();
  }

  protected Canvas canvas
  {
    get
    {
      if (Object.op_Equality((Object) this.m_Canvas, (Object) null))
        this.m_Canvas = (Canvas) ((Component) this).GetComponentInParent<Canvas>();
      return this.m_Canvas;
    }
  }

  protected RectTransform rectCanvas
  {
    get
    {
      if (Object.op_Equality((Object) this.m_RectCanvas, (Object) null))
        this.m_RectCanvas = ((Component) this.canvas).get_transform() as RectTransform;
      return this.m_RectCanvas;
    }
  }

  protected RectTransform rectTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.m_RectTransform, (Object) null))
        this.m_RectTransform = ((Component) this).get_transform() as RectTransform;
      return this.m_RectTransform;
    }
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    SortCanvas.select = this.canvas;
    Rect pixelRect = this.canvas.get_pixelRect();
    Vector2 sizeDelta1 = this.rectCanvas.get_sizeDelta();
    Vector2 sizeDelta2 = this.rectTransform.get_sizeDelta();
    Vector2 anchorMax = this.rectTransform.get_anchorMax();
    Vector2 pivot = this.rectTransform.get_pivot();
    ((Rect) ref this.rectArea).Set((float) (sizeDelta1.x * anchorMax.x + sizeDelta2.x * pivot.x), (float) (-(sizeDelta1.y * anchorMax.y) + sizeDelta2.y * pivot.y), (float) (sizeDelta1.x - sizeDelta2.x), (float) (sizeDelta1.y - sizeDelta2.y));
    this.vecRate.x = (__Null) ((double) ((Rect) ref pixelRect).get_width() / sizeDelta1.x);
    this.vecRate.y = (__Null) ((double) ((Rect) ref pixelRect).get_height() / sizeDelta1.y);
  }

  public void OnDrag(PointerEventData eventData)
  {
    Vector2 delta = eventData.get_delta();
    ref Vector2 local1 = ref delta;
    local1.x = local1.x / this.vecRate.x;
    ref Vector2 local2 = ref delta;
    local2.y = local2.y / this.vecRate.y;
    this.rectTransform.set_anchoredPosition(Rect.NormalizedToPoint(this.rectArea, Rect.PointToNormalized(this.rectArea, Vector2.op_Addition(delta, this.rectTransform.get_anchoredPosition()))));
  }

  public void OnEndDrag(PointerEventData eventData)
  {
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    SortCanvas.select = this.canvas;
  }
}
