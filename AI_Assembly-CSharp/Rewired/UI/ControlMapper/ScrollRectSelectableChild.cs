// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ScrollRectSelectableChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (Selectable))]
  public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
  {
    public bool useCustomEdgePadding;
    public float customEdgePadding;
    private ScrollRect parentScrollRect;
    private Selectable _selectable;

    public ScrollRectSelectableChild()
    {
      base.\u002Ector();
    }

    private RectTransform parentScrollRectContentTransform
    {
      get
      {
        return this.parentScrollRect.get_content();
      }
    }

    private Selectable selectable
    {
      get
      {
        return this._selectable ?? (this._selectable = (Selectable) ((Component) this).GetComponent<Selectable>());
      }
    }

    private RectTransform rectTransform
    {
      get
      {
        return ((Component) this).get_transform() as RectTransform;
      }
    }

    private void Start()
    {
      this.parentScrollRect = (ScrollRect) ((Component) ((Component) this).get_transform()).GetComponentInParent<ScrollRect>();
      if (!Object.op_Equality((Object) this.parentScrollRect, (Object) null))
        return;
      Debug.LogError((object) "Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
    }

    public void OnSelect(BaseEventData eventData)
    {
      if (Object.op_Equality((Object) this.parentScrollRect, (Object) null) || !(eventData is AxisEventData))
        return;
      RectTransform transform = ((Component) this.parentScrollRect).get_transform() as RectTransform;
      Rect rect1 = MathTools.TransformRect(this.rectTransform.get_rect(), (Transform) this.rectTransform, (Transform) transform);
      Rect rect2 = transform.get_rect();
      Rect rect3 = transform.get_rect();
      float num = !this.useCustomEdgePadding ? ((Rect) ref rect1).get_height() : this.customEdgePadding;
      ref Rect local1 = ref rect3;
      ((Rect) ref local1).set_yMax(((Rect) ref local1).get_yMax() - num);
      ref Rect local2 = ref rect3;
      ((Rect) ref local2).set_yMin(((Rect) ref local2).get_yMin() + num);
      Vector2 vector2;
      if (MathTools.RectContains(rect3, rect1) || !MathTools.GetOffsetToContainRect(rect3, rect1, ref vector2))
        return;
      Vector2 anchoredPosition = this.parentScrollRectContentTransform.get_anchoredPosition();
      anchoredPosition.x = (__Null) (double) Mathf.Clamp((float) (anchoredPosition.x + vector2.x), 0.0f, Mathf.Abs(((Rect) ref rect2).get_width() - (float) this.parentScrollRectContentTransform.get_sizeDelta().x));
      anchoredPosition.y = (__Null) (double) Mathf.Clamp((float) (anchoredPosition.y + vector2.y), 0.0f, Mathf.Abs(((Rect) ref rect2).get_height() - (float) this.parentScrollRectContentTransform.get_sizeDelta().y));
      this.parentScrollRectContentTransform.set_anchoredPosition(anchoredPosition);
    }
  }
}
