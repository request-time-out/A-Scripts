// Decompiled with JetBrains decompiler
// Type: AIProject.TitleLoadScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using GameLoadCharaFileSystem;
using Illusion.Component;
using Illusion.Extensions;
using Manager;
using SceneAssist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class TitleLoadScene : BaseLoader
  {
    [SerializeField]
    private int drawFileNum = 3;
    [SerializeField]
    private InputFieldInfo worldNameInput = new InputFieldInfo();
    [SerializeField]
    private string initWorldName = "No Name";
    [SerializeField]
    private List<TitleLoadScene.ModeSelectUI> lstSelectUI = new List<TitleLoadScene.ModeSelectUI>();
    private List<TitleSaveItemInfo> lstSaveInfo = new List<TitleSaveItemInfo>();
    private string strD = string.Empty;
    private readonly string[] localizeIsLoad = new string[5]
    {
      "ロードしますか？",
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    private readonly string[] localizeIsGameStart = new string[5]
    {
      "ゲームを開始しますか？",
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    private readonly string[] localizeIsDelete = new string[5]
    {
      "削除してもいいですか？",
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    [Header("ロードUI")]
    [SerializeField]
    private GameObject objLoadRoot;
    [SerializeField]
    private Button btnLoadUIClose;
    [SerializeField]
    private TitleSaveItemInfo infoAutoItem;
    [SerializeField]
    private GameObject nodeSaveButton;
    [SerializeField]
    private GameObject objSaveContentParent;
    [Header("NewGameUI")]
    [SerializeField]
    private GameObject objNewGameRoot;
    [SerializeField]
    private Button btnNewGameBack;
    [SerializeField]
    private Button btnNewGameEntry;
    [Header("モード選択UI")]
    [SerializeField]
    private GameObject objModeSelectRoot;
    [Header("Character選択UI")]
    [SerializeField]
    private GameLoadCharaWindow lcwPlayer;
    [SerializeField]
    private GameLoadCharaWindow lcwFemale;
    [Header("確認")]
    public GameObject objTitleMain;
    public GameObject objMap;
    [SerializeField]
    private GameObject objTitleLoad;
    public TitleScene titleScene;
    public Action actionClose;
    private int proc;
    private bool isCoroutine;
    private bool isDialogDraw;
    private WorldData selectWorldData;
    private int language;

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TitleLoadScene.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnBack()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      if (this.actionClose != null)
        this.actionClose();
      Singleton<Scene>.Instance.UnLoad();
    }

    private void StartLoad()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TitleLoadScene.\u003CStartLoad\u003Ec__AnonStorey1 loadCAnonStorey1 = new TitleLoadScene.\u003CStartLoad\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      loadCAnonStorey1.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      loadCAnonStorey1.isAutoFileExists = Singleton<Game>.Instance.Data.AutoData != null;
      // ISSUE: reference to a compiler-generated field
      this.infoAutoItem.objSave.SetActiveIfDifferent(loadCAnonStorey1.isAutoFileExists);
      // ISSUE: reference to a compiler-generated field
      this.infoAutoItem.objInitialize.SetActiveIfDifferent(!loadCAnonStorey1.isAutoFileExists);
      // ISSUE: reference to a compiler-generated field
      if (loadCAnonStorey1.isAutoFileExists)
      {
        this.infoAutoItem.txtTitle.set_text(Singleton<Game>.Instance.Data.AutoData.Name);
        this.infoAutoItem.txtDay.set_text(Singleton<Game>.Instance.Data.AutoData.SaveTime.ToShortDateString());
        this.infoAutoItem.txtTime.set_text(Singleton<Game>.Instance.Data.AutoData.SaveTime.ToLongTimeString());
      }
      // ISSUE: reference to a compiler-generated field
      ((Selectable) this.infoAutoItem.btnEntry).set_interactable(loadCAnonStorey1.isAutoFileExists);
      // ISSUE: reference to a compiler-generated field
      this.infoAutoItem.isData = loadCAnonStorey1.isAutoFileExists;
      this.infoAutoItem.num = 0;
      // ISSUE: method pointer
      this.infoAutoItem.action.listActionEnter.Add(new UnityAction((object) loadCAnonStorey1, __methodptr(\u003C\u003Em__0)));
      // ISSUE: method pointer
      this.infoAutoItem.action.listActionExit.Add(new UnityAction((object) loadCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.infoAutoItem.btnEntry), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__2));
      // ISSUE: reference to a compiler-generated method
      UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) ObserveExtensions.ObserveEveryValueChanged<TitleLoadScene, bool>((M0) this, (Func<M0, M1>) new Func<TitleLoadScene, bool>(loadCAnonStorey1.\u003C\u003Em__3), (FrameCountType) 0, false), (Selectable) this.infoAutoItem.btnEntry);
      this.lstSaveInfo.Clear();
      this.lstSaveInfo.Add(this.infoAutoItem);
      List<Transform> transformList = new List<Transform>();
      for (int index = 0; index < this.objSaveContentParent.get_transform().get_childCount(); ++index)
        transformList.Add(this.objSaveContentParent.get_transform().GetChild(index));
      this.objSaveContentParent.get_transform().DetachChildren();
      using (List<Transform>.Enumerator enumerator = transformList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.Destroy((Object) enumerator.Current);
      }
      for (int key = 0; key < this.drawFileNum; ++key)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TitleLoadScene.\u003CStartLoad\u003Ec__AnonStorey2 loadCAnonStorey2 = new TitleLoadScene.\u003CStartLoad\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        loadCAnonStorey2.\u003C\u003Ef__ref\u00241 = loadCAnonStorey1;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.nodeSaveButton);
        gameObject.get_transform().SetParent(this.objSaveContentParent.get_transform(), false);
        // ISSUE: reference to a compiler-generated field
        loadCAnonStorey2.info = (TitleSaveItemInfo) gameObject.GetComponent<TitleSaveItemInfo>();
        // ISSUE: reference to a compiler-generated field
        if (!Object.op_Implicit((Object) loadCAnonStorey2.info))
        {
          Object.Destroy((Object) gameObject);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          loadCAnonStorey2.isFileExists = Singleton<Game>.Instance.Data.WorldList.ContainsKey(key);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          loadCAnonStorey2.info.objSave.SetActiveIfDifferent(loadCAnonStorey2.isFileExists);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          loadCAnonStorey2.info.objInitialize.SetActiveIfDifferent(!loadCAnonStorey2.isFileExists);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          loadCAnonStorey2.info.isData = loadCAnonStorey2.isFileExists;
          // ISSUE: reference to a compiler-generated field
          loadCAnonStorey2.info.num = key + 1;
          // ISSUE: reference to a compiler-generated field
          if (loadCAnonStorey2.isFileExists)
          {
            WorldData world = Singleton<Game>.Instance.Data.WorldList[key];
            // ISSUE: reference to a compiler-generated field
            loadCAnonStorey2.info.txtTitle.set_text(world.Name);
            // ISSUE: reference to a compiler-generated field
            loadCAnonStorey2.info.txtDay.set_text(world.SaveTime.ToShortDateString());
            // ISSUE: reference to a compiler-generated field
            loadCAnonStorey2.info.txtTime.set_text(world.SaveTime.ToLongTimeString());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          loadCAnonStorey2.info.action.listActionEnter.Add(new UnityAction((object) loadCAnonStorey2, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          loadCAnonStorey2.info.action.listActionExit.Add(new UnityAction((object) loadCAnonStorey2, __methodptr(\u003C\u003Em__1)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(loadCAnonStorey2.info.btnEntry), (Action<M0>) new Action<Unit>(loadCAnonStorey2.\u003C\u003Em__2));
          // ISSUE: reference to a compiler-generated field
          UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) ObserveExtensions.ObserveEveryValueChanged<TitleLoadScene, bool>((M0) this, (Func<M0, M1>) (_ => !Singleton<Scene>.Instance.IsNowLoadingFade), (FrameCountType) 0, false), (Selectable) loadCAnonStorey2.info.btnEntry);
          // ISSUE: reference to a compiler-generated field
          this.lstSaveInfo.Add(loadCAnonStorey2.info);
        }
      }
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnLoadUIClose), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__4));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnLoadUIClose), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      IObservable<Unit> observable1 = (IObservable<Unit>) Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((UnityEngine.Component) this), (Func<M0, bool>) (_ => Object.op_Equality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) && Object.op_Equality((Object) Singleton<Game>.Instance.Config, (Object) null))), (Func<M0, bool>) (_ => !Singleton<Scene>.Instance.IsNowLoadingFade));
      IObservable<Unit> observable2 = (IObservable<Unit>) Observable.Where<Unit>((IObservable<M0>) observable1, (Func<M0, bool>) (_ => Input.GetMouseButtonDown(1)));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) observable2, (Func<M0, bool>) new Func<Unit, bool>(loadCAnonStorey1.\u003C\u003Em__5)), (Func<M0, bool>) new Func<Unit, bool>(loadCAnonStorey1.\u003C\u003Em__6)), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__7));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) observable2, (Func<M0, bool>) new Func<Unit, bool>(loadCAnonStorey1.\u003C\u003Em__8)), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__9));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) observable2, (Func<M0, bool>) new Func<Unit, bool>(loadCAnonStorey1.\u003C\u003Em__A)), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__B));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) observable1, (Func<M0, bool>) new Func<Unit, bool>(loadCAnonStorey1.\u003C\u003Em__C)), (Func<M0, bool>) (_ => Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue))), (Action<M0>) new Action<Unit>(loadCAnonStorey1.\u003C\u003Em__D));
    }

    private void StartNewGame()
    {
      ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this.worldNameInput.input), (Action<M0>) (str =>
      {
        if (str.IsNullOrEmpty())
        {
          str = this.initWorldName;
          this.worldNameInput.input.set_text(str);
        }
        this.worldNameInput.textDummy.set_text(str);
        this.selectWorldData.Name = str;
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNewGameBack), (Action<M0>) (_ => this.BackToLoad()));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnNewGameBack), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNewGameEntry), (Func<M0, bool>) (_ => !Singleton<Scene>.Instance.IsFadeNow)), (Action<M0>) (_ =>
      {
        Debug.Log((object) this.selectWorldData.Name);
        this.objNewGameRoot.SetActiveIfDifferent(false);
        ((UnityEngine.Component) this.lcwFemale).get_gameObject().SetActiveIfDifferent(true);
        this.lcwFemale.ReCreateList(true, true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnNewGameEntry), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((UnityEngine.Component) this), (Func<M0, bool>) (_ => this.objNewGameRoot.get_activeSelf())), (Action<M0>) (_ =>
      {
        bool isFocused = this.worldNameInput.input.get_isFocused();
        this.worldNameInput.objDummy.SetActiveIfDifferent(!isFocused);
        ((Behaviour) this.worldNameInput.inputText).set_enabled(isFocused);
      }));
    }

    private void BackToLoad()
    {
      this.objLoadRoot.SetActiveIfDifferent(true);
      this.objNewGameRoot.SetActiveIfDifferent(false);
      Singleton<Game>.Instance.RemoveWorldData(this.selectWorldData.WorldID);
      this.selectWorldData = (WorldData) null;
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
    }

    private void StartStrongNewGameSelect()
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.lstSelectUI[0].button), (Action<M0>) (_ =>
      {
        this.selectWorldData.FreeMode = false;
        this.objModeSelectRoot.SetActiveIfDifferent(false);
        this.objNewGameRoot.SetActiveIfDifferent(true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      }));
      // ISSUE: method pointer
      this.lstSelectUI[0].action.listAction.Add(new UnityAction((object) this, __methodptr(\u003CStartStrongNewGameSelect\u003Em__F)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.lstSelectUI[1].button), (Action<M0>) (_ =>
      {
        this.selectWorldData.FreeMode = true;
        this.objModeSelectRoot.SetActiveIfDifferent(false);
        this.objNewGameRoot.SetActiveIfDifferent(true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      }));
      // ISSUE: method pointer
      this.lstSelectUI[1].action.listAction.Add(new UnityAction((object) this, __methodptr(\u003CStartStrongNewGameSelect\u003Em__11)));
    }

    private void StartCharacterSelect()
    {
      this.lcwPlayer.onLoadItemFunc = (Action<GameCharaFileInfo>) (_data =>
      {
        this.selectWorldData.PlayerData.CharaFileNames[_data.sex] = _data.FileName;
        this.selectWorldData.PlayerData.Sex = (byte) _data.sex;
        ChaFileControl.InitializeCharaFile(this.selectWorldData.AgentTable[0].CharaFileName, 1);
        this.SetWorldData(this.selectWorldData, false);
        this.GoToNextScene();
      });
      this.lcwPlayer.onCharaCreateClickAction = (Action<int>) (sex =>
      {
        CharaCustom.CharaCustom.modeNew = true;
        CharaCustom.CharaCustom.modeSex = (byte) sex;
        CharaCustom.CharaCustom.actEixt = (Action) (() =>
        {
          if (Scene.isGameEnd)
            return;
          this.AllObjectVisible(true);
          if (Object.op_Implicit((Object) this.objTitleMain))
          {
            this.lcwPlayer.ReCreateListOnly(true, true);
            if (Object.op_Implicit((Object) this.titleScene))
              this.titleScene.PlayBGM();
          }
          Scene.ActiveScene = Scene.GetScene(TitleScene.mapFileName);
        });
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "CharaCustom",
          isAdd = true,
          isFade = true,
          onLoad = (Action) (() =>
          {
            Scene.ActiveScene = Scene.GetScene("CharaCustom");
            this.AllObjectVisible(false);
          })
        }, true);
      });
      this.lcwPlayer.onClickRightFunc = (Action) (() =>
      {
        if (!Object.op_Equality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) || !Object.op_Equality((Object) Singleton<Game>.Instance.Config, (Object) null))
          return;
        this.selectWorldData.AgentTable[0].CharaFileName = string.Empty;
        this.selectWorldData.PlayerData.Sex = (byte) 0;
        ((UnityEngine.Component) this.lcwPlayer).get_gameObject().SetActiveIfDifferent(false);
        ((UnityEngine.Component) this.lcwFemale).get_gameObject().SetActiveIfDifferent(true);
        this.lcwFemale.ReCreateList(true, true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      });
      this.lcwFemale.onLoadItemFunc = (Action<GameCharaFileInfo>) (_data =>
      {
        this.selectWorldData.AgentTable[0].CharaFileName = _data.FileName;
        ((UnityEngine.Component) this.lcwFemale).get_gameObject().SetActiveIfDifferent(false);
        ((UnityEngine.Component) this.lcwPlayer).get_gameObject().SetActiveIfDifferent(true);
        this.lcwPlayer.ReCreateList(true, true);
      });
      this.lcwFemale.onCharaCreateClickAction = (Action<int>) (sex =>
      {
        CharaCustom.CharaCustom.modeNew = true;
        CharaCustom.CharaCustom.modeSex = (byte) sex;
        CharaCustom.CharaCustom.actEixt = (Action) (() =>
        {
          if (Scene.isGameEnd)
            return;
          this.AllObjectVisible(true);
          if (Object.op_Implicit((Object) this.objTitleMain))
          {
            this.lcwFemale.ReCreateListOnly(true, true);
            if (Object.op_Implicit((Object) this.titleScene))
              this.titleScene.PlayBGM();
          }
          Scene.ActiveScene = Scene.GetScene(TitleScene.mapFileName);
        });
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "CharaCustom",
          isAdd = true,
          isFade = true,
          onLoad = (Action) (() =>
          {
            Scene.ActiveScene = Scene.GetScene("CharaCustom");
            this.AllObjectVisible(false);
          })
        }, true);
      });
      this.lcwFemale.onClickRightFunc = (Action) (() =>
      {
        if (!Object.op_Equality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) || !Object.op_Equality((Object) Singleton<Game>.Instance.Config, (Object) null))
          return;
        ((UnityEngine.Component) this.lcwFemale).get_gameObject().SetActiveIfDifferent(false);
        this.objNewGameRoot.SetActiveIfDifferent(true);
        this.worldNameInput.input.set_text(this.selectWorldData.Name);
        this.worldNameInput.textDummy.set_text(this.selectWorldData.Name);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      });
      ((UnityEngine.Component) this.lcwPlayer).get_gameObject().SetActiveIfDifferent(false);
      ((UnityEngine.Component) this.lcwFemale).get_gameObject().SetActiveIfDifferent(false);
    }

    private void ChangeSeletModeSelectUI(int _drawIndex)
    {
      for (int index = 0; index < this.lstSelectUI.Count; ++index)
        this.lstSelectUI[index].objSelect.SetActiveIfDifferent(index == _drawIndex);
    }

    private void SetWorldData(WorldData _worldData, bool isAuto = false)
    {
      Singleton<Game>.Instance.WorldData = new WorldData();
      Singleton<Game>.Instance.WorldData.Copy(_worldData);
      Singleton<Game>.Instance.IsAuto = isAuto;
    }

    private void GoToNextScene()
    {
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = Singleton<Resources>.Instance.DefinePack.SceneNames.MapScene,
        isAdd = false,
        isFade = true,
        isAsync = true,
        isDrawProgressBar = false
      }, false);
      TitleScene.startmode = 0;
    }

    private void AllObjectVisible(bool _visible)
    {
      if (Object.op_Implicit((Object) this.objTitleMain) && this.objTitleMain != null)
        this.objTitleMain.SetActiveIfDifferent(_visible);
      if (Object.op_Implicit((Object) this.objMap) && this.objMap != null)
        this.objMap.SetActiveIfDifferent(_visible);
      if (!Object.op_Implicit((Object) this.objTitleLoad) || this.objTitleLoad == null)
        return;
      this.objTitleLoad.SetActiveIfDifferent(_visible);
    }

    [Serializable]
    public class ModeSelectUI
    {
      public Button button;
      public GameObject objSelect;
      public PointerEnterAction action;
    }
  }
}
