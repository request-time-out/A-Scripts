// Decompiled with JetBrains decompiler
// Type: AIProject.DoorObstacleData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class DoorObstacleData : MonoBehaviour
  {
    private static Dictionary<int, DoorObstacleData.DoorObstaclePack> _table = new Dictionary<int, DoorObstacleData.DoorObstaclePack>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private NavMeshObstacle _obstacleInOpenRight;
    [SerializeField]
    private NavMeshObstacle _obstacleInOpenLeft;
    [SerializeField]
    private NavMeshObstacle _obstacleInClose;

    public DoorObstacleData()
    {
      base.\u002Ector();
    }

    public static ReadOnlyDictionary<int, DoorObstacleData.DoorObstaclePack> Table { get; } = new ReadOnlyDictionary<int, DoorObstacleData.DoorObstaclePack>((IDictionary<int, DoorObstacleData.DoorObstaclePack>) DoorObstacleData._table);

    private void Awake()
    {
      DoorObstacleData._table[this._id] = new DoorObstacleData.DoorObstaclePack()
      {
        OpenRight = this._obstacleInOpenRight,
        OpenLeft = this._obstacleInOpenLeft,
        Close = this._obstacleInClose
      };
    }

    [Serializable]
    public class DoorObstaclePack
    {
      public NavMeshObstacle OpenRight { get; set; }

      public NavMeshObstacle OpenLeft { get; set; }

      public NavMeshObstacle Close { get; set; }
    }
  }
}
