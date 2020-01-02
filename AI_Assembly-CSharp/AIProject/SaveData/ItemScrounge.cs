// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.ItemScrounge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class ItemScrounge : ICommandData
  {
    public ItemScrounge()
    {
    }

    public ItemScrounge(ItemScrounge other)
    {
      this.isEvent = other.isEvent;
      this.timer = other.timer;
      this.ItemList = other.ItemList.Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (x => new StuffItem(x))).ToList<StuffItem>();
    }

    [Key(0)]
    public bool isEvent { get; private set; }

    [Key(1)]
    public float timer { get; private set; }

    [Key(2)]
    public List<StuffItem> ItemList { get; private set; } = new List<StuffItem>();

    [IgnoreMember]
    public bool isEnd
    {
      get
      {
        return (double) this.timer >= (double) this.timeLimit;
      }
    }

    [IgnoreMember]
    private int timeLimit { get; } = 2400;

    [IgnoreMember]
    public int redZoneTime { get; } = 600;

    [IgnoreMember]
    public float remainingTime
    {
      get
      {
        return (float) this.timeLimit - this.timer;
      }
    }

    public void AddTimer(float add)
    {
      if (!this.isEvent)
        return;
      this.timer = Mathf.Min(this.timer + add, (float) this.timeLimit);
    }

    public void Finish()
    {
      this.timer = (float) this.timeLimit;
    }

    public void Reset()
    {
      this.isEvent = false;
      this.timer = 0.0f;
      this.ItemList.Clear();
    }

    public void Set(IReadOnlyCollection<StuffItem> items)
    {
      this.isEvent = true;
      this.timer = 0.0f;
      this.ItemList.AddRange((IEnumerable<StuffItem>) items);
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      List<CommandData> list = new List<CommandData>();
      list.Add(new CommandData(CommandData.Command.BOOL, head + string.Format(".{0}", (object) "isEvent"), (Func<object>) (() => (object) this.isEvent), (Action<object>) null));
      list.Add(new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "timer"), (Func<object>) (() => (object) this.timer), (Action<object>) null));
      string str = head + "ItemList";
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType10<StuffItem, int> anonType10 in this.ItemList.Select<StuffItem, \u003C\u003E__AnonType10<StuffItem, int>>((Func<StuffItem, int, \u003C\u003E__AnonType10<StuffItem, int>>) ((value, index) => new \u003C\u003E__AnonType10<StuffItem, int>(value, index))))
        anonType10.value.AddList(list, str + string.Format("[{0}]", (object) anonType10.index));
      return (IEnumerable<CommandData>) list;
    }
  }
}
