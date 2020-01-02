// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.ConditionOpenAreaIDCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AIProject.CaptionScript
{
  public class ConditionOpenAreaIDCommand : IScriptCommand
  {
    public static string DefaultTag = "condition openareaid";

    public string Tag
    {
      get
      {
        return ConditionOpenAreaIDCommand.DefaultTag;
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
      string key = Singleton<ADV>.Instance.TargetMerchant.OpenAreaID.ToString();
      string empty;
      if (!table.TryGetValue(key, out empty))
        empty = string.Empty;
      Singleton<ADV>.Instance.Captions.CommandSystem.GotoTag(empty);
      return true;
    }

    public Dictionary<string, string> Analysis(List<string> list)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      int num = 0;
      Dictionary<string, string> dictionary2 = dictionary1;
      List<string> stringList = list;
      int index1 = num;
      int index2 = index1 + 1;
      string str = stringList[index1].Replace(string.Empty, CommandSystem.ReplaceStrings);
      dictionary2["Tag"] = str;
      for (; index2 < list.Count; ++index2)
      {
        Match match = Regex.Match(list[index2], "(\\S+\\d*)=(\\S*\\d*)");
        if (match.Success)
          dictionary1[match.Groups[1].ToString()] = match.Groups[2].ToString();
      }
      return dictionary1;
    }
  }
}
