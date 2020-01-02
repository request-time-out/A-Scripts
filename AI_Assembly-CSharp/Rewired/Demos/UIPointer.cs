// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.UIPointer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (RectTransform))]
  public sealed class UIPointer : UIBehaviour
  {
    [Tooltip("Should the hardware pointer be hidden?")]
    [SerializeField]
    private bool _hideHardwarePointer;
    [Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
    [SerializeField]
    private bool _autoSort;
    [NonSerialized]
    private RectTransform _canvasRectTransform;

    public UIPointer()
    {
      base.\u002Ector();
    }

    public bool autoSort
    {
      get
      {
        return this._autoSort;
      }
      set
      {
        if (value == this._autoSort)
          return;
        this._autoSort = value;
        if (!value)
          return;
        ((Component) this).get_transform().SetAsLastSibling();
      }
    }

    protected virtual void Awake()
    {
      base.Awake();
      foreach (Graphic componentsInChild in (Graphic[]) ((Component) this).GetComponentsInChildren<Graphic>())
        componentsInChild.set_raycastTarget(false);
      if (this._hideHardwarePointer)
        Cursor.set_visible(false);
      if (this._autoSort)
        ((Component) this).get_transform().SetAsLastSibling();
      this.GetDependencies();
    }

    private void Update()
    {
      if (!this._autoSort || ((Component) this).get_transform().GetSiblingIndex() >= ((Component) this).get_transform().get_parent().get_childCount() - 1)
        return;
      ((Component) this).get_transform().SetAsLastSibling();
    }

    protected virtual void OnTransformParentChanged()
    {
      base.OnTransformParentChanged();
      this.GetDependencies();
    }

    protected virtual void OnCanvasGroupChanged()
    {
      base.OnCanvasGroupChanged();
      this.GetDependencies();
    }

    public void OnScreenPositionChanged(Vector2 screenPosition)
    {
      if (Object.op_Equality((Object) this._canvasRectTransform, (Object) null))
        return;
      Rect rect = this._canvasRectTransform.get_rect();
      Vector2 vector2 = Vector2.op_Implicit(Camera.get_main().ScreenToViewportPoint(Vector2.op_Implicit(screenPosition)));
      vector2.x = (__Null) (vector2.x * (double) ((Rect) ref rect).get_width() - this._canvasRectTransform.get_pivot().x * (double) ((Rect) ref rect).get_width());
      vector2.y = (__Null) (vector2.y * (double) ((Rect) ref rect).get_height() - this._canvasRectTransform.get_pivot().y * (double) ((Rect) ref rect).get_height());
      (((Component) this).get_transform() as RectTransform).set_anchoredPosition(vector2);
    }

    private void GetDependencies()
    {
      Canvas componentInChildren = (Canvas) ((Component) ((Component) this).get_transform().get_root()).GetComponentInChildren<Canvas>();
      this._canvasRectTransform = !Object.op_Inequality((Object) componentInChildren, (Object) null) ? (RectTransform) null : (RectTransform) ((Component) componentInChildren).GetComponent<RectTransform>();
    }
  }
}
