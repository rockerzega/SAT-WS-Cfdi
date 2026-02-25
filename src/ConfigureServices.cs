using DescargaMasiva.DescargaMasiva.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DescargaMasiva.DescargaMasiva.Domain.Ports; 
using DescargaMasiva.DescargaMasiva.Infrastructure.Services;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva;


public static class ConfigureServices
{
  public static IServiceCollection AddCfdiDescargaMasivaServices(this IServiceCollection services)
  {
    // 1. Registro del Cliente SOAP (Infraestructura pura)
    services.AddHttpClient<IHttpSoapClient, HttpSoapClient>();

    // 2. Inyección de los Servicios (Adaptadores) vinculados a sus Puertos (Interfaces)
    services.AddTransient<IAutenticacionService, AutenticacionService>();
    services.AddTransient<ISolicitudService, SolicitudService>();
    services.AddTransient<IVerificacionService, VerificacionService>();
    services.AddTransient<IDescargaService, DescargaService>();

    return services;
  }
}