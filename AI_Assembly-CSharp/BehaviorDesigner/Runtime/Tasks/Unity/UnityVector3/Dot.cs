// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.Dot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Stores the dot product of two Vector3 values.")]
  public class Dot : Action
  {
    [Tooltip("The left hand side of the dot product")]
    public SharedVector3 leftHandSide;
    [Tooltip("The right hand side of the dot product")]
    public SharedVector3 rightHandSide;
    [Tooltip("The dot product result")]
    [RequiredField]
    public SharedFloat storeResult;

    public Dot()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.Dot(this.leftHandSide.get_Value(), this.rightHandSide.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.leftHandSide = this.rightHandSide = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
