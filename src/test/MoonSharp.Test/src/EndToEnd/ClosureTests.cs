using System;
using MoonSharp.Interpreter.Execution;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class ClosureTests
	{
		[Fact]
		public void ClosureOnParam()
		{
			string script = @"
				local function g (z)
				  local function f(a)
					return a + z;
				  end
				  return f;
				end

				return (g(3)(2));";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(5, res.Number);
		}

		[Fact]
		public void LambdaFunctions()
		{
			string script = @"
g = |f, x|f(x, x+1)
f = |x, y, z|x*(y+z)
return g(|x,y|f(x,y,1), 2)
";
			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(8, res.Number);
		}



		[Fact]
		public void ClosureOnParamLambda()
		{
			string script = @"
				local function g (z)
				  return |a| a + z
				end

				return (g(3)(2));";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(5, res.Number);
		}


		[Fact]
		public void Closures()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								a[i] = function () y=y+1; x = x * 10; return x+y end
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";


			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(5, res.Tuple.Length);
			Assert.Equal(DataType.Number, res.Tuple[0].Type);
			Assert.Equal(DataType.Number, res.Tuple[1].Type);
			Assert.Equal(DataType.Number, res.Tuple[2].Type);
			Assert.Equal(DataType.Number, res.Tuple[3].Type);
			Assert.Equal(DataType.Number, res.Tuple[4].Type);
			Assert.Equal(201, res.Tuple[0].Number);
			Assert.Equal(2001, res.Tuple[1].Number);
			Assert.Equal(20001, res.Tuple[2].Number);
			Assert.Equal(200001, res.Tuple[3].Number);
			Assert.Equal(2000001, res.Tuple[4].Number);
		}

		[Fact]
		public void ClosuresNonAnonymousLocal()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								local function zz() y=y+1; x = x * 10; return x+y end
								a[i] = zz;
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(5, res.Tuple.Length);
			Assert.Equal(DataType.Number, res.Tuple[0].Type);
			Assert.Equal(DataType.Number, res.Tuple[1].Type);
			Assert.Equal(DataType.Number, res.Tuple[2].Type);
			Assert.Equal(DataType.Number, res.Tuple[3].Type);
			Assert.Equal(DataType.Number, res.Tuple[4].Type);
			Assert.Equal(201, res.Tuple[0].Number);
			Assert.Equal(2001, res.Tuple[1].Number);
			Assert.Equal(20001, res.Tuple[2].Number);
			Assert.Equal(200001, res.Tuple[3].Number);
			Assert.Equal(2000001, res.Tuple[4].Number);
		}


		[Fact]
		public void ClosuresNonAnonymous()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								function zz() y=y+1; x = x * 10; return x+y end
								a[i] = zz;
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(5, res.Tuple.Length);
			Assert.Equal(DataType.Number, res.Tuple[0].Type);
			Assert.Equal(DataType.Number, res.Tuple[1].Type);
			Assert.Equal(DataType.Number, res.Tuple[2].Type);
			Assert.Equal(DataType.Number, res.Tuple[3].Type);
			Assert.Equal(DataType.Number, res.Tuple[4].Type);
			Assert.Equal(201, res.Tuple[0].Number);
			Assert.Equal(2001, res.Tuple[1].Number);
			Assert.Equal(20001, res.Tuple[2].Number);
			Assert.Equal(200001, res.Tuple[3].Number);
			Assert.Equal(2000001, res.Tuple[4].Number);
		}

		[Fact]
		public void ClosureNoTable()
		{
			string script = @"
				x = 0

				function container()
					local x = 20

					for i=1,5 do
						local y = 0
		
						function zz() y=y+1; x = x * 10; return x+y end
		
						a1 = a2;
						a2 = a3;
						a3 = a4;
						a4 = a5;
						a5 = zz;
					end
				end

				container();

				x = 4000

				return a1(), a2(), a3(), a4(), a5()";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Tuple, res.Type);
			Assert.Equal(5, res.Tuple.Length);
			Assert.Equal(DataType.Number, res.Tuple[0].Type);
			Assert.Equal(DataType.Number, res.Tuple[1].Type);
			Assert.Equal(DataType.Number, res.Tuple[2].Type);
			Assert.Equal(DataType.Number, res.Tuple[3].Type);
			Assert.Equal(DataType.Number, res.Tuple[4].Type);
			Assert.Equal(201, res.Tuple[0].Number);
			Assert.Equal(2001, res.Tuple[1].Number);
			Assert.Equal(20001, res.Tuple[2].Number);
			Assert.Equal(200001, res.Tuple[3].Number);
			Assert.Equal(2000001, res.Tuple[4].Number);
		}


		[Fact]
		public void NestedUpvalues()
		{
			string script = @"
	local y = y;

	local x = 0;
	local m = { };

	function m:a()
		self.t = {
			dojob = function() 
				if (x == 0) then return 1; else return 0; end
			end,
		};
	end

	m:a();

	return 10 * m.t.dojob();
								";

			DynValue res = Script.RunString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(10, res.Number);
		}

		[Fact]
		public void NestedOutOfScopeUpvalues()
		{
			string script = @"

	function X()
		local y = y;

		local x = 0;
		local m = { };

		function m:a()
			self.t = {
				dojob = function() 
					if (x == 0) then return 1; else return 0; end
				end,
			};
		end

		return m;
	end

	Q = X();

	Q:a();

	return 10 * Q.t.dojob();
								";

			DynValue res = new Script(CoreModules.Preset_HardSandbox).DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(10, res.Number);
		}


		[Fact]
		public void LocalRedefinition()
		{
			string script = @"

				result = ''

				local hi = 'hello'

				local function test()
					result = result .. hi;
				end

				test();

				hi = 'X'

				test();

				local hi = '!';

				test();

				return result;
								";

			DynValue res = new Script(CoreModules.Preset_HardSandbox).DoString(script);

			Assert.Equal(DataType.String, res.Type);
			Assert.Equal("helloXX", res.String);
		}


	}
}
