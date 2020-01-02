// Decompiled with JetBrains decompiler
// Type: ADV.TextScenario
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV.Commands.Base;
using AIChara;
using AIProject;
using AIProject.CaptionScript;
using AIProject.SaveData;
using AIProject.UI;
using Cinemachine;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ADV
{
  [RequireComponent(typeof (CommandController))]
  public class TextScenario : MonoBehaviour
  {
    private static TextScenario.AlreadyReadInfo readInfo;
    [SerializeField]
    private Captions _captions;
    private bool _clicked;
    private IDisposable _choiceCommandDis;
    [SerializeField]
    private ADVUI _advUI;
    [SerializeField]
    private Regulate _regulate;
    [SerializeField]
    private Info _info;
    [SerializeField]
    private Camera _AdvCamera;
    private CinemachineVirtualCamera _virtualCamera;
    private CrossFade _crossFade;
    public TextScenario.CurrentCharaData currentCharaData;
    [SerializeField]
    private OpenData _openData;
    private CommandController _commandController;
    private ADVScene _advScene;
    [SerializeField]
    private ADVFade advFade;
    [SerializeField]
    private Camera backCamera;
    [SerializeField]
    private Transform characters;
    [SerializeField]
    private Image filterImage;
    [SerializeField]
    protected BoolReactiveProperty _isSkip;
    [SerializeField]
    protected BoolReactiveProperty _isAuto;
    [SerializeField]
    protected bool _isWait;
    [Header("Debug表示")]
    [SerializeField]
    protected int currentLine;
    [SerializeField]
    protected float autoWaitTimer;
    [SerializeField]
    protected float autoWaitTime;
    [SerializeField]
    protected bool _isText;
    [SerializeField]
    private bool _isSceneFadeRegulate;
    [SerializeField]
    private bool _isStartRun;
    private Illusion.Game.Elements.Single _single;
    private IDisposable voicePlayDis;
    private List<TextScenario.LoopVoicePack> _loopVoiceList;
    public const int VOICE_SET_NO = 1;

    public TextScenario()
    {
      base.\u002Ector();
    }

    public static void LoadReadInfo()
    {
      if (TextScenario.readInfo != null)
        return;
      TextScenario.readInfo = new TextScenario.AlreadyReadInfo();
    }

    public static void SaveReadInfo()
    {
      if (TextScenario.readInfo == null)
        return;
      TextScenario.readInfo.Save();
    }

    public event Action<string, string, IReadOnlyCollection<TextScenario.IVoice[]>> TextLog;

    public event Action VisibleLog;

    public void TextLogCall(ADV.Commands.Base.Text.Data data, IReadOnlyCollection<TextScenario.IVoice[]> voices)
    {
      if (this.TextLog == null)
        return;
      this.TextLog(data.name, data.text, voices);
    }

    public void FadeIn(float duration, bool ignoreTimeScale = true)
    {
      MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (FadeType) 0, duration, ignoreTimeScale);
    }

    public void FadeOut(float duration, bool ignoreTimeScale = true)
    {
      MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (FadeType) 1, duration, ignoreTimeScale);
    }

    public bool Fading
    {
      get
      {
        return MapUIContainer.FadeCanvas.IsFading;
      }
    }

    public CaptionSystem captionSystem
    {
      get
      {
        return this.captions.CaptionSystem;
      }
    }

    public Captions captions
    {
      get
      {
        return this._captions ?? (this._captions = Singleton<Manager.ADV>.Instance.Captions);
      }
    }

    private Input input
    {
      get
      {
        return Singleton<Input>.Instance;
      }
    }

    public string ChoiceON(string title, CommCommandList.CommandInfo[] options)
    {
      string text = MapUIContainer.ChoiceUI.Label.get_text();
      MapUIContainer.SetActiveChoiceUI(true, title);
      MapUIContainer.ChoiceUI.Refresh(options, MapUIContainer.ChoiceUI.CanvasGroup, (Action) null);
      return text;
    }

    public void ChoiceOFF(string label)
    {
      if (this._choiceCommandDis != null)
        this._choiceCommandDis.Dispose();
      this._choiceCommandDis = (IDisposable) null;
      if (!label.IsNullOrEmpty())
        this._choiceCommandDis = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) MapUIContainer.ChoiceUI.OnCompletedStopAsObservable(), 1), (Action<M0>) (_ => MapUIContainer.ChoiceUI.Label.set_text(label)));
      MapUIContainer.ChoiceUI.Visibled = false;
    }

    public ADVUI advUI
    {
      get
      {
        return this._advUI;
      }
    }

    public Regulate regulate
    {
      get
      {
        return this._regulate;
      }
    }

    public Info info
    {
      get
      {
        return this._info;
      }
    }

    public Camera AdvCamera
    {
      get
      {
        return this._AdvCamera;
      }
      set
      {
        this._AdvCamera = value;
      }
    }

    public CinemachineVirtualCamera virtualCamera
    {
      get
      {
        return this._virtualCamera;
      }
      set
      {
        this._virtualCamera = value;
      }
    }

    public void CrossFadeStart()
    {
      if (Object.op_Equality((Object) this._crossFade, (Object) null) || !this.isFadeAllEnd)
        return;
      this._crossFade.FadeStart(this.info.anime.play.crossFadeTime);
    }

    public CrossFade crossFade
    {
      get
      {
        return this._crossFade;
      }
    }

    public List<Program.Transfer> transferList
    {
      get
      {
        return this._transferList;
      }
      set
      {
        this._transferList = value;
      }
    }

    private List<Program.Transfer> _transferList { get; set; }

    public string fontColorKey { get; set; }

    public bool isSkip
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isSkip).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._isSkip).set_Value(value);
      }
    }

    public bool isAuto
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isAuto).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._isAuto).set_Value(value);
      }
    }

    public bool isWait
    {
      get
      {
        return this._isWait;
      }
      set
      {
        this._isWait = value;
      }
    }

    public int CurrentLine
    {
      get
      {
        return this.currentLine - 1;
      }
      set
      {
        this.currentLine = value;
      }
    }

    public OpenData openData
    {
      get
      {
        return this._openData;
      }
    }

    public string LoadBundleName
    {
      get
      {
        return this._openData.bundle;
      }
      set
      {
        this._openData.bundle = value;
      }
    }

    public string LoadAssetName
    {
      get
      {
        return this._openData.asset;
      }
      set
      {
        this._openData.asset = value;
      }
    }

    public bool isSceneFadeRegulate
    {
      get
      {
        return this._isSceneFadeRegulate;
      }
      set
      {
        this._isSceneFadeRegulate = value;
      }
    }

    public bool isCameraLock
    {
      get
      {
        return true;
      }
    }

    public bool isBackGroundCommanding { get; set; }

    public CommandController commandController
    {
      get
      {
        return ((Component) this).GetComponentCache<CommandController>(ref this._commandController);
      }
    }

    public ADVScene advScene
    {
      get
      {
        return ((Component) this).GetComponentCache<ADVScene>(ref this._advScene);
      }
    }

    public List<ScenarioData.Param> CommandPacks
    {
      get
      {
        return this.commandPacks;
      }
    }

    public bool isBackGroundCommandProcessing
    {
      get
      {
        return this.backCommandList.Count > 0;
      }
    }

    public Image FilterImage
    {
      get
      {
        return this.filterImage;
      }
    }

    public bool isFadeAllEnd
    {
      get
      {
        if (!Singleton<Manager.Scene>.IsInstance() || Object.op_Equality((Object) this.advScene, (Object) null) || (Object.op_Equality((Object) this.advScene.AdvFade, (Object) null) || Object.op_Equality((Object) this._crossFade, (Object) null)))
          return true;
        return !Singleton<Manager.Scene>.Instance.IsFadeNow && this.advScene.AdvFade.IsEnd && this._crossFade.isEnd;
      }
    }

    protected List<ScenarioData.Param> commandPacks { get; }

    private TextScenario.FileOpen fileOpenData { get; }

    protected bool isRequestLine { get; set; }

    protected HashSet<int> textHash { get; }

    public Dictionary<string, ValData> Vars
    {
      get
      {
        return this.vars;
      }
    }

    private Dictionary<string, ValData> vars { get; }

    public Dictionary<string, string> Replaces
    {
      get
      {
        return this.replaces;
      }
    }

    private Dictionary<string, string> replaces { get; }

    public void SetPackage(IPack package)
    {
      this.package = package;
      this.SetCharacters(package?.param);
    }

    public IPack package { get; private set; }

    public void SetCharacters(IParams[] param)
    {
      if (param == null)
        return;
      TextScenario.ParamData[] array = ((IEnumerable<IParams>) param).Select<IParams, TextScenario.ParamData>((Func<IParams, TextScenario.ParamData>) (p => new TextScenario.ParamData(p))).ToArray<TextScenario.ParamData>();
      this.heroineList = ((IEnumerable<TextScenario.ParamData>) array).Where<TextScenario.ParamData>((Func<TextScenario.ParamData, bool>) (p => p.isHeroine)).ToList<TextScenario.ParamData>();
      this.player = ((IEnumerable<TextScenario.ParamData>) array).FirstOrDefault<TextScenario.ParamData>((Func<TextScenario.ParamData, bool>) (p => p.isPlayer));
      CharaData.MotionReserver motionReserver = (CharaData.MotionReserver) null;
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType3<TextScenario.ParamData, int> anonType3 in this.heroineList.Select<TextScenario.ParamData, \u003C\u003E__AnonType3<TextScenario.ParamData, int>>((Func<TextScenario.ParamData, int, \u003C\u003E__AnonType3<TextScenario.ParamData, int>>) ((data, index) => new \u003C\u003E__AnonType3<TextScenario.ParamData, int>(data, index))))
        this.commandController.AddChara(anonType3.index, new CharaData(anonType3.data, this, motionReserver));
      if (this.player != null)
        this.commandController.AddChara(-1, new CharaData(this.player, this, motionReserver));
      if (!this.commandController.Characters.Any<KeyValuePair<int, CharaData>>())
        return;
      this.ChangeCurrentChara(this.commandController.Characters.First<KeyValuePair<int, CharaData>>().Key);
    }

    public TextScenario.ParamData player { get; private set; }

    public List<TextScenario.ParamData> heroineList { get; private set; }

    public CharaData currentChara
    {
      get
      {
        return this._currentChara;
      }
      set
      {
        this._currentChara = value;
      }
    }

    private CharaData _currentChara { get; set; }

    private BackupPosRot backCameraBackup { get; set; }

    private CommandList nowCommandList
    {
      get
      {
        return this._commandController.NowCommandList;
      }
    }

    private CommandList backCommandList
    {
      get
      {
        return this._commandController.BackGroundCommandList;
      }
    }

    public bool ChangeCurrentChara(int no)
    {
      this.currentChara = this.commandController.GetChara(no);
      return this.currentChara != null;
    }

    public string ReplaceVars(string arg)
    {
      ValData valData;
      return !this.Vars.TryGetValue(arg, out valData) ? arg : valData.o.ToString();
    }

    public string ReplaceText(string text)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int startIndex = 0;
      try
      {
        foreach (Match match in Regex.Matches(text, "\\[.*?\\]"))
        {
          if (match.Success)
          {
            string key = string.Empty;
            try
            {
              key = Regex.Replace(match.Value, "\\[|\\]", string.Empty);
            }
            catch (Exception ex)
            {
              Debug.LogErrorFormat("Text {0} Regex.Replace Error\n{1}", new object[2]
              {
                (object) match.Value,
                (object) text
              });
            }
            string str;
            if (this.Replaces.TryGetValue(key, out str))
            {
              if (!str.IsNullOrEmpty())
                str = this.ReplaceText(str);
              if (str == null)
                str = match.Value;
            }
            else
              str = match.Value;
            stringBuilder.Append(text.Substring(startIndex, match.Index - startIndex));
            stringBuilder.Append(str);
            startIndex = match.Index + match.Length;
          }
          else
            Debug.LogError((object) ("Text Match.Failed\n" + text));
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Text Regex.Match Error\n" + text));
      }
      stringBuilder.Append(text.Substring(startIndex, text.Length - startIndex));
      return stringBuilder.ToString();
    }

    public IObservable<Unit> OnInitializedAsync
    {
      get
      {
        return (IObservable<Unit>) this._single;
      }
    }

    public virtual void Initialize()
    {
      this.commandPacks.Clear();
      this.fileOpenData.Clear();
      this._regulate.SetRegulate((Regulate.Control) 0);
      if (this.backCameraBackup != null && Object.op_Inequality((Object) this.backCamera, (Object) null))
        this.backCameraBackup.Set(((Component) this.backCamera).get_transform());
      this.currentLine = 0;
      this.autoWaitTimer = 0.0f;
      this.commandController.Initialize();
      this.captionSystem.Clear();
      this.vars.Clear();
      this.replaces.Clear();
      this.advFade.SafeProcObject<ADVFade>((Action<ADVFade>) (p => p.Initialize()));
      this._single.Done();
      TextScenario.LoadReadInfo();
    }

    protected void MemberInit()
    {
      this.isSkip = false;
      this.isAuto = false;
      this._isWait = false;
      this._isSceneFadeRegulate = true;
      this._isStartRun = false;
      this.textHash.Clear();
    }

    public virtual void Release()
    {
      if (this.voicePlayDis != null)
        this.voicePlayDis.Dispose();
      this.voicePlayDis = (IDisposable) null;
      if (this._choiceCommandDis != null)
        this._choiceCommandDis.Dispose();
      this._choiceCommandDis = (IDisposable) null;
      this.loopVoiceList.ForEach((Action<TextScenario.LoopVoicePack>) (p =>
      {
        if (!Object.op_Inequality((Object) p.audio, (Object) null))
          return;
        Object.Destroy((Object) ((Component) p.audio).get_gameObject());
      }));
      this.loopVoiceList.Clear();
      this._single = new Illusion.Game.Elements.Single();
      this.commandController.Release();
      this.AdvCamera = (Camera) null;
      this.info.audio.is2D = false;
      this.info.audio.isNotMoveMouth = false;
      if (Singleton<Manager.Game>.IsInstance())
        return;
      TextScenario.SaveReadInfo();
    }

    public void CommandAdd(bool isNext, int line, bool multi, Command command, string[] args)
    {
      List<string> stringList1 = new List<string>();
      stringList1.Add("0");
      stringList1.Add(multi.ToString());
      stringList1.Add(command.ToString());
      List<string> stringList2 = stringList1;
      string[] strArray = args;
      if (strArray == null)
        strArray = new string[1]{ string.Empty };
      stringList2.AddRange((IEnumerable<string>) strArray);
      ScenarioData.Param obj = new ScenarioData.Param(stringList1.ToArray());
      if (this.commandPacks.Count == line)
        this.commandPacks.Add(obj);
      else
        this.commandPacks.Insert(line, obj);
      if (!isNext)
        return;
      this.RequestNextLine();
    }

    public virtual bool LoadFile(
      string bundle,
      string asset,
      bool isClear = true,
      bool isClearCheck = true,
      bool isNext = true)
    {
      if (isClear)
      {
        this.captionSystem.Clear();
        this.fileOpenData.Clear();
      }
      if (bundle.IsNullOrEmpty())
        bundle = this.LoadBundleName;
      if (!isClear && isClearCheck && this.fileOpenData.FileList.Any<RootData>((Func<RootData, bool>) (p => p.bundleName == bundle && p.fileName == asset)))
        return false;
      this.openData.Load(bundle, asset);
      if (!isClear)
      {
        this.commandPacks.InsertRange(this.currentLine, (IEnumerable<ScenarioData.Param>) this.openData.data.list);
        if (!this.fileOpenData.FileList.Any<RootData>((Func<RootData, bool>) (p => p.bundleName == bundle && p.fileName == asset && p.line == this.currentLine)))
          this.fileOpenData.FileList.Add(new RootData()
          {
            bundleName = bundle,
            fileName = asset,
            line = this.CurrentLine
          });
      }
      else
      {
        this.LoadBundleName = bundle;
        this.LoadAssetName = asset;
        string[] args = new string[5]
        {
          bundle,
          asset,
          bool.FalseString,
          bool.TrueString,
          bool.TrueString
        };
        this.currentLine = 0;
        this.commandPacks.Clear();
        this.CommandAdd(false, this.currentLine++, false, Command.Open, args);
        this.commandPacks.AddRange((IEnumerable<ScenarioData.Param>) this.openData.data.list);
      }
      if (isNext)
        this.RequestNextLine();
      this.openData.ClearData();
      return true;
    }

    public bool SearchTagJumpOrOpenFile(string jump, int localLine)
    {
      string[] strArray = jump.Split(':');
      if (strArray.Length == 1)
      {
        int n;
        if (this.SearchTag(jump, out n))
        {
          this.Jump(n);
          return true;
        }
        Debug.LogWarningFormat("not jump : {0}\nLine : {1}", new object[2]
        {
          (object) jump,
          (object) localLine
        });
        return false;
      }
      Open open = new Open();
      open.Set(Command.Open);
      string[] argsDefault = open.ArgsDefault;
      for (int index = 0; index < strArray.Length && index < argsDefault.Length; ++index)
        argsDefault[index] = this.ReplaceVars(strArray[index]);
      this.CommandAdd(false, localLine + 1, false, open.command, argsDefault);
      return true;
    }

    public bool SearchTag(string tagName, out int n)
    {
      n = this.commandPacks.TakeWhile<ScenarioData.Param>((Func<ScenarioData.Param, bool>) (p => p.Command != Command.Tag || this.ReplaceVars(p.Args[0]) != tagName)).Count<ScenarioData.Param>();
      return n < this.commandPacks.Count;
    }

    public void Jump(int n)
    {
      this.currentLine = n;
      this.RequestNextLine();
    }

    public virtual void ConfigProc()
    {
      if (!Manager.Config.initialized)
        return;
      this.autoWaitTime = Manager.Config.GameData.AutoWaitTime;
    }

    public void BackGroundCommandProcessEnd()
    {
      this.backCommandList.ProcessEnd();
    }

    public List<TextScenario.LoopVoicePack> loopVoiceList
    {
      get
      {
        return this._loopVoiceList;
      }
    }

    public void VoicePlay(List<TextScenario.IVoice[]> voices, Action onChange, Action onEnd)
    {
      if (this.voicePlayDis != null)
        this.voicePlayDis.Dispose();
      this.voicePlayDis = (IDisposable) null;
      Singleton<Manager.Voice>.Instance.StopAll(false);
      if (voices == null)
        return;
      if (this._loopVoiceList.Any<TextScenario.LoopVoicePack>())
      {
        HashSet<int> intSet = new HashSet<int>();
        foreach (TextScenario.IVoice[] voice1 in voices)
        {
          foreach (TextScenario.IVoice voice2 in voice1)
            intSet.Add(voice2.personality);
        }
        foreach (int num in intSet)
        {
          foreach (TextScenario.LoopVoicePack loopVoice in this._loopVoiceList)
          {
            if (loopVoice.voiceNo == num && Object.op_Inequality((Object) loopVoice.audio, (Object) null))
              loopVoice.audio.Pause();
          }
        }
      }
      this.voicePlayDis = ObservableExtensions.Subscribe<Unit>(Observable.FromCoroutine<Unit>((Func<IObserver<M0>, IEnumerator>) (observer => this.VoicePlayCoroutine(observer, voices))), (Action<M0>) (_ => onChange.Call()), (Action) (() =>
      {
        List<TextScenario.LoopVoicePack> loopVoicePackList = new List<TextScenario.LoopVoicePack>();
        foreach (TextScenario.LoopVoicePack loopVoice in this._loopVoiceList)
        {
          if (!loopVoice.Set() || Object.op_Equality((Object) loopVoice.audio, (Object) null))
            loopVoicePackList.Add(loopVoice);
          else
            loopVoice.audio.Play();
        }
        loopVoicePackList.ForEach((Action<TextScenario.LoopVoicePack>) (item => this._loopVoiceList.Remove(item)));
        onEnd.Call();
      }));
    }

    [DebuggerHidden]
    private IEnumerator VoicePlayCoroutine(
      IObserver<Unit> observer,
      List<TextScenario.IVoice[]> voiceList)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TextScenario.\u003CVoicePlayCoroutine\u003Ec__Iterator0()
      {
        voiceList = voiceList,
        observer = observer
      };
    }

    protected virtual bool StartRun()
    {
      if (this._isStartRun)
        return false;
      this.ADVCameraSetting();
      this._isStartRun = true;
      int line = 0;
      if (!this._transferList.IsNullOrEmpty<Program.Transfer>())
      {
        Program.Transfer transfer1 = this._transferList[0];
        if (transfer1.param.Command == Command.SceneFadeRegulate)
          this._isSceneFadeRegulate = bool.Parse(transfer1.param.Args[0]);
        foreach (Program.Transfer transfer2 in this._transferList)
        {
          this.CommandAdd(false, transfer2.line != -1 ? transfer2.line : line, transfer2.param.Multi, transfer2.param.Command, transfer2.param.Args);
          ++line;
        }
      }
      if (!this.LoadBundleName.IsNullOrEmpty() && !this.LoadAssetName.IsNullOrEmpty())
      {
        string[] args = new string[5]
        {
          this.LoadBundleName,
          this.LoadAssetName,
          bool.FalseString,
          bool.TrueString,
          bool.TrueString
        };
        this.CommandAdd(true, line, false, Command.Open, args);
      }
      else if (this._openData.HasData)
      {
        this.commandPacks.AddRange((IEnumerable<ScenarioData.Param>) this.openData.data.list);
        this.RequestNextLine();
        this.openData.ClearData();
      }
      return true;
    }

    protected void RequestNextLine()
    {
      this.StartCoroutine(this._RequestNextLine());
    }

    [DebuggerHidden]
    protected virtual IEnumerator _RequestNextLine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TextScenario.\u003C_RequestNextLine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    protected void ADVCameraSetting()
    {
    }

    protected bool MessageWindowProc(TextScenario.NextInfo nextInfo)
    {
      if (this.isRequestLine)
        return false;
      this.backCommandList.Process();
      if (this.nowCommandList.Process() || this.commandPacks.Count == 0)
        return false;
      if (this.regulate.control.HasFlag((Enum) Regulate.Control.ClickNext))
        nextInfo.isNext = false;
      if (this.regulate.control.HasFlag((Enum) Regulate.Control.Skip))
      {
        nextInfo.isSkip = false;
        this.isSkip = false;
      }
      if (this.regulate.control.HasFlag((Enum) Regulate.Control.Auto))
        this.isAuto = false;
      if (this.regulate.control.HasFlag((Enum) Regulate.Control.AutoForce))
        this.isAuto = true;
      bool completeDisplayText = nextInfo.isCompleteDisplayText;
      bool isNext = nextInfo.isNext;
      bool isSkip = nextInfo.isSkip;
      if (!completeDisplayText)
      {
        if (isNext || this.isSkip || isSkip)
        {
          this.captionSystem.ForceCompleteDisplayText();
          this.nowCommandList.ProcessEnd();
        }
        return false;
      }
      this.autoWaitTimer = Mathf.Min(this.autoWaitTimer + Time.get_deltaTime(), this.autoWaitTime);
      bool flag1 = this.nowCommandList.Count > 0;
      bool flag2 = this.textHash.Count > 0;
      bool flag3 = false;
      if (isSkip || this.isSkip)
        flag3 = ((flag3 ? 1 : 0) | 1) != 0;
      else if (this.isAuto && flag2)
        flag3 = ((flag3 ? 1 : 0) | ((double) this.autoWaitTimer < (double) this.autoWaitTime ? 0 : (!flag1 ? 1 : 0))) != 0;
      bool flag4 = flag3 | isNext;
      if (this.regulate.control.HasFlag((Enum) Regulate.Control.Next))
        flag4 = false;
      bool flag5 = ((flag4 ? 1 : 0) | (flag2 ? 0 : (!flag1 ? 1 : 0))) != 0;
      if (flag5)
      {
        this.currentCharaData.Clear();
        this.RequestNextLine();
      }
      return flag5;
    }

    protected virtual void UpdateBefore()
    {
    }

    protected virtual bool UpdateRegulate()
    {
      if (Manager.Scene.isReturnTitle || Manager.Scene.isGameEnd || (!Singleton<Manager.Scene>.IsInstance() || !Singleton<Manager.Game>.IsInstance()) || (Singleton<Manager.Scene>.Instance.IsNowLoading || this._isWait))
        return true;
      this.StartRun();
      foreach (CharaData charaData in this.commandController.Characters.Values)
      {
        if (!charaData.initialized)
          return true;
      }
      return this._isSceneFadeRegulate && Singleton<Manager.Scene>.Instance.sceneFade.IsFadeNow || (Singleton<Manager.Scene>.Instance.IsOverlap || Mathf.Max(0, Singleton<Manager.Scene>.Instance.NowSceneNames.IndexOf("ADV")) > 0) || (Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.Config, (Object) null) || Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.Dialog, (Object) null) || (Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.ExitScene, (Object) null) || Object.op_Inequality((Object) Singleton<Manager.Game>.Instance.MapShortcutUI, (Object) null))) || (this.Fading || !this.captionSystem.Visible);
    }

    protected virtual void Awake()
    {
      this._regulate = new Regulate(this);
      if (Object.op_Inequality((Object) this.backCamera, (Object) null))
        this.backCameraBackup = new BackupPosRot(((Component) this.backCamera).get_transform());
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerClickAsObservable((UIBehaviour) this.captionSystem.RaycasterImage), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (ped => ped != null)), (Action<M0>) (ped =>
      {
        bool flag1 = false;
        bool flag2 = false;
        switch ((int) ped.get_button())
        {
          case 0:
            flag1 = true;
            break;
          case 1:
            flag2 = true;
            break;
        }
        if ((flag1 || flag2) && !this.captionSystem.Visible)
        {
          this.captionSystem.Visible = true;
        }
        else
        {
          if (!flag1)
            return;
          this._clicked = true;
        }
      }));
      if (!Object.op_Inequality((Object) this.advUI, (Object) null))
        return;
      if (Object.op_Inequality((Object) this.advUI.skip, (Object) null))
      {
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._isSkip, (Func<M0, bool>) (isOn => isOn != this.advUI.skip.get_isOn())), (Action<M0>) (isOn =>
        {
          this.advUI.useSE = false;
          this.advUI.skip.set_isOn(isOn);
          this.advUI.useSE = true;
        }));
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.advUI.skip), (Func<M0, bool>) (isOn => isOn != this.isSkip)), (Action<M0>) (isOn =>
        {
          this.isSkip = isOn;
          this.advUI.PlaySE(SoundPack.SystemSE.OK_S);
        }));
      }
      if (Object.op_Inequality((Object) this.advUI.auto, (Object) null))
      {
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._isAuto, (Func<M0, bool>) (isOn => isOn != this.advUI.auto.get_isOn())), (Action<M0>) (isOn =>
        {
          this.advUI.useSE = false;
          this.advUI.auto.set_isOn(isOn);
          this.advUI.useSE = true;
        }));
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.advUI.auto), (Func<M0, bool>) (isOn => isOn != this.isAuto)), (Action<M0>) (isOn =>
        {
          this.isAuto = isOn;
          this.advUI.PlaySE(SoundPack.SystemSE.OK_S);
        }));
      }
      if (Object.op_Inequality((Object) this.advUI.log, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.advUI.log), (Action<M0>) (_ =>
        {
          if (this.VisibleLog != null)
            this.VisibleLog();
          this.advUI.PlaySE(SoundPack.SystemSE.OK_S);
        }));
      if (!Object.op_Inequality((Object) this.advUI.close, (Object) null))
        return;
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.advUI.close), (Func<M0, bool>) (_ => this.captionSystem.Visible)), (Action<M0>) (_ =>
      {
        this.captionSystem.Visible = false;
        this.advUI.PlaySE(SoundPack.SystemSE.Cancel);
      }));
    }

    protected virtual void OnEnable()
    {
      this.Initialize();
    }

    protected virtual void OnDisable()
    {
      if (Manager.Scene.isGameEnd)
        return;
      this.Release();
      this.MemberInit();
    }

    protected virtual void Update()
    {
      this.UpdateBefore();
      if (this.UpdateRegulate())
        return;
      bool isNext = this.input.IsPressedSubmit() || this._clicked;
      bool isSkip = this.input.IsDown((KeyCode) 306) || this.input.IsDown((KeyCode) 305);
      this._clicked = false;
      this.MessageWindowProc(new TextScenario.NextInfo(this.captionSystem.IsCompleteDisplayText, isNext, isSkip));
    }

    private class AlreadyReadInfo
    {
      private const string Path = "save";
      private const string FileName = "read.dat";

      public AlreadyReadInfo()
      {
        this.read.Add(0);
        this.Load();
      }

      private HashSet<int> read { get; } = new HashSet<int>();

      public bool Add(int i)
      {
        return this.read.Add(i);
      }

      public void Save()
      {
        Illusion.Utils.File.OpenWrite(UserData.Create("save") + "read.dat", false, (Action<FileStream>) (f =>
        {
          Debug.Log((object) "既読データセーブ".Coloring("yellow"));
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) f))
          {
            binaryWriter.Write(this.read.Count);
            foreach (int num in this.read)
              binaryWriter.Write(num);
          }
        }));
      }

      public bool Load()
      {
        return Illusion.Utils.File.OpenRead(UserData.Path + "save" + (object) '/' + "read.dat", (Action<FileStream>) (f =>
        {
          Debug.Log((object) "既読データロード".Coloring("yellow"));
          using (BinaryReader binaryReader = new BinaryReader((Stream) f))
          {
            int num = binaryReader.ReadInt32();
            for (int index = 0; index < num; ++index)
              this.read.Add(binaryReader.ReadInt32());
          }
        }));
      }
    }

    public interface IVoice
    {
      int personality { get; }

      string bundle { get; }

      string asset { get; }

      void Convert2D();

      AudioSource Play();

      bool Wait();
    }

    public interface IChara
    {
      int no { get; }

      void Play(TextScenario scenario);

      CharaData GetChara(TextScenario scenario);
    }

    public interface IMotion : TextScenario.IChara
    {
    }

    public interface IExpression : TextScenario.IChara
    {
    }

    public interface IExpressionIcon : TextScenario.IChara
    {
    }

    public class CurrentCharaData
    {
      private List<TextScenario.IVoice[]> _voiceList;
      private List<TextScenario.IMotion[]> _motionList;
      private List<TextScenario.IExpression[]> _expressionList;
      private Dictionary<int, string> _bundleVoices;

      public CurrentCharaData()
      {
        this._bundleVoices = new Dictionary<int, string>();
      }

      public Dictionary<int, string> bundleVoices
      {
        get
        {
          return this._bundleVoices;
        }
      }

      public bool isSkip { get; set; }

      public List<TextScenario.IVoice[]> voiceList
      {
        get
        {
          return this._voiceList;
        }
      }

      public List<TextScenario.IMotion[]> motionList
      {
        get
        {
          return this._motionList;
        }
      }

      public List<TextScenario.IExpression[]> expressionList
      {
        get
        {
          return this._expressionList;
        }
      }

      public void CreateVoiceList()
      {
        if (this.voiceList != null)
          return;
        this._voiceList = new List<TextScenario.IVoice[]>();
      }

      public void CreateMotionList()
      {
        if (this.motionList != null)
          return;
        this._motionList = new List<TextScenario.IMotion[]>();
      }

      public void CreateExpressionList()
      {
        if (this.expressionList != null)
          return;
        this._expressionList = new List<TextScenario.IExpression[]>();
      }

      public void Clear()
      {
        this._voiceList = (List<TextScenario.IVoice[]>) null;
        this._motionList = (List<TextScenario.IMotion[]>) null;
        this._expressionList = (List<TextScenario.IExpression[]>) null;
      }
    }

    public class ParamData
    {
      public ParamData(IParams param)
      {
        this.param = param;
        int num = -1;
        switch (param)
        {
          case PlayerData _:
            num = 0;
            break;
          case AgentData _:
            num = 1;
            break;
          case MerchantData _:
            num = 2;
            break;
        }
        this.type = num;
        this.charaParam = param?.param as CharaParams;
        int? charaId = this.charaParam?.charaID;
        this.voiceNo = !charaId.HasValue ? 0 : charaId.Value;
        this.actor = this.charaParam?.actor;
        if (Object.op_Inequality((Object) this.actor, (Object) null))
        {
          this.chaCtrl = this.actor.ChaControl;
          this.transform = ((Component) this.actor.Animation.Character).get_transform();
        }
        if (Object.op_Inequality((Object) this.chaCtrl, (Object) null))
          this.voicePitch = this.chaCtrl.fileParam.voicePitch;
        switch (num)
        {
          case 0:
            this.playerData = param as PlayerData;
            this.characterInfo = (ICharacterInfo) this.playerData;
            this.playerActor = this.actor as PlayerActor;
            this.sex = (int) this.playerData.Sex;
            break;
          case 1:
            this.agentData = param as AgentData;
            this.characterInfo = (ICharacterInfo) this.agentData;
            this.agentActor = this.actor as AgentActor;
            break;
          case 2:
            this.merchantData = param as MerchantData;
            this.characterInfo = (ICharacterInfo) this.merchantData;
            this.merchantActor = this.actor as MerchantActor;
            break;
        }
      }

      public bool isPlayer
      {
        get
        {
          return this.type == 0;
        }
      }

      public bool isHeroine
      {
        get
        {
          return MathfEx.IsRange<int>(1, this.type, 2, true);
        }
      }

      public int type { get; }

      public IParams param { get; }

      public CharaParams charaParam { get; }

      public Actor actor { get; }

      public PlayerActor playerActor { get; }

      public AgentActor agentActor { get; }

      public MerchantActor merchantActor { get; }

      public ICharacterInfo characterInfo { get; }

      public PlayerData playerData { get; }

      public AgentData agentData { get; }

      public MerchantData merchantData { get; }

      public bool isVisible
      {
        get
        {
          if (Object.op_Inequality((Object) this.actor, (Object) null) && this.type > 0)
            return this.actor.IsVisible;
          return Object.op_Inequality((Object) this.chaCtrl, (Object) null) && this.chaCtrl.visibleAll;
        }
        set
        {
          if (Object.op_Inequality((Object) this.actor, (Object) null) && this.type > 0)
          {
            this.actor.IsVisible = value;
          }
          else
          {
            if (!Object.op_Inequality((Object) this.chaCtrl, (Object) null))
              return;
            this.chaCtrl.visibleAll = value;
          }
        }
      }

      public int sex { get; } = 1;

      public int voiceNo { get; }

      public float voicePitch { get; } = 1f;

      public ChaControl chaCtrl { get; }

      public Transform transform { get; }

      public ChaControl Create()
      {
        if (Object.op_Equality((Object) this.actor, (Object) null))
          return (ChaControl) null;
        switch (this.type)
        {
          case 0:
            return this.sex == 0 ? Singleton<Character>.Instance.CreateChara((byte) 0, ((Component) this.actor).get_gameObject(), 0, (ChaFileControl) null) : Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this.actor).get_gameObject(), 0, (ChaFileControl) null);
          case 1:
            return Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this.actor).get_gameObject(), 0, (ChaFileControl) null);
          case 2:
            return Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this.actor).get_gameObject(), 0, (ChaFileControl) null);
          default:
            return (ChaControl) null;
        }
      }
    }

    public class LoopVoicePack
    {
      public LoopVoicePack(int voiceNo, ChaControl chaCtrl, AudioSource audio)
      {
        this.voiceNo = voiceNo;
        this.chaCtrl = chaCtrl;
        this.audio = audio;
      }

      public int voiceNo { get; private set; }

      public ChaControl chaCtrl { get; private set; }

      public AudioSource audio { get; private set; }

      public bool Set()
      {
        return !Object.op_Equality((Object) this.chaCtrl, (Object) null) && !Object.op_Equality((Object) this.audio, (Object) null);
      }
    }

    protected class NextInfo
    {
      public bool isCompleteDisplayText;
      public bool isNext;
      public bool isSkip;

      public NextInfo(bool isCompleteDisplayText, bool isNext, bool isSkip)
      {
        this.isCompleteDisplayText = isCompleteDisplayText;
        this.isNext = isNext;
        this.isSkip = isSkip;
      }
    }

    [Serializable]
    private sealed class FileOpen
    {
      [SerializeField]
      private List<RootData> fileList = new List<RootData>();
      [SerializeField]
      private List<RootData> rootList = new List<RootData>();

      public List<RootData> FileList
      {
        get
        {
          return this.fileList;
        }
      }

      public List<RootData> RootList
      {
        get
        {
          return this.rootList;
        }
      }

      public void Clear()
      {
        this.fileList.Clear();
        this.rootList.Clear();
      }
    }
  }
}
