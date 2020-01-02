// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CharaChangeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using GameLoadCharaFileSystem;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class CharaChangeUI : MenuUIBehaviour
  {
    private Dictionary<int, string> _prevCharaNames = new Dictionary<int, string>();
    private Dictionary<int, int> _prevCharaMapIDs = new Dictionary<int, int>();
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
    private BoolReactiveProperty _isActiveLCW = new BoolReactiveProperty(false);
    private GameCharaFileInfo[] _infos = new GameCharaFileInfo[4];
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button[] _charaButtons;
    [SerializeField]
    private RectTransform[] _elements;
    [SerializeField]
    private Text[] _charaTexts;
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
      for (int index = 0; index < this._charaButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaChangeUI.\u003COnBeforeStart\u003Ec__AnonStorey4 startCAnonStorey4 = new CharaChangeUI.\u003COnBeforeStart\u003Ec__AnonStorey4();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.\u0024this = this;
        Button charaButton = this._charaButtons[index];
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.id = index;
        // ISSUE: method pointer
        ((UnityEvent) charaButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey4, __methodptr(\u003C\u003Em__0)));
      }
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (Action<M0>) (x =>
      {
        ((Component) this._selectedImageTransform).get_gameObject().SetActiveIfDifferent(x != -1);
        if (!((Component) this._selectedImageTransform).get_gameObject().get_activeSelf())
          return;
        ((Transform) this._selectedImageTransform).set_localPosition(((Transform) this._elements[x]).get_localPosition());
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveLCWChangedAsObservable(), (Action<M0>) (active => this.SetActiveControlLCW(active)));
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__5)));
      this._lcw.onLoadItemFunc = (Action<GameCharaFileInfo>) (dat =>
      {
        string charaFileName = Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].CharaFileName;
        AgentActor agentActor;
        string str;
        if (Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value(), ref agentActor) && this._prevCharaNames.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value(), out str) && (str == charaFileName && !charaFileName.IsNullOrEmpty()))
          agentActor.ChaControl.chaFile.SaveCharaFile(agentActor.ChaControl.chaFile.charaFileName, byte.MaxValue, false);
        Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].CharaFileName = dat.FileName;
        if (this._prevCharaMapIDs[((ReactiveProperty<int>) this._selectedID).get_Value()] != Singleton<Manager.Map>.Instance.MapID)
        {
          if (!dat.FileName.IsNullOrEmpty())
            Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].MapID = Singleton<Manager.Map>.Instance.MapID;
          else
            Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].MapID = this._prevCharaMapIDs[((ReactiveProperty<int>) this._selectedID).get_Value()];
        }
        this._infos[((ReactiveProperty<int>) this._selectedID).get_Value()] = dat;
        this._charaTexts[((ReactiveProperty<int>) this._selectedID).get_Value()].set_text(dat.name ?? "-----");
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
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        CharaCustom.CharaCustom.modeNew = true;
        CharaCustom.CharaCustom.modeSex = (byte) 1;
        CharaCustom.CharaCustom.actEixt = (Action) null;
        CharaCustom.CharaCustom.nextScene = Singleton<Resources>.Instance.DefinePack.SceneNames.MapScene;
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "CharaCustom",
          isAdd = false,
          isFade = true
        }, false);
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
        ((ReactiveProperty<int>) this._selectedID).set_Value(-1);
        Time.set_timeScale(0.0f);
        List<string> stringList = ListPool<string>.Get();
        WorldData autoData = Singleton<Game>.Instance.Data.AutoData;
        if (autoData != null)
        {
          stringList.Add(autoData.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in autoData.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
        foreach (KeyValuePair<int, WorldData> world in Singleton<Game>.Instance.Data.WorldList)
        {
          stringList.Add(world.Value.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in world.Value.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
        Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData.AgentTable;
        for (int index = 0; index < 4; ++index)
        {
          AgentData agentData = agentTable[index];
          this._prevCharaNames[index] = agentData.CharaFileName;
          this._prevCharaMapIDs[index] = agentData.MapID;
          ChaFileControl chaFileControl = new ChaFileControl();
          if (!agentData.CharaFileName.IsNullOrEmpty() && chaFileControl.LoadCharaFile(agentData.CharaFileName, (byte) 1, false, true))
          {
            string empty = string.Empty;
            VoiceInfo.Param obj;
            string str = Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明";
            this._infos[index] = new GameCharaFileInfo()
            {
              name = chaFileControl.parameter.fullname,
              personality = str,
              voice = chaFileControl.parameter.personality,
              hair = chaFileControl.custom.hair.kind,
              birthMonth = (int) chaFileControl.parameter.birthMonth,
              birthDay = (int) chaFileControl.parameter.birthDay,
              strBirthDay = chaFileControl.parameter.strBirthDay,
              sex = (int) chaFileControl.parameter.sex,
              FullPath = string.Format("{0}chara/female/{1}.png", (object) UserData.Path, (object) agentData.CharaFileName),
              FileName = agentData.CharaFileName,
              gameRegistration = chaFileControl.gameinfo.gameRegistration,
              flavorState = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.flavorState),
              phase = chaFileControl.gameinfo.phase,
              normalSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.normalSkill),
              hSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.hSkill),
              favoritePlace = chaFileControl.gameinfo.favoritePlace,
              futanari = chaFileControl.parameter.futanari,
              data_uuid = chaFileControl.dataID,
              isInSaveData = stringList.Contains(Path.GetFileNameWithoutExtension(chaFileControl.charaFileName))
            };
          }
          else
            this._infos[index] = (GameCharaFileInfo) null;
          if (this._infos[index] != null)
            this._charaTexts[index].set_text(this._infos[index].name ?? "-----");
          else
            this._charaTexts[index].set_text("-----");
          ((Component) this._charaButtons[index]).get_gameObject().SetActiveIfDifferent(agentData.OpenState);
          if (((Component) this._charaButtons[index]).get_gameObject().get_activeSelf())
            ((Selectable) this._charaButtons[index]).set_interactable(agentData.PlayEnterScene || index == Singleton<Manager.Map>.Instance.AccessDeviceID);
        }
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
      return (IEnumerator) new CharaChangeUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaChangeUI.\u003CCloseCoroutine\u003Ec__Iterator1()
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
        if (!this._lcw.IsStartUp)
          this._lcw.Start();
        this._lcw.useDownload = true;
        int num = 0;
        foreach (GameCharaFileInfo info in this._infos)
        {
          if (info != null && !info.FileName.IsNullOrEmpty())
            ++num;
        }
        this._lcw.ReCreateList(true, num > 1);
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
      return (IEnumerator) new CharaChangeUI.\u003COpenLCWCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseLCWCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaChangeUI.\u003CCloseLCWCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }
  }
}
