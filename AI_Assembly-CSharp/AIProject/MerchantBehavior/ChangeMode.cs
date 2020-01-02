// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.ChangeMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class ChangeMode : MerchantAction
  {
    [SerializeField]
    private Merchant.ActionType _mode = Merchant.ActionType.Wait;

    public virtual TaskStatus OnUpdate()
    {
      this.Merchant.ChangeBehavior(this._mode);
      return (TaskStatus) 2;
    }
  }
}
