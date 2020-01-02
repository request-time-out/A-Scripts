// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.RecyclingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class RecyclingData
  {
    public RecyclingData()
    {
      if (this.DecidedItemList == null)
        this.DecidedItemList = new List<StuffItem>();
      if (this.CreatedItemList == null)
        this.CreatedItemList = new List<StuffItem>();
      this.CreateCounter = 0.0f;
      this.CreateCountEnabled = false;
    }

    public RecyclingData(RecyclingData data)
    {
      this.Copy(data);
    }

    [Key(0)]
    public List<StuffItem> DecidedItemList { get; set; } = new List<StuffItem>();

    [Key(1)]
    public List<StuffItem> CreatedItemList { get; set; } = new List<StuffItem>();

    [Key(2)]
    public float CreateCounter { get; set; }

    [Key(3)]
    public bool CreateCountEnabled { get; set; }

    public void Copy(RecyclingData data)
    {
      if (data == null)
        return;
      this.DecidedItemList.Clear();
      if (!data.DecidedItemList.IsNullOrEmpty<StuffItem>())
      {
        foreach (StuffItem decidedItem in data.DecidedItemList)
          this.DecidedItemList.Add(new StuffItem(decidedItem));
      }
      this.CreatedItemList.Clear();
      if (!data.CreatedItemList.IsNullOrEmpty<StuffItem>())
      {
        foreach (StuffItem createdItem in data.CreatedItemList)
          this.CreatedItemList.Add(new StuffItem(createdItem));
      }
      this.CreateCounter = data.CreateCounter;
      this.CreateCountEnabled = data.CreateCountEnabled;
    }
  }
}
