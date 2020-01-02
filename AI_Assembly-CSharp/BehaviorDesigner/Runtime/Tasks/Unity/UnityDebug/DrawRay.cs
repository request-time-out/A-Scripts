// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug.DrawRay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
  [TaskCategory("Unity/Debug")]
  [TaskDescription("Draws a debug ray")]
  public class DrawRay : Action
  {
    [Tooltip("The position")]
    public SharedVector3 start;
    [Tooltip("The direction")]
    public SharedVector3 direction;
    [Tooltip("The color")]
    public SharedColor color;

    public DrawRay()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Debug.DrawRay(this.start.get_Value(), this.direction.get_Value(), this.color.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.start = (SharedVector3) Vector3.get_zero();
      this.direction = (SharedVector3) Vector3.get_zero();
      this.color = (SharedColor) Color.get_white();
    }
  }
}
