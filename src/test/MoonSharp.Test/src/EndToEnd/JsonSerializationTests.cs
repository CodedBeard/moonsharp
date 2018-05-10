using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization.Json;
using Xunit;


namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class JsonSerializationTests
	{
		void AssertTableValues(Table t)
		{
			Assert.Equal(DataType.Number, t.Get("aNumber").Type);
			Assert.Equal(1, t.Get("aNumber").Number);

			Assert.Equal(DataType.String, t.Get("aString").Type);
			Assert.Equal("2", t.Get("aString").String);

			Assert.Equal(DataType.Table, t.Get("anObject").Type);
			Assert.Equal(DataType.Table, t.Get("anArray").Type);

			Table o = t.Get("anObject").Table;

			Assert.Equal(DataType.Number, o.Get("aNumber").Type);
			Assert.Equal(3, o.Get("aNumber").Number);

			Assert.Equal(DataType.String, o.Get("aString").Type);
			Assert.Equal("4", o.Get("aString").String);

			Table a = t.Get("anArray").Table;

			//				'anArray' : [ 5, '6', true, null, { 'aNumber' : 7, 'aString' : '8' } ]

			Assert.Equal(DataType.Number, a.Get(1).Type);
			Assert.Equal(5, a.Get(1).Number);

			Assert.Equal(DataType.String, a.Get(2).Type);
			Assert.Equal("6", a.Get(2).String);

			Assert.Equal(DataType.Boolean, a.Get(3).Type);
			Assert.True(a.Get(3).Boolean);

			Assert.Equal(DataType.Boolean, a.Get(3).Type);
			Assert.True(a.Get(3).Boolean);

			Assert.Equal(DataType.UserData, a.Get(4).Type);
			Assert.True(JsonNull.IsJsonNull(a.Get(4)));

			Assert.Equal(DataType.Table, a.Get(5).Type);
			Table s = a.Get(5).Table;

			Assert.Equal(DataType.Number, s.Get("aNumber").Type);
			Assert.Equal(7, s.Get("aNumber").Number);

			Assert.Equal(DataType.String, s.Get("aString").Type);
			Assert.Equal("8", s.Get("aString").String);
		}


		[Fact]
		public void JsonDeserialization()
		{
			string json = @"{
				'aNumber' : 1,
				'aString' : '2',
				'anObject' : { 'aNumber' : 3, 'aString' : '4' },
				'anArray' : [ 5, '6', true, null, { 'aNumber' : 7, 'aString' : '8' } ]
				}
			".Replace('\'', '\"');

			Table t = JsonTableConverter.JsonToTable(json);
			AssertTableValues(t);
		}

		[Fact]
		public void JsonSerialization()
		{
			string json = @"{
				'aNumber' : 1,
				'aString' : '2',
				'anObject' : { 'aNumber' : 3, 'aString' : '4' },
				'anArray' : [ 5, '6', true, null, { 'aNumber' : 7, 'aString' : '8' } ]
				}
			".Replace('\'', '\"');

			Table t1 = JsonTableConverter.JsonToTable(json);

			string json2 = JsonTableConverter.TableToJson(t1);

			Table t = JsonTableConverter.JsonToTable(json2);

			AssertTableValues(t);
		}


		[Fact]
		public void JsonObjectSerialization()
		{
			object o = new
			{
				aNumber = 1,
				aString = "2",
				anObject = new
				{
					aNumber = 3,
					aString = "4"
				},
				anArray = new object[]
				{
					5,
					"6",
					true,
					null,
					new
					{
						aNumber = 7,
						aString = "8"
					}
				}
			};


			string json = JsonTableConverter.ObjectToJson(o);

			Table t = JsonTableConverter.JsonToTable(json);

			AssertTableValues(t);
		}


	}
}
