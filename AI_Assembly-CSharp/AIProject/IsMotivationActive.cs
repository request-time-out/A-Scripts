// Decompiled with JetBrains decompiler
// Type: AIProject.IsMotivationActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsMotivationActive : AgentConditional
  {
    [SerializeField]
    private Desire.Type _key;

    public virtual TaskStatus OnUpdate()
    {
      float? motivation = this.Agent.GetMotivation(Desire.GetDesireKey(this._key));
      if (!motivation.HasValue)
        return (TaskStatus) 1;
      return (double) motivation.Value >= (double) Singleton<Resources>.Instance.AgentProfile.ActiveMotivationBorder ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
