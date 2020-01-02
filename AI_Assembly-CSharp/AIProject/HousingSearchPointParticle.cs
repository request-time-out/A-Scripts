// Decompiled with JetBrains decompiler
// Type: AIProject.HousingSearchPointParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using UnityEngine;

namespace AIProject
{
  public class HousingSearchPointParticle : MonoBehaviour
  {
    [SerializeField]
    private ParticleSystem[] _particleSystems;
    private SearchActionPoint _searchActionPoint;

    public HousingSearchPointParticle()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this._searchActionPoint = (SearchActionPoint) ((Component) this).GetComponent<SearchActionPoint>();
    }

    private void Update()
    {
      Environment.SearchActionInfo searchActionInfo;
      if (!Singleton<Game>.IsInstance() || Object.op_Equality((Object) this._searchActionPoint, (Object) null) || !Singleton<Game>.Instance.Environment?.SearchActionLockTable.TryGetValue(this._searchActionPoint.RegisterID, out searchActionInfo))
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
