// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemInfoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public abstract class ItemInfoUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _confirmLabel = string.Empty;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    protected Text _itemName;
    [SerializeField]
    protected Text _flavorText;
    [SerializeField]
    protected Button _submitButton;
    [SerializeField]
    protected GameObject _infoLayout;
    [SerializeField]
    private RectTransform _countViewerLayout;
    [SerializeField]
    protected CountViewer _countViewer;
    private IDisposable _fadeDisposable;

    public event Action OnSubmit;

    public event Action OnCancel;

    public void OnSubmitForce()
    {
      this.OnInputSubmit();
    }

    public string ConfirmLabel
    {
      get
      {
        return this._confirmLabel;
      }
    }

    public int Count
    {
      get
      {
        return this._countViewer.Count;
      }
    }

    public bool isCountViewerVisible
    {
      set
      {
        ((ReactiveProperty<bool>) this._isCountViewerVisible).set_Value(value);
      }
    }

    private BoolReactiveProperty _isCountViewerVisible { get; } = new BoolReactiveProperty(true);

    public bool isOpen
    {
      get
      {
        return this.IsActiveControl;
      }
    }

    public virtual void Open(StuffItem item)
    {
      this.Refresh(item);
      this.IsActiveControl = true;
    }

    public virtual void Close()
    {
      if (!this.isOpen)
        return;
      this.IsActiveControl = false;
    }

    public virtual void Refresh(int count)
    {
      this._countViewer.MaxCount = Mathf.Max(count, 1);
      this._countViewer.ForceCount = 1;
    }

    public virtual void Refresh(StuffItem item)
    {
      StuffItemInfo itemInfo = this.GetItemInfo(item);
      this._itemName.set_text(itemInfo.Name);
      this._flavorText.set_text(itemInfo.Explanation);
      this.Refresh(item.Count);
    }

    protected override void Start()
    {
      if (Object.op_Equality((Object) this._countViewer, (Object) null))
        ((MonoBehaviour) this).StartCoroutine(this.LoadCountViewer());
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (Object.op_Inequality((Object) this._submitButton, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._submitButton), (Action<M0>) (_ => this.OnInputSubmit()));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.SquareX
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand4);
      base.Start();
    }

    protected virtual void OnDestroy()
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = (IDisposable) null;
    }

    private StuffItemInfo GetItemInfo(StuffItem item)
    {
      if (item != null)
        return Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID) ?? Singleton<Resources>.Instance.GameInfo.GetItem_System(item.CategoryID, item.ID);
      Debug.LogError((object) "Item none");
      return (StuffItemInfo) null;
    }

    private void SetActiveControl(bool isActive)
    {
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator LoadCountViewer()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemInfoUI.\u003CLoadCountViewer\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemInfoUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemInfoUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      if (this.OnSubmit == null)
        return;
      this.OnSubmit();
    }

    private void OnInputCancel()
    {
      if (this.OnCancel == null)
        return;
      this.OnCancel();
    }
  }
}
