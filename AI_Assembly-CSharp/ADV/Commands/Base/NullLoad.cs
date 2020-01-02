// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.NullLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Base
{
  public class NullLoad : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Version", "Name" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "0", string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      Debug.LogError((object) "NullLoad:現在対応していません");
    }
  }
}
