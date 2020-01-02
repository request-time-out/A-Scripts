// Decompiled with JetBrains decompiler
// Type: AIProject.WeatherProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class WeatherProbability : ScriptableObject
  {
    [SerializeField]
    private float _clear;
    [SerializeField]
    private float _rain;
    [SerializeField]
    private float _storm;
    [SerializeField]
    private float _fog;
    [SerializeField]
    private float _cloud1;
    [SerializeField]
    private float _cloud2;
    [SerializeField]
    private float _cloud3;
    [SerializeField]
    private float _cloud4;
    [SerializeField]
    [HideInInspector]
    private int _clearNum;
    [SerializeField]
    [HideInInspector]
    private int _rainNum;
    [SerializeField]
    [HideInInspector]
    private int _stormNum;
    [SerializeField]
    [HideInInspector]
    private int _fogNum;
    [SerializeField]
    [HideInInspector]
    private int _cloud1Num;
    [SerializeField]
    [HideInInspector]
    private int _cloud2Num;
    [SerializeField]
    [HideInInspector]
    private int _cloud3Num;
    [SerializeField]
    [HideInInspector]
    private int _cloud4Num;

    public WeatherProbability()
    {
      base.\u002Ector();
    }

    private Dictionary<Weather, float> Table
    {
      get
      {
        return new Dictionary<Weather, float>()
        {
          [Weather.Clear] = this._clear,
          [Weather.Cloud1] = this._cloud1,
          [Weather.Cloud2] = this._cloud2,
          [Weather.Cloud3] = this._cloud3,
          [Weather.Cloud4] = this._cloud4,
          [Weather.Rain] = this._rain,
          [Weather.Storm] = this._storm,
          [Weather.Fog] = this._fog
        };
      }
    }

    public Weather Lottery()
    {
      float num = Random.Range(0.0f, this._clear + this._rain + this._storm + this._fog + this._cloud1 + this._cloud2 + this._cloud3 + this._cloud4);
      Weather weather = ~Weather.Clear;
      foreach (KeyValuePair<Weather, float> keyValuePair in this.Table)
      {
        if ((double) num <= (double) keyValuePair.Value)
        {
          weather = keyValuePair.Key;
          break;
        }
        num -= keyValuePair.Value;
      }
      return weather;
    }

    public Weather Lottery(List<Weather> list)
    {
      Dictionary<Weather, float> table = this.Table;
      List<KeyValuePair<Weather, float>> toRelease = ListPool<KeyValuePair<Weather, float>>.Get();
      foreach (KeyValuePair<Weather, float> keyValuePair in table)
      {
        foreach (Weather weather in list)
        {
          if (keyValuePair.Key == weather)
            toRelease.Add(keyValuePair);
        }
      }
      float num1 = 0.0f;
      foreach (KeyValuePair<Weather, float> keyValuePair in toRelease)
        num1 += keyValuePair.Value;
      float num2 = Random.Range(0.0f, num1);
      Weather weather1 = ~Weather.Clear;
      foreach (KeyValuePair<Weather, float> keyValuePair in toRelease)
      {
        if ((double) num2 <= (double) keyValuePair.Value)
        {
          weather1 = keyValuePair.Key;
          break;
        }
        num2 -= keyValuePair.Value;
      }
      ListPool<KeyValuePair<Weather, float>>.Release(toRelease);
      return weather1;
    }
  }
}
