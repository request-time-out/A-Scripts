// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeCore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeepSky.Haze
{
  [ExecuteInEditMode]
  [AddComponentMenu("DeepSky Haze/Controller", 51)]
  public class DS_HazeCore : MonoBehaviour
  {
    public static string kVersionStr = "DeepSky Haze v1.4.0";
    private static int kGUIHeight = 180;
    private static DS_HazeCore instance;
    [SerializeField]
    [Range(0.0f, 1f)]
    [Tooltip("The time at which Zones will evaluate their settings. Animate this or set in code to create time-of-day transitions.")]
    private float m_Time;
    [SerializeField]
    [Tooltip("The height falloff method to use globally (default Exponential).")]
    private DS_HazeCore.HeightFalloffType m_HeightFalloff;
    [SerializeField]
    private List<DS_HazeZone> m_Zones;
    [SerializeField]
    private DS_HazeCore.DebugGUIPosition m_DebugGUIPosition;
    private HashSet<DS_HazeLightVolume> m_LightVolumes;
    [SerializeField]
    private Texture3D m_NoiseLUT;
    [SerializeField]
    private bool m_ShowDebugGUI;
    private Vector2 m_GUIScrollPosition;
    private int m_GUISelectedView;
    private bool m_GUISelectionPopup;
    private DS_HazeView m_GUIDisplayedView;

    public DS_HazeCore()
    {
      base.\u002Ector();
    }

    public static DS_HazeCore Instance
    {
      get
      {
        if (Object.op_Equality((Object) DS_HazeCore.instance, (Object) null))
          DS_HazeCore.instance = (DS_HazeCore) Object.FindObjectOfType<DS_HazeCore>();
        return DS_HazeCore.instance;
      }
    }

    public float Time
    {
      get
      {
        return this.m_Time;
      }
      set
      {
        this.m_Time = Mathf.Clamp01(value);
      }
    }

    public Texture3D NoiseLUT
    {
      get
      {
        return this.m_NoiseLUT;
      }
    }

    public DS_HazeCore.HeightFalloffType HeightFalloff
    {
      get
      {
        return this.m_HeightFalloff;
      }
      set
      {
        this.m_HeightFalloff = value;
        this.SetGlobalHeightFalloff();
      }
    }

    private void SetGlobalHeightFalloff()
    {
      switch (this.m_HeightFalloff)
      {
        case DS_HazeCore.HeightFalloffType.Exponential:
          Shader.DisableKeyword("DS_HAZE_HEIGHT_FALLOFF_NONE");
          break;
        case DS_HazeCore.HeightFalloffType.None:
          Shader.EnableKeyword("DS_HAZE_HEIGHT_FALLOFF_NONE");
          break;
      }
    }

    private void OnTransformChildrenChanged()
    {
      this.m_Zones.Clear();
      this.m_Zones.AddRange((IEnumerable<DS_HazeZone>) ((Component) this).GetComponentsInChildren<DS_HazeZone>(true));
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) DS_HazeCore.instance, (Object) null))
      {
        DS_HazeCore.instance = this;
      }
      else
      {
        if (!Object.op_Inequality((Object) DS_HazeCore.instance, (Object) this))
          return;
        Debug.LogError((object) ("DeepSky::DS_HazeCore:Awake - There is more than one Haze Controller in this scene! Disabling " + ((Object) this).get_name()));
        ((Behaviour) this).set_enabled(false);
      }
    }

    private void OnEnable()
    {
      this.SetGlobalHeightFalloff();
      Shader.SetGlobalTexture("_SamplingOffsets", (Texture) this.m_NoiseLUT);
    }

    private void Reset()
    {
      this.OnTransformChildrenChanged();
    }

    public void SetGlobalNoiseLUT()
    {
      Shader.SetGlobalTexture("_SamplingOffsets", (Texture) this.m_NoiseLUT);
    }

    public void AddLightVolume(DS_HazeLightVolume lightVolume)
    {
      this.RemoveLightVolume(lightVolume);
      this.m_LightVolumes.Add(lightVolume);
    }

    public void RemoveLightVolume(DS_HazeLightVolume lightVolume)
    {
      this.m_LightVolumes.Remove(lightVolume);
    }

    public void GetRenderLightVolumes(
      Vector3 cameraPosition,
      List<DS_HazeLightVolume> lightVolumes,
      List<DS_HazeLightVolume> shadowVolumes)
    {
      foreach (DS_HazeLightVolume lightVolume in this.m_LightVolumes)
      {
        if (lightVolume.WillRender(cameraPosition))
        {
          if (lightVolume.CastShadows)
            shadowVolumes.Add(lightVolume);
          else
            lightVolumes.Add(lightVolume);
        }
      }
    }

    public DS_HazeContextItem GetRenderContextAtPosition(Vector3 position)
    {
      List<DS_HazeZone> dsHazeZoneList = new List<DS_HazeZone>();
      for (int index = 0; index < this.m_Zones.Count; ++index)
      {
        if (this.m_Zones[index].Contains(position) && ((Behaviour) this.m_Zones[index]).get_enabled())
          dsHazeZoneList.Add(this.m_Zones[index]);
      }
      if (dsHazeZoneList.Count == 0)
        return (DS_HazeContextItem) null;
      if (dsHazeZoneList.Count == 1)
        return dsHazeZoneList[0].Context.GetContextItemBlended(this.m_Time);
      dsHazeZoneList.Sort((Comparison<DS_HazeZone>) ((z1, z2) => z1 < z2 ? -1 : 1));
      DS_HazeContextItem contextItemBlended = dsHazeZoneList[0].Context.GetContextItemBlended(this.m_Time);
      for (int index = 1; index < dsHazeZoneList.Count; ++index)
      {
        float blendWeight = dsHazeZoneList[index].GetBlendWeight(position);
        contextItemBlended.Lerp(dsHazeZoneList[index].Context.GetContextItemBlended(this.m_Time), blendWeight);
      }
      return contextItemBlended;
    }

    public enum HeightFalloffType
    {
      Exponential,
      None,
    }

    public enum NoiseTextureSize
    {
      x8 = 8,
      x16 = 16, // 0x00000010
      x32 = 32, // 0x00000020
    }

    public enum DebugGUIPosition
    {
      TopLeft,
      TopCenter,
      TopRight,
      CenterLeft,
      Center,
      CenterRight,
      BottomLeft,
      BottomCenter,
      BottomRight,
    }
  }
}
