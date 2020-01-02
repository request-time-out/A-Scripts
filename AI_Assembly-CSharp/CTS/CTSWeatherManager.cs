// Decompiled with JetBrains decompiler
// Type: CTS.CTSWeatherManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CTS
{
  [Serializable]
  public class CTSWeatherManager : MonoBehaviour
  {
    [SerializeField]
    private float m_snowPower;
    [SerializeField]
    private float m_snowMinHeight;
    [SerializeField]
    private float m_rainPower;
    [SerializeField]
    private float m_maxRainSmoothness;
    [SerializeField]
    private float m_season;
    [SerializeField]
    private Color m_winterTint;
    [SerializeField]
    private Color m_springTint;
    [SerializeField]
    private Color m_summerTint;
    [SerializeField]
    private Color m_autumnTint;
    private bool m_somethingChanged;

    public CTSWeatherManager()
    {
      base.\u002Ector();
    }

    public float SnowPower
    {
      get
      {
        return this.m_snowPower;
      }
      set
      {
        if ((double) this.m_snowPower == (double) value)
          return;
        this.m_snowPower = Mathf.Clamp01(value);
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public float SnowMinHeight
    {
      get
      {
        return this.m_snowMinHeight;
      }
      set
      {
        if ((double) this.m_snowMinHeight == (double) value)
          return;
        this.m_snowMinHeight = value;
        if ((double) this.m_snowMinHeight < 0.0)
          this.m_snowMinHeight = 0.0f;
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public float RainPower
    {
      get
      {
        return this.m_rainPower;
      }
      set
      {
        if ((double) this.m_rainPower == (double) value)
          return;
        this.m_rainPower = Mathf.Clamp01(value);
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public float MaxRainSmoothness
    {
      get
      {
        return this.m_maxRainSmoothness;
      }
      set
      {
        if ((double) this.m_maxRainSmoothness == (double) value)
          return;
        this.m_maxRainSmoothness = Mathf.Clamp(value, 0.0f, 30f);
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public float Season
    {
      get
      {
        return this.m_season;
      }
      set
      {
        if ((double) this.m_season == (double) value)
          return;
        this.m_season = Mathf.Clamp(value, 0.0f, 3.9999f);
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public Color WinterTint
    {
      get
      {
        return this.m_winterTint;
      }
      set
      {
        if (!Color.op_Inequality(this.m_winterTint, value))
          return;
        this.m_winterTint = value;
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public Color SpringTint
    {
      get
      {
        return this.m_springTint;
      }
      set
      {
        if (!Color.op_Inequality(this.m_springTint, value))
          return;
        this.m_springTint = value;
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public Color SummerTint
    {
      get
      {
        return this.m_summerTint;
      }
      set
      {
        if (!Color.op_Inequality(this.m_summerTint, value))
          return;
        this.m_summerTint = value;
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    public Color AutumnTint
    {
      get
      {
        return this.m_autumnTint;
      }
      set
      {
        if (!Color.op_Inequality(this.m_autumnTint, value))
          return;
        this.m_autumnTint = value;
        this.m_somethingChanged = true;
        if (Application.get_isPlaying())
          return;
        this.BroadcastUpdates();
      }
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {
      this.BroadcastUpdates();
    }

    private void BroadcastUpdates()
    {
      if (!this.m_somethingChanged)
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastWeatherUpdate(this);
      this.m_somethingChanged = false;
    }
  }
}
