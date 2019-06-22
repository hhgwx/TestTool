using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace japanWord
{
    class ReadWriteFile
    {
        public List<String> readFile(String path)
        {
            List<String> wordList = new List<String>();
            try
            {
                // StreamReader の新しいインスタンスを生成する
                StreamReader sreader = (
                    new StreamReader(path, System.Text.Encoding.GetEncoding("UTF-8"))
                    );

                // 読み込んだ結果をすべて格納するための変数を宣言する
                string readline = string.Empty;

                // 読み込みできる文字がなくなるまで繰り返す
                while (sreader.Peek() >= 0)
                {
                    // ファイルを 1 行ずつ読み込む
                    readline = sreader.ReadLine().ToString();
                    if (readline.Length >= 1)
                    {
                        wordList.Add(readline);
                    }   // end if
                }   // end while

                // sreader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
                sreader.Close();

                return wordList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("設定ファイルの読込み処理でエラーが発生しました。：" + ex.Message.ToString());
                return wordList;
            }

        }

        //flag false:上書き/ true:追加
        public void writeFile(String path, String message, Boolean flag)
        {
            List<String> wordList = new List<String>();
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(
                    path,
                    flag,  //  （ false:上書き/ true:追加 ）
                    Encoding.GetEncoding("UTF-8"));

                writer.WriteLine(message);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("レコード登録でエラーが発生しました。：" + ex.Message.ToString());
            }

        }
    }
}
