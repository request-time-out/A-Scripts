// Decompiled with JetBrains decompiler
// Type: CTS.CTSMeshBlender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CTS
{
  [ExecuteInEditMode]
  [Serializable]
  public class CTSMeshBlender : MonoBehaviour
  {
    public float m_textureBlendOffset;
    public float m_textureBlendStart;
    public float m_textureBlendHeight;
    public float m_normalBlendOffset;
    public float m_normalBlendStart;
    public float m_normalBlendHeight;
    public float m_specular;
    public float m_smoothness;
    public bool m_useAO;
    public bool m_useAOTexture;
    public float m_geoMapClosePower;
    public float m_geoTilingClose;
    public float m_geoMapOffsetClose;
    public Texture2D m_geoMap;
    public Material m_sharedMaterial;
    public List<CTSMeshBlender.TextureData> m_textureList;
    private MaterialPropertyBlock m_materialProperties;
    [SerializeField]
    private MeshRenderer[] m_renderers;
    [SerializeField]
    private MeshFilter[] m_filters;
    [SerializeField]
    private Mesh[] m_originalMeshes;
    private static bool _ShadersIDsAreInitialized;
    private static int _Use_AO;
    private static int _Use_AO_Texture;
    private static int _Terrain_Specular;
    private static int _Terrain_Smoothness;
    private static int _Texture_Geological_Map;
    private static int _Geological_Tiling_Close;
    private static int _Geological_Map_Offset_Close;
    private static int _Geological_Map_Close_Power;
    private static int _Texture_Albedo_Sm_1;
    private static int _Texture_Color_1;
    private static int _Texture_Tiling_1;
    private static int _Texture_Normal_1;
    private static int _Texture_1_Normal_Power;
    private static int _Texture_GHeightAAO_1;
    private static int _Texture_1_AO_Power;
    private static int _Texture_1_Geological_Power;
    private static int _Texture_1_Height_Contrast;
    private static int _Texture_1_Heightmap_Depth;
    private static int _Texture_1_Heightblend_Close;
    private static int _Texture_Albedo_Sm_2;
    private static int _Texture_Color_2;
    private static int _Texture_Tiling_2;
    private static int _Texture_Normal_2;
    private static int _Texture_2_Normal_Power;
    private static int _Texture_GHeightAAO_2;
    private static int _Texture_2_AO_Power;
    private static int _Texture_2_Geological_Power;
    private static int _Texture_2_Height_Contrast;
    private static int _Texture_2_Heightmap_Depth;
    private static int _Texture_2_Heightblend_Close;
    private static int _Texture_Albedo_Sm_3;
    private static int _Texture_Color_3;
    private static int _Texture_Tiling_3;
    private static int _Texture_Normal_3;
    private static int _Texture_3_Normal_Power;
    private static int _Texture_GHeightAAO_3;
    private static int _Texture_3_AO_Power;
    private static int _Texture_3_Geological_Power;
    private static int _Texture_3_Height_Contrast;
    private static int _Texture_3_Heightmap_Depth;
    private static int _Texture_3_Heightblend_Close;

    public CTSMeshBlender()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
    }

    public void ClearBlend()
    {
      if (this.m_filters != null)
      {
        for (int index = 0; index < this.m_filters.Length; ++index)
          this.m_filters[index].set_sharedMesh(this.m_originalMeshes[index]);
      }
      if (this.m_renderers != null)
      {
        for (int index1 = 0; index1 < this.m_renderers.Length; ++index1)
        {
          MeshRenderer renderer = this.m_renderers[index1];
          List<Material> materialList = new List<Material>((IEnumerable<Material>) ((Renderer) renderer).get_sharedMaterials());
          int index2 = 0;
          while (index2 < materialList.Count)
          {
            Material material = materialList[index2];
            if (Object.op_Equality((Object) material, (Object) null) || ((Object) material).get_name() == "CTS Model Blend Shader")
            {
              materialList.RemoveAt(index2);
              this.m_sharedMaterial = material;
            }
            else
              ++index2;
          }
          ((Renderer) renderer).set_sharedMaterials(materialList.ToArray());
        }
      }
      if (Object.op_Inequality((Object) this.m_sharedMaterial, (Object) null))
      {
        Object.DestroyImmediate((Object) this.m_sharedMaterial);
        this.m_sharedMaterial = (Material) null;
      }
      this.m_renderers = (MeshRenderer[]) null;
      this.m_filters = (MeshFilter[]) null;
      this.m_originalMeshes = (Mesh[]) null;
      this.m_textureList.Clear();
    }

    public void CreateBlend()
    {
      this.ClearBlend();
      this.m_renderers = (MeshRenderer[]) ((Component) this).get_gameObject().GetComponentsInChildren<MeshRenderer>();
      this.m_filters = (MeshFilter[]) ((Component) this).get_gameObject().GetComponentsInChildren<MeshFilter>();
      this.m_originalMeshes = new Mesh[this.m_filters.Length];
      for (int index = 0; index < this.m_filters.Length; ++index)
      {
        if (Object.op_Equality((Object) this.m_filters[index].get_sharedMesh(), (Object) null))
        {
          this.m_originalMeshes[index] = (Mesh) null;
        }
        else
        {
          this.m_originalMeshes[index] = this.m_filters[index].get_sharedMesh();
          this.m_filters[index].set_sharedMesh((Mesh) Object.Instantiate<Mesh>((M0) this.m_originalMeshes[index]));
        }
      }
      this.GetTexturesAndSettingsAtCurrentLocation();
      Vector3 position = ((Component) this).get_transform().get_position();
      Vector3 localScale = ((Component) this).get_transform().get_localScale();
      Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
      for (int index1 = 0; index1 < this.m_filters.Length; ++index1)
      {
        Mesh sharedMesh = this.m_filters[index1].get_sharedMesh();
        if (Object.op_Inequality((Object) sharedMesh, (Object) null))
        {
          int length = sharedMesh.get_vertices().Length;
          Vector3[] vertices = sharedMesh.get_vertices();
          Vector3[] normals = sharedMesh.get_normals();
          Color[] colorArray = sharedMesh.get_colors();
          if (colorArray.Length == 0)
            colorArray = new Color[length];
          for (int index2 = 0; index2 < length; ++index2)
          {
            Vector3 locationWU = Vector3.op_Addition(position, Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), Vector3.Scale(vertices[index2], localScale)));
            Terrain terrain = this.GetTerrain(locationWU);
            if (Object.op_Inequality((Object) terrain, (Object) null))
            {
              Vector3 localPosition = this.GetLocalPosition(terrain, locationWU);
              float num1 = terrain.SampleHeight(locationWU);
              float num2 = num1 + this.m_textureBlendOffset;
              float num3 = num2 + this.m_textureBlendStart;
              float num4 = num3 + this.m_textureBlendHeight;
              float num5 = num1 + this.m_normalBlendOffset;
              float num6 = num5 + this.m_normalBlendStart;
              float num7 = num6 + this.m_normalBlendHeight;
              if (locationWU.y < (double) num2)
                colorArray[index2].a = (__Null) 0.0;
              else if (locationWU.y <= (double) num4)
              {
                Color color = (Color) null;
                float num8 = 1f;
                if (locationWU.y > (double) num3)
                  num8 = Mathf.Lerp(1f, 0.0f, ((float) locationWU.y - num3) / this.m_textureBlendHeight);
                color.a = (__Null) (double) num8;
                float[,,] texturesAtLocation = this.GetTexturesAtLocation(terrain, localPosition);
                if (this.m_textureList.Count >= 1)
                  color.r = (__Null) (double) texturesAtLocation[0, 0, this.m_textureList[0].m_terrainIdx];
                if (this.m_textureList.Count >= 2)
                  color.g = (__Null) (double) texturesAtLocation[0, 0, this.m_textureList[1].m_terrainIdx];
                if (this.m_textureList.Count >= 3)
                  color.b = (__Null) (double) texturesAtLocation[0, 0, this.m_textureList[2].m_terrainIdx];
                colorArray[index2] = color;
              }
              else
                colorArray[index2].a = (__Null) 0.0;
              if (locationWU.y >= (double) num5)
              {
                if (locationWU.y < (double) num6)
                  normals[index2] = this.GetNormalsAtLocation(terrain, localPosition);
                else if (locationWU.y <= (double) num7)
                  normals[index2] = Vector3.Lerp(this.GetNormalsAtLocation(terrain, localPosition), normals[index2], (num7 - (float) locationWU.y) / this.m_normalBlendHeight);
              }
            }
            else
              colorArray[index2].a = (__Null) 0.0;
          }
          sharedMesh.set_colors(colorArray);
          sharedMesh.set_normals(normals);
        }
      }
      this.InitializeMaterials();
      this.UpdateShader();
    }

    public Vector3 GetNearestVertice(Vector3 sourcePosition, GameObject targetObject)
    {
      float num1 = float.MaxValue;
      Vector3 vector3_1 = targetObject.get_transform().get_position();
      Vector3 vector3_2 = vector3_1;
      Vector3 localScale = targetObject.get_transform().get_localScale();
      Vector3 eulerAngles = targetObject.get_transform().get_eulerAngles();
      foreach (MeshFilter componentsInChild in (MeshFilter[]) targetObject.GetComponentsInChildren<MeshFilter>())
      {
        Mesh sharedMesh = componentsInChild.get_sharedMesh();
        if (Object.op_Inequality((Object) sharedMesh, (Object) null))
        {
          int length = sharedMesh.get_vertices().Length;
          Vector3[] vertices = sharedMesh.get_vertices();
          for (int index = 0; index < length; ++index)
          {
            Vector3 vector3_3 = Vector3.op_Addition(vector3_2, Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), Vector3.Scale(vertices[index], localScale)));
            float num2 = Vector3.Distance(sourcePosition, vector3_3);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              vector3_1 = vector3_3;
            }
          }
        }
      }
      return vector3_1;
    }

    private void InitializeShaderConstants()
    {
      if (CTSMeshBlender._ShadersIDsAreInitialized)
        return;
      Debug.Log((object) "Initialising shader IDs");
      CTSMeshBlender._ShadersIDsAreInitialized = true;
      CTSMeshBlender._Use_AO = Shader.PropertyToID("_Use_AO");
      CTSMeshBlender._Use_AO_Texture = Shader.PropertyToID("_Use_AO_Texture");
      CTSMeshBlender._Terrain_Specular = Shader.PropertyToID("_Terrain_Specular");
      CTSMeshBlender._Terrain_Smoothness = Shader.PropertyToID("_Terrain_Smoothness");
      CTSMeshBlender._Texture_Geological_Map = Shader.PropertyToID("_Texture_Geological_Map");
      CTSMeshBlender._Geological_Tiling_Close = Shader.PropertyToID("_Geological_Tiling_Close");
      CTSMeshBlender._Geological_Map_Offset_Close = Shader.PropertyToID("_Geological_Map_Offset_Close");
      CTSMeshBlender._Geological_Map_Close_Power = Shader.PropertyToID("_Geological_Map_Close_Power");
      CTSMeshBlender._Texture_Albedo_Sm_1 = Shader.PropertyToID("_Texture_Albedo_Sm_1");
      CTSMeshBlender._Texture_Color_1 = Shader.PropertyToID("_Texture_Color_1");
      CTSMeshBlender._Texture_Tiling_1 = Shader.PropertyToID("_Texture_Tiling_1");
      CTSMeshBlender._Texture_Normal_1 = Shader.PropertyToID("_Texture_Normal_1");
      CTSMeshBlender._Texture_1_Normal_Power = Shader.PropertyToID("_Texture_1_Normal_Power");
      CTSMeshBlender._Texture_GHeightAAO_1 = Shader.PropertyToID("_Texture_GHeightAAO_1");
      CTSMeshBlender._Texture_1_AO_Power = Shader.PropertyToID("_Texture_1_AO_Power");
      CTSMeshBlender._Texture_1_Geological_Power = Shader.PropertyToID("_Texture_1_Geological_Power");
      CTSMeshBlender._Texture_1_Height_Contrast = Shader.PropertyToID("_Texture_1_Height_Contrast");
      CTSMeshBlender._Texture_1_Heightmap_Depth = Shader.PropertyToID("_Texture_1_Heightmap_Depth");
      CTSMeshBlender._Texture_1_Heightblend_Close = Shader.PropertyToID("_Texture_1_Heightblend_Close");
      CTSMeshBlender._Texture_Albedo_Sm_2 = Shader.PropertyToID("_Texture_Albedo_Sm_2");
      CTSMeshBlender._Texture_Color_2 = Shader.PropertyToID("_Texture_Color_2");
      CTSMeshBlender._Texture_Tiling_2 = Shader.PropertyToID("_Texture_Tiling_2");
      CTSMeshBlender._Texture_Normal_2 = Shader.PropertyToID("_Texture_Normal_2");
      CTSMeshBlender._Texture_2_Normal_Power = Shader.PropertyToID("_Texture_2_Normal_Power");
      CTSMeshBlender._Texture_GHeightAAO_2 = Shader.PropertyToID("_Texture_GHeightAAO_2");
      CTSMeshBlender._Texture_2_AO_Power = Shader.PropertyToID("_Texture_2_AO_Power");
      CTSMeshBlender._Texture_2_Geological_Power = Shader.PropertyToID("_Texture_2_Geological_Power");
      CTSMeshBlender._Texture_2_Height_Contrast = Shader.PropertyToID("_Texture_2_Height_Contrast");
      CTSMeshBlender._Texture_2_Heightmap_Depth = Shader.PropertyToID("_Texture_2_Heightmap_Depth");
      CTSMeshBlender._Texture_2_Heightblend_Close = Shader.PropertyToID("_Texture_2_Heightblend_Close");
      CTSMeshBlender._Texture_Albedo_Sm_3 = Shader.PropertyToID("_Texture_Albedo_Sm_3");
      CTSMeshBlender._Texture_Color_3 = Shader.PropertyToID("_Texture_Color_3");
      CTSMeshBlender._Texture_Tiling_3 = Shader.PropertyToID("_Texture_Tiling_3");
      CTSMeshBlender._Texture_Normal_3 = Shader.PropertyToID("_Texture_Normal_3");
      CTSMeshBlender._Texture_3_Normal_Power = Shader.PropertyToID("_Texture_3_Normal_Power");
      CTSMeshBlender._Texture_GHeightAAO_3 = Shader.PropertyToID("_Texture_GHeightAAO_3");
      CTSMeshBlender._Texture_3_AO_Power = Shader.PropertyToID("_Texture_3_AO_Power");
      CTSMeshBlender._Texture_3_Geological_Power = Shader.PropertyToID("_Texture_3_Geological_Power");
      CTSMeshBlender._Texture_3_Height_Contrast = Shader.PropertyToID("_Texture_3_Height_Contrast");
      CTSMeshBlender._Texture_3_Heightmap_Depth = Shader.PropertyToID("_Texture_3_Heightmap_Depth");
      CTSMeshBlender._Texture_3_Heightblend_Close = Shader.PropertyToID("_Texture_3_Heightblend_Close");
    }

    private void InitializeMaterials()
    {
      for (int index1 = 0; index1 < this.m_renderers.Length; ++index1)
      {
        MeshRenderer renderer = this.m_renderers[index1];
        if (Object.op_Inequality((Object) renderer, (Object) null))
        {
          bool flag = false;
          List<Material> materialList = new List<Material>((IEnumerable<Material>) ((Renderer) this.m_renderers[index1]).get_sharedMaterials());
          for (int index2 = 0; index2 < materialList.Count; ++index2)
          {
            if (((Object) materialList[index2]).get_name() == "CTS Model Blend Shader")
            {
              flag = true;
              this.m_sharedMaterial = materialList[index2];
              break;
            }
          }
          if (!flag)
          {
            if (Object.op_Equality((Object) this.m_sharedMaterial, (Object) null))
            {
              Material material = new Material(Shader.Find("CTS/CTS_Model_Blend"));
              ((Object) material).set_name("CTS Model Blend Shader");
              this.m_sharedMaterial = material;
            }
            materialList.Add(this.m_sharedMaterial);
            ((Renderer) renderer).set_sharedMaterials(materialList.ToArray());
          }
        }
        else
          Debug.LogWarning((object) "Got nulll renderer!");
      }
    }

    private void UpdateShader()
    {
      this.InitializeShaderConstants();
      if (Object.op_Equality((Object) this.m_sharedMaterial, (Object) null))
        Debug.LogWarning((object) "CTS Blender Missing Material. Exiting without updating.");
      else if (this.m_renderers == null || this.m_renderers.Length == 0)
        Debug.LogWarning((object) "CTS Blender Missing Renderer. Exiting without updating.");
      else if (this.m_textureList.Count == 0)
      {
        Debug.LogWarning((object) "CTS Blender has no textures. Exiting without updating.");
      }
      else
      {
        if (this.m_materialProperties == null)
          this.m_materialProperties = new MaterialPropertyBlock();
        for (int index = 0; index < this.m_renderers.Length; ++index)
        {
          this.m_sharedMaterial.SetInt(CTSMeshBlender._Use_AO, !this.m_useAO ? 0 : 1);
          this.m_sharedMaterial.SetInt(CTSMeshBlender._Use_AO_Texture, !this.m_useAOTexture ? 0 : 1);
          this.m_sharedMaterial.SetFloat(CTSMeshBlender._Terrain_Specular, this.m_specular);
          this.m_sharedMaterial.SetFloat(CTSMeshBlender._Terrain_Smoothness, this.m_smoothness);
          this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Geological_Map, (Texture) this.m_geoMap);
          this.m_sharedMaterial.SetFloat(CTSMeshBlender._Geological_Tiling_Close, this.m_geoTilingClose);
          this.m_sharedMaterial.SetFloat(CTSMeshBlender._Geological_Map_Offset_Close, this.m_geoMapOffsetClose);
          this.m_sharedMaterial.SetFloat(CTSMeshBlender._Geological_Map_Close_Power, this.m_geoMapClosePower);
          if (this.m_textureList.Count >= 1)
          {
            CTSMeshBlender.TextureData texture = this.m_textureList[0];
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Albedo_Sm_1, (Texture) texture.m_albedo);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Normal_1, (Texture) texture.m_normal);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_GHeightAAO_1, (Texture) texture.m_hao_in_GA);
            this.m_sharedMaterial.SetVector(CTSMeshBlender._Texture_Color_1, texture.m_color);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_Tiling_1, texture.m_tiling);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_Normal_Power, texture.m_normalPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_AO_Power, texture.m_aoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_Geological_Power, texture.m_geoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_Height_Contrast, texture.m_heightContrast);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_Heightmap_Depth, texture.m_heightDepth);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_1_Heightblend_Close, texture.m_heightBlendClose);
          }
          if (this.m_textureList.Count >= 2)
          {
            CTSMeshBlender.TextureData texture = this.m_textureList[1];
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Albedo_Sm_2, (Texture) texture.m_albedo);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Normal_2, (Texture) texture.m_normal);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_GHeightAAO_2, (Texture) texture.m_hao_in_GA);
            this.m_sharedMaterial.SetVector(CTSMeshBlender._Texture_Color_2, texture.m_color);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_Tiling_2, texture.m_tiling);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_Normal_Power, texture.m_normalPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_AO_Power, texture.m_aoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_Geological_Power, texture.m_geoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_Height_Contrast, texture.m_heightContrast);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_Heightmap_Depth, texture.m_heightDepth);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_2_Heightblend_Close, texture.m_heightBlendClose);
          }
          if (this.m_textureList.Count >= 3)
          {
            CTSMeshBlender.TextureData texture = this.m_textureList[2];
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Albedo_Sm_3, (Texture) texture.m_albedo);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_Normal_3, (Texture) texture.m_normal);
            this.m_sharedMaterial.SetTexture(CTSMeshBlender._Texture_GHeightAAO_3, (Texture) texture.m_hao_in_GA);
            this.m_sharedMaterial.SetVector(CTSMeshBlender._Texture_Color_3, texture.m_color);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_Tiling_3, texture.m_tiling);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_Normal_Power, texture.m_normalPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_AO_Power, texture.m_aoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_Geological_Power, texture.m_geoPower);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_Height_Contrast, texture.m_heightContrast);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_Heightmap_Depth, texture.m_heightDepth);
            this.m_sharedMaterial.SetFloat(CTSMeshBlender._Texture_3_Heightblend_Close, texture.m_heightBlendClose);
          }
        }
      }
    }

    private void GetTexturesAndSettingsAtCurrentLocation()
    {
      this.m_textureList.Clear();
      CTSProfile ctsProfile = (CTSProfile) null;
      SplatPrototype[] splatPrototypeArray = new SplatPrototype[0];
      Vector3 position = ((Component) this).get_transform().get_position();
      Vector3 localScale = ((Component) this).get_transform().get_localScale();
      Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
      for (int index1 = this.m_filters.Length - 1; index1 >= 0; --index1)
      {
        Mesh sharedMesh = this.m_filters[index1].get_sharedMesh();
        if (Object.op_Inequality((Object) sharedMesh, (Object) null))
        {
          Vector3[] vertices = sharedMesh.get_vertices();
          for (int index2 = vertices.Length - 1; index2 >= 0; --index2)
          {
            Vector3 locationWU = Vector3.op_Addition(position, Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), Vector3.Scale(vertices[index2], localScale)));
            Terrain terrain = this.GetTerrain(locationWU);
            if (Object.op_Inequality((Object) terrain, (Object) null))
            {
              if (Object.op_Equality((Object) ctsProfile, (Object) null))
              {
                CompleteTerrainShader component = (CompleteTerrainShader) ((Component) terrain).get_gameObject().GetComponent<CompleteTerrainShader>();
                if (Object.op_Inequality((Object) component, (Object) null))
                  ctsProfile = component.Profile;
              }
              if (splatPrototypeArray.Length == 0)
                splatPrototypeArray = terrain.get_terrainData().get_splatPrototypes();
              Vector3 localPosition = this.GetLocalPosition(terrain, locationWU);
              float[,,] texturesAtLocation = this.GetTexturesAtLocation(terrain, localPosition);
              for (int index3 = 0; index3 < texturesAtLocation.GetLength(2); ++index3)
              {
                if (index3 == this.m_textureList.Count)
                  this.m_textureList.Add(new CTSMeshBlender.TextureData()
                  {
                    m_terrainIdx = index3,
                    m_terrainTextureStrength = texturesAtLocation[0, 0, index3]
                  });
                else
                  this.m_textureList[index3].m_terrainTextureStrength += texturesAtLocation[0, 0, index3];
              }
            }
          }
        }
      }
      List<CTSMeshBlender.TextureData> list = this.m_textureList.OrderByDescending<CTSMeshBlender.TextureData, float>((Func<CTSMeshBlender.TextureData, float>) (x => x.m_terrainTextureStrength)).ToList<CTSMeshBlender.TextureData>();
      while (list.Count > 3)
        list.RemoveAt(list.Count - 1);
      this.m_textureList = list.OrderBy<CTSMeshBlender.TextureData, int>((Func<CTSMeshBlender.TextureData, int>) (x => x.m_terrainIdx)).ToList<CTSMeshBlender.TextureData>();
      if (Object.op_Inequality((Object) ctsProfile, (Object) null))
      {
        this.m_geoMap = ctsProfile.GeoAlbedo;
        this.m_geoMapClosePower = ctsProfile.m_geoMapClosePower;
        this.m_geoMapOffsetClose = ctsProfile.m_geoMapCloseOffset;
        this.m_geoTilingClose = ctsProfile.m_geoMapTilingClose;
        this.m_smoothness = ctsProfile.m_globalTerrainSmoothness;
        this.m_specular = ctsProfile.m_globalTerrainSpecular;
        switch (ctsProfile.m_globalAOType)
        {
          case CTSConstants.AOType.None:
            this.m_useAO = false;
            this.m_useAOTexture = false;
            break;
          case CTSConstants.AOType.NormalMapBased:
            this.m_useAO = true;
            this.m_useAOTexture = false;
            break;
          case CTSConstants.AOType.TextureBased:
            this.m_useAO = true;
            this.m_useAOTexture = true;
            break;
        }
      }
      else
      {
        this.m_geoMap = (Texture2D) null;
        this.m_geoMapClosePower = 0.0f;
        this.m_geoMapOffsetClose = 0.0f;
        this.m_geoTilingClose = 0.0f;
        this.m_smoothness = 1f;
        this.m_specular = 1f;
        this.m_useAO = true;
        this.m_useAOTexture = false;
      }
      byte minHeight = 0;
      byte maxHeight = 0;
      for (int index = 0; index < this.m_textureList.Count; ++index)
      {
        CTSMeshBlender.TextureData texture = this.m_textureList[index];
        if (Object.op_Inequality((Object) ctsProfile, (Object) null) && texture.m_terrainIdx < ctsProfile.TerrainTextures.Count)
        {
          CTSTerrainTextureDetails terrainTexture = ctsProfile.TerrainTextures[texture.m_terrainIdx];
          texture.m_albedo = terrainTexture.Albedo;
          texture.m_normal = terrainTexture.Normal;
          texture.m_hao_in_GA = ctsProfile.BakeHAOTexture(((Object) terrainTexture.Albedo).get_name(), terrainTexture.Height, terrainTexture.AmbientOcclusion, out minHeight, out maxHeight);
          texture.m_aoPower = terrainTexture.m_aoPower;
          texture.m_color = new Vector4((float) terrainTexture.m_tint.r * terrainTexture.m_tintBrightness, (float) terrainTexture.m_tint.g * terrainTexture.m_tintBrightness, (float) terrainTexture.m_tint.b * terrainTexture.m_tintBrightness, terrainTexture.m_smoothness);
          texture.m_geoPower = terrainTexture.m_geologicalPower;
          texture.m_normalPower = terrainTexture.m_normalStrength;
          texture.m_tiling = terrainTexture.m_albedoTilingClose;
          texture.m_heightContrast = terrainTexture.m_heightContrast;
          texture.m_heightDepth = terrainTexture.m_heightDepth;
          texture.m_heightBlendClose = terrainTexture.m_heightBlendClose;
        }
        else if (texture.m_terrainIdx < splatPrototypeArray.Length)
        {
          SplatPrototype splatPrototype = splatPrototypeArray[texture.m_terrainIdx];
          texture.m_albedo = splatPrototype.get_texture();
          texture.m_normal = splatPrototype.get_normalMap();
          texture.m_hao_in_GA = (Texture2D) null;
          texture.m_aoPower = 0.0f;
          texture.m_color = Vector4.get_one();
          texture.m_geoPower = 0.0f;
          texture.m_normalPower = 1f;
          texture.m_tiling = (float) splatPrototype.get_tileSize().x;
          texture.m_heightContrast = 1f;
          texture.m_heightDepth = 1f;
          texture.m_heightBlendClose = 1f;
        }
      }
    }

    private Terrain GetTerrain(Vector3 locationWU)
    {
      Terrain activeTerrain1 = Terrain.get_activeTerrain();
      if (Object.op_Inequality((Object) activeTerrain1, (Object) null))
      {
        Vector3 position = activeTerrain1.GetPosition();
        Vector3 vector3 = Vector3.op_Addition(position, activeTerrain1.get_terrainData().get_size());
        if (locationWU.x >= position.x && locationWU.x <= vector3.x && (locationWU.z >= position.z && locationWU.z <= vector3.z))
          return activeTerrain1;
      }
      for (int index = 0; index < Terrain.get_activeTerrains().Length; ++index)
      {
        Terrain activeTerrain2 = Terrain.get_activeTerrains()[index];
        Vector3 position = activeTerrain2.GetPosition();
        Vector3 vector3 = Vector3.op_Addition(position, activeTerrain2.get_terrainData().get_size());
        if (locationWU.x >= position.x && locationWU.x <= vector3.x && (locationWU.z >= position.z && locationWU.z <= vector3.z))
          return activeTerrain2;
      }
      return (Terrain) null;
    }

    private Vector3 GetLocalPosition(Terrain terrain, Vector3 locationWU)
    {
      Vector3 vector3 = ((Component) terrain).get_transform().InverseTransformPoint(locationWU);
      return new Vector3(Mathf.InverseLerp(0.0f, (float) terrain.get_terrainData().get_size().x, (float) vector3.x), Mathf.InverseLerp(0.0f, (float) terrain.get_terrainData().get_size().y, (float) vector3.y), Mathf.InverseLerp(0.0f, (float) terrain.get_terrainData().get_size().z, (float) vector3.z));
    }

    private float[,,] GetTexturesAtLocation(Terrain terrain, Vector3 locationTU)
    {
      return terrain.get_terrainData().GetAlphamaps((int) (locationTU.x * (double) (terrain.get_terrainData().get_alphamapWidth() - 1)), (int) (locationTU.z * (double) (terrain.get_terrainData().get_alphamapHeight() - 1)), 1, 1);
    }

    private Vector3 GetNormalsAtLocation(Terrain terrain, Vector3 locationTU)
    {
      return terrain.get_terrainData().GetInterpolatedNormal((float) locationTU.x, (float) locationTU.z);
    }

    [Serializable]
    public class TextureData
    {
      public float m_heightContrast = 1f;
      public float m_heightDepth = 1f;
      public float m_heightBlendClose = 1f;
      public int m_terrainIdx;
      public float m_terrainTextureStrength;
      public Texture2D m_albedo;
      public Texture2D m_normal;
      public Texture2D m_hao_in_GA;
      public float m_tiling;
      public Vector4 m_color;
      public float m_normalPower;
      public float m_aoPower;
      public float m_geoPower;
    }
  }
}
