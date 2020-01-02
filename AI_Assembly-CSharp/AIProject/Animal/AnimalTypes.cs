// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject.Animal
{
  [Flags]
  public enum AnimalTypes
  {
    Cat = 1,
    Chicken = 2,
    Fish = 4,
    Butterfly = 8,
    Mecha = 16, // 0x00000010
    Frog = 32, // 0x00000020
    BirdFlock = 64, // 0x00000040
    CatWithFish = 128, // 0x00000080
    CatTank = 256, // 0x00000100
    Chick = 512, // 0x00000200
    Fairy = 1024, // 0x00000400
    DarkSpirit = 2048, // 0x00000800
    Ground = Chick | CatTank | Mecha | Chicken | Cat, // 0x00000313
    Flying = DarkSpirit | Fairy | BirdFlock | Butterfly, // 0x00000C48
    Insect = Frog, // 0x00000020
    Viewing = DarkSpirit | Fairy | Butterfly | Fish, // 0x00000C0C
    All = Viewing | Insect | Ground | BirdFlock, // 0x00000F7F
  }
}
