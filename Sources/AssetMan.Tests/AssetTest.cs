using NUnit.Framework;
using System;
namespace AssetMan.Tests
{
	[TestFixture()]
	public class AssetTest
	{
		[Test()]
		public void Export()
		{
			using(var asset = new Asset("Images/ic_warning@3x.png"))
			{
				asset.Export("Images", 100, 100);
			}
		}
	}
}
