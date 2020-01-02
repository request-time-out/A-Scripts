// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.SendCommandData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV.Commands.MapScene
{
  public class SendCommandData : CommandBase
  {
    private string name;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Name" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ string.Empty };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args = this.args;
      int index = num1;
      int num2 = index + 1;
      this.name = args[index];
    }

    public override void Do()
    {
      base.Do();
      foreach (CommandData command in (IEnumerable<CommandData>) this.scenario.package.commandList)
      {
        if (command.key == this.name)
        {
          command.ReceiveADV(this.scenario);
          break;
        }
      }
    }
  }
}
