// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.WaitFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class WaitFrame : MerchantAction
  {
    [SerializeField]
    private int _waitCount;
    private int _updateCount;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._updateCount = 0;
    }

    public virtual TaskStatus OnUpdate()
    {
      ++this._updateCount;
      return this._waitCount <= this._updateCount ? (TaskStatus) 2 : (TaskStatus) 3;
    }
  }
}
