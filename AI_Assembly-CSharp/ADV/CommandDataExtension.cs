// Decompiled with JetBrains decompiler
// Type: ADV.CommandDataExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV
{
  internal static class CommandDataExtension
  {
    public static bool AddList(this ICommandData self, List<CommandData> list, string head = null)
    {
      if (self == null || list == null)
        return false;
      list.AddRange(self.CreateCommandData(head));
      return true;
    }
  }
}
