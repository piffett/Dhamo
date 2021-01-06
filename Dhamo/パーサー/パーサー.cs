using System;
using System.Collections.Generic;
using System.Text;
using Dhamo.共通;

namespace Dhamo.パーサー
{
    public static class パーサー
    {
        // 複数行の同一インデントをブロックとして処理
        // TODO: インデントが変わった時の取り扱いなどは未実装
        public static ノード ブロック(LinkedList<トークン> トークン列)
        {
            var node = new ノード(ノード種別.ブロック);

            var ノード列 = new List<ノード>();
            while (トークン列.Count > 0)
            {
                // インデントのトークンをとりあえず削除
                トークン列.先頭ポップ(new トークン(トークン種別.インデント, 0));
                ノード列.Add(ステートメント(トークン列));
            }
            node.子ノード列 = ノード列;

            return node;
        }

        // TODO 現状一行しか対応していないので複数行で対応する
        public static ノード ステートメント(LinkedList<トークン> トークン列)
        {
            var node = new ノード();

            // インデントのトークンをとりあえず削除
            トークン列.RemoveFirst();

            if (トークン列.First.Value == (new トークン(トークン種別.リターン, "return")))
            {
                node = new ノード(ノード種別.リターン);
                トークン列.RemoveFirst();
                node.左(評価(トークン列));
            }
            return node;
        }
        public static ノード 評価(LinkedList<トークン> トークン列)
        {
            var node = 等号(トークン列);
            return node;
        }

        public static ノード 等号(LinkedList<トークン> トークン列)
        {
            var node = 比較(トークン列);
            if(トークン列.期待トークン(new トークン(トークン種別.記号, "=="))){
                トークン列.RemoveFirst();
                node = new ノード(ノード種別.等号, new List<ノード> { node, 比較(トークン列)});
            }
            return node;
        }

        public static ノード 比較(LinkedList<トークン> トークン列)
        {
            var node = 加算(トークン列);
            if (トークン列.期待トークン(new トークン(トークン種別.記号, "<"))){
                トークン列.RemoveFirst();
                node = new ノード(ノード種別.比較, new List<ノード> { node, 加算(トークン列) });
            }
            return node;
        }

        public static ノード 加算(LinkedList<トークン> トークン列)
        {
            var node = 乗算(トークン列);

            while (true)
            {
                if (トークン列.期待トークン(new トークン(トークン種別.記号, "+")))
                {
                    // +を削除
                    トークン列.RemoveFirst();
                    node = new ノード(ノード種別.加算, new List<ノード> { node, 乗算(トークン列) });
                }
                else
                {
                    return node;
                }
            }
        }

        public static ノード 乗算(LinkedList<トークン> トークン列)
        {
            var node = 値(トークン列);

            while (true)
            {
                if (トークン列.期待トークン(new トークン(トークン種別.記号, "*")))
                {
                    // 「*」を削除
                    トークン列.RemoveFirst();
                    node = new ノード(ノード種別.乗算, new List<ノード> { node, 値(トークン列) });
                }
                else
                {
                    return node;
                }
            }
        }

        //public static ノード 単項演算子(LinkedList<トークン> トークン列)
        //{

        //}

        public static ノード 値(LinkedList<トークン> トークン列)
        {
            if (トークン列.期待トークン(new トークン(トークン種別.記号, "(")))
            {
                // 「(」削除
                トークン列.RemoveFirst();
                var exprNode = 評価(トークン列);
                // 「)」削除
                トークン列.RemoveFirst();
                return exprNode;
            }
            var node = new ノード(new 値(トークン列.First.Value.トークン数値));
            トークン列.RemoveFirst();
            return node;
        }
    }
}
