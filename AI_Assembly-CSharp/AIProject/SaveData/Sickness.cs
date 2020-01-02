// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.Sickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class Sickness : ICommandData
  {
    private int _id = -1;

    public Sickness()
    {
    }

    public Sickness(Sickness source)
    {
      this._id = source._id;
      this.ElapsedTime = source.ElapsedTime;
      this.UsedMedicine = source.UsedMedicine;
      this.Enabled = source.Enabled;
    }

    [Key(0)]
    public int ID
    {
      get
      {
        return this._id;
      }
      set
      {
        if (this._id == value)
          return;
        this.SetID(value);
      }
    }

    [IgnoreMember]
    public int OverwritableID
    {
      get
      {
        return this._id;
      }
      set
      {
        this.SetID(value);
      }
    }

    private void SetID(int idValue)
    {
      this._id = idValue;
      this.UsedMedicine = false;
      this.Enabled = false;
      this.ElapsedTime = TimeSpan.Zero;
      this.Duration = TimeSpan.Zero;
    }

    [IgnoreMember]
    public string Name
    {
      get
      {
        if (this._id == -1)
          return "なし";
        string str;
        return !AIProject.Definitions.Sickness.NameTable.TryGetValue(this.ID, ref str) ? string.Format("無効:{0}", (object) this.ID.ToString()) : str;
      }
    }

    [Key(1)]
    public TimeSpan ElapsedTime { get; set; }

    [Key(2)]
    public bool UsedMedicine { get; set; }

    [Key(3)]
    public bool Enabled { get; set; }

    [Key(4)]
    public TimeSpan Duration { get; set; }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      return (IEnumerable<CommandData>) new CommandData[3]
      {
        new CommandData(CommandData.Command.String, head + string.Format(".{0}", (object) "Name"), new Func<object>(this.get_Name), (System.Action<object>) null),
        new CommandData(CommandData.Command.String, head + string.Format(".{0}", (object) "ElapsedTime"), (Func<object>) (() => (object) this.ElapsedTime.ToString()), (System.Action<object>) null),
        new CommandData(CommandData.Command.BOOL, head + string.Format(".{0}", (object) "UsedMedicine"), (Func<object>) (() => (object) this.UsedMedicine), (System.Action<object>) null)
      };
    }
  }
}
