// Decompiled with JetBrains decompiler
// Type: ConfigScene.GameSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigScene
{
  public class GameSetting : BaseSetting
  {
    [Header("既読スキップ")]
    [SerializeField]
    private Toggle[] readSkipToggles;
    [Header("次のテキスト表示時に音声を停止")]
    [SerializeField]
    private Toggle[] nextVoiceStopToggles;
    [Header("選択肢でもスキップ継続")]
    [SerializeField]
    private Toggle[] choiceSkipToggles;
    [Header("選択肢でもオート継続")]
    [SerializeField]
    private Toggle[] choiceAutoToggles;
    [Header("テキストウィンドウオプション")]
    [SerializeField]
    private Toggle[] optionToggles;
    [Header("文字表示速度")]
    [SerializeField]
    private Slider fontSpeedSlider;
    [Header("自動送り待ち時間")]
    [SerializeField]
    private Slider autoWaitTimeSlider;
    [Header("文字表示サンプル")]
    [SerializeField]
    private TypefaceAnimatorEx[] ta;
    [Header("操作ガイド")]
    [SerializeField]
    private Toggle[] guidToggles;
    [Header("ストーリーヘルプ")]
    [SerializeField]
    private Toggle[] helpToggles;
    [Header("ミニマップ")]
    [SerializeField]
    private Toggle[] minimapToggles;
    [Header("中央ポインター")]
    [SerializeField]
    private Toggle[] pointerToggles;
    [Header("女の子のパラメーターロック")]
    [SerializeField]
    private Toggle[] lockToggles;
    private IDisposable cancel;

    private void OnDestroy()
    {
      this.Release();
    }

    private void Release()
    {
      if (this.cancel == null)
        return;
      this.cancel.Dispose();
    }

    public override void Init()
    {
      GameConfigSystem data = Manager.Config.GameData;
      this.LinkToggleArray(this.readSkipToggles, (Action<int>) (i => data.ReadSkip = i == 0));
      this.LinkToggleArray(this.nextVoiceStopToggles, (Action<int>) (i => data.NextVoiceStop = i == 0));
      this.LinkToggleArray(this.choiceSkipToggles, (Action<int>) (i => data.ChoicesSkip = i == 0));
      this.LinkToggleArray(this.choiceAutoToggles, (Action<int>) (i => data.ChoicesAuto = i == 0));
      this.LinkToggleArray(this.optionToggles, (Action<int>) (i => data.TextWindowOption = i == 0));
      this.LinkToggleArray(this.guidToggles, (Action<int>) (i => data.ActionGuide = i == 0));
      this.LinkToggleArray(this.helpToggles, (Action<int>) (i => data.StoryHelp = i == 0));
      this.LinkToggleArray(this.minimapToggles, (Action<int>) (i => data.MiniMap = i == 0));
      this.LinkToggleArray(this.pointerToggles, (Action<int>) (i => data.CenterPointer = i == 0));
      this.LinkToggleArray(this.lockToggles, (Action<int>) (i => data.ParameterLock = i == 0));
      this.LinkSlider(this.fontSpeedSlider, (Action<float>) (value => data.FontSpeed = (int) value));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<float, int>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.fontSpeedSlider), (Func<M0, M1>) (value => (int) value)), (Action<M0>) (value =>
      {
        foreach (TypefaceAnimatorEx typefaceAnimatorEx in this.ta)
        {
          typefaceAnimatorEx.isNoWait = value == 100;
          if (!typefaceAnimatorEx.isNoWait)
          {
            typefaceAnimatorEx.timeMode = TypefaceAnimatorEx.TimeMode.Speed;
            typefaceAnimatorEx.speed = (float) value;
          }
        }
      }));
      this.LinkSlider(this.autoWaitTimeSlider, (Action<float>) (value => data.AutoWaitTime = value));
      ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.autoWaitTimeSlider), (Action<M0>) (value =>
      {
        if (this.cancel != null)
          this.cancel.Dispose();
        foreach (TypefaceAnimatorEx typefaceAnimatorEx in this.ta)
          typefaceAnimatorEx.Play();
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) Observable.Select<Unit, bool>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, M1>) (_ => this.ta[0].isPlaying))), (Func<M0, bool>) (isPlaying => !isPlaying)), (Action<M0>) (_ =>
      {
        if (this.cancel != null)
          this.cancel.Dispose();
        float autoWaitTimer = 0.0f;
        this.cancel = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (__ => (IEnumerator) new WaitWhile((Func<bool>) (() =>
        {
          float autoWaitTime = data.AutoWaitTime;
          autoWaitTimer = Mathf.Min(autoWaitTimer + Time.get_unscaledDeltaTime(), autoWaitTime);
          return (double) autoWaitTimer < (double) autoWaitTime;
        }))), false), (Action<M0>) (__ =>
        {
          foreach (TypefaceAnimatorEx typefaceAnimatorEx in this.ta)
            typefaceAnimatorEx.Play();
        }));
      }));
    }

    protected override void ValueToUI()
    {
      GameConfigSystem data = Manager.Config.GameData;
      this.SetToggleUIArray(this.readSkipToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.ReadSkip : data.ReadSkip)));
      this.SetToggleUIArray(this.nextVoiceStopToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.NextVoiceStop : data.NextVoiceStop)));
      this.SetToggleUIArray(this.choiceSkipToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.ChoicesSkip : data.ChoicesSkip)));
      this.SetToggleUIArray(this.choiceAutoToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.ChoicesAuto : data.ChoicesAuto)));
      this.SetToggleUIArray(this.optionToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.TextWindowOption : data.TextWindowOption)));
      this.fontSpeedSlider.set_value((float) data.FontSpeed);
      this.autoWaitTimeSlider.set_value(data.AutoWaitTime);
      this.SetToggleUIArray(this.guidToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.ActionGuide : data.ActionGuide)));
      this.SetToggleUIArray(this.helpToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.StoryHelp : data.StoryHelp)));
      this.SetToggleUIArray(this.minimapToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.MiniMap : data.MiniMap)));
      this.SetToggleUIArray(this.pointerToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.CenterPointer : data.CenterPointer)));
      this.SetToggleUIArray(this.lockToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.ParameterLock : data.ParameterLock)));
    }
  }
}
