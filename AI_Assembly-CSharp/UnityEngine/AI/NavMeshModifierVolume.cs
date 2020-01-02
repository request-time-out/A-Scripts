// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshModifierVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace UnityEngine.AI
{
  [ExecuteInEditMode]
  [AddComponentMenu("Navigation/NavMeshModifierVolume", 31)]
  [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
  public class NavMeshModifierVolume : MonoBehaviour
  {
    private static readonly List<NavMeshModifierVolume> s_NavMeshModifiers = new List<NavMeshModifierVolume>();
    [SerializeField]
    private Vector3 m_Size;
    [SerializeField]
    private Vector3 m_Center;
    [SerializeField]
    private int m_Area;
    [SerializeField]
    private List<int> m_AffectedAgents;

    public NavMeshModifierVolume()
    {
      base.\u002Ector();
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

    public static List<NavMeshModifierVolume> activeModifiers
    {
      get
      {
        return NavMeshModifierVolume.s_NavMeshModifiers;
      }
    }

    private void OnEnable()
    {
      if (NavMeshModifierVolume.s_NavMeshModifiers.Contains(this))
        return;
      NavMeshModifierVolume.s_NavMeshModifiers.Add(this);
    }

    private void OnDisable()
    {
      NavMeshModifierVolume.s_NavMeshModifiers.Remove(this);
    }

    public bool AffectsAgentType(int agentTypeID)
    {
      if (this.m_AffectedAgents.Count == 0)
        return false;
      return this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1;
    }
  }
}
