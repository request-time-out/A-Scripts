// Decompiled with JetBrains decompiler
// Type: AIProject.GetSulky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace AIProject
{
  public class GetSulky : AgentAction
  {
    [SerializeField]
    [FormerlySerializedAs("Duration")]
    private float _duration;
    private float _elapsedTime;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.Animation.Animator.CrossFadeInFixedTime("sulky", 0.1f, 0, 0.1f, 0.0f);
    }

    public virtual TaskStatus OnUpdate()
    {
      this._elapsedTime += Time.get_deltaTime();
      return (double) this._elapsedTime < (double) this._duration ? (TaskStatus) 3 : (TaskStatus) 2;
    }
  }
}
