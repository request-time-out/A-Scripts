// Decompiled with JetBrains decompiler
// Type: ConfigScene.SoundSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public class SoundSetting : BaseSetting
  {
    [SerializeField]
    private SoundSetting.SoundGroup Master;
    [SerializeField]
    private SoundSetting.SoundGroup BGM;
    [SerializeField]
    private SoundSetting.SoundGroup ENV;
    [SerializeField]
    private SoundSetting.SoundGroup SystemSE;
    [SerializeField]
    private SoundSetting.SoundGroup GameSE;

    private void InitSet(SoundSetting.SoundGroup sg, SoundData sd)
    {
      sg.toggle.set_isOn(sd.Mute);
      sg.slider.set_value((float) sd.Volume);
    }

    private void InitLink(SoundSetting.SoundGroup sg, SoundData sd, bool isSliderEvent)
    {
      this.LinkToggle(sg.toggle, (Action<bool>) (isOn => sd.Mute = isOn));
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) sg.toggle.onValueChanged), (Action<M0>) (isOn => ((Behaviour) sg.image).set_enabled(!isOn)));
      UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) Observable.Select<bool, bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(sg.toggle), (Func<M0, M1>) (b => !b)), (Selectable) sg.slider);
      if (isSliderEvent)
        this.LinkSlider(sg.slider, (Action<float>) (value => sd.Volume = (int) value));
      else
        ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) sg.slider), (Func<M0, bool>) (_ => Input.GetMouseButtonDown(0))), (Action<M0>) (_ => this.EnterSE()));
    }

    public override void Init()
    {
      SoundSystem soundData = Config.SoundData;
      this.InitLink(this.Master, soundData.Master, true);
      this.InitLink(this.ENV, soundData.ENV, true);
      this.InitLink(this.SystemSE, soundData.SystemSE, true);
      this.InitLink(this.GameSE, soundData.GameSE, true);
      this.InitLink(this.BGM, soundData.BGM, true);
    }

    protected override void ValueToUI()
    {
      SoundSystem soundData = Config.SoundData;
      this.InitSet(this.Master, soundData.Master);
      this.InitSet(this.BGM, soundData.BGM);
      this.InitSet(this.ENV, soundData.ENV);
      this.InitSet(this.SystemSE, soundData.SystemSE);
      this.InitSet(this.GameSE, soundData.GameSE);
    }

    [Serializable]
    private class SoundGroup
    {
      public Toggle toggle;
      public Slider slider;
      public Image image;
    }
  }
}
