// Decompiled with JetBrains decompiler
// Type: AIProject.WaitFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class WaitFrame : Action
  {
    [SerializeField]
    private int _waitCount;
    private int _updatedCount;

    public WaitFrame()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._updatedCount = 0;
    }

    public virtual TaskStatus OnUpdate()
    {
      ++this._updatedCount;
      return this._updatedCount >= this._waitCount ? (TaskStatus) 2 : (TaskStatus) 3;
    }
  }
}
