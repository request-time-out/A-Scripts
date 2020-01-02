// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.Lerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Lerp the Vector3 by an amount.")]
  public class Lerp : Action
  {
    [Tooltip("The from value")]
    public SharedVector3 fromVector3;
    [Tooltip("The to value")]
    public SharedVector3 toVector3;
    [Tooltip("The amount to lerp")]
    public SharedFloat lerpAmount;
    [Tooltip("The lerp resut")]
    [RequiredField]
    public SharedVector3 storeResult;

    public Lerp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.Lerp(this.fromVector3.get_Value(), this.toVector3.get_Value(), this.lerpAmount.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromVector3 = this.toVector3 = this.storeResult = (SharedVector3) Vector3.get_zero();
      this.lerpAmount = (SharedFloat) 0.0f;
    }
  }
}
