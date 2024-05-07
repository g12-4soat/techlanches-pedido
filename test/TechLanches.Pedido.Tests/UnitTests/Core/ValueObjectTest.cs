using TechLanches.Pedido.Tests.UnitTests.Core.Assets;

namespace TechLanches.Pedido.Tests.UnitTests.Core
{
    public class ValueObjectTest
    {
        [Theory(DisplayName = "Equals")]
        [InlineData("Avenida Paulista", "São Paulo", "São Paulo", "Brazil", "17280-000")]
        [Trait("Category", "Domain Tests")]
        public void EqualityTest(string street, string city, string state, string country, string zipcode)
        {
            Address address1 = new (street, city, state, country, zipcode);
            Address address2 = new (street, city, state, country, zipcode);

            Assert.True(address1 == address2);
            Assert.False(address1 != address2);
            Assert.True(address1.Equals(address2));
            Assert.Equal(address1, address2);
        }

        [Theory(DisplayName = "Not Equals")]
        [InlineData("Avenida Brasil", "Rio de Janeiro", "Rio de Janeiro", "Brazil", "01430-000")]
        [Trait("Category", "Domain Tests")]
        public void NonEqualityTest(string street, string city, string state, string country, string zipcode)
        {
            Address address1 = new (street, city, state, country, zipcode);
            Address address2 = new ("Avenida Paulista",
                                           "São Paulo",
                                           "São Paulo",
                                           "Brazil",
                                           "17280-000");

            Assert.True(address1 != address2);
            Assert.False(address1 == address2);
            Assert.False(address1.Equals(address2));
            Assert.NotEqual(address1, address2);
        }
    }
}
