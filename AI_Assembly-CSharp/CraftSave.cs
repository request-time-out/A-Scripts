// Decompiled with JetBrains decompiler
// Type: CraftSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CraftSave : Singleton<CraftSave>
{
  private CraftSaveData data;
  [SerializeField]
  private VirtualCameraController virtualCamera;
  private GameScreenShotAssist gameScreenShotAssist;
  private byte[] pngData;
  private const int nOffset = 2;

  public void Init()
  {
    this.data.MaxFloorNum = 0;
    this.data.nPutPartsNum = 0;
    this.data.GridPos = new List<Vector3>();
    this.data.GridUseState = new List<bool>();
    this.data.nFloorPartsHeight = new List<int>();
    this.data.SmallGridState = new List<List<List<int>>>();
    this.data.SmallGridOnParts = new List<List<List<int[]>>>();
    this.data.SmallGridOnStackWall = new List<List<List<int[]>>>();
    this.data.SmallGridCanRoofState = new List<List<List<int>>>();
    this.data.SmallGridInRoomState = new List<List<List<bool>>>();
    this.data.BuildPartsGridKind = new List<int>();
    this.data.BuildPartsKind = new List<int>();
    this.data.BuildPartsFloor = new List<int>();
    this.data.BuildPartsPos = new List<Vector3>();
    this.data.BuildPartsRot = new List<Quaternion>();
    this.data.BuildPartsPutGridInfos = new List<List<int>>();
    this.data.BuildPartsPutSmallGridInfos = new List<List<int>>();
    this.data.BuildPartsPutGridInfosNum = new List<int>();
    this.data.tmpGridActiveList = new List<bool[]>();
    this.data.tmpGridActiveListUpdate = new List<bool>();
    this.data.MaxPutHeight = new List<int>();
    this.gameScreenShotAssist = (GameScreenShotAssist) ((Component) this.virtualCamera).GetComponent<GameScreenShotAssist>();
  }

  public void Save(ObjPool Grid)
  {
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    int nMaxFloorCnt = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    this.data.MaxFloorNum = nMaxFloorCnt;
    this.data.nPutPartsNum = Singleton<CraftCommandListBaseObject>.Instance.nPutPartsNum;
    List<GameObject> list1 = Grid.GetList();
    for (int index = 0; index < list1.Count; ++index)
    {
      this.data.SmallGridState.Add(new List<List<int>>());
      this.data.SmallGridOnParts.Add(new List<List<int[]>>());
      this.data.SmallGridOnStackWall.Add(new List<List<int[]>>());
      this.data.SmallGridCanRoofState.Add(new List<List<int>>());
      this.data.SmallGridInRoomState.Add(new List<List<bool>>());
      GridInfo component = (GridInfo) list1[index].GetComponent<GridInfo>();
      this.data.GridPos.Add(component.InitPos);
      for (int floorcnt = 0; floorcnt < nMaxFloorCnt; ++floorcnt)
      {
        this.data.SmallGridState[index].Add(new List<int>());
        this.data.SmallGridOnParts[index].Add(new List<int[]>());
        this.data.SmallGridOnStackWall[index].Add(new List<int[]>());
        this.data.SmallGridCanRoofState[index].Add(new List<int>());
        this.data.SmallGridInRoomState[index].Add(new List<bool>());
        this.data.GridUseState.Add(component.GetUseState(floorcnt));
        this.data.nFloorPartsHeight.Add(component.nFloorPartsHeight[floorcnt]);
        for (int id = 0; id < 4; ++id)
        {
          this.data.SmallGridState[index][floorcnt].Add(component.GetStateSmallGrid(id, floorcnt));
          int[] partOnSmallGrid = component.GetPartOnSmallGrid(id, floorcnt);
          this.data.SmallGridOnParts[index][floorcnt].Add(partOnSmallGrid);
          this.data.SmallGridOnStackWall[index][floorcnt].Add(component.GetStackWallOnSmallGrid(id, floorcnt));
          this.data.SmallGridCanRoofState[index][floorcnt].Add(component.GetSmallGridCanRoof(id, floorcnt));
          this.data.SmallGridInRoomState[index][floorcnt].Add(component.GetSmallGridInRoom(id, floorcnt));
        }
      }
    }
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        List<GameObject> list2 = baseParts[index1][index2].GetList();
        for (int ID = 0; ID < list2.Count; ++ID)
        {
          if (baseParts[index1][index2].ReserveListCheck(ID))
          {
            BuildPartsInfo component = (BuildPartsInfo) list2[ID].GetComponent<BuildPartsInfo>();
            this.data.BuildPartsGridKind.Add(index1);
            this.data.BuildPartsKind.Add(index2);
            this.data.BuildPartsFloor.Add(component.nPutFloor);
            this.data.BuildPartsPos.Add(list2[ID].get_transform().get_localPosition());
            this.data.BuildPartsRot.Add(list2[ID].get_transform().get_localRotation());
            this.data.BuildPartsPutGridInfos.Add(component.putGridInfos.Select<GridInfo, int>((Func<GridInfo, int>) (v => v.nID)).ToList<int>());
            this.data.BuildPartsPutSmallGridInfos.Add(component.putSmallGridInfos);
            this.data.BuildPartsPutGridInfosNum.Add(component.putGridInfos.Count);
          }
        }
      }
    }
    this.data.tmpGridActiveList = Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList;
    this.data.tmpGridActiveListUpdate = Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate;
    this.data.MaxPutHeight = Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight;
    BinaryWriter binaryWriter = new BinaryWriter((Stream) new FileStream(Application.get_dataPath() + "/in-house/Scripts/Game/Scene/Map/Craft/SaveData" + string.Format("/CraftSave{0}_{1:MM}{2}_{3:HH}{4:mm}_{5:ss}{6:ff}.png", (object) DateTime.Now.Year, (object) DateTime.Now, (object) DateTime.Now.Day, (object) DateTime.Now, (object) DateTime.Now, (object) DateTime.Now, (object) DateTime.Now), FileMode.Create, FileAccess.Write), Encoding.UTF8);
    this.pngData = this.CreatePngScreen(320, 180);
    binaryWriter.Write(this.pngData);
    binaryWriter.Write(this.data.MaxFloorNum);
    binaryWriter.Write(this.data.nPutPartsNum);
    for (int index = 0; index < this.data.GridPos.Count; ++index)
    {
      binaryWriter.Write((double) this.data.GridPos[index].x);
      binaryWriter.Write((double) this.data.GridPos[index].y);
      binaryWriter.Write((double) this.data.GridPos[index].z);
    }
    binaryWriter.Write("P");
    for (int index = 0; index < this.data.GridUseState.Count; ++index)
      binaryWriter.Write(this.data.GridUseState[index]);
    binaryWriter.Write("U");
    for (int index = 0; index < this.data.nFloorPartsHeight.Count; ++index)
      binaryWriter.Write(this.data.nFloorPartsHeight[index]);
    binaryWriter.Write("F");
    for (int index1 = 0; index1 < list1.Count; ++index1)
    {
      for (int index2 = 0; index2 < nMaxFloorCnt; ++index2)
      {
        for (int index3 = 0; index3 < this.data.SmallGridState[index1][index2].Count; ++index3)
        {
          binaryWriter.Write(this.data.SmallGridState[index1][index2][index3]);
          for (int index4 = 0; index4 < this.data.SmallGridOnParts[index1][index2][index3].Length; ++index4)
            binaryWriter.Write(this.data.SmallGridOnParts[index1][index2][index3][index4]);
          for (int index4 = 0; index4 < this.data.SmallGridOnStackWall[index1][index2][index3].Length; ++index4)
            binaryWriter.Write(this.data.SmallGridOnStackWall[index1][index2][index3][index4]);
          binaryWriter.Write(this.data.SmallGridCanRoofState[index1][index2][index3]);
          binaryWriter.Write(this.data.SmallGridInRoomState[index1][index2][index3]);
        }
      }
    }
    binaryWriter.Write("S");
    for (int index1 = 0; index1 < this.data.BuildPartsPos.Count; ++index1)
    {
      binaryWriter.Write(this.data.BuildPartsGridKind[index1]);
      binaryWriter.Write(this.data.BuildPartsKind[index1]);
      binaryWriter.Write(this.data.BuildPartsFloor[index1]);
      binaryWriter.Write((double) this.data.BuildPartsPos[index1].x);
      binaryWriter.Write((double) this.data.BuildPartsPos[index1].y);
      binaryWriter.Write((double) this.data.BuildPartsPos[index1].z);
      binaryWriter.Write((double) this.data.BuildPartsRot[index1].x);
      binaryWriter.Write((double) this.data.BuildPartsRot[index1].y);
      binaryWriter.Write((double) this.data.BuildPartsRot[index1].z);
      binaryWriter.Write((double) this.data.BuildPartsRot[index1].w);
      binaryWriter.Write(this.data.BuildPartsPutGridInfosNum[index1]);
      for (int index2 = 0; index2 < this.data.BuildPartsPutGridInfos[index1].Count; ++index2)
      {
        binaryWriter.Write(this.data.BuildPartsPutGridInfos[index1][index2]);
        binaryWriter.Write(this.data.BuildPartsPutSmallGridInfos[index1][index2]);
      }
    }
    binaryWriter.Write("B");
    for (int index1 = 0; index1 < this.data.MaxFloorNum; ++index1)
    {
      for (int index2 = 0; index2 < this.data.GridPos.Count; ++index2)
        binaryWriter.Write(this.data.tmpGridActiveList[index1][index2]);
      binaryWriter.Write(this.data.tmpGridActiveListUpdate[index1]);
      binaryWriter.Write(this.data.MaxPutHeight[index1]);
    }
    binaryWriter.Write("A");
    binaryWriter.Close();
    this.data.MaxFloorNum = 0;
    this.data.nPutPartsNum = 0;
    this.data.GridPos.Clear();
    this.data.GridUseState.Clear();
    this.data.nFloorPartsHeight.Clear();
    this.data.SmallGridState.Clear();
    this.data.SmallGridOnParts.Clear();
    this.data.SmallGridOnStackWall.Clear();
    this.data.SmallGridCanRoofState.Clear();
    this.data.SmallGridInRoomState.Clear();
    this.data.BuildPartsGridKind.Clear();
    this.data.BuildPartsKind.Clear();
    this.data.BuildPartsFloor.Clear();
    this.data.BuildPartsPos.Clear();
    this.data.BuildPartsRot.Clear();
    this.data.BuildPartsPutGridInfos.Clear();
    this.data.BuildPartsPutSmallGridInfos.Clear();
    this.data.BuildPartsPutGridInfosNum.Clear();
    this.data.tmpGridActiveList.Clear();
    this.data.tmpGridActiveListUpdate.Clear();
    this.data.MaxPutHeight.Clear();
  }

  public void Load(GridPool Grid, string loadpath)
  {
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    FileStream fileStream = new FileStream(loadpath, FileMode.Open, FileAccess.Read);
    Encoding utF8 = Encoding.UTF8;
    BinaryReader br = new BinaryReader((Stream) fileStream, utF8);
    if (br != null)
    {
      PngFile.SkipPng(br);
      int num1 = (int) br.ReadChar();
      char ch = br.ReadChar();
      if (!ch.Equals('P'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      this.data.MaxFloorNum = br.ReadInt32();
      this.data.nPutPartsNum = br.ReadInt32();
      Vector3 vector3;
      while (!ch.Equals('P'))
      {
        vector3.x = (__Null) br.ReadDouble();
        vector3.y = (__Null) br.ReadDouble();
        vector3.z = (__Null) br.ReadDouble();
        this.data.GridPos.Add(vector3);
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('P'))
          fileStream.Seek(-2L, SeekOrigin.Current);
      }
      int num3 = (int) br.ReadChar();
      ch = br.ReadChar();
      if (!ch.Equals('U'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      while (!ch.Equals('U'))
      {
        this.data.GridUseState.Add(br.ReadBoolean());
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('U'))
          fileStream.Seek(-2L, SeekOrigin.Current);
      }
      int num4 = (int) br.ReadChar();
      ch = br.ReadChar();
      if (!ch.Equals('F'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      while (!ch.Equals('F'))
      {
        this.data.nFloorPartsHeight.Add(br.ReadInt32());
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('F'))
          fileStream.Seek(-2L, SeekOrigin.Current);
      }
      int num5 = (int) br.ReadChar();
      ch = br.ReadChar();
      if (!ch.Equals('S'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      int index1 = 0;
      int index2 = 0;
      int num6 = 0;
      this.data.SmallGridState.Add(new List<List<int>>());
      this.data.SmallGridOnParts.Add(new List<List<int[]>>());
      this.data.SmallGridOnStackWall.Add(new List<List<int[]>>());
      this.data.SmallGridCanRoofState.Add(new List<List<int>>());
      this.data.SmallGridInRoomState.Add(new List<List<bool>>());
      this.data.SmallGridState[index1].Add(new List<int>());
      this.data.SmallGridOnParts[index1].Add(new List<int[]>());
      this.data.SmallGridOnStackWall[index1].Add(new List<int[]>());
      this.data.SmallGridCanRoofState[index1].Add(new List<int>());
      this.data.SmallGridInRoomState[index1].Add(new List<bool>());
      while (!ch.Equals('S'))
      {
        this.data.SmallGridState[index1][index2].Add(br.ReadInt32());
        int[] numArray1 = new int[7];
        for (int index3 = 0; index3 < numArray1.Length; ++index3)
          numArray1[index3] = br.ReadInt32();
        this.data.SmallGridOnParts[index1][index2].Add(numArray1);
        int[] numArray2 = new int[9];
        for (int index3 = 0; index3 < numArray2.Length; ++index3)
          numArray2[index3] = br.ReadInt32();
        this.data.SmallGridOnStackWall[index1][index2].Add(numArray2);
        this.data.SmallGridCanRoofState[index1][index2].Add(br.ReadInt32());
        this.data.SmallGridInRoomState[index1][index2].Add(br.ReadBoolean());
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('S'))
          fileStream.Seek(-2L, SeekOrigin.Current);
        ++num6;
        if (num6 == 4)
        {
          num6 = 0;
          ++index2;
          if (index2 != this.data.MaxFloorNum)
          {
            this.data.SmallGridState[index1].Add(new List<int>());
            this.data.SmallGridOnParts[index1].Add(new List<int[]>());
            this.data.SmallGridOnStackWall[index1].Add(new List<int[]>());
            this.data.SmallGridCanRoofState[index1].Add(new List<int>());
            this.data.SmallGridInRoomState[index1].Add(new List<bool>());
          }
          if (index2 == this.data.MaxFloorNum)
          {
            index2 = 0;
            ++index1;
            this.data.SmallGridState.Add(new List<List<int>>());
            this.data.SmallGridOnParts.Add(new List<List<int[]>>());
            this.data.SmallGridOnStackWall.Add(new List<List<int[]>>());
            this.data.SmallGridCanRoofState.Add(new List<List<int>>());
            this.data.SmallGridInRoomState.Add(new List<List<bool>>());
            this.data.SmallGridState[index1].Add(new List<int>());
            this.data.SmallGridOnParts[index1].Add(new List<int[]>());
            this.data.SmallGridOnStackWall[index1].Add(new List<int[]>());
            this.data.SmallGridCanRoofState[index1].Add(new List<int>());
            this.data.SmallGridInRoomState[index1].Add(new List<bool>());
          }
        }
      }
      int num7 = (int) br.ReadChar();
      ch = br.ReadChar();
      if (!ch.Equals('B'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      while (!ch.Equals('B'))
      {
        this.data.BuildPartsGridKind.Add(br.ReadInt32());
        this.data.BuildPartsKind.Add(br.ReadInt32());
        this.data.BuildPartsFloor.Add(br.ReadInt32());
        vector3.x = (__Null) br.ReadDouble();
        vector3.y = (__Null) br.ReadDouble();
        vector3.z = (__Null) br.ReadDouble();
        this.data.BuildPartsPos.Add(vector3);
        Quaternion quaternion;
        quaternion.x = (__Null) br.ReadDouble();
        quaternion.y = (__Null) br.ReadDouble();
        quaternion.z = (__Null) br.ReadDouble();
        quaternion.w = (__Null) br.ReadDouble();
        this.data.BuildPartsRot.Add(quaternion);
        this.data.BuildPartsPutGridInfosNum.Add(br.ReadInt32());
        intList1.Clear();
        intList2.Clear();
        for (int index3 = 0; index3 < this.data.BuildPartsPutGridInfosNum[this.data.BuildPartsPutGridInfosNum.Count - 1]; ++index3)
        {
          intList1.Add(br.ReadInt32());
          intList2.Add(br.ReadInt32());
        }
        this.data.BuildPartsPutGridInfos.Add(intList1);
        this.data.BuildPartsPutSmallGridInfos.Add(intList2);
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('B'))
          fileStream.Seek(-2L, SeekOrigin.Current);
      }
      int num8 = (int) br.ReadChar();
      ch = br.ReadChar();
      if (!ch.Equals('A'))
        fileStream.Seek(-2L, SeekOrigin.Current);
      bool[] flagArray = new bool[this.data.GridPos.Count];
      while (!ch.Equals('A'))
      {
        for (int index3 = 0; index3 < flagArray.Length; ++index3)
          flagArray[index3] = br.ReadBoolean();
        this.data.tmpGridActiveList.Add(flagArray);
        this.data.tmpGridActiveListUpdate.Add(br.ReadBoolean());
        this.data.MaxPutHeight.Add(br.ReadInt32());
        int num2 = (int) br.ReadChar();
        ch = br.ReadChar();
        if (!ch.Equals('A'))
          fileStream.Seek(-2L, SeekOrigin.Current);
      }
      br.Close();
    }
    if (this.data.SmallGridState[this.data.SmallGridState.Count - 1][this.data.SmallGridState[this.data.SmallGridState.Count - 1].Count - 1].Count == 0)
    {
      this.data.SmallGridState.RemoveAt(this.data.SmallGridState.Count - 1);
      this.data.SmallGridOnParts.RemoveAt(this.data.SmallGridOnParts.Count - 1);
      this.data.SmallGridOnStackWall.RemoveAt(this.data.SmallGridOnStackWall.Count - 1);
      this.data.SmallGridCanRoofState.RemoveAt(this.data.SmallGridCanRoofState.Count - 1);
      this.data.SmallGridInRoomState.RemoveAt(this.data.SmallGridInRoomState.Count - 1);
    }
    Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = this.data.MaxFloorNum;
    Singleton<CraftCommandListBaseObject>.Instance.nPutPartsNum = this.data.nPutPartsNum;
    List<GameObject> list1 = Grid.GetList();
    List<GridInfo> Grid1 = new List<GridInfo>();
    using (List<GameObject>.Enumerator enumerator = list1.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        GameObject current = enumerator.Current;
        Grid1.Add((GridInfo) current.GetComponent<GridInfo>());
      }
    }
    int num = this.data.GridPos.Count - list1.Count;
    if (num > 0)
    {
      for (int index = 0; index < num; ++index)
        Grid.Get();
    }
    for (int index1 = 0; index1 < list1.Count; ++index1)
    {
      list1[index1].SetActive(true);
      list1[index1].get_transform().set_localPosition(this.data.GridPos[index1]);
      Grid1[index1].DelFloor(0);
      for (int floorNum = Grid1[index1].GetFloorNum(); floorNum < this.data.MaxFloorNum; ++floorNum)
        Grid1[index1].AddFloor();
      for (int index2 = 0; index2 < this.data.MaxFloorNum; ++index2)
      {
        Grid1[index1].SetUseState(index2, this.data.GridUseState[index2 + Grid1[index1].GetFloorNum() * index1]);
        Grid1[index1].nFloorPartsHeight[index2] = this.data.nFloorPartsHeight[index2];
        for (int index3 = 0; index3 < 4; ++index3)
        {
          for (int index4 = 0; index4 < 7; ++index4)
            Grid1[index1].ChangeSmallGrid(index3, this.data.SmallGridState[index1][index2][index3], this.data.SmallGridOnParts[index1][index2][index3][index4], index2, false);
          for (int index4 = 0; index4 < 5; ++index4)
            Grid1[index1].ChangeSmallGrid(index3, this.data.SmallGridState[index1][index2][index3], this.data.SmallGridOnStackWall[index1][index2][index3][index4], index2, false);
          if (index2 == 0)
            Grid1[index1].ChangeSmallGridColor(index2, index3);
          Grid1[index1].SetCanRoofSmallGrid(index3, index2, this.data.SmallGridCanRoofState[index1][index2][index3]);
          Grid1[index1].SetInRoomSmallGrid(index3, this.data.SmallGridInRoomState[index1][index2][index3], index2);
        }
      }
    }
    for (int floorcnt = 0; floorcnt < this.data.MaxFloorNum; ++floorcnt)
      GridInfo.ChangeGridInfo(Grid1, floorcnt);
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        List<GameObject> list2 = baseParts[index1][index2].GetList();
        for (int index3 = 0; index3 < list2.Count; ++index3)
        {
          if (list2[index3].get_activeSelf())
            list2[index3].SetActive(false);
          if (((BuildPartsInfo) list2[index3].GetComponent<BuildPartsInfo>()).nPutFloor != -1)
            ((BuildPartsInfo) list2[index3].GetComponent<BuildPartsInfo>()).nPutFloor = -1;
        }
        baseParts[index1][index2].ReserveListDel(0, 1);
      }
    }
    for (int index1 = 0; index1 < this.data.BuildPartsPos.Count; ++index1)
    {
      if (gameObjectList != baseParts[this.data.BuildPartsGridKind[index1]][this.data.BuildPartsKind[index1]].GetList())
        gameObjectList = baseParts[this.data.BuildPartsGridKind[index1]][this.data.BuildPartsKind[index1]].GetList();
      int ID = -1;
      baseParts[this.data.BuildPartsGridKind[index1]][this.data.BuildPartsKind[index1]].Get(ref ID);
      BuildPartsInfo component = (BuildPartsInfo) gameObjectList[ID].GetComponent<BuildPartsInfo>();
      gameObjectList[ID].SetActive(true);
      gameObjectList[ID].get_transform().set_localPosition(this.data.BuildPartsPos[index1]);
      gameObjectList[ID].get_transform().set_localRotation(this.data.BuildPartsRot[index1]);
      component.nPutFloor = this.data.BuildPartsFloor[index1];
      component.putGridInfos.Clear();
      component.putSmallGridInfos.Clear();
      for (int index2 = 0; index2 < this.data.BuildPartsPutGridInfos.Count; ++index2)
      {
        component.putGridInfos.Add(Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[this.data.BuildPartsPutGridInfos[index1][index2]]);
        component.putSmallGridInfos.Add(this.data.BuildPartsPutSmallGridInfos[index1][index2]);
      }
    }
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList = this.data.tmpGridActiveList;
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate = this.data.tmpGridActiveListUpdate;
    Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight = this.data.MaxPutHeight;
    this.data.MaxFloorNum = 0;
    this.data.nPutPartsNum = 0;
    this.data.GridPos.Clear();
    this.data.GridUseState.Clear();
    this.data.nFloorPartsHeight.Clear();
    this.data.SmallGridState.Clear();
    this.data.SmallGridOnParts.Clear();
    this.data.SmallGridOnStackWall.Clear();
    this.data.SmallGridCanRoofState.Clear();
    this.data.SmallGridInRoomState.Clear();
    this.data.BuildPartsGridKind.Clear();
    this.data.BuildPartsKind.Clear();
    this.data.BuildPartsFloor.Clear();
    this.data.BuildPartsPos.Clear();
    this.data.BuildPartsRot.Clear();
    this.data.BuildPartsPutGridInfos.Clear();
    this.data.BuildPartsPutSmallGridInfos.Clear();
    this.data.BuildPartsPutGridInfosNum.Clear();
    this.data.tmpGridActiveList.Clear();
    this.data.tmpGridActiveListUpdate.Clear();
    this.data.MaxPutHeight.Clear();
  }

  private byte[] CreatePngScreen(int _width, int _height)
  {
    Texture2D texture2D = new Texture2D(_width, _height, (TextureFormat) 3, false);
    int num = QualitySettings.get_antiAliasing() != 0 ? QualitySettings.get_antiAliasing() : 1;
    RenderTexture temporary = RenderTexture.GetTemporary(_width, _height, 24, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, num);
    Graphics.Blit((Texture) this.gameScreenShotAssist.rtCamera, temporary);
    RenderTexture.set_active(temporary);
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) _width, (float) _height), 0, 0);
    texture2D.Apply();
    RenderTexture.set_active((RenderTexture) null);
    byte[] png = ImageConversion.EncodeToPNG(texture2D);
    RenderTexture.ReleaseTemporary(temporary);
    return png;
  }
}
