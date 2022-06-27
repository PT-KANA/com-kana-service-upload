using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface ICreateable
    {
        Task<int> Create(object model);
    }
}
