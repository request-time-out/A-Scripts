// Decompiled with JetBrains decompiler
// Type: AIProject.IsDesireActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class IsDesireActive : AgentConditional
  {
    [SerializeField]
    private Desire.Type _key = Desire.Type.Hunt;
    private int _migrationBorder;
    [SerializeField]
    private bool _checkLimit;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      ValueTuple<int, int> desireBorder = Singleton<Resources>.Instance.GetDesireBorder(Desire.GetDesireKey(this._key));
      if (this._checkLimit)
        this._migrationBorder = (int) desireBorder.Item2;
      else
        this._migrationBorder = (int) desireBorder.Item1;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(this._key);
      float? desire = agent.GetDesire(desireKey);
      if (!desire.HasValue)
        return (TaskStatus) 1;
      if ((double) desire.Value >= (double) this._migrationBorder)
      {
        float? motivation = agent.GetMotivation(desireKey);
        if (motivation.HasValue)
        {
          if ((double) motivation.Value >= (double) Singleton<Resources>.Instance.AgentProfile.ActiveMotivationBorder)
            return (TaskStatus) 2;
          agent.SetDesire(desireKey, 0.0f);
          return (TaskStatus) 1;
        }
      }
      return (TaskStatus) 1;
    }
  }
}
