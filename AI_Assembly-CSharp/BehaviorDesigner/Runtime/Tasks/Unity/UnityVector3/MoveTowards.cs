// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.MoveTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Move from the current position to the target position.")]
  public class MoveTowards : Action
  {
    [Tooltip("The current position")]
    public SharedVector3 currentPosition;
    [Tooltip("The target position")]
    public SharedVector3 targetPosition;
    [Tooltip("The movement speed")]
    public SharedFloat speed;
    [Tooltip("The move resut")]
    [RequiredField]
    public SharedVector3 storeResult;

    public MoveTowards()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.MoveTowards(this.currentPosition.get_Value(), this.targetPosition.get_Value(), this.speed.get_Value() * Time.get_deltaTime()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.currentPosition = this.targetPosition = this.storeResult = (SharedVector3) Vector3.get_zero();
      this.speed = (SharedFloat) 0.0f;
    }
  }
}
