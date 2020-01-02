// Decompiled with JetBrains decompiler
// Type: MorphEditorCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using MorphAssist;
using UnityEngine;

public class MorphEditorCtrl : MonoBehaviour
{
  private AudioAssist audioAssist;
  private bool[,] clickButton;
  private bool playVoice;
  private bool clickPlay;
  private Morphing morphing;
  private MorphCtrlEyebrow EyebrowCtrl;
  private MorphCtrlEyes EyesCtrl;
  private MorphCtrlMouth MouthCtrl;

  public MorphEditorCtrl()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.morphing = (Morphing) ((Component) this).get_transform().FindTop().get_transform().FindLoop("MorphCtrl").GetComponent("Morphing");
    this.EyebrowCtrl = this.morphing.EyebrowCtrl;
    this.EyesCtrl = this.morphing.EyesCtrl;
    this.MouthCtrl = this.morphing.MouthCtrl;
    this.audioAssist = new AudioAssist();
  }

  private void Start()
  {
  }

  private void Update()
  {
    float audioWaveValue = this.audioAssist.GetAudioWaveValue((AudioSource) ((Component) this).GetComponent<AudioSource>());
    if (this.clickButton[0, 0])
    {
      this.EyebrowCtrl.ChangePtn(Mathf.Max(0, this.EyebrowCtrl.NowPtn - 1), true);
      this.clickButton[0, 0] = false;
    }
    else if (this.clickButton[0, 1])
    {
      this.EyebrowCtrl.ChangePtn(Mathf.Min(this.EyebrowCtrl.GetMaxPtn() - 1, this.EyebrowCtrl.NowPtn + 1), true);
      this.clickButton[0, 1] = false;
    }
    if (this.clickButton[1, 0])
    {
      this.EyesCtrl.ChangePtn(Mathf.Max(0, this.EyesCtrl.NowPtn - 1), true);
      this.clickButton[1, 0] = false;
    }
    else if (this.clickButton[1, 1])
    {
      this.EyesCtrl.ChangePtn(Mathf.Min(this.EyesCtrl.GetMaxPtn() - 1, this.EyesCtrl.NowPtn + 1), true);
      this.clickButton[1, 1] = false;
    }
    if (this.clickButton[2, 0])
    {
      this.MouthCtrl.ChangePtn(Mathf.Max(0, this.MouthCtrl.NowPtn - 1), true);
      this.clickButton[2, 0] = false;
    }
    else if (this.clickButton[2, 1])
    {
      this.MouthCtrl.ChangePtn(Mathf.Min(this.MouthCtrl.GetMaxPtn() - 1, this.MouthCtrl.NowPtn + 1), true);
      this.clickButton[2, 1] = false;
    }
    this.morphing.SetVoiceVaule(audioWaveValue);
    if (!this.clickPlay)
      return;
    if (this.playVoice)
    {
      this.playVoice = false;
      ((AudioSource) ((Component) this).GetComponent<AudioSource>()).Stop();
    }
    else
    {
      this.playVoice = true;
      ((AudioSource) ((Component) this).GetComponent<AudioSource>()).Play();
    }
    this.clickPlay = false;
  }

  private void PushEyebrowBackPtn()
  {
    this.clickButton[0, 0] = true;
  }

  private void PushEyebrowNextPtn()
  {
    this.clickButton[0, 1] = true;
  }

  private void PushEyesBackPtn()
  {
    this.clickButton[1, 0] = true;
  }

  private void PushEyesNextPtn()
  {
    this.clickButton[1, 1] = true;
  }

  private void PushMouthBackPtn()
  {
    this.clickButton[2, 0] = true;
  }

  private void PushMouthNextPtn()
  {
    this.clickButton[2, 1] = true;
  }

  private void PushPlayVoice()
  {
    this.clickPlay = true;
  }
}
