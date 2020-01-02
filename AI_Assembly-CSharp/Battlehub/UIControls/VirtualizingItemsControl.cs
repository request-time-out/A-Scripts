// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingItemsControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class VirtualizingItemsControl : Selectable, IDropHandler, IUpdateSelectedHandler, IUpdateFocusedHandler, IPointerClickHandler, IEventSystemHandler
  {
    [SerializeField]
    public EventSystem m_eventSystem;
    public InputProvider InputProvider;
    public bool SelectOnPointerUp;
    public bool CanUnselectAll;
    public bool CanSelectAll;
    public bool CanEdit;
    public bool CanRemove;
    private bool m_prevCanDrag;
    public bool CanDrag;
    public bool CanReorder;
    public bool ExpandChildrenWidth;
    public bool ExpandChildrenHeight;
    private bool m_isDropInProgress;
    private List<object> m_selectionBackup;
    private bool m_isFocused;
    private bool m_isSelected;
    private Canvas m_canvas;
    public Camera Camera;
    public float ScrollSpeed;
    public Vector4 ScrollMargin;
    private VirtualizingItemsControl.ScrollDir m_scrollDir;
    private PointerEnterExitListener m_pointerEventsListener;
    private RectTransformChangeListener m_rtcListener;
    private float m_width;
    private float m_height;
    private VirtualizingItemDropMarker m_dropMarker;
    private Repeater m_repeater;
    private bool m_externalDragOperation;
    private VirtualizingItemContainer m_dropTarget;
    private ItemContainerData[] m_dragItems;
    private object[] m_dragItemsData;
    private bool m_selectionLocked;
    private List<object> m_selectedItems;
    private HashSet<object> m_selectedItemsHS;
    private int m_selectedIndex;
    private Dictionary<object, ItemContainerData> m_itemContainerData;
    private VirtualizingScrollRect m_scrollRect;

    public VirtualizingItemsControl()
    {
      base.\u002Ector();
    }

    public event EventHandler<ItemArgs> ItemBeginDrag;

    public event EventHandler<ItemDropCancelArgs> ItemBeginDrop;

    public event EventHandler<ItemDropCancelArgs> ItemDragEnter;

    public event EventHandler ItemDragExit;

    public event EventHandler<ItemArgs> ItemDrag;

    public event EventHandler<ItemDropArgs> ItemDrop;

    public event EventHandler<ItemArgs> ItemEndDrag;

    public event EventHandler<SelectionChangedArgs> SelectionChanged;

    public event EventHandler<ItemArgs> ItemDoubleClick;

    public event EventHandler<ItemArgs> ItemClick;

    public event EventHandler<ItemsCancelArgs> ItemsRemoving;

    public event EventHandler<ItemsRemovedArgs> ItemsRemoved;

    public event EventHandler IsFocusedChanged;

    public event EventHandler Submit;

    public event EventHandler Cancel;

    public event EventHandler<PointerEventArgs> Click;

    public event EventHandler<PointerEventArgs> PointerEnter;

    public event EventHandler<PointerEventArgs> PointerExit;

    protected virtual bool CanScroll
    {
      get
      {
        return this.CanReorder;
      }
    }

    protected bool IsDropInProgress
    {
      get
      {
        return this.m_isDropInProgress;
      }
    }

    public bool IsFocused
    {
      get
      {
        return this.m_isFocused;
      }
      set
      {
        if (this.m_isFocused == value)
          return;
        this.m_isFocused = value;
        if (this.IsFocusedChanged != null)
          this.IsFocusedChanged((object) this, EventArgs.Empty);
        if (!this.m_isFocused || this.SelectedIndex != -1 || (this.m_scrollRect.ItemsCount <= 0 || this.CanUnselectAll))
          return;
        this.SelectedIndex = 0;
      }
    }

    public bool IsSelected
    {
      get
      {
        return this.m_isSelected;
      }
      protected set
      {
        if (this.m_isSelected == value)
          return;
        this.m_isSelected = value;
        this.m_selectionBackup = !this.m_isSelected ? (List<object>) null : this.m_selectedItems;
        if (this.m_isSelected)
          return;
        this.IsFocused = false;
      }
    }

    public object DropTarget
    {
      get
      {
        return Object.op_Equality((Object) this.m_dropTarget, (Object) null) ? (object) null : this.m_dropTarget.Item;
      }
    }

    public void ClearTarget()
    {
      this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
    }

    public ItemDropAction DropAction
    {
      get
      {
        return Object.op_Equality((Object) this.m_dropMarker, (Object) null) ? ItemDropAction.None : this.m_dropMarker.Action;
      }
    }

    public object[] DragItems
    {
      get
      {
        return (object[]) this.m_dragItems;
      }
    }

    protected VirtualizingItemDropMarker DropMarker
    {
      get
      {
        return this.m_dropMarker;
      }
    }

    public int SelectedItemsCount
    {
      get
      {
        return this.m_selectedItems == null ? 0 : this.m_selectedItems.Count;
      }
    }

    public bool IsItemSelected(object obj)
    {
      return this.m_selectedItemsHS != null && this.m_selectedItemsHS.Contains(obj);
    }

    public virtual IEnumerable SelectedItems
    {
      get
      {
        return (IEnumerable) this.m_selectedItems;
      }
      set
      {
        if (this.m_selectionLocked)
          return;
        this.m_selectionLocked = true;
        IList selectedItems = (IList) this.m_selectedItems;
        if (value != null)
        {
          this.m_selectedItems = value.OfType<object>().ToList<object>();
          this.m_selectedItemsHS = new HashSet<object>((IEnumerable<object>) this.m_selectedItems);
          for (int index = this.m_selectedItems.Count - 1; index >= 0; --index)
          {
            object selectedItem = this.m_selectedItems[index];
            ItemContainerData itemContainerData;
            if (this.m_itemContainerData.TryGetValue(selectedItem, out itemContainerData))
              itemContainerData.IsSelected = true;
            VirtualizingItemContainer itemContainer = this.GetItemContainer(selectedItem);
            if (Object.op_Inequality((Object) itemContainer, (Object) null))
              itemContainer.IsSelected = true;
          }
          this.m_selectedIndex = this.m_selectedItems.Count != 0 ? this.IndexOf(this.m_selectedItems[0]) : -1;
        }
        else
        {
          this.m_selectedItems = (List<object>) null;
          this.m_selectedItemsHS = (HashSet<object>) null;
          this.m_selectedIndex = -1;
        }
        List<object> objectList = new List<object>();
        if (selectedItems != null)
        {
          for (int index = 0; index < selectedItems.Count; ++index)
          {
            object key = selectedItems[index];
            if (this.m_selectedItemsHS == null || !this.m_selectedItemsHS.Contains(key))
            {
              ItemContainerData itemContainerData;
              if (this.m_itemContainerData.TryGetValue(key, out itemContainerData))
                itemContainerData.IsSelected = false;
              objectList.Add(key);
              VirtualizingItemContainer itemContainer = this.GetItemContainer(key);
              if (Object.op_Inequality((Object) itemContainer, (Object) null))
                itemContainer.IsSelected = false;
            }
          }
        }
        bool flag = selectedItems == null && this.m_selectedItems != null || selectedItems != null && this.m_selectedItems == null || selectedItems != null && this.m_selectedItems != null && selectedItems.Count != this.m_selectedItems.Count;
        if (!flag && selectedItems != null)
        {
          for (int index = 0; index < selectedItems.Count; ++index)
          {
            if (this.m_selectedItems[index] != selectedItems[index])
            {
              flag = true;
              break;
            }
          }
        }
        if (flag && this.SelectionChanged != null)
        {
          object[] newItems = this.m_selectedItems != null ? this.m_selectedItems.ToArray() : new object[0];
          this.SelectionChanged((object) this, new SelectionChangedArgs(objectList.ToArray(), newItems));
        }
        this.m_selectionLocked = false;
      }
    }

    public object SelectedItem
    {
      get
      {
        return this.m_selectedItems == null || this.m_selectedItems.Count == 0 ? (object) null : this.m_selectedItems[0];
      }
      set
      {
        this.SelectedIndex = this.IndexOf(value);
      }
    }

    public int SelectedIndex
    {
      get
      {
        return this.SelectedItem == null ? -1 : this.m_selectedIndex;
      }
      set
      {
        if (this.m_selectedIndex == value || this.m_selectionLocked)
          return;
        this.m_selectionLocked = true;
        ItemContainerData itemContainerData1;
        if (this.SelectedItem != null && this.m_itemContainerData.TryGetValue(this.SelectedItem, out itemContainerData1))
          itemContainerData1.IsSelected = false;
        VirtualizingItemContainer itemContainer1 = this.GetItemContainer(this.SelectedItem);
        if (Object.op_Inequality((Object) itemContainer1, (Object) null))
          itemContainer1.IsSelected = false;
        this.m_selectedIndex = value;
        object key1 = (object) null;
        if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.m_scrollRect.ItemsCount)
        {
          key1 = this.m_scrollRect.Items[this.m_selectedIndex];
          ItemContainerData itemContainerData2;
          if (key1 != null && this.m_itemContainerData.TryGetValue(key1, out itemContainerData2))
            itemContainerData2.IsSelected = true;
          VirtualizingItemContainer itemContainer2 = this.GetItemContainer(key1);
          if (Object.op_Inequality((Object) itemContainer2, (Object) null))
            itemContainer2.IsSelected = true;
        }
        object[] objArray;
        if (key1 != null)
          objArray = new object[1]{ key1 };
        else
          objArray = new object[0];
        object[] newItems = objArray;
        object[] oldItems = this.m_selectedItems != null ? this.m_selectedItems.Except<object>((IEnumerable<object>) newItems).ToArray<object>() : new object[0];
        for (int index = 0; index < oldItems.Length; ++index)
        {
          object key2 = oldItems[index];
          ItemContainerData itemContainerData2;
          if (key2 != null && this.m_itemContainerData.TryGetValue(key2, out itemContainerData2))
            itemContainerData2.IsSelected = false;
          VirtualizingItemContainer itemContainer2 = this.GetItemContainer(key2);
          if (Object.op_Inequality((Object) itemContainer2, (Object) null))
            itemContainer2.IsSelected = false;
        }
        this.m_selectedItems = ((IEnumerable<object>) newItems).ToList<object>();
        this.m_selectedItemsHS = new HashSet<object>((IEnumerable<object>) this.m_selectedItems);
        if (this.SelectionChanged != null)
          this.SelectionChanged((object) this, new SelectionChangedArgs(oldItems, newItems));
        this.m_selectionLocked = false;
      }
    }

    public int ItemsCount
    {
      get
      {
        return this.m_scrollRect.ItemsCount;
      }
    }

    public IEnumerable Items
    {
      get
      {
        return (IEnumerable) this.m_scrollRect.Items;
      }
      set
      {
        this.SetItems(value, true);
      }
    }

    public VirtualizingScrollRect VirtualizingScrollRect
    {
      get
      {
        return this.m_scrollRect;
      }
    }

    public void SetItems(IEnumerable value, bool updateSelection)
    {
      if (value == null)
      {
        this.SelectedItems = (IEnumerable) null;
        this.m_scrollRect.Items = (IList) null;
        this.m_scrollRect.set_verticalNormalizedPosition(1f);
        this.m_scrollRect.set_horizontalNormalizedPosition(0.0f);
        this.m_itemContainerData = new Dictionary<object, ItemContainerData>();
      }
      else
      {
        List<object> list = value.OfType<object>().ToList<object>();
        if (updateSelection && this.m_selectedItemsHS != null)
          this.m_selectedItems = list.Where<object>((Func<object, bool>) (item => this.m_selectedItemsHS.Contains(item))).ToList<object>();
        this.m_itemContainerData = new Dictionary<object, ItemContainerData>();
        for (int index = 0; index < list.Count; ++index)
          this.m_itemContainerData.Add(list[index], this.InstantiateItemContainerData(list[index]));
        this.m_scrollRect.Items = (IList) list;
        if (!updateSelection || !this.IsFocused || this.SelectedIndex != -1)
          return;
        this.SelectedIndex = 0;
      }
    }

    protected virtual ItemContainerData InstantiateItemContainerData(object item)
    {
      return new ItemContainerData() { Item = item };
    }

    protected virtual void Awake()
    {
      base.Awake();
      this.m_scrollRect = (VirtualizingScrollRect) ((Component) this).GetComponent<VirtualizingScrollRect>();
      if (Object.op_Equality((Object) this.m_scrollRect, (Object) null))
        Debug.LogError((object) "Scroll Rect is required");
      this.m_scrollRect.ItemDataBinding += new DataBindAction(this.OnScrollRectItemDataBinding);
      this.m_dropMarker = (VirtualizingItemDropMarker) ((Component) this).GetComponentInChildren<VirtualizingItemDropMarker>(true);
      this.m_rtcListener = (RectTransformChangeListener) ((Component) this).GetComponentInChildren<RectTransformChangeListener>();
      if (Object.op_Inequality((Object) this.m_rtcListener, (Object) null))
        this.m_rtcListener.RectTransformChanged += new RectTransformChanged(this.OnViewportRectTransformChanged);
      this.m_pointerEventsListener = (PointerEnterExitListener) ((Component) this).GetComponentInChildren<PointerEnterExitListener>();
      if (Object.op_Inequality((Object) this.m_pointerEventsListener, (Object) null))
      {
        this.m_pointerEventsListener.PointerEnter += new EventHandler<PointerEventArgs>(this.OnViewportPointerEnter);
        this.m_pointerEventsListener.PointerExit += new EventHandler<PointerEventArgs>(this.OnViewportPointerExit);
      }
      if (Object.op_Equality((Object) this.Camera, (Object) null))
      {
        Canvas componentInParent = (Canvas) ((Component) this).GetComponentInParent<Canvas>();
        if (Object.op_Inequality((Object) componentInParent, (Object) null))
          this.Camera = componentInParent.get_worldCamera();
      }
      this.m_prevCanDrag = this.CanDrag;
      this.OnCanDragChanged();
      this.AwakeOverride();
    }

    protected virtual void Start()
    {
      ((UIBehaviour) this).Start();
      if (Object.op_Equality((Object) this.m_eventSystem, (Object) null))
        this.m_eventSystem = EventSystem.get_current();
      if (Object.op_Equality((Object) this.InputProvider, (Object) null))
      {
        this.InputProvider = (InputProvider) ((Component) this).GetComponent<InputProvider>();
        if (Object.op_Equality((Object) this.InputProvider, (Object) null))
          this.InputProvider = this.CreateInputProviderOverride();
      }
      this.m_canvas = (Canvas) ((Component) this).GetComponentInParent<Canvas>();
      this.StartOverride();
    }

    protected virtual InputProvider CreateInputProviderOverride()
    {
      return (InputProvider) ((Component) this).get_gameObject().AddComponent<VirtualizingItemsControlInputProvider>();
    }

    private void Update()
    {
      if (this.m_scrollDir != VirtualizingItemsControl.ScrollDir.None)
      {
        Rect rect1 = this.m_scrollRect.get_content().get_rect();
        double height1 = (double) ((Rect) ref rect1).get_height();
        Rect rect2 = this.m_scrollRect.get_viewport().get_rect();
        double height2 = (double) ((Rect) ref rect2).get_height();
        float num1 = (float) (height1 - height2);
        float num2 = 0.0f;
        if ((double) num1 > 0.0)
          num2 = (float) ((double) this.ScrollSpeed / 10.0 * (1.0 / (double) num1));
        Rect rect3 = this.m_scrollRect.get_content().get_rect();
        double width1 = (double) ((Rect) ref rect3).get_width();
        Rect rect4 = this.m_scrollRect.get_viewport().get_rect();
        double width2 = (double) ((Rect) ref rect4).get_width();
        float num3 = (float) (width1 - width2);
        float num4 = 0.0f;
        if ((double) num3 > 0.0)
          num4 = (float) ((double) this.ScrollSpeed / 10.0 * (1.0 / (double) num3));
        if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Up)
        {
          VirtualizingScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_verticalNormalizedPosition(scrollRect.get_verticalNormalizedPosition() + num2);
          if ((double) this.m_scrollRect.get_verticalNormalizedPosition() > 1.0)
          {
            this.m_scrollRect.set_verticalNormalizedPosition(1f);
            this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
          }
        }
        else if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Down)
        {
          VirtualizingScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_verticalNormalizedPosition(scrollRect.get_verticalNormalizedPosition() - num2);
          if ((double) this.m_scrollRect.get_verticalNormalizedPosition() < 0.0)
          {
            this.m_scrollRect.set_verticalNormalizedPosition(0.0f);
            this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
          }
        }
        else if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Left)
        {
          VirtualizingScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_horizontalNormalizedPosition(scrollRect.get_horizontalNormalizedPosition() - num4);
          if ((double) this.m_scrollRect.get_horizontalNormalizedPosition() < 0.0)
          {
            this.m_scrollRect.set_horizontalNormalizedPosition(0.0f);
            this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
          }
        }
        if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Right)
        {
          VirtualizingScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_horizontalNormalizedPosition(scrollRect.get_horizontalNormalizedPosition() + num4);
          if ((double) this.m_scrollRect.get_horizontalNormalizedPosition() > 1.0)
          {
            this.m_scrollRect.set_horizontalNormalizedPosition(1f);
            this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
          }
        }
      }
      if (Object.op_Inequality((Object) this.InputProvider, (Object) null) && this.IsSelected)
      {
        if (this.InputProvider.IsDeleteButtonDown && this.CanRemove)
          this.RemoveSelectedItems();
        else if (this.InputProvider.IsSelectAllButtonDown && this.InputProvider.IsFunctionalButtonPressed && this.CanSelectAll)
          this.SelectedItems = (IEnumerable) this.m_scrollRect.Items;
        else if (this.InputProvider.IsSubmitButtonDown)
        {
          this.IsFocused = !this.IsFocused;
          if (!this.IsFocused && this.Submit != null)
            this.Submit((object) this, EventArgs.Empty);
        }
        else if (this.InputProvider.IsCancelButtonDown)
        {
          this.SelectedItems = (IEnumerable) this.m_selectionBackup;
          this.IsFocused = false;
          if (this.Cancel != null)
            this.Cancel((object) this, EventArgs.Empty);
        }
        if (this.IsFocused)
        {
          if (!Mathf.Approximately(this.InputProvider.VerticalAxis, 0.0f))
          {
            if (this.InputProvider.IsVerticalButtonDown)
              this.m_repeater = new Repeater(Time.get_time(), 0.0f, 0.4f, 0.05f, (Action) (() =>
              {
                float verticalAxis = this.InputProvider.VerticalAxis;
                if ((double) verticalAxis < 0.0)
                {
                  if (this.m_scrollRect.Index + this.m_scrollRect.VisibleItemsCount > this.SelectedIndex + this.m_scrollRect.ContainersPerGroup)
                  {
                    if (this.SelectedIndex + this.m_scrollRect.ContainersPerGroup >= this.m_scrollRect.ItemsCount)
                      return;
                    this.SelectedIndex += this.m_scrollRect.ContainersPerGroup;
                  }
                  else
                  {
                    this.m_scrollRect.Index += this.m_scrollRect.ContainersPerGroup;
                    if (this.m_scrollRect.Index + this.m_scrollRect.VisibleItemsCount <= this.SelectedIndex + this.m_scrollRect.ContainersPerGroup || this.SelectedIndex + this.m_scrollRect.ContainersPerGroup >= this.m_scrollRect.ItemsCount)
                      return;
                    this.SelectedIndex += this.m_scrollRect.ContainersPerGroup;
                  }
                }
                else
                {
                  if ((double) verticalAxis <= 0.0)
                    return;
                  if (this.m_scrollRect.Index < this.SelectedIndex - (this.m_scrollRect.ContainersPerGroup - 1))
                  {
                    this.SelectedIndex -= this.m_scrollRect.ContainersPerGroup;
                  }
                  else
                  {
                    this.m_scrollRect.Index -= this.m_scrollRect.ContainersPerGroup;
                    if (this.m_scrollRect.Index >= this.SelectedIndex - (this.m_scrollRect.ContainersPerGroup - 1))
                      return;
                    this.SelectedIndex -= this.m_scrollRect.ContainersPerGroup;
                  }
                }
              }));
            if (this.m_repeater != null)
              this.m_repeater.Repeat(Time.get_time());
          }
          else if (!Mathf.Approximately(this.InputProvider.HorizontalAxis, 0.0f))
          {
            if (this.m_scrollRect.UseGrid)
            {
              if (this.InputProvider.IsHorizontalButtonDown)
                this.m_repeater = new Repeater(Time.get_time(), 0.0f, 0.4f, 0.05f, (Action) (() =>
                {
                  float horizontalAxis = this.InputProvider.HorizontalAxis;
                  if ((double) horizontalAxis > 0.0)
                  {
                    if (this.m_scrollRect.Index + this.m_scrollRect.VisibleItemsCount > this.SelectedIndex + this.m_scrollRect.ContainersPerGroup)
                    {
                      ++this.SelectedIndex;
                    }
                    else
                    {
                      ++this.m_scrollRect.Index;
                      if (this.m_scrollRect.Index + this.m_scrollRect.VisibleItemsCount <= this.SelectedIndex + 1 || this.SelectedIndex >= this.m_scrollRect.ItemsCount - 1)
                        return;
                      ++this.SelectedIndex;
                    }
                  }
                  else
                  {
                    if ((double) horizontalAxis >= 0.0)
                      return;
                    if (this.m_scrollRect.Index < this.SelectedIndex)
                    {
                      --this.SelectedIndex;
                    }
                    else
                    {
                      --this.m_scrollRect.Index;
                      if (this.m_scrollRect.Index >= this.SelectedIndex)
                        return;
                      --this.SelectedIndex;
                    }
                  }
                }));
              this.m_repeater.Repeat(Time.get_time());
            }
          }
          else
            this.m_repeater = (Repeater) null;
        }
      }
      if (this.m_prevCanDrag != this.CanDrag)
      {
        this.OnCanDragChanged();
        this.m_prevCanDrag = this.CanDrag;
      }
      this.UpdateOverride();
    }

    protected virtual void OnEnable()
    {
      base.OnEnable();
      VirtualizingItemContainer.Selected += new EventHandler(this.OnItemSelected);
      VirtualizingItemContainer.Unselected += new EventHandler(this.OnItemUnselected);
      VirtualizingItemContainer.PointerUp += new VirtualizingItemEventHandler(this.OnItemPointerUp);
      VirtualizingItemContainer.PointerDown += new VirtualizingItemEventHandler(this.OnItemPointerDown);
      VirtualizingItemContainer.PointerEnter += new VirtualizingItemEventHandler(this.OnItemPointerEnter);
      VirtualizingItemContainer.PointerExit += new VirtualizingItemEventHandler(this.OnItemPointerExit);
      VirtualizingItemContainer.DoubleClick += new VirtualizingItemEventHandler(this.OnItemDoubleClick);
      VirtualizingItemContainer.Click += new VirtualizingItemEventHandler(this.OnItemClick);
      VirtualizingItemContainer.BeginEdit += new EventHandler(this.OnItemBeginEdit);
      VirtualizingItemContainer.EndEdit += new EventHandler(this.OnItemEndEdit);
      VirtualizingItemContainer.BeginDrag += new VirtualizingItemEventHandler(this.OnItemBeginDrag);
      VirtualizingItemContainer.Drag += new VirtualizingItemEventHandler(this.OnItemDrag);
      VirtualizingItemContainer.Drop += new VirtualizingItemEventHandler(this.OnItemDrop);
      VirtualizingItemContainer.EndDrag += new VirtualizingItemEventHandler(this.OnItemEndDrag);
      this.OnEnableOverride();
    }

    protected virtual void OnDisable()
    {
      base.OnDisable();
      VirtualizingItemContainer.Selected -= new EventHandler(this.OnItemSelected);
      VirtualizingItemContainer.Unselected -= new EventHandler(this.OnItemUnselected);
      VirtualizingItemContainer.PointerUp -= new VirtualizingItemEventHandler(this.OnItemPointerUp);
      VirtualizingItemContainer.PointerDown -= new VirtualizingItemEventHandler(this.OnItemPointerDown);
      VirtualizingItemContainer.PointerEnter -= new VirtualizingItemEventHandler(this.OnItemPointerEnter);
      VirtualizingItemContainer.PointerExit -= new VirtualizingItemEventHandler(this.OnItemPointerExit);
      VirtualizingItemContainer.DoubleClick -= new VirtualizingItemEventHandler(this.OnItemDoubleClick);
      VirtualizingItemContainer.Click -= new VirtualizingItemEventHandler(this.OnItemClick);
      VirtualizingItemContainer.BeginEdit -= new EventHandler(this.OnItemBeginEdit);
      VirtualizingItemContainer.EndEdit -= new EventHandler(this.OnItemEndEdit);
      VirtualizingItemContainer.BeginDrag -= new VirtualizingItemEventHandler(this.OnItemBeginDrag);
      VirtualizingItemContainer.Drag -= new VirtualizingItemEventHandler(this.OnItemDrag);
      VirtualizingItemContainer.Drop -= new VirtualizingItemEventHandler(this.OnItemDrop);
      VirtualizingItemContainer.EndDrag -= new VirtualizingItemEventHandler(this.OnItemEndDrag);
      this.IsFocused = false;
      this.OnDisableOverride();
    }

    protected virtual void OnDestroy()
    {
      ((UIBehaviour) this).OnDestroy();
      if (Object.op_Inequality((Object) this.m_scrollRect, (Object) null))
        this.m_scrollRect.ItemDataBinding -= new DataBindAction(this.OnScrollRectItemDataBinding);
      if (Object.op_Inequality((Object) this.m_rtcListener, (Object) null))
        this.m_rtcListener.RectTransformChanged -= new RectTransformChanged(this.OnViewportRectTransformChanged);
      if (Object.op_Inequality((Object) this.m_pointerEventsListener, (Object) null))
      {
        this.m_pointerEventsListener.PointerEnter -= new EventHandler<PointerEventArgs>(this.OnViewportPointerEnter);
        this.m_pointerEventsListener.PointerExit -= new EventHandler<PointerEventArgs>(this.OnViewportPointerExit);
      }
      this.OnDestroyOverride();
    }

    protected virtual void AwakeOverride()
    {
    }

    protected virtual void StartOverride()
    {
    }

    protected virtual void UpdateOverride()
    {
    }

    protected virtual void OnEnableOverride()
    {
    }

    protected virtual void OnDisableOverride()
    {
    }

    protected virtual void OnDestroyOverride()
    {
    }

    private void OnItemSelected(object sender, EventArgs e)
    {
      if (this.m_selectionLocked || !this.CanHandleEvent(sender))
        return;
      VirtualizingItemContainer.Unselected -= new EventHandler(this.OnItemUnselected);
      if (this.InputProvider.IsFunctionalButtonPressed)
      {
        IList list = this.m_selectedItems == null ? (IList) new List<object>() : (IList) this.m_selectedItems.ToList<object>();
        list.Add(((VirtualizingItemContainer) sender).Item);
        this.SelectedItems = (IEnumerable) list;
      }
      else if (this.InputProvider.IsFunctional2ButtonPressed)
        this.SelectRange((VirtualizingItemContainer) sender);
      else
        this.SelectedIndex = this.IndexOf(((VirtualizingItemContainer) sender).Item);
      VirtualizingItemContainer.Unselected += new EventHandler(this.OnItemUnselected);
    }

    private void SelectRange(VirtualizingItemContainer itemContainer)
    {
      if (this.m_selectedItems != null && this.m_selectedItems.Count > 0)
      {
        List<object> objectList = new List<object>();
        int val1 = this.IndexOf(this.m_selectedItems[0]);
        int val2 = this.IndexOf(itemContainer.Item);
        int num1 = Mathf.Min(val1, val2);
        int num2 = Math.Max(val1, val2);
        objectList.Add(this.m_selectedItems[0]);
        for (int index = num1; index < val1; ++index)
          objectList.Add(this.m_scrollRect.Items[index]);
        for (int index = val1 + 1; index <= num2; ++index)
          objectList.Add(this.m_scrollRect.Items[index]);
        this.SelectedItems = (IEnumerable) objectList;
      }
      else
        this.SelectedIndex = this.IndexOf(itemContainer.Item);
    }

    private void OnItemUnselected(object sender, EventArgs e)
    {
      if (this.m_selectionLocked || !this.CanHandleEvent(sender))
        return;
      IList list = this.m_selectedItems == null ? (IList) new List<object>() : (IList) this.m_selectedItems.ToList<object>();
      list.Remove(((VirtualizingItemContainer) sender).Item);
      this.SelectedItems = (IEnumerable) list;
    }

    private void TryToSelect(VirtualizingItemContainer sender)
    {
      if (this.InputProvider.IsFunctional2ButtonPressed)
      {
        if (sender.Item != null)
          this.SelectRange(sender);
      }
      else if (this.InputProvider.IsFunctionalButtonPressed)
      {
        if (sender.Item != null)
          sender.IsSelected = !sender.IsSelected;
      }
      else if (sender.Item != null)
        sender.IsSelected = true;
      else if (this.CanUnselectAll)
        this.SelectedIndex = -1;
      this.m_eventSystem.SetSelectedGameObject(((Component) this).get_gameObject());
      this.IsFocused = true;
    }

    private void OnItemPointerDown(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender) || this.m_externalDragOperation)
        return;
      this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
      this.m_dragItems = (ItemContainerData[]) null;
      this.m_dragItemsData = (object[]) null;
      this.m_isDropInProgress = false;
      if (sender.IsSelected && eventData.get_button() == 1 || this.SelectOnPointerUp)
        return;
      this.TryToSelect(sender);
    }

    private void OnItemPointerUp(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender) || this.m_externalDragOperation || this.m_dragItems != null || sender.IsSelected && eventData.get_button() == 1)
        return;
      if (this.SelectOnPointerUp && !this.m_isDropInProgress)
        this.TryToSelect(sender);
      if (this.InputProvider.IsFunctional2ButtonPressed || this.InputProvider.IsFunctionalButtonPressed || (this.m_selectedItems == null || this.m_selectedItems.Count <= 1))
        return;
      if (this.SelectedItem == sender.Item)
        this.SelectedItem = (object) null;
      this.SelectedItem = sender.Item;
    }

    private void OnItemPointerEnter(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.m_dropTarget = sender;
      ItemDropCancelArgs e = (ItemDropCancelArgs) null;
      if (this.m_dragItems != null && this.m_dragItems.Length > 0)
      {
        e = new ItemDropCancelArgs(this.m_dragItemsData, this.m_dropTarget.Item, this.m_dropMarker.Action, this.m_externalDragOperation, eventData);
        if (this.ItemDragEnter != null)
          this.ItemDragEnter((object) this, e);
      }
      if (this.m_dragItems == null && !this.m_externalDragOperation || this.m_scrollDir != VirtualizingItemsControl.ScrollDir.None)
        return;
      if (e == null || !e.Cancel)
        this.m_dropMarker.SetTarget(this.m_dropTarget);
      else
        this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
    }

    private void OnItemPointerExit(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.m_dropTarget = (VirtualizingItemContainer) null;
      if (this.m_dragItems != null || this.m_externalDragOperation)
        this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
      if (this.m_dragItems == null || this.ItemDragExit == null)
        return;
      this.ItemDragExit((object) this, EventArgs.Empty);
    }

    private void OnItemDoubleClick(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender) || sender.Item == null || this.ItemDoubleClick == null)
        return;
      this.ItemDoubleClick((object) this, new ItemArgs(new object[1]
      {
        sender.Item
      }, eventData));
    }

    private void OnItemClick(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      if (sender.Item == null)
      {
        if (this.Click == null)
          return;
        this.Click((object) this, new PointerEventArgs(eventData));
      }
      else
      {
        if (this.ItemClick == null)
          return;
        this.ItemClick((object) this, new ItemArgs(new object[1]
        {
          sender.Item
        }, eventData));
      }
    }

    private void OnItemBeginDrag(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      if (this.m_selectedItems != null && this.m_selectedItems.Contains(sender.Item))
        this.m_dragItems = this.GetDragItems();
      else
        this.m_dragItems = new ItemContainerData[1]
        {
          this.m_itemContainerData[sender.Item]
        };
      this.m_dragItemsData = ((IEnumerable<ItemContainerData>) this.m_dragItems).Select<ItemContainerData, object>((Func<ItemContainerData, object>) (di => di.Item)).ToArray<object>();
      if (this.ItemBeginDrag != null)
        this.ItemBeginDrag((object) this, new ItemArgs(this.m_dragItemsData, eventData));
      if (Object.op_Inequality((Object) this.m_dropTarget, (Object) null))
      {
        ItemDropCancelArgs e = new ItemDropCancelArgs(this.m_dragItemsData, this.m_dropTarget.Item, this.m_dropMarker.Action, this.m_externalDragOperation, eventData);
        if (this.ItemDragEnter != null)
          this.ItemDragEnter((object) this, e);
        if (e.Cancel)
          return;
        this.m_dropMarker.SetTarget(this.m_dropTarget);
        this.m_dropMarker.SetPosition(eventData.get_position());
      }
      else
        this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
    }

    private void OnItemDrag(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.ExternalItemDrag(Vector2.op_Implicit(eventData.get_position()));
      if (this.ItemDrag != null)
        this.ItemDrag((object) this, new ItemArgs(this.m_dragItemsData, eventData));
      Rect rect1 = this.m_scrollRect.get_viewport().get_rect();
      float height = ((Rect) ref rect1).get_height();
      Rect rect2 = this.m_scrollRect.get_viewport().get_rect();
      float width = ((Rect) ref rect2).get_width();
      Camera camera = (Camera) null;
      if (this.m_canvas.get_renderMode() == 2 || this.m_canvas.get_renderMode() == 1)
        camera = this.Camera;
      if (this.CanScroll)
      {
        Vector2 vector2;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_scrollRect.get_viewport(), eventData.get_position(), camera, ref vector2))
          return;
        if (vector2.y > 0.0 && vector2.y < this.ScrollMargin.y)
        {
          this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Up;
          this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
        }
        else if (vector2.y < -(double) height && vector2.y > -((double) height + this.ScrollMargin.w))
        {
          this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Down;
          this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
        }
        else if (vector2.x < 0.0 && vector2.x >= -this.ScrollMargin.x)
          this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Left;
        else if (vector2.x >= (double) width && vector2.x < (double) width + this.ScrollMargin.z)
          this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Right;
        else
          this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
      }
      else
        this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
    }

    private void OnItemDrop(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender) || this.m_dragItems == null)
        return;
      this.m_isDropInProgress = true;
      try
      {
        if (Object.op_Inequality((Object) this.m_dropTarget, (Object) null) && this.CanDrop(this.m_dragItems, this.GetItemContainerData(this.m_dropTarget.Item)))
        {
          bool flag = false;
          if (this.ItemBeginDrop != null)
          {
            ItemDropCancelArgs e = new ItemDropCancelArgs(this.m_dragItemsData, this.m_dropTarget.Item, this.m_dropMarker.Action, false, eventData);
            if (this.ItemBeginDrop != null)
            {
              this.ItemBeginDrop((object) this, e);
              flag = e.Cancel;
            }
          }
          if (!flag)
          {
            object[] dragItems = this.m_dragItems == null ? (object[]) null : this.m_dragItemsData;
            object dropTarget = !Object.op_Inequality((Object) this.m_dropTarget, (Object) null) ? (object) null : this.m_dropTarget.Item;
            this.Drop(this.m_dragItems, this.GetItemContainerData(dropTarget), this.m_dropMarker.Action);
            if (this.ItemDrop != null && dragItems != null && (dropTarget != null && Object.op_Inequality((Object) this.m_dropMarker, (Object) null)))
              this.ItemDrop((object) this, new ItemDropArgs(dragItems, dropTarget, this.m_dropMarker.Action, false, eventData));
          }
        }
        this.RaiseEndDrag(eventData);
      }
      finally
      {
        this.m_isDropInProgress = false;
      }
    }

    private void OnItemEndDrag(VirtualizingItemContainer sender, PointerEventData eventData)
    {
      if (Object.op_Inequality((Object) this.m_dropTarget, (Object) null))
      {
        this.OnItemDrop(sender, eventData);
      }
      else
      {
        if (!this.CanHandleEvent((object) sender))
          return;
        this.RaiseEndDrag(eventData);
      }
    }

    private void RaiseEndDrag(PointerEventData eventData)
    {
      if (this.m_dragItems == null)
        return;
      if (this.ItemEndDrag != null)
        this.ItemEndDrag((object) this, new ItemArgs(this.m_dragItemsData, eventData));
      this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
      this.m_dragItems = (ItemContainerData[]) null;
      this.m_dragItemsData = (object[]) null;
      this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
    }

    private void OnViewportRectTransformChanged()
    {
      if (!this.ExpandChildrenHeight && !this.ExpandChildrenWidth)
        return;
      Rect rect = this.m_scrollRect.get_viewport().get_rect();
      if ((double) ((Rect) ref rect).get_width() == (double) this.m_width && (double) ((Rect) ref rect).get_height() == (double) this.m_height)
        return;
      this.m_width = ((Rect) ref rect).get_width();
      this.m_height = ((Rect) ref rect).get_height();
      this.SetContainersSize();
    }

    private void OnViewportPointerEnter(object sender, PointerEventArgs e)
    {
      if (this.PointerEnter == null)
        return;
      this.PointerEnter((object) this, e);
    }

    private void OnViewportPointerExit(object sender, PointerEventArgs e)
    {
      if (this.PointerExit == null)
        return;
      this.PointerExit((object) this, e);
    }

    private void SetContainersSize()
    {
      this.m_scrollRect.ForEachContainer((Action<RectTransform>) (c => this.UpdateContainerSize((VirtualizingItemContainer) ((Component) c).GetComponent<VirtualizingItemContainer>())));
    }

    public void UpdateContainerSize(VirtualizingItemContainer container)
    {
      if (!Object.op_Inequality((Object) container, (Object) null) || !Object.op_Inequality((Object) container.LayoutElement, (Object) null))
        return;
      if (this.ExpandChildrenWidth)
        container.LayoutElement.set_minWidth(this.m_width);
      if (!this.ExpandChildrenHeight)
        return;
      container.LayoutElement.set_minHeight(this.m_height);
    }

    private void OnCanDragChanged()
    {
      this.m_scrollRect.ForEachContainer((Action<RectTransform>) (c =>
      {
        VirtualizingItemContainer component = (VirtualizingItemContainer) ((Component) c).GetComponent<VirtualizingItemContainer>();
        if (!Object.op_Inequality((Object) component, (Object) null))
          return;
        component.CanDrag = this.CanDrag;
      }));
    }

    protected bool CanHandleEvent(object sender)
    {
      if (sender is ItemContainerData)
      {
        ItemContainerData itemContainerData1 = (ItemContainerData) sender;
        ItemContainerData itemContainerData2;
        return this.m_itemContainerData.TryGetValue(itemContainerData1.Item, out itemContainerData2) && itemContainerData1 == itemContainerData2;
      }
      VirtualizingItemContainer virtualizingItemContainer = sender as VirtualizingItemContainer;
      return Object.op_Implicit((Object) virtualizingItemContainer) && this.m_scrollRect.IsParentOf(((Component) virtualizingItemContainer).get_transform());
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
      if (!this.CanReorder)
        return;
      if (this.m_dragItems == null)
      {
        GameObject pointerDrag = eventData.get_pointerDrag();
        if (!Object.op_Inequality((Object) pointerDrag, (Object) null))
          return;
        ItemContainer component = (ItemContainer) pointerDrag.GetComponent<ItemContainer>();
        if (!Object.op_Inequality((Object) component, (Object) null) || component.Item == null)
          return;
        object obj = component.Item;
        if (this.ItemDrop == null)
          return;
        this.ItemDrop((object) this, new ItemDropArgs(new object[1]
        {
          obj
        }, (object) null, ItemDropAction.SetLastChild, true, eventData));
      }
      else
      {
        if (this.m_scrollRect.ItemsCount > 0)
        {
          RectTransform rectTransform = this.m_scrollRect.LastContainer();
          if (Object.op_Inequality((Object) rectTransform, (Object) null))
          {
            this.m_dropTarget = (VirtualizingItemContainer) ((Component) rectTransform).GetComponent<VirtualizingItemContainer>();
            if (this.m_dropTarget.Item == this.m_scrollRect.Items[this.m_scrollRect.Items.Count - 1])
              this.m_dropMarker.Action = ItemDropAction.SetNextSibling;
            else
              this.m_dropTarget = (VirtualizingItemContainer) null;
          }
        }
        if (!Object.op_Inequality((Object) this.m_dropTarget, (Object) null))
          return;
        this.m_isDropInProgress = true;
        try
        {
          ItemContainerData itemContainerData = this.GetItemContainerData(this.m_dropTarget.Item);
          if (this.CanDrop(this.m_dragItems, itemContainerData))
          {
            this.Drop(this.m_dragItems, itemContainerData, this.m_dropMarker.Action);
            if (this.ItemDrop != null)
              this.ItemDrop((object) this, new ItemDropArgs(this.m_dragItemsData, this.m_dropTarget.Item, this.m_dropMarker.Action, false, eventData));
          }
          this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
          this.m_dragItems = (ItemContainerData[]) null;
          this.m_dragItemsData = (object[]) null;
        }
        finally
        {
          this.m_isDropInProgress = false;
        }
      }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      base.OnPointerDown(eventData);
      if (this.CanUnselectAll)
        this.SelectedIndex = -1;
      this.m_eventSystem.SetSelectedGameObject(((Component) this).get_gameObject());
      this.IsFocused = true;
    }

    protected virtual void OnItemBeginEdit(object sender, EventArgs e)
    {
    }

    protected virtual void OnItemEndEdit(object sender, EventArgs e)
    {
    }

    public virtual void DataBindItem(object item)
    {
      VirtualizingItemContainer itemContainer = this.GetItemContainer(item);
      if (!Object.op_Inequality((Object) itemContainer, (Object) null))
        return;
      this.DataBindItem(item, itemContainer);
    }

    public virtual void DataBindItem(object item, VirtualizingItemContainer itemContainer)
    {
    }

    private void OnScrollRectItemDataBinding(RectTransform container, object item)
    {
      VirtualizingItemContainer component = (VirtualizingItemContainer) ((Component) container).GetComponent<VirtualizingItemContainer>();
      component.Item = item;
      if (item != null)
      {
        this.m_selectionLocked = true;
        ItemContainerData itemContainerData = this.m_itemContainerData[item];
        itemContainerData.IsSelected = this.IsItemSelected(item);
        component.IsSelected = itemContainerData.IsSelected;
        component.CanDrag = this.CanDrag;
        this.m_selectionLocked = false;
      }
      this.DataBindItem(item, component);
      if (this.m_scrollRect.ItemsCount != 1)
        return;
      this.SetContainersSize();
    }

    public int IndexOf(object obj)
    {
      return this.m_scrollRect.Items == null || obj == null ? -1 : this.m_scrollRect.Items.IndexOf(obj);
    }

    public virtual void SetIndex(object obj, int newIndex)
    {
      int num = this.IndexOf(obj);
      if (num == -1)
        return;
      if (num == this.m_selectedIndex)
        this.m_selectedIndex = newIndex;
      if (num < newIndex)
        this.m_scrollRect.SetNextSibling(this.GetItemAt(newIndex), obj);
      else
        this.m_scrollRect.SetPrevSibling(this.GetItemAt(newIndex), obj);
    }

    public ItemContainerData LastItemContainerData()
    {
      return this.m_scrollRect.Items == null || this.m_scrollRect.ItemsCount == 0 ? (ItemContainerData) null : this.GetItemContainerData(this.m_scrollRect.Items[this.m_scrollRect.ItemsCount - 1]);
    }

    public VirtualizingItemContainer GetItemContainer(object item)
    {
      if (item == null)
        return (VirtualizingItemContainer) null;
      RectTransform container = this.m_scrollRect.GetContainer(item);
      return Object.op_Equality((Object) container, (Object) null) ? (VirtualizingItemContainer) null : (VirtualizingItemContainer) ((Component) container).GetComponent<VirtualizingItemContainer>();
    }

    public ItemContainerData GetItemContainerData(object item)
    {
      if (item == null)
        return (ItemContainerData) null;
      ItemContainerData itemContainerData = (ItemContainerData) null;
      this.m_itemContainerData.TryGetValue(item, out itemContainerData);
      return itemContainerData;
    }

    public ItemContainerData GetItemContainerData(int siblingIndex)
    {
      return siblingIndex < 0 || this.m_scrollRect.Items.Count <= siblingIndex ? (ItemContainerData) null : this.m_itemContainerData[this.m_scrollRect.Items[siblingIndex]];
    }

    protected virtual bool CanDrop(ItemContainerData[] dragItems, ItemContainerData dropTarget)
    {
      return dropTarget == null || dragItems != null && !((IEnumerable<object>) dragItems).Contains<object>(dropTarget.Item);
    }

    protected ItemContainerData[] GetDragItems()
    {
      ItemContainerData[] itemContainerDataArray = new ItemContainerData[this.m_selectedItems.Count];
      if (this.m_selectedItems != null)
      {
        for (int index = 0; index < this.m_selectedItems.Count; ++index)
          itemContainerDataArray[index] = this.m_itemContainerData[this.m_selectedItems[index]];
      }
      return ((IEnumerable<ItemContainerData>) itemContainerDataArray).OrderBy<ItemContainerData, int>((Func<ItemContainerData, int>) (di => this.IndexOf(di.Item))).ToArray<ItemContainerData>();
    }

    protected virtual void Drop(
      ItemContainerData[] dragItems,
      ItemContainerData dropTargetData,
      ItemDropAction action)
    {
      switch (action)
      {
        case ItemDropAction.SetPrevSibling:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            ItemContainerData dragItem = dragItems[index];
            this.SetPrevSiblingInternal(dropTargetData, dragItem);
          }
          break;
        case ItemDropAction.SetNextSibling:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            ItemContainerData dragItem = dragItems[index];
            this.SetNextSiblingInternal(dropTargetData, dragItem);
          }
          break;
      }
      this.UpdateSelectedItemIndex();
    }

    protected virtual void SetNextSiblingInternal(
      ItemContainerData sibling,
      ItemContainerData nextSibling)
    {
      this.m_scrollRect.SetNextSibling(sibling.Item, nextSibling.Item);
    }

    protected virtual void SetPrevSiblingInternal(
      ItemContainerData sibling,
      ItemContainerData prevSibling)
    {
      this.m_scrollRect.SetPrevSibling(sibling.Item, prevSibling.Item);
    }

    protected void UpdateSelectedItemIndex()
    {
      this.m_selectedIndex = this.IndexOf(this.SelectedItem);
    }

    public void ExternalBeginDrag(Vector3 position)
    {
      if (!this.CanDrag)
        return;
      this.m_externalDragOperation = true;
      if (Object.op_Equality((Object) this.m_dropTarget, (Object) null) || this.m_dragItems == null && !this.m_externalDragOperation)
        return;
      if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.None)
        this.m_dropMarker.SetTarget(this.m_dropTarget);
      else
        this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
    }

    public void ExternalItemDrag(Vector3 position)
    {
      if (!this.CanDrag || !Object.op_Inequality((Object) this.m_dropTarget, (Object) null))
        return;
      this.m_dropMarker.SetPosition(Vector2.op_Implicit(position));
    }

    public void ExternalItemDrop()
    {
      if (!this.CanDrag)
        return;
      this.m_externalDragOperation = false;
      this.m_dropMarker.SetTarget((VirtualizingItemContainer) null);
    }

    public void RemoveSelectedItems()
    {
      if (this.m_selectedItems == null)
        return;
      object[] items;
      if (this.ItemsRemoving != null)
      {
        ItemsCancelArgs e = new ItemsCancelArgs(this.m_selectedItems.ToList<object>());
        this.ItemsRemoving((object) this, e);
        items = e.Items != null ? e.Items.ToArray() : new object[0];
      }
      else
        items = this.m_selectedItems.ToArray();
      if (items.Length == 0)
        return;
      this.DestroyItems(items, true);
      if (this.ItemsRemoved == null)
        return;
      this.ItemsRemoved((object) this, new ItemsRemovedArgs(items));
    }

    protected virtual void DestroyItems(object[] items, bool unselect)
    {
      if (unselect)
      {
        for (int index = 0; index < items.Length; ++index)
        {
          object obj = items[index];
          if (this.m_selectedItems != null && this.m_selectedItems.Contains(obj))
          {
            this.m_selectedItems.Remove(obj);
            this.m_selectedItemsHS.Remove(obj);
            this.m_selectedIndex = this.m_selectedItems.Count != 0 ? this.IndexOf(this.m_selectedItems[0]) : -1;
          }
        }
      }
      this.m_scrollRect.RemoveItems(((IEnumerable<object>) items).Select<object, int>((Func<object, int>) (item => this.IndexOf(item))).ToArray<int>(), true);
      for (int index = 0; index < items.Length; ++index)
        this.m_itemContainerData.Remove(items[index]);
    }

    public ItemContainerData Add(object item)
    {
      return this.Insert(this.m_scrollRect.ItemsCount, item);
    }

    public virtual ItemContainerData Insert(int index, object item)
    {
      if (this.m_itemContainerData.ContainsKey(item))
        return this.m_itemContainerData[item];
      ItemContainerData itemContainerData = this.InstantiateItemContainerData(item);
      this.m_itemContainerData.Add(item, itemContainerData);
      this.m_scrollRect.InsertItem(index, item, true);
      return itemContainerData;
    }

    public void SetNextSibling(object sibling, object nextSibling)
    {
      ItemContainerData itemContainerData1 = this.GetItemContainerData(sibling);
      if (itemContainerData1 == null)
        return;
      ItemContainerData itemContainerData2 = this.GetItemContainerData(nextSibling);
      if (itemContainerData2 == null)
        return;
      this.Drop(new ItemContainerData[1]
      {
        itemContainerData2
      }, itemContainerData1, ItemDropAction.SetNextSibling);
    }

    public void SetPrevSibling(object sibling, object prevSibling)
    {
      ItemContainerData itemContainerData1 = this.GetItemContainerData(sibling);
      if (itemContainerData1 == null)
        return;
      ItemContainerData itemContainerData2 = this.GetItemContainerData(prevSibling);
      if (itemContainerData2 == null)
        return;
      this.Drop(new ItemContainerData[1]
      {
        itemContainerData2
      }, itemContainerData1, ItemDropAction.SetPrevSibling);
    }

    protected virtual void Remove(object[] items)
    {
      items = ((IEnumerable<object>) items).Where<object>((Func<object, bool>) (item => this.m_scrollRect.Items.Contains(item))).ToArray<object>();
      if (items.Length == 0)
        return;
      if (this.ItemsRemoving != null)
      {
        ItemsCancelArgs e = new ItemsCancelArgs(((IEnumerable<object>) items).ToList<object>());
        this.ItemsRemoving((object) this, e);
        items = e.Items != null ? e.Items.ToArray() : new object[0];
      }
      if (items.Length == 0)
        return;
      this.DestroyItems(items, true);
      if (this.ItemsRemoved == null)
        return;
      this.ItemsRemoved((object) this, new ItemsRemovedArgs(items));
    }

    public virtual void Remove(object item)
    {
      this.Remove(new object[1]{ item });
    }

    public object GetItemAt(int index)
    {
      return index < 0 || index >= this.m_scrollRect.Items.Count ? (object) null : this.m_scrollRect.Items[index];
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
      base.OnSelect(eventData);
      this.IsSelected = true;
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
      base.OnDeselect(eventData);
      this.IsSelected = false;
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
      if (!this.IsFocused)
        return;
      ((AbstractEventData) eventData).Use();
    }

    public void OnUpdateFocused(BaseEventData eventData)
    {
      if (!this.IsFocused)
        return;
      ((AbstractEventData) eventData).Use();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
      if (this.Click == null)
        return;
      this.Click((object) this, new PointerEventArgs(eventData));
    }

    public void Refresh()
    {
      this.m_scrollRect.Refresh();
    }

    private enum ScrollDir
    {
      None,
      Up,
      Down,
      Left,
      Right,
    }
  }
}
