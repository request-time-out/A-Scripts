// Decompiled with JetBrains decompiler
// Type: EnviroSeasonObjectSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Enviro/Utility/Seasons for GameObjects")]
public class EnviroSeasonObjectSwitcher : MonoBehaviour
{
  public GameObject SpringObject;
  public GameObject SummerObject;
  public GameObject AutumnObject;
  public GameObject WinterObject;

  public EnviroSeasonObjectSwitcher()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SwitchSeasonObject();
    EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.SwitchSeasonObject());
  }

  private void OnEnable()
  {
    if (Object.op_Equality((Object) this.SpringObject, (Object) null))
    {
      Debug.LogError((object) "Please assign a spring Object in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (Object.op_Equality((Object) this.SummerObject, (Object) null))
    {
      Debug.LogError((object) "Please assign a summer Object in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (Object.op_Equality((Object) this.AutumnObject, (Object) null))
    {
      Debug.LogError((object) "Please assign a autumn Object in Inspector!");
      ((Behaviour) this).set_enabled(false);
    }
    if (!Object.op_Equality((Object) this.WinterObject, (Object) null))
      return;
    Debug.LogError((object) "Please assign a winter Object in Inspector!");
    ((Behaviour) this).set_enabled(false);
  }

  private void SwitchSeasonObject()
  {
    switch (EnviroSky.instance.Seasons.currentSeasons)
    {
      case EnviroSeasons.Seasons.Spring:
        this.SummerObject.SetActive(false);
        this.AutumnObject.SetActive(false);
        this.WinterObject.SetActive(false);
        this.SpringObject.SetActive(true);
        break;
      case EnviroSeasons.Seasons.Summer:
        this.SpringObject.SetActive(false);
        this.AutumnObject.SetActive(false);
        this.WinterObject.SetActive(false);
        this.SummerObject.SetActive(true);
        break;
      case EnviroSeasons.Seasons.Autumn:
        this.SpringObject.SetActive(false);
        this.SummerObject.SetActive(false);
        this.WinterObject.SetActive(false);
        this.AutumnObject.SetActive(true);
        break;
      case EnviroSeasons.Seasons.Winter:
        this.SpringObject.SetActive(false);
        this.SummerObject.SetActive(false);
        this.AutumnObject.SetActive(false);
        this.WinterObject.SetActive(true);
        break;
    }
  }
}
