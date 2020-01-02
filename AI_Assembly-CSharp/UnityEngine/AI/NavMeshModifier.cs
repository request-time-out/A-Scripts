// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace UnityEngine.AI
{
  [ExecuteInEditMode]
  [AddComponentMenu("Navigation/NavMeshModifier", 32)]
  [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
  public class NavMeshModifier : MonoBehaviour
  {
    private static readonly List<NavMeshModifier> s_NavMeshModifiers = new List<NavMeshModifier>();
    [SerializeField]
    private bool m_OverrideArea;
    [SerializeField]
    private int m_Area;
    [SerializeField]
    private bool m_IgnoreFromBuild;
    [SerializeField]
    private List<int> m_AffectedAgents;

    public NavMeshModifier()
    {
      base.\u002Ector();
    }

    public bool overrideArea
    {
      get
      {
        return this.m_OverrideArea;
      }
      set
      {
        this.m_OverrideArea = value;
      }
    }

    public int area
    {
      get
      {
        return this.m_Area;
      }
      set
      {
        this.m_Area = value;
      }
    }

    public bool ignoreFromBuild
    {
      get
      {
        return this.m_IgnoreFromBuild;
      }
      set
      {
        this.m_IgnoreFromBuild = value;
      }
    }

    public static List<NavMeshModifier> activeModifiers
    {
      get
      {
        return NavMeshModifier.s_NavMeshModifiers;
      }
    }

    private void OnEnable()
    {
      if (NavMeshModifier.s_NavMeshModifiers.Contains(this))
        return;
      NavMeshModifier.s_NavMeshModifiers.Add(this);
    }

    private void OnDisable()
    {
      NavMeshModifier.s_NavMeshModifiers.Remove(this);
    }

    public bool AffectsAgentType(int agentTypeID)
    {
      if (this.m_AffectedAgents.Count == 0)
        return false;
      return this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1;
    }
  }
}
