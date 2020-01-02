// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsCaptureMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsCaptureMenu : MonoBehaviour
  {
    [Header("【描画】------------------------")]
    [SerializeField]
    private Toggle[] tglClothesState;
    [SerializeField]
    private Toggle[] tglAcsState;
    [Header("【キャラ】----------------------")]
    [SerializeField]
    private Toggle[] tglEyeLook;
    [SerializeField]
    private Toggle[] tglNeckLook;
    [SerializeField]
    private Button[] btnPose;
    [SerializeField]
    private InputField inpPoseNo;
    [SerializeField]
    private Button[] btnEyebrow;
    [SerializeField]
    private InputField inpEyebrowNo;
    [SerializeField]
    private Button[] btnEyePtn;
    [SerializeField]
    private InputField inpEyeNo;
    [SerializeField]
    private Slider sldEyeOpen;
    [SerializeField]
    private Button[] btnMouthPtn;
    [SerializeField]
    private InputField inpMouthNo;
    [SerializeField]
    private Slider sldMouthOpen;
    [SerializeField]
    private Button[] btnHandLPtn;
    [SerializeField]
    private InputField inpHandLNo;
    [SerializeField]
    private Button[] btnHandRPtn;
    [SerializeField]
    private InputField inpHandRNo;
    [SerializeField]
    private UI_ToggleOnOffEx tglPlayAnime;
    [Header("【ライト】----------------------")]
    [SerializeField]
    private Slider sldLightRotX;
    [SerializeField]
    private Slider sldLightRotY;
    [SerializeField]
    private CustomColorSet csLight;
    [SerializeField]
    private Slider sldLightPower;
    [SerializeField]
    private Button btnLightReset;
    [Header("【フレーム】--------------------")]
    [SerializeField]
    private GameObject objBackFrame;
    [SerializeField]
    private Toggle tglBFrameDraw;
    [SerializeField]
    private Button[] btnBFramePtn;
    [SerializeField]
    private Toggle tglFFrameDraw;
    [SerializeField]
    private Button[] btnFFramePtn;
    [Header("【背景】------------------------")]
    [SerializeField]
    private Toggle[] tglBG;
    [SerializeField]
    private GameObject objBGIndex;
    [SerializeField]
    private Button[] btnBGIndex;
    [SerializeField]
    private GameObject objBGColor;
    [SerializeField]
    private CustomColorSet csBG;
    [Header("【終了】------------------------")]
    [SerializeField]
    private Button btnCancel;
    [SerializeField]
    private Button btnSave;
    [SerializeField]
    private Text textSaveName;
    private int handLPtn;
    private int handRPtn;
    private bool backAutoClothesState;
    private int backClothesNo;
    private int backAutoClothesStateNo;

    public CvsCaptureMenu()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaListControl lstCtrl
    {
      get
      {
        return Singleton<Character>.Instance.chaListCtrl;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    public void BeginCapture()
    {
      this.backAutoClothesState = this.customBase.autoClothesState;
      this.backClothesNo = this.customBase.clothesStateNo;
      this.backAutoClothesStateNo = this.customBase.autoClothesStateNo;
      int clothesStateNo = this.customBase.clothesStateNo;
      this.tglClothesState[clothesStateNo].SetIsOnWithoutCallback(true);
      for (int index = 0; index < this.tglClothesState.Length; ++index)
      {
        if (index != clothesStateNo)
          this.tglClothesState[index].SetIsOnWithoutCallback(false);
      }
      this.tglAcsState[0].SetIsOnWithoutCallback(this.customBase.accessoryDraw);
      this.tglAcsState[1].SetIsOnWithoutCallback(!this.customBase.accessoryDraw);
      this.tglEyeLook[0].SetIsOnWithoutCallback(0 == this.customBase.eyelook);
      this.tglEyeLook[1].SetIsOnWithoutCallback(1 == this.customBase.eyelook);
      this.tglNeckLook[0].SetIsOnWithoutCallback(0 == this.customBase.necklook);
      this.tglNeckLook[1].SetIsOnWithoutCallback(1 == this.customBase.necklook);
      this.chaCtrl.ChangeEyesBlinkFlag(false);
      this.chaCtrl.ChangeMouthFixed(true);
      this.chaCtrl.ChangeMouthOpenMax(0.0f);
      this.sldEyeOpen.set_value(1f);
      this.sldMouthOpen.set_value(0.0f);
      this.chaCtrl.SetEnableShapeHand(0, false);
      this.chaCtrl.SetShapeHandIndex(0, 0, -1);
      this.chaCtrl.SetEnableShapeHand(1, false);
      this.chaCtrl.SetShapeHandIndex(1, 0, -1);
      this.inpPoseNo.set_text(this.customBase.poseNo.ToString());
      this.inpEyebrowNo.set_text((this.customBase.eyebrowPtn + 1).ToString());
      this.inpEyeNo.set_text((this.customBase.eyePtn + 1).ToString());
      this.inpMouthNo.set_text((this.customBase.mouthPtn + 1).ToString());
      this.inpHandLNo.set_text("ポーズ");
      this.inpHandRNo.set_text("ポーズ");
      this.tglPlayAnime.SetIsOnWithoutCallback(this.customBase.playPoseAnime);
      Vector3 localEulerAngles = ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles();
      this.sldLightRotX.set_value(89.0 >= localEulerAngles.x ? (float) (double) localEulerAngles.x : (float) (localEulerAngles.x - 360.0));
      this.sldLightRotY.set_value(180.0 > localEulerAngles.y ? (float) (double) localEulerAngles.y : (float) (localEulerAngles.y - 360.0));
      this.csLight.SetColor(this.customBase.lightCustom.get_color());
      this.sldLightPower.set_value(this.customBase.lightCustom.get_intensity());
      this.customBase.drawSaveFrameTop = true;
      this.tglBFrameDraw.set_isOn(this.customBase.drawSaveFrameBack);
      this.tglFFrameDraw.set_isOn(this.customBase.drawSaveFrameFront);
      this.tglBG[0].SetIsOnWithoutCallback(this.customBase.customCtrl.draw3D);
      this.tglBG[1].SetIsOnWithoutCallback(!this.customBase.customCtrl.draw3D);
      this.csBG.SetColor(this.customBase.customCtrl.GetBGColor());
      this.objBGIndex.SetActiveIfDifferent(!this.customBase.customCtrl.draw3D);
      this.objBGColor.SetActiveIfDifferent(this.customBase.customCtrl.draw3D);
      this.objBackFrame.SetActiveIfDifferent(!this.customBase.customCtrl.draw3D);
      if (this.customBase.customCtrl.saveMode)
        this.textSaveName.set_text("保存");
      else
        this.textSaveName.set_text("撮影");
    }

    public void EndCapture()
    {
      this.customBase.autoClothesState = this.backAutoClothesState;
      this.customBase.clothesStateNo = this.backClothesNo;
      this.customBase.autoClothesStateNo = this.backAutoClothesStateNo;
      this.customBase.ChangeClothesState(-1);
      this.chaCtrl.ChangeEyesBlinkFlag(true);
      this.chaCtrl.ChangeMouthFixed(false);
      this.chaCtrl.ChangeEyesOpenMax(1f);
      this.chaCtrl.ChangeMouthOpenMax(1f);
      this.chaCtrl.SetEnableShapeHand(0, false);
      this.chaCtrl.SetEnableShapeHand(1, false);
      this.customBase.drawSaveFrameTop = false;
      this.customBase.drawMenu.UpdateUI();
    }

    protected virtual void Start()
    {
      this.customBase.lstInputField.Add(this.inpPoseNo);
      this.customBase.lstInputField.Add(this.inpEyebrowNo);
      this.customBase.lstInputField.Add(this.inpEyeNo);
      this.customBase.lstInputField.Add(this.inpMouthNo);
      this.customBase.lstInputField.Add(this.inpHandLNo);
      this.customBase.lstInputField.Add(this.inpHandRNo);
      this.customBase.drawSaveFrameBack = true;
      this.customBase.drawSaveFrameFront = true;
      if (((IEnumerable<Toggle>) this.tglClothesState).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglClothesState).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.customBase.ChangeClothesState(item.idx + 1)))));
      }
      if (((IEnumerable<Toggle>) this.tglAcsState).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglAcsState).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.customBase.accessoryDraw = item.idx == 0))));
      }
      if (((IEnumerable<Toggle>) this.tglEyeLook).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglEyeLook).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.customBase.eyelook = item.idx))));
      }
      if (((IEnumerable<Toggle>) this.tglNeckLook).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglNeckLook).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.customBase.necklook = item.idx))));
      }
      if (((IEnumerable<Button>) this.btnPose).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnPose).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          if (item.idx == 2)
            this.customBase.poseNo = 1;
          else
            this.customBase.ChangeAnimationNext(item.idx);
          this.inpPoseNo.set_text(this.customBase.poseNo.ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpPoseNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpPoseNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (!int.TryParse(value, out result))
            result = 0;
          this.customBase.ChangeAnimationNo(result, false);
          this.inpPoseNo.set_text(this.customBase.poseNo.ToString());
        }));
      if (((IEnumerable<Button>) this.btnEyebrow).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnEyebrow).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          if (item.idx == 2)
            this.customBase.ChangeEyebrowPtnNext(-1);
          else
            this.customBase.ChangeEyebrowPtnNext(item.idx);
          this.inpEyebrowNo.set_text((this.customBase.eyebrowPtn + 1).ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpEyebrowNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpEyebrowNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (!int.TryParse(value, out result))
            result = 0;
          this.customBase.ChangeEyebrowPtnNo(result);
          this.inpEyebrowNo.set_text((this.customBase.eyebrowPtn + 1).ToString());
        }));
      if (((IEnumerable<Button>) this.btnEyePtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnEyePtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          if (item.idx == 2)
            this.customBase.ChangeEyePtnNext(-1);
          else
            this.customBase.ChangeEyePtnNext(item.idx);
          this.inpEyeNo.set_text((this.customBase.eyePtn + 1).ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpEyeNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpEyeNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (!int.TryParse(value, out result))
            result = 0;
          this.customBase.ChangeEyePtnNo(result);
          this.inpEyeNo.set_text((this.customBase.eyePtn + 1).ToString());
        }));
      if (Object.op_Implicit((Object) this.sldEyeOpen))
        ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldEyeOpen), (Action<M0>) (val => this.chaCtrl.ChangeEyesOpenMax(val)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldEyeOpen), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldEyeOpen.set_value(Mathf.Clamp(this.sldEyeOpen.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.0f, 100f));
      }));
      if (((IEnumerable<Button>) this.btnMouthPtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnMouthPtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          if (item.idx == 2)
            this.customBase.ChangeMouthPtnNext(-1);
          else
            this.customBase.ChangeMouthPtnNext(item.idx);
          this.inpMouthNo.set_text((this.customBase.mouthPtn + 1).ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpMouthNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpMouthNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (!int.TryParse(value, out result))
            result = 0;
          this.customBase.ChangeMouthPtnNo(result);
          this.inpMouthNo.set_text((this.customBase.mouthPtn + 1).ToString());
        }));
      if (Object.op_Implicit((Object) this.sldMouthOpen))
        ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldMouthOpen), (Action<M0>) (val => this.chaCtrl.ChangeMouthOpenMax(val)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldMouthOpen), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldMouthOpen.set_value(Mathf.Clamp(this.sldMouthOpen.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.0f, 100f));
      }));
      if (((IEnumerable<Button>) this.btnHandLPtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnHandLPtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          int num = this.chaCtrl.GetShapeIndexHandCount() + 1;
          this.handLPtn = item.idx != 2 ? (item.idx != 0 ? (this.handLPtn + 1) % num : (this.handLPtn + num - 1) % num) : 0;
          if (this.handLPtn == 0)
          {
            this.chaCtrl.SetEnableShapeHand(0, false);
          }
          else
          {
            this.chaCtrl.SetEnableShapeHand(0, true);
            this.chaCtrl.SetShapeHandIndex(0, this.handLPtn - 1, -1);
          }
          if (this.handLPtn == 0)
            this.inpHandLNo.set_text("ポーズ");
          else
            this.inpHandLNo.set_text(this.handLPtn.ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpHandLNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpHandLNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (value == "ポーズ")
            result = 0;
          else if (!int.TryParse(value, out result))
            result = 0;
          this.handLPtn = result;
          if (this.handLPtn == 0)
          {
            this.chaCtrl.SetEnableShapeHand(0, false);
          }
          else
          {
            this.chaCtrl.SetEnableShapeHand(0, true);
            this.chaCtrl.SetShapeHandIndex(0, this.handLPtn - 1, -1);
          }
          if (this.handLPtn == 0)
            this.inpHandLNo.set_text("ポーズ");
          else
            this.inpHandLNo.set_text(this.handLPtn.ToString());
        }));
      if (((IEnumerable<Button>) this.btnHandRPtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnHandRPtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ =>
        {
          int num = this.chaCtrl.GetShapeIndexHandCount() + 1;
          this.handRPtn = item.idx != 2 ? (item.idx != 0 ? (this.handRPtn + 1) % num : (this.handRPtn + num - 1) % num) : 0;
          if (this.handRPtn == 0)
          {
            this.chaCtrl.SetEnableShapeHand(1, false);
          }
          else
          {
            this.chaCtrl.SetEnableShapeHand(1, true);
            this.chaCtrl.SetShapeHandIndex(1, this.handRPtn - 1, -1);
          }
          if (this.handRPtn == 0)
            this.inpHandRNo.set_text("ポーズ");
          else
            this.inpHandRNo.set_text(this.handRPtn.ToString());
        }))));
      }
      if (Object.op_Implicit((Object) this.inpHandRNo))
        ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) this.inpHandRNo.get_onEndEdit()), (Action<M0>) (value =>
        {
          int result;
          if (value == "ポーズ")
            result = 0;
          else if (!int.TryParse(value, out result))
            result = 0;
          this.handRPtn = result;
          if (this.handRPtn == 0)
          {
            this.chaCtrl.SetEnableShapeHand(1, false);
          }
          else
          {
            this.chaCtrl.SetEnableShapeHand(1, true);
            this.chaCtrl.SetShapeHandIndex(1, this.handRPtn - 1, -1);
          }
          if (this.handRPtn == 0)
            this.inpHandRNo.set_text("ポーズ");
          else
            this.inpHandRNo.set_text(this.handRPtn.ToString());
        }));
      if (Object.op_Implicit((Object) this.tglPlayAnime))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) this.tglPlayAnime), (Action<M0>) (isOn => this.customBase.playPoseAnime = isOn));
      if (Object.op_Implicit((Object) this.sldLightRotX))
        ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldLightRotX), (Action<M0>) (val => ((Component) this.customBase.lightCustom).get_transform().set_localEulerAngles(new Vector3(val, (float) ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles().y, (float) ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles().z))));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldLightRotX), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldLightRotX.set_value(Mathf.Clamp(this.sldLightRotX.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.0f, 100f));
      }));
      if (Object.op_Implicit((Object) this.sldLightRotY))
        ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldLightRotY), (Action<M0>) (val => ((Component) this.customBase.lightCustom).get_transform().set_localEulerAngles(new Vector3((float) ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles().x, val, (float) ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles().z))));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldLightRotY), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldLightRotY.set_value(Mathf.Clamp(this.sldLightRotY.get_value() + (float) scl.get_scrollDelta().y, -88f, 88f));
      }));
      if (Object.op_Implicit((Object) this.csLight))
        this.csLight.actUpdateColor = (Action<Color>) (color => this.customBase.lightCustom.set_color(color));
      if (Object.op_Implicit((Object) this.sldLightPower))
        ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldLightPower), (Action<M0>) (val => this.customBase.lightCustom.set_intensity(val)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldLightPower), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldLightPower.set_value(Mathf.Clamp(this.sldLightPower.get_value() + (float) scl.get_scrollDelta().y, -178f, 178f));
      }));
      if (Object.op_Implicit((Object) this.btnLightReset))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnLightReset), (Action<M0>) (_ =>
        {
          this.customBase.ResetLightSetting();
          Vector3 localEulerAngles = ((Component) this.customBase.lightCustom).get_transform().get_localEulerAngles();
          this.sldLightRotX.set_value(89.0 >= localEulerAngles.x ? (float) (double) localEulerAngles.x : (float) (localEulerAngles.x - 360.0));
          this.sldLightRotY.set_value(180.0 > localEulerAngles.y ? (float) (double) localEulerAngles.y : (float) (localEulerAngles.y - 360.0));
          this.csLight.SetColor(this.customBase.lightCustom.get_color());
          this.sldLightPower.set_value(this.customBase.lightCustom.get_intensity());
        }));
      if (Object.op_Implicit((Object) this.tglBFrameDraw))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglBFrameDraw), (Action<M0>) (isOn => this.customBase.drawSaveFrameBack = isOn));
      if (((IEnumerable<Button>) this.btnBFramePtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnBFramePtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ => this.customBase.saveFrameAssist.ChangeSaveFrameBack((byte) item.idx, true)))));
      }
      if (Object.op_Implicit((Object) this.tglFFrameDraw))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglFFrameDraw), (Action<M0>) (isOn => this.customBase.drawSaveFrameFront = isOn));
      if (((IEnumerable<Button>) this.btnFFramePtn).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnFFramePtn).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ => this.customBase.saveFrameAssist.ChangeSaveFrameFront((byte) item.idx, true)))));
      }
      if (((IEnumerable<Toggle>) this.tglBG).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglBG).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ =>
        {
          this.customBase.customCtrl.draw3D = 0 == item.idx;
          this.objBGIndex.SetActiveIfDifferent(!this.customBase.customCtrl.draw3D);
          this.objBGColor.SetActiveIfDifferent(this.customBase.customCtrl.draw3D);
          this.objBackFrame.SetActiveIfDifferent(!this.customBase.customCtrl.draw3D);
          this.customBase.forceBackFrameHide = this.customBase.customCtrl.draw3D;
          if (this.customBase.customCtrl.draw3D)
            return;
          this.customBase.customCtrl.showColorCvs = false;
        }))));
      }
      if (((IEnumerable<Button>) this.btnBGIndex).Any<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Button>) this.btnBGIndex).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => item != null)).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ => this.customBase.customCtrl.ChangeBGImage(item.idx)))));
      }
      if (Object.op_Implicit((Object) this.csBG))
        this.csBG.actUpdateColor = (Action<Color>) (color => this.customBase.customCtrl.ChangeBGColor(color));
      if (Object.op_Implicit((Object) this.btnCancel))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCancel), (Action<M0>) (_ =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
          this.customBase.customCtrl.overwriteSavePath = string.Empty;
          this.EndCapture();
          this.customBase.customCtrl.saveMode = false;
          this.customBase.customCtrl.updatePng = false;
        }));
      if (!Object.op_Implicit((Object) this.btnSave))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnSave), (Action<M0>) (_ =>
      {
        if (this.customBase.customCtrl.saveMode)
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Save);
          this.chaCtrl.chaFile.pngData = this.customBase.customCtrl.customCap.CapCharaCard(true, this.customBase.saveFrameAssist, this.customBase.customCtrl.draw3D);
          string empty = string.Empty;
          string filename;
          if (this.customBase.customCtrl.overwriteSavePath.IsNullOrEmpty())
          {
            filename = this.chaCtrl.sex != (byte) 0 ? "AISChaF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") : "AISChaM_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
          }
          else
          {
            filename = this.customBase.customCtrl.overwriteSavePath;
            this.customBase.customCtrl.overwriteSavePath = string.Empty;
          }
          this.chaCtrl.chaFile.SaveCharaFile(filename, byte.MaxValue, false);
          this.customBase.updateCvsCharaSaveDelete = true;
          this.customBase.updateCvsCharaLoad = true;
        }
        else
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Photo);
          this.chaCtrl.chaFile.pngData = this.customBase.customCtrl.customCap.CapCharaCard(true, this.customBase.saveFrameAssist, this.customBase.customCtrl.draw3D);
        }
        this.EndCapture();
        this.customBase.customCtrl.saveMode = false;
        this.customBase.customCtrl.updatePng = false;
      }));
    }
  }
}
