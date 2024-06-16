﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Custom;
using WebApi.Models;
using WebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;
        private readonly Utilidades _utilidades;

        public AccesoController(DbpruebaContext dbPruebaContext, Utilidades utilidades)
        {
            _dbPruebaContext = dbPruebaContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSHA256(objeto.Clave)
            };

            try
            {
                await _dbPruebaContext.Usuarios.AddAsync(modeloUsuario);
                await _dbPruebaContext.SaveChangesAsync();

                if (modeloUsuario.IdUsuario != 0)
                    return Ok(new { isSuccess = true });
                else
                    return Ok(new { isSuccess = false });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            try
            {
                var usuarioEncontrado = await _dbPruebaContext.Usuarios
                    .Where(u => u.Correo == objeto.Correo && u.Clave == _utilidades.encriptarSHA256(objeto.Clave))
                    .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                    return Ok(new { isSuccess = false, token = "" });
                else
                    return Ok(new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
