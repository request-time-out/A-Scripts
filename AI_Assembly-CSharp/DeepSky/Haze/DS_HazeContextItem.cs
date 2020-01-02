// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeContextItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;

namespace DeepSky.Haze
{
  [AddComponentMenu("")]
  [Serializable]
  public class DS_HazeContextItem
  {
    [SerializeField]
    [Range(0.0f, 8f)]
    public float m_AirScatteringScale = 1f;
    [SerializeField]
    public DS_HazeContextItem.Multiplier m_AirScatteringMultiplier = DS_HazeContextItem.Multiplier.One;
    [SerializeField]
    [Range(0.0001f, 0.1f)]
    public float m_AirDensityHeightFalloff = 1f / 1000f;
    [SerializeField]
    [Range(0.0f, 8f)]
    public float m_HazeScatteringScale = 1f;
    [SerializeField]
    public DS_HazeContextItem.Multiplier m_HazeScatteringMultiplier = DS_HazeContextItem.Multiplier.One;
    [SerializeField]
    [Range(0.0001f, 0.1f)]
    public float m_HazeDensityHeightFalloff = 3f / 1000f;
    [SerializeField]
    [Range(-0.99f, 0.99f)]
    public float m_HazeScatteringDirection = 0.8f;
    [SerializeField]
    [Range(0.0f, 1f)]
    public float m_HazeSecondaryScatteringRatio = 0.8f;
    [SerializeField]
    [Range(0.0f, 1f)]
    public float m_FogOpacity = 1f;
    [SerializeField]
    [Range(0.0f, 8f)]
    public float m_FogScatteringScale = 1f;
    [SerializeField]
    [Range(0.0f, 8f)]
    public float m_FogExtinctionScale = 1f;
    [SerializeField]
    public DS_HazeContextItem.Multiplier m_FogExtinctionMultiplier = DS_HazeContextItem.Multiplier.One;
    [SerializeField]
    [Range(0.0001f, 1f)]
    public float m_FogDensityHeightFalloff = 0.01f;
    [SerializeField]
    [Range(-0.99f, 0.99f)]
    public float m_FogScatteringDirection = 0.7f;
    [SerializeField]
    public Color m_FogAmbientColour = Color.get_white();
    [SerializeField]
    public Color m_FogLightColour = Color.get_white();
    [SerializeField]
    public string m_Name;
    [SerializeField]
    public AnimationCurve m_Weight;
    [SerializeField]
    [Range(0.0f, 1f)]
    public float m_FogStartDistance;
    [SerializeField]
    [Range(-10000f, 10000f)]
    public float m_FogStartHeight;

    public DS_HazeContextItem()
    {
      this.m_Name = "New";
      this.m_Weight = new AnimationCurve(new Keyframe[3]
      {
        new Keyframe(0.25f, 0.0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(0.75f, 0.0f)
      });
    }

    public static float MultiplierAsFloat(DS_HazeContextItem.Multiplier mult)
    {
      switch (mult)
      {
        case DS_HazeContextItem.Multiplier.OneTenth:
          return 0.1f;
        case DS_HazeContextItem.Multiplier.OneFifth:
          return 0.2f;
        case DS_HazeContextItem.Multiplier.OneHalf:
          return 0.5f;
        case DS_HazeContextItem.Multiplier.One:
          return 1f;
        case DS_HazeContextItem.Multiplier.Two:
          return 2f;
        case DS_HazeContextItem.Multiplier.Five:
          return 5f;
        case DS_HazeContextItem.Multiplier.Ten:
          return 10f;
        case DS_HazeContextItem.Multiplier.OneHundredth:
          return 0.01f;
        default:
          return 1f;
      }
    }

    public static float ParamWithMultiplier(float param, DS_HazeContextItem.Multiplier mult)
    {
      switch (mult)
      {
        case DS_HazeContextItem.Multiplier.OneTenth:
          return param * 0.1f;
        case DS_HazeContextItem.Multiplier.OneFifth:
          return param * 0.2f;
        case DS_HazeContextItem.Multiplier.OneHalf:
          return param * 0.5f;
        case DS_HazeContextItem.Multiplier.One:
          return param * 1f;
        case DS_HazeContextItem.Multiplier.Two:
          return param * 2f;
        case DS_HazeContextItem.Multiplier.Five:
          return param * 5f;
        case DS_HazeContextItem.Multiplier.Ten:
          return param * 10f;
        case DS_HazeContextItem.Multiplier.OneHundredth:
          return param * 0.01f;
        default:
          return param * 1f;
      }
    }

    public void Lerp(DS_HazeContextItem other, float dt)
    {
      if (other == null)
        return;
      dt = Mathf.Clamp01(dt);
      float num = 1f - dt;
      this.m_AirScatteringScale = (float) ((double) this.m_AirScatteringScale * (double) num + (double) other.m_AirScatteringScale * (double) dt);
      this.m_AirDensityHeightFalloff = (float) ((double) this.m_AirDensityHeightFalloff * (double) num + (double) other.m_AirDensityHeightFalloff * (double) dt);
      this.m_HazeScatteringScale = (float) ((double) this.m_HazeScatteringScale * (double) num + (double) other.m_HazeScatteringScale * (double) dt);
      this.m_HazeDensityHeightFalloff = (float) ((double) this.m_HazeDensityHeightFalloff * (double) num + (double) other.m_HazeDensityHeightFalloff * (double) dt);
      this.m_HazeScatteringDirection = (float) ((double) this.m_HazeScatteringDirection * (double) num + (double) other.m_HazeScatteringDirection * (double) dt);
      this.m_HazeSecondaryScatteringRatio = (float) ((double) this.m_HazeSecondaryScatteringRatio * (double) num + (double) other.m_HazeSecondaryScatteringRatio * (double) dt);
      this.m_FogOpacity = (float) ((double) this.m_FogOpacity * (double) num + (double) other.m_FogOpacity * (double) dt);
      this.m_FogScatteringScale = (float) ((double) this.m_FogScatteringScale * (double) num + (double) other.m_FogScatteringScale * (double) dt);
      this.m_FogExtinctionScale = (float) ((double) this.m_FogExtinctionScale * (double) num + (double) other.m_FogExtinctionScale * (double) dt);
      this.m_FogDensityHeightFalloff = (float) ((double) this.m_FogDensityHeightFalloff * (double) num + (double) other.m_FogDensityHeightFalloff * (double) dt);
      this.m_FogStartDistance = (float) ((double) this.m_FogStartDistance * (double) num + (double) other.m_FogStartDistance * (double) dt);
      this.m_FogScatteringDirection = (float) ((double) this.m_FogScatteringDirection * (double) num + (double) other.m_FogScatteringDirection * (double) dt);
      this.m_FogStartHeight = (float) ((double) this.m_FogStartHeight * (double) num + (double) other.m_FogStartHeight * (double) dt);
      this.m_FogAmbientColour = Color.op_Addition(Color.op_Multiply(this.m_FogAmbientColour, num), Color.op_Multiply(other.m_FogAmbientColour, dt));
      this.m_FogLightColour = Color.op_Addition(Color.op_Multiply(this.m_FogLightColour, num), Color.op_Multiply(other.m_FogLightColour, dt));
    }

    public void CopyFrom(DS_HazeContextItem other)
    {
      if (other == null)
        return;
      Type type1 = this.GetType();
      Type type2 = other.GetType();
      foreach (FieldInfo field1 in type1.GetFields())
      {
        FieldInfo field2 = type2.GetField(field1.Name);
        field1.SetValue((object) this, field2.GetValue((object) other));
      }
      this.m_Weight = new AnimationCurve(this.m_Weight.get_keys());
    }

    public enum Multiplier
    {
      OneTenth,
      OneFifth,
      OneHalf,
      One,
      Two,
      Five,
      Ten,
      OneHundredth,
    }
  }
}
