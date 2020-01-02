// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingItemDropMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (RectTransform))]
  public class VirtualizingItemDropMarker : MonoBehaviour
  {
    private VirtualizingItemsControl m_itemsControl;
    private Canvas m_parentCanvas;
    public GameObject SiblingGraphics;
    private ItemDropAction m_action;
    protected RectTransform m_rectTransform;
    private VirtualizingItemContainer m_item;

    public VirtualizingItemDropMarker()
    {
      base.\u002Ector();
    }

    protected Canvas ParentCanvas
    {
      get
      {
        return this.m_parentCanvas;
      }
    }

    public virtual ItemDropAction Action
    {
      get
      {
        return this.m_action;
      }
      set
      {
        this.m_action = value;
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this.m_rectTransform;
      }
    }

    protected VirtualizingItemContainer Item
    {
      get
      {
        return this.m_item;
      }
    }

    private void Awake()
    {
      this.m_rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.SiblingGraphics.SetActive(true);
      this.m_parentCanvas = (Canvas) ((Component) this).GetComponentInParent<Canvas>();
      this.m_itemsControl = (VirtualizingItemsControl) ((Component) this).GetComponentInParent<VirtualizingItemsControl>();
      this.AwakeOverride();
    }

    protected virtual void AwakeOverride()
    {
    }

    public virtual void SetTarget(VirtualizingItemContainer item)
    {
      ((Component) this).get_gameObject().SetActive(Object.op_Inequality((Object) item, (Object) null));
      this.m_item = item;
      if (!Object.op_Equality((Object) this.m_item, (Object) null))
        return;
      this.Action = ItemDropAction.None;
    }

    public virtual void SetPosition(Vector2 position)
    {
      if (Object.op_Equality((Object) this.m_item, (Object) null) || !this.m_itemsControl.CanReorder)
        return;
      RectTransform rectTransform1 = this.Item.RectTransform;
      Vector2 sizeDelta = this.m_rectTransform.get_sizeDelta();
      ref Vector2 local = ref sizeDelta;
      Rect rect1 = rectTransform1.get_rect();
      double height = (double) ((Rect) ref rect1).get_height();
      local.y = (__Null) height;
      this.m_rectTransform.set_sizeDelta(sizeDelta);
      Camera camera = (Camera) null;
      if (this.ParentCanvas.get_renderMode() == 2 || this.ParentCanvas.get_renderMode() == 1)
        camera = this.m_itemsControl.Camera;
      Vector2 vector2;
      if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform1, position, camera, ref vector2))
        return;
      // ISSUE: variable of the null type
      __Null y = vector2.y;
      Rect rect2 = rectTransform1.get_rect();
      double num = -(double) ((Rect) ref rect2).get_height() / 2.0;
      if (y > num)
      {
        this.Action = ItemDropAction.SetPrevSibling;
        ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
      }
      else
      {
        this.Action = ItemDropAction.SetNextSibling;
        ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
        RectTransform rectTransform2 = this.RectTransform;
        Vector3 localPosition = ((Transform) this.RectTransform).get_localPosition();
        Rect rect3 = rectTransform1.get_rect();
        Vector3 vector3_1 = new Vector3(0.0f, (float) ((double) ((Rect) ref rect3).get_height() * (double) this.ParentCanvas.get_scaleFactor()), 0.0f);
        Vector3 vector3_2 = Vector3.op_Subtraction(localPosition, vector3_1);
        ((Transform) rectTransform2).set_localPosition(vector3_2);
      }
    }
  }
}
