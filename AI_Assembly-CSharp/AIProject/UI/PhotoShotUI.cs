// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PhotoShotUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ConfigScene;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class PhotoShotUI : MenuUIBehaviour
  {
    [SerializeField]
    private PhotoShotUI.GuidObject[] _guidElements = new PhotoShotUI.GuidObject[0];
    private bool _displayGuide = true;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Scrollbar _zoomValueBar;
    private IObservable<TimeInterval<float>> _lerpStream;
    private IDisposable _fadeDisposable;

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

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      this._lerpStream = (IObservable<TimeInterval<float>>) Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._alphaAccelerationTime, true), true), (Component) this);
    }

    protected override void OnAfterStart()
    {
      if (this._canvasGroup.get_blocksRaycasts())
        this._canvasGroup.set_blocksRaycasts(false);
      if (!this._canvasGroup.get_interactable())
        return;
      this._canvasGroup.set_interactable(false);
    }

    private void SetActiveControl(bool active)
    {
      if (active)
        this.UISetting();
      IEnumerator fadeCoroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => fadeCoroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PhotoShotUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PhotoShotUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void ElementSetting(PhotoShotUI.GuidObject obj, Sprite icon, string text)
    {
      if (obj == null)
        return;
      if (Object.op_Inequality((Object) obj.image, (Object) null) && Object.op_Inequality((Object) ((Component) obj.image).get_gameObject(), (Object) null))
      {
        obj.image.set_sprite(icon);
        this.SetActive((Component) obj.image, this._displayGuide);
      }
      if (!Object.op_Inequality((Object) obj.text, (Object) null) || !Object.op_Inequality((Object) ((Component) obj.text).get_gameObject(), (Object) null))
        return;
      obj.text.set_text(text);
      this.SetActive((Component) obj.text, this._displayGuide);
    }

    private void UISetting()
    {
      GameConfigSystem gameData = Manager.Config.GameData;
      this._displayGuide = gameData == null || gameData.ActionGuide;
      if (!Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Sprite> inputIconTable = Singleton<Resources>.Instance.itemIconTables.InputIconTable;
      Sprite icon;
      inputIconTable.TryGetValue(0, out icon);
      this.ElementSetting(this._guidElements.GetElement<PhotoShotUI.GuidObject>(0), icon, "移動");
      inputIconTable.TryGetValue(2, out icon);
      this.ElementSetting(this._guidElements.GetElement<PhotoShotUI.GuidObject>(1), icon, "ズーム");
      inputIconTable.TryGetValue(6, out icon);
      this.ElementSetting(this._guidElements.GetElement<PhotoShotUI.GuidObject>(2), icon, "撮影");
      inputIconTable.TryGetValue(1, out icon);
      this.ElementSetting(this._guidElements.GetElement<PhotoShotUI.GuidObject>(3), icon, "終了");
    }

    public void SetZoomValue(float value)
    {
      if (Object.op_Equality((Object) this._zoomValueBar, (Object) null))
        return;
      this._zoomValueBar.set_value(Mathf.Clamp01(value));
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (Object.op_Equality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }

    private void SetActive(Component com, bool active)
    {
      if (Object.op_Equality((Object) com, (Object) null) || Object.op_Equality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return;
      com.get_gameObject().SetActive(active);
    }

    [Serializable]
    public class GuidObject
    {
      public Text text;
      public Image image;
    }
  }
}
