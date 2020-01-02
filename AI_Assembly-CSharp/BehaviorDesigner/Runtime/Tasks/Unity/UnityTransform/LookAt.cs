// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform.LookAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
  [TaskCategory("Unity/Transform")]
  [TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
  public class LookAt : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The GameObject to look at. If null the world position will be used.")]
    public SharedGameObject targetLookAt;
    [Tooltip("Point to look at")]
    public SharedVector3 worldPosition;
    [Tooltip("Vector specifying the upward direction")]
    public Vector3 worldUp;
    private Transform targetTransform;
    private GameObject prevGameObject;

    public LookAt()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.targetTransform = (Transform) defaultGameObject.GetComponent<Transform>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.targetTransform, (Object) null))
      {
        Debug.LogWarning((object) "Transform is null");
        return (TaskStatus) 1;
      }
      if (Object.op_Inequality((Object) this.targetLookAt.get_Value(), (Object) null))
        this.targetTransform.LookAt(this.targetLookAt.get_Value().get_transform());
      else
        this.targetTransform.LookAt(this.worldPosition.get_Value(), this.worldUp);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.targetLookAt = (SharedGameObject) null;
      this.worldPosition = (SharedVector3) Vector3.get_up();
      this.worldUp = Vector3.get_up();
    }
  }
}
