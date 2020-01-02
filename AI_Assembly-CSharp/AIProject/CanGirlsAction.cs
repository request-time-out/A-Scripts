// Decompiled with JetBrains decompiler
// Type: AIProject.CanGirlsAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class CanGirlsAction : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.CanGirlsAction && (double) Random.Range(0.0f, 100f) <= (double) Singleton<Resources>.Instance.StatusProfile.GirlsActionProb ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
