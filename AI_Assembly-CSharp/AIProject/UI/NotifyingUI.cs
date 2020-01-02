// Decompiled with JetBrains decompiler
// Type: AIProject.UI.NotifyingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class NotifyingUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    public Button PouchOpen;
    public Button NotGet;
    [SerializeField]
    private Text _contentText;
    [SerializeField]
    private Image _raycastTarget;
    private IConnectableObservable<Unit> _fadeStream;

    public PointerClickTrigger ClickTrigger { get; private set; }

    protected override void Start()
    {
      this.ClickTrigger = (PointerClickTrigger) ((Component) this._raycastTarget).get_gameObject().AddComponent<PointerClickTrigger>();
      ((Behaviour) this.ClickTrigger).set_enabled(false);
      UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CStart\u003Em__0)));
      ((UITrigger) this.ClickTrigger).get_Triggers().Add(triggerEvent);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      base.Start();
      ((Component) this).get_gameObject().SetActive(false);
    }

    private void OnClick()
    {
      ((ReactiveProperty<bool>) this._isActive).set_Value(false);
    }

    public void Display(string text)
    {
      this._contentText.set_text(text);
      ((ReactiveProperty<bool>) this._isActive).set_Value(true);
    }

    public void Hide()
    {
      ((ReactiveProperty<bool>) this._isActive).set_Value(false);
    }

    private void SetActiveControl(bool isActive)
    {
      if (isActive)
        ((Component) this).get_gameObject().SetActive(isActive);
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      this._fadeStream = (IConnectableObservable<Unit>) Observable.PublishLast<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
      this._fadeStream.Connect();
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyingUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyingUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }
  }
}
