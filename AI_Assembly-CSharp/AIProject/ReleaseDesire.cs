// Decompiled with JetBrains decompiler
// Type: AIProject.ReleaseDesire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ReleaseDesire : AgentAction
  {
    [SerializeField]
    private Desire.Type _desireType;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.SetDesire(Desire.GetDesireKey(this._desireType), 0.0f);
      return (TaskStatus) 2;
    }
  }
}
