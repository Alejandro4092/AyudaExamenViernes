namespace AyudaExamenViernes.Helpers
{
    public class HelperFotoTransform
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperFotoTransform(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        // 1. Método original: Convierte IFormFile a byte[]
        public async Task<byte[]> ConvertirImagenABytesAsync(IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0) return null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imagen.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        // 2. NUEVO MÉTODO: Guarda el byte[] en un archivo físico y devuelve el nombre
        public async Task<string> GuardarArchivoByteAsync(byte[] datosImagen, string nombreOriginal, string carpeta)
        {
            if (datosImagen == null)
            {
                return null;
            }

            // Generamos un nombre único para no sobreescribir archivos
            string nombreArchivo = Guid.NewGuid().ToString() + "_" + nombreOriginal;

            // Construimos la ruta hacia la carpeta (ej. imagenes en la raíz del proyecto)
            string path = Path.Combine(this.hostEnvironment.ContentRootPath, carpeta, nombreArchivo);

            // Escribimos el arreglo de bytes directamente en un archivo físico
            await System.IO.File.WriteAllBytesAsync(path, datosImagen);

            // Retornamos el nombre para guardarlo en la base de datos
            return nombreArchivo;




        }
        //public async Task<string> GuardarArchivoByteAsync(byte[] datosImagen, string nombreOriginal, string carpeta)
        //{
        //    if (datosImagen == null) return null;

        //    string nombreArchivo = Guid.NewGuid().ToString() + "_" + nombreOriginal;

        //    // IMPORTANTE: WebRootPath apunta a wwwroot
        //    string path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, nombreArchivo);

        //    await System.IO.File.WriteAllBytesAsync(path, datosImagen);

        //    return nombreArchivo;
        //}
    }
}



//namespace practicaExamenViernes.Helpers
//{
//    public class HelperConvertirFoto
//    {
//        private IWebHostEnvironment hostEnvironment;

//        public HelperConvertirFoto(IWebHostEnvironment hostEnvironment)
//        {
//            this.hostEnvironment = hostEnvironment;
//        }

//        public async Task<byte[]> ConvertirFotoABytesAsync(IFormFile foto)
//        {
//            if (foto == null || foto.Length == 0) return null;

//            using (var memoryStream = new MemoryStream())
//            {
//                await foto.CopyToAsync(memoryStream);
//                return memoryStream.ToArray();
//            }
//        }

//        public async Task<string> GuardarFotoAsync(byte[] fotoBytes, string nombreOriginal, string carpeta)
//        {
//            if (fotoBytes == null) return null;

//            string nombreArchivo = Guid.NewGuid().ToString() + "_" + nombreOriginal;

//            string path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, nombreArchivo);

//            await System.IO.File.WriteAllBytesAsync(path, fotoBytes);

//            return $"/{carpeta}/{nombreArchivo}";
//        }

//        public async Task<string> GuardarFotoDesdeBase64Async(string base64, string nombreOriginal, string carpeta)
//        {
//            if (string.IsNullOrEmpty(base64)) return null;

//            // Eliminar el prefijo data:image/png;base64, si viene incluido
//            if (base64.Contains(","))
//                base64 = base64.Split(',')[1];

//            byte[] fotoBytes = Convert.FromBase64String(base64);
//            return await GuardarFotoAsync(fotoBytes, nombreOriginal, carpeta);
//        }
//    }
//}