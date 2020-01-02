// Decompiled with JetBrains decompiler
// Type: UI_DragWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DragWindow : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IEventSystemHandler
{
  public RectTransform rtDrag;
  public RectTransform rtMove;
  public RectTransform rtCanvas;
  private Canvas canvas;
  private CanvasScaler cscaler;
  private Vector2 dragStartPosBackup;
  private CameraControl camCtrl;

  public UI_DragWindow()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) null, (Object) this.rtMove))
      this.rtMove = ((Component) this).get_transform() as RectTransform;
    if (Object.op_Equality((Object) null, (Object) this.rtDrag))
      this.rtDrag = this.rtMove;
    if (Object.op_Equality((Object) null, (Object) this.rtCanvas))
      this.SearchCanvas();
    if (Object.op_Inequality((Object) null, (Object) this.rtCanvas) && Object.op_Equality((Object) null, (Object) this.canvas))
    {
      this.canvas = (Canvas) ((Component) this.rtCanvas).GetComponent<Canvas>();
      if (Object.op_Implicit((Object) this.canvas))
        this.cscaler = (CanvasScaler) ((Component) this.rtCanvas).GetComponent<CanvasScaler>();
    }
    if (!Object.op_Equality((Object) this.camCtrl, (Object) null) || !Object.op_Implicit((Object) Camera.get_main()))
      return;
    this.camCtrl = (CameraControl) ((Component) Camera.get_main()).GetComponent<CameraControl>();
  }

  private void SearchCanvas()
  {
    GameObject gameObject = ((Component) this).get_gameObject();
    while (true)
    {
      this.canvas = (Canvas) gameObject.GetComponent<Canvas>();
      if (!Object.op_Implicit((Object) this.canvas))
      {
        if (!Object.op_Equality((Object) null, (Object) gameObject.get_transform().get_parent()))
          gameObject = ((Component) gameObject.get_transform().get_parent()).get_gameObject();
        else
          goto label_5;
      }
      else
        break;
    }
    this.rtCanvas = gameObject.get_transform() as RectTransform;
    return;
label_5:;
  }

  private float GetScreenRate()
  {
    float width = (float) Screen.get_width();
    float height = (float) Screen.get_height();
    Vector2 one = Vector2.get_one();
    one.x = (__Null) ((double) width / this.cscaler.get_referenceResolution().x);
    one.y = (__Null) ((double) height / this.cscaler.get_referenceResolution().y);
    return (float) (one.x * (1.0 - (double) this.cscaler.get_matchWidthOrHeight()) + one.y * (double) this.cscaler.get_matchWidthOrHeight());
  }

  private void CalcDragPosOverlay(PointerEventData ped)
  {
    Vector2 vector2 = Vector2.op_Subtraction(ped.get_position(), this.dragStartPosBackup);
    vector2.x = vector2.x / ((Transform) this.rtCanvas).get_localScale().x;
    vector2.y = vector2.y / ((Transform) this.rtCanvas).get_localScale().y;
    Rect rect1 = this.rtDrag.get_rect();
    // ISSUE: variable of the null type
    __Null local1;
    if (((Rect) ref rect1).get_size().x == this.rtDrag.get_sizeDelta().x)
    {
      local1 = this.rtDrag.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtDrag.get_rect();
      local1 = ((Rect) ref rect2).get_size().x - this.rtDrag.get_sizeDelta().x;
    }
    float num1 = (float) local1;
    Rect rect3 = this.rtDrag.get_rect();
    // ISSUE: variable of the null type
    __Null local2;
    if (((Rect) ref rect3).get_size().y == this.rtDrag.get_sizeDelta().y)
    {
      local2 = this.rtDrag.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtDrag.get_rect();
      local2 = ((Rect) ref rect2).get_size().y - this.rtDrag.get_sizeDelta().y;
    }
    float num2 = (float) local2;
    vector2.x = (__Null) (double) Mathf.Clamp((float) vector2.x, 0.0f, (float) Screen.get_width() / (float) ((Transform) this.rtCanvas).get_localScale().x - num1);
    vector2.y = (__Null) -(double) Mathf.Clamp((float) -vector2.y, 0.0f, (float) Screen.get_height() / (float) ((Transform) this.rtCanvas).get_localScale().y - num2);
    Rect rect4 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local3;
    if (((Rect) ref rect4).get_size().x == this.rtMove.get_sizeDelta().x)
    {
      local3 = this.rtMove.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local3 = ((Rect) ref rect2).get_size().x - this.rtMove.get_sizeDelta().x;
    }
    float num3 = (float) local3;
    Rect rect5 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local4;
    if (((Rect) ref rect5).get_size().y == this.rtMove.get_sizeDelta().y)
    {
      local4 = this.rtMove.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local4 = ((Rect) ref rect2).get_size().y - this.rtMove.get_sizeDelta().y;
    }
    float num4 = (float) local4;
    vector2.x = (__Null) (vector2.x + (double) num3 * this.rtMove.get_pivot().x);
    vector2.y = (__Null) (vector2.y + (double) num4 * (this.rtMove.get_pivot().y - 1.0));
    this.rtMove.set_anchoredPosition(vector2);
  }

  private void CalcDragPosScreenSpace(PointerEventData ped)
  {
    Vector2 vector2 = Vector2.op_Subtraction(ped.get_position(), this.dragStartPosBackup);
    float screenRate = this.GetScreenRate();
    vector2.x = (__Null) (vector2.x / (double) screenRate);
    vector2.y = (__Null) (vector2.y / (double) screenRate);
    Rect rect1 = this.rtDrag.get_rect();
    // ISSUE: variable of the null type
    __Null local1;
    if (((Rect) ref rect1).get_size().x == this.rtDrag.get_sizeDelta().x)
    {
      local1 = this.rtDrag.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtDrag.get_rect();
      local1 = ((Rect) ref rect2).get_size().x - this.rtDrag.get_sizeDelta().x;
    }
    float num1 = (float) local1;
    Rect rect3 = this.rtDrag.get_rect();
    // ISSUE: variable of the null type
    __Null local2;
    if (((Rect) ref rect3).get_size().y == this.rtDrag.get_sizeDelta().y)
    {
      local2 = this.rtDrag.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtDrag.get_rect();
      local2 = ((Rect) ref rect2).get_size().y - this.rtDrag.get_sizeDelta().y;
    }
    float num2 = (float) local2;
    vector2.x = (__Null) (double) Mathf.Clamp((float) vector2.x, 0.0f, (float) Screen.get_width() / screenRate - num1);
    vector2.y = (__Null) -(double) Mathf.Clamp((float) -vector2.y, 0.0f, (float) Screen.get_height() / screenRate - num2);
    Rect rect4 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local3;
    if (((Rect) ref rect4).get_size().x == this.rtMove.get_sizeDelta().x)
    {
      local3 = this.rtMove.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local3 = ((Rect) ref rect2).get_size().x - this.rtMove.get_sizeDelta().x;
    }
    float num3 = (float) local3;
    Rect rect5 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local4;
    if (((Rect) ref rect5).get_size().y == this.rtMove.get_sizeDelta().y)
    {
      local4 = this.rtMove.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local4 = ((Rect) ref rect2).get_size().y - this.rtMove.get_sizeDelta().y;
    }
    float num4 = (float) local4;
    vector2.x = (__Null) (vector2.x + (double) num3 * this.rtMove.get_pivot().x);
    vector2.y = (__Null) (vector2.y + (double) num4 * (this.rtMove.get_pivot().y - 1.0));
    this.rtMove.set_anchoredPosition(vector2);
  }

  private void SetClickPosOverlay(PointerEventData ped)
  {
    Vector2 zero = Vector2.get_zero();
    Rect rect1 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local1;
    if (((Rect) ref rect1).get_size().x == this.rtMove.get_sizeDelta().x)
    {
      local1 = this.rtMove.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local1 = ((Rect) ref rect2).get_size().x - this.rtMove.get_sizeDelta().x;
    }
    float num1 = (float) local1;
    Rect rect3 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local2;
    if (((Rect) ref rect3).get_size().y == this.rtMove.get_sizeDelta().y)
    {
      local2 = this.rtMove.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local2 = ((Rect) ref rect2).get_size().y - this.rtMove.get_sizeDelta().y;
    }
    float num2 = (float) local2;
    zero.x = (__Null) ((this.rtMove.get_anchoredPosition().x - (double) num1 * this.rtMove.get_pivot().x) * ((Transform) this.rtCanvas).get_localScale().x);
    zero.y = (__Null) ((this.rtMove.get_anchoredPosition().y - (double) num2 * (this.rtMove.get_pivot().y - 1.0)) * ((Transform) this.rtCanvas).get_localScale().y);
    this.dragStartPosBackup = Vector2.op_Subtraction(ped.get_position(), zero);
  }

  private void SetClickPosScreenSpace(PointerEventData ped)
  {
    float screenRate = this.GetScreenRate();
    Vector2 zero = Vector2.get_zero();
    Rect rect1 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local1;
    if (((Rect) ref rect1).get_size().x == this.rtMove.get_sizeDelta().x)
    {
      local1 = this.rtMove.get_sizeDelta().x;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local1 = ((Rect) ref rect2).get_size().x - this.rtMove.get_sizeDelta().x;
    }
    float num1 = (float) local1;
    Rect rect3 = this.rtMove.get_rect();
    // ISSUE: variable of the null type
    __Null local2;
    if (((Rect) ref rect3).get_size().y == this.rtMove.get_sizeDelta().y)
    {
      local2 = this.rtMove.get_sizeDelta().y;
    }
    else
    {
      Rect rect2 = this.rtMove.get_rect();
      local2 = ((Rect) ref rect2).get_size().y - this.rtMove.get_sizeDelta().y;
    }
    float num2 = (float) local2;
    zero.x = (__Null) ((this.rtMove.get_anchoredPosition().x - (double) num1 * this.rtMove.get_pivot().x) * (double) screenRate);
    zero.y = (__Null) ((this.rtMove.get_anchoredPosition().y - (double) num2 * (this.rtMove.get_pivot().y - 1.0)) * (double) screenRate);
    this.dragStartPosBackup = Vector2.op_Subtraction(ped.get_position(), zero);
  }

  public void OnPointerDown(PointerEventData ped)
  {
    switch ((int) this.canvas.get_renderMode())
    {
      case 0:
        this.SetClickPosOverlay(ped);
        break;
      case 1:
        this.SetClickPosScreenSpace(ped);
        break;
    }
    if (!Object.op_Implicit((Object) this.camCtrl))
      return;
    this.camCtrl.NoCtrlCondition = (BaseCameraControl.NoCtrlFunc) (() => true);
  }

  public void OnBeginDrag(PointerEventData ped)
  {
    switch ((int) this.canvas.get_renderMode())
    {
      case 0:
        this.CalcDragPosOverlay(ped);
        break;
      case 1:
        this.CalcDragPosScreenSpace(ped);
        break;
    }
  }

  public void OnDrag(PointerEventData ped)
  {
    switch ((int) this.canvas.get_renderMode())
    {
      case 0:
        this.CalcDragPosOverlay(ped);
        break;
      case 1:
        this.CalcDragPosScreenSpace(ped);
        break;
    }
  }

  public void OnEndDrag(PointerEventData ped)
  {
    if (!Object.op_Implicit((Object) this.camCtrl))
      return;
    this.camCtrl.NoCtrlCondition = (BaseCameraControl.NoCtrlFunc) (() => false);
  }

  public void OnPointerUp(PointerEventData ped)
  {
    if (!Object.op_Implicit((Object) this.camCtrl))
      return;
    this.camCtrl.NoCtrlCondition = (BaseCameraControl.NoCtrlFunc) (() => false);
  }
}
