// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Tag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ADV.Commands.Base
{
  public class Tag : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Label" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }
  }
}
