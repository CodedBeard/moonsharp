using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter.Interop;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class ConfigPropertyAssignerTests
	{
		private class MySubclass
		{
			[MoonSharpProperty]
			public string MyString { get; set; }

			[MoonSharpProperty("number")]
			public int MyNumber { get; private set; }
		}

		private class MyClass
		{
			[MoonSharpProperty]
			public string MyString { get; set; }

			[MoonSharpProperty("number")]
			public int MyNumber { get; private set; }

			[MoonSharpProperty]
			internal Table SomeTable { get; private set; }

			[MoonSharpProperty]
			public DynValue NativeValue { get; private set; }

			[MoonSharpProperty]
			public MySubclass SubObj { get; private set; }
		}

		private MyClass Test(string tableDef)
		{
			Script s = new Script(CoreModules.None);

			DynValue table = s.DoString("return " + tableDef);

			Assert.Equal(DataType.Table, table.Type);

			PropertyTableAssigner<MyClass> pta = new PropertyTableAssigner<MyClass>("class");
			PropertyTableAssigner<MySubclass> pta2 = new PropertyTableAssigner<MySubclass>();

			pta.SetSubassigner(pta2);

			MyClass o = new MyClass();

			pta.AssignObject(o, table.Table);

			return o;
		}

		[Fact]
		public void ConfigProp_SimpleAssign()
		{
			MyClass x = Test(@"
				{
				class = 'oohoh',
				myString = 'ciao',
				number = 3,
				some_table = {},
				nativeValue = function() end,
				subObj = { number = 15, myString = 'hi' },
				}");

			Assert.Equal(x.MyNumber, 3);
			Assert.Equal(x.MyString, "ciao");
			Assert.Equal(x.NativeValue.Type, DataType.Function);
			Assert.Equal(x.SubObj.MyNumber, 15);
			Assert.Equal(x.SubObj.MyString, "hi");
			Assert.NotNull(x.SomeTable);

		}


		[Fact]
		public void ConfigProp_ThrowsOnInvalid()
		{
            Assert.Throws<SyntaxErrorException>(() => {
			MyClass x = Test(@"
				{
				class = 'oohoh',
				myString = 'ciao',
				number = 3,
				some_table = {},
				invalid = 3,that
				nativeValue = function() end,
				}");
                Assert.Equal(x.MyNumber, 3);
                Assert.Equal(x.MyString, "ciao");
                Assert.Equal(x.NativeValue.Type, DataType.Function);
                Assert.NotNull(x.SomeTable);
            });
            

		}

	}
}
