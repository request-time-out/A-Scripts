// Decompiled with JetBrains decompiler
// Type: AIProject.MapItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class MapItemData : MonoBehaviour
  {
    private static Dictionary<int, List<GameObject>> _table = new Dictionary<int, List<GameObject>>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private GameObject[] _objects;

    public MapItemData()
    {
      base.\u002Ector();
    }

    public static ReadOnlyDictionary<int, List<GameObject>> Table { get; } = new ReadOnlyDictionary<int, List<GameObject>>((IDictionary<int, List<GameObject>>) MapItemData._table);

    public GameObject[] Objects
    {
      get
      {
        return this._objects;
      }
    }

    protected virtual void Awake()
    {
      if (this._objects.IsNullOrEmpty<GameObject>())
        return;
      List<GameObject> gameObjectList;
      if (!MapItemData._table.TryGetValue(this._id, out gameObjectList) || gameObjectList == null)
        MapItemData._table[this._id] = gameObjectList = new List<GameObject>();
      foreach (GameObject gameObject in this._objects)
      {
        if (!Object.op_Equality((Object) gameObject, (Object) null) && !gameObjectList.Contains(gameObject))
          gameObjectList.Add(gameObject);
      }
    }

    private void OnDestroy()
    {
      List<GameObject> source;
      if (MapItemData.Table.IsNullOrEmpty<int, List<GameObject>>() || this._objects.IsNullOrEmpty<GameObject>() || (!MapItemData.Table.TryGetValue(this._id, ref source) || source.IsNullOrEmpty<GameObject>()))
        return;
      foreach (GameObject gameObject in this._objects)
      {
        if (Object.op_Inequality((Object) gameObject, (Object) null) && source.Contains(gameObject))
          source.Remove(gameObject);
      }
      source.RemoveAll((Predicate<GameObject>) (x => Object.op_Equality((Object) x, (Object) null)));
    }

    public static List<GameObject> Get(int id)
    {
      if (MapItemData.Table.IsNullOrEmpty<int, List<GameObject>>())
        return (List<GameObject>) null;
      List<GameObject> gameObjectList;
      return MapItemData.Table.TryGetValue(id, ref gameObjectList) ? gameObjectList : (List<GameObject>) null;
    }
  }
}
