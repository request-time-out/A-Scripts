// Decompiled with JetBrains decompiler
// Type: AIProject.MoveTowardArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class MoveTowardArea : AgentAction
  {
    [SerializeField]
    private MapArea.AreaType _area = MapArea.AreaType.Indoor;
    [SerializeField]
    private float _contactedRadius = 1f;
    private Waypoint _destination;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.ActivateTransfer(true);
      Chunk chunk;
      if (!Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(0, out chunk))
        return;
      MapArea mapArea1 = (MapArea) null;
      foreach (MapArea mapArea2 in chunk.MapAreas)
      {
        if (mapArea2.Type == this._area)
        {
          mapArea1 = mapArea2;
          break;
        }
      }
      if (Object.op_Equality((Object) mapArea1, (Object) null))
        return;
      int index = Random.Range(0, mapArea1.Waypoints.Count);
      Waypoint element = mapArea1.Waypoints.GetElement<Waypoint>(index);
      if (Object.op_Equality((Object) element, (Object) null))
        return;
      this._destination = element;
      this.Agent.NavMeshAgent.SetDestination(((Component) this._destination).get_transform().get_position());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._destination, (Object) null))
        return (TaskStatus) 1;
      return (double) Vector3.Distance(((Component) this._destination).get_transform().get_position(), this.Agent.Position) > (double) this._contactedRadius ? (TaskStatus) 3 : (TaskStatus) 2;
    }
  }
}
