// Decompiled with JetBrains decompiler
// Type: Carsol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Carsol : MonoBehaviour
{
  public VirtualCameraController Vcam;
  public GameObject[] gameObjects;
  public bool bMouseOperate;
  public Canvas cursoldir;
  private Camera Cam;
  private float fGridSize;
  private Vector3 MinPos;
  private CarsolObj[] carsolObjs;
  private Vector3 cursoldirInitPos;
  private int nKind;
  private int nDirection;
  private bool bRayFromMouseHit;
  private bool bMoveLimit;
  private RaycastHit[] HitInfo;
  private List<RaycastHit> HitGrid;

  public Carsol()
  {
    base.\u002Ector();
  }

  public void Init(Vector3 Pos, float gridsize)
  {
    this.Cam = CinemachineCore.get_Instance().GetActiveBrain(0).get_OutputCamera();
    this.carsolObjs = new CarsolObj[10];
    for (int index = 0; index < this.carsolObjs.Length; ++index)
    {
      this.carsolObjs[index].m_Obj = this.gameObjects[index];
      switch (index)
      {
        case 0:
          this.carsolObjs[index].m_CarsolAreaNumX = 1;
          this.carsolObjs[index].m_CarsolAreaNumZ = 1;
          break;
        case 1:
          this.carsolObjs[index].m_CarsolAreaNumX = 1;
          this.carsolObjs[index].m_CarsolAreaNumZ = 2;
          break;
        case 2:
          this.carsolObjs[index].m_CarsolAreaNumX = 2;
          this.carsolObjs[index].m_CarsolAreaNumZ = 1;
          break;
        case 3:
          this.carsolObjs[index].m_CarsolAreaNumX = 1;
          this.carsolObjs[index].m_CarsolAreaNumZ = 5;
          break;
        case 4:
          this.carsolObjs[index].m_CarsolAreaNumX = 2;
          this.carsolObjs[index].m_CarsolAreaNumZ = 2;
          break;
        case 5:
          this.carsolObjs[index].m_CarsolAreaNumX = 4;
          this.carsolObjs[index].m_CarsolAreaNumZ = 4;
          break;
        case 9:
          this.carsolObjs[index].m_CarsolAreaNumX = 2;
          this.carsolObjs[index].m_CarsolAreaNumZ = 3;
          break;
        default:
          this.carsolObjs[index].m_CarsolAreaNumX = this.carsolObjs[index].m_CarsolAreaNumZ = 0;
          break;
      }
    }
    this.fGridSize = gridsize;
    this.nDirection = 0;
    this.bRayFromMouseHit = false;
    this.MinPos = Pos;
    ref Vector3 local1 = ref this.MinPos;
    local1.x = (__Null) (local1.x - (double) this.fGridSize / 2.0);
    ref Vector3 local2 = ref this.MinPos;
    local2.y = (__Null) (local2.y + ((double) this.fGridSize + 1.0));
    ref Vector3 local3 = ref this.MinPos;
    local3.z = (__Null) (local3.z - (double) this.fGridSize / 2.0);
    this.nKind = 0;
    this.bMouseOperate = true;
    this.bMoveLimit = false;
    this.cursoldirInitPos = ((Component) this.cursoldir).get_transform().get_localPosition();
  }

  public void MoveCarsol(int nDirection = -1)
  {
    if (this.Vcam.isControlNow)
      return;
    ((Component) this).get_transform().get_position();
    float num1 = this.fGridSize / 2f;
    float[] numArray = new float[2];
    Vector3 point;
    if (nDirection < 0)
    {
      Vector3 mousePosition = Input.get_mousePosition();
      mousePosition.x = (__Null) (double) Mathf.Clamp((float) mousePosition.x, 0.0f, (float) Screen.get_width());
      mousePosition.y = (__Null) (double) Mathf.Clamp((float) mousePosition.y, 0.0f, (float) Screen.get_height());
      int num2 = Physics.RaycastNonAlloc(this.Cam.ScreenPointToRay(mousePosition), this.HitInfo, 100f);
      for (int index = 0; index < num2; ++index)
      {
        if (Object.op_Inequality((Object) ((Component) ((RaycastHit) ref this.HitInfo[index]).get_collider()).get_gameObject().GetComponent<GridInfo>(), (Object) null))
          this.HitGrid.Add(this.HitInfo[index]);
      }
      this.HitGrid.Sort((Comparison<RaycastHit>) ((a, b) => ((RaycastHit) ref a).get_distance().CompareTo(((RaycastHit) ref b).get_distance())));
      this.bRayFromMouseHit = this.HitGrid.Count > 0;
      if (!this.bRayFromMouseHit)
        return;
      RaycastHit raycastHit = this.HitGrid[0];
      point = ((RaycastHit) ref raycastHit).get_point();
    }
    else
    {
      Physics.RaycastNonAlloc(Vector3.op_Addition(((Component) this).get_transform().get_position(), new Vector3(0.0f, 1f, 0.0f)), Vector3.get_down(), this.HitInfo, 100f);
      for (int index = 0; index < this.HitInfo.Length; ++index)
      {
        if (Object.op_Inequality((Object) ((Component) ((RaycastHit) ref this.HitInfo[index]).get_collider()).get_gameObject().GetComponent<GridInfo>(), (Object) null))
          this.HitGrid.Add(this.HitInfo[index]);
      }
      this.HitGrid.Sort((Comparison<RaycastHit>) ((a, b) => ((RaycastHit) ref a).get_distance().CompareTo(((RaycastHit) ref b).get_distance())));
      RaycastHit raycastHit = this.HitGrid[0];
      point = ((RaycastHit) ref raycastHit).get_point();
      switch (nDirection)
      {
        case 0:
          if (!this.bMoveLimit)
          {
            ref Vector3 local = ref point;
            local.z = (__Null) (local.z + (double) num1);
            break;
          }
          ref Vector3 local1 = ref point;
          local1.z = (__Null) (local1.z + (double) this.fGridSize);
          break;
        case 1:
          if (!this.bMoveLimit)
          {
            ref Vector3 local2 = ref point;
            local2.x = (__Null) (local2.x + (double) num1);
            break;
          }
          ref Vector3 local3 = ref point;
          local3.x = (__Null) (local3.x + (double) this.fGridSize);
          break;
        case 2:
          if (!this.bMoveLimit)
          {
            ref Vector3 local2 = ref point;
            local2.z = (__Null) (local2.z - (double) num1);
            break;
          }
          ref Vector3 local4 = ref point;
          local4.z = (__Null) (local4.z - (double) this.fGridSize);
          break;
        case 3:
          if (!this.bMoveLimit)
          {
            ref Vector3 local2 = ref point;
            local2.x = (__Null) (local2.x - (double) num1);
            break;
          }
          ref Vector3 local5 = ref point;
          local5.x = (__Null) (local5.x - (double) this.fGridSize);
          break;
      }
    }
    Vector3 vector3;
    if (!this.bMoveLimit)
    {
      numArray[0] = numArray[1] = num1;
      vector3 = Vector3.op_Subtraction(point, this.MinPos);
      vector3.x = (__Null) ((double) numArray[0] * (double) Mathf.RoundToInt((float) vector3.x / numArray[0]));
      vector3.z = (__Null) ((double) numArray[1] * (double) Mathf.RoundToInt((float) vector3.z / numArray[1]));
      ref Vector3 local1 = ref vector3;
      local1.x = local1.x + this.MinPos.x;
      ref Vector3 local2 = ref vector3;
      local2.z = local2.z + this.MinPos.z;
    }
    else
    {
      if (this.nDirection == 0 || this.nDirection == 4)
      {
        numArray[0] = num1 * (float) this.carsolObjs[this.nKind].m_CarsolAreaNumX;
        numArray[1] = num1 * (float) this.carsolObjs[this.nKind].m_CarsolAreaNumZ;
      }
      else if (this.nDirection == 2 || this.nDirection == 6)
      {
        numArray[0] = num1 * (float) this.carsolObjs[this.nKind].m_CarsolAreaNumZ;
        numArray[1] = num1 * (float) this.carsolObjs[this.nKind].m_CarsolAreaNumX;
      }
      vector3 = Vector3.op_Subtraction(point, Vector3.op_Addition(this.MinPos, new Vector3(numArray[0], 0.0f, numArray[1])));
      vector3.x = (__Null) ((double) this.fGridSize * (double) Mathf.RoundToInt((float) vector3.x / this.fGridSize));
      vector3.z = (__Null) ((double) this.fGridSize * (double) Mathf.RoundToInt((float) vector3.z / this.fGridSize));
      ref Vector3 local1 = ref vector3;
      local1.x = (__Null) (local1.x + (this.MinPos.x + (double) numArray[0]));
      ref Vector3 local2 = ref vector3;
      local2.z = (__Null) (local2.z + (this.MinPos.z + (double) numArray[1]));
    }
    RaycastHit raycastHit1 = this.HitGrid[0];
    GridInfo component = (GridInfo) ((Component) ((RaycastHit) ref raycastHit1).get_collider()).GetComponent<GridInfo>();
    int nTargetFloorCnt = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
    int num3 = 0;
    foreach (int num2 in component.GetStackWallOnSmallGrid(0, nTargetFloorCnt))
    {
      if (num2 != -1)
        ++num3;
    }
    point.x = vector3.x;
    point.z = vector3.z;
    this.HitGrid.Clear();
    if (!Physics.Raycast(Vector3.op_Addition(point, new Vector3(0.0f, 1f, 0.0f)), Vector3.get_down(), 100f))
      return;
    ((Component) this).get_transform().set_position(point);
  }

  public void SetDirection(int dir)
  {
    this.nDirection = dir;
  }

  public int GetDirection()
  {
    return this.nDirection;
  }

  public List<RaycastHit[]> CheckCarsol(
    Quaternion CarsolRot,
    Quaternion rotation,
    int kind,
    ref BoxRange range,
    Vector3? pos = null)
  {
    Quaternion quaternion = Quaternion.op_Multiply(rotation, Quaternion.Inverse(CarsolRot));
    List<RaycastHit[]> raycastHitArrayList = new List<RaycastHit[]>();
    switch (kind)
    {
      case 6:
        Vector3[] vector3Array1 = new Vector3[2];
        vector3Array1[0].x = (__Null) (double) this.fGridSize;
        vector3Array1[0].y = (__Null) (double) this.fGridSize;
        vector3Array1[0].z = (__Null) ((double) this.fGridSize * 2.0);
        vector3Array1[1].x = (__Null) (double) this.fGridSize;
        vector3Array1[1].y = (__Null) (double) this.fGridSize;
        vector3Array1[1].z = (__Null) (double) this.fGridSize;
        for (int index = 0; index < 2; ++index)
        {
          ref Vector3 local1 = ref vector3Array1[index];
          local1 = Vector3.op_Division(local1, 2f);
          ref Vector3 local2 = ref vector3Array1[index];
          local2 = Vector3.op_Multiply(local2, 0.9f);
        }
        Transform[] componentsInChildren1 = (Transform[]) this.carsolObjs[kind].m_Obj.GetComponentsInChildren<Transform>();
        Vector3[] vector3Array2 = new Vector3[2];
        if (pos.HasValue)
        {
          for (int index = 0; index < componentsInChildren1.Length; ++index)
          {
            Transform transform = componentsInChildren1[index];
            transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Subtraction(pos.Value, ((Component) this).get_gameObject().get_transform().get_position())));
          }
        }
        vector3Array2[0] = Vector3.op_Addition(componentsInChildren1[1].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren1[2].get_position(), componentsInChildren1[1].get_position()), 2f));
        vector3Array2[1] = componentsInChildren1[3].get_position();
        for (int index = 0; index < 2; ++index)
          vector3Array2[index] = Vector3.op_Subtraction(vector3Array2[index], this.carsolObjs[kind].m_Obj.get_transform().get_position());
        for (int index = 0; index < 2; ++index)
        {
          vector3Array2[index] = Quaternion.op_Multiply(quaternion, vector3Array2[index]);
          ref Vector3 local = ref vector3Array2[index];
          local = Vector3.op_Addition(local, this.carsolObjs[kind].m_Obj.get_transform().get_position());
          raycastHitArrayList.Add(Physics.BoxCastAll(vector3Array2[index], vector3Array1[index], Vector3.get_down(), rotation, 100f));
        }
        BoxRange[] boxRangeArray1 = new BoxRange[2];
        for (int index = 0; index < 2; ++index)
        {
          Carsol.GetBoxCastRange(vector3Array2[index], vector3Array1[index], rotation, ref boxRangeArray1[index]);
          if (index == 0)
            range = boxRangeArray1[0];
          range.MinX = Mathf.Min(range.MinX, boxRangeArray1[index].MinX);
          range.MinZ = Mathf.Min(range.MinZ, boxRangeArray1[index].MinZ);
          range.MaxX = Mathf.Max(range.MaxX, boxRangeArray1[index].MaxX);
          range.MaxZ = Mathf.Max(range.MaxZ, boxRangeArray1[index].MaxZ);
        }
        break;
      case 7:
        Vector3[] vector3Array3 = new Vector3[3];
        vector3Array3[0].x = (__Null) (double) this.fGridSize;
        vector3Array3[0].y = (__Null) (double) this.fGridSize;
        vector3Array3[0].z = (__Null) ((double) this.fGridSize * 2.0);
        vector3Array3[1].x = (__Null) (double) this.fGridSize;
        vector3Array3[1].y = (__Null) (double) this.fGridSize;
        vector3Array3[1].z = (__Null) (double) this.fGridSize;
        vector3Array3[2] = vector3Array3[0];
        for (int index = 0; index < 3; ++index)
        {
          ref Vector3 local1 = ref vector3Array3[index];
          local1 = Vector3.op_Division(local1, 2f);
          ref Vector3 local2 = ref vector3Array3[index];
          local2 = Vector3.op_Multiply(local2, 0.9f);
        }
        Transform[] componentsInChildren2 = (Transform[]) this.carsolObjs[kind].m_Obj.GetComponentsInChildren<Transform>();
        Vector3[] vector3Array4 = new Vector3[3];
        if (pos.HasValue)
        {
          for (int index = 0; index < componentsInChildren2.Length; ++index)
          {
            Transform transform = componentsInChildren2[index];
            transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Subtraction(pos.Value, ((Component) this).get_gameObject().get_transform().get_position())));
          }
        }
        vector3Array4[0] = Vector3.op_Addition(componentsInChildren2[1].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren2[2].get_position(), componentsInChildren2[1].get_position()), 2f));
        vector3Array4[1] = componentsInChildren2[3].get_position();
        vector3Array4[2] = Vector3.op_Addition(componentsInChildren2[4].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren2[5].get_position(), componentsInChildren2[4].get_position()), 2f));
        for (int index = 0; index < 3; ++index)
          vector3Array4[index] = Vector3.op_Subtraction(vector3Array4[index], this.carsolObjs[kind].m_Obj.get_transform().get_position());
        for (int index = 0; index < 3; ++index)
        {
          vector3Array4[index] = Quaternion.op_Multiply(quaternion, vector3Array4[index]);
          ref Vector3 local = ref vector3Array4[index];
          local = Vector3.op_Addition(local, this.carsolObjs[kind].m_Obj.get_transform().get_position());
          raycastHitArrayList.Add(Physics.BoxCastAll(vector3Array4[index], vector3Array3[index], Vector3.get_down(), rotation, 100f));
        }
        BoxRange[] boxRangeArray2 = new BoxRange[3];
        for (int index = 0; index < 3; ++index)
        {
          Carsol.GetBoxCastRange(vector3Array4[index], vector3Array3[index], rotation, ref boxRangeArray2[index]);
          if (index == 0)
            range = boxRangeArray2[0];
          range.MinX = Mathf.Min(range.MinX, boxRangeArray2[index].MinX);
          range.MinZ = Mathf.Min(range.MinZ, boxRangeArray2[index].MinZ);
          range.MaxX = Mathf.Max(range.MaxX, boxRangeArray2[index].MaxX);
          range.MaxZ = Mathf.Max(range.MaxZ, boxRangeArray2[index].MaxZ);
        }
        break;
      case 8:
        Vector3[] vector3Array5 = new Vector3[3];
        vector3Array5[0].x = (__Null) (double) this.fGridSize;
        vector3Array5[0].y = (__Null) (double) this.fGridSize;
        vector3Array5[0].z = (__Null) ((double) this.fGridSize * 2.0);
        vector3Array5[1] = vector3Array5[0];
        vector3Array5[2] = vector3Array5[1];
        for (int index = 0; index < 3; ++index)
        {
          ref Vector3 local1 = ref vector3Array5[index];
          local1 = Vector3.op_Division(local1, 2f);
          ref Vector3 local2 = ref vector3Array5[index];
          local2 = Vector3.op_Multiply(local2, 0.9f);
        }
        Transform[] componentsInChildren3 = (Transform[]) this.carsolObjs[kind].m_Obj.GetComponentsInChildren<Transform>();
        Vector3[] vector3Array6 = new Vector3[3];
        if (pos.HasValue)
        {
          for (int index = 0; index < componentsInChildren3.Length; ++index)
          {
            Transform transform = componentsInChildren3[index];
            transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Subtraction(pos.Value, ((Component) this).get_gameObject().get_transform().get_position())));
          }
        }
        vector3Array6[0] = Vector3.op_Addition(componentsInChildren3[1].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren3[2].get_position(), componentsInChildren3[1].get_position()), 2f));
        vector3Array6[1] = Vector3.op_Addition(componentsInChildren3[3].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren3[4].get_position(), componentsInChildren3[3].get_position()), 2f));
        vector3Array6[2] = Vector3.op_Addition(componentsInChildren3[5].get_position(), Vector3.op_Division(Vector3.op_Subtraction(componentsInChildren3[6].get_position(), componentsInChildren3[5].get_position()), 2f));
        for (int index = 0; index < 3; ++index)
          vector3Array6[index] = Vector3.op_Subtraction(vector3Array6[index], this.carsolObjs[kind].m_Obj.get_transform().get_position());
        for (int index = 0; index < 3; ++index)
        {
          vector3Array6[index] = Quaternion.op_Multiply(quaternion, vector3Array6[index]);
          ref Vector3 local = ref vector3Array6[index];
          local = Vector3.op_Addition(local, this.carsolObjs[kind].m_Obj.get_transform().get_position());
          raycastHitArrayList.Add(Physics.BoxCastAll(vector3Array6[index], vector3Array5[index], Vector3.get_down(), rotation, 100f));
        }
        BoxRange[] boxRangeArray3 = new BoxRange[3];
        for (int index = 0; index < 3; ++index)
        {
          Carsol.GetBoxCastRange(vector3Array6[index], vector3Array5[index], rotation, ref boxRangeArray3[index]);
          if (index == 0)
            range = boxRangeArray3[0];
          range.MinX = Mathf.Min(range.MinX, boxRangeArray3[index].MinX);
          range.MinZ = Mathf.Min(range.MinZ, boxRangeArray3[index].MinZ);
          range.MaxX = Mathf.Max(range.MaxX, boxRangeArray3[index].MaxX);
          range.MaxZ = Mathf.Max(range.MaxZ, boxRangeArray3[index].MaxZ);
        }
        break;
      default:
        Vector3 RaySize;
        RaySize.x = (__Null) ((double) this.fGridSize * (double) this.carsolObjs[kind].m_CarsolAreaNumX);
        RaySize.y = (__Null) (double) this.fGridSize;
        RaySize.z = (__Null) ((double) this.fGridSize * (double) this.carsolObjs[kind].m_CarsolAreaNumZ);
        RaySize = Vector3.op_Division(RaySize, 2f);
        RaySize = Vector3.op_Multiply(RaySize, 0.9f);
        Vector3 Pos = this.carsolObjs[kind].m_Obj.get_transform().get_position();
        if (pos.HasValue)
          Pos = Vector3.op_Addition(Pos, Vector3.op_Subtraction(pos.Value, ((Component) this).get_gameObject().get_transform().get_position()));
        raycastHitArrayList.Add(Physics.BoxCastAll(Pos, RaySize, Vector3.get_down(), rotation, 100f));
        Carsol.GetBoxCastRange(Pos, RaySize, rotation, ref range);
        break;
    }
    return raycastHitArrayList;
  }

  private static void GetBoxCastRange(
    Vector3 Pos,
    Vector3 RaySize,
    Quaternion rotation,
    ref BoxRange range)
  {
    Vector3[] vector3Array = new Vector3[4];
    vector3Array[0].x = Pos.x - RaySize.x;
    vector3Array[0].y = Pos.y - RaySize.y;
    vector3Array[0].z = Pos.z - RaySize.z;
    vector3Array[1].x = Pos.x + RaySize.x;
    vector3Array[1].y = Pos.y - RaySize.y;
    vector3Array[1].z = Pos.z - RaySize.z;
    vector3Array[2].x = Pos.x - RaySize.x;
    vector3Array[2].y = Pos.y - RaySize.y;
    vector3Array[2].z = Pos.z + RaySize.z;
    vector3Array[3].x = Pos.x + RaySize.x;
    vector3Array[3].y = Pos.y - RaySize.y;
    vector3Array[3].z = Pos.z + RaySize.z;
    for (int index = 0; index < vector3Array.Length; ++index)
    {
      ref Vector3 local1 = ref vector3Array[index];
      local1 = Vector3.op_Subtraction(local1, Pos);
      vector3Array[index] = Quaternion.op_Multiply(rotation, vector3Array[index]);
      ref Vector3 local2 = ref vector3Array[index];
      local2 = Vector3.op_Addition(local2, Pos);
    }
    range.MinX = Mathf.Min((float) vector3Array[0].x, Mathf.Min((float) vector3Array[1].x, Mathf.Min((float) vector3Array[2].x, (float) vector3Array[3].x)));
    range.MinZ = Mathf.Min((float) vector3Array[0].z, Mathf.Min((float) vector3Array[1].z, Mathf.Min((float) vector3Array[2].z, (float) vector3Array[3].z)));
    range.MaxX = Mathf.Max((float) vector3Array[0].x, Mathf.Max((float) vector3Array[1].x, Mathf.Max((float) vector3Array[2].x, (float) vector3Array[3].x)));
    range.MaxZ = Mathf.Max((float) vector3Array[0].z, Mathf.Max((float) vector3Array[1].z, Mathf.Max((float) vector3Array[2].z, (float) vector3Array[3].z)));
  }

  public void SetMoveLimit(float itemId)
  {
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(0.0f, 0.0f, 0.0f));
    this.nDirection = 0;
    if ((double) itemId == 1.0)
      this.bMoveLimit = true;
    else
      this.bMoveLimit = false;
  }

  public void ChangeCarsol(int partsform)
  {
    this.nKind = partsform;
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(0.0f, 0.0f, 0.0f));
    for (int index = 0; index < this.carsolObjs.Length; ++index)
    {
      if (index == this.nKind)
        this.carsolObjs[index].m_Obj.SetActive(true);
      else
        this.carsolObjs[index].m_Obj.SetActive(false);
    }
  }

  public void ResetCursolDirPos()
  {
    ((Component) this.cursoldir).get_transform().set_localPosition(this.cursoldirInitPos);
  }

  public void SelectModeCarsolUnvisible()
  {
    this.carsolObjs[this.nKind].m_Obj.SetActive(((this.carsolObjs[this.nKind].m_Obj.get_activeSelf() ? 1 : 0) ^ 1) != 0);
  }
}
