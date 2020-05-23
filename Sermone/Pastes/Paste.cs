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
        Task<string[]> GetPaste(long PasteId);
        string GetPasteUrl(long PasteId);
        Task<long> CreatePaste(string Title, string[] Value);
        Task SetPaste(string Title, string[] Value, long PasteId);
        Task DeletePaste(long PasteId);
        Task<Dictionary<long, string>> EnumPastes();
    }
}
