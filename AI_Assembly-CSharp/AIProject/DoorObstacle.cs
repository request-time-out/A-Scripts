// Decompiled with JetBrains decompiler
// Type: AIProject.DoorObstacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (DoorPoint))]
  public class DoorObstacle : ActionPointComponentBase
  {
    [SerializeField]
    private int _id;
    [SerializeField]
    private DoorObstacleData.DoorObstaclePack _data;

    public DoorObstacleData.DoorObstaclePack Data
    {
      get
      {
        return this._data;
      }
    }

    protected override void OnStart()
    {
      this._data = DoorObstacleData.Table.get_Item(this._id);
      DoorPoint component = (DoorPoint) ((Component) this).GetComponent<DoorPoint>();
      component.ObstacleInOpenRight = this._data.OpenRight;
      component.ObstacleInOpenLeft = this._data.OpenLeft;
      component.ObstacleInClose = this._data.Close;
    }
  }
}
