// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ClosetUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
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

namespace AIProject.UI
{
  public class ClosetUI : MenuUIBehaviour
  {
    private BoolReactiveProperty _isActiveParameter = new BoolReactiveProperty();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CoordinateListUI _folderCoordinateListUI;
    [SerializeField]
    private CoordinateListUI _savedataCoordinateListUI;
    [SerializeField]
    private Button _buttonClose;
    [SerializeField]
    private bool _hideClose;
    [SerializeField]
    private CanvasGroup _canvasGroupParameter;
    [SerializeField]
    private GameObject _objParameterWindow;
    [SerializeField]
    private RawImage _rawImageCoordinateCard;
    [SerializeField]
    private Button _buttonInput;
    [SerializeField]
    private Button _buttonOutput;
    private int _femaleID;
    private List<GameCoordinateFileInfo> _folderCoordinateFileInfo;
    private List<GameCoordinateFileInfo> _savedataCoordinateFileInfo;
    private BoolReactiveProperty _selectFolder;
    private BoolReactiveProperty _selectSaveData;
    private GameCoordinateFileInfo _selectInfo;
    private MenuUIBehaviour[] _menuUIList;
    private IConnectableObservable<bool> _activeParameterChange;
    private IDisposable _fadeDisposable;
    private IDisposable _fadeParameterDisposable;

    public ClosetUI()
    {
      this.OnChangeFolderItemFunc = (CoordinateListUI.ChangeItemFunc) null;
      this.OnChangeSavedataItemFunc = (CoordinateListUI.ChangeItemFunc) null;
    }

    public CoordinateListUI FolderCoordinateListUI
    {
      get
      {
        return this._folderCoordinateListUI;
      }
    }

    public CoordinateListUI SaveDataCoordinateListUI
    {
      get
      {
        return this._savedataCoordinateListUI;
      }
    }

    public bool HideClose
    {
      get
      {
        return this._hideClose;
      }
      set
      {
        if (!Object.op_Implicit((Object) this._buttonClose))
          return;
        this._hideClose = value;
        ((Component) this._buttonClose).get_gameObject().SetActiveIfDifferent(!value);
      }
    }

    public CoordinateListUI.ChangeItemFunc OnChangeFolderItemFunc
    {
      set
      {
        if (!Object.op_Inequality((Object) this._folderCoordinateListUI, (Object) null))
          return;
        this._folderCoordinateListUI.OnChangeItemFunc = value;
      }
    }

    public CoordinateListUI.ChangeItemFunc OnChangeSavedataItemFunc
    {
      set
      {
        if (!Object.op_Inequality((Object) this._savedataCoordinateListUI, (Object) null))
          return;
        this._savedataCoordinateListUI.OnChangeItemFunc = value;
      }
    }

    public Action OnClickRightFunc { get; set; }

    public List<string> CoordinateFilterSource { get; set; }

    public MenuUIBehaviour[] MenuUIList
    {
      get
      {
        if (this._menuUIList == null)
          this._menuUIList = new MenuUIBehaviour[3]
          {
            (MenuUIBehaviour) this,
            (MenuUIBehaviour) this._folderCoordinateListUI,
            (MenuUIBehaviour) this._savedataCoordinateListUI
          };
        return this._menuUIList;
      }
    }

    public IObservable<bool> OnActiveParameterChangedAsObservable()
    {
      if (this._activeParameterChange == null)
      {
        this._activeParameterChange = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActiveParameter, ((Component) this).get_gameObject()));
        this._activeParameterChange.Connect();
      }
      return (IObservable<bool>) this._activeParameterChange;
    }

    public void InitCoordinateList()
    {
      this._folderCoordinateFileInfo = GameCoordinateFileInfoAssist.CreateCoordinateFileInfoList(false, true, this.CoordinateFilterSource);
      this._savedataCoordinateFileInfo = GameCoordinateFileInfoAssist.CreateCoordinateFileInfoQueryList(this.CoordinateFilterSource);
    }

    private void CreateCoordinateList(
      List<GameCoordinateFileInfo> list,
      List<GameCoordinateFileInfo> saveList,
      bool isSelectInfoClear = false)
    {
      this._folderCoordinateListUI.ClearList();
      this._folderCoordinateListUI.AddList(list);
      this._folderCoordinateListUI.Create(isSelectInfoClear);
      this._savedataCoordinateListUI.ClearList();
      this._savedataCoordinateListUI.AddList(saveList);
      this._savedataCoordinateListUI.Create(isSelectInfoClear);
    }

    public void ReCreateList(bool isSelectInfoClear)
    {
      this.InitCoordinateList();
      this._folderCoordinateListUI.InitSort();
      this._savedataCoordinateListUI.InitSort();
      this.CreateCoordinateList(this._folderCoordinateFileInfo, this._savedataCoordinateFileInfo, false);
    }

    private void SetParameter(GameCoordinateFileInfo data, bool fromFolder)
    {
      this._objParameterWindow.get_activeSelf();
      this._objParameterWindow.SetActiveIfDifferent(true);
      this._rawImageCoordinateCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(data.FullPath), 0, 0, (TextureFormat) 5, false));
      ((Component) this._buttonInput).get_gameObject().SetActiveIfDifferent(fromFolder);
      ((Component) this._buttonOutput).get_gameObject().SetActiveIfDifferent(!fromFolder);
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (active => this.SetActiveControl(active)));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveParameterChangedAsObservable(), (Action<M0>) (active => this.SetActiveParameter(active)));
      this._selectFolder = new BoolReactiveProperty();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._selectFolder, (Action<M0>) (x =>
      {
        if (x)
        {
          ((Component) this._buttonInput).get_gameObject().SetActiveIfDifferent(true);
          ((Component) this._buttonOutput).get_gameObject().SetActiveIfDifferent(false);
          ((ReactiveProperty<bool>) this._isActiveParameter).set_Value(true);
        }
        else
          ((ReactiveProperty<bool>) this._isActiveParameter).set_Value(false);
      }));
      this._selectSaveData = new BoolReactiveProperty();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._selectSaveData, (Action<M0>) (x =>
      {
        if (x)
        {
          ((Component) this._buttonInput).get_gameObject().SetActiveIfDifferent(false);
          ((Component) this._buttonOutput).get_gameObject().SetActiveIfDifferent(true);
          ((ReactiveProperty<bool>) this._isActiveParameter).set_Value(true);
        }
        else
          ((ReactiveProperty<bool>) this._isActiveParameter).set_Value(false);
      }));
      this._folderCoordinateListUI.OnChangeItemFunc = (CoordinateListUI.ChangeItemFunc) (dat =>
      {
        this._selectInfo = dat;
        if (dat == null)
          return;
        this._rawImageCoordinateCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(dat.FullPath), 0, 0, (TextureFormat) 5, false));
      });
      this._savedataCoordinateListUI.OnChangeItemFunc = (CoordinateListUI.ChangeItemFunc) (dat =>
      {
        this._selectInfo = dat;
        if (dat == null)
          return;
        this._rawImageCoordinateCard.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(dat.FullPath), 0, 0, (TextureFormat) 5, false));
      });
      this._folderCoordinateListUI.OnChangeItem = (Action<bool>) (isOn =>
      {
        if (isOn)
        {
          ((ReactiveProperty<bool>) this._selectSaveData).set_Value(!isOn);
          this._savedataCoordinateListUI.SelectDataClear();
          this._savedataCoordinateListUI.SetNowSelectToggle();
          ((ReactiveProperty<bool>) this._selectFolder).set_Value(isOn);
        }
        else
        {
          ((ReactiveProperty<bool>) this._selectFolder).set_Value(false);
          ((ReactiveProperty<bool>) this._selectSaveData).set_Value(false);
        }
      });
      this._savedataCoordinateListUI.OnChangeItem = (Action<bool>) (isOn =>
      {
        if (isOn)
        {
          ((ReactiveProperty<bool>) this._selectFolder).set_Value(!isOn);
          this._folderCoordinateListUI.SelectDataClear();
          this._folderCoordinateListUI.SetNowSelectToggle();
          ((ReactiveProperty<bool>) this._selectSaveData).set_Value(isOn);
        }
        else
        {
          ((ReactiveProperty<bool>) this._selectFolder).set_Value(false);
          ((ReactiveProperty<bool>) this._selectSaveData).set_Value(false);
        }
      });
      if (Object.op_Implicit((Object) this._buttonInput))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._buttonInput.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__8)));
      }
      if (Object.op_Implicit((Object) this._buttonOutput))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._buttonOutput.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__9)));
      }
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__A)));
      this._keyCommands.Add(keyCodeDownCommand);
      if (!Object.op_Implicit((Object) this._buttonClose))
        return;
      // ISSUE: method pointer
      ((UnityEvent) this._buttonClose.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__B)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._buttonClose), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      ((Component) this._buttonClose).get_gameObject().SetActiveIfDifferent(!this._hideClose);
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
        this.InitCoordinateList();
        this.CreateCoordinateList(this._folderCoordinateFileInfo, this._savedataCoordinateFileInfo, false);
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
      return (IEnumerator) new ClosetUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ClosetUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SetActiveParameter(bool active)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine = !active ? this.CloseParameterCoroutine() : this.OpenParameterCoroutine();
      if (this._fadeParameterDisposable != null)
        this._fadeParameterDisposable.Dispose();
      this._fadeParameterDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenParameterCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ClosetUI.\u003COpenParameterCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseParameterCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ClosetUI.\u003CCloseParameterCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }
  }
}
