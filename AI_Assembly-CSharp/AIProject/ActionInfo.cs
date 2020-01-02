// Decompiled with JetBrains decompiler
// Type: AIProject.ActionInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject
{
  public struct ActionInfo
  {
    public readonly bool hasAction;
    public readonly int randomCount;

    public ActionInfo(bool hasAction_, int randCount)
    {
      this.hasAction = hasAction_;
      this.randomCount = randCount;
    }
  }
}
