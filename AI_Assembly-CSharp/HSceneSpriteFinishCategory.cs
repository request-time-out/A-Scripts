// Decompiled with JetBrains decompiler
// Type: HSceneSpriteFinishCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class HSceneSpriteFinishCategory : HSceneSpriteCategory
{
  [Space]
  [SerializeField]
  private float[] MainPosY = new float[2];
  private BoolReactiveProperty BeforeActive = new BoolReactiveProperty(false);
  [SerializeField]
  private GameObject atari;
  [SerializeField]
  private Button Finish;
  private bool onFinish;
  [SerializeField]
  private Button[] FinishPattern;
  [SerializeField]
  [Space]
  private float smoothTime;
  private GameObject BeforeObj;
  private RectTransform rt;
  private bool oldBeforeActive;

  public void Init()
  {
    if (Object.op_Equality((Object) this.atari.GetComponent<PointerEnterTrigger>(), (Object) null))
    {
      UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
      ((UITrigger) this.atari.AddComponent<PointerEnterTrigger>()).get_Triggers().Add(triggerEvent);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CInit\u003Em__0)));
    }
    if (Object.op_Equality((Object) this.atari.GetComponent<PointerExitTrigger>(), (Object) null))
    {
      UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
      ((UITrigger) this.atari.AddComponent<PointerExitTrigger>()).get_Triggers().Add(triggerEvent);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CInit\u003Em__1)));
    }
    this.rt = (RectTransform) ((Component) this.Finish).GetComponent<RectTransform>();
    this.BeforeObj = ((Component) this.Finish).get_gameObject();
    ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this.BeforeActive, (Func<M0, bool>) (active => active != this.oldBeforeActive)), (Action<M0>) (active =>
    {
      this.oldBeforeActive = active;
      this.atari.SetActive(active);
    }));
    ((Component) this).get_gameObject().SetActive(true);
  }

  private void Update()
  {
    ((ReactiveProperty<bool>) this.BeforeActive).set_Value(this.BeforeObj.get_activeSelf());
    if (!this.atari.get_activeSelf())
      return;
    float deltaTime = Time.get_deltaTime();
    Vector3.get_zero();
    float num = 0.0f;
    Vector3 vector3 = Vector2.op_Implicit(this.rt.get_anchoredPosition());
    vector3.y = !this.onFinish ? (__Null) (double) Mathf.SmoothDamp((float) vector3.y, this.MainPosY[1], ref num, this.smoothTime, float.PositiveInfinity, deltaTime) : (__Null) (double) Mathf.SmoothDamp((float) vector3.y, this.MainPosY[0], ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
    this.rt.set_anchoredPosition(Vector2.op_Implicit(vector3));
  }
}
