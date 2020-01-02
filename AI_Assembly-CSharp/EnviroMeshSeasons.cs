// Decompiled with JetBrains decompiler
// Type: EnviroMeshSeasons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Enviro/Utility/Seasons for Meshes")]
public class EnviroMeshSeasons : MonoBehaviour
{
  public Material SpringMaterial;
  public Material SummerMaterial;
  public Material AutumnMaterial;
  public Material WinterMaterial;
  private MeshRenderer myRenderer;

  public EnviroMeshSeasons()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.myRenderer = (MeshRenderer) ((Component) this).GetComponent<MeshRenderer>();
    if (Object.op_Equality((Object) this.myRenderer, (Object) null))
    {
      Debug.LogError((object) "Please correct script placement! We need a MeshRenderer to work with!");
      ((Behaviour) this).set_enabled(false);
    }
    this.UpdateSeasonMaterial();
    EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.UpdateSeasonMaterial());
  }

  private void OnEnable()
  {
    if (Object.op_Equality((Object) this.SpringMaterial, (Object) null))
    {
      Debug.LogError((object) "Please assign a spring material in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (Object.op_Equality((Object) this.SummerMaterial, (Object) null))
    {
      Debug.LogError((object) "Please assign a summer material in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (Object.op_Equality((Object) this.AutumnMaterial, (Object) null))
    {
      Debug.LogError((object) "Please assign a autumn material in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (!Object.op_Equality((Object) this.WinterMaterial, (Object) null))
      return;
    Debug.LogError((object) "Please assign a winter material in Inspector!");
    ((Behaviour) this).set_enabled(false);
  }

  private void UpdateSeasonMaterial()
  {
    switch (EnviroSky.instance.Seasons.currentSeasons)
    {
      case EnviroSeasons.Seasons.Spring:
        ((Renderer) this.myRenderer).set_sharedMaterial(this.SpringMaterial);
        break;
      case EnviroSeasons.Seasons.Summer:
        ((Renderer) this.myRenderer).set_sharedMaterial(this.SummerMaterial);
        break;
      case EnviroSeasons.Seasons.Autumn:
        ((Renderer) this.myRenderer).set_sharedMaterial(this.AutumnMaterial);
        break;
      case EnviroSeasons.Seasons.Winter:
        ((Renderer) this.myRenderer).set_sharedMaterial(this.WinterMaterial);
        break;
    }
  }
}
