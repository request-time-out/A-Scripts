// Decompiled with JetBrains decompiler
// Type: UI_ToggleOnOffEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ToggleOnOffEx : Toggle
{
  [SerializeField]
  private Image imgOn;
  [SerializeField]
  private Image imgOff;
  [SerializeField]
  private Image imgOnOver;
  [SerializeField]
  private Image imgOffOver;

  public UI_ToggleOnOffEx()
  {
    base.\u002Ector();
  }

  private void Initialize()
  {
    if (Object.op_Equality((Object) null, (Object) this.imgOn))
    {
      Transform transform = ((Component) this).get_transform().Find("imgOn");
      if (Object.op_Inequality((Object) null, (Object) transform))
        this.imgOn = (Image) ((Component) transform).GetComponent<Image>();
    }
    if (Object.op_Equality((Object) null, (Object) this.imgOff))
    {
      Transform transform = ((Component) this).get_transform().Find("imgOff");
      if (Object.op_Inequality((Object) null, (Object) transform))
        this.imgOff = (Image) ((Component) transform).GetComponent<Image>();
    }
    if (Object.op_Equality((Object) null, (Object) this.imgOnOver))
    {
      Transform transform = ((Component) this).get_transform().Find("imgOnOver");
      if (Object.op_Inequality((Object) null, (Object) transform))
        this.imgOnOver = (Image) ((Component) transform).GetComponent<Image>();
      if (Object.op_Inequality((Object) null, (Object) this.imgOnOver))
        ((Behaviour) this.imgOnOver).set_enabled(false);
    }
    if (!Object.op_Equality((Object) null, (Object) this.imgOffOver))
      return;
    Transform transform1 = ((Component) this).get_transform().Find("imgOffOver");
    if (Object.op_Inequality((Object) null, (Object) transform1))
      this.imgOffOver = (Image) ((Component) transform1).GetComponent<Image>();
    if (!Object.op_Inequality((Object) null, (Object) this.imgOffOver))
      return;
    ((Behaviour) this.imgOffOver).set_enabled(false);
  }

  protected virtual void Start()
  {
    base.Start();
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) this), (Action<M0>) (isOn =>
    {
      if (Object.op_Inequality((Object) null, (Object) this.imgOn))
        ((Component) this.imgOn).get_gameObject().SetActiveIfDifferent(isOn);
      if (Object.op_Inequality((Object) null, (Object) this.imgOnOver))
        ((Component) this.imgOnOver).get_gameObject().SetActiveIfDifferent(isOn);
      if (Object.op_Inequality((Object) null, (Object) this.imgOff))
        ((Component) this.imgOff).get_gameObject().SetActiveIfDifferent(!isOn);
      if (!Object.op_Inequality((Object) null, (Object) this.imgOffOver))
        return;
      ((Component) this.imgOffOver).get_gameObject().SetActiveIfDifferent(!isOn);
    }));
  }

  public virtual void OnPointerEnter(PointerEventData eventData)
  {
    ((Selectable) this).OnPointerEnter(eventData);
    if (Object.op_Inequality((Object) null, (Object) this.imgOnOver))
      ((Behaviour) this.imgOnOver).set_enabled(((Selectable) this).get_interactable());
    if (!Object.op_Inequality((Object) null, (Object) this.imgOffOver))
      return;
    ((Behaviour) this.imgOffOver).set_enabled(((Selectable) this).get_interactable());
  }

  public virtual void OnPointerExit(PointerEventData eventData)
  {
    ((Selectable) this).OnPointerExit(eventData);
    if (Object.op_Inequality((Object) null, (Object) this.imgOnOver))
      ((Behaviour) this.imgOnOver).set_enabled(false);
    if (!Object.op_Inequality((Object) null, (Object) this.imgOffOver))
      return;
    ((Behaviour) this.imgOffOver).set_enabled(false);
  }
}
