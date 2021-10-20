using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HeyUrl.Utils.Tests
{
  [TestClass()]
	public class UrlUtilsTests
	{
		[TestMethod()]
		public void CheckGenShortUrlTest_Equal_url()
		{
			// arrange
			//case 1
			string originalUrl = "https://www.loom.com/share/74c86d7818af43dcb85cc1636e0a0c43";
			string shortUrl_exp = "BACFF"; 

			// act
			string shortUrl_gen = UrlUtils.GetShortUrl(originalUrl);
			
			// assert
			Assert.AreEqual(shortUrl_exp, shortUrl_gen); 
		}

		[TestMethod()]
		public void CheckGenShortUrlTest_Not_Equal_url()
		{
			// arrange			
			string originalUrl_1 = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string originalUrl_2 = "Loom.loom.com/share/74c86d7818af43dcb85cc1636e0a0c43";

      // act
      string shortUrl_1_gen = UrlUtils.GetShortUrl(originalUrl_1); 
			string shortUrl_2_gen = UrlUtils.GetShortUrl(originalUrl_2); 

			// assert			
			Assert.AreNotEqual(shortUrl_1_gen, shortUrl_2_gen); 
		}


		[TestMethod()]
		public void IsCheckedShortUrlRequestTest()
		{
			// arrange

			//-correct
			string url_1 = "CBEAB";
			string url_2 = "cbeab";
			string url_2_1 = "aBcdE";

			//-incorrect
			string url_3 = "CBEABA";
			string url_4 = "CBEA";
			string url_5 = "CB5AB";

			// act
			//true
			bool isValid_1 = UrlUtils.IsValidShortUrl(url_1);
			bool isValid_2 = UrlUtils.IsValidShortUrl(url_2);
			bool isValid_2_1 = UrlUtils.IsValidShortUrl(url_2_1);

			//false
			bool isValid_3 = UrlUtils.IsValidShortUrl(url_3);
			bool isValid_4 = UrlUtils.IsValidShortUrl(url_4);
			bool isValid_5 = UrlUtils.IsValidShortUrl(url_5);


			// assert
			Assert.IsTrue(isValid_1);
			Assert.IsTrue(isValid_2);
			Assert.IsTrue(isValid_2_1);

			Assert.IsFalse(isValid_3);
			Assert.IsFalse(isValid_4);
			Assert.IsFalse(isValid_5);

		}

		[TestMethod()]
		public void IsCheckedIncorrectUrlTest()
		{
			// arrange

			string errMes1 = "Incorrect Url";
			string errMes2 = "Enter a original Url";

			string url_1 = "qwerty";
			string url_2 = "";
			string url_3 = "https://www.loom.com/ share/74c86d7818af43dcb85cc1636e0a0c43"; 
			string url_4 = "https://githubcom/Vkto/ToyBlock-Challenge";   
      string url_5 = "https://githubcom\vkto/ToyBlock-Challenge"; 

			
			// act
			Tuple<bool, string> isValid_1 = UrlUtils.IsValidUrl(url_1);
			Tuple<bool, string> isValid_2 = UrlUtils.IsValidUrl(url_2);
			Tuple<bool, string> isValid_3 = UrlUtils.IsValidUrl(url_3);
			Tuple<bool, string> isValid_4 = UrlUtils.IsValidUrl(url_4);
			Tuple<bool, string> isValid_5 = UrlUtils.IsValidUrl(url_5);


			// assert
			Assert.IsFalse(isValid_1.Item1);
			Assert.AreEqual(errMes1, isValid_1.Item2);

			Assert.IsFalse(isValid_2.Item1);
			Assert.AreEqual(errMes2, isValid_2.Item2);

			Assert.IsFalse(isValid_3.Item1);
			Assert.AreEqual(errMes1, isValid_3.Item2);

			Assert.IsFalse(isValid_4.Item1);
			Assert.AreEqual(errMes1, isValid_4.Item2);

			Assert.IsFalse(isValid_5.Item1);
			Assert.AreEqual(errMes1, isValid_5.Item2);
		}

		[TestMethod()]
		public void IsValidCorrectUrlTest()
		{
			// arrange
			string mes1 = "OK";

			string url_1 = "www.google.com";
			string url_2 = "https://github.com/Vkto/ToyBlock-Challenge";
			string url_3 = "https://www.loom.com/share/74c86d7818af43dcb85cc1636e0a0c43";
			string url_4 = $"https://drive.google.com/file/d/1Eq2RbIiIcAcy4AxiElSutcu2rv_Y21zZ/view?usp=sharing";
			string url_5 = $"http://drive.google.com/file/d/1Eq2RbIiIcAcy4AxiElSutcu2rv_Y21zZ/view?usp=sharing";
      			
			// act
			Tuple<bool, string> isValid_1 = UrlUtils.IsValidUrl(url_1);
			Tuple<bool, string> isValid_2 = UrlUtils.IsValidUrl(url_2);
			Tuple<bool, string> isValid_3 = UrlUtils.IsValidUrl(url_3);
			Tuple<bool, string> isValid_4 = UrlUtils.IsValidUrl(url_4);
			Tuple<bool, string> isValid_5 = UrlUtils.IsValidUrl(url_5);


			// assert
			Assert.IsTrue(isValid_1.Item1);
			Assert.AreEqual(mes1, isValid_1.Item2);

			Assert.IsTrue(isValid_2.Item1);
			Assert.AreEqual(mes1, isValid_2.Item2);

			Assert.IsTrue(isValid_3.Item1);
			Assert.IsTrue(isValid_4.Item1);
			Assert.IsTrue(isValid_5.Item1);

		}

		
	}
}
