// Decompiled with JetBrains decompiler
// Type: AIProject.FootStepInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject
{
  public class FootStepInfo
  {
    public FootStepInfo.Key[] Keys = new FootStepInfo.Key[0];

    public FootStepInfo()
    {
    }

    public FootStepInfo(FootStepInfo info)
    {
      this.Threshold = info.Threshold;
      if (this.Keys.Length != info.Keys.Length)
        this.Keys = new FootStepInfo.Key[info.Keys.Length];
      for (int index = 0; index < info.Keys.Length; ++index)
        this.Keys[index] = info.Keys[index];
    }

    public FootStepInfo(float min, float max, float[] keys)
    {
      this.Threshold = new Threshold(min, max);
      if (this.Keys.Length != keys.Length)
        this.Keys = new FootStepInfo.Key[keys.Length];
      for (int index = 0; index < keys.Length; ++index)
        this.Keys[index] = new FootStepInfo.Key(keys[index], false);
    }

    public FootStepInfo(float min, float max, List<float> keys)
    {
      this.Threshold = new Threshold(min, max);
      if (this.Keys.Length != keys.Count)
        this.Keys = new FootStepInfo.Key[keys.Count];
      for (int index = 0; index < keys.Count; ++index)
        this.Keys[index] = new FootStepInfo.Key(keys[index], false);
    }

    public Threshold Threshold { get; private set; } = new Threshold();

    public struct Key
    {
      public Key(FootStepInfo.Key key)
      {
        this.Time = key.Time;
        this.Execute = key.Execute;
      }

      public Key(float time, bool execute)
      {
        this.Time = time;
        this.Execute = execute;
      }

      public float Time { get; set; }

      public bool Execute { get; set; }
    }
  }
}
