// Decompiled with JetBrains decompiler
// Type: AIProject.Break
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Break : AgentStateAction
  {
    public override void OnStart()
    {
      this.Agent.EventKey = EventType.Break;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      if (this._unchangeParamState)
        return;
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Break);
      agent.SetDesire(desireKey, 0.0f);
      if (Random.Range(0, 20) >= 1)
        return;
      agent.AgentData.SickState.ID = -1;
    }
  }
}
