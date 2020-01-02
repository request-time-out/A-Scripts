// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Animal
{
  public enum AnimalState
  {
    None = 0,
    Start = 1,
    Repop = 2,
    Depop = 3,
    Idle = 4,
    Wait = 5,
    SitWait = 6,
    Locomotion = 7,
    LovelyIdle = 8,
    LovelyFollow = 9,
    Escape = 10, // 0x0000000A
    Swim = 11, // 0x0000000B
    Sleep = 12, // 0x0000000C
    Toilet = 13, // 0x0000000D
    Rest = 14, // 0x0000000E
    Eat = 15, // 0x0000000F
    Drink = 16, // 0x00000010
    Actinidia = 17, // 0x00000011
    Grooming = 18, // 0x00000012
    MoveEars = 19, // 0x00000013
    Roar = 20, // 0x00000014
    Peck = 21, // 0x00000015
    ToDepop = 22, // 0x00000016
    ToIndoor = 23, // 0x00000017
    Action0 = 90, // 0x0000005A
    Action1 = 91, // 0x0000005B
    Action2 = 92, // 0x0000005C
    Action3 = 93, // 0x0000005D
    Action4 = 94, // 0x0000005E
    Action5 = 95, // 0x0000005F
    Action6 = 96, // 0x00000060
    Action7 = 97, // 0x00000061
    Action8 = 98, // 0x00000062
    Action9 = 99, // 0x00000063
    WithPlayer = 100, // 0x00000064
    WithAgent = 101, // 0x00000065
    WithMerchant = 102, // 0x00000066
    Destroyed = 103, // 0x00000067
  }
}
