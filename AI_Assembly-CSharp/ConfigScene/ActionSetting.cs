// Decompiled with JetBrains decompiler
// Type: ConfigScene.ActionSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public class ActionSetting : BaseSetting
  {
    [Header("注視点の表示")]
    [SerializeField]
    private Toggle[] lookToggles;
    [Header("TPS時のマウス感度X")]
    [SerializeField]
    private Slider tpsSensitivityXSlider;
    [Header("TPS時のマウス感度Y")]
    [SerializeField]
    private Slider tpsSensitivityYSlider;
    [Header("FPS時のマウス感度X")]
    [SerializeField]
    private Slider fpsSensitivityXSlider;
    [Header("FPS時のマウス感度Y")]
    [SerializeField]
    private Slider fpsSensitivityYSlider;
    [Header("カメラ移動Xの反転")]
    [SerializeField]
    private Toggle[] invertMoveXToggles;
    [Header("カメラ移動Yの反転")]
    [SerializeField]
    private Toggle[] invertMoveYToggles;
    [Header("TPS時のマウス感度Xリセット")]
    [SerializeField]
    private Button tpsSensitivityXResetButton;
    [Header("TPS時のマウス感度Yリセット")]
    [SerializeField]
    private Button tpsSensitivityYResetButton;
    [Header("FPS時のマウス感度Xリセット")]
    [SerializeField]
    private Button fpsSensitivityXResetButton;
    [Header("FPS時のマウス感度Yリセット")]
    [SerializeField]
    private Button fpsSensitivityYResetButton;

    public override void Init()
    {
      ActionSystem actData = Manager.Config.ActData;
      this.LinkToggleArray(this.lookToggles, (Action<int>) (i => actData.Look = i == 0));
      this.LinkSlider(this.tpsSensitivityXSlider, (Action<float>) (value => actData.TPSSensitivityY = (int) value));
      this.LinkSlider(this.tpsSensitivityYSlider, (Action<float>) (value => actData.TPSSensitivityX = (int) value));
      this.LinkSlider(this.fpsSensitivityXSlider, (Action<float>) (value => actData.FPSSensitivityY = (int) value));
      this.LinkSlider(this.fpsSensitivityYSlider, (Action<float>) (value => actData.FPSSensitivityX = (int) value));
      this.LinkToggleArray(this.invertMoveXToggles, (Action<int>) (i => actData.InvertMoveY = i == 1));
      this.LinkToggleArray(this.invertMoveYToggles, (Action<int>) (i => actData.InvertMoveX = i == 1));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.tpsSensitivityXResetButton), (Action<M0>) (_ => this.tpsSensitivityXSlider.set_value(0.0f)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.tpsSensitivityYResetButton), (Action<M0>) (_ => this.tpsSensitivityYSlider.set_value(0.0f)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.fpsSensitivityXResetButton), (Action<M0>) (_ => this.fpsSensitivityXSlider.set_value(0.0f)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.fpsSensitivityYResetButton), (Action<M0>) (_ => this.fpsSensitivityYSlider.set_value(0.0f)));
      ObservableExtensions.Subscribe<Unit>(Observable.Merge<Unit>((IObservable<M0>[]) new IObservable<Unit>[4]
      {
        UnityUIComponentExtensions.OnClickAsObservable(this.tpsSensitivityXResetButton),
        UnityUIComponentExtensions.OnClickAsObservable(this.tpsSensitivityYResetButton),
        UnityUIComponentExtensions.OnClickAsObservable(this.fpsSensitivityXResetButton),
        UnityUIComponentExtensions.OnClickAsObservable(this.fpsSensitivityYResetButton)
      }), (Action<M0>) (_ => this.EnterSE()));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Merge<PointerEventData>((IObservable<M0>[]) new IObservable<PointerEventData>[4]
      {
        ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.tpsSensitivityXResetButton),
        ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.tpsSensitivityYResetButton),
        ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.fpsSensitivityXResetButton),
        ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.fpsSensitivityYResetButton)
      }), (Action<M0>) (_ => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select)));
    }

    protected override void ValueToUI()
    {
      ActionSystem actData = Manager.Config.ActData;
      this.SetToggleUIArray(this.lookToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !actData.Look : actData.Look)));
      this.tpsSensitivityXSlider.set_value((float) actData.TPSSensitivityY);
      this.tpsSensitivityYSlider.set_value((float) actData.TPSSensitivityX);
      this.fpsSensitivityXSlider.set_value((float) actData.FPSSensitivityY);
      this.fpsSensitivityYSlider.set_value((float) actData.FPSSensitivityX);
      this.SetToggleUIArray(this.invertMoveXToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 1 ? !actData.InvertMoveY : actData.InvertMoveY)));
      this.SetToggleUIArray(this.invertMoveYToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 1 ? !actData.InvertMoveX : actData.InvertMoveX)));
    }
  }
}
