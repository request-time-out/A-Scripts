// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.DataItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace Battlehub.UIControls
{
  public class DataItem
  {
    public string Name;
    public DataItem Parent;
    public List<DataItem> Children;

    public DataItem(string name)
    {
      this.Name = name;
      this.Children = new List<DataItem>();
    }

    public override string ToString()
    {
      return this.Name;
    }
  }
}
