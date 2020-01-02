// Decompiled with JetBrains decompiler
// Type: Studio.Studio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Elements.Xml;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public sealed class Studio : Singleton<Studio.Studio>
  {
    public Dictionary<TreeNodeObject, ObjectCtrlInfo> dicInfo = new Dictionary<TreeNodeObject, ObjectCtrlInfo>();
    public Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = new Dictionary<int, ObjectCtrlInfo>();
    public Dictionary<int, ChangeAmount> dicChangeAmount = new Dictionary<int, ChangeAmount>();
    public WorkInfo workInfo = new WorkInfo();
    public const string savePath = "studio/scene";
    [SerializeField]
    private TreeNodeCtrl m_TreeNodeCtrl;
    [SerializeField]
    private RootButtonCtrl m_RootButtonCtrl;
    [SerializeField]
    private ManipulatePanelCtrl _manipulatePanelCtrl;
    [SerializeField]
    private CameraControl m_CameraCtrl;
    [SerializeField]
    private SystemButtonCtrl m_SystemButtonCtrl;
    [SerializeField]
    private CameraLightCtrl m_CameraLightCtrl;
    [SerializeField]
    private MapList _mapList;
    [SerializeField]
    private ColorPalette _colorPalette;
    [SerializeField]
    private WorkspaceCtrl m_WorkspaceCtrl;
    [SerializeField]
    private BackgroundCtrl m_BackgroundCtrl;
    [SerializeField]
    private BackgroundList m_BackgroundList;
    [SerializeField]
    private PatternSelectListCtrl _patternSelectListCtrl;
    [SerializeField]
    private GameScreenShot _gameScreenShot;
    [SerializeField]
    private FrameCtrl _frameCtrl;
    [SerializeField]
    private FrameList _frameList;
    [SerializeField]
    private LogoList _logoList;
    [SerializeField]
    private InputField _inputFieldNow;
    [SerializeField]
    private TMP_InputField _inputFieldTMPNow;
    [Space]
    [SerializeField]
    private RectTransform rectName;
    [SerializeField]
    private TextMeshProUGUI textName;
    private SingleAssignmentDisposable disposableName;
    [Space]
    [SerializeField]
    private Sprite spriteLight;
    [Space]
    [SerializeField]
    private Image imageCamera;
    [SerializeField]
    private TextMeshProUGUI textCamera;
    [SerializeField]
    private CameraSelector _cameraSelector;
    [Space]
    [SerializeField]
    private Texture _textureLine;
    [SerializeField]
    private RouteControl _routeControl;
    private const string UserPath = "studio";
    private const string FileName = "option.xml";
    private const string RootName = "Option";
    private Control xmlCtrl;
    private OCICamera _ociCamera;
    public Action<ObjectCtrlInfo> onDelete;
    public Action onChangeMap;

    public TreeNodeCtrl treeNodeCtrl
    {
      get
      {
        return this.m_TreeNodeCtrl;
      }
    }

    public RootButtonCtrl rootButtonCtrl
    {
      get
      {
        return this.m_RootButtonCtrl;
      }
    }

    public ManipulatePanelCtrl manipulatePanelCtrl
    {
      get
      {
        return this._manipulatePanelCtrl;
      }
    }

    public CameraControl cameraCtrl
    {
      get
      {
        return this.m_CameraCtrl;
      }
    }

    public SystemButtonCtrl systemButtonCtrl
    {
      get
      {
        return this.m_SystemButtonCtrl;
      }
    }

    public BGMCtrl bgmCtrl
    {
      get
      {
        return this.sceneInfo.bgmCtrl;
      }
    }

    public ENVCtrl envCtrl
    {
      get
      {
        return this.sceneInfo.envCtrl;
      }
    }

    public OutsideSoundCtrl outsideSoundCtrl
    {
      get
      {
        return this.sceneInfo.outsideSoundCtrl;
      }
    }

    public CameraLightCtrl cameraLightCtrl
    {
      get
      {
        return this.m_CameraLightCtrl;
      }
    }

    public MapList mapList
    {
      get
      {
        return this._mapList;
      }
    }

    public ColorPalette colorPalette
    {
      get
      {
        return this._colorPalette;
      }
    }

    public PatternSelectListCtrl patternSelectListCtrl
    {
      get
      {
        return this._patternSelectListCtrl;
      }
    }

    public GameScreenShot gameScreenShot
    {
      get
      {
        return this._gameScreenShot;
      }
    }

    public FrameCtrl frameCtrl
    {
      get
      {
        return this._frameCtrl;
      }
    }

    public LogoList logoList
    {
      get
      {
        return this._logoList;
      }
    }

    public bool isInputNow
    {
      get
      {
        if (Object.op_Implicit((Object) this._inputFieldNow))
          return this._inputFieldNow.get_isFocused();
        return Object.op_Implicit((Object) this._inputFieldTMPNow) && this._inputFieldTMPNow.get_isFocused();
      }
    }

    public CameraSelector cameraSelector
    {
      get
      {
        return this._cameraSelector;
      }
    }

    public Texture textureLine
    {
      get
      {
        return this._textureLine;
      }
    }

    public RouteControl routeControl
    {
      get
      {
        return this._routeControl;
      }
    }

    public SceneInfo sceneInfo { get; private set; }

    public static OptionSystem optionSystem { get; private set; }

    public OCICamera ociCamera
    {
      get
      {
        return this._ociCamera;
      }
      private set
      {
        this._ociCamera = value;
      }
    }

    public int cameraCount { get; private set; }

    public bool isVRMode { get; private set; }

    public void AddFemale(string _path)
    {
      OCICharFemale ociCharFemale = AddObjectFemale.Add(_path);
      Singleton<UndoRedoManager>.Instance.Clear();
      if (Studio.Studio.optionSystem.autoHide)
        this.rootButtonCtrl.OnClick(-1);
      if (!Studio.Studio.optionSystem.autoSelect || ociCharFemale == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociCharFemale.treeNodeObject, true);
    }

    [DebuggerHidden]
    private IEnumerator AddFemaleCoroutine(string _path)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Studio.Studio.\u003CAddFemaleCoroutine\u003Ec__Iterator0()
      {
        _path = _path,
        \u0024this = this
      };
    }

    public void AddMale(string _path)
    {
      OCICharMale ociCharMale = AddObjectMale.Add(_path);
      Singleton<UndoRedoManager>.Instance.Clear();
      if (Studio.Studio.optionSystem.autoHide)
        this.rootButtonCtrl.OnClick(-1);
      if (!Studio.Studio.optionSystem.autoSelect || ociCharMale == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociCharMale.treeNodeObject, true);
    }

    public void AddMap(int _no, bool _close = true, bool _wait = true, bool _coroutine = true)
    {
      if (_coroutine)
      {
        this.StartCoroutine(this.AddMapCoroutine(_no, _close, _wait));
      }
      else
      {
        Singleton<Map>.Instance.LoadMap(_no);
        this.SetupMap(_no, _close);
      }
    }

    [DebuggerHidden]
    private IEnumerator AddMapCoroutine(int _no, bool _close, bool _wait)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Studio.Studio.\u003CAddMapCoroutine\u003Ec__Iterator1()
      {
        _no = _no,
        _wait = _wait,
        _close = _close,
        \u0024this = this
      };
    }

    private void SetupMap(int _no, bool _close)
    {
      this.sceneInfo.map = _no;
      this.sceneInfo.caMap.Reset();
      if (this.onChangeMap != null)
        this.onChangeMap();
      this.m_CameraCtrl.CloerListCollider();
      Info.MapLoadInfo mapLoadInfo = (Info.MapLoadInfo) null;
      if (Singleton<Info>.Instance.dicMapLoadInfo.TryGetValue(Singleton<Map>.Instance.no, out mapLoadInfo))
        this.m_CameraCtrl.LoadVanish(mapLoadInfo.vanish.bundlePath, mapLoadInfo.vanish.fileName, Singleton<Map>.Instance.MapRoot);
      if (!_close)
        return;
      this.rootButtonCtrl.OnClick(-1);
    }

    public void AddItem(int _group, int _category, int _no)
    {
      OCIItem ociItem = AddObjectItem.Add(_group, _category, _no);
      Singleton<UndoRedoManager>.Instance.Clear();
      if (Studio.Studio.optionSystem.autoHide)
        this.rootButtonCtrl.OnClick(-1);
      if (!Studio.Studio.optionSystem.autoSelect || ociItem == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociItem.treeNodeObject, true);
    }

    public void AddLight(int _no)
    {
      OCILight ociLight = AddObjectLight.Add(_no);
      Singleton<UndoRedoManager>.Instance.Clear();
      if (Studio.Studio.optionSystem.autoHide)
        this.rootButtonCtrl.OnClick(-1);
      if (!Studio.Studio.optionSystem.autoSelect || ociLight == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociLight.treeNodeObject, true);
    }

    public void AddFolder()
    {
      OCIFolder ociFolder = AddObjectFolder.Add();
      Singleton<UndoRedoManager>.Instance.Clear();
      if (!Studio.Studio.optionSystem.autoSelect || ociFolder == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociFolder.treeNodeObject, true);
    }

    public void AddCamera()
    {
      if (this.cameraCount != int.MaxValue)
        ++this.cameraCount;
      OCICamera ociCamera = AddObjectCamera.Add();
      Singleton<UndoRedoManager>.Instance.Clear();
      if (Studio.Studio.optionSystem.autoSelect && ociCamera != null)
        this.m_TreeNodeCtrl.SelectSingle(ociCamera.treeNodeObject, true);
      this._cameraSelector.Init();
    }

    public void ChangeCamera(OCICamera _ociCamera, bool _active, bool _force = false)
    {
      if (_active)
      {
        if (this.ociCamera != null && this.ociCamera != _ociCamera)
        {
          this.ociCamera.SetActive(false);
          this.ociCamera = (OCICamera) null;
        }
        if (_ociCamera != null)
        {
          _ociCamera.SetActive(true);
          this.ociCamera = _ociCamera;
        }
      }
      else if (_force)
      {
        if (this.ociCamera != null)
          this.ociCamera.SetActive(false);
        _ociCamera?.SetActive(false);
        this.ociCamera = (OCICamera) null;
      }
      else if (this.ociCamera == _ociCamera)
      {
        _ociCamera?.SetActive(false);
        this.ociCamera = (OCICamera) null;
      }
      Singleton<Studio.Studio>.Instance.cameraCtrl.IsOutsideSetting = this.ociCamera != null;
      ((TMP_Text) this.textCamera).set_text(this.ociCamera != null ? this.ociCamera.cameraInfo.name : "-");
      this._cameraSelector.SetCamera(this.ociCamera);
    }

    public void ChangeCamera(OCICamera _ociCamera)
    {
      this.ChangeCamera(_ociCamera, this.ociCamera != _ociCamera, false);
    }

    public void DeleteCamera(OCICamera _ociCamera)
    {
      if (this.ociCamera != _ociCamera)
      {
        this._cameraSelector.Init();
      }
      else
      {
        this.ociCamera.SetActive(false);
        this.ociCamera = (OCICamera) null;
        ((Behaviour) Singleton<Studio.Studio>.Instance.cameraCtrl).set_enabled(true);
        ((TMP_Text) this.textCamera).set_text("-");
        this._cameraSelector.Init();
      }
    }

    public void AddRoute()
    {
      OCIRoute ociRoute = AddObjectRoute.Add();
      if (this._routeControl.visible)
        this._routeControl.Init();
      Singleton<UndoRedoManager>.Instance.Clear();
      if (!Studio.Studio.optionSystem.autoSelect || ociRoute == null)
        return;
      this.m_TreeNodeCtrl.SelectSingle(ociRoute.treeNodeObject, true);
    }

    public void SetDepthOfFieldForcus(int _key)
    {
      this.m_SystemButtonCtrl.SetDepthOfFieldForcus(_key);
    }

    public void SetSunCaster(int _key)
    {
      this.m_SystemButtonCtrl.SetSunCaster(_key);
    }

    public void UpdateCharaFKColor()
    {
      foreach (KeyValuePair<int, ObjectCtrlInfo> keyValuePair in this.dicObjectCtrl.Where<KeyValuePair<int, ObjectCtrlInfo>>((Func<KeyValuePair<int, ObjectCtrlInfo>, bool>) (v => v.Value is OCIChar)))
        (keyValuePair.Value as OCIChar).UpdateFKColor(FKCtrl.parts);
    }

    public void UpdateItemFKColor()
    {
      foreach (KeyValuePair<int, ObjectCtrlInfo> keyValuePair in this.dicObjectCtrl.Where<KeyValuePair<int, ObjectCtrlInfo>>((Func<KeyValuePair<int, ObjectCtrlInfo>, bool>) (v => v.Value is OCIItem)))
        (keyValuePair.Value as OCIItem).UpdateFKColor();
    }

    public void SetVisibleGimmick()
    {
      bool visibleGimmick = this.workInfo.visibleGimmick;
      foreach (OCIItem ociItem in this.dicObjectCtrl.Where<KeyValuePair<int, ObjectCtrlInfo>>((Func<KeyValuePair<int, ObjectCtrlInfo>, bool>) (v => v.Value is OCIItem)).Select<KeyValuePair<int, ObjectCtrlInfo>, OCIItem>((Func<KeyValuePair<int, ObjectCtrlInfo>, OCIItem>) (v => v.Value as OCIItem)))
        ociItem.VisibleIcon = visibleGimmick;
    }

    public void Duplicate()
    {
      Dictionary<int, ObjectInfo> _dicObject = new Dictionary<int, ObjectInfo>();
      Dictionary<int, Studio.Studio.DuplicateParentInfo> dictionary = new Dictionary<int, Studio.Studio.DuplicateParentInfo>();
      TreeNodeObject[] selectNodes = this.treeNodeCtrl.selectNodes;
      for (int index = 0; index < selectNodes.Length; ++index)
      {
        this.SavePreprocessingLoop(selectNodes[index]);
        ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
        if (this.dicInfo.TryGetValue(selectNodes[index], out objectCtrlInfo))
        {
          _dicObject.Add(objectCtrlInfo.objectInfo.dicKey, objectCtrlInfo.objectInfo);
          if (objectCtrlInfo.parentInfo != null)
            dictionary.Add(objectCtrlInfo.objectInfo.dicKey, new Studio.Studio.DuplicateParentInfo(objectCtrlInfo.parentInfo, objectCtrlInfo.treeNodeObject.parent));
        }
      }
      if (_dicObject.Count == 0)
        return;
      byte[] buffer = (byte[]) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter _writer = new BinaryWriter((Stream) memoryStream))
        {
          this.sceneInfo.Save(_writer, _dicObject);
          buffer = memoryStream.ToArray();
        }
      }
      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        using (BinaryReader _reader = new BinaryReader((Stream) memoryStream))
          this.sceneInfo.Import(_reader, this.sceneInfo.version);
      }
      foreach (KeyValuePair<int, ObjectInfo> keyValuePair in this.sceneInfo.dicImport)
      {
        Studio.Studio.DuplicateParentInfo duplicateParentInfo = (Studio.Studio.DuplicateParentInfo) null;
        if (dictionary.TryGetValue(this.sceneInfo.dicChangeKey[keyValuePair.Key], out duplicateParentInfo))
          AddObjectAssist.LoadChild(keyValuePair.Value, duplicateParentInfo.info, duplicateParentInfo.node);
        else
          AddObjectAssist.LoadChild(keyValuePair.Value, (ObjectCtrlInfo) null, (TreeNodeObject) null);
      }
      if (this._routeControl.visible)
        this._routeControl.Init();
      this.treeNodeCtrl.RefreshHierachy();
      this._cameraSelector.Init();
    }

    public void SaveScene()
    {
      foreach (KeyValuePair<int, ObjectCtrlInfo> keyValuePair in this.dicObjectCtrl)
        keyValuePair.Value.OnSavePreprocessing();
      this.sceneInfo.cameraSaveData = this.m_CameraCtrl.Export();
      DateTime now = DateTime.Now;
      this.sceneInfo.Save(UserData.Create("studio/scene") + string.Format("{0}_{1:00}{2:00}_{3:00}{4:00}_{5:00}_{6:000}.png", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute, (object) now.Second, (object) now.Millisecond));
    }

    public bool LoadScene(string _path)
    {
      if (!File.Exists(_path))
        return false;
      this.InitScene(false);
      this.sceneInfo = new SceneInfo();
      if (!this.sceneInfo.Load(_path))
        return false;
      AddObjectAssist.LoadChild(this.sceneInfo.dicObject, (ObjectCtrlInfo) null, (TreeNodeObject) null);
      ChangeAmount _source = this.sceneInfo.caMap.Clone();
      this.AddMap(this.sceneInfo.map, false, false, true);
      this.mapList.UpdateInfo();
      this.sceneInfo.caMap.Copy(_source, true, true, true);
      Singleton<MapCtrl>.Instance.Reflect();
      this.bgmCtrl.Play(this.bgmCtrl.no);
      this.envCtrl.Play(this.envCtrl.no);
      this.outsideSoundCtrl.Play(this.outsideSoundCtrl.fileName);
      this.m_BackgroundCtrl.Load(this.sceneInfo.background);
      this.m_BackgroundList.UpdateUI();
      this._frameCtrl.Load(this.sceneInfo.frame);
      this._frameList.UpdateUI();
      this.m_SystemButtonCtrl.UpdateInfo();
      this.treeNodeCtrl.RefreshHierachy();
      if (this.sceneInfo.cameraSaveData != null)
        this.m_CameraCtrl.Import(this.sceneInfo.cameraSaveData);
      this.cameraLightCtrl.Reflect();
      this._cameraSelector.Init();
      this.sceneInfo.dataVersion = this.sceneInfo.version;
      this.rootButtonCtrl.OnClick(-1);
      return true;
    }

    [DebuggerHidden]
    public IEnumerator LoadSceneCoroutine(string _path)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Studio.Studio.\u003CLoadSceneCoroutine\u003Ec__Iterator2()
      {
        _path = _path,
        \u0024this = this
      };
    }

    public bool ImportScene(string _path)
    {
      if (!File.Exists(_path) || !this.sceneInfo.Import(_path))
        return false;
      AddObjectAssist.LoadChild(this.sceneInfo.dicImport, (ObjectCtrlInfo) null, (TreeNodeObject) null);
      this.treeNodeCtrl.RefreshHierachy();
      this._cameraSelector.Init();
      return true;
    }

    public void InitScene(bool _close = true)
    {
      this.ChangeCamera((OCICamera) null, false, true);
      this.cameraCount = 0;
      this.treeNodeCtrl.DeleteAllNode();
      Singleton<GuideObjectManager>.Instance.DeleteAll();
      this.m_RootButtonCtrl.OnClick(-1);
      this.m_RootButtonCtrl.objectCtrlInfo = (ObjectCtrlInfo) null;
      foreach (KeyValuePair<TreeNodeObject, ObjectCtrlInfo> keyValuePair in this.dicInfo)
      {
        switch (keyValuePair.Value.kind)
        {
          case 0:
            if (keyValuePair.Value is OCIChar ociChar)
            {
              ociChar.StopVoice();
              break;
            }
            break;
          case 4:
            (keyValuePair.Value as OCIRoute).DeleteLine();
            break;
        }
        Object.Destroy((Object) ((Component) keyValuePair.Value.guideObject.transformTarget).get_gameObject());
      }
      Singleton<Character>.Instance.DeleteCharaAll();
      this.dicInfo.Clear();
      this.dicChangeAmount.Clear();
      this.dicObjectCtrl.Clear();
      Singleton<Map>.Instance.ReleaseMap();
      this.cameraCtrl.CloerListCollider();
      this.bgmCtrl.Stop();
      this.envCtrl.Stop();
      this.outsideSoundCtrl.Stop();
      this.sceneInfo.Init();
      this.m_SystemButtonCtrl.UpdateInfo();
      this.cameraCtrl.Reset(0);
      this.cameraLightCtrl.Reflect();
      this._cameraSelector.Init();
      this.mapList.UpdateInfo();
      if (this.onChangeMap != null)
        this.onChangeMap();
      this.m_BackgroundCtrl.Load(this.sceneInfo.background);
      this.m_BackgroundList.UpdateUI();
      this._frameCtrl.Load(this.sceneInfo.frame);
      this._frameList.UpdateUI();
      this.m_WorkspaceCtrl.UpdateUI();
      Singleton<UndoRedoManager>.Instance.Clear();
      if (!_close)
        return;
      this.rootButtonCtrl.OnClick(-1);
    }

    public void OnDeleteNode(TreeNodeObject _node)
    {
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      if (!this.dicInfo.TryGetValue(_node, out objectCtrlInfo))
        return;
      if (this.onDelete != null)
        this.onDelete(objectCtrlInfo);
      objectCtrlInfo.OnDelete();
      this.dicInfo.Remove(_node);
    }

    public void OnParentage(TreeNodeObject _parent, TreeNodeObject _child)
    {
      if (Object.op_Implicit((Object) _parent))
      {
        ObjectCtrlInfo loop = this.FindLoop(_parent);
        if (loop == null)
          Debug.LogWarning((object) "ありえないはず");
        else
          loop.OnAttach(_parent, this.dicInfo[_child]);
      }
      else
        this.dicInfo[_child].OnDetach();
    }

    public void ResetOption()
    {
      if (this.xmlCtrl == null)
        return;
      this.xmlCtrl.Init();
    }

    public void LoadOption()
    {
      if (this.xmlCtrl == null)
        return;
      this.xmlCtrl.Read();
    }

    public void SaveOption()
    {
      if (this.xmlCtrl == null)
        return;
      this.xmlCtrl.Write();
    }

    public static void AddInfo(ObjectInfo _info, ObjectCtrlInfo _ctrlInfo)
    {
      if (!Singleton<Studio.Studio>.IsInstance() || _info == null || _ctrlInfo == null)
        return;
      Singleton<Studio.Studio>.Instance.sceneInfo.dicObject.Add(_info.dicKey, _info);
      Singleton<Studio.Studio>.Instance.dicObjectCtrl[_info.dicKey] = _ctrlInfo;
    }

    public static void DeleteInfo(ObjectInfo _info, bool _delKey = true)
    {
      if (!Singleton<Studio.Studio>.IsInstance() || _info == null)
        return;
      if (Singleton<Studio.Studio>.Instance.sceneInfo.dicObject.ContainsKey(_info.dicKey))
        Singleton<Studio.Studio>.Instance.sceneInfo.dicObject.Remove(_info.dicKey);
      if (!_delKey)
        return;
      Singleton<Studio.Studio>.Instance.dicObjectCtrl.Remove(_info.dicKey);
      _info.DeleteKey();
      if (Singleton<Studio.Studio>.Instance.sceneInfo.sunCaster != _info.dicKey)
        return;
      Singleton<Studio.Studio>.Instance.SetSunCaster(-1);
    }

    public static ObjectInfo GetInfo(int _key)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return (ObjectInfo) null;
      ObjectInfo objectInfo = (ObjectInfo) null;
      return Singleton<Studio.Studio>.Instance.sceneInfo.dicObject.TryGetValue(_key, out objectInfo) ? objectInfo : (ObjectInfo) null;
    }

    public static void AddObjectCtrlInfo(ObjectCtrlInfo _ctrlInfo)
    {
      if (!Singleton<Studio.Studio>.IsInstance() || _ctrlInfo == null)
        return;
      Singleton<Studio.Studio>.Instance.dicObjectCtrl[_ctrlInfo.objectInfo.dicKey] = _ctrlInfo;
    }

    public static ObjectCtrlInfo GetCtrlInfo(int _key)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return (ObjectCtrlInfo) null;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      return Singleton<Studio.Studio>.Instance.dicObjectCtrl.TryGetValue(_key, out objectCtrlInfo) ? objectCtrlInfo : (ObjectCtrlInfo) null;
    }

    public static TreeNodeObject AddNode(string _name, TreeNodeObject _parent = null)
    {
      return !Singleton<Studio.Studio>.IsInstance() ? (TreeNodeObject) null : Singleton<Studio.Studio>.Instance.treeNodeCtrl.AddNode(_name, _parent);
    }

    public static void DeleteNode(TreeNodeObject _node)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return;
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.DeleteNode(_node);
    }

    public static void AddCtrlInfo(ObjectCtrlInfo _info)
    {
      if (!Singleton<Studio.Studio>.IsInstance() || _info == null)
        return;
      Singleton<Studio.Studio>.Instance.dicInfo.Add(_info.treeNodeObject, _info);
    }

    public static ObjectCtrlInfo GetCtrlInfo(TreeNodeObject _node)
    {
      if (!Singleton<Studio.Studio>.IsInstance() || Object.op_Equality((Object) _node, (Object) null))
        return (ObjectCtrlInfo) null;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      return Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo) ? objectCtrlInfo : (ObjectCtrlInfo) null;
    }

    public static int GetNewIndex()
    {
      return !Singleton<Studio.Studio>.IsInstance() ? -1 : Singleton<Studio.Studio>.Instance.sceneInfo.GetNewIndex();
    }

    public static int SetNewIndex(int _index)
    {
      return !Singleton<Studio.Studio>.IsInstance() || Singleton<Studio.Studio>.Instance.sceneInfo.SetNewIndex(_index) ? _index : Singleton<Studio.Studio>.Instance.sceneInfo.GetNewIndex();
    }

    public static bool DeleteIndex(int _index)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return false;
      bool flag = Singleton<Studio.Studio>.Instance.sceneInfo.DeleteIndex(_index);
      return Studio.Studio.DeleteChangeAmount(_index) | flag;
    }

    public static void AddChangeAmount(int _key, ChangeAmount _ca)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return;
      try
      {
        Singleton<Studio.Studio>.Instance.dicChangeAmount.Add(_key, _ca);
      }
      catch (Exception ex)
      {
        Debug.Log((object) _key);
      }
    }

    public static bool DeleteChangeAmount(int _key)
    {
      return Singleton<Studio.Studio>.IsInstance() && Singleton<Studio.Studio>.Instance.dicChangeAmount.Remove(_key);
    }

    public static ChangeAmount GetChangeAmount(int _key)
    {
      if (!Singleton<Studio.Studio>.IsInstance())
        return (ChangeAmount) null;
      ChangeAmount changeAmount = (ChangeAmount) null;
      return Singleton<Studio.Studio>.Instance.dicChangeAmount.TryGetValue(_key, out changeAmount) ? changeAmount : (ChangeAmount) null;
    }

    public static ObjectCtrlInfo[] GetSelectObjectCtrl()
    {
      return !Singleton<Studio.Studio>.IsInstance() ? (ObjectCtrlInfo[]) null : Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl;
    }

    public void Init()
    {
      this.sceneInfo = new SceneInfo();
      this.cameraLightCtrl.Init();
      this.systemButtonCtrl.Init();
      this.mapList.Init();
      this.logoList.Init();
      this._inputFieldNow = (InputField) null;
      this._inputFieldTMPNow = (TMP_InputField) null;
      this.treeNodeCtrl.onDelete += new Action<TreeNodeObject>(this.OnDeleteNode);
      this.treeNodeCtrl.onParentage += new Action<TreeNodeObject, TreeNodeObject>(this.OnParentage);
    }

    public void SelectInputField(InputField _input, TMP_InputField _inputTMP)
    {
      this._inputFieldNow = _input;
      this._inputFieldTMPNow = _inputTMP;
    }

    public void DeselectInputField(InputField _input, TMP_InputField _inputTMP)
    {
      if (Object.op_Equality((Object) this._inputFieldNow, (Object) _input))
        this._inputFieldNow = (InputField) null;
      if (!Object.op_Equality((Object) this._inputFieldTMPNow, (Object) _inputTMP))
        return;
      this._inputFieldTMPNow = (TMP_InputField) null;
    }

    public void ShowName(Transform _transform, string _name)
    {
      ((Component) this.rectName).get_gameObject().SetActive(true);
      ((Transform) this.rectName).set_position(Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(Camera.get_main(), _transform.get_position())));
      ((TMP_Text) this.textName).set_text(_name);
      if (this.disposableName != null)
        this.disposableName.Dispose();
      this.disposableName = new SingleAssignmentDisposable();
      this.disposableName.set_Disposable(ObservableExtensions.Subscribe<long>(Observable.TakeUntil<long, long>((IObservable<M0>) Observable.EveryUpdate(), (IObservable<M1>) Observable.Timer(TimeSpan.FromSeconds(2.0))), (Action<M0>) (_ =>
      {
        if (!Object.op_Inequality((Object) _transform, (Object) null))
          return;
        ((Transform) this.rectName).set_position(Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(Camera.get_main(), _transform.get_position())));
      }), (Action) (() => ((Component) this.rectName).get_gameObject().SetActive(false))));
    }

    private ObjectCtrlInfo FindLoop(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return (ObjectCtrlInfo) null;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      return this.dicInfo.TryGetValue(_node, out objectCtrlInfo) ? objectCtrlInfo : this.FindLoop(_node.parent);
    }

    private void SavePreprocessingLoop(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      if (this.dicInfo.TryGetValue(_node, out objectCtrlInfo))
        objectCtrlInfo.OnSavePreprocessing();
      if (_node.child.IsNullOrEmpty<TreeNodeObject>())
        return;
      foreach (TreeNodeObject _node1 in _node.child)
        this.SavePreprocessingLoop(_node1);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      ((Component) this.imageCamera).get_gameObject().SetActive(true);
      Studio.Studio.optionSystem = new OptionSystem("Option");
      this.xmlCtrl = new Control("studio", "option.xml", "Option", new Illusion.Elements.Xml.Data[1]
      {
        (Illusion.Elements.Xml.Data) Studio.Studio.optionSystem
      });
      this.LoadOption();
      this._logoList.UpdateInfo();
      if (this.workInfo == null)
        this.workInfo = new WorkInfo();
      this.workInfo.Load();
    }

    private void OnApplicationQuit()
    {
      this.SaveOption();
      this.workInfo.Save();
    }

    private class DuplicateParentInfo
    {
      public ObjectCtrlInfo info;
      public TreeNodeObject node;

      public DuplicateParentInfo(ObjectCtrlInfo _info, TreeNodeObject _node)
      {
        this.info = _info;
        this.node = _node;
      }
    }
  }
}
