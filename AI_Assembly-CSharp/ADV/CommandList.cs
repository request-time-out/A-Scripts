// Decompiled with JetBrains decompiler
// Type: ADV.CommandList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV.Commands.Base;
using ADV.Commands.Chara;
using ADV.Commands.Effect;
using ADV.Commands.EventCG;
using ADV.Commands.Game;
using ADV.Commands.H;
using ADV.Commands.MapScene;
using ADV.Commands.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public class CommandList : List<CommandBase>
  {
    public CommandList(TextScenario scenario)
    {
      this.scenario = scenario;
    }

    private TextScenario scenario { get; }

    public new void Add(CommandBase item)
    {
      Debug.LogError((object) "CommandList Add Error!");
    }

    public bool Add(ScenarioData.Param item, int currentLine)
    {
      CommandBase commandBase = CommandList.CommandGet(item.Command);
      if (commandBase == null)
      {
        Debug.LogError((object) ("CommandGet Failed:" + (object) item.Command));
        return true;
      }
      commandBase.Initialize(this.scenario, item.Command, item.Args);
      commandBase.ConvertBeforeArgsProc();
      for (int index = 0; index < commandBase.args.Length; ++index)
        commandBase.args[index] = this.scenario.ReplaceVars(commandBase.args[index]);
      commandBase.localLine = currentLine;
      commandBase.Do();
      base.Add(commandBase);
      return item.Multi;
    }

    public bool Process()
    {
      this.Where<CommandBase>((Func<CommandBase, bool>) (item => item.Process())).ToList<CommandBase>().ForEach((Action<CommandBase>) (item =>
      {
        this.Remove(item);
        item.Result(true);
      }));
      return this.Any<CommandBase>((Func<CommandBase, bool>) (p => CommandList.IsWait(p.command)));
    }

    public void ProcessEnd()
    {
      this.ForEach((Action<CommandBase>) (item => item.Result(false)));
      this.Clear();
    }

    private static bool IsWait(Command command)
    {
      switch (command)
      {
        case Command.Fade:
        case Command.FadeWait:
label_3:
          return true;
        default:
          switch (command - 196)
          {
            case Command.None:
            case Command.Calc:
              goto label_3;
            default:
              if (command != Command.Choice)
                return false;
              goto label_3;
          }
      }
    }

    public static CommandBase CommandGet(Command command)
    {
      switch (command)
      {
        case Command.VAR:
          return (CommandBase) new VAR();
        case Command.RandomVar:
          return (CommandBase) new RandomVar();
        case Command.Calc:
          return (CommandBase) new Calc();
        case Command.Clamp:
          return (CommandBase) new Clamp();
        case Command.Min:
          return (CommandBase) new Min();
        case Command.Max:
          return (CommandBase) new Max();
        case Command.Lerp:
          return (CommandBase) new Lerp();
        case Command.LerpAngle:
          return (CommandBase) new LerpAngle();
        case Command.InverseLerp:
          return (CommandBase) new InverseLerp();
        case Command.LerpV3:
          return (CommandBase) new LerpV3();
        case Command.LerpAngleV3:
          return (CommandBase) new LerpAngleV3();
        case Command.Tag:
          return (CommandBase) new Tag();
        case Command.Format:
          return (CommandBase) new Format();
        case Command.IF:
          return (CommandBase) new IF();
        case Command.Switch:
          return (CommandBase) new Switch();
        case Command.Text:
          return (CommandBase) new Text();
        case Command.Voice:
          return (CommandBase) new Voice();
        case Command.Motion:
          return (CommandBase) new ADV.Commands.Base.Motion();
        case Command.Expression:
          return (CommandBase) new ADV.Commands.Base.Expression();
        case Command.Open:
          return (CommandBase) new Open();
        case Command.Close:
          return (CommandBase) new Close();
        case Command.Jump:
          return (CommandBase) new Jump();
        case Command.Choice:
          return (CommandBase) new Choice();
        case Command.Wait:
          return (CommandBase) new Wait();
        case Command.TextClear:
          return (CommandBase) new Clear();
        case Command.FontColor:
          return (CommandBase) new ADV.Commands.Base.FontColor();
        case Command.Scene:
          return (CommandBase) new Scene();
        case Command.Regulate:
          return (CommandBase) new ADV.Commands.Base.Regulate();
        case Command.Replace:
          return (CommandBase) new Replace();
        case Command.Reset:
          return (CommandBase) new Reset();
        case Command.Vector:
          return (CommandBase) new Vector();
        case Command.NullLoad:
          return (CommandBase) new NullLoad();
        case Command.NullRelease:
          return (CommandBase) new NullRelease();
        case Command.NullSet:
          return (CommandBase) new NullSet();
        case Command.InfoAudioEco:
          return (CommandBase) new InfoAudioEco();
        case Command.InfoAnimePlay:
          return (CommandBase) new InfoAnimePlay();
        case Command.Fade:
          return (CommandBase) new Fade();
        case Command.SceneFade:
          return (CommandBase) new SceneFade();
        case Command.SceneFadeRegulate:
          return (CommandBase) new SceneFadeRegulate();
        case Command.FadeWait:
          return (CommandBase) new FadeWait();
        case Command.FilterImageLoad:
          return (CommandBase) new FilterImageLoad();
        case Command.FilterImageRelease:
          return (CommandBase) new FilterImageRelease();
        case Command.FilterSet:
          return (CommandBase) new FilterSet();
        case Command.Filter:
          return (CommandBase) new ADV.Commands.Effect.Filter();
        case Command.BGMPlay:
          return (CommandBase) new ADV.Commands.Sound.BGM.Play();
        case Command.BGMStop:
          return (CommandBase) new ADV.Commands.Sound.BGM.Stop();
        case Command.EnvPlay:
          return (CommandBase) new ADV.Commands.Sound.ENV.Play();
        case Command.EnvStop:
          return (CommandBase) new ADV.Commands.Sound.ENV.Stop();
        case Command.SE2DPlay:
          return (CommandBase) new ADV.Commands.Sound.SE2D.Play();
        case Command.SE2DStop:
          return (CommandBase) new ADV.Commands.Sound.SE2D.Stop();
        case Command.SE3DPlay:
          return (CommandBase) new ADV.Commands.Sound.SE3D.Play();
        case Command.SE3DStop:
          return (CommandBase) new ADV.Commands.Sound.SE3D.Stop();
        case Command.CharaStand:
          return (CommandBase) new StandPosition();
        case Command.CharaStandFind:
          return (CommandBase) new StandFindPosition();
        case Command.CharaPositionAdd:
          return (CommandBase) new AddPosition();
        case Command.CharaPositionSet:
          return (CommandBase) new SetPosition();
        case Command.CharaPositionLocalAdd:
          return (CommandBase) new AddPositionLocal();
        case Command.CharaPositionLocalSet:
          return (CommandBase) new SetPositionLocal();
        case Command.CharaMotion:
          return (CommandBase) new ADV.Commands.Chara.Motion();
        case Command.CharaMotionWait:
          return (CommandBase) new MotionWait();
        case Command.CharaMotionLayerWeight:
          return (CommandBase) new MotionLayerWeight();
        case Command.CharaMotionSetParam:
          return (CommandBase) new MotionSetParam();
        case Command.CharaMotionIKSetPartner:
          return (CommandBase) new MotionIKSetPartner();
        case Command.CharaExpression:
          return (CommandBase) new ADV.Commands.Chara.Expression();
        case Command.CharaFixEyes:
          return (CommandBase) new FixEyes();
        case Command.CharaFixMouth:
          return (CommandBase) new FixMouth();
        case Command.CharaGetShape:
          return (CommandBase) new GetShape();
        case Command.CharaClothState:
          return (CommandBase) new ClothState();
        case Command.CharaSiruState:
          return (CommandBase) new SiruState();
        case Command.CharaVoicePlay:
          return (CommandBase) new VoicePlay();
        case Command.CharaVoiceStop:
          return (CommandBase) new VoiceStop();
        case Command.CharaVoiceStopAll:
          return (CommandBase) new VoiceStopAll();
        case Command.CharaVoiceWait:
          return (CommandBase) new VoiceWait();
        case Command.CharaVoiceWaitAll:
          return (CommandBase) new VoiceWaitAll();
        case Command.CharaLookEyes:
          return (CommandBase) new LookEyes();
        case Command.CharaLookEyesTarget:
          return (CommandBase) new LookEyesTarget();
        case Command.CharaLookEyesTargetChara:
          return (CommandBase) new LookEyesTargetChara();
        case Command.CharaLookNeck:
          return (CommandBase) new LookNeck();
        case Command.CharaLookNeckTarget:
          return (CommandBase) new LookNeckTarget();
        case Command.CharaLookNeckTargetChara:
          return (CommandBase) new LookNeckTargetChara();
        case Command.CharaLookNeckSkip:
          return (CommandBase) new LookNeckSkip();
        case Command.CharaItemCreate:
          return (CommandBase) new ItemCreate();
        case Command.CharaItemDelete:
          return (CommandBase) new ItemDelete();
        case Command.CharaItemAnime:
          return (CommandBase) new ItemAnime();
        case Command.CharaItemFind:
          return (CommandBase) new ItemFind();
        case Command.EventCGSetting:
          return (CommandBase) new Setting();
        case Command.EventCGRelease:
          return (CommandBase) new Release();
        case Command.EventCGNext:
          return (CommandBase) new ADV.Commands.EventCG.Next();
        case Command.ObjectCreate:
          return (CommandBase) new Create();
        case Command.ObjectLoad:
          return (CommandBase) new Load();
        case Command.ObjectDelete:
          return (CommandBase) new Delete();
        case Command.ObjectPosition:
          return (CommandBase) new Position();
        case Command.ObjectRotation:
          return (CommandBase) new Rotation();
        case Command.ObjectScale:
          return (CommandBase) new Scale();
        case Command.ObjectParent:
          return (CommandBase) new Parent();
        case Command.ObjectComponent:
          return (CommandBase) new Component();
        case Command.ObjectAnimeParam:
          return (CommandBase) new AnimeParam();
        case Command.CharaActive:
          return (CommandBase) new CharaActive();
        case Command.CharaVisible:
          return (CommandBase) new CharaVisible();
        case Command.CharaColor:
          return (CommandBase) new CharaColor();
        case Command.CameraLookAt:
          return (CommandBase) new CameraLookAt();
        case Command.MozVisible:
          return (CommandBase) new MozVisible();
        case Command.AddCollider:
          return (CommandBase) new AddCollider();
        case Command.ColliderSetActive:
          return (CommandBase) new ColliderSetActive();
        case Command.AddNavMeshAgent:
          return (CommandBase) new AddNavMeshAgent();
        case Command.NavMeshAgentSetActive:
          return (CommandBase) new NavMeshAgentSetActive();
        case Command.BundleCheck:
          return (CommandBase) new BundleCheck();
        case Command.Prob:
          return (CommandBase) new Prob();
        case Command.Probs:
          return (CommandBase) new Probs();
        case Command.FormatVAR:
          return (CommandBase) new FormatVAR();
        case Command.Task:
          return (CommandBase) new Task();
        case Command.TaskWait:
          return (CommandBase) new TaskWait();
        case Command.TaskEnd:
          return (CommandBase) new TaskEnd();
        case Command.Log:
          return (CommandBase) new Log();
        case Command.CharaSetShape:
          return (CommandBase) new SetShape();
        case Command.CharaCoordinateChange:
          return (CommandBase) new CoordinateChange();
        case Command.InfoAudio:
          return (CommandBase) new InfoAudio();
        case Command.ReplaceLanguage:
          return (CommandBase) new ReplaceLanguage();
        case Command.InfoText:
          return (CommandBase) new InfoText();
        case Command.SendCommandData:
          return (CommandBase) new SendCommandData();
        case Command.SendCommandDataList:
          return (CommandBase) new SendCommandDataList();
        case Command.PlaySystemSE:
          return (CommandBase) new PlaySystemSE();
        case Command.PlayActionSE:
          return (CommandBase) new PlayActionSE();
        case Command.PlayEnviroSE:
          return (CommandBase) new PlayEnviroSE();
        case Command.PlayFootStepSE:
          return (CommandBase) new PlayFootStepSE();
        case Command.InventoryCheck:
          return (CommandBase) new InventoryCheck();
        case Command.SetPresent:
          return (CommandBase) new SetPresent();
        case Command.SetPresentBirthday:
          return (CommandBase) new SetPresentBirthday();
        case Command.ClearItems:
          return (CommandBase) new ClearItems();
        case Command.AddItem:
          return (CommandBase) new AddItem();
        case Command.RemoveItem:
          return (CommandBase) new RemoveItem();
        case Command.ChangeADVFixedAngleCamera:
          return (CommandBase) new ChangeADVFixedAngleCamera();
        case Command.InventoryGiveItem:
          return (CommandBase) new InventoryGiveItem();
        case Command.SetItemScrounge:
          return (CommandBase) new SetItemScrounge();
        case Command.CharaSetting:
          return (CommandBase) new CharaSetting();
        case Command.AddItemInPlayer:
          return (CommandBase) new AddItemInPlayer();
        default:
          return (CommandBase) null;
      }
    }
  }
}
