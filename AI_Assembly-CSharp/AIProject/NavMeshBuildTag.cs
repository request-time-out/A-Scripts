// Decompiled with JetBrains decompiler
// Type: AIProject.NavMeshBuildTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [DefaultExecutionOrder(-200)]
  public class NavMeshBuildTag : MonoBehaviour
  {
    private static List<NavMeshBuildTag.MeshInfo> _meshes = new List<NavMeshBuildTag.MeshInfo>();
    private static List<NavMeshBuildTag.TerrainInfo> _terrains = new List<NavMeshBuildTag.TerrainInfo>();
    [SerializeField]
    [NavMeshAreaEnum]
    private int _areaID;

    public NavMeshBuildTag()
    {
      base.\u002Ector();
    }

    public int AreaID
    {
      get
      {
        return this._areaID;
      }
    }

    private void OnEnable()
    {
      MeshFilter component1 = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        NavMeshBuildTag.MeshInfo meshInfo = new NavMeshBuildTag.MeshInfo(component1, this._areaID);
        NavMeshBuildTag._meshes.Add(meshInfo);
      }
      Terrain component2 = (Terrain) ((Component) this).GetComponent<Terrain>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      NavMeshBuildTag.TerrainInfo terrainInfo = new NavMeshBuildTag.TerrainInfo(component2, this._areaID);
      NavMeshBuildTag._terrains.Add(terrainInfo);
    }

    private void OnDisable()
    {
      MeshFilter m = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
      if (Object.op_Inequality((Object) m, (Object) null))
        NavMeshBuildTag._meshes.RemoveAll((Predicate<NavMeshBuildTag.MeshInfo>) (x => Object.op_Equality((Object) x.Filter, (Object) m)));
      Terrain t = (Terrain) ((Component) this).GetComponent<Terrain>();
      if (!Object.op_Inequality((Object) t, (Object) null))
        return;
      NavMeshBuildTag._terrains.RemoveAll((Predicate<NavMeshBuildTag.TerrainInfo>) (x => Object.op_Equality((Object) x.Terrain, (Object) t)));
    }

    public static void Collect(ref List<NavMeshBuildSource> sources)
    {
      sources.Clear();
      for (int index = 0; index < NavMeshBuildTag._meshes.Count; ++index)
      {
        NavMeshBuildTag.MeshInfo mesh = NavMeshBuildTag._meshes[index];
        if (mesh != null)
        {
          Mesh sharedMesh = mesh.Filter.get_sharedMesh();
          if (!Object.op_Equality((Object) sharedMesh, (Object) null))
          {
            NavMeshBuildSource navMeshBuildSource = (NavMeshBuildSource) null;
            ((NavMeshBuildSource) ref navMeshBuildSource).set_shape((NavMeshBuildSourceShape) 0);
            ((NavMeshBuildSource) ref navMeshBuildSource).set_sourceObject((Object) sharedMesh);
            ((NavMeshBuildSource) ref navMeshBuildSource).set_transform(((Component) mesh.Filter).get_transform().get_localToWorldMatrix());
            ((NavMeshBuildSource) ref navMeshBuildSource).set_area(mesh.Area);
            sources.Add(navMeshBuildSource);
          }
        }
      }
      for (int index = 0; index < NavMeshBuildTag._terrains.Count; ++index)
      {
        NavMeshBuildTag.TerrainInfo terrain = NavMeshBuildTag._terrains[index];
        if (terrain != null)
        {
          NavMeshBuildSource navMeshBuildSource = (NavMeshBuildSource) null;
          ((NavMeshBuildSource) ref navMeshBuildSource).set_shape((NavMeshBuildSourceShape) 1);
          ((NavMeshBuildSource) ref navMeshBuildSource).set_sourceObject((Object) terrain.Terrain.get_terrainData());
          ((NavMeshBuildSource) ref navMeshBuildSource).set_transform(Matrix4x4.TRS(((Component) terrain.Terrain).get_transform().get_position(), Quaternion.get_identity(), Vector3.get_one()));
          ((NavMeshBuildSource) ref navMeshBuildSource).set_area(terrain.Area);
          sources.Add(navMeshBuildSource);
        }
      }
    }

    public class MeshInfo
    {
      private MeshFilter _filter;
      private int _area;

      public MeshInfo(MeshFilter filter, int area)
      {
        this._filter = filter;
        this._area = area;
      }

      public MeshFilter Filter
      {
        get
        {
          return this._filter;
        }
        set
        {
          this._filter = value;
        }
      }

      public int Area
      {
        get
        {
          return this._area;
        }
        set
        {
          this._area = value;
        }
      }
    }

    public class TerrainInfo
    {
      private Terrain _terrain;
      private int _area;

      public TerrainInfo(Terrain terrain, int area)
      {
        this._terrain = terrain;
        this._area = area;
      }

      public Terrain Terrain
      {
        get
        {
          return this._terrain;
        }
        set
        {
          this._terrain = value;
        }
      }

      public int Area
      {
        get
        {
          return this._area;
        }
        set
        {
          this._area = value;
        }
      }
    }
  }
}
