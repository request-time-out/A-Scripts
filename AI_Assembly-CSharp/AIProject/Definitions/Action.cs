// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Action
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEx;

namespace AIProject.Definitions
{
  public static class Action
  {
    public static readonly Dictionary<int, AIProject.EventType> EventTypeTable = new Dictionary<int, AIProject.EventType>()
    {
      [0] = AIProject.EventType.Sleep,
      [1] = AIProject.EventType.Break,
      [2] = AIProject.EventType.Eat,
      [3] = AIProject.EventType.Toilet,
      [4] = AIProject.EventType.Bath,
      [5] = AIProject.EventType.Play,
      [6] = AIProject.EventType.Search,
      [7] = AIProject.EventType.StorageIn,
      [8] = AIProject.EventType.StorageOut,
      [9] = AIProject.EventType.Cook,
      [10] = AIProject.EventType.DressIn,
      [11] = AIProject.EventType.DressOut,
      [12] = AIProject.EventType.Masturbation,
      [13] = AIProject.EventType.Lesbian,
      [14] = AIProject.EventType.Move,
      [15] = AIProject.EventType.PutItem,
      [16] = AIProject.EventType.ShallowSleep,
      [17] = AIProject.EventType.Wash,
      [18] = AIProject.EventType.Location,
      [19] = AIProject.EventType.DoorOpen,
      [20] = AIProject.EventType.DoorClose,
      [21] = AIProject.EventType.Drink,
      [22] = AIProject.EventType.ClothChange,
      [23] = AIProject.EventType.Warp
    };
    public static readonly Dictionary<AIProject.EventType, ValueTuple<int, string>> NameTable = new Dictionary<AIProject.EventType, ValueTuple<int, string>>()
    {
      [AIProject.EventType.Sleep] = new ValueTuple<int, string>(0, "睡眠"),
      [AIProject.EventType.Break] = new ValueTuple<int, string>(1, "休憩"),
      [AIProject.EventType.Eat] = new ValueTuple<int, string>(2, "食事"),
      [AIProject.EventType.Toilet] = new ValueTuple<int, string>(3, "トイレ"),
      [AIProject.EventType.Bath] = new ValueTuple<int, string>(4, "沐浴"),
      [AIProject.EventType.Play] = new ValueTuple<int, string>(5, "遊戯"),
      [AIProject.EventType.Search] = new ValueTuple<int, string>(6, "探索"),
      [AIProject.EventType.StorageIn] = new ValueTuple<int, string>(7, "アイテムボックス(搬入)"),
      [AIProject.EventType.StorageOut] = new ValueTuple<int, string>(8, "アイテムボックス(搬出)"),
      [AIProject.EventType.Cook] = new ValueTuple<int, string>(9, "料理"),
      [AIProject.EventType.DressIn] = new ValueTuple<int, string>(10, "着替え(脱衣)"),
      [AIProject.EventType.DressOut] = new ValueTuple<int, string>(11, "着替え(着衣)"),
      [AIProject.EventType.Masturbation] = new ValueTuple<int, string>(12, "オナニー"),
      [AIProject.EventType.Lesbian] = new ValueTuple<int, string>(13, "レズ"),
      [AIProject.EventType.Move] = new ValueTuple<int, string>(14, "特殊移動"),
      [AIProject.EventType.PutItem] = new ValueTuple<int, string>(15, "アイテムを置く"),
      [AIProject.EventType.ShallowSleep] = new ValueTuple<int, string>(16, "浅い眠り"),
      [AIProject.EventType.Wash] = new ValueTuple<int, string>(17, "洗う"),
      [AIProject.EventType.Location] = new ValueTuple<int, string>(18, "ロケーション"),
      [AIProject.EventType.DoorOpen] = new ValueTuple<int, string>(19, "ドアを開ける"),
      [AIProject.EventType.DoorClose] = new ValueTuple<int, string>(20, "ドアを閉める"),
      [AIProject.EventType.Drink] = new ValueTuple<int, string>(21, "飲む"),
      [AIProject.EventType.ClothChange] = new ValueTuple<int, string>(22, "着替える"),
      [AIProject.EventType.Warp] = new ValueTuple<int, string>(23, "ワープ")
    };
  }
}
