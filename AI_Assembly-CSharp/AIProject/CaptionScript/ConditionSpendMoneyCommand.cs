// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.ConditionSpendMoneyCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AIProject.CaptionScript
{
  public class ConditionSpendMoneyCommand : IScriptCommand
  {
    public static string DefaultTag = "condition spendmoney";

    public string Tag
    {
      get
      {
        return ConditionSpendMoneyCommand.DefaultTag;
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
      List<Tuple<int, string>> toRelease = ListPool<Tuple<int, string>>.Get();
      foreach (KeyValuePair<string, string> keyValuePair in table)
      {
        int result;
        if (!(keyValuePair.Key == "Tag") && int.TryParse(keyValuePair.Key, out result))
          toRelease.Add(new Tuple<int, string>(result, keyValuePair.Value));
      }
      toRelease.Sort((Comparison<Tuple<int, string>>) ((a, b) => b.Item1 - a.Item1));
      string empty = string.Empty;
      for (int index = 0; index < toRelease.Count; ++index)
      {
        if (toRelease[index].Item1 <= Singleton<Manager.Map>.Instance.Player.PlayerData.SpendMoney)
        {
          empty = toRelease[index].Item2;
          break;
        }
      }
      ListPool<Tuple<int, string>>.Release(toRelease);
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
