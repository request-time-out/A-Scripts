// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class ActionPointItemData : MonoBehaviour
  {
    private static Dictionary<int, GameObject[]> _table = new Dictionary<int, GameObject[]>();
    private static Dictionary<int, MapItemKeyValuePair[]> _dic = new Dictionary<int, MapItemKeyValuePair[]>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private GameObject[] _objects;
    [SerializeField]
    private MapItemKeyValuePair[] _objectData;

    public ActionPointItemData()
    {
      base.\u002Ector();
    }

    public static ReadOnlyDictionary<int, GameObject[]> Table { get; } = new ReadOnlyDictionary<int, GameObject[]>((IDictionary<int, GameObject[]>) ActionPointItemData._table);

    public static ReadOnlyDictionary<int, MapItemKeyValuePair[]> Dic { get; } = new ReadOnlyDictionary<int, MapItemKeyValuePair[]>((IDictionary<int, MapItemKeyValuePair[]>) ActionPointItemData._dic);

    public GameObject[] Objects
    {
      get
      {
        return this._objects;
      }
    }

    public MapItemKeyValuePair[] ObjectData
    {
      get
      {
        return this._objectData;
      }
    }

    protected virtual void Awake()
    {
      ActionPointItemData._table[this._id] = this._objects;
      ActionPointItemData._dic[this._id] = this._objectData;
    }
  }
}
