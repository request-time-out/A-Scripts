// Decompiled with JetBrains decompiler
// Type: AIProject.IsAnimatorStateMatch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsAnimatorStateMatch : AgentConditional
  {
    [SerializeField]
    public string _stateName = string.Empty;
    private int _stateNameHash = -1;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._stateNameHash = Animator.StringToHash(this._stateName);
    }

    public virtual TaskStatus OnUpdate()
    {
      AnimatorStateInfo animatorStateInfo = this.Agent.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      return ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash() == this._stateNameHash ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
