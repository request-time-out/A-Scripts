// Decompiled with JetBrains decompiler
// Type: AIProject.IsEmptyMotivation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class IsEmptyMotivation : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(agent.RequestedDesire);
      float? motivation = agent.GetMotivation(desireKey);
      return motivation.HasValue && (double) motivation.Value <= 0.0 ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
