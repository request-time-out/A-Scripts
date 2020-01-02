// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.GetUpVector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Stores the up vector value.")]
  public class GetUpVector : Action
  {
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedVector2 storeResult;

    public GetUpVector()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.get_up());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.storeResult = (SharedVector2) Vector2.get_zero();
    }
  }
}
