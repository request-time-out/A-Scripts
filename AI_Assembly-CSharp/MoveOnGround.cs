// Decompiled with JetBrains decompiler
// Type: MoveOnGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class MoveOnGround : MonoBehaviour
{
  public bool IsRootMove;
  private EffectSettings effectSettings;
  private Transform tRoot;
  private Transform tTarget;
  private Vector3 targetPos;
  private bool isInitialized;
  private bool isFinished;
  private ParticleSystem[] particles;

  public MoveOnGround()
  {
    base.\u002Ector();
  }

  public event EventHandler<CollisionInfo> OnCollision;

  private void GetEffectSettingsComponent(Transform tr)
  {
    Transform parent = tr.get_parent();
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    this.effectSettings = (EffectSettings) ((Component) parent).GetComponentInChildren<EffectSettings>();
    if (!Object.op_Equality((Object) this.effectSettings, (Object) null))
      return;
    this.GetEffectSettingsComponent(((Component) parent).get_transform());
  }

  private void Start()
  {
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (Object.op_Equality((Object) this.effectSettings, (Object) null))
      Debug.Log((object) "Prefab root have not script \"PrefabSettings\"");
    this.particles = (ParticleSystem[]) ((Component) this.effectSettings).GetComponentsInChildren<ParticleSystem>();
    this.InitDefaultVariables();
    this.isInitialized = true;
  }

  private void OnEnable()
  {
    if (!this.isInitialized)
      return;
    this.InitDefaultVariables();
  }

  private void InitDefaultVariables()
  {
    foreach (ParticleSystem particle in this.particles)
      particle.Stop();
    this.isFinished = false;
    this.tTarget = this.effectSettings.Target.get_transform();
    if (this.IsRootMove)
    {
      this.tRoot = ((Component) this.effectSettings).get_transform();
    }
    else
    {
      this.tRoot = ((Component) this).get_transform().get_parent();
      this.tRoot.set_localPosition(Vector3.get_zero());
    }
    this.targetPos = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(Vector3.Normalize(Vector3.op_Subtraction(this.tTarget.get_position(), this.tRoot.get_position())), this.effectSettings.MoveDistance));
    RaycastHit raycastHit;
    Physics.Raycast(this.tRoot.get_position(), Vector3.get_down(), ref raycastHit);
    this.tRoot.set_position(((RaycastHit) ref raycastHit).get_point());
    foreach (ParticleSystem particle in this.particles)
      particle.Play();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.tTarget, (Object) null) || this.isFinished)
      return;
    Vector3 position = this.tRoot.get_position();
    RaycastHit raycastHit;
    Physics.Raycast(new Vector3((float) position.x, 0.5f, (float) position.z), Vector3.get_down(), ref raycastHit);
    this.tRoot.set_position(((RaycastHit) ref raycastHit).get_point());
    position = this.tRoot.get_position();
    Vector3 vector3_1 = !this.effectSettings.IsHomingMove ? this.targetPos : this.tTarget.get_position();
    Vector3 vector3_2;
    ((Vector3) ref vector3_2).\u002Ector((float) vector3_1.x, 0.0f, (float) vector3_1.z);
    if ((double) Vector3.Distance(new Vector3((float) position.x, 0.0f, (float) position.z), vector3_2) <= (double) this.effectSettings.ColliderRadius)
    {
      this.effectSettings.OnCollisionHandler(new CollisionInfo());
      this.isFinished = true;
    }
    this.tRoot.set_position(Vector3.MoveTowards(position, vector3_2, this.effectSettings.MoveSpeed * Time.get_deltaTime()));
  }
}
