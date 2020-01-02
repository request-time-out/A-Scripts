// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.Normalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Normalize the Vector2.")]
  public class Normalize : Action
  {
    [Tooltip("The Vector2 to normalize")]
    public SharedVector2 vector2Variable;
    [Tooltip("The normalized resut")]
    [RequiredField]
    public SharedVector2 storeResult;

    public Normalize()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      SharedVector2 storeResult = this.storeResult;
      Vector2 vector2 = this.vector2Variable.get_Value();
      Vector2 normalized = ((Vector2) ref vector2).get_normalized();
      storeResult.set_Value(normalized);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = this.storeResult = (SharedVector2) Vector2.get_zero();
    }
  }
}
