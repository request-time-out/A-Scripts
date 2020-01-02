// Decompiled with JetBrains decompiler
// Type: Exploder.CutterST
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class CutterST : ExploderTask
  {
    protected readonly HashSet<MeshObject> newFragments;
    protected readonly HashSet<MeshObject> meshToRemove;
    protected readonly MeshCutter cutter;
    protected readonly CuttingPlane cuttingPlane;
    private int cutAttempt;

    public CutterST(Core Core)
      : base(Core)
    {
      this.cutter = new MeshCutter();
      this.cutter.Init(512, 512);
      this.newFragments = new HashSet<MeshObject>();
      this.meshToRemove = new HashSet<MeshObject>();
      this.cuttingPlane = new CuttingPlane(Core);
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
      this.newFragments.Clear();
      this.meshToRemove.Clear();
      this.cutAttempt = 0;
    }

    public override bool Run(float frameBudget)
    {
      if (!this.Cut(frameBudget))
        return false;
      this.Watch.Stop();
      return true;
    }

    protected virtual bool Cut(float frameBudget)
    {
      bool flag1 = true;
      int num = 0;
      bool flag2 = false;
      this.cutAttempt = 0;
      while (flag1)
      {
        ++num;
        if (num > this.core.parameters.TargetFragments)
          return true;
        this.newFragments.Clear();
        this.meshToRemove.Clear();
        flag1 = false;
        foreach (MeshObject mesh in this.core.meshSet)
        {
          if (this.core.targetFragments.ContainsKey(mesh.id))
            ;
          if (this.core.targetFragments[mesh.id] > 1)
          {
            List<ExploderMesh> exploderMeshList = this.CutSingleMesh(mesh);
            flag1 = true;
            if (exploderMeshList != null)
            {
              foreach (ExploderMesh exploderMesh in exploderMeshList)
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
              if ((double) this.Watch.ElapsedMilliseconds > (double) frameBudget && num > 2)
              {
                flag2 = true;
                break;
              }
            }
          }
        }
        this.core.meshSet.ExceptWith((IEnumerable<MeshObject>) this.meshToRemove);
        this.core.meshSet.UnionWith((IEnumerable<MeshObject>) this.newFragments);
        if (flag2)
          break;
      }
      return !flag2;
    }

    protected List<ExploderMesh> CutSingleMesh(MeshObject mesh)
    {
      Plane plane = this.cuttingPlane.GetPlane(mesh.mesh, this.cutAttempt);
      bool flag = true;
      Color crossSectionVertexColor = Color.get_white();
      Vector4 crossSectionUv;
      ((Vector4) ref crossSectionUv).\u002Ector(0.0f, 0.0f, 1f, 1f);
      if (Object.op_Implicit((Object) mesh.option))
      {
        flag = !mesh.option.Plane2D;
        crossSectionVertexColor = mesh.option.CrossSectionVertexColor;
        crossSectionUv = mesh.option.CrossSectionUV;
        this.core.splitMeshIslands |= mesh.option.SplitMeshIslands;
      }
      if (this.core.parameters.Use2DCollision)
        flag = false;
      bool triangulateHoles = flag & !this.core.parameters.DisableTriangulation;
      List<ExploderMesh> meshes = (List<ExploderMesh>) null;
      double num = (double) this.cutter.Cut(mesh.mesh, mesh.transform, plane, triangulateHoles, this.core.parameters.DisableTriangulation, ref meshes, crossSectionVertexColor, crossSectionUv);
      if (meshes == null)
        ++this.cutAttempt;
      return meshes;
    }
  }
}
