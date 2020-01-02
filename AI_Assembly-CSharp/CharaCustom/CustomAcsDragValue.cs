// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomAcsDragValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomAcsDragValue : MonoBehaviour
  {
    [SerializeField]
    private CustomAcsCorrectSet customAcsCorrectSet;
    [SerializeField]
    private int type;
    [SerializeField]
    private int xyz;
    private Image imgCol;

    public CustomAcsDragValue()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.imgCol = (Image) ((Component) this).GetComponent<Image>();
    }

    private void Start()
    {
      float backMousePos = 0.0f;
      ObservableEventTrigger observableEventTrigger = (ObservableEventTrigger) ((Component) this).get_gameObject().AddComponent<ObservableEventTrigger>();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) Observable.Select<Unit, float>(Observable.RepeatUntilDestroy<Unit>(Observable.TakeUntil<Unit, PointerEventData>(Observable.SkipUntil<Unit, PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (IObservable<M1>) Observable.Do<PointerEventData>((IObservable<M0>) observableEventTrigger.OnPointerDownAsObservable(), (Action<M0>) (_ =>
      {
        backMousePos = (float) Input.get_mousePosition().x;
        if (!Object.op_Implicit((Object) this.imgCol))
          return;
        ((Graphic) this.imgCol).set_color(new Color((float) ((Graphic) this.imgCol).get_color().r, (float) ((Graphic) this.imgCol).get_color().g, (float) ((Graphic) this.imgCol).get_color().b, 1f));
      }))), (IObservable<M1>) Observable.Do<PointerEventData>((IObservable<M0>) observableEventTrigger.OnPointerUpAsObservable(), (Action<M0>) (_ =>
      {
        if (!Object.op_Implicit((Object) this.imgCol))
          return;
        ((Graphic) this.imgCol).set_color(new Color((float) ((Graphic) this.imgCol).get_color().r, (float) ((Graphic) this.imgCol).get_color().g, (float) ((Graphic) this.imgCol).get_color().b, 0.0f));
      }))), (Component) this), (Func<M0, M1>) (_ => (float) Input.get_mousePosition().x - backMousePos)), (Action<M0>) (move =>
      {
        backMousePos = (float) Input.get_mousePosition().x;
        if (this.type == 0 && this.xyz == 0)
          move *= -1f;
        this.customAcsCorrectSet.UpdateDragValue(this.type, this.xyz, move);
      })), (Component) this);
    }
  }
}
