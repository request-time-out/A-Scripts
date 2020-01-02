// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PlayerLookEditUI
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
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class PlayerLookEditUI : MenuUIBehaviour
  {
    private readonly string[] _localizeMale = new string[5]
    {
      "男",
      "Male",
      "Male",
      "Male",
      string.Empty
    };
    private readonly string[] _localizeFemale = new string[5]
    {
      "女",
      "Female",
      "Female",
      "Female",
      string.Empty
    };
    private readonly string[] _localizeFutanari = new string[5]
    {
      "（フタナリ）",
      "(Futanari)",
      "(Futanari)",
      "(Futanari)",
      string.Empty
    };
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button _charaButton;
    [SerializeField]
    private RectTransform _element;
    [SerializeField]
    private UnityEngine.UI.Text _charaText;
    [SerializeField]
    private RectTransform _selectedImageTransform;
    [SerializeField]
    private Button _charaCreateButton;
    [SerializeField]
    private Texture2D _texEmpty;
    [SerializeField]
    private GameObject _objPlayerParameterWindow;
    [SerializeField]
    private UnityEngine.UI.Text _txtPlayerCharaName;
    [SerializeField]
    private RawImage _riPlayerCard;
    [SerializeField]
    private UnityEngine.UI.Text _txtPlayerSex;
    private IConnectableObservable<int> _selectIDChange;
    private MenuUIBehaviour[] _menuUIList;
    private GameCharaFileInfo _info;
    private IDisposable _fadeDisposable;

    public IObservable<int> OnSelectIDChangedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((Component) this).get_gameObject()));
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
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (active => this.SetActiveControl(active)));
      // ISSUE: method pointer
      ((UnityEvent) this._charaButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (Action<M0>) (x =>
      {
        ((Component) this._selectedImageTransform).get_gameObject().SetActiveIfDifferent(x != 1);
        if (((Component) this._selectedImageTransform).get_gameObject().get_activeSelf())
          ((Transform) this._selectedImageTransform).set_localPosition(((Transform) this._element).get_localPosition());
        this._objPlayerParameterWindow.SetActiveIfDifferent(true);
        if (this._info == null)
          return;
        this._txtPlayerCharaName.set_text(this._info.name);
        this._riPlayerCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(this._info.FullPath), 0, 0, (TextureFormat) 5, false));
        int languageInt = Singleton<GameSystem>.Instance.languageInt;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this._info.sex != 0 ? this._localizeFemale[languageInt] : this._localizeMale[languageInt]);
        if (this._info.sex == 1 && this._info.futanari)
          stringBuilder.Append(this._localizeFutanari[languageInt]);
        this._txtPlayerSex.set_text(stringBuilder.ToString());
      }));
      this._objPlayerParameterWindow.SetActiveIfDifferent(false);
      this._txtPlayerCharaName.set_text("NoName");
      this._riPlayerCard.set_texture((Texture) null);
      this._txtPlayerSex.set_text(string.Empty);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      Button.ButtonClickedEvent onClick = this._charaCreateButton.get_onClick();
      // ISSUE: reference to a compiler-generated field
      if (PlayerLookEditUI.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        PlayerLookEditUI.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003COnBeforeStart\u003Em__5));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache0 = PlayerLookEditUI.\u003C\u003Ef__am\u0024cache0;
      ((UnityEvent) onClick).AddListener(fAmCache0);
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
      return (IEnumerator) new PlayerLookEditUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerLookEditUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }
  }
}
