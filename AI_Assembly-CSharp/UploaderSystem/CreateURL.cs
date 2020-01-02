// Decompiled with JetBrains decompiler
// Type: UploaderSystem.CreateURL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using System.Text;
using UnityEngine;

namespace UploaderSystem
{
  public class CreateURL : MonoBehaviour
  {
    [Space]
    public string AIS_Check_URL;
    [Button("CreateAIS_URL", "AIS_Check_URL作成", new object[] {0})]
    public int excuteCreateAIS_Check_URL;
    [Space]
    [Space]
    public string AIS_System_URL;
    [Button("CreateAIS_URL", "AIS_System_URL作成", new object[] {1})]
    public int excuteCreateAIS_System_URL;
    [Space]
    [Space]
    public string AIS_UploadChara_URL;
    [Button("CreateAIS_URL", "AIS_UploadChara_URL作成", new object[] {2})]
    public int excuteCreateAIS_UploadChara_URL;
    [Space]
    [Space]
    public string AIS_UploadHousing_URL;
    [Button("CreateAIS_URL", "AIS_UploadHousing_URL作成", new object[] {3})]
    public int excuteCreateAIS_UploadHousing_URL;
    [Space]
    [Space]
    public string AIS_Version_URL;
    [Button("CreateAIS_URL", "AIS_Version_URL作成", new object[] {4})]
    public int excuteCreateAIS_Version_URL;

    public CreateURL()
    {
      base.\u002Ector();
    }

    public void CreateAIS_URL(int kind)
    {
      string s = string.Empty;
      string str = string.Empty;
      switch (kind)
      {
        case 0:
          s = this.AIS_Check_URL;
          str = "ais_check_url.dat";
          break;
        case 1:
          s = this.AIS_System_URL;
          str = "ais_system_url.dat";
          break;
        case 2:
          s = this.AIS_UploadChara_URL;
          str = "ais_uploadChara_url.dat";
          break;
        case 3:
          s = this.AIS_UploadHousing_URL;
          str = "ais_uploadHousing_url.dat";
          break;
        case 4:
          s = this.AIS_Version_URL;
          str = "ais_version_url.dat";
          break;
      }
      byte[] buffer = YS_Assist.EncryptAES(Encoding.UTF8.GetBytes(s), "aisyoujyo", "phpaddress");
      string path = Application.get_dataPath() + "/../DefaultData/url/" + str;
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
          binaryWriter.Write(buffer);
      }
    }

    public static string LoadURL(string urlFile)
    {
      byte[] srcData = (byte[]) null;
      string path = Application.get_dataPath() + "/../DefaultData/url/" + urlFile;
      if (!File.Exists(path))
        return string.Empty;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
          srcData = binaryReader.ReadBytes((int) fileStream.Length);
      }
      if (srcData == null)
        return string.Empty;
      return Encoding.UTF8.GetString(YS_Assist.DecryptAES(srcData, "aisyoujyo", "phpaddress"));
    }
  }
}
