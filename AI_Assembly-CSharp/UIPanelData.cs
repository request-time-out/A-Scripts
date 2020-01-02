// Decompiled with JetBrains decompiler
// Type: UIPanelData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

public class UIPanelData
{
  public double mElapsedTicks;
  public int mCalls;
  public int mRebuildCount;
  public int mDrawCallNum;

  internal void Enlarge(UIPanelData value)
  {
    this.mElapsedTicks += value.mElapsedTicks;
    this.mCalls += value.mCalls;
    this.mRebuildCount += value.mRebuildCount;
    this.mDrawCallNum += value.mDrawCallNum;
  }
}
