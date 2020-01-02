// Decompiled with JetBrains decompiler
// Type: Benchmark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Exploder;
using Exploder.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Benchmark : MonoBehaviour
{
  public GameObject testObjects;
  private MeshRenderer[] objects;
  private int index;
  private int rounds;
  private int batchIndex;
  private int[] batches;
  private readonly Dictionary<string, Benchmark.Report> report;

  public Benchmark()
  {
    base.\u002Ector();
  }

  private void AddReport(Benchmark.Report r)
  {
    if (this.report.ContainsKey(r.name))
    {
      this.report[r.name].ms += r.ms;
      ++this.report[r.name].count;
      this.report[r.name].frames += r.frames;
    }
    else
      this.report.Add(r.name, r);
  }

  private void PrintReport()
  {
    Debug.Log((object) "PrintReportTotal");
    string contents = "Report:\n\n";
    foreach (Benchmark.Report report in this.report.Values)
    {
      string str = string.Format("{0}: {1}[ms] {2}[frames]", (object) report.name, (object) (float) ((double) report.ms / (double) report.count), (object) (report.frames / report.count));
      contents = contents + str + "\n";
    }
    File.WriteAllText("c:\\Development\\Unity\\AssetStore\\exploder-git\\benchmark.txt", contents);
  }

  private void Start()
  {
    this.objects = (MeshRenderer[]) this.testObjects.GetComponentsInChildren<MeshRenderer>(true);
  }

  private void OnGUI()
  {
    if (!GUI.Button(new Rect(10f, 10f, 100f, 50f), "Start"))
      return;
    this.ExplodeObject();
  }

  private void ExplodeObject()
  {
    if (this.rounds == 0)
    {
      ++this.batchIndex;
      this.rounds = 5;
      if (this.batchIndex >= this.batches.Length)
      {
        this.PrintReport();
        return;
      }
    }
    int targetFragments = this.batches[this.batchIndex];
    ExploderSingleton.Instance.TargetFragments = targetFragments;
    if (this.index >= this.objects.Length)
    {
      FragmentPool.Instance.DeactivateFragments();
      --this.rounds;
      this.index = 0;
    }
    ((Component) this.objects[this.index]).get_gameObject().SetActive(true);
    ExploderSingleton.Instance.ExplodeObject(((Component) this.objects[this.index]).get_gameObject(), (ExploderObject.OnExplosion) ((ms, state) =>
    {
      if (state != ExploderObject.ExplosionState.ExplosionFinished)
        return;
      int processingFrames = ExploderSingleton.Instance.ProcessingFrames;
      this.AddReport(new Benchmark.Report()
      {
        name = ((Object) ((Component) this.objects[this.index]).get_gameObject()).get_name() + "[" + (object) targetFragments + "]",
        ms = ms,
        frames = processingFrames,
        count = 1
      });
      ++this.index;
      this.ExplodeObject();
    }));
  }

  private class Report
  {
    public string name;
    public float ms;
    public int frames;
    public int count;
  }
}
