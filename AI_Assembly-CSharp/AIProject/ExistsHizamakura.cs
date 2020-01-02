// Decompiled with JetBrains decompiler
// Type: AIProject.ExistsHizamakura
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class ExistsHizamakura : AgentConditional
  {
    private static NavMeshPath _navMeshPath;

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      Chunk chunk;
      Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(agent.ChunkID, out chunk);
      Vector3 position = agent.Position;
      List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
      ExistsHizamakura.CreateList(agent, chunk.AppendActionPoints, actionPointList);
      bool flag = !actionPointList.IsNullOrEmpty<ActionPoint>();
      if (!flag)
      {
        ExistsHizamakura.CreateList(agent, chunk.ActionPoints, actionPointList);
        flag = !actionPointList.IsNullOrEmpty<ActionPoint>();
      }
      ListPool<ActionPoint>.Release(actionPointList);
      return flag ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    private static void CreateList(
      AgentActor agent,
      List<ActionPoint> source,
      List<ActionPoint> destination)
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
      Dictionary<int, bool> toRelease = DictionaryPool<int, bool>.Get();
      int hizamakuraID = Singleton<Resources>.Instance.PlayerProfile.HizamakuraPTID;
      foreach (ActionPoint actionPoint in source)
      {
        if (!Object.op_Equality((Object) actionPoint, (Object) null) && !Object.op_Equality((Object) actionPoint.OwnerArea, (Object) null) && (actionPoint.IsNeutralCommand && !actionPoint.IsReserved(agent)))
        {
          MapArea ownerArea = actionPoint.OwnerArea;
          bool flag;
          if (!toRelease.TryGetValue(ownerArea.AreaID, out flag))
            toRelease[ownerArea.AreaID] = flag = Singleton<Manager.Map>.Instance.CheckAvailableMapArea(ownerArea.AreaID);
          if (flag)
          {
            EventType eventType = actionPoint.PlayerDateEventType[(int) Singleton<Manager.Map>.Instance.Player.ChaControl.sex];
            if (!actionPoint.IDList.IsNullOrEmpty<int>() && ((IEnumerable<int>) actionPoint.IDList).Any<int>((Func<int, bool>) (x => x == hizamakuraID)) || actionPoint.IDList.IsNullOrEmpty<int>() && actionPoint.ID == hizamakuraID)
            {
              if (ExistsHizamakura._navMeshPath == null)
                ExistsHizamakura._navMeshPath = new NavMeshPath();
              NavMeshHit navMeshHit;
              if (agent.NavMeshAgent.CalculatePath(actionPoint.LocatedPosition, ExistsHizamakura._navMeshPath) && ExistsHizamakura._navMeshPath.get_status() == null && NavMesh.SamplePosition(actionPoint.LocatedPosition, ref navMeshHit, meshSampleDistance, agent.NavMeshAgent.get_areaMask()))
                destination.Add(actionPoint);
            }
          }
        }
      }
      DictionaryPool<int, bool>.Release(toRelease);
    }
  }
}
