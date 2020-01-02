// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class MerchantProfile : SerializedScriptableObject
  {
    [SerializeField]
    private Dictionary<Merchant.ActionType, int> _resultAddFriendlyRelationShipTable;
    [SerializeField]
    [LabelText("行動一周の日数")]
    [Min(4f)]
    private int _oneCycle;
    [SerializeField]
    [LabelText("待機後：探索移行率")]
    [Min(0.0f)]
    [MaxValue(100.0)]
    private float _toSearchSelectedRange;
    [SerializeField]
    [LabelText("違うエリア移行率")]
    [Min(0.0f)]
    [MaxValue(100.0)]
    private float _differentAreaSelectedRange;
    [SerializeField]
    [LabelText("NavMeshコンポーネントの始動遅延時間")]
    private float _activateNavMeshElementDelayTime;
    [SerializeField]
    [LabelText("商人の船のマップアイテムID")]
    private int _mapShipItemID;
    [SerializeField]
    private Dictionary<int, List<ValueTuple<int, int>>> _areaOpenState;
    [SerializeField]
    private int[] _spendMoneyBorder;
    [SerializeField]
    private int _spendMoneyMax;
    [SerializeField]
    private int _lastADVSafeguardID;

    public MerchantProfile()
    {
      base.\u002Ector();
    }

    public Dictionary<Merchant.ActionType, int> ResultAddFriendlyRelationShipTable
    {
      get
      {
        return this._resultAddFriendlyRelationShipTable;
      }
    }

    public int OneCycle
    {
      get
      {
        return this._oneCycle;
      }
    }

    public float ToSearchSelectedRange
    {
      get
      {
        return this._toSearchSelectedRange;
      }
    }

    public float DifferentAreaSelectedRange
    {
      get
      {
        return this._differentAreaSelectedRange;
      }
    }

    public float ActivateNavMeshElementDelayTime
    {
      get
      {
        return this._activateNavMeshElementDelayTime;
      }
    }

    public int MapShipItemID
    {
      get
      {
        return this._mapShipItemID;
      }
    }

    public IReadOnlyDictionary<int, List<ValueTuple<int, int>>> AreaOpenState
    {
      get
      {
        return (IReadOnlyDictionary<int, List<ValueTuple<int, int>>>) this._areaOpenState;
      }
    }

    public int[] SpendMoneyBorder
    {
      get
      {
        return this._spendMoneyBorder;
      }
    }

    public int SpendMoneyMax
    {
      get
      {
        return this._spendMoneyMax;
      }
    }

    public int LastADVSafeguardID
    {
      get
      {
        return this._lastADVSafeguardID;
      }
    }
  }
}
