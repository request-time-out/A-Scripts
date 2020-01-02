// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ChickenCoopListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ChickenCoopListUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _noneStr = string.Empty;
    [Header("Infomation Setting")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ChickenCoopUI _chickenCoopUI;
    [SerializeField]
    private Image _currentCursor;
    [SerializeField]
    private Image _selectCursor;
    [SerializeField]
    private Button _escapeButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private ChickenNameChangeUI _nameChangeUI;
    [SerializeField]
    private ChickenCoopListUI.Chicken[] _chickens;
    private IDisposable _fadeDisposable;

    public PlaySE playSE { get; } = new PlaySE();

    public event Action<int> IconChanged;

    public IObservable<Unit> Escape
    {
      get
      {
        return UnityUIComponentExtensions.OnClickAsObservable(this._escapeButton);
      }
    }

    public event Action OnSubmit;

    public event Action OnCancel;

    public int currentIndex
    {
      get
      {
        return ((ReactiveProperty<int>) this._currentIndex).get_Value();
      }
    }

    public bool currentActive
    {
      get
      {
        bool? isActive = this._chickens.SafeGet<ChickenCoopListUI.Chicken>(((ReactiveProperty<int>) this._currentIndex).get_Value())?.isActive;
        return isActive.HasValue && isActive.Value;
      }
    }

    public int Length
    {
      get
      {
        return this._chickens.Length;
      }
    }

    public void SetToggle(int index, bool isOn)
    {
      this._chickens.SafeGet<ChickenCoopListUI.Chicken>(index)?.toggle.set_isOn(isOn);
    }

    private IntReactiveProperty _currentIndex { get; } = new IntReactiveProperty(-1);

    private int _nameChangeIndex { get; set; } = -1;

    private AIProject.SaveData.Environment.ChickenInfo GetChickenInfo(int index)
    {
      return this._chickenCoopUI.currentChickens.SafeGet<AIProject.SaveData.Environment.ChickenInfo>(index);
    }

    public bool isOpen
    {
      get
      {
        return this.IsActiveControl;
      }
    }

    public virtual void Open()
    {
      this.IsActiveControl = true;
    }

    public virtual void Close()
    {
      if (!this.isOpen)
        return;
      this.IsActiveControl = false;
      this._nameChangeUI.Close();
    }

    public void Refresh(int index)
    {
      this._chickens[index].Set(this._noneStr, this.GetChickenInfo(index));
      ((ReactiveProperty<int>) this._currentIndex).SetValueAndForceNotify(index);
    }

    public virtual void Refresh()
    {
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int> anonType18 in ((IEnumerable<ChickenCoopListUI.Chicken>) this._chickens).Select<ChickenCoopListUI.Chicken, \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>>((Func<ChickenCoopListUI.Chicken, int, \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>>) ((p, i) => new \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>(p, i))))
        anonType18.p.Set(this._noneStr, this.GetChickenInfo(anonType18.i));
      this.SelectDefault();
    }

    public void SelectDefault()
    {
      this.ToggleOFF();
      ((ReactiveProperty<int>) this._currentIndex).SetValueAndForceNotify(0);
    }

    public void SelectNone()
    {
      this.ToggleOFF();
      ((ReactiveProperty<int>) this._currentIndex).SetValueAndForceNotify(-1);
    }

    public void ToggleOFF()
    {
      using (IEnumerator<Toggle> enumerator = ((IEnumerable<ChickenCoopListUI.Chicken>) this._chickens).Select<ChickenCoopListUI.Chicken, Toggle>((Func<ChickenCoopListUI.Chicken, Toggle>) (p => p.toggle)).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
          enumerator.Current.set_isOn(false);
      }
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (Object.op_Inequality((Object) this._closeButton, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Action<M0>) (_ =>
        {
          this.playSE.Play(SoundPack.SystemSE.Cancel);
          this.OnInputCancel();
        }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._currentIndex, (Action<M0>) (x =>
      {
        bool flag1 = x >= 0;
        ((Component) this._currentCursor).get_gameObject().SetActive(flag1);
        if (flag1)
        {
          CursorFrame.Set((RectTransform) ((Component) this._currentCursor).GetComponent<RectTransform>(), (RectTransform) ((Component) this._chickens[x].toggle).GetComponent<RectTransform>(), (RectTransform) null);
          if (this.IconChanged != null)
            this.IconChanged(x);
        }
        bool flag2 = this.GetChickenInfo(x) != null;
        ((Selectable) this._escapeButton).set_interactable(flag2);
        if (flag2)
          return;
        this._nameChangeUI.Close();
      }));
      DisposableExtensions.AddTo<CompositeDisposable>((M0) ((IEnumerable<Selectable>) ((IEnumerable<ChickenCoopListUI.Chicken>) this._chickens).Select<ChickenCoopListUI.Chicken, Toggle>((Func<ChickenCoopListUI.Chicken, Toggle>) (p => p.toggle))).BindToEnter(true, this._selectCursor), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ((IEnumerable<ChickenCoopListUI.Chicken>) this._chickens).Select<ChickenCoopListUI.Chicken, Toggle>((Func<ChickenCoopListUI.Chicken, Toggle>) (p => p.toggle)).BindToGroup((Action<int>) (sel => ((ReactiveProperty<int>) this._currentIndex).set_Value(sel))), (Component) this);
      this._nameChangeUI.OnSubmit += (Action) (() =>
      {
        this.playSE.Play(SoundPack.SystemSE.OK_S);
        AIProject.SaveData.Environment.ChickenInfo chickenInfo = this.GetChickenInfo(this._nameChangeIndex);
        this._chickens[this._nameChangeIndex].name.set_text(chickenInfo?.name ?? string.Empty);
        this.NameChanged(chickenInfo);
      });
      this._nameChangeUI.OnCancel += (Action) (() => this.playSE.Play(SoundPack.SystemSE.Cancel));
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int> anonType18 in ((IEnumerable<ChickenCoopListUI.Chicken>) this._chickens).Select<ChickenCoopListUI.Chicken, \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>>((Func<ChickenCoopListUI.Chicken, int, \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>>) ((p, i) => new \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int>(p, i))))
      {
        // ISSUE: variable of a compiler-generated type
        \u003C\u003E__AnonType18<ChickenCoopListUI.Chicken, int> item = anonType18;
        ChickenCoopListUI chickenCoopListUi = this;
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<Unit, int>((IObservable<M0>) item.p.NameChange, (Func<M0, M1>) (_ => item.i)), (Action<M0>) (index =>
        {
          chickenCoopListUi.playSE.Play(SoundPack.SystemSE.OK_S);
          chickenCoopListUi._nameChangeIndex = index;
          chickenCoopListUi._nameChangeUI.Open(chickenCoopListUi.GetChickenInfo(index));
        })), (Component) this);
      }
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__B)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__C)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.SquareX
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__D)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__E)));
      this._actionCommands.Add(actionIdDownCommand4);
      base.Start();
    }

    protected virtual void OnDestroy()
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = (IDisposable) null;
    }

    private void SetActiveControl(bool isActive)
    {
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenCoopListUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenCoopListUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      if (!this.isOpen || this.OnSubmit == null)
        return;
      this.OnSubmit();
    }

    private void OnInputCancel()
    {
      if (!this.isOpen)
        return;
      this.Close();
      if (this.OnCancel == null)
        return;
      this.OnCancel();
    }

    private void NameChanged(AIProject.SaveData.Environment.ChickenInfo info)
    {
      if (info == null || info.AnimalData == null)
        return;
      AIProject.SaveData.AnimalData animalData = info.AnimalData;
      ReadOnlyDictionary<int, AnimalBase> source = !Singleton<AnimalManager>.IsInstance() ? (ReadOnlyDictionary<int, AnimalBase>) null : Singleton<AnimalManager>.Instance.AnimalTable;
      AnimalBase animalBase;
      if (source.IsNullOrEmpty<int, AnimalBase>() || !source.TryGetValue(animalData.AnimalID, ref animalBase) || !(animalBase is PetChicken))
        return;
      (animalBase as PetChicken).Nickname = info.name;
    }

    [Serializable]
    private class Chicken
    {
      [SerializeField]
      private Toggle _toggle;
      [SerializeField]
      private Image _icon;
      [SerializeField]
      private Text _name;
      [SerializeField]
      private Button _nameChange;

      public bool isActive
      {
        get
        {
          return this._info != null;
        }
      }

      public Toggle toggle
      {
        get
        {
          return this._toggle;
        }
      }

      public Image icon
      {
        get
        {
          return this._icon;
        }
      }

      public Text name
      {
        get
        {
          return this._name;
        }
      }

      public IObservable<Unit> NameChange
      {
        get
        {
          return UnityUIComponentExtensions.OnClickAsObservable(this._nameChange);
        }
      }

      public Button nameChange
      {
        get
        {
          return this._nameChange;
        }
      }

      private AIProject.SaveData.Environment.ChickenInfo _info { get; set; }

      public void Set(string noneStr, AIProject.SaveData.Environment.ChickenInfo info)
      {
        this._info = info;
        ((Behaviour) this._icon).set_enabled(this.isActive);
        this._name.set_text(info?.name ?? noneStr);
        ((Component) this._nameChange).get_gameObject().SetActive(this.isActive);
      }
    }
  }
}
