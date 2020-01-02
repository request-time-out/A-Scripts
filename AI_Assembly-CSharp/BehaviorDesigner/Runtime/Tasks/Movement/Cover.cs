// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Cover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Find a place to hide and move to it using the Unity NavMesh.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=8")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CoverIcon.png")]
  public class Cover : NavMeshMovement
  {
    [Tooltip("The distance to search for cover")]
    public SharedFloat maxCoverDistance = (SharedFloat) 1000f;
    [Tooltip("The maximum number of raycasts that should be fired before the agent gives up looking for an agent to find cover behind")]
    public SharedInt maxRaycasts = (SharedInt) 100;
    [Tooltip("How large the step should be between raycasts")]
    public SharedFloat rayStep = (SharedFloat) 1f;
    [Tooltip("Once a cover point has been found, multiply this offset by the normal to prevent the agent from hugging the wall")]
    public SharedFloat coverOffset = (SharedFloat) 2f;
    [Tooltip("Should the agent look at the cover point after it has arrived?")]
    public SharedBool lookAtCoverPoint = (SharedBool) false;
    [Tooltip("The agent is done rotating to the cover point when the square magnitude is less than this value")]
    public SharedFloat rotationEpsilon = (SharedFloat) 0.5f;
    [Tooltip("The layermask of the available cover positions")]
    public LayerMask availableLayerCovers;
    [Tooltip("Max rotation delta if lookAtCoverPoint")]
    public SharedFloat maxLookAtRotationDelta;
    private Vector3 coverPoint;
    private Vector3 coverTarget;
    private bool foundCover;

    public override void OnStart()
    {
      int num1 = 0;
      Vector3 vector3 = ((Transform) ((Task) this).transform).get_forward();
      float num2 = 0.0f;
      this.coverTarget = ((Transform) ((Task) this).transform).get_position();
      this.foundCover = false;
      for (; num1 < this.maxRaycasts.get_Value(); ++num1)
      {
        Ray ray;
        ((Ray) ref ray).\u002Ector(((Transform) ((Task) this).transform).get_position(), vector3);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, ref raycastHit, this.maxCoverDistance.get_Value(), ((LayerMask) ref this.availableLayerCovers).get_value()) && ((RaycastHit) ref raycastHit).get_collider().Raycast(new Ray(Vector3.op_Subtraction(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(((RaycastHit) ref raycastHit).get_normal(), this.maxCoverDistance.get_Value())), ((RaycastHit) ref raycastHit).get_normal()), ref raycastHit, float.PositiveInfinity))
        {
          this.coverPoint = ((RaycastHit) ref raycastHit).get_point();
          this.coverTarget = Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(((RaycastHit) ref raycastHit).get_normal(), this.coverOffset.get_Value()));
          this.foundCover = true;
          break;
        }
        num2 += this.rayStep.get_Value();
        vector3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((Transform) ((Task) this).transform).get_eulerAngles().y + num2, 0.0f), Vector3.get_forward());
      }
      if (this.foundCover)
        this.SetDestination(this.coverTarget);
      base.OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (!this.foundCover)
        return (TaskStatus) 1;
      if (this.HasArrived())
      {
        Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(this.coverPoint, ((Transform) ((Task) this).transform).get_position()));
        if (!this.lookAtCoverPoint.get_Value() || (double) Quaternion.Angle(((Transform) ((Task) this).transform).get_rotation(), quaternion) < (double) this.rotationEpsilon.get_Value())
          return (TaskStatus) 2;
        ((Transform) ((Task) this).transform).set_rotation(Quaternion.RotateTowards(((Transform) ((Task) this).transform).get_rotation(), quaternion, this.maxLookAtRotationDelta.get_Value()));
      }
      return (TaskStatus) 3;
    }

    public override void OnReset()
    {
      base.OnStart();
      this.maxCoverDistance = (SharedFloat) 1000f;
      this.maxRaycasts = (SharedInt) 100;
      this.rayStep = (SharedFloat) 1f;
      this.coverOffset = (SharedFloat) 2f;
      this.lookAtCoverPoint = (SharedBool) false;
      this.rotationEpsilon = (SharedFloat) 0.5f;
    }
  }
}
