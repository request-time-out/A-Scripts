// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug.DrawLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
  [TaskCategory("Unity/Debug")]
  [TaskDescription("Draws a debug line")]
  public class DrawLine : Action
  {
    [Tooltip("The start position")]
    public SharedVector3 start;
    [Tooltip("The end position")]
    public SharedVector3 end;
    [Tooltip("The color")]
    public SharedColor color;
    [Tooltip("Duration the line will be visible for in seconds.\nDefault: 0 means 1 frame.")]
    public SharedFloat duration;
    [Tooltip("Whether the line should show through world geometry.")]
    public SharedBool depthTest;

    public DrawLine()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Debug.DrawLine(this.start.get_Value(), this.end.get_Value(), this.color.get_Value(), this.duration.get_Value(), this.depthTest.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.start = (SharedVector3) Vector3.get_zero();
      this.end = (SharedVector3) Vector3.get_zero();
      this.color = (SharedColor) Color.get_white();
      this.duration = (SharedFloat) 0.0f;
      this.depthTest = (SharedBool) true;
    }
  }
}
