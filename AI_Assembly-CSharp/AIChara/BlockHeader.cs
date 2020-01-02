// Decompiled with JetBrains decompiler
// Type: AIChara.BlockHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

namespace AIChara
{
  [MessagePackObject(true)]
  public class BlockHeader
  {
    public BlockHeader()
    {
      this.lstInfo = new List<BlockHeader.Info>();
    }

    public List<BlockHeader.Info> lstInfo { get; set; }

    public BlockHeader.Info SearchInfo(string name)
    {
      return this.lstInfo.Find((Predicate<BlockHeader.Info>) (n => n.name == name));
    }

    [MessagePackObject(true)]
    public class Info
    {
      public Info()
      {
        this.name = string.Empty;
        this.version = string.Empty;
        this.pos = 0L;
        this.size = 0L;
      }

      public string name { get; set; }

      public string version { get; set; }

      public long pos { get; set; }

      public long size { get; set; }
    }
  }
}
