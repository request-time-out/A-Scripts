// Decompiled with JetBrains decompiler
// Type: AIProject.ReduceDesire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ReduceDesire : AgentAction
  {
    [SerializeField]
    private Desire.Type _type = Desire.Type.Toilet;
    private int _desireKey;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._desireKey = Desire.GetDesireKey(this._type);
    }

    public virtual TaskStatus OnUpdate()
    {
      float? desire = this.Agent.GetDesire(this._desireKey);
      if (!desire.HasValue)
        return (TaskStatus) 1;
      return (double) desire.Value <= 0.0 ? (TaskStatus) 1 : (TaskStatus) 3;
    }
  }
}
