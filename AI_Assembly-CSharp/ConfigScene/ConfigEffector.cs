// Decompiled with JetBrains decompiler
// Type: ConfigScene.ConfigEffector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ConfigScene
{
  [DisallowMultipleComponent]
  public class ConfigEffector : MonoBehaviour
  {
    public PlaceholderSoftware.WetStuff.WetStuff Wetstuff;
    public PostProcessLayer PostProcessLayer;
    private List<Bloom> _bloom;
    private List<AmbientOcclusion> _ao;
    private List<ScreenSpaceReflections> _ssr;
    private List<DepthOfField> _dof;
    private List<Vignette> _vignette;
    public bool isInput;

    public ConfigEffector()
    {
      base.\u002Ector();
    }

    private void Refresh()
    {
      GraphicSystem graphicData = Manager.Config.GraphicData;
      using (List<Bloom>.Enumerator enumerator = this._bloom.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Bloom current = enumerator.Current;
          if (((PostProcessEffectSettings) current).active != (graphicData.Bloom ? 1 : 0))
            ((PostProcessEffectSettings) current).active = (__Null) (graphicData.Bloom ? 1 : 0);
        }
      }
      using (List<AmbientOcclusion>.Enumerator enumerator = this._ao.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AmbientOcclusion current = enumerator.Current;
          if (((PostProcessEffectSettings) current).active != (graphicData.SSAO ? 1 : 0))
            ((PostProcessEffectSettings) current).active = (__Null) (graphicData.SSAO ? 1 : 0);
        }
      }
      using (List<ScreenSpaceReflections>.Enumerator enumerator = this._ssr.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ScreenSpaceReflections current = enumerator.Current;
          if (((PostProcessEffectSettings) current).active != (graphicData.SSR ? 1 : 0))
            ((PostProcessEffectSettings) current).active = (__Null) (graphicData.SSR ? 1 : 0);
        }
      }
      using (List<DepthOfField>.Enumerator enumerator = this._dof.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DepthOfField current = enumerator.Current;
          if (((PostProcessEffectSettings) current).active != (graphicData.DepthOfField ? 1 : 0))
            ((PostProcessEffectSettings) current).active = (__Null) (graphicData.DepthOfField ? 1 : 0);
        }
      }
      using (List<Vignette>.Enumerator enumerator = this._vignette.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Vignette current = enumerator.Current;
          if (((PostProcessEffectSettings) current).active != (graphicData.Vignette ? 1 : 0))
            ((PostProcessEffectSettings) current).active = (__Null) (graphicData.Vignette ? 1 : 0);
        }
      }
      if (Singleton<Manager.Map>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null) && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator.EnviroSky, (Object) null))
      {
        EnviroSky enviroSky = Singleton<Manager.Map>.Instance.Simulator.EnviroSky;
        enviroSky.fogSettings.distanceFog = graphicData.Atmospheric;
        enviroSky.fogSettings.heightFog = graphicData.Atmospheric;
        enviroSky.volumeLighting = graphicData.Atmospheric;
        enviroSky.LightShafts.sunLightShafts = graphicData.Atmospheric;
        enviroSky.LightShafts.moonLightShafts = graphicData.Atmospheric;
      }
      if (!Object.op_Inequality((Object) this.Wetstuff, (Object) null) || ((Behaviour) this.Wetstuff).get_enabled() == graphicData.Rain)
        return;
      ((Behaviour) this.Wetstuff).set_enabled(graphicData.Rain);
    }

    private void Reset()
    {
      this.PostProcessLayer = (PostProcessLayer) ((Component) this).GetComponent<PostProcessLayer>();
      this.Wetstuff = (PlaceholderSoftware.WetStuff.WetStuff) ((Component) this).GetComponent<PlaceholderSoftware.WetStuff.WetStuff>();
    }

    private void Awake()
    {
    }

    private void Start()
    {
      List<PostProcessVolume> postProcessVolumeList = ListPool<PostProcessVolume>.Get();
      PostProcessManager.get_instance().GetActiveVolumes(this.PostProcessLayer, postProcessVolumeList, true, true);
      using (List<PostProcessVolume>.Enumerator enumerator = postProcessVolumeList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PostProcessVolume current = enumerator.Current;
          Bloom setting1 = (Bloom) current.get_profile().GetSetting<Bloom>();
          if (Object.op_Implicit((Object) setting1))
            this._bloom.Add(setting1);
          AmbientOcclusion setting2 = (AmbientOcclusion) current.get_profile().GetSetting<AmbientOcclusion>();
          if (Object.op_Inequality((Object) setting2, (Object) null) && ((PostProcessEffectSettings) setting2).active != null)
            this._ao.Add(setting2);
          ScreenSpaceReflections setting3 = (ScreenSpaceReflections) current.get_profile().GetSetting<ScreenSpaceReflections>();
          if (Object.op_Inequality((Object) setting3, (Object) null) && ((PostProcessEffectSettings) setting3).active != null)
            this._ssr.Add(setting3);
          DepthOfField setting4 = (DepthOfField) current.get_profile().GetSetting<DepthOfField>();
          if (Object.op_Inequality((Object) setting4, (Object) null) && ((PostProcessEffectSettings) setting4).active != null)
            this._dof.Add(setting4);
          Vignette setting5 = (Vignette) current.get_profile().GetSetting<Vignette>();
          if (Object.op_Inequality((Object) setting5, (Object) null) && ((PostProcessEffectSettings) setting5).active != null)
            this._vignette.Add(setting5);
        }
      }
      this.Refresh();
    }

    private void Update()
    {
      this.Refresh();
    }
  }
}
