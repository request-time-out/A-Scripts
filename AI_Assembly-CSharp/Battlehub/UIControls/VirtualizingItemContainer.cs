// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingItemContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (RectTransform), typeof (LayoutElement))]
  public class VirtualizingItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IPointerClickHandler, IEventSystemHandler
  {
    [HideInInspector]
    public bool CanDrag;
    [HideInInspector]
    public bool CanEdit;
    [HideInInspector]
    public bool CanBeParent;
    [HideInInspector]
    public bool CanSelect;
    public GameObject ItemPresenter;
    public GameObject EditorPresenter;
    private LayoutElement m_layoutElement;
    private RectTransform m_rectTransform;
    protected bool m_isSelected;
    private bool m_isEditing;
    private VirtualizingItemsControl m_itemsControl;
    private object m_item;
    private bool m_canBeginEdit;
    private IEnumerator m_coBeginEdit;

    public VirtualizingItemContainer()
    {
      base.\u002Ector();
    }

    public static event EventHandler Selected;

    public static event EventHandler Unselected;

    public static event VirtualizingItemEventHandler PointerDown;

    public static event VirtualizingItemEventHandler PointerUp;

    public static event VirtualizingItemEventHandler DoubleClick;

    public static event VirtualizingItemEventHandler Click;

    public static event VirtualizingItemEventHandler PointerEnter;

    public static event VirtualizingItemEventHandler PointerExit;

    public static event VirtualizingItemEventHandler BeginDrag;

    public static event VirtualizingItemEventHandler Drag;

    public static event VirtualizingItemEventHandler Drop;

    public static event VirtualizingItemEventHandler EndDrag;

    public static event EventHandler BeginEdit;

    public static event EventHandler EndEdit;

    public LayoutElement LayoutElement
    {
      get
      {
        return this.m_layoutElement;
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this.m_rectTransform;
      }
    }

    public virtual bool IsSelected
    {
      get
      {
        return this.m_isSelected;
      }
      set
      {
        if (this.m_isSelected == value)
          return;
        this.m_isSelected = value;
        if (this.m_isSelected)
        {
          if (VirtualizingItemContainer.Selected == null)
            return;
          VirtualizingItemContainer.Selected((object) this, EventArgs.Empty);
        }
        else
        {
          if (VirtualizingItemContainer.Unselected == null)
            return;
          VirtualizingItemContainer.Unselected((object) this, EventArgs.Empty);
        }
      }
    }

    public bool IsEditing
    {
      get
      {
        return this.m_isEditing;
      }
      set
      {
        if (this.Item == null || this.m_isEditing == value || !this.m_isSelected)
          return;
        this.m_isEditing = value && this.m_isSelected;
        if (Object.op_Inequality((Object) this.EditorPresenter, (Object) this.ItemPresenter))
        {
          if (Object.op_Inequality((Object) this.EditorPresenter, (Object) null))
            this.EditorPresenter.SetActive(this.m_isEditing);
          if (Object.op_Inequality((Object) this.ItemPresenter, (Object) null))
            this.ItemPresenter.SetActive(!this.m_isEditing);
        }
        if (this.m_isEditing)
        {
          if (VirtualizingItemContainer.BeginEdit == null)
            return;
          VirtualizingItemContainer.BeginEdit((object) this, EventArgs.Empty);
        }
        else
        {
          if (VirtualizingItemContainer.EndEdit == null)
            return;
          VirtualizingItemContainer.EndEdit((object) this, EventArgs.Empty);
        }
      }
    }

    protected VirtualizingItemsControl ItemsControl
    {
      get
      {
        if (Object.op_Equality((Object) this.m_itemsControl, (Object) null))
        {
          this.m_itemsControl = (VirtualizingItemsControl) ((Component) this).GetComponentInParent<VirtualizingItemsControl>();
          if (Object.op_Equality((Object) this.m_itemsControl, (Object) null))
          {
            for (Transform parent = ((Component) this).get_transform().get_parent(); Object.op_Inequality((Object) parent, (Object) null); parent = parent.get_parent())
            {
              this.m_itemsControl = (VirtualizingItemsControl) ((Component) parent).GetComponent<VirtualizingItemsControl>();
              if (Object.op_Inequality((Object) this.m_itemsControl, (Object) null))
                break;
            }
          }
        }
        return this.m_itemsControl;
      }
    }

    public virtual object Item
    {
      get
      {
        return this.m_item;
      }
      set
      {
        this.m_item = value;
        if (this.m_isEditing)
        {
          if (Object.op_Inequality((Object) this.EditorPresenter, (Object) null))
            this.EditorPresenter.SetActive(this.m_item != null);
        }
        else if (Object.op_Inequality((Object) this.ItemPresenter, (Object) null))
          this.ItemPresenter.SetActive(this.m_item != null);
        if (this.m_item != null)
          return;
        this.IsSelected = false;
      }
    }

    private void Awake()
    {
      this.m_rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.m_layoutElement = (LayoutElement) ((Component) this).GetComponent<LayoutElement>();
      if (Object.op_Equality((Object) this.ItemPresenter, (Object) null))
        this.ItemPresenter = ((Component) this).get_gameObject();
      if (Object.op_Equality((Object) this.EditorPresenter, (Object) null))
        this.EditorPresenter = ((Component) this).get_gameObject();
      this.AwakeOverride();
    }

    private void Start()
    {
      this.StartOverride();
      this.ItemsControl.UpdateContainerSize(this);
    }

    private void OnDestroy()
    {
      this.StopAllCoroutines();
      this.m_coBeginEdit = (IEnumerator) null;
      this.OnDestroyOverride();
    }

    protected virtual void AwakeOverride()
    {
    }

    protected virtual void StartOverride()
    {
    }

    protected virtual void OnDestroyOverride()
    {
    }

    public virtual void Clear()
    {
      this.m_isEditing = false;
      if (Object.op_Inequality((Object) this.EditorPresenter, (Object) this.ItemPresenter))
      {
        if (Object.op_Inequality((Object) this.EditorPresenter, (Object) null))
          this.EditorPresenter.SetActive(this.m_item != null && this.m_isEditing);
        if (Object.op_Inequality((Object) this.ItemPresenter, (Object) null))
          this.ItemPresenter.SetActive(this.m_item != null && !this.m_isEditing);
      }
      if (this.m_item != null)
        return;
      this.IsSelected = false;
    }

    [DebuggerHidden]
    private IEnumerator CoBeginEdit()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new VirtualizingItemContainer.\u003CCoBeginEdit\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      this.m_canBeginEdit = this.m_isSelected && Object.op_Inequality((Object) this.ItemsControl, (Object) null) && this.ItemsControl.SelectedItemsCount == 1 && this.ItemsControl.CanEdit;
      if (!this.CanSelect || VirtualizingItemContainer.PointerDown == null)
        return;
      VirtualizingItemContainer.PointerDown(this, eventData);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      if (eventData.get_clickCount() == 2)
      {
        if (VirtualizingItemContainer.DoubleClick != null)
          VirtualizingItemContainer.DoubleClick(this, eventData);
        if (!this.CanEdit || eventData.get_button() != null || this.m_coBeginEdit == null)
          return;
        this.StopCoroutine(this.m_coBeginEdit);
        this.m_coBeginEdit = (IEnumerator) null;
      }
      else
      {
        if (this.m_canBeginEdit && eventData.get_button() == null && this.m_coBeginEdit == null)
        {
          this.m_coBeginEdit = this.CoBeginEdit();
          this.StartCoroutine(this.m_coBeginEdit);
        }
        if (!this.CanSelect || VirtualizingItemContainer.PointerUp == null)
          return;
        VirtualizingItemContainer.PointerUp(this, eventData);
      }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
      if (!this.CanDrag)
      {
        ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(((Component) ((Component) this).get_transform().get_parent()).get_gameObject(), (BaseEventData) eventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_beginDragHandler());
      }
      else
      {
        this.m_canBeginEdit = false;
        if (VirtualizingItemContainer.BeginDrag == null)
          return;
        VirtualizingItemContainer.BeginDrag(this, eventData);
      }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
      if (VirtualizingItemContainer.Drop == null)
        return;
      VirtualizingItemContainer.Drop(this, eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      if (!this.CanDrag)
      {
        ExecuteEvents.ExecuteHierarchy<IDragHandler>(((Component) ((Component) this).get_transform().get_parent()).get_gameObject(), (BaseEventData) eventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_dragHandler());
      }
      else
      {
        if (VirtualizingItemContainer.Drag == null)
          return;
        VirtualizingItemContainer.Drag(this, eventData);
      }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
      if (!this.CanDrag)
      {
        ExecuteEvents.ExecuteHierarchy<IEndDragHandler>(((Component) ((Component) this).get_transform().get_parent()).get_gameObject(), (BaseEventData) eventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_endDragHandler());
      }
      else
      {
        if (VirtualizingItemContainer.EndDrag == null)
          return;
        VirtualizingItemContainer.EndDrag(this, eventData);
      }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      if (VirtualizingItemContainer.PointerEnter == null)
        return;
      VirtualizingItemContainer.PointerEnter(this, eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      if (VirtualizingItemContainer.PointerExit == null)
        return;
      VirtualizingItemContainer.PointerExit(this, eventData);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
      if (VirtualizingItemContainer.Click == null)
        return;
      VirtualizingItemContainer.Click(this, eventData);
    }
  }
}
