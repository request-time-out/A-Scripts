// Decompiled with JetBrains decompiler
// Type: LineProjectileCollisionBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class LineProjectileCollisionBehaviour : MonoBehaviour
{
  public GameObject EffectOnHit;
  public GameObject EffectOnHitObject;
  public GameObject ParticlesScale;
  public GameObject GoLight;
  public bool IsCenterLightPosition;
  public LineRenderer[] LineRenderers;
  private EffectSettings effectSettings;
  private Transform t;
  private Transform tLight;
  private Transform tEffectOnHit;
  private Transform tParticleScale;
  private RaycastHit hit;
  private RaycastHit oldRaycastHit;
  private bool isInitializedOnStart;
  private bool frameDroped;
  private ParticleSystem[] effectOnHitParticles;
  private EffectSettings effectSettingsInstance;

  public LineProjectileCollisionBehaviour()
  {
    base.\u002Ector();
  }

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
    this.t = ((Component) this).get_transform();
    if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
    {
      this.tEffectOnHit = this.EffectOnHit.get_transform();
      this.effectOnHitParticles = (ParticleSystem[]) this.EffectOnHit.GetComponentsInChildren<ParticleSystem>();
    }
    if (Object.op_Inequality((Object) this.ParticlesScale, (Object) null))
      this.tParticleScale = this.ParticlesScale.get_transform();
    this.GetEffectSettingsComponent(this.t);
    if (Object.op_Equality((Object) this.effectSettings, (Object) null))
      Debug.Log((object) "Prefab root or children have not script \"PrefabSettings\"");
    if (Object.op_Inequality((Object) this.GoLight, (Object) null))
      this.tLight = this.GoLight.get_transform();
    this.InitializeDefault();
    this.isInitializedOnStart = true;
  }

  private void OnEnable()
  {
    if (!this.isInitializedOnStart)
      return;
    this.InitializeDefault();
  }

  private void OnDisable()
  {
    this.CollisionLeave();
  }

  private void InitializeDefault()
  {
    this.hit = (RaycastHit) null;
    this.frameDroped = false;
  }

  private void Update()
  {
    if (!this.frameDroped)
    {
      this.frameDroped = true;
    }
    else
    {
      Vector3 vector3 = Vector3.op_Addition(this.t.get_position(), Vector3.op_Multiply(this.t.get_forward(), this.effectSettings.MoveDistance));
      RaycastHit raycastHit;
      if (Physics.Raycast(this.t.get_position(), this.t.get_forward(), ref raycastHit, this.effectSettings.MoveDistance + 1f, LayerMask.op_Implicit(this.effectSettings.LayerMask)))
      {
        this.hit = raycastHit;
        vector3 = ((RaycastHit) ref raycastHit).get_point();
        if (Object.op_Inequality((Object) ((RaycastHit) ref this.oldRaycastHit).get_collider(), (Object) ((RaycastHit) ref this.hit).get_collider()))
        {
          this.CollisionLeave();
          this.oldRaycastHit = this.hit;
          this.CollisionEnter();
          if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
          {
            foreach (ParticleSystem effectOnHitParticle in this.effectOnHitParticles)
              effectOnHitParticle.Play();
          }
        }
        if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
          this.tEffectOnHit.set_position(Vector3.op_Subtraction(((RaycastHit) ref this.hit).get_point(), Vector3.op_Multiply(this.t.get_forward(), this.effectSettings.ColliderRadius)));
      }
      else if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
      {
        foreach (ParticleSystem effectOnHitParticle in this.effectOnHitParticles)
          effectOnHitParticle.Stop();
      }
      if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
        this.tEffectOnHit.LookAt(Vector3.op_Addition(((RaycastHit) ref this.hit).get_point(), ((RaycastHit) ref this.hit).get_normal()));
      if (this.IsCenterLightPosition && Object.op_Inequality((Object) this.GoLight, (Object) null))
        this.tLight.set_position(Vector3.op_Division(Vector3.op_Addition(this.t.get_position(), vector3), 2f));
      foreach (LineRenderer lineRenderer in this.LineRenderers)
      {
        lineRenderer.SetPosition(0, vector3);
        lineRenderer.SetPosition(1, this.t.get_position());
      }
      if (!Object.op_Inequality((Object) this.ParticlesScale, (Object) null))
        return;
      this.tParticleScale.set_localScale(new Vector3(Vector3.Distance(this.t.get_position(), vector3) / 2f, 1f, 1f));
    }
  }

  private void CollisionEnter()
  {
    if (Object.op_Inequality((Object) this.EffectOnHitObject, (Object) null) && Object.op_Inequality((Object) ((RaycastHit) ref this.hit).get_transform(), (Object) null))
    {
      AddMaterialOnHit componentInChildren1 = (AddMaterialOnHit) ((Component) ((RaycastHit) ref this.hit).get_transform()).GetComponentInChildren<AddMaterialOnHit>();
      this.effectSettingsInstance = (EffectSettings) null;
      if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
        this.effectSettingsInstance = (EffectSettings) ((Component) componentInChildren1).get_gameObject().GetComponent<EffectSettings>();
      if (Object.op_Inequality((Object) this.effectSettingsInstance, (Object) null))
      {
        this.effectSettingsInstance.IsVisible = true;
      }
      else
      {
        Renderer componentInChildren2 = (Renderer) ((Component) ((RaycastHit) ref this.hit).get_transform()).GetComponentInChildren<Renderer>();
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.EffectOnHitObject);
        gameObject.get_transform().set_parent(((Component) componentInChildren2).get_transform());
        gameObject.get_transform().set_localPosition(Vector3.get_zero());
        ((AddMaterialOnHit) gameObject.GetComponent<AddMaterialOnHit>()).UpdateMaterial(this.hit);
        this.effectSettingsInstance = (EffectSettings) gameObject.GetComponent<EffectSettings>();
      }
    }
    this.effectSettings.OnCollisionHandler(new CollisionInfo()
    {
      Hit = this.hit
    });
  }

  private void CollisionLeave()
  {
    if (!Object.op_Inequality((Object) this.effectSettingsInstance, (Object) null))
      return;
    this.effectSettingsInstance.IsVisible = false;
  }
}
