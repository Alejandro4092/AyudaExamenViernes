using AyudaExamenViernes.Helpers;
using AyudaExamenViernes.Models;
using AyudaExamenViernes.Models.DTOs;
using AyudaExamenViernes.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace AyudaExamenViernes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicsController : ControllerBase
    {
        private RepositoryComics repo;
        private HelperFotoTransform helperFoto;
        private IConfiguration config; // NUEVA INYECCIÓN PARA LEER APPSETTINGS

        public ComicsController(RepositoryComics repo, HelperFotoTransform helperfoto, IConfiguration config)
        {
            this.repo = repo;
            this.helperFoto = helperfoto;
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult> GetComics()
        {
            List<Comic> comics = await this.repo.GetComicsAsync();
            foreach (var comic in comics)
            {
                comic.Imagen = $"https://localhost:7108/Fotos/{comic.Imagen}";
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

            comic.Imagen = $"https://localhost:7108/Fotos/{comic.Imagen}";

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
            string nombreArchivo = await this.helperFoto.GuardarArchivoByteAsync(imagenBytes, request.Imagen.FileName, "Fotos");
            // 3. Guardamos en base de datos usando las variables del FromForm y el nombre del archivo
            await this.repo.InsertComicAsync(request.Id, request.Nombre, request.Descripcion, nombreArchivo);

            return Ok(new
            {
                mensaje = "Archivo de bytes creado y cómic insertado correctamente",
                archivoGuardado = nombreArchivo
            });
        }

        [HttpGet("v2")]
        public async Task<ActionResult> GetComicsV2()
        {
            // 1. Leer los datos desde appsettings.json
            string urlBase = this.config.GetValue<string>("MisAjustes:UrlBase");
            string carpeta = this.config.GetValue<string>("MisAjustes:CarpetaImagenes");

            List<Comic> comics = await this.repo.GetComicsAsync();
            foreach (var comic in comics)
            {
                // 2. Construir la URL de forma totalmente dinámica
                comic.Imagen = $"{urlBase}{carpeta}/{comic.Imagen}";
            }
            return Ok(comics);
        }

        [HttpPost("v2")]
        public async Task<ActionResult> InsertarComicV2([FromForm] ComicRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Modelo incompleto");
            }

            // 1. Leer el nombre de la carpeta desde el appsettings.json
            string carpeta = this.config.GetValue<string>("MisAjustes:CarpetaImagenes");

            byte[] imagenBytes = await this.helperFoto.ConvertirImagenABytesAsync(request.Imagen);

            // 2. Pasarle la carpeta parametrizada al Helper
            string nombreArchivo = await this.helperFoto.GuardarArchivoByteAsync(imagenBytes, request.Imagen.FileName, carpeta);
            await this.repo.InsertComicAsync(request.Id, request.Nombre, request.Descripcion, nombreArchivo);

            //// Recuperas "fotos" desde el appsettings.json
            //string carpetaDestino = this.configuration.GetValue<string>("RutasFicheros:CarpetaImagenes");

            //// Llamas al helper, que ahora utiliza WebRootPath (wwwroot)
            //string nombreArchivoFinal = await this.helperFoto.GuardarArchivoByteAsync(
            //    imagenBytes,
            //    imagen.FileName,
            //    carpetaDestino // "fotos"

            return Ok(new
            {
                mensaje = "Archivo creado y cómic insertado correctamente usando AppSettings",
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