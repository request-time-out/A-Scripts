// Decompiled with JetBrains decompiler
// Type: EffectSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class EffectSettings : MonoBehaviour
{
  [Tooltip("Type of the effect")]
  public EffectSettings.EffectTypeEnum EffectType;
  [Tooltip("The radius of the collider is required to correctly calculate the collision point. For example, if the radius 0.5m, then the position of the collision is shifted on 0.5m relative motion vector.")]
  public float ColliderRadius;
  [Tooltip("The radius of the \"Area Of Damage (AOE)\"")]
  public float EffectRadius;
  [Tooltip("Get the position of the movement of the motion vector, and not to follow to the target.")]
  public bool UseMoveVector;
  [Tooltip("A projectile will be moved to the target (any object)")]
  public GameObject Target;
  [Tooltip("Motion vector for the projectile (eg Vector3.Forward)")]
  public Vector3 MoveVector;
  [Tooltip("The speed of the projectile")]
  public float MoveSpeed;
  [Tooltip("Should the projectile have move to the target, until the target not reaches?")]
  public bool IsHomingMove;
  [Tooltip("Distance flight of the projectile, after which the projectile is deactivated and call a collision event with a null value \"RaycastHit\"")]
  public float MoveDistance;
  [Tooltip("Allows you to smoothly activate / deactivate effects which have an indefinite lifetime")]
  public bool IsVisible;
  [Tooltip("Whether to deactivate or destroy the effect after a collision. Deactivation allows you to reuse the effect without instantiating, using \"effect.SetActive (true)\"")]
  public EffectSettings.DeactivationEnum InstanceBehaviour;
  [Tooltip("Delay before deactivating effect. (For example, after effect, some particles must have time to disappear).")]
  public float DeactivateTimeDelay;
  [Tooltip("Delay before deleting effect. (For example, after effect, some particles must have time to disappear).")]
  public float DestroyTimeDelay;
  [Tooltip("Allows you to adjust the layers, which can interact with the projectile.")]
  public LayerMask LayerMask;
  private GameObject[] active_key;
  private float[] active_value;
  private GameObject[] inactive_Key;
  private float[] inactive_value;
  private int lastActiveIndex;
  private int lastInactiveIndex;
  private int currentActiveGo;
  private int currentInactiveGo;
  private bool deactivatedIsWait;

  public EffectSettings()
  {
    base.\u002Ector();
  }

  public event EventHandler<CollisionInfo> CollisionEnter;

  public event EventHandler<EventArgs> EffectDeactivated;

  private void Start()
  {
    if (this.InstanceBehaviour != EffectSettings.DeactivationEnum.DestroyAfterTime)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject(), this.DestroyTimeDelay);
  }

  public void OnCollisionHandler(CollisionInfo e)
  {
    for (int index = 0; index < this.lastActiveIndex; ++index)
      this.Invoke("SetGoActive", this.active_value[index]);
    for (int index = 0; index < this.lastInactiveIndex; ++index)
      this.Invoke("SetGoInactive", this.inactive_value[index]);
    EventHandler<CollisionInfo> collisionEnter = this.CollisionEnter;
    if (collisionEnter != null)
      collisionEnter((object) this, e);
    if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.Deactivate && !this.deactivatedIsWait)
    {
      this.deactivatedIsWait = true;
      this.Invoke("Deactivate", this.DeactivateTimeDelay);
    }
    if (this.InstanceBehaviour != EffectSettings.DeactivationEnum.DestroyAfterCollision)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject(), this.DestroyTimeDelay);
  }

  public void OnEffectDeactivatedHandler()
  {
    EventHandler<EventArgs> effectDeactivated = this.EffectDeactivated;
    if (effectDeactivated == null)
      return;
    effectDeactivated((object) this, EventArgs.Empty);
  }

  public void Deactivate()
  {
    this.OnEffectDeactivatedHandler();
    ((Component) this).get_gameObject().SetActive(false);
  }

  private void SetGoActive()
  {
    this.active_key[this.currentActiveGo].SetActive(false);
    ++this.currentActiveGo;
    if (this.currentActiveGo < this.lastActiveIndex)
      return;
    this.currentActiveGo = 0;
  }

  private void SetGoInactive()
  {
    this.inactive_Key[this.currentInactiveGo].SetActive(true);
    ++this.currentInactiveGo;
    if (this.currentInactiveGo < this.lastInactiveIndex)
      return;
    this.currentInactiveGo = 0;
  }

  public void OnEnable()
  {
    for (int index = 0; index < this.lastActiveIndex; ++index)
      this.active_key[index].SetActive(true);
    for (int index = 0; index < this.lastInactiveIndex; ++index)
      this.inactive_Key[index].SetActive(false);
    this.deactivatedIsWait = false;
  }

  public void OnDisable()
  {
    this.CancelInvoke("SetGoActive");
    this.CancelInvoke("SetGoInactive");
    this.CancelInvoke("Deactivate");
    this.currentActiveGo = 0;
    this.currentInactiveGo = 0;
  }

  public void RegistreActiveElement(GameObject go, float time)
  {
    this.active_key[this.lastActiveIndex] = go;
    this.active_value[this.lastActiveIndex] = time;
    ++this.lastActiveIndex;
  }

  public void RegistreInactiveElement(GameObject go, float time)
  {
    this.inactive_Key[this.lastInactiveIndex] = go;
    this.inactive_value[this.lastInactiveIndex] = time;
    ++this.lastInactiveIndex;
  }

  public enum EffectTypeEnum
  {
    Projectile,
    AOE,
    Other,
  }

  public enum DeactivationEnum
  {
    Deactivate,
    DestroyAfterCollision,
    DestroyAfterTime,
    Nothing,
  }
}
