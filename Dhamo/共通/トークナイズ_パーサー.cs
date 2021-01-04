using System;
using System.Collections.Generic;
using System.Text;

namespace Dhamo.共通
{
    public class トークン
    {
        public トークン種別 種別;
        public string トークン文字列;
        public int トークン数値;
        public int インデント;
        public トークン(トークン種別 kind, string トークン文字列)
        {
            this.種別 = kind;
            this.トークン文字列 = トークン文字列;
        }
        public トークン(int 数値)
        {
            this.種別 = トークン種別.数値;
            this.トークン数値 = 数値;
        }

        public トークン(トークン種別 kind, int インデント)
        {
            this.種別 = トークン種別.インデント;
            this.インデント = インデント;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as トークン);
        }

        public bool Equals(トークン token)
        {
            if (token.種別 != this.種別) return false;

            if (token.トークン文字列 != this.トークン文字列 && token.トークン文字列 != null) return false;

            if (token.トークン数値 != this.トークン数値) return false;

            return true;
        }

        public static bool operator ==(トークン c1, トークン c2)
        {
            //nullの確認（構造体のようにNULLにならない型では不要）
            //両方nullか（参照元が同じか）
            //(c1 == c2)とすると、無限ループ
            if (object.ReferenceEquals(c1, c2))
            {
                return true;
            }
            //どちらかがnullか
            //(c1 == null)とすると、無限ループ
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            return c1.Equals(c2);
        }

        public static bool operator !=(トークン c1, トークン c2)
        {
            return !(c1 == c2);
            //(c1 != c2)とすると、無限ループ
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.トークン文字列;
        }
    }
    public enum トークン種別
    {
        名前,              // 変数名なんかになったり
        文字列,            // "aaaa"
        数値,              // 数値
        記号,              // / [ ] ( ) , { }
        リターン,          // return
        インデント,        // インデント
    }

    /// <summary>
    /// List<トークン>に対する拡張です
    /// </summary>
    public static class トークン列拡張
    {
        public static トークン 先頭ポップ(this LinkedList<トークン> self)
        {
            var トークン = self.First.Value;
            self.RemoveFirst();
            return トークン;
        }

        // TODO: この関数なくして期待トークンに統一したほうがいいかも知らん
        public static トークン 先頭ポップ(this LinkedList<トークン> self, トークン token)
        {
            var トークン = self.First.Value;
            if (トークン.Equals(token))
            {
                return null;
            }
            self.RemoveFirst();
            return トークン;
        }

        public static bool 期待トークン(this LinkedList<トークン> self, トークン token)
        {
            if (self.Count == 0) return false;

            if (self.First.Value.Equals(token)) return true;

            return false;
        }
    }
}
