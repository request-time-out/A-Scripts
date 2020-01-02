// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Follow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Follows the specified target using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=23")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FollowIcon.png")]
  public class Follow : NavMeshMovement
  {
    [Tooltip("Start moving towards the target if the target is further than the specified distance")]
    public SharedFloat moveDistance = (SharedFloat) 2f;
    [Tooltip("The GameObject that the agent is following")]
    public SharedGameObject target;
    private Vector3 lastTargetPosition;
    private bool hasMoved;

    public override void OnStart()
    {
      base.OnStart();
      this.lastTargetPosition = Vector3.op_Addition(this.target.get_Value().get_transform().get_position(), Vector3.op_Multiply(Vector3.get_one(), this.moveDistance.get_Value() + 1f));
      this.hasMoved = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.target.get_Value(), (Object) null))
        return (TaskStatus) 1;
      Vector3 position = this.target.get_Value().get_transform().get_position();
      Vector3 vector3_1 = Vector3.op_Subtraction(position, this.lastTargetPosition);
      if ((double) ((Vector3) ref vector3_1).get_magnitude() >= (double) this.moveDistance.get_Value())
      {
        this.SetDestination(position);
        this.lastTargetPosition = position;
        this.hasMoved = true;
      }
      else if (this.hasMoved)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(position, ((Transform) ((Task) this).transform).get_position());
        if ((double) ((Vector3) ref vector3_2).get_magnitude() < (double) this.moveDistance.get_Value())
        {
          this.Stop();
          this.hasMoved = false;
          this.lastTargetPosition = position;
        }
      }
      return (TaskStatus) 3;
    }

    public override void OnReset()
    {
      base.OnReset();
      this.target = (SharedGameObject) null;
      this.moveDistance = (SharedFloat) 2f;
    }
  }
}
