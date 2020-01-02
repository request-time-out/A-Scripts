// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Parent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Object
{
  public class Parent : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "Name",
          "FindType",
          "ChildName",
          "RootName"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string index2 = args1[index1];
      string[] args2 = this.args;
      int index3 = num2;
      int num3 = index3 + 1;
      string findType = args2[index3];
      string[] args3 = this.args;
      int index4 = num3;
      int num4 = index4 + 1;
      string childName = args3[index4];
      string[] args4 = this.args;
      int index5 = num4;
      int num5 = index5 + 1;
      string otherRootName = args4[index5];
      Transform get = ObjectEx.FindGet(findType, childName, otherRootName, this.scenario.commandController);
      this.scenario.commandController.Objects[index2].get_transform().SetParent(get, false);
    }
  }
}
