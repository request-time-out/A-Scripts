// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PresentItemListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class PresentItemListUI : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ItemListUI _itemList;
    [SerializeField]
    private Button _closeButton;
    [Header("Dialog")]
    [SerializeField]
    private CountSettingUI _countSetting;
    private int _selectedIndexOf;
    private int _categoryID;
    private int _itemID;
    private bool _isActive;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuElements;
    private Subject<Unit> _close;
    private IDisposable _labelSubscriber;
    private bool _entered;

    public PresentItemListUI()
    {
      base.\u002Ector();
    }

    public AgentActor Target { get; set; }

    public bool IsActiveControl
    {
      get
      {
        return this._isActive;
      }
      set
      {
        if (this._isActive == value)
          return;
        this._isActive = value;
        IEnumerator coroutine;
        if (value)
        {
          Time.set_timeScale(0.0f);
          Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
          Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
          coroutine = this.DoOpen();
        }
        else
        {
          Singleton<Input>.Instance.ClearMenuElements();
          Singleton<Input>.Instance.FocusLevel = -1;
          coroutine = this.DoClose();
        }
        if (this._fadeDisposable != null)
          this._fadeDisposable.Dispose();
        this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
      }
    }

    private MenuUIBehaviour[] MenuElements
    {
      get
      {
        MenuUIBehaviour[] menuElements = this._menuElements;
        if (menuElements != null)
          return menuElements;
        return this._menuElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this._itemList
        };
      }
    }

    public IObservable<Unit> OnClosedAsObservable()
    {
      return (IObservable<Unit>) this._close ?? (IObservable<Unit>) (this._close = new Subject<Unit>());
    }

    private void Start()
    {
      PointerClickTrigger pointerClickTrigger = (PointerClickTrigger) ((Component) this._canvasGroup).get_gameObject().AddComponent<PointerClickTrigger>();
      UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CStart\u003Em__2)));
      ((UITrigger) pointerClickTrigger).get_Triggers().Add(triggerEvent);
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      // ISSUE: method pointer
      this._itemList.OnSubmit.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      // ISSUE: method pointer
      this._itemList.OnCancel.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
    }

    private void Select(ItemNodeUI option)
    {
      if (Object.op_Equality((Object) option, (Object) null) || !option.IsInteractable)
        return;
      ((UnityEvent) option.OnClick)?.Invoke();
    }

    private void Close()
    {
      Time.set_timeScale(1f);
      this.IsActiveControl = false;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PresentItemListUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PresentItemListUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void InvokePresent(int count)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(this._categoryID, this._itemID);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      int num = 0;
      while (num < count)
        ++num;
      StuffItem stuffItem = new StuffItem(stuffItemInfo.CategoryID, stuffItemInfo.ID, count);
      this.Target.AgentData.ItemList.AddItem(stuffItem);
      player.PlayerData.ItemList.RemoveItem(stuffItem);
    }
  }
}
