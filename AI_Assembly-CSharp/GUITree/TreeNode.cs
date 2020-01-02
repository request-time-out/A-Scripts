// Decompiled with JetBrains decompiler
// Type: GUITree.TreeNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUITree
{
  [AddComponentMenu("GUITree/Tree Node", 1000)]
  [RequireComponent(typeof (RectTransform))]
  public class TreeNode : UIBehaviour, ITreeLayoutElement, ILayoutSelfController, ILayoutElement, ILayoutController
  {
    private float m_PreferredWidth;
    private float m_PreferredHeight;
    [NonSerialized]
    private RectTransform m_Rect;
    private DrivenRectTransformTracker m_Tracker;
    [SerializeField]
    private int m_Indent;
    [SerializeField]
    private float m_IndentSize;

    public TreeNode()
    {
      base.\u002Ector();
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
        return this.m_PreferredWidth + (float) this.indent * this.indentSize;
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
        return this.m_PreferredHeight;
      }
    }

    public virtual float flexibleHeight
    {
      get
      {
        return this.preferredHeight;
      }
    }

    public int layoutPriority
    {
      get
      {
        return int.MaxValue;
      }
    }

    public RectTransform rectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Rect, (Object) null))
          this.m_Rect = (RectTransform) ((Component) this).GetComponent<RectTransform>();
        return this.m_Rect;
      }
    }

    public int indent
    {
      get
      {
        return this.m_Indent;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<int>(ref this.m_Indent, value))
          return;
        this.SetDirty();
      }
    }

    public float indentSize
    {
      get
      {
        return this.m_IndentSize;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_IndentSize, value))
          return;
        this.SetDirty();
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
      if (Object.op_Implicit((Object) this.rectTransform))
        LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
      base.OnDisable();
    }

    public void CalculateLayoutInputHorizontal()
    {
      int childCount = ((Transform) this.rectTransform).get_childCount();
      float num = 0.0f;
      for (int index = 0; index < childCount; ++index)
      {
        ITreeLayoutElement component = (ITreeLayoutElement) ((Component) ((Transform) this.rectTransform).GetChild(index)).GetComponent<ITreeLayoutElement>();
        if (component != null)
          num += LayoutUtility.GetPreferredSize((ILayoutElement) component, 0);
      }
      this.m_PreferredWidth = num;
    }

    public void CalculateLayoutInputVertical()
    {
      int childCount = ((Transform) this.rectTransform).get_childCount();
      float num = 0.0f;
      for (int index = 0; index < childCount; ++index)
      {
        ITreeLayoutElement component = (ITreeLayoutElement) ((Component) ((Transform) this.rectTransform).GetChild(index)).GetComponent<ITreeLayoutElement>();
        if (component != null)
        {
          float preferredSize = LayoutUtility.GetPreferredSize((ILayoutElement) component, 1);
          if ((double) num < (double) preferredSize)
            num = preferredSize;
        }
      }
      this.m_PreferredHeight = num;
    }

    public void SetLayoutHorizontal()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.rectTransform, (DrivenTransformProperties) 4098);
      this.rectTransform.SetInsetAndSizeFromParentEdge((RectTransform.Edge) 0, (float) this.indent * this.indentSize, LayoutUtility.GetPreferredSize((ILayoutElement) this, 0));
    }

    public void SetLayoutVertical()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.rectTransform, (DrivenTransformProperties) 8192);
      this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, LayoutUtility.GetPreferredSize((ILayoutElement) this, 1));
    }

    protected void SetDirty()
    {
      if (!this.IsActive() || !Object.op_Inequality((Object) this.rectTransform, (Object) null))
        return;
      LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
    }
  }
}
