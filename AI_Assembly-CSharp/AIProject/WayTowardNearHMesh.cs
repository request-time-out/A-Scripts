// Decompiled with JetBrains decompiler
// Type: AIProject.WayTowardNearHMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class WayTowardNearHMesh : AgentAction
  {
    [SerializeField]
    private string _tagName = string.Empty;

    public virtual void OnStart()
    {
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(this._tagName);
      List<MeshFilter> toRelease = ListPool<MeshFilter>.Get();
      foreach (GameObject gameObject in gameObjectsWithTag)
      {
        foreach (MeshFilter componentsInChild in (MeshFilter[]) gameObject.GetComponentsInChildren<MeshFilter>())
          toRelease.Add(componentsInChild);
      }
      float num1 = float.PositiveInfinity;
      Vector3 vector3_1 = Vector3.get_zero();
      Vector3 position = this.Agent.Position;
      using (List<MeshFilter>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Vector3 vector3_2 = enumerator.Current.NearestVertexTo(position);
          float num2 = Vector3.Distance(position, vector3_2);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            vector3_1 = vector3_2;
          }
        }
      }
      ListPool<MeshFilter>.Release(toRelease);
      NavMeshHit navMeshHit;
      if (NavMesh.SamplePosition(vector3_1, ref navMeshHit, 100f, -1))
        vector3_1 = ((NavMeshHit) ref navMeshHit).get_position();
      this.Agent.DestPosition = new Vector3?(vector3_1);
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (!agent.DestPosition.HasValue)
        return (TaskStatus) 1;
      if (agent.DestPosition.HasValue)
        agent.NavMeshAgent.SetDestination(agent.DestPosition.Value);
      return (double) Vector3.Distance(agent.DestPosition.Value, agent.Position) > (double) Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPoint ? (TaskStatus) 3 : (TaskStatus) 2;
    }
  }
}
