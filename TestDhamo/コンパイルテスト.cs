using System;
using Xunit;
using Dhamo.共通;
using Dhamo.コンパイラ;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TestDhamo
{
    public class コンパイルテスト
    {
        private readonly ITestOutputHelper output;

        public コンパイルテスト(ITestOutputHelper output)
        {
            this.output = output;
        }

        int 一行コンパイル(string 文字列)
        {
            return new コンパイラ(new string[] { 文字列 }).コンパイル();
        }

        [Fact]
        public void 一行コンパイルテスト()
        {

            Assert.Equal(1, 一行コンパイル("return 1"));

            Assert.Equal(2, 一行コンパイル("return 1*2"));

            Assert.Equal(2, 一行コンパイル("return 1+2"));
        }
    }
}
