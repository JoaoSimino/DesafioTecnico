using Microsoft.AspNetCore.Mvc.Testing;

namespace DesafioTecnico1Teste;

public class ApiTestFixture : IDisposable
{
    public readonly HttpClient Client;
    public readonly WebApplicationFactory<Program> Factory;

    public ApiTestFixture()
    {
        Factory = new WebApplicationFactory<Program>();
        Client = Factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
