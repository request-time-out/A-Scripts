// Decompiled with JetBrains decompiler
// Type: MapActionCategoryUI
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

public class MapActionCategoryUI : MenuUIBehaviour
{
  private IntReactiveProperty CategoryFilterID = new IntReactiveProperty(0);
  [SerializeField]
  private Image Cursor;
  [SerializeField]
  private AllAreaMapUI areaMapUI;
  [SerializeField]
  private AllAreaMapActionFilter actionFilter;
  [SerializeField]
  private WarpListUI warpListUI;
  private Input Input;
  private MenuUIBehaviour[] _menuUIList;
  private IDisposable Dispose;

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
    if (this.CategoryFilterID != null)
      ((ReactiveProperty<int>) this.CategoryFilterID).Dispose();
    ObservableExtensions.Subscribe<int>((IObservable<M0>) this.CategoryFilterID, (Action<M0>) (x => ((Component) this.Cursor).get_transform().set_position(((Component) this.actionFilter.ActionToggles[x]).get_transform().get_position())));
    for (int index = 0; index < this.actionFilter.ActionToggles.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapActionCategoryUI.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new MapActionCategoryUI.\u003CInit\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.id = index;
      // ISSUE: reference to a compiler-generated field
      PointerEnterTrigger pointerEnterTrigger = (PointerEnterTrigger) ((Component) this.actionFilter.ActionToggles[initCAnonStorey0.id]).get_gameObject().GetComponent<PointerEnterTrigger>();
      if (Object.op_Equality((Object) pointerEnterTrigger, (Object) null))
      {
        // ISSUE: reference to a compiler-generated field
        pointerEnterTrigger = (PointerEnterTrigger) ((Component) this.actionFilter.ActionToggles[initCAnonStorey0.id]).get_gameObject().AddComponent<PointerEnterTrigger>();
      }
      UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
      if (((UITrigger) pointerEnterTrigger).get_Triggers().Count > 0)
        ((UITrigger) pointerEnterTrigger).get_Triggers().Clear();
      ((UITrigger) pointerEnterTrigger).get_Triggers().Add(triggerEvent);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
    }
    if (this.Dispose != null)
      this.Dispose.Dispose();
    this.Dispose = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.Input.FocusLevel == this.FocusLevel)), (Action<M0>) (_ => this.OnUpdate()));
    ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
    {
      ActionID = ActionID.Submit
    };
    // ISSUE: method pointer
    actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__3)));
    this._actionCommands.Clear();
    this._actionCommands.Add(actionIdDownCommand);
    this.Start();
  }

  private void OnUpdate()
  {
    if (!((Behaviour) this.Cursor).get_enabled())
      this.SetCursor();
    this.ChangeShowPointIcon();
    ((Component) this.Cursor).get_transform().set_position(((Component) this.actionFilter.ActionToggles[((ReactiveProperty<int>) this.CategoryFilterID).get_Value()]).get_transform().get_position());
  }

  private void ChangeShowPointIcon()
  {
    if (this.Input.IsPressedKey(ActionID.RightShoulder1) || this.Input.IsPressedKey((KeyCode) 101))
    {
      int num = ((ReactiveProperty<int>) this.CategoryFilterID).get_Value() + 1;
      ((ReactiveProperty<int>) this.CategoryFilterID).set_Value(this.areaMapUI.GameClear ? (num <= 29 ? num : 29) : (num <= 28 ? num : 28));
    }
    else
    {
      if (!this.Input.IsPressedKey(ActionID.LeftShoulder1) && !this.Input.IsPressedKey((KeyCode) 113))
        return;
      int num = ((ReactiveProperty<int>) this.CategoryFilterID).get_Value() - 1;
      ((ReactiveProperty<int>) this.CategoryFilterID).set_Value(num >= 0 ? num : 0);
    }
  }

  public void SetCursor()
  {
    ((Component) this.Cursor).get_transform().set_position(((Component) this.actionFilter.ActionToggles[((ReactiveProperty<int>) this.CategoryFilterID).get_Value()]).get_transform().get_position());
    ((Behaviour) this.Cursor).set_enabled(true);
  }

  public void DelCursor()
  {
    ((Behaviour) this.Cursor).set_enabled(false);
  }

  public override void OnInputMoveDirection(MoveDirection moveDir)
  {
    if (moveDir != 3 || this.areaMapUI._WarpNodes == null || this.areaMapUI._WarpNodes.Count <= 0)
      return;
    ((Behaviour) this.Cursor).set_enabled(false);
    this.Input.FocusLevel = this.warpListUI.FocusLevel;
    this.Input.MenuElements = this.warpListUI.MenuUIList;
    this.warpListUI._WarpID = !this.areaMapUI.GameClear ? 1 : 0;
    this.warpListUI.SetCursor();
  }

  private void OnInputSubmit()
  {
    Toggle actionToggle = this.actionFilter.ActionToggles[((ReactiveProperty<int>) this.CategoryFilterID).get_Value()];
    actionToggle.set_isOn(((actionToggle.get_isOn() ? 1 : 0) ^ 1) != 0);
  }
}
