// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Choice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.Base
{
  public class Choice : CommandBase
  {
    private string labelTitleBK = string.Empty;
    private Choice.ChoiceData choice;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Visible", "Case" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ bool.TrueString, "text,tag" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num = 0;
      string[] args = this.args;
      int index = num;
      int count = index + 1;
      if (!bool.Parse(args[index]))
        this.scenario.captionSystem.Clear();
      CommCommandList.CommandInfo[] array = ((IEnumerable<string>) this.args).Skip<string>(count).Where<string>((Func<string, bool>) (arg => !((IList<string>) this.args).IsNullOrEmpty<string>())).Select<string, string[]>((Func<string, string[]>) (arg => arg.Split(','))).Where<string[]>((Func<string[], bool>) (ss => ss.Length >= 2)).Select<string[], Choice.ChoiceData>((Func<string[], Choice.ChoiceData>) (ss => new Choice.ChoiceData(this.scenario.ReplaceText(ss[0]), this.scenario.ReplaceVars(ss[1])))).Select<Choice.ChoiceData, CommCommandList.CommandInfo>((Func<Choice.ChoiceData, CommCommandList.CommandInfo>) (choice => new CommCommandList.CommandInfo(choice.text, (Func<bool>) null, (Action<int>) (x => this.choice = choice)))).ToArray<CommCommandList.CommandInfo>();
      this.labelTitleBK = this.scenario.ChoiceON(string.Empty, array);
      this.scenario.isSkip = this.scenario.isSkip && Manager.Config.GameData.ChoicesSkip;
      this.scenario.isAuto = this.scenario.isAuto && Manager.Config.GameData.ChoicesAuto;
    }

    public override bool Process()
    {
      base.Process();
      return this.choice != null;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      if (!processEnd)
        return;
      this.scenario.ChoiceOFF(this.labelTitleBK);
      this.scenario.TextLogCall(new Text.Data(new string[2]
      {
        string.Empty,
        this.choice.text
      }), (IReadOnlyCollection<TextScenario.IVoice[]>) null);
      this.scenario.SearchTagJumpOrOpenFile(this.choice.jump, this.localLine);
      this.scenario.captions.CanvasGroupON();
    }

    public class ChoiceData
    {
      public readonly string text;
      public readonly string jump;

      public ChoiceData(string text, string jump)
      {
        this.text = text;
        this.jump = jump;
      }
    }
  }
}
