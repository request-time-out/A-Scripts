// Decompiled with JetBrains decompiler
// Type: Les
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Manager;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Les : ProcBase
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
  private int nextPlay;
  private bool oldHit;
  private ProcBase.animParm animPar;

  public Les(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[2];
    this.animPar.m = new float[2];
    this.CatID = 6;
  }

  public override bool Init(int _modeCtrl)
  {
    base.Init(_modeCtrl);
    return true;
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
      if (this.ctrlFlag.initiative != 0)
        ;
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
            this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 1, this.chaFemales[0], 0, false);
          else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
            this.voice.PlaySoundETC(!this.ctrlFlag.isFaintness ? "WLoop" : "D_WLoop", 2, this.chaFemales[0], 0, false);
        }
        else
        {
          this.setPlay("WLoop", false);
          if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
            this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
          else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
            this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
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
      this.Manual(this.FemaleAi, _infoAnimList);
    else
      this.Auto(this.FemaleAi, _infoAnimList);
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
    this.sprite.categoryFinish.SetActive((((AnimatorStateInfo) ref this.FemaleAi).IsName("WLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("SLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_WLoop") || ((AnimatorStateInfo) ref this.FemaleAi).IsName("D_SLoop")) & Manager.Config.HData.FinishButton, 3);
    this.ctrlMeta.Proc(this.FemaleAi, false);
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
          this.voice.playAnimation.SetAllFlags(true);
      }
      else
      {
        this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
        this.Hitem.SetUse(6, false);
      }
    }
    this.setAnimationParamater();
    return true;
  }

  private bool Manual(AnimatorStateInfo _ai, HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(false);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.LoopProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.LoopProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.OLoopProc(0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm"))
      this.OrgasmProc(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_A"))
    {
      this.StartProcTrigger(_wheel);
      this.StartProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.FaintnessStartProcTrigger(_wheel, true, _infoAnimList.nDownPtn != 0);
      this.FaintnessStartProc(true);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopProc(1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm"))
      this.OrgasmProc(1, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_A"))
    {
      this.FaintnessStartProcTrigger(_wheel, false, _infoAnimList.nDownPtn != 0);
      this.FaintnessStartProc(false);
    }
    return true;
  }

  private bool Auto(AnimatorStateInfo _ai, HScene.AnimationListInfo _infoAnimList)
  {
    float num = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * (!this.sprite.IsSpriteOver() ? 1f : 0.0f);
    float _wheel = (double) num >= 0.0 ? ((double) num <= 0.0 ? 0.0f : this.ctrlFlag.wheelActionCount) : -this.ctrlFlag.wheelActionCount;
    if (((AnimatorStateInfo) ref _ai).IsName("Idle"))
    {
      this.AutoStartProcTrigger(false);
      this.AutoStartProc(false);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("WLoop"))
      this.AutoLoopProc(0, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("SLoop"))
      this.AutoLoopProc(1, 0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("OLoop"))
      this.AutoOLoopProc(0, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm"))
      this.OrgasmProc(0, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("Orgasm_A"))
    {
      this.AutoStartProcTrigger(true);
      this.AutoStartProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Idle"))
    {
      this.FaintnessStartProcTrigger(_wheel, true, _infoAnimList.nDownPtn != 0);
      this.FaintnessStartProc(true);
    }
    else if (((AnimatorStateInfo) ref _ai).IsName("D_WLoop"))
      this.LoopProc(0, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_SLoop"))
      this.LoopProc(1, 1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_OLoop"))
      this.OLoopProc(1, _wheel, _infoAnimList);
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm"))
      this.OrgasmProc(1, ((AnimatorStateInfo) ref _ai).get_normalizedTime());
    else if (((AnimatorStateInfo) ref _ai).IsName("D_Orgasm_A"))
    {
      this.FaintnessStartProcTrigger(_wheel, false, _infoAnimList.nDownPtn != 0);
      this.FaintnessStartProc(false);
    }
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
      if (this.chaFemales[1].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null))
        this.animPar.heights[index] = this.chaFemales[index].isPlayer ? 0.75f : this.chaFemales[index].GetShapeBodyValue(0);
    }
    for (int index = 0; index < this.chaFemales.Length; ++index)
    {
      if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null))
      {
        this.chaFemales[index].setAnimatorParamFloat("height", this.animPar.heights[index]);
        this.chaFemales[index].setAnimatorParamFloat("speed", this.animPar.speed);
        this.chaFemales[index].setAnimatorParamFloat("motion", this.animPar.m[index]);
        this.chaFemales[index].setAnimatorParamFloat("breast", this.animPar.breast);
        if (this.isHeight1Parameter)
          this.chaFemales[index].setAnimatorParamFloat("height1", this.animPar.heights[index ^ 1]);
      }
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

  private bool StartProc(bool _isReStart)
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
      this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
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

  private bool FaintnessStartProcTrigger(float _wheel, bool _start, bool canFaintNess)
  {
    if ((double) _wheel == 0.0 || this.nextPlay != 0 || !_start && !canFaintNess)
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

  private bool FaintnessStartProc(bool _start)
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
      this.voice.PlaySoundETC("D_WLoop", 1, this.chaFemales[0], 0, false);
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      this.voice.PlaySoundETC("D_WLoop", 2, this.chaFemales[0], 0, false);
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

  private bool LoopProc(
    int _loop,
    int _state,
    float _wheel,
    HScene.AnimationListInfo _infoAnimList)
  {
    float num1 = 0.0f;
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
              float num2;
              if (this.allowMotions[index])
              {
                float num3 = 1f - this.ctrlFlag.motions[index];
                num2 = (double) num3 > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] + Random.Range(this.ctrlFlag.changeMotionMinRate, num3) : 1f;
                if ((double) num2 >= 1.0)
                  this.allowMotions[index] = false;
              }
              else
              {
                float motion = this.ctrlFlag.motions[index];
                num2 = (double) motion > (double) this.ctrlFlag.changeMotionMinRate ? this.ctrlFlag.motions[index] - Random.Range(this.ctrlFlag.changeMotionMinRate, motion) : 0.0f;
                if ((double) num2 <= 0.0)
                  this.allowMotions[index] = true;
              }
              this.lerpMotions[index] = new Vector2(this.ctrlFlag.motions[index], num2);
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
        float num2 = (num1 + Time.get_deltaTime() * this.ctrlFlag.speedGuageRate) * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant)
        {
          if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
            num2 *= this.ctrlFlag.SkilChangeSpeed(0);
          if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
            num2 *= this.ctrlFlag.SkilChangeSpeed(3);
        }
        this.ctrlFlag.feel_f += num2;
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
          {
            this.ctrlFlag.voice.playShorts[0] = 0;
            this.ctrlFlag.voice.playShorts[0] = 1;
          }
          if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
          {
            this.ctrlFlag.voice.playShorts[1] = 0;
            this.ctrlFlag.voice.playShorts[1] = 1;
          }
        }
        this.ctrlFlag.voice.dialog = false;
        if (this.ctrlFlag.voice.playVoices[1] && Object.op_Equality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
          this.ctrlFlag.voice.dialog = true;
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

  private bool OLoopProc(int _state, float _wheel, HScene.AnimationListInfo _infoAnimList)
  {
    float num1 = 0.0f;
    this.ctrlFlag.speed = Mathf.Clamp01(this.ctrlFlag.speed + _wheel);
    this.ctrlFlag.nowSpeedStateFast = (double) this.ctrlFlag.speed >= 0.5;
    this.feelHit.ChangeHit(_infoAnimList.nFeelHit, 2);
    this.ctrlFlag.isGaugeHit = this.feelHit.isHit(_infoAnimList.nFeelHit, 2, this.ctrlFlag.speed);
    ((ReactiveProperty<bool>) this.isAtariHit).set_Value(this.ctrlFlag.isGaugeHit);
    if (this.ctrlFlag.isGaugeHit)
    {
      float num2 = (num1 + Time.get_deltaTime() * this.ctrlFlag.speedGuageRate) * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
      if (!ProcBase.hSceneManager.bMerchant)
      {
        if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
          num2 *= this.ctrlFlag.SkilChangeSpeed(0);
        if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num2 *= this.ctrlFlag.SkilChangeSpeed(3);
      }
      this.ctrlFlag.feel_f += num2;
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
        {
          this.ctrlFlag.voice.playShorts[0] = 0;
          this.ctrlFlag.voice.playShorts[0] = 1;
        }
        if (_infoAnimList.nShortBreahtPlay == 1 || _infoAnimList.nShortBreahtPlay == 2)
        {
          this.ctrlFlag.voice.playShorts[1] = 0;
          this.ctrlFlag.voice.playShorts[1] = 1;
        }
      }
      this.ctrlFlag.voice.dialog = false;
      if (this.ctrlFlag.voice.playVoices[1] && Object.op_Equality((Object) ProcBase.hSceneManager.Agent[1], (Object) null))
        this.ctrlFlag.voice.dialog = true;
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
      if (ProcBase.hSceneManager.Player.ChaControl.sex == (byte) 1 && !ProcBase.hSceneManager.bFutanari)
        this.ctrlFlag.numOrgasmFemalePlayer = Mathf.Clamp(this.ctrlFlag.numOrgasmFemalePlayer + 1, 0, 10);
      this.ctrlFlag.nowOrgasm = true;
      this.ctrlFlag.voice.dialog = false;
      this.ctrlFlag.rateNip = 1f;
      if (Manager.Config.HData.Gloss)
        this.ctrlFlag.rateTuya = 1f;
      bool sio = Manager.Config.HData.Sio;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        bool flag = this.Hitem.Effect(5);
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.id == 0))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
          this.particle.Play(0);
      }
      else if (sio)
      {
        this.particle.Play(0);
        if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && (Object.op_Implicit((Object) this.chaFemales[1].objBodyBone) && this.ctrlFlag.nowAnimationInfo.id == 0))
          this.particle.Play(4);
      }
      bool urine = Manager.Config.HData.Urine;
      if (!ProcBase.hSceneManager.bMerchant)
      {
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (urine && this.chaFemales[1].visibleAll && (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone)) && this.ctrlFlag.nowAnimationInfo.id == 0)
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
          ++this.ctrlFlag.numUrine;
        }
      }
      else if (urine)
      {
        this.particle.Play(1);
        this.ctrlFlag.voice.urines[0] = true;
        if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
        {
          this.particle.Play(5);
          this.ctrlFlag.voice.urines[1] = true;
        }
      }
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
    }
    return true;
  }

  private bool GotoFaintness(int _state)
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

  private bool OrgasmProc(int _state, float _normalizedTime)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    this.GotoFaintness(_state);
    return true;
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

  private bool AutoStartProc(bool _isReStart)
  {
    if (this.nextPlay == 0)
      return false;
    if (this.nextPlay == 1)
    {
      if (!_isReStart)
      {
        this.nextPlay = 3;
      }
      else
      {
        this.nextPlay = 2;
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
    }
    this.nextPlay = 0;
    this.setPlay("WLoop", true);
    if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(0))
      this.voice.PlaySoundETC("WLoop", 1, this.chaFemales[0], 0, false);
    else if (this.ctrlFlag.nowAnimationInfo.hasVoiceCategory.Contains(1))
      this.voice.PlaySoundETC("WLoop", 2, this.chaFemales[0], 0, false);
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
          if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
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
            if (this.chaFemales[index].visibleAll && !Object.op_Equality((Object) this.chaFemales[index].objBodyBone, (Object) null) && (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice))
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
            if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
              num *= this.ctrlFlag.SkilChangeSpeed(3);
          }
          this.ctrlFlag.feel_f += num;
          this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
        }
      }
      else
      {
        this.ctrlFlag.feel_f += Time.get_deltaTime() * this.ctrlFlag.speedGuageRate * (float) ((!this.ctrlFlag.isGaugeHit ? 0.300000011920929 : 1.0) * (!this.ctrlFlag.stopFeelFemal ? 1.0 : 0.0));
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
          this.ctrlFlag.voice.playShorts[0] = 1;
          this.ctrlFlag.voice.playShorts[1] = 0;
          this.ctrlFlag.voice.playShorts[1] = 1;
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

  private bool AutoOLoopProc(int _state, float _wheel, HScene.AnimationListInfo _infoAnimList)
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
          if (ProcBase.hSceneManager.HSkil.ContainsValue(0) && this.ctrlFlag.nowAnimationInfo.lstSystem.Contains(2))
            num *= this.ctrlFlag.SkilChangeSpeed(0);
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
        if (ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num *= this.ctrlFlag.SkilChangeSpeed(3);
      }
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
        this.ctrlFlag.voice.playShorts[0] = 1;
        this.ctrlFlag.voice.playShorts[1] = 0;
        this.ctrlFlag.voice.playShorts[1] = 1;
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
      if (ProcBase.hSceneManager.Player.ChaControl.sex == (byte) 1)
        this.ctrlFlag.numOrgasmFemalePlayer = Mathf.Clamp(this.ctrlFlag.numOrgasmFemalePlayer + 1, 0, 10);
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
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
            this.particle.Play(4);
        }
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
          this.particle.Play(0);
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          this.ctrlFlag.voice.urines[0] = true;
          if (urine && this.chaFemales[1].visibleAll && (Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone)))
          {
            this.particle.Play(5);
            this.ctrlFlag.voice.urines[1] = true;
          }
          ++this.ctrlFlag.numUrine;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
        {
          this.particle.Play(0);
          if (this.chaFemales[1].visibleAll && Object.op_Implicit((Object) this.chaFemales[1]) && Object.op_Implicit((Object) this.chaFemales[1].objBodyBone))
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
        }
      }
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
    }
    return true;
  }
}
