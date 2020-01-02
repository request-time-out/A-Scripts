// Decompiled with JetBrains decompiler
// Type: WarpListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.UI;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class WarpListUI : MenuUIBehaviour
{
  private IntReactiveProperty WarpID = new IntReactiveProperty(-1);
  public int EndShowID = 4;
  [SerializeField]
  private Image[] Cursor;
  [SerializeField]
  private AllAreaMapUI areaMapUI;
  [SerializeField]
  private MapActionCategoryUI mapActionCategoryUI;
  public ScrollRect scrollRect;
  private Input Input;
  public int StartShowID;
  private const int ShowNodeNum = 5;
  private MenuUIBehaviour[] _menuUIList;
  private IDisposable disposable;

  public int _WarpID
  {
    get
    {
      return ((ReactiveProperty<int>) this.WarpID).get_Value();
    }
    set
    {
      ((ReactiveProperty<int>) this.WarpID).set_Value(value);
    }
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

  public void Init()
  {
    this.Input = Singleton<Input>.Instance;
    ObservableExtensions.Subscribe<int>((IObservable<M0>) this.WarpID, (Action<M0>) (x =>
    {
      if (x == 0)
      {
        ((Behaviour) this.Cursor[0]).set_enabled(true);
        ((Behaviour) this.Cursor[1]).set_enabled(false);
      }
      else
      {
        if (x <= 0)
          return;
        ((Behaviour) this.Cursor[0]).set_enabled(false);
        ((Behaviour) this.Cursor[1]).set_enabled(true);
      }
    }));
    this.disposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.Input.FocusLevel == this.FocusLevel)), (Action<M0>) (_ => this.OnUpdate()));
    PointerEnterTrigger pointerEnterTrigger = (PointerEnterTrigger) ((Component) this.areaMapUI._WorldMap).get_gameObject().GetComponent<PointerEnterTrigger>();
    if (Object.op_Equality((Object) pointerEnterTrigger, (Object) null))
      pointerEnterTrigger = (PointerEnterTrigger) ((Component) this.areaMapUI._WorldMap).get_gameObject().AddComponent<PointerEnterTrigger>();
    UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
    ((UITrigger) pointerEnterTrigger).get_Triggers().Add(triggerEvent);
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CInit\u003Em__3)));
    ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
    {
      ActionID = ActionID.Submit
    };
    // ISSUE: method pointer
    actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__4)));
    this._actionCommands.Add(actionIdDownCommand);
    this.Start();
  }

  public void SetCursor()
  {
    ((Behaviour) this.Cursor[((ReactiveProperty<int>) this.WarpID).get_Value()]).set_enabled(true);
  }

  public void DelCursor()
  {
    ((Behaviour) this.Cursor[0]).set_enabled(false);
    ((Behaviour) this.Cursor[1]).set_enabled(false);
    this._WarpID = -1;
  }

  private void OnUpdate()
  {
    if (!((Behaviour) this.Cursor[!this.areaMapUI.GameClear ? 1 : 0]).get_enabled() && ((ReactiveProperty<int>) this.WarpID).get_Value() == (!this.areaMapUI.GameClear ? 1 : 0))
      this.SetCursor();
    if (this._WarpID <= 0 || this.areaMapUI._WarpNodes == null || this.areaMapUI._WarpNodes.Count <= 0)
      return;
    ((Component) this.Cursor[1]).get_transform().set_position(((Component) this.areaMapUI._WarpNodes[this._WarpID - 1]).get_transform().get_position());
  }

  public override void OnInputMoveDirection(MoveDirection moveDir)
  {
    if (moveDir != 1)
    {
      if (moveDir != 3)
        return;
      int num = ((ReactiveProperty<int>) this.WarpID).get_Value() + 1;
      if (num > this.areaMapUI._WarpNodes.Count)
        num = this.areaMapUI._WarpNodes.Count;
      ((ReactiveProperty<int>) this.WarpID).set_Value(num);
      if (num > this.EndShowID + 1)
      {
        ++this.StartShowID;
        ++this.EndShowID;
      }
      this.scrollRect.set_verticalNormalizedPosition(Mathf.Clamp((float) (1.0 - (double) this.StartShowID / (double) (this.areaMapUI._WarpNodes.Count - 5)), 0.0f, 1f));
    }
    else
    {
      int index = ((ReactiveProperty<int>) this.WarpID).get_Value() - 1;
      if (index < (!this.areaMapUI.GameClear ? 1 : 0))
      {
        this.Input.FocusLevel = this.mapActionCategoryUI.FocusLevel;
        this.Input.MenuElements = this.mapActionCategoryUI.MenuUIList;
        index = !this.areaMapUI.GameClear ? 1 : 0;
        ((Behaviour) this.Cursor[index]).set_enabled(false);
      }
      ((ReactiveProperty<int>) this.WarpID).set_Value(index);
      if (index < this.StartShowID + 1)
      {
        --this.StartShowID;
        --this.EndShowID;
      }
      this.scrollRect.set_verticalNormalizedPosition(Mathf.Clamp((float) (1.0 - (double) this.StartShowID / (double) (this.areaMapUI._WarpNodes.Count - 5)), 0.0f, 1f));
    }
  }

  private void OnInputSubmit()
  {
    if (this.Input.FocusLevel != this.FocusLevel)
      return;
    if (((ReactiveProperty<int>) this.WarpID).get_Value() == 0)
      ((UnityEvent) this.areaMapUI._WorldMap.get_onClick())?.Invoke();
    else
      ((UnityEvent) this.areaMapUI._WarpNodes[((ReactiveProperty<int>) this.WarpID).get_Value() - 1].get_onClick())?.Invoke();
  }

  public void DisposeWarpListUI()
  {
    if (this.disposable == null)
      return;
    this.disposable.Dispose();
    this.disposable = (IDisposable) null;
  }
}
