// Decompiled with JetBrains decompiler
// Type: Studio.Anime.ListNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio.Anime
{
  public class ListNode : PointerAction
  {
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private TextMeshSlideEffect slideEffect;
    private Image imageButton;

    public TextMeshProUGUI TextMeshUGUI
    {
      get
      {
        return this.textMesh;
      }
    }

    private Image ImageButton
    {
      get
      {
        return this.imageButton ?? (this.imageButton = ((Selectable) this.button).get_image());
      }
    }

    public bool Interactable
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

    public bool Select
    {
      set
      {
        this.ImageButton.SafeProc<Image>((Action<Image>) (_i => ((Graphic) _i).set_color(!value ? Color.get_white() : Color.get_green())));
      }
    }

    public string Text
    {
      get
      {
        return Object.op_Inequality((Object) this.textMesh, (Object) null) ? ((TMP_Text) this.textMesh).get_text() : string.Empty;
      }
      set
      {
        this.textMesh.SafeProc<TextMeshProUGUI>((Action<TextMeshProUGUI>) (_t =>
        {
          ((TMP_Text) _t).set_text(value);
          if (!this.UseSlide || this.slideEffect == null)
            return;
          this.slideEffect.OnChangedText();
        }));
      }
    }

    public Color TextColor
    {
      set
      {
        this.textMesh.SafeProc<TextMeshProUGUI>((Action<TextMeshProUGUI>) (_t => ((Graphic) _t).set_color(value)));
      }
    }

    public Material TextMeshMaterial
    {
      set
      {
        ((TMP_Text) this.textMesh).set_fontSharedMaterial(value);
      }
    }

    public bool UseSlide { get; set; } = true;

    public void SetButtonAction(UnityAction _action)
    {
      if (Object.op_Equality((Object) this.button, (Object) null))
        return;
      ((UnityEventBase) this.button.get_onClick()).RemoveAllListeners();
      ((UnityEvent) this.button.get_onClick()).AddListener(_action);
      // ISSUE: method pointer
      ((UnityEvent) this.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CSetButtonAction\u003Em__0)));
    }
  }
}
