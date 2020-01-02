// Decompiled with JetBrains decompiler
// Type: AIProject.TitleScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ConfigScene;
using FadeCtrl;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace AIProject
{
  public class TitleScene : BaseLoader
  {
    public static string mapFileName = string.Empty;
    [SerializeField]
    private List<TitleScene.TimeLineInfo> lstTimeLlineInfos = new List<TitleScene.TimeLineInfo>();
    [SerializeField]
    private float fadeTime = 0.5f;
    private Dictionary<int, TitleScene.MapData> dicMapData = new Dictionary<int, TitleScene.MapData>();
    private Dictionary<int, TitleScene.MapData> dicMapSound = new Dictionary<int, TitleScene.MapData>();
    private Dictionary<int, TitleScene.MapData> dicMapSe = new Dictionary<int, TitleScene.MapData>();
    private TitleScene.ButtonStack<List<Button>> _buttonStack = new TitleScene.ButtonStack<List<Button>>();
    private readonly string titleListAssetBundleName = "list/title.unity3d";
    public static int startmode;
    [SerializeField]
    private GameObject objCanvas;
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private PlayableDirector menuPlayableDirector;
    [SerializeField]
    private PlayableAsset menuTimeLine;
    [SerializeField]
    private PlayableAsset menuTimeLine1;
    [SerializeField]
    private SpriteFadeCtrl spriteFadeCtrl;
    [SerializeField]
    private Illusion.Component.ObjectCategoryBehaviour objectCategory;
    [SerializeField]
    private TitleScene.ButtonGroup[] _buttons;
    [SerializeField]
    private RectTransform rtButtonRoot;
    [SerializeField]
    private RectTransform rtImgSelect;
    [SerializeField]
    private Animator animCaption;
    [SerializeField]
    private TextMeshProUGUI _version;
    [SerializeField]
    private bool isUploader;
    private int proc;
    private bool isCoroutine;
    private int selectNum;
    private Dictionary<int, List<Button>> _buttonDic;
    private TextMeshProUGUI tmpCaption;
    private bool isTitleLoad;
    private int charaCreateSex;
    [SerializeField]
    private GameObject objTitleMain;
    private GameObject objMap;

    public void OnStart()
    {
      if (!this.IsEnter())
        return;
      this.Enter("Title_Load");
    }

    public void OnCustom()
    {
      if (!this.IsEnter())
        return;
      this._buttonStack.Push(this._buttonDic[1]);
      this.InitImgSelectPostion(false);
      this.Enter("next");
    }

    public void OnCustomMale()
    {
      if (!this.IsEnter())
        return;
      this.charaCreateSex = 0;
      this.Enter("CharaCustom");
    }

    public void OnCustomFemale()
    {
      if (!this.IsEnter())
        return;
      this.charaCreateSex = 1;
      this.Enter("CharaCustom");
    }

    public void OnUploader()
    {
      if (!this.IsEnter())
        return;
      this.Enter("Uploader");
    }

    public void OnDownloader()
    {
      if (!this.IsEnter())
        return;
      this.Enter("Downloader");
    }

    public void OnBack()
    {
      if (!this.IsEnter())
        return;
      this._buttonStack.Pop();
      if (this._buttonDic[0].Count > 2 && !this.isUploader)
        ((UnityEngine.Component) this._buttonDic[0][2]).get_gameObject().SetActiveIfDifferent(false);
      this.InitImgSelectPostion(false);
      this.Enter(string.Empty);
    }

    public void OnOther()
    {
      if (!this.IsEnter())
        return;
      this._buttonStack.Push(this._buttonDic[2]);
      this.InitImgSelectPostion(false);
      this.Enter("next");
    }

    public void OnOtherEvent()
    {
      if (this.IsEnter())
        ;
    }

    public void OnConfig()
    {
      if (!this.IsEnter() || Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.Config, (Object) null))
        return;
      this.Enter("Config");
    }

    public void OnEnd()
    {
      if (!this.IsEnter() || Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.ExitScene, (Object) null))
        return;
      this.Enter("Exit");
    }

    private bool IsEnter()
    {
      return this.menuPlayableDirector.get_state() != 1 && !Singleton<Scene>.Instance.IsNowLoadingFade && !this.isTitleLoad;
    }

    private void InitImgSelectPostion(bool _isForce = false)
    {
      if (((IEnumerable<Button>) this._buttonStack.Peek()).Where<Button>((Func<Button, bool>) (bt => ((Selectable) bt).get_interactable())).Count<Button>() > this.selectNum && !_isForce)
        return;
      this.selectNum = 0;
      Vector2 anchoredPosition = this.rtImgSelect.get_anchoredPosition();
      anchoredPosition.y = this.rtButtonRoot.get_anchoredPosition().y;
      this.rtImgSelect.set_anchoredPosition(anchoredPosition);
    }

    private void Enter(string next)
    {
      if (next != null && next == string.Empty)
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      else
        this.PlaySE(1);
      if (next == null)
        return;
      if (!(next == "Title_Load"))
      {
        if (!(next == "CharaCustom"))
        {
          if (!(next == "Uploader"))
          {
            if (!(next == "Downloader"))
            {
              if (!(next == "Config"))
              {
                if (!(next == "Exit"))
                  return;
                Singleton<Manager.Game>.Instance.LoadExit();
              }
              else
              {
                ConfigWindow.backGroundColor = Color32.op_Implicit(new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 200));
                Singleton<Manager.Game>.Instance.LoadConfig();
              }
            }
            else
            {
              bool flag = !Singleton<GameSystem>.Instance.HandleName.IsNullOrEmpty();
              Singleton<GameSystem>.Instance.networkSceneName = "Downloader";
              if (flag)
                Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
                {
                  levelName = "NetworkCheckScene",
                  isAdd = false,
                  isFade = true,
                  isAsync = true
                }, true);
              else
                Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
                {
                  levelName = "EntryHandleName",
                  isFade = true
                }, false);
            }
          }
          else
          {
            bool flag = !Singleton<GameSystem>.Instance.HandleName.IsNullOrEmpty();
            Singleton<GameSystem>.Instance.networkSceneName = "Uploader";
            if (flag)
              Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
              {
                levelName = "NetworkCheckScene",
                isAdd = false,
                isFade = true,
                isAsync = true
              }, true);
            else
              Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
              {
                levelName = "EntryHandleName",
                isFade = true
              }, false);
          }
        }
        else
        {
          CharaCustom.CharaCustom.modeNew = true;
          CharaCustom.CharaCustom.modeSex = (byte) this.charaCreateSex;
          Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
          {
            levelName = next,
            isAdd = false,
            isFade = true
          }, true);
        }
      }
      else
      {
        this.isTitleLoad = true;
        this.AddScene("title/scene/title_load.unity3d", next, false, (Action) (() =>
        {
          this.objCanvas.SetActiveIfDifferent(false);
          TitleLoadScene rootComponent = Scene.GetRootComponent<TitleLoadScene>(next);
          if (Object.op_Equality((Object) rootComponent, (Object) null))
            return;
          rootComponent.titleScene = this;
          rootComponent.objMap = this.objMap;
          rootComponent.objTitleMain = this.objTitleMain;
          rootComponent.actionClose = (Action) (() =>
          {
            this.objCanvas.SetActiveIfDifferent(true);
            this.isTitleLoad = false;
          });
        }));
      }
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TitleScene.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private bool LoadMapList()
    {
      string listAssetBundleName = this.titleListAssetBundleName;
      TitleData titleData = CommonLib.LoadAsset<TitleData>(listAssetBundleName, "title_map", false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(listAssetBundleName, true, (string) null, false);
      foreach (TitleData.Param obj in titleData.param)
      {
        if (!this.dicMapData.ContainsKey(obj.id))
          this.dicMapData[obj.id] = new TitleScene.MapData();
        TitleScene.MapData mapData = this.dicMapData[obj.id];
        mapData.assetPath = obj.assetPath;
        mapData.fileName = obj.fileName;
        mapData.manifest = obj.manifest;
      }
      return true;
    }

    private bool LoadMapSoundList()
    {
      string listAssetBundleName = this.titleListAssetBundleName;
      TitleData titleData = CommonLib.LoadAsset<TitleData>(listAssetBundleName, "title_sound", false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(listAssetBundleName, true, (string) null, false);
      foreach (TitleData.Param obj in titleData.param)
      {
        if (!this.dicMapSound.ContainsKey(obj.id))
          this.dicMapSound[obj.id] = new TitleScene.MapData();
        TitleScene.MapData mapData = this.dicMapSound[obj.id];
        mapData.assetPath = obj.assetPath;
        mapData.fileName = obj.fileName;
        mapData.manifest = obj.manifest;
      }
      return true;
    }

    private bool LoadMapSeList()
    {
      string listAssetBundleName = this.titleListAssetBundleName;
      TitleData titleData = CommonLib.LoadAsset<TitleData>(listAssetBundleName, "title_se", false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(listAssetBundleName, true, (string) null, false);
      foreach (TitleData.Param obj in titleData.param)
      {
        if (!this.dicMapSe.ContainsKey(obj.id))
          this.dicMapSe[obj.id] = new TitleScene.MapData();
        TitleScene.MapData mapData = this.dicMapSe[obj.id];
        mapData.assetPath = obj.assetPath;
        mapData.fileName = obj.fileName;
        mapData.manifest = obj.manifest;
      }
      return true;
    }

    public void PlayBGM()
    {
      if (this.dicMapSound.Count == 0)
        return;
      TitleScene.MapData mapData = this.dicMapSound[0];
      Transform asset = Singleton<Manager.Sound>.Instance.FindAsset(Manager.Sound.Type.BGM, mapData.fileName, mapData.assetPath);
      if (Object.op_Inequality((Object) asset, (Object) null))
      {
        bool? isPlaying = ((AudioSource) ((UnityEngine.Component) asset).GetComponent<AudioSource>())?.get_isPlaying();
        bool? nullable = !isPlaying.HasValue ? new bool?() : new bool?(!isPlaying.Value);
        if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
          return;
        Singleton<Manager.Sound>.Instance.PlayBGM(Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime);
      }
      else
      {
        Illusion.Game.Utils.Sound.SettingBGM settingBgm = new Illusion.Game.Utils.Sound.SettingBGM();
        settingBgm.assetBundleName = mapData.assetPath;
        settingBgm.assetName = mapData.fileName;
        Illusion.Game.Utils.Sound.Play((Illusion.Game.Utils.Sound.Setting) settingBgm);
      }
    }

    [DebuggerHidden]
    private IEnumerator ModeChangeCoroutine(int _nextMode)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TitleScene.\u003CModeChangeCoroutine\u003Ec__Iterator1()
      {
        _nextMode = _nextMode,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator PlayableDirectorInit(PlayableDirector _director)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TitleScene.\u003CPlayableDirectorInit\u003Ec__Iterator2()
      {
        _director = _director
      };
    }

    private void ModeChange(int _nextMode)
    {
      this.objectCategory.SetActiveToggle(new int[2]{ 1, 0 }[_nextMode]);
      this.playableDirector.set_time(10.0);
      this.playableDirector.Evaluate();
      this.playableDirector.Play();
      switch (_nextMode)
      {
        case 0:
          this.InitImgSelectPostion(true);
          this.menuPlayableDirector.Stop();
          break;
        case 1:
          this.animCaption.set_speed(0.0f);
          ((Behaviour) this.animCaption).set_enabled(false);
          Color color = ((Graphic) this.tmpCaption).get_color();
          color.a = (__Null) 0.0;
          ((Graphic) this.tmpCaption).set_color(color);
          this.playableDirector.Stop();
          break;
      }
      this.proc = new int[2]{ 1, 0 }[_nextMode];
      if (_nextMode == 0)
      {
        this.menuPlayableDirector.Play();
      }
      else
      {
        if (_nextMode != 1)
          return;
        ((Behaviour) this.animCaption).set_enabled(true);
        this.playableDirector.Play();
      }
    }

    public AudioSource Get(int _seID)
    {
      return Illusion.Game.Utils.Sound.Get(Manager.Sound.Type.SystemSE, this.dicMapSe[_seID].assetPath, this.dicMapSe[_seID].fileName, (string) null);
    }

    public void PlaySE(int _seID)
    {
      AudioSource audioSource = this.Get(_seID);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return;
      audioSource.Play();
    }

    private void AddScene(string assetBundleName, string levelName, bool _isFade, Action onLoad)
    {
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        assetBundleName = assetBundleName,
        levelName = levelName,
        isAdd = true,
        isFade = _isFade,
        onLoad = onLoad
      }, false);
    }

    private void SetTileLine()
    {
      this.menuPlayableDirector.set_playableAsset(!this.isUploader ? this.menuTimeLine1 : this.menuTimeLine);
      for (int i = 0; i < this.lstTimeLlineInfos.Count; ++i)
      {
        PlayableBinding playableBinding = this.menuPlayableDirector.get_playableAsset().get_outputs().First<PlayableBinding>((Func<PlayableBinding, bool>) (c => ((PlayableBinding) ref c).get_streamName() == this.lstTimeLlineInfos[i].trackName));
        if (this.lstTimeLlineInfos[i].kind == TitleScene.TimeLineKind.animation)
          this.menuPlayableDirector.SetGenericBinding(((PlayableBinding) ref playableBinding).get_sourceObject(), (Object) this.lstTimeLlineInfos[i].animator);
        else if (this.lstTimeLlineInfos[i].kind == TitleScene.TimeLineKind.activeObject)
          this.menuPlayableDirector.SetGenericBinding(((PlayableBinding) ref playableBinding).get_sourceObject(), (Object) this.lstTimeLlineInfos[i].activeObject);
      }
    }

    public enum TimeLineKind
    {
      animation,
      activeObject,
    }

    public class MapData
    {
      public string assetPath;
      public string fileName;
      public string manifest;
    }

    [Serializable]
    public class TimeLineInfo
    {
      public string trackName;
      public TitleScene.TimeLineKind kind;
      public Animator animator;
      public GameObject activeObject;
    }

    public enum Mode
    {
      MainTitle,
      Menu,
    }

    public class ButtonStack<T> : Stack<T> where T : List<Button>
    {
      public new void Push(T item)
      {
        if (this.Count > 0)
          this.Peek().ForEach((Action<Button>) (b => ((UnityEngine.Component) b).get_gameObject().SetActive(false)));
        item.ForEach((Action<Button>) (b => ((UnityEngine.Component) b).get_gameObject().SetActive(true)));
        base.Push(item);
      }

      public new T Pop()
      {
        T obj = base.Pop();
        obj.ForEach((Action<Button>) (b => ((UnityEngine.Component) b).get_gameObject().SetActive(false)));
        if (this.Count > 0)
          this.Peek().ForEach((Action<Button>) (b => ((UnityEngine.Component) b).get_gameObject().SetActive(true)));
        return obj;
      }
    }

    [Serializable]
    public class ButtonGroup
    {
      [SerializeField]
      private int _group;
      [SerializeField]
      private Button _button;

      public int Group
      {
        get
        {
          return this._group;
        }
        set
        {
          this._group = value;
        }
      }

      public Button Button
      {
        get
        {
          return this._button;
        }
        set
        {
          this._button = value;
        }
      }
    }
  }
}
