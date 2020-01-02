// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.Instantiate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Instantiates a new GameObject. Returns Success.")]
  public class Instantiate : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The position of the new GameObject")]
    public SharedVector3 position;
    [Tooltip("The rotation of the new GameObject")]
    public SharedQuaternion rotation;
    [SharedRequired]
    [Tooltip("The instantiated GameObject")]
    public SharedGameObject storeResult;

    public Instantiate()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value((GameObject) Object.Instantiate<GameObject>((M0) this.targetGameObject.get_Value(), this.position.get_Value(), this.rotation.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.position = (SharedVector3) Vector3.get_zero();
      this.rotation = (SharedQuaternion) Quaternion.get_identity();
    }
  }
}
