// Decompiled with JetBrains decompiler
// Type: Studio.TreeNodeCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public static class TreeNodeCommand
  {
    public class MoveCopyInfo
    {
      public Vector3[] oldValue = new Vector3[2]
      {
        Vector3.get_zero(),
        Vector3.get_zero()
      };
      public Vector3[] newValue = new Vector3[2]
      {
        Vector3.get_zero(),
        Vector3.get_zero()
      };
      public int dicKey;

      public MoveCopyInfo(int _dicKey, ChangeAmount _old, ChangeAmount _new)
      {
        this.dicKey = _dicKey;
        this.oldValue = new Vector3[2]{ _old.pos, _old.rot };
        this.newValue = new Vector3[2]{ _new.pos, _new.rot };
      }
    }

    public class MoveCopyCommand : ICommand
    {
      private TreeNodeCommand.MoveCopyInfo[] changeAmountInfo;

      public MoveCopyCommand(TreeNodeCommand.MoveCopyInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount != null)
          {
            changeAmount.pos = this.changeAmountInfo[index].newValue[0];
            changeAmount.rot = this.changeAmountInfo[index].newValue[1];
          }
        }
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount != null)
          {
            changeAmount.pos = this.changeAmountInfo[index].oldValue[0];
            changeAmount.rot = this.changeAmountInfo[index].oldValue[1];
          }
        }
      }
    }
  }
}
