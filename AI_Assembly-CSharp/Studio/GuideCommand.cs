// Decompiled with JetBrains decompiler
// Type: Studio.GuideCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public static class GuideCommand
  {
    public class AddInfo
    {
      public int dicKey;
      public Vector3 value;
    }

    public class EqualsInfo
    {
      public int dicKey;
      public Vector3 oldValue;
      public Vector3 newValue;
    }

    public class MoveAddCommand : ICommand
    {
      private GuideCommand.AddInfo[] changeAmountInfo;

      public MoveAddCommand(GuideCommand.AddInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount1 = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount1 != null)
          {
            ChangeAmount changeAmount2 = changeAmount1;
            changeAmount2.pos = Vector3.op_Addition(changeAmount2.pos, this.changeAmountInfo[index].value);
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
          ChangeAmount changeAmount1 = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount1 != null)
          {
            ChangeAmount changeAmount2 = changeAmount1;
            changeAmount2.pos = Vector3.op_Subtraction(changeAmount2.pos, this.changeAmountInfo[index].value);
          }
        }
      }
    }

    public class MoveEqualsCommand : ICommand
    {
      private GuideCommand.EqualsInfo[] changeAmountInfo;

      public MoveEqualsCommand(GuideCommand.EqualsInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount != null)
            changeAmount.pos = this.changeAmountInfo[index].newValue;
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
            changeAmount.pos = this.changeAmountInfo[index].oldValue;
        }
      }
    }

    public class RotationAddCommand : ICommand
    {
      private GuideCommand.AddInfo[] changeAmountInfo;

      public RotationAddCommand(GuideCommand.AddInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount1 = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount1 != null)
          {
            ChangeAmount changeAmount2 = changeAmount1;
            changeAmount2.rot = Vector3.op_Addition(changeAmount2.rot, this.changeAmountInfo[index].value);
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
            changeAmount.rot = this.changeAmountInfo[index].value;
        }
      }
    }

    public class RotationEqualsCommand : ICommand
    {
      private GuideCommand.EqualsInfo[] changeAmountInfo;

      public RotationEqualsCommand(GuideCommand.EqualsInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount != null)
            changeAmount.rot = this.changeAmountInfo[index].newValue;
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
            changeAmount.rot = this.changeAmountInfo[index].oldValue;
        }
      }
    }

    public class ScaleEqualsCommand : ICommand
    {
      private GuideCommand.EqualsInfo[] changeAmountInfo;

      public ScaleEqualsCommand(GuideCommand.EqualsInfo[] _changeAmountInfo)
      {
        this.changeAmountInfo = _changeAmountInfo;
      }

      public void Do()
      {
        for (int index = 0; index < this.changeAmountInfo.Length; ++index)
        {
          ChangeAmount changeAmount = Studio.Studio.GetChangeAmount(this.changeAmountInfo[index].dicKey);
          if (changeAmount != null)
            changeAmount.scale = this.changeAmountInfo[index].newValue;
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
            changeAmount.scale = this.changeAmountInfo[index].oldValue;
        }
      }
    }
  }
}
