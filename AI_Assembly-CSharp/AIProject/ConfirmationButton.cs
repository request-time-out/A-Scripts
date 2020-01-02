// Decompiled with JetBrains decompiler
// Type: AIProject.ConfirmationButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class ConfirmationButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IEventSystemHandler
  {
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Image _selectedFrame;
    private BoolReactiveProperty _activeSelectedFrame;

    public ConfirmationButton()
    {
      base.\u002Ector();
    }

    public bool IsActiveSelectedFrame
    {
      get
      {
        return ((ReactiveProperty<bool>) this._activeSelectedFrame).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._activeSelectedFrame).set_Value(value);
      }
    }

    public UnityEvent OnPointerDownEvent { get; private set; }

    public UnityEvent OnPointerUpEvent { get; private set; }

    public UnityEvent OnPointerEnterEvent { get; private set; }

    public UnityEvent OnPointerExitEvent { get; private set; }

    private void SetCoverEnabled(bool active)
    {
      if (!Object.op_Equality((Object) this._image, (Object) null))
        return;
      Color color = ((Graphic) this._image).get_color();
      color.a = !active ? (__Null) 0.0 : (__Null) 1.0;
      ((Graphic) this._image).set_color(color);
    }

    public void AddListener(Action act)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ConfirmationButton.\u003CAddListener\u003Ec__AnonStorey0 listenerCAnonStorey0 = new ConfirmationButton.\u003CAddListener\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      listenerCAnonStorey0.act = act;
      if (!((Selectable) this._button).IsInteractable() || this._button == null)
        return;
      // ISSUE: method pointer
      ((UnityEvent) this._button.get_onClick()).AddListener(new UnityAction((object) listenerCAnonStorey0, __methodptr(\u003C\u003Em__0)));
    }

    public void Invoke()
    {
      ((UnityEvent) this._button.get_onClick())?.Invoke();
    }

    public void DisableRaycast()
    {
      ((Graphic) this._image).set_raycastTarget(false);
    }

    private void Reset()
    {
      this._button = (Button) ((Component) this).GetComponent<Button>();
      this._image = (Image) ((Component) this).GetComponent<Image>();
    }

    private void Awake()
    {
      // ISSUE: method pointer
      this.OnPointerEnterEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__0)));
      // ISSUE: method pointer
      this.OnPointerExitEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__1)));
      this.SetCoverEnabled(false);
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._activeSelectedFrame, (Action<M0>) (x =>
      {
        if (x)
          this.VisibleSelectedFrame();
        else
          this.ImvisibleSelectedFrame();
      }));
    }

    private void VisibleSelectedFrame()
    {
    }

    private void ImvisibleSelectedFrame()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      this.OnPointerDownEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      this.OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this.OnPointerExitEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      this.OnPointerUpEvent?.Invoke();
    }
  }
}
