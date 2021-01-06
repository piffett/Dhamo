using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Dhamo.共通;

namespace Dhamo.トークナイズ
{
    public static class トークナイザ
    {

        /// <summary>
        /// 文字列をすべてトークナイズします。改行記号で改行した場合は一行として処理します
        /// </summary>
        /// <param name="固定テキスト"></param>
        /// <returns>LinkedList<トークン></returns>
        public static LinkedList<トークン> トークナイズ(string[] テキスト列)
        {
            var 列 = 0;
            var トークナイズ実行行数 = 0;
            var トークン列 = new LinkedList<トークン>();
            foreach (var text in テキスト列)
            {
                var 可変行テキスト = text;
                トークナイズ実行行数++;

                // 行のインデントを深さを求めてトークナイズする
                可変行テキスト = 可変行テキスト.TrimStart(' ');
                列 += text.Length - 可変行テキスト.Length;
                トークン列.AddLast(new トークン(トークン種別.インデント, 列));

                try
                {
                    while (先頭トークナイズ(トークン列, ref 可変行テキスト).Item1)
                    {
                        Debug.WriteLine(可変行テキスト);
                    }
                }
                catch (Exception)
                {
                    throw new トークナイズ例外(トークナイズエラーメッセージ(text, トークナイズ実行行数, 列));
                }
            }

            return トークン列;
        }

        // トークナイズ中の可変行テキストからトークンを取り出してトークン列に格納します
        public static (bool, int) 先頭トークナイズ(LinkedList<トークン> トークン列, ref string 可変行テキスト)
        {
            if (可変行テキスト == String.Empty)
            {
                return (false, 0);
            }

            if (可変行テキスト.StartsWith(" "))
            {
                var 空白文字数 = 可変行テキスト.Length - 可変行テキスト.TrimStart(' ').Length;
                可変行テキスト = 可変行テキスト.TrimStart(' ');
                return (true, 空白文字数);
            }

            if (Char.IsDigit(可変行テキスト[0]))
            {
                var 結果 = 数字文字数(可変行テキスト);
                if (結果.is数値)
                {
                    var token = new トークン(int.Parse(可変行テキスト.Substring(0, 結果.長さ)));
                    トークン列.AddLast(token);
                    可変行テキスト = 可変行テキスト.Remove(0, 結果.長さ);
                    return (true, 結果.長さ);
                }
            }

            // TODO: リターン以外にも対応
            if (予約語判定(可変行テキスト) != 0)
            {
                var 文字数 = 予約語判定(可変行テキスト);
                var token = new トークン(トークン種別.リターン, 可変行テキスト.Substring(0, 文字数));
                トークン列.AddLast(token);
                可変行テキスト = 可変行テキスト.Remove(0, 文字数);
                return (true, 文字数);
            }

            if (記号判定(可変行テキスト) != 0)
            {
                var 文字数 = 記号判定(可変行テキスト);
                var token = new トークン(トークン種別.記号, 可変行テキスト.Substring(0, 文字数));
                トークン列.AddLast(token);
                可変行テキスト = 可変行テキスト.Remove(0, 文字数);
                return (true, 文字数);
            }

            if (識別子文字数(可変行テキスト) != 0)
            {
                var 文字数 = 識別子文字数(可変行テキスト);
                var token = new トークン(トークン種別.識別子, 可変行テキスト.Substring(0, 文字数));
                トークン列.AddLast(token);
                可変行テキスト = 可変行テキスト.Remove(0, 文字数);
                return (true, 文字数);
            }

            throw new NotImplementedException("このコードには到達しません");

        }

        // その文字が記号化どうかを判定し、記号の時は文字数を返します
        private static int 記号判定(string 判定文字列)
        {
            var 一文字記号文字 = new char[] { ':', '[', ']', ',', '=', '{', '}', '.', '(', ')', '!', '\'', '*', '+', '>', '<' };
            if (判定文字列.StartsWith("==") ||
                判定文字列.StartsWith("!=") ||
                判定文字列.StartsWith("<=") ||
                判定文字列.StartsWith(">=")
                )
            {
                return 2;
            }
            else if (一文字記号文字.Contains(判定文字列[0]))
            {
                return 1;
            }

            return 0;
        }

        // pfnなどの予約語を判定し、文字数を返す
        private static int 予約語判定(string 判定文字列)
        {
            if (判定文字列.StartsWith("pfn "))
            {
                return 3;
            }
            if (判定文字列.StartsWith("return "))
            {
                return 6;
            }
            return 0;
        }

        // 変数名など
        private static int 識別子文字数(string テキスト)
        {
            var 長さ = 0;
            while (true)
            {
                if (テキスト.Length <= 長さ)
                {
                    break;
                }

                if (Char.IsLetterOrDigit(テキスト[長さ]) || テキスト[長さ].CompareTo('_') == 0)
                {
                    長さ++;
                }
                else
                {
                    break;
                }
            }
            return 長さ;
        }

        private static (bool is数値, int 長さ) 数字文字数(string テキスト)
        {
            var 長さ = 0;
            var is整数 = false;

            while (true)
            {
                if (テキスト.Length <= 長さ)
                {
                    break;
                }

                if (Char.IsDigit(テキスト[長さ]))
                {
                    長さ++;
                }
                else if (テキスト[長さ].CompareTo('.') == 0)
                {
                    if (is整数 == false)
                    {
                        長さ++;
                        is整数 = true;
                    }
                    else
                    {
                        return (false, 0);
                    }
                }
                else
                {
                    break;
                }
            }
            return (true, 長さ);
        }

        private static string トークナイズエラーメッセージ(string 固定テキスト, int 実行完了行数, int 列)
        {
            return $"プログラムの解析に失敗しました。 {実行完了行数}行目" + Environment.NewLine + 固定テキスト + Environment.NewLine + new String(' ', 列) + $"^トークナイズできません。";
        }

    }

    [Serializable()]
    class トークナイズ例外 : Exception
    {
        public トークナイズ例外(string message)
            : base(message) { }
    }


}
