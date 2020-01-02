// Decompiled with JetBrains decompiler
// Type: AIProject.Warp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Warp : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.StateType = AIProject.Definitions.State.Type.Immobility;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      if (Object.op_Equality((Object) agent.CurrentPoint, (Object) null))
      {
        this.ClearDesire(agent);
        return (TaskStatus) 2;
      }
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      WarpPoint currentPoint = agent.CurrentPoint as WarpPoint;
      if (Object.op_Equality((Object) currentPoint, (Object) null))
      {
        agent.CurrentPoint = (ActionPoint) null;
        this.ClearDesire(agent);
        return (TaskStatus) 2;
      }
      WarpPoint warpPoint = currentPoint.PairPoint();
      Renderer[] renderers = warpPoint.Renderers;
      bool flag = false;
      if (!renderers.IsNullOrEmpty<Renderer>())
      {
        foreach (Renderer renderer in renderers)
        {
          if (renderer.get_isVisible())
          {
            flag = true;
            break;
          }
        }
      }
      if (agent.ChaControl.IsVisibleInCamera)
      {
        ActorCameraControl cameraControl = Singleton<Manager.Map>.Instance.Player.CameraControl;
        if ((double) Vector3.Distance(agent.Position, ((Component) cameraControl).get_transform().get_position()) <= (double) Singleton<Resources>.Instance.LocomotionProfile.CrossFadeEnableDistance)
          cameraControl.CrossFade.FadeStart(-1f);
      }
      else if (flag)
        Singleton<Manager.Map>.Instance.Player.CameraControl.CrossFade.FadeStart(-1f);
      this.PlayWarpSE(400, ((Component) currentPoint).get_transform());
      this.PlayWarpSE(401, ((Component) warpPoint).get_transform());
      agent.NavMeshWarp(((Component) warpPoint).get_transform(), 0, 100f);
      this.ClearDesire(agent);
      return (TaskStatus) 2;
    }

    private void PlayWarpSE(int clipID, Transform t)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Transform transform = ((Component) Manager.Map.GetCameraComponent())?.get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      SoundPack.Data3D data;
      if (!soundPack.TryGetActionSEData(clipID, out data))
        return;
      Vector3 position = t.get_position();
      float num = Mathf.Pow(data.MaxDistance + soundPack.Game3DInfo.MarginMaxDistance, 2f);
      Vector3 vector3 = Vector3.op_Subtraction(position, transform.get_position());
      float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
      if ((double) num < (double) sqrMagnitude)
        return;
      AudioSource audioSource = soundPack.Play((SoundPack.IData) data, Sound.Type.GameSE3D, 0.0f);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return;
      audioSource.Stop();
      ((Component) audioSource).get_transform().set_position(position);
      audioSource.Play();
    }

    private void ClearDesire(AgentActor agent)
    {
      int desireKey = Desire.GetDesireKey(Desire.Type.Game);
      agent.SetDesire(desireKey, 0.0f);
    }
  }
}
