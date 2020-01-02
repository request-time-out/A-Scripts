// Decompiled with JetBrains decompiler
// Type: Manager.Game
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject;
using AIProject.SaveData;
using AIProject.Scene;
using ConfigScene;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEx;

namespace Manager
{
  public sealed class Game : Singleton<Game>
  {
    public static readonly System.Version Version = new System.Version(1, 0);
    private static bool? _isAdd01 = new bool?();
    public Game.CustomSceneInfo customSceneInfo;
    private AIProject.SaveData.SaveData _bakData;

    private Game()
    {
      this.ReserveSceneName = string.Empty;
    }

    public void SetCustomSceneInfo(string prev, byte type, byte sex, string fileName)
    {
      this.customSceneInfo.previous = prev;
      this.customSceneInfo.type = type;
      this.customSceneInfo.sex = sex;
      this.customSceneInfo.fileName = fileName;
    }

    public static bool isAdd01
    {
      get
      {
        bool? isAdd01 = Game._isAdd01;
        return isAdd01.HasValue ? isAdd01.Value : (Game._isAdd01 = new bool?(AssetBundleCheck.IsManifest("add01"))).Value;
      }
    }

    public bool IsDebug { get; set; }

    public byte UploaderType { get; set; }

    public string ReserveSceneName { get; set; }

    public GlobalSaveData GlobalData { get; private set; }

    public AIProject.SaveData.SaveData Data { get; private set; }

    public WorldData WorldData { get; set; }

    public bool IsAuto { get; set; }

    public AIProject.SaveData.Environment Environment
    {
      get
      {
        return this.WorldData?.Environment;
      }
    }

    public static string PrevPlayerStateFromCharaCreate { get; set; }

    public static int PrevAccessDeviceID { get; set; }

    public bool ExistsBackup()
    {
      if (this._bakData == null || this.WorldData == null && !this.IsAuto)
        return false;
      return this.IsAuto ? this._bakData.AutoData != null : this._bakData.WorldList.ContainsKey(this.WorldData.WorldID);
    }

    public bool ExistsBackup(int id)
    {
      if (this._bakData == null)
        return false;
      return this.IsAuto ? this._bakData.AutoData != null : this._bakData.WorldList.ContainsKey(id);
    }

    public List<ValueTuple<string, string>> AssetBundlePaths { get; set; } = new List<ValueTuple<string, string>>();

    public Dictionary<int, AssetBundleInfo> LoadingSpriteABList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Debug.Log((object) "ーーーーーーーーーーーリソースマネージャー読み込みーーーーーーーーーーー");
      foreach (string str in CommonLib.GetAssetBundleNameListFromPath("scene/common/", false).OrderByDescending<string, string>((Func<string, string>) (bundle => bundle)).ToArray<string>())
      {
        string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
        string manifestAssetBundleName = !(withoutExtension == "00") ? string.Format("add{0}", (object) withoutExtension) : "abdata";
        GameObject gameObject = (GameObject) null;
        foreach (GameObject allAsset in AssetBundleManager.LoadAllAsset(str, typeof (GameObject), manifestAssetBundleName).GetAllAssets<GameObject>())
        {
          if (((Object) allAsset).get_name() == "resrcmanager")
          {
            gameObject = (GameObject) Object.Instantiate<GameObject>((M0) allAsset, ((Component) this).get_transform());
            break;
          }
          this.AssetBundlePaths.Add(new ValueTuple<string, string>(str, manifestAssetBundleName));
        }
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          break;
      }
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      List<string> nameListFromPath1 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ExpList, true);
      nameListFromPath1.Sort();
      foreach (string str in nameListFromPath1)
      {
        string file = str;
        foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
        {
          int key = int.Parse(((Object) allAsset).get_name().Replace("c", string.Empty));
          Dictionary<string, Game.Expression> dic;
          if (!this.CharaExpTable.TryGetValue(key, out dic))
          {
            Dictionary<string, Game.Expression> dictionary = new Dictionary<string, Game.Expression>();
            this.CharaExpTable[key] = dictionary;
            dic = dictionary;
          }
          Game.LoadExpExcelData(dic, allAsset);
          if (!this.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            this.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }
      List<string> nameListFromPath2 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.LoadingSpriteList, false);
      nameListFromPath2.Sort();
      foreach (string str in nameListFromPath2)
      {
        string file = str;
        foreach (LoadingImageData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (LoadingImageData), (string) null).GetAllAssets<LoadingImageData>())
        {
          foreach (LoadingImageData.Param obj in allAsset.param)
            this.LoadingSpriteABList[obj.ID] = new AssetBundleInfo(obj.Name, obj.Bundle, obj.Asset, obj.Manifest);
          if (!this.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            this.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(2, (FrameCountType) 0), (System.Action<M0>) (_ =>
      {
        using (List<ValueTuple<string, string>>.Enumerator enumerator = this.AssetBundlePaths.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ValueTuple<string, string> current = enumerator.Current;
            AssetBundleManager.UnloadAssetBundle((string) current.Item1, true, (string) current.Item2, false);
          }
        }
      }));
      TextScenario.LoadReadInfo();
    }

    private void OnApplicationQuit()
    {
      if (this.IsDebug)
        return;
      TextScenario.SaveReadInfo();
    }

    public ConfigWindow Config { get; set; }

    public ConfirmScene Dialog { get; set; }

    public ExitScene ExitScene { get; set; }

    public MapShortcutUI MapShortcutUI { get; set; }

    public void LoadShortcut()
    {
      MapShortcutUI.ImageIndex = 0;
      MapShortcutUI.ClosedEvent = (System.Action) null;
      Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>("scene/common/00.unity3d", Singleton<Resources>.Instance.DefinePack.SceneNames.MapShortcutScene, false, string.Empty), ((Component) this).get_transform(), false);
    }

    public void LoadShortcut(int index, System.Action closedEvent = null)
    {
      MapShortcutUI.ImageIndex = index;
      MapShortcutUI.ClosedEvent = closedEvent;
      Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>("scene/common/00.unity3d", Singleton<Resources>.Instance.DefinePack.SceneNames.MapShortcutScene, false, string.Empty), ((Component) this).get_transform(), false);
    }

    public void LoadConfig()
    {
      Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>("scene/common/00.unity3d", Singleton<Resources>.Instance.DefinePack.SceneNames.ConfigScene, false, string.Empty), ((Component) this).get_transform(), false);
    }

    public void LoadDialog()
    {
      Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>("scene/common/00.unity3d", Singleton<Resources>.Instance.DefinePack.SceneNames.DialogScene, false, string.Empty), ((Component) this).get_transform(), false);
    }

    public void DestroyDialog()
    {
      Object.Destroy((Object) ((Component) this.Dialog).get_gameObject());
      this.Dialog = (ConfirmScene) null;
      GC.Collect();
      Resources.UnloadUnusedAssets();
    }

    public void LoadExit()
    {
      Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>("scene/common/00.unity3d", Singleton<Resources>.Instance.DefinePack.SceneNames.ExitScene, false, string.Empty), ((Component) this).get_transform(), false);
    }

    public void SaveGlobalData()
    {
      if (this.GlobalData == null)
        return;
      string globalSaveDataFile = AIProject.Definitions.Path.GlobalSaveDataFile;
      if (!System.IO.Directory.Exists(AIProject.Definitions.Path.SaveDataDirectory))
        System.IO.Directory.CreateDirectory(AIProject.Definitions.Path.SaveDataDirectory);
      this.GlobalData.SaveFile(globalSaveDataFile);
    }

    public void LoadGlobalData()
    {
      this.GlobalData = GlobalSaveData.LoadFile(AIProject.Definitions.Path.GlobalSaveDataFile);
      if (this.GlobalData != null)
        return;
      this.GlobalData = new GlobalSaveData();
      this.SaveGlobalData();
    }

    public void SaveProfile(string path)
    {
      if (!System.IO.Directory.Exists(AIProject.Definitions.Path.SaveDataDirectory))
        System.IO.Directory.CreateDirectory(AIProject.Definitions.Path.SaveDataDirectory);
      this.Data.SaveFile(path);
      if (this._bakData == null)
      {
        this._bakData = new AIProject.SaveData.SaveData();
        this._bakData.Copy(this.Data);
      }
      else
      {
        path += ".bak";
        this._bakData.SaveFile(path);
        this._bakData.Copy(this.Data);
      }
    }

    public void LoadProfile(string path)
    {
      this.Data = AIProject.SaveData.SaveData.LoadFile(path);
      if (this.Data == null)
      {
        this.Data = new AIProject.SaveData.SaveData();
      }
      else
      {
        this._bakData = new AIProject.SaveData.SaveData();
        this._bakData.Copy(this.Data);
      }
    }

    public static WorldData CreateInitData(int id, bool cleared = false)
    {
      WorldData worldData = new WorldData();
      worldData.WorldID = id;
      AIProject.SaveData.Environment environment = worldData.Environment;
      environment.Time = new AIProject.SaveData.Environment.SerializableDateTime(1, 1, 1, 10, 0, 0);
      environment.Weather = Weather.Cloud1;
      environment.Temperature = Temperature.Normal;
      if (cleared)
      {
        environment.TutorialProgress = 29;
        Dictionary<int, bool> dictionary1;
        if (!environment.BasePointOpenState.TryGetValue(0, out dictionary1))
        {
          Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
          environment.BasePointOpenState[0] = dictionary2;
          dictionary1 = dictionary2;
        }
        dictionary1[-1] = true;
      }
      worldData.PlayerData.InventorySlotMax = Singleton<Resources>.Instance.PlayerProfile.DefaultInventoryMax;
      int agentMax = Singleton<Resources>.Instance.DefinePack.MapDefines.AgentMax;
      int agentDefaultNum = Singleton<Resources>.Instance.DefinePack.MapDefines.AgentDefaultNum;
      for (int index = 0; index < agentMax; ++index)
      {
        AgentData agentData1 = new AgentData();
        worldData.AgentTable[index] = agentData1;
        AgentData agentData2 = agentData1;
        agentData2.OpenState = index < 1;
        agentData2.PlayEnterScene = index < 1;
      }
      return worldData;
    }

    public static bool IsFirstGame
    {
      get
      {
        return Singleton<Game>.IsInstance() && !Singleton<Game>.Instance.ExistsBackup();
      }
    }

    public static bool IsFreeMode
    {
      get
      {
        if (!Singleton<Game>.IsInstance())
          return false;
        Game instance = Singleton<Game>.Instance;
        return instance.WorldData != null && instance.WorldData.FreeMode;
      }
    }

    public Dictionary<int, Dictionary<string, Game.Expression>> CharaExpTable { get; private set; } = new Dictionary<int, Dictionary<string, Game.Expression>>();

    public static void LoadExpExcelData(
      Dictionary<string, Game.Expression> dic,
      ExcelData excelData)
    {
      foreach (ExcelData.Param obj in excelData.list)
      {
        if (!obj.list.IsNullOrEmpty<string>())
        {
          Game.Expression expression = new Game.Expression(obj.list.Skip<string>(1).ToArray<string>())
          {
            IsChangeSkip = true
          };
          dic[obj.list[0]] = expression;
        }
      }
    }

    public static Game.Expression GetExpression(
      Dictionary<string, Game.Expression> dic,
      string key)
    {
      Game.Expression expression;
      dic.TryGetValue(key, out expression);
      return expression;
    }

    public Game.Expression GetExpression(int personality, string key)
    {
      Dictionary<string, Game.Expression> dic;
      return !this.CharaExpTable.TryGetValue(personality, out dic) ? (Game.Expression) null : Game.GetExpression(dic, key);
    }

    public void RemoveWorldData(int id)
    {
      bool flag = false;
      if (this.Data != null && !this.Data.WorldList.IsNullOrEmpty<int, WorldData>() && !this.Data.WorldList.IsNullOrEmpty<int, WorldData>())
        flag |= this.Data.WorldList.Remove(id);
      if (this._bakData != null && !this._bakData.WorldList.IsNullOrEmpty<int, WorldData>())
        flag |= this._bakData.WorldList.Remove(id);
      if (!flag)
        return;
      this.SaveProfile(AIProject.Definitions.Path.WorldSaveDataFile);
    }

    public class Expression
    {
      protected bool _useEyebrow;
      protected bool _useEyes;
      protected bool _useMouth;
      protected bool _useEyebrowOpen;
      protected bool _useEyesOpen;
      protected bool _useMouthOpen;
      protected bool _useEyesLook;
      protected bool _useHohoAkaRate;
      protected bool _useHighlight;
      protected bool _useTearsLv;
      protected bool _useBlink;

      public Expression()
      {
      }

      public Expression(Game.Expression other)
      {
        Game.Expression.Copy(other, this);
      }

      public Expression(string[] args, ref int index)
      {
        this.Initialize(args, ref index, false);
      }

      public Expression(string[] args)
      {
        int index = 0;
        this.Initialize(args, ref index, false);
      }

      public bool IsChangeSkip { private get; set; }

      public Game.Expression.Pattern Eyebrow { get; private set; }

      public Game.Expression.Pattern Eyes { get; private set; }

      public Game.Expression.Pattern Mouth { get; private set; }

      public float EyebrowOpen { get; private set; } = 1f;

      public float EyesOpen { get; private set; } = 1f;

      public float MouthOpen { get; private set; } = 1f;

      public int EyesLook { get; private set; }

      public float HohoAkaRate { get; private set; }

      public bool IsHighlight { get; private set; } = true;

      public float TearsRate { get; private set; }

      public bool IsBlink { get; private set; } = true;

      public virtual void Initialize(string[] args, ref int index, bool isThrow = false)
      {
        try
        {
          string[] source1 = args;
          int num1;
          index = (num1 = index) + 1;
          int index1 = num1;
          string element1 = source1.GetElement<string>(index1);
          this._useEyebrow = !element1.IsNullOrEmpty();
          if (this._useEyebrow)
            this.Eyebrow = new Game.Expression.Pattern(element1, true);
          string[] source2 = args;
          int num2;
          index = (num2 = index) + 1;
          int index2 = num2;
          string element2 = source2.GetElement<string>(index2);
          this._useEyes = !element2.IsNullOrEmpty();
          if (this._useEyes)
            this.Eyes = new Game.Expression.Pattern(element2, true);
          string[] source3 = args;
          int num3;
          index = (num3 = index) + 1;
          int index3 = num3;
          string element3 = source3.GetElement<string>(index3);
          this._useMouth = !element3.IsNullOrEmpty();
          if (this._useMouth)
            this.Mouth = new Game.Expression.Pattern(element3, true);
          string[] source4 = args;
          int num4;
          index = (num4 = index) + 1;
          int index4 = num4;
          string element4 = source4.GetElement<string>(index4);
          this._useEyebrowOpen = !element4.IsNullOrEmpty();
          if (this._useEyebrowOpen)
            this.EyebrowOpen = float.Parse(element4);
          string[] source5 = args;
          int num5;
          index = (num5 = index) + 1;
          int index5 = num5;
          string element5 = source5.GetElement<string>(index5);
          this._useEyesOpen = !element5.IsNullOrEmpty();
          if (this._useEyesOpen)
            this.EyesOpen = float.Parse(element5);
          string[] source6 = args;
          int num6;
          index = (num6 = index) + 1;
          int index6 = num6;
          string element6 = source6.GetElement<string>(index6);
          this._useMouthOpen = !element6.IsNullOrEmpty();
          if (this._useMouthOpen)
            this.MouthOpen = float.Parse(element6);
          string[] source7 = args;
          int num7;
          index = (num7 = index) + 1;
          int index7 = num7;
          string element7 = source7.GetElement<string>(index7);
          this._useEyesLook = !element7.IsNullOrEmpty();
          if (this._useEyesLook)
            this.EyesLook = int.Parse(element7);
          string[] source8 = args;
          int num8;
          index = (num8 = index) + 1;
          int index8 = num8;
          string element8 = source8.GetElement<string>(index8);
          this._useHohoAkaRate = !element8.IsNullOrEmpty();
          if (this._useHohoAkaRate)
            this.HohoAkaRate = float.Parse(element8);
          string[] source9 = args;
          int num9;
          index = (num9 = index) + 1;
          int index9 = num9;
          string element9 = source9.GetElement<string>(index9);
          this._useHighlight = !element9.IsNullOrEmpty();
          if (this._useHighlight)
            this.IsHighlight = bool.Parse(element9);
          string[] source10 = args;
          int num10;
          index = (num10 = index) + 1;
          int index10 = num10;
          string element10 = source10.GetElement<string>(index10);
          this._useTearsLv = !element10.IsNullOrEmpty();
          if (this._useTearsLv)
            this.TearsRate = float.Parse(element10);
          string[] source11 = args;
          int num11;
          index = (num11 = index) + 1;
          int index11 = num11;
          string element11 = source11.GetElement<string>(index11);
          this._useBlink = !element11.IsNullOrEmpty();
          if (!this._useBlink)
            return;
          this.IsBlink = bool.Parse(element11);
        }
        catch (Exception ex)
        {
          if (isThrow)
            throw new Exception(string.Format("Expression:{0}", (object) string.Join(",", args)));
          Debug.LogError((object) string.Format("Expression:{0}", (object) string.Join(",", args)));
        }
      }

      public static void Copy(Game.Expression source, Game.Expression destination)
      {
        Game.Expression expression1 = destination;
        if (!(source?.Eyebrow.Clone() is Game.Expression.Pattern pattern))
          pattern = new Game.Expression.Pattern();
        expression1.Eyebrow = pattern;
        Game.Expression expression2 = destination;
        if (!(source.Eyes?.Clone() is Game.Expression.Pattern pattern))
          pattern = new Game.Expression.Pattern();
        expression2.Eyes = pattern;
        Game.Expression expression3 = destination;
        if (!(source.Mouth?.Clone() is Game.Expression.Pattern pattern))
          pattern = new Game.Expression.Pattern();
        expression3.Mouth = pattern;
        destination.EyebrowOpen = source.EyebrowOpen;
        destination.EyesOpen = source.EyesOpen;
        destination.MouthOpen = source.MouthOpen;
        destination.EyesLook = source.EyesLook;
        destination.HohoAkaRate = source.HohoAkaRate;
        destination.TearsRate = source.TearsRate;
        destination.IsBlink = source.IsBlink;
        destination.IsChangeSkip = source.IsChangeSkip;
        destination._useEyebrow = source._useEyebrow;
        destination._useEyes = source._useEyes;
        destination._useMouth = source._useMouth;
        destination._useEyebrowOpen = source._useEyebrowOpen;
        destination._useEyesOpen = source._useEyesOpen;
        destination._useMouthOpen = source._useMouthOpen;
        destination._useEyesLook = source._useEyesLook;
        destination._useHohoAkaRate = source._useHohoAkaRate;
        destination._useTearsLv = source._useTearsLv;
        destination._useBlink = source._useBlink;
      }

      public void Copy(Game.Expression destination)
      {
        Game.Expression.Copy(this, destination);
      }

      public void Change(ChaControl charInfo)
      {
        bool isChangeSkip = this.IsChangeSkip;
        if (!isChangeSkip || this._useEyebrow)
          charInfo.ChangeEyebrowPtn(this.Eyebrow.Ptn, this.Eyebrow.Blend);
        if (!isChangeSkip || this._useEyes)
          charInfo.ChangeEyesPtn(this.Eyes.Ptn, this.Eyes.Blend);
        if (!isChangeSkip || this._useMouth)
          charInfo.ChangeMouthPtn(this.Mouth.Ptn, this.Mouth.Blend);
        if (!isChangeSkip || this._useEyebrowOpen)
          charInfo.ChangeEyebrowOpenMax(this.EyebrowOpen);
        if (!isChangeSkip || this._useEyesOpen)
          charInfo.ChangeEyesOpenMax(this.EyesOpen);
        if (!isChangeSkip || this._useMouthOpen)
          charInfo.ChangeMouthOpenMax(this.MouthOpen);
        if ((!isChangeSkip || this._useEyesLook) && this.EyesLook != -1)
          charInfo.ChangeLookEyesPtn(this.EyesLook);
        if (!isChangeSkip || this._useHohoAkaRate)
          charInfo.ChangeHohoAkaRate(this.HohoAkaRate);
        if (!isChangeSkip || this._useHighlight)
          charInfo.HideEyeHighlight(!this.IsHighlight);
        if (!isChangeSkip || this._useTearsLv)
          charInfo.ChangeTearsRate(this.TearsRate);
        if (this.IsChangeSkip && !this._useBlink)
          return;
        charInfo.ChangeEyesBlinkFlag(this._useBlink);
      }

      public class Pattern : ICloneable
      {
        public Pattern()
        {
          this.Initialize();
        }

        public Pattern(string arg, bool isThrow = false)
        {
          this.Initialize();
          if (arg.IsNullOrEmpty())
            return;
          string[] strArray = arg.Split(',');
          int num1 = 0;
          try
          {
            string[] source1 = strArray;
            int index1 = num1;
            int num2 = index1 + 1;
            string element1 = source1.GetElement<string>(index1);
            if (element1 != null)
              this.Ptn = int.Parse(element1);
            string[] source2 = strArray;
            int index2 = num2;
            int num3 = index2 + 1;
            string element2 = source2.GetElement<string>(index2);
            if (element2 == null)
              return;
            this.Blend = bool.Parse(element2);
          }
          catch (Exception ex)
          {
            if (isThrow)
              throw new Exception(string.Format("Expression Pattern:{0}", (object) string.Join(",", strArray)));
            Debug.LogError((object) string.Format("Expression Pattern:{0}", (object) string.Join(", ", strArray)));
          }
        }

        public int Ptn { get; set; }

        public bool Blend { get; set; }

        public void Initialize()
        {
          this.Ptn = 0;
          this.Blend = true;
        }

        public object Clone()
        {
          return this.MemberwiseClone();
        }
      }
    }

    public struct CustomSceneInfo
    {
      public string previous;
      public byte type;
      public byte sex;
      public bool isFemale;
      public string fileName;
    }
  }
}
