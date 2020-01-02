// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics.Linecast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics
{
  [TaskCategory("Unity/Physics")]
  [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
  public class Linecast : Action
  {
    [Tooltip("The starting position of the linecast")]
    public SharedVector3 startPosition;
    [Tooltip("The ending position of the linecast")]
    public SharedVector3 endPosition;
    [Tooltip("Selectively ignore colliders.")]
    public LayerMask layerMask;

    public Linecast()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return Physics.Linecast(this.startPosition.get_Value(), this.endPosition.get_Value(), LayerMask.op_Implicit(this.layerMask)) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.startPosition = (SharedVector3) Vector3.get_zero();
      this.endPosition = (SharedVector3) Vector3.get_zero();
      this.layerMask = LayerMask.op_Implicit(-1);
    }
  }
}
