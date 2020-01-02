// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.Distance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Returns the distance between two Vector3s.")]
  public class Distance : Action
  {
    [Tooltip("The first Vector3")]
    public SharedVector3 firstVector3;
    [Tooltip("The second Vector3")]
    public SharedVector3 secondVector3;
    [Tooltip("The distance")]
    [RequiredField]
    public SharedFloat storeResult;

    public Distance()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.Distance(this.firstVector3.get_Value(), this.secondVector3.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.firstVector3 = this.secondVector3 = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
