// Decompiled with JetBrains decompiler
// Type: AIProject.IsDesireCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsDesireCondition : AgentConditional
  {
    [SerializeField]
    private SharedFloat _boundingValue = (SharedFloat) 0.0f;
    [SerializeField]
    private Desire.Type _desireType;

    public virtual TaskStatus OnUpdate()
    {
      int desireKey = Desire.GetDesireKey(this._desireType);
      if (desireKey == -1)
        return (TaskStatus) 1;
      float? desire = this.Agent.GetDesire(desireKey);
      return (!desire.HasValue ? 0 : ((double) desire.GetValueOrDefault() < (double) this._boundingValue.get_Value() ? 1 : 0)) != 0 ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
