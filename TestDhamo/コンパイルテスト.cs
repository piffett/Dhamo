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

        値 一行コンパイル(string 文字列)
        {
            return new コンパイラ(new string[] { 文字列 }).コンパイル();
        }

        [Fact]
        public void 一行コンパイルテスト()
        {

            Assert.Equal(new 値(1), 一行コンパイル("return 1"));

            Assert.Equal(new 値(2), 一行コンパイル("return 1*2"));

            Assert.Equal(new 値(3), 一行コンパイル("return 1+2"));

            Assert.Equal(new 値(7), 一行コンパイル("return 1+2*3"));

            Assert.Equal(new 値(9), 一行コンパイル("return (1+2)*3"));

            Assert.Equal(new 値(false), 一行コンパイル("return 1 == 3"));

            Assert.Equal(new 値(true), 一行コンパイル("return 2 == 1+1"));

            Assert.Equal(new 値(false), 一行コンパイル("return 1 < 1"));

            Assert.Equal(new 値(true), 一行コンパイル("return 1+9 > 5"));

            Assert.Equal(new 値(true), 一行コンパイル("return 1 <= 1"));

            Assert.Equal(new 値(false), 一行コンパイル("return 2 <= 1"));
        }

        [Fact]
        public void 複行コンパイルテスト()
        {
            Assert.Equal(new 値(5), new コンパイラ(new string[] { "a = 5", "return a" }).コンパイル());
        }


    }
}
