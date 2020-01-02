// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.GetVector2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Stores the Vector2 value of the Vector3.")]
  public class GetVector2 : Action
  {
    [Tooltip("The Vector3 to get the Vector2 value of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The Vector2 value")]
    [RequiredField]
    public SharedVector2 storeResult;

    public GetVector2()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.op_Implicit(this.vector3Variable.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedVector2) Vector2.get_zero();
    }
  }
}
