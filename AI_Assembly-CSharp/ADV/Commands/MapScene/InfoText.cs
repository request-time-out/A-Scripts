// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.InfoText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using System;

namespace ADV.Commands.MapScene
{
  public class InfoText : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Text" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      MapUIContainer.WarningMessageUI.isFadeInForOutWait = true;
      MapUIContainer.PushMessageUI(this.args[0], 0, 1, (Action) null);
    }

    public override bool Process()
    {
      base.Process();
      return false;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      MapUIContainer.WarningMessageUI.isFadeInForOutWait = false;
    }
  }
}
