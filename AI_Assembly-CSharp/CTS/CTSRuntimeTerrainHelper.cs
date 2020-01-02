// Decompiled with JetBrains decompiler
// Type: CTS.CTSRuntimeTerrainHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CTS
{
  public class CTSRuntimeTerrainHelper : MonoBehaviour
  {
    public CTSProfile m_CTSProfile;
    public bool m_autoApplyProfile;
    public Terrain m_terrain;

    public CTSRuntimeTerrainHelper()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) this).GetComponent<Terrain>();
      if (!this.m_autoApplyProfile)
        return;
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.ApplyProfileToActiveTerrains();
      else
        this.ApplyProfileToTerrain();
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) this).GetComponent<Terrain>();
      if (!this.m_autoApplyProfile)
        return;
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.ApplyProfileToActiveTerrains();
      else
        this.ApplyProfileToTerrain();
    }

    private void OnGenerateCompleted(Terrain terrain)
    {
      if (!Object.op_Inequality((Object) terrain, (Object) null))
        return;
      this.m_terrain = terrain;
      this.ApplyProfileToTerrain();
    }

    public void ApplyProfileToTerrain()
    {
      if (!Object.op_Inequality((Object) this.m_terrain, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.AddCTSToTerrain(this.m_terrain);
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_CTSProfile, this.m_terrain);
    }

    public void ApplyProfileToTerrain(Terrain terrain)
    {
      CTSSingleton<CTSTerrainManager>.Instance.AddCTSToTerrain(terrain);
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_CTSProfile, terrain);
    }

    public void ApplyProfileToActiveTerrains()
    {
      CTSSingleton<CTSTerrainManager>.Instance.AddCTSToAllTerrains();
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_CTSProfile);
    }
  }
}
