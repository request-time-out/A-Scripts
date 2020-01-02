// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.Distance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Returns the distance between two Vector2s.")]
  public class Distance : Action
  {
    [Tooltip("The first Vector2")]
    public SharedVector2 firstVector2;
    [Tooltip("The second Vector2")]
    public SharedVector2 secondVector2;
    [Tooltip("The distance")]
    [RequiredField]
    public SharedFloat storeResult;

    public Distance()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.Distance(this.firstVector2.get_Value(), this.secondVector2.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.firstVector2 = this.secondVector2 = (SharedVector2) Vector2.get_zero();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
