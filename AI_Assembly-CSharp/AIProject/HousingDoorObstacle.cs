// Decompiled with JetBrains decompiler
// Type: AIProject.HousingDoorObstacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [RequireComponent(typeof (DoorPoint))]
  public class HousingDoorObstacle : DoorObstacle
  {
    [SerializeField]
    private NavMeshObstacle _obstacleInOpenRight;
    [SerializeField]
    private NavMeshObstacle _obstacleInOpenLeft;
    [SerializeField]
    private NavMeshObstacle _obstacleInClose;

    protected override void OnStart()
    {
      DoorPoint component = (DoorPoint) ((Component) this).GetComponent<DoorPoint>();
      component.ObstacleInOpenRight = this._obstacleInOpenRight;
      component.ObstacleInOpenLeft = this._obstacleInOpenLeft;
      component.ObstacleInClose = this._obstacleInClose;
    }
  }
}
