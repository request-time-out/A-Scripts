// Decompiled with JetBrains decompiler
// Type: GridMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
  private List<RoofReservePos> RoofReserve = new List<RoofReservePos>();
  private int[] nJudgeItemKind = new int[4]{ 3, 16, 6, 15 };
  private int nNumX;
  private int nNumZ;
  private GridMapCel[,] Map;
  private SerchMarcker[,] Serched;

  private void Start()
  {
  }

  public void Init(List<GameObject> GridList, int numX, int numZ)
  {
    this.nNumX = numX * 2;
    this.nNumZ = numZ * 2;
    this.Map = new GridMapCel[this.nNumZ, this.nNumX];
    this.Serched = new SerchMarcker[this.nNumZ, this.nNumX];
    this.ChangeCraftMap(GridList, 0);
  }

  public void ChangeCraftMap(List<GameObject> GridList, int floorCnt = 0)
  {
    int[] numArray = new int[5];
    for (int index1 = 0; index1 < this.nNumZ; ++index1)
    {
      for (int index2 = 0; index2 < this.nNumX; ++index2)
      {
        GridInfo component = (GridInfo) GridList[index2 / 2 + this.nNumX / 2 * (index1 / 2)].GetComponent<GridInfo>();
        if (index1 % 2 == 0)
        {
          int[] stackWallOnSmallGrid = component.GetStackWallOnSmallGrid(index2 % 2, floorCnt);
          this.Map[index1, index2].PartsOnMap = component.GetPartOnSmallGrid(index2 % 2, floorCnt);
          this.Map[index1, index2].StackWallOnMap = stackWallOnSmallGrid;
          this.Map[index1, index2].GridID = index2 / 2 + this.nNumX / 2 * (index1 / 2);
          this.Map[index1, index2].smallGridID = index2 % 2;
          this.Map[index1, index2].smallGridCanRoof = component.GetSmallGridCanRoof(index2 % 2, floorCnt);
        }
        else
        {
          int[] stackWallOnSmallGrid = component.GetStackWallOnSmallGrid(2 + index2 % 2, floorCnt);
          this.Map[index1, index2].PartsOnMap = component.GetPartOnSmallGrid(2 + index2 % 2, floorCnt);
          this.Map[index1, index2].StackWallOnMap = stackWallOnSmallGrid;
          this.Map[index1, index2].GridID = index2 / 2 + this.nNumX / 2 * (index1 / 2);
          this.Map[index1, index2].smallGridID = 2 + index2 % 2;
          this.Map[index1, index2].smallGridCanRoof = component.GetSmallGridCanRoof(index2 % 2, floorCnt);
        }
      }
    }
  }

  public void CraftMapSearchRoom(List<GameObject> GridList, int floorcnt)
  {
    this.WallCheck();
    this.SmallGridInRoomArrangement();
    this.ChangeGrid(GridList, floorcnt);
    this.RoofReserve.Clear();
  }

  private void WallCheck()
  {
    for (int z = 0; z < this.nNumZ; ++z)
    {
      for (int x = 0; x < this.nNumX; ++x)
      {
        bool flag = false;
        for (int index = 0; index < this.nJudgeItemKind.Length; ++index)
        {
          flag = this.Map[z, x].PartsOnMap[1] == this.nJudgeItemKind[index] || this.Map[z, x].PartsOnMap[2] == this.nJudgeItemKind[index];
          if (flag)
            break;
        }
        if (!flag)
        {
          for (int index = 0; index < this.Map[z, x].StackWallOnMap.Length; ++index)
          {
            flag = flag || this.Map[z, x].StackWallOnMap[index] == 4;
            if (flag)
              break;
          }
        }
        if (flag)
        {
          this.Map[z, x].smallGridCanRoof = 2;
        }
        else
        {
          int num = this.CheckWallNum(x, z, 0);
          if (num > 2)
          {
            this.Map[z, x].smallGridCanRoof = 1;
            RoofReservePos roofReservePos;
            roofReservePos.PosX = x;
            roofReservePos.PosZ = z;
            roofReservePos.WallHitNum = num;
            this.RoofReserve.Add(roofReservePos);
          }
          else
            this.Map[z, x].smallGridCanRoof = 0;
        }
      }
    }
  }

  private void SmallGridInRoomArrangement()
  {
    for (int index = 0; index < this.RoofReserve.Count; ++index)
    {
      this.SerchedAllFalse();
      bool[] flagArray = new bool[4]
      {
        true,
        true,
        true,
        true
      };
      this.Serched[this.RoofReserve[index].PosZ, this.RoofReserve[index].PosX].Serched = true;
      flagArray[0] = this.SerchCanRoof(this.RoofReserve[index].PosX, this.RoofReserve[index].PosZ + 1, 0);
      flagArray[1] = this.SerchCanRoof(this.RoofReserve[index].PosX + 1, this.RoofReserve[index].PosZ, 1);
      flagArray[2] = this.SerchCanRoof(this.RoofReserve[index].PosX, this.RoofReserve[index].PosZ - 1, 2);
      flagArray[3] = this.SerchCanRoof(this.RoofReserve[index].PosX - 1, this.RoofReserve[index].PosZ, 3);
      if (flagArray[0] & flagArray[1] & flagArray[2] & flagArray[3])
      {
        this.Serched[this.RoofReserve[index].PosZ, this.RoofReserve[index].PosX].rootTrue = true;
        this.Map[this.RoofReserve[index].PosZ, this.RoofReserve[index].PosX].smallGridCanRoof = 2;
        this.ChangeMapCanRoof(this.Serched);
        this.CheckWallNum(this.RoofReserve[index].PosX, this.RoofReserve[index].PosZ, 1);
      }
      else
        this.Map[this.RoofReserve[index].PosZ, this.RoofReserve[index].PosX].smallGridCanRoof = 0;
    }
  }

  private void ChangeGrid(List<GameObject> GridList, int floorCnt = 0)
  {
    for (int index1 = 0; index1 < this.nNumZ; ++index1)
    {
      for (int index2 = 0; index2 < this.nNumX; ++index2)
        ((GridInfo) GridList[this.Map[index1, index2].GridID].GetComponent<GridInfo>()).SetCanRoofSmallGrid(this.Map[index1, index2].smallGridID, floorCnt, this.Map[index1, index2].smallGridCanRoof);
    }
  }

  public bool CraftMapRoofDecide()
  {
    for (int index1 = 0; index1 < this.nNumZ; ++index1)
    {
      for (int index2 = 0; index2 < this.nNumX; ++index2)
      {
        if (this.Map[index1, index2].smallGridCanRoof == 2)
        {
          bool flag = false;
          for (int index3 = 0; index3 < this.nJudgeItemKind.Length; ++index3)
          {
            flag = this.Map[index1, index2].PartsOnMap[1] == this.nJudgeItemKind[index3] || this.Map[index1, index2].PartsOnMap[2] == this.nJudgeItemKind[index3];
            if (flag)
              break;
          }
          if (!flag)
          {
            for (int index3 = 0; index3 < this.Map[index1, index2].StackWallOnMap.Length; ++index3)
            {
              flag = flag || this.Map[index1, index2].StackWallOnMap[index3] == 4;
              if (flag)
                break;
            }
          }
          if (!flag)
            return true;
        }
      }
    }
    return false;
  }

  private int CheckWallNum(int x, int z, int Mode = 0)
  {
    bool[] flagArray = new bool[8]
    {
      this.CheckWall(x, z + 1, Mode),
      this.CheckWall(x + 1, z, Mode),
      this.CheckWall(x, z - 1, Mode),
      this.CheckWall(x - 1, z, Mode),
      this.CheckWall(x + 1, z + 1, Mode),
      this.CheckWall(x + 1, z - 1, Mode),
      this.CheckWall(x - 1, z - 1, Mode),
      this.CheckWall(x - 1, z + 1, Mode)
    };
    int num = 0;
    for (int index = 0; index < flagArray.Length; ++index)
    {
      if (flagArray[index])
        ++num;
    }
    return num;
  }

  private bool CheckWall(int x, int z, int Mode = 0)
  {
    if (x < 0 || z < 0 || (x == this.nNumX || z == this.nNumZ))
      return false;
    bool flag = false;
    for (int index = 0; index < this.nJudgeItemKind.Length; ++index)
    {
      flag = false;
      if (this.Map[z, x].PartsOnMap[1] == this.nJudgeItemKind[index] || this.Map[z, x].PartsOnMap[2] == this.nJudgeItemKind[index])
      {
        if (Mode == 1)
          this.Map[z, x].smallGridCanRoof = 2;
        return true;
      }
    }
    for (int index = 0; index < this.Map[z, x].StackWallOnMap.Length; ++index)
    {
      if (this.Map[z, x].StackWallOnMap[index] == 4)
      {
        if (Mode == 1)
          this.Map[z, x].smallGridCanRoof = 2;
        return true;
      }
    }
    return false;
  }

  private bool SerchCanRoof(int x, int z, int Dir)
  {
    if (x < 0 || z < 0 || (x == this.nNumX - 1 || z == this.nNumZ - 1))
      return false;
    if (this.Serched[z, x].Serched)
      return true;
    this.Serched[z, x].Serched = true;
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < this.nJudgeItemKind.Length; ++index)
    {
      flag2 = this.Map[z, x].PartsOnMap[1] == this.nJudgeItemKind[index] || this.Map[z, x].PartsOnMap[2] == this.nJudgeItemKind[index];
      if (flag2)
        break;
    }
    if (!flag2)
    {
      for (int index = 0; index < this.Map[z, x].StackWallOnMap.Length; ++index)
      {
        flag2 = this.Map[z, x].StackWallOnMap[index] == 4;
        if (flag2)
          break;
      }
    }
    if (flag2)
    {
      switch (Dir)
      {
        case 0:
          flag1 = this.CheckCanRoof(x, z - 1);
          break;
        case 1:
          flag1 = this.CheckCanRoof(x - 1, z);
          break;
        case 2:
          flag1 = this.CheckCanRoof(x, z + 1);
          break;
        case 3:
          flag1 = this.CheckCanRoof(x + 1, z);
          break;
      }
    }
    if (flag1)
      return flag1;
    bool flag3 = false;
    switch (Dir)
    {
      case 0:
        bool flag4 = this.SerchCanRoof(x, z + 1, 0);
        if (!flag4)
        {
          this.Serched[z, x].rootTrue = false;
          return flag4;
        }
        bool flag5 = this.SerchCanRoof(x + 1, z, 1);
        if (!flag5)
        {
          this.Serched[z, x].rootTrue = false;
          return flag5;
        }
        flag3 = this.SerchCanRoof(x - 1, z, 3);
        break;
      case 1:
        bool flag6 = this.SerchCanRoof(x, z + 1, 0);
        if (!flag6)
        {
          this.Serched[z, x].rootTrue = false;
          return flag6;
        }
        bool flag7 = this.SerchCanRoof(x + 1, z, 1);
        if (!flag7)
        {
          this.Serched[z, x].rootTrue = false;
          return flag7;
        }
        flag3 = this.SerchCanRoof(x, z - 1, 2);
        break;
      case 2:
        bool flag8 = this.SerchCanRoof(x + 1, z, 1);
        if (!flag8)
        {
          this.Serched[z, x].rootTrue = false;
          return flag8;
        }
        bool flag9 = this.SerchCanRoof(x, z - 1, 2);
        if (!flag9)
        {
          this.Serched[z, x].rootTrue = false;
          return flag9;
        }
        flag3 = this.SerchCanRoof(x - 1, z, 3);
        break;
      case 3:
        bool flag10 = this.SerchCanRoof(x, z + 1, 0);
        if (!flag10)
        {
          this.Serched[z, x].rootTrue = false;
          return flag10;
        }
        bool flag11 = this.SerchCanRoof(x, z - 1, 2);
        if (!flag11)
        {
          this.Serched[z, x].rootTrue = false;
          return flag11;
        }
        flag3 = this.SerchCanRoof(x - 1, z, 3);
        break;
    }
    if (!flag3)
      this.Serched[z, x].rootTrue = false;
    else
      this.Serched[z, x].rootTrue = true;
    return flag3;
  }

  private bool CheckCanRoof(int x, int z)
  {
    bool[] flagArray = new bool[8]
    {
      this.CheckWall(x, z + 1, 0),
      this.CheckWall(x + 1, z, 0),
      this.CheckWall(x, z - 1, 0),
      this.CheckWall(x - 1, z, 0),
      this.CheckWall(x + 1, z + 1, 0),
      this.CheckWall(x + 1, z - 1, 0),
      this.CheckWall(x - 1, z - 1, 0),
      this.CheckWall(x - 1, z + 1, 0)
    };
    return flagArray[0] && flagArray[4] && flagArray[7] || flagArray[1] && flagArray[4] && flagArray[5] || (flagArray[2] && flagArray[5] && flagArray[6] || flagArray[3] && flagArray[6] && flagArray[7]);
  }

  private void ChangeMapCanRoof(SerchMarcker[,] checker)
  {
    for (int index1 = 0; index1 < this.nNumZ; ++index1)
    {
      for (int index2 = 0; index2 < this.nNumX; ++index2)
      {
        if (checker[index1, index2].rootTrue)
          this.Map[index1, index2].smallGridCanRoof = 2;
      }
    }
  }

  private void SerchedAllFalse()
  {
    for (int index1 = 0; index1 < this.nNumZ; ++index1)
    {
      for (int index2 = 0; index2 < this.nNumX; ++index2)
      {
        this.Serched[index1, index2].Serched = false;
        this.Serched[index1, index2].rootTrue = false;
      }
    }
  }
}
