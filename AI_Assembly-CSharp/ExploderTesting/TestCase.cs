// Decompiled with JetBrains decompiler
// Type: ExploderTesting.TestCase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Exploder;
using Exploder.Utils;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ExploderTesting
{
  internal abstract class TestCase
  {
    private Stopwatch watch;

    protected TestCase()
    {
      this.watch = new Stopwatch();
    }

    [DebuggerHidden]
    public virtual IEnumerator Run()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TestCase.\u003CRun\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected ExploderObject Exploder
    {
      get
      {
        return ExploderSingleton.Instance;
      }
    }

    protected ExploderTester Tester
    {
      get
      {
        return ExploderTester.Instance;
      }
    }

    protected abstract IEnumerator RunTest();

    protected void OnTestSuccess()
    {
      Debug.LogFormat("Test success {0}", new object[1]
      {
        (object) this.ToString()
      });
    }

    protected void OnTestFailed(string reason)
    {
      Debug.LogErrorFormat("Test failed {0}, reason: {1}", new object[2]
      {
        (object) this.ToString(),
        (object) reason
      });
    }
  }
}
