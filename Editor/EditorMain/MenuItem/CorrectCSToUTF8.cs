#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
#region
//saber战棋
#endregion
namespace Saber
{
    public class CorrectCSToUTF8
    {
        [MenuItem("Saber/Function/CorrectToUTF-8")]
        public static void ToScanAssets()
        {
            if (Application.isPlaying) return;
            string folderPath = Path.GetFullPath(Application.dataPath);
            string[] files = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            var enc = Encoding.GetEncoding("GB2312");
            List<string> list=new List<string>();
            // Loop through the files
            foreach (string file in files)
            {
                string content = string.Empty;
                Encoding encoding = GetTextFileEncodingType(file);
                if (encoding !=Encoding.UTF8)
                {
                    content=File.ReadAllText(file,enc);
                }
                if (!string.IsNullOrEmpty(content))
                {
                    File.WriteAllText(file,content,Encoding.UTF8);
                    list.Add($"修改成功--->{file}");
                }
                //// Write the content of the file with the UTF-8 encoding
            }
            AssetDatabase.Refresh();
            foreach(string f in list)
                Debug.Log(f);
            Debug.Log($"完成扫描{Application.dataPath}下全部.cs文件，共修改<color=red>{list.Count}</color>个文件编码至UTF-8");
        }


        /// <summary>
        /// 获取文本文件的字符编码类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static Encoding GetTextFileEncodingType(string fileName)
        {
            Encoding encoding = Encoding.Default;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream, encoding);
            byte[] buffer = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            if (buffer.Length >= 3 && buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
            {
                encoding = Encoding.UTF8;
            }
            else if (buffer.Length >= 3 && buffer[0] == 254 && buffer[1] == 255 && buffer[2] == 0)
            {
                encoding = Encoding.BigEndianUnicode;
            }
            else if (buffer.Length >= 3 && buffer[0] == 255 && buffer[1] == 254 && buffer[2] == 65)
            {
                encoding = Encoding.Unicode;
            }
            else if (IsUTF8Bytes(buffer))
            {
                encoding = Encoding.UTF8;
            }
            return encoding;
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// BOM（Byte Order Mark），字节顺序标记，出现在文本文件头部，Unicode编码标准中用于标识文件是采用哪种格式的编码。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }

}
#endif