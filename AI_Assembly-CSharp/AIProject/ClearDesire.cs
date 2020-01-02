// Decompiled with JetBrains decompiler
// Type: AIProject.ClearDesire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class ClearDesire : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      int desireKey = Desire.GetDesireKey(this.Agent.RuntimeDesire);
      if (desireKey != -1)
        this.Agent.SetDesire(desireKey, 0.0f);
      return (TaskStatus) 2;
    }
  }
}
