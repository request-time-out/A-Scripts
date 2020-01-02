// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.VoiceWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class VoiceWait : CommandBase
  {
    private CharaData chara;

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
      this.chara = this.scenario.commandController.GetChara(int.Parse(this.args[0]));
    }

    public override bool Process()
    {
      base.Process();
      return !Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.chara.voiceNo, this.chara.voiceTrans, true);
    }
  }
}
