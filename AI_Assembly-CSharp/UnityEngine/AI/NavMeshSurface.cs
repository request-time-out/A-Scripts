// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshSurface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.AI
{
  [ExecuteInEditMode]
  [DefaultExecutionOrder(-102)]
  [AddComponentMenu("Navigation/NavMeshSurface", 30)]
  [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
  public class NavMeshSurface : MonoBehaviour
  {
    private static readonly List<NavMeshSurface> s_NavMeshSurfaces = new List<NavMeshSurface>();
    [SerializeField]
    private int m_AgentTypeID;
    [SerializeField]
    private CollectObjects m_CollectObjects;
    [SerializeField]
    private Vector3 m_Size;
    [SerializeField]
    private Vector3 m_Center;
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private NavMeshCollectGeometry m_UseGeometry;
    [SerializeField]
    private int m_DefaultArea;
    [SerializeField]
    private bool m_IgnoreNavMeshAgent;
    [SerializeField]
    private bool m_IgnoreNavMeshObstacle;
    [SerializeField]
    private bool m_OverrideTileSize;
    [SerializeField]
    private int m_TileSize;
    [SerializeField]
    private bool m_OverrideVoxelSize;
    [SerializeField]
    private float m_VoxelSize;
    [SerializeField]
    private bool m_BuildHeightMesh;
    [FormerlySerializedAs("m_BakedNavMeshData")]
    [SerializeField]
    private NavMeshData m_NavMeshData;
    private NavMeshDataInstance m_NavMeshDataInstance;
    private Vector3 m_LastPosition;
    private Quaternion m_LastRotation;

    public NavMeshSurface()
    {
      base.\u002Ector();
    }

    public int agentTypeID
    {
      get
      {
        return this.m_AgentTypeID;
      }
      set
      {
        this.m_AgentTypeID = value;
      }
    }

    public CollectObjects collectObjects
    {
      get
      {
        return this.m_CollectObjects;
      }
      set
      {
        this.m_CollectObjects = value;
      }
    }

    public Vector3 size
    {
      get
      {
        return this.m_Size;
      }
      set
      {
        this.m_Size = value;
      }
    }

    public Vector3 center
    {
      get
      {
        return this.m_Center;
      }
      set
      {
        this.m_Center = value;
      }
    }

    public LayerMask layerMask
    {
      get
      {
        return this.m_LayerMask;
      }
      set
      {
        this.m_LayerMask = value;
      }
    }

    public NavMeshCollectGeometry useGeometry
    {
      get
      {
        return this.m_UseGeometry;
      }
      set
      {
        this.m_UseGeometry = value;
      }
    }

    public int defaultArea
    {
      get
      {
        return this.m_DefaultArea;
      }
      set
      {
        this.m_DefaultArea = value;
      }
    }

    public bool ignoreNavMeshAgent
    {
      get
      {
        return this.m_IgnoreNavMeshAgent;
      }
      set
      {
        this.m_IgnoreNavMeshAgent = value;
      }
    }

    public bool ignoreNavMeshObstacle
    {
      get
      {
        return this.m_IgnoreNavMeshObstacle;
      }
      set
      {
        this.m_IgnoreNavMeshObstacle = value;
      }
    }

    public bool overrideTileSize
    {
      get
      {
        return this.m_OverrideTileSize;
      }
      set
      {
        this.m_OverrideTileSize = value;
      }
    }

    public int tileSize
    {
      get
      {
        return this.m_TileSize;
      }
      set
      {
        this.m_TileSize = value;
      }
    }

    public bool overrideVoxelSize
    {
      get
      {
        return this.m_OverrideVoxelSize;
      }
      set
      {
        this.m_OverrideVoxelSize = value;
      }
    }

    public float voxelSize
    {
      get
      {
        return this.m_VoxelSize;
      }
      set
      {
        this.m_VoxelSize = value;
      }
    }

    public bool buildHeightMesh
    {
      get
      {
        return this.m_BuildHeightMesh;
      }
      set
      {
        this.m_BuildHeightMesh = value;
      }
    }

    public NavMeshData navMeshData
    {
      get
      {
        return this.m_NavMeshData;
      }
      set
      {
        this.m_NavMeshData = value;
      }
    }

    public static List<NavMeshSurface> activeSurfaces
    {
      get
      {
        return NavMeshSurface.s_NavMeshSurfaces;
      }
    }

    private void OnEnable()
    {
      NavMeshSurface.Register(this);
      this.AddData();
    }

    private void OnDisable()
    {
      this.RemoveData();
      NavMeshSurface.Unregister(this);
    }

    public void AddData()
    {
      if (((NavMeshDataInstance) ref this.m_NavMeshDataInstance).get_valid())
        return;
      if (Object.op_Inequality((Object) this.m_NavMeshData, (Object) null))
      {
        this.m_NavMeshDataInstance = NavMesh.AddNavMeshData(this.m_NavMeshData, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
        ((NavMeshDataInstance) ref this.m_NavMeshDataInstance).set_owner((Object) this);
      }
      this.m_LastPosition = ((Component) this).get_transform().get_position();
      this.m_LastRotation = ((Component) this).get_transform().get_rotation();
    }

    public void RemoveData()
    {
      ((NavMeshDataInstance) ref this.m_NavMeshDataInstance).Remove();
      this.m_NavMeshDataInstance = (NavMeshDataInstance) null;
    }

    public NavMeshBuildSettings GetBuildSettings()
    {
      NavMeshBuildSettings settingsById = NavMesh.GetSettingsByID(this.m_AgentTypeID);
      if (((NavMeshBuildSettings) ref settingsById).get_agentTypeID() == -1)
      {
        Debug.LogWarning((object) ("No build settings for agent type ID " + (object) this.agentTypeID), (Object) this);
        ((NavMeshBuildSettings) ref settingsById).set_agentTypeID(this.m_AgentTypeID);
      }
      if (this.overrideTileSize)
      {
        ((NavMeshBuildSettings) ref settingsById).set_overrideTileSize(true);
        ((NavMeshBuildSettings) ref settingsById).set_tileSize(this.tileSize);
      }
      if (this.overrideVoxelSize)
      {
        ((NavMeshBuildSettings) ref settingsById).set_overrideVoxelSize(true);
        ((NavMeshBuildSettings) ref settingsById).set_voxelSize(this.voxelSize);
      }
      return settingsById;
    }

    public void BuildNavMesh()
    {
      List<NavMeshBuildSource> sources = this.CollectSources();
      Bounds worldBounds;
      ((Bounds) ref worldBounds).\u002Ector(this.m_Center, NavMeshSurface.Abs(this.m_Size));
      if (this.m_CollectObjects == CollectObjects.All || this.m_CollectObjects == CollectObjects.Children)
        worldBounds = this.CalculateWorldBounds(sources);
      NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(this.GetBuildSettings(), sources, worldBounds, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
      if (!Object.op_Inequality((Object) navMeshData, (Object) null))
        return;
      ((Object) navMeshData).set_name(((Object) ((Component) this).get_gameObject()).get_name());
      this.RemoveData();
      this.m_NavMeshData = navMeshData;
      if (!((Behaviour) this).get_isActiveAndEnabled())
        return;
      this.AddData();
    }

    public AsyncOperation UpdateNavMesh(NavMeshData data)
    {
      List<NavMeshBuildSource> sources = this.CollectSources();
      Bounds worldBounds;
      ((Bounds) ref worldBounds).\u002Ector(this.m_Center, NavMeshSurface.Abs(this.m_Size));
      if (this.m_CollectObjects == CollectObjects.All || this.m_CollectObjects == CollectObjects.Children)
        worldBounds = this.CalculateWorldBounds(sources);
      return NavMeshBuilder.UpdateNavMeshDataAsync(data, this.GetBuildSettings(), sources, worldBounds);
    }

    private static void Register(NavMeshSurface surface)
    {
      if (NavMeshSurface.s_NavMeshSurfaces.Count == 0)
      {
        // ISSUE: variable of the null type
        __Null onPreUpdate = NavMesh.onPreUpdate;
        // ISSUE: reference to a compiler-generated field
        if (NavMeshSurface.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          NavMeshSurface.\u003C\u003Ef__mg\u0024cache0 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateActive));
        }
        // ISSUE: reference to a compiler-generated field
        NavMesh.OnNavMeshPreUpdate fMgCache0 = NavMeshSurface.\u003C\u003Ef__mg\u0024cache0;
        NavMesh.onPreUpdate = (__Null) Delegate.Combine((Delegate) onPreUpdate, (Delegate) fMgCache0);
      }
      if (NavMeshSurface.s_NavMeshSurfaces.Contains(surface))
        return;
      NavMeshSurface.s_NavMeshSurfaces.Add(surface);
    }

    private static void Unregister(NavMeshSurface surface)
    {
      NavMeshSurface.s_NavMeshSurfaces.Remove(surface);
      if (NavMeshSurface.s_NavMeshSurfaces.Count != 0)
        return;
      // ISSUE: variable of the null type
      __Null onPreUpdate = NavMesh.onPreUpdate;
      // ISSUE: reference to a compiler-generated field
      if (NavMeshSurface.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        NavMeshSurface.\u003C\u003Ef__mg\u0024cache1 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateActive));
      }
      // ISSUE: reference to a compiler-generated field
      NavMesh.OnNavMeshPreUpdate fMgCache1 = NavMeshSurface.\u003C\u003Ef__mg\u0024cache1;
      NavMesh.onPreUpdate = (__Null) Delegate.Remove((Delegate) onPreUpdate, (Delegate) fMgCache1);
    }

    private static void UpdateActive()
    {
      for (int index = 0; index < NavMeshSurface.s_NavMeshSurfaces.Count; ++index)
        NavMeshSurface.s_NavMeshSurfaces[index].UpdateDataIfTransformChanged();
    }

    private void AppendModifierVolumes(ref List<NavMeshBuildSource> sources)
    {
      List<NavMeshModifierVolume> meshModifierVolumeList;
      if (this.m_CollectObjects == CollectObjects.Children)
      {
        meshModifierVolumeList = new List<NavMeshModifierVolume>((IEnumerable<NavMeshModifierVolume>) ((Component) this).GetComponentsInChildren<NavMeshModifierVolume>());
        meshModifierVolumeList.RemoveAll((Predicate<NavMeshModifierVolume>) (x => !((Behaviour) x).get_isActiveAndEnabled()));
      }
      else
        meshModifierVolumeList = NavMeshModifierVolume.activeModifiers;
      foreach (NavMeshModifierVolume meshModifierVolume in meshModifierVolumeList)
      {
        if ((LayerMask.op_Implicit(this.m_LayerMask) & 1 << ((Component) meshModifierVolume).get_gameObject().get_layer()) != 0 && meshModifierVolume.AffectsAgentType(this.m_AgentTypeID))
        {
          Vector3 vector3_1 = ((Component) meshModifierVolume).get_transform().TransformPoint(meshModifierVolume.center);
          Vector3 lossyScale = ((Component) meshModifierVolume).get_transform().get_lossyScale();
          Vector3 vector3_2;
          ((Vector3) ref vector3_2).\u002Ector((float) meshModifierVolume.size.x * Mathf.Abs((float) lossyScale.x), (float) meshModifierVolume.size.y * Mathf.Abs((float) lossyScale.y), (float) meshModifierVolume.size.z * Mathf.Abs((float) lossyScale.z));
          NavMeshBuildSource navMeshBuildSource = (NavMeshBuildSource) null;
          ((NavMeshBuildSource) ref navMeshBuildSource).set_shape((NavMeshBuildSourceShape) 5);
          ((NavMeshBuildSource) ref navMeshBuildSource).set_transform(Matrix4x4.TRS(vector3_1, ((Component) meshModifierVolume).get_transform().get_rotation(), Vector3.get_one()));
          ((NavMeshBuildSource) ref navMeshBuildSource).set_size(vector3_2);
          ((NavMeshBuildSource) ref navMeshBuildSource).set_area(meshModifierVolume.area);
          sources.Add(navMeshBuildSource);
        }
      }
    }

    private List<NavMeshBuildSource> CollectSources()
    {
      List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
      NavMeshSourceTag.Collect(ref sources, this.m_DefaultArea);
      if (this.m_IgnoreNavMeshAgent)
        sources.RemoveAll((Predicate<NavMeshBuildSource>) (x => Object.op_Inequality((Object) ((NavMeshBuildSource) ref x).get_component(), (Object) null) && Object.op_Inequality((Object) ((NavMeshBuildSource) ref x).get_component().get_gameObject().GetComponent<NavMeshAgent>(), (Object) null)));
      if (this.m_IgnoreNavMeshObstacle)
        sources.RemoveAll((Predicate<NavMeshBuildSource>) (x => Object.op_Inequality((Object) ((NavMeshBuildSource) ref x).get_component(), (Object) null) && Object.op_Inequality((Object) ((NavMeshBuildSource) ref x).get_component().get_gameObject().GetComponent<NavMeshObstacle>(), (Object) null)));
      this.AppendModifierVolumes(ref sources);
      return sources;
    }

    private static Vector3 Abs(Vector3 v)
    {
      return new Vector3(Mathf.Abs((float) v.x), Mathf.Abs((float) v.y), Mathf.Abs((float) v.z));
    }

    private static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
    {
      Vector3 vector3_1 = NavMeshSurface.Abs(((Matrix4x4) ref mat).MultiplyVector(Vector3.get_right()));
      Vector3 vector3_2 = NavMeshSurface.Abs(((Matrix4x4) ref mat).MultiplyVector(Vector3.get_up()));
      Vector3 vector3_3 = NavMeshSurface.Abs(((Matrix4x4) ref mat).MultiplyVector(Vector3.get_forward()));
      return new Bounds(((Matrix4x4) ref mat).MultiplyPoint(((Bounds) ref bounds).get_center()), Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(vector3_1, (float) ((Bounds) ref bounds).get_size().x), Vector3.op_Multiply(vector3_2, (float) ((Bounds) ref bounds).get_size().y)), Vector3.op_Multiply(vector3_3, (float) ((Bounds) ref bounds).get_size().z)));
    }

    private Bounds CalculateWorldBounds(List<NavMeshBuildSource> sources)
    {
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation(), Vector3.get_one());
      matrix4x4 = ((Matrix4x4) ref matrix4x4).get_inverse();
      Bounds bounds = (Bounds) null;
      using (List<NavMeshBuildSource>.Enumerator enumerator = sources.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NavMeshBuildSource current = enumerator.Current;
          switch ((int) ((NavMeshBuildSource) ref current).get_shape())
          {
            case 0:
              Mesh sourceObject1 = ((NavMeshBuildSource) ref current).get_sourceObject() as Mesh;
              ((Bounds) ref bounds).Encapsulate(NavMeshSurface.GetWorldBounds(Matrix4x4.op_Multiply(matrix4x4, ((NavMeshBuildSource) ref current).get_transform()), sourceObject1.get_bounds()));
              continue;
            case 1:
              TerrainData sourceObject2 = ((NavMeshBuildSource) ref current).get_sourceObject() as TerrainData;
              ((Bounds) ref bounds).Encapsulate(NavMeshSurface.GetWorldBounds(Matrix4x4.op_Multiply(matrix4x4, ((NavMeshBuildSource) ref current).get_transform()), new Bounds(Vector3.op_Multiply(0.5f, sourceObject2.get_size()), sourceObject2.get_size())));
              continue;
            case 2:
            case 3:
            case 4:
            case 5:
              ((Bounds) ref bounds).Encapsulate(NavMeshSurface.GetWorldBounds(Matrix4x4.op_Multiply(matrix4x4, ((NavMeshBuildSource) ref current).get_transform()), new Bounds(Vector3.get_zero(), ((NavMeshBuildSource) ref current).get_size())));
              continue;
            default:
              continue;
          }
        }
      }
      ((Bounds) ref bounds).Expand(0.1f);
      return bounds;
    }

    private bool HasTransformChanged()
    {
      return Vector3.op_Inequality(this.m_LastPosition, ((Component) this).get_transform().get_position()) || Quaternion.op_Inequality(this.m_LastRotation, ((Component) this).get_transform().get_rotation());
    }

    private void UpdateDataIfTransformChanged()
    {
      if (!this.HasTransformChanged())
        return;
      this.RemoveData();
      this.AddData();
    }
  }
}
