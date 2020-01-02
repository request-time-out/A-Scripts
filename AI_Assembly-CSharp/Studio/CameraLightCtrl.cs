// Decompiled with JetBrains decompiler
// Type: Studio.CameraLightCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class CameraLightCtrl : MonoBehaviour
  {
    [SerializeField]
    private CameraLightCtrl.LightCalc lightChara;

    public CameraLightCtrl()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      this.lightChara.Init();
    }

    public void Reflect()
    {
      this.lightChara.Reflect();
    }

    private void OnEnable()
    {
      this.lightChara.UpdateUI();
    }

    private void Start()
    {
      this.Init();
    }

    public class LightInfo
    {
      public Color color = Color.get_white();
      public float intensity = 1f;
      public float[] rot = new float[2];
      public bool shadow = true;

      public virtual void Init()
      {
        this.color = Utility.ConvertColor((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        this.intensity = 1f;
        this.rot[0] = 0.0f;
        this.rot[1] = 0.0f;
        this.shadow = true;
      }

      public virtual void Save(BinaryWriter _writer, Version _version)
      {
        _writer.Write(JsonUtility.ToJson((object) this.color));
        _writer.Write(this.intensity);
        _writer.Write(this.rot[0]);
        _writer.Write(this.rot[1]);
        _writer.Write(this.shadow);
      }

      public virtual void Load(BinaryReader _reader, Version _version)
      {
        this.color = (Color) JsonUtility.FromJson<Color>(_reader.ReadString());
        this.intensity = _reader.ReadSingle();
        this.rot[0] = _reader.ReadSingle();
        this.rot[1] = _reader.ReadSingle();
        this.shadow = _reader.ReadBoolean();
      }
    }

    public class MapLightInfo : CameraLightCtrl.LightInfo
    {
      public LightType type = (LightType) 1;

      public override void Init()
      {
        base.Init();
        this.type = (LightType) 1;
      }

      public override void Save(BinaryWriter _writer, Version _version)
      {
        base.Save(_writer, _version);
        _writer.Write((int) this.type);
      }

      public override void Load(BinaryReader _reader, Version _version)
      {
        base.Load(_reader, _version);
        this.type = (LightType) _reader.ReadInt32();
      }
    }

    [Serializable]
    private class LightCalc
    {
      public Light light;
      public Transform transRoot;
      public Button buttonColor;
      public Toggle toggleShadow;
      public Slider sliderIntensity;
      public InputField inputIntensity;
      public Button buttonIntensity;
      public Slider[] sliderAxis;
      public InputField[] inputAxis;
      public Button[] buttonAxis;
      private bool isUpdateInfo;

      public bool isInit { get; private set; }

      public void Init()
      {
        if (this.isInit)
          return;
        // ISSUE: method pointer
        ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleShadow.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangeShadow)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderIntensity.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangeIntensity)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.inputIntensity.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditIntensity)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonIntensity.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickIntensity)));
        for (int index = 0; index < 2; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CameraLightCtrl.LightCalc.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new CameraLightCtrl.LightCalc.\u003CInit\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          initCAnonStorey0.\u0024this = this;
          // ISSUE: reference to a compiler-generated field
          initCAnonStorey0.axis = index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent<float>) this.sliderAxis[initCAnonStorey0.axis].get_onValueChanged()).AddListener(new UnityAction<float>((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent<string>) this.inputAxis[initCAnonStorey0.axis].get_onEndEdit()).AddListener(new UnityAction<string>((object) initCAnonStorey0, __methodptr(\u003C\u003Em__1)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) this.buttonAxis[initCAnonStorey0.axis].get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__2)));
        }
        this.Reflect();
        this.isInit = true;
      }

      public void UpdateUI()
      {
        this.isUpdateInfo = true;
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.color);
        this.sliderIntensity.set_value(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity);
        this.inputIntensity.set_text(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity.ToString("0.00"));
        for (int index = 0; index < 2; ++index)
        {
          this.sliderAxis[index].set_value(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[index]);
          this.inputAxis[index].set_text(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[index].ToString("000"));
        }
        this.toggleShadow.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.shadow);
        this.isUpdateInfo = false;
      }

      public void Reflect()
      {
        this.light.set_color(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.color);
        this.light.set_intensity(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity);
        this.transRoot.set_localRotation(Quaternion.Euler(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[0], Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[1], 0.0f));
        this.light.set_shadows(!Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.shadow ? (LightShadows) 0 : (LightShadows) 2);
      }

      private void OnClickColor()
      {
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("キャラライト", Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.color, new Action<Color>(this.OnValueChangeColor), false);
        Singleton<Studio.Studio>.Instance.colorPalette.visible = true;
      }

      private void OnValueChangeColor(Color _color)
      {
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_color);
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.color = _color;
        this.Reflect();
      }

      private void OnValueChangeShadow(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.shadow = _value;
        this.Reflect();
      }

      private void OnValueChangeIntensity(float _value)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity = _value;
        this.inputIntensity.set_text(_value.ToString("0.00"));
        this.Reflect();
      }

      private void OnEndEditIntensity(string _text)
      {
        float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.1f, 2f);
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity = num;
        this.sliderIntensity.set_value(num);
        this.Reflect();
      }

      private void OnClickIntensity()
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity = 1f;
        this.sliderIntensity.set_value(1f);
        this.inputIntensity.set_text(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.intensity.ToString("0.00"));
        this.Reflect();
      }

      private void OnValueChangeAxis(float _value, int _axis)
      {
        if (this.isUpdateInfo)
          return;
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[_axis] = _value;
        this.inputAxis[_axis].set_text(_value.ToString("000"));
        this.Reflect();
      }

      private void OnEndEditAxis(string _text, int _axis)
      {
        float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 359f);
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[_axis] = num;
        this.sliderAxis[_axis].set_value(num);
        this.Reflect();
      }

      private void OnClickAxis(int _axis)
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[_axis] = 0.0f;
        this.sliderAxis[_axis].set_value(0.0f);
        this.inputAxis[_axis].set_text(Singleton<Studio.Studio>.Instance.sceneInfo.charaLight.rot[_axis].ToString("000"));
        this.Reflect();
      }
    }
  }
}
