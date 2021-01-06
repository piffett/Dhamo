using System;
using System.Collections.Generic;
using System.Text;
using Dhamo.パーサー;
using Dhamo.トークナイズ;
using System.Diagnostics;
using Dhamo.共通;

namespace Dhamo.コンパイラ
{
    public class コンパイラ
    {
        string[] テキスト列;
        public コンパイラ(string[] テキスト列)
        {
            this.テキスト列 = テキスト列;
        }

        public 値 コンパイル()
        {
            var a = パーサー.パーサー.ステートメント(トークナイザ.トークナイズ(テキスト列));
            var b = 実行(a);
            return b.value;
        }

        public ノード 実行(ノード node)
        {
            while (true)
            {
                if (node.種類 == ノード種別.加算)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value + node2.value);
                }
                else if (node.種類 == ノード種別.乗算)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value * node2.value);
                }
                else if (node.種類 == ノード種別.整数)
                {
                    return node;
                }
                else if (node.種類 == ノード種別.リターン)
                {
                    node = 実行(node.左());
                }
                else if (node.種類 == ノード種別.等号)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(new 値(node1.value == node2.value));
                }
                else if (node.種類 == ノード種別.比較)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value < node2.value);
                }

                return node;
            }
        }
    }


}
