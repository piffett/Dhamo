using System;
using System.Collections.Generic;
using System.Text;

namespace Dhamo.共通
{
    public class ノード
    {
        public ノード種別 種別;
        public List<ノード> 子ノード列 = new List<ノード>();
        public 値 value;
        public ノード()
        {
        }

        public ノード(ノード種別 種類)
        {
            this.種別 = 種類;
        }
        public ノード(ノード種別 種類, List<ノード> ノード列)
        {
            this.種別 = 種類;
            this.子ノード列 = ノード列;
        }
        public ノード(値 値)
        {
            if (値.type == 1)
            {
                this.種別 = ノード種別.整数;
            }
            else if (値.type == 2)
            {
                this.種別 = ノード種別.識別子;
            }
            this.value = 値;
        }
        public ノード(ノード種別 種類, string 文字列)
        {
            this.種別 = 種類;
            this.value = new 値(文字列);
        }

        public ノード 右()
        {
            return this.子ノード列[1];
        }
        public void 右(ノード node)
        {
            this.子ノード列[1] = node;
        }
        public ノード 左()
        {
            return this.子ノード列[0];
        }

        public void 左(ノード node)
        {
            if (this.子ノード列.Count > 0)
            {
                this.子ノード列[0] = node;
            }
            else
            {
                this.子ノード列.Add(node);
            }
        }

        public override string ToString()
        {
            return this.種別.ToString();
        }
    }

    public enum ノード種別
    {
        謎,       // デバック用
        ブロック, // 同じインデントが続く区切り
        整数,     // 231
        リターン, // return
        乗算,     // *
        加算,     // +
        等号,     // ==
        比較,     // <
        等比較,   // <=
        代入,     // a = 1
        識別子,   // a
    }


    public class 値
    {
        public int type; // 0 bool 1 int 2 string
        public bool a;
        int b;
        string c;
        public 値(bool a)
        {
            type = 0;
            this.a = a;
        }
        public 値(int b)
        {
            type = 1;
            this.b = b;
        }
        public 値(string c)
        {
            type = 2;
            this.c = c;
        }

        public string 名前()
        {
            return c;
        }
        public static 値 operator +(値 v1, 値 v2)
        {
            if (v1.type == 0 && v2.type == 0) return new 値(false);

            if (v1.type == 1 && v2.type == 1) return new 値(v1.b + v2.b);

            if (v1.type == 2 && v2.type == 2) return new 値(v1.c + v2.c);

            // TODO: 文字列と数値みたいな組み合わせにも対応する
            return new 値(false);

        }

        public static 値 operator *(値 v1, 値 v2)
        {
            if (v1.type == 0 && v2.type == 0) return new 値(false);

            if (v1.type == 1 && v2.type == 1) return new 値(v1.b * v2.b);

            // TODO: 文字列の掛け算はエラーに
            if (v1.type == 2 && v2.type == 2) return new 値(false);

            return new 値(false);

        }
        public static bool operator ==(値 c1, 値 c2)
        {
            if (object.ReferenceEquals(c1, c2))
            {
                return true;
            }
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            return c1.Equals(c2);
        }

        public static bool operator !=(値 c1, 値 c2)
        {
            return !(c1 == c2);
        }

        public static 値 operator <=(値 v1, 値 v2)
        {
            if (v1.type == 0 && v2.type == 0) return new 値(false);

            if (v1.type == 1 && v2.type == 1) return new 値(v1.b <= v2.b);

            if (v1.type == 2 && v2.type == 2) return new 値(false);

            return new 値(false);
        }

        public static 値 operator >=(値 v1, 値 v2)
        {
            // TODO: 文字列と数値みたいな組み合わせにも対応する
            return new 値(!(v1 < v2).a);
        }

        public static 値 operator <(値 v1, 値 v2)
        {
            if (v1.type == 0 && v2.type == 0) return new 値(false);

            if (v1.type == 1 && v2.type == 1) return new 値(v1.b < v2.b);

            if (v1.type == 2 && v2.type == 2) return new 値(false);

            return new 値(false);
        }

        public static 値 operator >(値 v1, 値 v2)
        {
            // TODO: 文字列と数値みたいな組み合わせにも対応する
            return new 値(!(v1 < v2).a);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as 値);
        }

        public bool Equals(値 value)
        {
            if (type != value.type) return false;

            if (type == 0) return this.a == value.a;

            if (type == 1) return this.b == value.b;

            if (type == 2) return this.c == value.c;

            return false;
        }

        public override string ToString()
        {
            if (type == 0) return "論理型:" + this.a;

            if (type == 1) return "整数型:" + this.b;

            if (type == 2) return "文字列型:" + this.c;

            return "無効";
        }
    }
}
