// Decompiled with JetBrains decompiler
// Type: Exploder.IsolateMeshIslands
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class IsolateMeshIslands : ExploderTask
  {
    private readonly List<MeshObject> islands;

    public IsolateMeshIslands(Core Core)
      : base(Core)
    {
      this.islands = new List<MeshObject>();
    }

    public override TaskType Type
    {
      get
      {
        return TaskType.IsolateMeshIslands;
      }
    }

    public override void Init()
    {
      base.Init();
      this.islands.Clear();
      this.core.poolIdx = 0;
      this.core.postList = new List<MeshObject>((IEnumerable<MeshObject>) this.core.meshSet);
    }

    public override bool Run(float frameBudget)
    {
      int count = this.core.postList.Count;
      while (this.core.poolIdx < count)
      {
        MeshObject post = this.core.postList[this.core.poolIdx];
        ++this.core.poolIdx;
        bool flag = false;
        if (this.core.parameters.SplitMeshIslands || Object.op_Implicit((Object) post.option) && post.option.SplitMeshIslands)
        {
          List<ExploderMesh> exploderMeshList = MeshUtils.IsolateMeshIslands(post.mesh);
          if (exploderMeshList != null)
          {
            flag = true;
            foreach (ExploderMesh exploderMesh in exploderMeshList)
              this.islands.Add(new MeshObject()
              {
                mesh = exploderMesh,
                material = post.material,
                transform = post.transform,
                original = post.original,
                skinnedOriginal = post.skinnedOriginal,
                parent = post.transform.parent,
                position = post.transform.position,
                rotation = post.transform.rotation,
                localScale = post.transform.localScale,
                option = post.option
              });
          }
        }
        if (!flag)
          this.islands.Add(post);
        if ((double) this.Watch.ElapsedMilliseconds > (double) frameBudget)
          return false;
      }
      this.core.postList = this.islands;
      this.Watch.Stop();
      return true;
    }
  }
}
