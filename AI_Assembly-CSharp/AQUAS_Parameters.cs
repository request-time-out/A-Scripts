// Decompiled with JetBrains decompiler
// Type: AQUAS_Parameters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
public class AQUAS_Parameters
{
  [Serializable]
  public class UnderWaterParameters
  {
    [Header("The following parameters apply for underwater only!")]
    [Space(5f)]
    public float fogDensity = 0.1f;
    public Color fogColor;
    [Space(5f)]
    [Header("Post Processing Profiles (Must NOT be empty!)")]
    [Space(5f)]
    public PostProcessProfile underwaterProfile;
    public PostProcessProfile defaultProfile;
  }

  [Serializable]
  public class GameObjects
  {
    [Space(5f)]
    [Header("Set waterplanes array size = number of waterplanes")]
    public List<GameObject> waterPlanes = new List<GameObject>();
    [Header("Set the game objects required for underwater mode.")]
    [Space(5f)]
    public GameObject mainCamera;
    public GameObject waterLens;
    public GameObject airLens;
    public GameObject bubble;
    public bool useSquaredPlanes;
  }

  [Serializable]
  public class WetLens
  {
    [Header("Set how long the lens stays wet after diving up.")]
    public float wetTime = 1f;
    [Space(5f)]
    [Header("Set how long the lens needs to dry.")]
    public float dryingTime = 1.5f;
    public float rundownSpeed = 72f;
    [Space(5f)]
    public Texture2D[] sprayFrames;
    public Texture2D[] sprayFramesCutout;
  }

  [Serializable]
  public class CausticSettings
  {
    [Header("The following values are 'Afloat'/'Underwater'")]
    public Vector2 causticIntensity = new Vector2(0.6f, 0.2f);
    public Vector2 causticTiling = new Vector2(300f, 100f);
    public float maxCausticDepth;
  }

  [Serializable]
  public class Audio
  {
    public AudioClip[] sounds;
    [Range(0.0f, 1f)]
    public float underwaterVolume;
    [Range(0.0f, 1f)]
    public float surfacingVolume;
    [Range(0.0f, 1f)]
    public float diveVolume;
  }

  [Serializable]
  public class BubbleSpawnCriteria
  {
    [Header("Spawn Criteria for big bubbles")]
    public int minBubbleCount = 20;
    public int maxBubbleCount = 40;
    [Space(5f)]
    public float maxSpawnDistance = 1f;
    public float averageUpdrift = 3f;
    [Space(5f)]
    public float baseScale = 0.06f;
    public float avgScaleSummand = 0.15f;
    [Space(5f)]
    [Header("Spawn Timer for initial dive")]
    public float minSpawnTimer = 0.005f;
    public float maxSpawnTimer = 0.03f;
    [Space(5f)]
    [Header("Spawn Timer for long dive")]
    public float minSpawnTimerL = 0.1f;
    public float maxSpawnTimerL = 0.5f;
  }
}
