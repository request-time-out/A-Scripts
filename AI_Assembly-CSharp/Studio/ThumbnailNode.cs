// Decompiled with JetBrains decompiler
// Type: Studio.ThumbnailNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class ThumbnailNode : PointerAction
  {
    [SerializeField]
    protected Button m_Button;
    [SerializeField]
    private RawImage m_Image;
    [SerializeField]
    protected ThumbnailNode.ClickSound clickSound;

    public Button button
    {
      get
      {
        return this.m_Button;
      }
    }

    public RawImage image
    {
      get
      {
        return this.m_Image;
      }
    }

    public Texture texture
    {
      set
      {
        this.m_Image.set_texture(value);
      }
      get
      {
        return this.m_Image.get_texture();
      }
    }

    public bool interactable
    {
      get
      {
        return ((Selectable) this.m_Button).get_interactable();
      }
      set
      {
        ((Selectable) this.m_Button).set_interactable(value);
        ((Graphic) this.m_Image).set_color(!value ? Color.get_clear() : Color.get_white());
      }
    }

    public UnityAction addOnClick
    {
      set
      {
        ((UnityEvent) this.m_Button.get_onClick()).AddListener(value);
      }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      if (!this.interactable)
        return;
      base.OnPointerEnter(eventData);
    }

    public virtual void Awake()
    {
      if (this.clickSound != ThumbnailNode.ClickSound.OK)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ThumbnailNode.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ThumbnailNode.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CAwake\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      this.addOnClick = ThumbnailNode.\u003C\u003Ef__am\u0024cache0;
    }

    protected enum ClickSound
    {
      NoSound,
      OK,
    }
  }
}
