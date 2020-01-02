// Decompiled with JetBrains decompiler
// Type: Exploder.CutterWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Exploder
{
  internal class CutterWorker
  {
    private readonly ManualResetEvent mre = new ManualResetEvent(false);
    private readonly HashSet<MeshObject> newFragments;
    private readonly HashSet<MeshObject> meshToRemove;
    private readonly HashSet<MeshObject> meshSet;
    private readonly MeshCutter cutter;
    private readonly CuttingPlane cuttingPlane;
    private readonly Core core;
    private volatile bool running;
    private int cutAttempt;
    private Thread thread;

    public CutterWorker(Core core, CuttingPlane cuttingPlane)
    {
      this.cutter = new MeshCutter();
      this.cutter.Init(512, 512);
      this.newFragments = new HashSet<MeshObject>();
      this.meshToRemove = new HashSet<MeshObject>();
      this.meshSet = new HashSet<MeshObject>();
      this.cuttingPlane = cuttingPlane;
      this.core = core;
      this.thread = new Thread(new ThreadStart(this.ThreadRun));
      this.thread.IsBackground = true;
      this.thread.Start();
    }

    public void Init()
    {
      this.meshSet.Clear();
      this.running = false;
      this.cutAttempt = 0;
    }

    public void AddMesh(MeshObject meshObject)
    {
      this.meshSet.Add(meshObject);
    }

    public void Run()
    {
      this.running = true;
      Thread.MemoryBarrier();
      this.mre.Set();
    }

    private void ThreadRun()
    {
      this.mre.WaitOne();
      try
      {
        this.Cut();
      }
      finally
      {
        this.running = false;
        Thread.MemoryBarrier();
        this.mre.Reset();
        this.thread = new Thread(new ThreadStart(this.ThreadRun));
        this.thread.IsBackground = true;
        this.thread.Start();
      }
    }

    public bool IsFinished()
    {
      return !this.running;
    }

    public HashSet<MeshObject> GetResults()
    {
      return this.meshSet;
    }

    public void Terminate()
    {
      this.mre.Close();
    }

    private void Cut()
    {
      bool flag = true;
      int num1 = 0;
      this.cutAttempt = 0;
      while (flag)
      {
        ++num1;
        if (num1 > this.core.parameters.TargetFragments)
          break;
        this.newFragments.Clear();
        this.meshToRemove.Clear();
        flag = false;
        foreach (MeshObject mesh in this.meshSet)
        {
          if (this.core.targetFragments[mesh.id] > 1)
          {
            Plane plane = this.cuttingPlane.GetPlane(mesh.mesh, this.cutAttempt);
            bool triangulateHoles = true;
            Color crossSectionVertexColor = Color.get_white();
            Vector4 crossSectionUv;
            ((Vector4) ref crossSectionUv).\u002Ector(0.0f, 0.0f, 1f, 1f);
            if (Object.op_Implicit((Object) mesh.option))
            {
              triangulateHoles = !mesh.option.Plane2D;
              crossSectionVertexColor = mesh.option.CrossSectionVertexColor;
              crossSectionUv = mesh.option.CrossSectionUV;
              this.core.splitMeshIslands |= mesh.option.SplitMeshIslands;
            }
            if (this.core.parameters.Use2DCollision)
              triangulateHoles = false;
            List<ExploderMesh> meshes = (List<ExploderMesh>) null;
            double num2 = (double) this.cutter.Cut(mesh.mesh, mesh.transform, plane, triangulateHoles, this.core.parameters.DisableTriangulation, ref meshes, crossSectionVertexColor, crossSectionUv);
            flag = true;
            if (meshes != null)
            {
              foreach (ExploderMesh exploderMesh in meshes)
                this.newFragments.Add(new MeshObject()
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
                });
              this.meshToRemove.Add(mesh);
              Dictionary<int, int> targetFragments;
              int id;
              (targetFragments = this.core.targetFragments)[id = mesh.id] = targetFragments[id] - 1;
            }
            else
              ++this.cutAttempt;
          }
        }
        this.meshSet.ExceptWith((IEnumerable<MeshObject>) this.meshToRemove);
        this.meshSet.UnionWith((IEnumerable<MeshObject>) this.newFragments);
      }
    }
  }
}
