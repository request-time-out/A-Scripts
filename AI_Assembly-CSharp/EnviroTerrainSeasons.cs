// Decompiled with JetBrains decompiler
// Type: EnviroTerrainSeasons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enviro/Utility/Seasons for Terrains")]
public class EnviroTerrainSeasons : MonoBehaviour
{
  public Terrain terrain;
  [Header("Terrain Textures")]
  public bool ChangeTextures;
  public List<EnviroTerrainSeasonsChangeOrder> TextureChanges;
  [Header("Grass Tint")]
  public bool ChangeGrassTint;
  public Color SpringGrassColor;
  public Color SummerGrassColor;
  public Color AutumnGrassColor;
  public Color WinterGrassColor;
  [Header("Grass Wind")]
  public bool ChangeGrassWind;
  public float windSpeedModificator;
  public float windSizeModificator;
  private SplatPrototype[] textureInSplats;
  private SplatPrototype[] texturesIn;

  public EnviroTerrainSeasons()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.terrain, (Object) null))
      this.terrain = (Terrain) ((Component) this).GetComponent<Terrain>();
    this.texturesIn = this.terrain.get_terrainData().get_splatPrototypes();
    this.UpdateSeason();
    EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.UpdateSeason());
  }

  private void OnEnable()
  {
    if (!this.ChangeTextures)
      return;
    for (int index = 0; index < this.TextureChanges.Count; ++index)
    {
      if (Object.op_Equality((Object) this.TextureChanges[index].SpringTexture, (Object) null))
      {
        Debug.LogError((object) "Please assign a spring texture in Inspector!");
        ((Behaviour) this).set_enabled(false);
      }
      if (Object.op_Equality((Object) this.TextureChanges[index].SummerTexture, (Object) null))
      {
        Debug.LogError((object) "Please assign a summer texture in Inspector!");
        ((Behaviour) this).set_enabled(false);
      }
      if (Object.op_Equality((Object) this.TextureChanges[index].AutumnTexture, (Object) null))
      {
        Debug.LogError((object) "Please assign a autumn texture in Inspector!");
        ((Behaviour) this).set_enabled(false);
      }
      if (Object.op_Equality((Object) this.TextureChanges[index].WinterTexture, (Object) null))
      {
        Debug.LogError((object) "Please assign a winter texture in Inspector!");
        ((Behaviour) this).set_enabled(false);
      }
      if (this.TextureChanges[index].terrainTextureID < 0)
      {
        Debug.LogError((object) "Please configure Texture ChangeSlot IDs!");
        ((Behaviour) this).set_enabled(false);
      }
    }
  }

  private void ChangeGrassColor(Color ChangeToColor)
  {
    this.terrain.get_terrainData().set_wavingGrassTint(ChangeToColor);
  }

  private void ChangeTexture(Texture2D inTexture, int id, Vector2 tiling)
  {
    this.textureInSplats = this.texturesIn;
    this.textureInSplats[id].set_texture(inTexture);
    this.textureInSplats[id].set_tileSize(tiling);
    this.terrain.get_terrainData().set_splatPrototypes(this.textureInSplats);
  }

  private void ChangeTexture(Texture2D inTexture, Texture2D inNormal, int id, Vector2 tiling)
  {
    this.textureInSplats = this.texturesIn;
    this.textureInSplats[id].set_texture(inTexture);
    this.textureInSplats[id].set_normalMap(inNormal);
    this.textureInSplats[id].set_tileSize(tiling);
    this.terrain.get_terrainData().set_splatPrototypes(this.textureInSplats);
  }

  private void UpdateSeason()
  {
    switch (EnviroSky.instance.Seasons.currentSeasons)
    {
      case EnviroSeasons.Seasons.Spring:
        for (int index = 0; index < this.TextureChanges.Count; ++index)
        {
          if (this.ChangeTextures)
          {
            if (Object.op_Inequality((Object) this.TextureChanges[index].SpringNormal, (Object) null))
              this.ChangeTexture(this.TextureChanges[index].SpringTexture, this.TextureChanges[index].SpringNormal, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            else
              this.ChangeTexture(this.TextureChanges[index].SpringTexture, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            this.terrain.Flush();
          }
        }
        if (!this.ChangeGrassTint)
          break;
        this.ChangeGrassColor(this.SpringGrassColor);
        break;
      case EnviroSeasons.Seasons.Summer:
        for (int index = 0; index < this.TextureChanges.Count; ++index)
        {
          if (this.ChangeTextures)
          {
            if (Object.op_Inequality((Object) this.TextureChanges[index].SummerNormal, (Object) null))
              this.ChangeTexture(this.TextureChanges[index].SummerTexture, this.TextureChanges[index].SummerNormal, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            else
              this.ChangeTexture(this.TextureChanges[index].SummerTexture, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            this.terrain.Flush();
          }
        }
        if (!this.ChangeGrassTint)
          break;
        this.ChangeGrassColor(this.SummerGrassColor);
        break;
      case EnviroSeasons.Seasons.Autumn:
        for (int index = 0; index < this.TextureChanges.Count; ++index)
        {
          if (this.ChangeTextures)
          {
            if (Object.op_Inequality((Object) this.TextureChanges[index].AutumnNormal, (Object) null))
              this.ChangeTexture(this.TextureChanges[index].AutumnTexture, this.TextureChanges[index].AutumnNormal, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            else
              this.ChangeTexture(this.TextureChanges[index].AutumnTexture, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            this.terrain.Flush();
          }
        }
        if (!this.ChangeGrassTint)
          break;
        this.ChangeGrassColor(this.AutumnGrassColor);
        break;
      case EnviroSeasons.Seasons.Winter:
        for (int index = 0; index < this.TextureChanges.Count; ++index)
        {
          if (this.ChangeTextures)
          {
            if (Object.op_Inequality((Object) this.TextureChanges[index].WinterNormal, (Object) null))
              this.ChangeTexture(this.TextureChanges[index].WinterTexture, this.TextureChanges[index].WinterNormal, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            else
              this.ChangeTexture(this.TextureChanges[index].WinterTexture, this.TextureChanges[index].terrainTextureID, this.TextureChanges[index].tiling);
            this.terrain.Flush();
          }
        }
        if (!this.ChangeGrassTint)
          break;
        this.ChangeGrassColor(this.WinterGrassColor);
        break;
    }
  }

  private void Update()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null) || !this.ChangeGrassWind || !Object.op_Inequality((Object) EnviroSky.instance.Weather.currentActiveWeatherPreset, (Object) null))
      return;
    this.terrain.get_terrainData().set_wavingGrassStrength(EnviroSky.instance.Weather.currentActiveWeatherPreset.WindStrenght * this.windSpeedModificator);
    this.terrain.get_terrainData().set_wavingGrassSpeed(EnviroSky.instance.Weather.currentActiveWeatherPreset.WindStrenght * this.windSizeModificator);
  }
}
