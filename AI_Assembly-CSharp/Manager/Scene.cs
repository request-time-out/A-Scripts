// Decompiled with JetBrains decompiler
// Type: Manager.Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
  public sealed class Scene : Singleton<Scene>
  {
    private Color initFadeColor;
    [SerializeField]
    private Image nowLoadingImage;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private Image loadingAnimeImage;
    private Scene.SceneStack<Scene.Data> sceneStack;
    private Stack<Scene.Data> loadStack;
    private int loadingCount;
    private const float FadeWaitTime = 0.1f;
    private Scene.Data _baseScene;
    public bool isGameEndCheck;
    public bool isSkipGameExit;

    protected override void Awake()
    {
      if (!this.CheckInstance())
      {
        Object.Destroy((Object) ((Component) this).get_gameObject());
      }
      else
      {
        Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
        GameObject asset = AssetBundleManager.LoadAsset("scene/scenemanager.unity3d", "scenemanager", typeof (GameObject), (string) null).GetAsset<GameObject>();
        this.manager = (GameObject) Object.Instantiate<GameObject>((M0) asset, ((Component) this).get_transform(), false);
        ((Object) this.manager).set_name(((Object) asset).get_name());
        this.nowLoadingImage = ((IEnumerable<Image>) this.manager.GetComponentsInChildren<Image>(true)).FirstOrDefault<Image>((Func<Image, bool>) (p => ((Object) p).get_name() == "NowLoading"));
        this.progressSlider = ((IEnumerable<Slider>) this.manager.GetComponentsInChildren<Slider>(true)).FirstOrDefault<Slider>((Func<Slider, bool>) (p => ((Object) p).get_name() == "Progress"));
        this.loadingAnimeImage = ((IEnumerable<Image>) this.manager.GetComponentsInChildren<Image>(true)).FirstOrDefault<Image>((Func<Image, bool>) (p => ((Object) p).get_name() == "LoadingAnime"));
        this.nowLoadingImage.SafeProc<Image>((Action<Image>) (t => ((Component) t).get_gameObject().SetActive(false)));
        this.progressSlider.SafeProc<Slider>((Action<Slider>) (t => ((Component) t).get_gameObject().SetActive(false)));
        this.loadingAnimeImage.SafeProc<Image>((Action<Image>) (t => ((Component) t).get_gameObject().SetActive(false)));
        this.loadingPanel = (LoadingPanel) this.manager.GetComponentInChildren<LoadingPanel>(true);
        this.loadingPanel.SafeProc<LoadingPanel>((Action<LoadingPanel>) (t => ((Component) t).get_gameObject().SetActive(false)));
        this.sceneFade = (SceneFade) this.manager.GetComponentInChildren<SceneFade>(true);
        this.initFadeColor = this.sceneFade._Color;
        this.sceneFade._Fade = SimpleFade.Fade.Out;
        this.sceneFade.ForceEnd();
        AssetBundleManager.UnloadAssetBundle("scene/scenemanager.unity3d", false, (string) null, false);
        Scene.Data data1 = new Scene.Data();
        Scene.Data data2 = data1;
        Scene activeScene = Scene.ActiveScene;
        string name = ((Scene) ref activeScene).get_name();
        data2.levelName = name;
        data1.isAdd = false;
        this.sceneStack = new Scene.SceneStack<Scene.Data>(data1);
        this.loadStack = new Stack<Scene.Data>();
        this.CreateSpace();
        Application.add_wantsToQuit(new Func<bool>(this.CanQuit));
        this.isGameEndCheck = true;
        this.isSkipGameExit = false;
      }
    }

    private bool CanQuit()
    {
      if (Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null))
      {
        this.GameExit();
        return true;
      }
      if (this.isSkipGameExit)
        return false;
      if (this.isGameEndCheck)
      {
        this.isGameEndCheck = false;
        if (true & !this.IsExit & !Application.get_isEditor() & !this.IsNowLoadingFade & this.LoadSceneName != "Initialize" & this.LoadSceneName != "Logo")
        {
          Singleton<Game>.Instance.LoadExit();
          this.isSkipGameExit = true;
          return false;
        }
        this.GameExit();
        return true;
      }
      this.GameExit();
      return true;
    }

    public GameObject commonSpace { get; private set; }

    public GameObject manager { get; private set; }

    public SceneFade sceneFade { get; private set; }

    public LoadingPanel loadingPanel { get; private set; }

    public Scene.Data baseScene
    {
      get
      {
        return this._baseScene;
      }
    }

    private void GameExit()
    {
      Scene.isGameEnd = true;
      if (Singleton<Config>.IsInstance())
        Singleton<Config>.Instance.Save();
      if (!Singleton<Voice>.IsInstance())
        return;
      Singleton<Voice>.Instance.Save();
    }

    [DebuggerHidden]
    private IEnumerator LoadSet(Scene.Data data)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CLoadSet\u003Ec__Iterator0()
      {
        data = data,
        \u0024this = this
      };
    }

    private void SetImageAlpha(Image image, float alpha)
    {
      if (Object.op_Equality((Object) image, (Object) null))
        return;
      Color color = ((Graphic) image).get_color();
      color.a = (__Null) (double) alpha;
      ((Graphic) image).set_color(color);
    }

    public void LoadBaseScene(Scene.Data data)
    {
      this.StartCoroutine(this.LoadBaseSceneCoroutine(data));
    }

    [DebuggerHidden]
    public IEnumerator LoadBaseSceneCoroutine(Scene.Data data)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CLoadBaseSceneCoroutine\u003Ec__Iterator1()
      {
        data = data,
        \u0024this = this
      };
    }

    public void UnloadBaseScene()
    {
      this.StartCoroutine(this.UnloadBaseSceneCoroutine(this.LoadSceneName));
    }

    [DebuggerHidden]
    public IEnumerator UnloadBaseSceneCoroutine(string levelName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CUnloadBaseSceneCoroutine\u003Ec__Iterator2()
      {
        levelName = levelName,
        \u0024this = this
      };
    }

    public static void MapSettingChange(LightMapDataObject lightMap, Scene.FogData fog = null)
    {
      if (Object.op_Inequality((Object) lightMap, (Object) null))
        lightMap.Change();
      fog?.Change();
    }

    public static bool isGameEnd { get; private set; }

    public static bool isReturnTitle { get; set; }

    public static Scene ActiveScene
    {
      get
      {
        return SceneManager.GetActiveScene();
      }
      set
      {
        SceneManager.SetActiveScene(value);
      }
    }

    public static Scene GetScene(string levelName)
    {
      return SceneManager.GetSceneByName(levelName);
    }

    public static GameObject[] GetRootGameObjects(string sceneName)
    {
      Scene scene = Scene.GetScene(sceneName);
      return !((Scene) ref scene).get_isLoaded() ? (GameObject[]) null : ((Scene) ref scene).GetRootGameObjects();
    }

    public static T GetRootComponent<T>(string sceneName) where T : Component
    {
      GameObject[] rootGameObjects = Scene.GetRootGameObjects(sceneName);
      if (rootGameObjects == null)
        return (T) null;
      foreach (GameObject gameObject in rootGameObjects)
      {
        T component = gameObject.GetComponent<T>();
        if (Object.op_Inequality((Object) (object) component, (Object) null))
          return component;
      }
      foreach (GameObject gameObject in rootGameObjects)
      {
        T[] componentsInChildren = gameObject.GetComponentsInChildren<T>(true);
        if (!((IList<T>) componentsInChildren).IsNullOrEmpty<T>())
          return componentsInChildren[0];
      }
      Debug.LogError((object) ("not scene component : " + sceneName));
      return (T) null;
    }

    public bool IsOverlap
    {
      get
      {
        return this.sceneStack.Any<Scene.Data>() && this.sceneStack.Peek().isOverlap;
      }
    }

    public bool IsExit
    {
      get
      {
        return Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null);
      }
    }

    public bool IsNowLoading
    {
      get
      {
        if (this.loadStack.Count > 0)
          return true;
        bool flag = false;
        foreach (Scene.Data scene in (Stack<Scene.Data>) this.sceneStack)
        {
          if (scene.isLoading)
          {
            flag = true;
            break;
          }
          if (scene.operation != null && !scene.operation.get_isDone())
          {
            flag = true;
            break;
          }
        }
        return flag;
      }
    }

    public bool IsNowLoadingFade
    {
      get
      {
        return this.IsNowLoading || this.sceneFade.IsFadeNow;
      }
    }

    public bool IsFadeNow
    {
      get
      {
        return this.sceneFade.IsFadeNow;
      }
    }

    public string LoadSceneName
    {
      get
      {
        return this.sceneStack.NowSceneNameList.Last<string>();
      }
    }

    public string AddSceneName
    {
      get
      {
        return this.sceneStack.NowSceneNameList.Count > 1 ? this.sceneStack.NowSceneNameList[0] : string.Empty;
      }
    }

    public string AddSceneNameOverlapRemoved
    {
      get
      {
        foreach (Scene.Data scene in (Stack<Scene.Data>) this.sceneStack)
        {
          if (!scene.isOverlap)
            return !scene.isAdd ? string.Empty : scene.levelName;
        }
        return string.Empty;
      }
    }

    public string PrevLoadSceneName
    {
      get
      {
        bool flag = false;
        foreach (Scene.Data scene in (Stack<Scene.Data>) this.sceneStack)
        {
          if (!scene.isAdd)
          {
            if (flag)
              return scene.levelName;
            flag = true;
          }
        }
        return string.Empty;
      }
    }

    public string PrevAddSceneName
    {
      get
      {
        return this.sceneStack.NowSceneNameList.Count > 2 ? this.sceneStack.NowSceneNameList[1] : string.Empty;
      }
    }

    public List<string> NowSceneNames
    {
      get
      {
        return this.sceneStack.NowSceneNameList;
      }
    }

    public void GameEnd(bool _isCheck = true)
    {
      this.isGameEndCheck = _isCheck;
      Application.Quit();
    }

    public void LoadReserve(Scene.Data data, bool isLoadingImageDraw)
    {
      this.StartCoroutine(this.LoadStart(data, isLoadingImageDraw));
    }

    [DebuggerHidden]
    public IEnumerator LoadSceneBack(bool isNowSceneRemove, bool isAddSceneLoad)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CLoadSceneBack\u003Ec__Iterator3()
      {
        isNowSceneRemove = isNowSceneRemove,
        isAddSceneLoad = isAddSceneLoad,
        \u0024this = this
      };
    }

    public void UnloadAddScene()
    {
      while (this.sceneStack.Peek().isAdd)
      {
        this.sceneStack.Peek().Unload();
        this.sceneStack.Pop();
      }
      Resources.UnloadUnusedAssets();
    }

    [DebuggerHidden]
    public IEnumerator UnloadAddScene(bool isFade, bool isLoadingImageDraw = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CUnloadAddScene\u003Ec__Iterator4()
      {
        isFade = isFade,
        isLoadingImageDraw = isLoadingImageDraw,
        \u0024this = this
      };
    }

    public void Reload()
    {
      this.StartCoroutine(this.LoadSceneBack(false, true));
    }

    public bool UnLoad()
    {
      bool flag = false;
      if (this.sceneStack.Count <= 1)
        return false;
      Scene.Data scene = this.sceneStack.Peek();
      AsyncOperation asyncOperation = scene.Unload();
      this.sceneStack.Pop();
      Scene.Data.UnloadType unloadType = Scene.Data.UnloadType.Success;
      if (asyncOperation != null)
      {
        if (this.NowSceneNames.Any<string>((Func<string, bool>) (s => s == scene.levelName)))
          unloadType = Scene.Data.UnloadType.Loaded;
      }
      else
        unloadType = Scene.Data.UnloadType.Fail;
      switch (unloadType)
      {
        case Scene.Data.UnloadType.Fail:
          if (this.AddSceneName.IsNullOrEmpty())
          {
            this.StartCoroutine(this.LoadStart(this.sceneStack.Pop(), true));
            break;
          }
          flag = true;
          break;
        case Scene.Data.UnloadType.Loaded:
          flag = true;
          break;
      }
      if (flag)
      {
        do
        {
          this.loadStack.Push(this.sceneStack.Pop());
        }
        while (this.loadStack.Peek().isAdd);
        while (this.loadStack.Any<Scene.Data>())
        {
          Scene.Data data = this.loadStack.Pop();
          bool isAsync = data.isAsync;
          data.isAsync = false;
          this.sceneStack.Push(data);
          data.isAsync = isAsync;
        }
      }
      return true;
    }

    [DebuggerHidden]
    public IEnumerator UnLoad(bool isLoadBack)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CUnLoad\u003Ec__Iterator5()
      {
        isLoadBack = isLoadBack,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UnLoad(string levelName, Action<bool> act = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CUnLoad\u003Ec__Iterator6()
      {
        levelName = levelName,
        act = act,
        \u0024this = this
      };
    }

    public bool IsFind(string levelName)
    {
      return this.sceneStack.Any<Scene.Data>((Func<Scene.Data, bool>) (scene => scene.levelName == levelName));
    }

    public void RollBack(string levelName)
    {
      while (this.sceneStack.Peek().levelName != levelName)
        this.sceneStack.Pop();
    }

    public bool StartFade(
      int _fadeType,
      Color _color,
      float _inTime = 1f,
      float _outTime = 1f,
      float _waitTime = 1f)
    {
      if (Object.op_Equality((Object) this.sceneFade, (Object) null))
        return false;
      switch (_fadeType)
      {
        case 0:
          this.sceneFade._Color = _color;
          this.sceneFade.FadeSet(SimpleFade.Fade.In, _inTime, (Texture2D) null);
          break;
        case 1:
          this.sceneFade._Color = _color;
          this.sceneFade.FadeSet(SimpleFade.Fade.Out, _outTime, (Texture2D) null);
          break;
        case 2:
          this.sceneFade.FadeInOutSet(new SimpleFade.FadeInOut()
          {
            inColor = _color,
            outColor = _color,
            inTime = _inTime,
            outTime = _outTime,
            waitTime = _waitTime
          }, (Texture2D) null);
          break;
      }
      return true;
    }

    public void SetFadeColor(Color _color)
    {
      this.sceneFade.SafeProc<SceneFade>((Action<SceneFade>) (fade => fade._Color = _color));
    }

    public void SetFadeColorDefault()
    {
      this.sceneFade.SafeProc<SceneFade>((Action<SceneFade>) (fade =>
      {
        float a = (float) fade._Color.a;
        fade._Color = this.initFadeColor;
        fade._Color.a = (__Null) (double) a;
      }));
    }

    public void CreateSpace()
    {
      Object.Destroy((Object) this.commonSpace);
      this.commonSpace = new GameObject("CommonSpace");
      Object.DontDestroyOnLoad((Object) this.commonSpace);
    }

    public void SpaceRegister(Transform trans, bool worldPositionStays = false)
    {
      trans.SetParent(this.commonSpace.get_transform(), worldPositionStays);
    }

    [DebuggerHidden]
    public IEnumerator Fade(SimpleFade.Fade fade, Action fadeWaitProc = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CFade\u003Ec__Iterator7()
      {
        fade = fade,
        fadeWaitProc = fadeWaitProc,
        \u0024this = this
      };
    }

    public void DrawImageAndProgress(float value = -1f, float alpha = -1f, bool isLoadingImageDraw = true)
    {
      bool isDraw = (double) value >= 0.0;
      this.progressSlider.SafeProc<Slider>((Action<Slider>) (t =>
      {
        t.set_value(!isDraw ? 0.0f : value);
        ((Component) t).get_gameObject().SetActive(isDraw);
      }));
      if ((double) alpha < 0.0)
        alpha = !isDraw ? 0.0f : 1f;
      this.SetImageAlpha(this.nowLoadingImage, alpha);
      this.nowLoadingImage.SafeProc<Image>((Action<Image>) (t => ((Component) t).get_gameObject().SetActive(isLoadingImageDraw && (double) alpha > 0.0)));
    }

    [DebuggerHidden]
    public IEnumerator LoadStart(Scene.Data data, bool isLoadingImageDraw = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scene.\u003CLoadStart\u003Ec__Iterator8()
      {
        data = data,
        isLoadingImageDraw = isLoadingImageDraw,
        \u0024this = this
      };
    }

    public class SceneStack<T> : Stack<T> where T : Scene.Data
    {
      private List<string> nowSceneNameList = new List<string>();

      public SceneStack(T item)
      {
        base.Push(item);
        this.nowSceneNameList.Push<string>(item.levelName);
      }

      public List<string> NowSceneNameList
      {
        get
        {
          return this.nowSceneNameList;
        }
      }

      public new void Push(T item)
      {
        base.Push(item);
        if (!item.isAdd)
          this.nowSceneNameList.Clear();
        this.nowSceneNameList.Push<string>(item.levelName);
        item.Load();
      }

      public new T Pop()
      {
        T obj1 = base.Pop();
        if (this.nowSceneNameList.Any<string>())
          this.nowSceneNameList.Pop<string>();
        if (!this.nowSceneNameList.Any<string>())
        {
          foreach (T obj2 in (Stack<T>) this)
          {
            this.nowSceneNameList.Add(obj2.levelName);
            if (!obj2.isAdd)
              break;
          }
        }
        AssetBundleManager.UnloadAssetBundle(obj1.assetBundleName, false, obj1.manifestFileName, false);
        Debug.LogFormat("Pop Scene\nlevelName:{0}\nisAdd:{1}\nisAsync:{2}\nassetBundleName:{3}", new object[4]
        {
          (object) obj1.levelName,
          (object) obj1.isAdd,
          (object) obj1.isAsync,
          (object) obj1.assetBundleName
        });
        return obj1;
      }
    }

    public class Data
    {
      public string assetBundleName = string.Empty;
      public string levelName = string.Empty;
      public bool isDrawProgressBar = true;
      public Scene.Data.FadeType fadeType;
      public bool isAdd;
      public bool isAsync;
      public bool isOverlap;
      public string manifestFileName;
      public bool isLoading;

      public bool isFade
      {
        set
        {
          this.fadeType = !value ? Scene.Data.FadeType.None : Scene.Data.FadeType.InOut;
        }
      }

      public bool isFadeIn
      {
        get
        {
          return this.fadeType == Scene.Data.FadeType.InOut || this.fadeType == Scene.Data.FadeType.In;
        }
      }

      public bool isFadeOut
      {
        get
        {
          return this.fadeType == Scene.Data.FadeType.InOut || this.fadeType == Scene.Data.FadeType.Out;
        }
      }

      public AsyncOperation operation { get; private set; }

      public Action onLoad { get; set; }

      public Func<IEnumerator> onFadeIn { get; set; }

      public Func<IEnumerator> onFadeOut { get; set; }

      public AssetBundleLoadLevelOperation assetBundleOperation { get; private set; }

      public AsyncOperation Unload()
      {
        return !this.isAdd ? (AsyncOperation) null : SceneManager.UnloadSceneAsync(this.levelName);
      }

      public void Load()
      {
        if (!this.assetBundleName.IsNullOrEmpty())
        {
          if (!this.isAsync)
            AssetBundleManager.LoadLevel(this.assetBundleName, this.levelName, this.isAdd, this.manifestFileName);
          else
            this.assetBundleOperation = AssetBundleManager.LoadLevelAsync(this.assetBundleName, this.levelName, this.isAdd, this.manifestFileName) as AssetBundleLoadLevelOperation;
        }
        else if (!this.isAsync)
          SceneManager.LoadScene(this.levelName, !this.isAdd ? (LoadSceneMode) 0 : (LoadSceneMode) 1);
        else
          this.operation = SceneManager.LoadSceneAsync(this.levelName, !this.isAdd ? (LoadSceneMode) 0 : (LoadSceneMode) 1);
      }

      public enum FadeType
      {
        None,
        InOut,
        In,
        Out,
      }

      public enum UnloadType
      {
        Success,
        Fail,
        Loaded,
      }
    }

    [Serializable]
    public class FogData
    {
      public FogMode mode = (FogMode) 2;
      public Color color = Color.get_clear();
      public float density = 0.01f;
      public float end = 1000f;
      public bool use;
      public float start;

      public void Change()
      {
        RenderSettings.set_fog(this.use);
        RenderSettings.set_fogMode(this.mode);
        RenderSettings.set_fogColor(this.color);
        RenderSettings.set_fogDensity(this.density);
        RenderSettings.set_fogStartDistance(this.start);
        RenderSettings.set_fogEndDistance(this.end);
        Debug.Log((object) "FogChange");
      }
    }
  }
}
