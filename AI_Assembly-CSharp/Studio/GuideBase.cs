// Decompiled with JetBrains decompiler
// Type: Studio.GuideBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
  {
    [SerializeField]
    protected Color colorNormal;
    [SerializeField]
    protected Color colorHighlighted;
    protected Renderer renderer;
    protected Collider collider;
    private BoolReactiveProperty _draw;
    public Action<Transform> pointerEnterAction;

    public GuideBase()
    {
      base.\u002Ector();
    }

    public Material material
    {
      get
      {
        return Object.op_Implicit((Object) this.renderer) ? this.renderer.get_material() : (Material) null;
      }
    }

    public bool draw
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

    protected Color colorNow
    {
      set
      {
        if (!Object.op_Implicit((Object) this.material))
          return;
        this.material.set_color(value);
        if (!this.material.HasProperty("_EmissionColor"))
          return;
        this.material.SetColor("_EmissionColor", value);
      }
    }

    public bool isDrag { get; private set; }

    public GuideObject guideObject { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (Singleton<GuideObjectManager>.Instance.isOperationTarget)
        return;
      this.colorNow = this.colorHighlighted;
      if (this.pointerEnterAction == null)
        return;
      this.pointerEnterAction(((Component) this).get_transform());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (this.isDrag)
        return;
      this.colorNow = this.colorNormal;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      this.isDrag = true;
      Singleton<GuideObjectManager>.Instance.operationTarget = this.guideObject;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      this.isDrag = false;
      this.colorNow = this.colorNormal;
      Singleton<GuideObjectManager>.Instance.operationTarget = (GuideObject) null;
    }

    private void OnDisable()
    {
      this.colorNow = this.colorNormal;
    }

    public virtual void Start()
    {
      this.renderer = (Renderer) ((Component) this).get_gameObject().GetComponent<Renderer>();
      if (Object.op_Equality((Object) this.renderer, (Object) null))
        this.renderer = (Renderer) ((Component) this).get_gameObject().GetComponentInChildren<Renderer>();
      this.collider = (Collider) ((Component) this.renderer).GetComponent<Collider>();
      this.colorNormal = this.ConvertColor(this.material.get_color());
      this.colorHighlighted = this.material.get_color();
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
        this.renderer.set_enabled(this.draw);
      if (Object.op_Implicit((Object) this.collider))
        this.collider.set_enabled(this.draw);
      this.colorNow = this.colorNormal;
      this.isDrag = false;
    }
  }
}
