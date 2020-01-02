// Decompiled with JetBrains decompiler
// Type: TestPageSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class TestPageSwitch : MonoBehaviour
{
  private int currentPage;

  public TestPageSwitch()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.ShowPage();
  }

  public void ShiftPage(int offset)
  {
    this.currentPage += offset;
    if (this.currentPage >= ((Component) this).get_transform().get_childCount())
      this.currentPage = 0;
    else if (this.currentPage < 0)
      this.currentPage = ((Component) this).get_transform().get_childCount() - 1;
    this.ShowPage();
  }

  private void ShowPage()
  {
    int num = 0;
    IEnumerator enumerator = ((Component) this).get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        ((Component) enumerator.Current).get_gameObject().SetActive(num == this.currentPage);
        ++num;
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }
}
