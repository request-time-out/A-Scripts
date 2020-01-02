// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CharaMigrateUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.Definitions;
using AIProject.SaveData;
using GameLoadCharaFileSystem;
using Illusion.Component.UI;
using Illusion.Extensions;
using Manager;
using SceneAssist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class CharaMigrateUI : MenuUIBehaviour
  {
    private Dictionary<int, int> _prevCharaMapIDs = new Dictionary<int, int>();
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
    private BoolReactiveProperty _isActiveMapSelect = new BoolReactiveProperty(false);
    private List<MigrateMapSelectNodeUI> _nodes = new List<MigrateMapSelectNodeUI>();
    private GameCharaFileInfo[] _infos = new GameCharaFileInfo[4];
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button[] _charaButtons;
    [SerializeField]
    private Button[] _charaArrowButtons;
    [SerializeField]
    private RectTransform[] _elements;
    [SerializeField]
    private Text[] _charaTexts;
    [SerializeField]
    private RectTransform _selectedImageTransform;
    [SerializeField]
    private GameObject _objFemaleParameterWindow;
    [SerializeField]
    private Text _txtFemaleCharaName;
    [SerializeField]
    private RawImage _riFemaleCard;
    [SerializeField]
    private SpriteChangeCtrl[] _sccStateOfProgress;
    [SerializeField]
    private Toggle[] _tglFemaleStateSelects;
    [SerializeField]
    private PointerEnterExitAction[] _actionStateSelects;
    [SerializeField]
    private GameObject[] _objFemaleStateSelectSels;
    [SerializeField]
    private GameObject[] _objFemaleParameterRoots;
    [SerializeField]
    private Text _txtLifeStyle;
    [SerializeField]
    private Text _txtGirlPower;
    [SerializeField]
    private Text _txtTrust;
    [SerializeField]
    private Text _txtHumanNature;
    [SerializeField]
    private Text _txtInstinct;
    [SerializeField]
    private Text _txtHentai;
    [SerializeField]
    private Text _txtVigilance;
    [SerializeField]
    private Text _txtSocial;
    [SerializeField]
    private Text _txtDarkness;
    [SerializeField]
    private Text[] _txtNormalSkillSlots;
    [SerializeField]
    private Text[] _txtHSkillSlots;
    [SerializeField]
    private GameObject _objMapSelectWindow;
    [SerializeField]
    private CanvasGroup _mapSelectCanvasGroup;
    [SerializeField]
    private Text _selectMapText;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private RectTransform _selectedMapImageTransform;
    [SerializeField]
    private MigrateMapSelectNodeUI _mapSelectNode;
    private IConnectableObservable<int> _selectIDChange;
    private IConnectableObservable<bool> _activeChangeMapSelect;
    private MenuUIBehaviour[] _menuUIList;
    private int _femaleParameterSelectNum;
    private IDisposable _fadeDisposable;
    private IDisposable _fadeMapSelectDisposable;

    public IObservable<int> OnSelectIDChangedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((UnityEngine.Component) this).get_gameObject()));
        this._selectIDChange.Connect();
      }
      return (IObservable<int>) this._selectIDChange;
    }

    public IObservable<bool> OnActiveMapSelectChangedAsObservable()
    {
      if (this._activeChangeMapSelect == null)
      {
        this._activeChangeMapSelect = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.Where<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActiveMapSelect, ((UnityEngine.Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())));
        this._activeChangeMapSelect.Connect();
      }
      return (IObservable<bool>) this._activeChangeMapSelect;
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
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (System.Action<M0>) (active => this.SetActiveControl(active)));
      for (int index = 0; index < this._charaButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey4 startCAnonStorey4 = new CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey4();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.\u0024this = this;
        Button charaButton = this._charaButtons[index];
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.id = index;
        // ISSUE: method pointer
        ((UnityEvent) charaButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey4, __methodptr(\u003C\u003Em__0)));
      }
      for (int index = 0; index < this._charaArrowButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey5 startCAnonStorey5 = new CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey5();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey5.\u0024this = this;
        Button charaArrowButton = this._charaArrowButtons[index];
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey5.id = index;
        // ISSUE: method pointer
        ((UnityEvent) charaArrowButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__0)));
      }
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (System.Action<M0>) (x =>
      {
        ((UnityEngine.Component) this._selectedImageTransform).get_gameObject().SetActiveIfDifferent(x != -1);
        if (((UnityEngine.Component) this._selectedImageTransform).get_gameObject().get_activeSelf())
          ((Transform) this._selectedImageTransform).set_localPosition(((Transform) this._elements[x]).get_localPosition());
        AgentData agentData;
        if (Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(x, out agentData))
        {
          ((ReactiveProperty<bool>) this._isActiveMapSelect).set_Value(x != -1);
          AssetBundleInfo assetBundleInfo;
          if (Singleton<Resources>.Instance.Map.MapList.TryGetValue(agentData.MapID, out assetBundleInfo))
          {
            this._selectMapText.set_text((string) assetBundleInfo.name);
            ((UnityEngine.Component) this._selectedMapImageTransform).get_gameObject().SetActiveIfDifferent(agentData.MapID != -1);
            if (((UnityEngine.Component) this._selectedMapImageTransform).get_gameObject().get_activeSelf())
              ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => ((Transform) this._selectedMapImageTransform).set_localPosition(((UnityEngine.Component) this._nodes[agentData.MapID]).get_transform().get_localPosition())));
          }
          else
            this._selectMapText.set_text("-----");
        }
        else
          this._selectMapText.set_text("-----");
        GameCharaFileInfo element = this._infos.GetElement<GameCharaFileInfo>(x);
        if (element == null)
          return;
        if (!element.FullPath.IsNullOrEmpty())
        {
          bool activeSelf = this._objFemaleParameterWindow.get_activeSelf();
          this._objFemaleParameterWindow.SetActiveIfDifferent(true);
          if (!activeSelf)
          {
            this._femaleParameterSelectNum = 0;
            this._tglFemaleStateSelects[0].SetIsOnWithoutCallback(true);
            for (int index = 0; index < this._objFemaleParameterRoots.Length; ++index)
              this._objFemaleParameterRoots[index].SetActiveIfDifferent(index == 0);
          }
          this._txtFemaleCharaName.set_text(element.name);
          this._riFemaleCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(element.FullPath), 0, 0, (TextureFormat) 5, false));
          for (int index = 0; index < this._sccStateOfProgress.Length; ++index)
            this._sccStateOfProgress[index].OnChangeValue(element.phase >= index ? 1 : 0);
          string str1 = string.Empty;
          str1 = !Lifestyle.LifestyleName.TryGetValue(element.lifeStyle, out str1) ? "--------------------" : (element.lifeStyle != 4 ? str1 : "E・シーカー");
          this._txtLifeStyle.set_text(str1);
          this._txtGirlPower.set_text(element.flavorState[0].ToString());
          this._txtTrust.set_text(element.flavorState[1].ToString());
          this._txtHumanNature.set_text(element.flavorState[2].ToString());
          this._txtInstinct.set_text(element.flavorState[3].ToString());
          this._txtHentai.set_text(element.flavorState[4].ToString());
          this._txtVigilance.set_text(element.flavorState[5].ToString());
          this._txtSocial.set_text(element.flavorState[7].ToString());
          this._txtDarkness.set_text(element.flavorState[6].ToString());
          for (int key = 0; key < this._txtNormalSkillSlots.Length; ++key)
          {
            string str2 = "--------------------";
            if (element.normalSkill.ContainsKey(key))
              str2 = Singleton<Resources>.Instance.GameInfo.GetItem(16, element.normalSkill[key])?.Name ?? "--------------------";
            this._txtNormalSkillSlots[key].set_text(str2);
          }
          for (int key = 0; key < this._txtHSkillSlots.Length; ++key)
          {
            string str2 = "--------------------";
            if (element.hSkill.ContainsKey(key))
              str2 = Singleton<Resources>.Instance.GameInfo.GetItem(17, element.hSkill[key])?.Name ?? "--------------------";
            this._txtHSkillSlots[key].set_text(str2);
          }
        }
        else
        {
          this._txtFemaleCharaName.set_text(string.Empty);
          for (int index = 0; index < this._sccStateOfProgress.Length; ++index)
            this._sccStateOfProgress[index].OnChangeValue(element.phase >= index ? 1 : 0);
          this._txtLifeStyle.set_text(string.Empty);
          this._txtGirlPower.set_text(string.Empty);
          this._txtTrust.set_text(string.Empty);
          this._txtHumanNature.set_text(string.Empty);
          this._txtInstinct.set_text(string.Empty);
          this._txtHentai.set_text(string.Empty);
          this._txtVigilance.set_text(string.Empty);
          this._txtSocial.set_text(string.Empty);
          this._txtDarkness.set_text(string.Empty);
          for (int index = 0; index < this._txtNormalSkillSlots.Length; ++index)
          {
            string str = "--------------------";
            this._txtNormalSkillSlots[index].set_text(str);
          }
          for (int index = 0; index < this._txtHSkillSlots.Length; ++index)
          {
            string str = "--------------------";
            this._txtHSkillSlots[index].set_text(str);
          }
        }
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveMapSelectChangedAsObservable(), (System.Action<M0>) (active => this.SetActiveControlMapSelect(active)));
      this._objFemaleParameterWindow.SetActiveIfDifferent(false);
      this._txtFemaleCharaName.set_text("NoName");
      this._riFemaleCard.set_texture((Texture) null);
      for (int index = 0; index < this._sccStateOfProgress.Length; ++index)
        this._sccStateOfProgress[index].OnChangeValue(index != 0 ? 0 : 1);
      for (int index = 0; index < this._tglFemaleStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey7 startCAnonStorey7 = new CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey7();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey7.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey7.sel = index;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this._tglFemaleStateSelects[index].onValueChanged), (Func<M0, bool>) new Func<bool, bool>(startCAnonStorey7.\u003C\u003Em__0)), (System.Action<M0>) new System.Action<bool>(startCAnonStorey7.\u003C\u003Em__1));
      }
      for (int index = 0; index < this._actionStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey8 startCAnonStorey8 = new CharaMigrateUI.\u003COnBeforeStart\u003Ec__AnonStorey8();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey8.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey8.sel = index;
        // ISSUE: method pointer
        this._actionStateSelects[index].listActionEnter.Add(new UnityAction((object) startCAnonStorey8, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        this._actionStateSelects[index].listActionExit.Add(new UnityAction((object) startCAnonStorey8, __methodptr(\u003C\u003Em__1)));
      }
      for (int index = 0; index < this._objFemaleStateSelectSels.Length; ++index)
        this._objFemaleStateSelectSels[index].SetActiveIfDifferent(false);
      this._txtLifeStyle.set_text(string.Empty);
      this._txtGirlPower.set_text("0");
      this._txtTrust.set_text("0");
      this._txtHumanNature.set_text("0");
      this._txtInstinct.set_text("0");
      this._txtHentai.set_text("0");
      this._txtVigilance.set_text("0");
      this._txtSocial.set_text("0");
      this._txtDarkness.set_text("0");
      for (int index = 0; index < this._txtNormalSkillSlots.Length; ++index)
        this._txtNormalSkillSlots[index].set_text("--------------------");
      for (int index = 0; index < this._txtHSkillSlots.Length; ++index)
        this._txtHSkillSlots[index].set_text("--------------------");
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__5)));
      ((UnityEngine.Component) this._selectedImageTransform).get_gameObject().SetActive(false);
      ((UnityEngine.Component) this._selectedMapImageTransform).get_gameObject().SetActive(false);
    }

    private void Close()
    {
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        Time.set_timeScale(0.0f);
        Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData.AgentTable;
        for (int index1 = 0; index1 < 4; ++index1)
        {
          AgentData agentData = agentTable[index1];
          Dictionary<int, int> prevCharaMapIds = this._prevCharaMapIDs;
          int index2 = index1;
          int? prevMapId = agentData.PrevMapID;
          int num = !prevMapId.HasValue ? agentData.MapID : prevMapId.Value;
          prevCharaMapIds[index2] = num;
          ChaFileControl chaFileControl = new ChaFileControl();
          if (!agentData.CharaFileName.IsNullOrEmpty() && chaFileControl.LoadCharaFile(agentData.CharaFileName, (byte) 1, false, true))
          {
            string empty = string.Empty;
            VoiceInfo.Param obj;
            string str = Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明";
            this._infos[index1] = new GameCharaFileInfo()
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
              lifeStyle = chaFileControl.gameinfo.lifestyle,
              data_uuid = chaFileControl.dataID
            };
          }
          else
            this._infos[index1] = (GameCharaFileInfo) null;
          if (this._infos[index1] != null)
            this._charaTexts[index1].set_text(this._infos[index1].name ?? "-----");
          else
            this._charaTexts[index1].set_text("-----");
          ((UnityEngine.Component) this._charaButtons[index1]).get_gameObject().SetActiveIfDifferent(agentData.OpenState);
          if (((UnityEngine.Component) this._charaButtons[index1]).get_gameObject().get_activeSelf())
            ((Selectable) this._charaButtons[index1]).set_interactable(agentData.PlayEnterScene && !agentData.CharaFileName.IsNullOrEmpty());
          ((UnityEngine.Component) this._charaArrowButtons[index1]).get_gameObject().SetActiveIfDifferent(agentData.OpenState && agentData.PlayEnterScene && !agentData.CharaFileName.IsNullOrEmpty());
          this._objFemaleParameterWindow.SetActiveIfDifferent(false);
          ((ReactiveProperty<int>) this._selectedID).set_Value(-1);
          ((ReactiveProperty<bool>) this._isActiveMapSelect).set_Value(false);
        }
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.OpenCoroutine();
      }
      else
      {
        ((ReactiveProperty<int>) this._selectedID).set_Value(-1);
        ((ReactiveProperty<bool>) this._isActiveMapSelect).set_Value(false);
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.CloseCoroutine();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaMigrateUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaMigrateUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SetActiveControlMapSelect(bool active)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CharaMigrateUI.\u003CSetActiveControlMapSelect\u003Ec__AnonStoreyD selectCAnonStoreyD = new CharaMigrateUI.\u003CSetActiveControlMapSelect\u003Ec__AnonStoreyD();
      // ISSUE: reference to a compiler-generated field
      selectCAnonStoreyD.active = active;
      // ISSUE: reference to a compiler-generated field
      selectCAnonStoreyD.\u0024this = this;
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      // ISSUE: reference to a compiler-generated field
      if (selectCAnonStoreyD.active)
      {
        foreach (MigrateMapSelectNodeUI node in this._nodes)
        {
          if (!Object.op_Equality((Object) node, (Object) null))
            Object.Destroy((Object) ((UnityEngine.Component) node).get_gameObject());
        }
        this._nodes.Clear();
        using (Dictionary<int, AssetBundleInfo>.Enumerator enumerator = Singleton<Resources>.Instance.Map.MapList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, AssetBundleInfo> current = enumerator.Current;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CharaMigrateUI.\u003CSetActiveControlMapSelect\u003Ec__AnonStoreyC selectCAnonStoreyC = new CharaMigrateUI.\u003CSetActiveControlMapSelect\u003Ec__AnonStoreyC();
            // ISSUE: reference to a compiler-generated field
            selectCAnonStoreyC.\u003C\u003Ef__ref\u002413 = selectCAnonStoreyD;
            // ISSUE: reference to a compiler-generated field
            selectCAnonStoreyC.node = (MigrateMapSelectNodeUI) ((GameObject) Object.Instantiate<GameObject>((M0) ((UnityEngine.Component) this._mapSelectNode).get_gameObject(), (Transform) this._scrollRect.get_content(), false)).GetComponent<MigrateMapSelectNodeUI>();
            // ISSUE: reference to a compiler-generated field
            selectCAnonStoreyC.id = current.Key;
            // ISSUE: reference to a compiler-generated field
            selectCAnonStoreyC.mapName = (string) current.Value.name;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            selectCAnonStoreyC.node.Text.set_text(selectCAnonStoreyC.mapName);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method pointer
            ((UnityEvent) selectCAnonStoreyC.node.Button.get_onClick()).AddListener(new UnityAction((object) selectCAnonStoreyC, __methodptr(\u003C\u003Em__0)));
            // ISSUE: reference to a compiler-generated field
            ((UnityEngine.Component) selectCAnonStoreyC.node).get_gameObject().SetActiveIfDifferent(true);
            // ISSUE: reference to a compiler-generated field
            this._nodes.Add(selectCAnonStoreyC.node);
          }
        }
        // ISSUE: reference to a compiler-generated field
        selectCAnonStoreyD.coroutine = this.OpenMapSelectCoroutine();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        selectCAnonStoreyD.coroutine = this.CloseMapSelectCoroutine();
      }
      if (this._fadeMapSelectDisposable != null)
        this._fadeMapSelectDisposable.Dispose();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this._fadeMapSelectDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(selectCAnonStoreyD.\u003C\u003Em__0), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)), new System.Action(selectCAnonStoreyD.\u003C\u003Em__1));
    }

    private void OnSelectMap(int id, string mapName)
    {
      int mapId = Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].MapID;
      if (mapId == id)
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
      }
      else
      {
        AgentActor agentActor;
        int num;
        if (Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value(), ref agentActor) && this._prevCharaMapIDs.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value(), out num) && num != mapId)
          agentActor.ChaControl.chaFile.SaveCharaFile(agentActor.ChaControl.chaFile.charaFileName, byte.MaxValue, false);
        if (Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].PrevMapID.HasValue)
        {
          int? prevMapId = Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].PrevMapID;
          if ((prevMapId.GetValueOrDefault() != id ? 0 : (prevMapId.HasValue ? 1 : 0)) != 0)
            Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].PrevMapID = new int?();
        }
        else
          Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].PrevMapID = new int?(this._prevCharaMapIDs[((ReactiveProperty<int>) this._selectedID).get_Value()]);
        Singleton<Game>.Instance.WorldData.AgentTable[((ReactiveProperty<int>) this._selectedID).get_Value()].MapID = id;
        this._selectMapText.set_text(mapName);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
      }
    }

    [DebuggerHidden]
    private IEnumerator OpenMapSelectCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaMigrateUI.\u003COpenMapSelectCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseMapSelectCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaMigrateUI.\u003CCloseMapSelectCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }
  }
}
