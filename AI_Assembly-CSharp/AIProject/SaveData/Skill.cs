// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class Skill : ICommandData
  {
    public Skill()
    {
    }

    public Skill(Skill other)
    {
      this.Level = other.Level;
      this.Experience = other.Experience;
      this.Parameter = other.Parameter;
    }

    [Key(0)]
    public int Level { get; set; } = 1;

    [Key(1)]
    public float Experience { get; set; }

    [Key(2)]
    public int Parameter { get; set; }

    [IgnoreMember]
    public Func<int, int> CalculationNextExp { get; set; } = (Func<int, int>) (x => 100 * x);

    [IgnoreMember]
    public int NextExperience
    {
      get
      {
        return this.CalculationNextExp(this.Level);
      }
    }

    [IgnoreMember]
    public Action<int, float> OnStatsChanged { get; set; }

    [IgnoreMember]
    public Action<int, int> OnLevelChanged { get; set; }

    public void AddExperience(float exp)
    {
      int num1 = this.CalculationNextExp(this.Level);
      this.Experience += exp;
      if ((double) Mathf.Sign(exp) > 0.0)
      {
        if ((double) this.Experience >= (double) num1)
        {
          if (this.Level >= 9999)
          {
            this.Experience = (float) num1;
            return;
          }
          float num2 = this.Experience / (float) num1;
          while ((double) num2 >= 1.0 && this.Level < 9999)
          {
            int level = this.Level;
            int num3 = ++this.Level;
            this.Experience -= (float) num1;
            num1 = this.CalculationNextExp(num3);
            num2 = this.Experience / (float) num1;
            Debug.Log((object) string.Format("レベルアップ： {0} -> {1}", (object) level.ToString(), (object) num3.ToString()));
            Action<int, int> onLevelChanged = this.OnLevelChanged;
            if (onLevelChanged != null)
              onLevelChanged(num3, level);
          }
        }
        Debug.Log((object) string.Format("経験値増加: {0}", (object) exp.ToString()));
      }
      else if ((double) this.Experience < 0.0)
      {
        if (this.Level <= 1)
        {
          this.Experience = 0.0f;
          return;
        }
        float num2 = this.Experience / (float) this.CalculationNextExp(this.Level - 1);
        while (Mathf.Approximately(num2, 0.0f) && this.Level > 1)
        {
          int level = this.Level;
          int num3 = --this.Level;
          int num4 = this.CalculationNextExp(num3);
          this.Experience += (float) num4;
          num2 = this.Experience / (float) num4;
          Debug.Log((object) string.Format("レベルダウン： {0} -> {1}", (object) level.ToString(), (object) num3.ToString()));
          Action<int, int> onLevelChanged = this.OnLevelChanged;
          if (onLevelChanged != null)
            onLevelChanged(num3, level);
        }
      }
      Action<int, float> onStatsChanged = this.OnStatsChanged;
      if (onStatsChanged == null)
        return;
      onStatsChanged(this.Level, this.Experience);
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      return (IEnumerable<CommandData>) new CommandData[3]
      {
        new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "Level"), (Func<object>) (() => (object) this.Level), (Action<object>) null),
        new CommandData(CommandData.Command.FLOAT, head + string.Format(".{0}", (object) "Experience"), (Func<object>) (() => (object) this.Experience), (Action<object>) null),
        new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "Parameter"), (Func<object>) (() => (object) this.Parameter), (Action<object>) null)
      };
    }
  }
}
