// Decompiled with JetBrains decompiler
// Type: ConfigScene.VoiceSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public class VoiceSetting : BaseSetting
  {
    private Dictionary<Transform, VoiceSetting.SetData> dic = new Dictionary<Transform, VoiceSetting.SetData>();
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private RectTransform rtcPcm;
    [SerializeField]
    private RectTransform node;

    public override void Init()
    {
      Dictionary<int, VoiceSystem.Voice> chara = Singleton<Manager.Voice>.Instance._Config.chara;
      this.Add((Transform) this.rtcPcm);
      for (int index = 0; index < ((Component) this.node).get_transform().get_childCount(); ++index)
        this.Add((Transform) (((Component) this.node).get_transform().GetChild(index) as RectTransform));
      int num = ((Component) this.node).get_transform().get_childCount() + (((Component) this.node).get_transform().get_childCount() % 2 != 0 ? 1 : 0);
      foreach (KeyValuePair<int, string> keyValuePair in Singleton<Manager.Voice>.Instance.voiceInfoList.ToDictionary<VoiceInfo.Param, int, string>((Func<VoiceInfo.Param, int>) (v => v.No), (Func<VoiceInfo.Param, string>) (v => v.Personality)))
      {
        if (chara.ContainsKey(keyValuePair.Key))
          this.Create(num++, keyValuePair.Key, keyValuePair.Value);
      }
    }

    protected override void ValueToUI()
    {
      using (Dictionary<Transform, VoiceSetting.SetData>.ValueCollection.Enumerator enumerator = this.dic.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          VoiceSetting.SetData current = enumerator.Current;
          current.toggle.set_isOn(current.sd.Mute);
          current.slider.set_value((float) current.sd.Volume);
        }
      }
    }

    private void Create(int num, int key, string name)
    {
      RectTransform component = (RectTransform) ((GameObject) Object.Instantiate<GameObject>((M0) this.prefab, ((Component) this.node).get_transform())).GetComponent<RectTransform>();
      ((Object) component).set_name(key.ToString());
      ((Text) ((Component) component).GetComponentInChildren<Text>()).set_text(name);
      this.Add(key, (Transform) component);
    }

    private bool Add(Transform trans)
    {
      if (this.dic.ContainsKey(trans))
        return false;
      VoiceSetting.SetData data = new VoiceSetting.SetData()
      {
        sd = Singleton<Manager.Voice>.Instance._Config.PCM,
        slider = (Slider) ((Component) trans).GetComponentInChildren<Slider>(),
        toggle = (Toggle) ((Component) trans).GetComponentInChildren<Toggle>(),
        image = (Image) ((Component) trans).GetComponentInChildren<Image>()
      };
      this.AddEvent(data);
      this.dic.Add(trans, data);
      return true;
    }

    private bool Add(int key, Transform trans)
    {
      if (this.dic.ContainsKey(trans))
        return false;
      VoiceSetting.SetData data = new VoiceSetting.SetData()
      {
        sd = Singleton<Manager.Voice>.Instance._Config.chara[key].sound,
        slider = (Slider) ((Component) trans).GetComponentInChildren<Slider>(),
        toggle = (Toggle) ((Component) trans).GetComponentInChildren<Toggle>(),
        image = (Image) ((Component) trans).GetComponentInChildren<Image>()
      };
      this.AddEvent(data);
      this.dic.Add(trans, data);
      return true;
    }

    private void AddEvent(VoiceSetting.SetData data)
    {
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) data.toggle.onValueChanged), (Action<M0>) (isOn =>
      {
        data.sd.Mute = isOn;
        ((Behaviour) data.image).set_enabled(!isOn);
        this.EnterSE();
      }));
      UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) Observable.Select<bool, bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(data.toggle), (Func<M0, M1>) (b => !b)), (Selectable) data.slider);
      ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<float, int>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) data.slider.get_onValueChanged()), (Func<M0, M1>) (value => (int) value)), (Action<M0>) (value => data.sd.Volume = value));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) data.slider), (Func<M0, bool>) (_ => Input.GetMouseButtonDown(0))), (Action<M0>) (_ => this.EnterSE()));
    }

    private class SetData
    {
      public SoundData sd;
      public Toggle toggle;
      public Slider slider;
      public Image image;
    }
  }
}
