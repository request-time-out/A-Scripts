// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.LerpAngle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Lerp the angle by an amount.")]
  public class LerpAngle : Action
  {
    [Tooltip("The from value")]
    public SharedFloat fromValue;
    [Tooltip("The to value")]
    public SharedFloat toValue;
    [Tooltip("The amount to lerp")]
    public SharedFloat lerpAmount;
    [Tooltip("The lerp resut")]
    [RequiredField]
    public SharedFloat storeResult;

    public LerpAngle()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Mathf.LerpAngle(this.fromValue.get_Value(), this.toValue.get_Value(), this.lerpAmount.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromValue = this.toValue = this.lerpAmount = this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
