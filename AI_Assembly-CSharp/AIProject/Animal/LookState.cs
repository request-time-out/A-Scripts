// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.LookState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Animal
{
  public struct LookState
  {
    public int ptnNo;
    public bool waitFlag;

    public LookState(int _ptnNo, bool _waitFlag)
    {
      this.ptnNo = _ptnNo;
      this.waitFlag = _waitFlag;
    }
  }
}
