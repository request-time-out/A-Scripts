// Decompiled with JetBrains decompiler
// Type: HParticleCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

public class HParticleCtrl
{
  private List<HParticleCtrl.ParticleInfo> lstParticle = new List<HParticleCtrl.ParticleInfo>();
  public Transform particlePlace;
  private GameObject objParent;
  private Resources.HSceneTables HSceneTables;

  public void Init()
  {
    this.HSceneTables = Singleton<Resources>.Instance.HSceneTable;
    this.lstParticle = new List<HParticleCtrl.ParticleInfo>();
    for (int index = 0; index < this.HSceneTables.lstHParticleCtrl.Count; ++index)
    {
      this.lstParticle.Add(new HParticleCtrl.ParticleInfo()
      {
        assetPath = this.HSceneTables.lstHParticleCtrl[index].assetPath,
        file = this.HSceneTables.lstHParticleCtrl[index].file,
        manifest = this.HSceneTables.lstHParticleCtrl[index].manifest,
        numParent = this.HSceneTables.lstHParticleCtrl[index].numParent,
        nameParent = this.HSceneTables.lstHParticleCtrl[index].nameParent,
        pos = this.HSceneTables.lstHParticleCtrl[index].pos,
        rot = this.HSceneTables.lstHParticleCtrl[index].rot
      });
      GameObject gameObject = CommonLib.LoadAsset<GameObject>(this.lstParticle[index].assetPath, this.lstParticle[index].file, true, this.lstParticle[index].manifest);
      Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(this.lstParticle[index].assetPath);
      this.lstParticle[index].particle = (ParticleSystem) gameObject.GetComponent<ParticleSystem>();
      this.lstParticle[index].particleCache.Item1 = (__Null) gameObject;
      ((GameObject) this.lstParticle[index].particleCache.Item1).SetActive(false);
      this.lstParticle[index].particleCache.Item2 = (__Null) gameObject.get_transform();
      ((Transform) this.lstParticle[index].particleCache.Item2).SetParent(this.particlePlace, false);
      ((Transform) this.lstParticle[index].particleCache.Item2).set_localPosition(Vector3.get_zero());
      ((Transform) this.lstParticle[index].particleCache.Item2).set_localRotation(Quaternion.get_identity());
      ((Transform) this.lstParticle[index].particleCache.Item2).set_localScale(Vector3.get_one());
    }
  }

  public void ParticleLoad(GameObject _objBody, int _sex)
  {
    if (Object.op_Equality((Object) _objBody, (Object) null))
      return;
    for (int index = 0; index < this.lstParticle.Count; ++index)
    {
      if (this.lstParticle[index].numParent == _sex)
      {
        this.objParent = _objBody.get_transform().FindLoop(this.lstParticle[index].nameParent);
        if (!Object.op_Equality((Object) this.objParent, (Object) null))
        {
          ((GameObject) this.lstParticle[index].particleCache.Item1).SetActive(true);
          this.lstParticle[index].particle.Stop();
          ((Transform) this.lstParticle[index].particleCache.Item2).SetParent(this.objParent.get_transform(), false);
          ((Transform) this.lstParticle[index].particleCache.Item2).set_localPosition(this.lstParticle[index].pos);
          ((Transform) this.lstParticle[index].particleCache.Item2).set_localRotation(Quaternion.Euler(this.lstParticle[index].rot));
          ((Transform) this.lstParticle[index].particleCache.Item2).set_localScale(Vector3.get_one());
        }
      }
    }
  }

  public bool ReleaseObject(int _sex)
  {
    for (int index = 0; index < this.lstParticle.Count; ++index)
    {
      if (this.lstParticle[index] != null && this.lstParticle[index].numParent == _sex)
      {
        Object.Destroy((Object) this.lstParticle[index].particleCache.Item1);
        this.lstParticle[index].particle = (ParticleSystem) null;
        this.lstParticle[index].particleCache.Item1 = null;
        this.lstParticle[index].particleCache.Item2 = null;
      }
    }
    return true;
  }

  public bool ReleaseObject()
  {
    for (int index = 0; index < this.lstParticle.Count; ++index)
    {
      if (this.lstParticle[index] != null && !Object.op_Equality((Object) this.lstParticle[index].particle, (Object) null))
      {
        Object.Destroy((Object) this.lstParticle[index].particleCache.Item1);
        this.lstParticle[index].particle = (ParticleSystem) null;
        this.lstParticle[index].particleCache.Item1 = null;
        this.lstParticle[index].particleCache.Item2 = null;
      }
    }
    return true;
  }

  public bool RePlaceObject()
  {
    for (int index = 0; index < this.lstParticle.Count; ++index)
    {
      if (this.lstParticle[index] != null && !Object.op_Equality((Object) this.lstParticle[index].particle, (Object) null) && Object.op_Inequality((Object) ((Transform) this.lstParticle[index].particleCache.Item2).get_parent(), (Object) this.particlePlace))
      {
        ((GameObject) this.lstParticle[index].particleCache.Item1).SetActive(false);
        ((Transform) this.lstParticle[index].particleCache.Item2).SetParent(this.particlePlace, false);
        ((Transform) this.lstParticle[index].particleCache.Item2).set_localPosition(Vector3.get_zero());
        ((Transform) this.lstParticle[index].particleCache.Item2).set_localRotation(Quaternion.get_identity());
        ((Transform) this.lstParticle[index].particleCache.Item2).set_localScale(Vector3.get_one());
      }
    }
    return true;
  }

  public bool Play(int _particle)
  {
    if (this.lstParticle.Count <= _particle || Object.op_Equality((Object) this.lstParticle[_particle].particle, (Object) null))
      return false;
    this.lstParticle[_particle].particle.Simulate(0.0f);
    this.lstParticle[_particle].particle.Play();
    return true;
  }

  public bool IsPlaying(int _particle)
  {
    return this.lstParticle.Count > _particle && !Object.op_Equality((Object) this.lstParticle[_particle].particle, (Object) null) && this.lstParticle[_particle].particle.get_isPlaying();
  }

  public class ParticleInfo
  {
    public string assetPath;
    public string file;
    public string manifest;
    public int numParent;
    public string nameParent;
    public Vector3 pos;
    public Vector3 rot;
    public ParticleSystem particle;
    public ValueTuple<GameObject, Transform> particleCache;
  }
}
