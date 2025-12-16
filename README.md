# SAT-WS-Cfdi

Estructura recomendada:
src
├── Api.Cfdi.DescargaMasiva
│   ├── Api.Cfdi.DescargaMasiva.csproj
│   ├── Core
│   │   ├── Entities
│   │   │   ├── AccessToken.cs
│   │   │   ├── AutenticacionRequest.cs
│   │   │   └── VerificacionResult.cs
│   │   ├── Interfaces
│   │   │   ├── IAutenticacionService.cs
│   │   │   ├── IDescargaService.cs
│   │   │   ├── ISolicitudService.cs
│   │   │   └── IVerificacionService.cs
│   │   └── Exceptions
│   │       └── InvalidResponseContentException.cs
│   ├── Application
│   │   ├── Services
│   │   │   ├── AutenticacionService.cs
│   │   │   ├── DescargaService.cs
│   │   │   └── SolicitudService.cs
│   │   ├── UseCases
│   │   │   ├── AuthenticateUserUseCase.cs
│   │   │   ├── DownloadComprobanteUseCase.cs
│   │   │   └── VerifyRequestUseCase.cs
│   │   └── Helpers
│   │       ├── SignedXmlHelper.cs
│   │       └── SoapRequestHelper.cs
│   ├── Infrastructure
│   │   ├── WebServices
│   │   │   ├── CfdiDescargaMasivaNamespaces.cs
│   │   │   ├── CfdiDescargaMasivaWebServiceUrls.cs
│   │   │   ├── HttpSoapClient.cs
│   │   └── Adaptadores
│   │       ├── AutenticacionServiceAdapter.cs
│   │       └── DescargaServiceAdapter.cs
│   ├── Web (API o UI)
│   │   └── Controllers
│   │       ├── AuthController.cs
│   │       ├── DownloadController.cs
│   │       └── VerificationController.cs
│   └── Constants
│       ├── CfdiDescargaMasivaNamespaces.cs
│       └── CfdiDescargaMasivaWebServiceUrls.cs
