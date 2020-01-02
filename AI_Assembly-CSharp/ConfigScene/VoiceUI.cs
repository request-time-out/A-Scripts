// Decompiled with JetBrains decompiler
// Type: ConfigScene.VoiceUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ConfigScene
{
  public class VoiceUI : MonoBehaviour
  {
    [SerializeField]
    protected int index;
    [SerializeField]
    protected Toggle toggle;
    [SerializeField]
    protected Slider slider;

    public VoiceUI()
    {
      base.\u002Ector();
    }

    public void Refresh()
    {
      if (!Singleton<Manager.Voice>.Instance._Config.chara.ContainsKey(this.index))
        return;
      this.toggle.set_isOn(Singleton<Manager.Voice>.Instance._Config.chara[this.index].sound.Mute);
      this.slider.set_value((float) Singleton<Manager.Voice>.Instance._Config.chara[this.index].sound.Volume);
    }

    protected void OnValueChangeToggle(bool _value)
    {
      Singleton<Manager.Voice>.Instance._Config.chara[this.index].sound.Mute = _value;
    }

    protected void OnValueChangeSlider(float _value)
    {
      Singleton<Manager.Voice>.Instance._Config.chara[this.index].sound.Volume = Mathf.FloorToInt(_value);
    }

    protected void Reset()
    {
      if (Object.op_Equality((Object) this.toggle, (Object) null))
        this.toggle = (Toggle) ((Component) this).GetComponentInChildren<Toggle>();
      if (!Object.op_Equality((Object) this.slider, (Object) null))
        return;
      this.slider = (Slider) ((Component) this).GetComponentInChildren<Slider>();
    }

    protected void Start()
    {
      if (!Singleton<Manager.Voice>.Instance._Config.chara.ContainsKey(this.index))
        return;
      this.Refresh();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__0)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CStart\u003Em__1)));
    }
  }
}
