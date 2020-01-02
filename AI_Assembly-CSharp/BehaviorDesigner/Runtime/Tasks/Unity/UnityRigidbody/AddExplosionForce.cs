// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody.AddExplosionForce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody
{
  [TaskCategory("Unity/Rigidbody")]
  [TaskDescription("Applies a force to the rigidbody that simulates explosion effects. Returns Success.")]
  public class AddExplosionForce : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The force of the explosion")]
    public SharedFloat explosionForce;
    [Tooltip("The position of the explosion")]
    public SharedVector3 explosionPosition;
    [Tooltip("The radius of the explosion")]
    public SharedFloat explosionRadius;
    [Tooltip("Applies the force as if it was applied from beneath the object")]
    public float upwardsModifier;
    [Tooltip("The type of force")]
    public ForceMode forceMode;
    private Rigidbody rigidbody;
    private GameObject prevGameObject;

    public AddExplosionForce()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.rigidbody = (Rigidbody) defaultGameObject.GetComponent<Rigidbody>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.rigidbody, (Object) null))
      {
        Debug.LogWarning((object) "Rigidbody is null");
        return (TaskStatus) 1;
      }
      this.rigidbody.AddExplosionForce(this.explosionForce.get_Value(), this.explosionPosition.get_Value(), this.explosionRadius.get_Value(), this.upwardsModifier, this.forceMode);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.explosionForce = (SharedFloat) 0.0f;
      this.explosionPosition = (SharedVector3) Vector3.get_zero();
      this.explosionRadius = (SharedFloat) 0.0f;
      this.upwardsModifier = 0.0f;
      this.forceMode = (ForceMode) 0;
    }
  }
}
