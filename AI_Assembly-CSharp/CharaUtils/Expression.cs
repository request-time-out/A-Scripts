// Decompiled with JetBrains decompiler
// Type: CharaUtils.Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CharaUtils
{
  public class Expression : MonoBehaviour
  {
    public Transform trfChara;
    public Expression.ScriptInfo[] info;
    public bool enable;
    private Dictionary<string, Transform> dictTrf;

    public Expression()
    {
      base.\u002Ector();
    }

    public void SetCharaTransform(Transform trf)
    {
      this.trfChara = trf;
    }

    public void Initialize()
    {
      if (Object.op_Equality((Object) null, (Object) this.trfChara))
        return;
      this.FindObjectAll(this.dictTrf, this.trfChara);
      Transform trf = (Transform) null;
      foreach (Expression.ScriptInfo scriptInfo in this.info)
      {
        if (scriptInfo.enableLookAt && scriptInfo.lookAt != null)
        {
          if (!this.dictTrf.TryGetValue(scriptInfo.lookAt.lookAtName, out trf))
          {
            Debug.LogWarning((object) string.Format("ない : {0}", (object) scriptInfo.lookAt.lookAtName));
          }
          else
          {
            scriptInfo.lookAt.SetLookAtTransform(trf);
            if (!this.dictTrf.TryGetValue(scriptInfo.lookAt.targetName, out trf))
            {
              Debug.LogWarning((object) string.Format("ない : {0}", (object) scriptInfo.lookAt.targetName));
            }
            else
            {
              scriptInfo.lookAt.SetTargetTransform(trf);
              this.dictTrf.TryGetValue(scriptInfo.lookAt.upAxisName, out trf);
              scriptInfo.lookAt.SetUpAxisTransform(trf);
            }
          }
        }
        if (scriptInfo.enableCorrect && scriptInfo.correct != null)
        {
          if (!this.dictTrf.TryGetValue(scriptInfo.correct.correctName, out trf))
          {
            Debug.LogWarning((object) string.Format("ない : {0}", (object) scriptInfo.correct.correctName));
          }
          else
          {
            scriptInfo.correct.SetCorrectTransform(trf);
            this.dictTrf.TryGetValue(scriptInfo.correct.referenceName, out trf);
            scriptInfo.correct.SetReferenceTransform(trf);
          }
        }
      }
    }

    public void FindObjectAll(Dictionary<string, Transform> _dictTrf, Transform _trf)
    {
      if (!_dictTrf.ContainsKey(((Object) _trf).get_name()))
        _dictTrf[((Object) _trf).get_name()] = _trf;
      for (int index = 0; index < _trf.get_childCount(); ++index)
        this.FindObjectAll(_dictTrf, _trf.GetChild(index));
    }

    public void EnableCategory(int categoryNo, bool _enable)
    {
      for (int index = 0; index < this.info.Length; ++index)
      {
        if (this.info[index].categoryNo == categoryNo)
          this.info[index].enable = _enable;
      }
    }

    public void EnableIndex(int indexNo, bool _enable)
    {
      if (0 > indexNo || indexNo >= this.info.Length)
        return;
      this.info[indexNo].enable = _enable;
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {
      if (this.info == null || !this.enable)
        return;
      foreach (Expression.ScriptInfo scriptInfo in this.info)
        scriptInfo.Update();
    }

    private void OnDestroy()
    {
      if (this.info == null)
        return;
      foreach (Expression.ScriptInfo scriptInfo in this.info)
        scriptInfo.Destroy();
    }

    public bool LoadSetting(string assetBundleName, string assetName)
    {
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Equality((Object) null, (Object) textAsset))
      {
        Debug.LogError((object) "あってはならない");
        return false;
      }
      string[] strArray = textAsset.get_text().Replace("\r", string.Empty).Split('\n');
      List<string> slist = new List<string>();
      slist.AddRange((IEnumerable<string>) strArray);
      AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, true);
      return this.LoadSettingSub(slist);
    }

    public bool LoadSetting(string path)
    {
      List<string> slist = new List<string>();
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream, Encoding.UTF8))
        {
          while (streamReader.Peek() > -1)
            slist.Add(streamReader.ReadLine());
        }
      }
      return this.LoadSettingSub(slist);
    }

    public bool LoadSettingSub(List<string> slist)
    {
      if (slist.Count == 0)
        return false;
      int length = int.Parse(slist[0].Split('\t')[0]);
      if (length > slist.Count - 1)
        return false;
      this.info = new Expression.ScriptInfo[length];
      for (int index1 = 0; index1 < length; ++index1)
      {
        string[] strArray1 = slist[index1 + 1].Split('\t');
        this.info[index1] = new Expression.ScriptInfo();
        this.info[index1].index = index1;
        int num1 = 0;
        Expression.ScriptInfo scriptInfo1 = this.info[index1];
        string[] strArray2 = strArray1;
        int index2 = num1;
        int num2 = index2 + 1;
        int num3 = int.Parse(strArray2[index2]);
        scriptInfo1.categoryNo = num3;
        Expression.ScriptInfo scriptInfo2 = this.info[index1];
        string[] strArray3 = strArray1;
        int index3 = num2;
        int num4 = index3 + 1;
        int num5 = !(strArray3[index3] == "○") ? 0 : 1;
        scriptInfo2.enableLookAt = num5 != 0;
        int num6;
        if (this.info[index1].enableLookAt)
        {
          Expression.LookAt lookAt1 = this.info[index1].lookAt;
          string[] strArray4 = strArray1;
          int index4 = num4;
          int num7 = index4 + 1;
          string str1 = strArray4[index4];
          lookAt1.lookAtName = str1;
          if ("0" == this.info[index1].lookAt.lookAtName)
            this.info[index1].lookAt.lookAtName = string.Empty;
          else
            this.info[index1].elementName = this.info[index1].lookAt.lookAtName;
          Expression.LookAt lookAt2 = this.info[index1].lookAt;
          string[] strArray5 = strArray1;
          int index5 = num7;
          int num8 = index5 + 1;
          string str2 = strArray5[index5];
          lookAt2.targetName = str2;
          if ("0" == this.info[index1].lookAt.targetName)
            this.info[index1].lookAt.targetName = string.Empty;
          Expression.LookAt lookAt3 = this.info[index1].lookAt;
          System.Type enumType1 = typeof (Expression.LookAt.AxisType);
          string[] strArray6 = strArray1;
          int index6 = num8;
          int num9 = index6 + 1;
          string str3 = strArray6[index6];
          int num10 = (int) Enum.Parse(enumType1, str3);
          lookAt3.targetAxisType = (Expression.LookAt.AxisType) num10;
          Expression.LookAt lookAt4 = this.info[index1].lookAt;
          string[] strArray7 = strArray1;
          int index7 = num9;
          int num11 = index7 + 1;
          string str4 = strArray7[index7];
          lookAt4.upAxisName = str4;
          if ("0" == this.info[index1].lookAt.upAxisName)
            this.info[index1].lookAt.upAxisName = string.Empty;
          Expression.LookAt lookAt5 = this.info[index1].lookAt;
          System.Type enumType2 = typeof (Expression.LookAt.AxisType);
          string[] strArray8 = strArray1;
          int index8 = num11;
          int num12 = index8 + 1;
          string str5 = strArray8[index8];
          int num13 = (int) Enum.Parse(enumType2, str5);
          lookAt5.upAxisType = (Expression.LookAt.AxisType) num13;
          Expression.LookAt lookAt6 = this.info[index1].lookAt;
          System.Type enumType3 = typeof (Expression.LookAt.AxisType);
          string[] strArray9 = strArray1;
          int index9 = num12;
          int num14 = index9 + 1;
          string str6 = strArray9[index9];
          int num15 = (int) Enum.Parse(enumType3, str6);
          lookAt6.sourceAxisType = (Expression.LookAt.AxisType) num15;
          Expression.LookAt lookAt7 = this.info[index1].lookAt;
          System.Type enumType4 = typeof (Expression.LookAt.AxisType);
          string[] strArray10 = strArray1;
          int index10 = num14;
          int num16 = index10 + 1;
          string str7 = strArray10[index10];
          int num17 = (int) Enum.Parse(enumType4, str7);
          lookAt7.limitAxisType = (Expression.LookAt.AxisType) num17;
          Expression.LookAt lookAt8 = this.info[index1].lookAt;
          System.Type enumType5 = typeof (Expression.LookAt.RotationOrder);
          string[] strArray11 = strArray1;
          int index11 = num16;
          int num18 = index11 + 1;
          string str8 = strArray11[index11];
          int num19 = (int) Enum.Parse(enumType5, str8);
          lookAt8.rotOrder = (Expression.LookAt.RotationOrder) num19;
          Expression.LookAt lookAt9 = this.info[index1].lookAt;
          string[] strArray12 = strArray1;
          int index12 = num18;
          int num20 = index12 + 1;
          double num21 = (double) float.Parse(strArray12[index12]);
          lookAt9.limitMin = (float) num21;
          Expression.LookAt lookAt10 = this.info[index1].lookAt;
          string[] strArray13 = strArray1;
          int index13 = num20;
          num6 = index13 + 1;
          double num22 = (double) float.Parse(strArray13[index13]);
          lookAt10.limitMax = (float) num22;
        }
        else
          num6 = num4 + 10;
        Expression.ScriptInfo scriptInfo3 = this.info[index1];
        string[] strArray14 = strArray1;
        int index14 = num6;
        int num23 = index14 + 1;
        int num24 = !(strArray14[index14] == "○") ? 0 : 1;
        scriptInfo3.enableCorrect = num24 != 0;
        if (this.info[index1].enableCorrect)
        {
          Expression.Correct correct1 = this.info[index1].correct;
          string[] strArray4 = strArray1;
          int index4 = num23;
          int num7 = index4 + 1;
          string str1 = strArray4[index4];
          correct1.correctName = str1;
          if ("0" == this.info[index1].correct.correctName)
            this.info[index1].correct.correctName = string.Empty;
          else
            this.info[index1].elementName = this.info[index1].correct.correctName;
          Expression.Correct correct2 = this.info[index1].correct;
          string[] strArray5 = strArray1;
          int index5 = num7;
          int num8 = index5 + 1;
          string str2 = strArray5[index5];
          correct2.referenceName = str2;
          if ("0" == this.info[index1].correct.referenceName)
            this.info[index1].correct.referenceName = string.Empty;
          Expression.Correct correct3 = this.info[index1].correct;
          System.Type enumType1 = typeof (Expression.Correct.CalcType);
          string[] strArray6 = strArray1;
          int index6 = num8;
          int num9 = index6 + 1;
          string str3 = strArray6[index6];
          int num10 = (int) Enum.Parse(enumType1, str3);
          correct3.calcType = (Expression.Correct.CalcType) num10;
          Expression.Correct correct4 = this.info[index1].correct;
          System.Type enumType2 = typeof (Expression.Correct.RotationOrder);
          string[] strArray7 = strArray1;
          int index7 = num9;
          int num11 = index7 + 1;
          string str4 = strArray7[index7];
          int num12 = (int) Enum.Parse(enumType2, str4);
          correct4.rotOrder = (Expression.Correct.RotationOrder) num12;
          Expression.Correct correct5 = this.info[index1].correct;
          string[] strArray8 = strArray1;
          int index8 = num11;
          int num13 = index8 + 1;
          double num14 = (double) float.Parse(strArray8[index8]);
          correct5.charmRate = (float) num14;
          Expression.Correct correct6 = this.info[index1].correct;
          string[] strArray9 = strArray1;
          int index9 = num13;
          int num15 = index9 + 1;
          int num16 = !(strArray9[index9] == "○") ? 0 : 1;
          correct6.useRX = num16 != 0;
          Expression.Correct correct7 = this.info[index1].correct;
          string[] strArray10 = strArray1;
          int index10 = num15;
          int num17 = index10 + 1;
          double num18 = (double) float.Parse(strArray10[index10]);
          correct7.valRXMin = (float) num18;
          Expression.Correct correct8 = this.info[index1].correct;
          string[] strArray11 = strArray1;
          int index11 = num17;
          int num19 = index11 + 1;
          double num20 = (double) float.Parse(strArray11[index11]);
          correct8.valRXMax = (float) num20;
          Expression.Correct correct9 = this.info[index1].correct;
          string[] strArray12 = strArray1;
          int index12 = num19;
          int num21 = index12 + 1;
          int num22 = !(strArray12[index12] == "○") ? 0 : 1;
          correct9.useRY = num22 != 0;
          Expression.Correct correct10 = this.info[index1].correct;
          string[] strArray13 = strArray1;
          int index13 = num21;
          int num25 = index13 + 1;
          double num26 = (double) float.Parse(strArray13[index13]);
          correct10.valRYMin = (float) num26;
          Expression.Correct correct11 = this.info[index1].correct;
          string[] strArray15 = strArray1;
          int index15 = num25;
          int num27 = index15 + 1;
          double num28 = (double) float.Parse(strArray15[index15]);
          correct11.valRYMax = (float) num28;
          Expression.Correct correct12 = this.info[index1].correct;
          string[] strArray16 = strArray1;
          int index16 = num27;
          int num29 = index16 + 1;
          int num30 = !(strArray16[index16] == "○") ? 0 : 1;
          correct12.useRZ = num30 != 0;
          Expression.Correct correct13 = this.info[index1].correct;
          string[] strArray17 = strArray1;
          int index17 = num29;
          int num31 = index17 + 1;
          double num32 = (double) float.Parse(strArray17[index17]);
          correct13.valRZMin = (float) num32;
          Expression.Correct correct14 = this.info[index1].correct;
          string[] strArray18 = strArray1;
          int index18 = num31;
          int num33 = index18 + 1;
          double num34 = (double) float.Parse(strArray18[index18]);
          correct14.valRZMax = (float) num34;
        }
      }
      return true;
    }

    [Serializable]
    public class LookAt
    {
      public string lookAtName = string.Empty;
      public string targetName = string.Empty;
      public Expression.LookAt.AxisType targetAxisType = Expression.LookAt.AxisType.Z;
      public string upAxisName = string.Empty;
      public Expression.LookAt.AxisType upAxisType = Expression.LookAt.AxisType.Y;
      public Expression.LookAt.AxisType sourceAxisType = Expression.LookAt.AxisType.Y;
      public Expression.LookAt.AxisType limitAxisType = Expression.LookAt.AxisType.None;
      public Expression.LookAt.RotationOrder rotOrder = Expression.LookAt.RotationOrder.ZXY;
      [Range(-180f, 180f)]
      public float limitMin;
      [Range(-180f, 180f)]
      public float limitMax;

      public LookAt()
      {
        this.trfLookAt = (Transform) null;
        this.trfTarget = (Transform) null;
        this.trfUpAxis = (Transform) null;
      }

      public Transform trfLookAt { get; private set; }

      public Transform trfTarget { get; private set; }

      public Transform trfUpAxis { get; private set; }

      public void SetLookAtTransform(Transform trf)
      {
        this.trfLookAt = trf;
      }

      public void SetTargetTransform(Transform trf)
      {
        this.trfTarget = trf;
      }

      public void SetUpAxisTransform(Transform trf)
      {
        this.trfUpAxis = trf;
      }

      public void Update()
      {
        if (Object.op_Equality((Object) null, (Object) this.trfTarget) || Object.op_Equality((Object) null, (Object) this.trfLookAt))
          return;
        Vector3 upVector = this.GetUpVector();
        Vector3 vector3_1 = Vector3.Normalize(Vector3.op_Subtraction(this.trfTarget.get_position(), this.trfLookAt.get_position()));
        Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(upVector, vector3_1));
        Vector3 vector3_3 = Vector3.Cross(vector3_1, vector3_2);
        if (this.targetAxisType == Expression.LookAt.AxisType.RevX || this.targetAxisType == Expression.LookAt.AxisType.RevY || this.targetAxisType == Expression.LookAt.AxisType.RevZ)
        {
          vector3_1 = Vector3.op_UnaryNegation(vector3_1);
          vector3_2 = Vector3.op_UnaryNegation(vector3_2);
        }
        Vector3 xvec = Vector3.get_zero();
        Vector3 yvec = Vector3.get_zero();
        Vector3 zvec = Vector3.get_zero();
        switch (this.targetAxisType)
        {
          case Expression.LookAt.AxisType.X:
          case Expression.LookAt.AxisType.RevX:
            xvec = vector3_1;
            if (this.sourceAxisType == Expression.LookAt.AxisType.Y)
            {
              yvec = vector3_3;
              zvec = Vector3.op_UnaryNegation(vector3_2);
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevY)
            {
              yvec = Vector3.op_UnaryNegation(vector3_3);
              zvec = vector3_2;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.Z)
            {
              yvec = vector3_2;
              zvec = vector3_3;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevZ)
            {
              yvec = Vector3.op_UnaryNegation(vector3_2);
              zvec = Vector3.op_UnaryNegation(vector3_3);
              break;
            }
            break;
          case Expression.LookAt.AxisType.Y:
          case Expression.LookAt.AxisType.RevY:
            yvec = vector3_1;
            if (this.sourceAxisType == Expression.LookAt.AxisType.X)
            {
              xvec = vector3_3;
              zvec = vector3_2;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevX)
            {
              xvec = Vector3.op_UnaryNegation(vector3_3);
              zvec = Vector3.op_UnaryNegation(vector3_2);
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.Z)
            {
              xvec = Vector3.op_UnaryNegation(vector3_2);
              zvec = vector3_3;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevZ)
            {
              xvec = vector3_2;
              zvec = Vector3.op_UnaryNegation(vector3_3);
              break;
            }
            break;
          case Expression.LookAt.AxisType.Z:
          case Expression.LookAt.AxisType.RevZ:
            zvec = vector3_1;
            if (this.sourceAxisType == Expression.LookAt.AxisType.X)
            {
              xvec = vector3_3;
              yvec = Vector3.op_UnaryNegation(vector3_2);
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevX)
            {
              xvec = Vector3.op_UnaryNegation(vector3_3);
              yvec = vector3_2;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.Y)
            {
              xvec = vector3_2;
              yvec = vector3_3;
              break;
            }
            if (this.sourceAxisType == Expression.LookAt.AxisType.RevY)
            {
              xvec = Vector3.op_UnaryNegation(vector3_2);
              yvec = Vector3.op_UnaryNegation(vector3_3);
              break;
            }
            break;
        }
        if (this.limitAxisType == Expression.LookAt.AxisType.None)
        {
          this.trfLookAt.set_rotation(this.LookAtQuat(xvec, yvec, zvec));
        }
        else
        {
          this.trfLookAt.set_rotation(this.LookAtQuat(xvec, yvec, zvec));
          ConvertRotation.RotationOrder rotOrder = (ConvertRotation.RotationOrder) this.rotOrder;
          Quaternion localRotation = this.trfLookAt.get_localRotation();
          Vector3 vector3_4 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, localRotation);
          Quaternion q = Quaternion.Slerp(localRotation, Quaternion.get_identity(), 0.5f);
          Vector3 vector3_5 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, q);
          if (this.limitAxisType == Expression.LookAt.AxisType.X)
          {
            if (vector3_4.x < 0.0 && vector3_5.x > 0.0 || vector3_4.x > 0.0 && vector3_5.x < 0.0)
            {
              ref Vector3 local = ref vector3_4;
              local.x = (__Null) (local.x * -1.0);
            }
            vector3_4.x = (__Null) (double) Mathf.Clamp((float) vector3_4.x, this.limitMin, this.limitMax);
          }
          else if (this.limitAxisType == Expression.LookAt.AxisType.Y)
          {
            if (vector3_4.y < 0.0 && vector3_5.y > 0.0 || vector3_4.y > 0.0 && vector3_5.y < 0.0)
            {
              ref Vector3 local = ref vector3_4;
              local.y = (__Null) (local.y * -1.0);
            }
            vector3_4.y = (__Null) (double) Mathf.Clamp((float) vector3_4.y, this.limitMin, this.limitMax);
          }
          else if (this.limitAxisType == Expression.LookAt.AxisType.Z)
          {
            if (vector3_4.z < 0.0 && vector3_5.z > 0.0 || vector3_4.z > 0.0 && vector3_5.z < 0.0)
            {
              ref Vector3 local = ref vector3_4;
              local.z = (__Null) (local.z * -1.0);
            }
            vector3_4.z = (__Null) (double) Mathf.Clamp((float) vector3_4.z, this.limitMin, this.limitMax);
          }
          this.trfLookAt.set_localRotation(ConvertRotation.ConvertDegreeToQuaternion(rotOrder, (float) vector3_4.x, (float) vector3_4.y, (float) vector3_4.z));
        }
      }

      private Vector3 GetUpVector()
      {
        Vector3 vector3 = Vector3.get_up();
        if (Object.op_Inequality((Object) null, (Object) this.trfUpAxis))
        {
          switch (this.upAxisType)
          {
            case Expression.LookAt.AxisType.X:
              vector3 = this.trfUpAxis.get_right();
              break;
            case Expression.LookAt.AxisType.Y:
              vector3 = this.trfUpAxis.get_up();
              break;
            case Expression.LookAt.AxisType.Z:
              vector3 = this.trfUpAxis.get_forward();
              break;
          }
        }
        return vector3;
      }

      private Quaternion LookAtQuat(Vector3 xvec, Vector3 yvec, Vector3 zvec)
      {
        float num1 = (float) (1.0 + xvec.x + yvec.y + zvec.z);
        if ((double) num1 == 0.0)
          return Quaternion.get_identity();
        float f = Mathf.Sqrt(num1) / 2f;
        if (float.IsNaN(f))
          return Quaternion.get_identity();
        float num2 = 4f * f;
        return (double) num2 == 0.0 ? Quaternion.get_identity() : new Quaternion((float) (yvec.z - zvec.y) / num2, (float) (zvec.x - xvec.z) / num2, (float) (xvec.y - yvec.x) / num2, f);
      }

      public enum AxisType
      {
        X,
        Y,
        Z,
        RevX,
        RevY,
        RevZ,
        None,
      }

      public enum RotationOrder
      {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX,
      }
    }

    [Serializable]
    public class Correct
    {
      public string correctName = string.Empty;
      public string referenceName = string.Empty;
      public Expression.Correct.RotationOrder rotOrder = Expression.Correct.RotationOrder.ZXY;
      public Expression.Correct.CalcType calcType;
      [Range(0.0f, 1f)]
      public float charmRate;
      public bool useRX;
      [Range(-1f, 1f)]
      public float valRXMin;
      [Range(-1f, 1f)]
      public float valRXMax;
      public bool useRY;
      [Range(-1f, 1f)]
      public float valRYMin;
      [Range(-1f, 1f)]
      public float valRYMax;
      public bool useRZ;
      [Range(-1f, 1f)]
      public float valRZMin;
      [Range(-1f, 1f)]
      public float valRZMax;

      public Correct()
      {
        this.trfCorrect = (Transform) null;
        this.trfReference = (Transform) null;
      }

      public Transform trfCorrect { get; private set; }

      public Transform trfReference { get; private set; }

      public void SetCorrectTransform(Transform trf)
      {
        this.trfCorrect = trf;
      }

      public void SetReferenceTransform(Transform trf)
      {
        this.trfReference = trf;
      }

      public void Update()
      {
        if (Object.op_Equality((Object) null, (Object) this.trfCorrect) || Object.op_Equality((Object) null, (Object) this.trfReference))
          return;
        if (this.calcType == Expression.Correct.CalcType.Euler)
        {
          ConvertRotation.RotationOrder rotOrder = (ConvertRotation.RotationOrder) this.rotOrder;
          Vector3 vector3_1 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, this.trfCorrect.get_localRotation());
          Vector3 vector3_2 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, this.trfReference.get_localRotation());
          Quaternion.get_identity();
          Vector3 vector3_3 = Vector3.get_zero();
          if ((double) this.charmRate != 0.0)
          {
            Quaternion q = Quaternion.Slerp(this.trfReference.get_localRotation(), Quaternion.get_identity(), this.charmRate);
            vector3_3 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, q);
          }
          if (this.useRX)
          {
            float num = Mathf.Lerp(this.valRXMin, this.valRXMax, Mathf.InverseLerp(0.0f, 90f, Mathf.Clamp(Mathf.Abs((float) vector3_2.x), 0.0f, 90f)));
            vector3_1.x = (__Null) (vector3_2.x * (double) num);
            if ((double) this.charmRate != 0.0 && (vector3_2.x < 0.0 && vector3_3.x > 0.0 || vector3_2.x > 0.0 && vector3_3.x < 0.0))
            {
              ref Vector3 local = ref vector3_1;
              local.x = (__Null) (local.x * -1.0);
            }
          }
          if (this.useRY)
          {
            float num = Mathf.Lerp(this.valRYMin, this.valRYMax, Mathf.InverseLerp(0.0f, 90f, Mathf.Clamp(Mathf.Abs((float) vector3_2.y), 0.0f, 90f)));
            vector3_1.y = (__Null) (vector3_2.y * (double) num);
            if ((double) this.charmRate != 0.0 && (vector3_2.y < 0.0 && vector3_3.y > 0.0 || vector3_2.y > 0.0 && vector3_3.y < 0.0))
            {
              ref Vector3 local = ref vector3_1;
              local.y = (__Null) (local.y * -1.0);
            }
          }
          if (this.useRZ)
          {
            float num = Mathf.Lerp(this.valRZMin, this.valRZMax, Mathf.InverseLerp(0.0f, 90f, Mathf.Clamp(Mathf.Abs((float) vector3_2.z), 0.0f, 90f)));
            vector3_1.z = (__Null) (vector3_2.z * (double) num);
            if ((double) this.charmRate != 0.0 && (vector3_2.z < 0.0 && vector3_3.z > 0.0 || vector3_2.z > 0.0 && vector3_3.z < 0.0))
            {
              ref Vector3 local = ref vector3_1;
              local.z = (__Null) (local.z * -1.0);
            }
          }
          this.trfCorrect.set_localRotation(ConvertRotation.ConvertDegreeToQuaternion(rotOrder, (float) vector3_1.x, (float) vector3_1.y, (float) vector3_1.z));
        }
        else
        {
          if (this.calcType != Expression.Correct.CalcType.Quaternion)
            return;
          Quaternion localRotation = this.trfCorrect.get_localRotation();
          if (this.useRX)
            localRotation.x = (__Null) (this.trfReference.get_localRotation().x * ((double) this.valRXMin + (double) this.valRXMax) * 0.5);
          if (this.useRY)
            localRotation.y = (__Null) (this.trfReference.get_localRotation().y * ((double) this.valRYMin + (double) this.valRYMax) * 0.5);
          if (this.useRZ)
            localRotation.z = (__Null) (this.trfReference.get_localRotation().z * ((double) this.valRZMin + (double) this.valRZMax) * 0.5);
          this.trfCorrect.set_localRotation(localRotation);
        }
      }

      public enum CalcType
      {
        Euler,
        Quaternion,
      }

      public enum RotationOrder
      {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX,
      }
    }

    [Serializable]
    public class ScriptInfo
    {
      public string elementName = string.Empty;
      public bool enable = true;
      public Expression.LookAt lookAt = new Expression.LookAt();
      public Expression.Correct correct = new Expression.Correct();
      public bool enableLookAt;
      public bool enableCorrect;
      public int index;
      public int categoryNo;

      public void Update()
      {
        if (!this.enable)
          return;
        if (this.enableLookAt && this.lookAt != null)
          this.lookAt.Update();
        if (!this.enableCorrect || this.correct == null)
          return;
        this.correct.Update();
      }

      public void UpdateArrow()
      {
      }

      public void Destroy()
      {
      }

      public void DestroyArrow()
      {
      }
    }
  }
}
