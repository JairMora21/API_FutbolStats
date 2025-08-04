using Xunit;
using Moq;
using API_FutbolStats.Controllers;
using API_FutbolStats.Service.Interfaz;
using API_FutbolStats.Models.DtoUpdate;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_FutbolStats.Tests
{
    public class TarjetaControllerTests
    {
        [Fact]
        public async Task UpdateTarjeta_InvalidModelState_ReturnsBadRequestAndServiceNotInvoked()
        {
            // Arrange
            var mockService = new Mock<ITarjetaService>();
            var controller = new TarjetaController(mockService.Object);
            controller.ModelState.AddModelError("IdTipoTarjeta", "Required");
            var dto = new TarjetaDtoUpdate();

            // Act
            var result = await controller.UpdatePartido(dto, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            mockService.Verify(s => s.UpdateTarjetas(It.IsAny<TarjetaDtoUpdate>(), It.IsAny<int>()), Times.Never);
        }
    }
}
