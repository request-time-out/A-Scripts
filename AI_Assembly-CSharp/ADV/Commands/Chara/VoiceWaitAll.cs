// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.VoiceWaitAll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class VoiceWaitAll : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return (string[]) null;
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override bool Process()
    {
      base.Process();
      return !Singleton<Manager.Voice>.Instance.IsVoiceCheck();
    }
  }
}
