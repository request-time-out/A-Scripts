// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingScrollRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class VirtualizingScrollRect : ScrollRect
  {
    public RectTransform ContainerPrefab;
    [SerializeField]
    private RectTransform m_virtualContent;
    private HorizontalOrVerticalLayoutGroup m_layoutGroup;
    private RectTransformChangeListener m_virtualContentTransformChangeListener;
    [SerializeField]
    private VirtualizingMode m_mode;
    [SerializeField]
    private bool m_useGrid;
    [SerializeField]
    private Vector2 m_gridSpacing;
    private GridLayoutGroup m_gridLayoutGroup;
    private LinkedList<RectTransform> m_containers;
    private IList m_items;
    private float m_normalizedIndex;

    public VirtualizingScrollRect()
    {
      base.\u002Ector();
    }

    public event DataBindAction ItemDataBinding;

    public bool UseGrid
    {
      get
      {
        return this.m_useGrid;
      }
    }

    public int ContainersPerGroup
    {
      get
      {
        return this.m_useGrid ? this.m_gridLayoutGroup.get_constraintCount() : 1;
      }
      set
      {
        if (!this.m_useGrid)
          return;
        this.m_gridLayoutGroup.set_constraintCount(value);
        this.set_scrollSensitivity(this.ContainerSize);
        this.UpdateContentSize();
      }
    }

    public IList Items
    {
      get
      {
        return this.m_items;
      }
      set
      {
        if (this.m_items == value)
          return;
        this.m_items = value;
        this.DataBind(this.RoundedIndex, true);
        this.UpdateContentSize();
      }
    }

    public int ItemsCount
    {
      get
      {
        return this.Items == null ? 0 : this.Items.Count;
      }
    }

    private int RoundedItemsCount
    {
      get
      {
        return Mathf.CeilToInt((float) this.ItemsCount / (float) this.ContainersPerGroup) * this.ContainersPerGroup;
      }
    }

    private float NormalizedIndex
    {
      get
      {
        return this.m_normalizedIndex;
      }
      set
      {
        if ((double) value == (double) this.m_normalizedIndex)
          return;
        this.OnNormalizedIndexChanged(value);
      }
    }

    public int Index
    {
      get
      {
        return this.RoundedIndex + this.LocalGroupIndex;
      }
      set
      {
        this.RoundedIndex = value;
      }
    }

    private int LocalGroupIndex
    {
      get
      {
        return this.RoundedItemsCount == 0 ? 0 : Mathf.RoundToInt(this.NormalizedIndex * (float) Mathf.Max(this.RoundedItemsCount - this.VisibleItemsCount, 0)) % this.ContainersPerGroup;
      }
    }

    public int RoundedIndex
    {
      get
      {
        return this.RoundedItemsCount == 0 ? 0 : Mathf.RoundToInt((this.NormalizedIndex + 0.5f / (float) this.RoundedItemsCount) * (float) Mathf.Max(this.RoundedItemsCount - this.VisibleItemsCount, 0)) / this.ContainersPerGroup * this.ContainersPerGroup;
      }
      set
      {
        if (value < 0 || value >= this.RoundedItemsCount)
          return;
        this.NormalizedIndex = this.EvalNormalizedIndex(value);
        if (this.m_mode == VirtualizingMode.Vertical)
          this.set_verticalNormalizedPosition(1f - this.NormalizedIndex);
        else
          this.set_horizontalNormalizedPosition(this.NormalizedIndex);
      }
    }

    private float EvalNormalizedIndex(int index)
    {
      int num = this.RoundedItemsCount - this.VisibleItemsCount;
      return num <= 0 ? 0.0f : (float) index / (float) num;
    }

    public int VisibleItemsCount
    {
      get
      {
        return Mathf.Min(this.ItemsCount, this.PossibleItemsCount);
      }
    }

    private int PossibleItemsCount
    {
      get
      {
        return (double) this.ContainerSize < 9.99999974737875E-06 ? 0 : Mathf.RoundToInt(this.Size / this.ContainerSize) * this.ContainersPerGroup;
      }
    }

    private float ContainerSize
    {
      get
      {
        if (this.m_mode == VirtualizingMode.Horizontal)
        {
          Rect rect = this.ContainerPrefab.get_rect();
          return Mathf.Max(0.0f, (float) ((double) ((Rect) ref rect).get_width() + (!this.m_useGrid ? (double) this.m_layoutGroup.get_spacing() : (double) this.m_gridSpacing.x)));
        }
        if (this.m_mode != VirtualizingMode.Vertical)
          throw new InvalidOperationException("Unable to eval container size in non-virtualizing mode");
        Rect rect1 = this.ContainerPrefab.get_rect();
        return Mathf.Max(0.0f, (float) ((double) ((Rect) ref rect1).get_height() + (!this.m_useGrid ? (double) this.m_layoutGroup.get_spacing() : (double) this.m_gridSpacing.y)));
      }
    }

    private float Size
    {
      get
      {
        if (this.m_mode == VirtualizingMode.Horizontal)
        {
          Rect rect = this.m_virtualContent.get_rect();
          return Mathf.Max(0.0f, ((Rect) ref rect).get_width());
        }
        Rect rect1 = this.m_virtualContent.get_rect();
        return Mathf.Max(0.0f, ((Rect) ref rect1).get_height());
      }
    }

    protected virtual void Awake()
    {
      ((UIBehaviour) this).Awake();
      if (Object.op_Equality((Object) this.m_virtualContent, (Object) null))
        return;
      this.m_virtualContentTransformChangeListener = (RectTransformChangeListener) ((Component) this.m_virtualContent).GetComponent<RectTransformChangeListener>();
      this.m_virtualContentTransformChangeListener.RectTransformChanged += new RectTransformChanged(this.OnVirtualContentTransformChaged);
      this.UpdateVirtualContentPosition();
      if (this.m_useGrid)
      {
        LayoutGroup component = (LayoutGroup) ((Component) this.m_virtualContent).GetComponent<LayoutGroup>();
        if (Object.op_Inequality((Object) component, (Object) null) && !(component is GridLayoutGroup))
          Object.DestroyImmediate((Object) component);
        GridLayoutGroup gridLayoutGroup1 = (GridLayoutGroup) ((Component) this.m_virtualContent).GetComponent<GridLayoutGroup>();
        if (Object.op_Equality((Object) gridLayoutGroup1, (Object) null))
          gridLayoutGroup1 = (GridLayoutGroup) ((Component) this.m_virtualContent).get_gameObject().AddComponent<GridLayoutGroup>();
        GridLayoutGroup gridLayoutGroup2 = gridLayoutGroup1;
        Rect rect = this.ContainerPrefab.get_rect();
        Vector2 size = ((Rect) ref rect).get_size();
        gridLayoutGroup2.set_cellSize(size);
        ((LayoutGroup) gridLayoutGroup1).set_childAlignment((TextAnchor) 0);
        gridLayoutGroup1.set_startCorner((GridLayoutGroup.Corner) 0);
        gridLayoutGroup1.set_spacing(this.m_gridSpacing);
        if (this.m_mode == VirtualizingMode.Vertical)
        {
          gridLayoutGroup1.set_startAxis((GridLayoutGroup.Axis) 0);
          gridLayoutGroup1.set_constraint((GridLayoutGroup.Constraint) 1);
        }
        else
        {
          gridLayoutGroup1.set_startAxis((GridLayoutGroup.Axis) 1);
          gridLayoutGroup1.set_constraint((GridLayoutGroup.Constraint) 2);
        }
        this.m_gridLayoutGroup = gridLayoutGroup1;
      }
      else if (this.m_mode == VirtualizingMode.Horizontal)
      {
        LayoutGroup component = (LayoutGroup) ((Component) this.m_virtualContent).GetComponent<LayoutGroup>();
        if (Object.op_Inequality((Object) component, (Object) null) && !(component is HorizontalLayoutGroup))
          Object.DestroyImmediate((Object) component);
        HorizontalLayoutGroup horizontalLayoutGroup = (HorizontalLayoutGroup) ((Component) this.m_virtualContent).GetComponent<HorizontalLayoutGroup>();
        if (Object.op_Equality((Object) horizontalLayoutGroup, (Object) null))
          horizontalLayoutGroup = (HorizontalLayoutGroup) ((Component) this.m_virtualContent).get_gameObject().AddComponent<HorizontalLayoutGroup>();
        ((HorizontalOrVerticalLayoutGroup) horizontalLayoutGroup).set_childControlHeight(true);
        ((HorizontalOrVerticalLayoutGroup) horizontalLayoutGroup).set_childControlWidth(false);
        ((HorizontalOrVerticalLayoutGroup) horizontalLayoutGroup).set_childForceExpandWidth(false);
        this.m_layoutGroup = (HorizontalOrVerticalLayoutGroup) horizontalLayoutGroup;
      }
      else
      {
        LayoutGroup component = (LayoutGroup) ((Component) this.m_virtualContent).GetComponent<LayoutGroup>();
        if (Object.op_Inequality((Object) component, (Object) null) && !(component is VerticalLayoutGroup))
          Object.DestroyImmediate((Object) component);
        VerticalLayoutGroup verticalLayoutGroup = (VerticalLayoutGroup) ((Component) this.m_virtualContent).GetComponent<VerticalLayoutGroup>();
        if (Object.op_Equality((Object) verticalLayoutGroup, (Object) null))
          verticalLayoutGroup = (VerticalLayoutGroup) ((Component) this.m_virtualContent).get_gameObject().AddComponent<VerticalLayoutGroup>();
        ((HorizontalOrVerticalLayoutGroup) verticalLayoutGroup).set_childControlWidth(true);
        ((HorizontalOrVerticalLayoutGroup) verticalLayoutGroup).set_childControlHeight(false);
        ((HorizontalOrVerticalLayoutGroup) verticalLayoutGroup).set_childForceExpandHeight(false);
        this.m_layoutGroup = (HorizontalOrVerticalLayoutGroup) verticalLayoutGroup;
      }
      this.set_scrollSensitivity(this.ContainerSize);
    }

    protected virtual void Start()
    {
      ((UIBehaviour) this).Start();
    }

    protected virtual void OnDestroy()
    {
      ((UIBehaviour) this).OnDestroy();
      if (!Object.op_Inequality((Object) this.m_virtualContentTransformChangeListener, (Object) null))
        return;
      this.m_virtualContentTransformChangeListener.RectTransformChanged -= new RectTransformChanged(this.OnVirtualContentTransformChaged);
    }

    private void OnVirtualContentTransformChaged()
    {
      if (this.m_containers.Count == 0)
      {
        this.DataBind(this.RoundedIndex, false);
        this.UpdateContentSize();
      }
      if (this.m_mode == VirtualizingMode.Horizontal)
      {
        RectTransform content = this.get_content();
        Rect rect1 = this.m_virtualContent.get_rect();
        double height = (double) ((Rect) ref rect1).get_height();
        content.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) height);
        if (!this.m_useGrid)
          return;
        if (this.m_gridLayoutGroup.get_cellSize().y + this.m_gridSpacing.y < 9.99999974737875E-06)
          Debug.LogError((object) "cellSize is too small");
        RectTransform parent = (RectTransform) ((Transform) this.m_virtualContent).get_parent();
        if (this.get_verticalScrollbarVisibility() == null)
        {
          Rect rect2 = parent.get_rect();
          this.ContainersPerGroup = Mathf.FloorToInt(((Rect) ref rect2).get_height() / Mathf.Max(1E-05f, (float) (this.m_gridLayoutGroup.get_cellSize().y + this.m_gridSpacing.y)));
        }
        else
        {
          Rect rect2 = parent.get_rect();
          this.ContainersPerGroup = Mathf.RoundToInt(((Rect) ref rect2).get_height() / Mathf.Max(1E-05f, (float) (this.m_gridLayoutGroup.get_cellSize().y + this.m_gridSpacing.y)));
        }
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Vertical)
          return;
        RectTransform content = this.get_content();
        Rect rect1 = this.m_virtualContent.get_rect();
        double width = (double) ((Rect) ref rect1).get_width();
        content.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) width);
        if (!this.m_useGrid)
          return;
        if (this.m_gridLayoutGroup.get_cellSize().x + this.m_gridSpacing.x < 9.99999974737875E-06)
          Debug.LogError((object) "cellSize is too small");
        RectTransform parent = (RectTransform) ((Transform) this.m_virtualContent).get_parent();
        if (this.get_horizontalScrollbarVisibility() == null)
        {
          Rect rect2 = parent.get_rect();
          this.ContainersPerGroup = Mathf.RoundToInt(((Rect) ref rect2).get_width() / Mathf.Max(1E-05f, (float) (this.m_gridLayoutGroup.get_cellSize().x + this.m_gridSpacing.x)));
        }
        else
        {
          Rect rect2 = parent.get_rect();
          this.ContainersPerGroup = Mathf.RoundToInt(((Rect) ref rect2).get_width() / Mathf.Max(1E-05f, (float) (this.m_gridLayoutGroup.get_cellSize().x + this.m_gridSpacing.x)));
        }
      }
    }

    protected virtual void SetNormalizedPosition(float value, int axis)
    {
      base.SetNormalizedPosition(value, axis);
      this.UpdateVirtualContentPosition();
      if (this.m_mode == VirtualizingMode.Vertical && axis == 1)
      {
        this.NormalizedIndex = 1f - value;
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Horizontal || axis != 0)
          return;
        this.NormalizedIndex = value;
      }
    }

    protected virtual void SetContentAnchoredPosition(Vector2 position)
    {
      base.SetContentAnchoredPosition(position);
      this.UpdateVirtualContentPosition();
      if (this.m_mode == VirtualizingMode.Vertical)
      {
        this.NormalizedIndex = 1f - this.get_verticalNormalizedPosition();
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Horizontal)
          return;
        this.NormalizedIndex = this.get_horizontalNormalizedPosition();
      }
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (!((Behaviour) this).get_isActiveAndEnabled())
        return;
      ((MonoBehaviour) this).StartCoroutine(this.CoRectTransformDimensionsChange());
    }

    [DebuggerHidden]
    private IEnumerator CoRectTransformDimensionsChange()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new VirtualizingScrollRect.\u003CCoRectTransformDimensionsChange\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void UpdateVirtualContentPosition()
    {
      if (!Object.op_Inequality((Object) this.m_virtualContent, (Object) null))
        return;
      if (this.m_mode == VirtualizingMode.Horizontal)
      {
        this.m_virtualContent.set_anchoredPosition(new Vector2(0.0f, (float) this.get_content().get_anchoredPosition().y));
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Vertical)
          return;
        this.m_virtualContent.set_anchoredPosition(new Vector2((float) this.get_content().get_anchoredPosition().x, 0.0f));
      }
    }

    private void UpdateContentSize()
    {
      if (this.m_mode == VirtualizingMode.Horizontal)
      {
        this.get_content().set_sizeDelta(new Vector2((float) Mathf.CeilToInt((float) this.RoundedItemsCount / (float) this.ContainersPerGroup) * this.ContainerSize, (float) this.get_content().get_sizeDelta().y));
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Vertical)
          return;
        this.get_content().set_sizeDelta(new Vector2((float) this.get_content().get_sizeDelta().x, (float) Mathf.CeilToInt((float) this.RoundedItemsCount / (float) this.ContainersPerGroup) * this.ContainerSize));
      }
    }

    private void OnNormalizedIndexChanged(float newValue)
    {
      newValue = Mathf.Clamp01(newValue);
      int roundedIndex1 = this.RoundedIndex;
      float normalizedIndex = this.m_normalizedIndex;
      this.m_normalizedIndex = newValue;
      int roundedIndex2 = this.RoundedIndex;
      if (roundedIndex2 < 0 || roundedIndex2 >= this.RoundedItemsCount)
      {
        this.m_normalizedIndex = normalizedIndex;
      }
      else
      {
        if (roundedIndex1 == roundedIndex2)
          return;
        int num1 = roundedIndex2 - roundedIndex1;
        bool flag = num1 > 0;
        int num2 = Mathf.Abs(num1);
        if (num2 > this.VisibleItemsCount)
          this.DataBind(roundedIndex2, false);
        else if (flag)
        {
          for (int index = 0; index < num2; ++index)
          {
            LinkedListNode<RectTransform> first = this.m_containers.First;
            RectTransform container;
            if (this.m_containers.Count > 1)
            {
              this.m_containers.RemoveFirst();
              int siblingIndex = ((Component) this.m_containers.Last.Value).get_transform().GetSiblingIndex();
              this.m_containers.AddLast(first);
              container = first.Value;
              ((Transform) container).SetSiblingIndex(siblingIndex + 1);
            }
            else
              container = first.Value;
            if (this.ItemDataBinding != null && this.Items != null)
            {
              if (roundedIndex1 + this.VisibleItemsCount < this.ItemsCount)
              {
                object obj = this.Items[roundedIndex1 + this.VisibleItemsCount];
                this.ItemDataBinding(container, obj);
              }
              else
                this.ItemDataBinding(container, (object) null);
            }
            ++roundedIndex1;
          }
        }
        else
        {
          for (int index = 0; index < num2; ++index)
          {
            LinkedListNode<RectTransform> last = this.m_containers.Last;
            RectTransform container;
            if (this.m_containers.Count > 1)
            {
              this.m_containers.RemoveLast();
              int siblingIndex = ((Component) this.m_containers.First.Value).get_transform().GetSiblingIndex();
              this.m_containers.AddFirst(last);
              container = last.Value;
              ((Transform) container).SetSiblingIndex(siblingIndex);
            }
            else
              container = last.Value;
            --roundedIndex1;
            if (this.ItemDataBinding != null && this.Items != null)
            {
              if (roundedIndex1 < this.ItemsCount)
              {
                object obj = this.Items[roundedIndex1];
                this.ItemDataBinding(container, obj);
              }
              else
                this.ItemDataBinding(container, (object) null);
            }
          }
        }
      }
    }

    private void DataBind(int firstItemIndex, bool sibling = false)
    {
      int num1 = this.VisibleItemsCount - this.m_containers.Count;
      if (num1 < 0)
      {
        for (int index = 0; index < -num1; ++index)
        {
          Object.Destroy((Object) ((Component) this.m_containers.Last.Value).get_gameObject());
          this.m_containers.RemoveLast();
        }
      }
      else
      {
        for (int index = 0; index < num1; ++index)
          this.m_containers.AddLast((RectTransform) Object.Instantiate<RectTransform>((M0) this.ContainerPrefab, (Transform) this.m_virtualContent));
      }
      if (this.ItemDataBinding == null || this.Items == null)
        return;
      int num2 = 0;
      using (LinkedList<RectTransform>.Enumerator enumerator = this.m_containers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          RectTransform current = enumerator.Current;
          if (firstItemIndex + num2 < this.Items.Count)
            this.ItemDataBinding(current, this.Items[firstItemIndex + num2]);
          else
            this.ItemDataBinding(current, (object) null);
          if (sibling && ((Transform) current).GetSiblingIndex() != num2)
            ((Transform) current).SetSiblingIndex(num2);
          ++num2;
        }
      }
    }

    public bool IsParentOf(Transform child)
    {
      return !Object.op_Equality((Object) this.m_virtualContent, (Object) null) && child.IsChildOf((Transform) this.m_virtualContent);
    }

    public void InsertItem(int index, object item, bool raiseItemDataBindingEvent = true)
    {
      int roundedIndex = this.RoundedIndex;
      int num = roundedIndex + this.VisibleItemsCount - 1;
      this.m_items.Insert(index, item);
      this.UpdateContentSize();
      this.UpdateScrollbar(roundedIndex);
      if (this.PossibleItemsCount >= this.m_items.Count && this.m_containers.Count < this.VisibleItemsCount)
      {
        this.m_containers.AddLast((RectTransform) Object.Instantiate<RectTransform>((M0) this.ContainerPrefab, (Transform) this.m_virtualContent));
        ++num;
      }
      if (roundedIndex <= index && index <= num)
      {
        RectTransform container = this.m_containers.Last.Value;
        this.m_containers.RemoveLast();
        if (index == roundedIndex)
        {
          this.m_containers.AddFirst(container);
          ((Transform) container).SetSiblingIndex(0);
        }
        else
        {
          this.m_containers.AddAfter(this.m_containers.Find(((IEnumerable<RectTransform>) this.m_containers).ElementAtOrDefault<RectTransform>(index - roundedIndex - 1)), container);
          ((Transform) container).SetSiblingIndex(index - roundedIndex);
        }
        if (!raiseItemDataBindingEvent || this.ItemDataBinding == null)
          return;
        this.ItemDataBinding(container, item);
      }
      else
      {
        if (index >= roundedIndex)
          return;
        this.UpdateScrollbar(roundedIndex + 1);
      }
    }

    public void RemoveItems(int[] indices, bool raiseItemDataBindingEvent = true)
    {
      int num = this.RoundedIndex;
      indices = ((IEnumerable<int>) indices).OrderBy<int, int>((Func<int, int>) (i => i)).ToArray<int>();
      for (int index1 = indices.Length - 1; index1 >= 0; --index1)
      {
        int index2 = indices[index1];
        if (index2 >= 0 && index2 < this.m_items.Count)
        {
          this.m_items.RemoveAt(index2);
          if (index2 < num)
            --num;
        }
      }
      if (num + this.VisibleItemsCount >= this.RoundedItemsCount)
        num = Mathf.Max(0, this.RoundedItemsCount - this.VisibleItemsCount);
      this.UpdateContentSize();
      this.UpdateScrollbar(num);
      this.DataBind(num, false);
      this.OnVirtualContentTransformChaged();
    }

    public void SetNextSibling(object sibling, object nextSibling)
    {
      if (sibling == nextSibling)
        return;
      int num = this.m_items.IndexOf(sibling);
      int index1 = this.m_items.IndexOf(nextSibling);
      int roundedIndex = this.RoundedIndex;
      int index2 = roundedIndex + this.VisibleItemsCount - 1;
      bool flag1 = roundedIndex <= index1 && index1 <= index2;
      int index3 = num;
      if (index1 > num)
        ++index3;
      int index4 = index1 - roundedIndex;
      int index5 = index3 - roundedIndex;
      bool flag2 = roundedIndex <= index3 && (index4 < 0 ? index3 < index2 : index3 <= index2);
      this.m_items.RemoveAt(index1);
      this.m_items.Insert(index3, nextSibling);
      if (flag2)
      {
        if (flag1)
        {
          RectTransform container = ((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index4);
          this.m_containers.Remove(container);
          if (index5 == 0)
          {
            this.m_containers.AddFirst(container);
            ((Transform) container).SetSiblingIndex(0);
          }
          else
            this.m_containers.AddAfter(this.m_containers.Find(((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index5 - 1)), container);
          ((Transform) container).SetSiblingIndex(index5);
          if (this.ItemDataBinding == null)
            return;
          this.ItemDataBinding(container, nextSibling);
        }
        else
        {
          RectTransform container = this.m_containers.Last.Value;
          this.m_containers.RemoveLast();
          if (index5 == 0)
            this.m_containers.AddFirst(container);
          else
            this.m_containers.AddAfter(this.m_containers.Find(index4 >= 0 ? ((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index5 - 1) : ((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index5)), container);
          if (index4 < 0)
          {
            this.UpdateScrollbar(roundedIndex - 1);
            ((Transform) container).SetSiblingIndex(index5 + 1);
          }
          else
            ((Transform) container).SetSiblingIndex(index5);
          if (this.ItemDataBinding == null)
            return;
          this.ItemDataBinding(container, nextSibling);
        }
      }
      else if (flag1)
      {
        if (index3 < roundedIndex)
        {
          RectTransform container = ((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index4);
          this.m_containers.Remove(container);
          this.m_containers.AddFirst(container);
          ((Transform) container).SetSiblingIndex(0);
          if (this.ItemDataBinding == null)
            return;
          this.ItemDataBinding(container, this.m_items[roundedIndex]);
        }
        else
        {
          if (index3 <= index2)
            return;
          RectTransform container = ((IEnumerable<RectTransform>) this.m_containers).ElementAt<RectTransform>(index4);
          this.m_containers.Remove(container);
          this.m_containers.AddLast(container);
          ((Transform) container).SetSiblingIndex(this.m_containers.Count - 1);
          if (this.ItemDataBinding == null)
            return;
          this.ItemDataBinding(container, this.m_items[index2]);
        }
      }
      else
      {
        if (index4 >= 0)
          return;
        this.UpdateScrollbar(roundedIndex - 1);
      }
    }

    public void SetPrevSibling(object sibling, object prevSibling)
    {
      int index = this.m_items.IndexOf(sibling) - 1;
      if (index >= 0)
      {
        sibling = this.m_items[index];
        this.SetNextSibling(sibling, prevSibling);
      }
      else
      {
        RectTransform container = this.GetContainer(prevSibling);
        this.m_items.RemoveAt(this.m_items.IndexOf(prevSibling));
        this.m_items.Insert(0, prevSibling);
        if (Object.op_Equality((Object) container, (Object) null))
        {
          container = this.m_containers.Last.Value;
          this.m_containers.RemoveLast();
        }
        else
          this.m_containers.Remove(container);
        this.m_containers.AddFirst(container);
        ((Transform) container).SetSiblingIndex(0);
        if (this.ItemDataBinding == null)
          return;
        this.ItemDataBinding(container, prevSibling);
      }
    }

    public RectTransform GetContainer(object obj)
    {
      if (this.m_items == null)
        return (RectTransform) null;
      int num1 = this.m_items.IndexOf(obj);
      if (num1 < 0)
        return (RectTransform) null;
      int roundedIndex = this.RoundedIndex;
      int num2 = roundedIndex + this.VisibleItemsCount - 1;
      if (roundedIndex > num1 || num1 > num2)
        return (RectTransform) null;
      int num3 = num1 - roundedIndex;
      return num3 < 0 || num3 >= this.m_containers.Count ? (RectTransform) null : ((IEnumerable<RectTransform>) this.m_containers).ElementAtOrDefault<RectTransform>(num1 - roundedIndex);
    }

    public RectTransform FirstContainer()
    {
      return this.m_containers.Count == 0 ? (RectTransform) null : this.m_containers.First.Value;
    }

    public void ForEachContainer(Action<RectTransform> action)
    {
      if (action == null)
        return;
      using (LinkedList<RectTransform>.Enumerator enumerator = this.m_containers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          RectTransform current = enumerator.Current;
          action(current);
        }
      }
    }

    public RectTransform LastContainer()
    {
      return this.m_containers.Count == 0 ? (RectTransform) null : this.m_containers.Last.Value;
    }

    private void UpdateScrollbar(int index)
    {
      this.m_normalizedIndex = this.EvalNormalizedIndex(index);
      if (this.m_mode == VirtualizingMode.Vertical)
      {
        this.set_verticalNormalizedPosition(1f - this.m_normalizedIndex);
      }
      else
      {
        if (this.m_mode != VirtualizingMode.Horizontal)
          return;
        this.set_horizontalNormalizedPosition(this.m_normalizedIndex);
      }
    }

    public void Refresh()
    {
      this.DataBind(this.RoundedIndex, false);
    }
  }
}
