// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControlSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class UIControlSet : MonoBehaviour
  {
    [SerializeField]
    private Text title;
    private Dictionary<int, UIControl> _controls;

    public UIControlSet()
    {
      base.\u002Ector();
    }

    private Dictionary<int, UIControl> controls
    {
      get
      {
        return this._controls ?? (this._controls = new Dictionary<int, UIControl>());
      }
    }

    public void SetTitle(string text)
    {
      if (Object.op_Equality((Object) this.title, (Object) null))
        return;
      this.title.set_text(text);
    }

    public T GetControl<T>(int uniqueId) where T : UIControl
    {
      UIControl uiControl;
      this.controls.TryGetValue(uniqueId, out uiControl);
      return uiControl as T;
    }

    public UISliderControl CreateSlider(
      GameObject prefab,
      Sprite icon,
      float minValue,
      float maxValue,
      Action<int, float> valueChangedCallback,
      Action<int> cancelCallback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIControlSet.\u003CCreateSlider\u003Ec__AnonStorey0 sliderCAnonStorey0 = new UIControlSet.\u003CCreateSlider\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      sliderCAnonStorey0.valueChangedCallback = valueChangedCallback;
      // ISSUE: reference to a compiler-generated field
      sliderCAnonStorey0.cancelCallback = cancelCallback;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) prefab);
      // ISSUE: reference to a compiler-generated field
      sliderCAnonStorey0.control = (UISliderControl) gameObject.GetComponent<UISliderControl>();
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) sliderCAnonStorey0.control, (Object) null))
      {
        Object.Destroy((Object) gameObject);
        Debug.LogError((object) "Prefab missing UISliderControl component!");
        return (UISliderControl) null;
      }
      gameObject.get_transform().SetParent(((Component) this).get_transform(), false);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Inequality((Object) sliderCAnonStorey0.control.iconImage, (Object) null))
      {
        // ISSUE: reference to a compiler-generated field
        sliderCAnonStorey0.control.iconImage.set_sprite(icon);
      }
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Inequality((Object) sliderCAnonStorey0.control.slider, (Object) null))
      {
        // ISSUE: reference to a compiler-generated field
        sliderCAnonStorey0.control.slider.set_minValue(minValue);
        // ISSUE: reference to a compiler-generated field
        sliderCAnonStorey0.control.slider.set_maxValue(maxValue);
        // ISSUE: reference to a compiler-generated field
        if (sliderCAnonStorey0.valueChangedCallback != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent<float>) sliderCAnonStorey0.control.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) sliderCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        }
        // ISSUE: reference to a compiler-generated field
        if (sliderCAnonStorey0.cancelCallback != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          sliderCAnonStorey0.control.SetCancelCallback(new Action(sliderCAnonStorey0.\u003C\u003Em__1));
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.controls.Add(sliderCAnonStorey0.control.id, (UIControl) sliderCAnonStorey0.control);
      // ISSUE: reference to a compiler-generated field
      return sliderCAnonStorey0.control;
    }
  }
}
