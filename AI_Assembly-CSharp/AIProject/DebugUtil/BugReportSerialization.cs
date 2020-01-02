// Decompiled with JetBrains decompiler
// Type: AIProject.DebugUtil.BugReportSerialization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

namespace AIProject.DebugUtil
{
  [MessagePackObject(false)]
  public class BugReportSerialization
  {
    [Key(0)]
    public Vector3 Position { get; set; }

    [Key(1)]
    public bool IsEvent { get; set; }

    [Key(2)]
    public bool IsLoadingScene { get; set; }

    [Key(3)]
    public int ChunkID { get; set; }

    [Key(4)]
    public int PrevEventID { get; set; }

    [Key(5)]
    public string Revision { get; set; }

    [Key(6)]
    public RuntimePlatform Platform { get; set; }

    [Key(7)]
    public TimeSpan RealTimeSinceStartup { get; set; }

    [Key(8)]
    public DateTime DateTimeInGame { get; set; }

    [Key(9)]
    public AIProject.TimeZone TimeZone { get; set; }

    [Key(10)]
    public Weather Weather { get; set; }

    [Key(11)]
    public Temperature Tempareture { get; set; }

    [Key(12)]
    public float FrameRate { get; set; }

    [Key(13)]
    public long MemoryUsage { get; set; }

    [Key(14)]
    public float MemoryAvailable { get; set; }

    [Key(15)]
    public string StackTrace { get; set; }
  }
}
