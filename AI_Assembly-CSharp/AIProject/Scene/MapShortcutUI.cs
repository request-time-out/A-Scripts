// Decompiled with JetBrains decompiler
// Type: AIProject.Scene.MapShortcutUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using ReMotion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.Scene
{
  public class MapShortcutUI : MenuUIBehaviour
  {
    [SerializeField]
    private Dictionary<int, Sprite> _imageTable = new Dictionary<int, Sprite>();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _image;
    private Input.ValidType _validType;

    public static int ImageIndex { get; set; }

    public static Action ClosedEvent { get; set; }

    protected override void Awake()
    {
      if (Singleton<Game>.IsInstance())
        Singleton<Game>.Instance.MapShortcutUI = this;
      if (Singleton<Input>.IsInstance())
      {
        this._validType = Singleton<Input>.Instance.State;
        Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
        Singleton<Input>.Instance.SetupState();
      }
      Sprite sprite;
      this._imageTable.TryGetValue(MapShortcutUI.ImageIndex, out sprite);
      this._image.set_sprite(sprite);
    }

    protected override void Start()
    {
      this.Open();
      KeyCodeDownCommand keyCodeDownCommand1 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 27
      };
      UnityEvent triggerEvent = keyCodeDownCommand1.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapShortcutUI.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapShortcutUI.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CStart\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache0 = MapShortcutUI.\u003C\u003Ef__am\u0024cache0;
      triggerEvent.AddListener(fAmCache0);
      this._keyCommands.Add(keyCodeDownCommand1);
      KeyCodeDownCommand keyCodeDownCommand2 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
      this._keyCommands.Add(keyCodeDownCommand2);
      KeyCodeDownCommand keyCodeDownCommand3 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 283
      };
      // ISSUE: method pointer
      keyCodeDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      this._keyCommands.Add(keyCodeDownCommand3);
      base.Start();
    }

    protected override void OnDisable()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Singleton<Game>.Instance.MapShortcutUI = (MapShortcutUI) null;
    }

    private void Open()
    {
      this.EnabledInput = false;
      this._canvasGroup.set_blocksRaycasts(false);
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
      {
        this._canvasGroup.set_blocksRaycasts(true);
        this.EnabledInput = true;
      }));
    }

    private void Close(Action onCompleted)
    {
      this.EnabledInput = false;
      this._canvasGroup.set_blocksRaycasts(false);
      if (Singleton<Input>.IsInstance())
      {
        Singleton<Input>.Instance.ReserveState(this._validType);
        Singleton<Input>.Instance.SetupState();
      }
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
      {
        Action action = onCompleted;
        if (action != null)
          action();
        Action closedEvent = MapShortcutUI.ClosedEvent;
        if (closedEvent == null)
          return;
        closedEvent();
      }));
    }
  }
}
