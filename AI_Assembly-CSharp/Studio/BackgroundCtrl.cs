// Decompiled with JetBrains decompiler
// Type: Studio.BackgroundCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class BackgroundCtrl : MonoBehaviour
  {
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    [Range(0.01f, 1f)]
    private float farRate;
    private bool m_IsVisible;
    private Camera m_Camera;
    private float oldFOV;

    public BackgroundCtrl()
    {
      base.\u002Ector();
    }

    public bool isVisible
    {
      get
      {
        return this.m_IsVisible;
      }
      set
      {
        this.m_IsVisible = value;
        ((Renderer) this.meshRenderer).set_enabled(value);
      }
    }

    private Camera mainCamera
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Camera, (Object) null))
          this.m_Camera = Camera.get_main();
        return this.m_Camera;
      }
    }

    public bool Load(string _file)
    {
      string str = UserData.Path + BackgroundList.dirName + "/" + _file;
      if (!File.Exists(str))
      {
        this.isVisible = false;
        Singleton<Studio.Studio>.Instance.sceneInfo.background = string.Empty;
        return false;
      }
      Texture texture = (Texture) PngAssist.LoadTexture(str);
      if (Object.op_Equality((Object) texture, (Object) null))
      {
        this.isVisible = false;
        return false;
      }
      Material material = ((Renderer) this.meshRenderer).get_material();
      material.SetTexture("_MainTex", texture);
      ((Renderer) this.meshRenderer).set_material(material);
      this.isVisible = true;
      Singleton<Studio.Studio>.Instance.sceneInfo.background = _file;
      Resources.UnloadUnusedAssets();
      GC.Collect();
      return true;
    }

    private void Reflect()
    {
      Vector3[] vertices = this.meshFilter.get_mesh().get_vertices();
      float num1 = this.mainCamera.get_fieldOfView() / 2f;
      float num2 = Mathf.Atan(Mathf.Tan((float) Math.PI / 180f * num1) * this.mainCamera.get_aspect()) * 57.29578f;
      Plane _plane;
      ((Plane) ref _plane).\u002Ector(Vector3.get_back(), this.mainCamera.get_farClipPlane() * this.farRate);
      Vector3 vector3_1 = this.Raycast(_plane, Vector3.get_forward());
      Vector3 vector3_2 = this.Raycast(_plane, Quaternion.op_Multiply(Quaternion.AngleAxis(num2, Vector3.get_up()), Vector3.get_forward()));
      Vector3 vector3_3 = this.Raycast(_plane, Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_right()), Vector3.get_forward()));
      vertices[0] = new Vector3((float) vector3_2.x, (float) -vector3_3.y, (float) vector3_1.z);
      vertices[1] = new Vector3((float) -vector3_2.x, (float) vector3_3.y, (float) vector3_1.z);
      vertices[2] = new Vector3((float) -vector3_2.x, (float) -vector3_3.y, (float) vector3_1.z);
      vertices[3] = new Vector3((float) vector3_2.x, (float) vector3_3.y, (float) vector3_1.z);
      this.meshFilter.get_mesh().set_vertices(vertices);
      this.meshFilter.get_mesh().RecalculateBounds();
      this.oldFOV = this.mainCamera.get_fieldOfView();
    }

    private Vector3 Raycast(Plane _plane, Vector3 _dir)
    {
      float num = 0.0f;
      ((Plane) ref _plane).Raycast(new Ray(Vector3.get_zero(), _dir), ref num);
      return Vector3.op_Multiply(_dir, num);
    }

    private void Start()
    {
      this.isVisible = false;
    }

    private void LateUpdate()
    {
      if (!this.isVisible || (double) this.oldFOV == (double) this.mainCamera.get_fieldOfView())
        return;
      this.Reflect();
    }
  }
}
