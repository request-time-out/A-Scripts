// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopGridViewInitParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace SuperScrollView
{
  public class LoopGridViewInitParam
  {
    public float mSmoothDumpRate = 0.3f;
    public float mSnapFinishThreshold = 0.01f;
    public float mSnapVecThreshold = 145f;

    public static LoopGridViewInitParam CopyDefaultInitParam()
    {
      return new LoopGridViewInitParam();
    }
  }
}
