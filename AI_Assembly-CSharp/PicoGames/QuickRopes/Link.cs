// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.Link
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [Serializable]
  public class Link
  {
    [SerializeField]
    public bool alternateJoints = true;
    [SerializeField]
    private int linkIndex = -1;
    [SerializeField]
    public bool overridePrefab;
    [SerializeField]
    public bool overrideOffsetSettings;
    [SerializeField]
    public bool overrideRigidbodySettings;
    [SerializeField]
    public bool overrideJointSettings;
    [SerializeField]
    public bool overrideColliderSettings;
    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public TransformSettings offsetSettings;
    [SerializeField]
    public RigidbodySettings rigidbodySettings;
    [SerializeField]
    public JointSettings jointSettings;
    [SerializeField]
    public ColliderSettings colliderSettings;
    [SerializeField]
    public GameObject gameObject;
    [SerializeField]
    public Collider collider;
    [SerializeField]
    private GameObject attachedPrefab;
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private ConfigurableJoint joint;
    [SerializeField]
    private bool prevIsKinematic;

    public Link(GameObject _gameObject, int _index)
    {
      this.gameObject = _gameObject;
      this.linkIndex = _index;
      this.IsActive = false;
    }

    [SerializeField]
    public Transform transform
    {
      get
      {
        return this.gameObject.get_transform();
      }
    }

    public GameObject Prefab
    {
      get
      {
        return this.prefab;
      }
      set
      {
        this.prefab = value;
      }
    }

    public Transform Parent
    {
      get
      {
        return this.transform.get_parent();
      }
      set
      {
        this.transform.set_parent(value);
      }
    }

    public Rigidbody ConnectedBody
    {
      get
      {
        return Object.op_Equality((Object) this.joint, (Object) null) ? (Rigidbody) null : ((UnityEngine.Joint) this.joint).get_connectedBody();
      }
      set
      {
        if (Object.op_Equality((Object) this.joint, (Object) null))
          return;
        ((UnityEngine.Joint) this.joint).set_connectedBody(value);
      }
    }

    public bool IsActive
    {
      get
      {
        return !Object.op_Equality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
      }
      set
      {
        if (Object.op_Equality((Object) this.gameObject, (Object) null))
          return;
        ((Object) this.gameObject).set_hideFlags(!value ? (HideFlags) 1 : (HideFlags) 0);
        this.gameObject.SetActive(value);
        this.IsPrefabActive = true;
      }
    }

    public bool IsPrefabActive
    {
      get
      {
        return Object.op_Inequality((Object) this.attachedPrefab, (Object) null) && this.attachedPrefab.get_activeSelf();
      }
      set
      {
        if (Object.op_Equality((Object) this.attachedPrefab, (Object) null))
          return;
        this.attachedPrefab.SetActive(value);
      }
    }

    public Rigidbody Rigidbody
    {
      get
      {
        return this.rigidbody;
      }
    }

    public ConfigurableJoint Joint
    {
      get
      {
        return this.joint;
      }
    }

    public bool TogglePhysicsEnabled(bool _enabled)
    {
      if (_enabled)
      {
        this.Rigidbody.set_isKinematic(this.prevIsKinematic);
      }
      else
      {
        this.prevIsKinematic = this.Rigidbody.get_isKinematic();
        this.Rigidbody.set_isKinematic(true);
      }
      return _enabled;
    }

    public void ApplyPrefabSettings()
    {
      if (this.transform.get_childCount() > 0)
      {
        if (!Application.get_isPlaying())
        {
          while (this.transform.get_childCount() > 0)
            Object.DestroyImmediate((Object) ((Component) this.transform.GetChild(0)).get_gameObject(), false);
        }
        else
        {
          for (int index = 0; index < this.transform.get_childCount(); ++index)
            Object.Destroy((Object) ((Component) this.transform.GetChild(index)).get_gameObject());
        }
      }
      if (!Object.op_Inequality((Object) this.prefab, (Object) null))
        return;
      this.attachedPrefab = !this.prefab.get_activeInHierarchy() ? (GameObject) Object.Instantiate<GameObject>((M0) this.prefab) : this.prefab;
      ((Object) this.attachedPrefab).set_name(((Object) this.prefab).get_name());
      this.attachedPrefab.get_transform().set_parent(this.transform);
      this.attachedPrefab.get_transform().set_localPosition(this.offsetSettings.position);
      this.attachedPrefab.get_transform().set_localRotation(Quaternion.op_Multiply(this.offsetSettings.rotation, this.alternateJoints ? Quaternion.AngleAxis(this.linkIndex % 2 != 0 ? 0.0f : 90f, Vector3.get_up()) : Quaternion.get_identity()));
      this.attachedPrefab.get_transform().set_localScale(this.offsetSettings.scale);
    }

    public void ApplyColliderSettings()
    {
      Collider[] components = (Collider[]) this.gameObject.GetComponents<Collider>();
      for (int index = 0; index < components.Length; ++index)
      {
        if (Application.get_isPlaying())
          Object.Destroy((Object) components[index]);
        else
          Object.DestroyImmediate((Object) components[index]);
      }
      switch (this.colliderSettings.type)
      {
        case QuickRope.ColliderType.None:
          this.collider = (Collider) null;
          break;
        case QuickRope.ColliderType.Sphere:
          this.collider = (Collider) this.gameObject.AddComponent<SphereCollider>();
          SphereCollider collider1 = this.collider as SphereCollider;
          ((Collider) collider1).set_material(this.colliderSettings.physicsMaterial);
          collider1.set_radius(this.colliderSettings.radius);
          collider1.set_center(this.colliderSettings.center);
          break;
        case QuickRope.ColliderType.Box:
          this.collider = (Collider) this.gameObject.AddComponent<BoxCollider>();
          BoxCollider collider2 = this.collider as BoxCollider;
          ((Collider) collider2).set_material(this.colliderSettings.physicsMaterial);
          collider2.set_size(this.colliderSettings.size);
          collider2.set_center(this.colliderSettings.center);
          break;
        case QuickRope.ColliderType.Capsule:
          this.collider = (Collider) this.gameObject.AddComponent<CapsuleCollider>();
          CapsuleCollider collider3 = this.collider as CapsuleCollider;
          ((Collider) collider3).set_material(this.colliderSettings.physicsMaterial);
          collider3.set_radius(this.colliderSettings.radius);
          collider3.set_height(this.colliderSettings.height);
          collider3.set_direction((int) this.colliderSettings.direction);
          collider3.set_center(this.colliderSettings.center);
          break;
      }
    }

    public void ApplyRigidbodySettings()
    {
      if (Object.op_Equality((Object) this.rigidbody, (Object) null))
      {
        this.rigidbody = (Rigidbody) this.gameObject.GetComponent<Rigidbody>();
        if (Object.op_Equality((Object) this.rigidbody, (Object) null))
          this.rigidbody = (Rigidbody) this.gameObject.AddComponent<Rigidbody>();
      }
      this.prevIsKinematic = this.rigidbodySettings.isKinematic;
      this.rigidbody.set_mass(this.rigidbodySettings.mass);
      this.rigidbody.set_drag(this.rigidbodySettings.drag);
      this.rigidbody.set_angularDrag(this.rigidbodySettings.angularDrag);
      this.rigidbody.set_useGravity(this.rigidbodySettings.useGravity);
      this.rigidbody.set_isKinematic(this.rigidbodySettings.isKinematic);
      this.rigidbody.set_interpolation(this.rigidbodySettings.interpolate);
      this.rigidbody.set_collisionDetectionMode(this.rigidbodySettings.collisionDetection);
      this.rigidbody.set_constraints(this.rigidbodySettings.constraints);
      this.rigidbody.set_solverIterations(this.rigidbodySettings.solverCount);
      if (this.rigidbody.get_isKinematic())
        return;
      this.rigidbody.set_velocity(Vector3.get_zero());
      this.rigidbody.set_angularVelocity(Vector3.get_zero());
      this.rigidbody.Sleep();
    }

    public void ApplyJointSettings(float _anchorOffset)
    {
      this.joint = (ConfigurableJoint) this.gameObject.GetComponent<ConfigurableJoint>();
      if (Object.op_Equality((Object) this.joint, (Object) null))
        this.joint = (ConfigurableJoint) this.gameObject.AddComponent<ConfigurableJoint>();
      this.joint.set_xMotion((ConfigurableJointMotion) 1);
      this.joint.set_yMotion((ConfigurableJointMotion) 1);
      this.joint.set_zMotion((ConfigurableJointMotion) 1);
      ConfigurableJoint joint1 = this.joint;
      SoftJointLimit softJointLimit1 = (SoftJointLimit) null;
      ((SoftJointLimit) ref softJointLimit1).set_limit(this.jointSettings.swingLimit);
      SoftJointLimit softJointLimit2 = softJointLimit1;
      joint1.set_linearLimit(softJointLimit2);
      ConfigurableJoint joint2 = this.joint;
      SoftJointLimitSpring jointLimitSpring1 = (SoftJointLimitSpring) null;
      ((SoftJointLimitSpring) ref jointLimitSpring1).set_spring(this.jointSettings.spring);
      ((SoftJointLimitSpring) ref jointLimitSpring1).set_damper(this.jointSettings.damper);
      SoftJointLimitSpring jointLimitSpring2 = jointLimitSpring1;
      joint2.set_linearLimitSpring(jointLimitSpring2);
    }

    public void RemoveJoint()
    {
      if (Object.op_Equality((Object) this.joint, (Object) null))
        return;
      if (Application.get_isPlaying())
        Object.Destroy((Object) this.joint);
      else
        Object.DestroyImmediate((Object) this.joint);
      this.joint = (ConfigurableJoint) null;
    }

    public void RemoveRigidbody()
    {
      if (Object.op_Equality((Object) this.rigidbody, (Object) null))
        return;
      this.RemoveJoint();
      if (Application.get_isPlaying())
        Object.Destroy((Object) this.rigidbody);
      else
        Object.DestroyImmediate((Object) this.rigidbody);
      this.rigidbody = (Rigidbody) null;
    }

    public GameObject AttachedPrefab()
    {
      return this.attachedPrefab;
    }

    public void Destroy()
    {
      if (Application.get_isPlaying())
        Object.Destroy((Object) this.gameObject);
      else
        Object.DestroyImmediate((Object) this.gameObject);
      this.gameObject = (GameObject) null;
    }
  }
}
