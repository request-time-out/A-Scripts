// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem7
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem7 : MonoBehaviour
  {
    public Text mText;
    public int mValue;

    public ListItem7()
    {
      base.\u002Ector();
    }

    public void Init()
    {
    }

    public int Value
    {
      get
      {
        return this.mValue;
      }
      set
      {
        this.mValue = value;
      }
    }
  }
}
