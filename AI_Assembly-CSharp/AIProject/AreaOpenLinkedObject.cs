// Decompiled with JetBrains decompiler
// Type: AIProject.AreaOpenLinkedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class AreaOpenLinkedObject : MonoBehaviour
  {
    private static Dictionary<int, List<GameObject>> _table = new Dictionary<int, List<GameObject>>();
    public static ReadOnlyDictionary<int, List<GameObject>> Table = (ReadOnlyDictionary<int, List<GameObject>>) null;
    [SerializeField]
    private int _areaOpenID;

    public AreaOpenLinkedObject()
    {
      base.\u002Ector();
    }

    public int AreaOpenID
    {
      get
      {
        return this._areaOpenID;
      }
    }

    private void Awake()
    {
      if (AreaOpenLinkedObject.Table == null)
        AreaOpenLinkedObject.Table = new ReadOnlyDictionary<int, List<GameObject>>((IDictionary<int, List<GameObject>>) AreaOpenLinkedObject._table);
      List<GameObject> gameObjectList;
      if (!AreaOpenLinkedObject._table.TryGetValue(this._areaOpenID, out gameObjectList))
        AreaOpenLinkedObject._table[this._areaOpenID] = gameObjectList = new List<GameObject>();
      if (gameObjectList.Contains(((Component) this).get_gameObject()))
        return;
      gameObjectList.Add(((Component) this).get_gameObject());
    }

    private void OnDestroy()
    {
      List<GameObject> source;
      if (!AreaOpenLinkedObject._table.TryGetValue(this._areaOpenID, out source) || source.IsNullOrEmpty<GameObject>() || !source.Contains(((Component) this).get_gameObject()))
        return;
      source.Remove(((Component) this).get_gameObject());
      if (source.Count != 0)
        return;
      AreaOpenLinkedObject._table.Remove(this._areaOpenID);
    }
  }
}
