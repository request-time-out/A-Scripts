// Decompiled with JetBrains decompiler
// Type: AIProject.SearchPointParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using UnityEngine;

namespace AIProject
{
  public class SearchPointParticle : MonoBehaviour
  {
    [SerializeField]
    private int _id;
    [SerializeField]
    private ParticleSystem[] _particleSystems;

    public SearchPointParticle()
    {
      base.\u002Ector();
    }

    public int ID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    private void Update()
    {
      Environment.SearchActionInfo searchActionInfo;
      if (!Singleton<Game>.IsInstance() || !Singleton<Game>.Instance.Environment?.SearchActionLockTable.TryGetValue(this._id, out searchActionInfo))
        return;
      foreach (ParticleSystem particleSystem in this._particleSystems)
      {
        if (!Object.op_Equality((Object) particleSystem, (Object) null))
        {
          ParticleSystem.EmissionModule emission = particleSystem.get_emission();
          bool enabled = ((ParticleSystem.EmissionModule) ref emission).get_enabled();
          bool flag = searchActionInfo.Count <= 0;
          if (enabled != flag)
            ((ParticleSystem.EmissionModule) ref emission).set_enabled(flag);
        }
      }
    }
  }
}
