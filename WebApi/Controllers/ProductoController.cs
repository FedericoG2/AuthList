using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Custom;
using WebApi.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;

        public ProductoController(DbpruebaContext dbPruebaContext)
        {
            _dbPruebaContext = dbPruebaContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var lista = await _dbPruebaContext.Productos.ToListAsync();
                return Ok(new { value = lista });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            try
            {
                var producto = await _dbPruebaContext.Productos.FindAsync(id);

                if (producto == null)
                    return NotFound(new { message = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> AgregarProducto(Producto producto)
        {
            try
            {
                await _dbPruebaContext.Productos.AddAsync(producto);
                await _dbPruebaContext.SaveChangesAsync();

                return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.IdProducto }, producto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto)
                return BadRequest(new { message = "ID del producto no coincide" });

            _dbPruebaContext.Entry(producto).State = EntityState.Modified;

            try
            {
                await _dbPruebaContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExiste(id))
                    return NotFound(new { message = "Producto no encontrado" });

                throw;
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                var producto = await _dbPruebaContext.Productos.FindAsync(id);

                if (producto == null)
                    return NotFound(new { message = "Producto no encontrado" });

                _dbPruebaContext.Productos.Remove(producto);
                await _dbPruebaContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("Buscar")]
        public async Task<IActionResult> BuscarProductos(string nombre)
        {
            try
            {
                var productos = await _dbPruebaContext.Productos
                    .Where(p => p.Nombre.Contains(nombre))
                    .ToListAsync();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        private bool ProductoExiste(int id)
        {
            return _dbPruebaContext.Productos.Any(e => e.IdProducto == id);
        }
    }
}
