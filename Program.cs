using System;
using System.Collections.Generic;
using System.IO;

namespace YoutubeSrtToMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin convert srt files to Md With omit Time-Line information !");
            try
            {
                string dic = "";
                if (args.Length > 0)
                {
                    dic = args[0];
                }
                else
                {
                    dic = GetDefaultDirectory();
                }
                ConvertSrcToMD(dic);
            }
            catch (Exception ex)
            {
                Console.WriteLine("convert failed !" + ex.Message);
            }
            Console.WriteLine("end convert srt files!");
            Console.ReadKey();
        }

        public static void ConvertSrcToMD(string dic)
        {
            List<FileInfo> fileInfos = GetSrtFileList(dic);
            foreach(var fileInfo in fileInfos)
            {
                StreamReader sr = fileInfo.OpenText();
                string newFullName = dic + "\\" + fileInfo.Name.Replace(".srt", ".md");
                StreamWriter sw = File.CreateText(newFullName);
                try
                {
                    int row = 1;
                    while (sr.Peek() > 0)
                    {
                        if (row == 1)
                        {
                            sr.ReadLine();
                            if (sr.Peek() > 0)
                            {
                                sr.ReadLine();
                            }
                            row = row + 2;//跳过当前及 之后1行
                        }
                        else if (row > 2)
                        {
                            string text = sr.ReadLine();
                            if (string.IsNullOrEmpty(text))
                            {
                                if (sr.Peek() > 0)
                                {
                                    sr.ReadLine();
                                }
                                if (sr.Peek() > 0)
                                {
                                    sr.ReadLine();
                                }
                                row = row + 3;//跳过当前及 之后两行
                            }
                            else
                            {
                                sw.WriteLine(text);
                                row++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("failed!" + ex.Message);
                }
                finally
                {
                    sr.Close();
                    sw.Close();
                }
            }
        }

        public static List<FileInfo> GetSrtFileList(string dic)
        {
            var dicInfo = new DirectoryInfo(dic);
            List<FileInfo> resultFileInfos = new List<FileInfo>();
            FileInfo[] fInfos = dicInfo.GetFiles();
            foreach(var fInfo in fInfos)
            {
                if (fInfo.Extension.ToLower() == ".srt")
                {
                    resultFileInfos.Add(fInfo);
                }
            }
            return resultFileInfos;
        }

        public static string GetDefaultDirectory()
        {
            return "E:\\work";
        }
    }
}
