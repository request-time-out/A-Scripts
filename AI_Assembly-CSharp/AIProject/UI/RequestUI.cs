// Decompiled with JetBrains decompiler
// Type: AIProject.UI.RequestUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Popup;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class RequestUI : MenuUIBehaviour
  {
    [SerializeField]
    private RequestUI.RequestElement[] requestElements = new RequestUI.RequestElement[0];
    [SerializeField]
    private Color whiteColor = (Color) null;
    [SerializeField]
    private Color redColor = (Color) null;
    private List<StuffItem> usedItems = new List<StuffItem>();
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private CanvasGroup requestCanvasGroup0;
    [SerializeField]
    private CanvasGroup requestCanvasGroup1;
    [SerializeField]
    private Text requestText;
    [SerializeField]
    private CanvasGroup requestCanvasGroup2;
    [SerializeField]
    private Text leftText_Type2;
    [SerializeField]
    private Text rightText_Type2;
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private Text submitText;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Text cancelText;
    private IObservable<TimeInterval<float>> lerpStream;
    private MenuUIBehaviour[] uiElements;
    private IDisposable fadeDiposable;
    private RequestInfo requestInfo;

    public System.Action SubmitEvent { get; set; }

    public bool Submit { get; private set; }

    public bool IsImpossible { get; private set; } = true;

    public Func<bool> SubmitCondition { get; set; }

    public System.Action ClosedEvent { get; set; }

    public System.Action CancelEvent { get; set; }

    public bool Cancel { get; private set; }

    public int RequestID { get; set; }

    public RequestInfo OpenInfo { get; set; }

    public override bool IsActiveControl
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActive).get_Value();
      }
      set
      {
        if (((ReactiveProperty<bool>) this._isActive).get_Value() == value)
          return;
        ((ReactiveProperty<bool>) this._isActive).set_Value(value);
      }
    }

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] uiElements = this.uiElements;
        if (uiElements != null)
          return uiElements;
        return this.uiElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (System.Action<M0>) (x => this.SetActiveControl(x)));
      this.lerpStream = (IObservable<TimeInterval<float>>) Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._alphaAccelerationTime, true), true), (Component) this);
      if (this.submitButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this.submitButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      }
      if (this.cancelButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this.cancelButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      }
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      this._keyCommands.Add(keyCodeDownCommand);
    }

    private void DoSubmit()
    {
      if (!this.IsActiveControl)
        return;
      this.PlaySubmitSE();
      this.Submit = true;
      this.IsActiveControl = false;
    }

    private void DoCancel()
    {
      if (!this.IsActiveControl)
        return;
      this.PlayCancelSE();
      this.Cancel = true;
      this.IsActiveControl = false;
    }

    protected override void OnAfterStart()
    {
      if (this.canvasGroup.get_blocksRaycasts())
        this.canvasGroup.set_blocksRaycasts(false);
      if (this.canvasGroup.get_interactable())
        this.canvasGroup.set_interactable(false);
      if (!((Component) this).get_gameObject().get_activeSelf())
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    private void SetActiveControl(bool _active)
    {
      if (_active)
        this.UISetting();
      IEnumerator _coroutine = !_active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this.fadeDiposable != null)
        this.fadeDiposable.Dispose();
      this.fadeDiposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    protected override void Awake()
    {
      base.Awake();
      this.Submit = false;
      this.Cancel = false;
    }

    private void HideElements()
    {
      this.messageText.set_text(string.Empty);
      CanvasGroup requestCanvasGroup0 = this.requestCanvasGroup0;
      float num1 = 0.0f;
      this.requestCanvasGroup2.set_alpha(num1);
      float num2 = num1;
      this.requestCanvasGroup1.set_alpha(num2);
      double num3 = (double) num2;
      requestCanvasGroup0.set_alpha((float) num3);
      this.IsImpossible = true;
      if (!((Component) this.submitButton).get_gameObject().get_activeSelf())
        return;
      ((Component) this.submitButton).get_gameObject().SetActive(false);
    }

    private void UISetting()
    {
      this.IsImpossible = true;
      this.usedItems.Clear();
      ReadOnlyDictionary<int, RequestInfo> requestTable = Singleton<Resources>.Instance.PopupInfo.RequestTable;
      this.requestInfo = (RequestInfo) null;
      RequestInfo requestInfo;
      if (!requestTable.TryGetValue(this.RequestID, ref requestInfo))
      {
        this.HideElements();
      }
      else
      {
        this.requestInfo = requestInfo;
        int index1 = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
        this.messageText.set_text(requestInfo.Title.GetElement<string>(index1) ?? string.Empty);
        switch (requestInfo.Type)
        {
          case 0:
            this.requestCanvasGroup0.set_alpha(1f);
            this.requestCanvasGroup1.set_alpha(0.0f);
            this.requestCanvasGroup2.set_alpha(0.0f);
            this.requestText.set_text(string.Empty);
            Resources.GameInfoTables gameInfo = Singleton<Resources>.Instance.GameInfo;
            List<StuffItem> itemList = Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList;
            int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
            bool flag1 = !requestInfo.Items.IsNullOrEmpty<Tuple<int, int, int>>();
            for (int index2 = 0; index2 < this.requestElements.Length; ++index2)
            {
              bool flag2 = requestInfo.Items.Length <= index2;
              this.requestElements[index2].canvas.set_alpha(!flag2 ? 1f : 0.0f);
              if (!flag2)
              {
                int category = requestInfo.Items[index2].Item1;
                int id = requestInfo.Items[index2].Item2;
                int count = requestInfo.Items[index2].Item3;
                this.usedItems.Add(new StuffItem(category, id, count));
                StuffItemInfo stuffItemInfo = gameInfo.GetItem(category, id);
                this.requestElements[index2].name.set_text(stuffItemInfo.Name);
                int num = 0;
                foreach (StuffItem stuffItem in itemList)
                {
                  if (stuffItem.CategoryID == category && stuffItem.ID == id)
                    num += stuffItem.Count;
                }
                this.requestElements[index2].count.set_text(itemSlotMax >= num ? string.Format("{0}", (object) num) : string.Format("{0}+", (object) itemSlotMax));
                this.requestElements[index2].need.set_text(count.ToString());
                bool flag3 = num < count;
                ((Graphic) this.requestElements[index2].count).set_color(!flag3 ? this.whiteColor : this.redColor);
                if (flag3)
                  flag1 = false;
              }
            }
            if (((Component) this.submitButton).get_gameObject().get_activeSelf() != flag1)
              ((Component) this.submitButton).get_gameObject().SetActive(flag1);
            this.IsImpossible = !flag1;
            break;
          case 1:
            this.requestCanvasGroup0.set_alpha(0.0f);
            this.requestCanvasGroup1.set_alpha(1f);
            this.requestCanvasGroup2.set_alpha(0.0f);
            this.requestText.set_text(requestInfo.Message.GetElement<string>(index1) ?? string.Empty);
            if (!((Component) this.submitButton).get_gameObject().get_activeSelf())
              break;
            ((Component) this.submitButton).get_gameObject().SetActive(false);
            break;
          case 2:
            this.requestCanvasGroup0.set_alpha(0.0f);
            this.requestCanvasGroup1.set_alpha(0.0f);
            this.requestCanvasGroup2.set_alpha(1f);
            this.requestText.set_text(string.Empty);
            int num1 = Manager.Map.GetTotalAgentFlavorAdditionAmount();
            int? nullable = requestInfo.Items.GetElement<Tuple<int, int, int>>(0)?.Item1;
            int num2 = !nullable.HasValue ? 9999 : nullable.Value;
            if (Singleton<Resources>.IsInstance())
            {
              Dictionary<int, int> additionBorderTable = Singleton<Resources>.Instance.PopupInfo.RequestFlavorAdditionBorderTable;
              if (!additionBorderTable.IsNullOrEmpty<int, int>())
              {
                foreach (KeyValuePair<int, int> keyValuePair in additionBorderTable)
                {
                  if (keyValuePair.Key < this.RequestID)
                    num1 -= keyValuePair.Value;
                  else
                    break;
                }
                if (num1 < 0)
                  num1 = 0;
              }
            }
            this.leftText_Type2.set_text(string.Format("{0}", (object) num1));
            this.rightText_Type2.set_text(string.Format("{0}", (object) num2));
            ((Graphic) this.leftText_Type2).set_color(num1 >= num2 ? this.whiteColor : this.redColor);
            if (!((Component) this.submitButton).get_gameObject().get_activeSelf())
              break;
            ((Component) this.submitButton).get_gameObject().SetActive(false);
            break;
          default:
            this.HideElements();
            break;
        }
      }
    }

    public void Open(AIProject.Definitions.Popup.Request.Type _type)
    {
      if (this.IsActiveControl)
        return;
      this.RequestID = (int) _type;
      this.IsActiveControl = true;
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RequestUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RequestUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void PlaySubmitSE()
    {
      this.PlaySE(SoundPack.SystemSE.OK_L);
    }

    private void PlayCancelSE()
    {
      this.PlaySE(SoundPack.SystemSE.Cancel);
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      if (Object.op_Equality((Object) soundPack, (Object) null))
        return;
      soundPack.Play(se);
    }

    [Serializable]
    public struct RequestElement
    {
      public CanvasGroup canvas;
      public Text name;
      public Text count;
      public Text need;
    }
  }
}
