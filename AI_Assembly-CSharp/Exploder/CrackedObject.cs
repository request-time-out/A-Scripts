// Decompiled with JetBrains decompiler
// Type: Exploder.CrackedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
  internal class CrackedObject
  {
    public List<Fragment> pool;
    private readonly Stopwatch watch;
    private readonly Quaternion initRot;
    private readonly Vector3 initPos;
    private readonly GameObject originalObject;
    private readonly ExploderParams parameters;
    private readonly FractureGrid fractureGrid;

    public CrackedObject(GameObject originalObject, ExploderParams parameters)
    {
      this.originalObject = originalObject;
      this.parameters = parameters;
      this.fractureGrid = new FractureGrid(this);
      this.initPos = originalObject.get_transform().get_position();
      this.initRot = originalObject.get_transform().get_rotation();
      this.watch = new Stopwatch();
    }

    public void CalculateFractureGrid()
    {
      this.fractureGrid.CreateGrid();
    }

    public long Explode()
    {
      int count = this.pool.Count;
      int index = 0;
      if (count == 0)
        return 0;
      this.watch.Start();
      if (this.parameters.Callback != null)
        this.parameters.Callback(0.0f, ExploderObject.ExplosionState.ExplosionStarted);
      Vector3 vector3 = Vector3.get_zero();
      Quaternion quaternion = Quaternion.get_identity();
      if (Object.op_Implicit((Object) this.originalObject))
      {
        vector3 = Vector3.op_Subtraction(this.originalObject.get_transform().get_position(), this.initPos);
        quaternion = Quaternion.op_Multiply(this.originalObject.get_transform().get_rotation(), Quaternion.Inverse(this.initRot));
      }
      while (index < count)
      {
        Fragment fragment = this.pool[index];
        ++index;
        if (Object.op_Inequality((Object) this.originalObject, (Object) this.parameters.ExploderGameObject))
        {
          ExploderUtils.SetActiveRecursively(this.originalObject, false);
        }
        else
        {
          ExploderUtils.EnableCollider(this.originalObject, false);
          ExploderUtils.SetVisible(this.originalObject, false);
        }
        Transform transform1 = ((Component) fragment).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), vector3));
        Transform transform2 = ((Component) fragment).get_transform();
        transform2.set_rotation(Quaternion.op_Multiply(transform2.get_rotation(), quaternion));
        fragment.Explode(this.parameters);
      }
      if (this.parameters.DestroyOriginalObject && Object.op_Implicit((Object) this.originalObject) && !Object.op_Implicit((Object) this.originalObject.GetComponent<Fragment>()))
        Object.Destroy((Object) this.originalObject);
      if (this.parameters.ExplodeSelf && !this.parameters.DestroyOriginalObject)
        ExploderUtils.SetActiveRecursively(this.parameters.ExploderGameObject, false);
      if (this.parameters.HideSelf)
        ExploderUtils.SetActiveRecursively(this.parameters.ExploderGameObject, false);
      this.watch.Stop();
      return this.watch.ElapsedMilliseconds;
    }

    public long ExplodePartial(
      GameObject gameObject,
      Vector3 shotDir,
      Vector3 hitPosition,
      float bulletSize)
    {
      int count = this.pool.Count;
      int index = 0;
      if (count == 0)
        return 0;
      this.watch.Start();
      if (this.parameters.Callback != null)
        this.parameters.Callback(0.0f, ExploderObject.ExplosionState.ExplosionStarted);
      Vector3 vector3 = Vector3.get_zero();
      Quaternion quaternion = Quaternion.get_identity();
      if (Object.op_Implicit((Object) this.originalObject))
      {
        vector3 = Vector3.op_Subtraction(this.originalObject.get_transform().get_position(), this.initPos);
        quaternion = Quaternion.op_Multiply(this.originalObject.get_transform().get_rotation(), Quaternion.Inverse(this.initRot));
      }
      CombineInstance[] combineInstanceArray = new CombineInstance[count];
      for (; index < count; ++index)
      {
        Fragment fragment = this.pool[index];
        ((CombineInstance) ref combineInstanceArray[index]).set_mesh(fragment.meshFilter.get_sharedMesh());
        ((CombineInstance) ref combineInstanceArray[index]).set_transform(((Component) fragment).get_transform().get_localToWorldMatrix());
      }
      Mesh mesh = new Mesh();
      mesh.CombineMeshes(combineInstanceArray, true, false);
      ((MeshFilter) this.originalObject.GetComponent<MeshFilter>()).set_mesh(mesh);
      this.watch.Stop();
      return this.watch.ElapsedMilliseconds;
    }
  }
}
