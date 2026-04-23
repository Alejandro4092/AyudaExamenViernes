using AyudaExamenViernes.Helpers;
using AyudaExamenViernes.Models;
using AyudaExamenViernes.Models.DTOs;
using AyudaExamenViernes.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AyudaExamenViernes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicsController : ControllerBase
    {
        private RepositoryComics repo;
        private HelperFotoTransform helperFoto;

        public ComicsController(RepositoryComics repo, HelperFotoTransform helperfoto)
        {
            this.repo = repo;
            this.helperFoto = helperfoto;
        }

        [HttpGet]
        public async Task<ActionResult> GetComics()
        {
            List<Comic> comics = await this.repo.GetComicsAsync();
            foreach (var comic in comics)
            {
                comic.Imagen = $"https://localhost:7108/Imagenes/{comic.Imagen}";
            }
            return Ok(comics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FindComic(int id)
        {
            Comic comic = await this.repo.FindComicAsync(id);

            if (comic == null)
            {
                return NotFound();
            }

            comic.Imagen = $"https://localhost:7108/Imagenes/{comic.Imagen}";

            return Ok(comic);
        }

        [HttpPost]
        public async Task<ActionResult> InsertarComic([FromForm] ComicRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Modelo incompleto");
            }

            // 1. Obtenemos el byte[] de la foto
            byte[] imagenBytes = await this.helperFoto.ConvertirImagenABytesAsync(request.Imagen);

            // 2. Guardamos ese byte[] como un archivo físico en la carpeta imagenes
            // Le pasamos el nombre original para conservar la extensión (.jpg, .png, etc.)
            string nombreArchivo = await this.helperFoto.GuardarArchivoByteAsync(imagenBytes, request.Imagen.FileName, "Imagenes");
            // 3. Guardamos en base de datos usando las variables del FromForm y el nombre del archivo
            await this.repo.InsertComicAsync(request.Id, request.Nombre, request.Descripcion, nombreArchivo);

            return Ok(new
            {
                mensaje = "Archivo de bytes creado y cómic insertado correctamente",
                archivoGuardado = nombreArchivo
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComic(int id, Comic comic)
        {
            await this.repo.UpdateComicAsync(comic);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComic(int id)
        {
            await this.repo.DeleteComicAsync(id);
            return NoContent();
        }
    }
}