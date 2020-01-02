// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D.Linecast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D
{
  [TaskCategory("Unity/Physics2D")]
  [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
  public class Linecast : Action
  {
    [Tooltip("The starting position of the linecast.")]
    public SharedVector2 startPosition;
    [Tooltip("The ending position of the linecast.")]
    public SharedVector2 endPosition;
    [Tooltip("Selectively ignore colliders.")]
    public LayerMask layerMask;

    public Linecast()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return RaycastHit2D.op_Implicit(Physics2D.Linecast(this.startPosition.get_Value(), this.endPosition.get_Value(), LayerMask.op_Implicit(this.layerMask))) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.startPosition = (SharedVector2) Vector2.get_zero();
      this.endPosition = (SharedVector2) Vector2.get_zero();
      this.layerMask = LayerMask.op_Implicit(-1);
    }
  }
}
