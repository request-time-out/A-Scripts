// Decompiled with JetBrains decompiler
// Type: CraftCommandList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using OutputLogControl;
using System.Collections.Generic;
using UnityEngine;

public class CraftCommandList : Singleton<CraftCommandList>
{
  public void GridChange(GridInfo source, CraftCommandList.ChangeValGrid val, int mode)
  {
    source.DelFloor(0);
    for (int index = 0; index < val.nFloorNum[mode]; ++index)
      source.AddFloor();
    for (int index1 = 0; index1 < val.nFloorNum[mode]; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
      {
        source.SetCanRoofSmallGrid(index2, index1, val.smallGrids[mode][index1][index2].m_canRoof);
        source.SetInRoomSmallGrid(index2, val.smallGrids[mode][index1][index2].m_inRoom, index1);
        source.ChangeSmallGrid(index2, val.smallGrids[mode][index1][index2].m_state, -1, index1, false);
        for (int changePlace = 0; changePlace < 7; ++changePlace)
          source.ChangeSmallGridItemKind(index1, index2, changePlace, val.smallGrids[mode][index1][index2].m_itemkind[changePlace]);
        int num1 = 0;
        for (int index3 = 0; index3 < val.smallGrids[mode][index1][index2].m_itemstackwall.Length && val.smallGrids[mode][index1][index2].m_itemstackwall[index3] != -1; ++index3)
          ++num1;
        for (int index3 = 0; index3 < num1; ++index3)
          source.ChangeSmallGridStack(index1, index2, 4, 0);
        int num2 = 0;
        for (int index3 = 0; index3 < val.smallGrids[mode][index1][index2].m_itemstackwall.Length && val.smallGrids[mode][index1][index2].m_itemdupulication[index3] != -1; ++index3)
          ++num2;
        for (int index3 = 0; index3 < num2; ++index3)
          source.ChangeSmallGridStack(index1, index2, val.smallGrids[mode][index1][index2].m_itemdupulication[index3], 0);
        source.ChangeSmallGridUnderCarsol(index1, index2, source.GetUnderTheCarsol(index1, index2));
        source.ChangeSmallGridColor(index1, index2);
        if (val.smallGrids[mode][index1][index2].m_PutElement != null)
        {
          for (int index3 = 0; index3 < val.smallGrids[mode][index1][index2].m_PutElement.Count; ++index3)
            source.SetSmallGridPutElement(index1, index2, val.smallGrids[mode][index1][index2].m_PutElement[index3], false, false);
        }
      }
      source.SetUseState(index1, val.bUse[mode][index1]);
    }
    source.nFloorPartsHeight = val.nFloorPartsHeight[mode];
    if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt >= source.GetFloorNum())
      --Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
    ((Component) source).get_gameObject().get_transform().set_position(val.Pos[mode]);
    if (!source.GetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt))
      return;
    ((Component) source).get_gameObject().SetActive(true);
  }

  public void PartsChange(
    List<BuildPartsPool>[] source,
    CraftCommandList.ChangeValParts val,
    int mode,
    bool Auto)
  {
    GameObject gameObject = source[val.nFormKind][val.nPoolID].GetList()[val.nItemID];
    if (!Auto)
    {
      if (val.active[mode])
        source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, 0);
      else
        source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, -1);
      gameObject.SetActive(val.active[mode]);
    }
    else
    {
      if (val.ReserveList[mode] >= 0)
        source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, 0);
      else
        source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, -1);
      if (val.nPutFloor[mode] == 0)
        gameObject.SetActive(val.active[mode]);
      else if (val.nPutFloor[mode] > 0 && val.nPutFloor[mode] <= Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
      {
        gameObject.SetActive(source[val.nFormKind][val.nPoolID].ReserveListCheck(val.nItemID));
      }
      else
      {
        if (val.nPutFloor[mode] >= 0)
          source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, 0);
        else
          source[val.nFormKind][val.nPoolID].ReserveListChange(val.nItemID, -1);
        gameObject.SetActive(false);
      }
    }
    gameObject.get_transform().set_position(val.Pos[mode]);
    gameObject.get_transform().set_rotation(val.Rot[mode]);
    BuildPartsInfo component = (BuildPartsInfo) gameObject.GetComponent<BuildPartsInfo>();
    component.nPutFloor = val.nPutFloor[mode];
    component.SetDirection(val.nDirection[mode]);
  }

  public class ChangeValParts
  {
    public bool[] active = new bool[2];
    public Vector3[] Pos = new Vector3[2];
    public Quaternion[] Rot = new Quaternion[2];
    public int[] nPutFloor = new int[2];
    public int[] nDirection = new int[2];
    public int[] ReserveList = new int[2];
    public int nFormKind;
    public int nPoolID;
    public int nItemID;
  }

  public class ChangeValGrid
  {
    public Vector3[] Pos = new Vector3[2]
    {
      (Vector3) null,
      (Vector3) null
    };
    public List<SmallGrid[]>[] smallGrids = new List<SmallGrid[]>[2];
    public List<int>[] nInRoom = new List<int>[2];
    public List<int>[] nCanRoof = new List<int>[2];
    public List<bool>[] bUse = new List<bool>[2];
    public List<int>[] nFloorPartsHeight = new List<int>[2];
    public int[] nFloorNum = new int[2];
    public List<Color[]>[] colors = new List<Color[]>[2];

    public ChangeValGrid()
    {
      for (int index = 0; index < 2; ++index)
      {
        this.smallGrids[index] = new List<SmallGrid[]>();
        this.nInRoom[index] = new List<int>();
        this.nCanRoof[index] = new List<int>();
        this.bUse[index] = new List<bool>();
        this.nFloorPartsHeight[index] = new List<int>();
        this.colors[index] = new List<Color[]>();
      }
    }
  }

  public static class PutBuildPartCommand
  {
    public class Command : ICommand
    {
      private int[] maxFloorCnt = new int[2];
      private CraftCommandList.ChangeValGrid[] changeValGrids;
      private CraftCommandList.ChangeValParts[] changeValParts;
      private CraftCommandList.ChangeValParts[] changeValAutoFloor;

      public Command(
        CraftCommandList.ChangeValGrid[] _changeValGrids,
        CraftCommandList.ChangeValParts[] _changeValParts,
        CraftCommandList.ChangeValParts[] _changeValAutoFloor,
        int[] _maxFloorCnt)
      {
        this.changeValGrids = _changeValGrids;
        this.changeValParts = _changeValParts;
        this.changeValAutoFloor = _changeValAutoFloor;
        this.maxFloorCnt = _maxFloorCnt;
      }

      public void Undo()
      {
        OutputLog.Log("PutBuildPartCommand.Undo", true, "Log");
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 0);
        for (int index = 0; index < this.changeValAutoFloor.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoFloor[index], 0, true);
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 0, false);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[0];
      }

      public void Redo()
      {
        OutputLog.Log("PutBuildPartCommand.Redo", true, "Log");
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 1);
        for (int index = 0; index < this.changeValAutoFloor.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoFloor[index], 1, true);
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 1, false);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[1];
      }
    }
  }

  public static class DeadBuildPartCommand
  {
    public class Command : ICommand
    {
      private int[] maxFloorCnt = new int[2];
      private CraftCommandList.ChangeValGrid[] changeValGrids;
      private CraftCommandList.ChangeValParts[] changeValParts;
      private CraftCommandList.ChangeValParts[] changeValAutoParts;

      public Command(
        CraftCommandList.ChangeValGrid[] _changeValGrids,
        CraftCommandList.ChangeValParts[] _changeValParts,
        CraftCommandList.ChangeValParts[] _changeValAutoParts,
        int[] _maxFloorCnt)
      {
        this.changeValGrids = _changeValGrids;
        this.changeValParts = _changeValParts;
        this.changeValAutoParts = _changeValAutoParts;
        this.maxFloorCnt = _maxFloorCnt;
      }

      public void Undo()
      {
        OutputLog.Log("PutBuildPartCommand.Undo", true, "Log");
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 0);
        for (int index = 0; index < this.changeValAutoParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoParts[index], 0, true);
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 0, false);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[0];
      }

      public void Redo()
      {
        OutputLog.Log("PutBuildPartCommand.Redo", true, "Log");
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 1);
        for (int index = 0; index < this.changeValAutoParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoParts[index], 1, true);
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 1, false);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[1];
      }
    }
  }

  public static class SelectBuildPartCommand
  {
    public class Command : ICommand
    {
      private int[] maxFloorCnt = new int[2];
      private CraftCommandList.ChangeValGrid[] changeValGrids;
      private CraftCommandList.ChangeValParts[] changeValParts;
      private CraftCommandList.ChangeValParts[] changeValAutoParts;

      public Command(
        CraftCommandList.ChangeValGrid[] _changeValGrids,
        CraftCommandList.ChangeValParts[] _changeValParts,
        CraftCommandList.ChangeValParts[] _changeValAutoParts,
        int[] _maxFloorCnt)
      {
        this.changeValGrids = _changeValGrids;
        this.changeValParts = _changeValParts;
        this.changeValAutoParts = _changeValAutoParts;
        this.maxFloorCnt = _maxFloorCnt;
      }

      public void Undo()
      {
        OutputLog.Log("SelectBuildPartCommand.Undo", true, "Log");
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 0, false);
        for (int index = 0; index < this.changeValAutoParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoParts[index], 0, true);
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 0);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[0];
      }

      public void Redo()
      {
        OutputLog.Log("SelectBuildPartCommand.Redo", true, "Log");
        for (int index = 0; index < this.changeValParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValParts[index], 1, false);
        for (int index = 0; index < this.changeValAutoParts.Length; ++index)
          Singleton<CraftCommandList>.Instance.PartsChange(Singleton<CraftCommandListBaseObject>.Instance.BaseParts, this.changeValAutoParts[index], 1, true);
        for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Count; ++index)
          Singleton<CraftCommandList>.Instance.GridChange(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index], this.changeValGrids[index], 1);
        Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.maxFloorCnt[1];
      }
    }
  }
}
