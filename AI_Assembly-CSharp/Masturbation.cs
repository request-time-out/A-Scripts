// Decompiled with JetBrains decompiler
// Type: Masturbation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System.Collections.Generic;
using UnityEngine;

public class Masturbation : ProcBase
{
  private RandomTimer[] aTimer = new RandomTimer[2];
  private List<string> row = new List<string>();
  private string nextAnimation;
  private float timeChangeSpeed;
  private ProcBase.animParm animPar;
  private float[][] Timers;

  public Masturbation(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[1];
    this.aTimer[0] = new RandomTimer();
    this.aTimer[1] = new RandomTimer();
    this.Timers = new float[2][]
    {
      new float[2],
      new float[2]
    };
    string assetHautoListFolder = Singleton<HSceneManager>.Instance.strAssetHAutoListFolder;
    string str1 = "masturbation";
    if (!GlobalMethod.AssetFileExist(assetHautoListFolder, str1, string.Empty))
      return;
    ExcelData excelData = CommonLib.LoadAsset<ExcelData>(assetHautoListFolder, str1, false, string.Empty);
    Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(assetHautoListFolder);
    if (Object.op_Equality((Object) excelData, (Object) null))
      return;
    int num1 = 2;
    while (num1 < excelData.MaxCell)
    {
      this.row.Clear();
      foreach (string str2 in excelData.list[num1++].list)
        this.row.Add(str2);
      int num2 = 0;
      float[][] timers1 = this.Timers;
      float[] numArray1 = new float[2];
      List<string> row1 = this.row;
      int index1 = num2;
      int num3 = index1 + 1;
      numArray1[0] = float.Parse(row1.GetElement<string>(index1));
      List<string> row2 = this.row;
      int index2 = num3;
      int num4 = index2 + 1;
      numArray1[1] = float.Parse(row2.GetElement<string>(index2));
      timers1[0] = numArray1;
      float[][] timers2 = this.Timers;
      float[] numArray2 = new float[2];
      List<string> row3 = this.row;
      int index3 = num4;
      int num5 = index3 + 1;
      numArray2[0] = float.Parse(row3.GetElement<string>(index3));
      List<string> row4 = this.row;
      int index4 = num5;
      int num6 = index4 + 1;
      numArray2[1] = float.Parse(row4.GetElement<string>(index4));
      timers2[1] = numArray2;
      this.aTimer[0].Init(this.Timers[0][0], this.Timers[0][0]);
      this.aTimer[1].Init(this.Timers[1][1], this.Timers[1][1]);
    }
    this.CatID = 4;
  }

  public override bool SetStartMotion(
    bool _isIdle,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.AtariEffect.Stop();
    this.TimerReset();
    this.ctrlFlag.speed = 1f;
    this.ctrlFlag.loopType = -1;
    return true;
  }

  public override bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    if (Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
      return false;
    AnimatorStateInfo animatorStateInfo = this.chaFemales[0].getAnimatorStateInfo(0);
    bool animatorParamBool = this.chaFemales[0].getAnimatorParamBool("change");
    bool flag1 = false;
    switch (this.ctrlFlag.nowAnimationInfo.id)
    {
      case 3:
      case 13:
      case 14:
        if ((((AnimatorStateInfo) ref animatorStateInfo).IsName("WLoop") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("MLoop") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("SLoop")) && this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.FinishBefore)
        {
          this.ctrlFlag.speed = 1f;
          this.ctrlFlag.loopType = -1;
          this.nextAnimation = "OLoop";
          this.chaFemales[0].setAnimatorParamBool("change", true);
          this.item.setAnimatorParamBool("change", true);
          this.setPlay("OLoop", true);
          this.ctrlFlag.feel_f = 0.75f;
          flag1 = true;
          break;
        }
        break;
    }
    if (!flag1)
    {
      if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Idle"))
        this.StartProc(0);
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("WLoop"))
      {
        this.GotoNextLoop(0.25f, animatorParamBool, "MLoop");
        this.ctrlFlag.speed = 1f;
        this.ctrlFlag.loopType = -1;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("MLoop"))
      {
        this.GotoNextLoop(0.5f, animatorParamBool, "SLoop");
        this.ctrlFlag.speed = 1f;
        this.ctrlFlag.loopType = -1;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("SLoop"))
      {
        this.GotoNextLoop(0.75f, animatorParamBool, "OLoop");
        this.ctrlFlag.speed = 1f;
        this.ctrlFlag.loopType = -1;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("OLoop"))
      {
        float num = this.ctrlFlag.speedGuageRate * Time.get_deltaTime() * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
        if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(3))
          num *= this.ctrlFlag.SkilChangeSpeed(3);
        this.ctrlFlag.feel_f += num;
        this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
        if (this.ctrlFlag.selectAnimationListInfo == null && (double) this.ctrlFlag.feel_f >= 1.0)
        {
          this.setPlay("Orgasm", true);
          this.ctrlFlag.speed = 1f;
          this.ctrlFlag.loopType = -1;
          if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 3 && this.ctrlFlag.nowAnimationInfo.id == 13)
            this.ctrlFlag.AddParam(18, 1);
          else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 3 && this.ctrlFlag.nowAnimationInfo.id == 14)
            this.ctrlFlag.AddParam(36, 1);
          this.ctrlFlag.feel_f = 0.0f;
          this.ctrlFlag.numOrgasm = Mathf.Clamp(this.ctrlFlag.numOrgasm + 1, 0, 10);
          this.ctrlFlag.AddOrgasm();
          this.ctrlFlag.voice.oldFinish = 0;
          this.sprite.objMotionListPanel.SetActive(false);
          this.sprite.SetEnableCategoryMain(false);
          this.sprite.SetEnableHItem(false);
          this.ctrlFlag.nowOrgasm = true;
          this.ctrlFlag.rateNip = 1f;
          if (Manager.Config.HData.Gloss)
            this.ctrlFlag.rateTuya = 1f;
          bool sio = Manager.Config.HData.Sio;
          if (!ProcBase.hSceneManager.bMerchant)
          {
            bool flag2 = this.Hitem.Effect(5);
            if (sio)
              this.particle.Play(0);
            else if ((this.ctrlFlag.numFaintness == 0 && this.ctrlFlag.numOrgasm >= this.ctrlFlag.gotoFaintnessCount || flag2) && ProcBase.hSceneManager.GetFlaverSkillLevel(2) >= 100)
              this.particle.Play(0);
          }
          else if (sio)
            this.particle.Play(0);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm"))
      {
        if ((double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() >= 1.0)
        {
          this.setPlay("Orgasm_A", true);
          this.ctrlFlag.speed = 1f;
          this.ctrlFlag.loopType = -1;
          this.ctrlFlag.nowOrgasm = false;
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_A"))
        this.StartProc(1);
    }
    if (animatorParamBool && ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.nextAnimation))
      this.chaFemales[0].setAnimatorParamBool("change", false);
    this.ctrlMeta.Proc(animatorStateInfo, false);
    switch (this.ctrlFlag.nowAnimationInfo.id)
    {
      case 3:
      case 13:
      case 14:
        this.sprite.categoryFinish.SetActive((((AnimatorStateInfo) ref animatorStateInfo).IsName("WLoop") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("MLoop") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("SLoop")) & Manager.Config.HData.FinishButton, 3);
        break;
    }
    this.setAnimationParamater();
    return true;
  }

  public override void setAnimationParamater()
  {
    this.animPar.speed = this.ctrlFlag.speed;
    if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objTop, (Object) null))
    {
      this.animPar.heights[0] = this.chaFemales[0].GetShapeBodyValue(0);
      this.chaFemales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
      this.chaFemales[0].setAnimatorParamFloat("speed", this.animPar.speed);
    }
    if (!Object.op_Inequality((Object) this.item.GetItem(), (Object) null))
      return;
    this.item.setAnimatorParamFloat("height", this.animPar.heights[0]);
    this.item.setAnimatorParamFloat("speed", this.animPar.speed);
  }

  private void setPlay(string _playAnimation, bool _isFade = true)
  {
    this.chaFemales[0].setPlay(_playAnimation, 0);
    if (this.item != null)
      this.item.setPlay(_playAnimation);
    for (int index = 0; index < this.lstMotionIK.Count; ++index)
      this.lstMotionIK[index].Item3.Calc(_playAnimation);
    if (!_isFade)
      return;
    this.fade.FadeStart(1f);
  }

  private bool StartProc(int _start)
  {
    if (!this.aTimer[_start].Check())
      return false;
    this.setPlay("WLoop", true);
    this.ctrlFlag.speed = 1f;
    this.ctrlFlag.loopType = 0;
    this.ctrlFlag.isNotCtrl = false;
    this.timeChangeSpeed = 0.0f;
    this.chaFemales[0].setAnimatorParamBool("change", false);
    this.item.setAnimatorParamBool("change", false);
    if (_start == 1)
      this.voice.AfterFinish();
    return true;
  }

  private bool GotoNextLoop(float _range, bool _isChangeTrigger, string _nextAnimation)
  {
    float num = this.ctrlFlag.speedGuageRate * Time.get_deltaTime() * (!this.ctrlFlag.stopFeelFemal ? 1f : 0.0f);
    if (!ProcBase.hSceneManager.bMerchant && ProcBase.hSceneManager.HSkil.ContainsValue(3))
      num *= this.ctrlFlag.SkilChangeSpeed(3);
    this.ctrlFlag.feel_f += num;
    this.ctrlFlag.feel_f = Mathf.Clamp01(this.ctrlFlag.feel_f);
    if ((double) this.ctrlFlag.feel_f < (double) _range || _isChangeTrigger)
      return false;
    this.ctrlFlag.speed = 1f;
    this.nextAnimation = _nextAnimation;
    this.chaFemales[0].setAnimatorParamBool("change", true);
    this.item.setAnimatorParamBool("change", true);
    this.setPlay(_nextAnimation, true);
    return true;
  }

  private void TimerReset()
  {
    this.aTimer[0] = new RandomTimer();
    this.aTimer[1] = new RandomTimer();
    this.aTimer[0].Init(this.Timers[0][0], this.Timers[0][0]);
    this.aTimer[1].Init(this.Timers[1][1], this.Timers[1][1]);
  }
}
