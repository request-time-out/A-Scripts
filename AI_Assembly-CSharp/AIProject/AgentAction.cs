// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  public abstract class AgentAction : Action
  {
    private AgentActor _agent;

    protected AgentAction()
    {
      base.\u002Ector();
    }

    protected AgentActor Agent
    {
      get
      {
        return this._agent ?? (this._agent = (((Task) this).get_Owner() as AgentBehaviorTree).SourceAgent);
      }
    }
  }
}
