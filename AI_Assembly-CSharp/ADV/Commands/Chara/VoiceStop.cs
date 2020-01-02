// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.VoiceStop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Chara
{
  public class VoiceStop : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "No" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ int.MaxValue.ToString() };
      }
    }

    public override void Do()
    {
      base.Do();
      CharaData chara = this.scenario.commandController.GetChara(int.Parse(this.args[0]));
      Singleton<Manager.Voice>.Instance.Stop(chara.voiceNo, chara.voiceTrans);
      this.scenario.loopVoiceList.RemoveAll((Predicate<TextScenario.LoopVoicePack>) (item => item.voiceNo == chara.voiceNo));
    }
  }
}
