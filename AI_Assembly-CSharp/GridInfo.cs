// Decompiled with JetBrains decompiler
// Type: GridInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
  private List<SmallGrid[]> smallGrids;
  private List<int> nInRoom;
  private List<int> nCanRoof;
  private List<bool> bUse;
  public Vector3 InitPos;
  public int nID;
  public List<int> nFloorPartsHeight;
  public static int nSmallGridStackWallMax;
  private int nFloorNum;
  public const int nSmallGridNum = 4;
  public const int nSmallGridItemKindMax = 7;

  public GridInfo()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.nFloorNum = 1;
  }

  public void Init(int floorcnt = 0)
  {
    this.smallGrids.Add(new SmallGrid[4]);
    Renderer[] componentsInChildren = (Renderer[]) ((Component) this).GetComponentsInChildren<Renderer>();
    GridInfo.nSmallGridStackWallMax = 9;
    for (int index = 0; index < 4; ++index)
    {
      this.smallGrids[floorcnt][index].m_state = 0;
      this.smallGrids[floorcnt][index].m_canRoof = 0;
      this.smallGrids[floorcnt][index].m_itemkind = new int[7];
      this.smallGrids[floorcnt][index].m_itemstackwall = new int[GridInfo.nSmallGridStackWallMax];
      this.smallGrids[floorcnt][index].m_itemdupulication = new int[GridInfo.nSmallGridStackWallMax];
      this.smallGrids[floorcnt][index].m_PutElement = new List<int>();
      this.ChangeSmallGrid(index, 0, -1, floorcnt, false);
      this.smallGrids[floorcnt][index].m_inRoom = false;
      this.smallGrids[floorcnt][index].m_UnderCarsol = false;
      this.smallGrids[floorcnt][index].m_color = componentsInChildren[index];
      this.ClearSmallGridItemKind(floorcnt, index);
      if (floorcnt != 0)
        this.ChangeSmallGrid(index, 0, -1, floorcnt, false);
    }
    this.nInRoom.Add(0);
    this.nCanRoof.Add(0);
    this.bUse.Add(false);
    this.nFloorPartsHeight.Add(0);
  }

  public static void ChangeGridInfo(List<GridInfo> Grid, int floorcnt)
  {
    for (int index1 = 0; index1 < Grid.Count; ++index1)
    {
      int num1 = 0;
      GridInfo gridInfo = Grid[index1];
      for (int index2 = 0; index2 < 4; ++index2)
      {
        if (gridInfo.smallGrids[floorcnt][index2].m_inRoom)
          ++num1;
      }
      gridInfo.nInRoom[floorcnt] = num1 != 4 ? 0 : 1;
      int num2 = 0;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        if (gridInfo.smallGrids[floorcnt][index2].m_canRoof == 2)
          ++num2;
      }
      gridInfo.nCanRoof[floorcnt] = num2 != 4 ? 0 : 1;
    }
  }

  public void ChangeSmallGrid(int ID, int state, int itemkind, int floorcnt, bool duplication = false)
  {
    if (state != 0 && itemkind >= 0)
    {
      if (!duplication)
      {
        switch (itemkind)
        {
          case 1:
            this.ChangeSmallGridItemKind(floorcnt, ID, 0, itemkind);
            break;
          case 2:
            this.ChangeSmallGridItemKind(floorcnt, ID, 0, itemkind);
            break;
          case 3:
            this.ChangeSmallGridItemKind(floorcnt, ID, 1, itemkind);
            break;
          case 4:
            this.ChangeSmallGridStack(floorcnt, ID, itemkind, 0);
            break;
          case 5:
            this.ChangeSmallGridItemKind(floorcnt, ID, 6, itemkind);
            break;
          case 10:
            this.ChangeSmallGridItemKind(floorcnt, ID, 3, itemkind);
            break;
          case 12:
            this.ChangeSmallGridItemKind(floorcnt, ID, 4, itemkind);
            break;
          case 14:
            this.ChangeSmallGridItemKind(floorcnt, ID, 5, itemkind);
            break;
          default:
            this.ChangeSmallGridItemKind(floorcnt, ID, 2, itemkind);
            break;
        }
      }
      else
        this.ChangeSmallGridStack(floorcnt, ID, itemkind, 0);
    }
    else if (state == 0)
    {
      this.ClearSmallGridItemKind(floorcnt, ID);
      this.ChangeSmallGridStack(floorcnt, ID, 4, -1);
      this.ChangeSmallGridStack(floorcnt, ID, itemkind, -1);
    }
    if (this.smallGrids[floorcnt][ID].m_state == state)
      return;
    this.smallGrids[floorcnt][ID].m_state = state;
  }

  public void ChangeSmallGridColor(int floorcnt, int ID)
  {
    Color color;
    switch (this.smallGrids[floorcnt][ID].m_state)
    {
      case 0:
        color.r = (__Null) 0.501960813999176;
        color.g = (__Null) 0.501960813999176;
        color.b = (__Null) 0.501960813999176;
        color.a = (__Null) 0.0392156876623631;
        break;
      case 1:
        color.r = (__Null) 1.0;
        color.g = (__Null) 0.0;
        color.b = (__Null) 0.0;
        color.a = (__Null) 0.941176474094391;
        break;
      case 2:
        color.r = (__Null) 0.501960813999176;
        color.g = (__Null) 0.501960813999176;
        color.b = (__Null) 0.501960813999176;
        color.a = (__Null) 0.117647059261799;
        break;
      default:
        color.r = (__Null) 0.501960813999176;
        color.g = (__Null) 0.501960813999176;
        color.b = (__Null) 0.501960813999176;
        color.a = (__Null) 0.941176474094391;
        break;
    }
    if (this.smallGrids[floorcnt][ID].m_UnderCarsol)
      color.g = (__Null) 1.0;
    if (this.smallGrids[floorcnt][ID].m_inRoom)
      color.b = (__Null) 1.0;
    this.smallGrids[floorcnt][ID].m_color.get_material().SetColor("_TintColor", color);
  }

  public void ChangeSmallGridColor(int floorcnt, int ID, Color changeColor)
  {
    this.smallGrids[floorcnt][ID].m_color.get_material().SetColor("_TintColor", changeColor);
  }

  public Color GetSmallGridColor(int floorcnt, int ID)
  {
    return this.smallGrids[floorcnt][ID].m_color.get_material().GetColor("_TintColor");
  }

  public void SetInRoomSmallGrid(int id, bool roomstate, int floorcnt)
  {
    if (this.smallGrids[floorcnt][id].m_inRoom == roomstate)
      return;
    this.smallGrids[floorcnt][id].m_inRoom = roomstate;
  }

  public void SetCanRoofSmallGrid(int id, int floorCnt, int setstate)
  {
    this.smallGrids[floorCnt][id].m_canRoof = setstate;
  }

  public void SetUseState(int floorcnt, bool use)
  {
    this.bUse[floorcnt] = use;
  }

  public int GetStateSmallGrid(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_state;
  }

  public int[] GetPartOnSmallGrid(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_itemkind;
  }

  public int[] GetStackWallOnSmallGrid(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_itemstackwall;
  }

  public int[] GetStackPartsOnSmallGrid(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_itemdupulication;
  }

  public bool GetSmallGridInRoom(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_inRoom;
  }

  public int GetSmallGridCanRoof(int id, int floorcnt)
  {
    return this.smallGrids[floorcnt][id].m_canRoof;
  }

  public int GetInRoomState(int floorcnt)
  {
    return this.nInRoom[floorcnt];
  }

  public int GetCanRoofState(int floorcnt)
  {
    return this.nCanRoof[floorcnt];
  }

  public bool GetUseState(int floorcnt)
  {
    return this.bUse[floorcnt];
  }

  public int GetFloorNum()
  {
    return this.nFloorNum;
  }

  public bool GetUnderTheCarsol(int nfloor, int smallid)
  {
    return this.smallGrids[nfloor][smallid].m_UnderCarsol;
  }

  public void AddFloor()
  {
    this.Init(this.smallGrids.Count);
    ++this.nFloorNum;
  }

  public void DelFloor(int floorcnt)
  {
    this.smallGrids.RemoveRange(floorcnt, this.smallGrids.Count - floorcnt);
    this.nInRoom.RemoveRange(floorcnt, this.nInRoom.Count - floorcnt);
    this.nCanRoof.RemoveRange(floorcnt, this.nCanRoof.Count - floorcnt);
    this.bUse.RemoveRange(floorcnt, this.bUse.Count - floorcnt);
    this.nFloorPartsHeight.RemoveRange(floorcnt, this.nCanRoof.Count - floorcnt);
    this.nFloorNum = this.smallGrids.Count;
    if (this.nFloorNum <= 0 || this.smallGrids[0][0].m_itemkind[2] != 11)
      return;
    for (int index = 0; index < 4; ++index)
    {
      this.ClearSmallGridItemKind(this.nFloorNum - 1, index);
      this.smallGrids[this.nFloorNum - 1][index].m_state = 0;
      this.ChangeSmallGridColor(this.nFloorNum - 1, index);
    }
  }

  private void ClearSmallGridItemKind(int floor, int smallID)
  {
    for (int changePlace = 0; changePlace < 7; ++changePlace)
      this.ChangeSmallGridItemKind(floor, smallID, changePlace, -1);
  }

  public void ChangeSmallGridItemKind(int floor, int smallID, int changePlace, int changeState)
  {
    if (this.smallGrids[floor][smallID].m_itemkind[changePlace] == changeState)
      return;
    this.smallGrids[floor][smallID].m_itemkind[changePlace] = changeState;
  }

  public void ChangeSmallGridUnderCarsol(int floor, int smallID, bool changeState)
  {
    this.smallGrids[floor][smallID].m_UnderCarsol = changeState;
    this.ChangeSmallGridColor(floor, smallID);
  }

  public void ChangeSmallGridStack(int floor, int smallID, int itemkind, int mode = 0)
  {
    int[] numArray = itemkind != 4 ? this.smallGrids[floor][smallID].m_itemdupulication : this.smallGrids[floor][smallID].m_itemstackwall;
    int num = ((IEnumerable<int>) numArray).Count<int>((Func<int, bool>) (n => n != -1));
    switch (mode)
    {
      case 0:
        if (num > GridInfo.nSmallGridStackWallMax)
          break;
        for (int index = 0; index < numArray.Length; ++index)
        {
          if (numArray[index] == -1)
          {
            numArray[index] = itemkind;
            break;
          }
        }
        break;
      case 1:
        if (num <= 0)
          break;
        for (int index = GridInfo.nSmallGridStackWallMax - 1; index >= 0; --index)
        {
          if (numArray[index] != -1)
          {
            numArray[index] = -1;
            break;
          }
        }
        break;
      default:
        for (int index = 0; index < GridInfo.nSmallGridStackWallMax; ++index)
        {
          if (numArray[index] != -1)
            numArray[index] = -1;
        }
        break;
    }
  }

  public void SetSmallGridPutElement(
    int floorCnt,
    int smallgridID,
    int element,
    bool del = false,
    bool floor = false)
  {
    if (!del)
    {
      if (!floor || this.smallGrids[floorCnt][smallgridID].m_PutElement.Count == 0)
        this.smallGrids[floorCnt][smallgridID].m_PutElement.Add(element);
      else
        this.smallGrids[floorCnt][smallgridID].m_PutElement.Insert(0, element);
    }
    else
      this.smallGrids[floorCnt][smallgridID].m_PutElement.RemoveAt(this.smallGrids[floorCnt][smallgridID].m_PutElement.Count - 1);
  }

  public List<int> GetSmallGridPutElement(int floorCnt, int smallgridID)
  {
    return this.smallGrids[floorCnt][smallgridID].m_PutElement.Count <= 0 ? (List<int>) null : this.smallGrids[floorCnt][smallgridID].m_PutElement;
  }
}
