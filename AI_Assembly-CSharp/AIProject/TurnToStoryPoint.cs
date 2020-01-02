// Decompiled with JetBrains decompiler
// Type: AIProject.TurnToStoryPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class TurnToStoryPoint : AgentAction
  {
    private bool _missing = true;
    private Vector3 _targetPoint = Vector3.get_zero();
    private AgentActor _agent;
    private StoryPoint _point;
    private NavMeshPath _path;
    private float _changeTime;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._point = this._agent != null ? this._agent.TargetStoryPoint : (StoryPoint) null;
      this._path = new NavMeshPath();
      if (this._missing = Object.op_Equality((Object) this._agent, (Object) null) || Object.op_Equality((Object) this._point, (Object) null))
        return;
      NavMesh.CalculatePath(this._agent.Position, this._point.Position, this._agent.NavMeshAgent.get_areaMask(), this._path);
      if (this._missing = this._path.get_status() != 0)
        return;
      this._targetPoint = this._path.get_corners().GetElement<Vector3>(1);
      this._agent.Animation.PlayTurnAnimation(this._targetPoint, 1f, Singleton<Resources>.Instance.DefinePack.AnimatorState.IdleStateInfo, false);
      this._agent.TutorialLocomoCaseID = 100;
      this._changeTime = Singleton<Resources>.Instance.CommonDefine.Tutorial.WalkToRunTime;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._missing)
        return (TaskStatus) 1;
      if (this._agent.Animation.PlayingTurnAnimation)
        return (TaskStatus) 3;
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) this._changeTime)), (Component) this._agent), (Action<M0>) (_ => this._agent.TutorialLocomoCaseID = 101));
      return (TaskStatus) 2;
    }
  }
}
