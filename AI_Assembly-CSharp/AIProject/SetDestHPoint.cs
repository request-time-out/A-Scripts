// Decompiled with JetBrains decompiler
// Type: AIProject.SetDestHPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class SetDestHPoint : AgentAction
  {
    private HPoint _destination;
    private static NavMeshPath _navMeshPath;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      Vector3 position = agent.Position;
      List<HPoint> hpointList1 = ListPool<HPoint>.Get();
      HPointList hpointList2;
      Singleton<Resources>.Instance.HSceneTable.hPointLists.TryGetValue(Singleton<Manager.Map>.Instance.MapID, out hpointList2);
      hpointList1.Clear();
      foreach (KeyValuePair<int, List<HPoint>> keyValuePair in hpointList2.lst)
      {
        if (agent.FromFemale)
        {
          SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 0);
          SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 3);
          SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 5);
          SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 6);
          SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 13);
        }
        else
        {
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          if (player.ChaControl.sex == (byte) 1 && !player.ChaControl.fileParam.futanari)
          {
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 0);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 1);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 2);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 3);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 5);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 6);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 13);
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, 4);
          }
          else
            SetDestHPoint.CreateList(agent, keyValuePair.Value, hpointList1, -1);
        }
      }
      foreach (KeyValuePair<int, Chunk> keyValuePair in Singleton<Manager.Map>.Instance.ChunkTable)
      {
        foreach (MapArea mapArea in keyValuePair.Value.MapAreas)
        {
          if (mapArea.AreaID == agent.AreaID)
            SetDestHPoint.CreateList(agent, mapArea.AppendHPoints, hpointList1, 0);
        }
      }
      if (hpointList1.IsNullOrEmpty<HPoint>())
        return;
      SetDestHPoint.NearestPoint(position, hpointList1, out this._destination);
    }

    private static void CreateList(
      AgentActor agent,
      List<HPoint> source,
      List<HPoint> destination,
      int placeID = -1)
    {
      float hsampleDistance = Singleton<Resources>.Instance.AgentProfile.HSampleDistance;
      foreach (HPoint hpoint in source)
      {
        if (!Object.op_Equality((Object) hpoint, (Object) null) && (placeID == -1 || hpoint._nPlace.Exists<int, ValueTuple<int, int>>((Predicate<KeyValuePair<int, ValueTuple<int, int>>>) (kvp => kvp.Value.Item1 == placeID))))
        {
          if (SetDestHPoint._navMeshPath == null)
            SetDestHPoint._navMeshPath = new NavMeshPath();
          if (agent.NavMeshAgent.CalculatePath(((Component) hpoint).get_transform().get_position(), SetDestHPoint._navMeshPath) && SetDestHPoint._navMeshPath.get_status() == null)
            destination.Add(hpoint);
        }
      }
    }

    private static void NearestPoint(
      Vector3 position,
      List<HPoint> hPoints,
      out HPoint destination)
    {
      destination = (HPoint) null;
      float? nullable = new float?();
      Vector3 vector3 = position;
      foreach (HPoint hPoint in hPoints)
      {
        float num = Vector3.Distance(vector3, ((Component) hPoint).get_transform().get_position());
        if (!nullable.HasValue)
        {
          nullable = new float?(num);
          destination = hPoint;
        }
        else if (!nullable.HasValue || (!nullable.HasValue ? 0 : ((double) nullable.GetValueOrDefault() <= (double) num ? 1 : 0)) == 0)
        {
          nullable = new float?(num);
          destination = hPoint;
        }
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.DestPosition.HasValue)
        return (TaskStatus) 2;
      float hsampleDistance = Singleton<Resources>.Instance.AgentProfile.HSampleDistance;
      NavMeshHit navMeshHit;
      agent.DestPosition = !NavMesh.SamplePosition(((Component) this._destination).get_transform().get_position(), ref navMeshHit, hsampleDistance, -1) ? new Vector3?(((Component) this._destination).get_transform().get_position()) : new Vector3?(((NavMeshHit) ref navMeshHit).get_position());
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      this._destination = (HPoint) null;
    }
  }
}
