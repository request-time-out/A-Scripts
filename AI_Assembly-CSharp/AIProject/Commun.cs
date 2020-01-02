// Decompiled with JetBrains decompiler
// Type: AIProject.Commun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Commun : AgentAction
  {
    private bool _updatedMotivation;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StateType = AIProject.Definitions.State.Type.Commun;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      Transform trfTarg = Singleton<Manager.Map>.Instance.Player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head);
      agent.ChaControl.ChangeLookEyesTarget(1, trfTarg, 0.5f, 0.0f, 1f, 2f);
      agent.ChaControl.ChangeLookEyesPtn(1);
      agent.ChaControl.ChangeLookNeckTarget(1, trfTarg, 0.5f, 0.0f, 1f, 0.8f);
      agent.ChaControl.ChangeLookNeckPtn(1, 1f);
      agent.MotivationInEncounter = agent.AgentData.StatsTable[5];
      agent.UpdateMotivation = true;
      Debug.Log((object) string.Format("Encounter.Commun.OnStart UpdateMotivation=true proc: {0}", (object) Time.get_frameCount()));
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if ((double) agent.MotivationInEncounter <= 0.0)
        return (TaskStatus) 2;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      return agent.ReleasableCommand && agent.IsFarPlayer ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      AgentActor agent = this.Agent;
      agent.ChaControl.ChangeLookEyesPtn(0);
      agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      agent.ChaControl.ChangeLookNeckPtn(3, 1f);
      agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      agent.ChangeDynamicNavMeshAgentAvoidance();
      agent.UpdateMotivation = false;
    }

    public virtual void OnPause(bool paused)
    {
      AgentActor agent = this.Agent;
      if (paused)
      {
        this._updatedMotivation = paused;
        agent.UpdateMotivation = false;
      }
      else
        agent.UpdateMotivation = this._updatedMotivation;
    }
  }
}
