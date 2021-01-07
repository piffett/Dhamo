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
        Dictionary<string, 値> 値列 = new Dictionary<string, 値>();
        public コンパイラ(string[] テキスト列)
        {
            this.テキスト列 = テキスト列;
        }

        public 値 コンパイル()
        {
            var a = パーサー.パーサー.ブロック(トークナイザ.トークナイズ(テキスト列));
            while (true)
            {


                if (a.子ノード列.Count > 0)
                {
                    var b = 実行(a);
                    if (b.種別 == ノード種別.リターン)
                    {
                        return b.左().value;
                    }
                    continue;
                }
                else
                {
                    return new 値(0);
                }

            }
        }

        public ノード 実行(ノード node)
        {
            while (true)
            {
                if (node.種別 == ノード種別.加算)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value + node2.value);
                }
                else if (node.種別 == ノード種別.乗算)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value * node2.value);
                }
                else if (node.種別 == ノード種別.整数)
                {
                    return node;
                }
                else if (node.種別 == ノード種別.リターン)
                {
                    node.左(実行(node.左()));
                    return node;
                }
                else if (node.種別 == ノード種別.等号)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(new 値(node1.value == node2.value));
                }
                else if (node.種別 == ノード種別.比較)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value < node2.value);
                }
                else if (node.種別 == ノード種別.等比較)
                {
                    var node1 = 実行(node.左());
                    var node2 = 実行(node.右());
                    return new ノード(node1.value <= node2.value);
                }
                else if (node.種別 == ノード種別.代入)
                {
                    var node2 = 実行(node.右());
                    値列[node.左().value.名前()] = node2.value;
                    return new ノード(node2.value);
                }
                else if (node.種別 == ノード種別.ブロック)
                {
                    var node1 = 実行(node.左());
                    node.子ノード列.RemoveAt(0);
                    return node1;
                }
                else if (node.種別 == ノード種別.識別子)
                {
                    return new ノード(値列[node.value.名前()]);
                }

                throw new Exception("実行エラー");
            }
        }
    }


}
