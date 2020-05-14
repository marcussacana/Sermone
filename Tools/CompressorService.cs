using BlazorWorker.WorkerBackgroundService;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using BrotliStream = BrotliSharpLib.BrotliStream;

namespace Sermone.Tools
{
    public static class CompressorHelper {
        public static async Task<byte[]> Compress(this IWorkerBackgroundService<CompressorService> Service, byte[] Data) {
            var lData = Convert.ToBase64String(Data);
            return Convert.FromBase64String(await Service.RunAsync((x) => x.Compress(lData)));
        }

        public static async Task<byte[]> Decompress(this IWorkerBackgroundService<CompressorService> Service, byte[] Data)
        {
            var lData = Convert.ToBase64String(Data);
            var cData = await Service.RunAsync((x) => x.Decompress(lData));
            if (cData == null)
                return null;

            return Convert.FromBase64String(cData);
        }
    }
    public class CompressorService
    {
        public string Compress(string B64) {
            using MemoryStream Stream = new MemoryStream(Convert.FromBase64String(B64));
            using MemoryStream Result = new MemoryStream();
            using BrotliStream Compressor = new BrotliStream(Result, CompressionMode.Compress);
            Compressor.SetQuality(8);
            Stream.CopyTo(Compressor);
            Compressor.Flush();
            var Rst = Convert.ToBase64String(Result.ToArray());
            return Rst;
        }
        public string Decompress(string B64)
        {
            try
            {
                using MemoryStream Stream = new MemoryStream(Convert.FromBase64String(B64));
                using MemoryStream Result = new MemoryStream();
                using BrotliStream Decompressor = new BrotliStream(Stream, CompressionMode.Decompress);
                Decompressor.CopyTo(Result);
                return Convert.ToBase64String(Result.ToArray());
            }
            catch { return null;; }
        }
    }
}
