// Decompiled with JetBrains decompiler
// Type: AIProject.SearchAreaProbabilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class SearchAreaProbabilities : ScriptableObject
  {
    [SerializeField]
    private float _hand;
    [SerializeField]
    private float _shovel;
    [SerializeField]
    private float _pickel;
    [SerializeField]
    private float _net;
    [SerializeField]
    private float _fishing;
    [SerializeField]
    [HideInInspector]
    private int _handNum;
    [SerializeField]
    [HideInInspector]
    private int _shovelNum;
    [SerializeField]
    [HideInInspector]
    private int _pickelNum;
    [SerializeField]
    [HideInInspector]
    private int _netNum;
    [SerializeField]
    [HideInInspector]
    private int _fishingNum;

    public SearchAreaProbabilities()
    {
      base.\u002Ector();
    }

    private Dictionary<int, float> Table
    {
      get
      {
        return new Dictionary<int, float>()
        {
          [0] = this._hand,
          [3] = this._shovel,
          [4] = this._pickel,
          [5] = this._net,
          [6] = this._fishing
        };
      }
    }

    public int Lottery(Dictionary<int, bool> filter = null, List<SearchAreaProbabilities.AddProb> list = null)
    {
      Dictionary<int, float> table = this.Table;
      int[] array = table.Keys.ToArray<int>();
      float num1 = 0.0f;
      if (!list.IsNullOrEmpty<SearchAreaProbabilities.AddProb>())
      {
        foreach (SearchAreaProbabilities.AddProb addProb in list)
        {
          float num2 = (float) ((double) addProb.add / (double) table.Count - 1.0);
          foreach (int num3 in array)
          {
            if (num3 == addProb.id)
            {
              Dictionary<int, float> dictionary;
              int index;
              (dictionary = table)[index = num3] = dictionary[index] + addProb.add;
            }
            else
            {
              Dictionary<int, float> dictionary;
              int index;
              (dictionary = table)[index = num3] = dictionary[index] - num2;
            }
          }
        }
      }
      Dictionary<int, float> dictionary1;
      if (!filter.IsNullOrEmpty<int, bool>())
      {
        dictionary1 = new Dictionary<int, float>();
        foreach (KeyValuePair<int, bool> keyValuePair in filter)
        {
          if (keyValuePair.Value)
            dictionary1[keyValuePair.Key] = table[keyValuePair.Key];
        }
      }
      else
        dictionary1 = table;
      foreach (int index in dictionary1.Keys.ToArray<int>())
        num1 += dictionary1[index];
      float num4 = Random.Range(0.0f, num1);
      int num5 = -1;
      foreach (KeyValuePair<int, float> keyValuePair in dictionary1)
      {
        if ((double) num4 <= (double) keyValuePair.Value)
        {
          num5 = keyValuePair.Key;
          break;
        }
        num4 -= keyValuePair.Value;
      }
      return num5;
    }

    public struct AddProb
    {
      public int id;
      public float add;

      public AddProb(int id, float add)
      {
        this.id = id;
        this.add = add;
      }
    }
  }
}
