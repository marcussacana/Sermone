using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sermone.Pastes
{
    public interface IPasteCreator
    {
        string Name { get; }
        Task<IPaste> Create(string Username, string Password);
    }

    public interface IPaste
    {
        Task<PasteData> GetPaste(long PasteId);
        string GetPasteUrl(long PasteId);
        Task<long> CreatePaste(string Title, string[] Value);
        Task SetPaste(string Title, string[] Value, long PasteId);
        Task DeletePaste(long PasteId);
        Task<PasteData[]> EnumPastes();
    }

    public struct PasteData {
        public long ID;
        public string Title;
        public string[] Content;
        public DateTime? CreatedAt;
    }
}
