// Decompiled with JetBrains decompiler
// Type: Houshi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Houshi : ProcBase
{
  private bool allowMotion = true;
  private Vector2 lerpMotion = Vector2.get_zero();
  private float timeMotion;
  private bool enableMotion;
  private float lerpTime;
  private int finishMotion;
  private List<HScene.AnimationListInfo> lstAnimation;
  private int nextPlay;
  private bool oldHit;
  private ProcBase.animParm animPar;

  public Houshi(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[1];
    this.animPar.m = new float[1];
    this.CatID = 1;
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
      this.setPlay(!this.ctrlFlag.isFaintness ? "Idle" : "D_Idle", false);
      this.ctrlFlag.loopType = -1;
      this.voice.HouchiTime = 0.0f;
    }
    else
    {
      if ((double) this.ctrlFlag.feel_m >= 0.699999988079071)
      {
        this.setPlay(!this.ctrlFlag.isFaintness ? "OLoop" : "D_OLoop", false);
        this.ctrlFlag.loopType = -1;
      }
      else
      {
        this.setPlay(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", false);
        if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
          this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 1, this.chaFemales[0], 0, false);
        else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
          this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 2, this.chaFemales[0], 0, false);
        this.ctrlFlag.loopType = 0;
      }
      this.ctrlFlag.speed = 0.0f;
      if (_infoAnimList.lstSystem.Contains(4))
        this.ctrlFlag.isPainActionParam = true;
      this.ctrlFlag.motions[0] = 0.0f;
      this.ctrlFlag.nowSpeedStateFast = false;
      this.auto.SetSpeed(0.0f);
    }
    this.ctrlMeta.SetParameterFromState(!this.ctrlFlag.isFaintness ? 0 : 1);
    this.nextPlay = 0;
    this.oldHit = false;
    return true;
  }

  public override bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    if (Object.op_Equality((Object) this.chaMales[0].objTop, (Object) null) || Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
      return false;
    this.FemaleAi = this.chaFemales[0].getAnimatorStateInfo(0);
    if (this.ctrlFlag.initiative == 0)
      this.Manual(this.FemaleAi, _modeCtrl, _infoAnimList);
    else
      this.Auto(this.FemaleAi, _modeCtrl, _infoAnimList);
    if (this.enableMotion)
    {
      this.timeMotion = Mathf.Clamp(this.timeMotion + Time.get_deltaTime(), 0.0f, this.lerpTime);
      float num = this.ctrlFlag.changeMotionCurve.Evaluate(Mathf.Clamp01(this.timeMotion / this.lerpTime));
      this.ctrlFlag.motions[0] = Mathf.Lerp((float) this.lerpMotion.x, (float) this.lerpMotion.y, num);
      if ((double) num >= 1.0)
        this.enableMotion = false;
    }
    this.ctrlMeta.Proc(this.FemaleAi, false);
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.RecoverFaintness)
    {
      if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Idle") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OLoop")) || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_OUT_A"))
      {
        this.ctrlFlag.selectAnimationListInfo = this.RecoverFaintnessAi();
        if (this.ctrlFlag.nowAnimationInfo == this.ctrlFlag.selectAnimationListInfo)
          this.setPlay("Orgasm_OUT_A", true);
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
      }
      else
      {
        this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
        this.Hitem.SetUse(6, false);
      }
    }
    this.SetFinishCategoryEnable(((AnimatorStateInfo) ref this.FemaleAi).IsName("OLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_OLoop"));
    this.sprite.categoryFinish.SetActive((((AnimatorStateInfo) ref this.FemaleAi).IsName("WLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop")) & Manager.Config.HData.FinishButton, 3);
    this.setAnimationParamater();
    return true;
  }

  private bool Manual(AnimatorStateInfo _ai, int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    float num = Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(0, false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.LoopProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.LoopProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.OLoopProc(0, _wheel, _modeCtrl, _infoAnimList);
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
      this.StartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(1, false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "D_Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(1, true);
    }
    return true;
  }

  private bool Auto(AnimatorStateInfo _ai, int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    float num = Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.AutoStartProcTrigger(false);
      this.AutoStartProc(0, false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.AutoLoopProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.AutoLoopProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.AutoOLoopProc(0, _wheel, _modeCtrl, _infoAnimList);
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
      this.AutoStartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Drink_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("Vomit_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartProc(0, true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(1, false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopProc(1, _wheel, _modeCtrl, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT"))
      this.SetNextFinishAnimation(((AnimatorStateInfo) ref _ai).get_normalizedTime(), "D_Orgasm_OUT_A", true, true);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_OUT_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(1, true);
    }
    return true;
  }

  public override void setAnimationParamater()
  {
    this.animPar.breast = this.chaFemales[0].GetShapeBodyValue(1);
    this.animPar.speed = this.ctrlFlag.loopType == 1 ? this.ctrlFlag.speed : this.ctrlFlag.speed + 1f;
    this.animPar.m[0] = this.ctrlFlag.motions[0];
    if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null))
    {
      this.animPar.heights[0] = this.chaFemales[0].GetShapeBodyValue(0);
      this.chaFemales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
      this.chaFemales[0].setAnimatorParamFloat("speed", this.animPar.speed);
      this.chaFemales[0].setAnimatorParamFloat("motion", this.animPar.m[0]);
      this.chaFemales[0].setAnimatorParamFloat("breast", this.animPar.breast);
    }
    if (Object.op_Inequality((Object) this.chaMales[0].objBodyBone, (Object) null))
    {
      this.chaMales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
      this.chaMales[0].setAnimatorParamFloat("speed", this.animPar.speed);
      this.chaMales[0].setAnimatorParamFloat("motion", this.animPar.m[0]);
      this.chaMales[0].setAnimatorParamFloat("breast", this.animPar.breast);
    }
    if (!Object.op_Inequality((Object) this.item.GetItem(), (Object) null))
      return;
    this.item.setAnimatorParamFloat("height", this.animPar.heights[0]);
    this.item.setAnimatorParamFloat("speed", this.animPar.speed);
    this.item.setAnimatorParamFloat("motion", this.animPar.m[0]);
  }

  private void setPlay(string _playAnimation, bool _isFade = true)
  {
    this.chaFemales[0].setPlay(_playAnimation, 0);
    if (Object.op_Inequality((Object) this.chaMales[0].objTop, (Object) null))
      this.chaMales[0].setPlay(_playAnimation, 0);
    if (this.item != null)
      this.item.setPlay(_playAnimation);
    for (int index = 0; index < this.lstMotionIK.Count; ++index)
      this.lstMotionIK[index].Item3.Calc(_playAnimation);
    if (!_isFade)
      return;
    this.fade.FadeStart(1f);
  }

  private bool LoopProc(
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
      this.ctrlFlag.speed += _wheel;
      if (_loop == 0)
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
      else
        this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 1.5;
      if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null))
      {
        this.timeChangeMotionDeltaTimes[0] += Time.get_deltaTime();
        if ((double) this.timeChangeMotions[0] <= (double) this.timeChangeMotionDeltaTimes[0] && !this.enableMotion)
        {
          this.timeChangeMotions[0] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
          this.timeChangeMotionDeltaTimes[0] = 0.0f;
          this.enableMotion = true;
          this.timeMotion = 0.0f;
          float num1;
          if (this.allowMotion)
          {
            float num2 = 1f - this.ctrlFlag.motions[0];
            num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[0] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
            if ((double) num1 >= 1.0)
              this.allowMotion = false;
          }
          else
          {
            float motion = this.ctrlFlag.motions[0];
            num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[0] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
            if ((double) num1 <= 0.0)
              this.allowMotion = true;
          }
          this.lerpMotion = new Vector2(this.ctrlFlag.motions[0], num1);
          this.lerpTime = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
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
        this.oldHit = false;
        this.feelHit.InitTime();
      }
    }
    return true;
  }

  private bool OLoopProc(
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
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.SetFinishCategoryEnable(false);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.oldFinish = 2;
      this.ctrlFlag.voice.dialog = false;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishDrink && _modeCtrl != 0)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 0;
      this.SetFinishCategoryEnable(false);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishVomit && _modeCtrl != 0)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.finishMotion = 1;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.SetFinishCategoryEnable(false);
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

  private bool StartProcTrigger(float _wheel)
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

  private bool StartProc(int _state, bool _restart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart && _state != 1)
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
      this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.isNotCtrl = false;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.oldHit = false;
    this.ctrlFlag.motions[0] = 0.0f;
    this.timeMotion = 0.0f;
    this.timeChangeMotions[0] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
    this.timeChangeMotionDeltaTimes[0] = 0.0f;
    this.feelHit.InitTime();
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    this.ctrlFlag.voice.playShorts[0] = 1;
    return true;
  }

  private void SetFinishCategoryEnable(bool _enable)
  {
    if (this.sprite.IsFinishVisible(0))
      this.sprite.categoryFinish.SetActive(_enable, 0);
    if (this.sprite.IsFinishVisible(4))
      this.sprite.categoryFinish.SetActive(_enable, 4);
    if (!this.sprite.IsFinishVisible(5))
      return;
    this.sprite.categoryFinish.SetActive(_enable, 5);
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

  private bool AutoStartProc(int _state, bool _restart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      this.nextPlay = 2;
      if (!_restart && _state != 1)
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
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 1, this.chaFemales[0], 0, false);
      else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
        this.voice.PlaySoundETC(_state != 0 ? "D_WLoop" : "WLoop", 2, this.chaFemales[0], 0, false);
    }
    else
      this.ctrlFlag.isAutoActionChange = true;
    this.nextPlay = 0;
    this.ctrlFlag.speed = 0.0f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.isNotCtrl = false;
    this.ctrlFlag.nowSpeedStateFast = false;
    this.oldHit = false;
    this.ctrlFlag.motions[0] = 0.0f;
    this.timeMotion = 0.0f;
    this.timeChangeMotions[0] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
    this.timeChangeMotionDeltaTimes[0] = 0.0f;
    this.feelHit.InitTime();
    if (_restart)
    {
      this.ctrlMeta.Clear();
      this.voice.AfterFinish();
    }
    this.ctrlFlag.voice.playShorts[0] = 1;
    this.auto.Reset();
    return true;
  }

  private bool AutoLoopProc(
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
      if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null))
      {
        this.timeChangeMotionDeltaTimes[0] += Time.get_deltaTime();
        if ((double) this.timeChangeMotions[0] <= (double) this.timeChangeMotionDeltaTimes[0] && !this.enableMotion)
        {
          this.timeChangeMotions[0] = Random.Range(this.ctrlFlag.changeAutoMotionTimeMin, this.ctrlFlag.changeAutoMotionTimeMax);
          this.timeChangeMotionDeltaTimes[0] = 0.0f;
          this.enableMotion = true;
          this.timeMotion = 0.0f;
          float num1;
          if (this.allowMotion)
          {
            float num2 = 1f - this.ctrlFlag.motions[0];
            num1 = (double) num2 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[0] + Random.Range(this.ctrlFlag.changeMotionMinRate, num2) : 1f;
            if ((double) num1 >= 1.0)
              this.allowMotion = false;
          }
          else
          {
            float motion = this.ctrlFlag.motions[0];
            num1 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[0] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
            if ((double) num1 <= 0.0)
              this.allowMotion = true;
          }
          this.lerpMotion = new Vector2(this.ctrlFlag.motions[0], num1);
          this.lerpTime = Random.Range(this.ctrlFlag.changeMotionTimeMin, this.ctrlFlag.changeMotionTimeMax);
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

  private bool AutoOLoopProc(
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
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.SetFinishCategoryEnable(false);
      this.ctrlFlag.numOutSide = Mathf.Clamp(this.ctrlFlag.numOutSide + 1, 0, 999999);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      if (!this.ctrlFlag.isPainAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(4))
        this.ctrlFlag.isPainAction = true;
      if (!this.ctrlFlag.isConstraintAction && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(5))
        this.ctrlFlag.isConstraintAction = true;
      this.ctrlFlag.voice.oldFinish = 2;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishDrink && _modeCtrl != 0)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 0;
      this.SetFinishCategoryEnable(false);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    else if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishVomit && _modeCtrl != 0)
    {
      this.setPlay("Orgasm_IN", true);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 25)
        this.ctrlFlag.AddParam(20, 1);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 1 && this.ctrlFlag.nowAnimationInfo.id == 30)
        this.ctrlFlag.AddParam(40, 1);
      this.ctrlFlag.feel_m = 0.0f;
      this.ctrlFlag.isGaugeHit = false;
      this.ctrlFlag.isGaugeHit_M = this.ctrlFlag.isGaugeHit;
      ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit_M);
      this.ctrlFlag.voice.dialog = false;
      this.finishMotion = 1;
      this.SetFinishCategoryEnable(false);
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.isHoushiFinish = true;
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.oldFinish = 1;
    }
    return true;
  }

  private HScene.AnimationListInfo RecoverFaintnessAi()
  {
    for (int index = 0; index < this.lstAnimation.Count; ++index)
    {
      if (this.lstAnimation[index].nPositons.Contains(ProcBase.hSceneManager.height))
        return this.lstAnimation[index];
    }
    return this.lstAnimation[0];
  }
}
