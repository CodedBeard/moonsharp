using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization.Json;
using NUnit.Framework;


namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	[TestFixture]
	public class JsonSerializationTests
	{
		void AssertTableValues(Table t)
		{
			NUnit.Framework.Assert.AreEqual(DataType.Number, t.Get("aNumber").Type);
			NUnit.Framework.Assert.AreEqual(1, t.Get("aNumber").Number);

			NUnit.Framework.Assert.AreEqual(DataType.String, t.Get("aString").Type);
			NUnit.Framework.Assert.AreEqual("2", t.Get("aString").String);

			NUnit.Framework.Assert.AreEqual(DataType.Table, t.Get("anObject").Type);
			NUnit.Framework.Assert.AreEqual(DataType.Table, t.Get("anArray").Type);

			Table o = t.Get("anObject").Table;

			NUnit.Framework.Assert.AreEqual(DataType.Number, o.Get("aNumber").Type);
			NUnit.Framework.Assert.AreEqual(3, o.Get("aNumber").Number);

			NUnit.Framework.Assert.AreEqual(DataType.String, o.Get("aString").Type);
			NUnit.Framework.Assert.AreEqual("4", o.Get("aString").String);

			Table a = t.Get("anArray").Table;

			//				'anArray' : [ 5, '6', true, null, { 'aNumber' : 7, 'aString' : '8' } ]

			NUnit.Framework.Assert.AreEqual(DataType.Number, a.Get(1).Type);
			NUnit.Framework.Assert.AreEqual(5, a.Get(1).Number);

			NUnit.Framework.Assert.AreEqual(DataType.String, a.Get(2).Type);
			NUnit.Framework.Assert.AreEqual("6", a.Get(2).String);

			NUnit.Framework.Assert.AreEqual(DataType.Boolean, a.Get(3).Type);
			NUnit.Framework.Assert.IsTrue(a.Get(3).Boolean);

			NUnit.Framework.Assert.AreEqual(DataType.Boolean, a.Get(3).Type);
			NUnit.Framework.Assert.IsTrue(a.Get(3).Boolean);

			NUnit.Framework.Assert.AreEqual(DataType.UserData, a.Get(4).Type);
			NUnit.Framework.Assert.IsTrue(JsonNull.IsJsonNull(a.Get(4)));

			NUnit.Framework.Assert.AreEqual(DataType.Table, a.Get(5).Type);
			Table s = a.Get(5).Table;

			NUnit.Framework.Assert.AreEqual(DataType.Number, s.Get("aNumber").Type);
			NUnit.Framework.Assert.AreEqual(7, s.Get("aNumber").Number);

			NUnit.Framework.Assert.AreEqual(DataType.String, s.Get("aString").Type);
			NUnit.Framework.Assert.AreEqual("8", s.Get("aString").String);
		}


		[Test]
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

		[Test]
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


		[Test]
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
