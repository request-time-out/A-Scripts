﻿// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.SetElapsedDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class SetElapsedDay : MerchantAction
  {
    [SerializeField]
    private bool _setValue;

    public virtual TaskStatus OnUpdate()
    {
      this.Merchant.ElapsedDay = this._setValue;
      return (TaskStatus) 2;
    }
  }
}