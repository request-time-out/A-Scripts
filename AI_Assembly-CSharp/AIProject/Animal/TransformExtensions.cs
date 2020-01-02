// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.TransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AIProject.Animal
{
  public static class TransformExtensions
  {
    public static void FindMatchLoop(this Transform trans, string str, ref List<Transform> list)
    {
      if (Object.op_Equality((Object) trans, (Object) null) || list == null)
        return;
      if (Regex.IsMatch(((Object) trans).get_name(), str))
        list.Add(trans);
      for (int index = 0; index < trans.get_childCount(); ++index)
        trans.GetChild(index).FindMatchLoop(str, ref list);
    }
  }
}
