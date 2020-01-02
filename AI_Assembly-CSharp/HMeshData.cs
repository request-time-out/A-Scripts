// Decompiled with JetBrains decompiler
// Type: HMeshData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HMeshData : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> Areas;
  private Collider[] colliders;
  public Dictionary<int, Collider[]> dicColliders;

  public HMeshData()
  {
    base.\u002Ector();
  }

  private void Reset()
  {
    this.Areas = new List<GameObject>();
    for (int index = 0; index < ((Component) this).get_transform().get_childCount(); ++index)
      this.Areas.Add(((Component) ((Component) this).get_transform().GetChild(index)).get_gameObject());
    this.Areas = this.Sort(this.Areas);
  }

  private void Start()
  {
    this.dicColliders = new Dictionary<int, Collider[]>();
    for (int index = 0; index < this.Areas.Count; ++index)
    {
      int key = index;
      this.colliders = (Collider[]) this.Areas[key].GetComponentsInChildren<Collider>();
      this.dicColliders.Add(key, this.colliders);
    }
  }

  private List<GameObject> Sort(List<GameObject> Areas)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    string[] array = new string[Areas.Count];
    for (int index = 0; index < Areas.Count; ++index)
      array[index] = ((Object) Areas[index]).get_name();
    Array.Sort<string>(array);
    for (int index = 0; index < array.Length; ++index)
    {
      using (List<GameObject>.Enumerator enumerator = Areas.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (!(array[index] != ((Object) current).get_name()))
          {
            gameObjectList.Add(current);
            break;
          }
        }
      }
    }
    return gameObjectList;
  }

  public void SetColliderAreaMap()
  {
    Dictionary<int, Chunk> chunkTable = Singleton<Manager.Map>.Instance.ChunkTable;
    using (Dictionary<int, Collider[]>.Enumerator enumerator = this.dicColliders.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, Collider[]> current = enumerator.Current;
        foreach (Chunk chunk in chunkTable.Values)
        {
          foreach (MapArea mapArea in chunk.MapAreas)
          {
            if (mapArea.AreaID == current.Key)
              mapArea.hColliders = current.Value;
          }
        }
      }
    }
  }
}
