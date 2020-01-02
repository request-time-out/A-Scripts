// Decompiled with JetBrains decompiler
// Type: MultiPlay_F2M1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Manager;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

public class MultiPlay_F2M1 : ProcBase
{
  private float[] timeMotions = new float[2];
  private bool[] enableMotions = new bool[2];
  private bool[] allowMotions = new bool[2]{ true, true };
  private Vector2[] lerpMotions = new Vector2[2]
  {
    Vector2.get_zero(),
    Vector2.get_zero()
  };
  private float[] lerpTimes = new float[2];
  private List<HScene.AnimationListInfo> lstAnimation;
  private int nextPlay;
  private bool oldHit;
  private int finishMotion;
  private bool finishMorS;
  private ProcBase.animParm animPar;

  public MultiPlay_F2M1(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[2];
    this.animPar.m = new float[2];
    this.CatID = 7;
  }

  public override bool Init(int _modeCtrl)
  {
    base.Init(_modeCtrl);
    return true;
  }

  public void SetAnimationList(List<HScene.AnimationListInfo> _list)
  {
    this.lstAnimation = _list;
  }

  public override bool SetStartMotion(
    bool _isIdle,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.AtariEffect.Stop();
    if (_isIdle)
    {
      if (_infoAnimList.nDownPtn != 0)
        this.setPlay(!this.ctrlFlag.isFaintness ? "Idle" : "D_Idle", false);
      else
        this.setPlay("Idle", false);
      this.voice.HouchiTime = 0.0f;
      this.ctrlFlag.loopType = -1;
    }
    else
    {
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
      {
        if (_infoAnimList.nDownPtn != 0)
          this.setPlay(!this.ctrlFlag.isFaintness ? "OLoop" : "D_OLoop", false);
        else
          this.setPlay("OLoop", false);
        this.ctrlFlag.loopType = -1;
      }
      else
      {
        if (_infoAnimList.nDownPtn != 0)
        {
          this.setPlay(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", false);
          if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
          {
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
              this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 1, this.chaFemales[0], 0, false);
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
              this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 1, this.chaFemales[1], 1, false);
          }
          else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
          {
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
              this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 2, this.chaFemales[0], 0, false);
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
              this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 2, this.chaFemales[1], 1, false);
          }
        }
        else
        {
          this.setPlay("WLoop", false);
          if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
          {
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
              this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
              this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
          }
          else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
          {
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
              this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
            if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
              this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
          }
        }
        this.ctrlFlag.loopType = 0;
      }
      if (_infoAnimList.lstSystem.Contains(4))
        this.ctrlFlag.isPainActionParam = true;
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.motions[0] = 0.0f;
      this.ctrlFlag.motions[1] = 0.0f;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.auto.SetSpeed(0.0f);
    }
    this.nextPlay = 0;
    this.oldHit = false;
    this.isHeight1Parameter = this.chaFemales[0].IsParameterInAnimator("height1");
    return true;
  }

  public override bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    if (Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
      return false;
    this.FemaleAi = this.chaFemales[0].getAnimatorStateInfo(0);
    if (this.ctrlFlag.initiative == 0)
    {
      switch (_modeCtrl)
      {
        case 0:
          this.ManualAibu(this.FemaleAi, _infoAnimList);
          break;
        case 1:
        case 2:
          this.ManualHoushi(this.FemaleAi, _modeCtrl, _infoAnimList);
          break;
        case 3:
        case 4:
          this.ManualSonyu(this.FemaleAi, _modeCtrl, _infoAnimList);
          break;
      }
    }
    else
    {
      switch (_modeCtrl)
      {
        case 0:
          this.AutoAibu(this.FemaleAi, _infoAnimList);
          break;
        case 1:
        case 2:
          this.AutoHoushi(this.FemaleAi, _modeCtrl, _infoAnimList);
          break;
        case 3:
        case 4:
          this.AutoSonyu(this.FemaleAi, _modeCtrl, _infoAnimList);
          break;
      }
    }
    for (int index = 0; index < 2; ++index)
    {
      if (this.enableMotions[index])
      {
        this.timeMotions[index] = Mathf.Clamp(this.timeMotions[index] + Time.get_deltaTime(), 0.0f, this.lerpTimes[index]);
        float num = this.ctrlFlag.changeMotionCurve.Evaluate(Mathf.Clamp01(this.timeMotions[index] / this.lerpTimes[index]));
        this.ctrlFlag.motions[index] = Mathf.Lerp((float) this.lerpMotions[index].x, (float) this.lerpMotions[index].y, num);
        if ((double) num >= 1.0)
          this.enableMotions[index] = false;
      }
    }
    this.SetFinishCategoryEnable(this.FemaleAi, _modeCtrl);
    this.ctrlMeta.Proc(this.FemaleAi, false);
    switch (_modeCtrl)
    {
      case 0:
        if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.RecoverFaintness)
        {
          if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Idle") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OLoop")) || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_A"))
          {
            this.setPlay("Orgasm_A", true);
            this.ctrlFlag.isFaintness = false;
            this.sprite.SetVisibleLeaveItToYou(true, true);
            this.ctrlFlag.numOrgasm = 0;
            this.sprite.SetAnimationMenu();
            this.ctrlFlag.isGaugeHit = false;
            this.sprite.SetMotionListDraw(false, -1);
            this.ctrlFlag.nowOrgasm = false;
            this.Hitem.SetUse(6, false);
            if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_A") && this.voice.playAnimation != null)
            {
              this.voice.playAnimation.SetAllFlags(true);
              break;
            }
            break;
          }
          this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
          this.Hitem.SetUse(6, false);
          break;
        }
        break;
      case 1:
      case 2:
        if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.RecoverFaintness)
        {
          if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Idle") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OLoop")) || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_OUT_A"))
          {
            this.ctrlFlag.selectAnimationListInfo = this.RecoverFaintnessAi();
            this.ctrlFlag.isFaintness = false;
            this.sprite.SetVisibleLeaveItToYou(true, true);
            this.ctrlFlag.numOrgasm = 0;
            this.ctrlMeta.Clear();
            this.sprite.SetAnimationMenu();
            this.ctrlFlag.isGaugeHit = false;
            this.ctrlFlag.isGaugeHit_M = false;
            this.sprite.SetMotionListDraw(false, -1);
            this.ctrlFlag.nowOrgasm = false;
            this.Hitem.SetUse(6, false);
            break;
          }
          this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
          this.Hitem.SetUse(6, false);
          break;
        }
        break;
      case 3:
      case 4:
        if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.RecoverFaintness)
        {
          if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Idle") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OLoop")) || (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_IN_A") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OrgasmM_OUT_A")))
          {
            if (_modeCtrl == 3)
              this.setPlay("Orgasm_IN_A", true);
            else
              this.setPlay("OrgasmM_OUT_A", true);
            this.ctrlMeta.Clear();
            this.ctrlFlag.isFaintness = false;
            this.ctrlFlag.numOrgasm = 0;
            this.sprite.SetFinishSelect(7, _modeCtrl, -1, -1);
            this.sprite.SetVisibleLeaveItToYou(true, true);
            this.ctrlMeta.SetParameterFromState(0);
            this.sprite.SetAnimationMenu();
            this.ctrlFlag.isGaugeHit = false;
            this.sprite.SetMotionListDraw(false, -1);
            this.ctrlFlag.nowOrgasm = false;
            this.Hitem.SetUse(6, false);
            if ((((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_IN_A") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OrgasmM_OUT_A")) && this.voice.playAnimation != null)
            {
              this.voice.playAnimation.SetAllFlags(true);
              break;
            }
            break;
          }
          this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
          this.Hitem.SetUse(6, false);
          break;
        }
        break;
    }
    this.setAnimationParamater();
    return true;
  }

  private bool ManualAibu(AnimatorStateInfo _ai, HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartAibuProc(false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.LoopAibuProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.LoopAibuProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.OLoopAibuProc(0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm"))
      this.OrgasmAibuProc(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartAibuProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.FaintnessStartProcTrigger(_wheel);
      this.FaintnessStartAibuProc(true);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopAibuProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopAibuProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopAibuProc(1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm"))
      this.OrgasmAibuProc(1, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_A"))
    {
      this.FaintnessStartProcTrigger(_wheel);
      this.FaintnessStartAibuProc(false);
    }
    return true;
  }

  private bool ManualHoushi(
    AnimatorStateInfo _ai,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(0, false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.LoopHoushiProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.LoopHoushiProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.OLoopHoushiProc(0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_IN"))
      this.SetAfterInsideFinishAnimation(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_IN"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Drink", false, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Drink_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_IN"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Vomit", false, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Vomit_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(1, false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopHoushiProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopHoushiProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopHoushiProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "D_Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(1, true);
    }
    return true;
  }

  private bool ManualSonyu(
    AnimatorStateInfo _ai,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(false, 0);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Insert"))
      this.InsertProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.LoopSonyuProc(0, 0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.LoopSonyuProc(1, 0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.OLoopSonyuProc(0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmF_IN"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmS_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_IN_A"))
      this.AfterTheInsideWaitingProc(0, _wheel, _modeCtrl);
    else if (((AnimatorStateInfo) ref _ai).IsName("Pull"))
      this.PullProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("Drop"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 4f, 0, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_OUT"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(true, 0);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(false, 1);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Insert"))
      this.InsertProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopSonyuProc(0, 1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopSonyuProc(1, 1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopSonyuProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmF_IN"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmS_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_IN_A"))
      this.AfterTheInsideWaitingProc(1, _wheel, _modeCtrl);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Pull"))
      this.PullProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Drop"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 4f, 1, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_OUT"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(true, 1);
    }
    return true;
  }

  private bool AutoAibu(AnimatorStateInfo _ai, HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.AutoStartProcTrigger(false);
      this.AutoStartAibuProc(false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.AutoLoopAibuProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.AutoLoopAibuProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.AutoOLoopAibuProc(0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm"))
      this.OrgasmAibuProc(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartAibuProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.FaintnessStartProcTrigger(_wheel);
      this.FaintnessStartAibuProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopAibuProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopAibuProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopAibuProc(1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm"))
      this.OrgasmAibuProc(1, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_A"))
    {
      this.FaintnessStartProcTrigger(_wheel);
      this.FaintnessStartAibuProc(false);
    }
    return true;
  }

  private bool AutoHoushi(
    AnimatorStateInfo _ai,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.AutoStartProcTrigger(false);
      this.AutoStartHoushiProc(0, false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.AutoLoopHoushiProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.AutoLoopHoushiProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.AutoOLoopHoushiProc(0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_IN"))
      this.SetAfterInsideFinishAnimation(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_IN"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Drink", false, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Drink_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_IN"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Vomit", false, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "Vomit_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_OUT_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartHoushiProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(1, false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopHoushiProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopHoushiProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopHoushiProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "D_Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartHoushiProc(1, true);
    }
    return true;
  }

  private bool AutoSonyu(
    AnimatorStateInfo _ai,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.AutoStartProcTrigger(false);
      this.AutoStartSonyuProc(false, 0, _modeCtrl);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Insert"))
      this.InsertProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.AutoLoopSonyuProc(0, 0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.AutoLoopSonyuProc(1, 0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.AutoOLoopProc(0, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmF_IN"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmS_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_IN_A"))
      this.AutoAfterTheInsideWaitingProc(0, _wheel);
    else if (((AnimatorStateInfo) ref _ai).IsName("Pull"))
      this.PullProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("Drop"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 4f, 0, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_OUT"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 0, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("OrgasmM_OUT_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartSonyuProc(true, 0, _modeCtrl);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(false, 1);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Insert"))
      this.InsertProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopSonyuProc(0, 1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopSonyuProc(1, 1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopSonyuProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmF_IN"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, false);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmS_IN"))
      this.FinishNextAnimationByMorS(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_IN_A"))
      this.AfterTheInsideWaitingProc(1, _wheel, _modeCtrl);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Pull"))
      this.PullProc(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Drop"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 4f, 1, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_OUT"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 1f, 1, _modeCtrl, 2);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OrgasmM_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartSonyuProc(true, 1);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 3f, 0, _modeCtrl, !this.finishMorS ? 1 : 0);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Vomit"))
      this.AfterTheNextWaitingAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), 3f, 1, _modeCtrl, !this.finishMorS ? 1 : 0);
    return true;
  }

  public override void setAnimationParamater()
  {
    this.animPar.breast = this.chaFemales[0].GetShapeBodyValue(1);
    this.animPar.speed = this.ctrlFlag.loopType == 1 ? this.ctrlFlag.speed : this.ctrlFlag.speed + 1f;
    this.animPar.m[0] = this.ctrlFlag.motions[0];
    this.animPar.m[1] = this.ctrlFlag.motions[1];
    for (int index = 0; index < this.chaFemales.Length; ++index)
    {
      if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        this.animPar.heights[index] = this.chaFemales[index].GetShapeBodyValue(0);
    }
    for (int index = 0; index < this.chaFemales.Length; ++index)
    {
      if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null))
      {
        this.chaFemales[index].setAnimatorParamFloat("height", this.animPar.heights[index]);
        this.chaFemales[index].setAnimatorParamFloat("speed", this.animPar.speed);
        this.chaFemales[index].setAnimatorParamFloat("motion", this.animPar.m[0]);
        this.chaFemales[index].setAnimatorParamFloat("motion1", this.animPar.m[1]);
        this.chaFemales[index].setAnimatorParamFloat("breast", this.animPar.breast);
        if (this.isHeight1Parameter)
          this.chaFemales[index].setAnimatorParamFloat("height1", this.animPar.heights[index ^ 1]);
      }
    }
    if (Object.op_Inequality((Object) this.chaMales[0].objBodyBone, (Object) null))
    {
      this.chaMales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
      this.chaMales[0].setAnimatorParamFloat("speed", this.animPar.speed);
      this.chaMales[0].setAnimatorParamFloat("motion", this.animPar.m[0]);
      this.chaMales[0].setAnimatorParamFloat("breast", this.animPar.breast);
      if (this.isHeight1Parameter)
        this.chaMales[0].setAnimatorParamFloat("height1", this.animPar.heights[1]);
    }
    if (!Object.op_Inequality((Object) this.item.GetItem(), (Object) null))
      return;
    this.item.setAnimatorParamFloat("height", this.animPar.heights[0]);
    this.item.setAnimatorParamFloat("speed", this.animPar.speed);
    this.item.setAnimatorParamFloat("motion", this.animPar.m[0]);
    if (!this.isHeight1Parameter)
      return;
    this.item.setAnimatorParamFloat("height1", this.animPar.heights[1]);
  }

  private void setPlay(string _playAnimation, bool _isFade = true)
  {
    this.chaFemales[0].setPlay(_playAnimation, 0);
    if (this.chaFemales[1].visibleAll && Object.op_Inequality((Object) this.chaFemales[1].objTop, (Object) null))
      this.chaFemales[1].setPlay(_playAnimation, 0);
    if (Object.op_Inequality((Object) this.chaMales[0].objTop, (Object) null))
      this.chaMales[0].setPlay(_playAnimation, 0);
    if (this.item != null)
      this.item.setPlay(_playAnimation);
    for (int index = 0; index < this.lstMotionIK.Count; ++index)
      this.lstMotionIK[index].Item3.Calc(_playAnimation);
    if (this.item != null)
      this.item.setPlay(_playAnimation);
    if (!_isFade)
      return;
    this.fade.FadeStart(1f);
  }

  private bool StartProcTrigger(float _wheel)
  {
    if ((double) _wheel == 0.0 || this.nextPlay != 0 || this.ctrlFlag.voice.playStart > 4)
      return false;
    this.nextPlay = 1;
    return true;
  }

  private bool StartAibuProc(bool _isReStart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_isReStart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
      if (this.ctrlFlag.voice.playStart > 4)
        return false;
    }
    this.nextPlay = 0;
    this.setPlay("WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
    }
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.ctrlFlag.nowSpeedStateFast = false;
    for (int index = 0; index < 2; ++index)
    {
      this.timeMotions[index] = 0.0f;
      this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
      this.timeChangeMotionDeltaTimes[index] = 0.0f;
    }
    this.ctrlFlag.isNotCtrl = false;
    this.oldHit = false;
    this.feelHit.InitTime();
    if (_isReStart)
      this.voice.AfterFinish();
    return true;
  }

  private bool FaintnessStartProcTrigger(float _wheel)
  {
    if ((double) _wheel == 0.0 || this.nextPlay != 0)
      return false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
        return false;
    }
    if (this.ctrlFlag.voice.playStart > 4)
      return false;
    this.nextPlay = 1;
    return true;
  }

  private bool FaintnessStartAibuProc(bool _start)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      if (_start)
      {
        this.nextPlay = 3;
      }
      else
      {
        this.nextPlay = 2;
        if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item2 == null)
          this.ctrlFlag.voice.playStart = 3;
      }
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
      if (this.ctrlFlag.voice.playStart > 4)
        return false;
    }
    this.nextPlay = 0;
    this.setPlay("D_WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC("D_WLoop", 1, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC("D_WLoop", 2, this.chaFemales[1], 1, false);
    }
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC("D_WLoop", 2, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC("D_WLoop", 2, this.chaFemales[1], 1, false);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.ctrlFlag.isNotCtrl = false;
    this.oldHit = false;
    this.feelHit.InitTime();
    this.voice.AfterFinish();
    return true;
  }

  private bool LoopAibuProc(
    int _loop,
    int _state,
    float _wheel,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.ctrlFlag.feel_f = 0.7f;
      this.oldHit = false;
      this.ctrlFlag.isGaugeHit = false;
    }
    else
    {
      this.ctrlFlag.speed += _wheel;
      if (_loop == 0)
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      else
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 1.5;
      if (_state == 0)
      {
        for (int index = 0; index < 2; ++index)
        {
          if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
          {
            this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
            if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index])
            {
              this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
              this.timeChangeMotionDeltaTimes[index] = 0.0f;
              this.enableMotions[index] = true;
              this.timeMotions[index] = 0.0f;
              float num1;
              if (this.allowMotions[index])
              {
                float num2 = 1f - this.ctrlFlag.motions[index];
                num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
                if ((double) num1 >= 1.0)
                  this.allowMotions[index] = false;
              }
              else
              {
                float motion = this.ctrlFlag.motions[index];
                num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
                if ((double) num1 <= 0.0)
                  this.allowMotions[index] = true;
              }
              this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
              this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
            }
          }
        }
      }
      if (_loop == 0)
      {
        if ((double) this.ctrlFlag.speed > 1.0 && this.ctrlFlag.loopType == 0)
        {
          this.setPlay(_state != 0 ? "D_SLoop" : "SLoop", true);
          this.ctrlFlag.nowSpeedStateFast = false;
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 1;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      else
      {
        if ((double) this.ctrlFlag.speed < 1.0 && this.ctrlFlag.loopType == 1)
        {
          this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
          this.ctrlFlag.nowSpeedStateFast = true;
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 0;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, _loop, _loop != 0 ? this.ctrlFlag.speed - 1f : this.ctrlFlag.speed);
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      if (this.ctrlFlag.isGaugeHit)
      {
        this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
            num *= this.ctrlFlag.SkilChangeSpeed(0);
          else if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0))
        {
          if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 3)
            this.ctrlFlag.voice.playShorts[0] = 0;
          if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
            this.ctrlFlag.voice.playShorts[1] = 0;
        }
        this.ctrlFlag.voice.dialog = false;
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        this.oldHit = false;
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool OLoopAibuProc(int _state, float _wheel, HScene.AnimationListInfo _infoAnimList)
  {
    this.ctrlFlag.speed = Mathf.Clamp01(this.ctrlFlag.speed + _wheel);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, 2, this.ctrlFlag.speed);
    ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
    if (this.ctrlFlag.isGaugeHit)
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
      if (!ProcBase.hSceneManager.bMerchant)
      {
        if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
          num *= this.ctrlFlag.SkilChangeSpeed(0);
        else if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
          num *= this.ctrlFlag.SkilChangeSpeed(15);
        if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num *= this.ctrlFlag.SkilChangeSpeed(3);
      }
      this.ctrlFlag.feel_f += num;
      this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
    }
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0))
      {
        if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 3)
          this.ctrlFlag.voice.playShorts[0] = 0;
        if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
          this.ctrlFlag.voice.playShorts[1] = 0;
      }
      this.ctrlFlag.voice.dialog = false;
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
    {
      this.setPlay(_state != 0 ? "D_Orgasm" : "Orgasm", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (_state != 0)
        this.ctrlFlag.AddParam(15, 1);
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.voice.oldFinish = 0;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.dialog = false;
      this.ctrlFlag.rateNip = 1f;
      if (Manager.Config.HData.Gloss)
        this.ctrlFlag.rateTuya = 1f;
      bool sio = Manager.Config.HData.Sio;
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.nowOrgasm = true;
    }
    return true;
  }

  private bool GotoFaintnessAibu(int _state)
  {
    bool flag = this.Hitem.Effect(5);
    if (_state == 0 && (this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag))
    {
      this.setPlay("D_Orgasm_A", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (flag)
        this.Hitem.SetUse(5, false);
      if (!ProcBase.hSceneManager.bMerchant && this.ctrlFlag.numFaintness == 0)
        this.ctrlFlag.AddParam(14, 1);
      this.ctrlFlag.isFaintness = true;
      this.ctrlFlag.numFaintness = Mathf.Clamp(this.ctrlFlag.numFaintness + 1, 0, 999999);
      this.sprite.SetVisibleLeaveItToYou(false, false);
      if (ProcBase.hSceneManager.EventKind == HSceneManager.HEvent.GyakuYobai)
      {
        ((Selectable) this.sprite.categoryMainButton).set_interactable(true);
        ((Selectable) this.sprite.hPointButton).set_interactable(true);
      }
      this.sprite.SetToggleLeaveItToYou(false);
      if (this.ctrlFlag.initiative != 0)
      {
        this.ctrlFlag.initiative = 0;
        this.sprite.MainCategoryOfLeaveItToYou(false);
      }
    }
    else
    {
      this.setPlay(_state != 0 ? "D_Orgasm_A" : "Orgasm_A", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
    }
    this.ctrlFlag.nowOrgasm = false;
    return true;
  }

  private bool OrgasmAibuProc(int _state, float _normalizedTime)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    this.GotoFaintnessAibu(_state);
    return true;
  }

  private bool StartHoushiProc(int _state, bool _restart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
      if (this.ctrlFlag.voice.playStart > 4)
        return false;
    }
    this.nextPlay = 0;
    this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.isNotCtrl = false;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.oldHit = false;
    for (int index = 0; index < 2; ++index)
    {
      this.ctrlFlag.motions[index] = 0.0f;
      this.timeMotions[index] = 0.0f;
      this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
      this.timeChangeMotionDeltaTimes[index] = 0.0f;
    }
    this.feelHit.InitTime();
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    return true;
  }

  private bool LoopHoushiProc(
    int _loop,
    int _state,
    float _wheel,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.ctrlFlag.feel_m = 0.7f;
      this.oldHit = false;
      if (this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 4)
      {
        if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null) && (this.voice.nowVoices[0].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[0].state == HVoiceCtrl.VoiceKind.startVoice))
          Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[0]);
        this.ctrlFlag.voice.dialog = false;
      }
      this.feelHit.InitTime();
      this.ctrlFlag.isGaugeHit = false;
    }
    else
    {
      this.ctrlFlag.speed += _wheel;
      if (_loop == 0)
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      else
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 1.5;
      for (int index = 0; index < 2; ++index)
      {
        if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        {
          this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
          if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index])
          {
            this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
            this.timeChangeMotionDeltaTimes[0] = 0.0f;
            this.enableMotions[index] = true;
            this.timeMotions[index] = 0.0f;
            float num1;
            if (this.allowMotions[index])
            {
              float num2 = 1f - this.ctrlFlag.motions[index];
              num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
              if ((double) num1 >= 1.0)
                this.allowMotions[index] = false;
            }
            else
            {
              float motion = this.ctrlFlag.motions[index];
              num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
              if ((double) num1 <= 0.0)
                this.allowMotions[index] = true;
            }
            this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
            this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
          }
        }
      }
      if (_loop == 0)
      {
        if ((double) this.ctrlFlag.speed > 1.0 && this.ctrlFlag.loopType == 0)
        {
          this.setPlay(_state != 0 ? "D_SLoop" : "SLoop", true);
          this.ctrlFlag.nowSpeedStateFast = false;
          if (this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 4)
          {
            for (int index = 0; index < 2; ++index)
            {
              if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
                Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
            }
            this.ctrlFlag.voice.dialog = false;
          }
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 1;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      else
      {
        if ((double) this.ctrlFlag.speed < 1.0 && this.ctrlFlag.loopType == 1)
        {
          this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
          this.ctrlFlag.nowSpeedStateFast = true;
          if (this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 4)
          {
            for (int index = 0; index < 2; ++index)
            {
              if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
                Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
            }
            this.ctrlFlag.voice.dialog = false;
          }
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 0;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, _loop, _loop != 0 ? this.ctrlFlag.speed - 1f : this.ctrlFlag.speed);
      if (this.ctrlFlag.isGaugeHit)
        this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 1.0 : 2.0) * (!this.ctrlFlag.stopFeelMale ? 1.0 : 0.0));
      if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(18))
        num *= this.ctrlFlag.SkilChangeSpeed(18);
      this.ctrlFlag.feel_m += num;
      this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        this.ctrlFlag.voice.dialog = false;
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_m >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        if (this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 4)
        {
          for (int index = 0; index < 2; ++index)
          {
            if (!this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
              Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
          }
          this.ctrlFlag.voice.dialog = false;
        }
        this.oldHit = false;
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool OLoopHoushiProc(
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.ctrlFlag.speed = Mathf.Clamp01(this.ctrlFlag.speed + _wheel);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, 2, this.ctrlFlag.speed);
    this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
    float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 1.0 : 2.0) * (!this.ctrlFlag.stopFeelMale ? 1.0 : 0.0));
    if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(18))
      num *= this.ctrlFlag.SkilChangeSpeed(18);
    this.ctrlFlag.feel_m += num;
    this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
    ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      this.ctrlFlag.voice.dialog = false;
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide)
    {
      this.setPlay(_state != 0 ? "D_Orgasm_OUT" : "Orgasm_OUT", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.voice.dialog = false;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishDrink && _modeCtrl != 1)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 0;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishVomit && _modeCtrl != 1)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 1;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    return true;
  }

  private bool SetNextFinishAnimation(
    float _normalizedTime,
    string _nextAnimation,
    bool _isSpriteSet = true,
    bool _isFade = true)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    this.setPlay(_nextAnimation, _isFade);
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = -1;
    if (_isSpriteSet)
      this.ctrlFlag.nowOrgasm = false;
    return true;
  }

  private bool SetAfterInsideFinishAnimation(int _state, float _normalizedTime)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    if (this.finishMotion == 0)
    {
      this.ctrlFlag.numDrink = Mathf.Clamp(this.ctrlFlag.numDrink + 1, 0, 999999);
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.setPlay("Drink_IN", true);
    }
    else if (this.finishMotion == 1)
    {
      this.ctrlFlag.numVomit = Mathf.Clamp(this.ctrlFlag.numVomit + 1, 0, 999999);
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.setPlay("Vomit_IN", true);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = -1;
    return true;
  }

  private bool StartSonyuProc(bool _restart, int _state)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
      if (this.ctrlFlag.voice.playStart > 4)
        return false;
    }
    this.setPlay(_state != 0 ? "D_Insert" : "Insert", true);
    this.ctrlFlag.loopType = -1;
    this.nextPlay = 0;
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.ctrlFlag.isNotCtrl = false;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.timeMotions[0] = 0.0f;
    this.timeMotions[1] = 0.0f;
    this.oldHit = false;
    this.feelHit.InitTime();
    if (_state == 0)
    {
      for (int index = 0; index < 2; ++index)
      {
        this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
        this.timeChangeMotionDeltaTimes[index] = 0.0f;
      }
    }
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    return true;
  }

  private bool InsertProc(float _normalizedTime, int _state)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    return true;
  }

  private bool LoopSonyuProc(
    int _loop,
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishInSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && _modeCtrl == 3)
    {
      this.setPlay(new string[2]
      {
        "OrgasmM_IN",
        "D_OrgasmM_IN"
      }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 1;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.dialog = false;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(new string[2]
      {
        "OrgasmM_OUT",
        "D_OrgasmM_OUT"
      }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.dialog = false;
      if (this.ctrlFlag.numOutSide < 4)
        ProcBase.hSceneManager.Bath += 10f;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(new string[2]{ "OLoop", "D_OLoop" }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_f = 0.7f;
      if ((double) this.ctrlFlag.feel_m <= 0.699999988079071)
        this.ctrlFlag.feel_m = 0.7f;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.oldHit = false;
      this.feelHit.InitTime();
      for (int index = 0; index < 2; ++index)
      {
        if (this.chaFemales[index].visibleAll && Object.op_Inequality((Object) this.chaFemales[index].objTop, (Object) null))
        {
          if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 3)
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
          else if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 3)
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
        }
      }
      this.ctrlFlag.isGaugeHit = false;
    }
    else
    {
      this.ctrlFlag.speed += _wheel;
      if (_loop == 0)
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      else
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 1.5;
      for (int index = 0; index < 2; ++index)
      {
        if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        {
          this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
          if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index] && _state == 0)
          {
            this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
            this.timeChangeMotionDeltaTimes[index] = 0.0f;
            this.enableMotions[index] = true;
            this.timeMotions[index] = 0.0f;
            float num1;
            if (this.allowMotions[index])
            {
              float num2 = 1f - this.ctrlFlag.motions[index];
              num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
              if ((double) num1 >= 1.0)
                this.allowMotions[index] = false;
            }
            else
            {
              float motion = this.ctrlFlag.motions[index];
              num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
              if ((double) num1 <= 0.0)
                this.allowMotions[index] = true;
            }
            this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
            this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
          }
        }
      }
      if (_loop == 0)
      {
        if ((double) this.ctrlFlag.speed > 1.0 && this.ctrlFlag.loopType == 0)
        {
          this.setPlay(_state != 0 ? "D_SLoop" : "SLoop", true);
          this.ctrlFlag.nowSpeedStateFast = false;
          for (int index = 0; index < 2; ++index)
          {
            if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null))
            {
              if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice)
                Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
              else if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
                Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
            }
          }
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 1;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      else
      {
        if ((double) this.ctrlFlag.speed < 1.0 && this.ctrlFlag.loopType == 1)
        {
          this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
          this.ctrlFlag.nowSpeedStateFast = true;
          for (int index = 0; index < 2; ++index)
          {
            if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice) && (this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 3 && this.ctrlFlag.nowAnimationInfo.id == 0))
              Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
          }
          this.feelHit.InitTime();
          this.ctrlFlag.loopType = 0;
        }
        this.ctrlFlag.speed = Mathf.Clamp(this.ctrlFlag.speed, 0.0f, 2f);
      }
      if (_state != 1 || _modeCtrl != 4)
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelMale ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(1))
          num *= this.ctrlFlag.SkilChangeSpeed(1);
        this.ctrlFlag.feel_m += num;
        this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
      }
      this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, _loop, _loop != 0 ? this.ctrlFlag.speed - 1f : this.ctrlFlag.speed);
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      if (this.ctrlFlag.isGaugeHit)
      {
        this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0) && _modeCtrl != 4)
        {
          if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 3)
            this.ctrlFlag.voice.playShorts[0] = 0;
          if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
            this.ctrlFlag.voice.playShorts[1] = 0;
        }
        this.ctrlFlag.voice.dialog = false;
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        this.oldHit = false;
        this.feelHit.InitTime();
        for (int index = 0; index < 2; ++index)
        {
          if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID == 3)
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
        }
      }
    }
    return true;
  }

  private bool OLoopSonyuProc(
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      if ((double) this.ctrlFlag.feel_m <= 0.699999988079071)
      {
        this.ctrlFlag.feel_m = 0.7f;
        this.ctrlFlag.isGaugeHit = false;
      }
      return true;
    }
    if (_state != 1 || _modeCtrl != 4)
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelMale ? 1f : 0.0f);
      if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(1))
        num *= this.ctrlFlag.SkilChangeSpeed(1);
      this.ctrlFlag.feel_m += num;
      this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
    }
    this.ctrlFlag.speed = Mathf.Clamp01(this.ctrlFlag.speed + _wheel);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, 2, this.ctrlFlag.speed);
    if (this.ctrlFlag.isGaugeHit)
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
      if (!ProcBase.hSceneManager.bMerchant)
      {
        if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
          num *= this.ctrlFlag.SkilChangeSpeed(15);
        if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num *= this.ctrlFlag.SkilChangeSpeed(3);
      }
      this.ctrlFlag.feel_f += num;
      this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
    }
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0) && _modeCtrl != 4)
      {
        if ((_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 3) && _infoAnimList.nAnimListInfoID != 3)
          this.ctrlFlag.voice.playShorts[0] = 0;
        if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
          this.ctrlFlag.voice.playShorts[1] = 0;
      }
      this.ctrlFlag.voice.dialog = false;
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
    {
      this.setPlay(_state != 0 ? "D_OrgasmF_IN" : "OrgasmF_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 0;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.dialog = false;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.rateNip = 1f;
      if (Manager.Config.HData.Gloss)
        this.ctrlFlag.rateTuya = 1f;
      bool sio = Manager.Config.HData.Sio;
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishInSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && _modeCtrl == 3)
    {
      this.setPlay(_state != 0 ? "D_OrgasmM_IN" : "OrgasmM_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 1;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.dialog = false;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(_state != 0 ? "D_OrgasmM_OUT" : "OrgasmM_OUT", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.dialog = false;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishSame && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(_state != 0 ? "D_OrgasmS_IN" : "OrgasmS_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      this.ctrlFlag.numSameOrgasm = Mathf.Clamp(this.ctrlFlag.numSameOrgasm + 1, 0, 999999);
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.dialog = false;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      if (_modeCtrl == 3)
        this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      else
        this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.voice.oldFinish = 3;
      bool urine = Manager.Config.HData.Urine;
      bool sio = Manager.Config.HData.Sio;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
    }
    return true;
  }

  private bool AfterTheNextWaitingAnimation(
    float _normalizedTime,
    float _loopCount,
    int _state,
    int _modeCtrl,
    int _nextAfter)
  {
    if ((double) _normalizedTime < (double) _loopCount)
      return false;
    switch (_nextAfter)
    {
      case 0:
        this.GotoFaintnessSonyu(_state, _modeCtrl, _modeCtrl != 3 ? 1 : 0);
        break;
      case 1:
        this.setPlay(_state != 0 ? "D_Orgasm_IN_A" : "Orgasm_IN_A", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        break;
      case 2:
        this.setPlay(_state != 0 ? "D_OrgasmM_OUT_A" : "OrgasmM_OUT_A", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.isInsert = false;
        break;
    }
    this.ctrlFlag.nowOrgasm = false;
    return true;
  }

  private bool GotoFaintnessSonyu(int _state, int _modeCtrl, int _nextAfter)
  {
    bool flag = this.Hitem.Effect(5);
    if (_state == 0 && (this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag))
    {
      this.setPlay(_nextAfter != 0 ? "D_OrgasmM_OUT_A" : "D_Orgasm_IN_A", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (flag)
        this.Hitem.SetUse(5, false);
      if (!ProcBase.hSceneManager.bMerchant && this.ctrlFlag.numFaintness == 0)
        this.ctrlFlag.AddParam(14, 1);
      this.ctrlFlag.isFaintness = true;
      this.ctrlFlag.numFaintness = Mathf.Clamp(this.ctrlFlag.numFaintness + 1, 0, 999999);
      this.sprite.SetVisibleLeaveItToYou(false, false);
      if (ProcBase.hSceneManager.EventKind == HSceneManager.HEvent.GyakuYobai)
      {
        ((Selectable) this.sprite.categoryMainButton).set_interactable(true);
        ((Selectable) this.sprite.hPointButton).set_interactable(true);
      }
      this.ctrlMeta.SetParameterFromState(1);
      this.sprite.SetToggleLeaveItToYou(false);
      if (this.ctrlFlag.initiative != 0)
      {
        this.ctrlFlag.initiative = 0;
        this.sprite.MainCategoryOfLeaveItToYou(false);
      }
      this.sprite.SetAnimationMenu();
      this.sprite.SetFinishSelect(7, _modeCtrl, -1, -1);
    }
    else
    {
      this.setPlay(_state != 0 ? (_nextAfter != 0 ? "D_OrgasmM_OUT_A" : "D_Orgasm_IN_A") : (_nextAfter != 0 ? "OrgasmM_OUT_A" : "Orgasm_IN_A"), true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
    }
    return true;
  }

  private bool FinishNextAnimationByMorS(
    float _normalizedTime,
    float _loopCount,
    int _state,
    int _modeCtrl,
    bool _finishMorS)
  {
    this.AfterTheNextWaitingAnimation(_normalizedTime, _loopCount, _state, _modeCtrl, !_finishMorS ? 1 : 0);
    return true;
  }

  private bool AfterTheInsideWaitingProc(int _state, float _wheel, int _modeCtrl)
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
        return false;
    }
    if (this.ctrlFlag.voice.dialog || this.ctrlFlag.voice.playStart > 4)
      return false;
    switch (this.nextPlay)
    {
      case 0:
        if ((double) _wheel < 0.0)
        {
          this.setPlay(_state != 0 ? "D_Pull" : "Pull", true);
          this.ctrlFlag.speed = 0.0f;
          this.ctrlFlag.loopType = -1;
          for (int index = 0; index < 2; ++index)
          {
            this.ctrlFlag.motions[index] = 0.0f;
            this.timeMotions[index] = 0.0f;
          }
          this.sprite.objMotionListPanel.SetActive(false);
          this.sprite.SetEnableCategoryMain(false);
          this.sprite.SetEnableHItem(false);
          this.ctrlFlag.nowOrgasm = true;
          this.voice.AfterFinish();
          this.oldHit = false;
          this.feelHit.InitTime();
          break;
        }
        if ((double) _wheel > 0.0)
        {
          this.ctrlFlag.voice.playStart = 3;
          this.nextPlay = 1;
          break;
        }
        break;
      case 1:
        this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
        if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
        {
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
        }
        else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
        {
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
        }
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = 0;
        this.ctrlFlag.nowSpeedStateFast = false;
        for (int index = 0; index < 2; ++index)
        {
          this.ctrlFlag.motions[index] = 0.0f;
          this.timeMotions[index] = 0.0f;
        }
        if (_state == 0)
        {
          for (int index = 0; index < 2; ++index)
          {
            this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
            this.timeChangeMotionDeltaTimes[index] = 0.0f;
          }
        }
        this.voice.AfterFinish();
        this.oldHit = false;
        this.feelHit.InitTime();
        this.nextPlay = 0;
        this.ctrlMeta.Clear();
        break;
    }
    return true;
  }

  private bool PullProc(float _normalizedTime, int _state)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    if (this.ctrlFlag.isInsert)
    {
      this.setPlay(_state != 0 ? "D_Drop" : "Drop", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
    }
    else
    {
      this.setPlay(_state != 0 ? "D_OrgasmM_OUT_A" : "OrgasmM_OUT_A", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowOrgasm = false;
    }
    return true;
  }

  private void SetFinishCategoryEnable(AnimatorStateInfo _ai, int _modeCtrl)
  {
    bool flag1 = ((AnimatorStateInfo) ref _ai).IsName("WLoop") || ((AnimatorStateInfo) ref _ai).IsName("SLoop") || ((AnimatorStateInfo) ref _ai).IsName("D_WLoop") || ((AnimatorStateInfo) ref _ai).IsName("D_SLoop") || ((AnimatorStateInfo) ref _ai).IsName("OLoop") || ((AnimatorStateInfo) ref _ai).IsName("D_OLoop");
    bool flag2 = (double) this.ctrlFlag.feel_m >= 0.699999988079071 && flag1;
    this.sprite.categoryFinish.SetActive((_modeCtrl == 3 || _modeCtrl == 4 ? ((double) this.ctrlFlag.feel_f < 0.699999988079071 || (double) this.ctrlFlag.feel_m < 0.699999988079071) && flag1 : (double) this.ctrlFlag.feel_f < 0.699999988079071 || (double) this.ctrlFlag.feel_m < 0.699999988079071) & Manager.Config.HData.FinishButton, 3);
    if (this.sprite.IsFinishVisible(0))
      this.sprite.categoryFinish.SetActive(flag2 && _modeCtrl != 0, 0);
    if (this.sprite.IsFinishVisible(1))
      this.sprite.categoryFinish.SetActive(flag2 && (double) this.ctrlFlag.feel_f >= 0.699999988079071 && _modeCtrl == 3, 1);
    if (this.sprite.IsFinishVisible(2))
      this.sprite.categoryFinish.SetActive(flag2 && _modeCtrl == 3, 2);
    if (this.sprite.IsFinishVisible(4))
      this.sprite.categoryFinish.SetActive(flag2 && _modeCtrl == 2, 4);
    if (!this.sprite.IsFinishVisible(5))
      return;
    this.sprite.categoryFinish.SetActive(flag2 && _modeCtrl == 2, 5);
  }

  private HScene.AnimationListInfo RecoverFaintnessAi()
  {
    using (List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>.Enumerator enumerator = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfo.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion> current = enumerator.Current;
        if ((HSceneManager.HEvent) current.Item1 == ProcBase.hSceneManager.EventKind && current.Item2 == ProcBase.hSceneManager.height && ((HScene.StartMotion) current.Item3).mode == 1)
          return this.lstAnimation[((HScene.StartMotion) current.Item3).id];
      }
    }
    this.sbWarning.Clear();
    if (this.lstAnimation.Count == 0)
    {
      this.sbWarning.Append("RecoverFaintnessAi：失敗\n");
      Debug.LogWarning((object) this.sbWarning.ToString());
      return (HScene.AnimationListInfo) null;
    }
    this.sbWarning.Append("RecoverFaintnessAi：失敗\n").Append("回復後の体位を").Append(this.lstAnimation[0].nameAnimation).Append("に設定");
    Debug.LogWarning((object) this.sbWarning.ToString());
    return this.lstAnimation[0];
  }

  private bool AutoStartProcTrigger(bool _start)
  {
    if (this.nextPlay != 0)
      return false;
    if (!_start)
    {
      if (!this.auto.IsStart())
        return false;
    }
    else if (!this.auto.IsReStart())
      return false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
        return false;
    }
    this.nextPlay = 1;
    return true;
  }

  private bool AutoStartAibuProc(bool _isReStart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_isReStart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
    }
    this.nextPlay = 0;
    if (!_isReStart || _isReStart && !this.auto.IsChangeActionAtRestart())
    {
      this.setPlay("WLoop", true);
      if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
      {
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
          this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
          this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
      }
      else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      {
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
          this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
          this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[1], 1, false);
      }
    }
    else
      this.ctrlFlag.isAutoActionChange = true;
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.oldHit = false;
    for (int index = 0; index < 2; ++index)
    {
      this.timeMotions[index] = 0.0f;
      this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
      this.timeChangeMotionDeltaTimes[index] = 0.0f;
    }
    this.ctrlFlag.isNotCtrl = false;
    this.auto.Reset();
    this.feelHit.InitTime();
    if (_isReStart)
      this.voice.AfterFinish();
    return true;
  }

  private bool AutoLoopAibuProc(
    int _loop,
    int _state,
    float _wheel,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_f = 0.7f;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.auto.SetSpeed(0.0f);
      this.oldHit = false;
      this.feelHit.InitTime();
      this.ctrlFlag.isGaugeHit = false;
    }
    else
    {
      if (_state == 0)
      {
        for (int index = 0; index < 2; ++index)
        {
          if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
          {
            this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
            if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index])
            {
              this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
              this.timeChangeMotionDeltaTimes[index] = 0.0f;
              this.enableMotions[index] = true;
              this.timeMotions[index] = 0.0f;
              float num1;
              if (this.allowMotions[index])
              {
                float num2 = 1f - this.ctrlFlag.motions[index];
                num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
                if ((double) num1 >= 1.0)
                  this.allowMotions[index] = false;
              }
              else
              {
                float motion = this.ctrlFlag.motions[index];
                num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
                if ((double) num1 <= 0.0)
                  this.allowMotions[index] = true;
              }
              this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
              this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
            }
          }
        }
      }
      this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
      Vector2 hitArea = this.feelHit.GetHitArea(_infoAnimList.nFeelHit, _loop);
      if (this.auto.ChangeLoopMotion(_loop == 1))
      {
        this.setPlay(_loop != 0 ? (_state != 0 ? "D_WLoop" : "WLoop") : (_state != 0 ? "D_SLoop" : "SLoop"), true);
        for (int index = 0; index < 2; ++index)
        {
          if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
        }
        this.feelHit.InitTime();
      }
      else if (this.auto.AddSpeed(_wheel, _loop))
      {
        this.setPlay(_loop != 0 ? (_state != 0 ? "D_WLoop" : "WLoop") : (_state != 0 ? "D_SLoop" : "SLoop"), true);
        for (int index = 0; index < 2; ++index)
        {
          if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
        }
        this.feelHit.InitTime();
      }
      if (_loop == 0)
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(false);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      else
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(true);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      this.ctrlFlag.isGaugeHit = GlobalMethod.RangeOn<float>(this.ctrlFlag.speed, (float) hitArea.x, (float) hitArea.y);
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      if (_state == 1)
      {
        if (this.ctrlFlag.isGaugeHit)
        {
          float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
          if (!ProcBase.hSceneManager.bMerchant)
          {
            if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
              num *= this.ctrlFlag.SkilChangeSpeed(0);
            else if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
              num *= this.ctrlFlag.SkilChangeSpeed(15);
            if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
              num *= this.ctrlFlag.SkilChangeSpeed(3);
          }
          this.ctrlFlag.feel_f += num;
          this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
        }
      }
      else
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 0.300000011920929 : 1.0) * (!this.ctrlFlag.stopFeelFemal ? 1.0 : 0.0));
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
            num *= this.ctrlFlag.SkilChangeSpeed(0);
          else if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
      if (this.ctrlFlag.selectAnimationListInfo == null)
        this.ctrlFlag.isAutoActionChange = this.auto.IsChangeActionAtLoop();
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0))
        {
          this.ctrlFlag.voice.playShorts[0] = 0;
          this.ctrlFlag.voice.playShorts[1] = 0;
        }
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        this.oldHit = false;
        this.auto.SetSpeed(0.0f);
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool AutoOLoopAibuProc(int _state, float _wheel, HScene.AnimationListInfo _infoAnimList)
  {
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    Vector2 hitArea = this.feelHit.GetHitArea(_infoAnimList.nFeelHit, 2);
    this.auto.ChangeSpeed(false, hitArea);
    this.auto.AddSpeed(_wheel, 2);
    this.ctrlFlag.speed = this.auto.GetSpeed(false);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.ctrlFlag.isGaugeHit = GlobalMethod.RangeOn<float>(this.ctrlFlag.speed, (float) hitArea.x, (float) hitArea.y);
    ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
    if (_state == 1)
    {
      if (this.ctrlFlag.isGaugeHit)
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
    }
    else
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 0.300000011920929 : 1.0) * (!this.ctrlFlag.stopFeelFemal ? 1.0 : 0.0));
      if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(3))
        num *= this.ctrlFlag.SkilChangeSpeed(3);
      this.ctrlFlag.feel_f += num;
      this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
    }
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0))
      {
        this.ctrlFlag.voice.playShorts[0] = 0;
        this.ctrlFlag.voice.playShorts[1] = 0;
      }
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
    {
      this.setPlay(_state != 0 ? "D_Orgasm" : "Orgasm", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (_state != 0)
        this.ctrlFlag.AddParam(15, 1);
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.voice.oldFinish = 0;
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.rateNip = 1f;
      if (Manager.Config.HData.Gloss)
        this.ctrlFlag.rateTuya = 1f;
      bool sio = Manager.Config.HData.Sio;
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.nowOrgasm = true;
    }
    return true;
  }

  private bool AutoStartHoushiProc(int _state, bool _restart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
    }
    if (!_restart || _restart && !this.auto.IsChangeActionAtRestart())
    {
      this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
      if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
      {
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
          this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
          this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
      }
      else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      {
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
          this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
        if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
          this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
      }
    }
    else
      this.ctrlFlag.isAutoActionChange = true;
    this.nextPlay = 0;
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.isNotCtrl = false;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.oldHit = false;
    for (int index = 0; index < 2; ++index)
    {
      this.ctrlFlag.motions[index] = 0.0f;
      this.timeMotions[index] = 0.0f;
      this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
      this.timeChangeMotionDeltaTimes[index] = 0.0f;
    }
    this.feelHit.InitTime();
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    this.auto.Reset();
    return true;
  }

  private bool AutoLoopHoushiProc(
    int _loop,
    int _state,
    float _wheel,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.ctrlFlag.feel_m = 0.7f;
      this.oldHit = false;
      this.feelHit.InitTime();
      this.ctrlFlag.isGaugeHit = false;
    }
    else
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        {
          this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
          if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index])
          {
            this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
            this.timeChangeMotionDeltaTimes[index] = 0.0f;
            this.enableMotions[index] = true;
            this.timeMotions[index] = 0.0f;
            float num1;
            if (this.allowMotions[index])
            {
              float num2 = 1f - this.ctrlFlag.motions[index];
              num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
              if ((double) num1 >= 1.0)
                this.allowMotions[index] = false;
            }
            else
            {
              float motion = this.ctrlFlag.motions[index];
              num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
              if ((double) num1 <= 0.0)
                this.allowMotions[index] = true;
            }
            this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
            this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
          }
        }
      }
      string _playAnimation = _loop != 0 ? (_state != 0 ? "D_WLoop" : "WLoop") : (_state != 0 ? "D_SLoop" : "SLoop");
      if (this.auto.ChangeLoopMotion(_loop == 1))
      {
        this.setPlay(_playAnimation, true);
        this.feelHit.InitTime();
      }
      else
      {
        this.auto.ChangeSpeed(_loop == 1, new Vector2(-1f, -1f));
        if (this.auto.AddSpeed(_wheel, _loop))
        {
          this.setPlay(_playAnimation, true);
          this.feelHit.InitTime();
        }
      }
      if (_loop == 0)
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(false);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      else
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(true);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
      this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, _loop, this.ctrlFlag.speed);
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 1.0 : 2.0) * (!this.ctrlFlag.stopFeelMale ? 1.0 : 0.0));
      if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(18))
        num *= this.ctrlFlag.SkilChangeSpeed(18);
      this.ctrlFlag.feel_m += num;
      this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
      if (this.ctrlFlag.selectAnimationListInfo == null)
        this.ctrlFlag.isAutoActionChange = this.auto.IsChangeActionAtLoop();
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        this.ctrlFlag.voice.dialog = false;
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_m >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        this.oldHit = false;
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool AutoOLoopHoushiProc(
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.auto.ChangeSpeed(false, new Vector2(-1f, -1f));
    this.auto.AddSpeed(_wheel, 2);
    this.ctrlFlag.speed = this.auto.GetSpeed(false);
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, 2, this.ctrlFlag.speed);
    this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
    float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 1.0 : 2.0) * (!this.ctrlFlag.stopFeelMale ? 1.0 : 0.0));
    if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(18))
      num *= this.ctrlFlag.SkilChangeSpeed(18);
    this.ctrlFlag.feel_m += num;
    this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
    ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      this.ctrlFlag.voice.dialog = false;
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide || this.ctrlFlag.initiative == 2 && (double) this.ctrlFlag.feel_m >= 1.0)
    {
      this.setPlay(_state != 0 ? "D_Orgasm_OUT" : "Orgasm_OUT", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.oldFinish = 2;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishDrink && _modeCtrl != 1)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 0;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishVomit && _modeCtrl != 1)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 1;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    return true;
  }

  private bool AutoStartSonyuProc(bool _restart, int _state, int _modeCtrl)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart)
        this.ctrlFlag.voice.playStart = 2;
      else
        this.ctrlFlag.voice.playStart = 3;
      return false;
    }
    if (this.nextPlay == 2)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
          return false;
      }
    }
    this.nextPlay = 0;
    if (!_restart || _restart && !this.auto.IsChangeActionAtRestart())
    {
      if (_modeCtrl == 3)
      {
        this.setPlay(_state != 0 ? "D_Insert" : "Insert", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
      }
      else
      {
        this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
        if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
        {
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
        }
        else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
        {
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
          if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
            this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
        }
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = 0;
      }
    }
    else
      this.ctrlFlag.isAutoActionChange = true;
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.timeMotions[0] = 0.0f;
    this.timeMotions[1] = 0.0f;
    this.oldHit = false;
    if (_state == 0)
    {
      for (int index = 0; index < 2; ++index)
      {
        this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
        this.timeChangeMotionDeltaTimes[index] = 0.0f;
      }
    }
    this.ctrlFlag.isNotCtrl = false;
    this.auto.Reset();
    this.feelHit.InitTime();
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    return true;
  }

  private bool AutoLoopSonyuProc(
    int _loop,
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    if ((this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishInSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 || this.ctrlFlag.initiative == 2 && (double) this.ctrlFlag.feel_m >= 1.0) && _modeCtrl == 3)
    {
      this.setPlay(new string[2]
      {
        "OrgasmM_IN",
        "D_OrgasmM_IN"
      }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 1;
      this.ctrlFlag.nowOrgasm = true;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(new string[2]
      {
        "OrgasmM_OUT",
        "D_OrgasmM_OUT"
      }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.nowOrgasm = true;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      this.setPlay(new string[2]{ "OLoop", "D_OLoop" }[_state], true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.ctrlFlag.feel_f = 0.7f;
      if ((double) this.ctrlFlag.feel_m <= 0.699999988079071)
        this.ctrlFlag.feel_m = 0.7f;
      this.oldHit = false;
      this.feelHit.InitTime();
    }
    else
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        {
          this.timeChangeMotionDeltaTimes[index] += Time.get_deltaTime();
          if ((double) this.timeChangeMotions[index] <= (double) this.timeChangeMotionDeltaTimes[index] && !this.enableMotions[index] && _state == 0)
          {
            this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
            this.timeChangeMotionDeltaTimes[index] = 0.0f;
            this.enableMotions[index] = true;
            this.timeMotions[index] = 0.0f;
            float num1;
            if (this.allowMotions[index])
            {
              float num2 = 1f - this.ctrlFlag.motions[index];
              num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
              if ((double) num1 >= 1.0)
                this.allowMotions[index] = false;
            }
            else
            {
              float motion = this.ctrlFlag.motions[index];
              num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
              if ((double) num1 <= 0.0)
                this.allowMotions[index] = true;
            }
            this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num1);
            this.lerpTimes[index] = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
          }
        }
      }
      this.feelHit.ChangeHit(_infoAnimList.nFeelHit, _loop);
      Vector2 hitArea = this.feelHit.GetHitArea(_infoAnimList.nFeelHit, _loop);
      if (this.auto.ChangeLoopMotion(_loop == 1))
      {
        this.setPlay(_loop != 0 ? (_state != 0 ? "D_WLoop" : "WLoop") : (_state != 0 ? "D_SLoop" : "SLoop"), true);
        for (int index = 0; index < 2; ++index)
        {
          if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
            Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
        }
        this.feelHit.InitTime();
      }
      else
      {
        this.auto.ChangeSpeed(_loop == 1, hitArea);
        if (this.auto.AddSpeed(_wheel, _loop))
        {
          this.setPlay(_loop != 0 ? (_state != 0 ? "D_WLoop" : "WLoop") : (_state != 0 ? "D_SLoop" : "SLoop"), true);
          for (int index = 0; index < 2; ++index)
          {
            if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
              Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[index]);
          }
          this.feelHit.InitTime();
        }
      }
      if (_loop == 0)
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(false);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      else
      {
        this.ctrlFlag.speed = this.auto.GetSpeed(true);
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      }
      this.ctrlFlag.isGaugeHit = GlobalMethod.RangeOn<float>(this.ctrlFlag.speed, (float) hitArea.x, (float) hitArea.y);
      if (_state == 1)
      {
        if (this.ctrlFlag.isGaugeHit)
        {
          float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate;
          this.ctrlFlag.feel_f *= !this.ctrlFlag.stopFeelFemal ? 1f : 0.0f;
          if (!ProcBase.hSceneManager.bMerchant)
          {
            if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
              num *= this.ctrlFlag.SkilChangeSpeed(15);
            if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
              num *= this.ctrlFlag.SkilChangeSpeed(3);
          }
          this.ctrlFlag.feel_f += num;
          this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
        }
      }
      else
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 0.300000011920929 : 1.0) * (!this.ctrlFlag.stopFeelFemal ? 1.0 : 0.0));
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
      if (_state != 1 || _modeCtrl != 4)
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelMale ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(1))
          num *= this.ctrlFlag.SkilChangeSpeed(1);
        this.ctrlFlag.feel_m += num;
        this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
      }
      if (this.ctrlFlag.selectAnimationListInfo == null)
        this.ctrlFlag.isAutoActionChange = this.auto.IsChangeActionAtLoop();
      if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
      {
        if (this.randVoicePlays[0].Get() == 0)
          this.ctrlFlag.voice.playVoices[0] = true;
        else if (this.randVoicePlays[1].Get() == 0)
          this.ctrlFlag.voice.playVoices[1] = true;
        if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0) && _modeCtrl != 4)
        {
          this.ctrlFlag.voice.playShorts[0] = 0;
          this.ctrlFlag.voice.playShorts[1] = 0;
        }
      }
      this.oldHit = this.ctrlFlag.isGaugeHit;
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
      {
        this.setPlay(_state != 0 ? "D_OLoop" : "OLoop", true);
        this.ctrlFlag.speed = 0.0f;
        this.ctrlFlag.loopType = -1;
        this.ctrlFlag.nowSpeedStateFast = false;
        this.oldHit = false;
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool AutoOLoopProc(
    int _state,
    float _wheel,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
    {
      if ((double) this.ctrlFlag.feel_m <= 0.699999988079071)
      {
        this.ctrlFlag.feel_m = 0.7f;
        this.ctrlFlag.isGaugeHit = false;
      }
      return true;
    }
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    Vector2 hitArea = this.feelHit.GetHitArea(_infoAnimList.nFeelHit, 2);
    this.auto.ChangeSpeed(false, hitArea);
    this.auto.AddSpeed(_wheel, 2);
    this.ctrlFlag.speed = this.auto.GetSpeed(false);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.ctrlFlag.isGaugeHit = GlobalMethod.RangeOn<float>(this.ctrlFlag.speed, (float) hitArea.x, (float) hitArea.y);
    if (_state == 1)
    {
      if (this.ctrlFlag.isGaugeHit)
      {
        float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
            num *= this.ctrlFlag.SkilChangeSpeed(15);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
    }
    else
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 0.300000011920929 : 1.0) * (!this.ctrlFlag.stopFeelFemal ? 1.0 : 0.0));
      if (!ProcBase.hSceneManager.bMerchant)
      {
        if (ProcBase.hSceneManager.HSkil.ContainsValue(15) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(3))
          num *= this.ctrlFlag.SkilChangeSpeed(15);
        if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num *= this.ctrlFlag.SkilChangeSpeed(3);
      }
      this.ctrlFlag.feel_f += num;
      this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
    }
    if (_state != 1 || _modeCtrl != 4)
    {
      float num = Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (!this.ctrlFlag.stopFeelMale ? 1f : 0.0f);
      if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(1))
        num *= this.ctrlFlag.SkilChangeSpeed(1);
      this.ctrlFlag.feel_m += num;
      this.ctrlFlag.feel_m = Mathf.Clamp01(this.ctrlFlag.feel_m);
    }
    if (this.ctrlFlag.isGaugeHit != this.oldHit && this.ctrlFlag.isGaugeHit && !this.ctrlFlag.isAutoActionChange)
    {
      if (this.randVoicePlays[0].Get() == 0)
        this.ctrlFlag.voice.playVoices[0] = true;
      else if (this.randVoicePlays[1].Get() == 0)
        this.ctrlFlag.voice.playVoices[1] = true;
      if (!this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(0) && _modeCtrl != 4)
      {
        this.ctrlFlag.voice.playShorts[0] = 0;
        this.ctrlFlag.voice.playShorts[1] = 0;
      }
    }
    this.oldHit = this.ctrlFlag.isGaugeHit;
    if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
    {
      this.setPlay(_state != 0 ? "D_OrgasmF_IN" : "OrgasmF_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 0;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.rateNip = 1f;
      if (Manager.Config.HData.Gloss)
        this.ctrlFlag.rateTuya = 1f;
      bool sio = Manager.Config.HData.Sio;
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
    }
    else if ((this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishInSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 || this.ctrlFlag.initiative == 2 && (double) this.ctrlFlag.feel_m >= 1.0) && _modeCtrl == 3)
    {
      this.setPlay(_state != 0 ? "D_OrgasmM_IN" : "OrgasmM_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 1;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishOutSide && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(_state != 0 ? "D_OrgasmM_OUT" : "OrgasmM_OUT", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      if ((double) this.ctrlFlag.feel_f > 0.5)
        this.ctrlFlag.feel_f = 0.5f;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishSame && (double) this.ctrlFlag.feel_m >= 0.699999988079071 && (_modeCtrl != 4 || _modeCtrl == 4 && _state == 0))
    {
      this.setPlay(_state != 0 ? "D_OrgasmS_IN" : "OrgasmS_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.isInsert = true;
      this.ctrlFlag.isGaugeHit = false;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      this.ctrlFlag.numSameOrgasm = Mathf.Clamp(this.ctrlFlag.numSameOrgasm + 1, 0, 999999);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      if (_modeCtrl == 3)
        this.ctrlFlag.numInside = Mathf.Clamp(this.ctrlFlag.numInside + 1, 0, 999999);
      else
        this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.voice.oldFinish = 3;
      bool sio = Manager.Config.HData.Sio;
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.nAnimListInfoID != 4))
            this.particle.Play(4);
        }
        if (urine)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[0], (Object) null))
            ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          if (Object.op_Inequality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
            ProcBase.hSceneManager.Agent[1].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
    }
    return true;
  }

  private bool AutoAfterTheInsideWaitingProc(int _state, float _wheel)
  {
    if (this.auto.IsPull(this.ctrlFlag.isInsert))
    {
      this.setPlay(_state != 0 ? "D_Pull" : "Pull", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.ctrlFlag.motions[0] = 0.0f;
      this.ctrlFlag.motions[1] = 0.0f;
      this.timeMotions[0] = 0.0f;
      this.timeMotions[1] = 0.0f;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.nowOrgasm = true;
      this.voice.AfterFinish();
      this.oldHit = false;
      this.feelHit.InitTime();
      this.auto.ReStartInit();
      return true;
    }
    if (!this.auto.IsReStart())
      return false;
    this.setPlay(_state != 0 ? "D_WLoop" : "WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
    {
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1 || this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 3)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
      if (this.ctrlFlag.nowAnimationInfo.nShortBreahtPlay == 1)
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[1], 1, false);
    }
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.ctrlFlag.motions[0] = 0.0f;
    this.ctrlFlag.motions[1] = 0.0f;
    this.timeMotions[0] = 0.0f;
    this.timeMotions[1] = 0.0f;
    this.oldHit = false;
    if (_state == 0)
    {
      for (int index = 0; index < 2; ++index)
      {
        this.timeChangeMotions[index] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
        this.timeChangeMotionDeltaTimes[index] = 0.0f;
      }
    }
    this.voice.AfterFinish();
    this.auto.ReStartInit();
    this.auto.PullInit();
    this.ctrlMeta.Clear();
    return true;
  }
}
