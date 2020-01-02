// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem14
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem14 : MonoBehaviour
  {
    public List<ListItem14Elem> mElemItemList;

    public ListItem14()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      int childCount = ((Component) this).get_transform().get_childCount();
      for (int index = 0; index < childCount; ++index)
      {
        Transform child = ((Component) this).get_transform().GetChild(index);
        this.mElemItemList.Add(new ListItem14Elem()
        {
          mRootObj = ((Component) child).get_gameObject(),
          mIcon = (Image) ((Component) child.Find("ItemIcon")).GetComponent<Image>(),
          mName = (Text) ((Component) child.Find("ItemIcon/name")).GetComponent<Text>()
        });
      }
    }
  }
}
