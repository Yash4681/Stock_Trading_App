using AutoFixture;
using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAppUnitTest
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory customWebApplicationFactory)
        {
            _httpClient = customWebApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Index_ToReturnView()
        {
            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("/Trade/Index/MSFT");

            //Assert
            httpResponse.IsSuccessStatusCode.Should().BeTrue();

            string content = await httpResponse.Content.ReadAsStringAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode node = document.DocumentNode;

            node.QuerySelectorAll(".price").Should().NotBeNull();
        }
    }
}
