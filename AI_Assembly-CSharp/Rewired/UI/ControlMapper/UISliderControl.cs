// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class UISliderControl : UIControl
  {
    public Image iconImage;
    public Slider slider;
    private bool _showIcon;
    private bool _showSlider;

    public bool showIcon
    {
      get
      {
        return this._showIcon;
      }
      set
      {
        if (Object.op_Equality((Object) this.iconImage, (Object) null))
          return;
        ((Component) this.iconImage).get_gameObject().SetActive(value);
        this._showIcon = value;
      }
    }

    public bool showSlider
    {
      get
      {
        return this._showSlider;
      }
      set
      {
        if (Object.op_Equality((Object) this.slider, (Object) null))
          return;
        ((Component) this.slider).get_gameObject().SetActive(value);
        this._showSlider = value;
      }
    }

    public override void SetCancelCallback(Action cancelCallback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UISliderControl.\u003CSetCancelCallback\u003Ec__AnonStorey0 callbackCAnonStorey0 = new UISliderControl.\u003CSetCancelCallback\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      callbackCAnonStorey0.cancelCallback = cancelCallback;
      // ISSUE: reference to a compiler-generated field
      base.SetCancelCallback(callbackCAnonStorey0.cancelCallback);
      // ISSUE: reference to a compiler-generated field
      if (callbackCAnonStorey0.cancelCallback == null || Object.op_Equality((Object) this.slider, (Object) null))
        return;
      if (this.slider is ICustomSelectable)
      {
        // ISSUE: method pointer
        (this.slider as ICustomSelectable).CancelEvent += new UnityAction((object) callbackCAnonStorey0, __methodptr(\u003C\u003Em__0));
      }
      else
      {
        EventTrigger eventTrigger = (EventTrigger) ((Component) this.slider).GetComponent<EventTrigger>();
        if (Object.op_Equality((Object) eventTrigger, (Object) null))
          eventTrigger = (EventTrigger) ((Component) this.slider).get_gameObject().AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.callback = (__Null) new EventTrigger.TriggerEvent();
        entry.eventID = (__Null) 16;
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) entry.callback).AddListener(new UnityAction<BaseEventData>((object) callbackCAnonStorey0, __methodptr(\u003C\u003Em__1)));
        if (eventTrigger.get_triggers() == null)
          eventTrigger.set_triggers(new List<EventTrigger.Entry>());
        eventTrigger.get_triggers().Add(entry);
      }
    }
  }
}
