// Decompiled with JetBrains decompiler
// Type: AIProject.MiniMapControler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.Definitions;
using AIProject.Player;
using IllusionUtility.SetUtility;
using Manager;
using ReMotion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class MiniMapControler : MonoBehaviour
  {
    public static string RoadPath = "list/map/minimap/";
    public GameObject ActionAreaImage;
    [Header("それぞれの軌跡")]
    public GameObject Trajectory;
    public GameObject[] TrajectoryGirl;
    public GameObject TrajectoryMerchant;
    public GameObject TrajectoryPet;
    [Header("それぞれのCanvas")]
    public Canvas PlayerIcon;
    public Canvas GirlIcon;
    public Canvas MerchantIcon;
    public Canvas PetIcon;
    public Canvas BaseIcon;
    public Canvas DeviceIcon;
    public Canvas FarmIcon;
    public Canvas EventIcon;
    public Canvas LockIcon;
    public Canvas TutorialSearchIcon;
    public Canvas ShipIcon;
    public Canvas PlayerlookArea;
    public Canvas CraftIcon;
    public Canvas JukeIcon;
    [Header("カメラ")]
    public GameObject MiniMap;
    public GameObject MiniMapIcon;
    public GameObject AllAreaMap;
    public GameObject AllAreaIconMap;
    [Header("ミニマップの描画エリア")]
    [SerializeField]
    private GameObject ShowMiniMapArea;
    [Header("全体マップの描画エリア")]
    [SerializeField]
    private GameObject ShowAllMapArea;
    [SerializeField]
    private CanvasGroup AllAreaMapCanvasGroup;
    private Transform AllAreaMapCanvasGroupTrans;
    [SerializeField]
    [Header("表示する道")]
    private GameObject Road;
    [SerializeField]
    private GameObject DefRoad;
    private Dictionary<int, GameObject> Roads;
    [HideInInspector]
    public MinimapNavimesh RoadNaviMesh;
    [Header("アイコンを置くためのヒエラルキー位置")]
    public GameObject PlayerIconArea;
    public GameObject GirlIconArea;
    public GameObject MerchantIconArea;
    public GameObject PetIconArea;
    public GameObject ActionIconArea;
    public GameObject BaseIconArea;
    public GameObject DeviceIconArea;
    public GameObject FarmIconArea;
    public GameObject EventIconArea;
    public GameObject LockIconArea;
    public GameObject TutorialSearchIconArea;
    public GameObject ShipIconArea;
    public GameObject CraftIconArea;
    public GameObject JukeIconArea;
    [Header("軌跡の設定")]
    public float PutTrajectoryTime;
    public float TrajectoryExistTime;
    [Space]
    public float fOffSetY;
    [Space]
    public bool FromHomeMenu;
    public MiniMapControler.OnWarp WarpProc;
    private TrajectoryPool TrajectoryPool;
    private TrajectoryPool[] TrajectoryGirlPool;
    private TrajectoryPool TrajectoryMerchantPool;
    private List<TrajectoryPool> TrajectoryPetPool;
    private AllAreaMapUI allAreaMapUI;
    private List<Canvas> GirlIcons;
    private List<MiniMapControler.PetIconInfo> PetIcons;
    private List<MiniMapControler.IconInfo> BaseIcons;
    private List<MiniMapControler.IconInfo> DeviceIcons;
    private List<MiniMapControler.IconInfo> FarmIcons;
    private List<MiniMapControler.IconInfo> FarmHousingIcons;
    private List<MiniMapControler.IconInfo> EventIcons;
    private List<Canvas> LockIcons;
    private List<MiniMapControler.TutorialSearchIconInfo> TutorialSearchIcons;
    private List<MiniMapControler.IconInfo> ShipIcons;
    private List<MiniMapControler.IconInfo> CraftIcons;
    private List<MiniMapControler.IconInfo> JukeIcons;
    private ActionPoint[] actionPoints;
    private ActionPoint[] actionPointsHousing;
    private BasePoint[] basePoints;
    private DevicePoint[] devicePoints;
    private FarmPoint[] farmPoints;
    private FarmPoint[] farmPointsHousing;
    private List<EventPoint> eventPoints;
    private ShipPoint[] shipPoints;
    private CraftPoint[] craftPoints;
    private JukePoint[] jukePoints;
    private List<MiniMapControler.PointIconInfo> actionPointIcon;
    private List<MiniMapControler.PointIconInfo> actionPointHousingIcon;
    private int nGirlCount;
    private int nPetCount;
    private float fTimer;
    private float PlayerLookAreaWidth;
    private GameObject IconObj;
    private RenderTexture MiniRenderTex;
    private RenderTexture AllRenderTex;
    private Dictionary<int, List<Vector3>> LastPos;
    private AllAreaCameraControler allArea;
    private EventType playerMask;
    private List<EventType> PlayerActionEvents;
    private Manager.Input input;
    private PlayerActor Player;
    private List<AnimalBase> Pets;
    private Transform icon;
    private List<GameObject> trajePool;
    private Vector3[] CalcIconPos;
    private bool endInit;
    private AllAreaMapObjects AllAreaMapObjects;
    private Dictionary<int, Dictionary<int, MinimapNavimesh.AreaGroupInfo>> areaGroupTable;
    public int VisibleMode;
    public int prevVisibleMode;
    public bool nowCloseAllMap;
    public bool TutorialLockRelease;
    private Dictionary<int, bool> tmpAgentNullCheckTable;
    private GameObject commonSpace;
    private Dictionary<int, AgentActor> sortedDic;
    private const int nTrajectoryNum = 3;
    private const int nPosYoffset = 10;
    private const float fLookPosYoffset = 13f;
    private DefinePack.MinimapUI MinimapUIDefine;
    private bool _prevMinimapConfig;
    private int _visibleModeMinimapConfigOFF;
    private IDisposable CamActivateSubscriber;
    public System.Action AllMapClosedAction;

    public MiniMapControler()
    {
      base.\u002Ector();
    }

    public void Init(int MapID = -1)
    {
      this.input = Singleton<Manager.Input>.Instance;
      this.Player = Singleton<Manager.Map>.Instance.Player;
      this.allAreaMapUI = MapUIContainer.AllAreaMapUI;
      if (this.MinimapUIDefine == null)
        this.MinimapUIDefine = Singleton<Resources>.Instance.DefinePack.MinimapUIDefine;
      this.areaGroupTable = Singleton<Resources>.Instance.Map.AreaGroupTable;
      this.AllAreaMapObjects = (AllAreaMapObjects) ((Component) this.allAreaMapUI).GetComponent<AllAreaMapObjects>();
      this.ShowMiniMapArea = MapUIContainer.MiniMapRenderTex;
      this.ShowAllMapArea = this.AllAreaMapObjects.ShowAllMapArea;
      this.AllAreaMapCanvasGroup = this.AllAreaMapObjects.AllAreaMapCanvasGroup;
      this.AllAreaMapCanvasGroupTrans = ((Component) this.AllAreaMapCanvasGroup).get_transform();
      this.SetMiniMapCamera();
      ((Camera) this.MiniMap.GetComponent<Camera>()).RemoveAllCommandBuffers();
      ((MiniMapDepthTexture) this.MiniMap.GetComponent<MiniMapDepthTexture>()).Initialize();
      Dictionary<int, MiniMapControler.MinimapInfo> minimapInfoTable = Singleton<Resources>.Instance.Map.MinimapInfoTable;
      int key = MapID >= 0 ? MapID : Singleton<Manager.Map>.Instance.MapID;
      if (this.Roads == null)
        this.Roads = new Dictionary<int, GameObject>();
      if (Object.op_Equality((Object) this.commonSpace, (Object) null))
        this.commonSpace = GameObject.Find("CommonSpace");
      Transform transform = this.commonSpace.get_transform();
      MiniMapControler.MinimapInfo minimapInfo;
      if (minimapInfoTable.TryGetValue(key, out minimapInfo) && GlobalMethod.AssetFileExist(minimapInfo.assetPath, minimapInfo.asset, minimapInfo.manifest))
      {
        if (!this.Roads.ContainsKey(key))
        {
          GameObject gameObject = CommonLib.LoadAsset<GameObject>(minimapInfo.assetPath, minimapInfo.asset, true, minimapInfo.manifest);
          gameObject.get_transform().SetParent(transform);
          gameObject.get_transform().set_localPosition(Vector3.get_zero());
          gameObject.get_transform().set_localRotation(Quaternion.get_identity());
          this.Roads.Add(key, gameObject);
          AssetBundleManager.UnloadAssetBundle(minimapInfo.assetPath, false, minimapInfo.manifest, false);
        }
        else if (Object.op_Equality((Object) this.Roads[key], (Object) null))
        {
          this.Roads[key] = CommonLib.LoadAsset<GameObject>(minimapInfo.assetPath, minimapInfo.asset, true, minimapInfo.manifest);
          this.Roads[key].get_transform().SetParent(transform);
          this.Roads[key].get_transform().set_localPosition(Vector3.get_zero());
          this.Roads[key].get_transform().set_localRotation(Quaternion.get_identity());
          AssetBundleManager.UnloadAssetBundle(minimapInfo.assetPath, false, minimapInfo.manifest, false);
        }
      }
      if (this.Roads.TryGetValue(key, out this.Road) && Object.op_Inequality((Object) this.Road, (Object) null))
      {
        this.Road.get_transform().SetLocalPositionY(this.fOffSetY);
        this.RoadNaviMesh = (MinimapNavimesh) this.Road.GetComponent<MinimapNavimesh>();
        this.RoadNaviMesh.Init();
      }
      else if (key == 0 && Object.op_Inequality((Object) this.DefRoad, (Object) null))
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.DefRoad, transform);
        gameObject.get_transform().set_localPosition(Vector3.get_zero());
        gameObject.get_transform().set_localRotation(Quaternion.get_identity());
        gameObject.get_transform().SetLocalPositionY(this.fOffSetY);
        this.RoadNaviMesh = (MinimapNavimesh) gameObject.GetComponent<MinimapNavimesh>();
        this.RoadNaviMesh.Init();
        this.Roads.Add(key, gameObject);
      }
      using (Dictionary<int, GameObject>.Enumerator enumerator = this.Roads.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) null))
          {
            if (current.Key - key != 0)
              current.Value.SetActive(false);
            else
              current.Value.SetActive(true);
          }
        }
      }
      this.playerMask = Singleton<Resources>.Instance.DefinePack.MapDefines.PlayerEventMask;
      this.playerMask = this.playerMask | EventType.Toilet | EventType.Bath | EventType.Wash | EventType.Eat | EventType.DressIn | EventType.Play | EventType.Lesbian;
      if (this.PlayerActionEvents != null)
        this.PlayerActionEvents.Clear();
      for (int index = 0; index < 24; ++index)
      {
        if (((int) this.playerMask >> index & 1) == 1)
          this.PlayerActionEvents.Add((EventType) (1 << index));
      }
      this.actionPoints = Singleton<Manager.Map>.Instance.PointAgent.ActionPoints;
      this.basePoints = Singleton<Manager.Map>.Instance.PointAgent.BasePoints;
      this.devicePoints = Singleton<Manager.Map>.Instance.PointAgent.DevicePoints;
      this.farmPoints = Singleton<Manager.Map>.Instance.PointAgent.FarmPoints;
      this.actionPointsHousing = Singleton<Manager.Housing>.Instance.ActionPoints;
      this.farmPointsHousing = Singleton<Manager.Housing>.Instance.FarmPoints;
      this.eventPoints = new List<EventPoint>((IEnumerable<EventPoint>) Singleton<Manager.Map>.Instance.PointAgent.EventPoints);
      this.TutorialSearchIcons = new List<MiniMapControler.TutorialSearchIconInfo>();
      this.shipPoints = Singleton<Manager.Map>.Instance.PointAgent.ShipPoints;
      this.craftPoints = Singleton<Manager.Housing>.Instance.CraftPoints;
      this.jukePoints = Singleton<Manager.Housing>.Instance.JukePoints;
      this.SetActionPointIcons();
      this.SetBasePointIcons();
      this.SetDevicePointIcons();
      this.SetFarmPointIcons();
      this.SetEventPointIcons();
      this.SetActionHousingIcons();
      this.SetFarmHousingIcons();
      this.SetShipPointIcons();
      this.SetCraftPointIcons();
      this.SetJukePointIcons();
      this.icon = this.PlayerIconArea.get_transform();
      this.TrajectoryPool = new TrajectoryPool();
      this.TrajectoryPool.CreatePool(this.Trajectory, 3, this.icon);
      this.trajePool = this.TrajectoryPool.getList();
      for (int index = 0; index < 3; ++index)
        ((global::Trajectory) this.trajePool[index].GetComponent<global::Trajectory>()).Init(this.TrajectoryExistTime);
      this.tmpAgentNullCheckTable.Clear();
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          this.tmpAgentNullCheckTable.Add(current.Key, Object.op_Equality((Object) current.Value, (Object) null));
        }
      }
      this.GirlIconInit(this.icon, this.trajePool);
      this.icon = this.MerchantIconArea.get_transform();
      ((Component) this.MerchantIcon).get_gameObject().SetActive(Singleton<Manager.Map>.Instance.Merchant.CurrentMode != Merchant.ActionType.Absent);
      if (Object.op_Inequality((Object) this.TrajectoryMerchant, (Object) null))
      {
        this.TrajectoryMerchantPool = new TrajectoryPool();
        this.TrajectoryMerchantPool.CreatePool(this.TrajectoryMerchant, 3, this.icon);
        this.trajePool = this.TrajectoryMerchantPool.getList();
        for (int index = 0; index < 3; ++index)
          ((global::Trajectory) this.trajePool[index].GetComponent<global::Trajectory>()).Init(this.TrajectoryExistTime);
      }
      this.PetIconInit(this.icon, this.trajePool);
      this.SetLastPos();
      this.IconObj = (GameObject) null;
      this.fTimer = 0.0f;
      this.PlayerLookAreaWidth = (float) ((RectTransform) ((Component) this.PlayerlookArea).GetComponent<RectTransform>()).get_sizeDelta().y;
      ((MiniMapCameraMove) this.MiniMap.GetComponent<MiniMapCameraMove>()).Init();
      this.SetAllMapCamera();
      this.allArea = (AllAreaCameraControler) this.AllAreaMap.GetComponent<AllAreaCameraControler>();
      this.allArea.Init();
      this.allArea.SetCameraCommandBuffer();
      this.allAreaMapUI.Init(this, this.AllAreaMap);
      this.endInit = true;
    }

    public void MinimapLockAreaInit()
    {
      if (this.LockIcons != null)
      {
        using (List<Canvas>.Enumerator enumerator = this.LockIcons.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Canvas current = enumerator.Current;
            if (!Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
              Object.Destroy((Object) ((Component) current).get_gameObject());
          }
        }
        this.LockIcons.Clear();
      }
      GameObject tutorialLockAreaObject = Singleton<Manager.Map>.Instance.TutorialLockAreaObject;
      if (Object.op_Inequality((Object) tutorialLockAreaObject, (Object) null))
      {
        LockArea[] componentsInChildren = (LockArea[]) tutorialLockAreaObject.GetComponentsInChildren<LockArea>();
        if (componentsInChildren != null)
        {
          this.LockIcons = new List<Canvas>();
          for (int index = 0; index < componentsInChildren.Length; ++index)
          {
            Canvas canvas = (Canvas) Object.Instantiate<Canvas>((M0) this.LockIcon, this.LockIconArea.get_transform());
            Vector3 position = ((Component) componentsInChildren[index]).get_transform().get_position();
            ref Vector3 local = ref position;
            local.y = (__Null) (local.y + (double) this.fOffSetY);
            ((Component) canvas).get_transform().set_position(position);
            Quaternion identity = Quaternion.get_identity();
            ((Quaternion) ref identity).set_eulerAngles(new Vector3(90f, 0.0f, 0.0f));
            ((Component) canvas).get_transform().set_rotation(identity);
            this.LockIcons.Add(canvas);
          }
        }
        this.TutorialLockRelease = false;
      }
      else
        this.TutorialLockRelease = true;
    }

    public void MinimapTutorialActionPointInit(TutorialSearchActionPoint point, int iconID)
    {
      Sprite sprite = (Sprite) null;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(iconID, out sprite);
      if (Object.op_Equality((Object) sprite, (Object) null))
        return;
      Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.TutorialSearchIcon, this.TutorialSearchIconArea.get_transform());
      ((Image) ((Component) _icon).GetComponentInChildren<Image>()).set_sprite(sprite);
      Vector3 position = ((Component) point).get_transform().get_position();
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y + (double) this.fOffSetY);
      ((Component) _icon).get_transform().set_position(position);
      Quaternion identity = Quaternion.get_identity();
      ((Quaternion) ref identity).set_eulerAngles(new Vector3(90f, 180f, 0.0f));
      ((Component) _icon).get_transform().set_rotation(identity);
      if (!((Component) _icon).get_gameObject().get_activeSelf())
        ((Component) _icon).get_gameObject().SetActive(true);
      int index1 = -1;
      for (int index2 = 0; index2 < this.TutorialSearchIcons.Count; ++index2)
      {
        if (!Object.op_Inequality((Object) this.TutorialSearchIcons[index2].Point, (Object) point))
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
        this.TutorialSearchIcons.Add(new MiniMapControler.TutorialSearchIconInfo(_icon, point));
      else
        this.TutorialSearchIcons[index1] = new MiniMapControler.TutorialSearchIconInfo(_icon, point);
    }

    public void MinimapTutorialActionPointDestroy(TutorialSearchActionPoint point)
    {
      if (this.TutorialSearchIcons == null || this.TutorialSearchIcons.Count == 0)
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < this.TutorialSearchIcons.Count; ++index2)
      {
        if (this.TutorialSearchIcons[index2] != null && !Object.op_Inequality((Object) this.TutorialSearchIcons[index2].Point, (Object) point))
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1 || this.TutorialSearchIcons[index1] == null || Object.op_Equality((Object) this.TutorialSearchIcons[index1].Icon, (Object) null))
        return;
      Object.Destroy((Object) ((Component) this.TutorialSearchIcons[index1].Icon).get_gameObject());
      this.TutorialSearchIcons.RemoveAt(index1);
    }

    private void GirlIconInit(Transform icon, List<GameObject> trajePool)
    {
      icon = this.GirlIconArea.get_transform();
      this.nGirlCount = 0;
      using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (!Object.op_Equality((Object) enumerator.Current, (Object) null))
            ++this.nGirlCount;
        }
      }
      if (this.GirlIcons != null && this.GirlIcons.Count > 0)
      {
        for (int index = 0; index < this.GirlIcons.Count; ++index)
        {
          Object.Destroy((Object) ((Component) this.GirlIcons[index]).get_gameObject());
          this.GirlIcons[index] = (Canvas) null;
        }
      }
      if (this.TrajectoryGirlPool != null && this.TrajectoryGirlPool.Length > 0)
      {
        for (int index1 = 0; index1 < this.TrajectoryGirlPool.Length; ++index1)
        {
          List<GameObject> list = this.TrajectoryGirlPool[index1].getList();
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            Object.Destroy((Object) list[index2].get_gameObject());
            list[index2] = (GameObject) null;
          }
        }
      }
      this.GirlIcons = new List<Canvas>();
      this.TrajectoryGirlPool = new TrajectoryPool[this.nGirlCount];
      this.sortedDic = this.SortGirlDictionary();
      if (this.sortedDic == null)
        return;
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this.sortedDic)
      {
        if (!Object.op_Equality((Object) keyValuePair.Value, (Object) null))
        {
          int count = this.GirlIcons.Count;
          this.GirlIcons.Add((Canvas) Object.Instantiate<Canvas>((M0) this.GirlIcon));
          ((Image) ((Component) this.GirlIcons[count]).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActorIconTable[keyValuePair.Value.ID]);
          ((Component) this.GirlIcons[count]).get_transform().SetParent(icon, false);
          ((Component) this.GirlIcons[count]).get_transform().set_localScale(Vector3.get_one());
          ((Component) this.GirlIcons[count]).get_gameObject().SetActive(true);
          this.TrajectoryGirlPool[count] = new TrajectoryPool();
          this.TrajectoryGirlPool[count].CreatePool(this.TrajectoryGirl[keyValuePair.Key], 3, icon);
          trajePool = this.TrajectoryGirlPool[count].getList();
          for (int index1 = 0; index1 < 3; ++index1)
          {
            int index2 = index1;
            ((global::Trajectory) trajePool[index2].GetComponent<global::Trajectory>()).Init(this.TrajectoryExistTime);
          }
        }
      }
    }

    private void PetIconInit(Transform icon, List<GameObject> trajePool)
    {
      icon = this.PetIconArea.get_transform();
      List<AnimalBase> petAnimals = Singleton<AnimalManager>.Instance.PetAnimals;
      if (this.PetIcons != null && this.PetIcons.Count > 0)
      {
        for (int index = 0; index < this.PetIcons.Count; ++index)
        {
          ((Component) this.PetIcons[index].Icon).get_gameObject().SetActive(false);
          Object.Destroy((Object) ((Component) this.PetIcons[index].Icon).get_gameObject());
          this.PetIcons[index].Icon = (Canvas) null;
        }
      }
      if (this.TrajectoryPetPool != null && this.TrajectoryPetPool.Count > 0)
      {
        for (int index1 = 0; index1 < this.TrajectoryPetPool.Count; ++index1)
        {
          List<GameObject> list = this.TrajectoryPetPool[index1].getList();
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            Object.Destroy((Object) list[index2].get_gameObject());
            list[index2] = (GameObject) null;
          }
        }
      }
      this.nPetCount = 0;
      this.Pets.Clear();
      foreach (AnimalBase animalBase in petAnimals)
      {
        if (((Component) animalBase).get_gameObject().get_activeSelf() && animalBase.AnimalTypeID == 0)
        {
          this.Pets.Add(animalBase);
          ++this.nPetCount;
        }
      }
      this.PetIcons = new List<MiniMapControler.PetIconInfo>();
      this.TrajectoryPetPool = new List<TrajectoryPool>();
      for (int index1 = 0; index1 < this.nPetCount; ++index1)
      {
        this.PetIcons.Add(new MiniMapControler.PetIconInfo((Canvas) Object.Instantiate<Canvas>((M0) this.PetIcon), this.Pets[index1].Name, ((Component) this.Pets[index1]).get_gameObject()));
        ((Component) this.PetIcons[index1].Icon).get_transform().SetParent(icon);
        ((Component) this.PetIcons[index1].Icon).get_transform().set_localScale(Vector3.get_one());
        ((Component) this.PetIcons[index1].Icon).get_gameObject().SetActive(true);
        Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Category, 9, (Image) ((Component) this.PetIcons[index1].Icon).GetComponentInChildren<Image>(), true);
        this.TrajectoryPetPool.Add(new TrajectoryPool());
        this.TrajectoryPetPool[index1].CreatePool(this.TrajectoryPet, 3, icon);
        trajePool = this.TrajectoryPetPool[index1].getList();
        for (int index2 = 0; index2 < 3; ++index2)
        {
          int index3 = index2;
          ((global::Trajectory) trajePool[index3].GetComponent<global::Trajectory>()).Init(this.TrajectoryExistTime);
        }
      }
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.SetPetPointIcon();
    }

    public void PetIconSet()
    {
      this.PetIconInit(this.icon, this.trajePool);
      this.SetLastPos();
    }

    private void SetLastPos()
    {
      this.LastPos = new Dictionary<int, List<Vector3>>();
      this.LastPos.Add(0, new List<Vector3>());
      if (Object.op_Inequality((Object) this.PlayerIcon, (Object) null))
        this.LastPos[0].Add(((Component) this.PlayerIcon).get_transform().get_position());
      this.LastPos.Add(1, new List<Vector3>());
      if (this.GirlIcons != null)
      {
        for (int index = 0; index < this.GirlIcons.Count; ++index)
          this.LastPos[1].Add(((Component) this.GirlIcons[index]).get_transform().get_position());
      }
      this.LastPos.Add(2, new List<Vector3>());
      if (Object.op_Inequality((Object) this.MerchantIcon, (Object) null))
        this.LastPos[2].Add(((Component) this.MerchantIcon).get_transform().get_position());
      this.LastPos.Add(3, new List<Vector3>());
      for (int index = 0; index < this.nPetCount; ++index)
      {
        if (!Object.op_Equality((Object) this.PetIcons[index].Icon, (Object) null))
          this.LastPos[3].Add(((Component) this.PetIcons[index].Icon).get_transform().get_position());
      }
    }

    private void Update()
    {
      if (!Singleton<Manager.Map>.IsInstance() || !this.endInit)
        return;
      if (this._prevMinimapConfig != Manager.Config.GameData.MiniMap)
      {
        this._prevMinimapConfig = Manager.Config.GameData.MiniMap;
        if (Manager.Config.GameData.MiniMap)
        {
          if (!this.MiniMap.get_activeSelf() && this.VisibleMode == 0)
            this.OpenMiniMap();
        }
        else if (!Manager.Config.GameData.MiniMap)
        {
          this._visibleModeMinimapConfigOFF = this.VisibleMode;
          if (this.MiniMap.get_activeSelf())
            this.CloseMiniMap();
        }
      }
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Inequality((Object) player, (Object) null))
      {
        this.IconMove(this.PlayerIcon, player.Position, player.Rotation, false);
        if (this.MiniMap.get_activeSelf())
          this.LookAreaMove(player);
      }
      bool flag = false;
      if (this.tmpAgentNullCheckTable.Count != Singleton<Manager.Map>.Instance.AgentTable.get_Count())
      {
        flag = true;
      }
      else
      {
        using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, AgentActor> current = enumerator.Current;
            if (!this.tmpAgentNullCheckTable.ContainsKey(current.Key))
            {
              flag = true;
              break;
            }
            if (this.tmpAgentNullCheckTable[current.Key] != Object.op_Equality((Object) current.Value, (Object) null))
            {
              flag = true;
              break;
            }
          }
        }
      }
      if (flag)
      {
        this.GirlIconInit(this.icon, this.trajePool);
        this.SetLastPos();
        this.tmpAgentNullCheckTable.Clear();
        using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, AgentActor> current = enumerator.Current;
            this.tmpAgentNullCheckTable.Add(current.Key, Object.op_Equality((Object) current.Value, (Object) null));
          }
        }
      }
      int num = 0;
      this.CheckRefleshGirls();
      if (this.sortedDic != null)
      {
        foreach (AgentActor agentActor in this.sortedDic.Values)
        {
          if (!Object.op_Equality((Object) agentActor, (Object) null))
          {
            Vector3 position = agentActor.Position;
            position.y = player.Position.y;
            this.IconMove(this.GirlIcons[num++], position, agentActor.Rotation, true);
          }
        }
      }
      MerchantActor merchant = Singleton<Manager.Map>.Instance.Merchant;
      if (Object.op_Inequality((Object) merchant, (Object) null))
      {
        if (!this.TutorialLockRelease)
          ((Component) this.MerchantIcon).get_gameObject().SetActive(false);
        else if (merchant.CurrentMode != Merchant.ActionType.Absent)
        {
          if (!((Component) this.MerchantIcon).get_gameObject().get_activeSelf())
            ((Component) this.MerchantIcon).get_gameObject().SetActive(true);
          Vector3 position = merchant.Position;
          position.y = player.Position.y;
          this.IconMove(this.MerchantIcon, position, merchant.Rotation, true);
        }
        else
          ((Component) this.MerchantIcon).get_gameObject().SetActive(false);
      }
      for (int index = 0; index < this.nPetCount; ++index)
        this.IconMove(this.PetIcons[index].Icon, this.Pets[index].Position, this.Pets[index].Rotation, true);
      this.fTimer += Time.get_deltaTime();
      if (this.LastPos == null)
        return;
      if ((double) this.fTimer >= (double) this.PutTrajectoryTime)
      {
        this.SetTrajectory(this.LastPos[0][0], this.PlayerIcon, this.TrajectoryPool);
        Image[] girlsIcon = this.GetGirlsIcon();
        if (girlsIcon != null)
        {
          for (int index = 0; index < this.nGirlCount; ++index)
          {
            if (((Behaviour) girlsIcon[index]).get_enabled())
              this.SetTrajectory(this.LastPos[1][index], this.GirlIcons[index], this.TrajectoryGirlPool[index]);
          }
        }
        if (((Component) this.MerchantIcon).get_gameObject().get_activeSelf() && ((Behaviour) this.GetMerchantIcon()).get_enabled())
          this.SetTrajectory(this.LastPos[2][0], this.MerchantIcon, this.TrajectoryMerchantPool);
        for (int index = 0; index < this.nPetCount; ++index)
          this.SetTrajectory(this.LastPos[3][index], this.PetIcons[index].Icon, this.TrajectoryPetPool[index]);
        this.fTimer = 0.0f;
      }
      if (this.VisibleMode == 1 && ((Behaviour) this.allArea).get_isActiveAndEnabled() && !this.nowCloseAllMap && (this.input.IsPressedKey(ActionID.MouseRight) || this.input.IsPressedKey(ActionID.Cancel)))
      {
        this.AllMapClosedAction += (System.Action) (() =>
        {
          if (this.prevVisibleMode != 0 || !Manager.Config.GameData.MiniMap)
            return;
          this.OpenMiniMap();
        });
        this.ChangeCamera(true, false);
        this.WarpMoveDispose();
      }
      if (Object.op_Inequality((Object) this.PlayerIcon, (Object) null))
        this.LastPos[0][0] = ((Component) this.PlayerIcon).get_transform().get_position();
      for (int index = 0; index < this.nGirlCount; ++index)
      {
        if (Object.op_Inequality((Object) this.GirlIcons[index], (Object) null))
          this.LastPos[1][index] = ((Component) this.GirlIcons[index]).get_transform().get_position();
      }
      if (Object.op_Inequality((Object) this.MerchantIcon, (Object) null))
        this.LastPos[2][0] = ((Component) this.MerchantIcon).get_transform().get_position();
      for (int index = 0; index < this.nPetCount; ++index)
      {
        if (this.PetIcons[index] != null)
          this.LastPos[3][index] = ((Component) this.PetIcons[index].Icon).get_transform().get_position();
      }
      this.IconVisibleChange();
      this.ClearCheck();
      if (!this.TutorialLockRelease && this.LockIcons != null)
      {
        if (!Object.op_Equality((Object) Singleton<Manager.Map>.Instance.TutorialLockAreaObject, (Object) null))
          return;
        for (int index = 0; index < this.LockIcons.Count; ++index)
        {
          if (!Object.op_Equality((Object) this.LockIcons[index], (Object) null))
          {
            Object.Destroy((Object) ((Component) this.LockIcons[index]).get_gameObject());
            this.LockIcons[index] = (Canvas) null;
          }
        }
        this.LockIcons = (List<Canvas>) null;
        this.TutorialLockRelease = true;
      }
      else
      {
        if (this.TutorialLockRelease || this.LockIcons != null)
          return;
        this.TutorialLockRelease = true;
      }
    }

    private void IconVisibleChange()
    {
      Dictionary<int, MinimapNavimesh.AreaGroupInfo> areaGroupInfo = this.GetAreaGroupInfo(Singleton<Manager.Map>.Instance.MapID);
      if (areaGroupInfo == null)
        return;
      foreach (KeyValuePair<int, MinimapNavimesh.AreaGroupInfo> keyValuePair in areaGroupInfo)
      {
        for (int index = 0; index < this.actionPointIcon.Count; ++index)
        {
          if (!Object.op_Equality((Object) this.actionPointIcon[index].Point, (Object) null) && !Object.op_Equality((Object) ((Component) this.actionPointIcon[index].Point).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.actionPointIcon[index].Point).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.actionPointIcon[index].Point.OwnerArea, (Object) null))
            {
              this.actionPointIcon[index].Icon.SetActive(activeSelf);
            }
            else
            {
              int areaId = this.actionPointIcon[index].Point.OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key] & !this.actionPointIcon[index].Point.TutorialHideMode();
                this.actionPointIcon[index].Icon.SetActive(flag);
              }
            }
          }
        }
        for (int index = 0; index < this.basePoints.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.basePoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.basePoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.basePoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.basePoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.BaseIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.basePoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key] & !((BasePoint) ((Component) this.BaseIcons[index].Point).GetComponent<BasePoint>()).TutorialHideMode();
                ((Component) this.BaseIcons[index].Icon).get_gameObject().SetActive(flag);
              }
            }
          }
        }
        for (int index = 0; index < this.devicePoints.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.devicePoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.devicePoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.devicePoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.devicePoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.DeviceIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.devicePoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key] & !((DevicePoint) ((Component) this.DeviceIcons[index].Point).GetComponent<DevicePoint>()).TutorialHideMode();
                ((Component) this.DeviceIcons[index].Icon).get_gameObject().SetActive(flag);
              }
            }
          }
        }
        for (int index = 0; index < this.farmPoints.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.farmPoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.farmPoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.farmPoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.farmPoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.FarmIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.farmPoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key] & !((FarmPoint) ((Component) this.FarmIcons[index].Point).GetComponent<FarmPoint>()).TutorialHideMode();
                ((Component) this.FarmIcons[index].Icon).get_gameObject().SetActive(flag);
              }
            }
          }
        }
        for (int index = 0; index < this.eventPoints.Count; ++index)
        {
          if (!Object.op_Equality((Object) this.eventPoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.eventPoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.eventPoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.eventPoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.EventIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.eventPoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag1 = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key];
                EventPoint component = (EventPoint) ((Component) this.EventIcons[index].Point).GetComponent<EventPoint>();
                bool flag2 = ((flag1 ? 1 : 0) & (!component.IsNeutralCommand ? 0 : (component.TargetPoint ? 1 : 0))) != 0;
                ((Component) this.EventIcons[index].Icon).get_gameObject().SetActive(flag2);
              }
            }
          }
        }
        for (int index = 0; index < this.craftPoints.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.craftPoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.craftPoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.craftPoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.craftPoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.CraftIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.craftPoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key] & !((CraftPoint) ((Component) this.CraftIcons[index].Point).GetComponent<CraftPoint>()).TutorialHideMode();
                ((Component) this.CraftIcons[index].Icon).get_gameObject().SetActive(flag);
              }
            }
          }
        }
        JukePoint jukePoint = (JukePoint) null;
        for (int index = 0; index < this.jukePoints.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.jukePoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.jukePoints[index]).get_gameObject(), (Object) null))
          {
            bool activeSelf = ((Component) this.jukePoints[index]).get_gameObject().get_activeSelf();
            if (Object.op_Equality((Object) this.jukePoints[index].OwnerArea, (Object) null))
            {
              ((Component) this.JukeIcons[index].Icon).get_gameObject().SetActive(activeSelf);
            }
            else
            {
              int areaId = this.jukePoints[index].OwnerArea.AreaID;
              if (keyValuePair.Value.areaID.Contains(areaId))
              {
                bool flag1 = activeSelf & this.RoadNaviMesh.areaGroupActive[keyValuePair.Key];
                jukePoint = (JukePoint) ((Component) this.JukeIcons[index].Point).GetComponent<JukePoint>();
                bool flag2 = flag1 & !Manager.Map.TutorialMode;
                ((Component) this.JukeIcons[index].Icon).get_gameObject().SetActive(flag2);
              }
            }
          }
        }
      }
    }

    private void ClearCheck()
    {
      ShipPoint shipPoint = (ShipPoint) null;
      for (int index = 0; index < this.shipPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.shipPoints[index], (Object) null) && !Object.op_Equality((Object) ((Component) this.shipPoints[index]).get_gameObject(), (Object) null))
        {
          bool activeSelf = ((Component) this.shipPoints[index]).get_gameObject().get_activeSelf();
          this.allAreaMapUI.GameClear = Singleton<Game>.Instance.WorldData.Cleared;
          bool flag = activeSelf & this.allAreaMapUI.GameClear;
          shipPoint = (ShipPoint) ((Component) this.ShipIcons[index].Point).GetComponent<ShipPoint>();
          ((Component) this.ShipIcons[index].Icon).get_gameObject().SetActive(flag);
        }
      }
    }

    private void IconMove(Canvas Icon, Vector3 pos, Quaternion rot, bool ignoreRot = false)
    {
      Vector3 vector3 = pos;
      Camera component1 = (Camera) this.MiniMapIcon.GetComponent<Camera>();
      Camera component2 = (Camera) this.AllAreaIconMap.GetComponent<Camera>();
      if (Object.op_Inequality((Object) component1, (Object) null) && ((Behaviour) component1).get_isActiveAndEnabled())
        vector3.y = (__Null) (this.MiniMap.get_transform().get_position().y - 1.0);
      else if (Object.op_Inequality((Object) component2, (Object) null) && ((Behaviour) component2).get_isActiveAndEnabled())
        vector3.y = (__Null) (((Component) component2).get_transform().get_position().y - 1.0);
      ((Component) Icon).get_transform().set_position(vector3);
      if (ignoreRot)
        return;
      ((Component) Icon).get_transform().set_rotation(Quaternion.op_Multiply(rot, Quaternion.LookRotation(new Vector3(0.0f, -1f, 0.0f))));
    }

    private void SetTrajectory(Vector3 lastPos, Canvas Target, TrajectoryPool pool)
    {
      this.CalcIconPos[0] = ((Component) Target).get_transform().get_position();
      this.CalcIconPos[1] = lastPos;
      Vector3 vector3 = Vector3.op_Division(Vector3.op_Subtraction(this.CalcIconPos[0], this.CalcIconPos[1]), Time.get_deltaTime());
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < 0.5)
        return;
      this.IconObj = pool.GetObject();
      if (Object.op_Equality((Object) this.IconObj.GetComponent<global::Trajectory>(), (Object) null))
        this.IconObj.AddComponent<global::Trajectory>();
      Vector3 position = ((Component) Target).get_transform().get_position();
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y - 1.0);
      ((global::Trajectory) this.IconObj.GetComponent<global::Trajectory>()).Set(position, ((Component) Target).get_transform().get_rotation());
    }

    private void DeadTrajectory(TrajectoryPool pool)
    {
      using (List<GameObject>.Enumerator enumerator = pool.getList().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (current.get_activeSelf())
          {
            current.SetActive(false);
            ((global::Trajectory) current.GetComponent<global::Trajectory>())?.Dead();
          }
        }
      }
    }

    public void SetMiniMapCamera()
    {
      if (!this.MiniMap.get_activeSelf() && Manager.Config.GameData.MiniMap)
      {
        this.MiniMap.SetActive(true);
        this.VisibleMode = 0;
      }
      if (Object.op_Equality((Object) this.MiniRenderTex, (Object) null))
        this.MiniRenderTex = new RenderTexture(256, 256, 24, (RenderTextureFormat) 0);
      this.ShowMiniMapArea.SetActive(this.MiniMap.get_activeSelf());
      ((Camera) this.MiniMap.GetComponent<Camera>()).set_targetTexture(this.MiniRenderTex);
      ((Camera) this.MiniMapIcon.GetComponent<Camera>()).set_targetTexture(this.MiniRenderTex);
      ((RawImage) this.ShowMiniMapArea.GetComponent<RawImage>()).set_texture((Texture) this.MiniRenderTex);
    }

    public void SetAllMapCamera()
    {
      if (this.AllAreaMap.get_activeSelf())
        this.AllAreaMap.SetActive(false);
      if (Object.op_Equality((Object) this.AllRenderTex, (Object) null))
        this.AllRenderTex = new RenderTexture(Screen.get_width(), Screen.get_height(), 24, (RenderTextureFormat) 0);
      this.ShowAllMapArea.SetActive(true);
      ((Camera) this.AllAreaMap.GetComponent<Camera>()).set_targetTexture(this.AllRenderTex);
      ((Camera) this.AllAreaIconMap.GetComponent<Camera>()).set_targetTexture(this.AllRenderTex);
      ((RawImage) this.ShowAllMapArea.GetComponent<RawImage>()).set_texture((Texture) this.AllRenderTex);
      this.ShowAllMapArea.SetActive(false);
    }

    public void DeadMiniMap()
    {
      this.MiniMap.SetActive(false);
      this.MiniMapIcon.SetActive(false);
      this.ShowMiniMapArea.SetActive(false);
      Object.Destroy((Object) this.MiniRenderTex);
      this.MiniRenderTex = (RenderTexture) null;
      this.AllAreaMap.SetActive(false);
      this.AllAreaIconMap.SetActive(false);
      this.ShowAllMapArea.SetActive(false);
      Object.Destroy((Object) this.AllRenderTex);
      this.AllRenderTex = (RenderTexture) null;
    }

    public void ChangeCamera(bool MouseRight = false, bool WarpUI = false)
    {
      bool flag = Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null);
      if (!this.AllAreaMap.get_activeSelf() || !WarpUI && flag)
        return;
      this.nowCloseAllMap = true;
      this.allArea.InitPosition();
      this.allArea.ShowAllIcon();
      if (!this.FromHomeMenu)
      {
        this.input.ReserveState(Manager.Input.ValidType.Action);
        this.input.FocusLevel = 0;
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(true);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
      }
      if (!this.CheckEndDelegate("SetupState"))
        this.AllMapClosedAction += new System.Action(this.SetupState);
      int from = 1;
      int to = 0;
      if (this.CamActivateSubscriber != null)
        this.CamActivateSubscriber.Dispose();
      this.CamActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), true), (System.Action<M0>) (x => this.AllAreaMapCanvasGroup.set_alpha(Mathf.Lerp((float) from, (float) to, ((TimeInterval<float>) ref x).get_Value()))), (System.Action<Exception>) (ex => Debug.LogException(ex)), (System.Action) (() =>
      {
        this.VisibleMode = !Manager.Config.GameData.MiniMap ? this._visibleModeMinimapConfigOFF : 2;
        if (this.AllMapClosedAction != null)
          this.AllMapClosedAction();
        this.AllMapClosedAction = (System.Action) null;
        this.AllAreaMap.SetActive(false);
        this.ShowAllMapArea.SetActive(false);
        MapUIContainer.SetVisibleHUD(true);
        this.nowCloseAllMap = false;
      }));
      this.AllAreaMapCanvasGroup.set_blocksRaycasts(false);
      if (!this.FromHomeMenu)
      {
        ((Behaviour) this.Player).set_enabled(true);
        ((Behaviour) this.Player.CameraControl).set_enabled(true);
        if (!this.CheckEndDelegate("PlayerNomalChange"))
          this.AllMapClosedAction += new System.Action(this.PlayerNomalChange);
      }
      this.DeadTrajectory(this.TrajectoryPool);
      for (int index = 0; index < this.TrajectoryGirlPool.Length; ++index)
        this.DeadTrajectory(this.TrajectoryGirlPool[index]);
      this.DeadTrajectory(this.TrajectoryMerchantPool);
      for (int index = 0; index < this.TrajectoryPetPool.Count; ++index)
        this.DeadTrajectory(this.TrajectoryPetPool[index]);
    }

    public void OpenAllMap(int prevMode = -1)
    {
      this.allArea.InitPosition();
      this.AllAreaMap.SetActive(true);
      this.allArea.Restart();
      this.allAreaMapUI.Refresh();
      this.ShowAllMapArea.SetActive(true);
      this.prevVisibleMode = prevMode;
      CanvasGroup minimapCanvasGroup = (CanvasGroup) ((Component) this.ShowMiniMapArea.get_transform().get_parent()).GetComponent<CanvasGroup>();
      if (this.MiniMap.get_activeSelf())
        this.CloseMiniMap();
      int from = 0;
      float fromMiniMap = minimapCanvasGroup.get_alpha();
      int to = 1;
      int toMiniMap = 0;
      if (this.CamActivateSubscriber != null)
        this.CamActivateSubscriber.Dispose();
      this.CamActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(1f, true), true), (System.Action<M0>) (x =>
      {
        this.AllAreaMapCanvasGroup.set_alpha(Mathf.Lerp((float) from, (float) to, ((TimeInterval<float>) ref x).get_Value()));
        ((Graphic) this.AllAreaMapObjects.Cursor).set_color(new Color((float) ((Graphic) this.AllAreaMapObjects.Cursor).get_color().r, (float) ((Graphic) this.AllAreaMapObjects.Cursor).get_color().g, (float) ((Graphic) this.AllAreaMapObjects.Cursor).get_color().b, 0.0f));
        minimapCanvasGroup.set_alpha(Mathf.Lerp(fromMiniMap, (float) toMiniMap, ((TimeInterval<float>) ref x).get_Value()));
      }), (System.Action<Exception>) (ex => Debug.LogException(ex)), (System.Action) (() => this.allArea.CursorSet()));
      this.AllAreaMapCanvasGroup.set_blocksRaycasts(true);
      ((Component) this.PlayerlookArea).get_gameObject().SetActive(false);
      this.DeadTrajectory(this.TrajectoryPool);
      for (int index = 0; index < this.TrajectoryGirlPool.Length; ++index)
        this.DeadTrajectory(this.TrajectoryGirlPool[index]);
      this.DeadTrajectory(this.TrajectoryMerchantPool);
      for (int index = 0; index < this.TrajectoryPetPool.Count; ++index)
        this.DeadTrajectory(this.TrajectoryPetPool[index]);
    }

    public void OpenMiniMap()
    {
      ((MiniMapCameraMove) this.MiniMap.GetComponent<MiniMapCameraMove>()).Init();
      this.MiniMap.SetActive(true);
      this.ShowMiniMapArea.SetActive(true);
      this.allArea.ShowAllIcon();
      this.prevVisibleMode = -1;
      this.VisibleMode = 0;
      CanvasGroup minimapCanvasGroup = (CanvasGroup) ((Component) this.ShowMiniMapArea.get_transform().get_parent()).GetComponent<CanvasGroup>();
      float from = minimapCanvasGroup.get_alpha();
      int to = 1;
      if (this.CamActivateSubscriber != null)
        this.CamActivateSubscriber.Dispose();
      this.CamActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(1f, true), true), (System.Action<M0>) (x => minimapCanvasGroup.set_alpha(Mathf.Lerp(from, (float) to, ((TimeInterval<float>) ref x).get_Value()))), (System.Action<Exception>) (ex => Debug.LogException(ex)));
      ((Component) this.PlayerlookArea).get_gameObject().SetActive(true);
      this.DeadTrajectory(this.TrajectoryPool);
      for (int index = 0; index < this.TrajectoryGirlPool.Length; ++index)
        this.DeadTrajectory(this.TrajectoryGirlPool[index]);
      this.DeadTrajectory(this.TrajectoryMerchantPool);
      for (int index = 0; index < this.TrajectoryPetPool.Count; ++index)
        this.DeadTrajectory(this.TrajectoryPetPool[index]);
    }

    public void CloseMiniMap()
    {
      this.MiniMap.SetActive(false);
      this.ShowMiniMapArea.SetActive(false);
      ((Component) this.PlayerlookArea).get_gameObject().SetActive(false);
      this.DeadTrajectory(this.TrajectoryPool);
      for (int index = 0; index < this.TrajectoryGirlPool.Length; ++index)
        this.DeadTrajectory(this.TrajectoryGirlPool[index]);
      this.DeadTrajectory(this.TrajectoryMerchantPool);
      for (int index = 0; index < this.TrajectoryPetPool.Count; ++index)
        this.DeadTrajectory(this.TrajectoryPetPool[index]);
    }

    private void LookAreaMove(PlayerActor player)
    {
      Quaternion rotation = ((Component) player.CameraControl.CameraComponent).get_gameObject().get_transform().get_rotation();
      Vector3 position = player.Position;
      rotation.x = (__Null) 0.0;
      rotation.z = (__Null) 0.0;
      Camera component1 = (Camera) this.MiniMapIcon.GetComponent<Camera>();
      Camera component2 = (Camera) this.AllAreaIconMap.GetComponent<Camera>();
      if (Object.op_Inequality((Object) component1, (Object) null) && ((Behaviour) component1).get_isActiveAndEnabled())
        position.y = (__Null) (this.MiniMap.get_transform().get_position().y - 0.5);
      else if (Object.op_Inequality((Object) component2, (Object) null) && ((Behaviour) component2).get_isActiveAndEnabled())
        position.y = (__Null) (((Component) component2).get_transform().get_position().y - 0.5);
      ref Vector3 local = ref position;
      local.z = (__Null) (local.z + (double) this.PlayerLookAreaWidth / 2.0);
      ((Component) this.PlayerlookArea).get_transform().set_position(position);
      ((Component) this.PlayerlookArea).get_transform().RotateAround(player.Position, Vector3.get_up(), (float) ((Quaternion) ref rotation).get_eulerAngles().y);
      ((Component) this.PlayerlookArea).get_transform().set_rotation(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(new Vector3(0.0f, 1f, 0.0f))));
    }

    private void SetActionPointIcons()
    {
      Transform transform = this.ActionIconArea.get_transform();
      int key = -1;
      foreach (MiniMapControler.PointIconInfo pointIconInfo in this.actionPointIcon)
        Object.Destroy((Object) pointIconInfo.Icon.get_gameObject());
      this.actionPointIcon.Clear();
      for (int index1 = 0; index1 < this.actionPoints.Length; ++index1)
      {
        int index2 = index1;
        int num = -1;
        for (int index3 = 0; index3 < this.PlayerActionEvents.Count; ++index3)
        {
          if (this.actionPoints[index2].PlayerEventType.Contains(this.PlayerActionEvents[index3]) || this.actionPoints[index2].AgentEventType.Contains(this.PlayerActionEvents[index3]))
          {
            num = index3;
            break;
          }
          if (this.actionPoints[index2].PlayerDateEventType[0].Contains(this.PlayerActionEvents[index3]) || this.actionPoints[index2].PlayerDateEventType[1].Contains(this.PlayerActionEvents[index3]))
          {
            num = index3;
            break;
          }
        }
        if (num >= 0)
        {
          bool flag = this.actionPoints[index2].IDList.IsNullOrEmpty<int>();
          if ((Singleton<Resources>.Instance.itemIconTables.MiniMapIcon.TryGetValue(this.actionPoints[index2].ID, out key) || !flag) && (flag || Singleton<Resources>.Instance.itemIconTables.MiniMapIcon.TryGetValue(this.actionPoints[index2].IDList[0], out key)))
          {
            Vector3 position = this.actionPoints[index2].Position;
            ref Vector3 local = ref position;
            local.y = (__Null) (local.y + (double) this.fOffSetY);
            if (key != -1)
            {
              string empty;
              if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(key, out empty))
                empty = string.Empty;
              switch (key)
              {
                case 0:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.BED, empty));
                  break;
                case 1:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.CHAIR, empty));
                  break;
                case 2:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.DESK, empty));
                  break;
                case 3:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.COOK, empty));
                  break;
                case 4:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.TOILET, empty));
                  break;
                case 5:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.BATH, empty));
                  break;
                case 6:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.SHOWER, empty));
                  break;
                case 7:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.STORAGE, empty));
                  break;
                case 10:
                case 11:
                case 12:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.PICKEL, empty));
                  break;
                case 13:
                case 14:
                case 15:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.SHOVEL, empty));
                  break;
                case 16:
                case 17:
                case 18:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.NET, empty));
                  break;
                case 19:
                case 20:
                case 21:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.GLOVE, empty));
                  break;
                case 28:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.WATERPET, empty));
                  break;
                case 31:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.CLOTHET, empty));
                  break;
                case 32:
                  this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.CLOTHETBOX, empty));
                  break;
                case 34:
                  if (Game.isAdd01)
                  {
                    this.actionPointIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPoints[index2], MapUIActionCategory.H, empty));
                    break;
                  }
                  continue;
              }
              int index3 = this.actionPointIcon.Count - 1;
              if (!Object.op_Inequality((Object) this.actionPointIcon[index3].Point, (Object) this.actionPoints[index2]))
              {
                this.actionPointIcon[index3].Icon.get_transform().SetParent(transform, false);
                this.actionPointIcon[index3].Icon.get_transform().set_localScale(Vector3.get_one());
                this.actionPointIcon[index3].Icon.get_transform().set_position(position);
                Image componentInChildren = (Image) this.actionPointIcon[index3].Icon.GetComponentInChildren<Image>();
                componentInChildren.set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[key]);
                if (Object.op_Inequality((Object) componentInChildren.get_sprite(), (Object) null))
                  ((Graphic) componentInChildren).set_color(Color.get_white());
              }
            }
          }
        }
      }
    }

    public void SetActionPointHousingIcons()
    {
      this.SetActionHousingIcons();
      this.SetFarmHousingIcons();
      this.SetCraftPointIcons();
      this.SetJukePointIcons();
    }

    public void SetActionHousingIcons()
    {
      Transform transform = this.ActionIconArea.get_transform();
      int key = -1;
      foreach (MiniMapControler.PointIconInfo pointIconInfo in this.actionPointHousingIcon)
        Object.Destroy((Object) pointIconInfo.Icon.get_gameObject());
      this.actionPointHousingIcon.Clear();
      this.actionPointsHousing = Singleton<Manager.Housing>.Instance.ActionPoints;
      for (int index1 = 0; index1 < this.actionPointsHousing.Length; ++index1)
      {
        int index2 = index1;
        int num = -1;
        for (int index3 = 0; index3 < this.PlayerActionEvents.Count; ++index3)
        {
          if (this.actionPointsHousing[index2].PlayerEventType.Contains(this.PlayerActionEvents[index3]) || this.actionPointsHousing[index2].AgentEventType.Contains(this.PlayerActionEvents[index3]))
          {
            num = index3;
            break;
          }
          if (this.actionPointsHousing[index2].PlayerDateEventType[0].Contains(this.PlayerActionEvents[index3]) || this.actionPointsHousing[index2].PlayerDateEventType[1].Contains(this.PlayerActionEvents[index3]))
          {
            num = index3;
            break;
          }
        }
        if (num >= 0)
        {
          bool flag = this.actionPointsHousing[index2].IDList.IsNullOrEmpty<int>();
          if ((Singleton<Resources>.Instance.itemIconTables.MiniMapIcon.TryGetValue(this.actionPointsHousing[index2].ID, out key) || !flag) && (flag || Singleton<Resources>.Instance.itemIconTables.MiniMapIcon.TryGetValue(this.actionPointsHousing[index2].IDList[0], out key)))
          {
            Vector3 position = this.actionPointsHousing[index2].Position;
            ref Vector3 local = ref position;
            local.y = (__Null) (local.y + (double) this.fOffSetY);
            if (key != -1)
            {
              string empty;
              if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(key, out empty))
                empty = string.Empty;
              switch (key)
              {
                case 0:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.BED, empty));
                  break;
                case 1:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.CHAIR, empty));
                  break;
                case 2:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.DESK, empty));
                  break;
                case 3:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.COOK, empty));
                  break;
                case 4:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.TOILET, empty));
                  break;
                case 5:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.BATH, empty));
                  break;
                case 6:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.SHOWER, empty));
                  break;
                case 7:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.STORAGE, empty));
                  break;
                case 10:
                case 11:
                case 12:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.PICKEL, empty));
                  break;
                case 13:
                case 14:
                case 15:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.SHOVEL, empty));
                  break;
                case 16:
                case 17:
                case 18:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.NET, empty));
                  break;
                case 19:
                case 20:
                case 21:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.GLOVE, empty));
                  break;
                case 28:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.WATERPET, empty));
                  break;
                case 31:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.CLOTHET, empty));
                  break;
                case 32:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.CLOTHETBOX, empty));
                  break;
                case 34:
                  if (Game.isAdd01)
                  {
                    this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.H, empty));
                    break;
                  }
                  continue;
                case 37:
                case 38:
                case 39:
                case 40:
                  this.actionPointHousingIcon.Add(new MiniMapControler.PointIconInfo((GameObject) Object.Instantiate<GameObject>((M0) this.ActionAreaImage), this.actionPointsHousing[index2], MapUIActionCategory.WARP, empty));
                  break;
              }
              int index3 = this.actionPointHousingIcon.Count - 1;
              if (!Object.op_Inequality((Object) this.actionPointHousingIcon[index3].Point, (Object) this.actionPointsHousing[index2]))
              {
                this.actionPointHousingIcon[index3].Icon.get_transform().SetParent(transform, false);
                this.actionPointHousingIcon[index3].Icon.get_transform().set_localScale(Vector3.get_one());
                this.actionPointHousingIcon[index3].Icon.get_transform().set_position(position);
                Image componentInChildren = (Image) this.actionPointHousingIcon[index3].Icon.GetComponentInChildren<Image>();
                componentInChildren.set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[key]);
                if (Object.op_Inequality((Object) componentInChildren.get_sprite(), (Object) null))
                  ((Graphic) componentInChildren).set_color(Color.get_white());
              }
            }
          }
        }
      }
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.RefleshActionHousingIcon();
    }

    private void SetBasePointIcons()
    {
      Transform transform = this.BaseIconArea.get_transform();
      if (this.BaseIcons != null)
      {
        foreach (MiniMapControler.IconInfo baseIcon in this.BaseIcons)
          Object.Destroy((Object) ((Component) baseIcon.Icon).get_gameObject());
      }
      this.BaseIcons = new List<MiniMapControler.IconInfo>();
      int baseIconId = this.MinimapUIDefine.BaseIconID;
      for (int index1 = 0; index1 < this.basePoints.Length; ++index1)
      {
        int index2 = index1;
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.BaseIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.BaseName.TryGetValue(this.basePoints[index2].ID, out _name))
          _name = string.Format("拠点{0}", (object) index1);
        this.BaseIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.basePoints[index2]));
        ((Image) ((Component) this.BaseIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[baseIconId]);
        ((Component) this.BaseIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.BaseIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.basePoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.BaseIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.BaseIcons[index2].Icon).get_gameObject().SetActive(true);
      }
    }

    private void SetDevicePointIcons()
    {
      Transform transform = this.DeviceIconArea.get_transform();
      if (this.DeviceIcons != null)
      {
        foreach (MiniMapControler.IconInfo deviceIcon in this.DeviceIcons)
          Object.Destroy((Object) ((Component) deviceIcon.Icon).get_gameObject());
      }
      this.DeviceIcons = new List<MiniMapControler.IconInfo>();
      int deviceIconId = this.MinimapUIDefine.DeviceIconID;
      for (int index1 = 0; index1 < this.devicePoints.Length; ++index1)
      {
        int index2 = index1;
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.DeviceIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(deviceIconId, out _name))
          _name = "端末";
        this.DeviceIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.devicePoints[index2]));
        ((Image) ((Component) this.DeviceIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[deviceIconId]);
        ((Component) this.DeviceIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.DeviceIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.devicePoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.DeviceIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.DeviceIcons[index2].Icon).get_gameObject().SetActive(true);
      }
    }

    private void SetFarmPointIcons()
    {
      Transform transform = this.FarmIconArea.get_transform();
      int num = -1;
      if (this.FarmIcons != null)
      {
        foreach (MiniMapControler.IconInfo farmIcon in this.FarmIcons)
          Object.Destroy((Object) ((Component) farmIcon.Icon).get_gameObject());
      }
      this.FarmIcons = new List<MiniMapControler.IconInfo>();
      num = this.MinimapUIDefine.FarmIconID;
      for (int index1 = 0; index1 < this.farmPoints.Length; ++index1)
      {
        int index2 = index1;
        int key;
        if (this.farmPoints[index2].Kind == FarmPoint.FarmKind.Plant)
          key = this.MinimapUIDefine.FarmIconID;
        else if (this.farmPoints[index2].Kind == FarmPoint.FarmKind.ChickenCoop)
        {
          key = this.MinimapUIDefine.ChickenIconID;
        }
        else
        {
          num = -1;
          continue;
        }
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.FarmIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(key, out _name))
        {
          if (this.farmPoints[index2].Kind == FarmPoint.FarmKind.Plant)
            _name = "畑";
          else if (this.farmPoints[index2].Kind == FarmPoint.FarmKind.ChickenCoop)
            _name = "鶏小屋";
        }
        this.FarmIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.farmPoints[index2]));
        ((Image) ((Component) this.FarmIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[key]);
        ((Component) this.FarmIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.FarmIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.farmPoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.FarmIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.FarmIcons[index2].Icon).get_gameObject().SetActive(true);
      }
    }

    private void SetFarmHousingIcons()
    {
      Transform transform = this.FarmIconArea.get_transform();
      int num = -1;
      if (this.FarmHousingIcons != null)
      {
        foreach (MiniMapControler.IconInfo farmHousingIcon in this.FarmHousingIcons)
          Object.Destroy((Object) ((Component) farmHousingIcon.Icon).get_gameObject());
      }
      this.FarmHousingIcons = new List<MiniMapControler.IconInfo>();
      this.farmPointsHousing = Singleton<Manager.Housing>.Instance.FarmPoints;
      for (int index1 = 0; index1 < this.farmPointsHousing.Length; ++index1)
      {
        int index2 = index1;
        int key;
        if (this.farmPointsHousing[index2].Kind == FarmPoint.FarmKind.Plant)
          key = this.MinimapUIDefine.FarmIconID;
        else if (this.farmPointsHousing[index2].Kind == FarmPoint.FarmKind.ChickenCoop)
        {
          key = this.MinimapUIDefine.ChickenIconID;
        }
        else
        {
          num = -1;
          continue;
        }
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.FarmIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(key, out _name))
        {
          if (this.farmPointsHousing[index2].Kind == FarmPoint.FarmKind.Plant)
            _name = "畑";
          else if (this.farmPointsHousing[index2].Kind == FarmPoint.FarmKind.ChickenCoop)
            _name = "鶏小屋";
        }
        this.FarmHousingIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.farmPointsHousing[index2]));
        ((Image) ((Component) this.FarmHousingIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[key]);
        ((Component) this.FarmHousingIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.FarmHousingIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.farmPointsHousing[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.FarmHousingIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.FarmHousingIcons[index2].Icon).get_gameObject().SetActive(true);
      }
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.RefleshFarmHousingIcon();
    }

    private void SetEventPointIcons()
    {
      Transform transform = this.EventIconArea.get_transform();
      if (this.EventIcons != null)
      {
        foreach (MiniMapControler.IconInfo eventIcon in this.EventIcons)
          Object.Destroy((Object) ((Component) eventIcon.Icon).get_gameObject());
      }
      this.EventIcons = new List<MiniMapControler.IconInfo>();
      for (int index1 = 0; index1 < this.eventPoints.Count; ++index1)
      {
        int index2 = index1;
        this.EventIcons.Add(new MiniMapControler.IconInfo((Canvas) Object.Instantiate<Canvas>((M0) this.EventIcon), "？？？", (Point) this.eventPoints[index2]));
        ((Component) this.EventIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.EventIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.eventPoints[index2].Position;
        ref Vector3 local1 = ref position;
        local1.y = (__Null) (local1.y + ((double) this.fOffSetY + 5.0));
        ref Vector3 local2 = ref position;
        local2.z = (__Null) (local2.z - ((RectTransform) ((Component) this.EventIcons[index2].Icon).GetComponent<RectTransform>()).get_sizeDelta().y / 4.0);
        ((Component) this.EventIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.EventIcons[index2].Icon).get_gameObject().SetActive(true);
      }
    }

    private void SetShipPointIcons()
    {
      Transform transform = this.ShipIconArea.get_transform();
      if (this.ShipIcons != null)
      {
        foreach (MiniMapControler.IconInfo shipIcon in this.ShipIcons)
          Object.Destroy((Object) ((Component) shipIcon.Icon).get_gameObject());
      }
      this.ShipIcons = new List<MiniMapControler.IconInfo>();
      for (int index1 = 0; index1 < this.shipPoints.Length; ++index1)
      {
        int index2 = index1;
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.ShipIcon);
        int shipIconId = Singleton<Resources>.Instance.CommonDefine.Icon.ShipIconID;
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(shipIconId, out _name))
          _name = "船";
        this.ShipIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.shipPoints[index2]));
        ((Image) ((Component) this.ShipIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[shipIconId]);
        ((Component) this.ShipIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.ShipIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.shipPoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.ShipIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.ShipIcons[index2].Icon).get_gameObject().SetActive(true);
      }
    }

    private void SetCraftPointIcons()
    {
      Transform transform = this.CraftIconArea.get_transform();
      int num = -1;
      if (this.CraftIcons != null)
      {
        foreach (MiniMapControler.IconInfo craftIcon in this.CraftIcons)
          Object.Destroy((Object) ((Component) craftIcon.Icon).get_gameObject());
      }
      this.CraftIcons = new List<MiniMapControler.IconInfo>();
      this.craftPoints = Singleton<Manager.Housing>.Instance.CraftPoints;
      for (int index1 = 0; index1 < this.craftPoints.Length; ++index1)
      {
        int index2 = index1;
        int key;
        if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Medicine)
          key = this.MinimapUIDefine.DragDeskIconID;
        else if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Pet)
          key = this.MinimapUIDefine.PetUnionIconID;
        else if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Recycling)
        {
          key = this.MinimapUIDefine.RecycleIconID;
        }
        else
        {
          num = -1;
          continue;
        }
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.CraftIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(key, out _name))
        {
          if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Medicine)
            _name = "薬台";
          else if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Pet)
            _name = "ペット合成装置";
          else if (this.craftPoints[index2].Kind == CraftPoint.CraftKind.Recycling)
            _name = "リサイクル装置";
        }
        this.CraftIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.craftPoints[index2]));
        ((Image) ((Component) this.CraftIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[key]);
        ((Component) this.CraftIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.CraftIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.craftPoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.CraftIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.CraftIcons[index2].Icon).get_gameObject().SetActive(true);
      }
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.RefleshCraftIcon();
    }

    private void SetJukePointIcons()
    {
      Transform transform = this.JukeIconArea.get_transform();
      if (this.JukeIcons != null)
      {
        foreach (MiniMapControler.IconInfo jukeIcon in this.JukeIcons)
          Object.Destroy((Object) ((Component) jukeIcon.Icon).get_gameObject());
      }
      this.JukeIcons = new List<MiniMapControler.IconInfo>();
      this.jukePoints = Singleton<Manager.Housing>.Instance.JukePoints;
      for (int index1 = 0; index1 < this.jukePoints.Length; ++index1)
      {
        int index2 = index1;
        int jukeIconId = this.MinimapUIDefine.JukeIconID;
        Canvas _icon = (Canvas) Object.Instantiate<Canvas>((M0) this.JukeIcon);
        string _name;
        if (!Singleton<Resources>.Instance.itemIconTables.MiniMapIconName.TryGetValue(jukeIconId, out _name))
          _name = "ジュークボックス";
        this.JukeIcons.Add(new MiniMapControler.IconInfo(_icon, _name, (Point) this.jukePoints[index2]));
        ((Image) ((Component) this.JukeIcons[index2].Icon).GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[jukeIconId]);
        ((Component) this.JukeIcons[index2].Icon).get_transform().SetParent(transform, false);
        ((Component) this.JukeIcons[index2].Icon).get_transform().set_localScale(Vector3.get_one());
        Vector3 position = this.jukePoints[index2].Position;
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.fOffSetY);
        ((Component) this.JukeIcons[index2].Icon).get_transform().set_position(position);
        ((Component) this.JukeIcons[index2].Icon).get_gameObject().SetActive(true);
      }
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.RefleshJukeIcon();
    }

    public Image[] GetGirlsIcon()
    {
      if (this.GirlIcons.Count <= 0)
        return (Image[]) null;
      Image[] imageArray = new Image[this.GirlIcons.Count];
      for (int index = 0; index < imageArray.Length; ++index)
        imageArray[index] = (Image) ((Component) this.GirlIcons[index]).GetComponentInChildren<Image>();
      return imageArray;
    }

    public Image GetMerchantIcon()
    {
      return (Image) ((Component) this.MerchantIcon).GetComponentInChildren<Image>();
    }

    public Canvas GetMerchantCanvas()
    {
      return this.MerchantIcon;
    }

    private bool CheckEndDelegate(string MethodName)
    {
      bool flag = false;
      if (this.AllMapClosedAction == null)
        return flag;
      foreach (Delegate invocation in this.AllMapClosedAction.GetInvocationList())
      {
        if (!(invocation.Method.Name != MethodName))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void PlayerNomalChange()
    {
      if (!(this.Player.Controller.State is WMap))
        return;
      if (this.Player.PlayerController.PrevStateName == "Onbu")
        this.Player.PlayerController.ChangeState("Onbu");
      else
        this.Player.PlayerController.ChangeState("Normal");
    }

    private void SetupState()
    {
      this.input.SetupState();
    }

    public Dictionary<int, MinimapNavimesh.AreaGroupInfo> GetAreaGroupInfo(
      int mapID)
    {
      Dictionary<int, MinimapNavimesh.AreaGroupInfo> dictionary = (Dictionary<int, MinimapNavimesh.AreaGroupInfo>) null;
      return !this.areaGroupTable.TryGetValue(mapID, out dictionary) ? (Dictionary<int, MinimapNavimesh.AreaGroupInfo>) null : dictionary;
    }

    public List<MiniMapControler.IconInfo> GetBaseIconInfos()
    {
      List<MiniMapControler.IconInfo> iconInfoList = (List<MiniMapControler.IconInfo>) null;
      if (this.BaseIcons != null)
        iconInfoList = new List<MiniMapControler.IconInfo>((IEnumerable<MiniMapControler.IconInfo>) this.BaseIcons);
      return iconInfoList;
    }

    public List<MiniMapControler.PointIconInfo> GetActionIconList()
    {
      return this.actionPointIcon;
    }

    public List<MiniMapControler.PointIconInfo> GetActionHousingIconList()
    {
      return this.actionPointHousingIcon;
    }

    public void IconAllDel()
    {
      foreach (MiniMapControler.PointIconInfo pointIconInfo in this.actionPointIcon)
      {
        if (!Object.op_Equality((Object) pointIconInfo.Icon, (Object) null) && !Object.op_Equality((Object) pointIconInfo.Icon.get_gameObject(), (Object) null))
          Object.Destroy((Object) pointIconInfo.Icon.get_gameObject());
      }
      this.actionPointIcon.Clear();
      foreach (MiniMapControler.PointIconInfo pointIconInfo in this.actionPointHousingIcon)
      {
        if (!Object.op_Equality((Object) pointIconInfo.Icon, (Object) null) && !Object.op_Equality((Object) pointIconInfo.Icon.get_gameObject(), (Object) null))
          Object.Destroy((Object) pointIconInfo.Icon.get_gameObject());
      }
      this.actionPointHousingIcon.Clear();
      using (List<Canvas>.Enumerator enumerator = this.GirlIcons.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Canvas current = enumerator.Current;
          if (!Object.op_Equality((Object) current, (Object) null) && !Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
            Object.Destroy((Object) ((Component) current).get_gameObject());
        }
      }
      this.GirlIcons.Clear();
      foreach (MiniMapControler.PetIconInfo petIcon in this.PetIcons)
      {
        if (!Object.op_Equality((Object) petIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) petIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) petIcon.Icon).get_gameObject());
      }
      this.PetIcons.Clear();
      foreach (MiniMapControler.IconInfo baseIcon in this.BaseIcons)
      {
        if (!Object.op_Equality((Object) baseIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) baseIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) baseIcon.Icon).get_gameObject());
      }
      this.BaseIcons.Clear();
      foreach (MiniMapControler.IconInfo deviceIcon in this.DeviceIcons)
      {
        if (!Object.op_Equality((Object) deviceIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) deviceIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) deviceIcon.Icon).get_gameObject());
      }
      this.DeviceIcons.Clear();
      foreach (MiniMapControler.IconInfo farmIcon in this.FarmIcons)
      {
        if (!Object.op_Equality((Object) farmIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) farmIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) farmIcon.Icon).get_gameObject());
      }
      this.FarmIcons.Clear();
      foreach (MiniMapControler.IconInfo farmHousingIcon in this.FarmHousingIcons)
      {
        if (!Object.op_Equality((Object) farmHousingIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) farmHousingIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) farmHousingIcon.Icon).get_gameObject());
      }
      this.FarmHousingIcons.Clear();
      foreach (MiniMapControler.IconInfo eventIcon in this.EventIcons)
      {
        if (!Object.op_Equality((Object) eventIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) eventIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) eventIcon.Icon).get_gameObject());
      }
      this.EventIcons.Clear();
      using (List<Canvas>.Enumerator enumerator = this.LockIcons.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Canvas current = enumerator.Current;
          if (!Object.op_Equality((Object) current, (Object) null) && !Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
            Object.Destroy((Object) ((Component) current).get_gameObject());
        }
      }
      this.LockIcons.Clear();
      foreach (MiniMapControler.TutorialSearchIconInfo tutorialSearchIcon in this.TutorialSearchIcons)
      {
        if (!Object.op_Equality((Object) tutorialSearchIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) tutorialSearchIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) tutorialSearchIcon.Icon).get_gameObject());
      }
      this.TutorialSearchIcons.Clear();
      foreach (MiniMapControler.IconInfo shipIcon in this.ShipIcons)
      {
        if (!Object.op_Equality((Object) shipIcon.Icon, (Object) null) && !Object.op_Equality((Object) ((Component) shipIcon.Icon).get_gameObject(), (Object) null))
          Object.Destroy((Object) ((Component) shipIcon.Icon).get_gameObject());
      }
      this.ShipIcons.Clear();
      if (!Object.op_Inequality((Object) this.allArea, (Object) null))
        return;
      this.allArea.AllIconReflesh();
    }

    public List<MiniMapControler.IconInfo> GetIconList(int kind)
    {
      switch (kind)
      {
        case 0:
          return this.BaseIcons;
        case 1:
          return this.DeviceIcons;
        case 2:
          return this.FarmIcons;
        case 3:
          return this.EventIcons;
        case 4:
          return this.FarmHousingIcons;
        case 5:
          return this.ShipIcons;
        case 6:
          return this.CraftIcons;
        case 7:
          return this.JukeIcons;
        default:
          return (List<MiniMapControler.IconInfo>) null;
      }
    }

    public List<MiniMapControler.PetIconInfo> GetPetIconList()
    {
      return this.PetIcons;
    }

    public void WarpMoveDispose()
    {
      if (this.allArea.WarpSelectSubscriber != null)
      {
        this.allArea.WarpSelectSubscriber.Dispose();
        this.allArea.WarpSelectSubscriber = (IDisposable) null;
      }
      if (this.allAreaMapUI.WarpSelectSubscriber == null)
        return;
      this.allAreaMapUI.WarpSelectSubscriber.Dispose();
      this.allAreaMapUI.WarpSelectSubscriber = (IDisposable) null;
    }

    public Dictionary<int, AgentActor> SortGirlDictionary()
    {
      Dictionary<int, AgentActor> dictionary = new Dictionary<int, AgentActor>();
      List<KeyValuePair<int, AgentActor>> keyValuePairList = new List<KeyValuePair<int, AgentActor>>((IEnumerable<KeyValuePair<int, AgentActor>>) Singleton<Manager.Map>.Instance.AgentTable);
      keyValuePairList.Sort((Comparison<KeyValuePair<int, AgentActor>>) ((a, b) => a.Key.CompareTo(b.Key)));
      for (int index = 0; index < keyValuePairList.Count; ++index)
        dictionary.Add(keyValuePairList[index].Key, keyValuePairList[index].Value);
      return dictionary;
    }

    private void CheckRefleshGirls()
    {
      bool flag = false;
      if (this.sortedDic.Count != Singleton<Manager.Map>.Instance.AgentTable.get_Count())
        flag = true;
      if (!flag)
      {
        foreach (KeyValuePair<int, AgentActor> keyValuePair in this.sortedDic)
        {
          if (!flag)
          {
            if (Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.AgentTable.get_Item(keyValuePair.Key), (Object) keyValuePair.Value))
              flag = true;
          }
          else
            break;
        }
      }
      if (!flag)
        return;
      this.sortedDic = this.SortGirlDictionary();
    }

    private void OnDestroy()
    {
      using (Dictionary<int, GameObject>.Enumerator enumerator = this.Roads.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) null))
            Object.Destroy((Object) this.Roads[current.Key]);
        }
      }
      this.Roads.Clear();
    }

    public delegate void OnWarp(BasePoint basePoint);

    public class PointIconInfo
    {
      public GameObject Icon;
      public ActionPoint Point;
      public MapUIActionCategory Category;
      public string Name;

      public PointIconInfo(
        GameObject _icon,
        ActionPoint _point,
        MapUIActionCategory _category,
        string _name)
      {
        this.Icon = _icon;
        this.Point = _point;
        this.Category = _category;
        this.Name = _name;
      }
    }

    public class IconInfo
    {
      public Canvas Icon;
      public Point Point;
      public string Name;

      public IconInfo(Canvas _icon, string _name, Point _point)
      {
        this.Icon = _icon;
        this.Name = _name;
        this.Point = _point;
      }
    }

    public class PetIconInfo
    {
      public Canvas Icon;
      public GameObject obj;
      public string Name;

      public PetIconInfo(Canvas _icon, string _name, GameObject _obj)
      {
        this.Icon = _icon;
        this.Name = _name;
        this.obj = _obj;
      }
    }

    public class TutorialSearchIconInfo
    {
      public Canvas Icon;
      public TutorialSearchActionPoint Point;

      public TutorialSearchIconInfo(Canvas _icon, TutorialSearchActionPoint _point)
      {
        this.Icon = _icon;
        this.Point = _point;
      }
    }

    public class MinimapInfo
    {
      public string assetPath;
      public string asset;
      public string manifest;
    }
  }
}
