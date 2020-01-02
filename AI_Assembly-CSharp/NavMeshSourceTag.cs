// Decompiled with JetBrains decompiler
// Type: NavMeshSourceTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
  public static Dictionary<GameObject, NavMeshSourceTag.Info> m_Meshes = new Dictionary<GameObject, NavMeshSourceTag.Info>();

  public NavMeshSourceTag()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    MeshFilter component = (MeshFilter) ((Component) this).GetComponent<MeshFilter>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    if (!NavMeshSourceTag.m_Meshes.ContainsKey(((Component) this).get_gameObject()))
    {
      NavMeshSourceTag.m_Meshes.Add(((Component) this).get_gameObject(), new NavMeshSourceTag.Info()
      {
        MeshFilter = component,
        NavMeshModifier = (NavMeshModifier) ((Component) this).GetComponent<NavMeshModifier>()
      });
    }
    else
    {
      NavMeshSourceTag.Info mesh = NavMeshSourceTag.m_Meshes[((Component) this).get_gameObject()];
      mesh.MeshFilter = component;
      mesh.NavMeshModifier = (NavMeshModifier) ((Component) this).GetComponent<NavMeshModifier>();
    }
  }

  private void OnDisable()
  {
    NavMeshSourceTag.m_Meshes.Remove(((Component) this).get_gameObject());
  }

  public static void Collect(ref List<NavMeshBuildSource> sources, int _defaultArea = 0)
  {
    sources.Clear();
    foreach (NavMeshSourceTag.Info info in ((IEnumerable<NavMeshSourceTag.Info>) NavMeshSourceTag.m_Meshes.Values).Where<NavMeshSourceTag.Info>((Func<NavMeshSourceTag.Info, bool>) (v => Object.op_Inequality((Object) v.MeshFilter, (Object) null))).Where<NavMeshSourceTag.Info>((Func<NavMeshSourceTag.Info, bool>) (v => Object.op_Inequality((Object) v.MeshFilter.get_sharedMesh(), (Object) null))).Where<NavMeshSourceTag.Info>((Func<NavMeshSourceTag.Info, bool>) (v => !v.Ignore)))
    {
      NavMeshBuildSource navMeshBuildSource = (NavMeshBuildSource) null;
      ((NavMeshBuildSource) ref navMeshBuildSource).set_shape((NavMeshBuildSourceShape) 0);
      ((NavMeshBuildSource) ref navMeshBuildSource).set_sourceObject((Object) info.MeshFilter.get_sharedMesh());
      ((NavMeshBuildSource) ref navMeshBuildSource).set_transform(((Component) info.MeshFilter).get_transform().get_localToWorldMatrix());
      ((NavMeshBuildSource) ref navMeshBuildSource).set_area(!info.OverrideArea ? _defaultArea : info.Area);
      sources.Add(navMeshBuildSource);
    }
  }

  public class Info
  {
    public MeshFilter MeshFilter { get; set; }

    public NavMeshModifier NavMeshModifier { get; set; }

    public bool Ignore
    {
      get
      {
        return Object.op_Implicit((Object) this.NavMeshModifier) && ((Behaviour) this.NavMeshModifier).get_isActiveAndEnabled() & this.NavMeshModifier.ignoreFromBuild;
      }
    }

    public bool OverrideArea
    {
      get
      {
        return Object.op_Implicit((Object) this.NavMeshModifier) && ((Behaviour) this.NavMeshModifier).get_isActiveAndEnabled() & this.NavMeshModifier.overrideArea;
      }
    }

    public int Area
    {
      get
      {
        return Object.op_Implicit((Object) this.NavMeshModifier) ? this.NavMeshModifier.area : 0;
      }
    }
  }
}
