// Decompiled with JetBrains decompiler
// Type: AIProject.Fight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class Fight : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.RuntimeDesire = Desire.Type.Lonely;
      this.Agent.StartTalkSequence(this.Agent.CommandPartner);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.LivesTalkSequence)
        return (TaskStatus) 3;
      int desireKey = Desire.GetDesireKey(this.Agent.RuntimeDesire);
      if (desireKey != -1)
        this.Agent.SetDesire(desireKey, 0.0f);
      this.Agent.RuntimeDesire = Desire.Type.None;
      return (TaskStatus) 2;
    }
  }
}
