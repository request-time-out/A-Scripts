// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.CanSeeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Check to see if the any objects are within sight of the agent.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=11")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
  public class CanSeeObject : Conditional
  {
    [Tooltip("Should the 2D version be used?")]
    public bool usePhysics2D;
    [Tooltip("The object that we are searching for")]
    public SharedGameObject targetObject;
    [Tooltip("The objects that we are searching for")]
    public SharedGameObjectList targetObjects;
    [Tooltip("The tag of the object that we are searching for")]
    public SharedString targetTag;
    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask objectLayerMask;
    [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
    public LayerMask ignoreLayerMask;
    [Tooltip("The field of view angle of the agent (in degrees)")]
    public SharedFloat fieldOfViewAngle;
    [Tooltip("The distance that the agent can see")]
    public SharedFloat viewDistance;
    [Tooltip("The raycast offset relative to the pivot position")]
    public SharedVector3 offset;
    [Tooltip("The target raycast offset relative to the pivot position")]
    public SharedVector3 targetOffset;
    [Tooltip("The offset to apply to 2D angles")]
    public SharedFloat angleOffset2D;
    [Tooltip("Should the target bone be used?")]
    public SharedBool useTargetBone;
    [Tooltip("The target's bone if the target is a humanoid")]
    public HumanBodyBones targetBone;
    [Tooltip("The object that is within sight")]
    public SharedGameObject returnedObject;

    public CanSeeObject()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.usePhysics2D)
      {
        if (this.targetObjects.get_Value() != null && this.targetObjects.get_Value().Count > 0)
        {
          GameObject gameObject1 = (GameObject) null;
          float num = float.PositiveInfinity;
          for (int index = 0; index < this.targetObjects.get_Value().Count; ++index)
          {
            float angle;
            GameObject gameObject2;
            if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.targetObjects.get_Value()[index], this.targetOffset.get_Value(), true, this.angleOffset2D.get_Value(), out angle, LayerMask.op_Implicit(this.ignoreLayerMask), this.useTargetBone.get_Value(), this.targetBone)), (Object) null) && (double) angle < (double) num)
            {
              num = angle;
              gameObject1 = gameObject2;
            }
          }
          this.returnedObject.set_Value(gameObject1);
        }
        else if (Object.op_Equality((Object) this.targetObject.get_Value(), (Object) null))
          this.returnedObject.set_Value(MovementUtility.WithinSight2D((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.objectLayerMask, this.targetOffset.get_Value(), this.angleOffset2D.get_Value(), this.ignoreLayerMask));
        else if (!string.IsNullOrEmpty(this.targetTag.get_Value()))
          this.returnedObject.set_Value(MovementUtility.WithinSight2D((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), GameObject.FindGameObjectWithTag(this.targetTag.get_Value()), this.targetOffset.get_Value(), this.angleOffset2D.get_Value(), this.ignoreLayerMask, this.useTargetBone.get_Value(), this.targetBone));
        else
          this.returnedObject.set_Value(MovementUtility.WithinSight2D((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.targetObject.get_Value(), this.targetOffset.get_Value(), this.angleOffset2D.get_Value(), this.ignoreLayerMask, this.useTargetBone.get_Value(), this.targetBone));
      }
      else if (this.targetObjects.get_Value() != null && this.targetObjects.get_Value().Count > 0)
      {
        GameObject gameObject1 = (GameObject) null;
        float num = float.PositiveInfinity;
        for (int index = 0; index < this.targetObjects.get_Value().Count; ++index)
        {
          float angle;
          GameObject gameObject2;
          if (Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.targetObjects.get_Value()[index], this.targetOffset.get_Value(), false, this.angleOffset2D.get_Value(), out angle, LayerMask.op_Implicit(this.ignoreLayerMask), this.useTargetBone.get_Value(), this.targetBone)), (Object) null) && (double) angle < (double) num)
          {
            num = angle;
            gameObject1 = gameObject2;
          }
        }
        this.returnedObject.set_Value(gameObject1);
      }
      else if (Object.op_Equality((Object) this.targetObject.get_Value(), (Object) null))
        this.returnedObject.set_Value(MovementUtility.WithinSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.objectLayerMask, this.targetOffset.get_Value(), this.ignoreLayerMask, this.useTargetBone.get_Value(), this.targetBone));
      else if (!string.IsNullOrEmpty(this.targetTag.get_Value()))
        this.returnedObject.set_Value(MovementUtility.WithinSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), GameObject.FindGameObjectWithTag(this.targetTag.get_Value()), this.targetOffset.get_Value(), this.ignoreLayerMask, this.useTargetBone.get_Value(), this.targetBone));
      else
        this.returnedObject.set_Value(MovementUtility.WithinSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.viewDistance.get_Value(), this.targetObject.get_Value(), this.targetOffset.get_Value(), this.ignoreLayerMask, this.useTargetBone.get_Value(), this.targetBone));
      return Object.op_Inequality((Object) this.returnedObject.get_Value(), (Object) null) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.fieldOfViewAngle = (SharedFloat) 90f;
      this.viewDistance = (SharedFloat) 1000f;
      this.offset = (SharedVector3) Vector3.get_zero();
      this.targetOffset = (SharedVector3) Vector3.get_zero();
      this.angleOffset2D = (SharedFloat) 0.0f;
      this.targetTag = (SharedString) string.Empty;
    }

    public virtual void OnDrawGizmos()
    {
      MovementUtility.DrawLineOfSight(((Component) ((Task) this).get_Owner()).get_transform(), this.offset.get_Value(), this.fieldOfViewAngle.get_Value(), this.angleOffset2D.get_Value(), this.viewDistance.get_Value(), this.usePhysics2D);
    }

    public virtual void OnBehaviorComplete()
    {
      MovementUtility.ClearCache();
    }
  }
}
