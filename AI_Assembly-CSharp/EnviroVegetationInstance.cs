// Decompiled with JetBrains decompiler
// Type: EnviroVegetationInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Enviro/Vegetation Growth Object")]
public class EnviroVegetationInstance : MonoBehaviour
{
  [HideInInspector]
  public int id;
  public EnviroVegetationAge Age;
  public EnviroVegetationSeasons Seasons;
  public List<EnviroVegetationStage> GrowStages;
  public Vector3 minScale;
  public Vector3 maxScale;
  public float GrowSpeedMod;
  public GameObject DeadPrefab;
  public Color GizmoColor;
  public float GizmoSize;
  private EnviroSeasons.Seasons currentSeason;
  private double ageInHours;
  private double maxAgeInHours;
  private int currentStage;
  private GameObject currentVegetationObject;
  private bool stay;
  private bool reBirth;
  private bool rescale;
  private bool canGrow;
  private bool shrink;

  public EnviroVegetationInstance()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    EnviroSky.instance.RegisterMe(this);
    this.currentSeason = EnviroSky.instance.Seasons.currentSeasons;
    this.maxAgeInHours = EnviroSky.instance.GetInHours(this.Age.maxAgeHours, this.Age.maxAgeDays, this.Age.maxAgeYears);
    EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.SetSeason());
    if (this.Age.randomStartAge)
    {
      this.Age.startAgeinHours = Random.Range(0.0f, (float) this.maxAgeInHours);
      this.Age.randomStartAge = false;
    }
    this.Birth(0, this.Age.startAgeinHours);
  }

  private void OnEnable()
  {
    if (this.GrowStages.Count < 1)
    {
      Debug.LogError((object) "Please setup GrowStages!");
      ((Behaviour) this).set_enabled(false);
    }
    for (int index = 0; index < this.GrowStages.Count; ++index)
    {
      if (Object.op_Equality((Object) this.GrowStages[index].GrowGameobjectAutumn, (Object) null) || Object.op_Equality((Object) this.GrowStages[index].GrowGameobjectSpring, (Object) null) || (Object.op_Equality((Object) this.GrowStages[index].GrowGameobjectSummer, (Object) null) || Object.op_Equality((Object) this.GrowStages[index].GrowGameobjectWinter, (Object) null)))
      {
        Debug.LogError((object) "One ore more GrowStages missing GrowPrefabs!");
        ((Behaviour) this).set_enabled(false);
      }
    }
  }

  private void SetSeason()
  {
    this.currentSeason = EnviroSky.instance.Seasons.currentSeasons;
    this.VegetationChange();
  }

  public void KeepVariablesClear()
  {
    this.GrowStages[0].minAgePercent = 0.0f;
    for (int index = 0; index < this.GrowStages.Count; ++index)
    {
      if ((double) this.GrowStages[index].minAgePercent > 100.0)
        this.GrowStages[index].minAgePercent = 100f;
    }
  }

  public void UpdateInstance()
  {
    if (this.reBirth)
      this.Birth(0, 0.0f);
    if (this.shrink)
      this.ShrinkAndDeactivate();
    if (!this.canGrow)
      return;
    this.UpdateGrowth();
  }

  public void UpdateGrowth()
  {
    this.ageInHours = EnviroSky.instance.currentTimeInHours - this.Age.birthdayInHours;
    this.KeepVariablesClear();
    if (this.stay)
      return;
    if (this.currentStage + 1 < this.GrowStages.Count)
    {
      if (this.maxAgeInHours * ((double) this.GrowStages[this.currentStage + 1].minAgePercent / 100.0) <= this.ageInHours && this.ageInHours > 0.0)
      {
        ++this.currentStage;
        this.VegetationChange();
      }
      else
      {
        if (this.GrowStages[this.currentStage].growAction != EnviroVegetationStage.GrowState.Grow)
          return;
        this.CalculateScale();
      }
    }
    else
    {
      if (this.stay)
        return;
      if (this.ageInHours > this.maxAgeInHours)
      {
        if (this.Age.Loop)
        {
          this.currentVegetationObject.SetActive(false);
          if (Object.op_Inequality((Object) this.DeadPrefab, (Object) null))
            this.DeadPrefabLoop();
          else
            this.Birth(this.Age.LoopFromGrowStage, 0.0f);
        }
        else
          this.stay = true;
      }
      else
      {
        if (this.GrowStages[this.currentStage].growAction != EnviroVegetationStage.GrowState.Grow)
          return;
        this.CalculateScale();
      }
    }
  }

  private void DeadPrefabLoop()
  {
    this.stay = true;
    ((GameObject) Object.Instantiate<GameObject>((M0) this.DeadPrefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform().set_localScale(this.currentVegetationObject.get_transform().get_localScale());
    this.Birth(this.Age.LoopFromGrowStage, 0.0f);
    this.stay = false;
  }

  [DebuggerHidden]
  private IEnumerator BirthColliders()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroVegetationInstance.\u003CBirthColliders\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void CalculateScale()
  {
    if (this.rescale)
    {
      this.currentVegetationObject.get_transform().set_localScale(this.minScale);
      this.rescale = false;
    }
    double num = this.ageInHours / this.maxAgeInHours * (double) this.GrowSpeedMod;
    this.currentVegetationObject.get_transform().set_localScale(Vector3.op_Addition(this.minScale, new Vector3((float) num, (float) num, (float) num)));
    if (this.currentVegetationObject.get_transform().get_localScale().y > this.maxScale.y)
      this.currentVegetationObject.get_transform().set_localScale(this.maxScale);
    if (this.currentVegetationObject.get_transform().get_localScale().y >= this.minScale.y)
      return;
    this.currentVegetationObject.get_transform().set_localScale(this.minScale);
  }

  public void Birth(int stage, float startAge)
  {
    this.Age.birthdayInHours = EnviroSky.instance.currentTimeInHours - (double) startAge;
    startAge = 0.0f;
    this.ageInHours = 0.0;
    this.currentStage = stage;
    this.rescale = true;
    this.reBirth = false;
    this.VegetationChange();
    this.StartCoroutine(this.BirthColliders());
  }

  private void SeasonAction()
  {
    if (this.Seasons.seasonAction == EnviroVegetationSeasons.SeasonAction.SpawnDeadPrefab)
    {
      if (Object.op_Inequality((Object) this.DeadPrefab, (Object) null))
        ((GameObject) Object.Instantiate<GameObject>((M0) this.DeadPrefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform().set_localScale(this.currentVegetationObject.get_transform().get_localScale());
      this.currentVegetationObject.SetActive(false);
    }
    else if (this.Seasons.seasonAction == EnviroVegetationSeasons.SeasonAction.Deactivate)
    {
      this.shrink = true;
    }
    else
    {
      if (this.Seasons.seasonAction != EnviroVegetationSeasons.SeasonAction.Destroy)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }

  private void CheckSeason(bool update)
  {
    if (!update && this.canGrow)
    {
      this.SeasonAction();
      this.canGrow = false;
    }
    else if (update && !this.canGrow)
    {
      this.canGrow = true;
      this.reBirth = true;
    }
    else
    {
      if (update || this.canGrow)
        return;
      this.SeasonAction();
    }
  }

  private void ShrinkAndDeactivate()
  {
    if (this.currentVegetationObject.get_transform().get_localScale().y > this.minScale.y)
    {
      this.currentVegetationObject.get_transform().set_localScale(new Vector3((float) (this.currentVegetationObject.get_transform().get_localScale().x - 0.100000001490116 * (double) Time.get_deltaTime()), (float) (this.currentVegetationObject.get_transform().get_localScale().y - 0.100000001490116 * (double) Time.get_deltaTime()), (float) (this.currentVegetationObject.get_transform().get_localScale().z - 0.100000001490116 * (double) Time.get_deltaTime())));
    }
    else
    {
      this.shrink = false;
      this.currentVegetationObject.SetActive(false);
    }
  }

  public void VegetationChange()
  {
    this.canGrow = true;
    if (Object.op_Inequality((Object) this.currentVegetationObject, (Object) null))
      this.currentVegetationObject.SetActive(false);
    switch (this.currentSeason)
    {
      case EnviroSeasons.Seasons.Spring:
        this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectSpring;
        this.CalculateScale();
        this.currentVegetationObject.SetActive(true);
        if (!this.Seasons.GrowInSpring)
        {
          this.CheckSeason(false);
          break;
        }
        if (!this.Seasons.GrowInSpring)
          break;
        this.CheckSeason(true);
        break;
      case EnviroSeasons.Seasons.Summer:
        this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectSummer;
        this.CalculateScale();
        this.currentVegetationObject.SetActive(true);
        if (!this.Seasons.GrowInSummer)
        {
          this.CheckSeason(false);
          break;
        }
        if (!this.Seasons.GrowInSummer)
          break;
        this.CheckSeason(true);
        break;
      case EnviroSeasons.Seasons.Autumn:
        this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectAutumn;
        this.CalculateScale();
        this.currentVegetationObject.SetActive(true);
        if (!this.Seasons.GrowInAutumn)
        {
          this.CheckSeason(false);
          break;
        }
        if (!this.Seasons.GrowInAutumn)
          break;
        this.CheckSeason(true);
        break;
      case EnviroSeasons.Seasons.Winter:
        this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectWinter;
        this.CalculateScale();
        this.currentVegetationObject.SetActive(true);
        if (!this.Seasons.GrowInWinter)
        {
          this.CheckSeason(false);
          break;
        }
        if (!this.Seasons.GrowInWinter)
          break;
        this.CheckSeason(true);
        break;
    }
  }

  private void LateUpdate()
  {
    if (!this.GrowStages[this.currentStage].billboard || !this.canGrow)
      return;
    ((Component) this).get_transform().set_rotation(((Component) Camera.get_main()).get_transform().get_rotation());
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(this.GizmoColor);
    Gizmos.DrawCube(((Component) this).get_transform().get_position(), new Vector3(this.GizmoSize, this.GizmoSize, this.GizmoSize));
  }
}
