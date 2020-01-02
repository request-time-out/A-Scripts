// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.IScriptCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.CaptionScript
{
  public interface IScriptCommand
  {
    string Tag { get; }

    bool IsBefore { get; }

    bool Execute(Dictionary<string, string> dic, int line);

    Dictionary<string, string> Analysis(List<string> list);
  }
}
