// Decompiled with JetBrains decompiler
// Type: AIProject.EventType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  [Flags]
  public enum EventType
  {
    Sleep = 1,
    Break = 2,
    Eat = 4,
    Toilet = 8,
    Bath = 16, // 0x00000010
    Play = 32, // 0x00000020
    Search = 64, // 0x00000040
    StorageIn = 128, // 0x00000080
    StorageOut = 256, // 0x00000100
    Cook = 512, // 0x00000200
    DressIn = 1024, // 0x00000400
    DressOut = 2048, // 0x00000800
    Masturbation = 4096, // 0x00001000
    Lesbian = 8192, // 0x00002000
    Move = 16384, // 0x00004000
    PutItem = 32768, // 0x00008000
    ShallowSleep = 65536, // 0x00010000
    Wash = 131072, // 0x00020000
    Location = 262144, // 0x00040000
    DoorOpen = 524288, // 0x00080000
    DoorClose = 1048576, // 0x00100000
    Drink = 2097152, // 0x00200000
    ClothChange = 4194304, // 0x00400000
    Warp = 8388608, // 0x00800000
  }
}
