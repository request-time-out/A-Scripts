// Decompiled with JetBrains decompiler
// Type: AIProject.LookAtPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class LookAtPlayer : AgentAction
  {
    private float _distanceTweenPlayer = float.MaxValue;
    private float _heightDistTweenPlayer = float.MaxValue;
    private bool _prevCloseToPlayer;
    private AgentActor _agent;
    private ChaControl _chara;
    private PlayerActor _player;
    private bool _missing;

    private bool IsCloseToPlayer
    {
      get
      {
        AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
        return (double) this._distanceTweenPlayer <= (double) rangeSetting.arrivedDistance && (double) this._heightDistTweenPlayer <= (double) rangeSetting.acceptableHeight;
      }
    }

    private bool IsFarPlayer
    {
      get
      {
        AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
        return (double) rangeSetting.leaveDistance < (double) this._distanceTweenPlayer || (double) rangeSetting.acceptableHeight < (double) this._heightDistTweenPlayer;
      }
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._chara = this._agent != null ? this._agent.ChaControl : (ChaControl) null;
      this._player = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
      this._missing = Object.op_Equality((Object) this._agent, (Object) null) || Object.op_Equality((Object) this._chara, (Object) null) || Object.op_Equality((Object) this._player, (Object) null);
      if (this._missing)
        return;
      this._prevCloseToPlayer = false;
      this._distanceTweenPlayer = float.MaxValue;
      this._heightDistTweenPlayer = float.MaxValue;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._missing)
        return (TaskStatus) 1;
      Vector3 position1 = this._agent.Position;
      Vector3 position2 = this._player.Position;
      Vector3 forward = this._agent.Forward;
      this._heightDistTweenPlayer = Mathf.Abs((float) (position1.y - position2.y));
      position1.y = (__Null) (double) (position2.y = forward.y = (__Null) 0.0f);
      this._distanceTweenPlayer = Vector3.Distance(position1, position2);
      if (!this._prevCloseToPlayer && this.IsCloseToPlayer)
      {
        this._prevCloseToPlayer = true;
        this.LookAtPlayerHead(true);
      }
      else if (this._prevCloseToPlayer && this.IsFarPlayer)
      {
        this._prevCloseToPlayer = false;
        this.LookAtPlayerHead(false);
      }
      return (TaskStatus) 3;
    }

    private void LookAtPlayerHead(bool flag)
    {
      if (Object.op_Equality((Object) this._chara, (Object) null))
        return;
      if (flag)
      {
        Transform trfTarg = this._player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head);
        this._chara.ChangeLookEyesTarget(1, trfTarg, 0.5f, 0.0f, 1f, 2f);
        this._chara.ChangeLookEyesPtn(1);
        this._chara.ChangeLookNeckTarget(1, trfTarg, 0.5f, 0.0f, 1f, 0.8f);
        this._chara.ChangeLookNeckPtn(1, 1f);
      }
      else
      {
        Transform transform = ((Component) this._player.CameraControl.CameraComponent).get_transform();
        this._chara.ChangeLookEyesTarget(0, transform, 0.5f, 0.0f, 1f, 2f);
        this._chara.ChangeLookEyesPtn(0);
        this._chara.ChangeLookNeckTarget(3, transform, 0.5f, 0.0f, 1f, 0.8f);
        this._chara.ChangeLookNeckPtn(3, 1f);
      }
    }

    public virtual void OnEnd()
    {
      this.LookAtPlayerHead(false);
      ((Task) this).OnEnd();
    }
  }
}
