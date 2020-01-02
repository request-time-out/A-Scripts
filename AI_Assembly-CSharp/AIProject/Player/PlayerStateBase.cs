// Decompiled with JetBrains decompiler
// Type: AIProject.Player.PlayerStateBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public abstract class PlayerStateBase : StateBase, IPlayer
  {
    protected DateTime _startTime = DateTime.MinValue;
    protected TimeSpan _totalMinute = TimeSpan.MinValue;
    protected string _loopStateName = string.Empty;
    protected float _duration;
    protected float _elapsedTime;
    protected bool _hasAction;
    protected int _randomCount;
    protected float _oldNormalizedTime;

    public Action OnCompleted { get; set; }

    public override void Awake(Actor actor)
    {
      Actor.InputInfo stateInfo = actor.StateInfo;
      stateInfo.move = Vector3.get_zero();
      actor.StateInfo = stateInfo;
      if (!(actor is PlayerActor))
        return;
      this.OnAwake(actor as PlayerActor);
    }

    protected abstract void OnAwake(PlayerActor player);

    public override void Release(Actor actor, EventType type)
    {
      PlayerActor player = actor as PlayerActor;
      this.OnRelease(player);
      switch (this)
      {
        case Normal _:
        case Houchi _:
        case Onbu _:
label_4:
          actor.ActivateNavMeshAgent();
          actor.IsKinematic = false;
          break;
        default:
          player.ReleaseCurrentPoint();
          if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
            ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
          MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
          goto label_4;
      }
    }

    protected virtual void OnRelease(PlayerActor player)
    {
    }

    public override void AfterUpdate(Actor actor, Actor.InputInfo info)
    {
      if (!(actor is PlayerActor))
        return;
      this.OnAfterUpdate(actor as PlayerActor, info);
    }

    protected virtual void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
    }

    public override void Update(Actor actor, ref Actor.InputInfo info)
    {
      if (!(actor is PlayerActor))
        return;
      this.OnUpdate(actor as PlayerActor, ref info);
    }

    protected abstract void OnUpdate(PlayerActor player, ref Actor.InputInfo info);

    public override void FixedUpdate(Actor actor, Actor.InputInfo info)
    {
      if (!(actor is PlayerActor))
        return;
      this.OnFixedUpdate(actor as PlayerActor, info);
    }

    protected virtual void OnFixedUpdate(PlayerActor player, Actor.InputInfo info)
    {
    }

    public override void OnAnimatorStateEnter(ActorController control, AnimatorStateInfo stateInfo)
    {
      if (!(control is PlayerController))
        return;
      this.OnAnimatorStateEnterInternal(control as PlayerController, stateInfo);
    }

    protected virtual void OnAnimatorStateEnterInternal(
      PlayerController control,
      AnimatorStateInfo stateInfo)
    {
    }

    public override void OnAnimatorStateExit(ActorController control, AnimatorStateInfo stateInfo)
    {
      if (!(control is PlayerController))
        return;
      this.OnAnimatorStateExitInternal(control as PlayerController, stateInfo);
    }

    protected virtual void OnAnimatorStateExitInternal(
      PlayerController control,
      AnimatorStateInfo stateInfo)
    {
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerStateBase.\u003CEnd\u003Ec__Iterator0()
      {
        actor = actor,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    protected virtual IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerStateBase.\u003COnEnd\u003Ec__Iterator1 onEndCIterator1 = new PlayerStateBase.\u003COnEnd\u003Ec__Iterator1();
      return (IEnumerator) onEndCIterator1;
    }

    protected IObservable<TimeInterval<float>> FadeOutActionAsObservable(
      PlayerActor actor,
      int sex,
      Transform t,
      ActionPoint actionPoint)
    {
      if (Object.op_Inequality((Object) t, (Object) null))
      {
        Vector3 position = actor.Position;
        Quaternion rotation = actor.Rotation;
        ActionPointInfo outInfo;
        actionPoint.TryGetPlayerActionPointInfo(actor.EventKey, out outInfo);
        Dictionary<int, Dictionary<int, PlayState>> dictionary1;
        Dictionary<int, PlayState> dictionary2;
        PlayState playState;
        if (Singleton<Resources>.Instance.Animation.PlayerActionAnimTable.TryGetValue(sex, out dictionary1) && dictionary1.TryGetValue(outInfo.eventID, out dictionary2) && dictionary2.TryGetValue(outInfo.poseID, out playState))
        {
          IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(playState.MainStateInfo.OutStateInfo.FadeSecond, false), false));
          iconnectableObservable.Connect();
          switch (playState.DirectionType)
          {
            case 0:
              if (playState.MainStateInfo.OutStateInfo.EnableFade)
              {
                ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x =>
                {
                  actor.Position = Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value());
                  actor.Rotation = Quaternion.Slerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value());
                }));
                break;
              }
              actor.Position = t.get_position();
              actor.Rotation = t.get_rotation();
              break;
            case 1:
              Quaternion lookRotation = Quaternion.LookRotation(Vector3.op_Subtraction(actionPoint.Position, position));
              if (playState.MainStateInfo.OutStateInfo.EnableFade)
              {
                ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => actor.Rotation = Quaternion.Slerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value())));
                break;
              }
              actor.Rotation = lookRotation;
              break;
          }
          return (IObservable<TimeInterval<float>>) iconnectableObservable;
        }
      }
      return (IObservable<TimeInterval<float>>) Observable.Empty<TimeInterval<float>>();
    }
  }
}
