// Decompiled with JetBrains decompiler
// Type: AIProject.UI.HomeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class HomeMenu : MenuUIBehaviour
  {
    private int _weatherIconID = -1;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text _timeLabel;
    [SerializeField]
    private Image _weatherImage;
    [SerializeField]
    private Text _temperatureLabel;
    [Header("IconLabel")]
    [SerializeField]
    private Text _statsText;
    [SerializeField]
    private Text _inventoryText;
    [SerializeField]
    private Text _mapText;
    [SerializeField]
    private Text _craftText;
    [SerializeField]
    private Text _cameraText;
    [SerializeField]
    private Text _callText;
    [SerializeField]
    private Text _helpText;
    [SerializeField]
    private Text _logText;
    [SerializeField]
    private Text _saveText;
    [SerializeField]
    private Text _optionText;
    [Header("Icon (Button)")]
    [SerializeField]
    private Button _statsButton;
    [SerializeField]
    private Button _inventoryButton;
    [SerializeField]
    private Button _mapButton;
    [SerializeField]
    private Button _craftButton;
    [SerializeField]
    private Button _cameraButton;
    [SerializeField]
    private Button _callButton;
    [SerializeField]
    private Button _helpButton;
    [SerializeField]
    private Button _logButton;
    [SerializeField]
    private Button _saveButton;
    [SerializeField]
    private Button _optionButton;
    private IDisposable _fadeSubscriber;
    private MenuUIBehaviour[] _menuUIList;
    private int _hour;
    private int _minute;
    private int _temperature;
    private Weather _weather;

    public SystemMenuUI Observer { get; set; }

    public Action OnClose { get; set; }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ObservableExtensions.Subscribe<long>(Observable.OnErrorRetry<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled() && this.IsActiveControl))), (Action<M0>) (_ => this.OnUpdate()));
      this._statsText.set_text("ステータス");
      this._inventoryText.set_text("ポーチ");
      this._mapText.set_text("マップ");
      this._craftText.set_text("クラフト");
      this._cameraText.set_text("カメラ");
      this._callText.set_text("コール");
      this._helpText.set_text("ヘルプ");
      this._logText.set_text("ログ");
      this._saveText.set_text("セーブ");
      this._optionText.set_text("オプション");
      if (Object.op_Implicit((Object) this._statsButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._statsButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      }
      if (Object.op_Implicit((Object) this._inventoryButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._inventoryButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      }
      if (Object.op_Implicit((Object) this._mapButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._mapButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__5)));
      }
      if (Object.op_Implicit((Object) this._craftButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._craftButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__6)));
      }
      if (Object.op_Implicit((Object) this._cameraButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._cameraButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__7)));
      }
      if (Object.op_Implicit((Object) this._callButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._callButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__8)));
      }
      if (Object.op_Implicit((Object) this._helpButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._helpButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__9)));
      }
      if (Object.op_Implicit((Object) this._logButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._logButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__A)));
      }
      if (Object.op_Implicit((Object) this._saveButton))
      {
        Button.ButtonClickedEvent onClick = this._saveButton.get_onClick();
        // ISSUE: reference to a compiler-generated field
        if (HomeMenu.\u003C\u003Ef__am\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          HomeMenu.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003COnBeforeStart\u003Em__B));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction fAmCache0 = HomeMenu.\u003C\u003Ef__am\u0024cache0;
        ((UnityEvent) onClick).AddListener(fAmCache0);
      }
      if (Object.op_Implicit((Object) this._optionButton))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._optionButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__C)));
      }
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__D)));
      this._keyCommands.Add(keyCodeDownCommand);
      this.EnabledInput = false;
    }

    private void SetActiveControl(bool active)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.DoClose();
      }
      if (this._fadeSubscriber != null)
        this._fadeSubscriber.Dispose();
      this._fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public MenuUIBehaviour[] MenuUIList
    {
      get
      {
        MenuUIBehaviour[] menuUiList = this._menuUIList;
        if (menuUiList != null)
          return menuUiList;
        return this._menuUIList = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    private void OnUpdate()
    {
      if (!Singleton<Manager.Map>.IsInstance() || Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null))
        return;
      EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
      DateTime now = simulator.Now;
      if (!this.EqualsTime(now))
      {
        this._hour = now.Hour;
        this._minute = now.Minute;
        this._timeLabel.set_text(string.Format("{0:00}：{1:00}", (object) this._hour, (object) this._minute));
      }
      int temperatureValue = (int) simulator.TemperatureValue;
      if (this._temperature != temperatureValue)
      {
        this._temperature = temperatureValue;
        this._temperatureLabel.set_text(string.Format("{0:0}℃", (object) this._temperature));
      }
      if (this._weather == simulator.Weather && this._weatherIconID != -1)
        return;
      this._weather = simulator.Weather;
      switch (this._weather)
      {
        case Weather.Clear:
        case Weather.Cloud1:
        case Weather.Cloud2:
          this._weatherIconID = 0;
          break;
        case Weather.Cloud3:
        case Weather.Cloud4:
          this._weatherIconID = 1;
          break;
        case Weather.Fog:
          this._weatherIconID = 4;
          break;
        case Weather.Rain:
          this._weatherIconID = 2;
          break;
        case Weather.Storm:
          this._weatherIconID = 3;
          break;
      }
      this._weatherImage.set_sprite((Sprite) null);
      this._weatherImage.set_sprite(Singleton<Resources>.Instance.itemIconTables.WeatherIconTable[this._weatherIconID]);
    }

    private bool EqualsTime(DateTime time)
    {
      return this._hour == time.Hour && this._minute == time.Minute;
    }

    private void OpenWindow(int id)
    {
      if (!Object.op_Inequality((Object) this.Observer, (Object) null))
        return;
      this.IsActiveControl = false;
      this.Observer.OpenModeMenu((SystemMenuUI.MenuMode) id);
    }

    private void Close()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      Action onClose = this.OnClose;
      if (onClose == null)
        return;
      onClose();
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HomeMenu.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HomeMenu.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void UsageRestriction()
    {
      bool tutorialMode = Manager.Map.TutorialMode;
      int tutorialProgress = Manager.Map.GetTutorialProgress();
      bool flag = true;
      PlayerActor player = Manager.Map.GetPlayer();
      if (Object.op_Inequality((Object) player, (Object) null))
        flag = player.PlayerController.PrevStateName != "Onbu";
      bool active = !tutorialMode;
      this.SetInteractable((Selectable) this._inventoryButton, active);
      this.SetInteractable((Selectable) this._cameraButton, active && flag);
      this.SetInteractable((Selectable) this._logButton, active);
      this.SetInteractable((Selectable) this._mapButton, active);
      this.SetInteractable((Selectable) this._callButton, active);
      if (tutorialMode)
      {
        this.SetInteractable((Selectable) this._statsButton, 5 <= tutorialProgress);
        this.SetInteractable((Selectable) this._craftButton, 4 <= tutorialProgress);
      }
      else
      {
        this.SetInteractable((Selectable) this._craftButton, true);
        this.SetInteractable((Selectable) this._statsButton, true);
      }
    }

    private bool SetInteractable(Selectable tar, bool active)
    {
      if (Object.op_Equality((Object) tar, (Object) null) || tar.get_interactable() == active)
        return false;
      tar.set_interactable(active);
      return true;
    }
  }
}
