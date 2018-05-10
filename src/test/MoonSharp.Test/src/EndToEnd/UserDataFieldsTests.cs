using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
#pragma warning disable 169 // unused private field

	
	public class UserDataFieldsTests
	{
		public class SomeClass
		{
			public int IntProp;
			public const int ConstIntProp = 115;
			public readonly int RoIntProp = 123;
			public int? NIntProp;
			public object ObjProp;
			public static string StaticProp;
			private string PrivateProp;
		}

		public void Test_ConstIntFieldGetter(InteropAccessMode opt)
		{
			string script = @"    
				x = myobj.ConstIntProp;
				return x;";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj", UserData.Create(obj));

			DynValue res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(115, res.Number);
		}

		public void Test_ReadOnlyIntFieldGetter(InteropAccessMode opt)
		{
			string script = @"    
				x = myobj.RoIntProp;
				return x;";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj", UserData.Create(obj));

			DynValue res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(123, res.Number);
		}

		public void Test_ConstIntFieldSetter(InteropAccessMode opt)
		{
			try
			{
				string script = @"    
				myobj.ConstIntProp = 1;
				return myobj.ConstIntProp;";

				Script S = new Script();

				SomeClass obj = new SomeClass() { IntProp = 321 };

				UserData.UnregisterType<SomeClass>();
				UserData.RegisterType<SomeClass>(opt);

				S.Globals.Set("myobj", UserData.Create(obj));

				DynValue res = S.DoString(script);

				Assert.Equal(DataType.Number, res.Type);
				Assert.Equal(115, res.Number);
			}
			catch (ScriptRuntimeException)
			{
				return;
			}

			////Assert.Fail();
		}

		public void Test_ReadOnlyIntFieldSetter(InteropAccessMode opt)
		{
			try
			{
				string script = @"    
				myobj.RoIntProp = 1;
				return myobj.RoIntProp;";

				Script S = new Script();

				SomeClass obj = new SomeClass() { IntProp = 321 };

				UserData.UnregisterType<SomeClass>();
				UserData.RegisterType<SomeClass>(opt);

				S.Globals.Set("myobj", UserData.Create(obj));

				DynValue res = S.DoString(script);

				Assert.Equal(DataType.Number, res.Type);
				Assert.Equal(123, res.Number);
			}
			catch (ScriptRuntimeException)
			{
				return;
			}

			////Assert.Fail();
		}




		public void Test_IntFieldGetter(InteropAccessMode opt)
		{
			string script = @"    
				x = myobj.IntProp;
				return x;";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj", UserData.Create(obj));

			DynValue res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(321, res.Number);
		}

		public void Test_NIntFieldGetter(InteropAccessMode opt)
		{
			string script = @"    
				x = myobj1.NIntProp;
				y = myobj2.NIntProp;
				return x,y;";

			Script S = new Script();

			SomeClass obj1 = new SomeClass() { NIntProp = 321 };
			SomeClass obj2 = new SomeClass() { NIntProp = null };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj1", UserData.Create(obj1));
			S.Globals.Set("myobj2", UserData.Create(obj2));

			DynValue res = S.DoString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(321.0, res.Tuple[0].Number);
			Assert.Equal(DataType.Number, res.Tuple[0].Type);
			Assert.Equal(DataType.Nil, res.Tuple[1].Type);
		}

		public void Test_ObjFieldGetter(InteropAccessMode opt)
		{
			string script = @"    
				x = myobj1.ObjProp;
				y = myobj2.ObjProp;
				z = myobj2.ObjProp.ObjProp;
				return x,y,z;";

			Script S = new Script();

			SomeClass obj1 = new SomeClass() { ObjProp = "ciao" };
			SomeClass obj2 = new SomeClass() { ObjProp = obj1 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj1", UserData.Create(obj1));
			S.Globals.Set("myobj2", UserData.Create(obj2));

			DynValue res = S.DoString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(DataType.String, res.Tuple[0].Type);
			Assert.Equal("ciao", res.Tuple[0].String);
			Assert.Equal(DataType.String, res.Tuple[2].Type);
			Assert.Equal("ciao", res.Tuple[2].String);
			Assert.Equal(DataType.UserData, res.Tuple[1].Type);
			Assert.Equal(obj1, res.Tuple[1].UserData.Object);
		}

		public void Test_IntFieldSetter(InteropAccessMode opt)
		{
			string script = @"    
				myobj.IntProp = 19;";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj", UserData.Create(obj));

			Assert.Equal(321, obj.IntProp);

			DynValue res = S.DoString(script);

			Assert.Equal(19, obj.IntProp);
		}

		public void Test_NIntFieldSetter(InteropAccessMode opt)
		{
			string script = @"    
				myobj1.NIntProp = nil;
				myobj2.NIntProp = 19;";

			Script S = new Script();

			SomeClass obj1 = new SomeClass() { NIntProp = 321 };
			SomeClass obj2 = new SomeClass() { NIntProp = null };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj1", UserData.Create(obj1));
			S.Globals.Set("myobj2", UserData.Create(obj2));

			Assert.Equal(321, obj1.NIntProp);
			Assert.Equal(null, obj2.NIntProp);

			DynValue res = S.DoString(script);

			Assert.Equal(null, obj1.NIntProp);
			Assert.Equal(19, obj2.NIntProp);
		}

		public void Test_ObjFieldSetter(InteropAccessMode opt)
		{
			string script = @"    
				myobj1.ObjProp = myobj2;
				myobj2.ObjProp = 'hello';";

			Script S = new Script();

			SomeClass obj1 = new SomeClass() { ObjProp = "ciao" };
			SomeClass obj2 = new SomeClass() { ObjProp = obj1 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj1", UserData.Create(obj1));
			S.Globals.Set("myobj2", UserData.Create(obj2));

			Assert.Equal("ciao", obj1.ObjProp);
			Assert.Equal(obj1, obj2.ObjProp);

			DynValue res = S.DoString(script);

			Assert.Equal(obj2, obj1.ObjProp);
			Assert.Equal("hello", obj2.ObjProp);
		}

		public void Test_InvalidFieldSetter(InteropAccessMode opt)
		{
			string script = @"    
				myobj.IntProp = '19';";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("myobj", UserData.Create(obj));

			Assert.Equal(321, obj.IntProp);

			DynValue res = S.DoString(script);

			Assert.Equal(19, obj.IntProp);
		}

		public void Test_StaticFieldAccess(InteropAccessMode opt)
		{
			string script = @"    
				static.StaticProp = 'asdasd' .. static.StaticProp;";

			Script S = new Script();

			SomeClass.StaticProp = "qweqwe";

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>(opt);

			S.Globals.Set("static", UserData.CreateStatic<SomeClass>());

			Assert.Equal("qweqwe", SomeClass.StaticProp);

			DynValue res = S.DoString(script);

			Assert.Equal("asdasdqweqwe", SomeClass.StaticProp);
		}

		[Fact]
		public void Interop_IntFieldGetter_None()
		{
			Test_IntFieldGetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_IntFieldGetter_Lazy()
		{
			Test_IntFieldGetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_IntFieldGetter_Precomputed()
		{
			Test_IntFieldGetter(InteropAccessMode.Preoptimized);
		}

		[Fact]
		public void Interop_NIntFieldGetter_None()
		{
			Test_NIntFieldGetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_NIntFieldGetter_Lazy()
		{
			Test_NIntFieldGetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_NIntFieldGetter_Precomputed()
		{
			Test_NIntFieldGetter(InteropAccessMode.Preoptimized);
		}

		[Fact]
		public void Interop_ObjFieldGetter_None()
		{
			Test_ObjFieldGetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ObjFieldGetter_Lazy()
		{
			Test_ObjFieldGetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ObjFieldGetter_Precomputed()
		{
			Test_ObjFieldGetter(InteropAccessMode.Preoptimized);
		}

		[Fact]
		public void Interop_IntFieldSetter_None()
		{
			Test_IntFieldSetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_IntFieldSetter_Lazy()
		{
			Test_IntFieldSetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_IntFieldSetter_Precomputed()
		{
			Test_IntFieldSetter(InteropAccessMode.Preoptimized);
		}

		[Fact]
		public void Interop_NIntFieldSetter_None()
		{
			Test_NIntFieldSetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_NIntFieldSetter_Lazy()
		{
			Test_NIntFieldSetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_NIntFieldSetter_Precomputed()
		{
			Test_NIntFieldSetter(InteropAccessMode.Preoptimized);
		}

		[Fact]
		public void Interop_ObjFieldSetter_None()
		{
			Test_ObjFieldSetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ObjFieldSetter_Lazy()
		{
			Test_ObjFieldSetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ObjFieldSetter_Precomputed()
		{
			Test_ObjFieldSetter(InteropAccessMode.Preoptimized);
		}


		[Fact]
		public void Interop_InvalidFieldSetter_None()
		{
		Assert.Throws<ScriptRuntimeException>(() => { Test_InvalidFieldSetter(InteropAccessMode.Reflection); });

		}

		[Fact]
		public void Interop_InvalidFieldSetter_Lazy()
		{
		Assert.Throws<ScriptRuntimeException>(() => { Test_InvalidFieldSetter(InteropAccessMode.LazyOptimized); });
			
		}

		[Fact]
		public void Interop_InvalidFieldSetter_Precomputed()
		{
		Assert.Throws<ScriptRuntimeException>(() => { Test_InvalidFieldSetter(InteropAccessMode.Preoptimized); });
			
		}


		[Fact]
		public void Interop_StaticFieldAccess_None()
		{
			Test_StaticFieldAccess(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_StaticFieldAccess_Lazy()
		{
			Test_StaticFieldAccess(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_StaticFieldAccess_Precomputed()
		{
			Test_StaticFieldAccess(InteropAccessMode.Preoptimized);
		}



		[Fact]
		public void Interop_IntFieldSetterWithSimplifiedSyntax()
		{
			string script = @"    
				myobj.IntProp = 19;";

			Script S = new Script();

			SomeClass obj = new SomeClass() { IntProp = 321 };

			UserData.UnregisterType<SomeClass>();
			UserData.RegisterType<SomeClass>();

			S.Globals["myobj"] = obj;

			Assert.Equal(321, obj.IntProp);

			DynValue res = S.DoString(script);

			Assert.Equal(obj, S.Globals["myobj"]);
			Assert.Equal(19, obj.IntProp);
		}




		[Fact]
		public void Interop_ConstIntFieldGetter_None()
		{
			Test_ConstIntFieldGetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ConstIntFieldGetter_Lazy()
		{
			Test_ConstIntFieldGetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ConstIntFieldGetter_Precomputed()
		{
			Test_ConstIntFieldGetter(InteropAccessMode.Preoptimized);
		}



		[Fact]
		public void Interop_ConstIntFieldSetter_None()
		{
			Test_ConstIntFieldSetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ConstIntFieldSetter_Lazy()
		{
			Test_ConstIntFieldSetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ConstIntFieldSetter_Precomputed()
		{
			Test_ConstIntFieldSetter(InteropAccessMode.Preoptimized);
		}



		[Fact]
		public void Interop_ReadOnlyIntFieldGetter_None()
		{
			Test_ReadOnlyIntFieldGetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ReadOnlyIntFieldGetter_Lazy()
		{
			Test_ReadOnlyIntFieldGetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ReadOnlyIntFieldGetter_Precomputed()
		{
			Test_ReadOnlyIntFieldGetter(InteropAccessMode.Preoptimized);
		}



		[Fact]
		public void Interop_ReadOnlyIntFieldSetter_None()
		{
			Test_ReadOnlyIntFieldSetter(InteropAccessMode.Reflection);
		}

		[Fact]
		public void Interop_ReadOnlyIntFieldSetter_Lazy()
		{
			Test_ReadOnlyIntFieldSetter(InteropAccessMode.LazyOptimized);
		}

		[Fact]
		public void Interop_ReadOnlyIntFieldSetter_Precomputed()
		{
			Test_ReadOnlyIntFieldSetter(InteropAccessMode.Preoptimized);
		}












	}
}
