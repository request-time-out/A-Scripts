// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.TagCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.CaptionScript
{
  public class TagCommand : IScriptCommand
  {
    public static string DefaultTag = "tag";

    public string Tag
    {
      get
      {
        return TagCommand.DefaultTag;
      }
    }

    public bool IsBefore
    {
      get
      {
        return false;
      }
    }

    public bool Execute(Dictionary<string, string> dic, int line)
    {
      return true;
    }

    public Dictionary<string, string> Analysis(List<string> list)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      int num1 = 0;
      Dictionary<string, string> dictionary2 = dictionary1;
      List<string> stringList1 = list;
      int index1 = num1;
      int num2 = index1 + 1;
      string str1 = stringList1[index1].Replace("'@", string.Empty).Replace("@", string.Empty);
      dictionary2["Tag"] = str1;
      Dictionary<string, string> dictionary3 = dictionary1;
      List<string> stringList2 = list;
      int index2 = num2;
      int num3 = index2 + 1;
      string str2 = stringList2[index2];
      dictionary3["name"] = str2;
      return dictionary1;
    }
  }
}
