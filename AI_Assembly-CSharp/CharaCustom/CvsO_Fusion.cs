// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_Fusion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using MessagePack;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsO_Fusion : CvsBase
  {
    [SerializeField]
    private CustomCharaWindow charaLoadWinA;
    [SerializeField]
    private CustomCharaWindow charaLoadWinB;
    [SerializeField]
    private Button btnFusion;
    [SerializeField]
    private Button btnExit;
    private bool isFusion;

    public void UpdateCharasList()
    {
      List<CustomCharaFileInfo> charaFileInfoList = CustomCharaFileInfoAssist.CreateCharaFileInfoList((byte) 0 == this.chaCtrl.sex, (byte) 1 == this.chaCtrl.sex, true, true, false, false);
      this.charaLoadWinA.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, false, charaFileInfoList);
      this.charaLoadWinB.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, false, charaFileInfoList);
    }

    public int RandomIntWhich(int a, int b)
    {
      return Random.Range(0, 2) == 0 ? a : b;
    }

    public Color ColorBlend(Color a, Color b, float rate)
    {
      return new Color(Mathf.Lerp((float) a.r, (float) b.r, rate), Mathf.Lerp((float) a.g, (float) b.g, rate), Mathf.Lerp((float) a.b, (float) b.b, rate), Mathf.Lerp((float) a.a, (float) b.a, rate));
    }

    public void FusionProc(string pathA, string pathB)
    {
      ChaFileControl chaFileControl1 = new ChaFileControl();
      chaFileControl1.LoadCharaFile(pathA, this.customBase.modeSex, true, true);
      ChaFileControl chaFileControl2 = new ChaFileControl();
      chaFileControl2.LoadCharaFile(pathB, this.customBase.modeSex, true, true);
      ChaFileFace face1 = chaFileControl1.custom.face;
      ChaFileFace face2 = chaFileControl2.custom.face;
      float num1 = 0.5f + Random.Range(-0.5f, 0.5f);
      float rate1 = 0.5f + Random.Range(-0.5f, 0.5f);
      float num2 = 0.5f + Random.Range(-0.2f, 0.2f);
      for (int index = 0; index < this.face.shapeValueFace.Length; ++index)
        this.face.shapeValueFace[index] = Mathf.Lerp(face1.shapeValueFace[index], face2.shapeValueFace[index], num2);
      this.face.headId = this.RandomIntWhich(face1.headId, face2.headId);
      this.face.skinId = this.RandomIntWhich(face1.skinId, face2.skinId);
      this.face.detailId = this.RandomIntWhich(face1.detailId, face2.detailId);
      this.face.detailPower = Mathf.Lerp(face1.detailPower, face2.detailPower, num1);
      this.face.eyebrowId = this.RandomIntWhich(face1.eyebrowId, face2.eyebrowId);
      this.face.eyebrowColor = this.ColorBlend(face1.eyebrowColor, face2.eyebrowColor, rate1);
      this.face.eyebrowLayout = Random.Range(0, 2) != 0 ? face2.eyebrowLayout : face1.eyebrowLayout;
      float num3 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.face.eyebrowTilt = Mathf.Lerp(face1.eyebrowTilt, face2.eyebrowTilt, num3);
      bool flag = Random.Range(0, 2) != 0 ? face2.pupilSameSetting : face1.pupilSameSetting;
      float rate2 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].whiteColor = this.face.pupil[0].whiteColor;
        else
          this.face.pupil[index].whiteColor = this.ColorBlend(face1.pupil[index].whiteColor, face2.pupil[index].whiteColor, rate2);
      }
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].pupilId = this.face.pupil[0].pupilId;
        else
          this.face.pupil[index].pupilId = this.RandomIntWhich(face1.pupil[index].pupilId, face2.pupil[index].pupilId);
      }
      float rate3 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].pupilColor = this.face.pupil[0].pupilColor;
        else
          this.face.pupil[index].pupilColor = this.ColorBlend(face1.pupil[index].pupilColor, face2.pupil[index].pupilColor, rate3);
      }
      float num4 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].pupilW = this.face.pupil[0].pupilW;
        else
          this.face.pupil[index].pupilW = Mathf.Lerp(face1.pupil[index].pupilW, face2.pupil[index].pupilW, num4);
      }
      float num5 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].pupilH = this.face.pupil[0].pupilH;
        else
          this.face.pupil[index].pupilH = Mathf.Lerp(face1.pupil[index].pupilH, face2.pupil[index].pupilH, num5);
      }
      float num6 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].pupilEmission = this.face.pupil[0].pupilEmission;
        else
          this.face.pupil[index].pupilEmission = Mathf.Lerp(face1.pupil[index].pupilEmission, face2.pupil[index].pupilEmission, num6);
      }
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].blackId = this.face.pupil[0].blackId;
        else
          this.face.pupil[index].blackId = this.RandomIntWhich(face1.pupil[index].blackId, face2.pupil[index].blackId);
      }
      float rate4 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].blackColor = this.face.pupil[0].blackColor;
        else
          this.face.pupil[index].blackColor = this.ColorBlend(face1.pupil[index].blackColor, face2.pupil[index].blackColor, rate4);
      }
      float num7 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].blackW = this.face.pupil[0].blackW;
        else
          this.face.pupil[index].blackW = Mathf.Lerp(face1.pupil[index].blackW, face2.pupil[index].blackW, num7);
      }
      float num8 = 0.5f + Random.Range(-0.5f, 0.5f);
      for (int index = 0; index < 2; ++index)
      {
        if (flag && index == 1)
          this.face.pupil[index].blackH = this.face.pupil[0].blackH;
        else
          this.face.pupil[index].blackH = Mathf.Lerp(face1.pupil[index].blackH, face2.pupil[index].blackH, num8);
      }
      float num9 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.face.pupilY = Mathf.Lerp(face1.pupilY, face2.pupilY, num9);
      this.face.hlId = this.RandomIntWhich(face1.hlId, face2.hlId);
      float rate5 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.face.hlColor = this.ColorBlend(face1.hlColor, face2.hlColor, rate5);
      this.face.hlLayout = Random.Range(0, 2) != 0 ? face2.hlLayout : face1.hlLayout;
      float num10 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.face.hlTilt = Mathf.Lerp(face1.hlTilt, face2.hlTilt, num10);
      float num11 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.face.whiteShadowScale = Mathf.Lerp(face1.whiteShadowScale, face2.whiteShadowScale, num11);
      this.face.eyelashesId = this.RandomIntWhich(face1.eyelashesId, face2.eyelashesId);
      this.face.eyelashesColor = this.ColorBlend(face1.eyelashesColor, face2.eyelashesColor, rate1);
      if (Random.Range(0, 2) == 0)
      {
        this.face.moleId = face1.moleId;
        this.face.moleColor = face1.moleColor;
        this.face.moleLayout = face1.moleLayout;
      }
      else
      {
        this.face.moleId = face2.moleId;
        this.face.moleColor = face2.moleColor;
        this.face.moleLayout = face2.moleLayout;
      }
      if (Random.Range(0, 2) == 0)
      {
        this.face.makeup.eyeshadowId = face1.makeup.eyeshadowId;
        this.face.makeup.eyeshadowColor = face1.makeup.eyeshadowColor;
        this.face.makeup.eyeshadowGloss = face1.makeup.eyeshadowGloss;
      }
      else
      {
        this.face.makeup.eyeshadowId = face2.makeup.eyeshadowId;
        this.face.makeup.eyeshadowColor = face2.makeup.eyeshadowColor;
        this.face.makeup.eyeshadowGloss = face2.makeup.eyeshadowGloss;
      }
      if (Random.Range(0, 2) == 0)
      {
        this.face.makeup.cheekId = face1.makeup.cheekId;
        this.face.makeup.cheekColor = face1.makeup.cheekColor;
        this.face.makeup.cheekGloss = face1.makeup.cheekGloss;
      }
      else
      {
        this.face.makeup.cheekId = face2.makeup.cheekId;
        this.face.makeup.cheekColor = face2.makeup.cheekColor;
        this.face.makeup.cheekGloss = face2.makeup.cheekGloss;
      }
      if (Random.Range(0, 2) == 0)
      {
        this.face.makeup.lipId = face1.makeup.lipId;
        this.face.makeup.lipColor = face1.makeup.lipColor;
        this.face.makeup.lipGloss = face1.makeup.lipGloss;
      }
      else
      {
        this.face.makeup.lipId = face2.makeup.lipId;
        this.face.makeup.lipColor = face2.makeup.lipColor;
        this.face.makeup.lipGloss = face2.makeup.lipGloss;
      }
      if (Random.Range(0, 2) == 0)
        this.face.makeup.paintInfo[0].Copy(face1.makeup.paintInfo[0]);
      else
        this.face.makeup.paintInfo[0].Copy(face2.makeup.paintInfo[0]);
      if (Random.Range(0, 2) == 0)
        this.face.makeup.paintInfo[1].Copy(face1.makeup.paintInfo[1]);
      else
        this.face.makeup.paintInfo[1].Copy(face2.makeup.paintInfo[1]);
      if (this.chaCtrl.sex == (byte) 0)
      {
        if (Random.Range(0, 2) == 0)
        {
          this.face.beardId = face1.beardId;
          this.face.beardColor = face1.beardColor;
        }
        else
        {
          this.face.beardId = face2.beardId;
          this.face.beardColor = face2.beardColor;
        }
      }
      ChaFileBody body1 = chaFileControl1.custom.body;
      ChaFileBody body2 = chaFileControl2.custom.body;
      float num12 = 0.5f + Random.Range(-0.2f, 0.2f);
      for (int index = 0; index < this.body.shapeValueBody.Length; ++index)
        this.body.shapeValueBody[index] = Mathf.Lerp(body1.shapeValueBody[index], body2.shapeValueBody[index], num12);
      float num13 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.bustSoftness = Mathf.Lerp(body1.bustSoftness, body2.bustSoftness, num13);
      float num14 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.bustWeight = Mathf.Lerp(body1.bustWeight, body2.bustWeight, num14);
      this.body.skinId = this.RandomIntWhich(body1.skinId, body2.skinId);
      this.body.detailId = this.RandomIntWhich(body1.detailId, body2.detailId);
      this.body.detailPower = Mathf.Lerp(body1.detailPower, body2.detailPower, num1);
      float rate6 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.skinColor = this.ColorBlend(body1.skinColor, body2.skinColor, rate6);
      float num15 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.skinGlossPower = Mathf.Lerp(body1.skinGlossPower, body2.skinGlossPower, num15);
      float num16 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.skinMetallicPower = Mathf.Lerp(body1.skinMetallicPower, body2.skinMetallicPower, num16);
      this.body.sunburnId = this.RandomIntWhich(body1.sunburnId, body2.sunburnId);
      float rate7 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.sunburnColor = this.ColorBlend(body1.sunburnColor, body2.sunburnColor, rate7);
      if (Random.Range(0, 2) == 0)
        this.body.paintInfo[0].Copy(body1.paintInfo[0]);
      else
        this.body.paintInfo[0].Copy(body2.paintInfo[0]);
      if (Random.Range(0, 2) == 0)
        this.body.paintInfo[1].Copy(body1.paintInfo[1]);
      else
        this.body.paintInfo[1].Copy(body2.paintInfo[1]);
      this.body.nipId = this.RandomIntWhich(body1.nipId, body2.nipId);
      float rate8 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.nipColor = this.ColorBlend(body1.nipColor, body2.nipColor, rate8);
      float num17 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.nipGlossPower = Mathf.Lerp(body1.nipGlossPower, body2.nipGlossPower, num17);
      float num18 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.areolaSize = Mathf.Lerp(body1.areolaSize, body2.areolaSize, num18);
      this.body.underhairId = this.RandomIntWhich(body1.underhairId, body2.underhairId);
      this.body.underhairColor = this.ColorBlend(body1.underhairColor, body2.underhairColor, rate1);
      float rate9 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.nailColor = this.ColorBlend(body1.nailColor, body2.nailColor, rate9);
      float num19 = 0.5f + Random.Range(-0.5f, 0.5f);
      this.body.nailGlossPower = Mathf.Lerp(body1.nailGlossPower, body2.nailGlossPower, num19);
      ChaFileHair hair1 = chaFileControl1.custom.hair;
      ChaFileHair hair2 = chaFileControl2.custom.hair;
      if (Random.Range(0, 2) == 0)
        this.chaCtrl.chaFile.custom.hair = (ChaFileHair) MessagePackSerializer.Deserialize<ChaFileHair>(MessagePackSerializer.Serialize<ChaFileHair>((M0) hair1));
      else
        this.chaCtrl.chaFile.custom.hair = (ChaFileHair) MessagePackSerializer.Deserialize<ChaFileHair>(MessagePackSerializer.Serialize<ChaFileHair>((M0) hair2));
      for (int index = 0; index < this.hair.parts.Length; ++index)
      {
        this.hair.parts[index].baseColor = this.ColorBlend(hair1.parts[index].baseColor, hair2.parts[index].baseColor, rate1);
        this.hair.parts[index].topColor = this.ColorBlend(hair1.parts[index].topColor, hair2.parts[index].topColor, rate1);
        this.hair.parts[index].underColor = this.ColorBlend(hair1.parts[index].underColor, hair2.parts[index].underColor, rate1);
        this.hair.parts[index].specular = this.ColorBlend(hair1.parts[index].specular, hair2.parts[index].specular, rate1);
        this.hair.parts[index].smoothness = Mathf.Lerp(hair1.parts[index].smoothness, hair2.parts[index].smoothness, rate1);
        this.hair.parts[index].metallic = Mathf.Lerp(hair1.parts[index].metallic, hair2.parts[index].metallic, rate1);
      }
      if (Random.Range(0, 2) == 0)
        this.chaCtrl.chaFile.CopyCoordinate(chaFileControl1.coordinate);
      else
        this.chaCtrl.chaFile.CopyCoordinate(chaFileControl2.coordinate);
      this.chaCtrl.ChangeNowCoordinate(false, true);
      Singleton<Character>.Instance.customLoadGCClear = false;
      this.chaCtrl.Reload(false, false, false, false, true);
      Singleton<Character>.Instance.customLoadGCClear = true;
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFusion += new Action(((CvsBase) this).UpdateCustomUI);
      this.UpdateCharasList();
      if (Object.op_Inequality((Object) null, (Object) this.btnFusion))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnFusion), (Action<M0>) (_ =>
        {
          this.FusionProc(this.charaLoadWinA.GetSelectInfo().info.FileName, this.charaLoadWinB.GetSelectInfo().info.FileName);
          this.isFusion = true;
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnFusion), (Action<M0>) (_ =>
        {
          CustomCharaScrollController.ScrollData selectInfo1 = this.charaLoadWinA.GetSelectInfo();
          CustomCharaScrollController.ScrollData selectInfo2 = this.charaLoadWinB.GetSelectInfo();
          ((Selectable) this.btnFusion).set_interactable(selectInfo1 != null && null != selectInfo2);
        }));
      }
      if (!Object.op_Inequality((Object) null, (Object) this.btnExit))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnExit), (Action<M0>) (_ =>
      {
        this.customBase.customCtrl.showFusionCvs = false;
        this.customBase.customCtrl.showMainCvs = true;
        this.charaLoadWinA.SelectInfoClear();
        this.charaLoadWinB.SelectInfoClear();
        if (this.isFusion)
        {
          this.customBase.updateCustomUI = true;
          for (int slotNo = 0; slotNo < 20; ++slotNo)
            this.customBase.ChangeAcsSlotName(slotNo);
          this.customBase.SetUpdateToggleSetting();
          this.customBase.forceUpdateAcsList = true;
        }
        this.isFusion = false;
      }));
    }
  }
}
