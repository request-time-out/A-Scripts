// Decompiled with JetBrains decompiler
// Type: AIProject.Blackout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Blackout : AgentAction
  {
    [SerializeField]
    private float _revivalBorderLine = 100f;
    [SerializeField]
    private float _minHour = 1f;
    [SerializeField]
    private float _maxHour = 1f;
    [SerializeField]
    private string _stateName = string.Empty;
    [SerializeField]
    private float _fadeTime = 0.1f;
    private int _stateNameHash = -1;
    private float _velocity;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._velocity = this._revivalBorderLine / Mathf.Lerp(this._minHour * 60f, this._maxHour * 60f, Random.get_value());
      this.Agent.DeactivateNavMeshAgent();
      if (!this.Agent.Animation.Animator.HasState(0, this._stateNameHash))
        return;
      this.Agent.Animation.Animator.CrossFadeInFixedTime(this._stateNameHash, this._fadeTime, 0);
      Debug.Log((object) string.Format("Cross Fade to {0}", (object) this._stateName));
    }

    public virtual TaskStatus OnUpdate()
    {
      Dictionary<int, float> statsTable1;
      (statsTable1 = this.Agent.AgentData.StatsTable)[3] = statsTable1[3] + this._velocity * Time.get_deltaTime();
      float num;
      this.Agent.AgentData.StatsTable.TryGetValue(3, out num);
      if ((double) num <= (double) this._revivalBorderLine)
        return (TaskStatus) 3;
      Dictionary<int, float> statsTable2;
      (statsTable2 = this.Agent.AgentData.StatsTable)[4] = statsTable2[4] - 3f;
      return (TaskStatus) 2;
    }
  }
}
