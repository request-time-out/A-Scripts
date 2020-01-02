// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System.IO;

namespace AIChara
{
  public class ChaFileAssist
  {
    public void SaveFileAssist<T>(string path, T info)
    {
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          byte[] buffer = MessagePackSerializer.Serialize<T>(info);
          binaryWriter.Write(buffer);
        }
      }
    }

    public void LoadFileAssist<T>(string path, out T info)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          byte[] numArray = binaryReader.ReadBytes((int) fileStream.Length);
          info = MessagePackSerializer.Deserialize<T>(numArray);
        }
      }
    }

    public void LoadFileAssist<T>(byte[] bytes, out T info)
    {
      info = MessagePackSerializer.Deserialize<T>(bytes);
    }
  }
}
