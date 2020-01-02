// Decompiled with JetBrains decompiler
// Type: Exploder.PostprocessExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class PostprocessExplode : Postprocess
  {
    public PostprocessExplode(Core Core)
      : base(Core)
    {
    }

    public override TaskType Type
    {
      get
      {
        return TaskType.PostprocessExplode;
      }
    }

    public override void Init()
    {
      base.Init();
      if (!this.core.splitMeshIslands)
        this.core.postList = new List<MeshObject>((IEnumerable<MeshObject>) this.core.meshSet);
      int count = this.core.postList.Count;
      if (count == 0)
        return;
      FragmentPool.Instance.Reset(this.core.parameters);
      this.core.pool = FragmentPool.Instance.GetAvailableFragments(count);
      if (this.core.parameters.Callback == null)
        return;
      this.core.parameters.Callback((float) this.Watch.ElapsedMilliseconds, ExploderObject.ExplosionState.ExplosionStarted);
    }

    public override bool Run(float frameBudget)
    {
      int count = this.core.pool.Count;
      while (this.core.poolIdx < count)
      {
        Fragment fragment = this.core.pool[this.core.poolIdx];
        MeshObject post = this.core.postList[this.core.poolIdx];
        ++this.core.poolIdx;
        if (Object.op_Implicit((Object) post.original))
        {
          Mesh unityMesh = post.mesh.ToUnityMesh();
          fragment.AssignMesh(unityMesh);
          if (Object.op_Implicit((Object) post.option) && Object.op_Implicit((Object) post.option.FragmentMaterial))
            ((Renderer) fragment.meshRenderer).set_sharedMaterial(post.option.FragmentMaterial);
          else if (Object.op_Inequality((Object) this.core.parameters.FragmentOptions.FragmentMaterial, (Object) null))
            ((Renderer) fragment.meshRenderer).set_sharedMaterial(this.core.parameters.FragmentOptions.FragmentMaterial);
          else
            ((Renderer) fragment.meshRenderer).set_sharedMaterial(post.material);
          unityMesh.RecalculateBounds();
          Transform parent = ((Component) fragment).get_transform().get_parent();
          ((Component) fragment).get_transform().set_parent(post.parent);
          ((Component) fragment).get_transform().set_position(post.position);
          ((Component) fragment).get_transform().set_rotation(post.rotation);
          ((Component) fragment).get_transform().set_localScale(post.localScale);
          ((Component) fragment).get_transform().set_parent((Transform) null);
          ((Component) fragment).get_transform().set_parent(parent);
          if (Object.op_Inequality((Object) post.original, (Object) this.core.parameters.ExploderGameObject))
          {
            ExploderUtils.SetActiveRecursively(post.original, false);
          }
          else
          {
            ExploderUtils.EnableCollider(post.original, false);
            ExploderUtils.SetVisible(post.original, false);
          }
          if (Object.op_Implicit((Object) post.skinnedOriginal) && Object.op_Inequality((Object) post.skinnedOriginal, (Object) this.core.parameters.ExploderGameObject))
          {
            ExploderUtils.SetActiveRecursively(post.skinnedOriginal, false);
          }
          else
          {
            ExploderUtils.EnableCollider(post.skinnedOriginal, false);
            ExploderUtils.SetVisible(post.skinnedOriginal, false);
          }
          if (Object.op_Implicit((Object) post.skinnedOriginal) && Object.op_Implicit((Object) post.bakeObject))
            Object.Destroy((Object) post.bakeObject, 1f);
          bool flag = Object.op_Implicit((Object) post.option) && post.option.Plane2D;
          bool use2Dcollision = this.core.parameters.Use2DCollision;
          if (!this.core.parameters.FragmentOptions.DisableColliders)
          {
            if (this.core.parameters.FragmentOptions.MeshColliders && !use2Dcollision)
            {
              if (!flag)
                fragment.meshCollider.set_sharedMesh(unityMesh);
            }
            else if (this.core.parameters.Use2DCollision)
            {
              MeshUtils.GeneratePolygonCollider(fragment.polygonCollider2D, unityMesh);
            }
            else
            {
              BoxCollider boxCollider1 = fragment.boxCollider;
              Bounds bounds1 = unityMesh.get_bounds();
              Vector3 center = ((Bounds) ref bounds1).get_center();
              boxCollider1.set_center(center);
              BoxCollider boxCollider2 = fragment.boxCollider;
              Bounds bounds2 = unityMesh.get_bounds();
              Vector3 extents = ((Bounds) ref bounds2).get_extents();
              boxCollider2.set_size(extents);
            }
          }
          fragment.Explode(this.core.parameters);
          float force = this.core.parameters.Force;
          if (Object.op_Implicit((Object) post.option) && post.option.UseLocalForce)
            force = post.option.Force;
          fragment.ApplyExplosion(post.transform, post.mesh.centroid, force, post.original, this.core.parameters);
          if ((double) this.Watch.ElapsedMilliseconds > (double) frameBudget)
            return false;
        }
      }
      if (this.core.parameters.DestroyOriginalObject)
      {
        foreach (MeshObject post in this.core.postList)
        {
          if (Object.op_Implicit((Object) post.original) && !Object.op_Implicit((Object) post.original.GetComponent<Fragment>()))
            Object.Destroy((Object) post.original);
          if (Object.op_Implicit((Object) post.skinnedOriginal))
            Object.Destroy((Object) post.skinnedOriginal);
        }
      }
      if (this.core.parameters.ExplodeSelf && !this.core.parameters.DestroyOriginalObject)
        ExploderUtils.SetActiveRecursively(this.core.parameters.ExploderGameObject, false);
      if (this.core.parameters.HideSelf)
        ExploderUtils.SetActiveRecursively(this.core.parameters.ExploderGameObject, false);
      this.Watch.Stop();
      return true;
    }
  }
}
