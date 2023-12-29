using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace WorldQuakeViewer.Updater
{
    internal class Program
    {
        private static readonly ConsoleColor defaultColor = Console.ForegroundColor;

        static void Main(string[] args)
        {
            try
            {
                ConWrite("///////////////////////////////////");
                ConWrite("//WorldQuakeViewer.Updater v1.0.1//");
                ConWrite("///////////////////////////////////");

                if (!File.Exists("WorldQuakeViewer.exe"))
                    throw new Exception("WorldQuakeViewer.exeと同じフォルダに入れてください。(自分で起動する必要はありません。)");

                if (!File.Exists("tmp.zip"))
                    throw new FileNotFoundException("tmp.zipが見つかりませんでした。");

                ConWrite("ファイルを展開しています...", ConsoleColor.Cyan);
                if (Directory.Exists("tmp"))
                    Directory.Delete("tmp", true);
                ZipFile.ExtractToDirectory("tmp.zip", "tmp");

                if (!Directory.Exists("tmp"))//何もない
                    throw new DirectoryNotFoundException("tmpフォルダが見つかりませんでした。");

                ConWrite("ファイルをコピーしています...", ConsoleColor.Cyan);
                foreach (string file in Directory.GetFiles("tmp"))
                {
                    File.Copy(file, Path.GetFileName(file), true);
                    ConWrite(Path.GetFileName(file), ConsoleColor.Green);
                }

                ConWrite("一時ファイルを削除しています...", ConsoleColor.Cyan);
                File.Delete("tmp.zip");
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
