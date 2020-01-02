// Decompiled with JetBrains decompiler
// Type: CTS.CTSTerrainManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace CTS
{
  public class CTSTerrainManager : CTSSingleton<CTSTerrainManager>
  {
    private HashSet<CompleteTerrainShader> m_shaderSet = new HashSet<CompleteTerrainShader>();
    private HashSet<CTSWeatherController> m_controllerSet = new HashSet<CTSWeatherController>();

    protected CTSTerrainManager()
    {
    }

    public void RegisterShader(CompleteTerrainShader shader)
    {
      this.m_shaderSet.Add(shader);
    }

    public void UnregisterShader(CompleteTerrainShader shader)
    {
      this.m_shaderSet.Remove(shader);
    }

    public void RegisterWeatherController(CTSWeatherController weatherController)
    {
      this.m_controllerSet.Add(weatherController);
    }

    public void UnregisterWeatherController(CTSWeatherController weatherController)
    {
      this.m_controllerSet.Remove(weatherController);
    }

    public void AddCTSToAllTerrains()
    {
      foreach (Terrain activeTerrain in Terrain.get_activeTerrains())
      {
        if (Object.op_Equality((Object) ((Component) activeTerrain).get_gameObject().GetComponent<CompleteTerrainShader>(), (Object) null))
        {
          ((Component) activeTerrain).get_gameObject().AddComponent<CompleteTerrainShader>();
          CompleteTerrainShader.SetDirty((Object) activeTerrain, false, false);
        }
      }
    }

    public void AddCTSToTerrain(Terrain terrain)
    {
      if (Object.op_Equality((Object) terrain, (Object) null) || !Object.op_Equality((Object) ((Component) terrain).get_gameObject().GetComponent<CompleteTerrainShader>(), (Object) null))
        return;
      ((Component) terrain).get_gameObject().AddComponent<CompleteTerrainShader>();
      CompleteTerrainShader.SetDirty((Object) terrain, false, false);
    }

    public bool ProfileIsActive(CTSProfile profile)
    {
      if (Object.op_Equality((Object) profile, (Object) null))
        return false;
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
      {
        if (Object.op_Inequality((Object) shader.Profile, (Object) null) && ((Object) shader.Profile).GetInstanceID() == ((Object) profile).GetInstanceID())
          return true;
      }
      return false;
    }

    public void BroadcastProfileSelect(CTSProfile profile)
    {
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
        shader.Profile = profile;
    }

    public void BroadcastProfileSelect(CTSProfile profile, Terrain terrain)
    {
      if (Object.op_Equality((Object) profile, (Object) null) || Object.op_Equality((Object) terrain, (Object) null))
        return;
      CompleteTerrainShader completeTerrainShader = (CompleteTerrainShader) ((Component) terrain).get_gameObject().GetComponent<CompleteTerrainShader>();
      if (Object.op_Equality((Object) completeTerrainShader, (Object) null))
        completeTerrainShader = (CompleteTerrainShader) ((Component) terrain).get_gameObject().AddComponent<CompleteTerrainShader>();
      completeTerrainShader.Profile = profile;
    }

    public void BroadcastProfileUpdate(CTSProfile profile)
    {
      if (Object.op_Equality((Object) profile, (Object) null))
      {
        Debug.LogWarning((object) "Cannot update shader on null profile.");
      }
      else
      {
        foreach (CompleteTerrainShader shader in this.m_shaderSet)
        {
          if (Object.op_Inequality((Object) shader.Profile, (Object) null) && ((Object) shader.Profile).get_name() == ((Object) profile).get_name())
            shader.UpdateShader();
        }
      }
    }

    public void BroadcastShaderSetup(CTSProfile profile)
    {
      if (Object.op_Inequality((Object) Terrain.get_activeTerrain(), (Object) null))
      {
        CompleteTerrainShader component = (CompleteTerrainShader) ((Component) Terrain.get_activeTerrain()).GetComponent<CompleteTerrainShader>();
        if (Object.op_Inequality((Object) component, (Object) null) && Object.op_Inequality((Object) component.Profile, (Object) null) && ((Object) component.Profile).get_name() == ((Object) profile).get_name())
        {
          component.UpdateProfileFromTerrainForced();
          this.BroadcastProfileUpdate(profile);
          return;
        }
      }
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
      {
        if (Object.op_Inequality((Object) shader.Profile, (Object) null))
        {
          if (Object.op_Equality((Object) profile, (Object) null))
            shader.UpdateProfileFromTerrainForced();
          else if (((Object) shader.Profile).get_name() == ((Object) profile).get_name())
          {
            shader.UpdateProfileFromTerrainForced();
            this.BroadcastProfileUpdate(profile);
            break;
          }
        }
      }
    }

    public void BroadcastBakeTerrains()
    {
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
      {
        if (shader.AutoBakeNormalMap)
          shader.BakeTerrainNormals();
        if (shader.AutoBakeColorMap)
        {
          if (!shader.AutoBakeGrassIntoColorMap)
            shader.BakeTerrainBaseMap();
          else
            shader.BakeTerrainBaseMapWithGrass();
        }
      }
    }

    public void BroadcastAlbedoTextureSwitch(
      CTSProfile profile,
      Texture2D texture,
      int textureIdx,
      float tiling)
    {
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
      {
        if (Object.op_Inequality((Object) shader.Profile, (Object) null))
        {
          if (Object.op_Equality((Object) profile, (Object) null))
            shader.ReplaceAlbedoInTerrain(texture, textureIdx, tiling);
          else if (((Object) shader.Profile).get_name() == ((Object) profile).get_name())
            shader.ReplaceAlbedoInTerrain(texture, textureIdx, tiling);
        }
      }
    }

    public void BroadcastNormalTextureSwitch(
      CTSProfile profile,
      Texture2D texture,
      int textureIdx,
      float tiling)
    {
      foreach (CompleteTerrainShader shader in this.m_shaderSet)
      {
        if (Object.op_Inequality((Object) shader.Profile, (Object) null))
        {
          if (Object.op_Equality((Object) profile, (Object) null))
            shader.ReplaceNormalInTerrain(texture, textureIdx, tiling);
          else if (((Object) shader.Profile).get_name() == ((Object) profile).get_name())
            shader.ReplaceNormalInTerrain(texture, textureIdx, tiling);
        }
      }
    }

    public void BroadcastWeatherUpdate(CTSWeatherManager manager)
    {
      foreach (CTSWeatherController controller in this.m_controllerSet)
        controller.ProcessWeatherUpdate(manager);
    }

    public void RemoveWorldSeams()
    {
      if (this.m_shaderSet.Count <= 0)
        return;
      using (HashSet<CompleteTerrainShader>.Enumerator enumerator = this.m_shaderSet.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        enumerator.Current.RemoveWorldSeams();
      }
    }
  }
}
