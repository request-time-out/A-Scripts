// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemsControl
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
  public abstract class ItemsControl : MonoBehaviour, IPointerDownHandler, IDropHandler, IEventSystemHandler
  {
    public KeyCode MultiselectKey;
    public KeyCode RangeselectKey;
    public KeyCode SelectAllKey;
    public KeyCode RemoveKey;
    public bool SelectOnPointerUp;
    public bool CanUnselectAll;
    public bool CanEdit;
    private bool m_prevCanDrag;
    public bool CanDrag;
    public bool CanReorder;
    public bool ExpandChildrenWidth;
    public bool ExpandChildrenHeight;
    private bool m_isDropInProgress;
    [SerializeField]
    private GameObject ItemContainerPrefab;
    [SerializeField]
    protected Transform Panel;
    private Canvas m_canvas;
    public Camera Camera;
    public float ScrollSpeed;
    private ItemsControl.ScrollDir m_scrollDir;
    private ScrollRect m_scrollRect;
    private RectTransformChangeListener m_rtcListener;
    private float m_width;
    private float m_height;
    private List<ItemContainer> m_itemContainers;
    private ItemDropMarker m_dropMarker;
    private bool m_externalDragOperation;
    private ItemContainer m_dropTarget;
    private ItemContainer[] m_dragItems;
    private IList<object> m_items;
    private bool m_selectionLocked;
    private List<object> m_selectedItems;
    private HashSet<object> m_selectedItemsHS;
    private ItemContainer m_selectedItemContainer;
    private int m_selectedIndex;

    protected ItemsControl()
    {
      base.\u002Ector();
    }

    public event EventHandler<ItemArgs> ItemBeginDrag;

    public event EventHandler<ItemDropCancelArgs> ItemBeginDrop;

    public event EventHandler<ItemDropArgs> ItemDrop;

    public event EventHandler<ItemArgs> ItemEndDrag;

    public event EventHandler<SelectionChangedArgs> SelectionChanged;

    public event EventHandler<ItemArgs> ItemDoubleClick;

    public event EventHandler<ItemsCancelArgs> ItemsRemoving;

    public event EventHandler<ItemsRemovedArgs> ItemsRemoved;

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

    public object DropTarget
    {
      get
      {
        return Object.op_Equality((Object) this.m_dropTarget, (Object) null) ? (object) null : this.m_dropTarget.Item;
      }
    }

    public ItemDropAction DropAction
    {
      get
      {
        return Object.op_Equality((Object) this.m_dropMarker, (Object) null) ? ItemDropAction.None : this.m_dropMarker.Action;
      }
    }

    protected ItemDropMarker DropMarker
    {
      get
      {
        return this.m_dropMarker;
      }
    }

    public IEnumerable Items
    {
      get
      {
        return (IEnumerable) this.m_items;
      }
      set
      {
        if (value == null)
        {
          this.m_items = (IList<object>) null;
          this.m_scrollRect.set_verticalNormalizedPosition(1f);
          this.m_scrollRect.set_horizontalNormalizedPosition(0.0f);
        }
        else
          this.m_items = (IList<object>) value.OfType<object>().ToList<object>();
        this.DataBind();
      }
    }

    public int ItemsCount
    {
      get
      {
        return this.m_items == null ? 0 : this.m_items.Count;
      }
    }

    protected void RemoveItemAt(int index)
    {
      this.m_items.RemoveAt(index);
    }

    protected void RemoveItemContainerAt(int index)
    {
      this.m_itemContainers.RemoveAt(index);
    }

    protected void InsertItem(int index, object value)
    {
      this.m_items.Insert(index, value);
    }

    protected void InsertItemContainerAt(int index, ItemContainer container)
    {
      this.m_itemContainers.Insert(index, container);
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
            ItemContainer itemContainer = this.GetItemContainer(this.m_selectedItems[index]);
            if (Object.op_Inequality((Object) itemContainer, (Object) null))
              itemContainer.IsSelected = true;
          }
          if (this.m_selectedItems.Count == 0)
          {
            this.m_selectedItemContainer = (ItemContainer) null;
            this.m_selectedIndex = -1;
          }
          else
          {
            this.m_selectedItemContainer = this.GetItemContainer(this.m_selectedItems[0]);
            this.m_selectedIndex = this.IndexOf(this.m_selectedItems[0]);
          }
        }
        else
        {
          this.m_selectedItems = (List<object>) null;
          this.m_selectedItemsHS = (HashSet<object>) null;
          this.m_selectedItemContainer = (ItemContainer) null;
          this.m_selectedIndex = -1;
        }
        List<object> objectList = new List<object>();
        if (selectedItems != null)
        {
          for (int index = 0; index < selectedItems.Count; ++index)
          {
            object obj = selectedItems[index];
            if (this.m_selectedItemsHS == null || !this.m_selectedItemsHS.Contains(obj))
            {
              objectList.Add(obj);
              ItemContainer itemContainer = this.GetItemContainer(obj);
              if (Object.op_Inequality((Object) itemContainer, (Object) null))
                itemContainer.IsSelected = false;
            }
          }
        }
        if (this.SelectionChanged != null)
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
        ItemContainer selectedItemContainer = this.m_selectedItemContainer;
        if (Object.op_Inequality((Object) selectedItemContainer, (Object) null))
          selectedItemContainer.IsSelected = false;
        this.m_selectedIndex = value;
        object obj = (object) null;
        if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.m_items.Count)
        {
          obj = this.m_items[this.m_selectedIndex];
          this.m_selectedItemContainer = this.GetItemContainer(obj);
          if (Object.op_Inequality((Object) this.m_selectedItemContainer, (Object) null))
            this.m_selectedItemContainer.IsSelected = true;
        }
        object[] objArray;
        if (obj != null)
          objArray = new object[1]{ obj };
        else
          objArray = new object[0];
        object[] newItems = objArray;
        object[] oldItems = this.m_selectedItems != null ? this.m_selectedItems.Except<object>((IEnumerable<object>) newItems).ToArray<object>() : new object[0];
        for (int index = 0; index < oldItems.Length; ++index)
        {
          ItemContainer itemContainer = this.GetItemContainer(oldItems[index]);
          if (Object.op_Inequality((Object) itemContainer, (Object) null))
            itemContainer.IsSelected = false;
        }
        this.m_selectedItems = ((IEnumerable<object>) newItems).ToList<object>();
        this.m_selectedItemsHS = new HashSet<object>((IEnumerable<object>) this.m_selectedItems);
        if (this.SelectionChanged != null)
          this.SelectionChanged((object) this, new SelectionChangedArgs(oldItems, newItems));
        this.m_selectionLocked = false;
      }
    }

    public int IndexOf(object obj)
    {
      return this.m_items == null || obj == null ? -1 : this.m_items.IndexOf(obj);
    }

    public virtual void SetIndex(object obj, int newIndex)
    {
      int index = this.IndexOf(obj);
      if (index == -1)
        return;
      if (index == this.m_selectedIndex)
        this.m_selectedIndex = newIndex;
      this.m_items.RemoveAt(index);
      this.m_items.Insert(newIndex, obj);
      ItemContainer itemContainer = this.m_itemContainers[index];
      this.m_itemContainers.RemoveAt(index);
      this.m_itemContainers.Insert(newIndex, itemContainer);
      ((Component) itemContainer).get_transform().SetSiblingIndex(newIndex);
    }

    public ItemContainer GetItemContainer(object obj)
    {
      return this.m_itemContainers.Where<ItemContainer>((Func<ItemContainer, bool>) (ic => ic.Item == obj)).FirstOrDefault<ItemContainer>();
    }

    public ItemContainer LastItemContainer()
    {
      return this.m_itemContainers == null || this.m_itemContainers.Count == 0 ? (ItemContainer) null : this.m_itemContainers[this.m_itemContainers.Count - 1];
    }

    public ItemContainer GetItemContainer(int siblingIndex)
    {
      return siblingIndex < 0 || siblingIndex >= this.m_itemContainers.Count ? (ItemContainer) null : this.m_itemContainers[siblingIndex];
    }

    public void ExternalBeginDrag(Vector3 position)
    {
      if (!this.CanDrag)
        return;
      this.m_externalDragOperation = true;
      if (Object.op_Equality((Object) this.m_dropTarget, (Object) null) || this.m_dragItems == null && !this.m_externalDragOperation || this.m_scrollDir != ItemsControl.ScrollDir.None)
        return;
      this.m_dropMarker.SetTraget(this.m_dropTarget);
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
      this.m_dropMarker.SetTraget((ItemContainer) null);
    }

    public ItemContainer Add(object item)
    {
      if (this.m_items == null)
      {
        this.m_items = (IList<object>) new List<object>();
        this.m_itemContainers = new List<ItemContainer>();
      }
      return this.Insert(this.m_items.Count, item);
    }

    public virtual ItemContainer Insert(int index, object item)
    {
      if (this.m_items == null)
      {
        this.m_items = (IList<object>) new List<object>();
        this.m_itemContainers = new List<ItemContainer>();
      }
      ItemContainer itemContainer1 = this.GetItemContainer(this.m_items.ElementAtOrDefault<object>(index));
      int siblingIndex = !Object.op_Inequality((Object) itemContainer1, (Object) null) ? this.m_itemContainers.Count : this.m_itemContainers.IndexOf(itemContainer1);
      this.m_items.Insert(index, item);
      ItemContainer itemContainer2 = this.InstantiateItemContainer(siblingIndex);
      if (Object.op_Inequality((Object) itemContainer2, (Object) null))
      {
        itemContainer2.Item = item;
        this.DataBindItem(item, itemContainer2);
      }
      return itemContainer2;
    }

    public void SetNextSibling(object sibling, object nextSibling)
    {
      ItemContainer itemContainer1 = this.GetItemContainer(sibling);
      if (Object.op_Equality((Object) itemContainer1, (Object) null))
        return;
      ItemContainer itemContainer2 = this.GetItemContainer(nextSibling);
      if (Object.op_Equality((Object) itemContainer2, (Object) null))
        return;
      this.Drop(new ItemContainer[1]{ itemContainer2 }, itemContainer1, ItemDropAction.SetNextSibling);
    }

    public void SetPrevSibling(object sibling, object prevSibling)
    {
      ItemContainer itemContainer1 = this.GetItemContainer(sibling);
      if (Object.op_Equality((Object) itemContainer1, (Object) null))
        return;
      ItemContainer itemContainer2 = this.GetItemContainer(prevSibling);
      if (Object.op_Equality((Object) itemContainer2, (Object) null))
        return;
      this.Drop(new ItemContainer[1]{ itemContainer2 }, itemContainer1, ItemDropAction.SetPrevSibling);
    }

    public virtual void Remove(object item)
    {
      if (item == null || this.m_items == null || !this.m_items.Contains(item))
        return;
      this.DestroyItem(item);
    }

    public void RemoveAt(int index)
    {
      if (this.m_items == null)
        return;
      if (index >= this.m_items.Count || index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      this.Remove(this.m_items[index]);
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) this.Panel, (Object) null))
        this.Panel = ((Component) this).get_transform();
      this.m_itemContainers = ((IEnumerable<ItemContainer>) ((Component) this).GetComponentsInChildren<ItemContainer>()).ToList<ItemContainer>();
      this.m_rtcListener = (RectTransformChangeListener) ((Component) this).GetComponentInChildren<RectTransformChangeListener>();
      if (Object.op_Inequality((Object) this.m_rtcListener, (Object) null))
        this.m_rtcListener.RectTransformChanged += new RectTransformChanged(this.OnViewportRectTransformChanged);
      this.m_dropMarker = (ItemDropMarker) ((Component) this).GetComponentInChildren<ItemDropMarker>(true);
      this.m_scrollRect = (ScrollRect) ((Component) this).GetComponent<ScrollRect>();
      if (Object.op_Equality((Object) this.Camera, (Object) null))
        this.Camera = Camera.get_main();
      this.m_prevCanDrag = this.CanDrag;
      this.OnCanDragChanged();
      this.AwakeOverride();
    }

    private void Start()
    {
      this.m_canvas = (Canvas) ((Component) this).GetComponentInParent<Canvas>();
      this.StartOverride();
    }

    private void Update()
    {
      if (this.m_scrollDir != ItemsControl.ScrollDir.None)
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
        if (this.m_scrollDir == ItemsControl.ScrollDir.Up)
        {
          ScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_verticalNormalizedPosition(scrollRect.get_verticalNormalizedPosition() + num2);
          if ((double) this.m_scrollRect.get_verticalNormalizedPosition() > 1.0)
          {
            this.m_scrollRect.set_verticalNormalizedPosition(1f);
            this.m_scrollDir = ItemsControl.ScrollDir.None;
          }
        }
        else if (this.m_scrollDir == ItemsControl.ScrollDir.Down)
        {
          ScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_verticalNormalizedPosition(scrollRect.get_verticalNormalizedPosition() - num2);
          if ((double) this.m_scrollRect.get_verticalNormalizedPosition() < 0.0)
          {
            this.m_scrollRect.set_verticalNormalizedPosition(0.0f);
            this.m_scrollDir = ItemsControl.ScrollDir.None;
          }
        }
        else if (this.m_scrollDir == ItemsControl.ScrollDir.Left)
        {
          ScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_horizontalNormalizedPosition(scrollRect.get_horizontalNormalizedPosition() - num4);
          if ((double) this.m_scrollRect.get_horizontalNormalizedPosition() < 0.0)
          {
            this.m_scrollRect.set_horizontalNormalizedPosition(0.0f);
            this.m_scrollDir = ItemsControl.ScrollDir.None;
          }
        }
        if (this.m_scrollDir == ItemsControl.ScrollDir.Right)
        {
          ScrollRect scrollRect = this.m_scrollRect;
          scrollRect.set_horizontalNormalizedPosition(scrollRect.get_horizontalNormalizedPosition() + num4);
          if ((double) this.m_scrollRect.get_horizontalNormalizedPosition() > 1.0)
          {
            this.m_scrollRect.set_horizontalNormalizedPosition(1f);
            this.m_scrollDir = ItemsControl.ScrollDir.None;
          }
        }
      }
      if (Input.GetKeyDown(this.RemoveKey))
        this.RemoveSelectedItems();
      if (Input.GetKeyDown(this.SelectAllKey) && Input.GetKey(this.RangeselectKey))
        this.SelectedItems = (IEnumerable) this.m_items;
      if (this.m_prevCanDrag != this.CanDrag)
      {
        this.OnCanDragChanged();
        this.m_prevCanDrag = this.CanDrag;
      }
      this.UpdateOverride();
    }

    private void OnEnable()
    {
      ItemContainer.Selected += new EventHandler(this.OnItemSelected);
      ItemContainer.Unselected += new EventHandler(this.OnItemUnselected);
      ItemContainer.PointerUp += new ItemEventHandler(this.OnItemPointerUp);
      ItemContainer.PointerDown += new ItemEventHandler(this.OnItemPointerDown);
      ItemContainer.PointerEnter += new ItemEventHandler(this.OnItemPointerEnter);
      ItemContainer.PointerExit += new ItemEventHandler(this.OnItemPointerExit);
      ItemContainer.DoubleClick += new ItemEventHandler(this.OnItemDoubleClick);
      ItemContainer.BeginEdit += new EventHandler(this.OnItemBeginEdit);
      ItemContainer.EndEdit += new EventHandler(this.OnItemEndEdit);
      ItemContainer.BeginDrag += new ItemEventHandler(this.OnItemBeginDrag);
      ItemContainer.Drag += new ItemEventHandler(this.OnItemDrag);
      ItemContainer.Drop += new ItemEventHandler(this.OnItemDrop);
      ItemContainer.EndDrag += new ItemEventHandler(this.OnItemEndDrag);
      this.OnEnableOverride();
    }

    private void OnDisable()
    {
      ItemContainer.Selected -= new EventHandler(this.OnItemSelected);
      ItemContainer.Unselected -= new EventHandler(this.OnItemUnselected);
      ItemContainer.PointerUp -= new ItemEventHandler(this.OnItemPointerUp);
      ItemContainer.PointerDown -= new ItemEventHandler(this.OnItemPointerDown);
      ItemContainer.PointerEnter -= new ItemEventHandler(this.OnItemPointerEnter);
      ItemContainer.PointerExit -= new ItemEventHandler(this.OnItemPointerExit);
      ItemContainer.DoubleClick -= new ItemEventHandler(this.OnItemDoubleClick);
      ItemContainer.BeginEdit -= new EventHandler(this.OnItemBeginEdit);
      ItemContainer.EndEdit -= new EventHandler(this.OnItemEndEdit);
      ItemContainer.BeginDrag -= new ItemEventHandler(this.OnItemBeginDrag);
      ItemContainer.Drag -= new ItemEventHandler(this.OnItemDrag);
      ItemContainer.Drop -= new ItemEventHandler(this.OnItemDrop);
      ItemContainer.EndDrag -= new ItemEventHandler(this.OnItemEndDrag);
      this.OnDisableOverride();
    }

    private void OnDestroy()
    {
      if (Object.op_Inequality((Object) this.m_rtcListener, (Object) null))
        this.m_rtcListener.RectTransformChanged -= new RectTransformChanged(this.OnViewportRectTransformChanged);
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

    private void OnViewportRectTransformChanged()
    {
      if (!this.ExpandChildrenHeight && !this.ExpandChildrenWidth)
        return;
      Rect rect = this.m_scrollRect.get_viewport().get_rect();
      if ((double) ((Rect) ref rect).get_width() == (double) this.m_width && (double) ((Rect) ref rect).get_height() == (double) this.m_height)
        return;
      this.m_width = ((Rect) ref rect).get_width();
      this.m_height = ((Rect) ref rect).get_height();
      if (this.m_itemContainers == null)
        return;
      for (int index = 0; index < this.m_itemContainers.Count; ++index)
      {
        ItemContainer itemContainer = this.m_itemContainers[index];
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
        {
          if (this.ExpandChildrenWidth)
            itemContainer.LayoutElement.set_minWidth(this.m_width);
          if (this.ExpandChildrenHeight)
            itemContainer.LayoutElement.set_minHeight(this.m_height);
        }
      }
    }

    private void OnCanDragChanged()
    {
      for (int index = 0; index < this.m_itemContainers.Count; ++index)
      {
        ItemContainer itemContainer = this.m_itemContainers[index];
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
          itemContainer.CanDrag = this.CanDrag;
      }
    }

    protected bool CanHandleEvent(object sender)
    {
      ItemContainer itemContainer = sender as ItemContainer;
      return Object.op_Implicit((Object) itemContainer) && ((Component) itemContainer).get_transform().IsChildOf(this.Panel);
    }

    private void OnItemSelected(object sender, EventArgs e)
    {
      if (this.m_selectionLocked || !this.CanHandleEvent(sender))
        return;
      ItemContainer.Unselected -= new EventHandler(this.OnItemUnselected);
      if (Input.GetKey(this.MultiselectKey))
      {
        IList list = this.m_selectedItems == null ? (IList) new List<object>() : (IList) this.m_selectedItems.ToList<object>();
        list.Add(((ItemContainer) sender).Item);
        this.SelectedItems = (IEnumerable) list;
      }
      else if (Input.GetKey(this.RangeselectKey))
        this.SelectRange((ItemContainer) sender);
      else
        this.SelectedIndex = this.IndexOf(((ItemContainer) sender).Item);
      ItemContainer.Unselected += new EventHandler(this.OnItemUnselected);
    }

    private void SelectRange(ItemContainer itemContainer)
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
          objectList.Add(this.m_items[index]);
        for (int index = val1 + 1; index <= num2; ++index)
          objectList.Add(this.m_items[index]);
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
      list.Remove(((ItemContainer) sender).Item);
      this.SelectedItems = (IEnumerable) list;
    }

    private void OnItemPointerDown(ItemContainer sender, PointerEventData e)
    {
      if (!this.CanHandleEvent((object) sender) || this.m_externalDragOperation)
        return;
      this.m_dropMarker.SetTraget((ItemContainer) null);
      this.m_dragItems = (ItemContainer[]) null;
      this.m_isDropInProgress = false;
      if (this.SelectOnPointerUp)
        return;
      if (Input.GetKey(this.RangeselectKey))
        this.SelectRange(sender);
      else if (Input.GetKey(this.MultiselectKey))
        sender.IsSelected = !sender.IsSelected;
      else
        sender.IsSelected = true;
    }

    private void OnItemPointerUp(ItemContainer sender, PointerEventData e)
    {
      if (!this.CanHandleEvent((object) sender) || this.m_externalDragOperation || this.m_dragItems != null)
        return;
      if (this.SelectOnPointerUp)
      {
        if (this.m_isDropInProgress)
          return;
        if (Input.GetKey(this.RangeselectKey))
          this.SelectRange(sender);
        else if (Input.GetKey(this.MultiselectKey))
          sender.IsSelected = !sender.IsSelected;
        else
          sender.IsSelected = true;
      }
      else
      {
        if (Input.GetKey(this.MultiselectKey) || Input.GetKey(this.RangeselectKey) || (this.m_selectedItems == null || this.m_selectedItems.Count <= 1))
          return;
        if (this.SelectedItem == sender.Item)
          this.SelectedItem = (object) null;
        this.SelectedItem = sender.Item;
      }
    }

    private void OnItemPointerEnter(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.m_dropTarget = sender;
      if (this.m_dragItems == null && !this.m_externalDragOperation || this.m_scrollDir != ItemsControl.ScrollDir.None)
        return;
      this.m_dropMarker.SetTraget(this.m_dropTarget);
    }

    private void OnItemPointerExit(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.m_dropTarget = (ItemContainer) null;
      if (this.m_dragItems == null && !this.m_externalDragOperation)
        return;
      this.m_dropMarker.SetTraget((ItemContainer) null);
    }

    private void OnItemDoubleClick(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender) || this.ItemDoubleClick == null)
        return;
      this.ItemDoubleClick((object) this, new ItemArgs(new object[1]
      {
        sender.Item
      }, eventData));
    }

    protected virtual void OnItemBeginEdit(object sender, EventArgs e)
    {
    }

    protected virtual void OnItemEndEdit(object sender, EventArgs e)
    {
    }

    private void OnItemBeginDrag(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      if (Object.op_Inequality((Object) this.m_dropTarget, (Object) null))
      {
        this.m_dropMarker.SetTraget(this.m_dropTarget);
        this.m_dropMarker.SetPosition(eventData.get_position());
      }
      if (this.m_selectedItems != null && this.m_selectedItems.Contains(sender.Item))
        this.m_dragItems = this.GetDragItems();
      else
        this.m_dragItems = new ItemContainer[1]{ sender };
      if (this.ItemBeginDrag == null)
        return;
      this.ItemBeginDrag((object) this, new ItemArgs(((IEnumerable<ItemContainer>) this.m_dragItems).Select<ItemContainer, object>((Func<ItemContainer, object>) (di => di.Item)).ToArray<object>(), eventData));
    }

    private void OnItemDrag(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.ExternalItemDrag(Vector2.op_Implicit(eventData.get_position()));
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
        if (vector2.y >= 0.0)
        {
          this.m_scrollDir = ItemsControl.ScrollDir.Up;
          this.m_dropMarker.SetTraget((ItemContainer) null);
        }
        else if (vector2.y < -(double) height)
        {
          this.m_scrollDir = ItemsControl.ScrollDir.Down;
          this.m_dropMarker.SetTraget((ItemContainer) null);
        }
        else if (vector2.x <= 0.0)
          this.m_scrollDir = ItemsControl.ScrollDir.Left;
        else if (vector2.x >= (double) width)
          this.m_scrollDir = ItemsControl.ScrollDir.Right;
        else
          this.m_scrollDir = ItemsControl.ScrollDir.None;
      }
      else
        this.m_scrollDir = ItemsControl.ScrollDir.None;
    }

    private void OnItemDrop(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.m_isDropInProgress = true;
      try
      {
        if (this.CanDrop(this.m_dragItems, this.m_dropTarget))
        {
          bool flag = false;
          if (this.ItemBeginDrop != null)
          {
            ItemDropCancelArgs e = new ItemDropCancelArgs(((IEnumerable<ItemContainer>) this.m_dragItems).Select<ItemContainer, object>((Func<ItemContainer, object>) (di => di.Item)).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false, eventData);
            if (this.ItemBeginDrop != null)
            {
              this.ItemBeginDrop((object) this, e);
              flag = e.Cancel;
            }
          }
          if (!flag)
          {
            this.Drop(this.m_dragItems, this.m_dropTarget, this.m_dropMarker.Action);
            if (this.ItemDrop != null)
            {
              if (this.m_dragItems == null)
                Debug.LogWarning((object) "m_dragItems");
              if (Object.op_Equality((Object) this.m_dropTarget, (Object) null))
                Debug.LogWarning((object) "m_dropTarget");
              if (Object.op_Equality((Object) this.m_dropMarker, (Object) null))
                Debug.LogWarning((object) "m_dropMarker");
              if (this.m_dragItems != null && Object.op_Inequality((Object) this.m_dropTarget, (Object) null) && Object.op_Inequality((Object) this.m_dropMarker, (Object) null))
                this.ItemDrop((object) this, new ItemDropArgs(((IEnumerable<ItemContainer>) this.m_dragItems).Select<ItemContainer, object>((Func<ItemContainer, object>) (di => di.Item)).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false, eventData));
            }
          }
        }
        this.RaiseEndDrag(eventData);
      }
      finally
      {
        this.m_isDropInProgress = false;
      }
    }

    private void OnItemEndDrag(ItemContainer sender, PointerEventData eventData)
    {
      if (!this.CanHandleEvent((object) sender))
        return;
      this.RaiseEndDrag(eventData);
    }

    private void RaiseEndDrag(PointerEventData pointerEventData)
    {
      if (this.m_dragItems == null)
        return;
      if (this.ItemEndDrag != null)
        this.ItemEndDrag((object) this, new ItemArgs(((IEnumerable<ItemContainer>) this.m_dragItems).Select<ItemContainer, object>((Func<ItemContainer, object>) (di => di.Item)).ToArray<object>(), pointerEventData));
      this.m_dropMarker.SetTraget((ItemContainer) null);
      this.m_dragItems = (ItemContainer[]) null;
      this.m_scrollDir = ItemsControl.ScrollDir.None;
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
        if (this.m_itemContainers != null && this.m_itemContainers.Count > 0)
        {
          this.m_dropTarget = this.m_itemContainers.Last<ItemContainer>();
          this.m_dropMarker.Action = ItemDropAction.SetNextSibling;
        }
        this.m_isDropInProgress = true;
        try
        {
          if (this.CanDrop(this.m_dragItems, this.m_dropTarget))
          {
            this.Drop(this.m_dragItems, this.m_dropTarget, this.m_dropMarker.Action);
            if (this.ItemDrop != null)
              this.ItemDrop((object) this, new ItemDropArgs(((IEnumerable<ItemContainer>) this.m_dragItems).Select<ItemContainer, object>((Func<ItemContainer, object>) (di => di.Item)).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false, eventData));
          }
          this.m_dropMarker.SetTraget((ItemContainer) null);
          this.m_dragItems = (ItemContainer[]) null;
        }
        finally
        {
          this.m_isDropInProgress = false;
        }
      }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      if (!this.CanUnselectAll)
        return;
      this.SelectedIndex = -1;
    }

    protected virtual bool CanDrop(ItemContainer[] dragItems, ItemContainer dropTarget)
    {
      return Object.op_Equality((Object) dropTarget, (Object) null) || dragItems != null && !((IEnumerable<object>) dragItems).Contains<object>(dropTarget.Item);
    }

    protected ItemContainer[] GetDragItems()
    {
      ItemContainer[] itemContainerArray = new ItemContainer[this.m_selectedItems.Count];
      if (this.m_selectedItems != null)
      {
        for (int index = 0; index < this.m_selectedItems.Count; ++index)
          itemContainerArray[index] = this.GetItemContainer(this.m_selectedItems[index]);
      }
      return ((IEnumerable<ItemContainer>) itemContainerArray).OrderBy<ItemContainer, int>((Func<ItemContainer, int>) (di => ((Component) di).get_transform().GetSiblingIndex())).ToArray<ItemContainer>();
    }

    protected virtual void SetNextSibling(ItemContainer sibling, ItemContainer nextSibling)
    {
      int siblingIndex1 = ((Component) sibling).get_transform().GetSiblingIndex();
      int siblingIndex2 = ((Component) nextSibling).get_transform().GetSiblingIndex();
      this.RemoveItemContainerAt(siblingIndex2);
      this.RemoveItemAt(siblingIndex2);
      if (siblingIndex2 > siblingIndex1)
        ++siblingIndex1;
      ((Component) nextSibling).get_transform().SetSiblingIndex(siblingIndex1);
      this.InsertItemContainerAt(siblingIndex1, nextSibling);
      this.InsertItem(siblingIndex1, nextSibling.Item);
    }

    protected virtual void SetPrevSibling(ItemContainer sibling, ItemContainer prevSibling)
    {
      int siblingIndex1 = ((Component) sibling).get_transform().GetSiblingIndex();
      int siblingIndex2 = ((Component) prevSibling).get_transform().GetSiblingIndex();
      this.RemoveItemContainerAt(siblingIndex2);
      this.RemoveItemAt(siblingIndex2);
      if (siblingIndex2 < siblingIndex1)
        --siblingIndex1;
      ((Component) prevSibling).get_transform().SetSiblingIndex(siblingIndex1);
      this.InsertItemContainerAt(siblingIndex1, prevSibling);
      this.InsertItem(siblingIndex1, prevSibling.Item);
    }

    protected virtual void Drop(
      ItemContainer[] dragItems,
      ItemContainer dropTarget,
      ItemDropAction action)
    {
      switch (action)
      {
        case ItemDropAction.SetPrevSibling:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            ItemContainer dragItem = dragItems[index];
            this.SetPrevSibling(dropTarget, dragItem);
          }
          break;
        case ItemDropAction.SetNextSibling:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            ItemContainer dragItem = dragItems[index];
            this.SetNextSibling(dropTarget, dragItem);
          }
          break;
      }
      this.UpdateSelectedItemIndex();
    }

    protected void UpdateSelectedItemIndex()
    {
      this.m_selectedIndex = this.IndexOf(this.SelectedItem);
    }

    protected virtual void DataBind()
    {
      this.m_itemContainers = ((IEnumerable<ItemContainer>) ((Component) this).GetComponentsInChildren<ItemContainer>()).ToList<ItemContainer>();
      if (this.m_items == null)
      {
        for (int index = 0; index < this.m_itemContainers.Count; ++index)
          Object.DestroyImmediate((Object) ((Component) this.m_itemContainers[index]).get_gameObject());
      }
      else
      {
        int num1 = this.m_items.Count - this.m_itemContainers.Count;
        if (num1 > 0)
        {
          for (int index = 0; index < num1; ++index)
            this.InstantiateItemContainer(this.m_itemContainers.Count);
        }
        else
        {
          int num2 = this.m_itemContainers.Count + num1;
          for (int siblingIndex = this.m_itemContainers.Count - 1; siblingIndex >= num2; --siblingIndex)
            this.DestroyItemContainer(siblingIndex);
        }
      }
      for (int index = 0; index < this.m_itemContainers.Count; ++index)
      {
        ItemContainer itemContainer = this.m_itemContainers[index];
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
          itemContainer.Clear();
      }
      if (this.m_items == null)
        return;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        object obj = this.m_items[index];
        ItemContainer itemContainer = this.m_itemContainers[index];
        itemContainer.CanDrag = this.CanDrag;
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
        {
          itemContainer.Item = obj;
          this.DataBindItem(obj, itemContainer);
        }
      }
    }

    public virtual void DataBindItem(object item, ItemContainer itemContainer)
    {
    }

    protected ItemContainer InstantiateItemContainer(int siblingIndex)
    {
      GameObject container = (GameObject) Object.Instantiate<GameObject>((M0) this.ItemContainerPrefab);
      ((Object) container).set_name("ItemContainer");
      container.get_transform().SetParent(this.Panel, false);
      container.get_transform().SetSiblingIndex(siblingIndex);
      ItemContainer itemContainer = this.InstantiateItemContainerOverride(container);
      itemContainer.CanDrag = this.CanDrag;
      if (this.ExpandChildrenWidth)
        itemContainer.LayoutElement.set_minWidth(this.m_width);
      if (this.ExpandChildrenHeight)
        itemContainer.LayoutElement.set_minHeight(this.m_height);
      this.m_itemContainers.Insert(siblingIndex, itemContainer);
      return itemContainer;
    }

    protected void DestroyItemContainer(int siblingIndex)
    {
      if (this.m_itemContainers == null || siblingIndex < 0 || siblingIndex >= this.m_itemContainers.Count)
        return;
      Object.DestroyImmediate((Object) ((Component) this.m_itemContainers[siblingIndex]).get_gameObject());
      this.m_itemContainers.RemoveAt(siblingIndex);
    }

    protected virtual ItemContainer InstantiateItemContainerOverride(
      GameObject container)
    {
      ItemContainer itemContainer = (ItemContainer) container.GetComponent<ItemContainer>();
      if (Object.op_Equality((Object) itemContainer, (Object) null))
        itemContainer = (ItemContainer) container.AddComponent<ItemContainer>();
      return itemContainer;
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
      this.SelectedItems = (IEnumerable) null;
      for (int index = 0; index < items.Length; ++index)
        this.DestroyItem(items[index]);
      if (this.ItemsRemoved == null)
        return;
      this.ItemsRemoved((object) this, new ItemsRemovedArgs(items));
    }

    protected virtual void DestroyItem(object item)
    {
      if (this.m_selectedItems != null && this.m_selectedItems.Contains(item))
      {
        this.m_selectedItems.Remove(item);
        this.m_selectedItemsHS.Remove(item);
        if (this.m_selectedItems.Count == 0)
        {
          this.m_selectedItemContainer = (ItemContainer) null;
          this.m_selectedIndex = -1;
        }
        else
        {
          this.m_selectedItemContainer = this.GetItemContainer(this.m_selectedItems[0]);
          this.m_selectedIndex = this.IndexOf(this.m_selectedItemContainer.Item);
        }
      }
      ItemContainer itemContainer = this.GetItemContainer(item);
      if (!Object.op_Inequality((Object) itemContainer, (Object) null))
        return;
      this.DestroyItemContainer(((Component) itemContainer).get_transform().GetSiblingIndex());
      this.m_items.Remove(item);
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
