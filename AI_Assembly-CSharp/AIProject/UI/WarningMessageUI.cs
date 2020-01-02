// Decompiled with JetBrains decompiler
// Type: AIProject.UI.WarningMessageUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI.Popup;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.UI
{
  [RequireComponent(typeof (CanvasGroup))]
  public class WarningMessageUI : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private bool existsNotPupup;
    [SerializeField]
    private float nextPopupTime;
    [SerializeField]
    private Transform[] roots;
    [SerializeField]
    private FadeInfo fadeInInfo;
    [SerializeField]
    private FadeInfo displayInfo;
    [SerializeField]
    private FadeInfo fadeOutInfo;
    [SerializeField]
    private Color whiteColor;
    [SerializeField]
    private Color yellowColor;
    [SerializeField]
    private Color redColor;
    private ValueTuple<string, string> PrefabAssetBundle;
    private GameObject prefab_;
    private bool visibled;
    private IDisposable visibleFadeDisposable;
    private List<WarningMessageElement> openElements;
    private List<WarningMessageElement> closeElements;
    private List<ValueTuple<string, int, Transform, System.Action>> messageStock;
    private IDisposable nextMessageCheckerDisposable;
    private int elmCount;

    public WarningMessageUI()
    {
      base.\u002Ector();
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this.canvasGroup, (Object) null) ? this.canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this.canvasGroup, (Object) null))
          return;
        this.canvasGroup.set_alpha(value);
      }
    }

    private GameObject Prefab
    {
      get
      {
        if (Object.op_Inequality((Object) this.prefab_, (Object) null))
          return this.prefab_;
        DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
        this.prefab_ = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefab, "WarningMessageElement", false, definePack.ABManifests.Default);
        if (Object.op_Inequality((Object) this.prefab_, (Object) null))
          this.PrefabAssetBundle = new ValueTuple<string, string>(definePack.ABPaths.MapScenePrefab, definePack.ABManifests.Default);
        return this.prefab_;
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
        float _startAlpha = this.CanvasAlpha;
        float _endAlpha = !value ? 0.0f : 1f;
        if (this.visibleFadeDisposable != null)
          this.visibleFadeDisposable.Dispose();
        this.visibleFadeDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), ((Component) this).get_gameObject()), true), (System.Action<M0>) (x => this.CanvasAlpha = Mathf.Lerp(_startAlpha, _endAlpha, ((TimeInterval<float>) ref x).get_Value())), (System.Action<Exception>) (ex => Debug.LogException(ex)));
      }
    }

    public bool isFadeInForOutWait
    {
      get
      {
        return !Object.op_Equality((Object) this.currentElement, (Object) null) && this.currentElement.isFadeInForOutWait;
      }
      set
      {
        this.isReserveFadeInForOutWait = value;
        if (!Object.op_Inequality((Object) this.currentElement, (Object) null))
          return;
        this.currentElement.isFadeInForOutWait = value;
      }
    }

    private bool isReserveFadeInForOutWait { get; set; }

    private WarningMessageElement currentElement { get; set; }

    private void Awake()
    {
      this.openElements = ListPool<WarningMessageElement>.Get();
      this.closeElements = ListPool<WarningMessageElement>.Get();
      this.messageStock = ListPool<ValueTuple<string, int, Transform, System.Action>>.Get();
      this.StartNextMessageCheker();
    }

    private void OnDestroy()
    {
      if (this.nextMessageCheckerDisposable != null)
        this.nextMessageCheckerDisposable.Dispose();
      ListPool<WarningMessageElement>.Release(this.openElements);
      ListPool<WarningMessageElement>.Release(this.closeElements);
      ListPool<ValueTuple<string, int, Transform, System.Action>>.Release(this.messageStock);
      this.openElements = (List<WarningMessageElement>) null;
      this.closeElements = (List<WarningMessageElement>) null;
      this.messageStock = (List<ValueTuple<string, int, Transform, System.Action>>) null;
      this.currentElement = (WarningMessageElement) null;
    }

    public void ClearStockMessage()
    {
      if (this.messageStock == null)
        return;
      this.messageStock.Clear();
    }

    private void StartNextMessageCheker()
    {
      if (this.nextMessageCheckerDisposable != null)
        return;
      IEnumerator _coroutine = this.NextMessageChecker();
      this.nextMessageCheckerDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this));
    }

    [DebuggerHidden]
    private IEnumerator NextMessageChecker()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningMessageUI.\u003CNextMessageChecker\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private Color GetColor(int _id)
    {
      if (_id == 1)
        return this.yellowColor;
      return _id == 2 ? this.redColor : this.whiteColor;
    }

    private WarningMessageElement GetElement()
    {
      WarningMessageElement warningMessageElement = !this.closeElements.IsNullOrEmpty<WarningMessageElement>() ? this.closeElements.Pop<WarningMessageElement>() : (WarningMessageElement) null;
      if (Object.op_Equality((Object) warningMessageElement, (Object) null))
      {
        if (Object.op_Equality((Object) this.Prefab, (Object) null))
          return (WarningMessageElement) null;
        warningMessageElement = (WarningMessageElement) ((GameObject) Object.Instantiate<GameObject>((M0) this.Prefab, ((Component) this).get_transform(), false))?.GetComponent<WarningMessageElement>();
        if (Object.op_Equality((Object) warningMessageElement, (Object) null))
          return (WarningMessageElement) null;
        ((Object) ((Component) warningMessageElement).get_gameObject()).set_name(string.Format("{0}_{1}", (object) ((Object) this.Prefab).get_name(), (object) this.elmCount++));
        warningMessageElement.Root = this;
      }
      if (((Component) warningMessageElement).get_gameObject().get_activeSelf())
        ((Component) warningMessageElement).get_gameObject().SetActive(false);
      warningMessageElement.EndAction = new System.Action<WarningMessageElement>(this.EndAction);
      return warningMessageElement;
    }

    private void EndAction(WarningMessageElement _elm)
    {
      _elm.isFadeInForOutWait = false;
      _elm.EndAction = (System.Action<WarningMessageElement>) null;
      this.ReturnElement(_elm);
    }

    private void ReturnElement(WarningMessageElement _elm)
    {
      this.currentElement = (WarningMessageElement) null;
      if (((Component) _elm).get_gameObject().get_activeSelf())
        ((Component) _elm).get_gameObject().SetActive(false);
      if (this.openElements.Contains(_elm))
        this.openElements.Remove(_elm);
      if (this.closeElements.Contains(_elm))
        return;
      this.closeElements.Add(_elm);
    }

    public void ShowMessage(
      AIProject.Definitions.Popup.Warning.Type _type,
      int _colorID,
      int _posID = 0,
      System.Action _onComplete = null)
    {
      string[] source;
      if (Singleton<Resources>.Instance.PopupInfo.WarningTable.TryGetValue((int) _type, ref source))
        ;
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      this.ShowMessage((source != null ? source.GetElement<string>(index) : (string) null) ?? string.Empty, _colorID, _posID, _onComplete);
    }

    public void ShowMessage(AIProject.Definitions.Popup.Warning.Type _type)
    {
      this.ShowMessage(_type, 2, 0, (System.Action) null);
    }

    public void ShowMessage(string _message, int _colorID, int _posID, System.Action _onComplete)
    {
      if (_message.IsNullOrEmpty())
        return;
      using (List<ValueTuple<string, int, Transform, System.Action>>.Enumerator enumerator = this.messageStock.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if ((string) enumerator.Current.Item1 == _message)
            return;
        }
      }
      foreach (WarningMessageElement openElement in this.openElements)
      {
        if (openElement.Text == _message)
        {
          if (openElement.PlayingDisplay)
          {
            openElement.StartDisplay();
            return;
          }
          if (!openElement.PlayingFadeOut || this.openElements.Count != 1)
            return;
          openElement.StartFadeIn();
          return;
        }
      }
      Transform element = this.roots.GetElement<Transform>(_posID);
      this.AddMessage(_message, _colorID, element, _onComplete);
    }

    private void AddMessage(string _message, int _colorID, Transform _root, System.Action onComplete)
    {
      this.messageStock.Add(new ValueTuple<string, int, Transform, System.Action>(_message, _colorID, _root, onComplete));
    }

    private void PopupWarning()
    {
      ValueTuple<string, int, Transform, System.Action> valueTuple = this.messageStock.Pop<ValueTuple<string, int, Transform, System.Action>>();
      WarningMessageElement element = this.GetElement();
      this.currentElement = element;
      if (Object.op_Equality((Object) element, (Object) null))
        return;
      element.ClosedAction = (System.Action) valueTuple.Item4;
      element.isFadeInForOutWait = this.isReserveFadeInForOutWait;
      element.SetFadeInfo(this.fadeInInfo, this.displayInfo, this.fadeOutInfo);
      element.Text = (string) valueTuple.Item1;
      element.Color = this.GetColor((int) valueTuple.Item2);
      if (Object.op_Inequality((Object) valueTuple.Item3, (Object) null))
        ((Component) element).get_transform().set_localPosition(((Transform) valueTuple.Item3).get_localPosition());
      if (!((Component) element).get_gameObject().get_activeSelf())
        ((Component) element).get_gameObject().SetActive(true);
      ((Component) element).get_transform().SetAsLastSibling();
      element.StartFadeIn();
      switch ((int) valueTuple.Item2)
      {
        case 1:
        case 2:
          this.PlaySE();
          break;
      }
      foreach (WarningMessageElement openElement in this.openElements)
      {
        if (!openElement.PlayingFadeOut)
          openElement.StartFadeOut();
      }
      this.openElements.Add(element);
    }

    private void PlaySE()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      if (Object.op_Equality((Object) soundPack, (Object) null))
        return;
      soundPack.Play(SoundPack.SystemSE.Error);
    }
  }
}
