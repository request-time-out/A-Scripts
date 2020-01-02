// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomSliderSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomSliderSet : MonoBehaviour
  {
    public Text title;
    public Slider slider;
    public InputField input;
    public Button button;
    public Action<float> onChange;
    public Action onPointerUp;
    public Func<float> onSetDefaultValue;
    public Action onEndSetDefaultValue;

    public CustomSliderSet()
    {
      base.\u002Ector();
    }

    public void Reset()
    {
      for (int index = 0; index < ((Component) this).get_transform().get_childCount(); ++index)
      {
        Transform child = ((Component) this).get_transform().GetChild(index);
        switch (((Object) child).get_name())
        {
          case "Text":
            this.title = (Text) ((Component) child).GetComponent<Text>();
            break;
          case "Slider":
            this.slider = (Slider) ((Component) child).GetComponent<Slider>();
            break;
          case "SldInputField":
            this.input = (InputField) ((Component) child).GetComponent<InputField>();
            break;
          case "Button":
            this.button = (Button) ((Component) child).GetComponent<Button>();
            break;
        }
      }
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    public void SetSliderValue(float value)
    {
      if (!Object.op_Implicit((Object) this.slider))
        return;
      this.slider.set_value(value);
    }

    public void SetInputTextValue(string value)
    {
      if (!Object.op_Implicit((Object) this.input))
        return;
      this.input.set_text(value);
    }

    public void Start()
    {
      this.customBase.lstInputField.Add(this.input);
      if (Object.op_Implicit((Object) this.slider))
      {
        ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.slider.get_onValueChanged()), (Action<M0>) (value =>
        {
          if (this.onChange != null)
            this.onChange(value);
          if (!Object.op_Implicit((Object) this.input))
            return;
          this.input.set_text(CustomBase.ConvertTextFromRate(0, 100, value));
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.slider), (Action<M0>) (scl =>
        {
          if (!this.customBase.sliderControlWheel)
            return;
          this.slider.set_value(Mathf.Clamp(this.slider.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.0f, 100f));
        }));
        ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) this.slider), (Action<M0>) (_ =>
        {
          if (this.onPointerUp == null)
            return;
          this.onPointerUp();
        }));
      }
      if (Object.op_Implicit((Object) this.input))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.input.get_onEndEdit()), (Action<M0>) (value =>
        {
          if (!Object.op_Implicit((Object) this.slider))
            return;
          this.slider.set_value(CustomBase.ConvertRateFromText(0, 100, value));
        }));
      if (!Object.op_Implicit((Object) this.button))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityEventExtensions.AsObservable((UnityEvent) this.button.get_onClick()), (Action<M0>) (_ =>
      {
        if (this.onSetDefaultValue == null)
          return;
        float num = this.onSetDefaultValue();
        if (Object.op_Implicit((Object) this.slider))
        {
          if (Object.op_Implicit((Object) this.input) && (double) this.slider.get_value() != (double) num)
            this.input.set_text(CustomBase.ConvertTextFromRate(0, 100, num));
          this.slider.set_value(num);
        }
        if (this.onChange != null)
          this.onChange(num);
        if (this.onEndSetDefaultValue == null)
          return;
        this.onEndSetDefaultValue();
      }));
    }

    private void OnDestroy()
    {
      if (!Singleton<CustomBase>.IsInstance())
        return;
      this.customBase.lstInputField.Remove(this.input);
    }
  }
}
