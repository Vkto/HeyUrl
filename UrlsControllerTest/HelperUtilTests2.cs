using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeyUrl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyUrl.Utils.Tests
{
	public class HelperUtilTests2
	{
		[TestMethod()]
		public void GetShortUrlTest()
		{
			// arrange
			string originalUrl = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string shortUrl = "CDADD";

			// act
			string _shortUrl = HelperUtil.GetShortUrl(originalUrl);

			// assert
			Assert.AreEqual(shortUrl, _shortUrl);
		}

		[TestMethod()]
		public void IsValidUrlTest()
		{
			Assert.Fail();
		}
	}
}