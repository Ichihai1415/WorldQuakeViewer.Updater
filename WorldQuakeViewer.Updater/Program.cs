using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace WorldQuakeViewer.Updater
{
    internal class Program
    {
        private static readonly string version = "1.0.0";
        private static readonly ConsoleColor defaultColor = Console.ForegroundColor;

        static void Main(string[] args)
        {
            try
            {
                ConWrite("///////////////////////////////////");
                ConWrite($"//WorldQuakeViewer.Updater v{version}//");
                ConWrite("///////////////////////////////////");

                if (File.Exists("tmp.zip"))//通常はここ
                {
                    ConWrite("ファイルを展開しています...", ConsoleColor.Cyan);
                    if (Directory.Exists("tmp"))
                        Directory.Delete("tmp", true);
                    ZipFile.ExtractToDirectory("tmp.zip", "tmp");
                    File.Delete("tmp.zip");
                }

                if (!Directory.Exists("tmp"))//何もない
                    throw new DirectoryNotFoundException("tmpフォルダが見つかりませんでした。");

                ConWrite("ファイルをコピーしています...", ConsoleColor.Cyan);
                foreach (string file in Directory.GetFiles("tmp"))
                {
                    File.Copy(file, Path.GetFileName(file), true);
                    ConWrite(Path.GetFileName(file), ConsoleColor.Green);
                }

                ConWrite("一時ファイルを削除しています...", ConsoleColor.Cyan);
                Directory.Delete("tmp", true);

                ConWrite("完了しました。1秒後起動します。", ConsoleColor.Cyan);
                Thread.Sleep(1000);
                Process.Start("WorldQuakeViewer.exe");
            }
            catch (Exception ex)
            {
                ConWrite($"エラーが発生しました。\n{ex}", ConsoleColor.Red);
                ConWrite("\n何かキーを押すと終了します。");
                Console.ReadKey();
                return;
            }
        }

        static void ConWrite(string text, bool withLine = true)
        {
            ConWrite(text, defaultColor, withLine);
        }

        static void ConWrite(string text, ConsoleColor color, bool withLine = true)
        {
            Console.ForegroundColor = color;
            if (withLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
        }
    }
}
