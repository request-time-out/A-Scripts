// Decompiled with JetBrains decompiler
// Type: ExploderTesting.ExploderTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ExploderTesting
{
  public class ExploderTester : MonoBehaviour
  {
    public GameObject[] testObjects;
    private List<TestCase> cases;
    public static ExploderTester Instance;

    public ExploderTester()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      ExploderTester.Instance = this;
      int[] numArray = new int[8]
      {
        0,
        1,
        2,
        5,
        10,
        50,
        100,
        200
      };
      this.cases.Add((TestCase) new DestroyedObject_Test());
      foreach (int count in numArray)
        this.cases.Add((TestCase) new FragmentCountTest(count));
      foreach (int target in numArray)
        this.cases.Add((TestCase) new FragmentCountTestMultiple(target, Random.Range(1, target)));
      foreach (int count in numArray)
        this.cases.Add((TestCase) new FragmentCrackCount(count));
    }

    private void OnGUI()
    {
      if (!GUI.Button(new Rect(10f, 10f, 100f, 50f), "Start Test"))
        return;
      this.StartCoroutine(this.StartTesting());
    }

    [DebuggerHidden]
    private IEnumerator StartTesting()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ExploderTester.\u003CStartTesting\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
