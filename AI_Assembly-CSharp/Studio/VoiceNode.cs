// Decompiled with JetBrains decompiler
// Type: Studio.VoiceNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class VoiceNode : PointerAction
  {
    [SerializeField]
    protected Button m_Button;
    [SerializeField]
    protected TextMeshProUGUI m_Text;
    [SerializeField]
    protected VoiceNode.ClickSound clickSound;

    public Button buttonUI
    {
      get
      {
        return this.m_Button;
      }
    }

    public TextMeshProUGUI textUI
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
        return ((TMP_Text) this.m_Text).get_text();
      }
      set
      {
        ((TMP_Text) this.m_Text).set_text(value);
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
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
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
      if (this.clickSound != VoiceNode.ClickSound.OK)
        return;
      // ISSUE: reference to a compiler-generated field
      if (VoiceNode.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        VoiceNode.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CAwake\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      this.addOnClick = VoiceNode.\u003C\u003Ef__am\u0024cache0;
    }

    protected enum ClickSound
    {
      NoSound,
      OK,
    }
  }
}
