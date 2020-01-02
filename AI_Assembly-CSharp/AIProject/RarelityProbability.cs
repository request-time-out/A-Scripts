// Decompiled with JetBrains decompiler
// Type: AIProject.RarelityProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class RarelityProbability : ScriptableObject
  {
    [SerializeField]
    private float _none;
    [SerializeField]
    private float _normal;
    [SerializeField]
    private float _rare;
    [SerializeField]
    private float _superRare;
    [SerializeField]
    private float _highRare;
    [SerializeField]
    private float _ultraRare;
    [SerializeField]
    [HideInInspector]
    private int _noneNum;
    [SerializeField]
    [HideInInspector]
    private int _normalNum;
    [SerializeField]
    [HideInInspector]
    private int _rareNum;
    [SerializeField]
    [HideInInspector]
    private int _superRareNum;
    [SerializeField]
    [HideInInspector]
    private int _highRareNum;
    [SerializeField]
    [HideInInspector]
    private int _ultraRareNum;
    [SerializeField]
    private float _failure;
    [SerializeField]
    private float _success;
    [SerializeField]
    private float _triumph;
    [SerializeField]
    private float _failureRate;
    [SerializeField]
    private float _successRate;
    [SerializeField]
    private float _triumphRate;
    [SerializeField]
    [HideInInspector]
    private int _failureNum;
    [SerializeField]
    [HideInInspector]
    private int _successNum;
    [SerializeField]
    [HideInInspector]
    private int _triumphNum;
    private ValueTuple<ResultType, float>[] _resultProbabilities;
    private ValueTuple<Rarelity, float>[] _probabilities;

    public RarelityProbability()
    {
      base.\u002Ector();
    }

    public float None
    {
      get
      {
        return this._none;
      }
    }

    public float Normal
    {
      get
      {
        return this._normal;
      }
    }

    public float Rare
    {
      get
      {
        return this._rare;
      }
    }

    public float SuperRare
    {
      get
      {
        return this._superRare;
      }
    }

    public float HighRare
    {
      get
      {
        return this._highRare;
      }
    }

    public float UltraRare
    {
      get
      {
        return this._ultraRare;
      }
    }

    public float Failure
    {
      get
      {
        return this._failure;
      }
    }

    public float Success
    {
      get
      {
        return this._success;
      }
    }

    public float Triumph
    {
      get
      {
        return this._triumph;
      }
    }

    public float FailureRate
    {
      get
      {
        return this._failureRate;
      }
    }

    public float SuccessRate
    {
      get
      {
        return this._successRate;
      }
    }

    public float TriumphRate
    {
      get
      {
        return this._triumphRate;
      }
    }

    private ValueTuple<ResultType, float>[] GetResultProbabilities()
    {
      this._resultProbabilities[0] = new ValueTuple<ResultType, float>(ResultType.Failure, this._failure);
      this._resultProbabilities[1] = new ValueTuple<ResultType, float>(ResultType.Success, this._success);
      this._resultProbabilities[2] = new ValueTuple<ResultType, float>(ResultType.Triumph, this._triumph);
      return this._resultProbabilities;
    }

    public ResultType LotteryResult()
    {
      float num = Random.Range(0.0f, this.Failure + this.Success + this.Triumph);
      ResultType resultType = ~ResultType.Failure;
      foreach (ValueTuple<ResultType, float> resultProbability in this.GetResultProbabilities())
      {
        if ((double) num <= resultProbability.Item2)
        {
          resultType = (ResultType) resultProbability.Item1;
          break;
        }
        num -= (float) resultProbability.Item2;
      }
      return resultType;
    }

    private ValueTuple<Rarelity, float>[] GetProbabilities()
    {
      this._probabilities[0] = new ValueTuple<Rarelity, float>(Rarelity.None, this._none);
      this._probabilities[1] = new ValueTuple<Rarelity, float>(Rarelity.N, this._normal);
      this._probabilities[2] = new ValueTuple<Rarelity, float>(Rarelity.R, this._rare);
      this._probabilities[3] = new ValueTuple<Rarelity, float>(Rarelity.SR, this._superRare);
      this._probabilities[4] = new ValueTuple<Rarelity, float>(Rarelity.SSR, this._highRare);
      this._probabilities[5] = new ValueTuple<Rarelity, float>(Rarelity.UR, this._ultraRare);
      return this._probabilities;
    }

    public Rarelity Lottery(bool containsNone)
    {
      float num1 = this.Normal + this.Rare + this.SuperRare + this.HighRare + this.UltraRare;
      if (containsNone)
        num1 += this._none;
      float num2 = Random.Range(0.0f, num1);
      Rarelity rarelity = ~Rarelity.None;
      foreach (ValueTuple<Rarelity, float> probability in this.GetProbabilities())
      {
        if ((double) num2 <= probability.Item2)
        {
          rarelity = (Rarelity) probability.Item1;
          break;
        }
        num2 -= (float) probability.Item2;
      }
      return rarelity;
    }

    public Rarelity Lottery(bool containsNone, Rarelity[] table)
    {
      ValueTuple<Rarelity, float>[] probabilities = this.GetProbabilities();
      List<ValueTuple<Rarelity, float>> toRelease = ListPool<ValueTuple<Rarelity, float>>.Get();
      foreach (ValueTuple<Rarelity, float> valueTuple in probabilities)
      {
        foreach (Rarelity rarelity in table)
        {
          if ((containsNone || rarelity != Rarelity.None) && (Rarelity) valueTuple.Item1 == rarelity)
            toRelease.Add(valueTuple);
        }
      }
      float num1 = 0.0f;
      using (List<ValueTuple<Rarelity, float>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<Rarelity, float> current = enumerator.Current;
          num1 += (float) current.Item2;
        }
      }
      float num2 = Random.Range(0.0f, num1);
      Rarelity rarelity1 = ~Rarelity.None;
      using (List<ValueTuple<Rarelity, float>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<Rarelity, float> current = enumerator.Current;
          if ((double) num2 <= current.Item2)
          {
            rarelity1 = (Rarelity) current.Item1;
            break;
          }
          num2 -= (float) current.Item2;
        }
      }
      ListPool<ValueTuple<Rarelity, float>>.Release(toRelease);
      return rarelity1;
    }
  }
}
