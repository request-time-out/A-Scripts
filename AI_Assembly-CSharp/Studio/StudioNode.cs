// Decompiled with JetBrains decompiler
// Type: Studio.StudioNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class StudioNode : PointerAction
  {
    [SerializeField]
    protected Button m_Button;
    [SerializeField]
    protected Image m_ImageButton;
    [SerializeField]
    protected Text m_Text;
    [SerializeField]
    private TextMeshProUGUI _textMesh;
    [SerializeField]
    protected StudioNode.ClickSound clickSound;
    protected bool m_Select;

    public Button buttonUI
    {
      get
      {
        return this.m_Button;
      }
    }

    public Image imageButton
    {
      get
      {
        return this.m_ImageButton ?? (this.m_ImageButton = ((Selectable) this.m_Button).get_image());
      }
    }

    public Text textUI
    {
      get
      {
        return this.m_Text;
      }
    }

    public string text
    {
      get
      {
        return this.m_Text.get_text();
      }
      set
      {
        this.m_Text.SafeProc<Text>((Action<Text>) (_t => _t.set_text(value)));
        this._textMesh.SafeProc<TextMeshProUGUI>((Action<TextMeshProUGUI>) (_t => ((TMP_Text) _t).set_text(value)));
      }
    }

    public Color TextColor
    {
      set
      {
        this.m_Text.SafeProc<Text>((Action<Text>) (_t => ((Graphic) _t).set_color(value)));
        this._textMesh.SafeProc<TextMeshProUGUI>((Action<TextMeshProUGUI>) (_t => ((Graphic) _t).set_color(value)));
      }
    }

    public bool select
    {
      get
      {
        return this.m_Select;
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Select, value))
          return;
        ((Graphic) this.imageButton).set_color(!this.m_Select ? Color.get_white() : Color.get_green());
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
      }
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        ((Component) this).get_gameObject().SetActiveIfDifferent(value);
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
      if (this.clickSound != StudioNode.ClickSound.OK)
        return;
      // ISSUE: reference to a compiler-generated field
      if (StudioNode.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        StudioNode.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CAwake\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      this.addOnClick = StudioNode.\u003C\u003Ef__am\u0024cache0;
    }

    protected enum ClickSound
    {
      NoSound,
      OK,
    }
  }
}
