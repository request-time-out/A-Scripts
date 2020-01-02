// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem15
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class ListItem15 : MonoBehaviour
  {
    public List<ListItem16> mItemList;

    public ListItem15()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      foreach (ListItem16 mItem in this.mItemList)
        mItem.Init();
    }
  }
}
