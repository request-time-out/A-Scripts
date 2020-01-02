// Decompiled with JetBrains decompiler
// Type: Studio.ListNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class ListNode : PointerAction
  {
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image imageSelect;
    [SerializeField]
    private Text content;
    [SerializeField]
    private TextMeshProUGUI textMesh;

    public bool interactable
    {
      get
      {
        return Object.op_Inequality((Object) this.button, (Object) null) && ((Selectable) this.button).get_interactable();
      }
      set
      {
        this.button.SafeProc<Button>((Action<Button>) (_b => ((Selectable) _b).set_interactable(value)));
      }
    }

    public bool select
    {
      get
      {
        return Object.op_Inequality((Object) this.imageSelect, (Object) null) && ((Behaviour) this.imageSelect).get_enabled();
      }
      set
      {
        this.imageSelect.SafeProc<Image>((Action<Image>) (_i => ((Behaviour) _i).set_enabled(value)));
      }
    }

    public Image image
    {
      get
      {
        return Object.op_Inequality((Object) this.button, (Object) null) ? ((Selectable) this.button).get_image() : (Image) null;
      }
    }

    public Sprite selectSprite
    {
      set
      {
        this.imageSelect.SafeProc<Image>((Action<Image>) (_i => _i.set_sprite(value)));
      }
    }

    public string text
    {
      get
      {
        if (Object.op_Inequality((Object) this.content, (Object) null))
          return this.content.get_text();
        return Object.op_Inequality((Object) this.textMesh, (Object) null) ? ((TMP_Text) this.textMesh).get_text() : string.Empty;
      }
      set
      {
        this.content.SafeProc<Text>((Action<Text>) (_t => _t.set_text(value)));
        this.textMesh.SafeProc<TextMeshProUGUI>((Action<TextMeshProUGUI>) (_t => ((TMP_Text) _t).set_text(value)));
      }
    }

    private void SetCoverEnabled(bool _enabled)
    {
      if (!Object.op_Inequality((Object) this.button, (Object) null) || ((Selectable) this.button).get_interactable())
        ;
    }

    private void PlaySelectSE()
    {
      if (!Object.op_Inequality((Object) this.button, (Object) null) || ((Selectable) this.button).get_interactable())
        ;
    }

    public void AddActionToButton(UnityAction _action)
    {
      if (this.button == null)
        return;
      ((UnityEvent) this.button.get_onClick()).AddListener(_action);
    }

    public void SetButtonAction(UnityAction _action)
    {
      if (Object.op_Equality((Object) this.button, (Object) null))
        return;
      ((UnityEventBase) this.button.get_onClick()).RemoveAllListeners();
      ((UnityEvent) this.button.get_onClick()).AddListener(_action);
    }

    private void Awake()
    {
      // ISSUE: method pointer
      this.listEnterAction.Add(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__0)));
      // ISSUE: method pointer
      this.listEnterAction.Add(new UnityAction((object) this, __methodptr(PlaySelectSE)));
      // ISSUE: method pointer
      this.listExitAction.Add(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__1)));
    }
  }
}
