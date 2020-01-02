// Decompiled with JetBrains decompiler
// Type: CTS.CTSWeatherController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CTS
{
  public class CTSWeatherController : MonoBehaviour
  {
    private Terrain m_terrain;

    public CTSWeatherController()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      CTSSingleton<CTSTerrainManager>.Instance.RegisterWeatherController(this);
    }

    private void OnDisable()
    {
      CTSSingleton<CTSTerrainManager>.Instance.UnregisterWeatherController(this);
    }

    public void ProcessWeatherUpdate(CTSWeatherManager manager)
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) this).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.Log((object) "CTS Weather Controller must be connected to a terrain to work.");
          return;
        }
      }
      if (this.m_terrain.get_materialType() != 3)
      {
        Debug.Log((object) "CTS Weather Controller needs a CTS Material to work with.");
      }
      else
      {
        Material materialTemplate = this.m_terrain.get_materialTemplate();
        if (Object.op_Equality((Object) materialTemplate, (Object) null))
        {
          Debug.Log((object) "CTS Weather Controller needs a Custom Material to work with.");
        }
        else
        {
          materialTemplate.SetFloat(CTSShaderID.Snow_Amount, manager.SnowPower * 2f);
          materialTemplate.SetFloat(CTSShaderID.Snow_Min_Height, manager.SnowMinHeight);
          float num = manager.RainPower * manager.MaxRainSmoothness;
          materialTemplate.SetFloat(CTSShaderID.Snow_Smoothness, num);
          Color.get_white();
          Color color = (double) manager.Season >= 1.0 ? ((double) manager.Season >= 2.0 ? ((double) manager.Season >= 3.0 ? Color.Lerp(manager.AutumnTint, manager.WinterTint, manager.Season - 3f) : Color.Lerp(manager.SummerTint, manager.AutumnTint, manager.Season - 2f)) : Color.Lerp(manager.SpringTint, manager.SummerTint, manager.Season - 1f)) : Color.Lerp(manager.WinterTint, manager.SpringTint, manager.Season);
          for (int index = 0; index < 16; ++index)
            materialTemplate.SetVector(CTSShaderID.Texture_X_Color[index], new Vector4((float) color.r, (float) color.g, (float) color.b, num));
        }
      }
    }
  }
}
