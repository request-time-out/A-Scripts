// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Patrol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=7")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
  public class Patrol : NavMeshMovement
  {
    [Tooltip("Should the agent patrol the waypoints randomly?")]
    public SharedBool randomPatrol = (SharedBool) false;
    [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
    public SharedFloat waypointPauseDuration = (SharedFloat) 0.0f;
    [Tooltip("The waypoints to move to")]
    public SharedGameObjectList waypoints;
    private int waypointIndex;
    private float waypointReachedTime;

    public override void OnStart()
    {
      base.OnStart();
      float num1 = float.PositiveInfinity;
      for (int index = 0; index < this.waypoints.get_Value().Count; ++index)
      {
        float num2;
        if ((double) (num2 = Vector3.Magnitude(Vector3.op_Subtraction(((Transform) ((Task) this).transform).get_position(), this.waypoints.get_Value()[index].get_transform().get_position()))) < (double) num1)
        {
          num1 = num2;
          this.waypointIndex = index;
        }
      }
      this.waypointReachedTime = -1f;
      this.SetDestination(this.Target());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.waypoints.get_Value().Count == 0)
        return (TaskStatus) 1;
      if (this.HasArrived())
      {
        if ((double) this.waypointReachedTime == -1.0)
          this.waypointReachedTime = Time.get_time();
        if ((double) this.waypointReachedTime + (double) this.waypointPauseDuration.get_Value() <= (double) Time.get_time())
        {
          if (this.randomPatrol.get_Value())
          {
            if (this.waypoints.get_Value().Count == 1)
            {
              this.waypointIndex = 0;
            }
            else
            {
              int num = this.waypointIndex;
              while (num == this.waypointIndex)
                num = Random.Range(0, this.waypoints.get_Value().Count);
              this.waypointIndex = num;
            }
          }
          else
            this.waypointIndex = (this.waypointIndex + 1) % this.waypoints.get_Value().Count;
          this.SetDestination(this.Target());
          this.waypointReachedTime = -1f;
        }
      }
      return (TaskStatus) 3;
    }

    private Vector3 Target()
    {
      return this.waypointIndex >= this.waypoints.get_Value().Count ? ((Transform) ((Task) this).transform).get_position() : this.waypoints.get_Value()[this.waypointIndex].get_transform().get_position();
    }

    public override void OnReset()
    {
      base.OnReset();
      this.randomPatrol = (SharedBool) false;
      this.waypointPauseDuration = (SharedFloat) 0.0f;
      this.waypoints = (SharedGameObjectList) null;
    }

    public virtual void OnDrawGizmos()
    {
    }
  }
}
