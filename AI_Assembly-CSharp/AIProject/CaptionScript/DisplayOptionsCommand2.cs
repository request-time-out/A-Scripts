﻿// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.DisplayOptionsCommand2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;

namespace AIProject.CaptionScript
{
  public class DisplayOptionsCommand2 : IScriptCommand
  {
    public static string DefaultTag = "display option";

    public string Tag
    {
      get
      {
        return DisplayOptionsCommand2.DefaultTag;
      }
    }

    public bool IsBefore
    {
      get
      {
        return false;
      }
    }

    public bool Execute(Dictionary<string, string> table, int line)
    {
      Singleton<ADV>.Instance.TargetMerchant.PopupCommands();
      return false;
    }

    public Dictionary<string, string> Analysis(List<string> list)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      int num1 = 0;
      Dictionary<string, string> dictionary2 = dictionary1;
      List<string> stringList = list;
      int index = num1;
      int num2 = index + 1;
      string str = stringList[index].Replace(string.Empty, CommandSystem.ReplaceStrings);
      dictionary2["Tag"] = str;
      return dictionary1;
    }
  }
}
