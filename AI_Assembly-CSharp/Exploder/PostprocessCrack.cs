// Decompiled with JetBrains decompiler
// Type: Exploder.PostprocessCrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class PostprocessCrack : Postprocess
  {
    private CrackedObject crackedObject;

    public PostprocessCrack(Core Core)
      : base(Core)
    {
    }

    public override TaskType Type
    {
      get
      {
        return TaskType.PostprocessCrack;
      }
    }

    public override void Init()
    {
      base.Init();
      FragmentPool.Instance.ResetTransform();
      FragmentPool.Instance.Reset(this.core.parameters);
      this.crackedObject = (CrackedObject) null;
      if (this.core.meshSet.Count <= 0)
        return;
      if (!this.core.splitMeshIslands)
        this.core.postList = new List<MeshObject>((IEnumerable<MeshObject>) this.core.meshSet);
      this.crackedObject = this.core.crackManager.Create(!Object.op_Implicit((Object) this.core.postList[0].skinnedOriginal) ? this.core.postList[0].original : this.core.postList[0].skinnedOriginal, this.core.parameters);
      this.crackedObject.pool = FragmentPool.Instance.GetAvailableFragments(this.core.postList.Count);
    }

    public override bool Run(float frameBudget)
    {
      if (this.crackedObject == null)
        return true;
      int count = this.crackedObject.pool.Count;
      while (this.core.poolIdx < count)
      {
        Fragment fragment = this.crackedObject.pool[this.core.poolIdx];
        MeshObject post = this.core.postList[this.core.poolIdx];
        ++this.core.poolIdx;
        if (Object.op_Implicit((Object) post.original))
        {
          ExploderUtils.SetActiveRecursively(((Component) fragment).get_gameObject(), false);
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
          fragment.Cracked = true;
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
          float force = this.core.parameters.Force;
          if (Object.op_Implicit((Object) post.option) && post.option.UseLocalForce)
            force = post.option.Force;
          fragment.ApplyExplosion(post.transform, post.mesh.centroid, force, post.original, this.core.parameters);
          if ((double) this.Watch.ElapsedMilliseconds > (double) frameBudget)
            return false;
        }
      }
      this.crackedObject.CalculateFractureGrid();
      this.Watch.Stop();
      return true;
    }
  }
}
