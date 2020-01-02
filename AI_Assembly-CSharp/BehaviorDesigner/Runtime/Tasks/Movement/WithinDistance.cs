// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.WithinDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
  [TaskCategory("Movement")]
  [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=18")]
  [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
  public class WithinDistance : Conditional
  {
    [Tooltip("Should the 2D version be used?")]
    public bool usePhysics2D;
    [Tooltip("The object that we are searching for")]
    public SharedGameObject targetObject;
    [Tooltip("The tag of the object that we are searching for")]
    public SharedString targetTag;
    [Tooltip("The LayerMask of the objects that we are searching for")]
    public LayerMask objectLayerMask;
    [Tooltip("The distance that the object needs to be within")]
    public SharedFloat magnitude;
    [Tooltip("If true, the object must be within line of sight to be within distance. For example, if this option is enabled then an object behind a wall will not be within distance even though it may be physically close to the other object")]
    public SharedBool lineOfSight;
    [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
    public LayerMask ignoreLayerMask;
    [Tooltip("The raycast offset relative to the pivot position")]
    public SharedVector3 offset;
    [Tooltip("The target raycast offset relative to the pivot position")]
    public SharedVector3 targetOffset;
    [Tooltip("The object variable that will be set when a object is found what the object is")]
    public SharedGameObject returnedObject;
    private List<GameObject> objects;
    private float sqrMagnitude;

    public WithinDistance()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      this.sqrMagnitude = this.magnitude.get_Value() * this.magnitude.get_Value();
      if (this.objects != null)
        this.objects.Clear();
      else
        this.objects = new List<GameObject>();
      if (Object.op_Equality((Object) this.targetObject.get_Value(), (Object) null))
      {
        if (!string.IsNullOrEmpty(this.targetTag.get_Value()))
        {
          foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(this.targetTag.get_Value()))
            this.objects.Add(gameObject);
        }
        else
        {
          foreach (Component component in Physics.OverlapSphere(((Transform) ((Task) this).transform).get_position(), this.magnitude.get_Value(), ((LayerMask) ref this.objectLayerMask).get_value()))
            this.objects.Add(component.get_gameObject());
        }
      }
      else
        this.objects.Add(this.targetObject.get_Value());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) ((Task) this).transform, (Object) null) || this.objects == null)
        return (TaskStatus) 1;
      for (int index = 0; index < this.objects.Count; ++index)
      {
        if (!Object.op_Equality((Object) this.objects[index], (Object) null) && (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(this.objects[index].get_transform().get_position(), Vector3.op_Addition(((Transform) ((Task) this).transform).get_position(), this.offset.get_Value()))) < (double) this.sqrMagnitude)
        {
          if (this.lineOfSight.get_Value())
          {
            if (Object.op_Implicit((Object) MovementUtility.LineOfSight((Transform) ((Task) this).transform, this.offset.get_Value(), this.objects[index], this.targetOffset.get_Value(), this.usePhysics2D, ((LayerMask) ref this.ignoreLayerMask).get_value())))
            {
              this.returnedObject.set_Value(this.objects[index]);
              return (TaskStatus) 2;
            }
          }
          else
          {
            this.returnedObject.set_Value(this.objects[index]);
            return (TaskStatus) 2;
          }
        }
      }
      return (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.usePhysics2D = false;
      this.targetObject = (SharedGameObject) null;
      this.targetTag = (SharedString) string.Empty;
      this.objectLayerMask = LayerMask.op_Implicit(0);
      this.magnitude = (SharedFloat) 5f;
      this.lineOfSight = (SharedBool) true;
      this.ignoreLayerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ignore Raycast"));
      this.offset = (SharedVector3) Vector3.get_zero();
      this.targetOffset = (SharedVector3) Vector3.get_zero();
    }

    public virtual void OnDrawGizmos()
    {
    }
  }
}
