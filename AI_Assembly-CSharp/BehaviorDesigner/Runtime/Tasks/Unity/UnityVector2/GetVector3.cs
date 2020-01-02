// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.GetVector3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Stores the Vector3 value of the Vector2.")]
  public class GetVector3 : Action
  {
    [Tooltip("The Vector2 to get the Vector3 value of")]
    public SharedVector2 vector3Variable;
    [Tooltip("The Vector3 value")]
    [RequiredField]
    public SharedVector3 storeResult;

    public GetVector3()
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
      this.vector3Variable = (SharedVector2) Vector2.get_zero();
      this.storeResult = (SharedVector3) Vector3.get_zero();
    }
  }
}
