using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter.Interop;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class ProxyObjectsTests
	{
		public class Proxy
		{
			[MoonSharpVisible(false)]
			public Random random;

			[MoonSharpVisible(false)]
			public Proxy(Random r)
			{
				random = r;
			}

			public int GetValue() { return 3; }
		}

		[Fact]
		public void ProxyTest()
		{
			UserData.RegisterProxyType<Proxy, Random>(r => new Proxy(r));

			Script S = new Script();

			S.Globals["R"] = new Random();
			S.Globals["func"] = (Action<Random>)(r => { Assert.NotNull(r); Assert.True(r is Random); });

			S.DoString(@"
				x = R.GetValue();
				func(R);
			");

			Assert.Equal(3.0, S.Globals.Get("x").Number);
		}


	}
}
