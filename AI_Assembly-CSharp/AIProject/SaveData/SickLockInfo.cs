// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.SickLockInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class SickLockInfo
  {
    [Key(0)]
    public bool Lock { get; set; }

    [Key(1)]
    public float ElapsedTime { get; set; }
  }
}
