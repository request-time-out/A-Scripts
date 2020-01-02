// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomGuideBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace CharaCustom
{
  public class CustomGuideBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
  {
    [SerializeField]
    protected Color colorNormal;
    [SerializeField]
    protected Color colorHighlighted;
    protected Renderer renderer;
    protected Collider collider;
    private bool m_Draw;

    public CustomGuideBase()
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
        return this.m_Draw;
      }
      set
      {
        if (this.m_Draw == value)
          return;
        this.m_Draw = value;
        if (Object.op_Implicit((Object) this.renderer))
          this.renderer.set_enabled(this.m_Draw);
        if (!Object.op_Implicit((Object) this.collider))
          return;
        this.collider.set_enabled(this.m_Draw);
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
      }
    }

    public bool isDrag { get; private set; }

    public CustomGuideObject guideObject { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      this.colorNow = this.colorHighlighted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (this.isDrag)
        return;
      this.colorNow = this.colorNormal;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      this.isDrag = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      this.isDrag = false;
      this.colorNow = this.colorNormal;
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
      if (Object.op_Implicit((Object) this.renderer))
        this.renderer.set_enabled(this.m_Draw);
      if (Object.op_Implicit((Object) this.collider))
        this.collider.set_enabled(this.m_Draw);
      this.colorNow = this.colorNormal;
      this.isDrag = false;
    }
  }
}
