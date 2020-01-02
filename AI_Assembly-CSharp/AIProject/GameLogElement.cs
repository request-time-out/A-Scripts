// Decompiled with JetBrains decompiler
// Type: AIProject.GameLogElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject
{
  public class GameLogElement : MonoBehaviour
  {
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Button _voiceButton;
    private IDisposable _disposable;

    public GameLogElement()
    {
      base.\u002Ector();
    }

    public Text NameText
    {
      get
      {
        return this._nameText;
      }
    }

    public Text Text
    {
      get
      {
        return this._text;
      }
    }

    public Button VoiceButton
    {
      get
      {
        return this._voiceButton;
      }
    }

    private IReadOnlyCollection<TextScenario.IVoice[]> _voices { get; set; }

    private void OnDisable()
    {
      if (this._disposable != null)
        this._disposable.Dispose();
      this._disposable = (IDisposable) null;
    }

    public void Add(string name, string text, IReadOnlyCollection<TextScenario.IVoice[]> voices)
    {
      this._nameText.set_text(name);
      this._text.set_text(text);
      this._voices = voices;
      Image image = ((Selectable) this._voiceButton).get_image();
      bool flag = this._voices != null && ((IEnumerable<TextScenario.IVoice[]>) this._voices).Any<TextScenario.IVoice[]>();
      ((Graphic) ((Selectable) this._voiceButton).get_image()).set_raycastTarget(flag);
      int num = flag ? 1 : 0;
      ((Behaviour) image).set_enabled(num != 0);
      ((UnityEventBase) this._voiceButton.get_onClick()).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent) this._voiceButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAdd\u003Em__0)));
    }

    [DebuggerHidden]
    private IEnumerator VoicePlay(IReadOnlyCollection<TextScenario.IVoice[]> voices)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new GameLogElement.\u003CVoicePlay\u003Ec__Iterator0()
      {
        voices = voices
      };
    }
  }
}
