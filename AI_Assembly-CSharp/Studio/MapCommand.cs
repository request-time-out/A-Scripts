// Decompiled with JetBrains decompiler
// Type: Studio.MapCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public static class MapCommand
  {
    public class EqualsInfo
    {
      public Vector3 oldValue;
      public Vector3 newValue;
    }

    public class MoveEqualsCommand : ICommand
    {
      private MapCommand.EqualsInfo changeAmountInfo;

      public MoveEqualsCommand(MapCommand.EqualsInfo _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos = this.changeAmountInfo.newValue;
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos = this.changeAmountInfo.oldValue;
      }
    }

    public class RotationEqualsCommand : ICommand
    {
      private MapCommand.EqualsInfo changeAmountInfo;

      public RotationEqualsCommand(MapCommand.EqualsInfo _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = this.changeAmountInfo.newValue;
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = this.changeAmountInfo.oldValue;
      }
    }
  }
}
