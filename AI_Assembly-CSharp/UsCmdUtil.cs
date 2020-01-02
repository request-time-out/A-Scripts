// Decompiled with JetBrains decompiler
// Type: UsCmdUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class UsCmdUtil
{
  public static List<int> ReadIntList(UsCmd c)
  {
    List<int> intList = new List<int>();
    int num = c.ReadInt32();
    for (int index = 0; index < num; ++index)
      intList.Add(c.ReadInt32());
    return intList;
  }

  public static void WriteIntList(UsCmd c, List<int> l)
  {
    c.WriteInt32(l.Count);
    foreach (int num in l)
      c.WriteInt32(num);
  }
}
