// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (RectTransform), typeof (LayoutElement))]
  public class ItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
  {
    public bool CanDrag;
    public bool CanEdit;
    public bool CanDrop;
    public GameObject ItemPresenter;
    public GameObject EditorPresenter;
    private LayoutElement m_layoutElement;
    private RectTransform m_rectTransform;
    protected bool m_isSelected;
    [SerializeField]
    private bool m_isEditing;
    private ItemsControl m_itemsControl;
    private bool m_canBeginEdit;
    private IEnumerator m_coBeginEdit;

    public ItemContainer()
    {
      base.\u002Ector();
    }

    public static event EventHandler Selected;

    public static event EventHandler Unselected;

    public static event ItemEventHandler PointerDown;

    public static event ItemEventHandler PointerUp;

    public static event ItemEventHandler DoubleClick;

    public static event ItemEventHandler PointerEnter;

    public static event ItemEventHandler PointerExit;

    public static event ItemEventHandler BeginDrag;

    public static event ItemEventHandler Drag;

    public static event ItemEventHandler Drop;

    public static event ItemEventHandler EndDrag;

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
          if (ItemContainer.Selected == null)
            return;
          ItemContainer.Selected((object) this, EventArgs.Empty);
        }
        else
        {
          if (ItemContainer.Unselected == null)
            return;
          ItemContainer.Unselected((object) this, EventArgs.Empty);
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
        if (this.m_isEditing == value || !this.m_isSelected)
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
          if (ItemContainer.BeginEdit == null)
            return;
          ItemContainer.BeginEdit((object) this, EventArgs.Empty);
        }
        else
        {
          if (ItemContainer.EndEdit == null)
            return;
          ItemContainer.EndEdit((object) this, EventArgs.Empty);
        }
      }
    }

    private ItemsControl ItemsControl
    {
      get
      {
        if (Object.op_Equality((Object) this.m_itemsControl, (Object) null))
          this.m_itemsControl = (ItemsControl) ((Component) this).GetComponentInParent<ItemsControl>();
        return this.m_itemsControl;
      }
    }

    public object Item { get; set; }

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
          this.EditorPresenter.SetActive(this.m_isEditing);
        if (Object.op_Inequality((Object) this.ItemPresenter, (Object) null))
          this.ItemPresenter.SetActive(!this.m_isEditing);
      }
      this.m_isSelected = false;
      this.Item = (object) null;
    }

    [DebuggerHidden]
    private IEnumerator CoBeginEdit()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemContainer.\u003CCoBeginEdit\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      this.m_canBeginEdit = this.m_isSelected && Object.op_Inequality((Object) this.ItemsControl, (Object) null) && this.ItemsControl.SelectedItemsCount == 1 && this.ItemsControl.CanEdit;
      if (ItemContainer.PointerDown == null)
        return;
      ItemContainer.PointerDown(this, eventData);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      if (eventData.get_clickCount() == 2)
      {
        if (ItemContainer.DoubleClick != null)
          ItemContainer.DoubleClick(this, eventData);
        if (!this.CanEdit || this.m_coBeginEdit == null)
          return;
        this.StopCoroutine(this.m_coBeginEdit);
        this.m_coBeginEdit = (IEnumerator) null;
      }
      else
      {
        if (this.m_canBeginEdit && this.m_coBeginEdit == null)
        {
          this.m_coBeginEdit = this.CoBeginEdit();
          this.StartCoroutine(this.m_coBeginEdit);
        }
        if (ItemContainer.PointerUp == null)
          return;
        ItemContainer.PointerUp(this, eventData);
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
        if (ItemContainer.BeginDrag == null)
          return;
        ItemContainer.BeginDrag(this, eventData);
      }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
      if (ItemContainer.Drop == null)
        return;
      ItemContainer.Drop(this, eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      if (!this.CanDrag)
      {
        ExecuteEvents.ExecuteHierarchy<IDragHandler>(((Component) ((Component) this).get_transform().get_parent()).get_gameObject(), (BaseEventData) eventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_dragHandler());
      }
      else
      {
        if (ItemContainer.Drag == null)
          return;
        ItemContainer.Drag(this, eventData);
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
        if (ItemContainer.EndDrag == null)
          return;
        ItemContainer.EndDrag(this, eventData);
      }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      if (ItemContainer.PointerEnter == null)
        return;
      ItemContainer.PointerEnter(this, eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      if (ItemContainer.PointerExit == null)
        return;
      ItemContainer.PointerExit(this, eventData);
    }
  }
}
