using System;
using Xunit;
using Dhamo.共通;
using System.Collections.Generic;

namespace TestDhamo
{
    public class トークナイズテスト
    {
        [Fact]
        public void 比較テスト()
        {
            Assert.Equal(new トークン(1), new トークン(1));

            Assert.Equal(new トークン(トークン種別.記号, "*"), new トークン(トークン種別.記号, "*"));
        }

        [Fact]
        public void Test1()
        {
            var list = new LinkedList<string>();
            参照テスト(list);
            Assert.Equal("aaa", list.Last.Value);
        }

        void 参照テスト(LinkedList<string> list)
        {
            list.AddLast("aaa");
        }

    }
}
