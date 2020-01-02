// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameLoadCharaWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Illusion.Component.UI;
using Illusion.Extensions;
using Manager;
using SceneAssist;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameLoadCharaFileSystem
{
  public class GameLoadCharaWindow : MonoBehaviour
  {
    [SerializeField]
    private bool _useMale;
    [SerializeField]
    private bool _useFemale;
    [SerializeField]
    private bool _useMyData;
    [SerializeField]
    private bool _useDownload;
    [SerializeField]
    private bool _addFirstEmpty;
    [SerializeField]
    private int _windowType;
    [SerializeField]
    private GameLoadCharaListCtrl _listCtrl;
    [SerializeField]
    private GameObject objPlayerTitleMenu;
    [SerializeField]
    private GameObject objFemaleTitleMenu;
    [SerializeField]
    private SpriteChangeCtrl sccFemaleTitleImageIcon;
    [SerializeField]
    private GameObject objSexSelect;
    [SerializeField]
    private Toggle tglMale;
    [SerializeField]
    private GameObject objMaleSelect;
    [SerializeField]
    private PointerEnterExitAction actionMaleSelect;
    [SerializeField]
    private Toggle tglFemale;
    [SerializeField]
    private GameObject objFemaleSelect;
    [SerializeField]
    private PointerEnterExitAction actionFemaleSelect;
    [SerializeField]
    private Button btnCharacterCreation;
    [SerializeField]
    private Button btnEntry;
    [SerializeField]
    private GameObject objEntrySelect;
    [SerializeField]
    private PointerEnterExitAction actionEntry;
    [SerializeField]
    private Button btnClose;
    [SerializeField]
    private bool _hideClose;
    [SerializeField]
    private bool _hideCharacterCreation;
    public GameLoadCharaWindow.OnCloseWindowFunc onCloseWindowFunc;
    public Action<GameCharaFileInfo> onLoadItemFunc;
    public Action onClickRightFunc;
    public int femaleNum;
    public Action<int> onCharaCreateClickAction;
    private List<GameCharaFileInfo> lstMaleCharaFileInfo;
    private List<GameCharaFileInfo> lstFemaleCharaFileInfo;
    private BoolReactiveProperty selectReactive;
    private int selectSex;

    public GameLoadCharaWindow()
    {
      base.\u002Ector();
      this.onChangeItemFunc = (GameLoadCharaListCtrl.OnChangeItemFunc) null;
    }

    public bool useMale
    {
      get
      {
        return this._useMale;
      }
    }

    public bool useFemale
    {
      get
      {
        return this._useFemale;
      }
    }

    public bool useMyData
    {
      get
      {
        return this._useMyData;
      }
    }

    public bool useDownload
    {
      set
      {
        this._useDownload = value;
      }
      get
      {
        return this._useDownload;
      }
    }

    public bool addFirstEmpty
    {
      set
      {
        this._addFirstEmpty = value;
      }
      get
      {
        return this._addFirstEmpty;
      }
    }

    public int windowType
    {
      get
      {
        return this._windowType;
      }
    }

    public GameLoadCharaListCtrl listCtrl
    {
      get
      {
        return this._listCtrl;
      }
    }

    public bool hideClose
    {
      get
      {
        return this._hideClose;
      }
      set
      {
        if (!Object.op_Implicit((Object) this.btnClose))
          return;
        this._hideClose = value;
        ((UnityEngine.Component) this.btnClose).get_gameObject().SetActiveIfDifferent(!value);
      }
    }

    public bool hideCharacterCreation
    {
      get
      {
        return this._hideCharacterCreation;
      }
      set
      {
        if (!Object.op_Implicit((Object) this.btnCharacterCreation))
          return;
        this._hideCharacterCreation = value;
        ((UnityEngine.Component) this.btnCharacterCreation).get_gameObject().SetActiveIfDifferent(!value);
      }
    }

    public GameLoadCharaListCtrl.OnChangeItemFunc onChangeItemFunc
    {
      set
      {
        if (!Object.op_Inequality((Object) null, (Object) this.listCtrl))
          return;
        this.listCtrl.onChangeItemFunc = value;
      }
    }

    public bool IsStartUp { get; private set; }

    public void InitCharaList(bool enableFirstEmpty = true)
    {
      this.lstMaleCharaFileInfo = GameCharaFileInfoAssist.CreateCharaFileInfoList(this.useMale, false, this.useMyData, this.useDownload, enableFirstEmpty && this.addFirstEmpty);
      this.lstFemaleCharaFileInfo = GameCharaFileInfoAssist.CreateCharaFileInfoList(false, this.useFemale, this.useMyData, this.useDownload, enableFirstEmpty && this.addFirstEmpty);
    }

    private void CreateCharaList(List<GameCharaFileInfo> _lst, bool _isSelectInfoClear = false)
    {
      this.listCtrl.ClearList();
      this.listCtrl.AddList(_lst);
      this.listCtrl.Create(_isSelectInfoClear);
    }

    private void CreateCharaListViewOnly(List<GameCharaFileInfo> _lst, bool _isSelectInfoClear = false)
    {
      this.listCtrl.ClearList();
      this.listCtrl.AddList(_lst);
      this.listCtrl.CreateListView(_isSelectInfoClear);
    }

    public void UpdateWindow(int _type, bool _isCreateList = true, bool _isSelectInfoClear = false)
    {
      this._windowType = _type;
      this.objEntrySelect.SetActiveIfDifferent(false);
      if (this.windowType == 0)
      {
        this.objPlayerTitleMenu.SetActiveIfDifferent(true);
        this.objFemaleTitleMenu.SetActiveIfDifferent(false);
        this.objSexSelect.SetActiveIfDifferent(true);
        this.objMaleSelect.SetActiveIfDifferent(false);
        this.objFemaleSelect.SetActiveIfDifferent(false);
        if (_isCreateList)
          this.CreateCharaList(!this.tglMale.get_isOn() ? this.lstFemaleCharaFileInfo : this.lstMaleCharaFileInfo, _isSelectInfoClear);
      }
      else if (this.windowType == 1)
      {
        this.selectSex = 1;
        this.objPlayerTitleMenu.SetActiveIfDifferent(false);
        this.objFemaleTitleMenu.SetActiveIfDifferent(true);
        this.sccFemaleTitleImageIcon.OnChangeValue(this.femaleNum);
        this.objSexSelect.SetActiveIfDifferent(false);
        if (_isCreateList)
          this.CreateCharaList(this.lstFemaleCharaFileInfo, _isSelectInfoClear);
      }
      if (!_isSelectInfoClear || this.selectReactive == null)
        return;
      ((ReactiveProperty<bool>) this.selectReactive).set_Value(false);
    }

    public void UpdateWindow(bool _isCreateList = true, bool _isSelectInfoClear = false)
    {
      if (this.windowType == 0)
      {
        if (_isCreateList)
          this.CreateCharaListViewOnly(!this.tglMale.get_isOn() ? this.lstFemaleCharaFileInfo : this.lstMaleCharaFileInfo, _isSelectInfoClear);
      }
      else if (this.windowType == 1 && _isCreateList)
        this.CreateCharaListViewOnly(this.lstFemaleCharaFileInfo, _isSelectInfoClear);
      if (!_isSelectInfoClear || this.selectReactive == null)
        return;
      ((ReactiveProperty<bool>) this.selectReactive).set_Value(false);
    }

    public void ReCreateList(bool _isSelectInfoClear, bool enableFirstEmpty = true)
    {
      this.InitCharaList(enableFirstEmpty);
      this.tglMale.SetIsOnWithoutCallback(true);
      this.tglFemale.SetIsOnWithoutCallback(false);
      this.selectSex = 0;
      this._listCtrl.InitSort();
      this.UpdateWindow(this.windowType, true, _isSelectInfoClear);
    }

    public void ReCreateListOnly(bool _isSelectInfoClear, bool enableFirstEmpty = true)
    {
      this.InitCharaList(enableFirstEmpty);
      this.UpdateWindow(true, _isSelectInfoClear);
    }

    public void Awake()
    {
    }

    public void Start()
    {
      if (this.IsStartUp)
        return;
      this.InitCharaList(true);
      if (Object.op_Implicit((Object) this.btnEntry))
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnEntry), (Func<M0, bool>) (_ => !Singleton<Scene>.Instance.IsNowLoadingFade)), (Action<M0>) (_ =>
        {
          if (this.onLoadItemFunc != null)
            this.onLoadItemFunc(this.listCtrl.GetNowSelectCard());
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        }));
      // ISSUE: method pointer
      this.actionEntry.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      // ISSUE: method pointer
      this.actionEntry.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this.UpdateWindow(this.windowType, true, false);
      if (Object.op_Implicit((Object) this.tglMale))
      {
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglMale.onValueChanged), (Func<M0, bool>) (_ => this.selectSex != 0)), (Action<M0>) (_isOn =>
        {
          if (!_isOn)
            return;
          this.selectSex = 0;
          this.CreateCharaList(this.lstMaleCharaFileInfo, false);
          this.listCtrl.SetNowSelectToggle();
          if (this.listCtrl.GetNowSelectCard() != null)
            this.listCtrl.SetParameterWindowVisible(true);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.tglMale), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      }
      if (Object.op_Implicit((Object) this.tglFemale))
      {
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglFemale.onValueChanged), (Func<M0, bool>) (_ => this.selectSex != 1)), (Action<M0>) (_isOn =>
        {
          if (!_isOn)
            return;
          this.selectSex = 1;
          this.CreateCharaList(this.lstFemaleCharaFileInfo, false);
          this.listCtrl.SetNowSelectToggle();
          if (this.listCtrl.GetNowSelectCard() != null)
            this.listCtrl.SetParameterWindowVisible(true);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.tglFemale), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      }
      if (Object.op_Implicit((Object) this.btnCharacterCreation))
      {
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCharacterCreation), (Func<M0, bool>) (_ => !Singleton<Scene>.Instance.IsNowLoadingFade)), (Action<M0>) (_ =>
        {
          if (this.onCharaCreateClickAction != null)
            this.onCharaCreateClickAction(this.selectSex);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnCharacterCreation), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
        ((UnityEngine.Component) this.btnCharacterCreation).get_gameObject().SetActiveIfDifferent(!this._hideCharacterCreation);
      }
      // ISSUE: method pointer
      this.actionFemaleSelect.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__D)));
      // ISSUE: method pointer
      this.actionFemaleSelect.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__E)));
      // ISSUE: method pointer
      this.actionMaleSelect.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__F)));
      // ISSUE: method pointer
      this.actionMaleSelect.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__10)));
      this.selectReactive = new BoolReactiveProperty(false);
      UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) this.selectReactive, (Selectable) this.btnEntry);
      this.listCtrl.onChangeItem = (Action<bool>) (_isOn => ((ReactiveProperty<bool>) this.selectReactive).set_Value(_isOn));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((UnityEngine.Component) this), (Func<M0, bool>) (_ => Input.GetMouseButtonUp(1))), (Func<M0, bool>) (_ => !Singleton<Scene>.Instance.IsNowLoadingFade)), (Action<M0>) (_ =>
      {
        if (this.onClickRightFunc == null)
          return;
        this.onClickRightFunc();
      }));
      if (Object.op_Implicit((Object) this.btnClose))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (_ =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
          if (this.onCloseWindowFunc == null)
            return;
          this.onCloseWindowFunc();
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.btnClose), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
        ((UnityEngine.Component) this.btnClose).get_gameObject().SetActiveIfDifferent(!this._hideClose);
      }
      this.IsStartUp = true;
    }

    public delegate void OnCloseWindowFunc();
  }
}
