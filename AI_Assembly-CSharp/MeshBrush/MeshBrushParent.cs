// Decompiled with JetBrains decompiler
// Type: MeshBrush.MeshBrushParent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

namespace MeshBrush
{
  public class MeshBrushParent : MonoBehaviour
  {
    private Transform[] paintedMeshes;
    private MeshFilter[] meshFilters;
    private Matrix4x4 localTransformationMatrix;
    private Hashtable materialToMesh;
    private MeshFilter currentMeshFilter;
    private Renderer currentRenderer;
    private Material[] materials;
    private CombineUtility.MeshInstance instance;
    private CombineUtility.MeshInstance[] instances;
    private ArrayList objects;
    private ArrayList elements;

    public MeshBrushParent()
    {
      base.\u002Ector();
    }

    private void Initialize()
    {
      this.paintedMeshes = (Transform[]) ((Component) this).GetComponentsInChildren<Transform>();
      this.meshFilters = (MeshFilter[]) ((Component) this).GetComponentsInChildren<MeshFilter>();
    }

    public void FlagMeshesAsStatic()
    {
      this.Initialize();
      for (int index = this.paintedMeshes.Length - 1; index >= 0; --index)
        ((Component) this.paintedMeshes[index]).get_gameObject().set_isStatic(true);
    }

    public void UnflagMeshesAsStatic()
    {
      this.Initialize();
      for (int index = this.paintedMeshes.Length - 1; index >= 0; --index)
        ((Component) this.paintedMeshes[index]).get_gameObject().set_isStatic(false);
    }

    public int GetMeshCount()
    {
      this.Initialize();
      return this.meshFilters.Length;
    }

    public int GetTrisCount()
    {
      this.Initialize();
      if (this.meshFilters.Length <= 0)
        return 0;
      int num = 0;
      for (int index = this.meshFilters.Length - 1; index >= 0; --index)
        num += this.meshFilters[index].get_sharedMesh().get_triangles().Length;
      return num / 3;
    }

    public void DeleteAllMeshes()
    {
      Object.DestroyImmediate((Object) ((Component) this).get_gameObject());
    }

    public void CombinePaintedMeshes(bool autoSelect, MeshFilter[] meshFilters)
    {
      if (meshFilters == null || meshFilters.Length == 0)
      {
        Debug.LogError((object) "MeshBrush: The meshFilters array you passed as an argument to the CombinePaintedMeshes function is empty or null... Combining action cancelled!");
      }
      else
      {
        this.localTransformationMatrix = ((Component) this).get_transform().get_worldToLocalMatrix();
        this.materialToMesh = new Hashtable();
        int num = 0;
        for (long index = 0; index < meshFilters.LongLength; ++index)
        {
          this.currentMeshFilter = meshFilters[index];
          num += this.currentMeshFilter.get_sharedMesh().get_vertexCount();
          if (num > 64000)
            return;
        }
        for (long index = 0; index < meshFilters.LongLength; ++index)
        {
          this.currentMeshFilter = meshFilters[index];
          this.currentRenderer = (Renderer) ((Component) meshFilters[index]).GetComponent<Renderer>();
          this.instance = new CombineUtility.MeshInstance();
          this.instance.mesh = this.currentMeshFilter.get_sharedMesh();
          if (Object.op_Inequality((Object) this.currentRenderer, (Object) null) && this.currentRenderer.get_enabled() && Object.op_Inequality((Object) this.instance.mesh, (Object) null))
          {
            this.instance.transform = Matrix4x4.op_Multiply(this.localTransformationMatrix, ((Component) this.currentMeshFilter).get_transform().get_localToWorldMatrix());
            this.materials = this.currentRenderer.get_sharedMaterials();
            for (int val1 = 0; val1 < this.materials.Length; ++val1)
            {
              this.instance.subMeshIndex = Math.Min(val1, this.instance.mesh.get_subMeshCount() - 1);
              this.objects = (ArrayList) this.materialToMesh[(object) this.materials[val1]];
              if (this.objects != null)
              {
                this.objects.Add((object) this.instance);
              }
              else
              {
                this.objects = new ArrayList();
                this.objects.Add((object) this.instance);
                this.materialToMesh.Add((object) this.materials[val1], (object) this.objects);
              }
            }
            Object.DestroyImmediate((Object) ((Component) this.currentRenderer).get_gameObject());
          }
        }
        foreach (DictionaryEntry dictionaryEntry in this.materialToMesh)
        {
          this.elements = (ArrayList) dictionaryEntry.Value;
          this.instances = (CombineUtility.MeshInstance[]) this.elements.ToArray(typeof (CombineUtility.MeshInstance));
          GameObject gameObject = new GameObject("Combined mesh");
          gameObject.get_transform().set_parent(((Component) this).get_transform());
          gameObject.get_transform().set_localScale(Vector3.get_one());
          gameObject.get_transform().set_localRotation(Quaternion.get_identity());
          gameObject.get_transform().set_localPosition(Vector3.get_zero());
          gameObject.AddComponent<MeshFilter>();
          gameObject.AddComponent<MeshRenderer>();
          gameObject.AddComponent<SaveCombinedMesh>();
          ((Renderer) gameObject.GetComponent<Renderer>()).set_material((Material) dictionaryEntry.Key);
          gameObject.set_isStatic(true);
          this.currentMeshFilter = (MeshFilter) gameObject.GetComponent<MeshFilter>();
          this.currentMeshFilter.set_mesh(CombineUtility.Combine(this.instances, false));
        }
        ((Component) this).get_gameObject().set_isStatic(true);
      }
    }
  }
}
