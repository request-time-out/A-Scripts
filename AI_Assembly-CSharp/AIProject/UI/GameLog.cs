// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GameLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using Illusion.Extensions;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class GameLog : MenuUIBehaviour
  {
    private Queue<GameLogElement> _logs = new Queue<GameLogElement>();
    private GameLog.GameLogElementPool _elementPool = new GameLog.GameLogElementPool();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private GameLogElement _node;
    [SerializeField]
    private RectTransform _guideRoot;
    private const float _alphaFadeDuration = 0.1f;
    private IObservable<TimeInterval<float>> _lerpStream;
    private MenuUIBehaviour[] _menuUIElements;
    private IDisposable _fadeSubscriber;

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] menuUiElements = this._menuUIElements;
        if (menuUiElements != null)
          return menuUiElements;
        return this._menuUIElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    protected override void OnBeforeStart()
    {
      this._elementPool = new GameLog.GameLogElementPool()
      {
        Source = this._node
      };
      this._lerpStream = (IObservable<TimeInterval<float>>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.1f, true), true);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (Object.op_Inequality((Object) this._closeButton, (Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      }
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._keyCommands.Add(keyCodeDownCommand);
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this._guideRoot, (Object) null))
        return;
      ((Component) this._guideRoot).get_gameObject().SetActiveIfDifferent(Manager.Config.GameData.ActionGuide);
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.Close() : this.Open();
      if (this._fadeSubscriber != null)
        this._fadeSubscriber.Dispose();
      this._fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    [DebuggerHidden]
    private IEnumerator Open()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new GameLog.\u003COpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator Close()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new GameLog.\u003CClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public Action OnClosed { get; set; }

    public void AddLog(
      string name,
      string message,
      IReadOnlyCollection<TextScenario.IVoice[]> voices)
    {
      if (this._logs.Count > 50)
        this._elementPool.Return(this._logs.Dequeue());
      GameLogElement gameLogElement = this._elementPool.Rent();
      ((Component) gameLogElement).get_transform().SetParent((Transform) this._scrollRect.get_content(), false);
      ((Component) gameLogElement).get_transform().set_localScale(Vector3.get_one());
      ((Component) gameLogElement).get_transform().SetAsFirstSibling();
      gameLogElement.Add(name, message, voices);
      this._logs.Enqueue(gameLogElement);
    }

    private void EnableRaycastTargetIfInvisible(bool isInvisible)
    {
      if (!isInvisible)
        return;
      this._canvasGroup.set_blocksRaycasts(true);
    }

    public class GameLogElementPool : ObjectPool<GameLogElement>
    {
      public GameLogElementPool()
      {
        base.\u002Ector();
      }

      public GameLogElement Source { get; set; }

      public Transform Parent { get; set; }

      protected virtual GameLogElement CreateInstance()
      {
        return (GameLogElement) ((Component) Object.Instantiate<GameLogElement>((M0) this.Source)).GetComponent<GameLogElement>();
      }

      protected virtual void OnBeforeReturn(GameLogElement instance)
      {
        if (Object.op_Equality((Object) instance, (Object) null))
          return;
        ((Component) instance).get_transform().SetParent(this.Parent);
        base.OnBeforeReturn(instance);
      }
    }
  }
}
