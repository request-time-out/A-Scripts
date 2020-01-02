// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CoordinateListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using SceneAssist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class CoordinateListUI : MenuUIBehaviour
  {
    [SerializeField]
    private GameCoordinateFileScrollInfo _scrollData = new GameCoordinateFileScrollInfo();
    private List<GameCoordinateFileInfo> _listFileInfo = new List<GameCoordinateFileInfo>();
    private BoolReactiveProperty _isActiveSortWindow = new BoolReactiveProperty(false);
    [SerializeField]
    private ClosetUI _closetUI;
    [SerializeField]
    private Toggle _toggleOrder;
    [SerializeField]
    private Image _imageOrder;
    [SerializeField]
    private PointerEnterExitAction _actionOrderSelect;
    [SerializeField]
    private GameObject _objOrderSelect;
    [SerializeField]
    private Button _buttonSortWindowOpen;
    [SerializeField]
    private GameObject _objSortWindow;
    [SerializeField]
    private CanvasGroup _canvasGroupSortWindow;
    [SerializeField]
    private Button _buttonSortWindowClose;
    [SerializeField]
    private Toggle _toggleSortDay;
    [SerializeField]
    private Toggle _toggleSortName;
    [SerializeField]
    private PointerEnterExitAction _actionSortDay;
    [SerializeField]
    private PointerEnterExitAction _actionSortName;
    [SerializeField]
    private GameObject _objSortDaySelect;
    [SerializeField]
    private GameObject _objSortNameSelect;
    private int _sortSelectNum;
    private int femaleParameterSelectNum;
    private IConnectableObservable<bool> _activeChangeSortWindow;
    private IDisposable _fadeSortWindowDisposable;

    public List<GameCoordinateFileInfo> GetListCharaFileInfo()
    {
      return this._listFileInfo;
    }

    [HideInInspector]
    public bool UpdateCategory { get; set; }

    public CoordinateListUI.ChangeItemFunc OnChangeItemFunc { get; set; }

    public Action<bool> OnChangeItem { get; set; }

    public IObservable<bool> OnActiveSortWindowChangedAsObservable()
    {
      if (this._activeChangeSortWindow == null)
      {
        this._activeChangeSortWindow = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.Where<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActiveSortWindow, ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())));
        this._activeChangeSortWindow.Connect();
      }
      return (IObservable<bool>) this._activeChangeSortWindow;
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this._toggleOrder.onValueChanged), (Action<M0>) (isOn =>
      {
        ((Behaviour) this._imageOrder).set_enabled(!isOn);
        if (this._toggleSortDay.get_isOn())
          this.SortDate(isOn);
        else
          this.SortFileName(isOn);
        this._scrollData.Init(this._listFileInfo);
        this._scrollData.SetNowSelectToggle();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ((Behaviour) this._imageOrder).set_enabled(!this._toggleOrder.get_isOn());
      this._objOrderSelect.SetActiveIfDifferent(false);
      // ISSUE: method pointer
      this._actionOrderSelect.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      // ISSUE: method pointer
      this._actionOrderSelect.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._buttonSortWindowOpen), (Action<M0>) (_ =>
      {
        ((ReactiveProperty<bool>) this._isActiveSortWindow).set_Value(true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._buttonSortWindowOpen), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      this._scrollData.OnSelect = (Action<GameCoordinateFileInfo>) (data =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        CoordinateListUI.ChangeItemFunc onChangeItemFunc = this.OnChangeItemFunc;
        if (onChangeItemFunc != null)
          onChangeItemFunc(data);
        Action<bool> onChangeItem = this.OnChangeItem;
        if (onChangeItem == null)
          return;
        onChangeItem(true);
      });
      this._scrollData.OnDeselect = (Action) (() =>
      {
        Action<bool> onChangeItem = this.OnChangeItem;
        if (onChangeItem == null)
          return;
        onChangeItem(false);
      });
      this._objSortWindow.SetActiveIfDifferent(false);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._buttonSortWindowClose), (Action<M0>) (_ =>
      {
        ((ReactiveProperty<bool>) this._isActiveSortWindow).set_Value(false);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._buttonSortWindowClose), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
      // ISSUE: method pointer
      this._actionSortDay.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__A)));
      // ISSUE: method pointer
      this._actionSortDay.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__B)));
      // ISSUE: method pointer
      this._actionSortName.listActionEnter.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__C)));
      // ISSUE: method pointer
      this._actionSortName.listActionExit.Add(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__D)));
      this._objSortDaySelect.SetActiveIfDifferent(false);
      this._objSortNameSelect.SetActiveIfDifferent(false);
      if (Object.op_Implicit((Object) this._toggleSortDay))
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this._toggleSortDay.onValueChanged), (Func<M0, bool>) (_ => this._sortSelectNum != 0)), (Action<M0>) (isOn =>
        {
          if (!isOn)
            return;
          this._sortSelectNum = 0;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.SortDate(this._toggleOrder.get_isOn());
          this._scrollData.Init(this._listFileInfo);
          this._scrollData.SetNowSelectToggle();
        }));
      if (Object.op_Implicit((Object) this._toggleSortName))
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this._toggleSortName.onValueChanged), (Func<M0, bool>) (_ => this._sortSelectNum != 1)), (Action<M0>) (isOn =>
        {
          if (!isOn)
            return;
          this._sortSelectNum = 1;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.SortFileName(this._toggleOrder.get_isOn());
          this._scrollData.Init(this._listFileInfo);
          this._scrollData.SetNowSelectToggle();
        }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveSortWindowChangedAsObservable(), (Action<M0>) (active => this.SetActiveSortWindow(active)));
    }

    public void SortDate(bool ascend)
    {
      if (this._listFileInfo.Count == 0)
        return;
      using (new GameSystem.CultureScope())
      {
        if (ascend)
          this._listFileInfo = this._listFileInfo.OrderBy<GameCoordinateFileInfo, DateTime>((Func<GameCoordinateFileInfo, DateTime>) (n => n.Time)).ThenBy<GameCoordinateFileInfo, string>((Func<GameCoordinateFileInfo, string>) (n => n.FileName)).ToList<GameCoordinateFileInfo>();
        else
          this._listFileInfo = this._listFileInfo.OrderByDescending<GameCoordinateFileInfo, DateTime>((Func<GameCoordinateFileInfo, DateTime>) (n => n.Time)).ThenByDescending<GameCoordinateFileInfo, string>((Func<GameCoordinateFileInfo, string>) (n => n.FileName)).ToList<GameCoordinateFileInfo>();
      }
    }

    public void SortFileName(bool ascend)
    {
      if (this._listFileInfo.Count == 0)
        return;
      using (new GameSystem.CultureScope())
      {
        if (ascend)
          this._listFileInfo = this._listFileInfo.OrderBy<GameCoordinateFileInfo, string>((Func<GameCoordinateFileInfo, string>) (n => n.FileName)).ThenBy<GameCoordinateFileInfo, DateTime>((Func<GameCoordinateFileInfo, DateTime>) (n => n.Time)).ToList<GameCoordinateFileInfo>();
        else
          this._listFileInfo = this._listFileInfo.OrderByDescending<GameCoordinateFileInfo, string>((Func<GameCoordinateFileInfo, string>) (n => n.FileName)).ThenByDescending<GameCoordinateFileInfo, DateTime>((Func<GameCoordinateFileInfo, DateTime>) (n => n.Time)).ToList<GameCoordinateFileInfo>();
      }
    }

    public void SelectDataClear()
    {
      if (this._scrollData == null)
        return;
      this._scrollData.SelectDataClear();
    }

    public void SetNowSelectToggle()
    {
      if (this._scrollData == null)
        return;
      this._scrollData.SetNowSelectToggle();
    }

    public void ClearList()
    {
      this._listFileInfo.Clear();
    }

    public void AddList(List<GameCoordinateFileInfo> list)
    {
      this._listFileInfo = new List<GameCoordinateFileInfo>((IEnumerable<GameCoordinateFileInfo>) list);
    }

    public GameCoordinateFileInfo GetNowSelectCard()
    {
      return this._scrollData.SelectData?.info;
    }

    public void InitSort()
    {
      this._toggleOrder.SetIsOnWithoutCallback(true);
      ((Behaviour) this._imageOrder).set_enabled(!this._toggleOrder.get_isOn());
      this._sortSelectNum = 0;
      this._toggleSortDay.SetIsOnWithoutCallback(true);
    }

    public void Create(bool isSelectInfoClear)
    {
      if (this._toggleSortDay.get_isOn())
        this.SortDate(this._toggleOrder.get_isOn());
      else
        this.SortFileName(this._toggleOrder.get_isOn());
      if (isSelectInfoClear)
      {
        this._scrollData.SelectDataClear();
        this._scrollData.SetTopLine();
      }
      this._scrollData.Init(this._listFileInfo);
      ((ReactiveProperty<bool>) this._isActiveSortWindow).set_Value(false);
      this._objSortWindow.SetActiveIfDifferent(false);
    }

    private void SetActiveSortWindow(bool active)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine = !active ? this.CloseSortWindowCoroutine() : this.OpenSortWindowCoroutine();
      if (this._fadeSortWindowDisposable != null)
        this._fadeSortWindowDisposable.Dispose();
      this._fadeSortWindowDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenSortWindowCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CoordinateListUI.\u003COpenSortWindowCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseSortWindowCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CoordinateListUI.\u003CCloseSortWindowCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public delegate void ChangeItemFunc(GameCoordinateFileInfo info);
  }
}
