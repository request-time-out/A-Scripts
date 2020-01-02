// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.IsMatchEventType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class IsMatchEventType : MerchantConditional
  {
    [SerializeField]
    private Merchant.EventType _matchType;
    private MerchantPoint targetPoint;
    private MerchantPoint mainTargetPoint;
    private NavMeshPath navMeshPath;

    public virtual void OnAwake()
    {
      ((Task) this).OnAwake();
      if (this.navMeshPath != null)
        return;
      this.navMeshPath = new NavMeshPath();
    }

    public virtual void OnStart()
    {
      if (Object.op_Equality((Object) (this.targetPoint = this.TargetInSightMerchantPoint), (Object) null))
        this.targetPoint = this.CurrentMerchantPoint;
      this.mainTargetPoint = this.MainTargetInsightMerchantPoint;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.targetPoint, (Object) null))
        return (TaskStatus) 1;
      MerchantPoint merchantPoint = !Object.op_Inequality((Object) this.mainTargetPoint, (Object) null) ? this.targetPoint : this.mainTargetPoint;
      if (this.Merchant.OpenAreaID < merchantPoint.AreaID)
        return (TaskStatus) 1;
      Merchant.EventType eventType = merchantPoint.EventType;
      if (!NavMesh.CalculatePath(this.Merchant.Position, this.targetPoint.Destination, this.Merchant.NavMeshAgent.get_areaMask(), this.navMeshPath) || this.navMeshPath.get_status() != null)
        return (TaskStatus) 1;
      return (eventType & this._matchType) == this._matchType ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
