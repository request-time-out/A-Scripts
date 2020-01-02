// Decompiled with JetBrains decompiler
// Type: AIProject.IsDesireRollTheDice
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
  public class IsDesireRollTheDice : AgentConditional
  {
    [SerializeField]
    private Desire.Type _key = Desire.Type.Toilet;
    [SerializeField]
    private bool _compareDowner;

    public virtual TaskStatus OnUpdate()
    {
      int desireKey = Desire.GetDesireKey(this._key);
      float num1 = (float) Singleton<Resources>.Instance.GetDesireBorder(desireKey).Item2;
      float? desire = this.Agent.GetDesire(desireKey);
      if (!desire.HasValue)
        return (TaskStatus) 1;
      float num2 = Random.Range(0.0f, num1);
      if (!this._compareDowner)
      {
        if (Mathf.Approximately(desire.Value, 0.0f))
          return (TaskStatus) 1;
        return (double) num2 <= (double) desire.Value ? (TaskStatus) 2 : (TaskStatus) 1;
      }
      if (Mathf.Approximately(desire.Value, num1))
        return (TaskStatus) 1;
      return (double) desire.Value <= (double) num2 ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
