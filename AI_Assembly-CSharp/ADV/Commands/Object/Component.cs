// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Component
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using Illusion.Extensions;
using System;
using UnityEngine;

namespace ADV.Commands.Object
{
  public class Component : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Type",
          "ComponentType",
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
        return new string[5]
        {
          Component.Type.Add.ToString(),
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
      int num3 = args1[index1].Check(true, Enum.GetNames(typeof (Component.Type)));
      string[] args2 = this.args;
      int index2 = num2;
      int num4 = index2 + 1;
      string typeName = args2[index2];
      string[] args3 = this.args;
      int index3 = num4;
      int num5 = index3 + 1;
      string findType = args3[index3];
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      string childName = args4[index4];
      string[] args5 = this.args;
      int index5 = num6;
      int num7 = index5 + 1;
      string otherRootName = args5[index5];
      GameObject gameObject = ((Component) ObjectEx.FindGet(findType, childName, otherRootName, this.scenario.commandController)).get_gameObject();
      System.Type type = Utils.Type.Get(typeName);
      switch ((Component.Type) num3)
      {
        case Component.Type.Add:
          gameObject.AddComponent(type);
          break;
        case Component.Type.Sub:
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject.GetComponent(type));
          break;
      }
    }

    private enum Type
    {
      Add,
      Sub,
    }
  }
}
