// Decompiled with JetBrains decompiler
// Type: Peeping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Scene;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Peeping : ProcBase
{
  private float oldFrame;
  private ProcBase.animParm animPar;

  public Peeping(DeliveryMember _delivery)
    : base(_delivery)
  {
    this.animPar.heights = new float[1];
    this.CatID = 5;
  }

  public override bool SetStartMotion(
    bool _isIdle,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    this.AtariEffect.Stop();
    this.ctrlFlag.isNotCtrl = false;
    this.oldFrame = 0.0f;
    return true;
  }

  public override bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    if (Object.op_Equality((Object) this.chaFemales[0].objTop, (Object) null))
      return false;
    this.FemaleAi = this.chaFemales[0].getAnimatorStateInfo(0);
    float num = ((AnimatorStateInfo) ref this.FemaleAi).get_normalizedTime() % 1f;
    if (this.ctrlFlag.nowAnimationInfo.id == 10 && !this.sprite.isFade)
    {
      HSceneSprite.FadeKindProc fadeKindProc = this.sprite.GetFadeKindProc();
      if (fadeKindProc != HSceneSprite.FadeKindProc.OutEnd)
      {
        if ((double) num > 0.930000007152557)
          this.sprite.FadeState(HSceneSprite.FadeKind.Out, 1.5f);
        if ((double) this.oldFrame <= 0.0500000007450581 && 0.0500000007450581 < (double) num)
          GlobalMethod.SetAllClothState(this.chaFemales[0], false, 2, true);
        else if ((double) this.oldFrame <= 0.209999993443489 && 0.209999993443489 < (double) num)
          this.particle.Play(2);
        else if ((double) this.oldFrame <= 0.819999992847443 && 0.819999992847443 < (double) num)
          GlobalMethod.SetAllClothState(this.chaFemales[0], false, 0, true);
      }
      else if (fadeKindProc == HSceneSprite.FadeKindProc.OutEnd)
      {
        GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, false);
        this.chaFemales[0].animBody.set_speed(0.0f);
        ConfirmScene.Sentence = string.Empty;
        ConfirmScene.OnClickedYes = (Action) (() => this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.PeepingRestart);
        ConfirmScene.OnClickedNo = (Action) (() => this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.SceneEnd);
        if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.PeepingRestart)
        {
          this.setRePlay(this.FemaleAi, 0.0f, false);
          this.sprite.FadeState(HSceneSprite.FadeKind.In, 0.5f);
          this.chaFemales[0].animBody.set_speed(1f);
          this.voice.StartCoroutine(this.InitOldMemberCoroutine());
        }
      }
    }
    this.oldFrame = num;
    this.ctrlMeta.Proc(this.FemaleAi, false);
    this.setAnimationParamater();
    return true;
  }

  public override void setAnimationParamater()
  {
    if (this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objTop, (Object) null))
    {
      this.animPar.heights[0] = this.chaFemales[0].GetShapeBodyValue(0);
      this.chaFemales[0].setAnimatorParamFloat("height", this.animPar.heights[0]);
    }
    if (!Object.op_Inequality((Object) this.item.GetItem(), (Object) null))
      return;
    this.item.setAnimatorParamFloat("height", this.animPar.heights[0]);
  }

  private void setRePlay(AnimatorStateInfo _ai, float _normalizetime, bool _isFade = true)
  {
    this.chaFemales[0].syncPlay(((AnimatorStateInfo) ref _ai).get_shortNameHash(), 0, _normalizetime);
    if (this.item != null)
      this.item.syncPlay(((AnimatorStateInfo) ref _ai).get_shortNameHash(), _normalizetime);
    if (!_isFade)
      return;
    this.fade.FadeStart(1f);
  }

  [DebuggerHidden]
  private IEnumerator InitOldMemberCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Peeping.\u003CInitOldMemberCoroutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
