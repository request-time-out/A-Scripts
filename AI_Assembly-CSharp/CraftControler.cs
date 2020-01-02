// Decompiled with JetBrains decompiler
// Type: CraftControler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class CraftControler : MonoBehaviour
{
  public GameObject GridPart;
  public Transform CreateGridPlace;
  public BuildPartsMgr partsMgr;
  public Transform CraftSet;
  public Carsol carsol;
  public Button AllReset;
  public Toggle CamLock;
  public Toggle SelectMode;
  public int AreaNumX;
  public int AreaNumY;
  public int AreaNumZ;
  public GameObject FloorUpTex;
  public CraftCamera Cam;
  public CraftItemButtonUI craftItemButtonUI;
  public CraftInfomationUI craftInfomationUI;
  public CraftItemBox craftItem;
  public CraftSelectPartsInfo craftSelectPartsInfo;
  public bool bFloorLimit;
  public bool bOparate;
  private Input input;
  private List<GridInfo> BaseGridInfo;
  private BoxRange BoxCastRange;
  private GridPool Gridpool;
  private List<GameObject> GridList;
  private List<GameObject> CarsolUnderGridList;
  private List<int> CarsolUnderSmallGridIDList;
  private int AllGridNum;
  private Vector3[] vGridPos;
  private Vector3 MinPos;
  private float fGridSize;
  private GridMap gridMap;
  private CraftSave save;
  private int nCarsolMode;
  private bool ViewMode;
  private bool prevViewMode;
  private List<ValueTuple<GameObject, int, float, GameObject>> SelectObj;
  private int nPartsForm;
  private int nID;
  private float fMoveCarsolTimelLimiter;
  private float fFloorUpTexExist;
  private CraftCommandList.ChangeValGrid[] SelectPutGridInfo;
  private List<CraftCommandList.ChangeValParts> SelectPutPartInfo;
  private List<CraftCommandList.ChangeValParts> SelectPutAutoPartInfo;
  private int[] SelectPutMaxFloorCnt;
  private int PutHeight;
  private const int MAX_FLOOR_NUM = 3;
  private const int PreGridMaxNum = 100;
  public const int FloorHeight = 5;

  public CraftControler()
  {
    base.\u002Ector();
  }

  public int _nPartsForm
  {
    get
    {
      return this.nPartsForm;
    }
    set
    {
      this.nPartsForm = value;
      this.ChangeKindPart(this.nPartsForm);
    }
  }

  public int _nID
  {
    get
    {
      return this.nID;
    }
    set
    {
      this.nID = value;
    }
  }

  private int nPutPartsNum
  {
    set
    {
      this.craftInfomationUI.nCost = value;
      Singleton<CraftCommandListBaseObject>.Instance.nPutPartsNum = value;
    }
    get
    {
      return this.craftInfomationUI.nCost;
    }
  }

  private void Start()
  {
    this.Init();
  }

  private void Init()
  {
    this.input = Singleton<Input>.Instance;
    this.MinPos = (Vector3) null;
    this.nPartsForm = 0;
    this.fGridSize = 1f;
    this.GridInit();
    this.carsol.Init(this.MinPos, this.fGridSize);
    if (Object.op_Inequality((Object) this.AllReset, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.AllReset.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(CraftAllReset)));
    }
    this.save = Singleton<CraftSave>.Instance;
    this.save.Init();
    if (Object.op_Inequality((Object) Singleton<SaveLoadUI>.Instance.save, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) Singleton<SaveLoadUI>.Instance.save.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(SaveCraft)));
    }
    if (Object.op_Inequality((Object) Singleton<SaveLoadUI>.Instance.dataLoad, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) Singleton<SaveLoadUI>.Instance.dataLoad.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(LoadCraft)));
    }
    this.nID = 0;
    this.gridMap = new GridMap();
    this.gridMap.Init(this.GridList, this.AreaNumX, this.AreaNumZ);
    Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt = 0;
    Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = 1;
    this.nCarsolMode = 0;
    this.ViewMode = false;
    this.prevViewMode = false;
    this.fMoveCarsolTimelLimiter = 0.0f;
    this.PutHeight = 0;
    for (int index = 0; index < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++index)
      Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight.Add(0);
    if (!this.bFloorLimit)
    {
      this.Cam.isLimitDir = false;
      this.Cam.isLimitPos = false;
    }
    this.nPutPartsNum = 0;
  }

  private void GridInit()
  {
    this.AllGridNum = this.AreaNumX * this.AreaNumY * this.AreaNumZ;
    this.Gridpool = new GridPool();
    if (100 > this.AllGridNum)
      this.Gridpool.CreatePool(this.GridPart, this.CreateGridPlace, this.AllGridNum);
    else
      this.Gridpool.CreatePool(this.GridPart, this.CreateGridPlace, 100);
    this.vGridPos = new Vector3[this.AllGridNum];
    float num1 = (float) (-(double) this.fGridSize * (double) this.AreaNumX / 2.0 + 1.0 + this.CraftSet.get_position().x);
    float num2 = (float) (-(double) this.fGridSize * (double) this.AreaNumZ / 2.0 + 1.0 + this.CraftSet.get_position().z);
    this.MinPos.x = (__Null) (double) num1;
    this.MinPos.y = this.CraftSet.get_position().y;
    this.MinPos.z = (__Null) (double) num2;
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList = new List<bool[]>();
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate = new List<bool>();
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList.Add(new bool[this.AllGridNum]);
    for (int index = 0; index < this.AllGridNum; ++index)
    {
      Vector3 vector3;
      vector3.x = (__Null) (this.MinPos.x + (double) this.fGridSize * (double) (index % this.AreaNumX));
      vector3.y = (__Null) (this.MinPos.y + (double) this.fGridSize * (double) (index / (this.AreaNumX * this.AreaNumZ)));
      vector3.z = (__Null) (this.MinPos.z + (double) this.fGridSize * (double) (index / this.AreaNumX % this.AreaNumZ));
      GameObject gameObject = this.Gridpool.Get();
      GridInfo component = (GridInfo) gameObject.GetComponent<GridInfo>();
      gameObject.SetActive(true);
      gameObject.get_transform().set_localPosition(vector3);
      component.Init(0);
      component.InitPos = vector3;
      component.nID = index;
      component.SetUseState(0, true);
      this.vGridPos[index] = gameObject.get_transform().get_localPosition();
      Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[0][index] = true;
    }
    Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate.Add(true);
    this.GridList = this.Gridpool.GetList();
    for (int index = 0; index < this.AllGridNum; ++index)
    {
      this.MinPos.x = (__Null) (double) Mathf.Min((float) this.MinPos.x, (float) this.GridList[index].get_transform().get_localPosition().x);
      this.MinPos.z = (__Null) (double) Mathf.Min((float) this.MinPos.z, (float) this.GridList[index].get_transform().get_localPosition().z);
    }
    Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo = new List<GridInfo>();
    using (List<GameObject>.Enumerator enumerator = this.GridList.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo.Add((GridInfo) enumerator.Current.GetComponent<GridInfo>());
    }
    this.BaseGridInfo = Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo;
  }

  private void Update()
  {
    if (!Singleton<CraftResource>.Instance.bEnd)
      return;
    if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts == null)
    {
      this.partsMgr.Init();
      Singleton<CraftCommandListBaseObject>.Instance.BaseParts = this.partsMgr.GetPool();
      this.craftItemButtonUI.CreateCategoryButton();
      for (int index = 0; index < this.craftItemButtonUI.ItemKind.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CraftControler.\u003CUpdate\u003Ec__AnonStorey0 updateCAnonStorey0 = new CraftControler.\u003CUpdate\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        updateCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        updateCAnonStorey0.tmpID = index;
        // ISSUE: reference to a compiler-generated field
        if (Object.op_Inequality((Object) this.craftItemButtonUI.ItemKind[updateCAnonStorey0.tmpID], (Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) this.craftItemButtonUI.ItemKind[updateCAnonStorey0.tmpID].get_onClick()).AddListener(new UnityAction((object) updateCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          EventSystem.get_current().SetSelectedGameObject((GameObject) null);
        }
      }
      this.carsol.SetMoveLimit(this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.MoveVal);
      this.craftItem.EndLoad = true;
    }
    else if (Object.op_Inequality((Object) EventSystem.get_current(), (Object) null) && !EventSystem.get_current().IsPointerOverGameObject())
    {
      Vector3 position = ((Component) this.carsol).get_transform().get_position();
      if ((double) this.fMoveCarsolTimelLimiter == 0.0)
      {
        if (!this.carsol.bMouseOperate)
        {
          if (!this.craftItemButtonUI.isActive && this.Cam.bLock)
          {
            if (this.input.IsPressedKey((KeyCode) 273))
              this.carsol.MoveCarsol(0);
            else if (this.input.IsPressedKey((KeyCode) 275))
              this.carsol.MoveCarsol(1);
            else if (this.input.IsPressedKey((KeyCode) 274))
              this.carsol.MoveCarsol(2);
            else if (this.input.IsPressedKey((KeyCode) 276))
              this.carsol.MoveCarsol(3);
          }
        }
        else
          this.carsol.MoveCarsol(-1);
        this.CarsolOffset();
      }
      this.fMoveCarsolTimelLimiter += Time.get_deltaTime();
      if ((double) this.fMoveCarsolTimelLimiter > 0.0500000007450581)
        this.fMoveCarsolTimelLimiter = 0.0f;
      int num = this.carsol.GetDirection();
      bool flag1 = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count == 0 || (double) this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.MoveVal != 1.0;
      if (this.partsMgr.BuildPartPoolDic[this.nPartsForm].Count > 0 && (this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Itemkind == 10 || this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Itemkind == 13))
        flag1 = false;
      Quaternion localRotation = ((Component) this.carsol).get_transform().get_localRotation();
      if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count != 0 && Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].GetCategoryKind() != 1)
      {
        if (this.input.IsPressedKey((KeyCode) 113))
        {
          if (flag1)
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -45f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            --num;
          }
          else
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -90f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            num -= 2;
          }
        }
        else if (this.input.IsPressedKey((KeyCode) 101))
        {
          if (flag1)
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 45f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            ++num;
          }
          else
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            num += 2;
          }
        }
        if (this.Cam.bLock)
        {
          if (flag1)
          {
            if ((double) this.input.ScrollValue() > 0.0)
            {
              ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -45f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
              --num;
            }
            else if ((double) this.input.ScrollValue() < 0.0)
            {
              ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 45f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
              ++num;
            }
          }
          else if ((double) this.input.ScrollValue() > 0.0)
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -90f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            num -= 2;
          }
          else if ((double) this.input.ScrollValue() < 0.0)
          {
            ((Component) this.carsol).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this.carsol).get_transform().get_localRotation()));
            num += 2;
          }
        }
        if (num < 0)
          num = !flag1 ? 6 : 7;
        else if (num == 8)
          num = 0;
        this.carsol.SetDirection(num);
      }
      if (Vector3.op_Inequality(((Component) this.carsol).get_transform().get_position(), position) || Quaternion.op_Inequality(((Component) this.carsol).get_transform().get_localRotation(), localRotation))
        this.CarsolUnderCheck();
      if (this.Cam.bLock && (this.carsol.bMouseOperate || !this.craftItemButtonUI.isActive && !this.carsol.bMouseOperate))
      {
        if (this.nCarsolMode == 0)
        {
          if (this.input.IsPressedKey((KeyCode) 120) || this.input.IsPressedKey(ActionID.MouseLeft))
            this.PutBuldPart(((Component) this.carsol).get_gameObject(), num);
          else if (this.input.IsPressedKey((KeyCode) 122) || this.input.IsPressedKey(ActionID.MouseRight))
            this.DeadBuildPart();
        }
        else if (this.nCarsolMode == 1)
        {
          if (this.input.IsPressedKey((KeyCode) 120) || this.input.IsPressedKey(ActionID.MouseLeft))
            this.SelectBuldPart();
          if (this.SelectObj.Count > 0)
          {
            List<CraftCommandList.ChangeValParts> valAutoParts = new List<CraftCommandList.ChangeValParts>();
            ++this.nCarsolMode;
            this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            if (this.BaseGridInfo[0].GetFloorNum() > Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 && !this.gridMap.CraftMapRoofDecide() && this.PillarOnGridNum() == 0)
              this.AllFloorDel(valAutoParts);
            this.FloorDel(valAutoParts);
            this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          }
        }
        else if (this.nCarsolMode == 2)
        {
          bool flag2 = false;
          if (this.input.IsPressedKey((KeyCode) 120) || this.input.IsPressedKey(ActionID.MouseLeft))
            flag2 = this.SelectPut();
          if (flag2)
          {
            this.carsol.SelectModeCarsolUnvisible();
            this.carsol.ResetCursolDirPos();
            this.nCarsolMode = 1;
            this.SelectObj.Clear();
            this.URPutPartsSetG(ref this.SelectPutGridInfo, 1);
            this.SelectPutMaxFloorCnt[1] = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
            Singleton<UndoRedoMgr>.Instance.Push((ICommand) new CraftCommandList.SelectBuildPartCommand.Command(this.SelectPutGridInfo, this.SelectPutPartInfo.ToArray(), this.SelectPutAutoPartInfo.ToArray(), this.SelectPutMaxFloorCnt));
          }
        }
      }
      if (!Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])
        this.TmpGridActiveUpdate();
      if (this.nCarsolMode == 0 && this.input.IsPressedKey((KeyCode) 119))
        this.OpelateFloorUp();
      if (this.input.IsPressedKey((KeyCode) 99) && !this.ViewMode)
      {
        Toggle camLock = this.CamLock;
        camLock.set_isOn(((camLock.get_isOn() ? 1 : 0) ^ 1) != 0);
        this.craftItemButtonUI.isActive = this.CamLock.get_isOn();
      }
      if (this.nCarsolMode == 0)
      {
        if (this.input.IsPressedKey((KeyCode) 97))
        {
          ++this.nPartsForm;
          this.ChangeKindPart(this.nPartsForm);
          this.ChangeParts();
        }
        else if (this.input.IsPressedKey((KeyCode) 115))
        {
          --this.nPartsForm;
          this.ChangeKindPart(this.nPartsForm);
          this.ChangeParts();
        }
        if (this.input.IsPressedKey((KeyCode) 100))
        {
          ++this.nID;
          this.nID %= Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count;
          this.ChangeParts();
        }
      }
      if (this.input.IsPressedKey((KeyCode) 118))
      {
        if (this.nCarsolMode == 0)
        {
          this.nCarsolMode = 1;
          this.SelectMode.set_isOn(true);
        }
        else if (this.nCarsolMode == 1)
        {
          this.nCarsolMode = 0;
          this.SelectObj.Clear();
          this.SelectMode.set_isOn(false);
        }
      }
      if (this.SelectMode.get_isOn())
      {
        if (this.nCarsolMode == 0)
          this.nCarsolMode = 1;
      }
      else if (this.nCarsolMode == 2)
        this.SelectMode.set_isOn(true);
      else if (this.nCarsolMode == 1)
        this.nCarsolMode = 0;
    }
    if (this.craftInfomationUI.GetWarningActive() && !this.craftInfomationUI.bFade)
    {
      this.craftInfomationUI.fWarningExist += Time.get_deltaTime();
      if ((double) this.craftInfomationUI.fWarningExist > 1.5)
        this.craftInfomationUI.bFade = true;
    }
    if (this.FloorUpTex.get_activeSelf())
    {
      this.fFloorUpTexExist += Time.get_deltaTime();
      if ((double) this.fFloorUpTexExist > 1.5)
      {
        this.fFloorUpTexExist = 0.0f;
        this.FloorUpTex.SetActive(false);
      }
    }
    if (this.craftItemButtonUI.isActive)
    {
      foreach (MenuUIBehaviour menuElement in this.input.MenuElements)
      {
        if (this.input.IsPressedKey((KeyCode) 274))
        {
          if (menuElement.EnabledInput && menuElement.FocusLevel == this.input.FocusLevel)
            menuElement.OnInputMoveDirection((MoveDirection) 3);
        }
        else if (this.input.IsPressedKey((KeyCode) 273) && menuElement.EnabledInput && menuElement.FocusLevel == this.input.FocusLevel)
          menuElement.OnInputMoveDirection((MoveDirection) 1);
      }
    }
    if (this.craftItem.isActive)
    {
      foreach (MenuUIBehaviour menuElement in this.input.MenuElements)
      {
        if (this.input.IsPressedKey((KeyCode) 275))
        {
          if (menuElement.EnabledInput && menuElement.FocusLevel == this.input.FocusLevel)
            menuElement.OnInputMoveDirection((MoveDirection) 2);
        }
        else if (this.input.IsPressedKey((KeyCode) 276) && menuElement.EnabledInput && menuElement.FocusLevel == this.input.FocusLevel)
          menuElement.OnInputMoveDirection((MoveDirection) 0);
      }
    }
    if (this.input.IsPressedKey((KeyCode) 103))
    {
      Carsol carsol = this.carsol;
      carsol.bMouseOperate = ((carsol.bMouseOperate ? 1 : 0) ^ 1) != 0;
    }
    if ((this.input.IsDown((KeyCode) 304) || this.input.IsDown((KeyCode) 303)) && this.input.IsPressedKey((KeyCode) 122))
      this.Undo();
    else if ((this.input.IsDown((KeyCode) 308) || this.input.IsDown((KeyCode) 307)) && this.input.IsPressedKey((KeyCode) 122))
      this.Redo();
    else if (this.input.IsPressedKey((KeyCode) 98))
      Singleton<UndoRedoMgr>.Instance.Clear();
    if ((this.input.IsPressedKey((KeyCode) 306) || this.input.IsPressedKey((KeyCode) 305)) && this.input.IsPressedKey((KeyCode) 97))
      this.SaveCraft();
    if (!this.Cam.bLock)
    {
      if (this.input.IsPressedKey((KeyCode) 111))
      {
        CraftControler craftControler = this;
        craftControler.ViewMode = ((craftControler.ViewMode ? 1 : 0) ^ 1) != 0;
      }
      if (this.ViewMode)
        ((Selectable) this.CamLock).set_interactable(false);
      else
        ((Selectable) this.CamLock).set_interactable(true);
    }
    if (this.prevViewMode != this.ViewMode)
    {
      this.prevViewMode = this.ViewMode;
      for (int index = 0; index < this.GridList.Count; ++index)
      {
        if (this.ViewMode)
        {
          if (this.GridList[index].get_activeSelf())
            this.GridList[index].SetActive(false);
        }
        else
          this.FloatingObjGrid(this.GridList);
      }
    }
    if (this.input.IsPressedKey((KeyCode) 109))
    {
      ++this.PutHeight;
      this.PutHeight %= 5;
    }
    else
    {
      if (!this.input.IsPressedKey((KeyCode) 110))
        return;
      --this.PutHeight;
      if (this.PutHeight >= 0)
        return;
      this.PutHeight = 4;
    }
  }

  private void DeadBuildPart()
  {
    List<GameObject> tmptarget = new List<GameObject>();
    List<int> tmptargetID = new List<int>();
    List<BuildPartsPool> tmptargetpool = new List<BuildPartsPool>();
    CraftCommandList.ChangeValGrid[] Val = new CraftCommandList.ChangeValGrid[this.AllGridNum];
    List<CraftCommandList.ChangeValParts> changeValPartsList1 = new List<CraftCommandList.ChangeValParts>();
    List<CraftCommandList.ChangeValParts> changeValPartsList2 = new List<CraftCommandList.ChangeValParts>();
    int[] _maxFloorCnt = new int[2]
    {
      Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt,
      0
    };
    GameObject gameObject = (GameObject) null;
    int ID = -1;
    int itemKind = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].GetItemKind();
    this.DecideDelParts(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID], ref tmptarget, ref tmptargetID, ref tmptargetpool, 0);
    if (tmptarget.Count > 0)
    {
      gameObject = tmptarget[0];
      ID = tmptargetID[0];
      for (int index = 1; index < tmptarget.Count; ++index)
      {
        if (gameObject.get_transform().get_localPosition().y <= tmptarget[index].get_transform().get_position().y)
        {
          gameObject = tmptarget[index];
          ID = tmptargetID[index];
        }
      }
    }
    if (Object.op_Equality((Object) gameObject, (Object) null))
      return;
    for (int index = 0; index < Val.Length; ++index)
      Val[index] = new CraftCommandList.ChangeValGrid();
    this.URPutPartsSetG(ref Val, 0);
    List<GridInfo> gridInfoList = new List<GridInfo>();
    List<int> intList = new List<int>();
    this.GridCheck(this.GridList, this.carsol, gridInfoList, intList, gameObject.get_transform().get_localRotation(), this.nPartsForm, new Vector3?());
    int count = gridInfoList.Count;
    if (count == 0 || !this.DelConditionCheck(count, gridInfoList, intList, itemKind, gameObject))
      return;
    this.DedPartsGridChange(count, gridInfoList, intList, itemKind, this.partsMgr.GetBuildPartsInfo(gameObject).nHeight);
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    if (this.BaseGridInfo[0].GetFloorNum() > Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 && !this.gridMap.CraftMapRoofDecide() && this.PillarOnGridNum() == 0)
      this.AllFloorDel(changeValPartsList2);
    this.FloorDel(changeValPartsList2);
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    int num = this.BaseGridInfo.Select<GridInfo, int>((Func<GridInfo, int>) (n => n.nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])).Max();
    this.FloorHeightMove(num - Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt], (float) (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] + 5), changeValPartsList2);
    this.URPutPartsSetG(ref Val, 1);
    Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = num;
    if (itemKind != 6 && itemKind != 3 && itemKind != 4)
    {
      changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
      changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = this.nPartsForm;
      changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = this.nID;
      changeValPartsList1[changeValPartsList1.Count - 1].nItemID = ID;
      this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 0);
      gameObject.SetActive(false);
      ((BuildPartsInfo) gameObject.GetComponent<BuildPartsInfo>()).nPutFloor = -1;
      Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].ReserveListDel(ID, 0);
      --this.nPutPartsNum;
      this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 1);
    }
    else if (itemKind == 3 || itemKind == 4)
    {
      tmptarget.Clear();
      tmptargetID.Clear();
      tmptargetpool.Clear();
      for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
      {
        for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
        {
          switch (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetItemKind())
          {
            case 3:
            case 4:
            case 10:
            case 13:
              this.DecideDelParts(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2], ref tmptarget, ref tmptargetID, ref tmptargetpool, 0);
              break;
          }
        }
      }
      if (itemKind == 3)
      {
        for (int index = 0; index < tmptarget.Count; ++index)
        {
          changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
          changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = tmptargetpool[index].GetFormKind();
          changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(1);
          changeValPartsList1[changeValPartsList1.Count - 1].nItemID = tmptargetID[index];
          this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], tmptarget[index], 0);
          tmptarget[index].SetActive(false);
          ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).nPutFloor = -1;
          tmptargetpool[index].ReserveListDel(tmptargetID[index], 0);
          this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], tmptarget[index], 1);
          --this.nPutPartsNum;
        }
      }
      else
      {
        changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
        changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = this.nPartsForm;
        changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = this.nID;
        changeValPartsList1[changeValPartsList1.Count - 1].nItemID = ID;
        this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 0);
        gameObject.SetActive(false);
        ((BuildPartsInfo) gameObject.GetComponent<BuildPartsInfo>()).nPutFloor = -1;
        Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].ReserveListDel(ID, 0);
        --this.nPutPartsNum;
        this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 1);
        Rect rect1;
        ((Rect) ref rect1).\u002Ector((float) gameObject.get_transform().get_position().x - (float) (this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Horizontal / 2), (float) gameObject.get_transform().get_position().z - (float) (this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Vertical / 2), (float) this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Horizontal, (float) this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Vertical);
        for (int index = 0; index < tmptarget.Count; ++index)
        {
          switch (((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(3))
          {
            case 10:
            case 13:
              int info1 = ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(2);
              int info2 = ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(1);
              int info3 = ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(0);
              CraftItemInfo craftItemInfo = this.partsMgr.BuildPartPoolDic[info1][info2].Item2;
              Rect rect2;
              ((Rect) ref rect2).\u002Ector((float) tmptarget[index].get_transform().get_position().x - (float) (craftItemInfo.Horizontal / 2), (float) tmptarget[index].get_transform().get_position().z - (float) (craftItemInfo.Vertical / 2), (float) craftItemInfo.Horizontal, (float) craftItemInfo.Vertical);
              if (((Rect) ref rect1).Overlaps(rect2, true) && gameObject.get_transform().get_position().y == tmptarget[index].get_transform().get_position().y)
              {
                changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
                changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = info1;
                changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = info2;
                changeValPartsList1[changeValPartsList1.Count - 1].nItemID = info3;
                this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 0);
                tmptarget[index].SetActive(false);
                ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).nPutFloor = -1;
                Singleton<CraftCommandListBaseObject>.Instance.BaseParts[info1][info2].ReserveListDel(info3, 0);
                --this.nPutPartsNum;
                this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 1);
                break;
              }
              break;
          }
        }
      }
    }
    else
    {
      tmptarget.Clear();
      tmptargetID.Clear();
      tmptargetpool.Clear();
      for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
      {
        for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
        {
          switch (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetItemKind())
          {
            case 3:
            case 4:
            case 10:
            case 13:
              this.DecideDelParts(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2], ref tmptarget, ref tmptargetID, ref tmptargetpool, 0);
              break;
          }
        }
      }
      changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
      changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = this.nPartsForm;
      changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = this.nID;
      changeValPartsList1[changeValPartsList1.Count - 1].nItemID = ID;
      this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 0);
      gameObject.SetActive(false);
      ((BuildPartsInfo) gameObject.GetComponent<BuildPartsInfo>()).nPutFloor = -1;
      Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].ReserveListDel(ID, 0);
      this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject, 1);
      --this.nPutPartsNum;
      for (int index = 0; index < tmptarget.Count; ++index)
      {
        changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
        changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = tmptargetpool[index].GetFormKind();
        changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).GetInfo(1);
        changeValPartsList1[changeValPartsList1.Count - 1].nItemID = tmptargetID[index];
        this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], tmptarget[index], 0);
        tmptarget[index].SetActive(false);
        ((BuildPartsInfo) tmptarget[index].GetComponent<BuildPartsInfo>()).nPutFloor = -1;
        tmptargetpool[index].ReserveListDel(tmptargetID[index], 0);
        this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], tmptarget[index], 1);
        --this.nPutPartsNum;
      }
    }
    _maxFloorCnt[1] = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    Singleton<UndoRedoMgr>.Instance.Push((ICommand) new CraftCommandList.DeadBuildPartCommand.Command(Val, changeValPartsList1.ToArray(), changeValPartsList2.ToArray(), _maxFloorCnt));
  }

  private void PutBuldPart(GameObject Carsol, int nDirection)
  {
    CraftCommandList.ChangeValGrid[] Val = new CraftCommandList.ChangeValGrid[this.AllGridNum];
    List<CraftCommandList.ChangeValParts> changeValPartsList1 = new List<CraftCommandList.ChangeValParts>();
    List<CraftCommandList.ChangeValParts> changeValPartsList2 = new List<CraftCommandList.ChangeValParts>();
    int[] _maxFloorCnt = new int[2]
    {
      Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt,
      0
    };
    for (int index = 0; index < Val.Length; ++index)
      Val[index] = new CraftCommandList.ChangeValGrid();
    this.URPutPartsSetG(ref Val, 0);
    changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
    int ID1 = -1;
    GameObject gameObject1 = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].Get(ref ID1);
    changeValPartsList1[0].nFormKind = this.nPartsForm;
    changeValPartsList1[0].nPoolID = this.nID;
    changeValPartsList1[0].nItemID = ID1;
    this.URPutPartsSetB(changeValPartsList1[0], gameObject1, 0);
    this.partsMgr.GetBuildPartsInfo(gameObject1).SetDirection(nDirection);
    List<DeleatWall> wall = new List<DeleatWall>();
    List<GridInfo> tmpGridInfo = new List<GridInfo>();
    List<int> tmpSmallGridID = new List<int>();
    bool flag1 = this.GridPutCheck(this.GridList, this.carsol, gameObject1, ref wall, ref tmpGridInfo, ref tmpSmallGridID);
    int info = this.partsMgr.GetBuildPartsInfo(gameObject1).GetInfo(3);
    if (flag1)
    {
      this.FloorPartsDel(gameObject1, tmpGridInfo, changeValPartsList2);
      this.ChangeGridInfo(gameObject1, tmpGridInfo, tmpSmallGridID, tmpSmallGridID.Count, changeValPartsList2);
      this.partsMgr.GetBuildPartsInfo(gameObject1).putGridInfos = tmpGridInfo;
      this.partsMgr.GetBuildPartsInfo(gameObject1).putSmallGridInfos = tmpSmallGridID;
      if (info == 6 || info == 16)
      {
        Transform transform = wall[0].Wall.get_transform();
        for (int index = 0; index < wall.Count; ++index)
        {
          changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
          changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = wall[index].PartForm;
          changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = wall[index].PartKind;
          changeValPartsList1[changeValPartsList1.Count - 1].nItemID = wall[index].WallID;
          this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], wall[index].Wall, 0);
          wall[index].Wall.SetActive(false);
          ((BuildPartsInfo) wall[index].Wall.GetComponent<BuildPartsInfo>()).nPutFloor = -1;
          this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], wall[index].Wall, 1);
          --this.nPutPartsNum;
          Singleton<CraftCommandListBaseObject>.Instance.BaseParts[wall[index].PartForm][wall[index].PartKind].ReserveListDel(wall[index].WallID, 0);
        }
        Vector3 position = transform.get_position();
        position.y = (__Null) (((double) wall.Max<DeleatWall>((Func<DeleatWall, float>) (v => (float) v.Wall.get_transform().get_position().y)) - (double) wall.Min<DeleatWall>((Func<DeleatWall, float>) (v => (float) v.Wall.get_transform().get_position().y))) / 2.0);
        transform.set_position(position);
        this.SetParts(transform, wall[0].Wall.get_transform().get_rotation(), gameObject1, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Horizontal, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Vertical, 1);
      }
      else
      {
        Vector3 position = Carsol.get_transform().get_position();
        if (info == 3 || info == 4 || info == 15)
        {
          for (int index1 = 0; index1 < tmpGridInfo.Count; ++index1)
          {
            if (tmpGridInfo[index1].GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == 1)
            {
              bool flag2 = true;
              List<GameObject> list = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[0][0].GetList();
              for (int index2 = 0; index2 < list.Count; ++index2)
              {
                if (list[index2].get_activeSelf() && ((BuildPartsInfo) list[index2].GetComponent<BuildPartsInfo>()).nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt && list[index2].get_transform().get_position().x == ((Component) tmpGridInfo[index1]).get_transform().get_position().x && list[index2].get_transform().get_position().z == ((Component) tmpGridInfo[index1]).get_transform().get_position().z)
                {
                  flag2 = false;
                  break;
                }
              }
              if (flag2)
              {
                int ID2 = -1;
                GameObject gameObject2 = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[0][0].Get(ref ID2);
                changeValPartsList1.Add(new CraftCommandList.ChangeValParts());
                changeValPartsList1[changeValPartsList1.Count - 1].nFormKind = 0;
                changeValPartsList1[changeValPartsList1.Count - 1].nPoolID = 0;
                changeValPartsList1[changeValPartsList1.Count - 1].nItemID = ID2;
                this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject2, 0);
                this.SetParts(((Component) tmpGridInfo[index1]).get_transform(), Carsol.get_transform().get_rotation(), gameObject2, 1, 1, 1);
                gameObject2.SetActive(true);
                this.URPutPartsSetB(changeValPartsList1[changeValPartsList1.Count - 1], gameObject2, 1);
              }
            }
            if (this.partsMgr.GetBuildPartsInfo(gameObject1).GetInfo(3) == 4)
            {
              int num = 0;
              int[] stackWallOnSmallGrid = tmpGridInfo[index1].GetStackWallOnSmallGrid(tmpSmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
              for (int index2 = 0; index2 < stackWallOnSmallGrid.Length && stackWallOnSmallGrid[index2] != -1; ++index2)
                ++num;
            }
          }
          Carsol.get_transform().set_position(position);
          this.SetParts(Carsol.get_transform(), Carsol.get_transform().get_rotation(), gameObject1, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Horizontal, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Vertical, 0);
          ref Vector3 local = ref position;
          local.y = (__Null) (local.y + (double) this.partsMgr.GetBuildPartsInfo(gameObject1).nHeight);
          Carsol.get_transform().set_position(position);
        }
        else
        {
          this.SetParts(Carsol.get_transform(), Carsol.get_transform().get_rotation(), gameObject1, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Horizontal, this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Vertical, 0);
          Carsol.get_transform().set_position(position);
        }
      }
      gameObject1.SetActive(true);
      this.CheckUnderRoof();
    }
    else
    {
      ((BuildPartsInfo) gameObject1.GetComponent<BuildPartsInfo>()).nPutFloor = -1;
      Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].ReserveListDel(ID1, 0);
    }
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    int num1 = this.PillarOnGridNum();
    if (this.gridMap.CraftMapRoofDecide() || info == 15 && num1 == 1)
      this.FloorUP(changeValPartsList2);
    this.URPutPartsSetG(ref Val, 1);
    this.URPutPartsSetB(changeValPartsList1[0], gameObject1, 1);
    _maxFloorCnt[1] = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    if (!flag1)
      return;
    Singleton<UndoRedoMgr>.Instance.Push((ICommand) new CraftCommandList.PutBuildPartCommand.Command(Val, changeValPartsList1.ToArray(), changeValPartsList2.ToArray(), _maxFloorCnt));
  }

  private bool GridPutCheck(
    List<GameObject> GridList,
    Carsol carsol,
    GameObject buildPart,
    ref List<DeleatWall> wall,
    ref List<GridInfo> tmpGridInfo,
    ref List<int> tmpSmallGridID)
  {
    this.GridCheck(GridList, carsol, tmpGridInfo, tmpSmallGridID, ((Component) carsol).get_transform().get_rotation(), this.nPartsForm, new Vector3?());
    return tmpSmallGridID.Count != 0 && this.PutCheck(tmpGridInfo, tmpSmallGridID, buildPart, carsol, ref wall) && ((double) this.BoxCastRange.MinX >= this.MinPos.x - (double) this.fGridSize / 2.0 && (double) this.BoxCastRange.MaxX <= this.MinPos.x - (double) this.fGridSize / 2.0 + (double) this.fGridSize * (double) this.AreaNumX) && ((double) this.BoxCastRange.MinZ >= this.MinPos.z - (double) this.fGridSize / 2.0 && (double) this.BoxCastRange.MaxZ <= this.MinPos.z - (double) this.fGridSize / 2.0 + (double) this.fGridSize * (double) this.AreaNumZ);
  }

  private void FloorPartsDel(
    GameObject buildPart,
    List<GridInfo> tmpGridInfo,
    List<CraftCommandList.ChangeValParts> AutoFloor)
  {
    if (this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(3) == 9)
    {
      this.PartsOnStairsDel(tmpGridInfo, AutoFloor);
    }
    else
    {
      if (this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(3) != 11)
        return;
      this.PartsOnElevatorDel(tmpGridInfo, AutoFloor);
    }
  }

  private void GridCheck(
    List<GameObject> GridList,
    Carsol carsol,
    List<GridInfo> tmpGridInfo,
    List<int> tmpSmallGridID,
    Quaternion Rayrot,
    int nkind,
    Vector3? pos = null)
  {
    bool[] flagArray = new bool[GridList.Count];
    for (int index = 0; index < GridList.Count; ++index)
    {
      flagArray[index] = GridList[index].get_activeSelf();
      GridList[index].SetActive(true);
    }
    this.GridCheck(carsol.CheckCarsol(((Component) carsol).get_transform().get_rotation(), Rayrot, nkind, ref this.BoxCastRange, pos), GridList, tmpGridInfo, tmpSmallGridID);
    for (int index = 0; index < GridList.Count; ++index)
      GridList[index].SetActive(flagArray[index]);
  }

  private void GridCheck(
    List<RaycastHit[]> HitsObjInfo,
    List<GameObject> GridList,
    List<GridInfo> GridInfo,
    List<int> SmallGridID)
  {
    for (int index1 = 0; index1 < GridList.Count; ++index1)
    {
      Transform[] componentsInChildren = (Transform[]) GridList[index1].GetComponentsInChildren<Transform>();
      GridInfo Gridinfo = this.BaseGridInfo[index1];
      for (int index2 = 0; index2 < HitsObjInfo.Count; ++index2)
      {
        for (int index3 = 0; index3 < HitsObjInfo[index2].Length; ++index3)
          this.GridCheck(((RaycastHit) ref HitsObjInfo[index2][index3]).get_transform(), componentsInChildren, GridInfo, SmallGridID, Gridinfo);
      }
    }
  }

  private void GridCheck(
    Transform HitsObjInfo,
    Transform[] smallGrids,
    List<GridInfo> gridInfos,
    List<int> SmallGridID,
    GridInfo Gridinfo)
  {
    for (int index = 1; index < smallGrids.Length; ++index)
    {
      if (Vector3.op_Equality(HitsObjInfo.get_position(), smallGrids[index].get_position()))
      {
        gridInfos.Add(Gridinfo);
        SmallGridID.Add(index - 1);
      }
    }
  }

  private bool PutCheck(
    List<GridInfo> gridInfo,
    List<int> SmallGridID,
    GameObject buildPart,
    Carsol carsol,
    ref List<DeleatWall> wall)
  {
    int info = this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(3);
    int num1 = gridInfo[0].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt];
    for (int index = 1; index < gridInfo.Count; ++index)
    {
      if (num1 != gridInfo[index].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])
        return false;
    }
    int[] numArray = new int[2]
    {
      ((IEnumerable<int>) gridInfo[0].GetStackPartsOnSmallGrid(SmallGridID[0], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)).Count<int>((Func<int, bool>) (v => v != -1)),
      0
    };
    for (int index1 = 1; index1 < gridInfo.Count; ++index1)
    {
      int[] partsOnSmallGrid = gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      numArray[1] = 0;
      for (int index2 = 0; index2 < partsOnSmallGrid.Length && partsOnSmallGrid[index2] != -1; ++index2)
        ++numArray[1];
      if (numArray[0] != numArray[1])
        return false;
    }
    for (int index = 0; index < gridInfo.Count && (this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.JudgeCondition[0] != 0 || this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.JudgeCondition[1] != 0); ++index)
    {
      List<int> smallGridPutElement = gridInfo[index].GetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index]);
      if (smallGridPutElement == null || this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.JudgeCondition[0] != smallGridPutElement[smallGridPutElement.Count - 1] || this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.JudgeCondition[1] == smallGridPutElement[smallGridPutElement.Count - 1])
        return false;
    }
    for (int index1 = 0; index1 < gridInfo.Count; ++index1)
    {
      if (info != 14 && info != 7 && !((Component) gridInfo[index1]).get_gameObject().get_activeSelf() || Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0 && gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1)[2] == 9)
        return false;
      switch (info)
      {
        case 1:
        case 2:
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (info != 1 && Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0)
            return false;
          break;
        case 3:
          if (this.bFloorLimit && Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 == 3)
            return false;
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          int[] partOnSmallGrid1 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (partOnSmallGrid1[2] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid1[4] == 12)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid1[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0 && partOnSmallGrid1[0] == -1)
            return false;
          break;
        case 4:
          if (this.bFloorLimit && Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 == 3)
            return false;
          int num2 = 0;
          foreach (int num3 in gridInfo[index1].GetStackWallOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt))
          {
            if (num3 != -1)
              ++num2;
          }
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            if (num2 == 0)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            if (num2 >= Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] + 5 - gridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])
              return false;
            if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt == 0)
            {
              if (!this.WallDireCheck((float) gridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt]))
                return false;
            }
            else if (!this.WallDireCheck((float) (gridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] + Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0])))
              return false;
          }
          int[] partOnSmallGrid2 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (partOnSmallGrid2[2] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid2[4] == 12)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid2[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0 && gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == -1)
            return false;
          break;
        case 5:
          if (gridInfo[index1].GetCanRoofState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) != 1)
            return false;
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[6] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 6:
        case 16:
          int[] partOnSmallGrid3 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (!this.WhitchDoorOnWall(carsol, this.partsMgr.GetBuildPartsInfo(buildPart).nHeight, ref wall) || partOnSmallGrid3[3] != -1)
            return false;
          break;
        case 7:
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0 && index1 == 0)
          {
            bool flag = false;
            for (int index2 = 0; index2 < gridInfo.Count; ++index2)
            {
              flag = gridInfo[index2].GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1;
              if (flag)
                break;
            }
            if (!flag)
              return false;
          }
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          int[] partOnSmallGrid4 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (partOnSmallGrid4[2] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid4[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 8:
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0 && gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == -1)
            return false;
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          int num4 = 0;
          foreach (int num3 in gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt))
          {
            if (num3 != -1)
              ++num4;
          }
          if (num4 >= Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] + 5 - gridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])
            return false;
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 9:
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          int[] partOnSmallGrid5 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (partOnSmallGrid5[2] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid5[4] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid5[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 != Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt)
          {
            if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1) == 1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            int[] partOnSmallGrid6 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1);
            if (partOnSmallGrid6[2] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            if (partOnSmallGrid6[4] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            if (partOnSmallGrid6[5] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1)[0] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0 && gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == -1)
            return false;
          break;
        case 10:
          int[] partOnSmallGrid7 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int[] stackWallOnSmallGrid1 = gridInfo[index1].GetStackWallOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 0)
            return false;
          bool flag1 = true;
          for (int index2 = 0; index2 < stackWallOnSmallGrid1.Length; ++index2)
          {
            if (stackWallOnSmallGrid1[index2] != -1)
            {
              flag1 = false;
              break;
            }
          }
          if (partOnSmallGrid7[1] == -1 && flag1)
            return false;
          if (partOnSmallGrid7[3] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (partOnSmallGrid7[4] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 11:
          if (Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt < 3)
            return false;
          int num5 = 0;
          for (int floorcnt = 0; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++floorcnt)
          {
            if (gridInfo[index1].GetUseState(floorcnt))
              ++num5;
          }
          if (gridInfo[index1].GetPartOnSmallGrid(0, num5 - 1)[0] != 1)
            return false;
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt == 0)
          {
            if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            int[] partOnSmallGrid6 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], 0);
            for (int index2 = 0; index2 < partOnSmallGrid6.Length; ++index2)
            {
              if (index2 != 0 && index2 != 6 && partOnSmallGrid6[index2] != -1)
              {
                this.craftInfomationUI.SetWarningMessage(0);
                return false;
              }
            }
            if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], 0)[0] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
          }
          for (int floorcnt = 1; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1; ++floorcnt)
          {
            if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], floorcnt) == 1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
            int[] partOnSmallGrid6 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], floorcnt);
            for (int index2 = 0; index2 < partOnSmallGrid6.Length; ++index2)
            {
              if (index2 != 0 && index2 != 6 && partOnSmallGrid6[index2] != -1)
              {
                this.craftInfomationUI.SetWarningMessage(0);
                return false;
              }
            }
            if (gridInfo[index1].GetStackPartsOnSmallGrid(SmallGridID[index1], floorcnt)[0] != -1)
            {
              this.craftInfomationUI.SetWarningMessage(0);
              return false;
            }
          }
          break;
        case 12:
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[6] == -1)
            return false;
          int[] partOnSmallGrid8 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          bool flag2 = false;
          foreach (int num3 in gridInfo[index1].GetStackWallOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt))
          {
            if (num3 != -1)
            {
              flag2 = true;
              break;
            }
          }
          if (partOnSmallGrid8[1] != -1 && flag2)
            return false;
          if (partOnSmallGrid8[4] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 13:
          int[] partOnSmallGrid9 = gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int[] stackWallOnSmallGrid2 = gridInfo[index1].GetStackWallOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 0)
            return false;
          int num6 = 0;
          for (int index2 = 0; index2 < stackWallOnSmallGrid2.Length; ++index2)
          {
            if (stackWallOnSmallGrid2[index2] != -1)
              ++num6;
          }
          bool flag3 = num6 < this.PutHeight + 1;
          if (partOnSmallGrid9[1] == -1 && flag3 || partOnSmallGrid9[4] != -1)
            return false;
          if (partOnSmallGrid9[3] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          break;
        case 14:
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[5] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[1] != -1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          int[] stackWallOnSmallGrid3 = gridInfo[index1].GetStackWallOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int num7 = 0;
          for (int index2 = 0; index2 < stackWallOnSmallGrid3.Length && stackWallOnSmallGrid3[index2] != -1; ++index2)
            ++num7;
          if (num7 > 0)
            return false;
          if (!((Component) gridInfo[index1]).get_gameObject().get_activeSelf())
          {
            ((Component) gridInfo[index1]).get_gameObject().SetActive(true);
            break;
          }
          break;
        default:
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1)
          {
            this.craftInfomationUI.SetWarningMessage(0);
            return false;
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0 && gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == -1)
            return false;
          break;
      }
    }
    return true;
  }

  private void SetParts(
    Transform CarsolTransform,
    Quaternion rot,
    GameObject buildPart,
    int numX,
    int numZ,
    int mode = 0)
  {
    Vector3 localPosition = CarsolTransform.get_localPosition();
    int info = this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(3);
    switch (info)
    {
      case 2:
      case 13:
      case 14:
        localPosition.y = (__Null) (this.CarsolUnderGridList[0].get_transform().get_position().y + (double) this.fGridSize / 2.0);
        ref Vector3 local1 = ref localPosition;
        local1.y = (__Null) (local1.y + (double) this.PutHeight);
        break;
      case 5:
        localPosition.y = (__Null) (double) (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5 + 5 + Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0]);
        break;
      case 12:
        localPosition.y = (__Null) (double) (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5 + 5 + Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0] - this.partsMgr.GetBuildPartsInfo(buildPart).nHeight / 2);
        break;
    }
    if (mode == 0)
    {
      ref Vector3 local2 = ref localPosition;
      local2.y = (__Null) (local2.y - (double) this.fGridSize / 2.0);
    }
    buildPart.get_transform().set_localPosition(localPosition);
    Transform transform = buildPart.get_transform();
    Quaternion localRotation1 = buildPart.get_transform().get_localRotation();
    // ISSUE: variable of the null type
    __Null x = ((Quaternion) ref localRotation1).get_eulerAngles().x;
    Quaternion localRotation2 = buildPart.get_transform().get_localRotation();
    // ISSUE: variable of the null type
    __Null z = ((Quaternion) ref localRotation2).get_eulerAngles().z;
    Quaternion quaternion = Quaternion.Euler((float) x, 0.0f, (float) z);
    transform.set_localRotation(quaternion);
    ((BuildPartsInfo) buildPart.GetComponent<BuildPartsInfo>()).nPutFloor = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
    if (info != 1)
      buildPart.get_transform().set_localRotation(Quaternion.op_Multiply(rot, buildPart.get_transform().get_localRotation()));
    ++this.nPutPartsNum;
  }

  private void ChangeKindPart(int kind)
  {
    this.nPartsForm = kind;
    this.nPartsForm %= 10;
    if (this.nPartsForm < 0)
      this.nPartsForm = 9;
    this.ChangeCarsol();
    this.nID = 0;
  }

  private void ChangeCarsol()
  {
    this.carsol.ChangeCarsol(this.nPartsForm);
  }

  private void CraftAllReset()
  {
    for (int targetfloor = 0; targetfloor < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++targetfloor)
    {
      this.TargetFloorReset(targetfloor);
      Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate[targetfloor] = false;
    }
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
        Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].ReserveListDel(0, 1);
    }
    this.FloorResetGrid();
    Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt = 0;
    Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = 1;
    this.gridMap.ChangeCraftMap(this.GridList, 0);
    this.gridMap.CraftMapSearchRoom(this.GridList, 0);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.nCarsolMode = 0;
    this.Cam.Reset(0);
    this.Cam.bLock = false;
    this.Cam.nFloorCnt = 0;
    this.Cam.ForceMoveCam(0.0f);
  }

  private void TargetFloorReset(int targetfloor)
  {
    for (int index1 = 0; index1 < this.GridList.Count; ++index1)
    {
      GridInfo gridInfo = this.BaseGridInfo[index1];
      Transform[] componentsInChildren = (Transform[]) this.GridList[index1].GetComponentsInChildren<Transform>();
      for (int index2 = 0; index2 < componentsInChildren.Length - 1; ++index2)
      {
        gridInfo.ChangeSmallGrid(index2, 0, -1, targetfloor, false);
        gridInfo.ChangeSmallGridColor(targetfloor, index2);
        gridInfo.SetInRoomSmallGrid(index2, false, targetfloor);
        gridInfo.SetCanRoofSmallGrid(index2, targetfloor, 0);
      }
    }
    for (int index1 = 0; index1 < 10; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
      {
        List<GameObject> list = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetList();
        for (int index3 = 0; index3 < list.Count; ++index3)
        {
          BuildPartsInfo component = (BuildPartsInfo) list[index3].GetComponent<BuildPartsInfo>();
          if ((list[index3].get_activeSelf() || component.nPutFloor != -1) && component.nPutFloor == targetfloor)
          {
            list[index3].SetActive(false);
            component.nPutFloor = -1;
          }
        }
      }
    }
  }

  private void FloorResetGrid()
  {
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      GridInfo gridInfo = this.BaseGridInfo[index];
      this.GridList[index].SetActive(true);
      Vector3 position = this.GridList[index].get_transform().get_position();
      position.y = this.MinPos.y;
      this.GridList[index].get_transform().set_position(position);
      gridInfo.DelFloor(0);
      gridInfo.AddFloor();
      gridInfo.SetUseState(0, true);
    }
  }

  private void SaveCraft()
  {
    this.save.Save((ObjPool) this.Gridpool);
    Singleton<SaveLoadUI>.Instance.saveEnd.SetActive(true);
  }

  private void LoadCraft()
  {
    this.save.Load(this.Gridpool, Singleton<SaveLoadUI>.Instance.saveFiles[Singleton<SaveLoadUI>.Instance.nSaveID]);
    Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt = 0;
    this.TargetFloorUp();
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    Singleton<SaveLoadUI>.Instance.loadEnd.SetActive(true);
    Singleton<UndoRedoMgr>.Instance.Clear();
  }

  private bool WhitchDoorOnWall(Carsol carsol, int height, ref List<DeleatWall> wall)
  {
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count; ++index1)
    {
      switch (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][index1].GetItemKind())
      {
        case 3:
        case 4:
          List<GameObject> list = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][index1].GetList();
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            if (list[index2].get_activeSelf() && ((BuildPartsInfo) list[index2].GetComponent<BuildPartsInfo>()).nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt && list[index2].get_transform().get_position().x == ((Component) carsol).get_transform().get_position().x && (list[index2].get_transform().get_position().y <= (double) (height - 1 + Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5) && list[index2].get_transform().get_position().z == ((Component) carsol).get_transform().get_position().z))
            {
              DeleatWall deleatWall;
              deleatWall.Wall = list[index2];
              deleatWall.PartForm = this.nPartsForm;
              deleatWall.PartKind = index1;
              deleatWall.WallID = index2;
              wall.Add(deleatWall);
            }
          }
          break;
      }
    }
    wall = wall.Distinct<DeleatWall>().ToList<DeleatWall>();
    return wall.Count == height;
  }

  private void FloorUP(List<CraftCommandList.ChangeValParts> AutoFloor)
  {
    bool flag1 = false;
    if (Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1 == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
    {
      if (!this.bFloorLimit || Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt < 3)
      {
        for (int index = 0; index < this.GridList.Count; ++index)
          this.BaseGridInfo[index].AddFloor();
        Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList.Add(new bool[this.AllGridNum]);
        Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate.Add(false);
        ++Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
        flag1 = true;
        Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight.Add(0);
        this.Cam.setLimitPos(10f);
      }
    }
    if (Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1)
      return;
    GameObject gameObject = (GameObject) null;
    int index1 = 0;
    int index2 = 0;
    int num = 6;
    for (int index3 = 0; index3 < this.GridList.Count; ++index3)
    {
      if (Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index3].nFloorPartsHeight[0] > 0 && num > Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index3].nFloorPartsHeight[0])
        num = Singleton<CraftCommandListBaseObject>.Instance.BaseGridInfo[index3].nFloorPartsHeight[0];
    }
    for (int index3 = 0; index3 < this.GridList.Count; ++index3)
    {
      GridInfo gridInfo = this.BaseGridInfo[index3];
      if (gridInfo.GetFloorNum() > Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1)
      {
        if ((gridInfo.GetCanRoofState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1 || gridInfo.GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[2] == 9) && !gridInfo.GetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1))
        {
          bool flag2 = false;
          for (int id = 0; id < 4; ++id)
          {
            int[] partOnSmallGrid1 = gridInfo.GetPartOnSmallGrid(id, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            int[] partOnSmallGrid2 = gridInfo.GetPartOnSmallGrid(id, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1);
            if (partOnSmallGrid1[2] == 9 || partOnSmallGrid2[0] != -1)
            {
              if (partOnSmallGrid1[2] == 9)
              {
                for (int index4 = 0; index4 < 4; ++index4)
                {
                  gridInfo.ChangeSmallGrid(index4, 1, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
                  gridInfo.ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, index4, 0, -1);
                  gridInfo.ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, index4);
                }
              }
              flag2 = true;
              break;
            }
          }
          int ID;
          if (!flag2)
          {
            ID = -1;
            gameObject = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[0][0].Get(ref ID);
            AutoFloor.Add(new CraftCommandList.ChangeValParts());
            AutoFloor[AutoFloor.Count - 1].nFormKind = 0;
            AutoFloor[AutoFloor.Count - 1].nPoolID = 0;
            AutoFloor[AutoFloor.Count - 1].nItemID = ID;
            this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 0);
            if (((GridInfo) this.GridList[index3].GetComponent<GridInfo>()).GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[2] != 15)
            {
              Vector3 position = this.GridList[index3].get_transform().get_position();
              position.y = (__Null) 0.0;
              for (int index4 = 0; index4 < Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1; ++index4)
              {
                ref Vector3 local = ref position;
                local.y = (__Null) (local.y + (double) (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[index4] + 5));
              }
              gameObject.get_transform().set_position(position);
              ((BuildPartsInfo) gameObject.GetComponent<BuildPartsInfo>()).nPutFloor = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1;
              gameObject.SetActive(false);
              for (int index4 = 0; index4 < 4; ++index4)
              {
                gridInfo.ChangeSmallGrid(index4, 2, 1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
                gridInfo.SetCanRoofSmallGrid(index4, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, 2);
              }
            }
            else
            {
              for (int index4 = 0; index4 < 4; ++index4)
              {
                gridInfo.ChangeSmallGrid(index4, 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
                gridInfo.SetCanRoofSmallGrid(index4, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, 2);
              }
            }
            gridInfo.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, true);
            Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1][gridInfo.nID] = true;
            this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 1);
            ++this.nPutPartsNum;
          }
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt == 0)
          {
            ID = -1;
            List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
            for (int index4 = 0; index4 < baseParts.Length; ++index4)
            {
              for (int index5 = 0; index5 < baseParts[index4].Count; ++index5)
              {
                if ((Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0] != 0 || baseParts[index4][index5].GetItemKind() == 1) && (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0] == 0 || baseParts[index4][index5].GetItemKind() == 2))
                {
                  gameObject = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index4][index5].Get(ref ID);
                  index1 = index4;
                  index2 = index5;
                  break;
                }
              }
            }
            AutoFloor.Add(new CraftCommandList.ChangeValParts());
            AutoFloor[AutoFloor.Count - 1].nFormKind = index1;
            AutoFloor[AutoFloor.Count - 1].nPoolID = index2;
            AutoFloor[AutoFloor.Count - 1].nItemID = ID;
            this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 0);
            Vector3 position = this.GridList[index3].get_transform().get_position();
            if (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0] != 0)
            {
              ref Vector3 local = ref position;
              local.y = (__Null) (local.y + (double) num);
            }
            gameObject.get_transform().set_position(position);
            BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo(gameObject);
            buildPartsInfo.nPutFloor = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
            if (gridInfo.GetPartOnSmallGrid(0, 0)[0] != -1)
            {
              gameObject.SetActive(false);
              Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].ReserveListDel(ID, 0);
              buildPartsInfo.nPutFloor = -1;
            }
            else
            {
              gameObject.SetActive(true);
              for (int index4 = 0; index4 < 4; ++index4)
              {
                gridInfo.ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, index4, 0, buildPartsInfo.GetInfo(3));
                if (gridInfo.GetStateSmallGrid(index4, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) != 1)
                  gridInfo.ChangeSmallGrid(index4, 2, buildPartsInfo.GetInfo(3), Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
                gridInfo.SetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, index4, this.partsMgr.BuildPartPoolDic[index1][index2].Item2.Element, false, true);
              }
              gridInfo.nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = num <= 5 ? num : 0;
              ++this.nPutPartsNum;
            }
            this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 1);
          }
          gameObject = (GameObject) null;
          ID = -1;
          for (int index4 = 0; index4 < 10; ++index4)
          {
            for (int index5 = 0; index5 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index4].Count; ++index5)
            {
              if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index4][index5].GetItemKind() == 5)
              {
                gameObject = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index4][index5].Get(ref ID);
                break;
              }
            }
            if (Object.op_Inequality((Object) gameObject, (Object) null))
              break;
          }
          BuildPartsInfo buildPartsInfo1 = this.partsMgr.GetBuildPartsInfo(gameObject);
          AutoFloor.Add(new CraftCommandList.ChangeValParts());
          AutoFloor[AutoFloor.Count - 1].nFormKind = buildPartsInfo1.GetInfo(2);
          AutoFloor[AutoFloor.Count - 1].nPoolID = buildPartsInfo1.GetInfo(1);
          AutoFloor[AutoFloor.Count - 1].nItemID = ID;
          this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 0);
          Vector3 position1 = this.GridList[index3].get_transform().get_position();
          position1.y = (__Null) (double) (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5 + 5 + Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0]);
          gameObject.get_transform().set_position(position1);
          BuildPartsInfo buildPartsInfo2 = this.partsMgr.GetBuildPartsInfo(gameObject);
          buildPartsInfo2.nPutFloor = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
          if (gridInfo.GetPartOnSmallGrid(0, buildPartsInfo2.nPutFloor)[6] == 5)
          {
            gameObject.SetActive(false);
            Singleton<CraftCommandListBaseObject>.Instance.BaseParts[AutoFloor[AutoFloor.Count - 1].nFormKind][AutoFloor[AutoFloor.Count - 1].nPoolID].ReserveListDel(ID, 0);
            buildPartsInfo2.nPutFloor = -1;
          }
          else
          {
            gameObject.SetActive(true);
            for (int index4 = 0; index4 < 4; ++index4)
            {
              gridInfo.ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, index4, 6, 5);
              if (gridInfo.GetStateSmallGrid(index4, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) != 1)
                gridInfo.ChangeSmallGrid(index4, 2, 5, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
            }
            ++this.nPutPartsNum;
          }
          this.URPutPartsSetB(AutoFloor[AutoFloor.Count - 1], gameObject, 1);
        }
      }
      else
        break;
    }
    this.PartsOnElevatorDelFU(AutoFloor);
    if (flag1)
    {
      if (!this.FloorUpTex.get_activeSelf())
        this.FloorUpTex.SetActive(true);
      ++Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
      Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt %= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
      this.TargetFloorUp();
    }
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
  }

  private void AllFloorDel(List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      GridInfo gridInfo = this.BaseGridInfo[index];
      for (int ID = 0; ID < 4; ++ID)
        gridInfo.ChangeSmallGrid(ID, 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
      gridInfo.DelFloor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1);
    }
    this.FloorDeleteBuildParts(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, valAutoParts);
    Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1;
    Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight.RemoveAt(Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight.Count - 1);
    this.Cam.setLimitPos(-10f);
  }

  private void FloorDel(List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      GridInfo gridInfo = this.BaseGridInfo[index];
      if (gridInfo.GetCanRoofState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 0 && gridInfo.GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[2] != 9)
      {
        for (int floorcnt = Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++floorcnt)
        {
          for (int ID = 0; ID < 4; ++ID)
            gridInfo.ChangeSmallGrid(ID, 0, -1, floorcnt, false);
          gridInfo.SetUseState(floorcnt, false);
          Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[floorcnt][index] = false;
        }
      }
    }
    this.FloorDeleteBuildParts(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, this.BaseGridInfo, valAutoParts);
  }

  private void FloorDeleteBuildParts(
    int nFloorCnt,
    List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    List<List<GameObject>> gameObjectListList = new List<List<GameObject>>();
    List<BuildPartsPool> buildPartsPoolList = new List<BuildPartsPool>();
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
      {
        gameObjectListList.Add(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetList());
        buildPartsPoolList.Add(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2]);
      }
    }
    for (int index = 0; index < gameObjectListList.Count; ++index)
    {
      for (int ID = 0; ID < gameObjectListList[index].Count; ++ID)
      {
        BuildPartsInfo component = (BuildPartsInfo) gameObjectListList[index][ID].GetComponent<BuildPartsInfo>();
        if (component.nPutFloor >= nFloorCnt)
        {
          valAutoParts.Add(new CraftCommandList.ChangeValParts());
          valAutoParts[valAutoParts.Count - 1].nFormKind = buildPartsPoolList[index].GetFormKind();
          valAutoParts[valAutoParts.Count - 1].nPoolID = component.GetInfo(1);
          valAutoParts[valAutoParts.Count - 1].nItemID = component.GetInfo(0);
          this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index][ID], 0);
          component.nPutFloor = -1;
          gameObjectListList[index][ID].SetActive(false);
          buildPartsPoolList[index].ReserveListDel(ID, 0);
          this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index][ID], 1);
          --this.nPutPartsNum;
        }
      }
    }
  }

  private void FloorDeleteBuildParts(
    int nFloorCnt,
    List<GridInfo> Grid,
    List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    List<List<GameObject>> gameObjectListList = new List<List<GameObject>>();
    List<BuildPartsPool> buildPartsPoolList = new List<BuildPartsPool>();
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
      {
        gameObjectListList.Add(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetList());
        buildPartsPoolList.Add(Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2]);
      }
    }
    for (int index1 = 0; index1 < gameObjectListList.Count; ++index1)
    {
      for (int ID = 0; ID < gameObjectListList[index1].Count; ++ID)
      {
        BuildPartsInfo component = (BuildPartsInfo) gameObjectListList[index1][ID].GetComponent<BuildPartsInfo>();
        if (component.nPutFloor >= nFloorCnt)
        {
          for (int index2 = 0; index2 < Grid.Count; ++index2)
          {
            if (Grid[index2].GetCanRoofState(nFloorCnt - 1) == 0)
            {
              Rect rect;
              ((Rect) ref rect).\u002Ector((float) (((Component) Grid[index2]).get_transform().get_position().x - (double) this.fGridSize / 2.0), (float) (((Component) Grid[index2]).get_transform().get_position().z - (double) this.fGridSize / 2.0), this.fGridSize, this.fGridSize);
              Vector2 vector2;
              ((Vector2) ref vector2).\u002Ector((float) gameObjectListList[index1][ID].get_transform().get_position().x, (float) gameObjectListList[index1][ID].get_transform().get_position().z);
              if (((Rect) ref rect).Contains(vector2))
              {
                valAutoParts.Add(new CraftCommandList.ChangeValParts());
                valAutoParts[valAutoParts.Count - 1].nFormKind = buildPartsPoolList[index1].GetFormKind();
                valAutoParts[valAutoParts.Count - 1].nPoolID = component.GetInfo(1);
                valAutoParts[valAutoParts.Count - 1].nItemID = component.GetInfo(0);
                this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index1][ID], 0);
                component.nPutFloor = -1;
                gameObjectListList[index1][ID].SetActive(false);
                buildPartsPoolList[index1].ReserveListDel(ID, 0);
                this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index1][ID], 1);
                --this.nPutPartsNum;
                break;
              }
            }
          }
        }
      }
    }
  }

  private void TargetFloorUp()
  {
    this.TargetFloorUpCamera();
    this.TargetFloorUpPart();
    this.TargetFloorUpGrid();
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      for (int ID = 0; ID < 4; ++ID)
        this.BaseGridInfo[index].ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, ID);
    }
  }

  private void TargetFloorUpCamera()
  {
    this.Cam.CameraUp(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
  }

  private void TargetFloorUpGrid()
  {
    for (int index1 = 0; index1 < this.GridList.Count; ++index1)
    {
      GridInfo component = (GridInfo) this.GridList[index1].GetComponent<GridInfo>();
      if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt == 0)
      {
        if (!this.ViewMode)
          this.GridList[index1].SetActive(true);
      }
      else
      {
        bool flag = false;
        GridInfo gridInfo = this.BaseGridInfo[index1];
        if (gridInfo.GetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt))
          flag = true;
        if (!flag)
        {
          for (int id = 0; id < 4; ++id)
          {
            int[] partOnSmallGrid1 = gridInfo.GetPartOnSmallGrid(id, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            if (partOnSmallGrid1[0] != -1)
            {
              flag = true;
              gridInfo.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
              break;
            }
            if (partOnSmallGrid1[5] == 14)
            {
              flag = true;
              gridInfo.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
              break;
            }
            int[] partOnSmallGrid2 = gridInfo.GetPartOnSmallGrid(id, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1);
            if (partOnSmallGrid2[0] == 5)
            {
              flag = true;
              gridInfo.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
              break;
            }
            if (partOnSmallGrid2[2] == 9)
            {
              flag = true;
              gridInfo.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
              break;
            }
          }
        }
        if (!flag)
          this.GridList[index1].SetActive(false);
      }
      Vector3 position = this.GridList[index1].get_transform().get_position();
      position.y = this.MinPos.y;
      ref Vector3 local1 = ref position;
      local1.y = (__Null) (local1.y + (double) (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5));
      if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0)
      {
        ref Vector3 local2 = ref position;
        local2.y = (__Null) (local2.y + (double) Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0]);
      }
      this.GridList[index1].get_transform().set_position(position);
      if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0 && component.GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1)[2] == 15)
      {
        Rect rect;
        ((Rect) ref rect).\u002Ector((float) (this.GridList[index1].get_transform().get_position().x - (double) this.fGridSize * 2.0), (float) (this.GridList[index1].get_transform().get_position().z - (double) this.fGridSize * 2.0), this.fGridSize * 5f, this.fGridSize * 5f);
        for (int index2 = 0; index2 < this.GridList.Count; ++index2)
        {
          Vector2 vector2;
          ((Vector2) ref vector2).\u002Ector((float) this.GridList[index2].get_transform().get_position().x, (float) this.GridList[index2].get_transform().get_position().z);
          if (((Rect) ref rect).Contains(vector2))
          {
            this.GridList[index2].get_gameObject().SetActive(true);
            component.SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
          }
        }
      }
    }
  }

  private void TargetFloorUpPart()
  {
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count; ++index2)
      {
        List<GameObject> list = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetList();
        for (int index3 = 0; index3 < list.Count; ++index3)
        {
          BuildPartsInfo component = (BuildPartsInfo) list[index3].GetComponent<BuildPartsInfo>();
          if (component.nPutFloor >= 0 && component.nPutFloor <= Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
            list[index3].SetActive(true);
          else
            list[index3].SetActive(false);
        }
      }
    }
  }

  private void ChangeGridInfo(
    GameObject buildPart,
    List<GridInfo> gridInfo,
    List<int> SmallGridID,
    int targetCnt,
    List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    int info1 = this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(3);
    int info2 = this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(2);
    int info3 = this.partsMgr.GetBuildPartsInfo(buildPart).GetInfo(1);
    bool floor = this.partsMgr.BuildPartPoolDic[info2][info3].Item2.Catkind == 1;
    for (int index1 = 0; index1 < targetCnt; ++index1)
    {
      switch (info1)
      {
        case 3:
        case 4:
          gridInfo[index1].SetCanRoofSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, 2);
          if (gridInfo[index1].GetPartOnSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == -1)
          {
            gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 0, 1);
            break;
          }
          break;
        case 7:
          if (!((Component) gridInfo[index1]).get_gameObject().get_activeSelf())
          {
            ((Component) gridInfo[index1]).get_gameObject().SetActive(true);
            gridInfo[index1].SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
            Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt][gridInfo[index1].nID] = true;
            break;
          }
          break;
        default:
          if (info1 == 15)
            goto case 3;
          else
            break;
      }
      switch (info1 - 1)
      {
        case 0:
        case 1:
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 0)
            gridInfo[index1].ChangeSmallGrid(SmallGridID[index1], 2, info1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          else
            gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 0, info1);
          if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt != 0)
          {
            for (int id = 0; id < 4; ++id)
              gridInfo[index1].SetCanRoofSmallGrid(id, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1, 2);
          }
          if (info1 != 1 && Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] < this.PutHeight)
          {
            this.FloorHeightMove(this.PutHeight - Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt], (float) (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] + 5), valAutoParts);
            Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = this.PutHeight;
            break;
          }
          break;
        case 2:
        case 3:
          for (int index2 = 0; index2 < this.partsMgr.GetBuildPartsInfo(buildPart).nHeight; ++index2)
            gridInfo[index1].ChangeSmallGrid(SmallGridID[index1], 1, info1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          break;
        case 4:
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 6, 5);
          break;
        case 5:
          for (int index2 = 0; index2 < this.partsMgr.GetBuildPartsInfo(buildPart).nHeight; ++index2)
            gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 1, -1);
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 2, 6);
          break;
        case 9:
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 3, 10);
          break;
        case 11:
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 4, 12);
          break;
        case 12:
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 4, 13);
          break;
        case 13:
          if (gridInfo[index1].GetStateSmallGrid(SmallGridID[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) != 1)
            gridInfo[index1].ChangeSmallGrid(SmallGridID[index1], 1, 14, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          else
            gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 5, 14);
          gridInfo[index1].SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
          Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt][gridInfo[index1].nID] = gridInfo[index1].GetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          break;
        case 15:
          for (int index2 = 0; index2 < ((BuildPartsInfo) buildPart.GetComponent<BuildPartsInfo>()).nHeight; ++index2)
            gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 1, -1);
          gridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], 2, 16);
          break;
        default:
          if (this.partsMgr.BuildPartPoolDic[info2][info3].Item2.PutFlag != 0)
          {
            for (int index2 = 0; index2 < this.partsMgr.GetBuildPartsInfo(buildPart).nHeight; ++index2)
              gridInfo[index1].ChangeSmallGrid(SmallGridID[index1], 2, info1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, true);
            break;
          }
          gridInfo[index1].ChangeSmallGrid(SmallGridID[index1], 1, info1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          break;
      }
      if (info1 == 2)
        gridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = this.PutHeight;
      if (info1 != 12 && info1 != 5 && info1 != 14)
        gridInfo[index1].SetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1], this.partsMgr.BuildPartPoolDic[info2][info3].Item2.Element, false, floor);
      gridInfo[index1].ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index1]);
    }
  }

  private void CheckUnderRoof()
  {
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    for (int index1 = 0; index1 < this.GridList.Count; ++index1)
    {
      GridInfo gridInfo = this.BaseGridInfo[index1];
      for (int floorcnt = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1; floorcnt >= 0; --floorcnt)
      {
        for (int id = 0; id < 4; ++id)
        {
          bool flag = false;
          for (int index2 = 0; index2 < intList1.Count; ++index2)
          {
            if (intList1[index2] == index1 && intList2[index2] == id)
            {
              flag = true;
              break;
            }
          }
          if (!flag && gridInfo.GetPartOnSmallGrid(id, floorcnt)[2] == 7)
          {
            intList1.Add(index1);
            intList2.Add(id);
            intList3.Add(floorcnt);
          }
        }
      }
    }
    for (int floorcnt = 0; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++floorcnt)
    {
      for (int index = 0; index < intList1.Count; ++index)
      {
        if (floorcnt < intList3[index])
          this.BaseGridInfo[intList1[index]].SetInRoomSmallGrid(intList2[index], true, floorcnt);
      }
      GridInfo.ChangeGridInfo(this.BaseGridInfo, floorcnt);
    }
  }

  private void PartsOnStairsDel(
    List<GridInfo> HitGridInfo,
    List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    GridInfo gridInfo1 = (GridInfo) null;
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    for (int index1 = 0; index1 < HitGridInfo.Count; ++index1)
    {
      gridInfo1 = (GridInfo) null;
      GridInfo gridInfo2 = HitGridInfo[index1];
      if (Object.op_Inequality((Object) gridInfo2, (Object) null) && gridInfo2.GetFloorNum() > Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1)
      {
        bool flag = false;
        List<List<GameObject>> gameObjectListList = new List<List<GameObject>>();
        List<Tuple<int, int>> tupleList = new List<Tuple<int, int>>();
        for (int index2 = 0; index2 < baseParts.Length; ++index2)
        {
          for (int index3 = 0; index3 < baseParts[index2].Count; ++index3)
          {
            switch (baseParts[index2][index3].GetItemKind())
            {
              case 1:
              case 5:
                tupleList.Add(new Tuple<int, int>(index2, index3));
                gameObjectListList.Add(baseParts[index2][index3].GetList());
                break;
            }
          }
        }
        for (int index2 = 0; index2 < gameObjectListList.Count; ++index2)
        {
          for (int ID = 0; ID < gameObjectListList[index2].Count; ++ID)
          {
            BuildPartsInfo component = (BuildPartsInfo) gameObjectListList[index2][ID].GetComponent<BuildPartsInfo>();
            if (!gameObjectListList[index2][ID].get_activeSelf() && component.nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1)
            {
              if (gameObjectListList[index2][ID].get_transform().get_position().x == ((Component) gridInfo2).get_transform().get_position().x && gameObjectListList[index2][ID].get_transform().get_position().z == ((Component) gridInfo2).get_transform().get_position().z)
              {
                valAutoParts.Add(new CraftCommandList.ChangeValParts());
                valAutoParts[valAutoParts.Count - 1].nFormKind = tupleList[index2].Item1;
                valAutoParts[valAutoParts.Count - 1].nPoolID = tupleList[index2].Item2;
                valAutoParts[valAutoParts.Count - 1].nItemID = ID;
                this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 0);
                gameObjectListList[index2][ID].SetActive(false);
                component.nPutFloor = -1;
                Singleton<CraftCommandListBaseObject>.Instance.BaseParts[tupleList[index2].Item1][tupleList[index2].Item2].ReserveListDel(ID, 0);
                flag = true;
                this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 1);
                --this.nPutPartsNum;
                break;
              }
              if (flag)
                break;
            }
          }
          for (int index3 = 0; index3 < 4; ++index3)
          {
            gridInfo2.ChangeSmallGrid(index3, 1, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
            gridInfo2.ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, index3, 0, -1);
            gridInfo2.ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, index3, 6, -1);
            gridInfo2.ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, index3);
          }
        }
      }
    }
  }

  private void PartsOnElevatorDel(
    List<GridInfo> gridinfo,
    List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    if (this.BaseGridInfo[0].GetFloorNum() <= 2 || gridinfo.Count == 0)
      return;
    List<Tuple<int, int>> tupleList = (List<Tuple<int, int>>) null;
    List<List<GameObject>> gameObjectListList = (List<List<GameObject>>) null;
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        switch (baseParts[index1][index2].GetItemKind())
        {
          case 1:
          case 5:
            tupleList.Add(new Tuple<int, int>(index1, index2));
            gameObjectListList.Add(baseParts[index1][index2].GetList());
            break;
        }
      }
    }
    for (int index1 = 0; index1 < gridinfo.Count; ++index1)
    {
      for (int index2 = 0; index2 < gameObjectListList.Count; ++index2)
      {
        for (int ID = 0; ID < gameObjectListList[index2].Count; ++ID)
        {
          BuildPartsInfo component = (BuildPartsInfo) gameObjectListList[index2][ID].GetComponent<BuildPartsInfo>();
          if (!gameObjectListList[index2][ID].get_activeSelf() && component.nPutFloor != Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1 && gameObjectListList[index2][ID].get_transform().get_position().x == ((Component) gridinfo[index1]).get_transform().get_position().x && gameObjectListList[index2][ID].get_transform().get_position().z == ((Component) gridinfo[index1]).get_transform().get_position().z)
          {
            valAutoParts.Add(new CraftCommandList.ChangeValParts());
            valAutoParts[valAutoParts.Count - 1].nFormKind = tupleList[index2].Item1;
            valAutoParts[valAutoParts.Count - 1].nPoolID = tupleList[index2].Item2;
            valAutoParts[valAutoParts.Count - 1].nItemID = ID;
            this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 0);
            component.nPutFloor = -1;
            baseParts[tupleList[index2].Item1][tupleList[index2].Item2].ReserveListDel(ID, 0);
            this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 1);
            --this.nPutPartsNum;
          }
        }
      }
    }
    for (int index1 = 0; index1 < gridinfo.Count; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
      {
        for (int index3 = 1; index3 < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1; ++index3)
        {
          gridinfo[index1].ChangeSmallGrid(index2, 1, -1, index3, false);
          gridinfo[index1].ChangeSmallGridItemKind(index3, index2, 0, -1);
          gridinfo[index1].ChangeSmallGridItemKind(index3 - 1, index2, 6, -1);
          gridinfo[index1].ChangeSmallGridColor(index3, index2);
        }
      }
    }
  }

  private void PartsOnElevatorDelFU(List<CraftCommandList.ChangeValParts> valAutoParts)
  {
    if (this.BaseGridInfo[0].GetFloorNum() <= 2)
      return;
    List<int> intList = new List<int>();
    List<GridInfo> gridInfoList = new List<GridInfo>();
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      GridInfo gridInfo = this.BaseGridInfo[index];
      if (gridInfo.GetPartOnSmallGrid(0, 0)[2] == 11)
        gridInfoList.Add(gridInfo);
    }
    if (gridInfoList.Count == 0)
      return;
    List<Tuple<int, int>> tupleList = (List<Tuple<int, int>>) null;
    List<List<GameObject>> gameObjectListList = (List<List<GameObject>>) null;
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        switch (baseParts[index1][index2].GetItemKind())
        {
          case 1:
          case 5:
            tupleList.Add(new Tuple<int, int>(index1, index2));
            gameObjectListList.Add(baseParts[index1][index2].GetList());
            break;
        }
      }
    }
    for (int index1 = 0; index1 < gridInfoList.Count; ++index1)
    {
      int num = 0;
      for (int floorcnt = 0; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt; ++floorcnt)
      {
        if (gridInfoList[index1].GetUseState(floorcnt))
          ++num;
      }
      for (int index2 = 0; index2 < gameObjectListList.Count; ++index2)
      {
        for (int ID = 0; ID < gameObjectListList[index2].Count; ++ID)
        {
          BuildPartsInfo component = (BuildPartsInfo) gameObjectListList[index2][ID].GetComponent<BuildPartsInfo>();
          if (component.nPutFloor > 0 && component.nPutFloor != num - 1 && gameObjectListList[index2][ID].get_transform().get_position().x == ((Component) gridInfoList[index1]).get_transform().get_position().x && gameObjectListList[index2][ID].get_transform().get_position().z == ((Component) gridInfoList[index1]).get_transform().get_position().z)
          {
            valAutoParts.Add(new CraftCommandList.ChangeValParts());
            valAutoParts[valAutoParts.Count - 1].nFormKind = tupleList[index2].Item1;
            valAutoParts[valAutoParts.Count - 1].nPoolID = tupleList[index2].Item2;
            valAutoParts[valAutoParts.Count - 1].nItemID = ID;
            this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 0);
            gameObjectListList[index2][ID].SetActive(false);
            component.nPutFloor = -1;
            Singleton<CraftCommandListBaseObject>.Instance.BaseParts[tupleList[index2].Item1][tupleList[index2].Item2].ReserveListDel(ID, 0);
            intList.Add(index1);
            this.URPutPartsSetB(valAutoParts[valAutoParts.Count - 1], gameObjectListList[index2][ID], 1);
            --this.nPutPartsNum;
          }
        }
      }
    }
    for (int index1 = 0; index1 < gridInfoList.Count; ++index1)
    {
      if (intList.Contains(index1))
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          for (int index3 = 1; index3 < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1; ++index3)
          {
            gridInfoList[index1].ChangeSmallGrid(index2, 1, -1, index3, false);
            gridInfoList[index1].ChangeSmallGridItemKind(index3, index2, 0, -1);
            gridInfoList[index1].ChangeSmallGridItemKind(index3 - 1, index2, 6, -1);
            gridInfoList[index1].ChangeSmallGridColor(index3, index2);
          }
        }
      }
    }
  }

  private bool WallDireCheck(float floorHeight)
  {
    bool flag = false;
    for (int index1 = 0; index1 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts.Length && !flag; ++index1)
    {
      for (int index2 = 0; index2 < Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1].Count && !flag; ++index2)
      {
        switch (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetItemKind())
        {
          case 4:
          case 6:
            List<GameObject> list = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[index1][index2].GetList();
            for (int index3 = 0; index3 < list.Count && !flag; ++index3)
            {
              if (list[index3].get_activeSelf() && ((BuildPartsInfo) list[index3].GetComponent<BuildPartsInfo>()).nPutFloor != -1 && list[index3].get_transform().get_position().x == ((Component) this.carsol).get_transform().get_position().x && (list[index3].get_transform().get_position().y == (double) (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt * 5) + (double) floorHeight && list[index3].get_transform().get_position().z == ((Component) this.carsol).get_transform().get_position().z))
              {
                Quaternion rotation1 = list[index3].get_transform().get_rotation();
                Vector3 eulerAngles1 = ((Quaternion) ref rotation1).get_eulerAngles();
                Quaternion rotation2 = ((Component) this.carsol).get_transform().get_rotation();
                Vector3 eulerAngles2 = ((Quaternion) ref rotation2).get_eulerAngles();
                if (Vector3.op_Equality(eulerAngles1, eulerAngles2))
                  flag = true;
              }
            }
            break;
        }
      }
    }
    return flag;
  }

  private void DecideDelParts(
    BuildPartsPool pool,
    ref List<GameObject> tmptarget,
    ref List<int> tmptargetID,
    ref List<BuildPartsPool> tmptargetpool,
    int mode = 0)
  {
    List<GameObject> list = pool.GetList();
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].get_activeSelf())
      {
        BuildPartsInfo component = (BuildPartsInfo) list[index].GetComponent<BuildPartsInfo>();
        if (mode == 1)
        {
          if (component.nPutFloor != Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1)
            continue;
        }
        else if (component.nPutFloor != Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
          continue;
        if (list[index].get_transform().get_localPosition().x == ((Component) this.carsol).get_transform().get_position().x && list[index].get_transform().get_localPosition().z == ((Component) this.carsol).get_transform().get_position().z)
        {
          tmptarget.Add(list[index]);
          tmptargetID.Add(index);
          tmptargetpool.Add(pool);
        }
      }
    }
  }

  private void CarsolUnderCheck()
  {
    this.CarsolUnderGridList.Clear();
    this.CarsolUnderSmallGridIDList.Clear();
    for (int index = 0; index < this.GridList.Count; ++index)
    {
      for (int smallID = 0; smallID < 4; ++smallID)
        this.BaseGridInfo[index].ChangeSmallGridUnderCarsol(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, smallID, false);
    }
    List<RaycastHit[]> HitsObjInfo = this.carsol.CheckCarsol(((Component) this.carsol).get_transform().get_rotation(), ((Component) this.carsol).get_transform().get_rotation(), this.nPartsForm, ref this.BoxCastRange, new Vector3?());
    List<GridInfo> gridInfoList = new List<GridInfo>();
    List<int> SmallGridID = new List<int>();
    this.GridCheck(HitsObjInfo, this.GridList, gridInfoList, SmallGridID);
    for (int index = 0; index < gridInfoList.Count; ++index)
      gridInfoList[index].ChangeSmallGridUnderCarsol(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, SmallGridID[index], true);
    List<GridInfo> list = gridInfoList.ToList<GridInfo>();
    for (int index = 0; index < list.Count; ++index)
    {
      this.CarsolUnderGridList.Add(((Component) list[index]).get_gameObject());
      this.CarsolUnderSmallGridIDList.Add(SmallGridID[index]);
    }
  }

  private void TmpGridActiveUpdate()
  {
    if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt <= 0)
      return;
    for (int index = 0; index < this.AllGridNum; ++index)
      Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt][index] = this.BaseGridInfo[index].GetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
  }

  private void FloatingObjGrid(List<GameObject> gridList)
  {
    if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count == 0)
      return;
    if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].GetItemKind() == 14)
    {
      Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = true;
      for (int index = 0; index < gridList.Count; ++index)
      {
        gridList[index].SetActive(true);
        GridInfo gridInfo = this.BaseGridInfo[index];
        for (int ID = 0; ID < 4; ++ID)
          gridInfo.ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, ID);
      }
    }
    else
    {
      for (int index = 0; index < gridList.Count; ++index)
        gridList[index].SetActive(Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt][index]);
      Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveListUpdate[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = false;
    }
  }

  public void SelectBuldPart()
  {
    int itemKind = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm][this.nID].GetItemKind();
    switch (itemKind)
    {
      case 1:
        break;
      case 2:
        break;
      case 5:
        break;
      case 6:
        break;
      case 16:
        break;
      default:
        this.SelectPutGridInfo = new CraftCommandList.ChangeValGrid[this.AllGridNum];
        for (int index = 0; index < this.AllGridNum; ++index)
          this.SelectPutGridInfo[index] = new CraftCommandList.ChangeValGrid();
        this.URPutPartsSetG(ref this.SelectPutGridInfo, 0);
        this.SelectPutMaxFloorCnt[0] = Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
        int num1 = ((IEnumerable<GameObject>) this.CarsolUnderGridList).Select<GameObject, int>((Func<GameObject, int>) (v => ((GridInfo) v.GetComponent<GridInfo>()).nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt])).Max();
        List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
        List<List<GameObject>> gameObjectListList = new List<List<GameObject>>();
        for (int index1 = 0; index1 < baseParts.Length; ++index1)
        {
          for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
            gameObjectListList.Add(baseParts[index1][index2].GetList());
        }
        List<ValueTuple<GameObject, List<GridInfo>, List<int>>> valueTupleList1 = new List<ValueTuple<GameObject, List<GridInfo>, List<int>>>();
        for (int index1 = 0; index1 < gameObjectListList.Count; ++index1)
        {
          for (int index2 = 0; index2 < gameObjectListList[index1].Count; ++index2)
          {
            if (this.partsMgr.GetBuildPartsInfo(gameObjectListList[index1][index2]).nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
              valueTupleList1.Add(new ValueTuple<GameObject, List<GridInfo>, List<int>>(gameObjectListList[index1][index2], this.partsMgr.GetBuildPartsInfo(gameObjectListList[index1][index2]).putGridInfos, this.partsMgr.GetBuildPartsInfo(gameObjectListList[index1][index2]).putSmallGridInfos));
          }
        }
        for (int index1 = 0; index1 < this.CarsolUnderGridList.Count; ++index1)
        {
          for (int index2 = 0; index2 < valueTupleList1.Count; ++index2)
          {
            for (int index3 = 0; index3 < ((List<GridInfo>) valueTupleList1[index2].Item2).Count; ++index3)
            {
              if (!Object.op_Inequality((Object) this.CarsolUnderGridList[index1], (Object) ((Component) ((List<GridInfo>) valueTupleList1[index2].Item2)[index3]).get_gameObject()) && this.CarsolUnderSmallGridIDList[index1] == ((List<int>) valueTupleList1[index2].Item3)[index3])
              {
                this.SelectObj.Add(new ValueTuple<GameObject, int, float, GameObject>((GameObject) valueTupleList1[index2].Item1, num1, (float) ((GameObject) valueTupleList1[index2].Item1).get_transform().get_position().y, ((Component) ((GameObject) valueTupleList1[index2].Item1).get_transform().get_parent()).get_gameObject()));
                break;
              }
            }
          }
        }
        this.SelectObj = ((IEnumerable<ValueTuple<GameObject, int, float, GameObject>>) this.SelectObj).Distinct<ValueTuple<GameObject, int, float, GameObject>>().ToList<ValueTuple<GameObject, int, float, GameObject>>();
        this.SelectObj = ((IEnumerable<ValueTuple<GameObject, int, float, GameObject>>) ((IEnumerable<ValueTuple<GameObject, int, float, GameObject>>) this.SelectObj).OrderByDescending<ValueTuple<GameObject, int, float, GameObject>, float>((Func<ValueTuple<GameObject, int, float, GameObject>, float>) (n => (float) ((GameObject) n.Item1).get_transform().get_position().y))).ToList<ValueTuple<GameObject, int, float, GameObject>>();
        GameObject gameObject = (GameObject) null;
        for (int index = 0; index < this.SelectObj.Count; ++index)
        {
          BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index].Item1);
          if (buildPartsInfo.GetInfo(2) == this.nPartsForm && buildPartsInfo.GetInfo(1) == this.nID)
          {
            if (Mathf.Approximately((float) ((GameObject) this.SelectObj[index].Item1).get_transform().get_position().x, (float) ((Component) this.carsol).get_transform().get_position().x) && Mathf.Approximately((float) ((GameObject) this.SelectObj[index].Item1).get_transform().get_position().z, (float) ((Component) this.carsol).get_transform().get_position().z))
            {
              if (((GameObject) this.SelectObj[index].Item1).get_transform().get_rotation().y == ((Component) this.carsol).get_transform().get_rotation().y)
              {
                if (Object.op_Equality((Object) gameObject, (Object) null))
                  gameObject = (GameObject) this.SelectObj[index].Item1;
                else if (gameObject.get_transform().get_position().y > ((GameObject) this.SelectObj[index].Item1).get_transform().get_position().y)
                  gameObject = (GameObject) this.SelectObj[index].Item1;
              }
            }
            else
              break;
          }
        }
        if (Object.op_Equality((Object) gameObject, (Object) null))
        {
          this.SelectObj.Clear();
          break;
        }
        List<ValueTuple<GameObject, int, float, GameObject>> valueTupleList2 = new List<ValueTuple<GameObject, int, float, GameObject>>();
        if (itemKind == 10 || itemKind == 13)
        {
          valueTupleList2.Clear();
          for (int index = 0; index < this.SelectObj.Count; ++index)
          {
            if (this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index].Item1).GetInfo(3) != itemKind)
              valueTupleList2.Add(this.SelectObj[index]);
          }
        }
        else
        {
          for (int index = 0; index < this.SelectObj.Count; ++index)
          {
            int info = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index].Item1).GetInfo(3);
            if (gameObject.get_transform().get_position().y > ((GameObject) this.SelectObj[index].Item1).get_transform().get_position().y || info == 1 || info == 5)
              valueTupleList2.Add(this.SelectObj[index]);
          }
        }
        for (int index = 0; index < valueTupleList2.Count; ++index)
          this.SelectObj.Remove(valueTupleList2[index]);
        if (this.SelectObj.Count == 0)
          break;
        this.SelectObj = ((IEnumerable<ValueTuple<GameObject, int, float, GameObject>>) ((IEnumerable<ValueTuple<GameObject, int, float, GameObject>>) this.SelectObj).OrderBy<ValueTuple<GameObject, int, float, GameObject>, float>((Func<ValueTuple<GameObject, int, float, GameObject>, float>) (n => (float) ((GameObject) n.Item1).get_transform().get_position().y))).ToList<ValueTuple<GameObject, int, float, GameObject>>();
        List<Tuple<GridInfo, int>> tupleList = new List<Tuple<GridInfo, int>>();
        for (int index1 = 0; index1 < this.SelectObj.Count; ++index1)
        {
          List<GridInfo> putGridInfos = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putGridInfos;
          List<int> putSmallGridInfos = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putSmallGridInfos;
          for (int index2 = 0; index2 < putGridInfos.Count; ++index2)
          {
            Tuple<GridInfo, int> tuple = new Tuple<GridInfo, int>(putGridInfos[index2], putSmallGridInfos[index2]);
            bool flag = false;
            for (int index3 = 0; index3 < tupleList.Count; ++index3)
            {
              if (tupleList[index3].Item1.nID == tuple.Item1.nID && tupleList[index3].Item2 == tuple.Item2)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if (index1 > 0)
              {
                this.SelectObj.Clear();
                return;
              }
              tupleList.Add(tuple);
            }
          }
        }
        float num2 = (float) (((Component) this.carsol).get_transform().get_position().y - ((GameObject) this.SelectObj[0].Item1).get_transform().get_position().y);
        this.carsol.SelectModeCarsolUnvisible();
        for (int index = 0; index < this.SelectObj.Count; ++index)
        {
          ((GameObject) this.SelectObj[index].Item1).get_transform().SetParent(((Component) this.carsol).get_transform());
          Vector3 localPosition = ((GameObject) this.SelectObj[index].Item1).get_transform().get_localPosition();
          ref Vector3 local = ref localPosition;
          local.y = (__Null) (local.y + ((double) num2 - (double) this.fGridSize / 2.0));
          ((GameObject) this.SelectObj[index].Item1).get_transform().set_localPosition(localPosition);
        }
        Vector3 localPosition1 = ((Component) this.carsol.cursoldir).get_transform().get_localPosition();
        localPosition1.y = (__Null) 0.0;
        ref Vector3 local1 = ref localPosition1;
        local1.y = (__Null) (local1.y + (double) num2);
        ((Component) this.carsol.cursoldir).get_transform().set_localPosition(localPosition1);
        for (int index1 = 0; index1 < this.SelectObj.Count; ++index1)
        {
          BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1);
          List<GridInfo> putGridInfos = buildPartsInfo.putGridInfos;
          List<int> putSmallGridInfos = buildPartsInfo.putSmallGridInfos;
          for (int index2 = 0; index2 < putGridInfos.Count; ++index2)
          {
            switch (this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).GetInfo(3))
            {
              case 3:
                putGridInfos[index2].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 1, -1);
                break;
              case 4:
                putGridInfos[index2].ChangeSmallGridStack(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 4, 1);
                break;
              case 12:
              case 13:
                putGridInfos[index2].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 4, -1);
                break;
              case 14:
                putGridInfos[index2].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 5, -1);
                break;
              case 15:
                putGridInfos[index2].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 2, -1);
                break;
              default:
                if (this.partsMgr.BuildPartPoolDic[buildPartsInfo.GetInfo(2)][buildPartsInfo.GetInfo(1)].Item2.PutFlag != 0)
                {
                  putGridInfos[index2].ChangeSmallGridStack(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 8, 1);
                  break;
                }
                putGridInfos[index2].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], 2, -1);
                break;
            }
            putGridInfos[index2].SetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, putSmallGridInfos[index2], -1, false, false);
          }
        }
        for (int index1 = 0; index1 < tupleList.Count; ++index1)
        {
          int[] partOnSmallGrid = tupleList[index1].Item1.GetPartOnSmallGrid(tupleList[index1].Item2, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int[] stackWallOnSmallGrid = tupleList[index1].Item1.GetStackWallOnSmallGrid(tupleList[index1].Item2, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int[] partsOnSmallGrid = tupleList[index1].Item1.GetStackPartsOnSmallGrid(tupleList[index1].Item2, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
          int num3 = 0;
          int num4 = 0;
          int num5 = 0;
          for (int index2 = 0; index2 < partOnSmallGrid.Length; ++index2)
          {
            if (partOnSmallGrid[index2] != -1)
              ++num3;
          }
          for (int index2 = 0; index2 < stackWallOnSmallGrid.Length && stackWallOnSmallGrid[index2] != -1; ++index2)
            ++num4;
          for (int index2 = 0; index2 < partsOnSmallGrid.Length && partsOnSmallGrid[index2] != -1; ++index2)
            ++num5;
          if (num3 + num4 + num5 == 0)
            tupleList[index1].Item1.ChangeSmallGrid(tupleList[index1].Item2, 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          else
            tupleList[index1].Item1.ChangeSmallGrid(tupleList[index1].Item2, 2, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          tupleList[index1].Item1.ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tupleList[index1].Item2);
        }
        this.SelectPutPartInfo = new List<CraftCommandList.ChangeValParts>();
        for (int index = 0; index < this.SelectObj.Count; ++index)
        {
          this.SelectPutPartInfo.Add(new CraftCommandList.ChangeValParts());
          BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index].Item1);
          this.SelectPutPartInfo[this.SelectPutPartInfo.Count - 1].nFormKind = buildPartsInfo.GetInfo(2);
          this.SelectPutPartInfo[this.SelectPutPartInfo.Count - 1].nPoolID = buildPartsInfo.GetInfo(1);
          this.SelectPutPartInfo[this.SelectPutPartInfo.Count - 1].nItemID = buildPartsInfo.GetInfo(0);
          this.URPutPartsSetB(this.SelectPutPartInfo[this.SelectPutPartInfo.Count - 1], (GameObject) this.SelectObj[index].Item1, 0);
        }
        break;
    }
  }

  private bool SelectPut()
  {
    this.SelectPutAutoPartInfo = new List<CraftCommandList.ChangeValParts>();
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < this.SelectObj.Count; ++index)
    {
      switch (this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index].Item1).GetInfo(3))
      {
        case 3:
        case 4:
        case 15:
          gameObjectList.Add((GameObject) this.SelectObj[index].Item1);
          break;
      }
    }
    List<DeleatWall> wall = new List<DeleatWall>();
    List<GridInfo> tmpGridInfo = new List<GridInfo>();
    List<int> tmpSmallGridID = new List<int>();
    if (!this.GridPutCheck(this.GridList, this.carsol, (GameObject) this.SelectObj[0].Item1, ref wall, ref tmpGridInfo, ref tmpSmallGridID))
      return false;
    this.FloorPartsDel((GameObject) this.SelectObj[0].Item1, tmpGridInfo, this.SelectPutAutoPartInfo);
    this.ChangeGridInfo((GameObject) this.SelectObj[0].Item1, tmpGridInfo, tmpSmallGridID, tmpSmallGridID.Count, this.SelectPutAutoPartInfo);
    this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[0].Item1).putGridInfos.Clear();
    this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[0].Item1).putSmallGridInfos.Clear();
    for (int index = 0; index < tmpGridInfo.Count; ++index)
    {
      this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[0].Item1).putGridInfos.Add(tmpGridInfo[index]);
      this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[0].Item1).putSmallGridInfos.Add(tmpSmallGridID[index]);
    }
    this.gridMap.ChangeCraftMap(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    this.gridMap.CraftMapSearchRoom(this.GridList, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    GridInfo.ChangeGridInfo(this.BaseGridInfo, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
    int num = this.PillarOnGridNum();
    if (this.gridMap.CraftMapRoofDecide() || this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[0].Item1).GetInfo(3) == 15 && num == 1)
      this.FloorUP(this.SelectPutAutoPartInfo);
    Vector3 position = ((Component) this.carsol).get_transform().get_position();
    for (int index1 = 0; index1 < this.SelectObj.Count; ++index1)
    {
      if (index1 != 0)
      {
        tmpGridInfo.Clear();
        tmpSmallGridID.Clear();
        this.GridCheck(this.GridList, this.carsol, tmpGridInfo, tmpSmallGridID, ((Component) this.carsol).get_transform().get_rotation(), this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).GetInfo(2), new Vector3?(((GameObject) this.SelectObj[index1].Item1).get_transform().get_position()));
        this.ChangeGridInfo((GameObject) this.SelectObj[index1].Item1, tmpGridInfo, tmpSmallGridID, tmpSmallGridID.Count, this.SelectPutAutoPartInfo);
        this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putGridInfos.Clear();
        this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putSmallGridInfos.Clear();
        for (int index2 = 0; index2 < tmpGridInfo.Count; ++index2)
        {
          this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putGridInfos.Add(tmpGridInfo[index2]);
          this.partsMgr.GetBuildPartsInfo((GameObject) this.SelectObj[index1].Item1).putSmallGridInfos.Add(tmpSmallGridID[index2]);
        }
      }
      position.y = (__Null) (((GameObject) this.SelectObj[index1].Item1).get_transform().get_position().y - (double) this.fGridSize / 2.0);
      if (position.y >= (double) (Singleton<CraftCommandListBaseObject>.Instance.MaxPutHeight[0] + 5 * Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 5))
      {
        BuildPartsInfo component = (BuildPartsInfo) ((GameObject) this.SelectObj[index1].Item1).GetComponent<BuildPartsInfo>();
        this.DedPartsGridChange(component.putGridInfos.Count, component.putGridInfos, component.putSmallGridInfos, component.GetInfo(3), component.nHeight);
        ((GameObject) this.SelectObj[index1].Item1).SetActive(false);
        component.nPutFloor = -1;
        Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.SelectPutPartInfo[index1].nFormKind][this.SelectPutPartInfo[index1].nPoolID].ReserveListDel(this.SelectPutPartInfo[index1].nItemID, 0);
        --this.nPutPartsNum;
      }
      else
        ((GameObject) this.SelectObj[index1].Item1).get_transform().SetParent(((GameObject) this.SelectObj[index1].Item4).get_transform());
      this.URPutPartsSetB(this.SelectPutPartInfo[index1], (GameObject) this.SelectObj[index1].Item1, 1);
    }
    if (gameObjectList.Count > 0)
    {
      CraftItemInfo craftItemInfo = this.partsMgr.BuildPartPoolDic[0][0].Item2;
      List<GameObject> list1 = this.partsMgr.BuildPartPoolDic[0][0].Item1.GetList();
      List<GameObject> list2 = ((IEnumerable<GameObject>) this.CarsolUnderGridList).Distinct<GameObject>().ToList<GameObject>();
      for (int index1 = 0; index1 < list2.Count; ++index1)
      {
        GridInfo component = (GridInfo) list2[index1].GetComponent<GridInfo>();
        if (component.GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[0] == 1)
        {
          bool flag = true;
          for (int index2 = 0; index2 < list1.Count; ++index2)
          {
            if (list1[index2].get_activeSelf() && ((BuildPartsInfo) list1[index2].GetComponent<BuildPartsInfo>()).nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt && list1[index2].get_transform().get_position().x == ((Component) component).get_transform().get_position().x && list1[index2].get_transform().get_position().z == ((Component) component).get_transform().get_position().z)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            int ID = -1;
            GameObject gameObject = this.partsMgr.BuildPartPoolDic[0][0].Item1.Get(ref ID);
            this.SelectPutAutoPartInfo.Add(new CraftCommandList.ChangeValParts());
            this.SelectPutAutoPartInfo[this.SelectPutAutoPartInfo.Count - 1].nFormKind = 0;
            this.SelectPutAutoPartInfo[this.SelectPutAutoPartInfo.Count - 1].nPoolID = 0;
            this.SelectPutAutoPartInfo[this.SelectPutAutoPartInfo.Count - 1].nItemID = ID;
            this.URPutPartsSetB(this.SelectPutAutoPartInfo[this.SelectPutAutoPartInfo.Count - 1], gameObject, 0);
            this.SetParts(((Component) component).get_transform(), ((Component) this.carsol).get_transform().get_rotation(), gameObject, craftItemInfo.Horizontal, craftItemInfo.Vertical, 1);
            gameObject.SetActive(true);
            this.URPutPartsSetB(this.SelectPutAutoPartInfo[this.SelectPutAutoPartInfo.Count - 1], gameObject, 1);
          }
        }
      }
    }
    return true;
  }

  private void URPutPartsSetG(ref CraftCommandList.ChangeValGrid[] Val, int mode)
  {
    for (int index1 = 0; index1 < this.GridList.Count; ++index1)
    {
      Val[index1].nFloorNum[mode] = this.BaseGridInfo[index1].GetFloorNum();
      Val[index1].Pos[mode] = this.GridList[index1].get_transform().get_position();
      Renderer[] componentsInChildren = (Renderer[]) this.GridList[index1].GetComponentsInChildren<Renderer>();
      for (int index2 = 0; index2 < this.BaseGridInfo[index1].GetFloorNum(); ++index2)
      {
        Val[index1].smallGrids[mode].Add(new SmallGrid[4]);
        Val[index1].colors[mode].Add(new Color[4]);
        for (int index3 = 0; index3 < 4; ++index3)
        {
          Val[index1].smallGrids[mode][index2][index3].m_canRoof = this.BaseGridInfo[index1].GetSmallGridCanRoof(index3, index2);
          Val[index1].smallGrids[mode][index2][index3].m_inRoom = this.BaseGridInfo[index1].GetSmallGridInRoom(index3, index2);
          Val[index1].smallGrids[mode][index2][index3].m_state = this.BaseGridInfo[index1].GetStateSmallGrid(index3, index2);
          Val[index1].smallGrids[mode][index2][index3].m_itemkind = new int[7];
          for (int index4 = 0; index4 < 7; ++index4)
            Val[index1].smallGrids[mode][index2][index3].m_itemkind[index4] = this.BaseGridInfo[index1].GetPartOnSmallGrid(index3, index2)[index4];
          Val[index1].smallGrids[mode][index2][index3].m_itemstackwall = new int[GridInfo.nSmallGridStackWallMax];
          Val[index1].smallGrids[mode][index2][index3].m_itemdupulication = new int[GridInfo.nSmallGridStackWallMax];
          int num1 = 0;
          int[] stackWallOnSmallGrid = this.BaseGridInfo[index1].GetStackWallOnSmallGrid(index3, index2);
          for (int index4 = 0; index4 < stackWallOnSmallGrid.Length && stackWallOnSmallGrid[index4] != -1; ++index4)
            ++num1;
          for (int index4 = 0; index4 < Val[index1].smallGrids[mode][index2][index3].m_itemstackwall.Length; ++index4)
            Val[index1].smallGrids[mode][index2][index3].m_itemstackwall[index4] = index4 >= num1 ? -1 : 4;
          int num2 = 0;
          int[] partsOnSmallGrid = this.BaseGridInfo[index1].GetStackPartsOnSmallGrid(index3, index2);
          for (int index4 = 0; index4 < partsOnSmallGrid.Length && partsOnSmallGrid[index4] != -1; ++index4)
            ++num2;
          for (int index4 = 0; index4 < Val[index1].smallGrids[mode][index2][index3].m_itemdupulication.Length; ++index4)
            Val[index1].smallGrids[mode][index2][index3].m_itemdupulication[index4] = index4 >= num2 ? -1 : partsOnSmallGrid[index4];
          Val[index1].smallGrids[mode][index2][index3].m_color = componentsInChildren[index3];
          Val[index1].colors[mode][index2][index3] = this.BaseGridInfo[index1].GetSmallGridColor(index2, index3);
          Val[index1].smallGrids[mode][index2][index3].m_UnderCarsol = this.BaseGridInfo[index1].GetUnderTheCarsol(index2, index3);
          Val[index1].smallGrids[mode][index2][index3].m_PutElement = this.BaseGridInfo[index1].GetSmallGridPutElement(index2, index3);
        }
        Val[index1].bUse[mode].Add(this.BaseGridInfo[index1].GetUseState(index2));
        Val[index1].nFloorPartsHeight[mode].Add(this.BaseGridInfo[index1].nFloorPartsHeight[index2]);
        Val[index1].nCanRoof[mode].Add(this.BaseGridInfo[index1].GetCanRoofState(index2));
        Val[index1].nInRoom[mode].Add(this.BaseGridInfo[index1].GetInRoomState(index2));
      }
    }
  }

  private void URPutPartsSetB(CraftCommandList.ChangeValParts Val, GameObject parts, int mode)
  {
    Val.active[mode] = parts.get_activeSelf();
    Val.Pos[mode] = parts.get_transform().get_position();
    Val.Rot[mode] = parts.get_transform().get_rotation();
    BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo(parts);
    Val.nPutFloor[mode] = buildPartsInfo.nPutFloor;
    Val.nDirection[mode] = buildPartsInfo.GetInfo(4);
    if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[buildPartsInfo.GetInfo(2)][buildPartsInfo.GetInfo(1)].ReserveListCheck(buildPartsInfo.GetInfo(0)))
      Val.ReserveList[mode] = buildPartsInfo.GetInfo(0);
    else
      Val.ReserveList[mode] = -1;
  }

  public void ChangeParts()
  {
    if (Singleton<CraftCommandListBaseObject>.Instance.BaseParts[this.nPartsForm].Count != 0)
    {
      this.craftSelectPartsInfo.szItemName = this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.Name;
      this.carsol.SetMoveLimit(this.partsMgr.BuildPartPoolDic[this.nPartsForm][this.nID].Item2.MoveVal);
    }
    else
    {
      this.craftSelectPartsInfo.szItemName = "Non";
      this.carsol.SetMoveLimit(0.5f);
    }
    this.CarsolUnderCheck();
    this.FloatingObjGrid(this.GridList);
    this.PutHeight = 0;
  }

  private int PillarOnGridNum()
  {
    List<int[]> numArrayList = new List<int[]>();
    for (int index = 0; index < this.GridList.Count; ++index)
      numArrayList.Add(this.BaseGridInfo[index].GetPartOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt));
    int num = 0;
    for (int index = 0; index < numArrayList.Count; ++index)
    {
      if (numArrayList[index][2] == 15)
        ++num;
    }
    return num;
  }

  private void ChangeItemBox(int tmpID)
  {
    if (!this.craftItem.isActive)
    {
      this.craftItem.isActive = true;
      this.craftItem.ChangeList(tmpID + 1);
    }
    else
      this.craftItem.ChangeList(tmpID + 1);
    this.craftItemButtonUI.isActive = false;
  }

  public void Undo()
  {
    Singleton<UndoRedoMgr>.Instance.Undo();
    if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt >= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt)
      Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt %= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    this.TargetFloorUp();
    this.CarsolUnderCheck();
  }

  public void Redo()
  {
    Singleton<UndoRedoMgr>.Instance.Redo();
    if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt >= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt)
      Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt %= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    this.TargetFloorUp();
    this.CarsolUnderCheck();
  }

  public void OpelateFloorUp()
  {
    ++Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt;
    Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt %= Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt;
    this.TargetFloorUp();
  }

  private void CarsolOffset()
  {
    int num1 = 0;
    List<GameObject> list = ((IEnumerable<GameObject>) this.CarsolUnderGridList).Distinct<GameObject>().ToList<GameObject>();
    for (int index = 0; index < list.Count; ++index)
    {
      int num2 = ((GridInfo) list[index].GetComponent<GridInfo>()).nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt];
      if (num2 > num1)
        num1 = num2;
    }
    int num3 = 0;
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      int[] stackWallOnSmallGrid = ((GridInfo) list[index1].GetComponent<GridInfo>()).GetStackWallOnSmallGrid(0, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      int num2 = 0;
      for (int index2 = 0; index2 < stackWallOnSmallGrid.Length && stackWallOnSmallGrid[index2] != -1; ++index2)
        ++num2;
      if (num3 < num2)
        num3 = num2;
    }
    int num4 = 0;
    for (int index1 = 0; index1 < this.CarsolUnderGridList.Count; ++index1)
    {
      int[] partsOnSmallGrid = ((GridInfo) this.CarsolUnderGridList[index1].GetComponent<GridInfo>()).GetStackPartsOnSmallGrid(this.CarsolUnderSmallGridIDList[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      int num2 = 0;
      for (int index2 = 0; index2 < partsOnSmallGrid.Length && partsOnSmallGrid[index2] != -1; ++index2)
        ++num2;
      if (num4 < num2)
        num4 = num2;
    }
    Vector3 position = ((Component) this.carsol).get_transform().get_position();
    if (list.Count > 0)
    {
      position.y = (__Null) (list[0].get_transform().get_position().y + (double) this.fGridSize / 2.0);
      ref Vector3 local1 = ref position;
      local1.y = (__Null) (local1.y + (double) num1);
      ref Vector3 local2 = ref position;
      local2.y = (__Null) (local2.y + (double) num3);
      ref Vector3 local3 = ref position;
      local3.y = (__Null) (local3.y + (double) num4);
    }
    ((Component) this.carsol).get_transform().set_position(position);
  }

  private void FloorHeightMove(
    int moveval,
    float targetPartsHeight,
    List<CraftCommandList.ChangeValParts> AutoParts)
  {
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        List<GameObject> list = baseParts[index1][index2].GetList();
        for (int ID = 0; ID < list.Count; ++ID)
        {
          BuildPartsInfo component = (BuildPartsInfo) list[ID].GetComponent<BuildPartsInfo>();
          if (component.nPutFloor != -1)
          {
            if (list[ID].get_transform().get_position().y < (double) targetPartsHeight)
            {
              if (list[ID].get_transform().get_position().y >= (double) targetPartsHeight + (double) moveval)
              {
                this.DedPartsGridChange(component.putGridInfos.Count, component.putGridInfos, component.putSmallGridInfos, component.GetInfo(3), component.nHeight);
                AutoParts.Add(new CraftCommandList.ChangeValParts());
                AutoParts[AutoParts.Count - 1].nFormKind = index1;
                AutoParts[AutoParts.Count - 1].nPoolID = index2;
                AutoParts[AutoParts.Count - 1].nItemID = ID;
                this.URPutPartsSetB(AutoParts[AutoParts.Count - 1], list[ID], 0);
                list[ID].SetActive(false);
                component.nPutFloor = -1;
                baseParts[index1][index2].ReserveListDel(ID, 0);
                this.URPutPartsSetB(AutoParts[AutoParts.Count - 1], list[ID], 1);
                --this.nPutPartsNum;
              }
            }
            else
            {
              AutoParts.Add(new CraftCommandList.ChangeValParts());
              AutoParts[AutoParts.Count - 1].nFormKind = index1;
              AutoParts[AutoParts.Count - 1].nPoolID = index2;
              AutoParts[AutoParts.Count - 1].nItemID = ID;
              this.URPutPartsSetB(AutoParts[AutoParts.Count - 1], list[ID], 0);
              Vector3 position = list[ID].get_transform().get_position();
              ref Vector3 local = ref position;
              local.y = (__Null) (local.y + (double) moveval);
              list[ID].get_transform().set_position(position);
              this.URPutPartsSetB(AutoParts[AutoParts.Count - 1], list[ID], 1);
            }
          }
        }
      }
    }
  }

  private void DedPartsGridChange(
    int tmpCnt,
    List<GridInfo> tmpGridInfo,
    List<int> tmpSmallGridCount,
    int TargetKind,
    int height)
  {
    for (int index1 = 0; index1 < tmpCnt; ++index1)
    {
      int[] partOnSmallGrid = tmpGridInfo[index1].GetPartOnSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      int[] stackWallOnSmallGrid1 = tmpGridInfo[index1].GetStackWallOnSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      int[] partsOnSmallGrid = tmpGridInfo[index1].GetStackPartsOnSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int index2 = 0; index2 < partOnSmallGrid.Length; ++index2)
      {
        if (partOnSmallGrid[index2] != -1)
          ++num1;
      }
      for (int index2 = 0; index2 < stackWallOnSmallGrid1.Length && stackWallOnSmallGrid1[index2] != -1; ++index2)
        ++num2;
      for (int index2 = 0; index2 < partsOnSmallGrid.Length && partsOnSmallGrid[index2] != -1; ++index2)
        ++num3;
      if (num1 + num2 + num3 - height < 1)
      {
        tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
        tmpGridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = 0;
        if (TargetKind == 9 && Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt)
          tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
        if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0 && TargetKind == 14)
        {
          tmpGridInfo[index1].SetUseState(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
          Singleton<CraftCommandListBaseObject>.Instance.tmpGridActiveList[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt][tmpGridInfo[index1].nID] = false;
        }
      }
      else
      {
        switch (TargetKind)
        {
          case 1:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 0, -1);
            break;
          case 2:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 0, -1);
            tmpGridInfo[index1].nFloorPartsHeight[Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt] = 0;
            break;
          case 3:
          case 4:
            if (TargetKind == 3)
            {
              tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 1, -1);
            }
            else
            {
              for (int index2 = 0; index2 < height; ++index2)
                tmpGridInfo[index1].ChangeSmallGridStack(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 4, 1);
            }
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 3, -1);
            int[] stackWallOnSmallGrid2 = tmpGridInfo[index1].GetStackWallOnSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
            int num4 = 0;
            for (int index2 = 0; index2 < stackWallOnSmallGrid2.Length && stackWallOnSmallGrid2[index2] != -1; ++index2)
              ++num4;
            if (partOnSmallGrid[1] == -1 && num4 == 0)
            {
              tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 2, partOnSmallGrid[0], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
              break;
            }
            break;
          case 5:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 6, -1);
            break;
          case 6:
            tmpGridInfo[index1].ChangeSmallGridStack(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 4, -1);
            tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 2, 1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 2, -1);
            break;
          case 10:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 3, -1);
            break;
          case 12:
          case 13:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 4, -1);
            break;
          case 14:
            tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 5, -1);
            break;
          default:
            if (tmpGridInfo[index1].GetStateSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) != 1)
            {
              for (int index2 = 0; index2 < height; ++index2)
                tmpGridInfo[index1].ChangeSmallGridStack(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], TargetKind, 1);
            }
            else
            {
              tmpGridInfo[index1].ChangeSmallGridItemKind(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], 2, -1);
              tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 2, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, false);
            }
            if (TargetKind == 9)
            {
              if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1 < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt)
              {
                tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 0, -1, Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt + 1, false);
                break;
              }
              break;
            }
            if (TargetKind == 11)
            {
              for (int floorcnt = 1; floorcnt < Singleton<CraftCommandListBaseObject>.Instance.nMaxFloorCnt - 1; ++floorcnt)
                tmpGridInfo[index1].ChangeSmallGrid(tmpSmallGridCount[index1], 0, -1, floorcnt, false);
              break;
            }
            break;
        }
      }
      if (TargetKind == 7)
      {
        for (int floorcnt = 0; floorcnt <= Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt; ++floorcnt)
          tmpGridInfo[index1].SetInRoomSmallGrid(tmpSmallGridCount[index1], false, floorcnt);
      }
      tmpGridInfo[index1].ChangeSmallGridColor(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1]);
      tmpGridInfo[index1].SetCanRoofSmallGrid(tmpSmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, 0);
      if (TargetKind != 1)
        tmpGridInfo[index1].SetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], -1, true, false);
      else
        tmpGridInfo[index1].SetSmallGridPutElement(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt, tmpSmallGridCount[index1], -1, false, false);
    }
  }

  private bool DelConditionCheck(
    int GridCnt,
    List<GridInfo> GridInfo,
    List<int> SmallGridCount,
    int TargetKind,
    GameObject targetObj)
  {
    bool flag = false;
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    List<List<GameObject>> gameObjectListList = new List<List<GameObject>>();
    List<BuildPartsInfo> buildPartsInfoList = new List<BuildPartsInfo>();
    for (int index1 = 0; index1 < GridCnt; ++index1)
    {
      if (GridInfo[index1].GetStateSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 0)
        return false;
      switch (TargetKind)
      {
        case 1:
        case 2:
          if (SmallGridCount[index1] == 0)
          {
            if (Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt > 0)
            {
              int[] partOnSmallGrid = GridInfo[index1].GetPartOnSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1);
              foreach (int num in GridInfo[index1].GetStackWallOnSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt - 1))
              {
                if (num == 4)
                {
                  flag = true;
                  break;
                }
              }
              if (partOnSmallGrid[1] == 3 || flag)
                return false;
            }
            if (GridInfo[index1].GetStateSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt) == 1 || GridInfo[index1].GetPartOnSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[2] != -1)
              return false;
            break;
          }
          break;
        case 5:
          if (GridInfo[index1].GetPartOnSmallGrid(SmallGridCount[index1], Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)[4] != -1)
            return false;
          break;
        default:
          for (int index2 = 0; index2 < baseParts.Length; ++index2)
          {
            for (int index3 = 0; index3 < baseParts[index2].Count; ++index3)
            {
              if (baseParts[index2][index3].GetItemKind() == 8)
                gameObjectListList.Add(baseParts[index2][index3].GetList());
            }
          }
          for (int index2 = 0; index2 < gameObjectListList.Count; ++index2)
          {
            for (int index3 = 0; index3 < gameObjectListList[index2].Count; ++index3)
            {
              BuildPartsInfo buildPartsInfo = this.partsMgr.GetBuildPartsInfo(gameObjectListList[index2][index3]);
              if (buildPartsInfo.nPutFloor == Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt)
                buildPartsInfoList.Add(buildPartsInfo);
            }
          }
          for (int index2 = 0; index2 < buildPartsInfoList.Count; ++index2)
          {
            for (int index3 = 0; index3 < buildPartsInfoList[index2].putGridInfos.Count; ++index3)
            {
              if (!Object.op_Inequality((Object) ((Component) buildPartsInfoList[index2].putGridInfos[index3]).get_gameObject(), (Object) ((Component) GridInfo[index1]).get_gameObject()) && buildPartsInfoList[index2].putSmallGridInfos[index3] == SmallGridCount[index1] && targetObj.get_transform().get_position().y < ((Component) buildPartsInfoList[index2]).get_gameObject().get_transform().get_position().y)
                return false;
            }
          }
          break;
      }
    }
    return true;
  }
}
