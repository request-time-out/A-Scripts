// Decompiled with JetBrains decompiler
// Type: Studio.ConfigCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ConfigScene;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class ConfigCtrl : MonoBehaviour
  {
    [SerializeField]
    private Button buttonColor;
    [SerializeField]
    private Toggle toggleShield;
    [SerializeField]
    private Toggle[] togglesTexture;
    [SerializeField]
    private Toggle[] toggleSound;
    [SerializeField]
    private Slider[] sliderSound;

    public ConfigCtrl()
    {
      base.\u002Ector();
    }

    private SoundData[] soundData { get; set; }

    private void OnClickColor()
    {
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check("背景色"))
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      else
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("背景色", Manager.Config.GraphicData.BackColor, new Action<Color>(this.OnValueChangeColor), false);
    }

    private void OnValueChangeColor(Color _color)
    {
      Manager.Config.GraphicData.BackColor = _color;
      ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_color);
      Camera.get_main().set_backgroundColor(_color);
    }

    private void OnOnValueChangedTexture(int _no)
    {
    }

    private void OnValueChangedMute(bool _value, int _idx)
    {
      this.soundData[_idx].Mute = !_value;
      ((Selectable) this.sliderSound[_idx]).set_interactable(_value);
    }

    private void OnValueChangedVolume(float _value, int _idx)
    {
      this.soundData[_idx].Volume = Mathf.FloorToInt(_value);
    }

    private void Start()
    {
      this.soundData = new SoundData[6]
      {
        Manager.Config.SoundData.Master,
        Manager.Config.SoundData.BGM,
        Manager.Config.SoundData.GameSE,
        Manager.Config.SoundData.SystemSE,
        Manager.Config.SoundData.ENV,
        Singleton<Manager.Voice>.Instance._Config.PCM
      };
      ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Manager.Config.GraphicData.BackColor);
      this.toggleShield.set_isOn(Manager.Config.GraphicData.Shield);
      for (int index = 0; index < 6; ++index)
      {
        this.toggleSound[index].set_isOn(!this.soundData[index].Mute);
        ((Selectable) this.sliderSound[index]).set_interactable(!this.soundData[index].Mute);
        this.sliderSound[index].set_value((float) this.soundData[index].Volume);
      }
      // ISSUE: method pointer
      ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
      // ISSUE: variable of the null type
      __Null onValueChanged = this.toggleShield.onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (ConfigCtrl.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ConfigCtrl.\u003C\u003Ef__am\u0024cache0 = new UnityAction<bool>((object) null, __methodptr(\u003CStart\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache0 = ConfigCtrl.\u003C\u003Ef__am\u0024cache0;
      ((UnityEvent<bool>) onValueChanged).AddListener(fAmCache0);
      for (int index = 0; index < 3; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.togglesTexture[index].onValueChanged).AddListener(new UnityAction<bool>((object) new ConfigCtrl.\u003CStart\u003Ec__AnonStorey0()
        {
          limit = (byte) index
        }, __methodptr(\u003C\u003Em__0)));
      }
      for (int index = 0; index < 6; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ConfigCtrl.\u003CStart\u003Ec__AnonStorey1 startCAnonStorey1 = new ConfigCtrl.\u003CStart\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey1.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey1.no = index;
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleSound[index].onValueChanged).AddListener(new UnityAction<bool>((object) startCAnonStorey1, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderSound[index].get_onValueChanged()).AddListener(new UnityAction<float>((object) startCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      }
    }
  }
}
