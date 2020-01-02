// Decompiled with JetBrains decompiler
// Type: Exploder.Fragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder
{
  public class Fragment : MonoBehaviour
  {
    [NonSerialized]
    public bool Cracked;
    [NonSerialized]
    public bool Visible;
    [NonSerialized]
    public bool IsActive;
    [NonSerialized]
    public MeshRenderer meshRenderer;
    [NonSerialized]
    public MeshCollider meshCollider;
    [NonSerialized]
    public MeshFilter meshFilter;
    [NonSerialized]
    public BoxCollider boxCollider;
    [NonSerialized]
    public PolygonCollider2D polygonCollider2D;
    [NonSerialized]
    public AudioSource audioSource;
    private ParticleSystem[] particleSystems;
    private GameObject particleChild;
    private Rigidbody2D rigid2D;
    private Rigidbody rigidBody;
    private ExploderParams settings;
    private Vector3 originalScale;
    private float visibilityCheckTimer;
    private float deactivateTimer;
    private float emmitersTimeout;
    private bool stopEmitOnTimeout;
    private bool collided;
    private static AudioSource activePlayback;

    public Fragment()
    {
      base.\u002Ector();
    }

    public bool IsSleeping()
    {
      return Object.op_Implicit((Object) this.rigid2D) ? this.rigid2D.IsSleeping() : this.rigidBody.IsSleeping();
    }

    public void Sleep()
    {
      if (Object.op_Implicit((Object) this.rigid2D))
        this.rigid2D.Sleep();
      else
        this.rigidBody.Sleep();
    }

    public void WakeUp()
    {
      if (Object.op_Implicit((Object) this.rigid2D))
        this.rigid2D.WakeUp();
      else
        this.rigidBody.WakeUp();
    }

    public void SetConstraints(RigidbodyConstraints constraints)
    {
      if (!Object.op_Implicit((Object) this.rigidBody))
        return;
      this.rigidBody.set_constraints(constraints);
    }

    public void InitSFX(FragmentSFX sfx)
    {
      if (Object.op_Implicit((Object) sfx.FragmentEmitter))
      {
        if (!Object.op_Implicit((Object) this.particleChild))
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) sfx.FragmentEmitter);
          if (Object.op_Implicit((Object) gameObject))
          {
            gameObject.get_transform().set_position(Vector3.get_zero());
            this.particleChild = new GameObject("Particles");
            this.particleChild.get_transform().set_parent(((Component) this).get_gameObject().get_transform());
            gameObject.get_transform().set_parent(this.particleChild.get_transform());
          }
        }
        if (Object.op_Implicit((Object) this.particleChild))
          this.particleSystems = (ParticleSystem[]) this.particleChild.GetComponentsInChildren<ParticleSystem>();
        this.emmitersTimeout = sfx.ParticleTimeout;
        this.stopEmitOnTimeout = (double) this.emmitersTimeout > 0.0;
      }
      else
      {
        if (!Object.op_Implicit((Object) this.particleChild))
          return;
        Object.Destroy((Object) this.particleChild);
        sfx.ParticleTimeout = -1f;
        this.stopEmitOnTimeout = false;
      }
    }

    private void OnCollisionEnter()
    {
      if (!this.settings.FragmentSFX.MixMultipleSounds && Object.op_Implicit((Object) Fragment.activePlayback) && Fragment.activePlayback.get_isPlaying())
        return;
      bool flag1 = !this.settings.FragmentSFX.PlayOnlyOnce || !this.collided;
      bool flag2 = Random.Range(0, 100) <= this.settings.FragmentSFX.ChanceToPlay;
      this.collided = true;
      if (!flag1 || !flag2 || !Object.op_Implicit((Object) this.audioSource))
        return;
      this.audioSource.Play();
      Fragment.activePlayback = this.audioSource;
    }

    public void DisableColliders(bool disable, bool meshColliders, bool physics2d)
    {
      if (disable)
      {
        if (physics2d)
        {
          Object.Destroy((Object) this.polygonCollider2D);
        }
        else
        {
          if (Object.op_Implicit((Object) this.meshCollider))
            Object.Destroy((Object) this.meshCollider);
          if (!Object.op_Implicit((Object) this.boxCollider))
            return;
          Object.Destroy((Object) this.boxCollider);
        }
      }
      else if (physics2d)
      {
        if (Object.op_Implicit((Object) this.polygonCollider2D))
          return;
        this.polygonCollider2D = (PolygonCollider2D) ((Component) this).get_gameObject().AddComponent<PolygonCollider2D>();
      }
      else if (meshColliders)
      {
        if (Object.op_Implicit((Object) this.meshCollider))
          return;
        this.meshCollider = (MeshCollider) ((Component) this).get_gameObject().AddComponent<MeshCollider>();
      }
      else
      {
        if (Object.op_Implicit((Object) this.boxCollider))
          return;
        this.boxCollider = (BoxCollider) ((Component) this).get_gameObject().AddComponent<BoxCollider>();
      }
    }

    public void ApplyExplosion(
      ExploderTransform meshTransform,
      Vector3 centroid,
      float force,
      GameObject original,
      ExploderParams set)
    {
      this.settings = set;
      if (Object.op_Implicit((Object) this.rigid2D))
      {
        this.ApplyExplosion2D(meshTransform, centroid, force, original);
      }
      else
      {
        Rigidbody rigidBody = this.rigidBody;
        Vector3 vector3_1 = Vector3.get_zero();
        Vector3 vector3_2 = Vector3.get_zero();
        float num = this.settings.FragmentOptions.Mass;
        bool useGravity = this.settings.FragmentOptions.UseGravity;
        rigidBody.set_maxAngularVelocity(this.settings.FragmentOptions.MaxAngularVelocity);
        if (this.settings.FragmentOptions.InheritParentPhysicsProperty && Object.op_Implicit((Object) original) && Object.op_Implicit((Object) original.GetComponent<Rigidbody>()))
        {
          Rigidbody component = (Rigidbody) original.GetComponent<Rigidbody>();
          vector3_1 = component.get_velocity();
          vector3_2 = component.get_angularVelocity();
          num = component.get_mass() / (float) this.settings.TargetFragments;
          useGravity = component.get_useGravity();
        }
        Vector3 vector3_3 = Vector3.op_Subtraction(meshTransform.TransformPoint(centroid), this.settings.Position);
        Vector3 vector3_4 = ((Vector3) ref vector3_3).get_normalized();
        Vector3 vector3_5 = Vector3.op_Multiply(this.settings.FragmentOptions.AngularVelocity, !this.settings.FragmentOptions.RandomAngularVelocityVector ? this.settings.FragmentOptions.AngularVelocityVector : Random.get_onUnitSphere());
        if (this.settings.UseForceVector)
          vector3_4 = this.settings.ForceVector;
        rigidBody.set_velocity(Vector3.op_Addition(Vector3.op_Multiply(vector3_4, force), vector3_1));
        rigidBody.set_angularVelocity(Vector3.op_Addition(vector3_5, vector3_2));
        rigidBody.set_mass(num);
        rigidBody.set_useGravity(useGravity);
      }
    }

    private void ApplyExplosion2D(
      ExploderTransform meshTransform,
      Vector3 centroid,
      float force,
      GameObject original)
    {
      Rigidbody2D rigid2D = this.rigid2D;
      Vector2 vector2_1 = Vector2.get_zero();
      float num1 = 0.0f;
      float num2 = this.settings.FragmentOptions.Mass;
      if (this.settings.FragmentOptions.InheritParentPhysicsProperty && Object.op_Implicit((Object) original) && Object.op_Implicit((Object) original.GetComponent<Rigidbody2D>()))
      {
        Rigidbody2D component = (Rigidbody2D) original.GetComponent<Rigidbody2D>();
        vector2_1 = component.get_velocity();
        num1 = component.get_angularVelocity();
        num2 = component.get_mass() / (float) this.settings.TargetFragments;
      }
      Vector3 vector3 = Vector3.op_Subtraction(meshTransform.TransformPoint(centroid), this.settings.Position);
      Vector2 vector2_2 = Vector2.op_Implicit(((Vector3) ref vector3).get_normalized());
      float num3 = this.settings.FragmentOptions.AngularVelocity * (!this.settings.FragmentOptions.RandomAngularVelocityVector ? (float) this.settings.FragmentOptions.AngularVelocityVector.y : (float) Random.get_insideUnitCircle().x);
      if (this.settings.UseForceVector)
        vector2_2 = Vector2.op_Implicit(this.settings.ForceVector);
      rigid2D.set_velocity(Vector2.op_Addition(Vector2.op_Multiply(vector2_2, force), vector2_1));
      rigid2D.set_angularVelocity(num3 + num1);
      rigid2D.set_mass(num2);
    }

    public void RefreshComponentsCache()
    {
      this.audioSource = (AudioSource) ((Component) this).get_gameObject().GetComponent<AudioSource>();
      this.meshFilter = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
      this.meshRenderer = (MeshRenderer) ((Component) this).GetComponent<MeshRenderer>();
      this.meshCollider = (MeshCollider) ((Component) this).GetComponent<MeshCollider>();
      this.boxCollider = (BoxCollider) ((Component) this).GetComponent<BoxCollider>();
      this.rigidBody = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
      this.rigid2D = (Rigidbody2D) ((Component) this).GetComponent<Rigidbody2D>();
      this.polygonCollider2D = (PolygonCollider2D) ((Component) this).GetComponent<PolygonCollider2D>();
    }

    public void Explode(ExploderParams parameters)
    {
      this.settings = parameters;
      this.IsActive = true;
      ExploderUtils.SetActiveRecursively(((Component) this).get_gameObject(), true);
      this.visibilityCheckTimer = 0.1f;
      this.Visible = true;
      this.Cracked = false;
      this.collided = false;
      this.deactivateTimer = this.settings.FragmentDeactivation.DeactivateTimeout;
      this.originalScale = ((Component) this).get_transform().get_localScale();
      if (this.settings.FragmentOptions.ExplodeFragments)
        ((Component) this).set_tag(ExploderObject.Tag);
      this.Emit(true);
    }

    public void Emit(bool centerToBound)
    {
      if (this.particleSystems == null)
        return;
      if (centerToBound && Object.op_Implicit((Object) this.particleChild) && Object.op_Implicit((Object) this.meshRenderer))
      {
        Transform transform = this.particleChild.get_transform();
        Bounds bounds = ((Renderer) this.meshRenderer).get_bounds();
        Vector3 center = ((Bounds) ref bounds).get_center();
        transform.set_position(center);
      }
      foreach (ParticleSystem particleSystem in this.particleSystems)
      {
        particleSystem.Clear();
        particleSystem.Play();
      }
    }

    public void Deactivate()
    {
      if (Object.op_Equality((Object) Fragment.activePlayback, (Object) this.audioSource))
        Fragment.activePlayback = (AudioSource) null;
      this.Sleep();
      ExploderUtils.SetActiveRecursively(((Component) this).get_gameObject(), false);
      this.Visible = false;
      this.IsActive = false;
      if (Object.op_Implicit((Object) this.meshFilter) && Object.op_Implicit((Object) this.meshFilter.get_sharedMesh()))
        Object.DestroyImmediate((Object) this.meshFilter.get_sharedMesh(), true);
      if (this.particleSystems == null)
        return;
      foreach (ParticleSystem particleSystem in this.particleSystems)
        particleSystem.Clear();
    }

    public void AssignMesh(Mesh mesh)
    {
      if (Object.op_Implicit((Object) this.meshFilter.get_sharedMesh()))
        Object.DestroyImmediate((Object) this.meshFilter.get_sharedMesh(), true);
      this.meshFilter.set_sharedMesh(mesh);
    }

    private void Start()
    {
      this.visibilityCheckTimer = 1f;
      this.RefreshComponentsCache();
      this.Visible = false;
    }

    private void Update()
    {
      if (!this.IsActive)
        return;
      float maxVelocity = this.settings.FragmentOptions.MaxVelocity;
      if (Object.op_Implicit((Object) this.rigidBody))
      {
        Vector3 velocity1 = this.rigidBody.get_velocity();
        if ((double) ((Vector3) ref velocity1).get_sqrMagnitude() > (double) maxVelocity * (double) maxVelocity)
        {
          Vector3 velocity2 = this.rigidBody.get_velocity();
          this.rigidBody.set_velocity(Vector3.op_Multiply(((Vector3) ref velocity2).get_normalized(), maxVelocity));
        }
      }
      else if (Object.op_Implicit((Object) this.rigid2D))
      {
        Vector2 velocity1 = this.rigid2D.get_velocity();
        if ((double) ((Vector2) ref velocity1).get_sqrMagnitude() > (double) maxVelocity * (double) maxVelocity)
        {
          Vector2 velocity2 = this.rigid2D.get_velocity();
          this.rigid2D.set_velocity(Vector2.op_Multiply(((Vector2) ref velocity2).get_normalized(), maxVelocity));
        }
      }
      if (this.settings.FragmentDeactivation.DeactivateOptions == DeactivateOptions.Timeout)
      {
        this.deactivateTimer -= Time.get_deltaTime();
        if ((double) this.deactivateTimer < 0.0)
        {
          this.Deactivate();
          if (this.settings.FragmentDeactivation.FadeoutOptions == FadeoutOptions.FadeoutAlpha)
            ;
        }
        else
        {
          float num = this.deactivateTimer / this.settings.FragmentDeactivation.DeactivateTimeout;
          switch (this.settings.FragmentDeactivation.FadeoutOptions)
          {
            case FadeoutOptions.FadeoutAlpha:
              if (Object.op_Implicit((Object) ((Renderer) this.meshRenderer).get_material()) && ((Renderer) this.meshRenderer).get_material().HasProperty("_Color"))
              {
                Color color = ((Renderer) this.meshRenderer).get_material().get_color();
                color.a = (__Null) (double) num;
                ((Renderer) this.meshRenderer).get_material().set_color(color);
                break;
              }
              break;
            case FadeoutOptions.ScaleDown:
              ((Component) this).get_gameObject().get_transform().set_localScale(Vector3.op_Multiply(this.originalScale, num));
              break;
          }
        }
      }
      if (this.stopEmitOnTimeout)
      {
        this.emmitersTimeout -= Time.get_deltaTime();
        if ((double) this.emmitersTimeout < 0.0)
        {
          if (Object.op_Inequality((Object) this.particleChild, (Object) null))
          {
            ParticleSystem componentInChildren = (ParticleSystem) this.particleChild.GetComponentInChildren<ParticleSystem>();
            if (Object.op_Implicit((Object) componentInChildren))
              componentInChildren.Stop();
          }
          this.stopEmitOnTimeout = false;
        }
      }
      this.visibilityCheckTimer -= Time.get_deltaTime();
      if ((double) this.visibilityCheckTimer >= 0.0 || !Object.op_Implicit((Object) Camera.get_main()))
        return;
      Vector3 viewportPoint = Camera.get_main().WorldToViewportPoint(((Component) this).get_transform().get_position());
      if (viewportPoint.z < 0.0 || viewportPoint.x < 0.0 || (viewportPoint.y < 0.0 || viewportPoint.x > 1.0) || viewportPoint.y > 1.0)
      {
        if (this.settings.FragmentDeactivation.DeactivateOptions == DeactivateOptions.OutsideOfCamera)
          this.Deactivate();
        this.Visible = false;
      }
      else
        this.Visible = true;
      this.visibilityCheckTimer = Random.Range(0.1f, 0.3f);
    }
  }
}
