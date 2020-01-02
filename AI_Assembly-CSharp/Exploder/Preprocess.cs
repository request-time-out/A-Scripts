// Decompiled with JetBrains decompiler
// Type: Exploder.Preprocess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class Preprocess : ExploderTask
  {
    public Preprocess(Core Core)
      : base(Core)
    {
      Core.targetFragments = new Dictionary<int, int>(4);
    }

    public override TaskType Type
    {
      get
      {
        return TaskType.Preprocess;
      }
    }

    public override void Init()
    {
      base.Init();
      this.core.targetFragments.Clear();
    }

    public override bool Run(float frameBudget)
    {
      List<MeshObject> meshList = this.GetMeshList();
      if (meshList.Count == 0)
      {
        this.Watch.Stop();
        this.core.meshSet.Clear();
        return true;
      }
      this.core.meshSet.Clear();
      foreach (MeshObject meshObject in meshList)
      {
        if (this.core.targetFragments[meshObject.id] > 0)
          this.core.meshSet.Add(meshObject);
      }
      this.core.splitMeshIslands = this.core.parameters.SplitMeshIslands;
      this.Watch.Stop();
      return true;
    }

    private List<MeshObject> GetMeshList()
    {
      List<GameObject> gameObjectList;
      if (this.core.parameters.Targets != null)
        gameObjectList = new List<GameObject>((IEnumerable<GameObject>) this.core.parameters.Targets);
      else if (this.core.parameters.DontUseTag)
      {
        Object[] objectsOfType = Object.FindObjectsOfType(typeof (Explodable));
        gameObjectList = new List<GameObject>(objectsOfType.Length);
        foreach (Explodable explodable in objectsOfType)
        {
          if (Object.op_Implicit((Object) explodable))
            gameObjectList.Add(((Component) explodable).get_gameObject());
        }
      }
      else
        gameObjectList = new List<GameObject>((IEnumerable<GameObject>) GameObject.FindGameObjectsWithTag(ExploderObject.Tag));
      if (this.core.parameters.ExplodeSelf)
        gameObjectList.Add(this.core.parameters.ExploderGameObject);
      List<MeshObject> meshObjectList = new List<MeshObject>(gameObjectList.Count);
      int num1 = 0;
      Vector3 vector3_1 = Vector3.get_zero();
      int num2 = 0;
      using (List<GameObject>.Enumerator enumerator = gameObjectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Implicit((Object) current) && (this.core.parameters.ExplodeSelf || !Object.op_Equality((Object) current, (Object) this.core.parameters.ExploderGameObject)) && (!Object.op_Inequality((Object) current, (Object) this.core.parameters.ExploderGameObject) || !this.core.parameters.ExplodeSelf || !this.core.parameters.DisableRadiusScan) && (this.core.parameters.Targets != null || this.IsInRadius(current)))
          {
            List<Preprocess.MeshData> meshData = this.GetMeshData(current);
            int count = meshData.Count;
            for (int index = 0; index < count; ++index)
            {
              Vector3 centroid = meshData[index].centroid;
              if (this.core.parameters.Targets != null)
              {
                this.core.parameters.Position = centroid;
                vector3_1 = Vector3.op_Addition(vector3_1, centroid);
                ++num2;
              }
              Vector3 vector3_2 = Vector3.op_Subtraction(centroid, this.core.parameters.Position);
              float magnitude = ((Vector3) ref vector3_2).get_magnitude();
              meshObjectList.Add(new MeshObject()
              {
                id = num1++,
                mesh = new ExploderMesh(meshData[index].sharedMesh),
                material = meshData[index].sharedMaterial,
                transform = new ExploderTransform(meshData[index].gameObject.get_transform()),
                parent = meshData[index].gameObject.get_transform().get_parent(),
                position = meshData[index].gameObject.get_transform().get_position(),
                rotation = meshData[index].gameObject.get_transform().get_rotation(),
                localScale = meshData[index].gameObject.get_transform().get_localScale(),
                bakeObject = meshData[index].gameObject,
                distanceRatio = this.GetDistanceRatio(magnitude, this.core.parameters.Radius),
                original = meshData[index].parentObject,
                skinnedOriginal = meshData[index].skinnedBakeOriginal,
                option = (ExploderOption) current.GetComponent<ExploderOption>()
              });
            }
          }
        }
      }
      if (num2 > 0)
        this.core.parameters.Position = Vector3.op_Division(vector3_1, (float) num2);
      if (meshObjectList.Count == 0)
        return meshObjectList;
      if (this.core.parameters.UniformFragmentDistribution || this.core.parameters.Targets != null)
      {
        int num3 = this.core.parameters.TargetFragments / meshObjectList.Count;
        int targetFragments1 = this.core.parameters.TargetFragments;
        foreach (MeshObject meshObject in meshObjectList)
        {
          this.core.targetFragments[meshObject.id] = num3;
          targetFragments1 -= num3;
        }
        while (targetFragments1 > 0)
        {
          --targetFragments1;
          MeshObject meshObject = meshObjectList[Random.Range(0, meshObjectList.Count - 1)];
          Dictionary<int, int> targetFragments2;
          int id;
          (targetFragments2 = this.core.targetFragments)[id = meshObject.id] = targetFragments2[id] + 1;
        }
      }
      else
      {
        float num3 = 0.0f;
        int num4 = 0;
        foreach (MeshObject meshObject in meshObjectList)
          num3 += meshObject.distanceRatio;
        foreach (MeshObject meshObject in meshObjectList)
        {
          this.core.targetFragments[meshObject.id] = (int) ((double) meshObject.distanceRatio / (double) num3 * (double) this.core.parameters.TargetFragments);
          num4 += this.core.targetFragments[meshObject.id];
        }
        if (num4 < this.core.parameters.TargetFragments)
        {
          int num5 = this.core.parameters.TargetFragments - num4;
          while (num5 > 0)
          {
            foreach (MeshObject meshObject in meshObjectList)
            {
              Dictionary<int, int> targetFragments;
              int id;
              (targetFragments = this.core.targetFragments)[id = meshObject.id] = targetFragments[id] + 1;
              --num5;
              if (num5 == 0)
                break;
            }
          }
        }
      }
      return meshObjectList;
    }

    private List<Preprocess.MeshData> GetMeshData(GameObject obj)
    {
      MeshRenderer[] componentsInChildren1 = (MeshRenderer[]) obj.GetComponentsInChildren<MeshRenderer>();
      MeshFilter[] componentsInChildren2 = (MeshFilter[]) obj.GetComponentsInChildren<MeshFilter>();
      if (componentsInChildren1.Length != componentsInChildren2.Length)
        return new List<Preprocess.MeshData>();
      List<Preprocess.MeshData> meshDataList1 = new List<Preprocess.MeshData>(componentsInChildren1.Length);
      for (int index = 0; index < componentsInChildren1.Length; ++index)
      {
        if (!Object.op_Equality((Object) componentsInChildren2[index].get_sharedMesh(), (Object) null))
        {
          if (!Object.op_Implicit((Object) componentsInChildren2[index].get_sharedMesh()) || !componentsInChildren2[index].get_sharedMesh().get_isReadable())
          {
            Debug.LogWarning((object) ("Mesh is not readable: " + ((Object) componentsInChildren2[index]).get_name()));
          }
          else
          {
            List<Preprocess.MeshData> meshDataList2 = meshDataList1;
            Preprocess.MeshData meshData1 = new Preprocess.MeshData();
            meshData1.sharedMesh = componentsInChildren2[index].get_sharedMesh();
            meshData1.sharedMaterial = ((Renderer) componentsInChildren1[index]).get_sharedMaterial();
            meshData1.gameObject = ((Component) componentsInChildren1[index]).get_gameObject();
            ref Preprocess.MeshData local = ref meshData1;
            Bounds bounds = ((Renderer) componentsInChildren1[index]).get_bounds();
            Vector3 center = ((Bounds) ref bounds).get_center();
            local.centroid = center;
            meshData1.parentObject = obj;
            Preprocess.MeshData meshData2 = meshData1;
            meshDataList2.Add(meshData2);
          }
        }
      }
      SkinnedMeshRenderer[] componentsInChildren3 = (SkinnedMeshRenderer[]) obj.GetComponentsInChildren<SkinnedMeshRenderer>();
      for (int index = 0; index < componentsInChildren3.Length; ++index)
      {
        Mesh mesh = new Mesh();
        componentsInChildren3[index].BakeMesh(mesh);
        GameObject bakeObject = this.core.bakeSkinManager.CreateBakeObject(((Object) obj).get_name());
        ((MeshFilter) bakeObject.AddComponent<MeshFilter>()).set_sharedMesh(mesh);
        MeshRenderer meshRenderer = (MeshRenderer) bakeObject.AddComponent<MeshRenderer>();
        ((Renderer) meshRenderer).set_sharedMaterial(((Renderer) componentsInChildren3[index]).get_material());
        bakeObject.get_transform().set_position(((Component) componentsInChildren3[index]).get_gameObject().get_transform().get_position());
        bakeObject.get_transform().set_rotation(((Component) componentsInChildren3[index]).get_gameObject().get_transform().get_rotation());
        ExploderUtils.SetVisible(bakeObject, false);
        List<Preprocess.MeshData> meshDataList2 = meshDataList1;
        Preprocess.MeshData meshData1 = new Preprocess.MeshData();
        meshData1.sharedMesh = mesh;
        meshData1.sharedMaterial = ((Renderer) meshRenderer).get_sharedMaterial();
        meshData1.gameObject = bakeObject;
        ref Preprocess.MeshData local = ref meshData1;
        Bounds bounds = ((Renderer) meshRenderer).get_bounds();
        Vector3 center = ((Bounds) ref bounds).get_center();
        local.centroid = center;
        meshData1.parentObject = bakeObject;
        meshData1.skinnedBakeOriginal = obj;
        Preprocess.MeshData meshData2 = meshData1;
        meshDataList2.Add(meshData2);
      }
      return meshDataList1;
    }

    private float GetDistanceRatio(float distance, float radius)
    {
      return 1f - Mathf.Clamp01(distance / radius);
    }

    private bool IsInRadius(GameObject o)
    {
      Vector3 centroid = ExploderUtils.GetCentroid(o);
      if (this.core.parameters.UseCubeRadius)
      {
        Vector3 vector3_1 = this.core.parameters.ExploderGameObject.get_transform().InverseTransformPoint(centroid);
        Vector3 vector3_2 = this.core.parameters.ExploderGameObject.get_transform().InverseTransformPoint(this.core.parameters.Position);
        return (double) Mathf.Abs((float) (vector3_1.x - vector3_2.x)) < this.core.parameters.CubeRadius.x && (double) Mathf.Abs((float) (vector3_1.y - vector3_2.y)) < this.core.parameters.CubeRadius.y && (double) Mathf.Abs((float) (vector3_1.z - vector3_2.z)) < this.core.parameters.CubeRadius.z;
      }
      double num = (double) this.core.parameters.Radius * (double) this.core.parameters.Radius;
      Vector3 vector3 = Vector3.op_Subtraction(centroid, this.core.parameters.Position);
      double sqrMagnitude = (double) ((Vector3) ref vector3).get_sqrMagnitude();
      return num > sqrMagnitude;
    }

    private struct MeshData
    {
      public Mesh sharedMesh;
      public Material sharedMaterial;
      public GameObject gameObject;
      public GameObject parentObject;
      public GameObject skinnedBakeOriginal;
      public Vector3 centroid;
    }
  }
}
