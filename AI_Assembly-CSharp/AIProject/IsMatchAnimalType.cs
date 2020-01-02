// Decompiled with JetBrains decompiler
// Type: AIProject.IsMatchAnimalType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsMatchAnimalType : AgentConditional
  {
    [SerializeField]
    private AnimalTypes targetAnimalType;

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.Agent.TargetInSightAnimal, (Object) null))
        return (TaskStatus) 1;
      return this.Agent.TargetInSightAnimal.AnimalType == this.targetAnimalType ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
