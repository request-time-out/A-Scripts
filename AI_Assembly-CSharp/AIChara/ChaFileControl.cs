// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIChara
{
  public class ChaFileControl : ChaFile
  {
    public bool skipRangeCheck;

    public static bool CheckDataRangeFace(ChaFile chafile, List<string> checkInfo = null)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      ChaFileFace face = chafile.custom.face;
      byte sex = chafile.parameter.sex;
      bool flag = false;
      for (int index = 0; index < face.shapeValueFace.Length; ++index)
      {
        if (!MathfEx.RangeEqualOn<float>(0.0f, face.shapeValueFace[index], 1f))
        {
          checkInfo?.Add(string.Format("【{0}】値:{1}", (object) ChaFileDefine.cf_headshapename[index], (object) face.shapeValueFace[index]));
          flag = true;
          face.shapeValueFace[index] = Mathf.Clamp(face.shapeValueFace[index], 0.0f, 1f);
        }
      }
      if (face.shapeValueFace.Length > ChaFileDefine.cf_headshapename.Length)
      {
        float[] numArray = new float[ChaFileDefine.cf_headshapename.Length];
        Array.Copy((Array) face.shapeValueFace, (Array) numArray, numArray.Length);
        face.shapeValueFace = new float[ChaFileDefine.cf_headshapename.Length];
        Array.Copy((Array) numArray, (Array) face.shapeValueFace, numArray.Length);
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_head : ChaListDefine.CategoryNo.mo_head).ContainsKey(face.headId))
      {
        checkInfo?.Add(string.Format("【頭の種類】値:{0}", (object) face.headId));
        flag = true;
        face.headId = sex != (byte) 0 ? 0 : 0;
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_f : ChaListDefine.CategoryNo.mt_skin_f).ContainsKey(face.skinId))
      {
        checkInfo?.Add(string.Format("【肌の種類】値:{0}", (object) face.skinId));
        flag = true;
        face.skinId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_f : ChaListDefine.CategoryNo.mt_detail_f).ContainsKey(face.detailId))
      {
        checkInfo?.Add(string.Format("【彫りの種類】値:{0}", (object) face.detailId));
        flag = true;
        face.detailId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eyebrow).ContainsKey(face.eyebrowId))
      {
        checkInfo?.Add(string.Format("【眉毛の種類】値:{0}", (object) face.eyebrowId));
        flag = true;
        face.eyebrowId = 0;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eye).ContainsKey(face.pupil[index].pupilId))
        {
          checkInfo?.Add(string.Format("【瞳の種類】値:{0}", (object) face.pupil[index].pupilId));
          flag = true;
          face.pupil[index].pupilId = 0;
        }
        if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eyeblack).ContainsKey(face.pupil[index].blackId))
        {
          checkInfo?.Add(string.Format("【黒目の種類】値:{0}", (object) face.pupil[index].blackId));
          flag = true;
          face.pupil[index].blackId = 0;
        }
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eye_hl).ContainsKey(face.hlId))
      {
        checkInfo?.Add(string.Format("【ハイライト種類】値:{0}", (object) face.hlId));
        flag = true;
        face.hlId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eyelash).ContainsKey(face.eyelashesId))
      {
        checkInfo?.Add(string.Format("【睫毛の種類】値:{0}", (object) face.eyelashesId));
        flag = true;
        face.eyelashesId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_mole).ContainsKey(face.moleId))
      {
        checkInfo?.Add(string.Format("【ホクロの種類】値:{0}", (object) face.moleId));
        flag = true;
        face.moleId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_eyeshadow).ContainsKey(face.makeup.eyeshadowId))
      {
        checkInfo?.Add(string.Format("【アイシャドウ種類】値:{0}", (object) face.makeup.eyeshadowId));
        flag = true;
        face.makeup.eyeshadowId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_cheek).ContainsKey(face.makeup.cheekId))
      {
        checkInfo?.Add(string.Format("【チークの種類】値:{0}", (object) face.makeup.cheekId));
        flag = true;
        face.makeup.cheekId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_lip).ContainsKey(face.makeup.lipId))
      {
        checkInfo?.Add(string.Format("【リップの種類】値:{0}", (object) face.makeup.lipId));
        flag = true;
        face.makeup.lipId = 0;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_paint).ContainsKey(face.makeup.paintInfo[index].id))
        {
          checkInfo?.Add(string.Format("【ペイント種類】値:{0}", (object) face.makeup.paintInfo[index].id));
          flag = true;
          face.makeup.paintInfo[index].id = 0;
        }
      }
      if (sex == (byte) 0 && !chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.mt_beard).ContainsKey(face.beardId))
      {
        checkInfo?.Add(string.Format("【ヒゲの種類】値:{0}", (object) face.beardId));
        flag = true;
        face.beardId = 0;
      }
      return flag;
    }

    public static bool CheckDataRangeBody(ChaFile chafile, List<string> checkInfo = null)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      ChaFileBody body = chafile.custom.body;
      byte sex = chafile.parameter.sex;
      bool flag = false;
      for (int index = 0; index < body.shapeValueBody.Length; ++index)
      {
        if (!MathfEx.RangeEqualOn<float>(0.0f, body.shapeValueBody[index], 1f))
        {
          checkInfo?.Add(string.Format("【{0}】値:{1}", (object) ChaFileDefine.cf_bodyshapename[index], (object) body.shapeValueBody[index]));
          flag = true;
          body.shapeValueBody[index] = Mathf.Clamp(body.shapeValueBody[index], 0.0f, 1f);
        }
      }
      if (!MathfEx.RangeEqualOn<float>(0.0f, body.bustSoftness, 1f))
      {
        checkInfo?.Add(string.Format("【胸の柔らかさ】値:{0}", (object) body.bustSoftness));
        flag = true;
        body.bustSoftness = Mathf.Clamp(body.bustSoftness, 0.0f, 1f);
      }
      if (!MathfEx.RangeEqualOn<float>(0.0f, body.bustWeight, 1f))
      {
        checkInfo?.Add(string.Format("【胸の重さ】値:{0}", (object) body.bustWeight));
        flag = true;
        body.bustWeight = Mathf.Clamp(body.bustWeight, 0.0f, 1f);
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_b : ChaListDefine.CategoryNo.mt_skin_b).ContainsKey(body.skinId))
      {
        checkInfo?.Add(string.Format("【肌の種類】値:{0}", (object) body.skinId));
        flag = true;
        body.skinId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_b : ChaListDefine.CategoryNo.mt_detail_b).ContainsKey(body.detailId))
      {
        checkInfo?.Add(string.Format("【筋肉の種類】値:{0}", (object) body.detailId));
        flag = true;
        body.detailId = 0;
      }
      if (!chaListCtrl.GetCategoryInfo(sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_sunburn : ChaListDefine.CategoryNo.mt_sunburn).ContainsKey(body.sunburnId))
      {
        checkInfo?.Add(string.Format("【日焼けの種類】値:{0}", (object) body.sunburnId));
        flag = true;
        body.sunburnId = 0;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_paint).ContainsKey(body.paintInfo[index].id))
        {
          checkInfo?.Add(string.Format("【ペイントの種類】値:{0}", (object) body.paintInfo[index].id));
          flag = true;
          body.paintInfo[index].id = 0;
        }
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_nip).ContainsKey(body.nipId))
      {
        checkInfo?.Add(string.Format("【乳首の種類】値:{0}", (object) body.nipId));
        flag = true;
        body.nipId = 0;
      }
      if (!MathfEx.RangeEqualOn<float>(0.0f, body.areolaSize, 1f))
      {
        checkInfo?.Add(string.Format("【乳輪のサイズ】値:{0}", (object) body.areolaSize));
        flag = true;
        body.areolaSize = Mathf.Clamp(body.areolaSize, 0.0f, 1f);
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_underhair).ContainsKey(body.underhairId))
      {
        checkInfo?.Add(string.Format("【アンダーヘア種類】値:{0}", (object) body.underhairId));
        flag = true;
        body.underhairId = 0;
      }
      return flag;
    }

    public static bool CheckDataRangeHair(ChaFile chafile, List<string> checkInfo = null)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      ChaFileHair hair = chafile.custom.hair;
      byte sex = chafile.parameter.sex;
      bool flag = false;
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.so_hair_b).ContainsKey(hair.parts[0].id))
      {
        checkInfo?.Add(string.Format("【後ろ髪】値:{0}", (object) hair.parts[0].id));
        flag = true;
        hair.parts[0].id = sex != (byte) 0 ? 0 : 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.so_hair_f).ContainsKey(hair.parts[1].id))
      {
        checkInfo?.Add(string.Format("【前髪】値:{0}", (object) hair.parts[1].id));
        flag = true;
        hair.parts[1].id = sex != (byte) 0 ? 1 : 2;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.so_hair_s).ContainsKey(hair.parts[2].id))
      {
        checkInfo?.Add(string.Format("【横髪】値:{0}", (object) hair.parts[2].id));
        flag = true;
        hair.parts[2].id = sex != (byte) 0 ? 0 : 0;
      }
      if (!chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.so_hair_o).ContainsKey(hair.parts[3].id))
      {
        checkInfo?.Add(string.Format("【エクステ】値:{0}", (object) hair.parts[3].id));
        flag = true;
        hair.parts[3].id = sex != (byte) 0 ? 0 : 0;
      }
      return flag;
    }

    public static bool CheckDataRangeCoordinate(ChaFile chafile, List<string> checkInfo = null)
    {
      return ChaFileControl.CheckDataRangeCoordinate(chafile.coordinate, (int) chafile.parameter.sex, checkInfo);
    }

    public static bool CheckDataRangeCoordinate(
      ChaFileCoordinate coorde,
      int sex,
      List<string> checkInfo = null)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      bool flag = false;
      string[] strArray = new string[8]
      {
        "トップス",
        "ボトムス",
        "インナー上",
        "インナー下",
        "手袋",
        "パンスト",
        "靴下",
        "靴"
      };
      ChaListDefine.CategoryNo[] categoryNoArray = new ChaListDefine.CategoryNo[8]
      {
        sex != 0 ? ChaListDefine.CategoryNo.fo_top : ChaListDefine.CategoryNo.mo_top,
        sex != 0 ? ChaListDefine.CategoryNo.fo_bot : ChaListDefine.CategoryNo.mo_bot,
        sex != 0 ? ChaListDefine.CategoryNo.fo_inner_t : ChaListDefine.CategoryNo.unknown,
        sex != 0 ? ChaListDefine.CategoryNo.fo_inner_b : ChaListDefine.CategoryNo.unknown,
        sex != 0 ? ChaListDefine.CategoryNo.fo_gloves : ChaListDefine.CategoryNo.mo_gloves,
        sex != 0 ? ChaListDefine.CategoryNo.fo_panst : ChaListDefine.CategoryNo.unknown,
        sex != 0 ? ChaListDefine.CategoryNo.fo_socks : ChaListDefine.CategoryNo.unknown,
        sex != 0 ? ChaListDefine.CategoryNo.fo_shoes : ChaListDefine.CategoryNo.mo_shoes
      };
      int[] numArray1 = new int[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
      int[] numArray2 = new int[8]
      {
        sex != 0 ? 0 : 0,
        sex != 0 ? 0 : 0,
        sex != 0 ? 0 : -1,
        sex != 0 ? 0 : -1,
        sex != 0 ? 0 : 0,
        sex != 0 ? 0 : -1,
        sex != 0 ? 0 : -1,
        sex != 0 ? 0 : 0
      };
      ChaFileClothes clothes = coorde.clothes;
      for (int index1 = 0; index1 < numArray1.Length; ++index1)
      {
        if (categoryNoArray[index1] != ChaListDefine.CategoryNo.unknown)
        {
          if (!chaListCtrl.GetCategoryInfo(categoryNoArray[index1]).ContainsKey(clothes.parts[numArray1[index1]].id))
          {
            checkInfo?.Add(string.Format("【{0}】値:{1}", (object) strArray[index1], (object) clothes.parts[numArray1[index1]].id));
            flag = true;
            clothes.parts[numArray1[index1]].id = numArray2[index1];
          }
          Dictionary<int, ListInfoBase> categoryInfo = chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_pattern);
          for (int index2 = 0; index2 < clothes.parts[numArray1[index1]].colorInfo.Length; ++index2)
          {
            if (!categoryInfo.ContainsKey(clothes.parts[numArray1[index1]].colorInfo[index2].pattern))
            {
              checkInfo?.Add(string.Format("【柄】値:{0}", (object) clothes.parts[numArray1[index1]].colorInfo[index2].pattern));
              flag = true;
              clothes.parts[numArray1[index1]].colorInfo[index2].pattern = 0;
            }
          }
        }
      }
      ChaFileAccessory accessory = coorde.accessory;
      for (int index = 0; index < accessory.parts.Length; ++index)
      {
        int type = accessory.parts[index].type;
        int id = accessory.parts[index].id;
        Dictionary<int, ListInfoBase> categoryInfo = chaListCtrl.GetCategoryInfo((ChaListDefine.CategoryNo) type);
        if (categoryInfo != null && !categoryInfo.ContainsKey(accessory.parts[index].id))
        {
          checkInfo?.Add(string.Format("【{0}】値:{1}", (object) ChaAccessoryDefine.GetAccessoryTypeName((ChaListDefine.CategoryNo) type), (object) accessory.parts[index].id));
          flag = true;
          accessory.parts[index].MemberInit();
        }
      }
      return flag;
    }

    public static bool CheckDataRangeParameter(ChaFile chafile, List<string> checkInfo = null)
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      ChaFileParameter parameter = chafile.parameter;
      bool flag = false;
      if (parameter.sex == (byte) 0)
        return false;
      if (!Singleton<Manager.Voice>.Instance.voiceInfoDic.ContainsKey(parameter.personality))
      {
        checkInfo?.Add(string.Format("【性格】値:{0}", (object) parameter.personality));
        flag = true;
        parameter.personality = 0;
      }
      return flag;
    }

    public static bool CheckDataRange(
      ChaFile chafile,
      bool chk_custom,
      bool chk_clothes,
      bool chk_parameter,
      List<string> checkInfo = null)
    {
      bool flag = false;
      if (chk_custom)
        flag = flag | ChaFileControl.CheckDataRangeFace(chafile, checkInfo) | ChaFileControl.CheckDataRangeBody(chafile, checkInfo) | ChaFileControl.CheckDataRangeHair(chafile, checkInfo);
      if (chk_clothes)
        flag |= ChaFileControl.CheckDataRangeCoordinate(chafile, checkInfo);
      if (chk_parameter)
        flag |= ChaFileControl.CheckDataRangeParameter(chafile, checkInfo);
      if (flag)
        Debug.LogWarningFormat("【{0}】 範囲外！", new object[1]
        {
          (object) chafile.parameter.fullname
        });
      return flag;
    }

    public static bool InitializeCharaFile(string filename, int sex)
    {
      ChaFileControl chaFileControl = new ChaFileControl();
      if (!chaFileControl.LoadCharaFile(filename, (byte) sex, false, true))
        return false;
      if (chaFileControl.gameinfo.gameRegistration)
        return true;
      chaFileControl.InitGameInfoParam();
      chaFileControl.gameinfo.gameRegistration = true;
      chaFileControl.SaveCharaFile(filename, (byte) sex, false);
      return true;
    }

    public bool InitGameInfoParam()
    {
      ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
      ListInfoBase listInfo = chaListCtrl.GetListInfo(ChaListDefine.CategoryNo.init_type_param, this.parameter.personality);
      this.gameinfo.moodBound.lower = (float) listInfo.GetInfoInt(ChaListDefine.KeyType.MoodLow);
      this.gameinfo.moodBound.upper = (float) listInfo.GetInfoInt(ChaListDefine.KeyType.MoodUp);
      this.gameinfo.flavorState = new Dictionary<int, int>();
      for (int index = 0; index < 8; ++index)
        this.gameinfo.flavorState[index] = 0;
      this.gameinfo.desireDefVal = new Dictionary<int, float>();
      this.gameinfo.desireBuffVal = new Dictionary<int, float>();
      for (int index = 0; index < 16; ++index)
      {
        this.gameinfo.desireDefVal[index] = 0.0f;
        this.gameinfo.desireBuffVal[index] = 0.0f;
      }
      this.gameinfo.motivation = 0;
      this.gameinfo.immoral = 0;
      this.gameinfo.morality = 50;
      for (int index1 = 0; index1 < 8; ++index1)
      {
        Dictionary<int, int> flavorState;
        int index2;
        (flavorState = this.gameinfo.flavorState)[index2 = index1] = flavorState[index2] + listInfo.GetInfoInt((ChaListDefine.KeyType) (96 + index1));
      }
      for (int index1 = 0; index1 < 16; ++index1)
      {
        Dictionary<int, float> desireDefVal;
        int index2;
        (desireDefVal = this.gameinfo.desireDefVal)[index2 = index1] = desireDefVal[index2] + (float) listInfo.GetInfoInt((ChaListDefine.KeyType) (104 + index1));
        Dictionary<int, float> desireBuffVal;
        int index3;
        (desireBuffVal = this.gameinfo.desireBuffVal)[index3 = index1] = desireBuffVal[index3] + (float) listInfo.GetInfoInt((ChaListDefine.KeyType) (120 + index1));
      }
      this.gameinfo.motivation += listInfo.GetInfoInt(ChaListDefine.KeyType.Motivation);
      this.gameinfo.immoral += listInfo.GetInfoInt(ChaListDefine.KeyType.Immoral);
      Dictionary<int, ListInfoBase> categoryInfo = chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.init_wish_param);
      if (this.parameter.hsWish != null && this.parameter.hsWish.Count != 0)
      {
        foreach (int key in this.parameter.hsWish)
        {
          ListInfoBase listInfoBase;
          if (categoryInfo.TryGetValue(key, out listInfoBase))
          {
            this.gameinfo.moodBound.lower += (float) listInfoBase.GetInfoInt(ChaListDefine.KeyType.MoodLow);
            this.gameinfo.moodBound.upper += (float) listInfoBase.GetInfoInt(ChaListDefine.KeyType.MoodUp);
            for (int index1 = 0; index1 < 8; ++index1)
            {
              Dictionary<int, int> flavorState;
              int index2;
              (flavorState = this.gameinfo.flavorState)[index2 = index1] = flavorState[index2] + listInfoBase.GetInfoInt((ChaListDefine.KeyType) (96 + index1));
            }
            for (int index1 = 0; index1 < 16; ++index1)
            {
              Dictionary<int, float> desireDefVal;
              int index2;
              (desireDefVal = this.gameinfo.desireDefVal)[index2 = index1] = desireDefVal[index2] + (float) listInfoBase.GetInfoInt((ChaListDefine.KeyType) (104 + index1));
              Dictionary<int, float> desireBuffVal;
              int index3;
              (desireBuffVal = this.gameinfo.desireBuffVal)[index3 = index1] = desireBuffVal[index3] + (float) listInfoBase.GetInfoInt((ChaListDefine.KeyType) (120 + index1));
            }
            this.gameinfo.motivation += listInfoBase.GetInfoInt(ChaListDefine.KeyType.Motivation);
            this.gameinfo.immoral += listInfoBase.GetInfoInt(ChaListDefine.KeyType.Immoral);
          }
        }
      }
      for (int index = 0; index < 8; ++index)
        this.gameinfo.flavorState[index] = Mathf.Max(this.gameinfo.flavorState[index], 0);
      return true;
    }

    public bool SaveCharaFile(string filename, byte sex = 255, bool newFile = false)
    {
      string path = this.ConvertCharaFilePath(filename, sex, newFile);
      string directoryName = Path.GetDirectoryName(path);
      if (!System.IO.Directory.Exists(directoryName))
        System.IO.Directory.CreateDirectory(directoryName);
      this.charaFileName = Path.GetFileName(path);
      string userId = this.userID;
      string dataId = this.dataID;
      if (this.userID != Singleton<GameSystem>.Instance.UserUUID)
        this.dataID = YS_Assist.CreateUUID();
      else if (!File.Exists(path))
        this.dataID = YS_Assist.CreateUUID();
      this.userID = Singleton<GameSystem>.Instance.UserUUID;
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        bool flag = this.SaveCharaFile((Stream) fileStream, true);
        this.userID = userId;
        this.dataID = dataId;
        return flag;
      }
    }

    public bool SaveCharaFile(Stream st, bool savePng)
    {
      using (BinaryWriter bw = new BinaryWriter(st))
        return this.SaveCharaFile(bw, savePng);
    }

    public bool SaveCharaFile(BinaryWriter bw, bool savePng)
    {
      return this.SaveFile(bw, savePng, (int) Singleton<GameSystem>.Instance.language);
    }

    public bool LoadCharaFile(string filename, byte sex = 255, bool noLoadPng = false, bool noLoadStatus = true)
    {
      if (filename.IsNullOrEmpty())
        return false;
      this.charaFileName = Path.GetFileName(filename);
      string path = this.ConvertCharaFilePath(filename, sex, false);
      if (!File.Exists(path))
        return false;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        return this.LoadCharaFile((Stream) fileStream, noLoadPng, noLoadStatus);
    }

    public bool LoadCharaFile(Stream st, bool noLoadPng = false, bool noLoadStatus = true)
    {
      using (BinaryReader br = new BinaryReader(st))
        return this.LoadCharaFile(br, noLoadPng, noLoadStatus);
    }

    public bool LoadCharaFile(BinaryReader br, bool noLoadPng = false, bool noLoadStatus = true)
    {
      bool flag = this.LoadFile(br, (int) Singleton<GameSystem>.Instance.language, noLoadPng, noLoadStatus);
      if (!this.skipRangeCheck)
        Singleton<Character>.Instance.isMod = ChaFileControl.CheckDataRange((ChaFile) this, true, true, true, (List<string>) null);
      return flag;
    }

    public bool LoadFileLimited(
      string filename,
      byte sex = 255,
      bool face = true,
      bool body = true,
      bool hair = true,
      bool parameter = true,
      bool coordinate = true)
    {
      string path = this.ConvertCharaFilePath(filename, sex, false);
      ChaFileControl chaFileControl = new ChaFileControl();
      if (chaFileControl.LoadFile(path, (int) Singleton<GameSystem>.Instance.language, false, true))
      {
        if (!this.skipRangeCheck)
          Singleton<Character>.Instance.isMod = ChaFileControl.CheckDataRange((ChaFile) chaFileControl, true, true, true, (List<string>) null);
        if (face)
          this.custom.face = (ChaFileFace) MessagePackSerializer.Deserialize<ChaFileFace>(MessagePackSerializer.Serialize<ChaFileFace>((M0) chaFileControl.custom.face));
        if (body)
          this.custom.body = (ChaFileBody) MessagePackSerializer.Deserialize<ChaFileBody>(MessagePackSerializer.Serialize<ChaFileBody>((M0) chaFileControl.custom.body));
        if (hair)
          this.custom.hair = (ChaFileHair) MessagePackSerializer.Deserialize<ChaFileHair>(MessagePackSerializer.Serialize<ChaFileHair>((M0) chaFileControl.custom.hair));
        if (parameter)
          this.parameter.Copy(chaFileControl.parameter);
        if (coordinate)
          this.CopyCoordinate(chaFileControl.coordinate);
        if (face && body && (hair && parameter) && coordinate)
        {
          this.userID = chaFileControl.userID;
          this.dataID = chaFileControl.dataID;
        }
      }
      return false;
    }

    public bool LoadMannequinFile(
      string assetBundleName,
      string assetName,
      bool face = true,
      bool body = true,
      bool hair = true,
      bool parameter = true,
      bool coordinate = true)
    {
      ChaFileControl chaFileControl = new ChaFileControl();
      TextAsset ta = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Equality((Object) null, (Object) ta) || !chaFileControl.LoadFromTextAsset(ta, true, true))
        return false;
      if (face)
        this.custom.face = (ChaFileFace) MessagePackSerializer.Deserialize<ChaFileFace>(MessagePackSerializer.Serialize<ChaFileFace>((M0) chaFileControl.custom.face));
      if (body)
        this.custom.body = (ChaFileBody) MessagePackSerializer.Deserialize<ChaFileBody>(MessagePackSerializer.Serialize<ChaFileBody>((M0) chaFileControl.custom.body));
      if (hair)
        this.custom.hair = (ChaFileHair) MessagePackSerializer.Deserialize<ChaFileHair>(MessagePackSerializer.Serialize<ChaFileHair>((M0) chaFileControl.custom.hair));
      if (parameter)
        this.parameter.Copy(chaFileControl.parameter);
      if (coordinate)
        this.CopyCoordinate(chaFileControl.coordinate);
      return false;
    }

    public bool LoadFromAssetBundle(
      string assetBundleName,
      string assetName,
      bool noSetPNG = false,
      bool noLoadStatus = true)
    {
      TextAsset ta = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Equality((Object) null, (Object) ta))
        return false;
      bool flag = this.LoadFromTextAsset(ta, noSetPNG, noLoadStatus);
      AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, true);
      return flag;
    }

    public bool LoadFromTextAsset(TextAsset ta, bool noSetPNG = false, bool noLoadStatus = true)
    {
      if (Object.op_Equality((Object) null, (Object) ta))
        return false;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        memoryStream.Write(ta.get_bytes(), 0, ta.get_bytes().Length);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        return this.LoadCharaFile((Stream) memoryStream, noSetPNG, noLoadStatus);
      }
    }

    public bool LoadFromBytes(byte[] bytes, bool noSetPNG = false, bool noLoadStatus = true)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        memoryStream.Write(bytes, 0, bytes.Length);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        bool flag = this.LoadCharaFile((Stream) memoryStream, noSetPNG, noLoadStatus);
        if (!this.skipRangeCheck)
          Singleton<Character>.Instance.isMod = ChaFileControl.CheckDataRange((ChaFile) this, true, true, true, (List<string>) null);
        return flag;
      }
    }

    public void SaveFace(string path)
    {
      if (this.custom == null)
        return;
      this.custom.SaveFace(path);
    }

    public void LoadFace(string path)
    {
      if (this.custom == null)
        return;
      this.custom.LoadFace(path);
    }

    public void LoadFacePreset()
    {
      ListInfoBase listInfo = Singleton<Character>.Instance.chaListCtrl.GetListInfo(this.parameter.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_head : ChaListDefine.CategoryNo.mo_head, this.custom.face.headId);
      string info1 = listInfo.GetInfo(ChaListDefine.KeyType.MainManifest);
      string info2 = listInfo.GetInfo(ChaListDefine.KeyType.MainAB);
      string info3 = listInfo.GetInfo(ChaListDefine.KeyType.Preset);
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(info2, info3, false, info1);
      if (Object.op_Equality((Object) null, (Object) textAsset))
      {
        Debug.LogError((object) "顔のプリセットデータ読み込みに失敗");
      }
      else
      {
        this.custom.LoadFace(textAsset.get_bytes());
        AssetBundleManager.UnloadAssetBundle(info2, true, (string) null, false);
      }
    }

    public string ConvertCharaFilePath(string path, byte _sex, bool newFile = false)
    {
      byte num = _sex != byte.MaxValue ? _sex : this.parameter.sex;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (!(path != string.Empty))
        return string.Empty;
      string directoryName = Path.GetDirectoryName(path);
      string path1 = Path.GetFileName(path);
      string str = !(directoryName == string.Empty) ? directoryName + "/" : UserData.Path + (num != (byte) 0 ? "chara/female/" : "chara/male/");
      if (path1 == string.Empty)
        path1 = newFile || this.charaFileName == string.Empty ? (num != (byte) 0 ? "AISChaF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") : "AISChaM_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")) : this.charaFileName;
      return Path.GetExtension(path1).IsNullOrEmpty() ? str + Path.GetFileNameWithoutExtension(path1) + ".png" : str + path1;
    }

    public static string ConvertCoordinateFilePath(string path, byte sex)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (!(path != string.Empty))
        return string.Empty;
      string directoryName = Path.GetDirectoryName(path);
      string path1 = Path.GetFileName(path);
      string str = !(directoryName == string.Empty) ? directoryName + "/" : UserData.Path + (sex != (byte) 0 ? "coordinate/female/" : "coordinate/male/");
      if (path1 == string.Empty)
        path1 = sex != (byte) 0 ? "AISCoordeF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") : "AISCoordeM_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
      return Path.GetExtension(path1).IsNullOrEmpty() ? str + Path.GetFileNameWithoutExtension(path1) + ".png" : str + path1;
    }
  }
}
