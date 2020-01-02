// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAnimatorAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  public abstract class AgentAnimatorAction : AgentAction
  {
    protected Animator _animator;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._animator = this.Agent.Animation.Animator;
    }
  }
}
