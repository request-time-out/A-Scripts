// Decompiled with JetBrains decompiler
// Type: Studio.AnimeCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Studio
{
  public static class AnimeCommand
  {
    public class SpeedCommand : ICommand
    {
      private int[] arrayKey;
      private float speed;
      private float[] oldSpeed;

      public SpeedCommand(int[] _arrayKey, float _speed, float[] _oldSpeed)
      {
        this.arrayKey = _arrayKey;
        this.speed = _speed;
        this.oldSpeed = _oldSpeed;
      }

      public void Do()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(this.arrayKey[index]);
          if (ctrlInfo != null)
            ctrlInfo.animeSpeed = this.speed;
        }
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(this.arrayKey[index]);
          if (ctrlInfo != null)
            ctrlInfo.animeSpeed = this.oldSpeed[index];
        }
      }
    }

    public class PatternCommand : ICommand
    {
      private int[] arrayKey;
      private float value;
      private float[] oldvalue;

      public PatternCommand(int[] _arrayKey, float _value, float[] _oldvalue)
      {
        this.arrayKey = _arrayKey;
        this.value = _value;
        this.oldvalue = _oldvalue;
      }

      public void Do()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          if (Studio.Studio.GetCtrlInfo(this.arrayKey[index]) is OCIChar ctrlInfo)
            ctrlInfo.animePattern = this.value;
        }
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          if (Studio.Studio.GetCtrlInfo(this.arrayKey[index]) is OCIChar ctrlInfo)
            ctrlInfo.animePattern = this.oldvalue[index];
        }
      }
    }

    public class OptionParamCommand : ICommand
    {
      private int[] arrayKey;
      private float value;
      private float[] oldvalue;
      private int kind;

      public OptionParamCommand(int[] _arrayKey, float _value, float[] _oldvalue, int _kind)
      {
        this.arrayKey = _arrayKey;
        this.value = _value;
        this.oldvalue = _oldvalue;
        this.kind = _kind;
      }

      public void Do()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          if (Studio.Studio.GetCtrlInfo(this.arrayKey[index]) is OCIChar ctrlInfo)
          {
            if (this.kind == 0)
              ctrlInfo.animeOptionParam1 = this.value;
            else
              ctrlInfo.animeOptionParam2 = this.value;
          }
        }
      }

      public void Redo()
      {
        this.Do();
      }

      public void Undo()
      {
        for (int index = 0; index < this.arrayKey.Length; ++index)
        {
          if (Studio.Studio.GetCtrlInfo(this.arrayKey[index]) is OCIChar ctrlInfo)
          {
            if (this.kind == 0)
              ctrlInfo.animeOptionParam1 = this.oldvalue[index];
            else
              ctrlInfo.animeOptionParam2 = this.oldvalue[index];
          }
        }
      }
    }
  }
}
