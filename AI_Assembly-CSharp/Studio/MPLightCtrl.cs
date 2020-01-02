// Decompiled with JetBrains decompiler
// Type: Studio.MPLightCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPLightCtrl : MonoBehaviour
  {
    [SerializeField]
    private MPLightCtrl.BackgroundInfo backgroundInfoDirectional;
    [SerializeField]
    private MPLightCtrl.BackgroundInfo backgroundInfoPoint;
    [SerializeField]
    private MPLightCtrl.BackgroundInfo backgroundInfoSpot;
    [SerializeField]
    private Image imageSample;
    [SerializeField]
    private Button buttonSample;
    [SerializeField]
    private Toggle toggleVisible;
    [SerializeField]
    private Toggle toggleTarget;
    [SerializeField]
    private Toggle toggleShadow;
    [SerializeField]
    private MPLightCtrl.ValueInfo viIntensity;
    [SerializeField]
    private MPLightCtrl.ValueInfo viRange;
    [SerializeField]
    private MPLightCtrl.ValueInfo viSpotAngle;
    private OCILight m_OCILight;
    private bool isUpdateInfo;
    private bool isColorFunc;

    public MPLightCtrl()
    {
      base.\u002Ector();
    }

    public OCILight ociLight
    {
      get
      {
        return this.m_OCILight;
      }
      set
      {
        this.m_OCILight = value;
        if (this.m_OCILight == null)
          return;
        this.UpdateInfo();
      }
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        ((Component) this).get_gameObject().SetActive(value);
        if (((Component) this).get_gameObject().get_activeSelf())
          return;
        if (this.isColorFunc)
          Singleton<Studio.Studio>.Instance.colorPalette.Close();
        this.isColorFunc = false;
      }
    }

    public bool Deselect(OCILight _ociLight)
    {
      if (this.m_OCILight != _ociLight)
        return false;
      this.ociLight = (OCILight) null;
      this.active = false;
      return true;
    }

    private void UpdateInfo()
    {
      this.isUpdateInfo = true;
      if (this.m_OCILight == null)
        return;
      LightType lightType1 = this.m_OCILight.lightType;
      if (lightType1 != 1)
      {
        if (lightType1 != 2)
        {
          if (lightType1 == null)
          {
            this.viRange.slider.set_minValue(0.5f);
            this.viRange.slider.set_maxValue(100f);
          }
        }
        else
        {
          this.viRange.slider.set_minValue(0.1f);
          this.viRange.slider.set_maxValue(100f);
        }
      }
      this.toggleVisible.set_isOn(this.m_OCILight.lightInfo.enable);
      this.toggleTarget.set_isOn(this.m_OCILight.lightInfo.drawTarget);
      this.toggleShadow.set_isOn(this.m_OCILight.lightInfo.shadow);
      if (Object.op_Implicit((Object) this.imageSample))
        ((Graphic) this.imageSample).set_color(this.m_OCILight.lightInfo.color);
      this.viIntensity.slider.set_value(this.m_OCILight.lightInfo.intensity);
      this.viIntensity.inputField.set_text(this.m_OCILight.lightInfo.intensity.ToString("0.000"));
      this.viRange.slider.set_value(this.m_OCILight.lightInfo.range);
      this.viRange.inputField.set_text(this.m_OCILight.lightInfo.range.ToString("0.000"));
      this.viSpotAngle.slider.set_value(this.m_OCILight.lightInfo.spotAngle);
      this.viSpotAngle.inputField.set_text(this.m_OCILight.lightInfo.spotAngle.ToString("0.000"));
      LightType lightType2 = this.m_OCILight.lightType;
      if (lightType2 != 1)
      {
        if (lightType2 != 2)
        {
          if (lightType2 == null)
          {
            this.backgroundInfoDirectional.active = false;
            this.backgroundInfoPoint.active = false;
            this.backgroundInfoSpot.active = true;
            this.backgroundInfoSpot.target = this.m_OCILight.lightTarget;
            this.viRange.active = true;
            this.viSpotAngle.active = true;
          }
        }
        else
        {
          this.backgroundInfoDirectional.active = false;
          this.backgroundInfoPoint.active = true;
          this.backgroundInfoPoint.target = this.m_OCILight.lightTarget;
          this.backgroundInfoSpot.active = false;
          this.viRange.active = true;
          this.viSpotAngle.active = false;
        }
      }
      else
      {
        this.backgroundInfoDirectional.active = true;
        this.backgroundInfoDirectional.target = this.m_OCILight.lightTarget;
        this.backgroundInfoPoint.active = false;
        this.backgroundInfoSpot.active = false;
        this.viRange.active = false;
        this.viSpotAngle.active = false;
      }
      this.isUpdateInfo = false;
    }

    private void OnClickColor()
    {
      Singleton<Studio.Studio>.Instance.colorPalette.Setup("ライト", this.m_OCILight.lightInfo.color, new Action<Color>(this.OnValueChangeColor), false);
      this.isColorFunc = true;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = true;
    }

    private void OnValueChangeColor(Color _color)
    {
      if (this.m_OCILight != null)
        this.m_OCILight.SetColor(_color);
      if (!Object.op_Implicit((Object) this.imageSample))
        return;
      ((Graphic) this.imageSample).set_color(_color);
    }

    private void OnValueChangeEnable(bool _value)
    {
      this.m_OCILight.SetEnable(_value, false);
    }

    private void OnValueChangeDrawTarget(bool _value)
    {
      this.m_OCILight.SetDrawTarget(_value, false);
    }

    private void OnValueChangeShadow(bool _value)
    {
      this.m_OCILight.SetShadow(_value, false);
    }

    private void OnValueChangeIntensity(float _value)
    {
      if (this.isUpdateInfo || !this.m_OCILight.SetIntensity(Mathf.Clamp(_value, 0.1f, 2f), false))
        return;
      this.viIntensity.inputField.set_text(this.m_OCILight.lightInfo.intensity.ToString("0.00"));
    }

    private void OnEndEditIntensity(string _text)
    {
      this.m_OCILight.SetIntensity(Mathf.Clamp(Utility.StringToFloat(_text), 0.1f, 2f), false);
      this.viIntensity.inputField.set_text(this.m_OCILight.lightInfo.intensity.ToString("0.00"));
      this.viIntensity.slider.set_value(this.m_OCILight.lightInfo.intensity);
    }

    private void OnClickIntensity()
    {
      if (!this.m_OCILight.SetIntensity(1f, false))
        return;
      this.viIntensity.inputField.set_text(this.m_OCILight.lightInfo.intensity.ToString("0.00"));
      this.viIntensity.slider.set_value(this.m_OCILight.lightInfo.intensity);
    }

    private void OnValueChangeRange(float _value)
    {
      if (this.isUpdateInfo || !this.m_OCILight.SetRange(_value, false))
        return;
      this.viRange.inputField.set_text(this.m_OCILight.lightInfo.range.ToString("0.000"));
    }

    private void OnEndEditRange(string _text)
    {
      this.m_OCILight.SetRange(Mathf.Max(this.m_OCILight.lightType != null ? 0.1f : 0.5f, Utility.StringToFloat(_text)), false);
      this.viRange.inputField.set_text(this.m_OCILight.lightInfo.range.ToString("0.000"));
      this.viRange.slider.set_value(this.m_OCILight.lightInfo.range);
    }

    private void OnClickRange()
    {
      if (!this.m_OCILight.SetRange(30f, false))
        return;
      this.viRange.inputField.set_text(this.m_OCILight.lightInfo.range.ToString("0.000"));
      this.viRange.slider.set_value(this.m_OCILight.lightInfo.range);
    }

    private void OnValueChangeSpotAngle(float _value)
    {
      if (this.isUpdateInfo || !this.m_OCILight.SetSpotAngle(_value, false))
        return;
      this.viSpotAngle.inputField.set_text(this.m_OCILight.lightInfo.spotAngle.ToString("0.000"));
    }

    private void OnEndEditSpotAngle(string _text)
    {
      this.m_OCILight.SetSpotAngle(Mathf.Clamp(Utility.StringToFloat(_text), 1f, 179f), false);
      this.viSpotAngle.inputField.set_text(this.m_OCILight.lightInfo.spotAngle.ToString("0.000"));
      this.viSpotAngle.slider.set_value(this.m_OCILight.lightInfo.spotAngle);
    }

    private void OnClickSpotAngle()
    {
      if (!this.m_OCILight.SetSpotAngle(30f, false))
        return;
      this.viSpotAngle.inputField.set_text(this.m_OCILight.lightInfo.spotAngle.ToString("0.000"));
      this.viSpotAngle.slider.set_value(this.m_OCILight.lightInfo.spotAngle);
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonSample.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleVisible.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangeEnable)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleTarget.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangeDrawTarget)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleShadow.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangeShadow)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.viIntensity.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangeIntensity)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.viIntensity.inputField.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
      // ISSUE: method pointer
      ((UnityEvent) this.viIntensity.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensity)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.viRange.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangeRange)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.viRange.inputField.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditRange)));
      // ISSUE: method pointer
      ((UnityEvent) this.viRange.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRange)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.viSpotAngle.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangeSpotAngle)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.viSpotAngle.inputField.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSpotAngle)));
      // ISSUE: method pointer
      ((UnityEvent) this.viSpotAngle.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSpotAngle)));
      this.isUpdateInfo = false;
    }

    [Serializable]
    private class BackgroundInfo
    {
      public GameObject obj;
      public Sprite[] sprit;
      public Image image;

      public bool active
      {
        set
        {
          if (this.obj.get_activeSelf() == value)
            return;
          this.obj.SetActive(value);
        }
      }

      public Info.LightLoadInfo.Target target
      {
        set
        {
          this.image.set_sprite(this.sprit[(int) value]);
        }
      }
    }

    [Serializable]
    private class ValueInfo
    {
      public GameObject obj;
      public Slider slider;
      public InputField inputField;
      public Button button;

      public bool active
      {
        set
        {
          if (this.obj.get_activeSelf() == value)
            return;
          this.obj.SetActive(value);
        }
      }
    }
  }
}
