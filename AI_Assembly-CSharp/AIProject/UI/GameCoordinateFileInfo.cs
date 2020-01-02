// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GameCoordinateFileInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject.UI
{
  public class GameCoordinateFileInfo
  {
    public int Index { get; set; }

    public string FullPath { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public DateTime Time { get; set; }

    public byte[] PngData { get; set; }

    public bool IsInSaveData { get; set; }
  }
}
