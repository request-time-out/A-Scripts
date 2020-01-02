// Decompiled with JetBrains decompiler
// Type: DebugEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class DebugEx
{
  public static void ArrayPrint<T>(T[] print, string head, Object context = null)
  {
  }

  public static string ConvertString<T>(T[] print, string head)
  {
    StringBuilder stringBuilder = new StringBuilder(head);
    // ISSUE: object of a compiler-generated type is created
    foreach (\u003C\u003E__AnonType34<string, int> anonType34 in ((IEnumerable<T>) print).Select<T, string>((Func<T, string>) (p => p.ToString())).Select<string, \u003C\u003E__AnonType34<string, int>>((Func<string, int, \u003C\u003E__AnonType34<string, int>>) ((s, i) => new \u003C\u003E__AnonType34<string, int>(s, i))))
      stringBuilder.AppendFormat("{0}{1}", (object) anonType34.s, anonType34.i >= print.Length - 1 ? (object) string.Empty : (object) ",");
    return stringBuilder.ToString();
  }

  public static void PathCheck()
  {
    Debug.Log((object) ("temporaryCachePath:" + Application.get_temporaryCachePath()));
    Debug.Log((object) ("streamingAssetsPath:" + Application.get_streamingAssetsPath()));
    Debug.Log((object) ("dataPath:" + Application.get_dataPath()));
    Debug.Log((object) ("persistentDataPath:" + Application.get_persistentDataPath()));
  }
}
