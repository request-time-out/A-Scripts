// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DeepSky.Haze
{
  [ExecuteInEditMode]
  [AddComponentMenu("DeepSky Haze/Zone", 52)]
  public class DS_HazeZone : MonoBehaviour
  {
    [SerializeField]
    private DS_HazeContext m_Context;
    [SerializeField]
    [Range(0.0f, 250f)]
    private int m_Priority;
    [SerializeField]
    [Range(0.001f, 1f)]
    private float m_BlendRange;
    private Bounds m_AABB;
    private float m_BlendRangeInverse;

    public DS_HazeZone()
    {
      base.\u002Ector();
    }

    public DS_HazeContext Context
    {
      get
      {
        return this.m_Context;
      }
    }

    public int Priority
    {
      get
      {
        return this.m_Priority;
      }
      set
      {
        this.m_Priority = value <= 0 ? 0 : value;
      }
    }

    public float BlendRange
    {
      get
      {
        return this.m_BlendRange;
      }
      set
      {
        this.m_BlendRange = Mathf.Clamp01(value);
      }
    }

    private void Setup()
    {
      this.m_AABB = new Bounds(Vector3.get_zero(), ((Component) this).get_transform().get_localScale());
      this.m_BlendRangeInverse = (float) (1.0 / (double) Mathf.Max(Mathf.Min(new float[3]
      {
        (float) ((Bounds) ref this.m_AABB).get_extents().x,
        (float) ((Bounds) ref this.m_AABB).get_extents().y,
        (float) ((Bounds) ref this.m_AABB).get_extents().z
      }) * this.m_BlendRange, (float) Mathf.Epsilon));
    }

    private void Start()
    {
      this.Setup();
    }

    private void OnValidate()
    {
      this.Setup();
    }

    public bool Contains(Vector3 position)
    {
      if (((Component) this).get_transform().get_hasChanged())
        this.Setup();
      Vector3 vector3 = ((Component) this).get_transform().InverseTransformPoint(position);
      ((Vector3) ref vector3).Scale(((Component) this).get_transform().get_localScale());
      return ((Bounds) ref this.m_AABB).Contains(vector3);
    }

    public float GetBlendWeight(Vector3 position)
    {
      Vector3 vector3 = ((Component) this).get_transform().InverseTransformPoint(position);
      ((Vector3) ref vector3).Scale(((Component) this).get_transform().get_localScale());
      return Mathf.Clamp01(Mathf.Min(new float[3]
      {
        Mathf.Abs((float) ((Bounds) ref this.m_AABB).get_extents().x - Mathf.Abs((float) vector3.x)),
        Mathf.Abs((float) ((Bounds) ref this.m_AABB).get_extents().y - Mathf.Abs((float) vector3.y)),
        Mathf.Abs((float) ((Bounds) ref this.m_AABB).get_extents().z - Mathf.Abs((float) vector3.z))
      }) * this.m_BlendRangeInverse);
    }

    public static bool operator >(DS_HazeZone c1, DS_HazeZone c2)
    {
      return c1.m_Priority == c2.m_Priority ? (double) Vector3.Dot(((Bounds) ref c1.m_AABB).get_extents(), ((Bounds) ref c1.m_AABB).get_extents()) > (double) Vector3.Dot(((Bounds) ref c2.m_AABB).get_extents(), ((Bounds) ref c2.m_AABB).get_extents()) : c1.m_Priority > c2.m_Priority;
    }

    public static bool operator <(DS_HazeZone c1, DS_HazeZone c2)
    {
      return c1.m_Priority == c2.m_Priority ? (double) Vector3.Dot(((Bounds) ref c1.m_AABB).get_extents(), ((Bounds) ref c1.m_AABB).get_extents()) < (double) Vector3.Dot(((Bounds) ref c2.m_AABB).get_extents(), ((Bounds) ref c2.m_AABB).get_extents()) : c1.m_Priority < c2.m_Priority;
    }
  }
}
