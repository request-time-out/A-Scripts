// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CharaLookEditUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

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

namespace AIProject.UI
{
  public class CharaLookEditUI : MenuUIBehaviour
  {
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
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
    private Button _charaCreateButton;
    [SerializeField]
    private Texture2D _texEmpty;
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
    private IConnectableObservable<int> _selectIDChange;
    private MenuUIBehaviour[] _menuUIList;
    private int _femaleParameterSelectNum;
    private IDisposable _fadeDisposable;

    public IObservable<int> OnSelectIDChangedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((UnityEngine.Component) this).get_gameObject()));
        this._selectIDChange.Connect();
      }
      return (IObservable<int>) this._selectIDChange;
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
        CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey2 startCAnonStorey2 = new CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey2.\u0024this = this;
        Button charaButton = this._charaButtons[index];
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey2.id = index;
        // ISSUE: method pointer
        ((UnityEvent) charaButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey2, __methodptr(\u003C\u003Em__0)));
      }
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (System.Action<M0>) (x =>
      {
        ((Selectable) this._charaCreateButton).set_interactable(x != -1);
        ((UnityEngine.Component) this._selectedImageTransform).get_gameObject().SetActiveIfDifferent(x != -1);
        if (((UnityEngine.Component) this._selectedImageTransform).get_gameObject().get_activeSelf())
          ((Transform) this._selectedImageTransform).set_localPosition(((Transform) this._elements[x]).get_localPosition());
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
          string empty = string.Empty;
          this._txtLifeStyle.set_text(!Lifestyle.LifestyleName.TryGetValue(element.lifeStyle, out empty) ? "--------------------" : (element.lifeStyle != 4 ? empty : "E・シーカー"));
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
            string str = "--------------------";
            if (element.normalSkill.ContainsKey(key))
              str = Singleton<Resources>.Instance.GameInfo.GetItem(16, element.normalSkill[key])?.Name ?? "--------------------";
            this._txtNormalSkillSlots[key].set_text(str);
          }
          for (int key = 0; key < this._txtHSkillSlots.Length; ++key)
          {
            string str = "--------------------";
            if (element.hSkill.ContainsKey(key))
              str = Singleton<Resources>.Instance.GameInfo.GetItem(17, element.hSkill[key])?.Name ?? "--------------------";
            this._txtHSkillSlots[key].set_text(str);
          }
        }
        else
        {
          this._txtFemaleCharaName.set_text(string.Empty);
          this._riFemaleCard.set_texture((Texture) this._texEmpty);
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
      this._objFemaleParameterWindow.SetActiveIfDifferent(false);
      this._txtFemaleCharaName.set_text("NoName");
      this._riFemaleCard.set_texture((Texture) null);
      for (int index = 0; index < this._sccStateOfProgress.Length; ++index)
        this._sccStateOfProgress[index].OnChangeValue(index != 0 ? 0 : 1);
      for (int index = 0; index < this._tglFemaleStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey3 startCAnonStorey3 = new CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey3.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey3.sel = index;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this._tglFemaleStateSelects[index].onValueChanged), (Func<M0, bool>) new Func<bool, bool>(startCAnonStorey3.\u003C\u003Em__0)), (System.Action<M0>) new System.Action<bool>(startCAnonStorey3.\u003C\u003Em__1));
      }
      for (int index = 0; index < this._actionStateSelects.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey4 startCAnonStorey4 = new CharaLookEditUI.\u003COnBeforeStart\u003Ec__AnonStorey4();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey4.sel = index;
        // ISSUE: method pointer
        this._actionStateSelects[index].listActionEnter.Add(new UnityAction((object) startCAnonStorey4, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        this._actionStateSelects[index].listActionExit.Add(new UnityAction((object) startCAnonStorey4, __methodptr(\u003C\u003Em__1)));
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
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      // ISSUE: method pointer
      ((UnityEvent) this._charaCreateButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      ((Selectable) this._charaCreateButton).set_interactable(false);
      ((UnityEngine.Component) this._selectedImageTransform).get_gameObject().SetActive(false);
    }

    private void Close()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        ((ReactiveProperty<int>) this._selectedID).set_Value(-1);
        Time.set_timeScale(0.0f);
        Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData.AgentTable;
        for (int key = 0; key < 4; ++key)
        {
          AgentData agentData = agentTable[key];
          ChaFileControl chaFileControl = new ChaFileControl();
          if (!agentData.CharaFileName.IsNullOrEmpty() && chaFileControl.LoadCharaFile(agentData.CharaFileName, (byte) 1, false, true))
          {
            string empty = string.Empty;
            VoiceInfo.Param obj;
            string str = Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明";
            this._infos[key] = new GameCharaFileInfo()
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
            this._infos[key] = (GameCharaFileInfo) null;
          if (this._infos[key] != null)
            this._charaTexts[key].set_text(this._infos[key].name ?? "-----");
          else
            this._charaTexts[key].set_text("-----");
          this._objFemaleParameterWindow.SetActiveIfDifferent(false);
          ((UnityEngine.Component) this._charaButtons[key]).get_gameObject().SetActiveIfDifferent(agentData.OpenState);
          if (((UnityEngine.Component) this._charaButtons[key]).get_gameObject().get_activeSelf())
            ((Selectable) this._charaButtons[key]).set_interactable(agentTable.ContainsKey(key) && !agentTable[key].CharaFileName.IsNullOrEmpty());
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
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaLookEditUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaLookEditUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }
  }
}
