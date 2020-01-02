// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.CanHearObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Check to see if the any objects are within hearing range of the current agent.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=12")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanHearObjectIcon.png")]
  public class CanHearObject : Conditional
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
    [Tooltip("How far away the unit can hear")]
    public SharedFloat hearingRadius;
    [Tooltip("The further away a sound source is the less likely the agent will be able to hear it. Set a threshold for the the minimum audibility level that the agent can hear")]
    public SharedFloat audibilityThreshold;
    [Tooltip("The hearing offset relative to the pivot position")]
    public SharedVector3 offset;
    [Tooltip("The returned object that is heard")]
    public SharedGameObject returnedObject;

    public CanHearObject()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.targetObjects.get_Value() != null && this.targetObjects.get_Value().Count > 0)
      {
        GameObject gameObject1 = (GameObject) null;
        for (int index = 0; index < this.targetObjects.get_Value().Count; ++index)
        {
          float audibility = 0.0f;
          GameObject gameObject2;
          if ((double) Vector3.Distance(this.targetObjects.get_Value()[index].get_transform().get_position(), ((Transform) ((Task) this).transform).get_position()) < (double) this.hearingRadius.get_Value() && Object.op_Inequality((Object) (gameObject2 = MovementUtility.WithinHearingRange((Transform) ((Task) this).transform, this.offset.get_Value(), this.audibilityThreshold.get_Value(), this.targetObjects.get_Value()[index], ref audibility)), (Object) null))
            gameObject1 = gameObject2;
        }
        this.returnedObject.set_Value(gameObject1);
      }
      else if (Object.op_Equality((Object) this.targetObject.get_Value(), (Object) null))
      {
        if (this.usePhysics2D)
          this.returnedObject.set_Value(MovementUtility.WithinHearingRange2D((Transform) ((Task) this).transform, this.offset.get_Value(), this.audibilityThreshold.get_Value(), this.hearingRadius.get_Value(), this.objectLayerMask));
        else
          this.returnedObject.set_Value(MovementUtility.WithinHearingRange((Transform) ((Task) this).transform, this.offset.get_Value(), this.audibilityThreshold.get_Value(), this.hearingRadius.get_Value(), this.objectLayerMask));
      }
      else if ((double) Vector3.Distance((string.IsNullOrEmpty(this.targetTag.get_Value()) ? this.targetObject.get_Value() : GameObject.FindGameObjectWithTag(this.targetTag.get_Value())).get_transform().get_position(), ((Transform) ((Task) this).transform).get_position()) < (double) this.hearingRadius.get_Value())
        this.returnedObject.set_Value(MovementUtility.WithinHearingRange((Transform) ((Task) this).transform, this.offset.get_Value(), this.audibilityThreshold.get_Value(), this.targetObject.get_Value()));
      return Object.op_Inequality((Object) this.returnedObject.get_Value(), (Object) null) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.hearingRadius = (SharedFloat) 50f;
      this.audibilityThreshold = (SharedFloat) 0.05f;
    }

    public virtual void OnDrawGizmos()
    {
    }

    public virtual void OnBehaviorComplete()
    {
      MovementUtility.ClearCache();
    }
  }
}
