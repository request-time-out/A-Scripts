// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.TextCommand2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AIProject.CaptionScript
{
  public class TextCommand2 : IScriptCommand
  {
    public static string DefaultTag = "text";

    public string Tag
    {
      get
      {
        return TextCommand2.DefaultTag;
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
      string name;
      if (!table.TryGetValue("designation", out name))
      {
        string s;
        table.TryGetValue("name", out s);
        int result;
        if (int.TryParse(s, out result))
        {
          switch (result)
          {
            case 1:
              Singleton<ADV>.Instance.Captions.CaptionSystem.SetName(((Object) ((Component) Singleton<ADV>.Instance.TargetMerchant.ChaControl).get_gameObject()).get_name());
              break;
            case 2:
              Singleton<ADV>.Instance.Captions.CaptionSystem.SetName(string.Empty);
              break;
          }
        }
        else
          Singleton<ADV>.Instance.Captions.CaptionSystem.SetName(string.Empty);
      }
      else
        Singleton<ADV>.Instance.Captions.CaptionSystem.SetName(name);
      bool result1 = false;
      string text;
      if (table.TryGetValue("wait", out text))
        bool.TryParse(text, out result1);
      table.TryGetValue("content", out text);
      Singleton<ADV>.Instance.Captions.CaptionSystem.Action = new Action(this.OnComplete);
      Singleton<ADV>.Instance.Captions.CaptionSystem.SetText(text, result1);
      return false;
    }

    private void OnComplete()
    {
      Singleton<ADV>.Instance.Captions.CommandSystem.CompletedCommand = true;
    }

    public Dictionary<string, string> Analysis(List<string> list)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      int num1 = 0;
      Dictionary<string, string> dictionary2 = dictionary1;
      List<string> stringList1 = list;
      int index1 = num1;
      int num2 = index1 + 1;
      string str1 = stringList1[index1].Replace(string.Empty, CommandSystem.ReplaceStrings);
      dictionary2["Tag"] = str1;
      Dictionary<string, string> dictionary3 = dictionary1;
      List<string> stringList2 = list;
      int index2 = num2;
      int num3 = index2 + 1;
      string str2 = stringList2[index2];
      dictionary3["name"] = str2;
      Dictionary<string, string> dictionary4 = dictionary1;
      List<string> stringList3 = list;
      int index3 = num3;
      int num4 = index3 + 1;
      string str3 = stringList3[index3];
      dictionary4["content"] = str3;
      for (int index4 = 3; index4 < list.Count; ++index4)
      {
        Match match = Regex.Match(list[index4], "(\\S+\\d*)=(\\S*\\d*)");
        if (match.Success)
          dictionary1[match.Groups[1].Value] = match.Groups[2].Value;
      }
      return dictionary1;
    }
  }
}
