// Decompiled with JetBrains decompiler
// Type: ParticleEffectsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsLibrary : MonoBehaviour
{
  public static ParticleEffectsLibrary GlobalAccess;
  public int TotalEffects;
  public int CurrentParticleEffectIndex;
  public int CurrentParticleEffectNum;
  public Vector3[] ParticleEffectSpawnOffsets;
  public float[] ParticleEffectLifetimes;
  public GameObject[] ParticleEffectPrefabs;
  private string effectNameString;
  private List<Transform> currentActivePEList;
  private Vector3 spawnPosition;

  public ParticleEffectsLibrary()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    ParticleEffectsLibrary.GlobalAccess = this;
    this.currentActivePEList = new List<Transform>();
    this.TotalEffects = this.ParticleEffectPrefabs.Length;
    this.CurrentParticleEffectNum = 1;
    if (this.ParticleEffectSpawnOffsets.Length != this.TotalEffects)
      Debug.LogError((object) "ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
    if (this.ParticleEffectPrefabs.Length != this.TotalEffects)
      Debug.LogError((object) "ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
    this.effectNameString = ((Object) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]).get_name() + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
  }

  private void Start()
  {
  }

  public string GetCurrentPENameString()
  {
    return ((Object) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]).get_name() + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
  }

  public void PreviousParticleEffect()
  {
    if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0 && this.currentActivePEList.Count > 0)
    {
      for (int index = 0; index < this.currentActivePEList.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.currentActivePEList[index], (Object) null))
          Object.Destroy((Object) ((Component) this.currentActivePEList[index]).get_gameObject());
      }
      this.currentActivePEList.Clear();
    }
    if (this.CurrentParticleEffectIndex > 0)
      --this.CurrentParticleEffectIndex;
    else
      this.CurrentParticleEffectIndex = this.TotalEffects - 1;
    this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
    this.effectNameString = ((Object) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]).get_name() + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
  }

  public void NextParticleEffect()
  {
    if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0 && this.currentActivePEList.Count > 0)
    {
      for (int index = 0; index < this.currentActivePEList.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.currentActivePEList[index], (Object) null))
          Object.Destroy((Object) ((Component) this.currentActivePEList[index]).get_gameObject());
      }
      this.currentActivePEList.Clear();
    }
    if (this.CurrentParticleEffectIndex < this.TotalEffects - 1)
      ++this.CurrentParticleEffectIndex;
    else
      this.CurrentParticleEffectIndex = 0;
    this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
    this.effectNameString = ((Object) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]).get_name() + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
  }

  public void SpawnParticleEffect(Vector3 positionInWorldToSpawn)
  {
    this.spawnPosition = Vector3.op_Addition(positionInWorldToSpawn, this.ParticleEffectSpawnOffsets[this.CurrentParticleEffectIndex]);
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex], this.spawnPosition, this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].get_transform().get_rotation());
    ((Object) gameObject).set_name("PE_" + (object) this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]);
    if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0)
      this.currentActivePEList.Add(gameObject.get_transform());
    this.currentActivePEList.Add(gameObject.get_transform());
    if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0)
      return;
    Object.Destroy((Object) gameObject, this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex]);
  }
}
