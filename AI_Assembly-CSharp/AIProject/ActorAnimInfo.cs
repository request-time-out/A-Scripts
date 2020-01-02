// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject
{
  public struct ActorAnimInfo
  {
    public bool inEnableBlend;
    public float inBlendSec;
    public float inFadeOutTime;
    public bool outEnableBlend;
    public float outBlendSec;
    public int directionType;
    public bool endEnableBlend;
    public float endBlendSec;
    public bool isLoop;
    public int loopMinTime;
    public int loopMaxTime;
    public bool hasAction;
    public string loopStateName;
    public int randomCount;
    public float oldNormalizedTime;
    public int layer;
  }
}
