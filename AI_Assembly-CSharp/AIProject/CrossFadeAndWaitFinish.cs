// Decompiled with JetBrains decompiler
// Type: AIProject.CrossFadeAndWaitFinish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class CrossFadeAndWaitFinish : AgentAnimatorAction
  {
    [SerializeField]
    private SharedFloat _transitionDuration = (SharedFloat) 0.0f;
    [SerializeField]
    private SharedString _stateName;
    [SerializeField]
    private int _layer;
    [SerializeField]
    private float _normalizedTime;
    [SerializeField]
    private bool _fadesSameState;

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._animator, (Object) null))
      {
        Debug.LogError((object) "Animatorがない");
        return (TaskStatus) 1;
      }
      if (!this._fadesSameState)
      {
        AnimatorStateInfo animatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this._layer);
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this._stateName.get_Value()))
        {
          Debug.Log((object) "同一ステートへの遷移はしない設定");
          return (TaskStatus) 2;
        }
      }
      this._animator.CrossFade(this._stateName.get_Value(), this._transitionDuration.get_Value(), this._layer, this._normalizedTime);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
    }
  }
}
