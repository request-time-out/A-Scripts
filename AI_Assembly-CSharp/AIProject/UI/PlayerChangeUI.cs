// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PlayerChangeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using GameLoadCharaFileSystem;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class PlayerChangeUI : MenuUIBehaviour
  {
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
    private BoolReactiveProperty _isActiveLCW = new BoolReactiveProperty(false);
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button _playerButton;
    [SerializeField]
    private RectTransform _element;
    [SerializeField]
    private Text _charaText;
    [SerializeField]
    private RectTransform _selectedImageTransform;
    [SerializeField]
    private GameLoadCharaWindow _lcw;
    [SerializeField]
    private CanvasGroup _lcwCanvasGroup;
    [SerializeField]
    private GameObject _lcwObject;
    private IConnectableObservable<int> _selectIDChange;
    private IConnectableObservable<bool> _activeChangeLCW;
    private MenuUIBehaviour[] _menuUIList;
    private GameCharaFileInfo _info;
    private IDisposable _fadeDisposable;
    private IDisposable _fadeLCWDisposable;

    public IObservable<int> OnSelectIDChangedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((Component) this).get_gameObject()));
        this._selectIDChange.Connect();
      }
      return (IObservable<int>) this._selectIDChange;
    }

    private IObservable<bool> OnActiveLCWChangedAsObservable()
    {
      if (this._activeChangeLCW == null)
      {
        this._activeChangeLCW = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.Where<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActiveLCW, ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())));
        this._activeChangeLCW.Connect();
      }
      return (IObservable<bool>) this._activeChangeLCW;
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

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (active => this.SetActiveControl(active)));
      // ISSUE: method pointer
      ((UnityEvent) this._playerButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (Action<M0>) (x =>
      {
        ((Component) this._selectedImageTransform).get_gameObject().SetActiveIfDifferent(x != 1);
        if (!((Component) this._selectedImageTransform).get_gameObject().get_activeSelf())
          return;
        ((Transform) this._selectedImageTransform).set_localPosition(((Transform) this._element).get_localPosition());
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveLCWChangedAsObservable(), (Action<M0>) (active => this.SetActiveControlLCW(active)));
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__5)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__6)));
      this._lcw.onLoadItemFunc = (Action<GameCharaFileInfo>) (dat =>
      {
        PlayerData playerData = Singleton<Game>.Instance.WorldData.PlayerData;
        playerData.CharaFileNames[dat.sex] = dat.FileName;
        playerData.Sex = (byte) dat.sex;
        this._info = dat;
        this._charaText.set_text(dat.name);
        ((ReactiveProperty<bool>) this._isActiveLCW).set_Value(false);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      });
      this._lcw.onClickRightFunc = (Action) (() =>
      {
        if (!((ReactiveProperty<bool>) this._isActiveLCW).get_Value())
          return;
        ((ReactiveProperty<bool>) this._isActiveLCW).set_Value(false);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      });
      this._lcw.onCloseWindowFunc = (GameLoadCharaWindow.OnCloseWindowFunc) (() =>
      {
        if (!((ReactiveProperty<bool>) this._isActiveLCW).get_Value())
          return;
        ((ReactiveProperty<bool>) this._isActiveLCW).set_Value(false);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      });
      this._lcw.onCharaCreateClickAction = (Action<int>) (sex =>
      {
        if (Singleton<Scene>.Instance.IsNowLoadingFade)
          return;
        CharaCustom.CharaCustom.modeNew = true;
        CharaCustom.CharaCustom.modeSex = (byte) sex;
        CharaCustom.CharaCustom.actEixt = (Action) null;
        CharaCustom.CharaCustom.nextScene = Singleton<Resources>.Instance.DefinePack.SceneNames.MapScene;
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "CharaCustom",
          isAdd = false,
          isFade = true
        }, false);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      });
      ((Component) this._selectedImageTransform).get_gameObject().SetActive(false);
    }

    private void Close()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        Time.set_timeScale(0.0f);
        ChaFileControl chaFileControl = new ChaFileControl();
        PlayerData playerData = Singleton<Game>.Instance.WorldData.PlayerData;
        if (!playerData.CharaFileName.IsNullOrEmpty() && chaFileControl.LoadCharaFile(playerData.CharaFileName, playerData.Sex, false, true))
        {
          string empty = string.Empty;
          VoiceInfo.Param obj;
          string str1 = Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明";
          string str2 = playerData.Sex != (byte) 0 ? string.Format("{0}chara/female/{1}.png", (object) UserData.Path, (object) playerData.CharaFileName) : string.Format("{0}chara/male/{1}.png", (object) UserData.Path, (object) playerData.CharaFileName);
          this._info = new GameCharaFileInfo()
          {
            name = chaFileControl.parameter.fullname,
            personality = str1,
            voice = chaFileControl.parameter.personality,
            hair = chaFileControl.custom.hair.kind,
            birthMonth = (int) chaFileControl.parameter.birthMonth,
            birthDay = (int) chaFileControl.parameter.birthDay,
            strBirthDay = chaFileControl.parameter.strBirthDay,
            sex = (int) chaFileControl.parameter.sex,
            FullPath = str2,
            FileName = playerData.CharaFileName,
            gameRegistration = chaFileControl.gameinfo.gameRegistration,
            flavorState = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.flavorState),
            phase = chaFileControl.gameinfo.phase,
            normalSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.normalSkill),
            hSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.hSkill),
            favoritePlace = chaFileControl.gameinfo.favoritePlace,
            futanari = chaFileControl.parameter.futanari,
            data_uuid = chaFileControl.dataID
          };
        }
        else
          this._info = (GameCharaFileInfo) null;
        if (this._info != null)
          this._charaText.set_text(this._info.name);
        else
          this._charaText.set_text("-----");
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.OpenCoroutine();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.CloseCoroutine();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerChangeUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerChangeUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SetActiveControlLCW(bool active)
    {
      Input input = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        input.FocusLevel = 1;
        ((Component) this._lcw).get_gameObject().SetActiveIfDifferent(true);
        this._lcw.useDownload = true;
        this._lcw.ReCreateList(true, true);
        coroutine = this.OpenLCWCoroutine();
      }
      else
        coroutine = this.CloseLCWCoroutine();
      if (this._fadeLCWDisposable != null)
        this._fadeLCWDisposable.Dispose();
      this._fadeLCWDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
      {
        if (active)
          return;
        input.FocusLevel = 0;
        ((Component) this._lcw).get_gameObject().SetActive(false);
      }));
    }

    [DebuggerHidden]
    private IEnumerator OpenLCWCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerChangeUI.\u003COpenLCWCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseLCWCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerChangeUI.\u003CCloseLCWCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }
  }
}
