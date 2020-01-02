// Decompiled with JetBrains decompiler
// Type: Studio.StartScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Studio
{
  public class StartScene : MonoBehaviour
  {
    public StartScene()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    private IEnumerator LoadCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StartScene.\u003CLoadCoroutine\u003Ec__Iterator0 coroutineCIterator0 = new StartScene.\u003CLoadCoroutine\u003Ec__Iterator0();
      return (IEnumerator) coroutineCIterator0;
    }

    private void Start()
    {
      this.StartCoroutine(this.LoadCoroutine());
    }
  }
}
