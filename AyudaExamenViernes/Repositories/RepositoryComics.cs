using AyudaExamenViernes.Data;
using AyudaExamenViernes.Models;
using Microsoft.EntityFrameworkCore;

namespace AyudaExamenViernes.Repositories
{
    public class RepositoryComics
    {

        private ComicContext comicContext;

        public RepositoryComics(ComicContext context)
        {
            comicContext = context;
        }
        public async Task<List<Comic>> GetComicsAsync()
        {
            return await comicContext.Comics.ToListAsync();
        }

        public async Task<Comic> FindComicAsync(int id)
        {
            return await this.comicContext.Comics.FindAsync(id);
        }

        public async Task InsertComicAsync(int id, string nombre, string descripcion, string imagen)
        {
            Comic comic = new Comic
            {
                IdComic = id,
                Nombre = nombre,
                Descripcion = descripcion,
                Imagen = imagen
            };

            await comicContext.Comics.AddAsync(comic);
            await comicContext.SaveChangesAsync();
        }

        public async Task UpdateComicAsync(Comic comic)
        {
            this.comicContext.Comics.Update(comic);
            await this.comicContext.SaveChangesAsync();
        }

        public async Task DeleteComicAsync(int id)
        {
            Comic comic = await this.FindComicAsync(id);

            this.comicContext.Comics.Remove(comic);
            await this.comicContext.SaveChangesAsync();
        }
    }
}