// Decompiled with JetBrains decompiler
// Type: Exploder.CutterMT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
  internal class CutterMT : CutterST
  {
    private readonly Stopwatch localWatch = new Stopwatch();
    protected readonly int THREAD_MAX;
    protected readonly CutterWorker[] workers;
    private readonly int[] splitIDs;
    private bool cutInitialized;

    public CutterMT(Core Core)
      : base(Core)
    {
      this.splitIDs = new int[2];
      this.THREAD_MAX = Mathf.Clamp((int) (Core.parameters.ThreadOptions + 2), 1, 4);
      Debug.LogFormat("Exploder: using {0} threads.", new object[1]
      {
        (object) (this.THREAD_MAX - 1)
      });
      this.workers = new CutterWorker[this.THREAD_MAX - 1];
      for (int index = 0; index < this.THREAD_MAX - 1; ++index)
        this.workers[index] = new CutterWorker(Core, new CuttingPlane(Core));
    }

    public override TaskType Type
    {
      get
      {
        return TaskType.ProcessCutter;
      }
    }

    public override void Init()
    {
      base.Init();
      this.cutInitialized = false;
      foreach (CutterWorker worker in this.workers)
        worker.Init();
      this.localWatch.Reset();
    }

    public override void OnDestroy()
    {
      foreach (CutterWorker worker in this.workers)
        worker.Terminate();
    }

    public override bool Run(float frameBudget)
    {
      if (this.Cut(frameBudget))
      {
        bool flag = true;
        foreach (CutterWorker worker in this.workers)
          flag &= worker.IsFinished();
        if (flag)
        {
          foreach (CutterWorker worker in this.workers)
            this.core.meshSet.UnionWith((IEnumerable<MeshObject>) worker.GetResults());
          this.Watch.Stop();
          return true;
        }
      }
      return false;
    }

    protected override bool Cut(float frameBudget)
    {
      if (this.cutInitialized)
        return true;
      this.localWatch.Start();
      this.cutInitialized = true;
      if (this.core.parameters.TargetFragments < 2 || this.core.meshSet.Count == this.core.parameters.TargetFragments)
        return true;
      int num1 = this.THREAD_MAX - 1 - this.core.meshSet.Count;
      if (num1 > this.core.parameters.TargetFragments - 1)
        num1 = this.core.parameters.TargetFragments - 1;
      int num2 = 0;
      while (num1 > 0)
      {
        this.newFragments.Clear();
        foreach (MeshObject mesh in this.core.meshSet)
        {
          List<ExploderMesh> exploderMeshList = this.CutSingleMesh(mesh);
          ++num2;
          if (num2 > this.core.parameters.TargetFragments)
          {
            num1 = 0;
            break;
          }
          if (exploderMeshList != null)
          {
            --num1;
            int[] numArray = this.SplitMeshTargetFragments(mesh.id);
            int num3 = 0;
            foreach (ExploderMesh exploderMesh in exploderMeshList)
            {
              MeshObject meshObject = new MeshObject()
              {
                mesh = exploderMesh,
                material = mesh.material,
                transform = mesh.transform,
                id = mesh.id,
                original = mesh.original,
                skinnedOriginal = mesh.skinnedOriginal,
                bakeObject = mesh.bakeObject,
                parent = mesh.transform.parent,
                position = mesh.transform.position,
                rotation = mesh.transform.rotation,
                localScale = mesh.transform.localScale,
                option = mesh.option
              };
              meshObject.id = numArray[num3++];
              this.newFragments.Add(meshObject);
            }
            this.meshToRemove.Add(mesh);
            break;
          }
        }
        this.core.meshSet.ExceptWith((IEnumerable<MeshObject>) this.meshToRemove);
        this.core.meshSet.UnionWith((IEnumerable<MeshObject>) this.newFragments);
      }
      if (this.core.meshSet.Count >= this.core.parameters.TargetFragments)
        return true;
      int num4 = this.core.meshSet.Count / (this.THREAD_MAX - 1);
      int index = 0;
      int num5 = 0;
      foreach (MeshObject mesh in this.core.meshSet)
      {
        this.workers[index].AddMesh(mesh);
        ++num5;
        if (num5 >= num4 && index < this.THREAD_MAX - 2)
        {
          num5 = 0;
          ++index;
        }
      }
      this.core.meshSet.Clear();
      foreach (CutterWorker worker in this.workers)
        worker.Run();
      this.localWatch.Stop();
      return true;
    }

    private int[] SplitMeshTargetFragments(int id)
    {
      int num1 = this.core.targetFragments[id] / 2;
      int num2 = num1;
      if (this.core.targetFragments[id] % 2 == 1)
        ++num2;
      this.splitIDs[0] = (id + 1) * 100;
      this.splitIDs[1] = (id + 1) * 200;
      this.core.targetFragments.Add(this.splitIDs[0], num1);
      this.core.targetFragments.Add(this.splitIDs[1], num2);
      return this.splitIDs;
    }
  }
}
