// Decompiled with JetBrains decompiler
// Type: Spnking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using UnityEngine;

public class Spnking : ProcBase
{
  private bool upFeel;
  private float backupFeel;
  private float timeFeelUp;
  private ProcBase.animParm animPar;

  public Spnking(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[1];
    this.CatID = 3;
  }

  public override bool SetStartMotion(
    bool _isIdle,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.AtariEffect.Stop();
    if (_isIdle)
    {
      this.setPlay(!this.ctrlFlag.isFaintness ? "WIdle" : "D_Orgasm_A", false);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.voice.HouchiTime = 0.0f;
    }
    else
    {
      if (this.ctrlFlag.isFaintness)
        this.setPlay("D_Orgasm_A", false);
      else
        this.setPlay((double) this.ctrlFlag.feel_f < 0.5 ? "WIdle" : "SIdle", false);
      this.ctrlFlag.speed = 0.0f;
      this.ctrlFlag.loopType = -1;
      this.voice.HouchiTime = 0.0f;
      this.ctrlFlag.motions[0] = 0.0f;
      this.ctrlFlag.motions[1] = 0.0f;
      if (_infoAnimList.lstSystem.Contains(4))
        this.ctrlFlag.isPainActionParam = true;
    }
    this.isHeight1Parameter = this.chaFemales[0].IsParameterInAnimator("height1");
    return true;
  }

  public override bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    if (Object.op_Equality((Object) this.chaMales[0].objTop, (Object) null) || Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
      return false;
    this.FemaleAi = this.chaFemales[0].getAnimatorStateInfo(0);
    if (((AnimatorStateInfo) ref this.FemaleAi).IsName("WIdle"))
    {
      this.SpankingProc(0);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("WAction"))
      this.ActionProc(((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime(), 0, _infoAnimList);
    if (((AnimatorStateInfo) ref this.FemaleAi).IsName("SIdle"))
    {
      this.SpankingProc(1);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("SAction"))
      this.ActionProc(((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime(), 1, _infoAnimList);
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("Orgasm"))
      this.AfterWaitingAnimation(((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime(), 0);
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Idle"))
    {
      this.SpankingProc(2);
      this.voice.HouchiTime += Time.get_unscaledDeltaTime();
    }
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Action"))
      this.ActionProc(((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime(), 2, _infoAnimList);
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm"))
      this.AfterWaitingAnimation(((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime(), 1);
    else if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_A"))
      this.SpankingProc(2);
    if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.RecoverFaintness)
    {
      if (((AnimatorStateInfo) ref this.FemaleAi).IsName("D_Orgasm_A"))
      {
        this.setPlay("WIdle", true);
        this.voice.HouchiTime = 0.0f;
        this.ctrlFlag.isFaintness = false;
        this.sprite.SetVisibleLeaveItToYou(true, true);
        this.ctrlFlag.numOrgasm = 0;
        this.sprite.SetAnimationMenu();
        this.sprite.SetMotionListDraw(false, -1);
        this.Hitem.SetUse(6, false);
      }
      else
      {
        this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
        this.Hitem.SetUse(6, false);
      }
    }
    if (this.upFeel)
    {
      this.timeFeelUp = Mathf.Clamp(this.timeFeelUp + Time.get_deltaTime(), 0.0f, 0.3f);
      float num1 = Mathf.Clamp01(this.timeFeelUp / 0.3f);
      if (!this.ctrlFlag.stopFeelFemal)
      {
        float num2 = Mathf.Lerp(this.backupFeel, this.backupFeel + this.ctrlFlag.feelSpnking, num1);
        if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num2 *= this.ctrlFlag.SkilChangeSpeed(3);
        this.ctrlFlag.feel_f += num2;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
      }
      if ((double) num1 >= 1.0)
        this.upFeel = false;
    }
    this.ctrlMeta.Proc(this.FemaleAi, false);
    this.setAnimationParamater();
    return true;
  }

  public override void setAnimationParamater()
  {
    if (this.chaFemales[0].visibleAll && Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
    {
      this.animPar.heights[0] = this.chaFemales[0].GetShapeBodyValue(0);
      this.chaFemales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
    }
    if (Object.op_Inequality((Object) this.chaMales[0].objTop, (Object) null))
      this.chaMales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
    if (!Object.op_Inequality((Object) this.item.GetItem(), (Object) null))
      return;
    this.item.setAnimatorParamFloat("height", this.animPar.heights[0]);
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

  private bool SpankingProc(int _state)
  {
    if (!this.ctrlFlag.stopFeelFemal)
      this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f - this.ctrlFlag.guageDecreaseRate * Time.get_deltaTime());
    if (_state == 1 && (double) this.ctrlFlag.feel_f < 0.5)
    {
      this.setPlay("WIdle", true);
      this.voice.HouchiTime = 0.0f;
      return true;
    }
    if ((double) Singleton<Manager.Input>.Instance.ScrollValue() * (!this.sprite.IsSpriteOver() ? 1.0 : 0.0) == 0.0)
      return false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.voice || this.voice.nowVoices[index].state == HVoiceCtrl.VoiceKind.startVoice)
        return false;
    }
    string _playAnimation;
    switch (_state)
    {
      case 0:
        _playAnimation = "WAction";
        break;
      case 1:
        _playAnimation = "SAction";
        break;
      default:
        _playAnimation = "D_Action";
        break;
    }
    this.setPlay(_playAnimation, false);
    this.upFeel = true;
    this.timeFeelUp = 0.0f;
    this.backupFeel = this.ctrlFlag.feel_f;
    this.ctrlFlag.isNotCtrl = false;
    if (this.randVoicePlays[0].Get() == 0)
      this.ctrlFlag.voice.playVoices[0] = true;
    else if (this.randVoicePlays[1].Get() == 0)
      this.ctrlFlag.voice.playVoices[1] = true;
    this.ctrlFlag.voice.playShorts[0] = 0;
    return true;
  }

  private bool ActionProc(
    float _normalizedTime,
    int _state,
    HScene.AnimationListInfo _infoAnimList)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    if (_state == 0)
    {
      this.setPlay((double) this.ctrlFlag.feel_f < 0.5 ? "WIdle" : "SIdle", false);
      this.voice.HouchiTime = 0.0f;
    }
    else if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
    {
      this.setPlay(_state != 1 ? "D_Orgasm" : "Orgasm", true);
      this.ctrlFlag.feel_f = 0.0f;
      this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
      this.ctrlFlag.AddOrgasm();
      this.sprite.objMotionListPanel.SetActive(false);
      this.sprite.SetEnableCategoryMain(false);
      this.sprite.SetEnableHItem(false);
      this.ctrlFlag.voice.oldFinish = 0;
      this.ctrlFlag.nowOrgasm = true;
      if (_state != 0)
        this.ctrlFlag.AddParam(15, 1);
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
          this.particle.Play(0);
        else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
          this.particle.Play(0);
        if (urine || (double) ProcBase.hSceneManager.Toilet >= 70.0)
        {
          this.particle.Play(1);
          ++this.ctrlFlag.numUrine;
          this.ctrlFlag.voice.urines[0] = true;
          ProcBase.hSceneManager.Toilet = 30f;
          int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
          ProcBase.hSceneManager.Agent[0].SetDesire(desireKey, ProcBase.hSceneManager.Toilet);
        }
      }
      else
      {
        if (sio)
          this.particle.Play(0);
        if (urine)
        {
          this.particle.Play(1);
          ++this.ctrlFlag.numUrine;
          this.ctrlFlag.voice.urines[0] = true;
        }
      }
    }
    else
      this.setPlay(_state != 1 ? "D_Orgasm_A" : "SIdle", false);
    return true;
  }

  private bool AfterWaitingAnimation(float _normalizedTime, int _state)
  {
    if ((double) _normalizedTime < 1.0)
      return false;
    bool flag = this.Hitem.Effect(5);
    if (_state == 0 && (this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag))
    {
      this.setPlay("D_Orgasm_A", true);
      if (flag)
        this.Hitem.SetUse(5, false);
      if (!ProcBase.hSceneManager.bMerchant && this.ctrlFlag.numFaintness == 0)
        this.ctrlFlag.AddParam(14, 1);
      this.ctrlFlag.isFaintness = true;
      this.ctrlFlag.numFaintness = Mathf.Clamp(this.ctrlFlag.numFaintness + 1, 0, 999999);
      this.sprite.SetVisibleLeaveItToYou(false, false);
      this.sprite.SetAnimationMenu();
    }
    else
    {
      this.setPlay(_state != 0 ? "D_Orgasm_A" : "WIdle", false);
      this.voice.HouchiTime = 0.0f;
    }
    this.ctrlFlag.nowOrgasm = false;
    this.voice.AfterFinish();
    return true;
  }
}
