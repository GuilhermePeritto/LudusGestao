using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using LudusGestao.Application.DTOs;
using LudusGestao.Application.Services;
using LudusGestao.Domain.Entities;
using LudusGestao.Domain.Interfaces;
using LudusGestao.Domain.Enums;
using LudusGestao.Domain.Interfaces.Repositories;

namespace LudusGestao.Application.Tests.Services
{
    public class ReservaServiceTests
    {
        [Fact]
        public async Task CriarReservaAsync_DeveRetornarReservaDTO()
        {
            // Arrange
            var repoMock = new Mock<IReservaRepository>();
            var mapperMock = new Mock<IMapper>();
            var dto = new CreateReservaDTO { ClienteId = Guid.NewGuid(), LocalId = Guid.NewGuid(), DataReserva = DateTime.Now.AddDays(1) };
            var reserva = new Reserva { Id = Guid.NewGuid(), ClienteId = dto.ClienteId, LocalId = dto.LocalId, DataReserva = dto.DataReserva, Status = StatusReserva.Pendente };
            var reservaDTO = new ReservaDTO { Id = reserva.Id, ClienteId = reserva.ClienteId, LocalId = reserva.LocalId, DataReserva = reserva.DataReserva, Status = reserva.Status.ToString() };

            mapperMock.Setup(m => m.Map<Reserva>(dto)).Returns(reserva);
            mapperMock.Setup(m => m.Map<ReservaDTO>(reserva)).Returns(reservaDTO);

            var service = new ReservaService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.CriarReservaAsync(dto);

            // Assert
            Assert.Equal(reservaDTO.Id, result.Id);
            Assert.Equal(reservaDTO.Status, result.Status);
        }
    }
} 