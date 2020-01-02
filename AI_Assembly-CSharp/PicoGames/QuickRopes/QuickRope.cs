// Decompiled with JetBrains decompiler
// Type: PicoGames.QuickRopes.QuickRope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PicoGames.Utilities;
using System;
using UnityEngine;

namespace PicoGames.QuickRopes
{
  [AddComponentMenu("PicoGames/QuickRopes/QuickRope")]
  [DisallowMultipleComponent]
  [RequireComponent(typeof (Spline))]
  public class QuickRope : MonoBehaviour
  {
    private const int MAX_JOINT_COUNT = 128;
    private const float MIN_JOINT_SCALE = 0.001f;
    private const float MIN_JOINT_SPACING = 0.001f;
    private const string VERSION = "3.1.6";
    [SerializeField]
    [Min(0.001f)]
    public float linkScale;
    [SerializeField]
    [Min(0.001f)]
    public float linkSpacing;
    [SerializeField]
    [Min(3f)]
    public int maxLinkCount;
    [SerializeField]
    [Min(1f)]
    public int minLinkCount;
    [SerializeField]
    public bool alternateRotation;
    [SerializeField]
    public bool usePhysics;
    [SerializeField]
    public bool canResize;
    [SerializeField]
    public GameObject defaultPrefab;
    [SerializeField]
    public TransformSettings defaultPrefabOffsets;
    [SerializeField]
    public RigidbodySettings defaultRigidbodySettings;
    [SerializeField]
    public JointSettings defaultJointSettings;
    [SerializeField]
    public ColliderSettings defaultColliderSettings;
    [SerializeField]
    public float velocityAccel;
    [SerializeField]
    public float velocityDampen;
    [SerializeField]
    [HideInInspector]
    private float velocity;
    [SerializeField]
    [HideInInspector]
    private float kVelocity;
    [SerializeField]
    [HideInInspector]
    private int activeLinkCount;
    [SerializeField]
    private Link[] links;
    [SerializeField]
    [HideInInspector]
    private Spline spline;

    public QuickRope()
    {
      base.\u002Ector();
    }

    public Spline Spline
    {
      get
      {
        if (Object.op_Equality((Object) this.spline, (Object) null))
        {
          this.spline = (Spline) ((Component) this).get_gameObject().GetComponent<Spline>();
          if (Object.op_Equality((Object) this.spline, (Object) null))
          {
            this.spline = (Spline) ((Component) this).get_gameObject().AddComponent<Spline>();
            this.spline.Reset();
          }
        }
        return this.spline;
      }
    }

    public int ActiveLinkCount
    {
      get
      {
        return this.activeLinkCount;
      }
    }

    public Link[] Links
    {
      get
      {
        return this.links;
      }
    }

    public float Velocity
    {
      get
      {
        return this.velocity;
      }
      set
      {
        this.velocity = value;
      }
    }

    private void Reset()
    {
      if (Application.get_isPlaying())
        return;
      this.ClearLinks();
      this.Spline.Reset();
    }

    public void Generate()
    {
      if (this.spline.IsLooped)
        this.canResize = false;
      SplinePoint[] splinePointArray = this.ResizeLinkArray();
      if (splinePointArray.Length == 0)
        return;
      Rigidbody rigidbody = (Rigidbody) null;
      for (int index = this.links.Length - 1; index >= 0; --index)
      {
        this.links[index].IsActive = index < this.activeLinkCount || index == this.links.Length - 1;
        this.links[index].transform.set_localScale(Vector3.op_Multiply(Vector3.get_one(), this.linkScale));
        this.links[index].gameObject.set_layer(((Component) this).get_gameObject().get_layer());
        this.links[index].gameObject.set_tag(((Component) this).get_gameObject().get_tag());
        if (index < splinePointArray.Length - 1)
        {
          this.links[index].transform.set_rotation(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), splinePointArray[index].rotation));
          this.links[index].transform.set_position(((Component) this).get_transform().TransformPoint(splinePointArray[index].position));
        }
        else if (index == this.links.Length - 1)
        {
          this.links[index].transform.set_rotation(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), splinePointArray[splinePointArray.Length - 1].rotation));
          this.links[index].transform.set_position(((Component) this).get_transform().TransformPoint(splinePointArray[splinePointArray.Length - 1].position));
        }
        if (index != this.links.Length - 1)
        {
          if (!this.links[index].overridePrefab)
            this.links[index].prefab = this.defaultPrefab;
          if (!this.links[index].overrideOffsetSettings)
            this.links[index].offsetSettings = this.defaultPrefabOffsets;
          if (Object.op_Inequality((Object) this.links[index].AttachedPrefab(), (Object) null))
            ((Object) this.links[index].AttachedPrefab()).set_hideFlags((HideFlags) 9);
        }
        this.links[index].alternateJoints = this.alternateRotation;
        this.links[index].ApplyPrefabSettings();
        if (!this.links[index].overrideRigidbodySettings)
          this.links[index].rigidbodySettings = this.defaultRigidbodySettings;
        this.links[index].ApplyRigidbodySettings();
        if (index != this.links.Length - 1)
        {
          if (!this.links[index].overrideJointSettings)
            this.links[index].jointSettings = this.defaultJointSettings;
          this.links[index].ApplyJointSettings(this.linkSpacing * (1f / this.linkScale));
        }
        if (!this.links[index].overrideColliderSettings)
          this.links[index].colliderSettings = this.defaultColliderSettings;
        this.links[index].ApplyColliderSettings();
        if (this.links[index].TogglePhysicsEnabled(this.usePhysics))
        {
          this.links[index].ConnectedBody = rigidbody;
          rigidbody = this.links[index].Rigidbody;
        }
      }
      if (!this.usePhysics)
        return;
      if (this.spline.IsLooped)
      {
        this.links[this.links.Length - 1].IsActive = false;
        --this.activeLinkCount;
        this.links[this.links.Length - 2].ConnectedBody = this.links[0].Rigidbody;
      }
      else
      {
        this.links[this.links.Length - 1].RemoveJoint();
        this.links[this.links.Length - 1].IsPrefabActive = false;
        if (!this.canResize || this.activeLinkCount == this.links.Length)
          return;
        this.links[this.activeLinkCount - 1].ConnectedBody = this.links[this.links.Length - 1].Rigidbody;
      }
    }

    private SplinePoint[] ResizeLinkArray()
    {
      this.maxLinkCount = Mathf.Min(this.maxLinkCount, 128);
      SplinePoint[] splinePointArray = this.Spline.GetSpacedPointsReversed(this.linkSpacing);
      this.activeLinkCount = Mathf.Min(this.maxLinkCount, splinePointArray.Length) - 1;
      int newSize = !this.canResize ? splinePointArray.Length : this.maxLinkCount;
      if (newSize <= 0 && this.links.Length > 0)
      {
        for (int index = 0; index < this.links.Length; ++index)
          this.links[index].Destroy();
        this.links = new Link[0];
        splinePointArray = new SplinePoint[0];
      }
      else if (this.links.Length != newSize)
      {
        if (newSize > this.links.Length)
        {
          int length = this.links.Length;
          Array.Resize<Link>(ref this.links, newSize);
          for (int _index = length; _index < newSize; ++_index)
          {
            this.links[_index] = new Link(new GameObject("Link_[" + (object) _index + "]"), _index);
            this.links[_index].Parent = ((Component) this).get_transform();
          }
        }
        else if (newSize > 0)
        {
          for (int index = this.links.Length - 1; index >= newSize; --index)
            this.links[index].Destroy();
          Array.Resize<Link>(ref this.links, newSize);
        }
      }
      return splinePointArray;
    }

    private void ClearLinks()
    {
      if (Application.get_isPlaying())
      {
        Debug.LogError((object) "Cannot destroy links whilst in 'Play' mode.");
      }
      else
      {
        while (((Component) this).get_transform().get_childCount() > 0)
        {
          if (Application.get_isPlaying())
            Object.Destroy((Object) ((Component) ((Component) this).get_transform().GetChild(0)).get_gameObject());
          else
            Object.DestroyImmediate((Object) ((Component) ((Component) this).get_transform().GetChild(0)).get_gameObject());
        }
        this.links = new Link[0];
      }
    }

    private void FixedUpdate()
    {
      this.kVelocity = (double) this.velocity == 0.0 ? Mathf.Lerp(this.kVelocity, this.velocity, Time.get_deltaTime() * this.velocityDampen) : Mathf.Lerp(this.kVelocity, this.velocity, Time.get_deltaTime() * this.velocityAccel);
      if ((double) this.kVelocity > 0.0)
        this.kVelocity = !this.Extend(this.kVelocity) ? 0.0f : this.kVelocity;
      if ((double) this.kVelocity >= 0.0)
        return;
      this.kVelocity = !this.Retract(this.kVelocity, this.minLinkCount) ? 0.0f : this.kVelocity;
    }

    public bool Extend(float _speed)
    {
      if (!this.canResize)
        return false;
      Link link1 = this.links[this.links.Length - 1];
      Link link2 = this.links[this.activeLinkCount - 1];
      Vector3 vector3_1 = Vector3.op_Subtraction(link1.transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(link2.transform.get_up(), this.linkSpacing), 2f));
      if (this.activeLinkCount < this.maxLinkCount - 1)
      {
        link2.ConnectedBody = (Rigidbody) null;
        link2.transform.set_position(Vector3.MoveTowards(link2.transform.get_position(), vector3_1, Mathf.Abs(_speed) * Time.get_deltaTime()));
        if ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(link2.transform.get_position(), link1.transform.get_position())) > (double) this.linkSpacing * (double) this.linkSpacing)
        {
          Link link3 = this.links[this.activeLinkCount];
          Transform transform = link3.transform;
          Vector3 position = link2.transform.get_position();
          Vector3 vector3_2 = Vector3.op_Subtraction(link1.transform.get_position(), link2.transform.get_position());
          Vector3 vector3_3 = Vector3.op_Multiply(((Vector3) ref vector3_2).get_normalized(), this.linkSpacing);
          Vector3 vector3_4 = Vector3.op_Addition(position, vector3_3);
          transform.set_position(vector3_4);
          link3.transform.set_rotation(link2.transform.get_rotation());
          ++this.activeLinkCount;
          link3.IsActive = true;
          link2.ConnectedBody = link3.Rigidbody;
          link2 = link3;
        }
        link2.ApplyJointSettings(Vector3.Distance(link1.transform.get_position(), link2.transform.get_position()) * (1f / this.linkScale));
        link2.ConnectedBody = link1.Rigidbody;
      }
      else
        this.kVelocity = 0.0f;
      return true;
    }

    public bool Retract(float _speed, int _minJointCount)
    {
      if (!this.canResize)
        return false;
      Link link1 = this.links[this.links.Length - 1];
      Link link2 = this.links[this.activeLinkCount - 1];
      link2.ConnectedBody = (Rigidbody) null;
      link2.transform.set_position(Vector3.MoveTowards(link2.transform.get_position(), link1.transform.get_position(), Mathf.Abs(_speed) * Time.get_deltaTime()));
      if (this.activeLinkCount > _minJointCount)
      {
        if ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(link1.transform.get_position(), link2.transform.get_position())) <= 0.00999999977648258)
        {
          link2.IsActive = false;
          --this.activeLinkCount;
          link2 = this.links[this.activeLinkCount - 1];
        }
      }
      else
      {
        this.kVelocity = 0.0f;
        link2.transform.set_position(Vector3.op_Subtraction(link1.transform.get_position(), Vector3.op_Multiply(link2.transform.get_up(), this.linkSpacing)));
      }
      ((Joint) link2.Joint).set_anchor(new Vector3(0.0f, Vector3.Distance(link1.transform.get_position(), link2.transform.get_position()) * (1f / this.linkScale), 0.0f));
      link2.ConnectedBody = link1.Rigidbody;
      return true;
    }

    public static QuickRope Create(
      Vector3 _position,
      GameObject _prefab,
      float _linkSpacing = 1f,
      float _prefabScale = 0.5f)
    {
      return QuickRope.Create(_position, Vector3.op_Addition(_position, new Vector3(0.0f, 5f, 0.0f)), _prefab, _linkSpacing, _prefabScale);
    }

    public static QuickRope Create(
      Vector3 _pointA,
      Vector3 _pointB,
      GameObject _prefab,
      float _linkSpacing = 1f,
      float _prefabScale = 0.5f)
    {
      GameObject gameObject = new GameObject("Rope");
      gameObject.get_transform().set_position(_pointA);
      QuickRope quickRope = (QuickRope) gameObject.AddComponent<QuickRope>();
      quickRope.defaultPrefab = _prefab;
      Vector3 vector3 = Vector3.op_Subtraction(_pointB, _pointA);
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      quickRope.Spline.SetControlPoint(1, _pointB, (Space) 0);
      quickRope.Spline.SetPoint(1, Vector3.op_Addition(_pointA, normalized), (Space) 0);
      quickRope.Spline.SetPoint(2, Vector3.op_Subtraction(_pointB, normalized), (Space) 0);
      quickRope.Generate();
      return quickRope;
    }

    public enum RenderType
    {
      Prefab,
      Rendered,
    }

    public enum ColliderType
    {
      None,
      Sphere,
      Box,
      Capsule,
    }
  }
}
