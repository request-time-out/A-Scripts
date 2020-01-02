// Decompiled with JetBrains decompiler
// Type: GUITree.TextSizeFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GUITree
{
  [AddComponentMenu("GUITree/Text Size Fitter", 1002)]
  [ExecuteInEditMode]
  [RequireComponent(typeof (RectTransform))]
  public class TextSizeFitter : UIBehaviour, ITreeLayoutElement, ILayoutGroup, ILayoutElement, ILayoutController
  {
    [SerializeField]
    protected RectOffset m_Padding;
    [FormerlySerializedAs("m_Alignment")]
    [SerializeField]
    protected TextAnchor m_ChildAlignment;
    [NonSerialized]
    private RectTransform m_Rect;
    protected DrivenRectTransformTracker m_Tracker;
    private Vector2 m_TotalMinSize;
    private Vector2 m_TotalPreferredSize;
    private Vector2 m_TotalFlexibleSize;
    private Text m_Text;
    private RectTransform m_RectText;
    private ILayoutElement m_ElementText;
    private ContentSizeFitter.FitMode m_FitModeHorizontal;
    private ContentSizeFitter.FitMode m_FitModeVertical;
    [SerializeField]
    protected bool m_ChildForceExpandWidth;
    [SerializeField]
    protected bool m_ChildForceExpandHeight;

    protected TextSizeFitter()
    {
      base.\u002Ector();
      if (this.m_Padding != null)
        return;
      this.m_Padding = new RectOffset();
    }

    public RectOffset padding
    {
      get
      {
        return this.m_Padding;
      }
      set
      {
        this.SetProperty<RectOffset>(ref this.m_Padding, value);
      }
    }

    public TextAnchor childAlignment
    {
      get
      {
        return this.m_ChildAlignment;
      }
      set
      {
        this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
      }
    }

    protected RectTransform rectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Rect, (Object) null))
          this.m_Rect = (RectTransform) ((Component) this).GetComponent<RectTransform>();
        return this.m_Rect;
      }
    }

    protected Text text
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Text, (Object) null))
          this.m_Text = (Text) ((Component) this).GetComponentInChildren<Text>();
        return this.m_Text;
      }
    }

    protected RectTransform rectText
    {
      get
      {
        if (Object.op_Equality((Object) this.m_RectText, (Object) null) && Object.op_Inequality((Object) this.text, (Object) null))
          this.m_RectText = ((Graphic) this.text).get_rectTransform();
        return this.m_RectText;
      }
    }

    protected ILayoutElement elementText
    {
      get
      {
        if (this.m_ElementText == null && Object.op_Inequality((Object) this.text, (Object) null))
          this.m_ElementText = (ILayoutElement) ((Component) this.text).GetComponent<ILayoutElement>();
        return this.m_ElementText;
      }
    }

    private bool isContentSizeFitter { get; set; }

    public void CalculateLayoutInputHorizontal()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
      ContentSizeFitter component = (ContentSizeFitter) ((Component) this.rectTransform).GetComponent<ContentSizeFitter>();
      this.isContentSizeFitter = Object.op_Inequality((Object) component, (Object) null);
      if (this.isContentSizeFitter)
      {
        this.m_FitModeHorizontal = component.get_horizontalFit();
        this.m_FitModeVertical = component.get_verticalFit();
      }
      this.CalcAlongAxis(0);
    }

    public void CalculateLayoutInputVertical()
    {
      this.CalcAlongAxis(1);
    }

    public virtual float minWidth
    {
      get
      {
        return this.GetTotalMinSize(0);
      }
    }

    public virtual float preferredWidth
    {
      get
      {
        return this.GetTotalPreferredSize(0);
      }
    }

    public virtual float flexibleWidth
    {
      get
      {
        return this.GetTotalFlexibleSize(0);
      }
    }

    public virtual float minHeight
    {
      get
      {
        return this.GetTotalMinSize(1);
      }
    }

    public virtual float preferredHeight
    {
      get
      {
        return this.GetTotalPreferredSize(1);
      }
    }

    public virtual float flexibleHeight
    {
      get
      {
        return this.GetTotalFlexibleSize(1);
      }
    }

    public virtual int layoutPriority
    {
      get
      {
        return int.MaxValue;
      }
    }

    public void SetLayoutHorizontal()
    {
      this.SetChildrenAlongAxis(0);
    }

    public void SetLayoutVertical()
    {
      this.SetChildrenAlongAxis(1);
    }

    protected virtual void OnEnable()
    {
      base.OnEnable();
      this.SetDirty();
    }

    protected virtual void OnDisable()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
      if (Object.op_Inequality((Object) this.rectTransform, (Object) null))
        LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
      base.OnDisable();
    }

    protected virtual void OnDidApplyAnimationProperties()
    {
      this.SetDirty();
    }

    public bool childForceExpandWidth
    {
      get
      {
        return this.m_ChildForceExpandWidth;
      }
      set
      {
        this.SetProperty<bool>(ref this.m_ChildForceExpandWidth, value);
      }
    }

    public bool childForceExpandHeight
    {
      get
      {
        return this.m_ChildForceExpandHeight;
      }
      set
      {
        this.SetProperty<bool>(ref this.m_ChildForceExpandHeight, value);
      }
    }

    protected void CalcAlongAxis(int axis)
    {
      float num1 = axis != 0 ? (float) this.padding.get_vertical() : (float) this.padding.get_horizontal();
      float num2 = num1;
      float num3 = num1;
      float num4 = 0.0f;
      bool flag = axis == 1;
      float minSize = LayoutUtility.GetMinSize(this.elementText, axis);
      float preferredSize = LayoutUtility.GetPreferredSize(this.elementText, axis);
      float num5 = LayoutUtility.GetFlexibleSize(this.elementText, axis);
      if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
        num5 = Mathf.Max(num5, 1f);
      float totalMin;
      float num6;
      float totalFlexible;
      if (flag)
      {
        totalMin = Mathf.Max(minSize + num1, num2);
        num6 = Mathf.Max(preferredSize + num1, num3);
        totalFlexible = Mathf.Max(num5, num4);
      }
      else
      {
        totalMin = num2 + minSize;
        num6 = num3 + preferredSize;
        totalFlexible = num4 + num5;
      }
      float totalPreferred = Mathf.Max(totalMin, num6);
      this.SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
    }

    protected void SetChildrenAlongAxis(int axis)
    {
      Rect rect = this.rectTransform.get_rect();
      Vector2 size1 = ((Rect) ref rect).get_size();
      float num1 = ((Vector2) ref size1).get_Item(axis);
      int num2;
      switch (axis)
      {
        case 0:
          num2 = this.padding.get_left();
          break;
        case 1:
          float num3 = num1 - (axis != 0 ? (float) this.padding.get_vertical() : (float) this.padding.get_horizontal());
          float minSize1 = LayoutUtility.GetMinSize(this.elementText, axis);
          float preferredSize1 = LayoutUtility.GetPreferredSize(this.elementText, axis);
          float num4 = LayoutUtility.GetFlexibleSize(this.elementText, axis);
          if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
            num4 = Mathf.Max(num4, 1f);
          float num5 = Mathf.Clamp(num3, minSize1, (double) num4 <= 0.0 ? preferredSize1 : num1);
          float startOffset = this.GetStartOffset(axis, num5);
          this.SetChildAlongAxis(this.rectText, axis, startOffset, num5);
          return;
        default:
          num2 = this.padding.get_top();
          break;
      }
      float pos = (float) num2;
      if ((double) this.GetTotalFlexibleSize(axis) == 0.0 && (double) this.GetTotalPreferredSize(axis) < (double) num1)
        pos = this.GetStartOffset(axis, this.GetTotalPreferredSize(axis) - (axis != 0 ? (float) this.padding.get_vertical() : (float) this.padding.get_horizontal()));
      float num6 = 0.0f;
      if ((double) this.GetTotalMinSize(axis) != (double) this.GetTotalPreferredSize(axis))
        num6 = Mathf.Clamp01((float) (((double) num1 - (double) this.GetTotalMinSize(axis)) / ((double) this.GetTotalPreferredSize(axis) - (double) this.GetTotalMinSize(axis))));
      float num7 = 0.0f;
      if ((double) num1 > (double) this.GetTotalPreferredSize(axis) && (double) this.GetTotalFlexibleSize(axis) > 0.0)
        num7 = (num1 - this.GetTotalPreferredSize(axis)) / this.GetTotalFlexibleSize(axis);
      float minSize2 = LayoutUtility.GetMinSize(this.elementText, axis);
      float preferredSize2 = LayoutUtility.GetPreferredSize(this.elementText, axis);
      float num8 = LayoutUtility.GetFlexibleSize(this.elementText, axis);
      if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
        num8 = Mathf.Max(num8, 1f);
      float size2 = Mathf.Lerp(minSize2, preferredSize2, num6) + num8 * num7;
      this.SetChildAlongAxis(this.rectText, axis, pos, size2);
      float num9 = pos + size2;
    }

    protected float GetTotalMinSize(int axis)
    {
      return ((Vector2) ref this.m_TotalMinSize).get_Item(axis);
    }

    protected float GetTotalPreferredSize(int axis)
    {
      if (!this.isContentSizeFitter || this.isContentSizeFitter & (axis != 0 ? this.m_FitModeVertical : this.m_FitModeHorizontal) == 2)
        return ((Vector2) ref this.m_TotalPreferredSize).get_Item(axis);
      Vector2 sizeDelta = this.rectTransform.get_sizeDelta();
      return ((Vector2) ref sizeDelta).get_Item(axis);
    }

    protected float GetTotalFlexibleSize(int axis)
    {
      return ((Vector2) ref this.m_TotalFlexibleSize).get_Item(axis);
    }

    protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
    {
      float num1 = requiredSpaceWithoutPadding + (axis != 0 ? (float) this.padding.get_vertical() : (float) this.padding.get_horizontal());
      Rect rect = this.rectTransform.get_rect();
      Vector2 size = ((Rect) ref rect).get_size();
      float num2 = ((Vector2) ref size).get_Item(axis) - num1;
      float num3 = axis != 0 ? (float) (this.childAlignment / 3) * 0.5f : (float) (this.childAlignment % 3) * 0.5f;
      return (axis != 0 ? (float) this.padding.get_top() : (float) this.padding.get_left()) + num2 * num3;
    }

    protected void SetLayoutInputForAxis(
      float totalMin,
      float totalPreferred,
      float totalFlexible,
      int axis)
    {
      ((Vector2) ref this.m_TotalMinSize).set_Item(axis, totalMin);
      ((Vector2) ref this.m_TotalPreferredSize).set_Item(axis, totalPreferred);
      ((Vector2) ref this.m_TotalFlexibleSize).set_Item(axis, totalFlexible);
    }

    protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
    {
      if (Object.op_Equality((Object) rect, (Object) null))
        return;
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, rect, (DrivenTransformProperties) 16134);
      rect.SetInsetAndSizeFromParentEdge(axis != 0 ? (RectTransform.Edge) 2 : (RectTransform.Edge) 0, pos, size);
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
    }

    protected virtual void OnTransformChildrenChanged()
    {
      this.SetDirty();
    }

    protected void SetProperty<T>(ref T currentValue, T newValue)
    {
      if ((object) currentValue == null && (object) newValue == null || (object) currentValue != null && currentValue.Equals((object) newValue))
        return;
      currentValue = newValue;
      this.SetDirty();
    }

    protected void SetDirty()
    {
      if (!this.IsActive() || !Object.op_Inequality((Object) this.rectTransform, (Object) null))
        return;
      LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
    }
  }
}
