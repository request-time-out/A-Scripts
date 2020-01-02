// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Seek
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Seek the target specified using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
  public class Seek : NavMeshMovement
  {
    [Tooltip("The GameObject that the agent is seeking")]
    public SharedGameObject target;
    [Tooltip("If target is null then use the target position")]
    public SharedVector3 targetPosition;

    public override void OnStart()
    {
      base.OnStart();
      this.SetDestination(this.Target());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.HasArrived())
        return (TaskStatus) 2;
      this.SetDestination(this.Target());
      return (TaskStatus) 3;
    }

    private Vector3 Target()
    {
      return Object.op_Inequality((Object) this.target.get_Value(), (Object) null) ? this.target.get_Value().get_transform().get_position() : this.targetPosition.get_Value();
    }

    public override void OnReset()
    {
      base.OnReset();
      this.target = (SharedGameObject) null;
      this.targetPosition = (SharedVector3) Vector3.get_zero();
    }
  }
}
