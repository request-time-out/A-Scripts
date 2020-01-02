// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.GetSqrMagnitude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Stores the square magnitude of the Vector2.")]
  public class GetSqrMagnitude : Action
  {
    [Tooltip("The Vector2 to get the square magnitude of")]
    public SharedVector2 vector2Variable;
    [Tooltip("The square magnitude of the vector")]
    [RequiredField]
    public SharedFloat storeResult;

    public GetSqrMagnitude()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      SharedFloat storeResult = this.storeResult;
      Vector2 vector2 = this.vector2Variable.get_Value();
      double sqrMagnitude = (double) ((Vector2) ref vector2).get_sqrMagnitude();
      storeResult.set_Value((float) sqrMagnitude);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = (SharedVector2) Vector2.get_zero();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
