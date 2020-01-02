// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.RandomGotoTagCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.CaptionScript
{
  public class RandomGotoTagCommand : IScriptCommand
  {
    public static string DefaultTag = "random gototag";

    public string Tag
    {
      get
      {
        return RandomGotoTagCommand.DefaultTag;
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
      List<string> stringList = ListPool<string>.Get();
      foreach (KeyValuePair<string, string> keyValuePair in table)
      {
        if (!(keyValuePair.Key == "Tag") && !keyValuePair.Key.IsNullOrEmpty())
          stringList.Add(keyValuePair.Key);
      }
      string empty = string.Empty;
      if (!stringList.IsNullOrEmpty<string>())
        empty = stringList[Random.Range(0, stringList.Count)];
      ListPool<string>.Release(stringList);
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
        if (!list[index2].IsNullOrEmpty())
          dictionary1[list[index2]] = string.Empty;
      }
      return dictionary1;
    }
  }
}
