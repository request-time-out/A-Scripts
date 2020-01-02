// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishingLevelInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace AIProject.MiniGames.Fishing
{
  public struct FishingLevelInfo
  {
    public FishingLevelInfo(int _level, float _timeScale)
    {
      this.Level = _level;
      this.TimeScale = _timeScale;
    }

    public int Level { get; private set; }

    public float TimeScale { get; private set; }
  }
}
