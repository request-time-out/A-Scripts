// Decompiled with JetBrains decompiler
// Type: AIProject.LightSwitchData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class LightSwitchData : MonoBehaviour
  {
    private static Dictionary<int, LightSwitchData> _table = new Dictionary<int, LightSwitchData>();
    [SerializeField]
    private int _linkID;
    [SerializeField]
    private GameObject[] _onModeObjects;
    [SerializeField]
    private GameObject[] _offModeObjects;

    public LightSwitchData()
    {
      base.\u002Ector();
    }

    public static IReadOnlyDictionary<int, LightSwitchData> Table
    {
      get
      {
        return (IReadOnlyDictionary<int, LightSwitchData>) LightSwitchData._table;
      }
    }

    public static LightSwitchData Get(int linkID)
    {
      LightSwitchData lightSwitchData;
      LightSwitchData._table.TryGetValue(linkID, out lightSwitchData);
      return lightSwitchData;
    }

    public GameObject[] OnModeObjects
    {
      get
      {
        return this._onModeObjects;
      }
    }

    public GameObject[] OffModeObjects
    {
      get
      {
        return this._offModeObjects;
      }
    }

    private void Awake()
    {
      LightSwitchData._table[this._linkID] = this;
    }

    private void OnDestroy()
    {
      LightSwitchData._table.Remove(this._linkID);
    }
  }
}
