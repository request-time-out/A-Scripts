// Decompiled with JetBrains decompiler
// Type: AIProject.UI.NotifyMessageList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI.Popup;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UniRx;
using UnityEngine;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  public class NotifyMessageList : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private int displayMaxElement;
    [SerializeField]
    private float popupWidth;
    [SerializeField]
    private float popupHeight;
    [SerializeField]
    private float nextPopupTime;
    [SerializeField]
    private bool fullNotPopup;
    [SerializeField]
    private float fadeInTime;
    [SerializeField]
    private float displayTime;
    [SerializeField]
    private float fadeOutTime;
    [SerializeField]
    private float startAlpha;
    [SerializeField]
    private float endAlpha;
    [SerializeField]
    private OneColor whiteColor;
    [SerializeField]
    private OneColor playerColor;
    [SerializeField]
    private OneColor[] personColors;
    private List<NotifyMessageElement> openElements;
    private List<NotifyMessageElement> closeElements;
    private List<string> messageStock;
    private GameObject _prefab;
    private bool visibled;
    private IDisposable fadeDisposable;
    private IDisposable nextLogChekerDisposable;
    private int elmCount;

    public NotifyMessageList()
    {
      base.\u002Ector();
    }

    public OneColor PersonColor(int _id)
    {
      return _id == -99 ? this.playerColor : this.personColors.GetElement<OneColor>(_id);
    }

    public string PersonName(int _id)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return "テストネーム";
      if (_id == -99)
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.Player.CharaName : (string) null;
      if (!Singleton<Manager.Map>.IsInstance())
        return (string) null;
      AgentActor agentActor;
      return Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(_id, ref agentActor) ? agentActor.CharaName : (string) null;
    }

    public void ClearStockMessage()
    {
      if (this.messageStock == null)
        return;
      this.messageStock.Clear();
    }

    private GameObject Prefab
    {
      get
      {
        if (Object.op_Inequality((Object) this._prefab, (Object) null))
          return this._prefab;
        DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
        this._prefab = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefab, "NotifyMessageElement", false, definePack.ABManifests.Default);
        return this._prefab;
      }
    }

    public bool Visibled
    {
      get
      {
        return this.visibled;
      }
      set
      {
        if (this.visibled == value)
          return;
        this.visibled = value;
        float _from = this.canvasGroup.get_alpha();
        float _to = !value ? 0.0f : 1f;
        if (this.fadeDisposable != null)
          this.fadeDisposable.Dispose();
        this.fadeDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), true), (Action<M0>) (x => this.canvasGroup.set_alpha(Mathf.Lerp(_from, _to, ((TimeInterval<float>) ref x).get_Value()))), (Action<Exception>) (ex => Debug.LogException(ex)));
      }
    }

    private void Awake()
    {
      this.openElements = ListPool<NotifyMessageElement>.Get();
      this.closeElements = ListPool<NotifyMessageElement>.Get();
      this.messageStock = ListPool<string>.Get();
      this.StartNextLogCheker();
    }

    private void StartNextLogCheker()
    {
      if (this.nextLogChekerDisposable != null)
        return;
      IEnumerator _coroutine = this.NextLogChecker();
      this.nextLogChekerDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
    }

    [DebuggerHidden]
    private IEnumerator NextLogChecker()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyMessageList.\u003CNextLogChecker\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      ListPool<NotifyMessageElement>.Release(this.openElements);
      ListPool<NotifyMessageElement>.Release(this.closeElements);
      ListPool<string>.Release(this.messageStock);
      if (this.nextLogChekerDisposable != null)
        this.nextLogChekerDisposable.Dispose();
      this.openElements = (List<NotifyMessageElement>) null;
      this.closeElements = (List<NotifyMessageElement>) null;
      this.messageStock = (List<string>) null;
    }

    private NotifyMessageElement GetElement()
    {
      NotifyMessageElement notifyMessageElement = !this.closeElements.IsNullOrEmpty<NotifyMessageElement>() ? this.closeElements.Pop<NotifyMessageElement>() : (NotifyMessageElement) null;
      if (Object.op_Equality((Object) notifyMessageElement, (Object) null))
      {
        notifyMessageElement = (NotifyMessageElement) ((GameObject) Object.Instantiate<GameObject>((M0) this.Prefab, ((Component) this).get_transform(), false))?.GetComponent<NotifyMessageElement>();
        if (Object.op_Equality((Object) notifyMessageElement, (Object) null))
          return (NotifyMessageElement) null;
        ((Object) ((Component) notifyMessageElement).get_gameObject()).set_name(string.Format("{0}_{1}", (object) ((Object) this.Prefab.get_gameObject()).get_name(), (object) this.elmCount++));
        notifyMessageElement.Root = this;
      }
      notifyMessageElement.EndActionEvent = new Action<NotifyMessageElement>(this.EndAction);
      return notifyMessageElement;
    }

    private void ReturnElement(NotifyMessageElement _elm)
    {
      if (((Component) _elm).get_gameObject().get_activeSelf())
        ((Component) _elm).get_gameObject().SetActive(false);
      if (this.openElements.Contains(_elm))
        this.openElements.Remove(_elm);
      if (this.closeElements.Contains(_elm))
        return;
      this.closeElements.Add(_elm);
    }

    private void EndAction(NotifyMessageElement _elm)
    {
      _elm.EndActionEvent = (Action<NotifyMessageElement>) null;
      this.ReturnElement(_elm);
    }

    public void AddMessage(string _message)
    {
      this.AddLog(_message ?? string.Empty);
    }

    public void AddMessage(int _actorID, string _message)
    {
      if (_message.IsNullOrEmpty() && _message == null)
        _message = string.Empty;
      OneColor oneColor = this.PersonColor(_actorID);
      string self = this.PersonName(_actorID) ?? string.Empty;
      if (self.IsNullOrEmpty() && _message.IsNullOrEmpty())
        Debug.LogWarning((object) "AddNotify: キャラ名もメッセージも空っぽだったので追加せず");
      else if (self.IsNullOrEmpty())
      {
        this.AddLog(_message);
      }
      else
      {
        if ((string) oneColor != (string) null)
        {
          StringBuilder toRelease = StringBuilderPool.Get();
          toRelease.AppendFormat("<color={0}>{1}</color>{2}", (object) oneColor, (object) self, (object) _message);
          _message = toRelease.ToString();
          StringBuilderPool.Release(toRelease);
        }
        else
        {
          StringBuilder toRelease = StringBuilderPool.Get();
          toRelease.AppendFormat("{0}{1}", (object) self, (object) _message);
          _message = toRelease.ToString();
          StringBuilderPool.Release(toRelease);
        }
        this.AddLog(_message);
      }
    }

    private void AddLog(string _message)
    {
      if (this.messageStock == null)
        return;
      this.messageStock.Add(_message);
    }

    private void PopupLog()
    {
      string str = this.messageStock.Pop<string>();
      NotifyMessageElement element = this.GetElement();
      if (Object.op_Equality((Object) element, (Object) null))
        return;
      element.SetTime(this.fadeInTime, this.displayTime, this.fadeOutTime);
      element.SetAlpha(this.startAlpha, this.endAlpha);
      element.LocalPosition = Vector3.get_zero();
      element.MessageText = str;
      element.MessageColor = (Color) this.whiteColor;
      element.SpeechBubbleIconActive = true;
      if (!((Component) element).get_gameObject().get_activeSelf())
        ((Component) element).get_gameObject().SetActive(true);
      ((Component) element).get_transform().SetAsLastSibling();
      element.StartFadeIn(this.popupWidth);
      float _posY = 0.0f;
      for (int index = 0; index < this.openElements.Count; ++index)
      {
        NotifyMessageElement openElement = this.openElements[index];
        if (Object.op_Equality((Object) openElement, (Object) null))
        {
          this.openElements.RemoveAt(index);
          --index;
        }
        else
        {
          openElement.SpeechBubbleIconActive = false;
          _posY += this.popupHeight;
          if (index <= this.displayMaxElement - 2)
            openElement.StartSlidUp(_posY);
          else if (!openElement.PlayingFadeOut)
            openElement.StartFadeOut();
        }
      }
      this.openElements.Insert(0, element);
    }
  }
}
