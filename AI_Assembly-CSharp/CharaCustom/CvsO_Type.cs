// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_Type
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsO_Type : CvsBase
  {
    [SerializeField]
    private GameObject objTop;
    [SerializeField]
    private GameObject objTemp;
    [SerializeField]
    private CustomSliderSet ssVoiceRate;
    private Toggle[] tglType;
    private AudioSource audioSource;
    private int[] voiceCnt;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssVoiceRate.SetSliderValue(this.parameter.voiceRate);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      int index1 = Array.IndexOf<int>(this.customBase.dictPersonality.Keys.ToArray<int>(), this.parameter.personality);
      this.tglType[index1].SetIsOnWithoutCallback(true);
      for (int index2 = 0; index2 < this.tglType.Length; ++index2)
      {
        if (index2 != index1)
          this.tglType[index2].SetIsOnWithoutCallback(false);
      }
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsO_Type.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void PlayVoice()
    {
      if (!this.customBase.playVoiceBackup.playSampleVoice)
      {
        this.customBase.playVoiceBackup.backEyebrowPtn = this.chaCtrl.fileStatus.eyebrowPtn;
        this.customBase.playVoiceBackup.backEyesPtn = this.chaCtrl.fileStatus.eyesPtn;
        this.customBase.playVoiceBackup.backBlink = this.chaCtrl.fileStatus.eyesBlink;
        this.customBase.playVoiceBackup.backEyesOpen = this.chaCtrl.fileStatus.eyesOpenMax;
        this.customBase.playVoiceBackup.backMouthPtn = this.chaCtrl.fileStatus.mouthPtn;
        this.customBase.playVoiceBackup.backMouthFix = this.chaCtrl.fileStatus.mouthFixed;
        this.customBase.playVoiceBackup.backMouthOpen = this.chaCtrl.fileStatus.mouthOpenMax;
      }
      ListInfoBase listInfo = Singleton<Character>.Instance.chaListCtrl.GetListInfo(ChaListDefine.CategoryNo.cha_sample_voice, this.parameter.personality);
      if (listInfo == null)
        return;
      ChaListDefine.KeyType[] keyTypeArray1 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.Eyebrow01,
        ChaListDefine.KeyType.Eyebrow02,
        ChaListDefine.KeyType.Eyebrow03
      };
      ChaListDefine.KeyType[] keyTypeArray2 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.Eye01,
        ChaListDefine.KeyType.Eye02,
        ChaListDefine.KeyType.Eye03
      };
      ChaListDefine.KeyType[] keyTypeArray3 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.EyeMax01,
        ChaListDefine.KeyType.EyeMax02,
        ChaListDefine.KeyType.EyeMax03
      };
      ChaListDefine.KeyType[] keyTypeArray4 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.Mouth01,
        ChaListDefine.KeyType.Mouth02,
        ChaListDefine.KeyType.Mouth03
      };
      ChaListDefine.KeyType[] keyTypeArray5 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.MouthMax01,
        ChaListDefine.KeyType.MouthMax02,
        ChaListDefine.KeyType.MouthMax03
      };
      ChaListDefine.KeyType[] keyTypeArray6 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.EyeHiLight01,
        ChaListDefine.KeyType.EyeHiLight02,
        ChaListDefine.KeyType.EyeHiLight03
      };
      ChaListDefine.KeyType[] keyTypeArray7 = new ChaListDefine.KeyType[3]
      {
        ChaListDefine.KeyType.Data01,
        ChaListDefine.KeyType.Data02,
        ChaListDefine.KeyType.Data03
      };
      int index = this.voiceCnt[this.parameter.personality] = (this.voiceCnt[this.parameter.personality] + 1) % keyTypeArray1.Length;
      this.chaCtrl.ChangeEyebrowPtn(listInfo.GetInfoInt(keyTypeArray1[index]), true);
      this.chaCtrl.ChangeEyesPtn(listInfo.GetInfoInt(keyTypeArray2[index]), true);
      this.chaCtrl.HideEyeHighlight("0" == listInfo.GetInfo(keyTypeArray6[index]));
      this.chaCtrl.ChangeEyesBlinkFlag(false);
      this.chaCtrl.ChangeEyesOpenMax(listInfo.GetInfoFloat(keyTypeArray3[index]));
      this.chaCtrl.ChangeMouthPtn(listInfo.GetInfoInt(keyTypeArray4[index]), true);
      this.chaCtrl.ChangeMouthFixed(false);
      this.chaCtrl.ChangeMouthOpenMax(listInfo.GetInfoFloat(keyTypeArray5[index]));
      this.customBase.playVoiceBackup.playSampleVoice = true;
      Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.SystemSE);
      Transform trfVoice = Illusion.Game.Utils.Sound.Play(new Illusion.Game.Utils.Sound.Setting()
      {
        type = Manager.Sound.Type.SystemSE,
        assetBundleName = listInfo.GetInfo(ChaListDefine.KeyType.MainAB),
        assetName = listInfo.GetInfo(keyTypeArray7[index])
      });
      this.audioSource = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
      this.audioSource.set_pitch(this.parameter.voicePitch);
      this.chaCtrl.SetVoiceTransform(trfVoice);
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsType += new Action(((CvsBase) this).UpdateCustomUI);
      this.voiceCnt = new int[this.customBase.dictPersonality.Count];
      this.tglType = new Toggle[this.customBase.dictPersonality.Keys.Count];
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType15<KeyValuePair<int, string>, int> anonType15 in this.customBase.dictPersonality.Select<KeyValuePair<int, string>, \u003C\u003E__AnonType15<KeyValuePair<int, string>, int>>((Func<KeyValuePair<int, string>, int, \u003C\u003E__AnonType15<KeyValuePair<int, string>, int>>) ((val, idx) => new \u003C\u003E__AnonType15<KeyValuePair<int, string>, int>(val, idx))))
      {
        GameObject self = (GameObject) Object.Instantiate<GameObject>((M0) this.objTemp);
        ((Object) self).set_name("tglRbSel_" + anonType15.idx.ToString("00"));
        this.tglType[anonType15.idx] = (Toggle) self.GetComponent<Toggle>();
        ToggleGroup component1 = (ToggleGroup) this.objTop.GetComponent<ToggleGroup>();
        this.tglType[anonType15.idx].set_group(component1);
        self.get_transform().SetParent(this.objTop.get_transform(), false);
        Transform transform = self.get_transform().Find("textRbSelect");
        if (Object.op_Inequality((Object) null, (Object) transform))
        {
          Text component2 = (Text) ((Component) transform).GetComponent<Text>();
          if (Object.op_Implicit((Object) component2))
            component2.set_text(anonType15.val.Value);
        }
        self.SetActiveIfDifferent(true);
      }
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Toggle>) this.tglType).Select<Toggle, \u003C\u003E__AnonType12<Toggle, byte>>((Func<Toggle, int, \u003C\u003E__AnonType12<Toggle, byte>>) ((p, idx) => new \u003C\u003E__AnonType12<Toggle, byte>(p, (byte) idx))).ToList<\u003C\u003E__AnonType12<Toggle, byte>>().ForEach((Action<\u003C\u003E__AnonType12<Toggle, byte>>) (p => ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) p.toggle.onValueChanged), (Action<M0>) (isOn =>
      {
        if (this.customBase.updateCustomUI || !isOn)
          return;
        this.parameter.personality = this.customBase.dictPersonality.Keys.ToArray<int>()[(int) p.index];
        this.PlayVoice();
      }))));
      this.ssVoiceRate.onChange = (Action<float>) (value =>
      {
        this.parameter.voiceRate = value;
        if (!Singleton<Manager.Sound>.Instance.IsPlay(Manager.Sound.Type.SystemSE, (string) null))
          return;
        this.audioSource.set_pitch(this.parameter.voicePitch);
      });
      this.ssVoiceRate.onPointerUp = (Action) (() => this.PlayVoice());
      this.ssVoiceRate.onSetDefaultValue = (Func<float>) (() => 0.5f);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
