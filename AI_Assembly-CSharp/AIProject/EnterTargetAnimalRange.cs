// Decompiled with JetBrains decompiler
// Type: AIProject.EnterTargetAnimalRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterTargetAnimalRange : AgentConditional
  {
    private float arrivedDistance;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.arrivedDistance = Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance;
    }

    public virtual TaskStatus OnUpdate()
    {
      AnimalBase targetInSightAnimal = this.Agent.TargetInSightAnimal;
      return Object.op_Inequality((Object) targetInSightAnimal, (Object) null) && (double) Vector3.Distance(this.Agent.Position, targetInSightAnimal.Position) <= (double) this.arrivedDistance ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
