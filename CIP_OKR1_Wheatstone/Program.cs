﻿using System;
using System.Text;

namespace CIP_OKR1_Wheatstone
{
	class Program
	{
		static void Main(string[] args)
		{
			var cypher = new Wheatstone("первый", "второй");
			var toEncrypt = "я кабы не было да было.. а кабы было то было бы то да, но нет";
			var result = cypher.Encrypt(toEncrypt);
			Console.WriteLine(result);
			var decryptResult = cypher.Decrypt(result);
			Console.WriteLine(decryptResult);
		}
	}
}
