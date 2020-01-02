// Decompiled with JetBrains decompiler
// Type: GUITree.PreferredSizeFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUITree
{
  [AddComponentMenu("GUITree/Preferred Size Fitter", 1001)]
  [ExecuteInEditMode]
  [RequireComponent(typeof (RectTransform))]
  public class PreferredSizeFitter : UIBehaviour, ITreeLayoutElement, ILayoutSelfController, ILayoutElement, ILayoutController
  {
    [SerializeField]
    private float m_PreferredWidth;
    [SerializeField]
    private float m_PreferredHeight;
    [NonSerialized]
    private RectTransform m_Rect;
    private DrivenRectTransformTracker m_Tracker;

    protected PreferredSizeFitter()
    {
      base.\u002Ector();
    }

    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    public virtual void CalculateLayoutInputVertical()
    {
    }

    public virtual float minWidth
    {
      get
      {
        return this.m_PreferredWidth;
      }
    }

    public virtual float minHeight
    {
      get
      {
        return this.m_PreferredHeight;
      }
    }

    public virtual float preferredWidth
    {
      get
      {
        return this.m_PreferredWidth;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_PreferredWidth, value))
          return;
        this.SetDirty();
      }
    }

    public virtual float preferredHeight
    {
      get
      {
        return this.m_PreferredHeight;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_PreferredHeight, value))
          return;
        this.SetDirty();
      }
    }

    public virtual float flexibleWidth
    {
      get
      {
        return this.m_PreferredWidth;
      }
    }

    public virtual float flexibleHeight
    {
      get
      {
        return this.m_PreferredHeight;
      }
    }

    public virtual int layoutPriority
    {
      get
      {
        return int.MaxValue;
      }
    }

    private RectTransform rectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Rect, (Object) null))
          this.m_Rect = (RectTransform) ((Component) this).GetComponent<RectTransform>();
        return this.m_Rect;
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

    protected virtual void OnRectTransformDimensionsChange()
    {
      this.SetDirty();
    }

    private void HandleSelfFittingAlongAxis(int axis)
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.rectTransform, axis != 0 ? (DrivenTransformProperties) 8192 : (DrivenTransformProperties) 4096);
      this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) axis, LayoutUtility.GetPreferredSize((ILayoutElement) this, axis));
    }

    public virtual void SetLayoutHorizontal()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
      this.HandleSelfFittingAlongAxis(0);
    }

    public virtual void SetLayoutVertical()
    {
      this.HandleSelfFittingAlongAxis(1);
    }

    protected void SetDirty()
    {
      if (!this.IsActive() || !Object.op_Inequality((Object) this.rectTransform, (Object) null))
        return;
      LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
    }
  }
}
