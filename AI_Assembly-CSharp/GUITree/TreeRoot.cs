// Decompiled with JetBrains decompiler
// Type: GUITree.TreeRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUITree
{
  [AddComponentMenu("GUITree/Tree Root", 1003)]
  [DisallowMultipleComponent]
  [ExecuteInEditMode]
  [RequireComponent(typeof (RectTransform))]
  public class TreeRoot : UIBehaviour, ITreeLayoutElement, ILayoutGroup, ILayoutElement, ILayoutController
  {
    private RectTransform m_Rect;
    protected DrivenRectTransformTracker m_Tracker;
    private Vector2 m_TotalPreferredSize;
    private List<TreeNode> m_TreeLayoutElement;
    [SerializeField]
    protected float m_Spacing;

    protected TreeRoot()
    {
      base.\u002Ector();
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

    protected List<TreeNode> treeChildren
    {
      get
      {
        return this.m_TreeLayoutElement;
      }
    }

    public float spacing
    {
      get
      {
        return this.m_Spacing;
      }
      set
      {
        this.SetProperty<float>(ref this.m_Spacing, value);
      }
    }

    protected void CalcAlongAxis(int axis)
    {
      float num = 0.0f;
      bool flag = (1 ^ (axis == 1 ? 1 : 0)) != 0;
      for (int index = 0; index < this.treeChildren.Count; ++index)
      {
        float preferredSize = LayoutUtility.GetPreferredSize((ILayoutElement) this.treeChildren[index], axis);
        if (flag)
          num = Mathf.Max(preferredSize, num);
        else
          num += preferredSize + this.spacing;
      }
      if (!flag && this.treeChildren.Count > 0)
        num -= this.spacing;
      ((Vector2) ref this.m_TotalPreferredSize).set_Item(axis, num);
    }

    public virtual void CalculateLayoutInputHorizontal()
    {
      this.m_TreeLayoutElement.Clear();
      for (int index = 0; index < ((Transform) this.rectTransform).get_childCount(); ++index)
      {
        TreeNode component = (TreeNode) ((Component) ((Transform) this.rectTransform).GetChild(index)).GetComponent<TreeNode>();
        if (!Object.op_Equality((Object) component, (Object) null) && component.IsActive())
          this.m_TreeLayoutElement.Add(component);
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
        return this.preferredWidth;
      }
    }

    public virtual float preferredWidth
    {
      get
      {
        return ((Vector2) ref this.m_TotalPreferredSize).get_Item(0);
      }
    }

    public virtual float flexibleWidth
    {
      get
      {
        return this.preferredWidth;
      }
    }

    public virtual float minHeight
    {
      get
      {
        return this.preferredHeight;
      }
    }

    public virtual float preferredHeight
    {
      get
      {
        return ((Vector2) ref this.m_TotalPreferredSize).get_Item(1);
      }
    }

    public virtual float flexibleHeight
    {
      get
      {
        return this.preferredHeight;
      }
    }

    public virtual int layoutPriority
    {
      get
      {
        return 0;
      }
    }

    public void SetLayoutHorizontal()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
    }

    public void SetLayoutVertical()
    {
      float num = 0.0f;
      for (int index = 0; index < this.treeChildren.Count; ++index)
      {
        TreeNode treeChild = this.treeChildren[index];
        float preferredSize = LayoutUtility.GetPreferredSize((ILayoutElement) treeChild, 1);
        ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, treeChild.rectTransform, (DrivenTransformProperties) 4);
        treeChild.rectTransform.SetInsetAndSizeFromParentEdge((RectTransform.Edge) 2, num, preferredSize);
        num += preferredSize + this.spacing;
      }
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

    private bool isRootLayoutGroup
    {
      get
      {
        return Object.op_Equality((Object) ((Component) this).get_transform().get_parent(), (Object) null) || Object.op_Equality((Object) ((Component) ((Component) this).get_transform().get_parent()).GetComponent(typeof (ILayoutGroup)), (Object) null);
      }
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (!this.isRootLayoutGroup)
        return;
      this.SetDirty();
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
