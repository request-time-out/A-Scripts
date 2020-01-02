// Decompiled with JetBrains decompiler
// Type: CharaCustom.BackgroundCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UnityEngine;

namespace CharaCustom
{
  public class BackgroundCtrl : MonoBehaviour
  {
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Camera backCam;
    [SerializeField]
    private int type;
    private FolderAssist dirBG;
    private string lastBGName;
    private bool m_IsVisible;
    private bool initialize;

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

    public bool ChangeBGImage(byte changeNo, bool listUpdate = true)
    {
      if (listUpdate)
      {
        string str = "bg";
        string[] searchPattern = new string[1]{ "*.png" };
        this.dirBG.lstFile.Clear();
        this.dirBG.CreateFolderInfoEx(DefaultData.Path + str, searchPattern, true);
        this.dirBG.CreateFolderInfoEx(UserData.Path + str, searchPattern, false);
        this.dirBG.SortName(true);
      }
      int fileCount = this.dirBG.GetFileCount();
      if (fileCount == 0)
        return false;
      int index = this.dirBG.GetIndexFromFileName(this.lastBGName);
      if (index == -1)
      {
        index = 0;
      }
      else
      {
        switch (changeNo)
        {
          case 0:
            index = (index + 1) % fileCount;
            break;
          case 1:
            index = (index + fileCount - 1) % fileCount;
            break;
        }
      }
      Texture texture1 = (Texture) PngAssist.LoadTexture(this.dirBG.lstFile[index].FullPath);
      if (Object.op_Implicit((Object) this.meshRenderer) && Object.op_Implicit((Object) ((Renderer) this.meshRenderer).get_material()))
      {
        Texture texture2 = ((Renderer) this.meshRenderer).get_material().GetTexture(ChaShader.MainTex);
        if (Object.op_Implicit((Object) texture2))
          Object.Destroy((Object) texture2);
        ((Renderer) this.meshRenderer).get_material().SetTexture(ChaShader.MainTex, texture1);
      }
      this.lastBGName = this.dirBG.lstFile[index].FileName;
      return true;
    }

    private void Reflect()
    {
      if (Object.op_Equality((Object) null, (Object) this.backCam))
        return;
      Vector3[] vertices = this.meshFilter.get_mesh().get_vertices();
      float num1 = this.backCam.get_fieldOfView() / 2f;
      float num2 = Mathf.Atan(Mathf.Tan((float) Math.PI / 180f * num1) * this.backCam.get_aspect()) * 57.29578f;
      Plane _plane;
      ((Plane) ref _plane).\u002Ector(Vector3.get_back(), this.backCam.get_farClipPlane() - 2f);
      Vector3 vector3_1 = this.Raycast(_plane, Vector3.get_forward());
      Vector3 vector3_2 = this.Raycast(_plane, Quaternion.op_Multiply(Quaternion.AngleAxis(num2, Vector3.get_up()), Vector3.get_forward()));
      Vector3 vector3_3 = this.Raycast(_plane, Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_right()), Vector3.get_forward()));
      if (this.type == 0)
      {
        vertices[0] = new Vector3((float) vector3_2.x, (float) -vector3_3.y, (float) vector3_1.z);
        vertices[1] = new Vector3((float) -vector3_2.x, (float) vector3_3.y, (float) vector3_1.z);
        vertices[2] = new Vector3((float) -vector3_2.x, (float) -vector3_3.y, (float) vector3_1.z);
        vertices[3] = new Vector3((float) vector3_2.x, (float) vector3_3.y, (float) vector3_1.z);
      }
      else
      {
        float num3 = 0.39375f;
        float num4 = 0.97777f;
        vertices[0] = new Vector3((float) vector3_2.x * num3, (float) -vector3_3.y * num4, (float) (vector3_1.z - 0.100000001490116));
        vertices[1] = new Vector3((float) -vector3_2.x * num3, (float) vector3_3.y * num4, (float) (vector3_1.z - 0.100000001490116));
        vertices[2] = new Vector3((float) -vector3_2.x * num3, (float) -vector3_3.y * num4, (float) (vector3_1.z - 0.100000001490116));
        vertices[3] = new Vector3((float) vector3_2.x * num3, (float) vector3_3.y * num4, (float) (vector3_1.z - 0.100000001490116));
      }
      this.meshFilter.get_mesh().set_vertices(vertices);
      this.meshFilter.get_mesh().RecalculateBounds();
    }

    private Vector3 Raycast(Plane _plane, Vector3 _dir)
    {
      float num = 0.0f;
      ((Plane) ref _plane).Raycast(new Ray(Vector3.get_zero(), _dir), ref num);
      return Vector3.op_Multiply(_dir, num);
    }

    private void Start()
    {
      this.isVisible = true;
    }

    private void LateUpdate()
    {
      if (!this.isVisible || !this.initialize)
        return;
      this.Reflect();
      if (this.type == 0)
        this.ChangeBGImage((byte) 0, true);
      this.initialize = false;
    }
  }
}
