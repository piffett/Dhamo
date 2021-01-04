using System;
using System.Collections.Generic;
using System.Text;

namespace Dhamo.共通
{
    public class ノード
    {
        public ノード種別 種類;
        public List<ノード> 子ノード列 = new List<ノード>();
        public int 値;
        public string 文字列;
        int 変数先;
        public ノード()
        {
        }

        public ノード(ノード種別 種類)
        {
            this.種類 = 種類;
        }
        public ノード(ノード種別 種類, List<ノード> ノード列)
        {
            this.種類 = 種類;
            this.子ノード列 = ノード列;
        }
        public ノード(int 値)
        {
            this.種類 = ノード種別.整数;
            this.値 = 値;
        }
        public ノード(ノード種別 種類, string 文字列)
        {
            this.種類 = 種類;
            this.文字列 = 文字列;
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
    }

    public enum ノード種別
    {
        整数,     // 231
        リターン, // return
        乗算,     // *
    }
}
