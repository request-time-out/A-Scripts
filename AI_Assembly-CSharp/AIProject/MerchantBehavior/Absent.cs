// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.Absent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class Absent : MerchantAction
  {
    private bool prevTalkable = true;
    private MerchantPoint _currentPoint;

    private PlayerActor Player
    {
      get
      {
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.Player : (PlayerActor) null;
      }
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      MerchantPoint sightMerchantPoint = this.TargetInSightMerchantPoint;
      this.CurrentMerchantPoint = sightMerchantPoint;
      if (Object.op_Equality((Object) sightMerchantPoint, (Object) null))
      {
        MerchantPoint exitPoint = this.Merchant.ExitPoint;
        this.TargetInSightMerchantPoint = exitPoint;
        this.CurrentMerchantPoint = exitPoint;
      }
      this._currentPoint = this.CurrentMerchantPoint;
      if (this.prevTalkable = this.Merchant.Talkable)
        this.Merchant.Talkable = false;
      if (this.Merchant.ChaControl.visibleAll || Object.op_Inequality((Object) this._currentPoint, (Object) null) && this._currentPoint.AnyActiveItemObjects())
        this.CrossFade();
      this.Merchant.Hide();
      if (Object.op_Inequality((Object) this._currentPoint, (Object) null))
        this._currentPoint.HideItemObjects();
      this.Merchant.DeactivateNavMeshElement();
      this.Merchant.Animation.StopAllAnimCoroutine();
      this.Merchant.Animation.Animator.Play(Singleton<Resources>.Instance.DefinePack.AnimatorState.MerchantIdleState, 0);
      string forwardMove = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.ForwardMove;
      foreach (AnimatorControllerParameter parameter in this.Merchant.Animation.Animator.get_parameters())
      {
        if (parameter.get_name() == forwardMove && parameter.get_type() == 1)
          this.Merchant.Animation.Animator.SetFloat(forwardMove, 0.0f);
      }
      if (Object.op_Equality((Object) this._currentPoint, (Object) null))
        return;
      this.Merchant.SetPointIDInfo(this.CurrentMerchantPoint);
      Tuple<MerchantPointInfo, Transform, Transform> eventInfo = this.CurrentMerchantPoint.GetEventInfo(Merchant.EventType.Wait);
      this.Merchant.Position = eventInfo.Item2.get_position();
      this.Merchant.Rotation = eventInfo.Item2.get_rotation();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }

    private void CrossFade()
    {
      ActorCameraControl actorCameraControl = !Object.op_Inequality((Object) this.Player, (Object) null) ? (ActorCameraControl) null : this.Player.CameraControl;
      if (!Object.op_Inequality((Object) actorCameraControl, (Object) null))
        return;
      bool flag = false;
      float fadeEnableDistance = Singleton<Resources>.Instance.LocomotionProfile.CrossFadeEnableDistance;
      int num1 = flag | (double) Vector3.Distance(this.Merchant.Position, ((Component) actorCameraControl).get_transform().get_position()) <= (double) fadeEnableDistance ? 1 : 0;
      bool? nullable = this.CurrentMerchantPoint?.CheckDistance(((Component) actorCameraControl).get_transform().get_position(), fadeEnableDistance);
      int num2 = !nullable.HasValue ? 0 : (nullable.Value ? 1 : 0);
      if ((num1 | num2) == 0)
        return;
      actorCameraControl.CrossFade.FadeStart(-1f);
    }

    public virtual void OnEnd()
    {
      this.CrossFade();
      this.Merchant.Show();
      if (Object.op_Inequality((Object) this._currentPoint, (Object) null))
        this._currentPoint.ShowItemObjects();
      if (Object.op_Inequality((Object) this.CurrentMerchantPoint, (Object) null))
      {
        this.PrevMerchantPoint = this.CurrentMerchantPoint;
        this.CurrentMerchantPoint = (MerchantPoint) null;
      }
      if (this.prevTalkable)
        this.Merchant.Talkable = true;
      this.Merchant.MerchantData.PointID = -1;
      ((Task) this).OnEnd();
    }
  }
}
