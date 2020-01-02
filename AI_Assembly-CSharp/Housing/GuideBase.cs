// Decompiled with JetBrains decompiler
// Type: Housing.GuideBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Housing
{
  public class GuideBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    [SerializeField]
    protected Color colorNormal;
    [SerializeField]
    protected Color colorHighlighted;
    protected Renderer renderer;
    protected Collider collider;
    protected BoolReactiveProperty _draw;
    public Action<Transform> pointerEnterAction;
    public Action<Transform> pointerExitAction;
    public Action<Transform> beginDragAction;
    public Action<Transform> endDragAction;
    public Action pointerDownAction;
    public Action pointerUpAction;

    public GuideBase()
    {
      base.\u002Ector();
    }

    public Material Material
    {
      get
      {
        return Object.op_Implicit((Object) this.renderer) ? this.renderer.get_material() : (Material) null;
      }
    }

    public bool Draw
    {
      get
      {
        return ((ReactiveProperty<bool>) this._draw).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._draw).set_Value(value);
      }
    }

    protected Color ConvertColor(Color _color)
    {
      ref Color local1 = ref _color;
      local1.r = (__Null) (local1.r * 0.75);
      ref Color local2 = ref _color;
      local2.g = (__Null) (local2.g * 0.75);
      ref Color local3 = ref _color;
      local3.b = (__Null) (local3.b * 0.75);
      _color.a = (__Null) 0.25;
      return _color;
    }

    protected Color ColorNow
    {
      set
      {
        if (!Object.op_Implicit((Object) this.Material))
          return;
        this.Material.set_color(value);
        if (!this.Material.HasProperty("_EmissionColor"))
          return;
        this.Material.SetColor("_EmissionColor", value);
      }
    }

    public bool IsInit { get; protected set; }

    public bool IsDrag { get; protected set; }

    public GuideObject GuideObject { get; protected set; }

    public virtual void Init(GuideObject _guideObject)
    {
      if (this.IsInit)
        return;
      this.GuideObject = _guideObject;
      this.renderer = (Renderer) ((Component) this).get_gameObject().GetComponent<Renderer>();
      if (Object.op_Equality((Object) this.renderer, (Object) null))
        this.renderer = (Renderer) ((Component) this).get_gameObject().GetComponentInChildren<Renderer>();
      this.collider = (Collider) ((Component) this.renderer).GetComponent<Collider>();
      this.colorNormal = this.ConvertColor(this.Material.get_color());
      this.colorHighlighted = this.Material.get_color();
      this.colorHighlighted.a = (__Null) 0.75;
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._draw, (Action<M0>) (_b =>
      {
        if (Object.op_Implicit((Object) this.renderer))
          this.renderer.set_enabled(_b);
        if (!Object.op_Implicit((Object) this.collider))
          return;
        this.collider.set_enabled(_b);
      }));
      if (Object.op_Implicit((Object) this.renderer))
        this.renderer.set_enabled(this.Draw);
      if (Object.op_Implicit((Object) this.collider))
        this.collider.set_enabled(this.Draw);
      this.ColorNow = this.colorNormal;
      this.IsDrag = false;
      this.IsInit = true;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.ColorNow = this.colorHighlighted;
      if (this.pointerEnterAction == null)
        return;
      this.pointerEnterAction(((Component) this).get_transform());
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      if (!this.IsDrag)
        this.ColorNow = this.colorNormal;
      if (this.pointerExitAction == null)
        return;
      this.pointerExitAction(((Component) this).get_transform());
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (!eventData.get_dragging())
        return;
      this.IsDrag = true;
      if (this.beginDragAction == null)
        return;
      this.beginDragAction(((Component) this).get_transform());
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      if (!eventData.get_dragging() || eventData.get_button() != null)
        return;
      this.IsDrag = false;
      this.ColorNow = this.colorNormal;
      if (this.endDragAction != null)
        this.endDragAction(((Component) this).get_transform());
      if (!Singleton<GuideManager>.IsInstance())
        return;
      Singleton<GuideManager>.Instance.IsGuide = false;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      if (this.pointerDownAction != null)
        this.pointerDownAction();
      if (!Singleton<GuideManager>.IsInstance())
        return;
      Singleton<GuideManager>.Instance.IsGuide = true;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      if (this.pointerUpAction != null)
        this.pointerUpAction();
      if (!Singleton<GuideManager>.IsInstance())
        return;
      Singleton<GuideManager>.Instance.IsGuide = false;
    }

    private void OnDisable()
    {
      this.ColorNow = this.colorNormal;
    }
  }
}
