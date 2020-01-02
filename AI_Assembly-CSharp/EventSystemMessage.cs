// Decompiled with JetBrains decompiler
// Type: EventSystemMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
  private bool _UpdateSelected;

  public EventSystemMessage()
  {
    base.\u002Ector();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnPointerEnter : " + (object) eventData));
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnPointerExit : " + (object) eventData));
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnPointerDown : " + (object) eventData));
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnPointerUp : " + (object) eventData));
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnPointerClick : " + (object) eventData));
  }

  public void OnInitializePotentialDrag(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnInitializePotentialDrag : " + (object) eventData));
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnBeginDrag : " + (object) eventData));
  }

  public void OnDrag(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnDrag : " + (object) eventData));
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnEndDrag : " + (object) eventData));
  }

  public void OnDrop(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnDrop : " + (object) eventData));
  }

  public void OnScroll(PointerEventData eventData)
  {
    MonoBehaviour.print((object) ("OnScroll : " + (object) eventData));
  }

  public void OnUpdateSelected(BaseEventData eventData)
  {
    if (this._UpdateSelected)
      return;
    MonoBehaviour.print((object) ("OnUpdateSelected : " + (object) eventData));
    this._UpdateSelected = true;
  }

  public void OnSelect(BaseEventData eventData)
  {
    MonoBehaviour.print((object) ("OnSelect : " + (object) eventData));
    this._UpdateSelected = false;
  }

  public void OnDeselect(BaseEventData eventData)
  {
    MonoBehaviour.print((object) ("OnDeselect : " + (object) eventData));
  }

  public void OnMove(AxisEventData eventData)
  {
    MonoBehaviour.print((object) ("OnMove : " + (object) eventData));
  }

  public void OnSubmit(BaseEventData eventData)
  {
    MonoBehaviour.print((object) ("OnSubmit : " + (object) eventData));
  }

  public void OnCancel(BaseEventData eventData)
  {
    MonoBehaviour.print((object) ("OnCancel : " + (object) eventData));
  }
}
