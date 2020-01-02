// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.Lerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Lerp the Vector2 by an amount.")]
  public class Lerp : Action
  {
    [Tooltip("The from value")]
    public SharedVector2 fromVector2;
    [Tooltip("The to value")]
    public SharedVector2 toVector2;
    [Tooltip("The amount to lerp")]
    public SharedFloat lerpAmount;
    [Tooltip("The lerp resut")]
    [RequiredField]
    public SharedVector2 storeResult;

    public Lerp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.Lerp(this.fromVector2.get_Value(), this.toVector2.get_Value(), this.lerpAmount.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromVector2 = this.toVector2 = this.storeResult = (SharedVector2) Vector2.get_zero();
      this.lerpAmount = (SharedFloat) 0.0f;
    }
  }
}
